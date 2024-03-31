using ParcelCheckout.Api.Core;

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
        return TypedResults.Ok(1);
    }
}