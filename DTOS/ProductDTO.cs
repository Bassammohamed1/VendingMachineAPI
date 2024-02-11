using System.ComponentModel.DataAnnotations;

namespace TheTask.DTOS
{
    public class ProductDTO
    {
        [StringLength(50)]
        public string ProductName { get; set; }
        public int AmountAvailable { get; set; }
        public Double Cost { get; set; }
        public string SellerId { get; set; }
    }
}