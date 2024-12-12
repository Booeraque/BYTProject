using BYTProject.Data;

namespace BYTProject.Models;

public class MusicAlbum
{
    // Mandatory attribute: AlbumID
    private int _albumId;
    public int AlbumId
    {
        get => _albumId;
        set
        {
            if (value <= 0) throw new ArgumentException("AlbumID must be positive.");
            _albumId = value;
        }
    }

    // Mandatory attribute: Type
    private string _type;
    public string Type
    {
        get => _type;
        set
        {
            if (!AllowedTypes.Contains(value)) throw new ArgumentException($"Type must be one of the following: {string.Join(", ", AllowedTypes)}.");
            _type = value;
        }
    }

    // Mandatory attribute: Description
    private string _description;
    public string Description
    {
        get => _description;
        set
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Description can't be empty.");
            _description = value;
        }
    }

    // Collection to store Music objects that are part of the album
    private List<Music> _musicList = new List<Music>();
    public IReadOnlyList<Music> MusicList => _musicList.AsReadOnly();

    // Static field to store allowed types
    public static readonly List<string> AllowedTypes = new List<string> { "Single", "Mix", "Live", "Default" };

    // Method to add Music to the album
    public void AddMusic(Music music)
    {
        if (music == null) throw new ArgumentException("Music cannot be null.");
        _musicList.Add(music);
    }

    // Private static extent collection to store all MusicAlbum objects
    private static List<MusicAlbum> _musicAlbumsExtent = new List<MusicAlbum>();

    // Private static method to add a MusicAlbum to the extent, with validation
    internal static void AddMusicAlbum(MusicAlbum musicAlbum)
    {
        if (musicAlbum == null) throw new ArgumentException("MusicAlbum cannot be null.");
        _musicAlbumsExtent.Add(musicAlbum);
    }

    // Public static method to get a read-only copy of the extent
    public static IReadOnlyList<MusicAlbum> GetMusicAlbums()
    {
        return _musicAlbumsExtent.AsReadOnly();
    }

    // Constructor to initialize MusicAlbum with mandatory attributes and automatically add to extent
    public MusicAlbum(int albumId, string type, string description)
    {
        AlbumId = albumId;
        Type = type;
        Description = description;

        // Automatically add to extent
        AddMusicAlbum(this);
    }

    public MusicAlbum()
    {
        
    }

    // Method to save all music albums to XML (for persistence)
    public static void SaveMusicAlbums()
    {
        PersistenceManager.SaveExtent(_musicAlbumsExtent, "MusicAlbums.xml");
    }

    // Method to load all music albums from XML (for persistence)
    public static void LoadMusicAlbums()
    {
        _musicAlbumsExtent = PersistenceManager.LoadExtent<MusicAlbum>("MusicAlbums.xml");
    }
    public static void ClearMusicAlbums()
    {
        _musicAlbumsExtent.Clear();
    }
    
    // Association: Many MusicAlbums -> One Musician
    private Musician _musician;

    // Getter for the associated musician
    public Musician GetMusician() => _musician;

    // Set the musician (reverse connection)
    public void SetMusician(Musician musician)
    {
        if (_musician == musician) return; // Avoid redundant operations

        if (_musician != null)
        {
            // Temporarily skip reverse updates to avoid recursion
            var previousMusician = _musician;
            _musician = null; // Break the current reference
            previousMusician.RemoveMusicAlbum(this); // Remove this album from the old musician
        }

        _musician = musician;

        if (_musician != null && !_musician.GetMusicAlbums().Contains(this))
        {
            // Add this album to the new musician only if it doesn't already exist
            _musician.AddMusicAlbum(this);
        }
    }

    // Remove the association with the musician
    public void RemoveMusician()
    {
        if (_musician == null) return; // Nothing to remove

        var oldMusician = _musician;
        _musician = null; // Break the association

        // Ensure reverse reference removal only if the musician still has this album
        if (oldMusician.GetMusicAlbums().Contains(this))
        {
            oldMusician.RemoveMusicAlbum(this);
        }
    }
}