using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Music_Shuffler;

namespace Music_Shuffler {
    /// <summary>
    /// Generates multiple albums from each folder within rootFolder that contains music files
    /// Generates a randomised playlist from those albums where each album maintains internal order
    /// The album itself might still be shuffled beforehand
    /// </summary>
    class Playlist {
        public String rootFolder = "";
        public List<Album> albums = new List<Album>();
        public List<String> playlist = new List<string>();
        public List<String> musicExtensions = new List<string>();

        public Playlist(String _rootFolder, List<String> _musicExtensions) {
            this.rootFolder = _rootFolder;
            this.musicExtensions = _musicExtensions;
            this.generateAlbums();
            this.generatePlaylist();

            //testing print loop
            foreach (Album album in this.albums) {
                Console.WriteLine(album.albumRoot);
                foreach (String song in album.albumSongs) {
                    Console.WriteLine("\t" + song);
                }
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
            //do nothing yet
        }
    }
}
