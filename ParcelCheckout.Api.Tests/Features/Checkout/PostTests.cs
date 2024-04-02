using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Moq.EntityFrameworkCore;
using ParcelCheckout.Api.Data.DTOs;
using ParcelCheckout.Api.Features.Checkout;
using ParcelCheckout.Application.Checkout;
using ParcelCheckout.Data.Configuration;

namespace ParcelCheckout.Api.Tests.Features.Checkout
{
    [TestFixture]
    internal class PostTests
    {

        [Test]
        public async Task HandleAsync_returns_ok_and_the_total_amount()
        {
            var expectedTotal = 1000;
            var requestDto = new CheckoutCriteria(['a']);
            var mockDbContext = new Mock<DbContext>();
            mockDbContext.Setup(c => c.Services).ReturnsDbSet(Enumerable.Empty<Service>());

            var mockCheckout = new Mock<ICheckout>();
            mockCheckout.Setup(c => c.GetTotalPrice(It.IsAny<IEnumerable<Service>>())).Returns(expectedTotal);

            var result = await Post.HandleAsync(requestDto, mockDbContext.Object, mockCheckout.Object) as Ok<int>;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(result.Value, Is.EqualTo(expectedTotal));
        }

        [Test]
        public async Task HandleAsync_returns_internal_server_error_when_an_exception_is_thrown()
        {
            var requestDto = new CheckoutCriteria(['a']);
            var mockDbContext = new Mock<ParcelCheckout.Data.Configuration.DbContext>();
            mockDbContext.Setup(c => c.Services).Throws<InvalidOperationException>();
            var mockCheckout = new Mock<ICheckout>();

            var result = await Post.HandleAsync(requestDto, mockDbContext.Object, mockCheckout.Object) as ProblemHttpResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }
    }
}