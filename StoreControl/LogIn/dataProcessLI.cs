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
                if (translated.Item1 == "userName")
                    frameLogIn.userName.Content = translated.Item2;
                else if (translated.Item1 == "password")
                    frameLogIn.passwort.Content = translated.Item2;
                else if (translated.Item1 == "logIn")
                    frameLogIn.logInB.Content = translated.Item2;
            }

            return translateds;
        }
    }
}
