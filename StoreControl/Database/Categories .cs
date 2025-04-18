using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreControl.Database
{
    public class Categories
    {
        public int categoriesId { get; set; }
        public string? categoryName { get; set; }
        public int userId { get; set; }
        public User? User { get; set; }   // Navigation Property

        public List<Products>? Products { get; set; }// Navigation Property

    }
}
