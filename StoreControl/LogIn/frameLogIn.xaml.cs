using StoreControl.Database;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Security.Cryptography;
using StoreControl.Json;
using Microsoft.EntityFrameworkCore;
using StoreControl.ListF;

namespace StoreControl.LogIn
{
    /// <summary>
    /// Interaktionslogik für frameLogIn.xaml
    /// </summary>
    public partial class frameLogIn : Page
    {
        private dataProcessJ dpJ;
        private dataProcessL? dpL;
        private dataProcessLI dpLI;
        public frameLogIn()
        {
            InitializeComponent();

            dpLI ??= new dataProcessLI();
            dpJ ??= new dataProcessJ();
            dpL ??= new dataProcessL();

            this.Loaded += FrameLogIn_Loaded;
        }

       
        private void logInB_Click(object sender, RoutedEventArgs e)
        {
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
                        staticVariable.currentUser = user;
                        dpL!.buttonsEnable(true);
                       
                        // as jason save (data suggestion, and user reminder zeck)
                        dpJ.saveOrUpdateUser(user);
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
        private void FrameLogIn_Loaded(object sender, RoutedEventArgs e)
        {
            dpLI.firstProcess();
        }
    }
}
