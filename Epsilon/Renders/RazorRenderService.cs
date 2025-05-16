using Azure.Core.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Diagnostics;
using System.Text.Encodings.Web;



namespace Epsilon.Renders
{
    public class RazorRenderService : IRazorRenderService
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IActionContextAccessor _actionContext;
        private readonly IRazorPageActivator _activator;
        private readonly ILogger _logger;

        public RazorRenderService(IRazorViewEngine razorViewEngine,
                      ITempDataProvider tempDataProvider,
                      IServiceProvider serviceProvider,
                      IHttpContextAccessor httpContext,
                      IActionContextAccessor actionContext,
                      IRazorPageActivator activator,
                      ILoggerProvider loggerProvider)
        {
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _httpContext = httpContext;
            _actionContext = actionContext;
            _activator = activator;
            _logger = loggerProvider.CreateLogger("RenderPartials");
        }

        public async Task<string> ToStringAsync<T>(string pageName, T model)
        {
            _logger.LogInformation(GetEventId(), "Cargando la vista: '" + pageName + "',con los datos: " + Truncate(JsonObjectSerializer.Default.Serialize(model).ToString(), 1000));

            var actionContext = new ActionContext(_httpContext.HttpContext, _httpContext.HttpContext.GetRouteData(),
                _actionContext.ActionContext.ActionDescriptor);


            using (var sw = new StringWriter())
            {
                var result = _razorViewEngine.FindPage(actionContext, pageName);

                if (result.Page == null)
                {
                    throw new ArgumentNullException($"The page {pageName} cannot be found.");
                }

                var view = new RazorView(_razorViewEngine, _activator, new List<IRazorPage>(), result.Page, HtmlEncoder.Default, new DiagnosticListener("RazorRenderService"));

                var viewContext = new ViewContext(actionContext, view, new ViewDataDictionary<T>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                },
                new TempDataDictionary(_httpContext.HttpContext, _tempDataProvider), sw, new HtmlHelperOptions());

                var page = (result.Page);
                page.ViewContext = viewContext;
                _activator.Activate(page, viewContext);
                await page.ExecuteAsync();
                return sw.ToString();
            }
        }

        private IRazorPage FindPage(ActionContext actionContext, string pageName)
        {
            var getPageResult = _razorViewEngine.GetPage(executingFilePath: null, pagePath: pageName);

            if (getPageResult.Page != null) {
                return getPageResult.Page;
            }

            var findPageResult = _razorViewEngine.FindPage(actionContext, pageName);

            if (findPageResult.Page != null) { 
                return findPageResult.Page;
            }

            var searchedLocations = getPageResult.SearchedLocations.Concat(findPageResult.SearchedLocations);
            var errorMessage = string.Join(Environment.NewLine, new[] { $"Unable to find page' {pageName} '. The following locations were searched:" }.Concat(searchedLocations));
            throw new InvalidOperationException(errorMessage);
        }

        protected EventId GetEventId()
        {
            return new EventId((this.GetType().FullName ?? "RenderPartials").GetHashCode(),this.GetType().FullName);
        }

        private string Truncate(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return text.Length <= maxLength ? text : text.Substring(0, maxLength) + "...";
        }
    }
}
