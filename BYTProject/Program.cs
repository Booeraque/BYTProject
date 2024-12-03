using BYTProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BYTProject
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load all accounts from XML
            Console.WriteLine("Loading accounts from XML...");
            Account.LoadAccounts();

            // Load all posts from XML
            Console.WriteLine("Loading posts from XML...");
            Post.LoadPosts();

            // Example: Add a new account if not already existing
            const int newAccountId = 13;
            var account = Account.GetAccounts().FirstOrDefault(acc => acc.AccountId == newAccountId);

            if (account == null)
            {
                account = new Account(
                    newAccountId, // AccountID
                    "jacob", // Username
                    "jacob@example.com", // Email
                    new DateTime(1995, 1, 1), // BirthDate
                    "123 Main St", // Address
                    "password123" // Password
                );

                Console.WriteLine($"Created new account: {account.Username} (ID: {account.AccountId})");
            }
            else
            {
                Console.WriteLine($"Account with ID {newAccountId} already exists.");
            }

            // Add posts to the account
            var post1 = new Post(101, "My first post!", DateTime.Now);
            var post2 = new Post(102, "Another update.", DateTime.Now);
            var post3 = new Post(103, "A quick thought.", DateTime.Now);

            Console.WriteLine("\n--- Adding Posts to Account ---");
            account.AddPost(post1);
            account.AddPost(post2);

            Console.WriteLine($"Added {account.Posts.Count} posts to account {account.Username}.");

            // Display the reverse connection
            Console.WriteLine($"Post 101 is associated with account: {post1.Account?.Username}");
            Console.WriteLine($"Post 102 is associated with account: {post2.Account?.Username}");

            // Remove a post and check reverse connection
            Console.WriteLine("\n--- Removing a Post from Account ---");
            account.RemovePost(post1);
            Console.WriteLine($"Removed post 101. Is post still associated? {post1.Account != null}");

            // Edit a post connection
            Console.WriteLine("\n--- Editing a Post Association ---");
            account.UpdatePost(post2, post3);
            Console.WriteLine($"Post 102 reassigned to Post 103. Account Posts Count: {account.Posts.Count}");
            Console.WriteLine($"Post 102 is associated with account: {post2.Account?.Username}");
            Console.WriteLine($"Post 103 is associated with account: {post3.Account?.Username}");

            // Save accounts and posts back to XML
            Console.WriteLine("\nSaving accounts and posts to XML...");
            Account.SaveAccounts();
            Post.SavePosts();

            // Output all accounts and their posts
            Console.WriteLine("\n--- All Accounts and Their Posts ---");
            foreach (var acc in Account.GetAccounts())
            {
                Console.WriteLine($"Account ID: {acc.AccountId}, Username: {acc.Username}, Posts: {acc.Posts.Count}");
                foreach (var post in acc.Posts)
                {
                    Console.WriteLine($"  Post ID: {post.PostId}, Caption: {post.Caption}");
                }
            }

            // Verify reverse connections are maintained
            Console.WriteLine("\n--- Verifying Reverse Connections ---");
            foreach (var post in Post.GetPosts())
            {
                Console.WriteLine($"Post ID: {post.PostId}, Caption: {post.Caption}, Associated Account: {post.Account?.Username ?? "None"}");
            }
        }
    }
}
