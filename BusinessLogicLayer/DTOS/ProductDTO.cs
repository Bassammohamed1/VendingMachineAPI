using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.DTOS
{
    public class ProductDTO
    {
        [StringLength(50)]
        public string ProductName { get; set; }
        public int AmountAvailable { get; set; }
        public double Cost { get; set; }
        public string SellerId { get; set; }
    }
}