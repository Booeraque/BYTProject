using BYTProject.Data;

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
}