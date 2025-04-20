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
        private frameList frameList;
        private dataProcessJ dpJ;
        public dataProcessL(frameList frameList)
        {
            this.frameList = frameList;
            dpJ ??= new dataProcessJ();
            if (frameList.comboLang.Items.Count == 0) getComboBoxLang();
        }

        private void getComboBoxLang()
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

            frameList.lang.Content = translateds.Find(c => c.Item1.ToString() == "lang").Item2;
            frameList.logInB.Content = translateds.Find(c => c.Item1.ToString() == "logIn").Item2;
            frameList.exitB.Content = translateds.Find(c => c.Item1.ToString() == "products").Item2;
            frameList.customerB.Content = translateds.Find(c => c.Item1.ToString() == "customer").Item2;
            frameList.exitB.Content = translateds.Find(c => c.Item1.ToString() == "exit").Item2;

            return translateds;
        }
    }
}
