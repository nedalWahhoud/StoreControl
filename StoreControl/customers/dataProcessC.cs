using StoreControl.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Runtime.Intrinsics.Arm;

namespace StoreControl.customers
{
    internal class dataProcessC
    {
        public dataProcessC()
        {
            
        }
        public void firstProcess()
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    List<Translation> translations = context.translation
                        .Where(t => staticVariable.keywordC.Contains(t.Key_word))
                        .ToList();
                    List<(string, string)> translateds = setTranslation(translations);
                    // add Columns
                    staticVariable.staticFC!.dataGrid.Columns.Clear();
                    staticVariable.staticFC!.dataGrid.Items.Clear();

                    // add Items datagrid
                    addColumnsDG(translateds);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // datagrid
        public void addColumnsDG(List<(string, string)> translateds)
        {
            // dataGrid sorting 
            List<(string, string)> sortedColmuns = translateds
                .Where(x => staticVariable.columnOrderC.Any(co => co == x.Item1))
                .OrderBy(x => staticVariable.columnOrderC.FindIndex(co => co == x.Item1))
                .ToList();
        }
        public List<(string, string)> setTranslation(List<Translation> translations)
        {
            List<(string, string)> translateds = translations
             .Select(translation =>
               staticVariable.lang == "en"
               ? (translation.Key_word, translation.en)
               : (translation.Key_word, translation.de))
               .ToList();   

            staticVariable.staticFC!.customerId.Content = translateds.Find(c => c.Item1.ToString() == "customerId").Item2;
            staticVariable.staticFC!.firstName.Content = translateds.Find(c => c.Item1.ToString() == "firstName").Item2;
            staticVariable.staticFC!.lastName.Content = translateds.Find(c => c.Item1.ToString() == "lastName").Item2;
            staticVariable.staticFC!.phone.Content = translateds.Find(c => c.Item1.ToString() == "phone").Item2;
            staticVariable.staticFC!.email.Content = translateds.Find(c => c.Item1.ToString() == "email").Item2;
            staticVariable.staticFC!.street.Content = translateds.Find(c => c.Item1.ToString() == "street").Item2;
            staticVariable.staticFC!.cityLabel.Content = $"{translateds.Find(c => c.Item1.ToString() == "city").Item2}/{translateds.Find(c => c.Item1.ToString() == "country").Item2}";
            staticVariable.staticFC!.latitude.Content = translateds.Find(c => c.Item1.ToString() == "latitude").Item2;
            staticVariable.staticFC!.longitude.Content = translateds.Find(c => c.Item1.ToString() == "longitude").Item2;
            staticVariable.staticFC!.NoDataText.Text = translateds.Find(c => c.Item1.ToString() == "noData").Item2;
            return translateds;
        }
        public void checkDataDG(DataGrid dataGrid)
        {
            if (dataGrid.Items.Count == 0)
            {
                staticVariable.staticFC!.dataGrid.Visibility = Visibility.Collapsed;
                staticVariable.staticFC!.NoDataText.Visibility = Visibility.Visible;
            }
            else
            {
                staticVariable.staticFC!.dataGrid.Visibility = Visibility.Visible;
                staticVariable.staticFC!.NoDataText.Visibility = Visibility.Collapsed;
            }
        }
    }
}
