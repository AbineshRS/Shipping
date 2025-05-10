using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shipping.Model
{
    public class ShippingProduct
    {
        [Key]
        public int ShippingId { get; set; }

        public DateOnly ShippingDate { get; set; }

        public string CustomerId { get; set; }

        public decimal ShippingWeight { get; set; }

        public decimal ShippingHeight { get; set; }

        public decimal ShippingWidth { get; set; }

        public string ShippingMode { get; set; }

        public bool Status { get; set; }

        public int RequestId { get; set; }

        [ForeignKey("RequestId")]
        [JsonIgnore] 

        public ShippingRequest? ShippingRequest { get; set; }  
    }
}
