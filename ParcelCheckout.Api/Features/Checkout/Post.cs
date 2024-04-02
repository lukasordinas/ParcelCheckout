using Microsoft.EntityFrameworkCore;
using ParcelCheckout.Api.Core;
using ParcelCheckout.Application.Checkout;

namespace ParcelCheckout.Api.Features.Checkout;

public class Post : IEndpoint
{
    void IEndpoint.Map(WebApplication app)
    {
        app.MapPost("/v1/checkout",
                async (Data.DTOs.CheckoutCriteria requestDto,
                ParcelCheckout.Data.Configuration.DbContext dbContext,
                ICheckout checkout) => await HandleAsync(requestDto, dbContext, checkout))
            .WithSummary("Processes the order from a JSON array of chars and returns the total cost in pence.")
            .WithOpenApi()
            .AddEndpointFilter<Filters.CheckoutBasketFilter>()
            .Produces<int>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    internal static async Task<IResult> HandleAsync(Data.DTOs.CheckoutCriteria requestDto, ParcelCheckout.Data.Configuration.DbContext dbContext, ICheckout checkout)
    {
        // Input has been checked and validated by the filters.
        try
        {
            var services = requestDto.Services;

            foreach (var service in services)
            {
                checkout.Scan(service);
            }

            var configuration = await dbContext.Services.Include(s => s.Multibuy).ToListAsync();
            var totalPrice = checkout.GetTotalPrice(configuration);

            return TypedResults.Ok(totalPrice);
        }
        catch
        {
            return TypedResults.Problem(statusCode: StatusCodes.Status500InternalServerError, detail: "Unexpected exception.");
        }
    }
}