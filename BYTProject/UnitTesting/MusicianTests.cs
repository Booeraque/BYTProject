using BYTProject.Models;
using Xunit;
using System;

namespace BYTProject.UnitTesting
{
    public class MusicianTests
    {
        public MusicianTests()
        {
            // Clear musicians and music before each test
            Musician.ClearMusicians();
            Music.ClearMusic();
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
        
        //test for music
        
        [Fact]
        public void AddMusic_ShouldAddMusicToMusician()
        {
            var musician = new Musician(1, "Test Bio", 100);
            var music = new Music(1, "Test Music", musician, new MusicAlbum(1, "Single", "Test Album"));

            musician.AddMusic(music);

            Assert.Contains(music, musician.GetMusicList());
        }

        
        [Fact]
        public void AddMusic_ShouldSetMusicianOnMusic()
        {
            var musician = new Musician(1, "Test Bio", 100);
            var music = new Music(1, "Test Music", musician, new MusicAlbum(1, "Single", "Test Album"));

            musician.AddMusic(music);

            Assert.Equal(musician, music.Musician);
        }

        [Fact]
        public void AddMusic_ShouldThrowException_WhenMusicIsNull()
        {
            var musician = new Musician(1, "Test Bio", 100);

            Assert.Throws<ArgumentNullException>(() => musician.AddMusic(null));
        }

        [Fact]
        public void AddMusic_ShouldThrowException_WhenMusicAlreadyExists()
        {
            var musician = new Musician(1, "Test Bio", 100);
            var music = new Music(1, "Test Music", musician, new MusicAlbum(1, "Single", "Test Album"));

            musician.AddMusic(music);

            Assert.Throws<InvalidOperationException>(() => musician.AddMusic(music));
        }

        [Fact]
        public void RemoveMusic_ShouldRemoveMusicFromMusician()
        {
            var musician = new Musician(1, "Test Bio", 100);
            var music = new Music(1, "Test Music", musician, new MusicAlbum(1, "Single", "Test Album"));

            musician.AddMusic(music);
            musician.RemoveMusic(music);

            Assert.DoesNotContain(music, musician.GetMusicList());
        }

        [Fact]
        public void RemoveMusic_ShouldRemoveMusicianFromMusic()
        {
            var musician = new Musician(1, "Test Bio", 100);
            var music = new Music(1, "Test Music", musician, new MusicAlbum(1, "Single", "Test Album"));

            musician.AddMusic(music);
            musician.RemoveMusic(music);

            Assert.Null(music.Musician);
        }

        [Fact]
        public void RemoveMusic_ShouldThrowException_WhenMusicIsNull()
        {
            var musician = new Musician(1, "Test Bio", 100);

            Assert.Throws<ArgumentNullException>(() => musician.RemoveMusic(null));
        }

        [Fact]
        public void RemoveMusic_ShouldThrowException_WhenMusicDoesNotExist()
        {
            var musician = new Musician(1, "Test Bio", 100);
            var album = new MusicAlbum(1, "Single", "Test Album");
            var music = new Music(1, "Test Music", musician, album); // Valid Musician provided

            // Do not add the music to the musician
            Assert.Throws<InvalidOperationException>(() => musician.RemoveMusic(music));
        }


        [Fact]
        public void Musician_ShouldNotHaveDuplicateMusic()
        {
            var musician = new Musician(1, "Test Bio", 100);
            var music1 = new Music(1, "Music 1", musician, new MusicAlbum(1, "Single", "Album 1"));
            var music2 = new Music(2, "Music 2", musician, new MusicAlbum(2, "Single", "Album 2"));

            musician.AddMusic(music1);
            musician.AddMusic(music2);

            Assert.Equal(2, musician.GetMusicList().Count);
        }

        [Fact]
        public void Music_ShouldChangeMusician_WhenReassigned()
        {
            var musician1 = new Musician(1, "Bio 1", 100);
            var musician2 = new Musician(2, "Bio 2", 101);
            var music = new Music(1, "Test Music", musician1, new MusicAlbum(1, "Single", "Test Album"));

            musician1.AddMusic(music);
            musician2.AddMusic(music);

            Assert.DoesNotContain(music, musician1.GetMusicList());
            Assert.Contains(music, musician2.GetMusicList());
            Assert.Equal(musician2, music.Musician);
        }

        [Fact]
        public void Music_ShouldRemoveOldMusician_WhenReassigned()
        {
            var musician1 = new Musician(1, "Bio 1", 100);
            var musician2 = new Musician(2, "Bio 2", 101);
            var album = new MusicAlbum(1, "Single", "Test Album");
            var music = new Music(1, "Test Music", musician1, album);

            musician1.AddMusic(music);
            musician2.AddMusic(music); // This reassignment will now work as musician1 is valid.

            Assert.DoesNotContain(music, musician1.GetMusicList());
            Assert.Contains(music, musician2.GetMusicList());
            Assert.Equal(musician2, music.Musician);
        }
    }

}
