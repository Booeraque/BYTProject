namespace BYTProject.Models;

public class Video
{
    // Mandatory attribute: VideoID
    private int _videoId;
    public int VideoId
    {
        get => _videoId;
        set
        {
            if (value <= 0) throw new ArgumentException("VideoID must be positive.");
            _videoId = value;
        }
    }

    // Mandatory attribute: Description
    private string _description;
    public string Description
    {
        get => _description;
        set
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Description can't be empty.");
            _description = value;
        }
    }

    // Mandatory attribute: VideoEditor
    private VideoEditor _videoEditor;
    public VideoEditor VideoEditor
    {
        get => _videoEditor;
        set
        {
            if (value == null) throw new ArgumentException("VideoEditor cannot be null.");
            _videoEditor = value;
        }
    }

    // Private static extent collection to store all Video objects
    private static List<Video> _videoExtent = new List<Video>();

    // Parameterless constructor for XML serialization
    public Video() { }

    // Constructor to initialize Video object with mandatory attributes and automatically add to extent
    public Video(int videoId, string description, VideoEditor videoEditor)
    {
        VideoId = videoId;
        Description = description;
        VideoEditor = videoEditor;

        // Automatically add to extent
        AddVideo(this);
    }

    // Private static method to add a Video object to the extent, with validation
    internal static void AddVideo(Video video)
    {
        if (video == null)
        {
            throw new ArgumentException("Video cannot be null.");
        }
        _videoExtent.Add(video);
    }

    // Public static method to get a read-only copy of the extent
    public static IReadOnlyList<Video> GetVideoList()
    {
        return _videoExtent.AsReadOnly();
    }

    // Method to save all videos to XML (for persistence)
    public static void SaveVideos()
    {
        Console.WriteLine("Saving videos to XML...");
        PersistenceManager.SaveExtent(_videoExtent, "Videos.xml");
        Console.WriteLine("Save complete.");
    }

    public static void LoadVideos()
    {
        Console.WriteLine("Loading videos from XML...");
        _videoExtent = PersistenceManager.LoadExtent<Video>("Videos.xml");
        Console.WriteLine($"Load complete. Loaded {_videoExtent.Count} videos.");
    }

}
