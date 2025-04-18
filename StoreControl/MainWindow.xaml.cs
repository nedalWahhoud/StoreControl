﻿using StoreControl.LogIn;
using System.Windows;

namespace StoreControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();

            frameList.Content = new frameList();
            frameLogIn.Content = new frameLogIn();
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            System.Windows.Size currentSizeMW = new System.Windows.Size(this.ActualWidth, this.ActualHeight);
            resizeFP(currentSizeMW);
            //
        }
        public void resizeFP(System.Windows.Size s)
        {
            if (frameProducts.Content != null && !Flags.isResizeFP)
            {
                Flags.isResizeFP = true;

                double mainDefaultWidth = 1770;
                var fp = (frameProducts)frameProducts.Content;

                // get mainWindows size
                double sizeMwWidth = s.Width;
                // get frameList size
                double sizeFlWidth = this.frameList.ActualWidth;
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

            

            if (frameList.Content != null)
            {
                var fl = (frameList)frameList.Content;
                // exit button
                fl.exitB.Margin = new Thickness(21, s.Height - 100, 0, 0);
            }

            Flags.isResizeFP = false;
        }
    }
}