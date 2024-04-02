using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Moq.EntityFrameworkCore;
using ParcelCheckout.Api.Data.DTOs;
using ParcelCheckout.Data.Configuration;

namespace ParcelCheckout.Api.Features.Checkout.Filters.Tests;

[TestFixture]
internal class CheckoutBasketFilterTests
{
    [Test]
    public async Task InvokeAsync_returns_bad_request_when_the_request_dto_is_null()
    {
        var endpointFilterInvocationContext = CreateEndpointFilterInvocationContext(null);
        var filter = CreateEndpointFilter();

        static ValueTask<object?> next(EndpointFilterInvocationContext context) => ValueTask.FromResult<object?>(TypedResults.Ok());

        var result = await filter.InvokeAsync(endpointFilterInvocationContext, next) as ProblemHttpResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
    }

    [Test]
    public async Task InvokeAsync_returns_bad_request_when_the_request_dto_does_not_contain_any_services()
    {
        var requestDto = new CheckoutCriteria([]);
        var endpointFilterInvocationContext = CreateEndpointFilterInvocationContext(requestDto);
        var filter = CreateEndpointFilter();

        static ValueTask<object?> next(EndpointFilterInvocationContext context) => ValueTask.FromResult<object?>(TypedResults.Ok());

        var result = await filter.InvokeAsync(endpointFilterInvocationContext, next) as ProblemHttpResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
    }

    [Test]
    public async Task InvokeAsync_returns_bad_request_when_the_request_dto_contains_services_that_do_not_exist()
    {
        var requestDto = new CheckoutCriteria(['a', 'b', 'z']);
        var endpointFilterInvocationContext = CreateEndpointFilterInvocationContext(requestDto);
        var filter = CreateEndpointFilter();

        static ValueTask<object?> next(EndpointFilterInvocationContext context) => ValueTask.FromResult<object?>(TypedResults.Ok());

        var result = await filter.InvokeAsync(endpointFilterInvocationContext, next) as ProblemHttpResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
    }

    [Test]
    public async Task InvokeAsync_calls_next_when_all_services_in_the_basket_are_verified()
    {
        var requestDto = new CheckoutCriteria(['a', 'b']);
        var endpointFilterInvocationContext = CreateEndpointFilterInvocationContext(requestDto);
        var filter = CreateEndpointFilter();

        bool nextWasCalled = false;
        ValueTask<object?> next(EndpointFilterInvocationContext context)
        {
            nextWasCalled = true;
            return ValueTask.FromResult<object?>(TypedResults.Ok());
        }

        var result = await filter.InvokeAsync(endpointFilterInvocationContext, next) as ProblemHttpResult;

        Assert.That(nextWasCalled, Is.True);
    }

    private static IEndpointFilter CreateEndpointFilter()
    {
        var mockDbContext = new Mock<DbContext>();
        var services = new List<Service>
        {
            new Service
            {
                Category = 'a',
            },
            new Service
            {
                Category = 'b',
            }
        };

        mockDbContext.Setup(c => c.Services).ReturnsDbSet(services);
        return new CheckoutBasketFilter(mockDbContext.Object);
    }

    private static EndpointFilterInvocationContext CreateEndpointFilterInvocationContext(CheckoutCriteria? requestDto)
    {
        var httpContext = new DefaultHttpContext();
        return new DefaultEndpointFilterInvocationContext(httpContext, requestDto);
    }
}