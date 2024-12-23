﻿using BYTProject.Data;

namespace BYTProject.Models;

public class Musician
{
    // Mandatory attribute: MusicianID
    private int _musicianId;
    public int MusicianId
    {
        get => _musicianId;
        set
        {
            if (value <= 0) throw new ArgumentException("MusicianID must be positive.");
            _musicianId = value;
        }
    }

    // Mandatory attribute: MusicianBIO
    private string _musicianBio;
    public string MusicianBio
    {
        get => _musicianBio;
        set
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("MusicianBIO can't be empty.");
            _musicianBio = value;
        }
    }

    // Mandatory attribute: AccountID
    private int _accountId;
    public int AccountId
    {
        get => _accountId;
        set
        {
            if (value <= 0) throw new ArgumentException("AccountID must be positive.");
            _accountId = value;
        }
    }
    
    // Private static extent collection to store all Musician objects
    private static List<Musician> _musiciansExtent = new List<Musician>();

    // Private static method to add a Musician to the extent, with validation
    internal static void AddMusician(Musician musician)
    {
        if (musician == null)
        {
            throw new ArgumentException("Musician cannot be null.");
        }
        _musiciansExtent.Add(musician);
    }

    // Public static method to get a read-only copy of the extent
    public static IReadOnlyList<Musician> GetMusicians()
    {
        return _musiciansExtent.AsReadOnly();
    }

    // Constructor to initialize Musician with mandatory attributes and automatically add to extent
    public Musician(int musicianID, string musicianBIO, int accountId)
    {
        MusicianId = musicianID;
        MusicianBio = musicianBIO;
        AccountId = accountId;

        // Automatically add to extent
        AddMusician(this);
    }
    public Musician(){}

    // Method to save all musicians to XML (for persistence)
    public static void SaveMusicians()
    {
        PersistenceManager.SaveExtent(_musiciansExtent, "Musicians.xml");
    }

    // Method to load all musicians from XML (for persistence)
    public static void LoadMusicians()
    {
        _musiciansExtent = PersistenceManager.LoadExtent<Musician>("Musicians.xml");
    }
    
    public static void ClearMusicians()
    {
        _musiciansExtent.Clear();
    }
    
    
    // Association: One Musician -> Many MusicAlbums
    private readonly List<MusicAlbum> _musicAlbums = new List<MusicAlbum>();

    // Getter for music albums (read-only view)
    public IReadOnlyList<MusicAlbum> GetMusicAlbums() => _musicAlbums.AsReadOnly();

    // Add a MusicAlbum to the Musician
    public void AddMusicAlbum(MusicAlbum album)
    {
        if (album == null) throw new ArgumentNullException(nameof(album));
        if (_musicAlbums.Contains(album)) throw new InvalidOperationException("This album is already associated with the musician.");
            
        _musicAlbums.Add(album);
        album.SetMusician(this); // Set reverse reference
    }

    // Remove a MusicAlbum from the Musician
    public void RemoveMusicAlbum(MusicAlbum album)
    {
        if (album == null) throw new ArgumentNullException(nameof(album));
        if (!_musicAlbums.Contains(album)) throw new InvalidOperationException("This album is not associated with the musician.");

        _musicAlbums.Remove(album);

        // Ensure reverse reference is cleared only if it still points to this musician
        if (album.GetMusician() == this)
        {
            album.RemoveMusician();
        }
    }
    
    
    //music
    
    // Association: One Musician -> Many Music Tracks
    private readonly List<Music> _musicList = new List<Music>();

    // Getter for the music list (read-only view)
    public IReadOnlyList<Music> GetMusicList() => _musicList.AsReadOnly();

    // Method to add a Music track
    public void AddMusic(Music music)
    {
        if (music == null) throw new ArgumentNullException(nameof(music));
        if (_musicList.Contains(music)) throw new InvalidOperationException("This music track is already associated with this musician.");
    
        if (music.Musician != null && music.Musician != this)
        {
            // Remove the music from the previous musician
            music.Musician.RemoveMusic(music);
        }
    
        _musicList.Add(music);
        music.SetMusician(this); // Set the reverse reference
    }


    // Method to remove a Music track
    public void RemoveMusic(Music music)
    {
        if (music == null) throw new ArgumentNullException(nameof(music));
        if (!_musicList.Remove(music)) 
            throw new InvalidOperationException("This music track is not associated with the musician.");

        music.RemoveMusician(); // Clear the reverse reference
    }

}