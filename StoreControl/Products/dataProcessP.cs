using StoreControl.Database;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics.Eventing.Reader;

namespace StoreControl.ProductsF
{
    internal class dataProcessP
    {
        
        public dataProcessP()
        {
            // 
            getAllPath();
        }
    
        public void firstProcess()
        {
            if (staticVariable.staticFP!.dataGrid.Columns.Count == 0)
            {
                try
                {
                    using (var context = new MyDbContext())
                    {
                        List<Translation> translations = context.translation
                             .Where(t => staticVariable.keywordP.Contains(t.Key_word))
                                    .ToList();
                        //
                        List<(string, string)> translateds = setTranslation(translations);

                        // add Columns
                        staticVariable.staticFP.dataGrid.Columns.Clear();
                        staticVariable.staticFP.dataGrid.Items.Clear();

                        // insert Categories
                        setCategoriesC();
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
        public List<(string, string)> setTranslation(List<Translation> translations)
        {
            if(staticVariable.staticFP == null)
                return null!;

            List<(string, string)> translateds = translations
             .Select(translation =>
               staticVariable.lang == "en"
               ? (translation.Key_word, translation.en)
               : (translation.Key_word, translation.de))
               .ToList();

            foreach (var translated in translateds)
            {
                if (translated.Item1 == "productsId")
                    staticVariable.staticFP.productId.Content = translated.Item2;
                else if (translated.Item1 == "productName")
                    staticVariable.staticFP.productName.Content = translated.Item2;
                else if (translated.Item1 == "category")
                    staticVariable.staticFP.category.Content = translated.Item2;
                else if (translated.Item1 == "description")
                    staticVariable.staticFP.description.Content = translated.Item2;
                else if (translated.Item1 == "articleNumber")
                    staticVariable.staticFP.articleNumber.Content = translated.Item2;
                else if (translated.Item1 == "purchasePrice")
                    staticVariable.staticFP.purchasePrice.Content = translated.Item2;
                else if (translated.Item1 == "sellingPrice")
                    staticVariable.staticFP.sellingPrice.Content = translated.Item2;
                else if (translated.Item1 == "quantity")
                    staticVariable.staticFP.quantity.Content = translated.Item2;
                else if (translated.Item1 == "minimumStock")
                {
                    staticVariable.staticFP.minimumStock.Content = translated.Item2;
                    staticVariable.staticFP.minimumStockB.Content = translated.Item2;
                }
                else if (translated.Item1 == "create")
                    staticVariable.staticFP.newB.Content = translated.Item2;
                else if (translated.Item1 == "edit")
                    staticVariable.staticFP.editB.Content = translated.Item2;
                else if (translated.Item1 == "delete")
                    staticVariable.staticFP.deleteB.Content = translated.Item2;
                else if (translated.Item1 == "clear")
                    staticVariable.staticFP.clearB.Content = translated.Item2;
                else if (translated.Item1 == "uploadImg")
                    staticVariable.staticFP.imgB.Content = translated.Item2;
                else if (translated.Item1 == "searchTB")
                    textboxBackground(translated.Item2);
                else if (translated.Item1 == "noData")
                    staticVariable.staticFP.NoDataText.Text = translated.Item2;
                
                // datagrid column
                int countDG = staticVariable.staticFP.dataGrid.Columns.Count;
                if (countDG > 0)
                {
                    for (int i = 0; i < countDG; i++)
                    {
                        if (translated.Item1 == staticVariable.staticFP.dataGrid.Columns[i].SortMemberPath)
                        {
                            staticVariable.staticFP.dataGrid.Columns[i].Header = translated.Item2;
                            break;
                        }
                    }
                }
            }

            return translateds;
        }
        private void getAllPath()
        {
            staticVariable.folderPath = AppDomain.CurrentDomain.BaseDirectory;
        }
        public void setCategoriesC()
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    List<Categories> categories = context.categories
                        .Where(c => c.userId == staticVariable.currentUser.userId)
                        .ToList();
                    List<Categories> categoriesList = new List<Categories>();
                    foreach (var category in categories)
                    {
                        Categories categories1 = new Categories();
                        categories1.categoriesId = category.categoriesId;
                        categories1.categoryName = category.categoryName;
                        categories1.userId = category.userId;
                        categoriesList.Add(categories1);
                    }
                    staticVariable.staticFP!.categoryCB.ItemsSource = null;
                    staticVariable.staticFP!.categoryCB.ItemsSource = categoriesList;
                    staticVariable.staticFP!.categoryCB.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public async Task<List<Products>> LoadMoreProducts()
        {
            if (Flags.allProductsLoadedDB) return new List<Products>();

            List<Products> products1 = new List<Products>();
            try
            {
                using (var context = new MyDbContext())
                {
                    var query = context.products
                     .Include(p => p.Category)
                     .AsQueryable();

                    query = query.Where(p => p.userId == staticVariable.currentUser.userId && (Flags.minimumStockMode == true ? p.quantity <= p.minimumStock : true));

                    var products = await query
                        .OrderByDescending(products => products.productsId)
                        .Skip(staticVariable.currentPageDG * staticVariable.pageSizeDG)
                        .Take(staticVariable.pageSizeDG)
                        .ToListAsync();

                    // cecck if all products loaded
                    if (products.Count == 0)
                    {
                        Flags.allProductsLoadedDB = true;
                        return products!;
                    }
                    // 👇 Make the category list only ONCE
                    if (staticVariable.staticFP!.dataGrid.Items.Count == 0)
                    {
                        var usedCategoriesDB = products
                            .Select(p => p.Category)
                            .Where(c => c!.userId == staticVariable.currentUser.userId && c != null)
                            .Distinct()
                            .ToList();

                        // we use the dispatcher to switch to the main UI thread
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            // Set ItemsSource für ComboBox
                            if (staticVariable.staticFP.dataGrid.Columns[staticVariable.columnOrderP.FindIndex(c => c == "category")] is DataGridComboBoxColumn comboColumn)
                            {
                                comboColumn.ItemsSource = usedCategoriesDB;
                            }
                        });
                    }
                    products1 = products;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null!;
            }
            finally
            {
                staticVariable.currentPageDG++;
            }
            return products1;
        }
        // datagrid
        public void checkDataDG(DataGrid dataGrid)
        {
            if (dataGrid.Items.Count == 0)
            {
                staticVariable.staticFP!.dataGrid.Visibility = Visibility.Collapsed;
                staticVariable.staticFP!.NoDataText.Visibility = Visibility.Visible;
            }
            else
            {
                staticVariable.staticFP!.dataGrid.Visibility = Visibility.Visible;
                staticVariable.staticFP!.NoDataText.Visibility = Visibility.Collapsed;
            }
        }
        public void addItemsDG(List<Products> products, boolInt ifInsert)
        {
            int existingCount = staticVariable.staticFP!.dataGrid.Items.Count;
            int rowIndex = 0;
            for (int i = 0; i < products.Count; i++)
            {
                rowIndex = existingCount + i + 1;

                productsDG newProduct = new productsDG()
                {
                    rowIndex = rowIndex,
                    productsId = products[i].productsId,
                    productName = products[i].productName,
                    description = products[i].description,
                    categoriesId = products[i].categoriesId,
                    Category = products[i].Category,
                    articleNumber = products[i].articleNumber,
                    quantity = products[i].quantity,
                    purchasePrice = products[i].purchasePrice,
                    sellingPrice = products[i].sellingPrice,
                    minimumStock = products[i].minimumStock,
                    // binding for DataTrigger in datagrid as value 
                    isMinimumStock = products[i].quantity <= products[i].minimumStock,
                    img = products[i].img,
                    userId = products[i].userId,
                    User = staticVariable.currentUser,
                    
                };

                if (!ifInsert.bo)
                    staticVariable.staticFP!.dataGrid.Items.Add(newProduct);
                else
                    staticVariable.staticFP!.dataGrid.Items.Insert(ifInsert.Int, newProduct);
            }
        }
        public void addColumnsDG(List<(string, string)> translateds)
        {
            // dataGrid sorting 
            List<(string, string)> sortedColmuns = translateds
                .Where(x => staticVariable.columnOrderP.Any(co => co == x.Item1))
                .OrderBy(x => staticVariable.columnOrderP.FindIndex(co => co == x.Item1))
                .ToList();

            // add Columns
            {
                double rowHeight = 100;
                //
                foreach ((string, string) st in sortedColmuns)
                {
                    if (st.Item1 == "img")
                    {
                        DataGridTemplateColumn imageColumn = new DataGridTemplateColumn
                        {
                            Header = st.Item2,
                            SortMemberPath = st.Item1,
                        };

                        DataTemplate dataTemplate = new DataTemplate();

                        FrameworkElementFactory imageFactory = new FrameworkElementFactory(typeof(Image));

                        imageFactory.SetBinding(Image.SourceProperty, new Binding(st.Item1));

                        dataTemplate.VisualTree = imageFactory;

                        imageColumn.CellTemplate = dataTemplate;

                        staticVariable.staticFP!.dataGrid.Columns.Add(imageColumn);
                    }
                    else if (st.Item1 == "category")
                    {
                        var comboBoxColumn = new DataGridComboBoxColumn
                        {
                            Header = st.Item2,
                            SelectedValueBinding = new Binding("Category.categoriesId"),
                            SelectedValuePath = "categoriesId",
                            DisplayMemberPath = "categoryName",
                            ItemsSource = staticVariable.staticFP!.categoryCB.ItemsSource
                        };


                        staticVariable.staticFP!.dataGrid.Columns.Add(comboBoxColumn);
                    }
                    else if (st.Item1 == "quantity")
                    {
                        DataGridTextColumn quantityColumn = new DataGridTextColumn
                        {
                            Header = st.Item2,
                            Binding = new Binding(st.Item1),
                        };

                        // Set ElementStyle for DataTrigger
                        quantityColumn.ElementStyle = new Style(typeof(TextBlock))
                        {
                            Triggers =
                           {
                        new DataTrigger
                        {
                            Binding = new Binding("isMinimumStock"),
                            Value = true,
                            Setters =
                            {
                                new Setter { Property = TextBlock.BackgroundProperty, Value = Brushes.Red },
                                new Setter { Property = TextBlock.ForegroundProperty, Value = Brushes.Black }
                            }
                        }
                         }
                        };

                        staticVariable.staticFP!.dataGrid.Columns.Add(quantityColumn);
                    }
                    else
                    {
                        staticVariable.staticFP!.dataGrid.Columns.Add(new DataGridTextColumn() { Header = st.Item2, Binding = new Binding(st.Item1)});
                    }
                }
                // datagrid properties
                staticVariable.staticFP!.dataGrid.RowHeight = rowHeight;
                // columnWidth is defined in function resizeFP in mainWindows
            }
        }
        // search
        public async Task<List<Products>> Search(string keyWord, bool exactSearch)
        {
            List<Products> products = new List<Products>();
            using (var context = new MyDbContext())
            {
                try
                {
                    var lowerKeyword = keyWord.ToLower();
                    var query = context.products
                     .Include(p => p.Category)
                     .AsQueryable();

                    if (exactSearch)
                    {
                        query = query.Where(p => p.userId == staticVariable.currentUser.userId && p.productsId.ToString().Equals(lowerKeyword));
                    }
                    else
                    {

                        query = query.Where(p =>
                         p.userId == staticVariable.currentUser.userId && (
                         (p.productsId.ToString().Contains(lowerKeyword) ||
                         p.productName!.ToLower().Contains(lowerKeyword) ||
                         p.Category!.categoryName!.ToLower().Contains(lowerKeyword)) &&
                         (Flags.minimumStockMode == true ? p.quantity < p.minimumStock : true)
                         ));
                    }
                        products = await query
                        .OrderByDescending(p => p.productsId)
                        .Skip(staticVariable.currentPageDG * staticVariable.pageSizeDG)
                        .Take(staticVariable.pageSizeDG)
                        .ToListAsync();

                    // 👇 Nur EINMAL die Kategorienliste machen
                    if (staticVariable.staticFP!.dataGrid.Items.Count == 0)
                    {
                        var usedCategoriesDB = products
                            .Select(p => p.Category)
                             .Where(c => c!.userId == staticVariable.currentUser.userId && c != null)
                            .Distinct()
                            .ToList();

                        // Set ItemsSource für ComboBox
                        if (staticVariable.staticFP.dataGrid.Columns[staticVariable.columnOrderP.FindIndex(c => c == "category")] is DataGridComboBoxColumn comboColumn)
                        {
                            comboColumn.ItemsSource = usedCategoriesDB;
                        }
                    }
                    return products;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null!;
                }
                finally
                {
                   
                    staticVariable.currentPageDG++;
                }

            }
        }
        // image
        public BitmapImage? ConvertByteToImage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
                return null;

            BitmapImage bitmap = new BitmapImage();
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                bitmap.Freeze();
            }
            return bitmap;
        }
        public byte[]? ConvertImageToByte(BitmapImage image)
        {
            if (image == null)
                return null;
            // Check if the image size exceeds the Mediumblob limit
            //int imageSize = GetBitmapImageSizeInBytes(image);

            return ResizeBitmapImage(image, staticVariable.maxWidth, staticVariable.maxHeight);

        }
        public byte[] ResizeBitmapImage(BitmapImage original, int maxWidth, int maxHeight)
        {
            double scale = Math.Min((double)maxWidth / original.PixelWidth, (double)maxHeight / original.PixelHeight);
            var scaledBitmap = new TransformedBitmap(original, new ScaleTransform(scale, scale));

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.QualityLevel = 85;
            encoder.Frames.Add(BitmapFrame.Create(scaledBitmap));

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                return ms.ToArray();
            }
        }
        public void setDefaultImage()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "defaultImage.jpg");
            staticVariable.staticFP!.img.Source = new BitmapImage(new Uri(path));
        }
        public bool imageCompare()
        {
            bool isImageChanged = Flags.isImageChanged == true;
            // (for Compare image)
            Flags.isImageChanged = false;

            return isImageChanged;
        }
        // clear
        public void allClear(bool withCategory, bool withSearchTB)
        {
            foreach (TextBox textBox in staticVariable.staticFP!.gridFP.Children.OfType<TextBox>())
            {
                if (textBox.Name == "searchTB")
                {
                    if (withSearchTB)
                        textBox.Clear();
                    continue;
                }

                textBox.Clear();
            }

            if (withCategory)
                staticVariable.staticFP.categoryCB.SelectedItem = null;
            setDefaultImage();
            Flags.isImageChanged = false;
            staticVariable.currentProduct = new productsDG();
            Flags.isSearching = false;
        }
        public void dataGridClear()
        {
            staticVariable.currentPageDG = 0;
            Flags.allProductsLoadedDB = false;
            Flags.isLoadingDB = false;
            staticVariable.staticFP!.dataGrid.Items.Clear();
        }
        public Products? getProduct(bool withCategory, bool  withUser)
        {
            if (!checkTextbox() || staticVariable.staticFP == null)
                return null;
            Products products = new Products()
            {
                productName = staticVariable.staticFP.productNameTB.Text,
                description = staticVariable.staticFP.descriptionTB.Text,
                categoriesId = (staticVariable.staticFP.categoryCB.SelectedItem as Categories)!.categoriesId,
                articleNumber = int.Parse(staticVariable.staticFP.articleNumberTB.Text),
                quantity = int.Parse(staticVariable.staticFP.quantityTB.Text),
                purchasePrice = double.Parse(staticVariable.staticFP.purchasePriceTB.Text),
                sellingPrice = double.Parse(staticVariable.staticFP.sellingPriceTB.Text),
                minimumStock = int.Parse(staticVariable.staticFP.minimumStockTB.Text),
                img = ConvertImageToByte((BitmapImage)staticVariable.staticFP.img.Source)!,
                userId = staticVariable.currentUser.userId,
            };

            if (withCategory) products.Category = (staticVariable.staticFP.categoryCB.SelectedItem as Categories)!;
            if (withUser) products.User = staticVariable.currentUser;

            return products;
        }
        public bool checkTextbox()
        {
            if (string.IsNullOrWhiteSpace(staticVariable.staticFP!.productNameTB.Text) || string.IsNullOrWhiteSpace(staticVariable.staticFP.descriptionTB.Text) ||
               string.IsNullOrWhiteSpace(staticVariable.staticFP.quantityTB.Text) || staticVariable.staticFP.categoryCB.SelectedItem == null ||
              string.IsNullOrWhiteSpace(staticVariable.staticFP.purchasePriceTB.Text) || string.IsNullOrWhiteSpace(staticVariable.staticFP.sellingPriceTB.Text))
            {
                return false;
            }
            else
                return true;
        }
        public void textboxBackground(string searchTB)
        {
            TextBlock watermarkTextBlock = (TextBlock)staticVariable.staticFP!.FindResource("WatermarkTextBlock");
            watermarkTextBlock.Text = searchTB;
        }
    }
}
