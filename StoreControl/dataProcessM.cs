using System.Net.Http;


namespace StoreControl
{
    internal class dataProcessM
    {
        public dataProcessM()
        {
            // Constructor logic here
        }
        public static async Task<bool> CheckInternetAsync()
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
        }
    }
}
