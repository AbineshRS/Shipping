using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Model
{
    public class ShippingRequest
    {
        [Key]
        public int RequestId { get; set; }

        public DateOnly RequestDate { get; set; }

        public string CustomerId { get; set; }

        public string ShippingAddress { get; set; }

        public string Country { get; set; }

        public bool Status { get; set; }

        // Navigation property - 1 request → many products
        public List<ShippingProduct> Products { get; set; }

    }
}
