using StoreControl.customers;
using StoreControl.Database;
using StoreControl.LogIn;
using StoreControl.ProductsF;
using System.Windows.Media;

namespace StoreControl
{
    internal class staticVariable
    {
        public static string? lang = null;
        public static string folderPath = "";
        public static string[] langArray = new string[] { "en", "de" };
        public const int MediumblobLimit = 16 * 1024 * 1024; // 16 MB
        public static int maxWidth = 1024;
        public static int maxHeight = 1200;
        // for datagrid
        public static List<string> columnOrderP = new List<string>  { "rowIndex", "productsId",  "productName", "description",  "category", 
            "quantity",  "purchasePrice", "sellingPrice", "articleNumber", "minimumStock", "img"};
        public static List<string> columnOrderC = new List<string>  { "rowIndex",  "firstName", "lastName", "phone", "email" , "street",
            "houseNumber", "postalCode" , "city", "country", "latitude", "longitude"};
        // for datagrid get data from database
        public static int currentPageDG = 0;
        public const int pageSizeDG = 11;
        public static productsDG currentProduct = new productsDG();
        // customer database
        public static int currentPageDgC = 0;
        public const int pageSizeDgC = 11;
        public static customerDG currentCustomer = new customerDG();
        // for translation
        public static List<string> keywordP = new List<string> { "rowIndex", "productsId", "productName", "category", "description", "articleNumber", "purchasePrice", "sellingPrice", "quantity", "minimumStock",
            "create", "edit", "delete", "clear", "uploadImg", "searchTB", "noData","img" };
        public static List<string> keywordL = new List<string> { "lang", "logIn", "products", "customer", "logOut", "exit" };
        public static List<string> keywordLI = new List<string> { "userName", "password", "logIn" };
        public static List<string> keywordC = new List<string> { "rowIndex" , "customersId", "firstName", "lastName", "phone", "email", "street", "houseNumber", "city","country", "postalCode", "latitude", "longitude", "noData",
            "create", "edit", "delete", "clear","searchTB" };
        // frames
        public static frameProducts? staticFP = null;
        public static frameLogIn? staticFL = null;
        public static frameCustomers? staticFC = null;
        public static frameList? staticFLI = null;
        // User
        public static User currentUser = new User();
    }
}
