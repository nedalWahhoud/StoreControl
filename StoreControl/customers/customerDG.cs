using StoreControl.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreControl.customers
{
     class customerDG
    {
        public int rowIndex { get; set; }
        public int customersId { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public int addressId { get; set; }
        public string? street { get; set; }
        public string? houseNumber { get; set; }
        public int postalCode { get; set; }
        public string? city { get; set; }
        public string? country { get; set; }
        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }
        public int userId { get; set; }
    }
}
