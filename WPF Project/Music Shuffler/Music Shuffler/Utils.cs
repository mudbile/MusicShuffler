using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_Shuffler {
    /// <summary>
    /// Small class for storing a folder path and some file paths within that folder.
    /// Stores the full file paths for the folder and the files
    /// </summary>
    class Leaf {
        public String root = "";
        public List<String> files = new List<string>();
        public Leaf() { }
        
        public Leaf(String _root, List<String> _files) {
            //Normalise the path formats
            this.root = Path.GetFullPath(_root);
            for (int i = 0; i != _files.Count; ++i) {
                 _files[i] = Path.GetFullPath(_files[i]);
            }
            this.files = _files;
        }
    }



    static class Utils {
        public static Random randomGenerator = new Random();

        /// <summary>
        /// Shuffle a list -> just a simple fisher yates, no need for heaps of randomness
        /// </summary>
        static public void ShuffleList<T>(List<T> pool) {
            for (int randIndex, thisIndex = 0; thisIndex < pool.Count; ++thisIndex) {
                randIndex = randomGenerator.Next(thisIndex + 1);
                T temp = pool[randIndex];
                pool[randIndex] = pool[thisIndex];
                pool[thisIndex] = temp;
            }
        }


        /// <summary>
        /// Walks down a directory tree from rootFolder (inclusive) and returns a list of Leaf objects 
        /// for those folders that contain at least one file with a validExtensions extensions
        /// </summary>
        public static List<Leaf> FolderWalk(List<String> rootFolders, List<String> validExtensions) {
            List<Leaf> leaves = new List<Leaf>();
            foreach (String rootFolder in rootFolders) {
                leaves.AddRange(FolderWalk(rootFolder, validExtensions));
            }
            return leaves;
        }


            /// <summary>
            /// Walks down a directory tree from rootFolder (inclusive) and returns a list of Leaf objects 
            /// for those folders that contain at least one file with a validExtensions extensions
            /// </summary>
            public static List<Leaf> FolderWalk(string rootFolder, List<String> validExtensions) {
            List<Leaf> leaves = new List<Leaf>();
  
            //make folders a list (dynamic) because we need to add the rootFolder
            List<String> folders = Directory.GetDirectories(rootFolder).ToList();
            folders.Add(rootFolder);

            foreach (string folder in folders) {
                List<string> files = Directory.GetFiles(rootFolder).Where(
                    file => validExtensions.Contains(Path.GetExtension(file))
                ).ToList();

                if (files.Count != 0) {
                    Leaf leaf = new Leaf(folder, files);
                    leaves.Add(leaf);
                }

                //recursive call - avoid an infinite loop
                if (folder != rootFolder) {
                    leaves.AddRange(FolderWalk(folder, validExtensions));
                }
            }

            return leaves;
        }
    }
}
