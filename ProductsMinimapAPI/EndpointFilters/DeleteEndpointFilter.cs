using ProductsMinimapAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductsMinimapAPI.EndpointFilters
{
    public class DeleteEndpointFilter : IEndpointFilter
    {
        private readonly ILogger<DeleteEndpointFilter> _logger;

        public DeleteEndpointFilter(ILogger<DeleteEndpointFilter> logger)
        {
            _logger = logger;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var product = context.Arguments.OfType<Product>().FirstOrDefault();
            _logger.LogInformation("Readed argument values from endpoint - {argument}", product);

            if (product == null)
            {
                _logger.LogInformation("Subsequent argument values was not finded and returned as null");
                return Results.BadRequest("Product id not match");
            }

            var validation = new ValidationContext(product);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(product, validation, validationResults, true);

            if (!isValid)
            {
                return Results.BadRequest(new { error = validationResults.FirstOrDefault()?.ErrorMessage });
            } 

            var result = await next(context);

            _logger.LogDebug("after logic of endpoint delegate");

            return result;
        }
    }
}
