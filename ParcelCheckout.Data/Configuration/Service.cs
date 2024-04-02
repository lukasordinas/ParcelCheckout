using System.ComponentModel.DataAnnotations.Schema;

namespace ParcelCheckout.Data.Configuration;

[Table("Services")]
public class Service
{
    public int ID { get; set; }

    public char Category { get; set; }

    public string Description { get; set; } = default!;

    public int Price { get; set; }

    public Multibuy? Multibuy { get; set; }
}
