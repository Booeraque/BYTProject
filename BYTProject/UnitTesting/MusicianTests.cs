using BYTProject.Models;
using Xunit;
using System;

namespace BYTProject.UnitTesting
{
    public class MusicianTests
    {
        public MusicianTests()
        {
            // Clear musicians before each test
            Musician.ClearMusicians();
        }
        
        [Fact]
        public void MusicianId_ShouldThrowException_WhenValueIsNonPositive()
        {
            Assert.Throws<ArgumentException>(() => new Musician(0, "Test Bio", 1));
        }

        [Fact]
        public void MusicianId_ShouldReturnCorrectValue()
        {
            var musician = new Musician(1, "Test Bio", 100);
            Assert.Equal(1, musician.MusicianId);
        }

        [Fact]
        public void MusicianBio_ShouldThrowException_WhenValueIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => new Musician(1, "", 1));
        }

        [Fact]
        public void MusicianBio_ShouldReturnCorrectValue()
        {
            var musician = new Musician(1, "Test Bio", 100);
            Assert.Equal("Test Bio", musician.MusicianBio);
        }

        [Fact]
        public void AccountId_ShouldThrowException_WhenValueIsNonPositive()
        {
            Assert.Throws<ArgumentException>(() => new Musician(1, "Test Bio", 0));
        }

        [Fact]
        public void AccountId_ShouldReturnCorrectValue()
        {
            var musician = new Musician(1, "Test Bio", 100);
            Assert.Equal(100, musician.AccountId);
        }

        [Fact]
        public void AddMusician_ShouldThrowException_WhenMusicianIsNull()
        {
            Assert.Throws<ArgumentException>(() => Musician.AddMusician(null));
        }

        [Fact]
        public void AddMusician_ShouldAddMusicianToExtent()
        {
            var musician = new Musician(1, "Test Bio", 100);
            var musicians = Musician.GetMusicians();
            Assert.Contains(musician, musicians);
        }

        [Fact]
        public void GetMusicians_ShouldReturnCorrectExtent()
        {
            var musician1 = new Musician(1, "Bio1", 100);
            var musician2 = new Musician(2, "Bio2", 101);

            var musicians = Musician.GetMusicians();
            Assert.Contains(musician1, musicians);
            Assert.Contains(musician2, musicians);
        }

        [Fact]
        public void SaveAndLoadMusicians_ShouldPersistExtentCorrectly()
        {
            Musician.ClearMusicians();
            var musician1 = new Musician(1, "Bio11", 100);
            var musician2 = new Musician(2, "Bio21", 101);

            Musician.SaveMusicians();
            Musician.LoadMusicians();

            var musicians = Musician.GetMusicians();
            Assert.Equal(2, musicians.Count);
    
            // Updated expected values to match the actual data
            Assert.Contains(musicians, m => m.MusicianId == 1 && m.MusicianBio == "Bio11" && m.AccountId == 100);
            Assert.Contains(musicians, m => m.MusicianId == 2 && m.MusicianBio == "Bio21" && m.AccountId == 101);
        }
        
        // New Tests for AddMusicAlbum

        [Fact]
        public void AddMusicAlbum_ShouldAddAlbumToMusician()
        {
            var musician = new Musician(1, "Test Bio", 100);
            var album = new MusicAlbum(1, "Single", "First Album");

            musician.AddMusicAlbum(album);

            Assert.Contains(album, musician.GetMusicAlbums());
        }

        [Fact]
        public void AddMusicAlbum_ShouldSetMusicianOnAlbum()
        {
            var musician = new Musician(1, "Test Bio", 100);
            var album = new MusicAlbum(1, "Single", "First Album");

            musician.AddMusicAlbum(album);

            Assert.Equal(musician, album.GetMusician());
        }

        [Fact]
        public void AddMusicAlbum_ShouldThrowException_WhenAlbumIsNull()
        {
            var musician = new Musician(1, "Test Bio", 100);
            Assert.Throws<ArgumentNullException>(() => musician.AddMusicAlbum(null));
        }

        [Fact]
        public void AddMusicAlbum_ShouldThrowException_WhenAlbumAlreadyExists()
        {
            var musician = new Musician(1, "Test Bio", 100);
            var album = new MusicAlbum(1, "Single", "First Album");

            musician.AddMusicAlbum(album);

            Assert.Throws<InvalidOperationException>(() => musician.AddMusicAlbum(album));
        }

        // New Tests for RemoveMusicAlbum

        [Fact]
        public void RemoveMusicAlbum_ShouldRemoveAlbumFromMusician()
        {
            var musician = new Musician(1, "Test Bio", 100);
            var album = new MusicAlbum(1, "Single", "First Album");

            musician.AddMusicAlbum(album);
            musician.RemoveMusicAlbum(album);

            Assert.DoesNotContain(album, musician.GetMusicAlbums());
        }

        [Fact]
        public void RemoveMusicAlbum_ShouldRemoveMusicianFromAlbum()
        {
            var musician = new Musician(1, "Test Bio", 100);
            var album = new MusicAlbum(1, "Single", "First Album");

            musician.AddMusicAlbum(album);
            musician.RemoveMusicAlbum(album);

            Assert.Null(album.GetMusician());
        }

        [Fact]
        public void RemoveMusicAlbum_ShouldThrowException_WhenAlbumIsNull()
        {
            var musician = new Musician(1, "Test Bio", 100);
            Assert.Throws<ArgumentNullException>(() => musician.RemoveMusicAlbum(null));
        }

        [Fact]
        public void RemoveMusicAlbum_ShouldThrowException_WhenAlbumDoesNotExist()
        {
            var musician = new Musician(1, "Test Bio", 100);
            var album = new MusicAlbum(1, "Single", "First Album");

            Assert.Throws<InvalidOperationException>(() => musician.RemoveMusicAlbum(album));
        }

        // New Tests for Bidirectional Relationship

        [Fact]
        public void Musician_ShouldNotHaveDuplicateAlbums()
        {
            var musician = new Musician(1, "Test Bio", 100);
            var album1 = new MusicAlbum(1, "Single", "First Album");
            var album2 = new MusicAlbum(2, "Live", "Second Album");

            musician.AddMusicAlbum(album1);
            musician.AddMusicAlbum(album2);

            Assert.Equal(2, musician.GetMusicAlbums().Count);
        }

        [Fact]
        public void Album_ShouldChangeMusician_WhenReassigned()
        {
            var musician1 = new Musician(1, "Bio 1", 100);
            var musician2 = new Musician(2, "Bio 2", 101);
            var album = new MusicAlbum(1, "Single", "First Album");

            musician1.AddMusicAlbum(album);
            musician2.AddMusicAlbum(album);

            Assert.DoesNotContain(album, musician1.GetMusicAlbums());
            Assert.Contains(album, musician2.GetMusicAlbums());
            Assert.Equal(musician2, album.GetMusician());
        }

    }
}
