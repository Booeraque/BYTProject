using System;
using System.Collections.Generic;
using Xunit;

public class UserTests
{
    [Fact]
    public void AccountID_ShouldThrowException_WhenValueIsNonPositive()
    {
        var user = new User(1, true);
        Assert.Throws<ArgumentException>(() => user.AccountID = 0);
    }
    
    [Fact]
    public void AddUser_ShouldThrowException_WhenUserIsNull()
    {
        Assert.Throws<ArgumentException>(() => User.AddUser(null));
    }

    [Fact]
    public void UserConstructor_ShouldThrowException_WhenIsAdminIsNotProvided()
    {
        Assert.Throws<ArgumentException>(() => new User(1));
    }

    [Fact]
    public void SaveAndLoadUsers_ShouldPersistDataCorrectly()
    {
        // Arrange
        var user1 = new User(1, true);
        var user2 = new User(2, false);

        // Act
        User.SaveUsers();
        User.LoadUsers();

        // Assert
        var users = User.GetUsers();
        Assert.Equal(2, users.Count);
        Assert.Equal(1, users[0].AccountID);
        Assert.True(users[0].IsAdmin);
        Assert.Equal(2, users[1].AccountID);
        Assert.False(users[1].IsAdmin);
    }
}