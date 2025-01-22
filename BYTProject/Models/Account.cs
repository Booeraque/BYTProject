using BYTProject.Data;

namespace BYTProject.Models;

public class Account
{
    // Role Enum for User and Moderator
    [Flags]
    public enum RoleType
    {
        User = 1,        // Default role
        Moderator = 2,   // Moderator role
        Both = User | Moderator
    }

    private RoleType _roles = RoleType.User; // Default is User
    public RoleType Roles
    {
        get => _roles;
        private set => _roles = value;
    }

    // Role Enum for Musician and VideoEditor (Independent Roles)
    public enum AdditionalRoleType
    {
        None = 0,
        Musician = 1,
        VideoEditor = 2
    }

    private AdditionalRoleType _additionalRole = AdditionalRoleType.None;
    public AdditionalRoleType AdditionalRole
    {
        get => _additionalRole;
        private set => _additionalRole = value;
    }

    // Role assignment tracking
    private readonly Dictionary<AdditionalRoleType, DateTime> _additionalRoleAssignments = new();

    // Dynamic Role Management for User/Moderator
    public void AddRole(RoleType role)
    {
        if (Roles.HasFlag(role))
            throw new InvalidOperationException($"Account already has the role: {role}");

        Roles |= role;
        Console.WriteLine($"Added role: {role} to Account {AccountId}");
    }

    public void RemoveRole(RoleType role)
    {
        if (role == RoleType.User && !Roles.HasFlag(RoleType.Moderator))
            throw new InvalidOperationException("An account must have at least one role. Cannot remove the User role.");

        Roles &= ~role;
        Console.WriteLine($"Removed role: {role} from Account {AccountId}");
    }

    // Dynamic Role Management for Musician/VideoEditor
    public void SetAdditionalRole(AdditionalRoleType role)
    {
        if (AdditionalRole != AdditionalRoleType.None)
        {
            throw new InvalidOperationException($"Account already has an additional role: {AdditionalRole}");
        }

        if (role == AdditionalRoleType.None)
        {
            throw new InvalidOperationException("Cannot explicitly set role to None.");
        }

        _additionalRole = role;
        _additionalRoleAssignments[role] = DateTime.Now;

        Console.WriteLine($"Assigned {role} role to Account {AccountId} at {_additionalRoleAssignments[role]}");
    }

    public void RemoveAdditionalRole()
    {
        if (AdditionalRole == AdditionalRoleType.None)
        {
            throw new InvalidOperationException("Account does not have an additional role to remove.");
        }

        Console.WriteLine($"Removed {AdditionalRole} role from Account {AccountId}");
        _additionalRole = AdditionalRoleType.None;
    }

    public DateTime? GetAdditionalRoleAssignmentDate(AdditionalRoleType role)
    {
        return _additionalRoleAssignments.ContainsKey(role) ? _additionalRoleAssignments[role] : null;
    }

    // Role-based behavior simulation
    public void PerformRoleBasedAction()
    {
        if (Roles.HasFlag(RoleType.User))
        {
            Console.WriteLine($"Account {AccountId} is performing User actions.");
        }
        if (Roles.HasFlag(RoleType.Moderator))
        {
            Console.WriteLine($"Account {AccountId} is performing Moderator actions.");
        }

        if (AdditionalRole == AdditionalRoleType.Musician)
        {
            Console.WriteLine($"Account {AccountId} is performing Musician actions.");
        }
        else if (AdditionalRole == AdditionalRoleType.VideoEditor)
        {
            Console.WriteLine($"Account {AccountId} is performing VideoEditor actions.");
        }
    }


    
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
        
        Roles = RoleType.User;

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
    
    // Association: One Account -> Many Posts
    private readonly List<Post> _posts = new List<Post>();

    // Getter: Return a copy of the posts
    public IReadOnlyList<Post> Posts => _posts.AsReadOnly();

    // Method: Add a Post to the Account
    public void AddPost(Post post)
    {
        if (post == null)
            throw new ArgumentNullException(nameof(post), "Post cannot be null.");

        // Check if a post with the same PostId already exists
        if (_posts.Any(p => p.PostId == post.PostId))
            throw new InvalidOperationException("The post is already associated with this account.");

        // Ensure the post is disassociated from any previous account
        post.SetAccount(this);

        // Add the post to this account's list
        _posts.Add(post);
    }



    
    // Method: Remove a Post from the Account
    public void RemovePost(Post post)
    {
        if (post == null)
            throw new ArgumentNullException(nameof(post), "Post cannot be null.");

        // Remove the post and handle reverse reference removal
        if (_posts.Remove(post))
        {
            _posts.Remove(post);
            post.RemoveAccount(); // Ensure the reverse connection is removed
        }
    }


    // Method: Update an existing Post
    public void UpdatePost(Post oldPost, Post newPost)
    {
        if (oldPost == null || newPost == null)
            throw new ArgumentNullException("Posts cannot be null.");

        RemovePost(oldPost);
        AddPost(newPost);
    }
    
    // Association: One Account -> Many Comments
    private readonly List<Comment> _comments = new List<Comment>();

    // Getter: Return a copy of the comments
    public IReadOnlyList<Comment> Comments => _comments.AsReadOnly();

    // Method: Add a Comment to the Account
    public void AddComment(Comment comment)
    {
        if (comment == null)
            throw new ArgumentNullException(nameof(comment), "Comment cannot be null.");

        // Check if a comment with the same CommentId already exists
        if (_comments.Any(c => c.CommentId == comment.CommentId))
            throw new InvalidOperationException("The comment is already associated with this account.");

        // Ensure the comment is disassociated from any previous account
        comment.SetAccount(this);

        // Add the comment to this account's list
        _comments.Add(comment);
    }

    // Method: Remove a Comment from the Account
    public void RemoveComment(Comment comment)
    {
        if (comment == null)
            throw new ArgumentNullException(nameof(comment), "Comment cannot be null.");

        // Remove the comment and handle reverse reference removal
        if (_comments.Remove(comment))
        {
            comment.RemoveAccount(); // Ensure the reverse connection is removed
        }
    }

    // Association: One Account -> Many Likes
    private readonly List<Like> _likes = new List<Like>();

    // Getter: Return a copy of the likes
    public IReadOnlyList<Like> Likes => _likes.AsReadOnly();

    // Method: Add a Like to the Account
    public void AddLike(Like like)
    {
        if (like == null)
            throw new ArgumentNullException(nameof(like), "Like cannot be null.");

        // Check if a like with the same LikeId already exists
        if (_likes.Any(l => l.LikeId == like.LikeId))
            throw new InvalidOperationException("The like is already associated with this account.");

        // Ensure the like is disassociated from any previous account
        like.SetAccount(this);

        // Add the like to this account's list
        _likes.Add(like);
    }

    // Method: Remove a Like from the Account
    public void RemoveLike(Like like)
    {
        if (like == null)
            throw new ArgumentNullException(nameof(like), "Like cannot be null.");

        // Remove the like and handle reverse reference removal
        if (_likes.Remove(like))
        {
            like.RemoveAccount(); // Ensure the reverse connection is removed
        }
    }
    
    public void RemoveAccount()
    {
        foreach (var comment in _comments.ToList())
        {
            RemoveComment(comment);
        }

        foreach (var like in _likes.ToList())
        {
            RemoveLike(like);
        }

        // Additional logic to remove the account from the global list, if applicable
    }
}
