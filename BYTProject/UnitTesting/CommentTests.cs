using Xunit;
using System;
using BYTProject.Models;

namespace BYTProject.UnitTesting
{
    public class CommentTests
    {
        public CommentTests()
        {
            // Clear comments before each test
            Comment.ClearComments();
        }
        
        [Fact]
        public void CommentID_ShouldThrowException_WhenValueIsNonPositive()
        {
            var comment = new Comment(1, "Content 1", DateTime.Now);
            Assert.Throws<ArgumentException>(() => comment.CommentId = 0);
        }

        [Fact]
        public void CommentID_ShouldReturnCorrectValue()
        {
            var comment = new Comment(1, "Content 1", DateTime.Now);
            Assert.Equal(1, comment.CommentId);
        }

        [Fact]
        public void Content_ShouldThrowException_WhenValueIsEmpty()
        {
            var comment = new Comment(1, "Content 1", DateTime.Now);
            Assert.Throws<ArgumentException>(() => comment.Content = "");
        }

        [Fact]
        public void Content_ShouldReturnCorrectValue()
        {
            var comment = new Comment(1, "Content 1", DateTime.Now);
            Assert.Equal("Content 1", comment.Content);
        }

        [Fact]
        public void CreatedAt_ShouldThrowException_WhenValueIsInTheFuture()
        {
            var comment = new Comment(1, "Content 1", DateTime.Now);
            Assert.Throws<ArgumentException>(() => comment.CreatedAt = DateTime.Now.AddDays(1));
        }

        [Fact]
        public void CreatedAt_ShouldReturnCorrectValue()
        {
            var createdAt = DateTime.Now;
            var comment = new Comment(1, "Content 1", createdAt);
            Assert.Equal(createdAt, comment.CreatedAt);
        }

        [Fact]
        public void Edited_ShouldReturnCorrectValue()
        {
            var comment = new Comment(1, "Content 1", DateTime.Now, true);
            Assert.True(comment.Edited);
        }

        [Fact]
        public void AddComment_ShouldThrowException_WhenCommentIsNull()
        {
            Assert.Throws<ArgumentException>(() => Comment.AddComment(null));
        }

        [Fact]
        public void AddComment_ShouldAddCommentCorrectly()
        {
            var comment = new Comment(1, "Content 1", DateTime.Now);
            Comment.AddComment(comment);
            Assert.Contains(comment, Comment.GetComments());
        }

        [Fact]
        public void GetComments_ShouldReturnCorrectList()
        {
            var comment = new Comment(1, "Content 1", DateTime.Now);
            var comments = Comment.GetComments();
            Assert.Contains(comment, comments);
        }

        [Fact]
        public void SaveAndLoadComments_ShouldPersistDataCorrectly()
        {
            var comment1 = new Comment(1, "Content 1", DateTime.Now);
            var comment2 = new Comment(2, "Content 2", DateTime.Now);

            Comment.SaveComments();
            Comment.LoadComments();

            var comments = Comment.GetComments();
            Assert.Equal(2, comments.Count);
            Assert.Equal(1, comments[0].CommentId);
            Assert.Equal("Content 1", comments[0].Content);
            Assert.Equal(2, comments[1].CommentId);
            Assert.Equal("Content 2", comments[1].Content);
        }
    }
}
