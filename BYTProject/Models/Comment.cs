namespace BYTProject.Models;

public class Comment
{
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
}