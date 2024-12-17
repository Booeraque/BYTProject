using BYTProject.Models;
using Xunit;
using System;
using System.Collections.Generic;

namespace BYTProject.UnitTesting
{
    public class PostTagTests
    {
        public PostTagTests()
        {
            // Clear post tags and tags before each test
            PostTag.ClearPostTags();
            Tag.ClearTags();
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
        public void AddTag_ShouldAssociateTagCorrectly()
        {
            var postTag = new PostTag(DateTime.Now);
            var tag = new Tag(1);

            postTag.AddTag(tag);

            Assert.Contains(tag, postTag.Tags); // Forward association
            Assert.Contains(postTag, tag.PostTags); // Reverse association
        }

        [Fact]
        public void AddTag_ShouldThrowException_WhenTagIsNull()
        {
            var postTag = new PostTag(DateTime.Now);
            Assert.Throws<ArgumentNullException>(() => postTag.AddTag(null));
        }

        [Fact]
        public void AddTag_ShouldThrowException_WhenTagAlreadyAdded()
        {
            var postTag = new PostTag(DateTime.Now);
            var tag = new Tag(1);

            postTag.AddTag(tag);
            Assert.Throws<InvalidOperationException>(() => postTag.AddTag(tag));
        }

        [Fact]
        public void RemoveTag_ShouldDisassociateTagCorrectly()
        {
            var postTag = new PostTag(DateTime.Now);
            var tag = new Tag(1);

            postTag.AddTag(tag);
            postTag.RemoveTag(tag);

            Assert.DoesNotContain(tag, postTag.Tags); // Forward association cleared
            Assert.DoesNotContain(postTag, tag.PostTags); // Reverse association cleared
        }

        [Fact]
        public void RemoveTag_ShouldThrowException_WhenTagIsNull()
        {
            var postTag = new PostTag(DateTime.Now);
            Assert.Throws<ArgumentNullException>(() => postTag.RemoveTag(null));
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
            PostTag.LoadPostTags();

            var postTags = PostTag.GetPostTags();
            Assert.Equal(2, postTags.Count);
            Assert.Contains(postTags, pt => pt.AddedAt == postTag1.AddedAt);
            Assert.Contains(postTags, pt => pt.AddedAt == postTag2.AddedAt);
        }
    }
}
