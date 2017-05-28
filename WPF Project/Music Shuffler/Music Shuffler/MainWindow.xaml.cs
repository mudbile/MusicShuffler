using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Music_Shuffler {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        Playlist playlist;
        List<String> musicFileExtensions = new List<string>() { ".mp3", ".wav" };
        public MainWindow() {
            InitializeComponent();
        }


        public void btnChooseRootClicked(object sender, RoutedEventArgs e) {
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog()) {
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() != CommonFileDialogResult.Ok) {
                    return;
                }
                txtRootFolder.Text = dialog.FileName;
            }
        }

            
        public void btnGetAlbumsClicked(object sender, RoutedEventArgs eevent) {
            String rootFolder = txtRootFolder.Text;
            if (!Directory.Exists(rootFolder)) {
                MessageBox.Show("Folder does not exist");
                return;
            }
            playlist = new Playlist(rootFolder, musicFileExtensions);
        }
    }
}
