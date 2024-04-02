using ParcelCheckout.Data.Configuration;

namespace ParcelCheckout.Application.Checkout
{
    public class Checkout : ICheckout
    {
        private readonly List<char> _basket = [];

        public void Scan(char service)
        {
            _basket.Add(char.ToLower(service));
        }

        public int GetTotalPrice(IEnumerable<Service> configuration)
        {
            var distinctServices = _basket.Distinct();
            int totalPrice = 0;

            foreach (var d in distinctServices)
            {
                var service = configuration.Single(s => char.ToLower(s.Category) == d);
                var amount = _basket.Where(s => s == d).Count();

                if (service.Multibuy is not null)
                {
                    totalPrice += service.Multibuy.Price * (amount / service.Multibuy.Amount);
                    totalPrice += service.Price * (amount % service.Multibuy.Amount);
                }
                else
                {
                    totalPrice += service.Price * amount;
                }
            }

            return totalPrice;
        }
    }
}
