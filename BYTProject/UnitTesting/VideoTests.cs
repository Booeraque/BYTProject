using BYTProject.Models;
using Xunit;
using System;

namespace BYTProject.UnitTesting
{
    public class VideoTests
    {
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
    }
}
