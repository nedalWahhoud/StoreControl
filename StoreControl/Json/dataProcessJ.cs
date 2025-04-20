using StoreControl.Database;
using System.IO;
using System.Text.Json;


namespace StoreControl.Json
{
    internal class dataProcessJ
    {
        public dataProcessJ() { }

        public string ifJsonFileExists(string fileName, object value)
        {
            // directory 
            string programPath = AppDomain.CurrentDomain.BaseDirectory;
            string directpryJason = programPath + "Json";
            if (!Directory.Exists(directpryJason)) Directory.CreateDirectory(directpryJason);
            // file
            string fullPath = Path.Combine(directpryJason, fileName);
            if (!File.Exists(fullPath))
            {
                File.Create(fullPath).Close();
                // create default data 
                File.WriteAllText(fullPath, JsonSerializer.Serialize(value, new JsonSerializerOptions { WriteIndented = true }));
            }

            return fullPath;
        }
        public async void saveOrUpdateUser(User currentUserData)
        {
            try
            {
                string fullPath = ifJsonFileExists("usersData.json", new UsersData());
                //
                string jsonString = await File.ReadAllTextAsync(fullPath);
                UsersData UsersData = JsonSerializer.Deserialize<UsersData>(jsonString) ?? new UsersData();

                // check if user already exists
                var existingUser = UsersData.Users.FirstOrDefault(u => u.userName == currentUserData.userName);

                if (existingUser == null)
                {
                    // Add new user
                    UsersData.Users.Add(currentUserData);
                }
                else
                {
                    // Update existing user
                    existingUser.passwordHash = currentUserData.passwordHash;
                    existingUser.CreatedAt = currentUserData.CreatedAt;
                }

                string updatedJson = JsonSerializer.Serialize(UsersData, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(fullPath, updatedJson);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., file not found, JSON parsing errors)
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        public async Task<UsersData> findUserByName(string name, bool ifFirstItems)
        {
            if (string.IsNullOrEmpty(name) && !ifFirstItems) return new UsersData();
            try
            {
                string fullPath = ifJsonFileExists("usersData.json", new UsersData());
                //
                string jsonString = await File.ReadAllTextAsync(fullPath);
                UsersData UsersData = JsonSerializer.Deserialize<UsersData>(jsonString) ?? new UsersData();

                var query = UsersData.Users
                    .AsQueryable();

                if (ifFirstItems)
                    query = query.Take(10);
                else
                    query = query.Where(u => u.userName!.Contains(name));

                var existingUser = query.ToList();

                return new UsersData { Users = existingUser };

            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., file not found, JSON parsing errors)
                Console.WriteLine($"Error: {ex.Message}");
                return new UsersData();
            }
        }


        public async void updateLang(string? currentLang)
        {
            try
            {
                string fullPath = ifJsonFileExists("lang.json", new Lang());
                //
                string jsonString = await File.ReadAllTextAsync(fullPath);
                Lang lang = JsonSerializer.Deserialize<Lang>(jsonString) ?? new Lang();

                // einfach update die Lang in json
                lang.lang = currentLang!;

                string updatedJson = JsonSerializer.Serialize(lang, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(fullPath, updatedJson);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., file not found, JSON parsing errors)
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        public Lang getLang()
        {
            try
            {
                string fullPath = ifJsonFileExists("lang.json", new Lang());
                //
                string jsonString =  File.ReadAllText(fullPath);
                Lang lang = JsonSerializer.Deserialize<Lang>(jsonString) ?? new Lang();

                return new Lang { lang = lang.lang };
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., file not found, JSON parsing errors)
                Console.WriteLine($"Error: {ex.Message}");
                return new Lang();
            }
        }
    }
}
