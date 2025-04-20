using StoreControl.Clients;
using StoreControl.Database;
using StoreControl.Json;
using StoreControl.ListF;
using StoreControl.LogIn;
using StoreControl.ProductsF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
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

namespace StoreControl
{
    /// <summary>
    /// Interaktionslogik für frameList.xaml
    /// </summary>
    public partial class frameList : Page
    {
        private MainWindow mainWindow;
        private dataProcessL dpL; 
        private dataProcessJ dpJ;
        public frameList()
        {
            InitializeComponent();
            mainWindow = (MainWindow)Application.Current.MainWindow;
            dpL ??= new dataProcessL(this);
            dpJ ??= new dataProcessJ();
            // get saved language
            Lang lang = dpJ.getLang();
            staticVariable.lang = lang.lang;
            comboLang.SelectedItem = staticVariable.lang;
        }
        // buttons
        private void logInB_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindow.frameLogIn == null || !(mainWindow.frameLogIn.Content is frameLogIn fl))
            {
                var fl1 = new frameLogIn();

                mainWindow.frameProducts!.Content = fl1;

                if (staticVariable.dpLI == null)
                    staticVariable.dpLI = new dataProcessLI(fl1);
            }
        }
        private  void productsB_Click(object sender, RoutedEventArgs e)
        {
            showFrameP();
        }
        private void exitB_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        //
        public void showFrameP()
        {
            if (mainWindow.frameProducts == null || !(mainWindow.frameProducts.Content is frameProducts fp))
            {
                mainWindow.frameLogIn.Content = null;
                var fp1 = new frameProducts();
             //   mainWindow.frameProducts!.Content = fp1;

                staticVariable.dpP = new dataProcessP(fp1);

                staticVariable.dpP!.mainProcess();

                mainWindow.frameMain.Content = fp1;


                mainWindow.frameProducts!.Content = fp1;
            }
            else
            {
                mainWindow.frameMain.Content = mainWindow.frameProducts.Content;
            }
        }
        private void comboLang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboLang.SelectedItem.ToString() == "en")
            {
                staticVariable.lang = "en";
            }
            else if (comboLang.SelectedItem.ToString() == "de")
            {
                staticVariable.lang = "de";
            }
            // set the new language
            try
            {
                using (var context = new MyDbContext())
                {
                    // cget from database translation only für configured frames
                    bool productsActive = mainWindow.frameProducts.Content != null;
                    bool logInActive = mainWindow.frameLogIn.Content != null;

                    List<Translation> translations = context.translation
                    .Where(t =>
                       staticVariable.keywordL.Contains(t.Key_word) ||
                       (productsActive && staticVariable.keywordP.Contains(t.Key_word)) ||
                       (logInActive && staticVariable.keywordLI.Contains(t.Key_word)))
                       .ToList();

                    if (productsActive) staticVariable.dpP!.setTranslation(translations);
                    if (logInActive) staticVariable.dpLI!.setTranslation(translations);

                    dpL.setTranslation(translations);
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // update saved language
                dpJ.updateLang(staticVariable.lang);
            }
        }
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            System.Windows.Size currentSizeMW = new System.Windows.Size(mainWindow.ActualWidth, mainWindow.ActualHeight);
            mainWindow.resizeFP(currentSizeMW);
        }

        private void clientsB_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindow.frameClients != null && !(mainWindow.frameClients.Content is frameClients))
            {
                var fc1 = new frameClients();

                mainWindow.frameMain.Content = fc1;
                mainWindow.frameClients.Content = fc1;
            }
            else
                mainWindow.frameMain.Content = mainWindow.frameClients!.Content;
        }
    }
}
