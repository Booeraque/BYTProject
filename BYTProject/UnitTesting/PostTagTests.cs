using BYTProject.Models;
using Xunit;
using System;

namespace BYTProject.UnitTesting
{
    public class PostTagTests
    {
        public PostTagTests()
        {
            // Clear post tags and tags before each test
            PostTag.ClearPostTags();
            Tag.ClearTags();
            Post.ClearPosts();
        }

        [Fact]
        public void AddedAt_ShouldThrowException_WhenValueIsInFuture()
        {
            Assert.Throws<ArgumentException>(() => new PostTag(DateTime.Now.AddMinutes(10)));
        }

        [Fact]
        public void AddedAt_ShouldReturnCorrectValue()
        {
            var addedAt = DateTime.Now.AddMinutes(-10);
            var postTag = new PostTag(addedAt);
            Assert.Equal(addedAt, postTag.AddedAt);
        }

        [Fact]
        public void SetTag_ShouldAssignTagToPostTag()
        {
            var tag = new Tag(1, new List<string> { "Category1" });
            var postTag = new PostTag(DateTime.Now);

            postTag.SetTag(tag);

            Assert.Equal(tag, postTag.Tag);
            Assert.Contains(postTag, tag.PostTags); // Reverse connection
        }

        [Fact]
        public void RemoveTag_ShouldDisassociateTagFromPostTag()
        {
            var tag = new Tag(1, new List<string> { "Category1" });
            var postTag = new PostTag(DateTime.Now);

            postTag.SetTag(tag);
            postTag.RemoveTag();

            Assert.Null(postTag.Tag); // Verify PostTag no longer references Tag
            Assert.DoesNotContain(postTag, tag.PostTags); // Verify Tag no longer contains PostTag
        }


        [Fact]
        public void AddPost_ShouldAssociatePostWithPostTag()
        {
            var postTag = new PostTag(DateTime.Now);
            var post = new Post(1, "Post Caption", DateTime.Now);

            postTag.AddPost(post);

            Assert.Contains(post, postTag.Posts);
        }

        [Fact]
        public void AddPost_ShouldThrowException_WhenPostIsNull()
        {
            var postTag = new PostTag(DateTime.Now);
            Assert.Throws<ArgumentNullException>(() => postTag.AddPost(null));
        }

        [Fact]
        public void AddPost_ShouldThrowException_WhenPostAlreadyAssociated()
        {
            var postTag = new PostTag(DateTime.Now);
            var post = new Post(1, "Post Caption", DateTime.Now);

            postTag.AddPost(post);
            var exception = Assert.Throws<InvalidOperationException>(() => postTag.AddPost(post));

            Assert.Equal("Post is already associated with this tag.", exception.Message);
        }

        [Fact]
        public void RemovePost_ShouldDisassociatePostFromPostTag()
        {
            var postTag = new PostTag(DateTime.Now);
            var post = new Post(1, "Post Caption", DateTime.Now);

            postTag.AddPost(post);
            postTag.RemovePost(post);

            Assert.DoesNotContain(post, postTag.Posts);
        }

        [Fact]
        public void RemovePost_ShouldDoNothing_WhenPostNotAssociated()
        {
            var postTag = new PostTag(DateTime.Now);
            var post = new Post(1, "Post Caption", DateTime.Now);

            postTag.RemovePost(post); // Should not throw an error
            Assert.Empty(postTag.Posts);
        }

        [Fact]
        public void GetPostTags_ShouldReturnCorrectExtent()
        {
            var postTag1 = new PostTag(DateTime.Now.AddMinutes(-5));
            var postTag2 = new PostTag(DateTime.Now.AddMinutes(-10));

            var postTags = PostTag.GetPostTags();
            Assert.Contains(postTag1, postTags);
            Assert.Contains(postTag2, postTags);
        }

        [Fact]
        public void SaveAndLoadPostTags_ShouldPersistExtentCorrectly()
        {
            var postTag1 = new PostTag(DateTime.Now.AddMinutes(-5));
            var postTag2 = new PostTag(DateTime.Now.AddMinutes(-10));

            PostTag.SavePostTags();
            PostTag.ClearPostTags();
            PostTag.LoadPostTags();

            var postTags = PostTag.GetPostTags();
            Assert.Equal(2, postTags.Count);
            Assert.Contains(postTags, pt => pt.AddedAt == postTag1.AddedAt);
            Assert.Contains(postTags, pt => pt.AddedAt == postTag2.AddedAt);
        }
    }
}
