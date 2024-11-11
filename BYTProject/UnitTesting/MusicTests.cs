using BYTProject.Models;
using Xunit;

namespace BYTProject.UnitTesting
{
    public class MusicTests
    {
        public MusicTests()
        {
            // Clear music before each test
            Music.ClearMusic();
        }
        private Musician CreateMockMusician()
        {
            return new Musician(1, "John Doe Bio", 100); // Mock data for testing
        }

        private MusicAlbum CreateMockMusicAlbum()
        {
            return new MusicAlbum(1, "Single", "Mock Album Description");
        }

        [Fact]
        public void MusicId_ShouldThrowException_WhenValueIsNonPositive()
        {
            var musician = CreateMockMusician();
            var musicAlbum = CreateMockMusicAlbum();
            var music = new Music(1, "Description 1", musician, musicAlbum);
            Assert.Throws<ArgumentException>(() => music.MusicId = 0);
        }

        [Fact]
        public void MusicId_ShouldReturnCorrectValue()
        {
            var musician = CreateMockMusician();
            var musicAlbum = CreateMockMusicAlbum();
            var music = new Music(1, "Description 1", musician, musicAlbum);
            Assert.Equal(1, music.MusicId);
        }

        [Fact]
        public void Description_ShouldThrowException_WhenValueIsEmpty()
        {
            var musician = CreateMockMusician();
            var musicAlbum = CreateMockMusicAlbum();
            var music = new Music(1, "Description 1", musician, musicAlbum);
            Assert.Throws<ArgumentException>(() => music.Description = "");
        }

        [Fact]
        public void Description_ShouldReturnCorrectValue()
        {
            var musician = CreateMockMusician();
            var musicAlbum = CreateMockMusicAlbum();
            var music = new Music(1, "Description 1", musician, musicAlbum);
            Assert.Equal("Description 1", music.Description);
        }

        [Fact]
        public void Musician_ShouldThrowException_WhenValueIsNull()
        {
            var musicAlbum = CreateMockMusicAlbum();
            Assert.Throws<ArgumentException>(() => new Music(1, "Description", null, musicAlbum));
        }

        [Fact]
        public void MusicAlbum_ShouldThrowException_WhenValueIsNull()
        {
            var musician = CreateMockMusician();
            Assert.Throws<ArgumentException>(() => new Music(1, "Description", musician, null));
        }

        [Fact]
        public void AddMusic_ShouldThrowException_WhenMusicIsNull()
        {
            Assert.Throws<ArgumentException>(() => Music.AddMusic(null));
        }

        [Fact]
        public void AddMusic_ShouldAddMusicCorrectly()
        {
            var musician = CreateMockMusician();
            var musicAlbum = CreateMockMusicAlbum();
            var music = new Music(1, "Description 1", musician, musicAlbum);
            Music.AddMusic(music);
            Assert.Contains(music, Music.GetMusicList());
        }

        [Fact]
        public void GetMusicList_ShouldReturnCorrectList()
        {
            var musician = CreateMockMusician();
            var musicAlbum = CreateMockMusicAlbum();
            var music = new Music(1, "Description 1", musician, musicAlbum);
            Music.AddMusic(music);
            Assert.Contains(music, Music.GetMusicList());
        }

        [Fact]
        public void SaveAndLoadMusic_ShouldPersistDataCorrectly()
        {
            var musician = CreateMockMusician();
            var musicAlbum = CreateMockMusicAlbum();
            var music1 = new Music(1, "Description 1", musician, musicAlbum);
            var music2 = new Music(2, "Description 2", musician, musicAlbum);

            Music.SaveMusic();
            Music.LoadMusic();

            var musicList = Music.GetMusicList();
            Assert.Equal(2, musicList.Count);
            Assert.Equal(1, musicList[0].MusicId);
            Assert.Equal("Description 1", musicList[0].Description);
            Assert.Equal(2, musicList[1].MusicId);
            Assert.Equal("Description 2", musicList[1].Description);
        }
    }
}
