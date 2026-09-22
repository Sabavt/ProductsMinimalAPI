using ProductsMinimapAPI.Models; 
using System.Text.Json;

namespace ProductsMinimapAPI.RouteGroups;

public static class ProductsMapGroup
{
    private static List<Product> products = [
            new Product(){ProductName = "Orange", Id = 1},
    new Product(){ProductName = "Apple", Id = 2},
    new Product(){ProductName = "Banana", Id = 3}
            ];

    public static RouteGroupBuilder ProductsAPI(this RouteGroupBuilder routeGroup)
    { 
        routeGroup.MapGet("/", async (HttpContext context) =>
        {
            await context.Response.WriteAsync(JsonSerializer.Serialize(products));
        });

        routeGroup.MapGet("/{Id:int}", async (HttpContext context, int Id) =>
        {
            var matchingProduct = products.SingleOrDefault(t => t.Id == Id);

            await context.Response.WriteAsync(JsonSerializer.Serialize(matchingProduct));
        });

        routeGroup.MapPost("/", async (HttpContext context, Product product) =>
        {
            products.Add(product);

            await context.Response.WriteAsync($"Added product with ID: {product.Id}");
        });

        return routeGroup;
    } 
}
