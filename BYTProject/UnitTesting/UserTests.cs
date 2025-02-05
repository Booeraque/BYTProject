using BYTProject.Models;
using Xunit;

namespace BYTProject.UnitTesting;

public class UserTests
{
    public UserTests()
    {
        // Clear users before each test
        User.ClearUsers();
    }

    [Fact]
    public void AccountID_ShouldThrowException_WhenValueIsNonPositive()
    {
        var user = new User(1, true);
        Assert.Throws<ArgumentException>(() => user.AccountId = 0);
    }

    [Fact]
    public void AccountID_ShouldReturnCorrectValue()
    {
        var user = new User(1, true);
        Assert.Equal(1, user.AccountId);
    }

    [Fact]
    public void IsAdmin_ShouldReturnCorrectValue()
    {
        var user = new User(1, true);
        Assert.True(user.IsAdmin);

        var user2 = new User(2, false);
        Assert.False(user2.IsAdmin);
    }

    [Fact]
    public void AddUser_ShouldThrowException_WhenUserIsNull()
    {
        Assert.Throws<ArgumentException>(() => User.AddUser(null));
    }

    [Fact]
    public void AddUser_ShouldAddUserCorrectly()
    {
        var user = new User(1, true);
        User.AddUser(user);
        Assert.Contains(user, User.GetUsers());
    }

    [Fact]
    public void GetUsers_ShouldReturnCorrectList()
    {
        var user = new User(1, true);
        var users = User.GetUsers();
        Assert.Contains(user, users);
    }
    
    [Fact]
    public void SaveAndLoadUsers_ShouldPersistDataCorrectly()
    {
        User.ClearUsers();
        var user1 = new User(1, true);
        var user2 = new User(2, false);

        User.SaveUsers();
        User.LoadUsers();

        var users = User.GetUsers();
        Assert.Equal(2, users.Count);
        Assert.Equal(1, users[0].AccountId);
        Assert.True(users[0].IsAdmin);
        Assert.Equal(2, users[1].AccountId);
        Assert.False(users[1].IsAdmin);
    }
    [Fact]
    public void SetGroup_ShouldAssignGroupToUser()
    {
        var user = new User(1, true);
        var group = new Group(1, "Group 1", "Sample Description");

        user.SetGroup(group);

        Assert.Equal(group, user.Group);
    }

    [Fact]
    public void SetGroup_ShouldThrowException_WhenGroupIsNull()
    {
        var user = new User(1, true);

        Assert.Throws<ArgumentNullException>(() => user.SetGroup(null));
    }

    [Fact]
    public void RemoveGroup_ShouldDisassociateGroupFromUser()
    {
        var user = new User(1, true);
        var group = new Group(1, "Group 1", "Sample Description");

        user.SetGroup(group);
        user.RemoveGroup();

        Assert.Null(user.Group);
    }

}
