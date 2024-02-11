using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TheTask.Models
{
    public class User : IdentityUser
    {
        public Double? Deposit { get; set; }
        [StringLength(25)]
        public string Role { get; set; }
        public List<Product>? Products { get; set; }
    }
}
