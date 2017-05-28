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
        public int cursorIndex = 0;
        //used for natural ordering (e.g. 1 2 10 instead of 1 10 2)
        private NaturalComparer.NaturalComparer naturalComparer = new NaturalComparer.NaturalComparer();

        public Album() { }

        public Album(String _albumRoot, List<String> _albumSongs, bool _randomiseSongs) {
            this.albumRoot = _albumRoot;
            this.albumSongs = _albumSongs;
            this.sort();
            this.randomiseSongs = _randomiseSongs;
        }

        //Deep clone copy constructor
        public Album(Album album) {
            this.albumRoot = album.albumRoot;
            this.albumSongs = album.albumSongs.ToList();
            this.randomiseSongs = album.randomiseSongs;
            this.cursorIndex = album.cursorIndex;
            this.naturalComparer = new NaturalComparer.NaturalComparer();
        }

        public int numSongs() {
            return this.albumSongs.Count;
        }

        public void shuffle() {
            Utils.ShuffleList(this.albumSongs);
        }

        public void sort() {
            albumSongs.Sort(naturalComparer);
        }
    }
}
