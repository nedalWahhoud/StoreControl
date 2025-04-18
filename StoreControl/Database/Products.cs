using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace StoreControl.Database
{
    public class Products
    {
        public int productsId { get; set; }
        public string? productName { get; set; }
        public string? description { get; set; }
        public int categoriesId { get; set; }
        public Categories? Category { get; set; }   // Navigation Property

        public int articleNumber { get; set; }
        public int quantity { get; set; }
        public double purchasePrice  { get; set; }
        public double sellingPrice { get; set; }
        public int minimumStock { get; set; }
        public byte[]? img { get; set; }
        public int userId { get; set; }
        public User? User { get; set; }   // Navigation Property
    }
}
