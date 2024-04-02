using ParcelCheckout.Data.Configuration;

namespace ParcelCheckout.Application.Tests.Checkout
{
    public class Tests
    {
        private readonly IEnumerable<Service> Configuration = new List<Service>
        {
            new Service { Category = 'A', Price = 1000, Multibuy = new Multibuy { Amount = 3, Price = 2500 } },
            new Service { Category = 'B', Price = 1200, Multibuy = new Multibuy { Amount = 2, Price = 2000 } },
            new Service { Category = 'C', Price = 1500 },
            new Service { Category = 'D', Price = 2500 },
            new Service { Category = 'F', Price = 800, Multibuy = new Multibuy { Amount = 2, Price = 1500 } }
        };

        [TestCase(new[] { 'a' }, 1000)]
        [TestCase(new[] { 'b', 'b' }, 2000)]
        [TestCase(new[] { 'f', 'c' }, 2300)]
        [TestCase(new[] { 'f', 'f', 'b' }, 2700)]
        public void GetTotalPrice_returns_the_expected_price(char[] services, int expectedPrice)
        {
            var checkout = new Application.Checkout.Checkout();

            foreach (var service in services)
            {
                checkout.Scan(service);
            }

            var actualPrice = checkout.GetTotalPrice(Configuration);

            Assert.That(actualPrice, Is.EqualTo(expectedPrice));
        }
    }
}