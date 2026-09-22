using ProductsMinimapAPI.Models;
using System.ComponentModel.DataAnnotations;
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

            return Results.Ok();
        });


        routeGroup.MapGet("/{Id:int}", async (HttpContext context, int Id) =>
        {
            var matchingProduct = products.SingleOrDefault(t => t.Id == Id);

            await context.Response.WriteAsync(JsonSerializer.Serialize(matchingProduct));

            return Results.Ok();
        });


        routeGroup.MapPost("/", async (HttpContext context, Product product) =>
        {
            products.Add(product);

            await context.Response.WriteAsync($"Added product with ID: {product.Id}");

            return Results.Ok();
        });


        routeGroup.MapPut("/{productID:int}/{newName:alpha}", async (HttpContext context, string newName, int productID) =>
        {
            var product = products.FirstOrDefault((p) => p.Id == productID);

            if (product == null)
            {
                return Results.BadRequest(new { message = "Incorrect Id" });
            }

            product.ProductName = newName;
            return Results.Ok();
        });

        routeGroup.MapDelete("/{productID:int}", async (HttpContext context, int productID) =>
        {
            var product = products.FirstOrDefault((p) => p.Id == productID);

            if (product == null)
            {
                return Results.BadRequest();
            }

            products.Remove(product);
            return Results.Ok(new { message = "Person deleted" });
        }).AddEndpointFilter(async (EndpointFilterInvocationContext context, EndpointFilterDelegate next) =>
        {
            var product = context.Arguments.OfType<Product>().FirstOrDefault();

            if (product == null)
            {
                return Results.BadRequest("Product id not match");
            }

            var validation = new ValidationContext(product); 
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(product, validation, validationResults, true);

            if(!isValid)
            {
                return Results.BadRequest(new { error = validationResults.FirstOrDefault()?.ErrorMessage });
            }

            return await next(context);
        });

        return routeGroup;
    }
}
