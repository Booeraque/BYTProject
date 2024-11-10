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
}