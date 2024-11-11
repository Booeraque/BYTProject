using BYTProject.Models;

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
        }
    }
}