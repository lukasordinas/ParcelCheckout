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
            var distinctItems = _basket.Distinct();
            int totalPrice = 0;

            foreach (var item in distinctItems)
            {
                var service = configuration.Single(s => char.ToLower(s.Category) == item);
                var amount = _basket.Where(s => s == item).Count();

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
