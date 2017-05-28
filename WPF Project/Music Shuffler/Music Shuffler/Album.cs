using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Music_Shuffler;

namespace Music_Shuffler {
    /// <summary>
    /// Stores a list of music file pathnames
    /// </summary>
    class Album {
        public String albumRoot = "";
        public List<String> albumSongs = new List<string>();
        public bool randomiseSongs = false;

        public Album() { }

        public Album(String _albumRoot, List<String> _albumSongs, bool _randomiseSongs) {
            this.albumRoot = _albumRoot;
            this.albumSongs = _albumSongs;
            this.randomiseSongs = _randomiseSongs;
            //Move this until when the user clicks to make playlist afte rselecting which to shuffle
            if (this.randomiseSongs) {
                Utils.ShuffleList(albumSongs);
            }
        }

        //Deep clone copy constructor
        public Album(Album album) {
            this.albumRoot = album.albumRoot;
            this.albumSongs = album.albumSongs;
            this.randomiseSongs = album.randomiseSongs;
        }

        public int numSongs() {
            return this.albumSongs.Count;
        }

        public void shuffle() {
            Utils.ShuffleList(this.albumSongs);
        }
    }
}
