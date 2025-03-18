using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMDT.Models.ViewModels
{
    public class ProductDetailsViewModel
    {
        public ShoppingCart ShoppingCart { get; set; }
        public IEnumerable<Product> RecommendedProducts { get; set; }
    }

}
