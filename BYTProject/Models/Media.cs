using BYTProject.Data;

namespace BYTProject.Models;

public class Media
{
    // Mandatory attribute: MediaID
    private int _mediaId;
    public int MediaId
    {
        get => _mediaId;
        set
        {
            if (value <= 0) throw new ArgumentException("MediaID must be positive.");
            _mediaId = value;
        }
    }

    // Mandatory attribute: MediaType
    private string _mediaType;
    public string MediaType
    {
        get => _mediaType;
        set
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("MediaType can't be empty.");
            _mediaType = value;
        }
    }

    // Private static extent collection to store all Media objects
    private static List<Media> _mediaExtent = new List<Media>();

    // Private static method to add a Media object to the extent, with validation
    internal static void AddMedia(Media media)
    {
        if (media == null)
        {
            throw new ArgumentException("Media cannot be null.");
        }
        _mediaExtent.Add(media);
    }

    // Public static method to get a read-only copy of the extent
    public static IReadOnlyList<Media> GetMediaList()
    {
        return _mediaExtent.AsReadOnly();
    }

    // Constructor to initialize Media object with mandatory attributes and automatically add to extent
    public Media(int mediaID, string mediaType)
    {
        MediaId = mediaID;
        _mediaType = mediaType;
        MediaType = mediaType;

        // Automatically add to extent
        AddMedia(this);
    }
    public Media(){}

    // Method to save all media to XML (for persistence)
    public static void SaveMedia()
    {
        PersistenceManager.SaveExtent(_mediaExtent, "Media.xml");
    }

    // Method to load all media from XML (for persistence)
    public static void LoadMedia()
    {
        _mediaExtent = PersistenceManager.LoadExtent<Media>("Media.xml");
    }
    
    public static void ClearMedia()
    {
        _mediaExtent.Clear();
    }
    // Association: One Media -> One Group
    private Group _group;

// Getter: Get the associated group
    public Group Group => _group;

// Method: Set the Group for the Media
    public void SetGroup(Group group)
    {
        if (_group == group) return;

        // Disassociate from the current group, if any
        _group?.RemoveMedia(this);

        // Set the new group
        _group = group;

        // Add this media to the new group
        group?.AddMedia(this);
    }

// Method: Remove the Group reference
    public void RemoveGroup()
    {
        if (_group != null)
        {
            var previousGroup = _group;
            _group = null; // Clear forward reference
            previousGroup.RemoveMedia(this); // Remove from the group's media list
        }
    }

}