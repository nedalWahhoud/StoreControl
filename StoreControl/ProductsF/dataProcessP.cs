using StoreControl.Database;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StoreControl.ProductsF
{
    internal class dataProcessP
    {
        
        private frameProducts frameProducts;
        public dataProcessP(frameProducts frameProducts)
        {
            this.frameProducts = frameProducts;
            setDefaultImage();

            // 
            getAllPath();

        }
        public void setDefaultImage()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "defaultImage.jpg");
            frameProducts.img.Source = new BitmapImage(new Uri(path));
        }
        public void mainProcess()
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
                    frameProducts.dataGrid.Columns.Clear();
                    frameProducts.dataGrid.Items.Clear();

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
        public List<(string, string)> setTranslation(List<Translation> translations)
        {
            List<(string, string)> translateds = translations
             .Select(translation =>
               staticVariable.lang == "en"
               ? (translation.Key_word, translation.en)
               : (translation.Key_word, translation.de))
               .ToList();

            foreach (var translated in translateds)
            {
                if (translated.Item1 == "productsId")
                    frameProducts.productId.Content = translated.Item2;
                else if (translated.Item1 == "productName")
                    frameProducts.productName.Content = translated.Item2;
                else if (translated.Item1 == "category")
                    frameProducts.category.Content = translated.Item2;
                else if (translated.Item1 == "description")
                    frameProducts.description.Content = translated.Item2;
                else if (translated.Item1 == "articleNumber")
                    frameProducts.articleNumber.Content = translated.Item2;
                else if (translated.Item1 == "purchasePrice")
                    frameProducts.purchasePrice.Content = translated.Item2;
                else if (translated.Item1 == "sellingPrice")
                    frameProducts.sellingPrice.Content = translated.Item2;
                else if (translated.Item1 == "quantity")
                    frameProducts.quantity.Content = translated.Item2;
                else if (translated.Item1 == "minimumStock")
                {
                    frameProducts.minimumStock.Content = translated.Item2;
                    frameProducts.minimumStockB.Content = translated.Item2;
                }
                else if (translated.Item1 == "create")
                    frameProducts.newB.Content = translated.Item2;
                else if (translated.Item1 == "edit")
                    frameProducts.editB.Content = translated.Item2;
                else if (translated.Item1 == "delete")
                    frameProducts.deleteB.Content = translated.Item2;
                else if (translated.Item1 == "clear")
                    frameProducts.clearB.Content = translated.Item2;
                else if (translated.Item1 == "uploadImg")
                    frameProducts.imgB.Content = translated.Item2;
                else if (translated.Item1 == "searchTB")
                    textboxBackground(translated.Item2);
                else if (translated.Item1 == "noData")
                    frameProducts.NoDataText.Text = translated.Item2;
                
                // datagrid column
                int countDG = frameProducts.dataGrid.Columns.Count;
                if (countDG > 0)
                {
                    for (int i = 0; i < countDG; i++)
                    {
                        if (translated.Item1 == frameProducts.dataGrid.Columns[i].SortMemberPath)
                        {
                            frameProducts.dataGrid.Columns[i].Header = translated.Item2;
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
                    frameProducts.categoryCB.ItemsSource = null;
                    frameProducts.categoryCB.ItemsSource = categoriesList;
                    frameProducts.categoryCB.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public async Task<List<Products>> LoadMoreProducts()
        {
            if (Flags.allProductsLoadedDB) return null!;
            

            List<Products> products1 = new List<Products>();
            using (var context = new MyDbContext())
            {
                try
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
                        return null!;
                    }
                    // 👇 Make the category list only ONCE
                    if (frameProducts.dataGrid.Items.Count == 0)
                    {
                        var usedCategoriesDB = products
                            .Select(p => p.Category)
                            .Where(c => c.userId == staticVariable.currentUser.userId && c != null)
                            .Distinct()
                            .ToList();

                        // we use the dispatcher to switch to the main UI thread
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            // Set ItemsSource für ComboBox
                            if (frameProducts.dataGrid.Columns[staticVariable.columnOrder.FindIndex(c => c == "category")] is DataGridComboBoxColumn comboColumn)
                            {
                                comboColumn.ItemsSource = usedCategoriesDB;
                            }
                        });
                    }
                    products1 = products;
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
            return products1;
        }
        public void checkDataDG(DataGrid dataGrid)
        {
            if (dataGrid.Items.Count == 0)
            {
                frameProducts.dataGrid.Visibility = Visibility.Collapsed;
                frameProducts.NoDataText.Visibility = Visibility.Visible;
            }
            else
            {
                frameProducts.dataGrid.Visibility = Visibility.Visible;
                frameProducts.NoDataText.Visibility = Visibility.Collapsed;
            }
        }

        public void addItemsDG(List<Products> products, boolInt ifInsert)
        {
            int existingCount = frameProducts.dataGrid.Items.Count;
            int rowIndex = 0;
            for (int i = 0; i < products.Count; i++)
            {
                rowIndex = existingCount + i + 1;


                if (products[i].productsId == 91)
                {

                }
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
                    isMinimumStock = products[i].quantity <= products[i].minimumStock,
                    img = products[i].img,
                    userId = products[i].userId,
                    User = staticVariable.currentUser,
                    
                };

                if (!ifInsert.bo)
                    frameProducts.dataGrid.Items.Add(newProduct);
                else
                    frameProducts.dataGrid.Items.Insert(ifInsert.Int, newProduct);
            }
        }
        public void addColumnsDG(List<(string, string)> translateds)
        {
            // dataGrid sorting 
            List<(string, string)> sortedTranslateds = translateds
                .Where(x => staticVariable.columnOrder.Any(co => co == x.Item1))
                .OrderBy(x => staticVariable.columnOrder.FindIndex(co => co == x.Item1))
                .ToList();

            // add Columns
            {
                double totalWidth = 0;
                double columnWidth = 149;
                double columnWidthC = 50;
                double rowHeight = 100;
                
                //
                foreach ((string, string) st in sortedTranslateds)
                {
                    if(st.Item1 == "rowIndex")
                        columnWidth = columnWidthC;

                    if (st.Item1 == "img")
                    {
                        DataGridTemplateColumn imageColumn = new DataGridTemplateColumn
                        {
                            Header = st.Item2,
                            Width = columnWidth,
                            SortMemberPath = st.Item1,
                        };

                        DataTemplate dataTemplate = new DataTemplate();

                        FrameworkElementFactory imageFactory = new FrameworkElementFactory(typeof(Image));

                        imageFactory.SetBinding(Image.SourceProperty, new Binding(st.Item1));

                        dataTemplate.VisualTree = imageFactory;

                        imageColumn.CellTemplate = dataTemplate;

                        frameProducts.dataGrid.Columns.Add(imageColumn);
                    }
                    else if (st.Item1 == "category")
                    {
                        var comboBoxColumn = new DataGridComboBoxColumn
                        {
                            Header = st.Item2,
                            Width = columnWidth,
                            SelectedValueBinding = new Binding("Category.categoriesId"),
                            SelectedValuePath = "categoriesId",
                            DisplayMemberPath = "categoryName",
                            ItemsSource = frameProducts.categoryCB.ItemsSource
                        };
                        

                        frameProducts.dataGrid.Columns.Add(comboBoxColumn);
                    }
                    else if (st.Item1 == "quantity")
                    {
                        DataGridTextColumn quantityColumn = new DataGridTextColumn
                        {
                            Header = st.Item2,
                            Binding = new Binding(st.Item1),
                            Width = columnWidth
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

                        frameProducts.dataGrid.Columns.Add(quantityColumn);
                    }
                    else
                    {
                        frameProducts.dataGrid.Columns.Add(new DataGridTextColumn() { Header = st.Item2, Binding = new Binding(st.Item1), Width = columnWidth });
                    }
                    totalWidth += columnWidth + 8;
                }
                // datagrid properties
                frameProducts.dataGrid.RowHeight = rowHeight;
                frameProducts.dataGrid.Width = totalWidth;
            }
        }
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
                    if (frameProducts.dataGrid.Items.Count == 0)
                    {
                        var usedCategoriesDB = products
                            .Select(p => p.Category)
                             .Where(c => c.userId == staticVariable.currentUser.userId && c != null)
                            .Distinct()
                            .ToList();

                        // Set ItemsSource für ComboBox
                        if (frameProducts.dataGrid.Columns[staticVariable.columnOrder.FindIndex(c => c == "category")] is DataGridComboBoxColumn comboColumn)
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
        public bool imageCompare()
        {
            bool isImageChanged = Flags.isImageChanged == true;
            // (for Compare image)
            Flags.isImageChanged = false;

            return isImageChanged;
        }
        public void allClear(bool withCategory)
        {
            frameProducts.productIdTB.Clear();
            frameProducts.productNameTB.Clear();
            frameProducts.descriptionTB.Clear();
            if (withCategory)
                frameProducts.categoryCB.SelectedItem = null;
            frameProducts.articleNumberTB.Clear();
            frameProducts.quantityTB.Clear();
            frameProducts.purchasePriceTB.Clear();
            frameProducts.sellingPriceTB.Clear();
            frameProducts.minimumStockTB.Clear();

            setDefaultImage();
            Flags.isImageChanged = false;
            staticVariable.currentProduct = new Products();
            Flags.isSearching = false;
        }
        public void dataGridClear()
        {
            staticVariable.currentPageDG = 0;
            Flags.allProductsLoadedDB = false;
            Flags.isLoadingDB = false;
            frameProducts.dataGrid.Items.Clear();
        }
        public Products? getProduct(bool withCategory, bool  withUser)
        {
            if (!checkTextbox())
                return null;
            Products products = new Products()
            {
                productName = frameProducts.productNameTB.Text,
                description = frameProducts.descriptionTB.Text,
                categoriesId = (frameProducts.categoryCB.SelectedItem as Categories)!.categoriesId,
                articleNumber = int.Parse(frameProducts.articleNumberTB.Text),
                quantity = int.Parse(frameProducts.quantityTB.Text),
                purchasePrice = double.Parse(frameProducts.purchasePriceTB.Text),
                sellingPrice = double.Parse(frameProducts.sellingPriceTB.Text),
                minimumStock = int.Parse(frameProducts.minimumStockTB.Text),
                img = ConvertImageToByte((BitmapImage)frameProducts.img.Source)!,
                userId = staticVariable.currentUser.userId,
            };

            if (withCategory) products.Category = (frameProducts.categoryCB.SelectedItem as Categories)!;
            if (withUser) products.User = staticVariable.currentUser;

            return products;
        }
        public bool checkTextbox()
        {
            if (string.IsNullOrWhiteSpace(frameProducts.productNameTB.Text) || string.IsNullOrWhiteSpace(frameProducts.descriptionTB.Text) ||
               string.IsNullOrWhiteSpace(frameProducts.quantityTB.Text) || frameProducts.categoryCB.SelectedItem == null ||
              string.IsNullOrWhiteSpace(frameProducts.purchasePriceTB.Text) || string.IsNullOrWhiteSpace(frameProducts.sellingPriceTB.Text))
            {
                return false;
            }
            else
                return true;
        }
        public void textboxBackground(string searchTB)
        {
            TextBlock watermarkTextBlock = (TextBlock)frameProducts.FindResource("WatermarkTextBlock");
            watermarkTextBlock.Text = searchTB;
        }
    }
}
