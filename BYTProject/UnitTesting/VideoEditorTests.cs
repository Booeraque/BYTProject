using BYTProject.Models;
using Xunit;
using System;

namespace BYTProject.UnitTesting
{
    public class VideoEditorTests
    {
        public VideoEditorTests()
        {
            // Clear video editors before each test
            VideoEditor.ClearVideoEditors();
        }
        [Fact]
        public void VideoEdId_ShouldThrowException_WhenValueIsNonPositive()
        {
            Assert.Throws<ArgumentException>(() => new VideoEditor(0, "Test Bio", 1));
        }

        [Fact]
        public void VideoEdId_ShouldReturnCorrectValue()
        {
            var editor = new VideoEditor(1, "Test Bio", 1);
            Assert.Equal(1, editor.VideoEdId);
        }

        [Fact]
        public void VideoEdBio_ShouldThrowException_WhenValueIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => new VideoEditor(1, "", 1));
        }

        [Fact]
        public void VideoEdBio_ShouldReturnCorrectValue()
        {
            var editor = new VideoEditor(1, "Test Bio", 1);
            Assert.Equal("Test Bio", editor.VideoEdBio);
        }

        [Fact]
        public void AccountId_ShouldThrowException_WhenValueIsNonPositive()
        {
            Assert.Throws<ArgumentException>(() => new VideoEditor(1, "Test Bio", 0));
        }

        [Fact]
        public void AccountId_ShouldReturnCorrectValue()
        {
            var editor = new VideoEditor(1, "Test Bio", 1);
            Assert.Equal(1, editor.AccountId);
        }

        [Fact]
        public void AddVideoEditor_ShouldThrowException_WhenVideoEditorIsNull()
        {
            Assert.Throws<ArgumentException>(() => VideoEditor.AddVideoEditor(null));
        }

        [Fact]
        public void AddVideoEditor_ShouldAddEditorToExtent()
        {
            var editor = new VideoEditor(1, "Test Bio", 1);
            var editors = VideoEditor.GetVideoEditors();
            Assert.Contains(editor, editors);
        }

        [Fact]
        public void GetVideoEditors_ShouldReturnCorrectExtent()
        {
            var editor1 = new VideoEditor(1, "Bio1", 100);
            var editor2 = new VideoEditor(2, "Bio2", 101);

            var editors = VideoEditor.GetVideoEditors();
            Assert.Contains(editor1, editors);
            Assert.Contains(editor2, editors);
        }

        [Fact]
        public void SaveAndLoadVideoEditors_ShouldPersistExtentCorrectly()
        {
            VideoEditor.ClearVideoEditors();
            VideoEditor.ClearVideoEditors();
            var editor1 = new VideoEditor(1, "Bio1", 100);
            var editor2 = new VideoEditor(2, "Bio2", 101);

            VideoEditor.SaveVideoEditors();
            VideoEditor.LoadVideoEditors();

            var editors = VideoEditor.GetVideoEditors();
            Assert.Equal(2, editors.Count);
            Assert.Contains(editors, e => e.VideoEdId == 1 && e.VideoEdBio == "Bio1" && e.AccountId == 100);
            Assert.Contains(editors, e => e.VideoEdId == 2 && e.VideoEdBio == "Bio2" && e.AccountId == 101);
        }
        
        
        [Fact]
        public void AddVideo_ShouldAssociateVideoWithEditor()
        {
            var editor = new VideoEditor(1, "Editor Bio", 100);
            var video = new Video(1, "Test Video", editor);

            editor.AddVideo(video);

            Assert.Contains(video, editor.GetVideos());
            Assert.Equal(editor, video.VideoEditor);
        }

        [Fact]
        public void RemoveVideo_ShouldDisassociateVideoFromEditor()
        {
            var videoEditor = new VideoEditor(1, "Editor Bio", 1);
            var video = new Video(1, "Test Video", videoEditor);

            videoEditor.AddVideo(video); // Ensure the video is added
            videoEditor.RemoveVideo(video); // Now remove it

            Assert.DoesNotContain(video, videoEditor.UploadedVideos);
            Assert.Null(video.VideoEditor);
        }

        [Fact]
        public void AddVideo_ShouldThrowException_WhenVideoIsNull()
        {
            var editor = new VideoEditor(1, "Editor Bio", 100);
            Assert.Throws<ArgumentNullException>(() => editor.AddVideo(null));
        }

        [Fact]
        public void RemoveVideo_ShouldThrowException_WhenVideoIsNotAssociated()
        {
            var editor = new VideoEditor(1, "Editor Bio", 100);
            var video = new Video(1, "Test Video", new VideoEditor(2, "Other Editor", 101));

            Assert.Throws<InvalidOperationException>(() => editor.RemoveVideo(video));
        }

        [Fact]
        public void Videos_ShouldNotContainDuplicates()
        {
            var editor = new VideoEditor(1, "Editor Bio", 100);
            var video = new Video(1, "Test Video", editor);

            editor.AddVideo(video);
            Assert.Throws<InvalidOperationException>(() => editor.AddVideo(video));
        }

        [Fact]
        public void SetVideoEditor_ShouldReassignVideoToNewEditor()
        {
            var editor1 = new VideoEditor(1, "Editor 1 Bio", 100);
            var editor2 = new VideoEditor(2, "Editor 2 Bio", 101);
            var video = new Video(1, "Test Video", editor1);

            editor1.AddVideo(video);
            editor2.AddVideo(video);

            Assert.DoesNotContain(video, editor1.GetVideos());
            Assert.Contains(video, editor2.GetVideos());
            Assert.Equal(editor2, video.VideoEditor);
        }
        
    }
}
