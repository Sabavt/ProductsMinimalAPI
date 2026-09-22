using ProductsMinimapAPI.RouteGroups;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((HostBuilderContext builder, IServiceProvider services, LoggerConfiguration logger) =>
{
    logger
    .ReadFrom.Configuration(builder.Configuration)
    .ReadFrom.Services(services);
});

var app = builder.Build();

var routeGroup = app.MapGroup("/products").ProductsAPI();
 
app.Run(); 