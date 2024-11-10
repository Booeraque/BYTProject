using Xunit;
using System;

namespace BYTProject.UnitTesting
{
    public class PostTests
    {
        [Fact]
        public void PostID_ShouldThrowException_WhenValueIsNonPositive()
        {
            var post = new Post(1, "Caption 1", DateTime.Now);
            Assert.Throws<ArgumentException>(() => post.PostId = 0);
        }

        [Fact]
        public void PostID_ShouldReturnCorrectValue()
        {
            var post = new Post(1, "Caption 1", DateTime.Now);
            Assert.Equal(1, post.PostId);
        }

        [Fact]
        public void Caption_ShouldThrowException_WhenValueIsEmpty()
        {
            var post = new Post(1, "Caption 1", DateTime.Now);
            Assert.Throws<ArgumentException>(() => post.Caption = "");
        }

        [Fact]
        public void Caption_ShouldReturnCorrectValue()
        {
            var post = new Post(1, "Caption 1", DateTime.Now);
            Assert.Equal("Caption 1", post.Caption);
        }

        [Fact]
        public void CreatedAt_ShouldThrowException_WhenValueIsInTheFuture()
        {
            var post = new Post(1, "Caption 1", DateTime.Now);
            Assert.Throws<ArgumentException>(() => post.CreatedAt = DateTime.Now.AddDays(1));
        }

        [Fact]
        public void CreatedAt_ShouldReturnCorrectValue()
        {
            var createdAt = DateTime.Now;
            var post = new Post(1, "Caption 1", createdAt);
            Assert.Equal(createdAt, post.CreatedAt);
        }

        [Fact]
        public void AddPost_ShouldThrowException_WhenPostIsNull()
        {
            Assert.Throws<ArgumentException>(() => Post.AddPost(null));
        }

        [Fact]
        public void AddPost_ShouldAddPostCorrectly()
        {
            var post = new Post(1, "Caption 1", DateTime.Now);
            Post.AddPost(post);
            Assert.Contains(post, Post.GetPosts());
        }

        [Fact]
        public void GetPosts_ShouldReturnCorrectList()
        {
            var post = new Post(1, "Caption 1", DateTime.Now);
            var posts = Post.GetPosts();
            Assert.Contains(post, posts);
        }

        [Fact]
        public void SaveAndLoadPosts_ShouldPersistDataCorrectly()
        {
            var post1 = new Post(1, "Caption 1", DateTime.Now);
            var post2 = new Post(2, "Caption 2", DateTime.Now);

            Post.SavePosts();
            Post.LoadPosts();

            var posts = Post.GetPosts();
            Assert.Equal(2, posts.Count);
            Assert.Equal(1, posts[0].PostId);
            Assert.Equal("Caption 1", posts[0].Caption);
            Assert.Equal(2, posts[1].PostId);
            Assert.Equal("Caption 2", posts[1].Caption);
        }
    }
}
