using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Domain.Models
{
    public class CartOrder
    {
        [Key]
        public int Id { get; set; }
        public string CartId { get; set; } 
        public int UserId { get; set; }   
        public int OrderId { get; set; }  
        public bool IsActive { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } 
    }
}
