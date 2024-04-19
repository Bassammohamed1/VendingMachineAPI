using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Models
{
    public class User : IdentityUser
    {
        public double? Deposit { get; set; }
        [StringLength(25)]
        public string Role { get; set; }
        public List<Product>? Products { get; set; }
    }
}
