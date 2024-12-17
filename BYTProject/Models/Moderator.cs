using BYTProject.Data;
using System.Xml.Serialization;
using System.IO;

namespace BYTProject.Models
{
    public class Moderator
    {
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

        private List<string> _rights;
        public List<string> Rights
        {
            get => _rights;
            set
            {
                if (value == null || value.Count == 0 || value.Count > 5) throw new ArgumentException("Rights must be between 1 and 5 items.");
                _rights = value;
            }
        }

        private static List<Moderator> _moderatorsExtent = new List<Moderator>();

        internal static void AddModerator(Moderator moderator)
        {
            if (moderator == null) throw new ArgumentException("Moderator cannot be null.");
            _moderatorsExtent.Add(moderator);
        }

        public static IReadOnlyList<Moderator> GetModerators()
        {
            return _moderatorsExtent.AsReadOnly();
        }

        public Moderator(int accountId, DateTime dateOfAssignment, List<string> rights)
        {
            AccountId = accountId;
            DateOfAssignment = dateOfAssignment;
            _rights = rights;
            Rights = rights;

            AddModerator(this);
        }

        public Moderator(List<string> rights)
        {
            _rights = rights;
        }

        public static void SaveModerators()
        {
            try
            {
                Console.WriteLine("Saving moderators to XML...");
                XmlSerializer serializer = new XmlSerializer(typeof(List<Moderator>));
                using (FileStream fs = new FileStream("Moderators.xml", FileMode.Create))
                {
                    serializer.Serialize(fs, _moderatorsExtent);
                }
                Console.WriteLine("Save complete.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving moderators: {ex.Message}");
            }
        }

        public static void LoadModerators()
        {
            try
            {
                Console.WriteLine("Loading moderators from XML...");
                XmlSerializer serializer = new XmlSerializer(typeof(List<Moderator>));
                using (FileStream fs = new FileStream("Moderators.xml", FileMode.Open))
                {
                    _moderatorsExtent = (List<Moderator>)serializer.Deserialize(fs);
                }
                Console.WriteLine($"Loaded {_moderatorsExtent.Count} moderators from Moderators.xml.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading Moderator data from Moderators.xml: {ex.Message}");
            }
        }

        public static void ClearModerators()
        {
            _moderatorsExtent.Clear();
        }

        // Association handling methods added here
        private readonly List<string> _managedRights = new List<string>();

        // Getter: Return a copy of the managed rights
        public IReadOnlyList<string> ManagedRights => _managedRights.AsReadOnly();

        // Method: Add a Right to the Moderator
        public void AddManagedRight(string right)
        {
            if (string.IsNullOrEmpty(right))
                throw new ArgumentNullException(nameof(right), "Right cannot be null or empty.");

            if (_managedRights.Count >= 5)
                throw new InvalidOperationException("A moderator cannot have more than 5 rights.");

            if (_managedRights.Contains(right))
                throw new InvalidOperationException("The right is already assigned to this moderator.");

            _managedRights.Add(right);
        }

        // Method: Remove a Right from the Moderator
        public void RemoveManagedRight(string right)
        {
            if (string.IsNullOrEmpty(right))
                throw new ArgumentNullException(nameof(right), "Right cannot be null or empty.");

            if (!_managedRights.Remove(right))
                throw new InvalidOperationException("The right does not exist in this moderator's managed rights.");
        }
    }
}
