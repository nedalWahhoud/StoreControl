using StoreControl.Database;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Security.Cryptography;
using StoreControl.Json;
using Microsoft.EntityFrameworkCore;

namespace StoreControl.LogIn
{
    /// <summary>
    /// Interaktionslogik für frameLogIn.xaml
    /// </summary>
    public partial class frameLogIn : Page
    {

        private MainWindow mainWindow;
        private frameList? frameList;
        private dataProcessJ dpJ;
        public frameLogIn()
        {
            InitializeComponent();

            if (staticVariable.dpLI == null)
                staticVariable.dpLI = new dataProcessLI(this);

            dpJ ??= new dataProcessJ();

            mainWindow = (MainWindow)Application.Current.MainWindow;

            // translation of this frame
            if (Flags.isDatabaseConnected)
                frameTranslation();
        }

        private  void frameTranslation()
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    List<Translation> translations =  context.translation
                        .Where(t => staticVariable.keywordLI.Contains(t.Key_word))
                        .ToList();
                    staticVariable.dpLI!.setTranslation(translations);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void logInB_Click(object sender, RoutedEventArgs e)
        {
            if (!await dataProcessM.CheckInternetAsync())
            {
                MessageBox.Show("No internet connection. Please check your connection and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var context = new MyDbContext())
            {
                try
                {
                    string userName = userNameCB.Text;
                    string password = passwordTB.Password;
                  
                    if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                    {
                        MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    string passwordHash = string.Concat(SHA256.HashData(Encoding.UTF8.GetBytes(password)).Select(b => b.ToString("x2")));

                    User user = context.users
                        .FirstOrDefault(u => u.userName == userName && u.passwordHash == passwordHash)!;

                    if (user != null)
                    {
                        frameList = (frameList)mainWindow.frameList.Content;
                        staticVariable.currentUser = user;
                        frameList.productsB.IsEnabled = true;
                        frameList.customerB.IsEnabled = true;
                        frameList.showFrameP();
                        frameList.logInB.Visibility = Visibility.Hidden;
                        frameList.productsB.Margin = new Thickness(frameList.productsB.Margin.Left, frameList.productsB.Margin.Top - 100, frameList.productsB.Margin.Right, frameList.productsB.Margin.Bottom);
                        frameList.customerB.Margin = new Thickness(frameList.customerB.Margin.Left, frameList.customerB.Margin.Top - 100, frameList.customerB.Margin.Right, frameList.customerB.Margin.Bottom);
                        // as jason save (data suggestion, and user reminder zeck)
                        dpJ.saveOrUpdateUser(user);
                        // 
                        Flags.isDatabaseConnected = true;
                    }
                    else
                    {
                        MessageBox.Show("Invalid login details", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        passwordTB.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
                }
            }
        }

        private void userNameCB_Selected(object sender, RoutedEventArgs e)
        {
            userNameCB.Text = userNameCB.SelectedItem.ToString()!;
        }

        private static string currentText = string.Empty;
        private async void userNameCB_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            string changedText = userNameCB.Text;
            //
            if(e.Key == Key.Down &&  userNameCB.SelectedIndex == -1)
                userNameCB.SelectedIndex = userNameCB.SelectedIndex + 1;

            if (e.Key == Key.Down || e.Key == Key.Up || e.Key == Key.Enter || e.Key == Key.Tab || currentText == changedText || string.IsNullOrWhiteSpace(changedText))
            {
               
               currentText = changedText;
               return; 
            }
            
            UsersData usersData = await dpJ.findUserByName(currentText,false);

            if (usersData.Users.Count > 0)
            {
                userNameCB.ItemsSource = usersData.Users;

                userNameCB.IsDropDownOpen = usersData.Users.Any();
            }
            
            currentText = changedText;
            userNameCB.Text = changedText;
            TextBox? templateChild = userNameCB.Template.FindName("PART_EditableTextBox", userNameCB) as TextBox;
            if (templateChild != null)
            {
                templateChild!.SelectionStart = userNameCB.Text.Length;
            }
        }

        private void passwordTB_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                logInB_Click(sender, e);
            }
        }

        private async void userNameCB_GotFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(userNameCB.Text))
            {

                UsersData usersData = await dpJ.findUserByName(null!, true);

                if (usersData.Users.Count > 0)
                {
                    userNameCB.ItemsSource = usersData.Users;

                    userNameCB.IsDropDownOpen = usersData.Users.Any();
                }
            }
        }
    }
}
