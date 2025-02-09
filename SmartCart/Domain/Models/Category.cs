using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryImage { get; set; }
        public string CategoryName { get; set; }
        public bool IsOffer { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
