using Microsoft.EntityFrameworkCore;
using ParcelCheckout.Api.Core;
using ParcelCheckout.Api.Data.Configuration;

namespace ParcelCheckout.Api.Features.HealthCheck;

public class Post : IEndpoint
{
    void IEndpoint.Map(WebApplication app)
    {
        app.MapPost("/v1/checkout",
                async (Data.DTOs.CheckoutCriteria requestDto,
                Data.Configuration.DbContext dbContext) => await HandleAsync(requestDto, dbContext))
            .WithSummary("Processes the order and returns the total cost.")
            .WithOpenApi()
            .Produces<int>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    internal static async Task<IResult> HandleAsync(Data.DTOs.CheckoutCriteria requestDto, Data.Configuration.DbContext dbContext)
    {
        var categories = requestDto.Services.Distinct();

        foreach (var category in categories)
        {
            var service = dbContext.Services.Include(s => s.Multibuy).Single(s => s.Category == category);
            var count = requestDto.Services.Where(s => s == category).Count();
        }

        return TypedResults.Ok(-1);
    }

    internal static int GetCost(Service service, int count)
    {
        return -1;
    }
}