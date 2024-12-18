using Xunit;
using System;
using BYTProject.Models;

namespace BYTProject.UnitTesting
{
    public class GroupTests
    {
        public GroupTests()
        {
            // Clear groups and posts before each test
            Group.ClearGroups();
            Post.ClearPosts();
        }

        [Fact]
        public void GroupID_ShouldThrowException_WhenValueIsNonPositive()
        {
            var group = new Group(1, "Group 1", "Sample Description");
            Assert.Throws<ArgumentException>(() => group.GroupId = 0);
        }

        [Fact]
        public void GroupID_ShouldReturnCorrectValue()
        {
            var group = new Group(1, "Group 1", "Sample Description");
            Assert.Equal(1, group.GroupId);
        }

        [Fact]
        public void GroupName_ShouldThrowException_WhenValueIsEmpty()
        {
            var group = new Group(1, "Group 1", "Sample Description");
            Assert.Throws<ArgumentException>(() => group.GroupName = "");
        }

        [Fact]
        public void GroupName_ShouldReturnCorrectValue()
        {
            var group = new Group(1, "Group 1", "Sample Description");
            Assert.Equal("Group 1", group.GroupName);
        }

        [Fact]
        public void Description_ShouldThrowException_WhenValueIsEmpty()
        {
            var group = new Group(1, "Group 1", "Sample Description");
            Assert.Throws<ArgumentException>(() => group.Description = "");
        }

        [Fact]
        public void Description_ShouldReturnCorrectValue()
        {
            var group = new Group(1, "Group 1", "Sample Description");
            Assert.Equal("Sample Description", group.Description);
        }

        [Fact]
        public void AddGroup_ShouldThrowException_WhenGroupIsNull()
        {
            Assert.Throws<ArgumentException>(() => Group.AddGroup(null));
        }

        [Fact]
        public void AddGroup_ShouldAddGroupCorrectly()
        {
            var group = new Group(1, "Group 1", "Sample Description");
            Group.AddGroup(group);
            Assert.Contains(group, Group.GetGroups());
        }

        [Fact]
        public void AddPost_ShouldCreateReverseConnection()
        {
            var group = new Group(1, "Group 1", "Sample Description");
            var post = new Post(1, "Post Caption", DateTime.Now);

            group.AddPost(post);

            Assert.Contains(post, group.Posts);
            Assert.Equal(group, post.Group); // Verify reverse connection
        }

        [Fact]
        public void RemovePost_ShouldRemoveReverseConnection()
        {
            var group = new Group(1, "Group 1", "Sample Description");
            var post = new Post(1, "Post Caption", DateTime.Now);

            group.AddPost(post);
            group.RemovePost(post);

            Assert.DoesNotContain(post, group.Posts);
            Assert.Null(post.Group); // Verify reverse connection is removed
        }

        [Fact]
        public void AddPost_ShouldThrowException_WhenPostAlreadyInGroup()
        {
            var group = new Group(1, "Group 1", "Sample Description");
            var post = new Post(1, "Post Caption", DateTime.Now);

            group.AddPost(post);

            var exception = Assert.Throws<InvalidOperationException>(() => group.AddPost(post));
            Assert.Equal("Post is already in this group.", exception.Message);
        }

        [Fact]
        public void ModifyPostConnection_ShouldUpdateGroup()
        {
            var group1 = new Group(1, "Group 1", "Description 1");
            var group2 = new Group(2, "Group 2", "Description 2");
            var post = new Post(1, "Post Caption", DateTime.Now);

            group1.AddPost(post);
            Assert.Equal(group1, post.Group);

            // Move post to a different group
            group2.AddPost(post);

            Assert.DoesNotContain(post, group1.Posts);
            Assert.Contains(post, group2.Posts);
            Assert.Equal(group2, post.Group); // Verify updated reverse connection
        }

        [Fact]
        public void AddPost_ShouldThrowException_WhenPostIsNull()
        {
            var group = new Group(1, "Group 1", "Sample Description");
            Assert.Throws<ArgumentNullException>(() => group.AddPost(null));
        }

        [Fact]
        public void RemovePost_ShouldDoNothing_WhenPostNotInGroup()
        {
            var group = new Group(1, "Group 1", "Sample Description");
            var post = new Post(1, "Post Caption", DateTime.Now);

            group.RemovePost(post); // Should not throw or fail
            Assert.Empty(group.Posts);
        }

        [Fact]
        public void SaveAndLoadGroups_ShouldPersistDataCorrectly()
        {
            Group.ClearGroups();
            var group1 = new Group(1, "Group 1", "Description 1");
            var group2 = new Group(2, "Group 2", "Description 2");

            Group.SaveGroups();
            Group.ClearGroups();
            Group.LoadGroups();

            var groups = Group.GetGroups();
            Assert.Equal(2, groups.Count);
            Assert.Equal(1, groups[0].GroupId);
            Assert.Equal("Group 1", groups[0].GroupName);
            Assert.Equal("Description 1", groups[0].Description);
            Assert.Equal(2, groups[1].GroupId);
            Assert.Equal("Group 2", groups[1].GroupName);
            Assert.Equal("Description 2", groups[1].Description);
        }
    }
}
