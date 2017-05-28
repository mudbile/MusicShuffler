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


        /// <summary>
        /// Actually copies the music files
        /// </summary>
        public void btnMakePlaylistClicked(object sender, RoutedEventArgs ev) {
            //Make sure the directory is all G
            String outputFolder = txtOutputFolder.Text;
            if (outputFolder == "") {
                MessageBox.Show("Output Folder required", "Unable to make playlist");
                return;
            }
            if (!Directory.Exists(outputFolder)) {
                Directory.CreateDirectory(outputFolder);
            } else {
                if (Directory.GetFiles(outputFolder).Any(file => !musicFileExtensions.Contains(Path.GetExtension(file)))) {
                    MessageBox.Show("I will only clobber a folder if there are nothing but music files inside it.", "Unable to make playlist");
                    return;
                } else {
                    Directory.Delete(outputFolder, recursive: true);
                    Directory.CreateDirectory(outputFolder);
                }
            }

            //Weed out the albums that aren't ticked to be included and randomise/sort the rest
            List<Album> albumsToInclude = new List<Album>();
            foreach (ListBoxItem albumItem in lstbxAlbums.Items) {
                Album album = albumItem.Tag as Album;
                if ((bool)(((Grid)albumItem.Content).Children[0] as CheckBox).IsChecked) {
                    Console.WriteLine("Added: " + album.albumRoot);
                    albumsToInclude.Add(album);
                    album.randomiseSongs = (bool)(((Grid)albumItem.Content).Children[1] as CheckBox).IsChecked;
                    Console.WriteLine(album.albumRoot + " shuffled");
                    if (album.randomiseSongs) {
                        album.shuffle();
                    } else {
                        album.sort();
                    }
                }
                
            }
            
            playlist.generatePlaylist(albumsToInclude, outputFolder);
        }


        /// <summary>
        /// Opens folder dialogue to assist in choosing an output folder
        /// </summary>
        public void btnChooseOutputClicked(object sender, RoutedEventArgs ev) {
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog()) {
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() != CommonFileDialogResult.Ok) {
                    txtOutputFolder.Text = "";
                }
                txtOutputFolder.Text = dialog.FileName;
            }
        }

        /***********************************************************************************************/
        /*       Select All / Select None         and         Shuffle All / Shuffle None               */
        /***********************************************************************************************/
        public void btnSelectNoneClicked(object sender, RoutedEventArgs ev) {
            setAllAlbumsSelectedTo(false);
        }
        public void btnSelectAllClicked(object sender, RoutedEventArgs ev) {
            setAllAlbumsSelectedTo(true);
        }
        private void setAllAlbumsSelectedTo(bool isSelected) {
            foreach (ListBoxItem albumItem in lstbxAlbums.Items) {
                (((Grid)albumItem.Content).Children[0] as CheckBox).IsChecked = isSelected;
            }
        }

        public void btnShuffleNoneClicked(object sender, RoutedEventArgs ev) {
            setAllAlbumsShuffledTo(false);
        }
        public void btnShuffleAllClicked(object sender, RoutedEventArgs ev) {
            setAllAlbumsShuffledTo(true);
        }
        private void setAllAlbumsShuffledTo(bool isShuffled) {
            foreach (ListBoxItem albumItem in lstbxAlbums.Items) {
                (((Grid)albumItem.Content).Children[1] as CheckBox).IsChecked = isShuffled;
            }
        }
        /***********************************************************************************************/
        /***********************************************************************************************/


        /// <summary>
        /// Retrieves albums from root folder and calls populateGUIAlbums to populate gui
        /// </summary>
        public void btnGetAlbumsClicked(object sender, RoutedEventArgs ev) {
            List<String> roots = new List<string>();

            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog()) {
                dialog.IsFolderPicker = true;
                dialog.Multiselect = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok) {
                    roots = dialog.FileNames.ToList();
                }
            }
            if (roots.Count == 0) {
                return;
            }

            //generates albums
            playlist = new Playlist(roots, musicFileExtensions);

            this.clearGUIAlbums();
            this.populateGUIAlbums();


        }

        public void clearGUIAlbums() {
            lstbxAlbums.Items.Clear();
        }


        /// <summary>
        /// creates a GUI element for each album and populates 
        /// the list box lstbxAlbums with them. 
        /// Also adds the album object as a tag to the listboxitem
        /// </summary>
        public void populateGUIAlbums() {
            foreach (Album album in playlist.albums) {
                //Make checkboxes
                CheckBox includeAlbum = new CheckBox();
                includeAlbum.Content = Path.GetFileName(album.albumRoot);
                includeAlbum.ToolTip = album.albumRoot;
                CheckBox shuffleAlbum = new CheckBox();
                shuffleAlbum.Content = "Shuffle";
                //shuffle checkbox will not be enabled when the album is not being included
                Binding shuffletoIncludeAlbumBinding = new Binding();
                shuffletoIncludeAlbumBinding.Source = includeAlbum;
                shuffletoIncludeAlbumBinding.Path = new PropertyPath("IsChecked");
                BindingOperations.SetBinding(shuffleAlbum, CheckBox.IsEnabledProperty, shuffletoIncludeAlbumBinding);

                //Make grid to hold checkboxes
                Grid albumGrid = new Grid();
                ColumnDefinition c1 = new ColumnDefinition();
                c1.Width = new GridLength(2, GridUnitType.Star);
                ColumnDefinition c2 = new ColumnDefinition();
                c2.Width = new GridLength(1, GridUnitType.Star);
                albumGrid.ColumnDefinitions.Add(c1);
                albumGrid.ColumnDefinitions.Add(c2);
                //add the two checkboxes to the grid
                Grid.SetColumn(includeAlbum, 0);
                albumGrid.Children.Add(includeAlbum);
                Grid.SetColumn(shuffleAlbum, 1);
                albumGrid.Children.Add(shuffleAlbum);

                //Make ListBoxItem to hold grid
                ListBoxItem albumItem = new ListBoxItem();
                albumItem.Content = albumGrid;
                albumItem.Tag = album;

                //add ListBoxItem to ListBox
                lstbxAlbums.Items.Add(albumItem);
            }
        }
    }
}
