﻿using System;
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
        public List<String> rootFolders = new List<string>();
        public List<Album> albums = new List<Album>();
        public Dictionary<String, String> playlist = new Dictionary<String, String>();
        public List<String> musicExtensions = new List<string>();
        public String outputFolder = "";

        public Playlist(List<String> _rootFolders, List<String> _musicExtensions) {
            this.rootFolders = _rootFolders;
            this.musicExtensions = _musicExtensions;
            this.generateAlbums();
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
            //the albums are already shuffled if they needed to be, so we just keep randomly selecting 
            //an album and plucking the first song out until all the albums are empty
            //we need a deep copy of the albums list because we're deleting stuff from it
            List<Album> tempAlbumsList = albumsToInclude.Select(album => new Album(album)).ToList();
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
            makePlaylistReal(outputFolder);
            playlist.Clear();
        }

        /// <summary>
        /// Copy the files
        /// </summary>
        public void makePlaylistReal(String outputFolder) {
            foreach (KeyValuePair<String, String> song in this.playlist) {
                Console.WriteLine("Copying " + song.Value + "...");
                File.Copy(song.Key, Path.Combine(outputFolder, song.Value));
            }
        }
    }
}
