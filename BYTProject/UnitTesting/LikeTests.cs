using Xunit;
using System;
using BYTProject.Models;

namespace BYTProject.UnitTesting
{
    public class LikeTests
    {
        public LikeTests()
        {
            // Clear likes before each test
            Like.ClearLikes();
        }
        
        [Fact]
        public void LikeID_ShouldThrowException_WhenValueIsNonPositive()
        {
            var like = new Like(1, DateTime.Now);
            Assert.Throws<ArgumentException>(() => like.LikeId = 0);
        }

        [Fact]
        public void LikeID_ShouldReturnCorrectValue()
        {
            var like = new Like(1, DateTime.Now);
            Assert.Equal(1, like.LikeId);
        }

        [Fact]
        public void CreatedAt_ShouldThrowException_WhenValueIsInTheFuture()
        {
            var like = new Like(1, DateTime.Now);
            Assert.Throws<ArgumentException>(() => like.CreatedAt = DateTime.Now.AddDays(1));
        }

        [Fact]
        public void CreatedAt_ShouldReturnCorrectValue()
        {
            var createdAt = DateTime.Now;
            var like = new Like(1, createdAt);
            Assert.Equal(createdAt, like.CreatedAt);
        }

        [Fact]
        public void AddLike_ShouldThrowException_WhenLikeIsNull()
        {
            Assert.Throws<ArgumentException>(() => Like.AddLike(null));
        }

        [Fact]
        public void AddLike_ShouldAddLikeCorrectly()
        {
            var like = new Like(1, DateTime.Now);
            Like.AddLike(like);
            Assert.Contains(like, Like.GetLikes());
        }

        [Fact]
        public void GetLikes_ShouldReturnCorrectList()
        {
            var like = new Like(1, DateTime.Now);
            var likes = Like.GetLikes();
            Assert.Contains(like, likes);
        }

        [Fact]
        public void SaveAndLoadLikes_ShouldPersistDataCorrectly()
        {
            var like1 = new Like(1, DateTime.Now);
            var like2 = new Like(2, DateTime.Now);

            Like.SaveLikes();
            Like.LoadLikes();

            var likes = Like.GetLikes();
            Assert.Equal(2, likes.Count);
            Assert.Equal(1, likes[0].LikeId);
            Assert.Equal(2, likes[1].LikeId);
        }
    }
}
