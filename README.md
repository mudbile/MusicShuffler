# Motivation

I have an mp3 player that doesn't shuffle and doesn't allow subdirectories. I wanted a way to randomise 
songs from a bunch of folders, but keep internal ordering for some of them (i.e. podcast segments and audiobooks).

It started out as a small python prototype but I wanted a GUI so it's a WPF application- you can just download the
"Music Shuffler" executable and run it (or, if you want to, you can dig down to the bin/release folder and grab the latest build).


# How to use


![Screenshot_1.png](/screenshots/Screenshot_1.png) 

Note: The text box at the top lists the extensions that the app will search for as music files. Each extension should
be separated by a comma, semi-colon or vertical bar. It doesn't matter if there's spaces between them. 

Start the application and click **GET ALBUMS**. You can select multiple top-level directories and he will dig down 
into the directory trees to find folders that contain music files. These will be displayed in the list box as albums. 

You can select / deselect which albums to include by right-clicking on them. If it's faded out, the album
won't be included. 


![Screenshot_2.png](/screenshots/Screenshot_2.png)


If you check the **Shuffle** checkbox, you are saying it's okay for this album to be 
randomised. If you leave this checkbox unchecked, the app will keep internal ordering within that album.

You can click on the dropdown boxes and select which track to start at (useful if you're halfway through an audio book
or something). This is inclusive, so if you leave it on the album name or on the first item then all the
tracks will be included. 

You can either type in or use the folder browser dialogue to configure where you want to store the playlist. 

Once you click **MAKE PLAYLIST** the copying will begin. 


![Screenshot_3.png](/screenshots/Screenshot_3.png)


If you'd like, you can click **STOP** and the process will stop after it is finished copying the current file.
Afterwards, you have your playlist in the folder you chose:

![Screenshot_4.png](/screenshots/Screenshot_4.png)


# Thanks

I found a useful class for providing natural ordering [here](https://www.codeproject.com/Articles/22978/Implementing-the-NET-IComparer-interface-to-get-a).

I also used Fody to wrap everything up into one executable, and the old WindowsAPICodepack for the folder dialogue.

