using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Music_Shuffler;
using System.IO;

namespace Music_Shuffler {
    /// <summary>
    /// Generates multiple albums from each folder within rootFolder that contains music files
    /// Generates a randomised playlist from those albums where each album maintains internal order
    /// The album itself might still be shuffled beforehand
    /// </summary>
    class Playlist {
        public String rootFolder = "";
        public List<Album> albums = new List<Album>();
        public Dictionary<String, String> playlist = new Dictionary<String, String>();
        public List<String> musicExtensions = new List<string>();

        public Playlist(String _rootFolder, List<String> _musicExtensions) {
            this.rootFolder = _rootFolder;
            this.musicExtensions = _musicExtensions;
            this.generateAlbums();


            //testing print loop
            foreach (String song in playlist.Keys) {
                Console.WriteLine(song);
            }
        }

        /// <summary>
        /// Generates albums from the direcotry tree. 
        /// Eahc album is initially set to stay sorted
        /// </summary>
        public void generateAlbums() {
            List<Leaf> leaves = Utils.FolderWalk(this.rootFolder, this.musicExtensions);
            foreach (Leaf leaf in leaves) {
                this.albums.Add(new Album(leaf.root, leaf.files, _randomiseSongs: false));
            }
        }

        public void generatePlaylist() {
            foreach (Album album in this.albums) {
                if (album.randomiseSongs) {
                    album.shuffle();
                }
            }

            //we need a full copy because we pull out songs until each album is empty
            List<Album> deepCopyOfAlbums = albums.ConvertAll(album => new Album(album));
            int counter = 0;
            String prefixTemplateString = "D" + albums.Sum(album => album.numSongs()) / 10 + 1;

            while (deepCopyOfAlbums.Count != 0) {
                int index = Utils.randomGenerator.Next(deepCopyOfAlbums.Count);
                List<String> albumSongsLeft = deepCopyOfAlbums[index].albumSongs;
                String originalSong = albumSongsLeft.First();
                playlist[originalSong] = counter.ToString(prefixTemplateString) + " - " + Path.GetFileName(originalSong);
                albumSongsLeft.Remove(originalSong);
                if (albumSongsLeft.Count == 0) {
                    deepCopyOfAlbums.Remove(deepCopyOfAlbums[index]);
                }
                ++counter;
            }
        }
    }
}
