using StoreControl.Database;
using StoreControl.ProductsF;
using Microsoft.Win32;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StoreControl
{
    /// <summary>
    /// Interaktionslogik für frameProducts.xaml
    /// </summary>
    public partial class frameProducts : Page
    {
        
        private dataProcessP? dpP;
        public frameProducts()
        {
            InitializeComponent();

            dpP ??= new dataProcessP();
            // load completed
            this.Loaded += FrameProducts_Loaded;
        }
       
        // search
        private async void searchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            dpP!.allClear(true);
            Flags.isSearching = true;
            staticVariable.currentPageDG = 0;
            string keyWord = searchTB.Text?.ToLower()!;
            // check if the search text is empty
            if (string.IsNullOrEmpty(keyWord))
            {
                Flags.isSearching = false;
                dataGrid.SelectedItem = null;
                Keyboard.ClearFocus();
                await Task.Delay(staticVariable.DebounceDelay);
                dpP!.dataGridClear();
                await LoadMoreProductsAsync();
                if (dataGrid.Items.Count > 0)
                    dataGrid.ScrollIntoView(dataGrid.Items[0]);
                return;
            }

            dpP!.dataGridClear();
            
            await LoadMoreSearchedProductsAsync();
            
        }
        private void xSearching_Click(object sender, RoutedEventArgs e)
        {
            searchTB.Text = null;
        }
        // datagrid
        private async void dataGrid_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if(Flags.isLoadingDB) return;
            if (e.VerticalOffset == e.ExtentHeight - e.ViewportHeight)
            {

               
                // normal loading
                if (!Flags.isSearching)
                    await LoadMoreProductsAsync();
                else if (Flags.isSearching)
                {
                    await LoadMoreSearchedProductsAsync();
                }
               
                await Task.Delay(300); 
                dataGrid.Items.Refresh();
            }
        }
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = dataGrid.SelectedItem;
            if (selectedItem != null)
            {
                // always with new selection the uploaded image is no longer necessary
                Flags.isImageChanged = false;

                productIdTB.Text = selectedItem.GetType().GetProperty(staticVariable.columnOrderP.Find(c => c == "productsId")!)?.GetValue(selectedItem)!.ToString();
                productNameTB.Text = selectedItem.GetType().GetProperty(staticVariable.columnOrderP.Find(c => c == "productName")!)?.GetValue(selectedItem)!.ToString();
                descriptionTB.Text = selectedItem.GetType().GetProperty(staticVariable.columnOrderP.Find(c => c == "description")!)?.GetValue(selectedItem)!.ToString();
                categoryCB.SelectedValue = (selectedItem.GetType().GetProperty("Category")?.GetValue(selectedItem) as Categories)!.categoriesId;
                quantityTB.Text = selectedItem.GetType().GetProperty(staticVariable.columnOrderP.Find(c => c == "quantity")!)?.GetValue(selectedItem)!.ToString();
                purchasePriceTB.Text = selectedItem.GetType().GetProperty(staticVariable.columnOrderP.Find(c => c == "purchasePrice")!)?.GetValue(selectedItem)!.ToString();
                sellingPriceTB.Text = selectedItem.GetType().GetProperty(staticVariable.columnOrderP.Find(c => c == "sellingPrice")!)?.GetValue(selectedItem)!.ToString();
                articleNumberTB.Text = selectedItem.GetType().GetProperty(staticVariable.columnOrderP.Find(c => c == "articleNumber")!)?.GetValue(selectedItem)!.ToString();
                minimumStockTB.Text = selectedItem.GetType().GetProperty(staticVariable.columnOrderP.Find(c => c == "minimumStock")!)?.GetValue(selectedItem)!.ToString();
                img.Source = dpP!.ConvertByteToImage((selectedItem.GetType().GetProperty(staticVariable.columnOrderP.Find(c => c == "img")!)?.GetValue(selectedItem) as byte[])!);
                // set current product
                Products product = dpP!.getProduct(true,false)!;
                staticVariable.currentProduct = product;
            }
        }
        //
        public async Task LoadMoreSearchedProductsAsync()
        {
            Flags.isLoadingDB = true;
            string keyWord = searchTB.Text?.ToLower()!;
            if (!string.IsNullOrEmpty(keyWord))
            {
                List<Products> searchedProducts = await dpP!.Search(keyWord, false);
                if (searchedProducts != null)
                    dpP!.addItemsDG(searchedProducts, new boolInt(false, 0));
            }
            Flags.isLoadingDB = false;
        }
        public async Task LoadMoreProductsAsync()
        {
            Flags.isLoadingDB = true;
            List<Products> task = await dpP!.LoadMoreProducts();
            if (task != null)
                dpP!.addItemsDG(task, new boolInt(false, 0));

            dpP!.checkDataDG(dataGrid);
            Flags.isLoadingDB = false;
        }
        // buttons
        private async void newB_Click(object sender, RoutedEventArgs e)
        {
            Flags.isSearching = false;
            Flags.isImageChanged = false;

            if (!dpP!.checkTextbox())
            {
                MessageBox.Show("Please fill all the fields.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
           
            var resault = MessageBox.Show("Do you want to add this product?", "Add Product", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resault == MessageBoxResult.Yes)
            {
                // without category for database
                Products products = dpP!.getProduct(false,false)!;

                using (var context = new MyDbContext())
                {
                    try
                    {
                        // add database
                        context.products.Add(products);
                        context.SaveChanges();
                        
                        dataGrid.Items.Refresh();
                        searchTB.Clear();
                        dpP!.allClear(true);
                        dpP!.dataGridClear();
                        await LoadMoreProductsAsync();

                        // scroll to the first item,
                        if (dataGrid.Items.Count > 0)
                        {
                            dataGrid.SelectedItem = dataGrid.Items[0];
                            dataGrid.ScrollIntoView(dataGrid.Items[0]);
                        }
                        MessageBox.Show("Product added successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

            }
        }
        private void deleteB_Click(object sender, RoutedEventArgs e)
        {
            Flags.isSearching = false;
            Flags.isImageChanged = false;
            if (dataGrid.SelectedItem != null)
            {

                using (var context = new MyDbContext())
                {
                    try
                    {
                        int productsId = int.Parse(dataGrid.SelectedItem.GetType().GetProperty(staticVariable.columnOrderP.Find(c => c == "productsId")!)!.GetValue(dataGrid.SelectedItem)!.ToString()!);

                        var result = MessageBox.Show("Do you want to delete this product?", "Delete Product", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            var selectedProduct = context.products.SingleOrDefault(p => p.productsId == productsId);
                            int selectedIndex = dataGrid.SelectedIndex;
                            // delete from database
                            context.products.Remove(selectedProduct!);
                            context.SaveChanges();

                            searchTB.Clear();
                            dpP!.allClear(true);
                            // delete from datagrid
                            dataGrid.Items.Remove(dataGrid.SelectedItem);
                            // scroll to the first item,
                            dataGrid.SelectedItem = null;
                            Keyboard.ClearFocus();
                            MessageBox.Show("Product deleted successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

            }
            else
                MessageBox.Show("Please select a product to delete", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void editB_Click(object sender, RoutedEventArgs e)
        {
            // check if all textboxes are filled
            if (!dpP!.checkTextbox() || dataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please fill all the fields or not selected.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //
            int productsId = int.Parse(dataGrid.SelectedItem!.GetType().GetProperty(staticVariable.columnOrderP.Find(c => c == "productsId")!)!.GetValue(dataGrid.SelectedItem)!.ToString()!);
            // Compare with the original values ​​of StaticVariable.currentProduct
            Products products = dpP!.getProduct(true,false)!;
            bool isProductNameChanged = staticVariable.currentProduct.productName != products.productName;
            bool isDescriptionChanged = staticVariable.currentProduct.description != products.description;
            bool isCategoryChanged = staticVariable.currentProduct.Category!.categoriesId != products.categoriesId;
            bool isArticleNumberChanged = staticVariable.currentProduct.articleNumber != products.articleNumber;
            bool isQuantityChanged = staticVariable.currentProduct.quantity != products.quantity;
            bool isPurchasePriceChanged = staticVariable.currentProduct.purchasePrice != products.purchasePrice;
            bool isSellingPriceChanged = staticVariable.currentProduct.sellingPrice != products.sellingPrice;
            bool isMinimumStockChanged = staticVariable.currentProduct.minimumStock != products.minimumStock;
            bool isImageChanged = dpP!.imageCompare();

            // Check if any of the values have changed
            if (!isProductNameChanged && !isDescriptionChanged && !isCategoryChanged && !isArticleNumberChanged && !isQuantityChanged &&
                !isPurchasePriceChanged && !isSellingPriceChanged && !isMinimumStockChanged && !isImageChanged)
            {
                MessageBox.Show("No changes made to the product.", "No Changes", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show("Do you want to update this product?", "Update Product", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                using (var context = new MyDbContext())
                {
                    try
                    {
                        var selectedProduct = context.products.SingleOrDefault(p => p.productsId == productsId);
                        if (selectedProduct == null)
                        {
                            MessageBox.Show("Product not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        // change in database
                        if (isProductNameChanged)  selectedProduct.productName = products.productName; 
                        if (isDescriptionChanged) selectedProduct.description = products.description;
                        if (isCategoryChanged) selectedProduct.categoriesId = products.categoriesId; 
                        if (isArticleNumberChanged)   selectedProduct.articleNumber = products.articleNumber;
                        if (isQuantityChanged) selectedProduct.quantity = products.quantity;
                        if (isPurchasePriceChanged) selectedProduct.purchasePrice = products.purchasePrice;
                        if (isSellingPriceChanged) selectedProduct.sellingPrice = products.sellingPrice;
                        if (isMinimumStockChanged) selectedProduct.minimumStock = products.minimumStock; 
                        if (isImageChanged)  selectedProduct.img = products.img; 

                        context.SaveChanges();
                        // change in datagrid
                        var selectedRow = dataGrid.SelectedItem as productsDG;
                        if (selectedRow != null)
                        {
                            if (isProductNameChanged) selectedRow.productName = products.productName;
                            if (isDescriptionChanged) selectedRow.description = products.description;
                            if (isCategoryChanged)
                            {
                                selectedRow.categoriesId = products.categoriesId;
                                selectedRow.Category = products.Category;
                            }
                            if (isArticleNumberChanged) selectedRow.articleNumber = products.articleNumber;
                            if (isQuantityChanged) {
                                selectedRow.quantity = products.quantity;
                                // Recalculate isMinimumStock after possible changes
                                selectedRow.isMinimumStock = selectedRow.quantity <= selectedRow.minimumStock;
                            }
                            if (isPurchasePriceChanged) selectedRow.purchasePrice = products.purchasePrice;
                            if (isSellingPriceChanged) selectedRow.sellingPrice = products.sellingPrice;
                            if (isMinimumStockChanged)
                            {
                                selectedRow.minimumStock = products.minimumStock;
                                // Recalculate isMinimumStock after possible changes
                                selectedRow.isMinimumStock = selectedRow.quantity <= selectedRow.minimumStock;
                            }
                            if (isImageChanged) selectedRow.img = products.img;
                            if (dataGrid.Columns[staticVariable.columnOrderP.FindIndex(c => c == "category")] is DataGridComboBoxColumn comboColumn)
                            {
                                comboColumn.ItemsSource = categoryCB.ItemsSource;
                            }
                            dataGrid.Items.Refresh();
                        }
                        MessageBox.Show("Product updated successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private async void clearB_Click(object sender, RoutedEventArgs e)
        {
            // if in minimum stock mode
            if (Flags.minimumStockMode == true)
            {
                // no longer run MinimumStock mode
                Flags.minimumStockMode = false;
                minimumStockB.Foreground = new SolidColorBrush(Colors.White);
                // clear the datagrid, if searching than clear in searchchanged
                if (!Flags.isSearching)
                {
                    dpP!.dataGridClear();

                    await LoadMoreProductsAsync();
                }
            }

            searchTB.Clear();
            dpP!.allClear(true);
            if (dataGrid.SelectedItem != null)
                dataGrid.SelectedItem = null;
            // at clear the uploaded image is no longer necessary (for Compare image)
            Flags.isImageChanged = false;
        }
        private async void minimumStockB_Click(object sender, RoutedEventArgs e)
        {
            Flags.minimumStockMode = true;
            dpP!.allClear(true);
            dpP!.dataGridClear();

            await LoadMoreProductsAsync();

            minimumStockB.Foreground = new SolidColorBrush(Colors.Red);
        }
        //
        private void imgB_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            openFileDialog.Title = "Select an Image ";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string filePath = openFileDialog.FileName;
                    BitmapImage bitmap = new BitmapImage(new Uri(filePath));
                    img.Source = bitmap;
                    Flags.isImageChanged = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void categoryCB_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // add caytegory
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                // chceck if already exist
                string text = categoryCB.Text;
                bool exists = categoryCB.Items.Cast<Categories>().Any(c => c.categoryName!.ToLower() == text.ToLower());
                if (!exists)
                {
                    var result = MessageBox.Show("Do you want to insert a new category?", "Insert category", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result.HasFlag(MessageBoxResult.Yes))
                    {
                        // clear the textboxs
                        if (dataGrid.SelectedItem != null)
                        {
                            dpP!.allClear(true);
                            dataGrid.SelectedItem = null;
                        }

                        try
                        {
                            using (var context = new MyDbContext())
                            {
                                var newCategory = new Categories
                                {
                                    categoryName = text,
                                    userId = staticVariable.currentUser.userId,
                                };

                                context.categories.Add(newCategory);
                                context.SaveChanges();
                                MessageBox.Show("Category added successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            // Update the ComboBox with the new category
                            dpP!.setCategoriesC();
                            // update the COmboBoxColmun in datagrid
                            if (dataGrid.Columns[staticVariable.columnOrderP.FindIndex(c => c == "category")] is DataGridComboBoxColumn comboColumn)
                            {
                                comboColumn.ItemsSource = categoryCB.ItemsSource;
                                dataGrid.Items.Refresh();   
                            }
                            // select the new category
                            categoryCB.SelectedItem = categoryCB.Items.Cast<Categories>()
                                .FirstOrDefault(c => c.categoryName!.Equals(text, StringComparison.OrdinalIgnoreCase));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Category already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            // delete catogory
            else if (e.Key == System.Windows.Input.Key.Delete)
            {
                var result = MessageBox.Show("Would you like to delete the category?", "Delete category", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result.HasFlag(MessageBoxResult.Yes))
                {
                    // clear the textboxs
                    if (dataGrid.SelectedItem != null)
                    {
                        dpP!.allClear(false);
                        dataGrid.SelectedItem = null;
                    }
                    try
                    {
                        using (var context = new MyDbContext())
                        {
                            var selectedCategory = categoryCB.SelectedItem as Categories;
                            if (selectedCategory != null)
                            {
                                // Check if the category is used in any product
                                var isUsed = context.products.Any(p => p.Category!.categoriesId == selectedCategory.categoriesId);
                                if (!isUsed)
                                {
                                    var category = context.categories.SingleOrDefault(c => c.categoriesId == selectedCategory.categoriesId);
                                    if (category != null)
                                    {
                                        context.categories.Remove(category);
                                        context.SaveChanges();
                                        MessageBox.Show("Category deleted successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                                        dpP!.setCategoriesC();
                                        // clearn focus so that selectedindex of categoryCB works
                                        Keyboard.ClearFocus();
                                    }
                                    else
                                        MessageBox.Show("Category not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                                else
                                {
                                    MessageBox.Show("Cannot delete this category because it is used in a product.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    // clearn focus so that selectedindex of categoryCB works
                                    Keyboard.ClearFocus();
                                }
                            }
                            else
                            {
                                MessageBox.Show("No category selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private void minimumStockTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }
        private void sellingPriceTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (sender as TextBox)!;
            string newText = textBox!.Text.Insert(textBox.SelectionStart, e.Text);

            // Nur Punkt als Dezimaltrennzeichen akzeptieren (InvariantCulture)
            bool isValid = double.TryParse(newText, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
            e.Handled = !isValid;
        }
        private void purchasePriceTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (sender as TextBox)!;
            string newText = textBox!.Text.Insert(textBox.SelectionStart, e.Text);

            // Nur Punkt als Dezimaltrennzeichen akzeptieren (InvariantCulture)
            bool isValid = double.TryParse(newText, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
            e.Handled = !isValid;
        }
        private void quantityTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }
        private void articleNumberTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }
        private void articleNumberTB_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }
        private void quantityTB_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }
        private void purchasePriceTB_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }
        private void sellingPriceTB_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }
        private void minimumStockTB_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            System.Windows.Size currentSizeMW = new System.Windows.Size(mainWindow.ActualWidth, mainWindow.ActualHeight);
            mainWindow.resizeFP(currentSizeMW);
        }
        private void FrameProducts_Loaded(object sender, RoutedEventArgs e)
        {
            dpP ??= new dataProcessP();
            dpP!.firstProcess();
            dpP!.setDefaultImage();
            Page_SizeChanged(this, null!);
        }
    }
}
