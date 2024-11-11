namespace BYTProject.Models;

public class Tag
{
    // Mandatory attribute: TagID
    private int _tagId;
    public int TagId
    {
        get => _tagId;
        set
        {
            if (value <= 0) throw new ArgumentException("TagID must be positive.");
            _tagId = value;
        }
    }

    // Mandatory attribute: Categories
    private List<string> _categories;
    public List<string> Categories
    {
        get => _categories;
        set
        {
            if (value == null) throw new ArgumentException("Categories cannot be null.");
            if (value.Count == 0 || value.Contains(string.Empty)) throw new ArgumentException("Categories cannot be empty.");
            if (value.Count > 10) throw new ArgumentException("Categories cannot contain more than 10 items.");
            _categories = value;
        }
    }

    // Constructor to initialize Tag with mandatory attributes
    public Tag(int tagId, List<string> categories)
    {
        TagId = tagId;
        Categories = categories;
    }

    // Static extent collection to store all Tag objects
    private static List<Tag> _tagsExtent = new List<Tag>();

    // Method to clear all tags (for testing purposes)
    public static void ClearTags()
    {
        _tagsExtent.Clear();
    }

    // Method to add a Tag to the extent, with validation
    public static void AddTag(Tag tag)
    {
        if (tag == null) throw new ArgumentException("Tag cannot be null.");
        _tagsExtent.Add(tag);
    }

    // Public static method to get a read-only copy of the extent
    public static IReadOnlyList<Tag> GetTags()
    {
        return _tagsExtent.AsReadOnly();
    }

    // Method to save all tags to XML (for persistence)
    public static void SaveTags()
    {
        PersistenceManager.SaveExtent(_tagsExtent, "Tags.xml");
    }

    // Method to load all tags from XML (for persistence)
    public static void LoadTags()
    {
        _tagsExtent = PersistenceManager.LoadExtent<Tag>("Tags.xml");
    }
    
}