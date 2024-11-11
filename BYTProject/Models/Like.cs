namespace BYTProject.Models;

public class Like
{
    // Mandatory attribute: LikeID
    private int _likeId;
    public int LikeId
    {
        get => _likeId;
        set
        {
            if (value <= 0) throw new ArgumentException("LikeID must be positive.");
            _likeId = value;
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
    private static List<Like> _likesExtent = new List<Like>();

    // Private static method to add a Like to the extent
    internal static void AddLike(Like like)
    {
        if (like == null)
        {
            throw new ArgumentException("Like cannot be null.");
        }
        _likesExtent.Add(like);
    }

    // Public static method to get a read-only copy of the extent
    public static IReadOnlyList<Like> GetLikes()
    {
        return _likesExtent.AsReadOnly();
    }

    // Constructor to initialize Like object with mandatory attributes and automatically add to extent
    public Like(int likeId, DateTime createdAt)
    {
        LikeId = likeId;
        CreatedAt = createdAt;

        // Automatically add to extent
        AddLike(this);
    }
    public Like(){}

    // Method to save all likes to XML (for persistence)
    public static void SaveLikes()
    {
        PersistenceManager.SaveExtent(_likesExtent, "Likes.xml");
    }

    // Method to load all likes from XML (for persistence)
    public static void LoadLikes()
    {
        _likesExtent = PersistenceManager.LoadExtent<Like>("Likes.xml");
    }
    
    public static void ClearLikes()
    {
        _likesExtent.Clear();
    }
}