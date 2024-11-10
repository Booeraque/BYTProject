using Xunit;

namespace BYTProject.UnitTesting;

public class AccountTests
{
    [Fact]
    public void AccountID_ShouldThrowException_WhenValueIsNonPositive()
    {
        var account = new Account(1, "User1", "user1@example.com", DateTime.Now.AddYears(-20), "Address 1", "Password1");
        Assert.Throws<ArgumentException>(() => account.AccountID = 0);
    }
    
    [Fact]
    public void AddAccount_ShouldThrowException_WhenAccountIsNull()
    {
        Assert.Throws<ArgumentException>(() => Account.AddAccount(null));
    }

    [Fact]
    public void AccountConstructor_ShouldThrowException_WhenEmailIsNotProvided()
    {
        Assert.Throws<ArgumentException>(() => new Account(1, "User1", null, DateTime.Now.AddYears(-20), "Address 1", "Password1"));
    }

    [Fact]
    public void Username_ShouldThrowException_WhenValueIsEmpty()
    {
        var account = new Account(1, "User1", "user1@example.com", DateTime.Now.AddYears(-20), "Address 1", "Password1");
        Assert.Throws<ArgumentException>(() => account.Username = "");
    }

    [Fact]
    public void Email_ShouldThrowException_WhenValueIsEmpty()
    {
        var account = new Account(1, "User1", "user1@example.com", DateTime.Now.AddYears(-20), "Address 1", "Password1");
        Assert.Throws<ArgumentException>(() => account.Email = "");
    }

    [Fact]
    public void BirthDate_ShouldThrowException_WhenValueIsInTheFuture()
    {
        var account = new Account(1, "User1", "user1@example.com", DateTime.Now.AddYears(-20), "Address 1", "Password1");
        Assert.Throws<ArgumentException>(() => account.BirthDate = DateTime.Now.AddDays(1));
    }

    [Fact]
    public void Age_ShouldReturnCorrectValue()
    {
        var birthDate = DateTime.Now.AddYears(-20);
        var account = new Account(1, "User1", "user1@example.com", birthDate, "Address 1", "Password1");
        Assert.Equal(20, account.Age);
    }

    [Fact]
    public void SaveAndLoadAccounts_ShouldPersistDataCorrectly()
    {
        // Arrange
        var account1 = new Account(1, "User1", "user1@example.com", DateTime.Now.AddYears(-20), "Address 1", "Password1");
        var account2 = new Account(2, "User2", "user2@example.com", DateTime.Now.AddYears(-25), "Address 2", "Password2");

        // Act
        Account.SaveAccounts();
        Account.LoadAccounts();

        // Assert
        var accounts = Account.GetAccounts();
        Assert.Equal(2, accounts.Count);
        Assert.Equal(1, accounts[0].AccountID);
        Assert.Equal("User1", accounts[0].Username);
        Assert.Equal(2, accounts[1].AccountID);
        Assert.Equal("User2", accounts[1].Username);
    }
    
}