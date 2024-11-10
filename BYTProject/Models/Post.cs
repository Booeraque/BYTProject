using System;
using System.Collections.Generic;

public class Post
{
    // Mandatory attribute: PostID
    private int _postId;
    public int PostId
    {
        get => _postId;
        set
        {
            if (value <= 0) throw new ArgumentException("PostID must be positive.");
            _postId = value;
        }
    }

    // Mandatory attribute: Caption
    private string _caption;
    public string Caption
    {
        get => _caption;
        set
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Caption can't be empty.");
            _caption = value;
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

    // Static extent collection to store all Post objects
    private static List<Post> _postsExtent = new List<Post>();

    // Static method to add a Post to the extent, with validation
    internal static void AddPost(Post post)
    {
        if (post == null)
        {
            throw new ArgumentException("Post cannot be null.");
        }
        _postsExtent.Add(post);
    }

    // Public static method to get a read-only copy of the extent
    public static IReadOnlyList<Post> GetPosts()
    {
        return _postsExtent.AsReadOnly();
    }

    // Constructor to initialize Post with mandatory attributes and automatically add to extent
    public Post(int postID, string caption, DateTime createdAt)
    {
        PostId = postID;
        _caption = caption;
        Caption = caption;
        CreatedAt = createdAt;

        // Automatically add to extent
        AddPost(this);
    }

    // Method to save all posts to XML (for persistence)
    public static void SavePosts()
    {
        PersistenceManager.SaveExtent(_postsExtent, "Posts.xml");
    }

    // Method to load all posts from XML (for persistence)
    public static void LoadPosts()
    {
        _postsExtent = PersistenceManager.LoadExtent<Post>("Posts.xml");
    }
}
