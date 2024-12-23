using BYTProject.Data;

namespace BYTProject.Models;

public class Music
{
    // Mandatory attribute: MusicID
    private int _musicId;
    public int MusicId
    {
        get => _musicId;
        set
        {
            if (value <= 0) throw new ArgumentException("MusicID must be positive.");
            _musicId = value;
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

    // Mandatory attribute: Musician
    private Musician _musician;
    public Musician Musician
    {
        get => _musician;
        set
        {
            if (value == null) throw new ArgumentException("Musician cannot be null.");
            _musician = value;
        }
    }

    // Mandatory attribute: MusicAlbum
    private MusicAlbum _musicAlbum;
    public MusicAlbum MusicAlbum
    {
        get => _musicAlbum;
        set
        {
            if (value == null) throw new ArgumentException("MusicAlbum cannot be null.");
            _musicAlbum = value;
        }
    }

    // Private static extent collection to store all Music objects
    private static List<Music> _musicExtent = new List<Music>();

    // Private static method to add a Music object to the extent, with validation
    internal static void AddMusic(Music music)
    {
        if (music == null)
        {
            throw new ArgumentException("Music cannot be null.");
        }
        _musicExtent.Add(music);
    }

    // Public static method to get a read-only copy of the extent
    public static IReadOnlyList<Music> GetMusicList()
    {
        return _musicExtent.AsReadOnly();
    }

    // Constructor to initialize Music object with mandatory attributes and automatically add to extent
    public Music(int musicId, string description, Musician musician, MusicAlbum musicAlbum)
    {
        MusicId = musicId;
        Description = description;
        Musician = musician;
        MusicAlbum = musicAlbum;

        // Automatically add to extent
        AddMusic(this);
    }
    public Music(){}

    // Method to save all music to XML (for persistence)
    public static void SaveMusic()
    {
        PersistenceManager.SaveExtent(_musicExtent, "Music.xml");
    }

    // Method to load all music from XML (for persistence)
    public static void LoadMusic()
    {
        _musicExtent = PersistenceManager.LoadExtent<Music>("Music.xml");
    }
    public static void ClearMusic()
    {
        _musicExtent.Clear();
    }
    
    //relation to musician
    // Method to set the musician with bidirectional association
    public void SetMusician(Musician musician)
    {
        if (_musician == musician) return; // Avoid redundant operations

        if (_musician != null)
        {
            // Remove this music from the old musician
            var oldMusician = _musician;
            _musician = null; // Temporarily break the association
            oldMusician.RemoveMusic(this);
        }

        _musician = musician;

        if (_musician != null && !_musician.GetMusicList().Contains(this))
        {
            // Add this music to the new musician if not already added
            _musician.AddMusic(this);
        }
    }

    // Method to remove the association with the musician
    public void RemoveMusician()
    {
        if (_musician == null) return; // No association to remove

        var oldMusician = _musician;
        _musician = null; // Clear the musician reference

        if (oldMusician.GetMusicList().Contains(this))
        {
            oldMusician.RemoveMusic(this); // Remove reverse association
        }
    }
}
    
    
    