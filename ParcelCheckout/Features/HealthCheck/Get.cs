using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ParcelCheckout.Api.Core;

namespace ParcelCheckout.Api.Features.HealthCheck;

public class Get : IEndpoint
{
    void IEndpoint.Map(WebApplication app)
    {
        app.MapGet("/v1/healthCheck", () => Handle())
            .WithSummary("Runs the health check.")
            .WithOpenApi()
            .Produces<string>(StatusCodes.Status200OK);
    }

    internal static IResult Handle()
    {
        return TypedResults.Ok("Healthy");
    }
}