using Xunit;

namespace BYTProject.UnitTesting;

public class PostTests
{
    [Fact]
    public void PostID_ShouldThrowException_WhenValueIsNonPositive()
    {
        var post = new Post(1, "Caption 1", DateTime.Now);
        Assert.Throws<ArgumentException>(() => post.PostId = 0);
    }

    [Fact]
    public void Caption_ShouldThrowException_WhenValueIsEmpty()
    {
        var post = new Post(1, "Caption 1", DateTime.Now);
        Assert.Throws<ArgumentException>(() => post.Caption = "");
    }
    
    [Fact]
    public void AddPost_ShouldThrowException_WhenPostIsNull()
    {
        Assert.Throws<ArgumentException>(() => Post.AddPost(null));
    }

    [Fact]
    public void PostConstructor_ShouldThrowException_WhenCaptionIsNotProvided()
    {
        Assert.Throws<ArgumentException>(() => new Post(1, null, DateTime.Now));
    }

    [Fact]
    public void SaveAndLoadPosts_ShouldPersistDataCorrectly()
    {
        // Arrange
        var post1 = new Post(1, "Caption 1", DateTime.Now);
        var post2 = new Post(2, "Caption 2", DateTime.Now);

        // Act
        Post.SavePosts();
        Post.LoadPosts();

        // Assert
        var posts = Post.GetPosts();
        Assert.Equal(2, posts.Count);
        Assert.Equal(1, posts[0].PostId);
        Assert.Equal("Caption 1", posts[0].Caption);
        Assert.Equal(2, posts[1].PostId);
        Assert.Equal("Caption 2", posts[1].Caption);
    }
}