
using BYTProject.Data;

namespace BYTProject.Models;

public class PostTag
{
    // Mandatory attribute: AddedAt
    private DateTime _addedAt;
    public DateTime AddedAt
    {
        get => _addedAt;
        set
        {
            if (value > DateTime.Now) throw new ArgumentException("AddedAt cannot be in the future.");
            _addedAt = value;
        }
    }

    // Private static extent collection to store all PostTag objects
    private static List<PostTag> _postTagsExtent = new List<PostTag>();

    // Private static method to add a PostTag to the extent, with validation
    public static void AddTag(PostTag postTag)
    {
        if (postTag == null)
        {
            throw new ArgumentException("PostTag cannot be null.");
        }
        _postTagsExtent.Add(postTag);
    }

    // Public static method to get a read-only copy of the extent
    public static IReadOnlyList<PostTag> GetPostTags()
    {
        return _postTagsExtent.AsReadOnly();
    }

    // Constructor to initialize PostTag with mandatory attributes and automatically add to extent
    public PostTag(DateTime addedAt)
    {
        AddedAt = addedAt;

        // Automatically add to extent
        AddTag(this);
    }
    public PostTag(){}

    // Method to save all post tags to XML (for persistence)
    public static void SavePostTags()
    {
        PersistenceManager.SaveExtent(_postTagsExtent, "PostTags.xml");
    }

    // Method to load all post tags from XML (for persistence)
    public static void LoadPostTags()
    {
        _postTagsExtent = PersistenceManager.LoadExtent<PostTag>("PostTags.xml");
    }
    public static void ClearPostTags()
    {
        _postTagsExtent.Clear();
    }
    private Tag _tag;

    public Tag Tag => _tag;

    public void SetTag(Tag tag)
    {
        if (_tag == tag) return;

        _tag?.RemovePostTag(this);
        _tag = tag;
        tag?.AddPostTag(this);
    }

    public void RemoveTag()
    {
        if (_tag != null)
        {
            _tag.RemovePostTag(this); // Remove this PostTag from the Tag's PostTags collection
            _tag = null;              // Nullify the reference in PostTag
        }
    }

// Association: Tag -> Post
private readonly List<Post> _posts = new List<Post>();

public IReadOnlyList<Post> Posts => _posts.AsReadOnly();

public void AddPost(Post post)
{
    if (post == null) throw new ArgumentNullException(nameof(post));
    if (_posts.Contains(post)) throw new InvalidOperationException("Post is already associated with this tag.");

    _posts.Add(post);
}

public void RemovePost(Post post)
{
    if (post == null) throw new ArgumentNullException(nameof(post));
    _posts.Remove(post);
}

}