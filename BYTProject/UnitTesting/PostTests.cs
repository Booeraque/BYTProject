using Xunit;
using System;
using BYTProject.Models;

namespace BYTProject.UnitTesting
{
    public class PostTests
    {
        public PostTests()
        {
            // Clear posts before each test
            Post.ClearPosts();
            Comment.ClearComments();
            Like.ClearLikes();
        }

        [Fact]
        public void PostID_ShouldThrowException_WhenValueIsNonPositive()
        {
            var post = new Post(1, "Caption 1", DateTime.Now);
            Assert.Throws<ArgumentException>(() => post.PostId = 0);
        }

        [Fact]
        public void PostID_ShouldReturnCorrectValue()
        {
            var post = new Post(1, "Caption 1", DateTime.Now);
            Assert.Equal(1, post.PostId);
        }

        [Fact]
        public void Caption_ShouldThrowException_WhenValueIsEmpty()
        {
            var post = new Post(1, "Caption 1", DateTime.Now);
            Assert.Throws<ArgumentException>(() => post.Caption = "");
        }

        [Fact]
        public void Caption_ShouldReturnCorrectValue()
        {
            var post = new Post(1, "Caption 1", DateTime.Now);
            Assert.Equal("Caption 1", post.Caption);
        }

        [Fact]
        public void CreatedAt_ShouldThrowException_WhenValueIsInTheFuture()
        {
            var post = new Post(1, "Caption 1", DateTime.Now);
            Assert.Throws<ArgumentException>(() => post.CreatedAt = DateTime.Now.AddDays(1));
        }

        [Fact]
        public void CreatedAt_ShouldReturnCorrectValue()
        {
            var createdAt = DateTime.Now;
            var post = new Post(1, "Caption 1", createdAt);
            Assert.Equal(createdAt, post.CreatedAt);
        }

        [Fact]
        public void AddPost_ShouldThrowException_WhenPostIsNull()
        {
            Assert.Throws<ArgumentException>(() => Post.AddPost(null));
        }

        [Fact]
        public void AddPost_ShouldAddPostCorrectly()
        {
            // Clear posts before the test to avoid the extent being pre-filled
            Post.ClearPosts();

            var post = new Post(1, "Caption 1", DateTime.Now);
    
            // Add the post to the extent
            Post.AddPost(post);

            // Verify that the post has been added
            Assert.Contains(post, Post.GetPosts());
        }

        [Fact]
        public void AddPost_ShouldThrowException_WhenDuplicatePostIsAdded()
        {
            Post.ClearPosts();
            
            var post1 = new Post(1, "Caption 1", DateTime.Now);

            // Add the first post to the extent
            Post.AddPost(post1);

            // Now try to add a duplicate post (with the same PostId)
            var post2 = new Post(1, "Caption 2", DateTime.Now); // Same PostId as post1

            // Try adding the duplicate post
            Assert.Throws<InvalidOperationException>(() => Post.AddPost(post2));
        }
        

        [Fact]
        public void GetPosts_ShouldReturnCorrectList()
        {
            var post = new Post(1, "Caption 1", DateTime.Now);
            Post.AddPost(post);
            var posts = Post.GetPosts();
            Assert.Contains(post, posts);
        }

        [Fact]
        public void SaveAndLoadPosts_ShouldPersistDataCorrectly()
        {
            var post1 = new Post(1, "Caption 1", DateTime.Now);
            var post2 = new Post(2, "Caption 2", DateTime.Now);

            Post.SavePosts();
            Post.LoadPosts();

            var posts = Post.GetPosts();
            Assert.Equal(2, posts.Count);
            Assert.Equal(1, posts[0].PostId);
            Assert.Equal("Caption 1", posts[0].Caption);
            Assert.Equal(2, posts[1].PostId);
            Assert.Equal("Caption 2", posts[1].Caption);
        }

        [Fact]
        public void UpdatePost_ShouldUpdateCaptionAndCreatedAtCorrectly()
        {
            var post = new Post(1, "Original Caption", DateTime.Now);

            var newCaption = "Updated Caption";
            var newCreatedAt = DateTime.Now.AddMinutes(-1);

            post.UpdatePost(newCaption, newCreatedAt);

            // Verify that the post's caption and creation date have been updated correctly
            Assert.Equal(newCaption, post.Caption);
            Assert.Equal(newCreatedAt, post.CreatedAt);
        }

        [Fact]
        public void UpdatePost_ShouldThrowException_WhenNewCaptionIsEmpty()
        {
            var post = new Post(1, "Original Caption", DateTime.Now);

            Assert.Throws<ArgumentException>(() => post.UpdatePost("", DateTime.Now.AddMinutes(-1)));
        }

        [Fact]
        public void UpdatePost_ShouldThrowException_WhenNewCreatedAtIsInTheFuture()
        {
            var post = new Post(1, "Original Caption", DateTime.Now);

            Assert.Throws<ArgumentException>(() => post.UpdatePost("Updated Caption", DateTime.Now.AddDays(1)));
        }

        [Fact]
        public void RemovePost_ShouldThrowException_WhenPostIsNull()
        {
            Assert.Throws<ArgumentException>(() => Post.RemovePost(null));
        }

        [Fact]
        public void RemovePost_ShouldRemovePostCorrectly()
        {
            // Create and add a post
            var post = new Post(1, "Caption 1", DateTime.Now);
            Post.AddPost(post);

            // Verify the post is added first
            Assert.Contains(post, Post.GetPosts());

            // Now, remove the post
            Post.RemovePost(post);

            // Verify that the post has been removed
            Assert.DoesNotContain(post, Post.GetPosts());
        }
        
        [Fact]
        public void RemovePost_ShouldRemoveAssociatedCommentsAndLikes()
        {
            var post = new Post(1, "Caption 1", DateTime.Now);
            var comment = new Comment(1, "Content 1", DateTime.Now);
            var like = new Like(1, DateTime.Now);

            post.AddComment(comment);
            post.AddLike(like);

            post.RemoveComment(comment);
            post.RemoveLike(like);

            Assert.DoesNotContain(comment, post.Comments);
            Assert.DoesNotContain(like, post.Likes);
        }

        [Fact]
        public void RemovePost_ShouldDisassociateCommentsAndLikes()
        {
            var post = new Post(1, "Caption 1", DateTime.Now);
            var comment = new Comment(1, "Content 1", DateTime.Now);
            var like = new Like(1, DateTime.Now);

            post.AddComment(comment);
            post.AddLike(like);

            Post.RemovePost(post);

            Assert.DoesNotContain(comment, Comment.GetComments());
            Assert.DoesNotContain(like, Like.GetLikes());
        }
        [Fact]
        public void AddMedia_ShouldAddMediaToPost()
        {
            var post = new Post(1, "Caption", DateTime.Now);
            var media = new Media(1, "Image");

            post.AddMedia(media);

            Assert.Contains(media, post.MediaList);
            Assert.Equal(post, media.Post); // Verify reverse connection
        }

        [Fact]
        public void AddMedia_ShouldThrowException_WhenMediaAlreadyExists()
        {
            var post = new Post(1, "Caption", DateTime.Now);
            var media = new Media(1, "Image");

            post.AddMedia(media);

            var exception = Assert.Throws<InvalidOperationException>(() => post.AddMedia(media));
            Assert.Equal("Media is already associated with this post.", exception.Message);
        }

        [Fact]
        public void AddMedia_ShouldThrowException_WhenExceedingMediaLimit()
        {
            var post = new Post(1, "Caption", DateTime.Now);

            for (int i = 1; i <= 10; i++)
            {
                post.AddMedia(new Media(i, $"Image{i}"));
            }

            var extraMedia = new Media(11, "ExtraImage");

            var exception = Assert.Throws<InvalidOperationException>(() => post.AddMedia(extraMedia));
            Assert.Equal("A post can have a maximum of 10 media items.", exception.Message);
        }

        [Fact]
        public void RemoveMedia_ShouldDisassociateMediaFromPost()
        {
            var post = new Post(1, "Caption", DateTime.Now);
            var media = new Media(1, "Image");

            post.AddMedia(media);
            post.RemoveMedia(media);

            Assert.DoesNotContain(media, post.MediaList);
            Assert.Null(media.Post); // Verify reverse connection is removed
        }
        [Fact]
        public void SetGroup_ShouldAssignGroupToPost()
        {
            var group = new Group(1, "Group 1", "Description");
            var post = new Post(1, "Caption", DateTime.Now);

            post.SetGroup(group);

            Assert.Equal(group, post.Group);
            Assert.Contains(post, group.Posts); // Verify reverse connection
        }

        [Fact]
        public void RemoveGroup_ShouldDisassociateGroupFromPost()
        {
            var group = new Group(1, "Group 1", "Description");
            var post = new Post(1, "Caption", DateTime.Now);

            post.SetGroup(group);
            post.RemoveGroup();

            Assert.Null(post.Group); // Verify post no longer references the group
            Assert.DoesNotContain(post, group.Posts); // Verify group no longer contains the post
        }

        [Fact]
        public void AddTag_ShouldAssociateTagWithPost()
        {
            var post = new Post(1, "Caption", DateTime.Now);
            var tag = new Tag(1, new System.Collections.Generic.List<string> { "Tag1" });

            post.AddTag(tag);

            Assert.Contains(tag, post.Tags);
            Assert.Contains(post, tag.Posts); // Verify reverse connection
        }

        [Fact]
        public void AddTag_ShouldThrowException_WhenTagAlreadyAssociated()
        {
            var post = new Post(1, "Caption", DateTime.Now);
            var tag = new Tag(1, new System.Collections.Generic.List<string> { "Tag1" });

            post.AddTag(tag);

            var exception = Assert.Throws<InvalidOperationException>(() => post.AddTag(tag));
            Assert.Equal("Tag is already associated with this post.", exception.Message);
        }

        [Fact]
        public void RemoveTag_ShouldDisassociateTagFromPost()
        {
            var post = new Post(1, "Caption", DateTime.Now);
            var tag = new Tag(1, new System.Collections.Generic.List<string> { "Tag1" });

            post.AddTag(tag);
            post.RemoveTag(tag);

            Assert.DoesNotContain(tag, post.Tags);
            Assert.DoesNotContain(post, tag.Posts); // Verify reverse connection
        }

    }
}
