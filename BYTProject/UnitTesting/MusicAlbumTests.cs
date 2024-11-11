using System;
using System.Collections.Generic;
using BYTProject.Models;
using Xunit;

namespace BYTProject.UnitTesting
{
    public class MusicAlbumTests
    {
        [Fact]
        public void AlbumId_ShouldThrowException_WhenValueIsNonPositive()
        {
            Assert.Throws<ArgumentException>(() => new MusicAlbum(0, "Single", "Test Description"));
        }

        [Fact]
        public void AlbumId_ShouldReturnCorrectValue()
        {
            var album = new MusicAlbum(1, "Single", "Test Description");
            Assert.Equal(1, album.AlbumId);
        }

        [Fact]
        public void Type_ShouldThrowException_WhenValueIsInvalid()
        {
            Assert.Throws<ArgumentException>(() => new MusicAlbum(1, "InvalidType", "Test Description"));
        }

        [Fact]
        public void Type_ShouldReturnCorrectValue()
        {
            var album = new MusicAlbum(1, "Single", "Test Description");
            Assert.Equal("Single", album.Type);
        }

        [Fact]
        public void Description_ShouldThrowException_WhenValueIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => new MusicAlbum(1, "Single", ""));
        }

        [Fact]
        public void Description_ShouldReturnCorrectValue()
        {
            var album = new MusicAlbum(1, "Single", "Test Description");
            Assert.Equal("Test Description", album.Description);
        }

        [Fact]
        public void MusicList_ShouldReturnEmptyInitially()
        {
            var album = new MusicAlbum(1, "Single", "Test Description");
            Assert.Empty(album.MusicList);
        }

        [Fact]
        public void AddMusic_ShouldThrowException_WhenMusicIsNull()
        {
            var album = new MusicAlbum(1, "Single", "Test Description");
            Assert.Throws<ArgumentException>(() => album.AddMusic(null));
        }

        [Fact]
        public void AddMusic_ShouldAddMusicCorrectly()
        {
            var album = new MusicAlbum(1, "Single", "Test Description");
            var music = new Music(1, "Test Music", new Musician(1, "Bio", 1), album);
            album.AddMusic(music);

            Assert.Contains(music, album.MusicList);
        }

        [Fact]
        public void GetMusicAlbums_ShouldReturnAllAlbums()
        {
            var album1 = new MusicAlbum(1, "Single", "Album 1");
            var album2 = new MusicAlbum(2, "Mix", "Album 2");

            var albums = MusicAlbum.GetMusicAlbums();
            Assert.Contains(album1, albums);
            Assert.Contains(album2, albums);
        }

        [Fact]
        public void SaveAndLoadMusicAlbums_ShouldPersistAlbums()
        {
            var album1 = new MusicAlbum(1, "Single", "Album 1");
            var album2 = new MusicAlbum(2, "Mix", "Album 2");

            MusicAlbum.SaveMusicAlbums();
            MusicAlbum.LoadMusicAlbums();

            var albums = MusicAlbum.GetMusicAlbums();
            Assert.Equal(2, albums.Count);
            Assert.Contains(albums, a => a.AlbumId == 1 && a.Description == "Album 1");
            Assert.Contains(albums, a => a.AlbumId == 2 && a.Description == "Album 2");
        }
    }
}
