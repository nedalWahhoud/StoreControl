using StoreControl.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StoreControl.customers
{
    /// <summary>
    /// Interaktionslogik für frameCustomers.xaml
    /// </summary>
    public partial class frameCustomers : Page
    {
        private dataProcessC? dpC;
        public frameCustomers()
        {
            InitializeComponent();

            dpC ??= new dataProcessC();
            
            this.Loaded += FrameCustomers_Loaded;
        }
        // datagrid
        private void dataGrid_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            LoadMoreDataAsync();
        }
        private void LoadMoreDataAsync()
        {
            // check if dataGrid is empty
            dpC!.checkDataDG(dataGrid);
        }
        private void FrameCustomers_Loaded(object sender, RoutedEventArgs e)
        {
            dpC!.firstProcess();
        }
    }
}
