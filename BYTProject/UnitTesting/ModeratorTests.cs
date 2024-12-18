using Xunit;
using System;
using System.Collections.Generic;
using BYTProject.Models;

namespace BYTProject.UnitTesting
{
    public class ModeratorTests
    {
        public ModeratorTests()
        {
            // Clear all extents before each test
            Moderator.ClearModerators();
            Group.ClearGroups();
        }

        [Fact]
        public void AccountId_ShouldThrowException_WhenValueIsNonPositive()
        {
            Assert.Throws<ArgumentException>(() => new Moderator(0, DateTime.Now, new List<string> { "Right1" }));
        }

        [Fact]
        public void AccountId_ShouldReturnCorrectValue()
        {
            var moderator = new Moderator(1, DateTime.Now, new List<string> { "Right1" });
            Assert.Equal(1, moderator.AccountId);
        }

        [Fact]
        public void DateOfAssignment_ShouldThrowException_WhenValueIsInTheFuture()
        {
            var moderator = new Moderator(1, DateTime.Now, new List<string> { "Right1" });
            Assert.Throws<ArgumentException>(() => moderator.DateOfAssignment = DateTime.Now.AddDays(1));
        }

        [Fact]
        public void DateOfAssignment_ShouldReturnCorrectValue()
        {
            var dateOfAssignment = DateTime.Now;
            var moderator = new Moderator(1, dateOfAssignment, new List<string> { "Right1" });
            Assert.Equal(dateOfAssignment, moderator.DateOfAssignment);
        }

        [Fact]
        public void Rights_ShouldThrowException_WhenValueIsInvalid()
        {
            var moderator = new Moderator(1, DateTime.Now, new List<string> { "Right1" });
            Assert.Throws<ArgumentException>(() => moderator.Rights = null);
            Assert.Throws<ArgumentException>(() => moderator.Rights = new List<string>());
            Assert.Throws<ArgumentException>(() => moderator.Rights = new List<string> { "R1", "R2", "R3", "R4", "R5", "R6" });
        }

        [Fact]
        public void Rights_ShouldReturnCorrectValue()
        {
            var rights = new List<string> { "Right1", "Right2" };
            var moderator = new Moderator(1, DateTime.Now, rights);
            Assert.Equal(rights, moderator.Rights);
        }

        [Fact]
        public void AddGroup_ShouldAddGroupToModerator()
        {
            var moderator = new Moderator(1, DateTime.Now, new List<string> { "Right1" });
            var group = new Group(1, "Group 1", "Description 1");

            moderator.AddGroup(group);

            Assert.Contains(group, moderator.Groups);
        }

        [Fact]
        public void AddGroup_ShouldThrowException_WhenGroupIsNull()
        {
            var moderator = new Moderator(1, DateTime.Now, new List<string> { "Right1" });
            Assert.Throws<ArgumentNullException>(() => moderator.AddGroup(null));
        }

        [Fact]
        public void AddGroup_ShouldThrowException_WhenGroupAlreadyExists()
        {
            var moderator = new Moderator(1, DateTime.Now, new List<string> { "Right1" });
            var group = new Group(1, "Group 1", "Description 1");

            moderator.AddGroup(group);
            var exception = Assert.Throws<InvalidOperationException>(() => moderator.AddGroup(group));

            Assert.Equal("Group is already managed by this moderator.", exception.Message);
        }

        [Fact]
        public void RemoveGroup_ShouldRemoveGroupFromModerator()
        {
            var moderator = new Moderator(1, DateTime.Now, new List<string> { "Right1" });
            var group = new Group(1, "Group 1", "Description 1");

            moderator.AddGroup(group);
            moderator.RemoveGroup(group);

            Assert.DoesNotContain(group, moderator.Groups);
        }

        [Fact]
        public void RemoveGroup_ShouldDoNothing_WhenGroupNotManaged()
        {
            var moderator = new Moderator(1, DateTime.Now, new List<string> { "Right1" });
            var group = new Group(1, "Group 1", "Description 1");

            moderator.RemoveGroup(group); // Should not throw or fail
            Assert.Empty(moderator.Groups);
        }

       
        [Fact]
        public void SaveAndLoadModerators_ShouldPersistDataCorrectly()
        {
            var date1 = DateTime.Now.AddDays(-10);
            var date2 = DateTime.Now.AddDays(-5);
            var moderator1 = new Moderator(1, date1, new List<string> { "Right1", "Right2" });
            var moderator2 = new Moderator(2, date2, new List<string> { "Right3", "Right4" });

            // Act
            Moderator.SaveModerators();
            Moderator.LoadModerators();

            // Assert
            var moderators = Moderator.GetModerators();
            Assert.Equal(2, moderators.Count);
            Assert.Contains(moderators, m => m.AccountId == 1 && m.DateOfAssignment == date1 && m.Rights.SequenceEqual(new List<string> { "Right1", "Right2" }));
            Assert.Contains(moderators, m => m.AccountId == 2 && m.DateOfAssignment == date2 && m.Rights.SequenceEqual(new List<string> { "Right3", "Right4" }));
        }
    }
}
