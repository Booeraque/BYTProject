namespace BYTProject.Models;

public class User
{
    // Mandatory attribute: AccountID
    private int _accountId;
    public int AccountId
    {
        get => _accountId;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentException("AccountID must be positive.");
            }
            _accountId = value;
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
    private static List<User> _usersExtent = new List<User>();

    // Private static method to add a User to the extent, with validation
    internal static void AddUser(User user)
    {
        if (user == null)
        {
            throw new ArgumentException("User cannot be null.");
        }
        _usersExtent.Add(user);
    }

    // Public static method to get a read-only copy of the extent
    public static IReadOnlyList<User> GetUsers()
    {
        return _usersExtent.AsReadOnly();
    }

    // Constructor to initialize User with mandatory attributes and automatically add to extent
    public User(int accountId, bool isAdmin)
    {
        AccountId = accountId;
        IsAdmin = isAdmin;

        // Automatically add to extent
        AddUser(this);
    }
    public User(){}

    // Method to save all users to XML (for persistence)
    public static void SaveUsers()
    {
        PersistenceManager.SaveExtent(_usersExtent, "Users.xml");
    }

    // Method to load all users from XML (for persistence)
    public static void LoadUsers()
    {
        _usersExtent = PersistenceManager.LoadExtent<User>("Users.xml");
    }
}