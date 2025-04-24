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
        public static Products currentProduct = new Products();
        public static int maxWidth = 1024;
        public static int maxHeight = 1200;
        public static List<string> columnOrder = new List<string>  { "rowIndex", "productsId",  "productName", "description",  "category", 
            "quantity",  "purchasePrice", "sellingPrice", "articleNumber", "minimumStock", "img"};
        // 1: for selectitem, 2: new image
        public static int currentPageDG = 0;
        public const int pageSizeDG = 11;
        public const int DebounceDelay = 500;
        public static dataProcessP? dpP = null;
        public static dataProcessLI? dpLI = null;
        public static List<string> keywordP = new List<string> { "rowIndex", "productsId", "productName", "category", "description", "articleNumber", "purchasePrice", "sellingPrice", "quantity", "minimumStock",
            "create", "edit", "delete", "clear", "uploadImg", "searchTB", "noData","img" };
        public static List<string> keywordL = new List<string> { "lang", "logIn", "products", "customer", "logOut", "exit" };
        public static List<string> keywordLI = new List<string> { "userName", "password", "logIn" };
        public static User currentUser = new User();
        public static frameProducts? staticFP = null;
        public static frameLogIn? staticFL = null;
        public static frameCustomers? staticFC = null;
        public static frameList? staticFLi = null;
    }
}
