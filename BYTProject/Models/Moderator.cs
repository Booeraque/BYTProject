namespace BYTProject.Models;

public class Moderator
{
    // Mandatory attribute: AccountID
    private int _accountId;
    public int AccountId
    {
        get => _accountId;
        set
        {
            if (value <= 0) throw new ArgumentException("AccountID must be positive.");
            _accountId = value;
        }
    }

    // Complex attribute: DateOfAssignment
    private DateTime _dateOfAssignment;
    public DateTime DateOfAssignment
    {
        get => _dateOfAssignment;
        set
        {
            if (value > DateTime.Now) throw new ArgumentException("Date of assignment cannot be in the future.");
            _dateOfAssignment = value;
        }
    }

    // Multi-value attribute: Rights (1..5 values)
    private List<string> _rights = new List<string>();
    public List<string> Rights
    {
        get => new List<string>(_rights); // Return a copy to prevent external modification
        set
        {
            if (value == null || value.Count < 1 || value.Count > 5)
            {
                throw new ArgumentException("Rights must contain between 1 and 5 values.");
            }
            _rights = value;
        }
    }

    // Private static extent collection to store all Moderator objects
    private static List<Moderator> _moderatorsExtent = new List<Moderator>();

    // Private static method to add a Moderator to the extent, with validation
    internal static void AddModerator(Moderator moderator)
    {
        if (moderator == null)
        {
            throw new ArgumentException("Moderator cannot be null.");
        }
        _moderatorsExtent.Add(moderator);
    }

    // Public static method to get a read-only copy of the extent
    public static IReadOnlyList<Moderator> GetModerators()
    {
        return _moderatorsExtent.AsReadOnly();
    }

    // Constructor to initialize Moderator with mandatory attributes and automatically add to extent
    public Moderator(int accountID, DateTime dateOfAssignment, List<string> rights)
    {
        AccountId = accountID;
        DateOfAssignment = dateOfAssignment;
        Rights = rights;

        // Automatically add to extent
        AddModerator(this);
    }

    // Method to save all moderators to XML (for persistence)
    public static void SaveModerators()
    {
        PersistenceManager.SaveExtent(_moderatorsExtent, "Moderators.xml");
    }

    // Method to load all moderators from XML (for persistence)
    public static void LoadModerators()
    {
        _moderatorsExtent = PersistenceManager.LoadExtent<Moderator>("Moderators.xml");
    }
}