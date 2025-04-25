using StoreControl.customers;
using StoreControl.LogIn;
using System.Net.Http;
using System.Windows;


namespace StoreControl
{
    internal class dataProcessM
    {
        public dataProcessM()
        {

        }
        /*private static async Task<bool> CheckInternetAsync1()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Timeout auf 5 Sekunden setzen
                    client.Timeout = TimeSpan.FromSeconds(5);

                    // Anfrage an eine zuverlässige Website
                    HttpResponseMessage response = await client.GetAsync("https://www.google.com");

                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }
        }*/
        public static void frameMain(object frame)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.frameMain.Content = frame;
        }
        public static object frameNewOrExists(Type frame)
        {
            object? content = null;
            switch (frame)
            {
                case Type t when t == typeof(frameLogIn):
                    if (staticVariable.staticFL == null)
                    {
                        staticVariable.staticFL = new frameLogIn();
                        content = staticVariable.staticFL;
                    }
                    else
                        content = staticVariable.staticFL;
                    break;
                case Type t when t == typeof(frameProducts):
                    if (staticVariable.staticFP == null)
                    {
                        staticVariable.staticFP = new frameProducts();
                        content = staticVariable.staticFP;
                    }
                    else
                        content = staticVariable.staticFP;
                    break;
                case Type t when t == typeof(frameCustomers):
                    if (staticVariable.staticFC == null)
                    {
                        staticVariable.staticFC = new frameCustomers();
                        content = staticVariable.staticFC;
                    }
                    else
                        content = staticVariable.staticFC;
                    break;
            }

            return content!;
        }
      
    }
}
