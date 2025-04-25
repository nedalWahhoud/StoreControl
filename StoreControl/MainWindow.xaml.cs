using StoreControl.LogIn;
using System.Runtime.Intrinsics.Arm;
using System.Windows;
using System.Windows.Controls;

namespace StoreControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // check database connection
            dataConnectionAsyne();

            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        private async void dataConnectionAsyne()
        {
            int counter = 0;
            const int maxRetries = 3;
            bool bo = false;
            while (true)
            {
                bo = await Database.MyDbContext.CheckDatabaseConnection();
                if (bo)
                    break;
                if (counter >= maxRetries)
                    break;
                counter++;
            }
            Flags.isDatabaseConnected = bo;
        }
        private  void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Flags.isDatabaseConnected)
            {
                staticVariable.staticFLI = new frameList();
                frameList.Content = staticVariable.staticFLI;

                // if the frame is null, create a new one
                object content = dataProcessM.frameNewOrExists(typeof(frameLogIn));
                dataProcessM.frameMain(content);
            }
            else
                noDatabase.Visibility = Visibility.Visible;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            System.Windows.Size currentSizeMW = new System.Windows.Size(this.ActualWidth, this.ActualHeight);
            resizeFP(currentSizeMW);
            //
        }
        public void resizeFP(System.Windows.Size s)
        {
            if (staticVariable.staticFP != null && !Flags.isResizeFP)
            {
                if (staticVariable.staticFP.IsLoaded)
                {
                    Flags.isResizeFP = true;

                    double mainDefaultWidth = 1770;
                    var fp = (frameProducts)frameMain.Content;

                    // get mainWindows size
                    double sizeMwWidth = s.Width;
                    // get frameList size
                    double sizeFlWidth = staticVariable.staticFLI!.ActualWidth;
                    // get frameProducts size
                    double sizeFpWidth = this.ActualWidth;
                    // set new frameProducts size
                    double newFpWidth = sizeMwWidth - sizeFlWidth - 20;

                    fp.Width = newFpWidth;
                    gridMW.ColumnDefinitions[1].Width = new GridLength(newFpWidth);

                    // set the new size of the datagrid
                    fp.dataGrid.Width = newFpWidth - 35;
                    // set the new size of the Column od datagrid
                    double columnWidth = (newFpWidth - 75) / 11;
                    fp.dataGrid.Columns[0].Width = columnWidth;
                    fp.dataGrid.Columns[1].Width = columnWidth;
                    fp.dataGrid.Columns[2].Width = columnWidth;
                    fp.dataGrid.Columns[3].Width = columnWidth;
                    fp.dataGrid.Columns[4].Width = columnWidth;
                    fp.dataGrid.Columns[5].Width = columnWidth;
                    fp.dataGrid.Columns[6].Width = columnWidth;
                    fp.dataGrid.Columns[7].Width = columnWidth;
                    fp.dataGrid.Columns[8].Width = columnWidth;
                    fp.dataGrid.Columns[9].Width = columnWidth;
                    fp.dataGrid.Columns[10].Width = columnWidth;
                    // image size and position
                    double imgWidth = 350;
                    double defaultWidth = 350;
                    if (newFpWidth == mainDefaultWidth)
                        imgWidth = defaultWidth;
                    else if (newFpWidth < mainDefaultWidth)
                        imgWidth = (defaultWidth - ((newFpWidth - mainDefaultWidth) * 0.2));
                    else if (newFpWidth > mainDefaultWidth)
                        imgWidth = (defaultWidth + ((newFpWidth - mainDefaultWidth) * 0.2));
                    fp.img.Width = imgWidth;
                    // image height
                    double imgDefaultHeight = 300;
                    fp.img.Height = imgDefaultHeight;
                }
            }
            

            if (staticVariable.staticFLI != null)
            {
                // logOut button
                staticVariable.staticFLI.logOutB.Margin = new Thickness(21, s.Height - 200, 0, 0);
                // exit button
                staticVariable.staticFLI.exitB.Margin = new Thickness(21, s.Height - 100, 0, 0);
            }

            Flags.isResizeFP = false;
        }
    }
}