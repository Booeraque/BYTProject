using BYTProject.Models;
using Xunit;

namespace BYTProject.UnitTesting;

public class MediaTests
{        
    // Clear media before each test
    public MediaTests()
    {
        Media.ClearMedia();
        Group.ClearGroups();
    }
    
    [Fact]
    public void MediaID_ShouldThrowException_WhenValueIsNonPositive()
    {
        var media = new Media(1, "Video");
        Assert.Throws<ArgumentException>(() => media.MediaId = 0);
    }

    [Fact]
    public void MediaID_ShouldReturnCorrectValue()
    {
        var media = new Media(1, "Video");
        Assert.Equal(1, media.MediaId);
    }

    [Fact]
    public void MediaType_ShouldThrowException_WhenValueIsEmpty()
    {
        var media = new Media(2, "Image");
        Assert.Throws<ArgumentException>(() => media.MediaType = "");
    }

    [Fact]
    public void MediaType_ShouldReturnCorrectValue()
    {
        var media = new Media(2, "Image");
        Assert.Equal("Image", media.MediaType);
    }

    [Fact]
    public void AddMedia_ShouldThrowException_WhenMediaIsNull()
    {
        Assert.Throws<ArgumentException>(() => Media.AddMedia(null));
    }

    [Fact]
    public void AddMedia_ShouldAddMediaCorrectly()
    {
        var media = new Media(1, "Video");
        Media.AddMedia(media);
        Assert.Contains(media, Media.GetMediaList());
    }

    [Fact]
    public void GetMediaList_ShouldReturnCorrectList()
    {
        var media = new Media(1, "Video");
        Media.AddMedia(media);
        var mediaList = Media.GetMediaList();
        Assert.Contains(media, mediaList);
    }

    [Fact]
    public void SetGroup_ShouldAssignGroupCorrectly()
    {
        var group = new Group(1, "Test Group", "Test Description");
        var media = new Media(1, "Video");

        media.SetGroup(group);

        Assert.Equal(group, media.Group); // Forward association
        Assert.Contains(media, group.MediaList); // Reverse association
    }

    [Fact]
    public void SetGroup_ShouldReplaceExistingGroup()
    {
        var group1 = new Group(1, "Group 1", "Description 1");
        var group2 = new Group(2, "Group 2", "Description 2");
        var media = new Media(1, "Video");

        media.SetGroup(group1);
        media.SetGroup(group2);

        Assert.Equal(group2, media.Group);
        Assert.DoesNotContain(media, group1.MediaList); // Removed from previous group
        Assert.Contains(media, group2.MediaList); // Added to new group
    }

    [Fact]
    public void RemoveGroup_ShouldDisassociateGroupCorrectly()
    {
        var group = new Group(1, "Test Group", "Test Description");
        var media = new Media(1, "Video");

        media.SetGroup(group);
        media.RemoveGroup();

        Assert.Null(media.Group); // Forward association cleared
        Assert.DoesNotContain(media, group.MediaList); // Reverse association cleared
    }

    [Fact]
    public void SaveAndLoadMedia_ShouldPersistDataCorrectly()
    {
        var media1 = new Media(1, "Video");
        var media2 = new Media(2, "Image");

        Media.SaveMedia();
        Media.LoadMedia();

        var mediaList = Media.GetMediaList();
        Assert.Equal(2, mediaList.Count);
        Assert.Equal(1, mediaList[0].MediaId);
        Assert.Equal("Video", mediaList[0].MediaType);
        Assert.Equal(2, mediaList[1].MediaId);
        Assert.Equal("Image", mediaList[1].MediaType);
    }
}
