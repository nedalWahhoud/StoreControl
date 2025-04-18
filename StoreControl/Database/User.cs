using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreControl.Database
{
    public class User
    {
        public int userId { get; set; }
        public string? userName { get; set; } 
        public string? passwordHash { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
