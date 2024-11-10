using System;
using System.Collections.Generic;
using Xunit;

public class MediaTests
{
    [Fact]
    public void MediaID_ShouldThrowException_WhenValueIsNonPositive()
    {
        var media = new Media();
        Assert.Throws<ArgumentException>(() => media.MediaID = 0);
    }

    [Fact]
    public void MediaType_ShouldThrowException_WhenValueIsEmpty()
    {
        var media = new Media();
        Assert.Throws<ArgumentException>(() => media.MediaType = "");
    }

    [Fact]
    public void SaveAndLoadMedia_ShouldPersistDataCorrectly()
    {
        // Arrange
        var media1 = new Media { MediaID = 1, MediaType = "Video" };
        var media2 = new Media { MediaID = 2, MediaType = "Image" };
        Media.MediaList.Add(media1);
        Media.MediaList.Add(media2);

        // Act
        Media.SaveMedia();
        Media.MediaList.Clear();
        Media.LoadMedia();

        // Assert
        Assert.Equal(2, Media.MediaList.Count);
        Assert.Equal(1, Media.MediaList[0].MediaID);
        Assert.Equal("Video", Media.MediaList[0].MediaType);
        Assert.Equal(2, Media.MediaList[1].MediaID);
        Assert.Equal("Image", Media.MediaList[1].MediaType);
    }
}