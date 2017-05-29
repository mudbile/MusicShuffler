using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        /// Walks down multiple directory trees (inclusive) and returns a list of Leaf objects 
        /// for those folders that contain at least one file with a validExtensions extensions
        /// </summary>
        public static List<Leaf> FolderWalk(List<String> rootFolders, List<String> validExtensions) {
            List<Leaf> leaves = new List<Leaf>();
            foreach (String rootFolder in rootFolders) {
                leaves.AddRange(FolderWalk(rootFolder, validExtensions, firstTime: true));
            }
			Console.WriteLine(DateTime.Now);
			//List<Leaf> result = leaves.GroupBy(leaf => leaf.root)
			//	  .Select(groupMember => groupMember.First())
			//      .ToList();
			Console.WriteLine(DateTime.Now);
			return leaves;
        }


        /// <summary>
        /// Walks down a single directory tree from rootFolder (inclusive) and returns a list of Leaf objects 
        /// for those folders that contain at least one file with a validExtensions extensions
		/// This returns multiple copies of some folders so call .distinct() on the list. I need the ordering so
		/// it's either remove the multiples or sort. the hash into a list at some stage.
        /// </summary>
        public static List<Leaf> FolderWalk(string rootFolder, List<String> validExtensions, bool firstTime = true) {
            List<Leaf> leaves = new List<Leaf>();

            //make folders a list (dynamic) because we need to add the rootFolder
            List<String> folders = Directory.GetDirectories(rootFolder).ToList();
			if (firstTime) {
				folders.Add(rootFolder);
			}

            foreach (string folder in folders) {
				try {
					List<string> files = Directory.GetFiles(folder).Where(
						file => validExtensions.Contains(Path.GetExtension(file))
					).ToList();

					if (files.Count != 0) {
						Leaf leaf = new Leaf(folder, files);
						leaves.Add(leaf);
					}

				} catch (UnauthorizedAccessException) {
					Console.WriteLine(folder);
					continue;
				}

                

                //recursive call - avoid an infinite loop
                if (folder != rootFolder) {
                    leaves.AddRange(FolderWalk(folder, validExtensions, firstTime: false));
                }
            }

            return leaves;
        }
    }
}
