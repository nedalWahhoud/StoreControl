using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreControl.Database
{
    public class Addresses
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int addressesId { get; set; }
        public string? street { get; set; }
        public string? houseNumber { get; set; }
        public int postalCode { get; set; }
        public string? city { get; set; }
        public  string? country { get; set; }
        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }
        public int userId { get; set; }
        public User? User { get; set; }   // Navigation Property
    }
}
