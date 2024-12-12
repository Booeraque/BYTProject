using System;
using System.Collections.Generic;
using BYTProject.Models;
using Xunit;

namespace BYTProject.UnitTesting
{
    public class MusicAlbumTests
    {
        public MusicAlbumTests()
        {
            // Clear albums and musicians before each test
            MusicAlbum.ClearMusicAlbums();
            Musician.ClearMusicians();
        }
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
        [Fact]
        public void SetMusician_ShouldSetMusicianCorrectly()
        {
            var musician = new Musician(1, "Test Bio", 1);
            var album = new MusicAlbum(1, "Single", "Test Description");

            album.SetMusician(musician);

            Assert.Equal(musician, album.GetMusician());
            Assert.Contains(album, musician.GetMusicAlbums());
        }

        [Fact]
        public void SetMusician_ShouldReassignMusicianCorrectly()
        {
            var musician1 = new Musician(1, "Bio 1", 1);
            var musician2 = new Musician(2, "Bio 2", 2);
            var album = new MusicAlbum(1, "Single", "Test Description");

            album.SetMusician(musician1);
            album.SetMusician(musician2);

            Assert.Equal(musician2, album.GetMusician());
            Assert.DoesNotContain(album, musician1.GetMusicAlbums());
            Assert.Contains(album, musician2.GetMusicAlbums());
        }

        [Fact]
        public void SetMusician_ShouldDoNothing_WhenSameMusicianIsSet()
        {
            var musician = new Musician(1, "Test Bio", 1);
            var album = new MusicAlbum(1, "Single", "Test Description");

            album.SetMusician(musician);
            album.SetMusician(musician); // Set the same musician again

            Assert.Equal(musician, album.GetMusician());
            Assert.Single(musician.GetMusicAlbums()); // Ensure no duplicates are added
        }

        [Fact]
        public void RemoveMusician_ShouldRemoveAssociation()
        {
            var musician = new Musician(1, "Test Bio", 1);
            var album = new MusicAlbum(1, "Single", "Test Description");

            album.SetMusician(musician);
            album.RemoveMusician();

            Assert.Null(album.GetMusician());
            Assert.DoesNotContain(album, musician.GetMusicAlbums());
        }

        [Fact]
        public void RemoveMusician_ShouldDoNothing_WhenNoMusicianIsSet()
        {
            var album = new MusicAlbum(1, "Single", "Test Description");

            album.RemoveMusician(); // Attempt to remove when no musician is associated

            Assert.Null(album.GetMusician()); // Ensure no errors occur
        }

        [Fact]
        public void BidirectionalAssociation_ShouldBeConsistent()
        {
            var musician = new Musician(1, "Test Bio", 1);
            var album = new MusicAlbum(1, "Single", "Test Description");

            album.SetMusician(musician);
            Assert.Equal(musician, album.GetMusician());
            Assert.Contains(album, musician.GetMusicAlbums());

            album.RemoveMusician();
            Assert.Null(album.GetMusician());
            Assert.DoesNotContain(album, musician.GetMusicAlbums());
        }
    }
}
