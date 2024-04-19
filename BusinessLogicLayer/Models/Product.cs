using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BusinessLogicLayer.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [StringLength(50)]
        public string ProductName { get; set; }
        public int AmountAvailable { get; set; }
        public double Cost { get; set; }
        public string SellerId { get; set; }
        [ForeignKey(nameof(SellerId))]
        [JsonIgnore]
        [IgnoreDataMember]
        public User? Seller { get; set; }
    }
}
