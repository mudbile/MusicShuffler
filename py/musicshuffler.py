import sys, os, glob
import random
from datetime import datetime
import math
from shutil import copyfile


#Stores a list of (randomised or not) music file pathnames
class Album:
	def __init__(self, albumRoot = "", albumSongs = [], randomiseSongs = False):
		self.albumRoot = albumRoot
		self.randomiseSongs = randomiseSongs
		self.iter_index = 0
		self.albumSongs = albumSongs
		if self.randomiseSongs:
			random.shuffle(self.albumSongs)
	
	def numSongs(self):
		return len(self.albumSongs)
	#-----------------------------------Iterator returns next song in album-------------------------------#
	def __iter__(self):
		self.iter_index = 0
		return self

	def __next__(self):
		if self.iter_index < len(self.albumSongs):
			temp_index = self.iter_index
			self.iter_index += 1
			return self.albumSongs[temp_index]
		else:
			raise StopIteration()
	#------------------------------------------------------------------------------------------------------#





# Generates multiple albums from each folder within rootFolder that contains music files
# Generates a randomised playlist from those albums where each album maintains internal order
# This order itself depends on the randomiseSongs argument given when each album is initialised
# Iterator gives next song as (original full path, modified full path)
class Playlist:

	# rootFolder -> root from which to dig down, generating albums
	# musicExtensions -> files with these extensions ware considered music tracks (case insensitive)
	def __init__(self, rootFolder, musicExtensions = [".mp3", ".wav"]):
		self.rootFolder = rootFolder
		self.iter_index = 0
		self.albums = self._generateAlbums()
		self.playlist = self._generatePlaylist()
		
	#returns a list of albums from the directory tree
	def _generateAlbums(self):
		albums = []
		for root, dirs, files in os.walk(self.rootFolder):
			songFiles = [file for file in files if os.path.splitext(file)[1] in musicExtensions]
			if not len(songFiles) == 0:
				albums.append(Album(root, songFiles))
		return albums

	# randomises songs from all albums - songs maintain order with respect to their album
	# returns {"originals": [list of original file names], "modified": [list of modified file names]}
	# where the modified file names are prefixed to enforce order
	def _generatePlaylist(self):
		playlist = {"originals": [], "modified": []}
		albumIterators = [iter(album) for album in self.albums]
		counter = 0
		numSongs = sum([a.numSongs() for a in self.albums])
		numDigitsInPrefix = math.floor(numSongs / 10) + 1
		while len(albumIterators) > 0:
			index = random.randint(0, len(albumIterators) - 1)
			albumIterator = albumIterators[index]
			try:
				song = next(albumIterator)
			except StopIteration:
				albumIterators.remove(albumIterator)
				continue
			playlist["originals"].append(os.path.join(self.albums[index].albumRoot, song))
			playlist["modified"].append(str(counter).zfill(numDigitsInPrefix) + " - " + song)
			counter += 1
		return playlist

	#copies the playlist files into outputFolder with their modified names
	def make(self, outputFolder = "Playlist"):
		if not os.path.isabs(outputFolder):
			outputFolder = os.path.join(rootFolder, outputFolder)
		if not os.path.isdir(outputFolder):
			os.makedirs(outputFolder)
		for song in self:
			copyfile(song[0], os.path.join(outputFolder, song[1]))
			print("Copied " + song[1])

	def printSongs(self):
		for song in self:
			print(song[1])
	
	def printAlbums(self):
		for album in self.albums:
			print(album.albumRoot)
			for song in album:
				print("\t" + song)

	#-----------------------------------Iterator returns (original full path, modified full path)-------------------------------#
	def __iter__(self):
		self.iter_index = 0
		return self

	def __next__(self):
		#"originals" and "modified" lists are same size
		if self.iter_index < len(self.playlist["modified"]): 
			temp_index = self.iter_index
			self.iter_index += 1
			return (self.playlist["originals"][temp_index], self.playlist["modified"][temp_index])
		else:
			raise StopIteration()
	#------------------------------------------------------------------------------------------------------#







random.seed(datetime.now())
musicExtensions = [".mp3", ".wav"] 
rootFolder = os.getcwd() if len(sys.argv) == 1 else sys.argv[1] 

playlist = Playlist(rootFolder)

playlist.printSongs()
playlist.make()