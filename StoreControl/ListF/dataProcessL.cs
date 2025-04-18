using StoreControl.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace StoreControl.ListF
{
    internal class dataProcessL
    {
        private frameList frameList;
        public dataProcessL(frameList frameList)
        {
            this.frameList = frameList;
            if (frameList.comboLang.Items.Count == 0) getComboBox();
        }

        private void getComboBox()
        {
            foreach (string st in staticVariable.langArray)
            {
                frameList.comboLang.Items.Add(st);
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
                if (translated.Item1 == "lang")
                    frameList.lang.Content = translated.Item2;
                else if(translated.Item1 == "exit")
                    frameList.exitB.Content = translated.Item2;
                else if (translated.Item1 == "products")
                    frameList.productsB.Content = translated.Item2;
                else if (translated.Item1 == "logIn")
                    frameList.logInB.Content = translated.Item2;
                else if (translated.Item1 == "clients")
                    frameList.clientsB.Content = translated.Item2;
            }

            return translateds;
        }
    }
}
