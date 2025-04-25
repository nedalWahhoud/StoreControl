using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreControl.Database
{
    public class Addresses
    {
        public int addressId { get; set; }
        public string? street { get; set; }
        public string? houseNumber { get; set; }
        public string? city { get; set; }
        public int postalCode { get; set; }
        public  string? country { get; set; }
        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }
        public int userId { get; set; }
        public User? User { get; set; }   // Navigation Property
    }
}
