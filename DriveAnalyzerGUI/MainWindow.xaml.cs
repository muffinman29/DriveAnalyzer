using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Windows.Controls.Primitives;
using System.Data;
using System.Collections;
using Microsoft.VisualBasic;

namespace DriveAnalyzerGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(tbFilePath.Text.Trim()))
            {
                tbFilePath.Text = dialog.SelectedPath;
            }
            else
            {
                tbFilePath.Text = "";
            }
        }

        private void btnScan_Click(object sender, RoutedEventArgs e)
        {
            BindGrid();
        }

        private void btnDeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            if (dgFiles.SelectedItems.Count > 0)
            {
                IList selectedItems = dgFiles.SelectedItems;

                foreach (KeyValuePair<string, long> item in selectedItems)
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(item.Key, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                }

                BindGrid();
            }
        }

        private async void BindGrid()
        {
            if (!string.IsNullOrWhiteSpace(tbFilePath.Text))
            {
                dgFiles.ItemsSource = null;
                dgFiles.Items.Refresh();
                prgProgress.Visibility = Visibility.Visible;
                sbiProgress.Content = "Scanning directories. This may take a while.";
                DriveAnalyzerLibrary.DriveAnalyzer da = new DriveAnalyzerLibrary.DriveAnalyzer();
                var filePath = new System.IO.DirectoryInfo(tbFilePath.Text);
                await Task.Factory.StartNew(() => da.EnumerateSubDirectories(filePath));
                DataContext = da;
                dgFiles.ItemsSource = da.AllFiles;
                dgFiles.Items.Refresh();
                sbiProgress.Content = String.Format("Scan complete. Found {0:N0} files.", da.GetNumberOfFiles());
                System.Media.SystemSounds.Exclamation.Play();
                prgProgress.Visibility = Visibility.Hidden;
            }
        }

        private void OpenFileItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = (System.Windows.Controls.MenuItem)sender;

            var contextMenu = (System.Windows.Controls.ContextMenu)menuItem.Parent;

            var item = (System.Windows.Controls.DataGrid)contextMenu.PlacementTarget;

            var selectedRow = (KeyValuePair<string, long>)item.SelectedCells[0].Item;
            if (System.IO.File.Exists(selectedRow.Key))
            {
                System.Diagnostics.Process.Start(selectedRow.Key);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("The file has been moved or deleted.", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }            
        }

        private void OpenContainingFolderItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = (System.Windows.Controls.MenuItem)sender;

            var contextMenu = (System.Windows.Controls.ContextMenu)menuItem.Parent;

            var item = (System.Windows.Controls.DataGrid)contextMenu.PlacementTarget;

            var selectedRow = (KeyValuePair<string, long>)item.SelectedCells[0].Item;
            if (System.IO.File.Exists(selectedRow.Key))
            {
                System.Diagnostics.Process.Start("explorer.exe", "/select, " + selectedRow.Key);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("The file has been moved or deleted.", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void DeleteFileItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = (System.Windows.Controls.MenuItem)sender;

            var contextMenu = (System.Windows.Controls.ContextMenu)menuItem.Parent;

            var item = (System.Windows.Controls.DataGrid)contextMenu.PlacementTarget;            

            var selectedItems = item.SelectedItems;

            foreach (KeyValuePair<string, long> selected in selectedItems)
            {
                if (System.IO.File.Exists(selected.Key))
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(selected.Key, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);                    
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("The file has been moved or deleted.", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }
            }

            BindGrid();
        }
    }
}
