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
            Account.LoadAccounts();

            // Example: Add a new account to the list
            const int newAccountId = 13;
            if (Account.GetAccounts().All(acc => acc.AccountId != newAccountId))
            {
                var account = new Account(
                    newAccountId, // AccountID
                    "jacob", // Username
                    "jacob@example.com", // Email
                    new DateTime(1995, 1, 1), // BirthDate
                    "123 Main St", // Address
                    "password123" // Password
                );

                // No need to call Account.AddAccount(account); here, as it's already added in the constructor.
            }
            else
            {
                Console.WriteLine($"An account with AccountID {newAccountId} already exists.");
            }

            // Save the accounts back to XML
            Account.SaveAccounts();

            // Output all accounts to the console
            foreach (var acc in Account.GetAccounts())
            {
                Console.WriteLine($"Account ID: {acc.AccountId}, Username: {acc.Username}");
            }

            // Test for Tag functionality
            Console.WriteLine("\n--- Testing Tag Saving and Loading ---");
            
            // Clear existing tags, if any
            Tag.ClearTags();

            // Create some example tags
            var tag1 = new Tag(1, new List<string> { "Category 111" });
            var tag2 = new Tag(2, new List<string> { "Category 222" });


            // Save tags to XML
            Tag.SaveTags();
            Console.WriteLine("Tags saved to Tags.xml.");

            // Clear tags from memory to simulate loading
            Tag.ClearTags();

            // Load tags from XML
            Tag.LoadTags();
            Console.WriteLine("Tags loaded from Tags.xml.");

            // Output loaded tags to confirm
            var tags = Tag.GetTags();
            Console.WriteLine($"Loaded {tags.Count} tags:");
            foreach (var tag in tags)
            {
                Console.WriteLine($"Tag ID: {tag.TagId}, Categories: {string.Join(", ", tag.Categories)}");
            }

            // Check if the correct number of tags were loaded
            if (tags.Count == 2)
            {
                Console.WriteLine("Tag loading successful. Test passed.");
            }
            else
            {
                Console.WriteLine("Tag loading failed. Test did not pass.");
            }
        }
    }
}
