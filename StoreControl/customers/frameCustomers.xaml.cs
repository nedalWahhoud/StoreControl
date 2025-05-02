using Microsoft.EntityFrameworkCore;
using StoreControl.Database;
using StoreControl.ProductsF;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            if (Flags.isLoadingDbC) 
                return;
            // if the last datagrid
            if (e.VerticalOffset == e.ExtentHeight - e.ViewportHeight)
            {
                 double offset = e.VerticalOffset;

                if (!Flags.isSearchingC)
                    _ = LoadMoreDataAsync();
                else if (Flags.isSearchingC)
                    _ = LoadMoreSearchedsAsync();

                if (dataGrid.Items.Count > 0)
                    dataGrid.ScrollIntoView(dataGrid.Items[(int)offset]);
            }
        }
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = dataGrid.SelectedItem as customerDG;
            if (selectedItem != null)
            {
                customerIdTB.Text = selectedItem.customersId.ToString();
                firstNameTB.Text = selectedItem.firstName;
                lastNameTB.Text = selectedItem.lastName;
                phoneTB.Text = selectedItem.phone;
                emailTB.Text = selectedItem.email;
                streetTB.Text = selectedItem.street;
                houseNummerTB.Text = selectedItem.houseNumber;
                postalCodeTB.Text = selectedItem.postalCode.ToString();
                cityTB.Text = selectedItem.city;
                countryTB.Text = selectedItem.country;
                latitudeTB.Text = selectedItem.latitude.ToString();
                longitudeTB.Text = selectedItem.longitude.ToString();
                // set current customer
                staticVariable.currentCustomer = selectedItem;
            }
        }
        private async Task LoadMoreDataAsync()
        {
            Flags.isLoadingDbC = true;

            List<Customers> customers = await dpC!.LoadMoreCustomers();
            if (customers.Count > 0)
            {
                dpC.addItemsDG(customers);
                // prevent the scroll changed from activating one after the other
                scrollDG();
            }
            // check if dataGrid is empty
            dpC.checkDataDG(dataGrid);
            Flags.isLoadingDbC = false;
        }
        private void scrollDG()
        {
             // prevent the scroll changed from activating one after the other
                if (dataGrid.Items.Count > 0 && dataGrid.Items.Count > staticVariable.pageSizeDgC)
                    dataGrid.ScrollIntoView(dataGrid.Items[dataGrid.Items.Count - staticVariable.pageSizeDgC]);
        }
        // buttons
        private async void newB_Click(object sender, RoutedEventArgs e)
        {
            Flags.isSearchingC = false;
            if (!dpC!.checkTextbox())
            {
                MessageBox.Show("Please fill all the fields.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var resault = MessageBox.Show("Do you want to add this Customer?", "Add Customer", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resault == MessageBoxResult.Yes)
            {
                Customers customer = dpC.getCustomer();
                try
                {
                    using (var context = new MyDbContext())
                    {
                        context.customers.Add(customer);
                        context.SaveChanges();
                        //
                        dpC.allClear(true);
                        dpC.dataGridClear();
                        //
                        await LoadMoreDataAsync();
                        // scroll to the first item,
                        if (dataGrid.Items.Count > 0)
                        {
                            dataGrid.SelectedItem = dataGrid.Items[0];
                            dataGrid.ScrollIntoView(dataGrid.Items[0]);
                        }
                    }
                    MessageBox.Show("Product added successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void editB_Click(object sender, RoutedEventArgs e)
        {
            // check if all textboxes are filled
            if (!dpC!.checkTextbox() || dataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please fill all the fields or not selected.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var selectedItem = dataGrid.SelectedItem as customerDG;

            int customersId = selectedItem!.customersId;

            Customers customers = dpC.getCustomer();

            bool isFirstNameChanged = staticVariable.currentCustomer.firstName != customers.firstName;
            bool isLastNameChanged = staticVariable.currentCustomer.lastName != customers.lastName;
            bool isPhoneChanged = staticVariable.currentCustomer.phone != customers.phone;
            bool isEmailChanged = staticVariable.currentCustomer.email != customers.email;
            bool isStreetChanged = staticVariable.currentCustomer.street != customers.Addresses!.street;
            bool isHouseNumberChanged = staticVariable.currentCustomer.houseNumber != customers.Addresses.houseNumber;
            bool isPostalCodeChanged = staticVariable.currentCustomer.postalCode != customers.Addresses.postalCode;
            bool isCityChanged = staticVariable.currentCustomer.city != customers.Addresses.city;
            bool isCountryChanged = staticVariable.currentCustomer.country != customers.Addresses.country;
            bool isLatitudeChanged = staticVariable.currentCustomer.latitude != customers.Addresses.latitude;
            bool isLongitudeChanged = staticVariable.currentCustomer.longitude != customers.Addresses.longitude;

            if (!isFirstNameChanged && !isLastNameChanged && !isPhoneChanged && !isEmailChanged && !isStreetChanged && !isHouseNumberChanged && !isPostalCodeChanged
                && !isCityChanged && !isCountryChanged && !isLatitudeChanged && !isLongitudeChanged)
            {
                MessageBox.Show("No changes made to the Customer.", "No Changes", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var resault = MessageBox.Show("Do you want to edit this Customer?", "Edit Customer", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resault == MessageBoxResult.Yes)
            {
                try
                {
                    using(var context = new MyDbContext())
                    {
                        var selectedCustomer = context.customers
                            .Include(c => c.Addresses)
                            .SingleOrDefault(c => c.customersId == customersId);
                        if (selectedCustomer == null)
                        {
                            MessageBox.Show("Product not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        // change in datab
                        if(isFirstNameChanged) selectedCustomer.firstName = customers.firstName;
                        if (isLastNameChanged) selectedCustomer.lastName = customers.lastName;
                        if (isPhoneChanged) selectedCustomer.phone = customers.phone;
                        if (isEmailChanged) selectedCustomer.email = customers.email;
                        if (isStreetChanged) selectedCustomer.Addresses!.street = customers.Addresses.street;
                        if (isHouseNumberChanged) selectedCustomer.Addresses!.houseNumber = customers.Addresses.houseNumber;
                        if (isPostalCodeChanged) selectedCustomer.Addresses!.postalCode = customers.Addresses.postalCode;
                        if (isCityChanged) selectedCustomer.Addresses!.city = customers.Addresses.city;
                        if (isCountryChanged) selectedCustomer.Addresses!.country = customers.Addresses.country;
                        if (isLatitudeChanged) selectedCustomer.Addresses!.latitude = customers.Addresses.latitude;
                        if (isLongitudeChanged) selectedCustomer.Addresses!.longitude = customers.Addresses.longitude;
                        
                        context.SaveChanges();
                        // datagrid update
                        var selectedItemDG = dataGrid.SelectedItem as customerDG;
                        if (selectedItemDG != null)
                        {
                            selectedItemDG.firstName = customers.firstName;
                            selectedItemDG.lastName = customers.lastName;
                            selectedItemDG.phone = customers.phone;
                            selectedItemDG.email = customers.email;
                            selectedItemDG.street = customers.Addresses!.street;
                            selectedItemDG.houseNumber = customers.Addresses.houseNumber;
                            selectedItemDG.postalCode = customers.Addresses.postalCode;
                            selectedItemDG.city = customers.Addresses.city;
                            selectedItemDG.country = customers.Addresses.country;
                            selectedItemDG.latitude = customers.Addresses.latitude;
                            selectedItemDG.longitude = customers.Addresses.longitude;
                            dataGrid.Items.Refresh();
                        }
                        MessageBox.Show("Customer updated successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void deleteB_Click(object sender, RoutedEventArgs e)
        {
            Flags.isSearchingC = false;
            if (dataGrid.SelectedItem != null)
            {
                try
                {
                    var selectedItem = dataGrid.SelectedItem as customerDG;

                    int customersId = selectedItem!.customersId;
                    var resault = MessageBox.Show("Do you want to delete this Customer?", "Delete Customer", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (resault == MessageBoxResult.Yes)
                    {
                        using (var context = new MyDbContext())
                        {
                            var selectedCustomer = context.customers
                                .Include(c => c.Addresses)
                                .SingleOrDefault(c => c.customersId == customersId);
                            if (selectedCustomer == null)
                            {
                                MessageBox.Show("Product not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            // delete from database
                            context.customers.Remove(selectedCustomer);
                            context.addresses.Remove(selectedCustomer.Addresses!);
                            
                            context.SaveChanges();

                            dpC!.allClear(true);
                            // delete from datagrid
                            dataGrid.Items.Remove(selectedItem);
                            // scroll to the first item,
                            dataGrid.SelectedItem = null;
                            dataGrid.Items.Refresh();
                            Keyboard.ClearFocus();
                            MessageBox.Show("Customer deleted successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
                MessageBox.Show("Please select a Customer.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);

        }
        private void clearB_Click(object sender, RoutedEventArgs e)
        {
            dpC!.allClear(true);
            if (dataGrid.SelectedItem != null)
                dataGrid.SelectedItem = null;
        }
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            System.Windows.Size currentSizeMW = new System.Windows.Size(mainWindow.ActualWidth, mainWindow.ActualHeight);
            mainWindow.resizeFP(currentSizeMW);
        }
        // search
        private async void searchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            dpC!.allClear(false);
            Flags.isSearchingC = true;
            staticVariable.currentPageDgC = 0;
            string keyWord = searchTB.Text.ToLower();
            // check if the search text is empty
            if (string.IsNullOrEmpty(keyWord))
            {
                Flags.isSearchingC = false;
                dataGrid.SelectedItem = null;
                dpC.dataGridClear();
                await LoadMoreDataAsync();
                await Task.Delay(500);
                if (dataGrid.Items.Count > 0)
                    dataGrid.ScrollIntoView(dataGrid.Items[0]);
                return;
            }
            dpC.dataGridClear();
            _ = LoadMoreSearchedsAsync();
        }
        private void xSearching_Click(object sender, RoutedEventArgs e)
        {
            searchTB.Text = null;
        }
        private async Task LoadMoreSearchedsAsync()
        {
            Flags.isLoadingDbC = true;
            string keyWord = searchTB.Text?.ToLower()!;
            if (!string.IsNullOrEmpty(keyWord))
            {
                List<Customers> customers = await dpC!.search(keyWord);
                if (customers.Count > 0)
                {
                    dpC.addItemsDG(customers);
                    // prevent the scroll changed from activating one after the other
                    scrollDG();
                }
            }
            Flags.isLoadingDbC = false;
        }
        //
        private void postalCodeTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }
        private void postalCodeTB_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }
        private void latitudeTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender == null) return;

            TextBox? textBox = sender as TextBox;

            // Neue Eingabe simulieren
            string newText = textBox!.Text.Insert(textBox.SelectionStart, e.Text);

            // Regex erlaubt nur: ganze Zahlen ODER Zahlen mit EINEM Komma
            Regex regex = new Regex(@"^\d{0,9}(,\d{0,6})?$");

            e.Handled = !regex.IsMatch(newText);
        }
        private void latitudeTB_Pasting(object sender, DataObjectPastingEventArgs e)
        {

            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string pasteText = (string)e.DataObject.GetData(typeof(string));
                Regex regex = new Regex(@"^\d{0,9}(,\d{0,6})?$");
                if (!regex.IsMatch(pasteText))
                    e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }
        private void longitudeTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender == null) return;

            TextBox? textBox = sender as TextBox;

            // Neue Eingabe simulieren
            string newText = textBox!.Text.Insert(textBox.SelectionStart, e.Text);

            // Regex erlaubt nur: ganze Zahlen ODER Zahlen mit EINEM Komma
            Regex regex = new Regex(@"^\d{0,9}(,\d{0,6})?$");

            e.Handled = !regex.IsMatch(newText);
        }
        private void longitudeTB_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string pasteText = (string)e.DataObject.GetData(typeof(string));
                Regex regex = new Regex(@"^\d{0,9}(,\d{0,6})?$");
                if (!regex.IsMatch(pasteText))
                    e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }
        private void FrameCustomers_Loaded(object sender, RoutedEventArgs e)
        {
            dpC!.firstProcess();
        }

    }
}
