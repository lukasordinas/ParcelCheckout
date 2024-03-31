using System.ComponentModel.DataAnnotations.Schema;

namespace ParcelCheckout.Api.Data.Configuration;

[Table("Services")]
public class Service
{
    public int ID { get; set; }

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public int Price { get; set; }
}
