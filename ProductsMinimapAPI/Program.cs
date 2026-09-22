using ProductsMinimapAPI.RouteGroups; 

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var routeGroup = app.MapGroup("/products").ProductsAPI();
 
app.Run();
