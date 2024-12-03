using Xunit;
using System;
using System.IO;
using BYTProject.Models;

namespace BYTProject.UnitTesting
{
    public class AccountTests
    {
        public AccountTests()
        {
            // Clear accounts before each test
            Account.ClearAccounts();
        }
        
        [Fact]
        public void AccountID_ShouldThrowException_WhenValueIsNonPositive()
        {
            Assert.Throws<ArgumentException>(() => new Account(0, "User1", "user1@example.com", DateTime.Now.AddYears(-20), "Address 1", "Password1"));
        }

        [Fact]
        public void AccountID_ShouldReturnCorrectValue()
        {
            var account = new Account(1, "User1", "user1@example.com", DateTime.Now.AddYears(-20), "Address 1", "Password1");
            Assert.Equal(1, account.AccountId);
        }

        [Fact]
        public void Username_ShouldThrowException_WhenValueIsEmpty()
        {
            var account = new Account(1, "User1", "user1@example.com", DateTime.Now.AddYears(-20), "Address 1", "Password1");
            Assert.Throws<ArgumentException>(() => account.Username = "");
        }

        [Fact]
        public void Username_ShouldReturnCorrectValue()
        {
            var account = new Account(1, "User1", "user1@example.com", DateTime.Now.AddYears(-20), "Address 1", "Password1");
            Assert.Equal("User1", account.Username);
        }

        [Fact]
        public void Email_ShouldThrowException_WhenValueIsEmpty()
        {
            var account = new Account(1, "User1", "user1@example.com", DateTime.Now.AddYears(-20), "Address 1", "Password1");
            Assert.Throws<ArgumentException>(() => account.Email = "");
        }

        [Fact]
        public void Email_ShouldReturnCorrectValue()
        {
            var account = new Account(1, "User1", "user1@example.com", DateTime.Now.AddYears(-20), "Address 1", "Password1");
            Assert.Equal("user1@example.com", account.Email);
        }

        [Fact]
        public void BirthDate_ShouldThrowException_WhenValueIsInTheFuture()
        {
            var account = new Account(1, "User1", "user1@example.com", DateTime.Now.AddYears(-20), "Address 1", "Password1");
            Assert.Throws<ArgumentException>(() => account.BirthDate = DateTime.Now.AddDays(1));
        }

        [Fact]
        public void BirthDate_ShouldReturnCorrectValue()
        {
            var birthDate = DateTime.Now.AddYears(-20);
            var account = new Account(1, "User1", "user1@example.com", birthDate, "Address 1", "Password1");
            Assert.Equal(birthDate, account.BirthDate);
        }

        [Fact]
        public void Age_ShouldThrowException_WhenBirthDateIsNotSet()
        {
            var account = new Account(1, "User1", "user1@example.com", DateTime.MinValue, "Address 1", "Password1");
            Assert.Throws<InvalidOperationException>(() => _ = account.Age);
        }

        [Fact]
        public void Age_ShouldReturnCorrectValue()
        {
            var birthDate = DateTime.Now.AddYears(-20);
            var account = new Account(1, "User1", "user1@example.com", birthDate, "Address 1", "Password1");
            Assert.Equal(20, account.Age);
        }

        [Fact]
        public void Address_ShouldThrowException_WhenValueIsEmpty()
        {
            var account = new Account(1, "User1", "user1@example.com", DateTime.Now.AddYears(-20), "Address 1", "Password1");
            Assert.Throws<ArgumentException>(() => account.Address = "");
        }

        [Fact]
        public void Address_ShouldReturnCorrectValue()
        {
            var account = new Account(1, "User1", "user1@example.com", DateTime.Now.AddYears(-20), "Address 1", "Password1");
            Assert.Equal("Address 1", account.Address);
        }

        [Fact]
        public void Password_ShouldThrowException_WhenValueIsEmpty()
        {
            var account = new Account(1, "User1", "user1@example.com", DateTime.Now.AddYears(-20), "Address 1", "Password1");
            Assert.Throws<ArgumentException>(() => account.Password = "");
        }

        [Fact]
        public void Password_ShouldReturnCorrectValue()
        {
            var account = new Account(1, "User1", "user1@example.com", DateTime.Now.AddYears(-20), "Address 1", "Password1");
            Assert.Equal("Password1", account.Password);
        }

        [Fact]
        public void AddAccount_ShouldThrowException_WhenAccountIsNull()
        {
            Assert.Throws<ArgumentException>(() => Account.AddAccount(null));
        }

        [Fact]
        public void AddAccount_ShouldAddAccountCorrectly()
        {
            var account = new Account(1, "User1", "user1@example.com", DateTime.Now.AddYears(-20), "Address 1", "Password1");
            Assert.Contains(account, Account.GetAccounts());
        }
        
        [Fact]
        public void AddPost_ShouldAssociatePostWithAccount()
        {
            var account = new Account(1, "User1", "user1@example.com", DateTime.Now.AddYears(-20), "Address 1", "Password1");
            var post = new Post(1, "Caption 1", DateTime.Now);

            account.AddPost(post);

            Assert.Contains(post, account.Posts);
            Assert.Equal(account, post.Account);
        }

        [Fact]
        public void RemovePost_ShouldDisassociatePostFromAccount()
        {
            var account = new Account(1, "User1", "user1@example.com", DateTime.Now.AddYears(-20), "Address 1", "Password1");
            var post = new Post(1, "Caption 1", DateTime.Now);

            account.AddPost(post);
            account.RemovePost(post);

            Assert.DoesNotContain(post, account.Posts);
            Assert.Null(post.Account);
        }

        [Fact]
        public void UpdatePost_ShouldReplaceOldPostWithNewPost()
        {
            var account = new Account(1, "User1", "user1@example.com", DateTime.Now.AddYears(-20), "Address 1", "Password1");
            var oldPost = new Post(1, "Caption 1", DateTime.Now);
            var newPost = new Post(2, "Caption 2", DateTime.Now);

            account.AddPost(oldPost);
            account.UpdatePost(oldPost, newPost);

            Assert.DoesNotContain(oldPost, account.Posts);
            Assert.Contains(newPost, account.Posts);
            Assert.Equal(account, newPost.Account);
            Assert.Null(oldPost.Account);
        }

        [Fact]
        public void AddPost_ShouldThrowException_WhenPostIsNull()
        {
            var account = new Account(1, "User1", "user1@example.com", DateTime.Now.AddYears(-20), "Address 1", "Password1");

            Assert.Throws<ArgumentNullException>(() => account.AddPost(null));
        }

        [Fact]
        public void AddPost_ShouldThrowException_WhenPostAlreadyAssociated()
        {
            var account = new Account(1, "User1", "user1@example.com", DateTime.Now.AddYears(-20), "Address 1", "Password1");
            var post = new Post(1, "Caption 1", DateTime.Now);

            account.AddPost(post);

            Assert.Throws<InvalidOperationException>(() => account.AddPost(post));
        }

        [Fact]
        public void SaveAndLoadAccounts_ShouldPersistDataCorrectly()
        {
            var account1 = new Account(1, "User1", "user1@example.com", DateTime.Now.AddYears(-20), "Address 1", "Password1");
            var account2 = new Account(2, "User2", "user2@example.com", DateTime.Now.AddYears(-25), "Address 2", "Password2");

            Account.SaveAccounts("Accounts.xml");
            Account.LoadAccounts("Accounts.xml");

            var accounts = Account.GetAccounts();
            Assert.Equal(2, accounts.Count);
            Assert.Equal(1, accounts[0].AccountId);
            Assert.Equal("User1", accounts[0].Username);
            Assert.Equal(2, accounts[1].AccountId);
            Assert.Equal("User2", accounts[1].Username);

            File.Delete("Accounts.xml");
        }
    }
}
