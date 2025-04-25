using StoreControl.Database;
using StoreControl.Json;
using System.Windows;

namespace StoreControl.ListF
{
    internal class dataProcessL
    {
        private dataProcessJ dpJ;
        public dataProcessL()
        {
            dpJ ??= new dataProcessJ();
            if (staticVariable.staticFLI!.comboLang.Items.Count == 0) getComboBoxLang();
        }

        private void getComboBoxLang()
        {
            foreach (string st in staticVariable.langArray)
            {
                staticVariable.staticFLI!.comboLang.Items.Add(st);
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

            staticVariable.staticFLI!.lang.Content = translateds.Find(c => c.Item1.ToString() == "lang").Item2;
            staticVariable.staticFLI!.logInB.Content = translateds.Find(c => c.Item1.ToString() == "logIn").Item2;
            staticVariable.staticFLI!.productsB.Content = translateds.Find(c => c.Item1.ToString() == "products").Item2;
            staticVariable.staticFLI!.customerB.Content = translateds.Find(c => c.Item1.ToString() == "customer").Item2;
            staticVariable.staticFLI!.logOutB.Content = translateds.Find(c => c.Item1.ToString() == "logOut").Item2;
            staticVariable.staticFLI!.exitB.Content = translateds.Find(c => c.Item1.ToString() == "exit").Item2;

            return translateds;
        }
        public void buttonsEnable(bool ifLogIn)
        {
            if (ifLogIn)
            {
                staticVariable.staticFLI!.productsB.IsEnabled = true;
                staticVariable.staticFLI!.customerB.IsEnabled = true;
                staticVariable.staticFLI!.logInB.Visibility = Visibility.Hidden;
                staticVariable.staticFLI!.logOutB.IsEnabled = true;
                // logInb hidden or disabled and buttons and Arrangement of another Buttons key positions
                staticVariable.staticFLI!.productsB.Margin = new Thickness(staticVariable.staticFLI!.productsB.Margin.Left, staticVariable.staticFLI!.productsB.Margin.Top - 100, staticVariable.staticFLI!.productsB.Margin.Right, staticVariable.staticFLI!.productsB.Margin.Bottom);
                staticVariable.staticFLI!.customerB.Margin = new Thickness(staticVariable.staticFLI!.customerB.Margin.Left, staticVariable.staticFLI!.customerB.Margin.Top - 100, staticVariable.staticFLI!.customerB.Margin.Right, staticVariable.staticFLI!.customerB.Margin.Bottom);
                // if the frame is null, create a new one
                object content = dataProcessM.frameNewOrExists(typeof(frameProducts));
                dataProcessM.frameMain(content);
            }
            else
            {
                staticVariable.staticFLI!.productsB.IsEnabled = false;
                staticVariable.staticFLI!.customerB.IsEnabled = false;
                staticVariable.staticFLI!.logInB.Visibility = Visibility.Visible;
                staticVariable.staticFLI!.logOutB.IsEnabled = false;
                // logInb hidden or disabled and buttons and Arrangement of another Buttons key positions
                staticVariable.staticFLI!.productsB.Margin = new Thickness(staticVariable.staticFLI!.productsB.Margin.Left, staticVariable.staticFLI!.productsB.Margin.Top + 100, staticVariable.staticFLI!.productsB.Margin.Right, staticVariable.staticFLI!.productsB.Margin.Bottom);
                staticVariable.staticFLI!.customerB.Margin = new Thickness(staticVariable.staticFLI!.customerB.Margin.Left, staticVariable.staticFLI!.customerB.Margin.Top + 100, staticVariable.staticFLI!.customerB.Margin.Right, staticVariable.staticFLI!.customerB.Margin.Bottom);
            }
        }
    }
}
