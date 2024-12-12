using BYTProject.Data;

namespace BYTProject.Models;

public class Like
{
    private Account _account;
    private Post _post;
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
    
    // Getter: Get the associated Account
    public Account Account => _account;

    // Internal method: Set the Account for this Like
    internal void SetAccount(Account account)
    {
        if (_account == account)
            return;

        if (account != null && _account != null)
            throw new InvalidOperationException("The like is already associated with another account.");

        // Disassociate from the current account, if any
        _account?.RemoveLike(this);

        // Set the new account
        _account = account;

        // Associate the like with the new account
        account?.AddLike(this);
    }

    // Internal method: Remove the Account reference
    internal void RemoveAccount()
    {
        _account = null;
    }

    // Getter: Get the associated Post
    public Post Post => _post;

    // Internal method: Set the Post for this Like
    internal void SetPost(Post post)
    {
        if (_post == post)
            return;

        if (post != null && _post != null)
            throw new InvalidOperationException("The like is already associated with another post.");

        // Disassociate from the current post, if any
        _post?.RemoveLike(this);

        // Set the new post
        _post = post;

        // Associate the like with the new post
        post?.AddLike(this);
    }

    // Internal method: Remove the Post reference
    internal void RemovePost()
    {
        _post = null;
    }
}