using Epsilon.Renders;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Negocio.Persistencia;
using Negocio.Servicios;
using System;
using System.Security.Principal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.PageViewLocationFormats.Add("/Pages/Partials/{0}" + RazorViewEngine.ViewExtension);
});


// Cadena de conexion a BBDD 
builder.Services.AddDbContext<EpsilonDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSQL")));
//builder.Services.AppDbContext(options => options.UseSqlServer(_configuration.GetConnectionString("ConexionSQL")));


builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<IRazorRenderService, RazorRenderService>();


//builder.Services.AddSingleton<HttpContextAccessor, HttpContextAccessor>();
//builder.Services.AddTransient<IPrincipal>(p => p.GetService<IHttpContextAccessor>()?.HttpContext?.User ?? WindowsPrincipal.Current);

// Se dan de alta los servicios de Negocio
builder.Services.AddScoped<ISeguridad, Seguridad>();
builder.Services.AddScoped<IPlanificacion, Planificacion>();
builder.Services.AddScoped<IGestionUsuarios, GestionUsuarios>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
