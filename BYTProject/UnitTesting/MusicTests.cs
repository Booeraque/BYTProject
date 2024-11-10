using System;
using System.Collections.Generic;
using BYTProject.Models;
using Xunit;

public class MusicTests
{
    [Fact]
    public void MusicID_ShouldThrowException_WhenValueIsNonPositive()
    {
        var music = new Music { MusicId = 1, Description = "Description 1" };
        Assert.Throws<ArgumentException>(() => music.MusicId = 0);
    }

    [Fact]
    public void Description_ShouldThrowException_WhenValueIsEmpty()
    {
        var music = new Music { MusicId = 1, Description = "Description 1" };
        Assert.Throws<ArgumentException>(() => music.Description = "");
    }
    
    [Fact]
    public void AddMusic_ShouldThrowException_WhenMusicIsNull()
    {
        Assert.Throws<ArgumentException>(() => Music.MusicList.Add(null));
    }

    [Fact]
    public void SaveAndLoadMusic_ShouldPersistDataCorrectly()
    {
        // Arrange
        var music1 = new Music { MusicId = 1, Description = "Description 1" };
        var music2 = new Music { MusicId = 2, Description = "Description 2" };
        Music.MusicList.Add(music1);
        Music.MusicList.Add(music2);

        // Act
        Music.SaveMusic();
        Music.MusicList.Clear();
        Music.LoadMusic();

        // Assert
        Assert.Equal(2, Music.MusicList.Count);
        Assert.Equal(1, Music.MusicList[0].MusicId);
        Assert.Equal("Description 1", Music.MusicList[0].Description);
        Assert.Equal(2, Music.MusicList[1].MusicId);
        Assert.Equal("Description 2", Music.MusicList[1].Description);
    }
}