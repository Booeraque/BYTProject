using System;
using System.Collections.Generic;

public class Tag
{
    // Mandatory attribute: TagID
    private int _tagID;
    public int TagID
    {
        get => _tagID;
        set
        {
            if (value <= 0) throw new ArgumentException("TagID must be positive.");
            _tagID = value;
        }
    }

    // Optional multi-value attribute: Category (0..10 values)
    private List<string> _categories = new List<string>();
    public List<string> Categories
    {
        get => new List<string>(_categories); // Return a copy to prevent external modification
        set
        {
            if (value == null || value.Count > 10)
            {
                throw new ArgumentException("Categories must contain between 0 and 10 values.");
            }
            foreach (var category in value)
            {
                if (string.IsNullOrEmpty(category))
                {
                    throw new ArgumentException("Category cannot be empty.");
                }
            }
            _categories = new List<string>(value); // Assign a copy to ensure encapsulation
        }
    }

    // Private static extent collection to store all Tag objects
    private static List<Tag> tagsExtent = new List<Tag>();

    // Static method to add a Tag to the extent, with validation
    internal static void AddTag(Tag tag)
    {
        if (tag == null)
        {
            throw new ArgumentException("Tag cannot be null.");
        }
        tagsExtent.Add(tag);
    }

    // Public static method to get a read-only copy of the extent
    public static IReadOnlyList<Tag> GetTags()
    {
        return tagsExtent.AsReadOnly();
    }

    // Constructor to initialize Tag object with mandatory attributes and automatically add to extent
    public Tag(int tagID, List<string> categories = null)
    {
        TagID = tagID;
        Categories = categories ?? new List<string>(); // Allow empty list as it's optional

        // Automatically add to extent
        AddTag(this);
    }

    // Method to save all tags to XML (for persistence)
    public static void SaveTags()
    {
        PersistenceManager.SaveExtent(tagsExtent, "Tags.xml");
    }

    // Method to load all tags from XML (for persistence)
    public static void LoadTags()
    {
        tagsExtent = PersistenceManager.LoadExtent<Tag>("Tags.xml");
    }
}
