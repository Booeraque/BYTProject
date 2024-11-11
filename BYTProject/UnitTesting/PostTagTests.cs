using BYTProject.Models;
using Xunit;
using System;
using System.Collections.Generic;

namespace BYTProject.UnitTesting
{
    public class PostTagTests
    {
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
        public void AddTag_ShouldThrowException_WhenPostTagIsNull()
        {
            Assert.Throws<ArgumentException>(() => PostTag.AddTag(null));
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