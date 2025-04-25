using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreControl.Database
{
    public class Customers
    {
        public int customersId { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public int addressId { get; set; }
        public Addresses? Address { get; set; }   // Navigation Property
        public string? phoneNumber { get; set; }
        public string? email { get; set; }
        public int userId { get; set; }
        public User? User { get; set; }   // Navigation Property
    }
}
