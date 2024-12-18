using BYTProject.Models;
using Xunit;
using System;

namespace BYTProject.UnitTesting
{
    public class MediaTests
    {
        public MediaTests()
        {
            // Clear media and posts before each test
            Media.ClearMedia();
            Post.ClearPosts();
        }

        [Fact]
        public void MediaID_ShouldThrowException_WhenValueIsNonPositive()
        {
            var media = new Media(1, "Video");
            Assert.Throws<ArgumentException>(() => media.MediaId = 0);
        }

        [Fact]
        public void MediaID_ShouldReturnCorrectValue()
        {
            var media = new Media(1, "Video");
            Assert.Equal(1, media.MediaId);
        }

        [Fact]
        public void MediaType_ShouldThrowException_WhenValueIsEmpty()
        {
            var media = new Media(1, "Video");
            Assert.Throws<ArgumentException>(() => media.MediaType = "");
        }

        [Fact]
        public void MediaType_ShouldReturnCorrectValue()
        {
            var media = new Media(1, "Video");
            Assert.Equal("Video", media.MediaType);
        }

        [Fact]
        public void AddMedia_ShouldThrowException_WhenMediaIsNull()
        {
            Assert.Throws<ArgumentException>(() => Media.AddMedia(null));
        }

        [Fact]
        public void AddMedia_ShouldAddMediaCorrectly()
        {
            var media = new Media(1, "Video");
            Media.AddMedia(media);
            Assert.Contains(media, Media.GetMediaList());
        }

        [Fact]
        public void GetMediaList_ShouldReturnCorrectList()
        {
            var media = new Media(1, "Video");
            Media.AddMedia(media);
            var mediaList = Media.GetMediaList();
            Assert.Contains(media, mediaList);
        }

        [Fact]
        public void SetPost_ShouldAssignPostToMedia()
        {
            var media = new Media(1, "Video");
            var post = new Post(1, "Caption", DateTime.Now);

            media.SetPost(post);

            Assert.Equal(post, media.Post);
            Assert.Contains(media, post.MediaList); // Verify reverse connection
        }

        [Fact]
        public void RemovePost_ShouldDisassociatePostFromMedia()
        {
            var media = new Media(1, "Video");
            var post = new Post(1, "Caption", DateTime.Now);

            media.SetPost(post);
            media.RemovePost();

            Assert.Null(media.Post);
            Assert.DoesNotContain(media, post.MediaList); // Verify reverse connection is removed
        }

        [Fact]
        public void SetPost_ShouldUpdatePostAssociation()
        {
            var media = new Media(1, "Video");
            var post1 = new Post(1, "Caption 1", DateTime.Now);
            var post2 = new Post(2, "Caption 2", DateTime.Now);

            // Initial association
            media.SetPost(post1);
            Assert.Equal(post1, media.Post);
            Assert.Contains(media, post1.MediaList);

            // Update association
            media.SetPost(post2);
            Assert.Equal(post2, media.Post); // Verify new association
            Assert.Contains(media, post2.MediaList); // Reverse connection added
            Assert.DoesNotContain(media, post1.MediaList); // Reverse connection removed
        }

        [Fact]
        public void RemovePost_ShouldDoNothing_WhenPostNotAssociated()
        {
            var media = new Media(1, "Video");
            var post = new Post(1, "Caption", DateTime.Now);

            media.RemovePost(); // Should not throw an exception
            Assert.Null(media.Post); // Media has no association
            Assert.Empty(post.MediaList); // Post remains unaffected
        }

        [Fact]
        public void SaveAndLoadMedia_ShouldPersistDataCorrectly()
        {
            var media1 = new Media(1, "Video");
            var media2 = new Media(2, "Image");

            Media.SaveMedia();
            Media.ClearMedia();
            Media.LoadMedia();

            var mediaList = Media.GetMediaList();
            Assert.Equal(2, mediaList.Count);
            Assert.Equal(1, mediaList[0].MediaId);
            Assert.Equal("Video", mediaList[0].MediaType);
            Assert.Equal(2, mediaList[1].MediaId);
            Assert.Equal("Image", mediaList[1].MediaType);
        }
    }
}
