using StoreControl.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Runtime.Intrinsics.Arm;
using System.Windows.Data;
using Microsoft.EntityFrameworkCore;

namespace StoreControl.customers
{
    internal class dataProcessC
    {
        public dataProcessC()
        {

        }
        public void firstProcess()
        {
            if (staticVariable.staticFC!.dataGrid.Columns.Count == 0)
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
        }
        // datagrid
        public async Task<List<Customers>> LoadMoreCustomers()
        {
            if (Flags.allCustomersLoadedDB) return new List<Customers>();

            List<Customers> customers = new List<Customers>();
            try
            {
                using (var context = new MyDbContext())
                {
                    var query = context.customers
                        .Include(c => c.Addresses)
                        .AsQueryable();

                    query = query.Where(c => c.userId == staticVariable.currentUser.userId);

                    var customers1 = await query
                        .OrderByDescending(c => c.customersId)
                        .Skip(staticVariable.currentPageDgC * staticVariable.pageSizeDgC)
                        .Take(staticVariable.pageSizeDgC)
                        .ToListAsync();

                    if (customers1.Count == 0)
                    {
                        Flags.allCustomersLoadedDB = true;
                        return customers1;
                    }

                    customers = customers1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                staticVariable.currentPageDgC++;
            }
            return customers;
        }
        public void addItemsDG(List<Customers> customers)
        {
            int existingCount = staticVariable.staticFC!.dataGrid.Items.Count;
            int rowIndex = 0;
            for (int i = 0; i < customers.Count; i++)
            {
                rowIndex = existingCount + i + 1;
                customerDG customerDG = new customerDG()
                {
                    rowIndex = rowIndex,
                    customersId = customers[i].customersId,
                    firstName = customers[i].firstName,
                    lastName = customers[i].lastName,
                    phone = customers[i].phone,
                    email = customers[i].email,
                    addressId = customers[i].addressesId,
                    street = customers[i].Addresses!.street,
                    houseNumber = customers[i].Addresses!.houseNumber,
                    postalCode = customers[i].Addresses!.postalCode,
                    city = customers[i].Addresses!.city,
                    country = customers[i].Addresses!.country,
                    latitude = customers[i].Addresses!.latitude,
                    longitude = customers[i].Addresses!.longitude,
                    userId = customers[i].userId,
                };

                staticVariable.staticFC.dataGrid.Items.Add(customerDG);
            }
        }
        public void addColumnsDG(List<(string, string)> translateds)
        {
            // dataGrid sorting 
            List<(string, string)> sortedColmuns = translateds
                .Where(x => staticVariable.columnOrderC.Any(co => co == x.Item1))
                .OrderBy(x => staticVariable.columnOrderC.FindIndex(co => co == x.Item1))
                .ToList();

            double totalWidth = 0;
            double columnWidth = 130;
            double rowHeight = 100;
            foreach (var st in sortedColmuns)
            {
                // set warping and trimming
                var textFactory = new FrameworkElementFactory(typeof(TextBlock));
                textFactory.SetBinding(TextBlock.TextProperty, new Binding(st.Item1));
                textFactory.SetValue(TextBlock.TextWrappingProperty, TextWrapping.Wrap);
                textFactory.SetValue(TextBlock.TextTrimmingProperty, TextTrimming.None);
                var dataTemplate = new DataTemplate { VisualTree = textFactory };

                // column
                var column = new DataGridTemplateColumn()
                {
                    Header = st.Item2,
                    CellTemplate = dataTemplate,
                    Width = columnWidth
                };

                staticVariable.staticFC!.dataGrid.Columns.Add(column);
                totalWidth += columnWidth;
            }
            // datagrid properties
            staticVariable.staticFC!.dataGrid.RowHeight = rowHeight;
            staticVariable.staticFC.dataGrid.Width = totalWidth;
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
        // translation
        public List<(string, string)> setTranslation(List<Translation> translations)
        {
            List<(string, string)> translateds = translations
             .Select(translation =>
               staticVariable.lang == "en"
               ? (translation.Key_word, translation.en)
               : (translation.Key_word, translation.de))
               .ToList();

            staticVariable.staticFC!.customerId.Content = translateds.Find(c => c.Item1.ToString() == "customerId").Item2;
            staticVariable.staticFC.firstName.Content = translateds.Find(c => c.Item1.ToString() == "firstName").Item2;
            staticVariable.staticFC.lastName.Content = translateds.Find(c => c.Item1.ToString() == "lastName").Item2;
            staticVariable.staticFC.phone.Content = translateds.Find(c => c.Item1.ToString() == "phone").Item2;
            staticVariable.staticFC.email.Content = translateds.Find(c => c.Item1.ToString() == "email").Item2;
            staticVariable.staticFC.street.Content = translateds.Find(c => c.Item1.ToString() == "street").Item2;
            staticVariable.staticFC.cityLabel.Content = $"{translateds.Find(c => c.Item1.ToString() == "city").Item2}/{translateds.Find(c => c.Item1.ToString() == "country").Item2}";
            staticVariable.staticFC.latitude.Content = translateds.Find(c => c.Item1.ToString() == "latitude").Item2;
            staticVariable.staticFC.longitude.Content = translateds.Find(c => c.Item1.ToString() == "longitude").Item2;
            staticVariable.staticFC.NoDataText.Text = translateds.Find(c => c.Item1.ToString() == "noData").Item2;
            // buttons
            staticVariable.staticFC.newB.Content = translateds.Find(c => c.Item1.ToString() == "create").Item2;
            staticVariable.staticFC.editB.Content = translateds.Find(c => c.Item1.ToString() == "edit").Item2;
            staticVariable.staticFC.deleteB.Content = translateds.Find(c => c.Item1.ToString() == "delete").Item2;
            staticVariable.staticFC.clearB.Content = translateds.Find(c => c.Item1.ToString() == "clear").Item2;
            textboxBackground(translateds.Find(c => c.Item1.ToString() == "searchTB").Item2);
            return translateds;
        }
        // Search
        public async Task<List<Customers>> search(string searchText)
        {
            List<Customers> customers = new List<Customers>();
            try
            {
                using (var context = new MyDbContext())
                {
                    var query = context.customers
                        .Include(c => c.Addresses)
                        .AsQueryable();
                    query = query.Where(c => c.userId == staticVariable.currentUser.userId &&
                    (c.firstName!.Contains(searchText) || c.lastName!.Contains(searchText) || c.phone!.Contains(searchText) || c.email!.Contains(searchText)
                    // address
                    || c.Addresses!.street!.Contains(searchText) || c.Addresses!.postalCode!.ToString().Contains(searchText) || c.Addresses!.city!.Contains(searchText) || c.Addresses!.country!.Contains(searchText)));

                    var customers1 = await query
                        .OrderByDescending(c => c.customersId)
                        .Skip(staticVariable.currentPageDgC * staticVariable.pageSizeDgC)
                        .Take(staticVariable.pageSizeDgC)
                        .ToListAsync();

                    customers = customers1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                staticVariable.currentPageDgC++;
            }
            return customers;
        }
        public bool checkTextbox()
        {
            foreach (TextBox textBox in staticVariable.staticFC!.gridC.Children.OfType<TextBox>())
            {
                if (textBox.Name != "customerIdTB" && textBox.Name != "emailTB" && textBox.Name != "latitudeTB" && textBox.Name != "longitudeTB" && textBox.Name != "searchTB")
                {
                    if (string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        public Customers getCustomer()
        {
            Customers customer = new Customers()
            {
                firstName = staticVariable.staticFC!.firstNameTB.Text,
                lastName = staticVariable.staticFC!.lastNameTB.Text,
                phone = staticVariable.staticFC!.phoneTB.Text,
                email = staticVariable.staticFC!.emailTB.Text,
                Addresses = new Addresses()
                {
                    street = staticVariable.staticFC!.streetTB.Text,
                    houseNumber = staticVariable.staticFC!.houseNummerTB.Text,
                    postalCode = int.Parse(staticVariable.staticFC!.postalCodeTB.Text),
                    city = staticVariable.staticFC!.cityTB.Text,
                    country = staticVariable.staticFC!.countryTB.Text,
                    latitude = string.IsNullOrEmpty(staticVariable.staticFC!.latitudeTB.Text) ? 0 : decimal.Parse(staticVariable.staticFC!.latitudeTB.Text),
                    longitude = string.IsNullOrEmpty(staticVariable.staticFC!.longitudeTB.Text) ? 0 : decimal.Parse(staticVariable.staticFC!.longitudeTB.Text),
                    userId = staticVariable.currentUser.userId,
                },
                userId = staticVariable.currentUser.userId,
            };
            return customer;
        }
        public void allClear(bool withSearchTB)
        {
            foreach (TextBox textBox in staticVariable.staticFC!.gridC.Children.OfType<TextBox>())
            {
                if (textBox.Name == "searchTB")
                {
                    if (withSearchTB)
                        textBox.Clear();
                    continue;
                }

                textBox.Clear();
            }
            staticVariable.currentCustomer = new customerDG();
            Flags.isSearchingC = false;
        }
        public void dataGridClear()
        {
            staticVariable.currentPageDgC = 0;
            Flags.allCustomersLoadedDB = false;
            Flags.isLoadingDbC = false;
            staticVariable.staticFC!.dataGrid.Items.Clear();
        }
        private void textboxBackground(string searchTB)
        {
            TextBlock watermarkTextBlock = (TextBlock)staticVariable.staticFC!.FindResource("WatermarkTextBlock");
            watermarkTextBlock.Text = searchTB;
        }
    }
}
