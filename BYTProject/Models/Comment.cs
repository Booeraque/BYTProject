using BYTProject.Data;

namespace BYTProject.Models;

public class Comment
{
    private Account _account;
    private Post _post;
    // Mandatory attribute: CommentID
    private int _commentId;
    public int CommentId
    {
        get => _commentId;
        set
        {
            if (value <= 0) throw new ArgumentException("CommentID must be positive.");
            _commentId = value;
        }
    }

    // Mandatory attribute: Content
    private string _content;  // Renamed from Text to Content
    public string Content
    {
        get => _content;
        set
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Content can't be empty.");
            _content = value;
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

    // Boolean attribute: Edited
    private bool _edited;
    public bool Edited
    {
        get => _edited;
        set => _edited = value;  // No validation necessary for a boolean
    }

    // Private static extent collection to store all Comment objects
    private static List<Comment> _commentsExtent = new List<Comment>();

    // Private static method to add a Comment to the extent, with validation
    internal static void AddComment(Comment comment)
    {
        if (comment == null)
        {
            throw new ArgumentException("Comment cannot be null.");
        }
        _commentsExtent.Add(comment);
    }

    // Public static method to get a read-only copy of the extent
    public static IReadOnlyList<Comment> GetComments()
    {
        return _commentsExtent.AsReadOnly();
    }

    // Constructor to initialize Comment object and automatically add to extent
    public Comment(int commentID, string content, DateTime createdAt, bool edited = false)
    {
        CommentId = commentID;
        _content = content;
        Content = content;
        CreatedAt = createdAt;
        Edited = edited;

        // Automatically add to extent
        AddComment(this);
    }
    public Comment(){}

    // Method to save all comments to XML
    public static void SaveComments()
    {
        PersistenceManager.SaveExtent(_commentsExtent, "Comments.xml");
    }

    // Method to load all comments from XML
    public static void LoadComments()
    {
        _commentsExtent = PersistenceManager.LoadExtent<Comment>("Comments.xml");
    }
    
    public static void ClearComments()
    {
        _commentsExtent.Clear();
    }
    
    // Getter: Get the associated Account
    public Account Account => _account;

    // Internal method: Set the Account for this Comment
    internal void SetAccount(Account account)
    {
        if (_account == account)
            return;

        if (account != null && _account != null)
            throw new InvalidOperationException("The comment is already associated with another account.");

        // Disassociate from the current account, if any
        _account?.RemoveComment(this);

        // Set the new account
        _account = account;

        // Associate the comment with the new account
        account?.AddComment(this);
    }

    // Internal method: Remove the Account reference
    internal void RemoveAccount()
    {
        _account = null;
    }

    // Getter: Get the associated Post
    public Post Post => _post;

    // Internal method: Set the Post for this Comment
    internal void SetPost(Post post)
    {
        if (_post == post)
            return;

        if (post != null && _post != null)
            throw new InvalidOperationException("The comment is already associated with another post.");

        // Disassociate from the current post, if any
        _post?.RemoveComment(this);

        // Set the new post
        _post = post;

        // Associate the comment with the new post
        post?.AddComment(this);
    }

    // Internal method: Remove the Post reference
    internal void RemovePost()
    {
        if (_post != null)
        {
            var post = _post;
            _post = null;
            post.RemoveComment(this);
            _commentsExtent.Remove(this);
        }
    }
}