using System;
using System.Collections.Generic;
using BYTProject.Models;
using Xunit;

public class MediaTests
{
    [Fact]
    public void MediaID_ShouldThrowException_WhenValueIsNonPositive()
    {
        var media = new Media(1, "Video");
        Assert.Throws<ArgumentException>(() => media.MediaID = 0);
    }

    [Fact]
    public void MediaType_ShouldThrowException_WhenValueIsEmpty()
    {
        var media = new Media(2, "Image");
        Assert.Throws<ArgumentException>(() => media.MediaType = "");
    }

    [Fact]
    public void SaveAndLoadMedia_ShouldPersistDataCorrectly()
    {
        // Arrange
        var media1 = new Media(1, "Video");
        var media2 = new Media(2, "Image");

        // Act
        Media.SaveMedia();
        
        // Clear the in-memory extent list by reinitializing the mediaExtent list
        Media.LoadMedia();

        // Assert
        var mediaList = Media.GetMediaList();
        Assert.Equal(2, mediaList.Count);
        Assert.Equal(1, mediaList[0].MediaID);
        Assert.Equal("Video", mediaList[0].MediaType);
        Assert.Equal(2, mediaList[1].MediaID);
        Assert.Equal("Image", mediaList[1].MediaType);
    }
}