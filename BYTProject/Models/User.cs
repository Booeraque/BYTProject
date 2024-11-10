using System;
using System.Collections.Generic;

public class User
{
    // Mandatory attribute: AccountID
    private int _accountID;
    public int AccountID
    {
        get => _accountID;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentException("AccountID must be positive.");
            }
            _accountID = value;
        }
    }

    // Boolean attribute: IsAdmin
    private bool _isAdmin;
    public bool IsAdmin
    {
        get => _isAdmin;
        set => _isAdmin = value;  // No validation needed for boolean
    }

    // Private static extent collection to store all User objects
    private static List<User> usersExtent = new List<User>();

    // Private static method to add a User to the extent, with validation
    private static void AddUser(User user)
    {
        if (user == null)
        {
            throw new ArgumentException("User cannot be null.");
        }
        usersExtent.Add(user);
    }

    // Public static method to get a read-only copy of the extent
    public static IReadOnlyList<User> GetUsers()
    {
        return usersExtent.AsReadOnly();
    }

    // Constructor to initialize User with mandatory attributes and automatically add to extent
    public User(int accountID, bool isAdmin)
    {
        AccountID = accountID;
        IsAdmin = isAdmin;

        // Automatically add to extent
        AddUser(this);
    }

    // Method to save all users to XML (for persistence)
    public static void SaveUsers()
    {
        PersistenceManager.SaveExtent(usersExtent, "Users.xml");
    }

    // Method to load all users from XML (for persistence)
    public static void LoadUsers()
    {
        usersExtent = PersistenceManager.LoadExtent<User>("Users.xml");
    }
}
