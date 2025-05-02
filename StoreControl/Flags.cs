using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreControl
{
    internal class Flags
    {
        public static bool isLoadingDB { get; set; } = false;
        public static bool isLoadingDbC { get; set; } = false;
        public static bool isSearching { get; set; } = false;
        public static bool isSearchingC { get; set; } = false;
        public static bool isImageChanged { get; set; } = false;
        public static bool isResizeFP { get; set; } = false;
        public static bool minimumStockMode { get; set; } = false;
        public static bool allProductsLoadedDB = false;
        public static bool allCustomersLoadedDB = false;
        public static bool isDatabaseConnected { set; get; } = true;
    }
}
