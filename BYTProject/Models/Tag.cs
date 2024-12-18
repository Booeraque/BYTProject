using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace BYTProject.Models
{
    public class Tag
    {
        // Mandatory attribute: TagID
        private int _tagId;

        public int TagId
        {
            get => _tagId;
            set
            {
                if (value <= 0) throw new ArgumentException("TagID must be positive.");
                _tagId = value;
            }
        }

        // Mandatory attribute: Categories
        private List<string> _categories;

        [XmlArray("Categories")]
        [XmlArrayItem("Category")]
        public List<string> Categories
        {
            get => _categories;
            set
            {
                if (value == null) throw new ArgumentException("Categories cannot be null.");
                if (value.Count == 0 || value.Contains(string.Empty))
                    throw new ArgumentException("Categories cannot be empty.");
                if (value.Count > 10) throw new ArgumentException("Categories cannot contain more than 10 items.");
                _categories = value;
            }
        }

        // Constructor to initialize Tag with mandatory attributes
        public Tag(int tagId, List<string> categories)
        {
            TagId = tagId;
            Categories = categories;
            AddTag(this); // Automatically add to extent
        }

        // Parameterless constructor for XML serialization
        public Tag() { }

        // Static extent collection to store all Tag objects
        private static List<Tag> _tagsExtent = new List<Tag>();

        public static void ClearTags()
        {
            _tagsExtent.Clear();
        }

        // Method to add a Tag to the extent, with validation
        public static void AddTag(Tag tag)
        {
            if (tag == null) throw new ArgumentException("Tag cannot be null.");
            _tagsExtent.Add(tag);
        }

        // Public static method to get a read-only copy of the extent
        public static IReadOnlyList<Tag> GetTags()
        {
            return _tagsExtent.AsReadOnly();
        }

        // Method to save all tags to XML (for persistence)
        public static void SaveTags(string filename = "Tags.xml")
        {
            Console.WriteLine($"Saving tags to {filename}...");
            try
            {
                using (var writer = new StreamWriter(filename))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Tag>));
                    serializer.Serialize(writer, _tagsExtent);
                }
                Console.WriteLine("Tag data saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving Tag data to {filename}: {ex.Message}");
            }
        }

        // Method to load all tags from XML (for persistence)
        public static void LoadTags(string filename = "Tags.xml")
        {
            Console.WriteLine($"Loading tags from {filename}...");
            try
            {
                if (File.Exists(filename))
                {
                    using (var reader = new StreamReader(filename))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<Tag>));
                        _tagsExtent = (List<Tag>)serializer.Deserialize(reader);
                    }
                    Console.WriteLine("Tags loaded successfully.");
                }
                else
                {
                    Console.WriteLine($"No Tag file found at {filename}. Starting fresh.");
                    _tagsExtent = new List<Tag>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading Tag data: {ex.Message}");
            }
        }
        
        // Association: Many Tags -> Many Posts
        private readonly List<Post> _posts = new List<Post>();

        public IReadOnlyList<Post> Posts => _posts.AsReadOnly();

        public void AddPost(Post post)
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post), "Post cannot be null.");

            if (_posts.Contains(post))
                throw new InvalidOperationException("Post is already associated with this tag.");

            _posts.Add(post);

        }

        public void RemovePost(Post post)
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post), "Post cannot be null.");

            _posts.Remove(post);
        }

        private readonly List<PostTag> _postTags = new List<PostTag>();

        public IReadOnlyList<PostTag> PostTags => _postTags.AsReadOnly();

        public void AddPostTag(PostTag postTag)
        {
            if (postTag == null) throw new ArgumentNullException(nameof(postTag), "PostTag cannot be null.");
            if (_postTags.Contains(postTag)) throw new InvalidOperationException("PostTag is already associated with this tag.");

            _postTags.Add(postTag);
        }

        public void RemovePostTag(PostTag postTag)
        {
            if (postTag == null) throw new ArgumentNullException(nameof(postTag), "PostTag cannot be null.");

            _postTags.Remove(postTag);
        }

    }
}
