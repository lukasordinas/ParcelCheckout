using Microsoft.AspNetCore.Builder;
using ParcelCheckout.Api.Core;
using System.Reflection;

namespace ParcelCheckout.Api.Extensions;

public static class WebApplicationExtensions
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = GetEndpoints();
        foreach (var endpoint in endpoints)
        {
            endpoint.Map(app);
        }
    }

    public static IEnumerable<IEndpoint> GetEndpoints()
    {
        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && typeof(IEndpoint).IsAssignableFrom(t))
            .Select(Activator.CreateInstance)
            .Cast<IEndpoint>();
    }
}
