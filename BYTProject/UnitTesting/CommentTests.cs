using BYTProject.Models;
using Xunit;

namespace BYTProject.UnitTesting;

public class CommentTests
{
    [Fact]
    public void CommentID_ShouldThrowException_WhenValueIsNonPositive()
    {
        var comment = new Comment(1, "Content 1", DateTime.Now);
        Assert.Throws<ArgumentException>(() => comment.CommentId = 0);
    }

    [Fact]
    public void Content_ShouldThrowException_WhenValueIsEmpty()
    {
        var comment = new Comment(1, "Content 1", DateTime.Now);
        Assert.Throws<ArgumentException>(() => comment.Content = "");
    }

    [Fact]
    public void CreatedAt_ShouldThrowException_WhenValueIsInTheFuture()
    {
        var comment = new Comment(1, "Content 1", DateTime.Now);
        Assert.Throws<ArgumentException>(() => comment.CreatedAt = DateTime.Now.AddDays(1));
    }
        
    [Fact]
    public void AddComment_ShouldThrowException_WhenCommentIsNull()
    {
        Assert.Throws<ArgumentException>(() => Comment.AddComment(null));
    }

    [Fact]
    public void CommentConstructor_ShouldThrowException_WhenContentIsNotProvided()
    {
        Assert.Throws<ArgumentException>(() => new Comment(1, null, DateTime.Now));
    }

    [Fact]
    public void SaveAndLoadComments_ShouldPersistDataCorrectly()
    {
        // Arrange
        var comment1 = new Comment(1, "Content 1", DateTime.Now);
        var comment2 = new Comment(2, "Content 2", DateTime.Now);

        // Act
        Comment.SaveComments();
        Comment.LoadComments();

        // Assert
        var comments = Comment.GetComments();
        Assert.Equal(2, comments.Count);
        Assert.Equal(1, comments[0].CommentId);
        Assert.Equal("Content 1", comments[0].Content);
        Assert.Equal(2, comments[1].CommentId);
        Assert.Equal("Content 2", comments[1].Content);
    }
        
}