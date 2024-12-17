using Xunit;
using System;
using BYTProject.Models;

namespace BYTProject.UnitTesting
{
    public class GroupTests
    {
        public GroupTests()
        {
            // Clear groups and media before each test
            Group.ClearGroups();
            Media.ClearMedia();
        }
        
        [Fact]
        public void GroupID_ShouldThrowException_WhenValueIsNonPositive()
        {
            var group = new Group(1, "Group 1", "Sample Description");
            Assert.Throws<ArgumentException>(() => group.GroupId = 0);
        }

        [Fact]
        public void GroupID_ShouldReturnCorrectValue()
        {
            var group = new Group(1, "Group 1", "Sample Description");
            Assert.Equal(1, group.GroupId);
        }

        [Fact]
        public void GroupName_ShouldThrowException_WhenValueIsEmpty()
        {
            var group = new Group(1, "Group 1", "Sample Description");
            Assert.Throws<ArgumentException>(() => group.GroupName = "");
        }

        [Fact]
        public void GroupName_ShouldReturnCorrectValue()
        {
            var group = new Group(1, "Group 1", "Sample Description");
            Assert.Equal("Group 1", group.GroupName);
        }

        [Fact]
        public void Description_ShouldThrowException_WhenValueIsEmpty()
        {
            var group = new Group(1, "Group 1", "Sample Description");
            Assert.Throws<ArgumentException>(() => group.Description = "");
        }

        [Fact]
        public void Description_ShouldReturnCorrectValue()
        {
            var group = new Group(1, "Group 1", "Sample Description");
            Assert.Equal("Sample Description", group.Description);
        }

        [Fact]
        public void AddGroup_ShouldThrowException_WhenGroupIsNull()
        {
            Assert.Throws<ArgumentException>(() => Group.AddGroup(null));
        }

        [Fact]
        public void AddGroup_ShouldAddGroupCorrectly()
        {
            var group = new Group(1, "Group 1", "Sample Description");
            Group.AddGroup(group);
            Assert.Contains(group, Group.GetGroups());
        }

        [Fact]
        public void AddMedia_ShouldAddMediaCorrectly()
        {
            var group = new Group(1, "Group 1", "Sample Description");
            var media = new Media(1, "Video");

            group.AddMedia(media);

            Assert.Contains(media, group.MediaList);
            Assert.Equal(group, media.Group);
        }

        [Fact]
        public void AddMedia_ShouldThrowException_WhenMediaIsNull()
        {
            var group = new Group(1, "Group 1", "Sample Description");
            Assert.Throws<ArgumentNullException>(() => group.AddMedia(null));
        }

        [Fact]
        public void AddMedia_ShouldThrowException_WhenMediaIsAlreadyAdded()
        {
            var group = new Group(1, "Group 1", "Sample Description");
            var media = new Media(1, "Video");

            group.AddMedia(media);
            Assert.Throws<InvalidOperationException>(() => group.AddMedia(media));
        }

        [Fact]
        public void RemoveMedia_ShouldRemoveMediaCorrectly()
        {
            var group = new Group(1, "Group 1", "Sample Description");
            var media = new Media(1, "Video");

            group.AddMedia(media);
            group.RemoveMedia(media);

            Assert.DoesNotContain(media, group.MediaList);
            Assert.Null(media.Group);
        }

        [Fact]
        public void RemoveMedia_ShouldThrowException_WhenMediaIsNull()
        {
            var group = new Group(1, "Group 1", "Sample Description");
            Assert.Throws<ArgumentNullException>(() => group.RemoveMedia(null));
        }

        [Fact]
        public void SaveAndLoadGroups_ShouldPersistDataCorrectly()
        {
            Group.ClearGroups();
            var group1 = new Group(1, "Group 1", "Description 1");
            var group2 = new Group(2, "Group 2", "Description 2");

            Group.SaveGroups();
            Group.ClearGroups();
            Group.LoadGroups();

            var groups = Group.GetGroups();
            Assert.Equal(2, groups.Count);
            Assert.Equal(1, groups[0].GroupId);
            Assert.Equal("Group 1", groups[0].GroupName);
            Assert.Equal("Description 1", groups[0].Description);
            Assert.Equal(2, groups[1].GroupId);
            Assert.Equal("Group 2", groups[1].GroupName);
            Assert.Equal("Description 2", groups[1].Description);
        }
    }
}
