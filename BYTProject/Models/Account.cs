using System;
using System.Collections.Generic;

public class Account
{
    // Mandatory attribute: AccountID
    private int _accountID;
    public int AccountID
    {
        get => _accountID;
        set
        {
            if (value <= 0) throw new ArgumentException("AccountID must be positive.");
            _accountID = value;
        }
    }

    // Mandatory attribute: Username
    private string _username;
    public string Username
    {
        get => _username;
        set
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Username can't be empty.");
            _username = value;
        }
    }

    // Mandatory attribute: Email
    private string _email;
    public string Email
    {
        get => _email;
        set
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Email can't be empty.");
            _email = value;
        }
    }

    // Complex attribute: BirthDate (with encapsulation)
    private DateTime _birthDate;
    public DateTime BirthDate
    {
        get => _birthDate;
        set
        {
            if (value > DateTime.Now) throw new ArgumentException("Birth date cannot be in the future.");
            _birthDate = value;
        }
    }

    // Derived attribute: Age (based on BirthDate)
    public int Age
    {
        get
        {
            if (BirthDate == DateTime.MinValue) throw new InvalidOperationException("BirthDate must be set before calculating age.");
            var today = DateTime.Today;
            var age = today.Year - BirthDate.Year;
            if (BirthDate.Date > today.AddYears(-age)) age--;  // Adjust if the birthday hasn't occurred yet this year.
            return age;
        }
    }

    // Mandatory attribute: Address
    private string _address;
    public string Address
    {
        get => _address;
        set
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Address can't be empty.");
            _address = value;
        }
    }

    // Mandatory attribute: Password
    private string _password;
    public string Password
    {
        get => _password;
        set
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Password can't be empty.");
            _password = value;
        }
    }

    // Private static extent collection to store all Account objects
    private static List<Account> accountsExtent = new List<Account>();

    // Private static method to add an Account to the extent, with validation
    internal static void AddAccount(Account account)
    {
        if (account == null)
        {
            throw new ArgumentException("Account cannot be null.");
        }
        if (accountsExtent.Exists(a => a.AccountID == account.AccountID))
        {
            throw new ArgumentException("An account with the same AccountID already exists.");
        }
        accountsExtent.Add(account);
    }

    // Public static method to get a read-only copy of the extent
    public static IReadOnlyList<Account> GetAccounts()
    {
        return accountsExtent.AsReadOnly();
    }

    // Constructor to initialize Account object with mandatory attributes and automatically add to extent
    public Account(int accountID, string username, string email, DateTime birthDate, string address, string password)
    {
        AccountID = accountID;
        Username = username;
        Email = email;
        BirthDate = birthDate;
        Address = address;
        Password = password;

        // Automatically add to extent
        AddAccount(this);
    }

    // Parameterless constructor needed for XML serialization
    public Account() { }

    // Method to save all accounts to XML (for persistence)
    public static void SaveAccounts(string filename = "XML Files/Accounts.xml")
    {
        PersistenceManager.SaveExtent(accountsExtent, filename);
    }
    // Method to load all accounts from XML (for persistence)
    public static void LoadAccounts(string filename = "XML Files/Accounts.xml")
    {
        accountsExtent = PersistenceManager.LoadExtent<Account>(filename);
    }
    
    public static void ClearAccounts()
    {
        accountsExtent.Clear();
    }

}
