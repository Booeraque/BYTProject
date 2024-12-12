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
    
    // Association: One Post -> Many Comments
    private readonly List<Comment> _comments = new List<Comment>();

    // Getter: Return a copy of the comments
    public IReadOnlyList<Comment> Comments => _comments.AsReadOnly();

    // Method: Add a Comment to the Post
    public void AddComment(Comment comment)
    {
        if (comment == null)
            throw new ArgumentNullException(nameof(comment), "Comment cannot be null.");

        // Check if a comment with the same CommentId already exists
        if (_comments.Any(c => c.CommentId == comment.CommentId))
            throw new InvalidOperationException("The comment is already associated with this post.");

        // Ensure the comment is disassociated from any previous post
        comment.SetPost(this);

        // Add the comment to this post's list
        _comments.Add(comment);
    }

    // Method: Remove a Comment from the Post
    public void RemoveComment(Comment comment)
    {
        if (comment == null)
            throw new ArgumentNullException(nameof(comment), "Comment cannot be null.");

        // Remove the comment and handle reverse reference removal
        if (_comments.Remove(comment))
        {
            comment.RemovePost(); // Ensure the reverse connection is removed
        }
    }

    // Association: One Post -> Many Likes
    private readonly List<Like> _likes = new List<Like>();

    // Getter: Return a copy of the likes
    public IReadOnlyList<Like> Likes => _likes.AsReadOnly();

    // Method: Add a Like to the Post
    public void AddLike(Like like)
    {
        if (like == null)
            throw new ArgumentNullException(nameof(like), "Like cannot be null.");

        // Check if a like with the same LikeId already exists
        if (_likes.Any(l => l.LikeId == like.LikeId))
            throw new InvalidOperationException("The like is already associated with this post.");

        // Ensure the like is disassociated from any previous post
        like.SetPost(this);

        // Add the like to this post's list
        _likes.Add(like);
    }

    // Method: Remove a Like from the Post
    public void RemoveLike(Like like)
    {
        if (like == null)
            throw new ArgumentNullException(nameof(like), "Like cannot be null.");

        // Remove the like and handle reverse reference removal
        if (_likes.Remove(like))
        {
            like.RemovePost(); // Ensure the reverse connection is removed
        }
    }
}
