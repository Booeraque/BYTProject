using Xunit;

namespace BYTProject.UnitTesting;

public class GroupTests
{
    [Fact]
    public void GroupID_ShouldThrowException_WhenValueIsNonPositive()
    {
        var group = new Group(1, "Group 1");
        Assert.Throws<ArgumentException>(() => group.GroupID = 0);
    }

    [Fact]
    public void GroupName_ShouldThrowException_WhenValueIsEmpty()
    {
        var group = new Group(1, "Group 1");
        Assert.Throws<ArgumentException>(() => group.GroupName = "");
    }
    
    [Fact]
    public void AddGroup_ShouldThrowException_WhenGroupIsNull()
    {
        Assert.Throws<ArgumentException>(() => Group.AddGroup(null));
    }

    [Fact]
    public void GroupConstructor_ShouldThrowException_WhenGroupNameIsNotProvided()
    {
        Assert.Throws<ArgumentException>(() => new Group(1, null));
    }

    [Fact]
    public void SaveAndLoadGroups_ShouldPersistDataCorrectly()
    {
        // Arrange
        var group1 = new Group(1, "Group 1");
        var group2 = new Group(2, "Group 2");

        // Act
        Group.SaveGroups();
        Group.LoadGroups();

        // Assert
        var groups = Group.GetGroups();
        Assert.Equal(2, groups.Count);
        Assert.Equal(1, groups[0].GroupID);
        Assert.Equal("Group 1", groups[0].GroupName);
        Assert.Equal(2, groups[1].GroupID);
        Assert.Equal("Group 2", groups[1].GroupName);
    }
}