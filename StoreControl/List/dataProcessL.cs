using StoreControl.Database;
using StoreControl.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace StoreControl.ListF
{
    internal class dataProcessL
    {
        private dataProcessJ dpJ;
        public dataProcessL()
        {
            dpJ ??= new dataProcessJ();
            if (staticVariable.staticFLi!.comboLang.Items.Count == 0) getComboBoxLang();
        }

        private void getComboBoxLang()
        {
            foreach (string st in staticVariable.langArray)
            {
                staticVariable.staticFLi!.comboLang.Items.Add(st);
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

            staticVariable.staticFLi!.lang.Content = translateds.Find(c => c.Item1.ToString() == "lang").Item2;
            staticVariable.staticFLi!.logInB.Content = translateds.Find(c => c.Item1.ToString() == "logIn").Item2;
            staticVariable.staticFLi!.productsB.Content = translateds.Find(c => c.Item1.ToString() == "products").Item2;
            staticVariable.staticFLi!.customerB.Content = translateds.Find(c => c.Item1.ToString() == "customer").Item2;
            staticVariable.staticFLi!.logOutB.Content = translateds.Find(c => c.Item1.ToString() == "logOut").Item2;
            staticVariable.staticFLi!.exitB.Content = translateds.Find(c => c.Item1.ToString() == "exit").Item2;

            return translateds;
        }
        public void buttonsEnable(bool ifLogIn)
        {
            if (ifLogIn)
            {
                staticVariable.staticFLi!.productsB.IsEnabled = true;
                staticVariable.staticFLi!.customerB.IsEnabled = true;
                staticVariable.staticFLi!.logInB.Visibility = Visibility.Hidden;
                staticVariable.staticFLi!.logOutB.IsEnabled = true;
                // logInb hidden or disabled and buttons and Arrangement of another Buttons key positions
                staticVariable.staticFLi!.productsB.Margin = new Thickness(staticVariable.staticFLi!.productsB.Margin.Left, staticVariable.staticFLi!.productsB.Margin.Top - 100, staticVariable.staticFLi!.productsB.Margin.Right, staticVariable.staticFLi!.productsB.Margin.Bottom);
                staticVariable.staticFLi!.customerB.Margin = new Thickness(staticVariable.staticFLi!.customerB.Margin.Left, staticVariable.staticFLi!.customerB.Margin.Top - 100, staticVariable.staticFLi!.customerB.Margin.Right, staticVariable.staticFLi!.customerB.Margin.Bottom);
                // if the frame is null, create a new one
                object content = dataProcessM.frameNewOrExists(typeof(frameProducts));
                dataProcessM.frameMain(content);
            }
            else
            {
                staticVariable.staticFLi!.productsB.IsEnabled = false;
                staticVariable.staticFLi!.customerB.IsEnabled = false;
                staticVariable.staticFLi!.logInB.Visibility = Visibility.Visible;
                staticVariable.staticFLi!.logOutB.IsEnabled = false;
                // logInb hidden or disabled and buttons and Arrangement of another Buttons key positions
                staticVariable.staticFLi!.productsB.Margin = new Thickness(staticVariable.staticFLi!.productsB.Margin.Left, staticVariable.staticFLi!.productsB.Margin.Top + 100, staticVariable.staticFLi!.productsB.Margin.Right, staticVariable.staticFLi!.productsB.Margin.Bottom);
                staticVariable.staticFLi!.customerB.Margin = new Thickness(staticVariable.staticFLi!.customerB.Margin.Left, staticVariable.staticFLi!.customerB.Margin.Top + 100, staticVariable.staticFLi!.customerB.Margin.Right, staticVariable.staticFLi!.customerB.Margin.Bottom);
            }
        }
    }
}
