using System;
using System.Collections.Generic;
using BYTProject.Models;

public class Account
{
    // Mandatory attribute: AccountID
    private int _accountId;
    public int AccountId
    {
        get => _accountId;
        set  // Changed to public set for XML deserialization
        {
            if (value <= 0) throw new ArgumentException("AccountID must be positive.");
            _accountId = value;
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
    private static List<Account> _accountsExtent = new List<Account>();

    // Private static method to add an Account to the extent, with validation
    internal static void AddAccount(Account account)
    {
        if (account == null)
        {
            throw new ArgumentException("Account cannot be null.");
        }
        if (_accountsExtent.Exists(a => a.AccountId == account.AccountId))
        {
            throw new ArgumentException("An account with the same AccountID already exists.");
        }
        _accountsExtent.Add(account);
    }

    // Public static method to get a read-only copy of the extent
    public static IReadOnlyList<Account> GetAccounts()
    {
        return _accountsExtent.AsReadOnly();
    }

    // Constructor to initialize Account object with mandatory attributes and automatically add to extent
    public Account(int accountID, string username, string email, DateTime birthDate, string address, string password)
    {
        AccountId = accountID;
        Username = username;
        Email = email;
        BirthDate = birthDate;
        Address = address;
        Password = password;

        // Automatically add to extent
        AddAccount(this);
    }

    // Parameterless constructor needed for XML serialization
    public Account() {}

    // Method to save all accounts to XML (for persistence)
    public static void SaveAccounts(string filename = "Accounts.xml")
    {
        if (string.IsNullOrEmpty(filename))
        {
            Console.WriteLine("Error: Filename for saving accounts is empty.");
            return;
        }

        Console.WriteLine($"Saving accounts to {filename}...");
        PersistenceManager.SaveExtent(_accountsExtent, filename);
    }

    // Method to load all accounts from XML (for persistence)
    public static void LoadAccounts(string filename = "Accounts.xml")
    {
        if (string.IsNullOrEmpty(filename))
        {
            Console.WriteLine("Error: Filename for loading accounts is empty.");
            return;
        }

        Console.WriteLine($"Attempting to load accounts from {filename}...");

        try
        {
            _accountsExtent = PersistenceManager.LoadExtent<Account>(filename);
            Console.WriteLine($"Loaded {_accountsExtent.Count} accounts from {filename}.");

            // Output loaded accounts to confirm
            foreach (var account in _accountsExtent)
            {
                Console.WriteLine($"Loaded Account ID: {account.AccountId}, Username: {account.Username}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading Account data from {filename}: {ex.Message}");
        }
    }

    // Method to clear all accounts from the list
    public static void ClearAccounts()
    {
        _accountsExtent.Clear();
    }
}
