namespace BYTProject
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load all accounts from XML
            Account.LoadAccounts();

            // Example: Add a new account to the list
            int newAccountId = 10;
            if (!Account.GetAccounts().Any(acc => acc.AccountID == newAccountId))
            {
                Account account = new Account(
                    newAccountId, // AccountID
                    "nova", // Username
                    "nova@example.com", // Email
                    new DateTime(1995, 1, 1), // BirthDate
                    "123 Main St", // Address
                    "password123" // Password
                );

                // Add the account to the extent collection using AddAccount
                Account.AddAccount(account);
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
                Console.WriteLine($"Account ID: {acc.AccountID}, Username: {acc.Username}");
            }
        }
    }
}