using Microsoft.EntityFrameworkCore;
using ParcelCheckout.Api.Data.DTOs;

namespace ParcelCheckout.Api.Features.Checkout.Filters;

internal class CheckoutBasketFilter : IEndpointFilter
{
    private readonly ParcelCheckout.Data.Configuration.DbContext _dbContext;

    public CheckoutBasketFilter(ParcelCheckout.Data.Configuration.DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    async ValueTask<object?> IEndpointFilter.InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        return await VerifyBasket(context) ? await next(context) : TypedResults.Problem(statusCode: StatusCodes.Status400BadRequest, detail: "Request is missing data or contains invalid services.");
    }

    private async Task<bool> VerifyBasket(EndpointFilterInvocationContext context)
    {
        var requestDto = context.Arguments.OfType<CheckoutCriteria>().SingleOrDefault();

        if (requestDto is null)
        {
            return false;
        }

        if (!requestDto.Services.Any())
        {
            return false;
        }

        foreach (var item in requestDto.Services)
        {
            if (!await _dbContext.Services.AnyAsync(s => char.ToLower(s.Category) == char.ToLower(item)))
            {
                return false;
            }
        }

        return true;
    }
}