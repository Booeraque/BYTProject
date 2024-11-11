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
            // Clear moderators before each test
            Moderator.ClearModerators();
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
            Assert.Throws<ArgumentException>(() => moderator.Rights = new List<string> { "Right1", "Right2", "Right3", "Right4", "Right5", "Right6" });
        }

        [Fact]
        public void Rights_ShouldReturnCorrectValue()
        {
            var rights = new List<string> { "Right1", "Right2" };
            var moderator = new Moderator(1, DateTime.Now, rights);
            Assert.Equal(rights, moderator.Rights);
        }

        [Fact]
        public void AddModerator_ShouldThrowException_WhenModeratorIsNull()
        {
            Assert.Throws<ArgumentException>(() => Moderator.AddModerator(null));
        }

        [Fact]
        public void AddModerator_ShouldAddModeratorCorrectly()
        {
            var moderator = new Moderator(1, DateTime.Now, new List<string> { "Right1" });
            Assert.Contains(moderator, Moderator.GetModerators());
        }

        [Fact]
        public void SaveAndLoadModerators_ShouldPersistDataCorrectly()
        {
            Moderator.ClearModerators();
            var moderator1 = new Moderator(1, DateTime.Now, new List<string> { "Right1" });
            var moderator2 = new Moderator(2, DateTime.Now, new List<string> { "Right2" });

            Moderator.SaveModerators();
            Moderator.ClearModerators();
            Moderator.LoadModerators();

            var moderators = Moderator.GetModerators();
            Assert.Equal(2, moderators.Count);
            Assert.Equal(1, moderators[0].AccountId);
            Assert.Equal("Right1", moderators[0].Rights[0]);
            Assert.Equal(2, moderators[1].AccountId);
            Assert.Equal("Right2", moderators[1].Rights[0]);
        }
    }
}