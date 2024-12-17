using BYTProject.Models;
using Xunit;
using System;

namespace BYTProject.UnitTesting
{
    public class VideoTests
    {
        public VideoTests()
        {
            // Clear videos before each test
            Video.ClearVideos();
        }
        
        private VideoEditor CreateMockVideoEditor()
        {
            return new VideoEditor(1, "Editor Bio", 1);
        }

        [Fact]
        public void VideoId_ShouldThrowException_WhenValueIsNonPositive()
        {
            var editor = CreateMockVideoEditor();
            Assert.Throws<ArgumentException>(() => new Video(0, "Test Description", editor));
        }

        [Fact]
        public void VideoId_ShouldReturnCorrectValue()
        {
            var editor = CreateMockVideoEditor();
            var video = new Video(1, "Test Description", editor);
            Assert.Equal(1, video.VideoId);
        }

        [Fact]
        public void Description_ShouldThrowException_WhenValueIsEmpty()
        {
            var editor = CreateMockVideoEditor();
            Assert.Throws<ArgumentException>(() => new Video(1, "", editor));
        }

        [Fact]
        public void Description_ShouldReturnCorrectValue()
        {
            var editor = CreateMockVideoEditor();
            var video = new Video(1, "Test Description", editor);
            Assert.Equal("Test Description", video.Description);
        }

        [Fact]
        public void VideoEditor_ShouldThrowException_WhenValueIsNull()
        {
            Assert.Throws<ArgumentException>(() => new Video(1, "Test Description", null));
        }

        [Fact]
        public void VideoEditor_ShouldReturnCorrectValue()
        {
            var editor = CreateMockVideoEditor();
            var video = new Video(1, "Test Description", editor);
            Assert.Equal(editor, video.VideoEditor);
        }

        [Fact]
        public void AddVideo_ShouldThrowException_WhenVideoIsNull()
        {
            Assert.Throws<ArgumentException>(() => Video.AddVideo(null));
        }

        [Fact]
        public void AddVideo_ShouldAddVideoToExtent()
        {
            var editor = CreateMockVideoEditor();
            var video = new Video(1, "Test Description", editor);
            var videos = Video.GetVideoList();
            Assert.Contains(video, videos);
        }

        [Fact]
        public void GetVideoList_ShouldReturnCorrectExtent()
        {
            var editor = CreateMockVideoEditor();
            var video1 = new Video(1, "Description 1", editor);
            var video2 = new Video(2, "Description 2", editor);

            var videos = Video.GetVideoList();
            Assert.Contains(video1, videos);
            Assert.Contains(video2, videos);
        }

        [Fact]
        public void SaveAndLoadVideos_ShouldPersistExtentCorrectly()
        {
            Video.ClearVideos();
            var editor = CreateMockVideoEditor();
            var video1 = new Video(1, "Description 1", editor);
            var video2 = new Video(2, "Description 2", editor);

            Video.SaveVideos();
            Video.LoadVideos();

            var videos = Video.GetVideoList();
            Assert.Equal(2, videos.Count);
            Assert.Contains(videos, v => v.VideoId == 1 && v.Description == "Description 1");
            Assert.Contains(videos, v => v.VideoId == 2 && v.Description == "Description 2");
        }
        
        // New Tests for Associations
        
        [Fact]
        public void SetVideoEditor_ShouldUpdateVideoEditorCorrectly()
        {
            // Arrange
            var editor1 = new VideoEditor(1, "Editor 1 Bio", 1);
            var editor2 = new VideoEditor(2, "Editor 2 Bio", 2);
            var video = new Video(1, "Test Video", editor1);

            editor1.AddVideo(video); // Establish the association properly

            // Act
            video.SetVideoEditor(editor2);

            // Assert
            Assert.Equal(editor2, video.VideoEditor); // Video should now be associated with editor2
            Assert.DoesNotContain(video, editor1.UploadedVideos); // Video should no longer be in editor1's list
            Assert.Contains(video, editor2.UploadedVideos); // Video should now be in editor2's list
        }


        [Fact]
        public void SetVideoEditor_ShouldThrowException_WhenVideoEditorIsNull()
        {
            var editor = CreateMockVideoEditor();
            var video = new Video(1, "Test Video", editor);

            Assert.Throws<ArgumentNullException>(() => video.SetVideoEditor(null));
        }

        [Fact]
        public void RemoveVideoEditor_ShouldClearVideoEditor()
        {
            var editor = CreateMockVideoEditor();
            var video = new Video(1, "Test Video", editor);

            video.RemoveVideoEditor();

            Assert.Null(video.VideoEditor);
            Assert.DoesNotContain(video, editor.UploadedVideos);
        }

        [Fact]
        public void RemoveVideoEditor_ShouldHandleNoEditorGracefully()
        {
            var video = new Video(1, "Test Video", CreateMockVideoEditor());

            video.RemoveVideoEditor(); // First removal

            // Calling again to check idempotence
            video.RemoveVideoEditor();

            Assert.Null(video.VideoEditor);
        }

        [Fact]
        public void AddVideo_ToVideoEditor_ShouldMaintainBidirectionalAssociation()
        {
            var editor = CreateMockVideoEditor();
            var video = new Video(1, "Test Video", editor);

            editor.AddVideo(video);

            Assert.Contains(video, editor.UploadedVideos);
            Assert.Equal(editor, video.VideoEditor);
        }

        [Fact]
        public void RemoveVideo_FromVideoEditor_ShouldMaintainBidirectionalAssociation()
        {
            // Arrange
            var editor = new VideoEditor(1, "Editor Bio", 1);
            var video = new Video(1, "Test Video", editor);

            editor.AddVideo(video); // Explicitly add the video to the editor

            // Act
            editor.RemoveVideo(video); // Remove the video

            // Assert
            Assert.DoesNotContain(video, editor.UploadedVideos); // Editor should no longer contain the video
            Assert.Null(video.VideoEditor); // Video should no longer have an associated editor
        }

    }
}
