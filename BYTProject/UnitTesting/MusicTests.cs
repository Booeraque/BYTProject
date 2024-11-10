using System;
using System.Collections.Generic;
using Xunit;

public class MusicTests
{
    [Fact]
    public void MusicID_ShouldThrowException_WhenValueIsNonPositive()
    {
        var music = new Music { MusicID = 1, Description = "Description 1" };
        Assert.Throws<ArgumentException>(() => music.MusicID = 0);
    }

    [Fact]
    public void MusicID_ShouldReturnCorrectValue()
    {
        var music = new Music { MusicID = 1, Description = "Description 1" };
        Assert.Equal(1, music.MusicID);
    }

    [Fact]
    public void Description_ShouldThrowException_WhenValueIsEmpty()
    {
        var music = new Music { MusicID = 1, Description = "Description 1" };
        Assert.Throws<ArgumentException>(() => music.Description = "");
    }

    [Fact]
    public void Description_ShouldReturnCorrectValue()
    {
        var music = new Music { MusicID = 1, Description = "Description 1" };
        Assert.Equal("Description 1", music.Description);
    }

    [Fact]
    public void AddMusic_ShouldThrowException_WhenMusicIsNull()
    {
        Assert.Throws<ArgumentException>(() => Music.AddMusic(null));
    }

    [Fact]
    public void AddMusic_ShouldAddMusicCorrectly()
    {
        var music = new Music { MusicID = 1, Description = "Description 1" };
        Music.AddMusic(music);
        Assert.Contains(music, Music.MusicList);
    }

    [Fact]
    public void GetMusicList_ShouldReturnCorrectList()
    {
        var music = new Music { MusicID = 1, Description = "Description 1" };
        Music.AddMusic(music);
        Assert.Contains(music, Music.MusicList);
    }

    [Fact]
    public void SaveAndLoadMusic_ShouldPersistDataCorrectly()
    {
        var music1 = new Music { MusicID = 1, Description = "Description 1" };
        var music2 = new Music { MusicID = 2, Description = "Description 2" };
        Music.MusicList.Add(music1);
        Music.MusicList.Add(music2);

        Music.SaveMusic();
        Music.MusicList.Clear();
        Music.LoadMusic();

        Assert.Equal(2, Music.MusicList.Count);
        Assert.Equal(1, Music.MusicList[0].MusicID);
        Assert.Equal("Description 1", Music.MusicList[0].Description);
        Assert.Equal(2, Music.MusicList[1].MusicID);
        Assert.Equal("Description 2", Music.MusicList[1].Description);
    }
}
