class Program
{
	static void Main(string[] args)
	{
		// Load all accounts from XML
		Account.LoadAccounts();

		// Example: Add a new account to the list
		Account account = new Account
		{
			AccountID = 1,
			Username = "nova",
			Email = "nova@example.com",
			BirthDate = new DateTime(1995, 1, 1),
			Phone = "123-456-7890",
			Address = "123 Main St",
			Password = "password123"
		};
		Account.Accounts.Add(account);

		// Save the accounts back to XML
		Account.SaveAccounts();

		// Output all accounts to the console
		foreach (var acc in Account.Accounts)
		{
			Console.WriteLine($"Account ID: {acc.AccountID}, Username: {acc.Username}");
		}
	}
}
