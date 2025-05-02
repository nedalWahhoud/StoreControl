using StoreControl.customers;
using StoreControl.Database;
using StoreControl.Json;
using StoreControl.ListF;
using StoreControl.LogIn;
using StoreControl.ProductsF;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace StoreControl
{
    /// <summary>
    /// Interaktionslogik für frameList.xaml
    /// </summary>
    public partial class frameList : Page
    {
        private MainWindow mainWindow;

        private dataProcessL? dpL;
        private dataProcessJ? dpJ;
        private dataProcessC? dpC;
        private dataProcessP? dpP;
        private dataProcessLI? dpLI;
        public frameList()
        {
            InitializeComponent();
            mainWindow = (MainWindow)Application.Current.MainWindow;
            this.Loaded += FrameList_Loaded;
        }
        private void FrameList_Loaded(object sender, RoutedEventArgs e)
        {
            dpL ??= new dataProcessL();
            dpJ ??= new dataProcessJ();
            dpC ??= new dataProcessC();
            dpP ??= new dataProcessP();
            dpLI ??= new dataProcessLI();

            // get saved language
            Lang lang = dpJ.getLang();
            staticVariable.lang = lang.lang;
            comboLang.SelectedItem = staticVariable.lang;
        }

        // buttons
        private void logInB_Click(object sender, RoutedEventArgs e)
        {
            // if the frame is null, create a new one
            object content = dataProcessM.frameNewOrExists(typeof(frameLogIn));
            dataProcessM.frameMain(content);
        }
        private  void productsB_Click(object sender, RoutedEventArgs e)
        {
            // if the frame is null, create a new one
            object content = dataProcessM.frameNewOrExists(typeof(frameProducts));
            dataProcessM.frameMain(content);
        }
        private void customerB_Click(object sender, RoutedEventArgs e)
        {
            // if the frame is null, create a new one
            object content = dataProcessM.frameNewOrExists(typeof(frameCustomers));
            dataProcessM.frameMain(content);
        }
        private void logOutB_Click(object sender, RoutedEventArgs e)
        {
            // clear all
            staticVariable.currentUser = new User();
            dpL!.buttonsEnable(false);
            // products
            if (staticVariable.staticFP != null)
            {
                dpP!.allClear(true, true);
                dpP!.dataGridClear();
            }
            // custimers
            if (staticVariable.staticFC != null)
            {
                dpC!.allClear(true);
                dpC!.dataGridClear();
            }
            // all frames empty
            staticVariable.staticFC = null;
            staticVariable.staticFP = null;
            staticVariable.staticFL = null;
            // to login frame
            object content = dataProcessM.frameNewOrExists(typeof(frameLogIn));
            dataProcessM.frameMain(content);
        }
        private void exitB_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        //
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
                    bool productsActive = staticVariable.staticFP != null;
                    bool logInActive = staticVariable.staticFL != null;
                    bool customerActive = staticVariable.staticFC != null;

                    List<Translation> translations = context.translation
                    .Where(t =>
                       staticVariable.keywordL.Contains(t.Key_word) ||
                       (productsActive && staticVariable.keywordP.Contains(t.Key_word)) ||
                       (logInActive && staticVariable.keywordLI.Contains(t.Key_word)) ||
                       (customerActive && staticVariable.keywordC.Contains(t.Key_word))
                       )
                       .ToList();

                    if (productsActive) dpP!.setTranslation(translations);
                    if (logInActive) dpLI!.setTranslation(translations);
                    if (customerActive) dpC!.setTranslation(translations);

                    dpL!.setTranslation(translations);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // update saved language
                dpJ!.updateLang(staticVariable.lang);
            }
        }
        
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            System.Windows.Size currentSizeMW = new System.Windows.Size(mainWindow.ActualWidth, mainWindow.ActualHeight);
            mainWindow.resizeFP(currentSizeMW);
        }

    }
}
