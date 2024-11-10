using System;
using System.Collections.Generic;
using BYTProject.Models;
using Xunit;

public class UserTests
{
    [Fact]
    public void AccountID_ShouldThrowException_WhenValueIsNonPositive()
    {
        var user = new User(1, true);
        Assert.Throws<ArgumentException>(() => user.AccountId = 0);
    }
    
    [Fact]
    public void AddUser_ShouldThrowException_WhenUserIsNull()
    {
        Assert.Throws<ArgumentException>(() => User.AddUser(null));
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
        Assert.Equal(1, users[0].AccountId);
        Assert.True(users[0].IsAdmin);
        Assert.Equal(2, users[1].AccountId);
        Assert.False(users[1].IsAdmin);
    }
}
