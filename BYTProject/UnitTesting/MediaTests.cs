using BYTProject.Models;
using Xunit;

namespace BYTProject.UnitTesting;

public class MediaTests
{
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
        var mediaList = Media.GetMediaList();
        Assert.Contains(media, mediaList);
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