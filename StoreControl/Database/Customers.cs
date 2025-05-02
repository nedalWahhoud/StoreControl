
namespace StoreControl.Database
{
    public class Customers
    {
        public int customersId { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public int addressesId { get; set; }
        public Addresses? Addresses { get; set; }   // Navigation Property
        public string? phone { get; set; }
        public string? email { get; set; }
        public int userId { get; set; }
        public User? User { get; set; }   // Navigation Property
    }
}
