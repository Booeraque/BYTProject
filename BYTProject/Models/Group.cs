using BYTProject.Data;

namespace BYTProject.Models;

public class Group
{
    // Mandatory attribute: GroupID
    private int _groupId;
    public int GroupId
    {
        get => _groupId;
        set
        {
            if (value <= 0) throw new ArgumentException("GroupID must be positive.");
            _groupId = value;
        }
    }

    // Mandatory attribute: GroupName
    private string _groupName;
    public string GroupName
    {
        get => _groupName;
        set
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Group name can't be empty.");
            _groupName = value;
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

    // Private static extent collection to store all Group objects
    private static List<Group> _groupsExtent = new List<Group>();

    // Private static method to add a Group to the extent, with validation
    internal static void AddGroup(Group group)
    {
        if (group == null)
        {
            throw new ArgumentException("Group cannot be null.");
        }
        _groupsExtent.Add(group);
    }

    // Public static method to get a read-only copy of the extent
    public static IReadOnlyList<Group> GetGroups()
    {
        return _groupsExtent.AsReadOnly();
    }
    
    public static void ClearGroups()
    {
        _groupsExtent.Clear();
    }


    // Constructor to initialize Group with mandatory attributes and automatically add to extent
    public Group(int groupID, string groupName, string description)
    {
        GroupId = groupID;
        _groupName = groupName;
        _description = description;
        GroupName = groupName;
        Description = description;

        // Automatically add to extent
        AddGroup(this);
    }
    public Group(){}

    // Method to save all groups to XML (for persistence)
    public static void SaveGroups()
    {
        PersistenceManager.SaveExtent(_groupsExtent, "Groups.xml");
    }

    // Method to load all groups from XML (for persistence)
    public static void LoadGroups()
    {
        _groupsExtent = PersistenceManager.LoadExtent<Group>("Groups.xml");
    }
    // Association: One Group -> Many Media
    private readonly List<Media> _mediaList = new List<Media>();

// Getter: Return a copy of the media list
    public IReadOnlyList<Media> MediaList => _mediaList.AsReadOnly();

// Method: Add a Media to the Group
    public void AddMedia(Media media)
    {
        if (media == null)
            throw new ArgumentNullException(nameof(media), "Media cannot be null.");

        // Check if the media is already associated with this group
        if (_mediaList.Any(m => m.MediaId == media.MediaId))
            throw new InvalidOperationException("The media is already associated with this group.");

        // Ensure the media is disassociated from any previous group
        media.SetGroup(this);

        // Add the media to this group's list
        _mediaList.Add(media);
    }

// Method: Remove a Media from the Group
    public void RemoveMedia(Media media)
    {
        if (media == null)
            throw new ArgumentNullException(nameof(media), "Media cannot be null.");

        // Remove the media and handle reverse reference removal
        if (_mediaList.Remove(media))
        {
            media.RemoveGroup(); // Ensure the reverse connection is removed
        }
    }

    
}