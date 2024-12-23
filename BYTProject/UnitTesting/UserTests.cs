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
    public void AddAdminRight_ShouldAddRightCorrectly()
    {
        var user = new User(1, true);
        user.AddAdminRight("ManagePosts");
        Assert.Contains("ManagePosts", user.AdminRights);
    }

    [Fact]
    public void AddAdminRight_ShouldThrowException_WhenRightIsNullOrEmpty()
    {
        var user = new User(1, true);
        Assert.Throws<ArgumentNullException>(() => user.AddAdminRight(null));
        Assert.Throws<ArgumentNullException>(() => user.AddAdminRight(""));
    }

    [Fact]
    public void AddAdminRight_ShouldThrowException_WhenUserIsNotAdmin()
    {
        var user = new User(1, false);
        Assert.Throws<InvalidOperationException>(() => user.AddAdminRight("ManagePosts"));
    }

    [Fact]
    public void AddAdminRight_ShouldThrowException_WhenRightIsDuplicate()
    {
        var user = new User(1, true);
        user.AddAdminRight("ManagePosts");
        Assert.Throws<InvalidOperationException>(() => user.AddAdminRight("ManagePosts"));
    }

    [Fact]
    public void RemoveAdminRight_ShouldRemoveRightCorrectly()
    {
        var user = new User(1, true);
        user.AddAdminRight("ManagePosts");
        user.RemoveAdminRight("ManagePosts");

        Assert.DoesNotContain("ManagePosts", user.AdminRights);
    }

    [Fact]
    public void RemoveAdminRight_ShouldThrowException_WhenRightDoesNotExist()
    {
        var user = new User(1, true);
        Assert.Throws<InvalidOperationException>(() => user.RemoveAdminRight("NonExistentRight"));
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
}
