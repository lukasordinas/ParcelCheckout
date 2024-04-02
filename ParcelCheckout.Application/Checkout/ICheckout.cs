using ParcelCheckout.Data.Configuration;

namespace ParcelCheckout.Application.Checkout
{
    public interface ICheckout
    {
        public void Scan(char service);

        public int GetTotalPrice(IEnumerable<Service> configuration);
    }
}
