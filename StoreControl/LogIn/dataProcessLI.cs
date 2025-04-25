using StoreControl.Database;
using System.Windows;


namespace StoreControl.LogIn
{
    internal class dataProcessLI
    {
       
        public dataProcessLI()
        {
           
        }
        public void firstProcess()
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    List<Translation> translations = context.translation
                        .Where(t => staticVariable.keywordLI.Contains(t.Key_word))
                        .ToList();
                    setTranslation(translations);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public List<(string, string)> setTranslation(List<Translation> translations)
        {
            List<(string, string)> translateds = translations
             .Select(translation =>
               staticVariable.lang == "en"
               ? (translation.Key_word, translation.en)
               : (translation.Key_word, translation.de))
               .ToList();

            foreach (var translated in translateds)
            {
                staticVariable.staticFL!.userName.Content = translateds.Find(c => c.Item1.ToString() == "userName").Item2;
                staticVariable.staticFL!.passwort.Content = translateds.Find(c => c.Item1.ToString() == "password").Item2;
                staticVariable.staticFL!.logInB.Content = translateds.Find(c => c.Item1.ToString() == "logIn").Item2;
            }

            return translateds;
        }
    }
}
