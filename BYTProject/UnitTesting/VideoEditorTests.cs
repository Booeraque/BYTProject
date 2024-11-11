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
    }
}
