using ParcelCheckout.Data.Configuration;

namespace ParcelCheckout.Application.Checkout
{
    public class Checkout : ICheckout
    {
        private readonly List<char> _basket = [];

        public void Scan(char category)
        {
            _basket.Add(char.ToLower(category));
        }

        public int GetTotalPrice(IEnumerable<Service> configuration)
        {
            var distinctCategories = _basket.Distinct();
            int totalPrice = 0;

            foreach (var dc in distinctCategories)
            {
                var service = configuration.Single(s => char.ToLower(s.Category) == dc);
                var amount = _basket.Where(s => s == dc).Count();

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
