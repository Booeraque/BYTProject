using BYTProject.Models;
using Xunit;

namespace BYTProject.UnitTesting;

public class LikeTests
{
    [Fact]
    public void LikeID_ShouldThrowException_WhenValueIsNonPositive()
    {
        var like = new Like(1, DateTime.Now);
        Assert.Throws<ArgumentException>(() => like.LikeId = 0);
    }

    [Fact]
    public void CreatedAt_ShouldThrowException_WhenValueIsInTheFuture()
    {
        var like = new Like(1, DateTime.Now);
        Assert.Throws<ArgumentException>(() => like.CreatedAt = DateTime.Now.AddDays(1));
    }
    
    [Fact]
    public void AddLike_ShouldThrowException_WhenLikeIsNull()
    {
        Assert.Throws<ArgumentException>(() => Like.AddLike(null));
    }

    [Fact]
    public void SaveAndLoadLikes_ShouldPersistDataCorrectly()
    {
        // Arrange
        var like1 = new Like(1, DateTime.Now);
        var like2 = new Like(2, DateTime.Now);

        // Act
        Like.SaveLikes();
        Like.LoadLikes();

        // Assert
        var likes = Like.GetLikes();
        Assert.Equal(2, likes.Count);
        Assert.Equal(1, likes[0].LikeId);
        Assert.Equal(2, likes[1].LikeId);
    }
    
}