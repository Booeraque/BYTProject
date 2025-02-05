﻿using BYTProject.Data;
namespace BYTProject.Models;

public class VideoEditor
{
    // Mandatory attribute: VideoEdID
    private int _videoEdId;
    public int VideoEdId
    {
        get => _videoEdId;
        set
        {
            if (value <= 0) throw new ArgumentException("VideoEdID must be positive.");
            _videoEdId = value;
        }
    }

    // Mandatory attribute: VideoEdBIO
    private string _videoEdBio;
    public string VideoEdBio
    {
        get => _videoEdBio;
        set
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("VideoEdBIO can't be empty.");
            _videoEdBio = value;
        }
    }

    // Mandatory attribute: AccountID
    private int _accountId;
    public int AccountId
    {
        get => _accountId;
        set
        {
            if (value <= 0) throw new ArgumentException("AccountID must be positive.");
            _accountId = value;
        }
    }

    // Private static extent collection to store all VideoEditor objects
    private static List<VideoEditor> _videoEditorsExtent = new List<VideoEditor>();

    // Private static method to add a VideoEditor to the extent, with validation
    internal static void AddVideoEditor(VideoEditor videoEditor)
    {
        if (videoEditor == null)
        {
            throw new ArgumentException("VideoEditor cannot be null.");
        }
        _videoEditorsExtent.Add(videoEditor);
    }

    // Public static method to get a read-only copy of the extent
    public static IReadOnlyList<VideoEditor> GetVideoEditors()
    {
        return _videoEditorsExtent.AsReadOnly();
    }

    // Constructor to initialize VideoEditor with mandatory attributes and automatically add to extent
    public VideoEditor(int videoEdId, string videoEdBio, int accountId)
    {
        VideoEdId = videoEdId;
        _videoEdBio = videoEdBio;
        VideoEdBio = videoEdBio;
        AccountId = accountId;

        // Automatically add to extent
        AddVideoEditor(this);
    }


    public VideoEditor()
    {
        
    }

    // Method to save all video editors to XML (for persistence)
    public static void SaveVideoEditors()
    {
        PersistenceManager.SaveExtent(_videoEditorsExtent, "VideoEditors.xml");
    }

    // Method to load all video editors from XML (for persistence)
    public static void LoadVideoEditors()
    {
        _videoEditorsExtent = PersistenceManager.LoadExtent<VideoEditor>("VideoEditors.xml");
    }
    
    public static void ClearVideoEditors()
    {
        _videoEditorsExtent.Clear();
    }
    
    
    // Association: One VideoEditor -> Many Videos
    private readonly List<Video> _uploadedVideos = new List<Video>();
    public IReadOnlyList<Video> UploadedVideos => _uploadedVideos.AsReadOnly();

    // Method to add a Video to the VideoEditor
    public void AddVideo(Video video)
    {
        if (video == null) throw new ArgumentNullException(nameof(video));
        if (_uploadedVideos.Contains(video)) throw new InvalidOperationException("This video is already uploaded by this editor.");

        _uploadedVideos.Add(video);
        video.SetVideoEditor(this); // Maintain bidirectional association
    }

    // Method to remove a Video from the VideoEditor
    public void RemoveVideo(Video video)
    {
        if (video == null) throw new ArgumentNullException(nameof(video));
        if (!_uploadedVideos.Remove(video)) // Check if video exists in the list
            throw new InvalidOperationException("This video is not uploaded by this editor.");

        video.RemoveVideoEditor(); // Clear reverse reference
    }
    

    
    public IReadOnlyList<Video> GetVideos()
    {
        return UploadedVideos;
    }

    
}