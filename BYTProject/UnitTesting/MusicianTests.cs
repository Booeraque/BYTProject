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
            var musician1 = new Musician(1, "Bio1", 100);
            var musician2 = new Musician(2, "Bio2", 101);

            Musician.SaveMusicians();
            Musician.LoadMusicians();

            var musicians = Musician.GetMusicians();
            Assert.Equal(2, musicians.Count);
            Assert.Contains(musicians, m => m.MusicianId == 1 && m.MusicianBio == "Bio1" && m.AccountId == 100);
            Assert.Contains(musicians, m => m.MusicianId == 2 && m.MusicianBio == "Bio2" && m.AccountId == 101);
        }
    }
}
