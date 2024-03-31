using System.ComponentModel.DataAnnotations.Schema;

namespace ParcelCheckout.Api.Data.Configuration;

[Table("Multibuys")]
public class Multibuy
{
    public int ID { get; set; }

    public int ServiceID { get; set; }

    public int Amount { get; set; }

    public int Price { get; set; }

    public Multibuy? MultibuyDiscount { get; set; }
}
