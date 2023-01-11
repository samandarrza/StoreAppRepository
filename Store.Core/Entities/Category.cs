using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Entities
{
    public class Category:BaseEntities
    {
        public string  Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
