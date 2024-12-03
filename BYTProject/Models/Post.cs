using System;
using System.Collections.Generic;
using BYTProject.Data;
using BYTProject.Models;

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
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Caption cannot be empty or whitespace.");
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
            if (value > DateTime.Now)
                throw new ArgumentException("Creation date cannot be in the future.");
            _createdAt = value;
        }
    }

    // Static extent collection to store all Post objects
    private static readonly List<Post> _postsExtent = new List<Post>();

    // Static method to add a Post to the extent, with validation
    public static void AddPost(Post post)
    {
        if (post == null)
            throw new ArgumentException("Post cannot be null.");
        
        _postsExtent.Add(post);
    }

    // Static method to remove a Post from the extent
    public static void RemovePost(Post post)
    {
        if (post == null)
            throw new ArgumentException("Post cannot be null.");

        _postsExtent.Remove(post);
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
        Caption = caption;
        CreatedAt = createdAt;

        // Automatically add to extent
        AddPost(this);
    }

    public Post()
    {
        // Default constructor for persistence use
    }

    // Method to save all posts to XML (for persistence)
    public static void SavePosts()
    {
        try
        {
            PersistenceManager.SaveExtent(_postsExtent, "Posts.xml");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to save posts.", ex);
        }
    }

    // Method to load all posts from XML (for persistence)
    public static void LoadPosts()
    {
        try
        {
            _postsExtent.Clear();
            _postsExtent.AddRange(PersistenceManager.LoadExtent<Post>("Posts.xml"));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to load posts.", ex);
        }
    }

    public static void ClearPosts()
    {
        _postsExtent.Clear();
    }

    private Account _account;

    // Getter: Get the associated Account
    public Account Account => _account;

    // Internal method: Set the Account for this Post
    internal void SetAccount(Account account)
    {
        if (_account == account)
            return;

        if (account != null && _account != null)
            throw new InvalidOperationException("The post is already associated with another account.");

        // Disassociate from the current account, if any
        _account?.RemovePost(this);

        // Set the new account
        _account = account;

        // Associate the post with the new account
        account?.AddPost(this);
    }

    // Internal method: Remove the Account reference
    internal void RemoveAccount()
    {
        _account = null;
    }

    // Public method: Update the post's attributes
    public void UpdatePost(string newCaption, DateTime newCreatedAt)
    {
        // Validate the new caption
        if (string.IsNullOrWhiteSpace(newCaption))
            throw new ArgumentException("Caption cannot be empty or whitespace.");

        // Validate the new creation date
        if (newCreatedAt > DateTime.Now)
            throw new ArgumentException("Creation date cannot be in the future.");

        // Update attributes
        Caption = newCaption;
        CreatedAt = newCreatedAt;
    }
    
    
}
