using System;
using System.Collections.Generic;

public class Like
{
    // Mandatory attribute: LikeID
    private int _likeID;
    public int LikeID
    {
        get => _likeID;
        set
        {
            if (value <= 0) throw new ArgumentException("LikeID must be positive.");
            _likeID = value;
        }
    }

    // Complex attribute: CreatedAt
    private DateTime _createdAt;
    public DateTime CreatedAt
    {
        get => _createdAt;
        set
        {
            if (value > DateTime.Now) throw new ArgumentException("Creation date cannot be in the future.");
            _createdAt = value;
        }
    }

    // Private static extent collection to store all Like objects
    private static List<Like> likesExtent = new List<Like>();

    // Private static method to add a Like to the extent
    internal static void AddLike(Like like)
    {
        if (like == null)
        {
            throw new ArgumentException("Like cannot be null.");
        }
        likesExtent.Add(like);
    }

    // Public static method to get a read-only copy of the extent
    public static IReadOnlyList<Like> GetLikes()
    {
        return likesExtent.AsReadOnly();
    }

    // Constructor to initialize Like object with mandatory attributes and automatically add to extent
    public Like(int likeID, DateTime createdAt)
    {
        LikeID = likeID;
        CreatedAt = createdAt;

        // Automatically add to extent
        AddLike(this);
    }

    // Method to save all likes to XML (for persistence)
    public static void SaveLikes()
    {
        PersistenceManager.SaveExtent(likesExtent, "Likes.xml");
    }

    // Method to load all likes from XML (for persistence)
    public static void LoadLikes()
    {
        likesExtent = PersistenceManager.LoadExtent<Like>("Likes.xml");
    }
}
