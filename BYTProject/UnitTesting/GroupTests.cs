using Xunit;
using System;
using BYTProject.Models;

namespace BYTProject.UnitTesting
{
    public class GroupTests
    {
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
            Group.ClearGroups();
            var group = new Group(1, "Group 1", "Sample Description");
            Group.AddGroup(group);
            Assert.Contains(group, Group.GetGroups());
        }

        [Fact]
        public void GetGroups_ShouldReturnCorrectList()
        {
            Group.ClearGroups();
            var group = new Group(1, "Group 1", "Sample Description");
            var groups = Group.GetGroups();
            Assert.Contains(group, groups);
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
