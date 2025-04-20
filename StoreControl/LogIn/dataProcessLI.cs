using StoreControl.Database;


namespace StoreControl.LogIn
{
    internal class dataProcessLI
    {
        private frameLogIn frameLogIn;
        public dataProcessLI(frameLogIn frameLogIn)
        {
            this.frameLogIn = frameLogIn;
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
                frameLogIn.userName.Content = translateds.Find(c => c.Item1.ToString() == "userName").Item2;
                frameLogIn.passwort.Content = translateds.Find(c => c.Item1.ToString() == "password").Item2;
                frameLogIn.logInB.Content = translateds.Find(c => c.Item1.ToString() == "logIn").Item2;
            }

            return translateds;
        }
    }
}
