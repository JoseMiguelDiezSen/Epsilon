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
    /// <summary>
    /// Proporciona servicios para renderizar vistas Razor a cadenas utilizando el motor de vistas de ASP.NET Core.
    /// </summary>
    /// <remarks>Esta clase permite convertir vistas Razor en representaciones de texto, lo que resulta útil
    /// para generar contenido HTML dinámico fuera del flujo estándar de MVC, como en correos electrónicos o respuestas
    /// personalizadas. Utiliza dependencias del entorno de ASP.NET Core para garantizar que el renderizado respete el
    /// contexto de la aplicación, incluyendo datos de vista y de ruta. No es seguro para subprocesos; se recomienda
    /// crear una instancia por solicitud o utilizar la inyección de dependencias estándar.</remarks>
    public class RazorRenderService : IRazorRenderService
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IActionContextAccessor _actionContext;
        private readonly IRazorPageActivator _activator;
        private readonly ILogger _logger;

        /// <summary>
        /// Inicializa una nueva instancia de la clase RazorRenderService con los servicios necesarios para renderizar
        /// vistas Razor de forma programática.
        /// </summary>
        /// <param name="razorViewEngine">El motor de vistas Razor utilizado para localizar y renderizar vistas.</param>
        /// <param name="tempDataProvider">El proveedor de datos temporales utilizado para mantener datos entre solicitudes.</param>
        /// <param name="serviceProvider">El proveedor de servicios que resuelve dependencias requeridas durante el renderizado.</param>
        /// <param name="httpContext">El accesor de contexto HTTP que proporciona acceso al contexto de la solicitud actual.</param>
        /// <param name="actionContext">El accesor de contexto de acción que proporciona información sobre la acción actual de MVC.</param>
        /// <param name="activator">El activador de páginas Razor responsable de inicializar instancias de páginas Razor.</param>
        /// <param name="loggerProvider">El proveedor de registros utilizado para crear instancias de logger para el servicio.</param>
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

        /// <summary>
        /// Generates an EventId based on the full type name of the current instance.
        /// </summary>
        /// <remarks>This method can be used to consistently identify log events originating from a
        /// specific type. If the type's full name is unavailable, a default name of "RenderPartials" is used.</remarks>
        /// <returns>An EventId whose Id is the hash code of the type's full name and whose Name is the type's full name.</returns>
        protected EventId GetEventId()
        {
            return new EventId((this.GetType().FullName ?? "RenderPartials").GetHashCode(),this.GetType().FullName);
        }

        /// <summary>
        /// Truncates the specified string to a maximum length and appends an ellipsis if truncation occurs.
        /// </summary>
        /// <param name="text">The string to be truncated. If null or empty, the method returns the original value.</param>
        /// <param name="maxLength">The maximum number of characters allowed before truncation. Must be non-negative.</param>
        /// <returns>A string that is no longer than the specified maximum length. If truncation occurs, an ellipsis ("...") is
        /// appended. Returns the original string if it is null, empty, or shorter than the maximum length.</returns>
        private string Truncate(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return text.Length <= maxLength ? text : text.Substring(0, maxLength) + "...";
        }
    }
}
