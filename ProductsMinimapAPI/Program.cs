using Microsoft.AspNetCore.Mvc;
using ProductsMinimapAPI.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Product> products = [
    new Product(){ProductName = "Orange", Id = 1},
    new Product(){ProductName = "Apple", Id = 2},
    new Product(){ProductName = "Banana", Id = 3} 
    ];

app.MapGet("/products", async (HttpContext context) =>
{
    var products_text = string.Join("\n", products.Select(p => p.ToString()));

    await context.Response.WriteAsync(products_text);
});

app.MapPost("/", async (HttpContext context, Product product) =>
{
    products.Add(product); 

    await context.Response.WriteAsync($"Added product with ID: {product.Id}");
});

app.Run();
