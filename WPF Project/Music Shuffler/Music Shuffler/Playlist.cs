using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Music_Shuffler;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;

namespace Music_Shuffler {
    /// <summary>
    /// Generates multiple albums from each folder within rootFolder that contains music files
    /// Generates a randomised playlist from those albums where each album maintains internal order
    /// The album itself might still be shuffled beforehand
    /// </summary>
    class Playlist {
        public List<String> rootFolders = new List<string>();
        public List<Album> albums = new List<Album>();
        public Dictionary<String, String> playlist = new Dictionary<String, String>();
        public List<String> musicExtensions = new List<string>();
        public String outputFolder = "";
        BackgroundWorker bw; //handles the copying of files
        TextBox messageBox;
        ListBox lstBox;

        public Playlist(List<String> _rootFolders, List<String> _musicExtensions) {
            this.rootFolders = _rootFolders;
            this.musicExtensions = _musicExtensions;
            this.generateAlbums();

            messageBox = (Application.Current.MainWindow.Content as MainPage).txtMessageBlock;
            lstBox = (Application.Current.MainWindow.Content as MainPage).lstbxAlbums;

            bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = false;
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(bwCopyFiles);
            bw.ProgressChanged += new ProgressChangedEventHandler(bwUpdateText);
        }

        /// <summary>
        /// Is called when makePlaylistReal calls bw.RunWorkerAsync();
        /// copies eahc file in the background, calling ReportProgress (ie. bwUpdateText)
        /// to update the text box. Also clears the playlist after its finished
        /// </summary>
        private void bwCopyFiles(object sender, DoWorkEventArgs e) {
            Application.Current.Dispatcher.Invoke(() => {
                messageBox.Visibility = Visibility.Visible;
                lstBox.Visibility = Visibility.Hidden;
                (Application.Current.MainWindow.Content as MainPage).btnChooseOutput.IsEnabled = false;
                (Application.Current.MainWindow.Content as MainPage).btnGetAlbums.IsEnabled = false;
                (Application.Current.MainWindow.Content as MainPage).btnMakePlaylist.IsEnabled = false;
                (Application.Current.MainWindow.Content as MainPage).txtOutputFolder.IsEnabled = false;
                (Application.Current.MainWindow.Content as MainPage).chkSelectAll.IsEnabled = false;
                (Application.Current.MainWindow.Content as MainPage).chkShuffleAll.IsEnabled = false;
            });
            bw.ReportProgress(0, "Preparing to copy to " + this.outputFolder + "...");
            foreach (KeyValuePair<String, String> song in this.playlist) {
                String modifiedPath = Path.Combine(this.outputFolder, this.playlist[song.Key]);
                File.Copy(song.Key, modifiedPath);
                bw.ReportProgress(0, song.Value);
            }
            playlist.Clear();
            Application.Current.Dispatcher.Invoke(() => {
                bw.ReportProgress(0, "Finished!");
                MessageBox.Show("Playlist created!", "Music Shuffler");
                messageBox.Visibility = Visibility.Hidden;
                lstBox.Visibility = Visibility.Visible;
                (Application.Current.MainWindow.Content as MainPage).btnChooseOutput.IsEnabled = true;
                (Application.Current.MainWindow.Content as MainPage).btnGetAlbums.IsEnabled = true;
                (Application.Current.MainWindow.Content as MainPage).btnMakePlaylist.IsEnabled = true;
                (Application.Current.MainWindow.Content as MainPage).txtOutputFolder.IsEnabled = true;
                (Application.Current.MainWindow.Content as MainPage).chkSelectAll.IsEnabled = true;
                (Application.Current.MainWindow.Content as MainPage).chkShuffleAll.IsEnabled = true;
            });
            
        }

        /// <summary>
        /// Writes out to the textbox when called by the background worker delegate
        /// </summary>
        private void bwUpdateText(object sender, ProgressChangedEventArgs e){
            messageBox.Text += e.UserState as String + "\n";

            //keep it scrolled to the end
            messageBox.Focus();
            if (!(System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftCtrl)
                  || System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.RightCtrl))) {
                messageBox.CaretIndex = messageBox.Text.Length;
                messageBox.ScrollToEnd();
            }
        }


        /// <summary>
        /// Generates albums from the direcotry tree. 
        /// Eahc album is initially set to stay sorted
        /// </summary>
        public void generateAlbums() {
            this.albums.Clear();
            List<Leaf> leaves = Utils.FolderWalk(this.rootFolders, this.musicExtensions);
            foreach (Leaf leaf in leaves) {
                this.albums.Add(new Album(leaf.root, leaf.files, _randomiseSongs: false));
            }
        }

        /// <summary>
        /// randomises songs from all albums - songs maintain order with respect to their album
        /// and copies the resultant playlist to the outputFolder
        /// </summary>
        public void generatePlaylist(List<Album> albumsToInclude, String outputFolder) {
            this.outputFolder = outputFolder;


            //we just keep randomly selecting 
            //an album and plucking the first song out until all the albums are empty
            //we need a deep copy of the albums list because we're deleting stuff from it
            List<Album> tempAlbumsList = albumsToInclude.Select(album => new Album(album)).ToList();

            //remove entries before the combo selection 
            foreach (Album album in tempAlbumsList) {
                //- 2 accounts for mapping from the combobox and the fact that it's inclusive
                //So 0 -> -2; 1 -> -1; 2 -> 0 (i.e. first will be deleted); 3 -> 1 etc.
                for (int i = album.cursorIndex - 2; i >= 0; --i) {
                    album.albumSongs.RemoveAt(i);
                }
                //and shuffle if need be
                if (album.randomiseSongs) {
                    album.shuffle();
                } 
            }


            int counter = 0;
            int numSongs = tempAlbumsList.Sum(album => album.numSongs());
            String prefixTemplateString = "D" + (Math.Floor(Math.Log10(numSongs)) + 1);
            while (tempAlbumsList.Count != 0) {
                int index = Utils.randomGenerator.Next(tempAlbumsList.Count);
                List<String> albumSongsLeft = tempAlbumsList[index].albumSongs;
                String originalSong = albumSongsLeft.First();
                playlist[originalSong] = counter.ToString(prefixTemplateString) + " - " + Path.GetFileName(originalSong);
                Console.WriteLine(playlist[originalSong]);
                albumSongsLeft.Remove(originalSong);
                if (albumSongsLeft.Count == 0) {
                    tempAlbumsList.Remove(tempAlbumsList[index]);
                }
                ++counter;
            }

            //actually copy the files and reset the playlist
            makePlaylistReal();
        }

        /// <summary>
        /// Copy the files
        /// </summary>
        public void makePlaylistReal() {
            if (bw.IsBusy != true) {
                bw.RunWorkerAsync();
            }
        }
    }
}
