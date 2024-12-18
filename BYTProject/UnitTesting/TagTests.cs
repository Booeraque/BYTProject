using BYTProject.Models;
using Xunit;
using System.Collections.Generic;

namespace BYTProject.UnitTesting;

public class TagTests
{
    public TagTests()
    {
        // Clear tags and post tags before each test
        Tag.ClearTags();
        PostTag.ClearPostTags();
    }

    [Fact]
    public void TagID_ShouldThrowException_WhenValueIsNonPositive()
    {
        var tag = new Tag(1, new List<string> { "Category 1" });
        Assert.Throws<ArgumentException>(() => tag.TagId = 0);
    }

    [Fact]
    public void TagID_ShouldReturnCorrectValue()
    {
        var tag = new Tag(1, new List<string> { "Category 1" });
        Assert.Equal(1, tag.TagId);
    }

    [Fact]
    public void Categories_ShouldThrowException_WhenValueIsEmpty()
    {
        var tag = new Tag(1, new List<string> { "Category 1" });
        Assert.Throws<ArgumentException>(() => tag.Categories = new List<string> { "" });
    }

    [Fact]
    public void Categories_ShouldThrowException_WhenValueIsNull()
    {
        Assert.Throws<ArgumentException>(() => new Tag(1, null));
    }

    [Fact]
    public void Categories_ShouldThrowException_WhenMoreThan10Items()
    {
        var longCategoryList = new List<string> { "Cat1", "Cat2", "Cat3", "Cat4", "Cat5", "Cat6", "Cat7", "Cat8", "Cat9", "Cat10", "Cat11" };
        Assert.Throws<ArgumentException>(() => new Tag(1, longCategoryList));
    }

    [Fact]
    public void Categories_ShouldReturnCorrectValues()
    {
        var categories = new List<string> { "Category 1", "Category 2" };
        var tag = new Tag(1, categories);
        Assert.Equal(categories, tag.Categories);
    }
    
    [Fact]
    public void AddTag_ShouldThrowException_WhenTagIsNull()
    {
        Assert.Throws<ArgumentException>(() => Tag.AddTag(null));
    }

    [Fact]
    public void AddTag_ShouldAddTagCorrectly()
    {
        var tag = new Tag(1, new List<string> { "Category 1" });
        Tag.AddTag(tag);
        Assert.Contains(tag, Tag.GetTags());
    }

    [Fact]
    public void GetTags_ShouldReturnCorrectList()
    {
        var tag = new Tag(1, new List<string> { "Category 1" });
        Tag.AddTag(tag);
        var tags = Tag.GetTags();
        Assert.Contains(tag, tags);
    }

    [Fact]
    public void SaveAndLoadTags_ShouldPersistDataCorrectly()
    {
        Tag.ClearTags();
        var tag1 = new Tag(1, new List<string> { "Category 10" });
        var tag2 = new Tag(2, new List<string> { "Category 20" });

        Tag.SaveTags();
        Tag.LoadTags();

        var tags = Tag.GetTags();
        Assert.Equal(2, tags.Count);
        Assert.Equal(1, tags[0].TagId);
        Assert.Equal(new List<string> { "Category 10" }, tags[0].Categories);
        Assert.Equal(2, tags[1].TagId);
        Assert.Equal(new List<string> { "Category 20" }, tags[1].Categories);
    }
    [Fact]
    public void AddPost_ShouldAssociatePostWithTag()
    {
        var tag = new Tag(1, new List<string> { "Category 1" });
        var post = new Post(1, "Post Caption", DateTime.Now);

        tag.AddPost(post);

        Assert.Contains(post, tag.Posts);
    }

    [Fact]
    public void AddPost_ShouldThrowException_WhenPostAlreadyAssociated()
    {
        var tag = new Tag(1, new List<string> { "Category 1" });
        var post = new Post(1, "Post Caption", DateTime.Now);

        tag.AddPost(post);
        var exception = Assert.Throws<InvalidOperationException>(() => tag.AddPost(post));

        Assert.Equal("Post is already associated with this tag.", exception.Message);
    }

    [Fact]
    public void RemovePost_ShouldDisassociatePostFromTag()
    {
        var tag = new Tag(1, new List<string> { "Category 1" });
        var post = new Post(1, "Post Caption", DateTime.Now);

        tag.AddPost(post);
        tag.RemovePost(post);

        Assert.DoesNotContain(post, tag.Posts);
    }

    [Fact]
    public void RemovePost_ShouldDoNothing_WhenPostNotAssociated()
    {
        var tag = new Tag(1, new List<string> { "Category 1" });
        var post = new Post(1, "Post Caption", DateTime.Now);

        tag.RemovePost(post); // Should not throw
        Assert.Empty(tag.Posts);
    }
    [Fact]
    public void AddPostTag_ShouldAssociatePostTagWithTag()
    {
        var tag = new Tag(1, new List<string> { "Category 1" });
        var postTag = new PostTag(DateTime.Now);

        tag.AddPostTag(postTag);

        Assert.Contains(postTag, tag.PostTags);
    }

    [Fact]
    public void AddPostTag_ShouldThrowException_WhenPostTagAlreadyAssociated()
    {
        var tag = new Tag(1, new List<string> { "Category 1" });
        var postTag = new PostTag(DateTime.Now);

        tag.AddPostTag(postTag);
        var exception = Assert.Throws<InvalidOperationException>(() => tag.AddPostTag(postTag));

        Assert.Equal("PostTag is already associated with this tag.", exception.Message);
    }

    [Fact]
    public void RemovePostTag_ShouldDisassociatePostTagFromTag()
    {
        var tag = new Tag(1, new List<string> { "Category 1" });
        var postTag = new PostTag(DateTime.Now);

        tag.AddPostTag(postTag);
        tag.RemovePostTag(postTag);

        Assert.DoesNotContain(postTag, tag.PostTags);
    }

    [Fact]
    public void RemovePostTag_ShouldDoNothing_WhenPostTagNotAssociated()
    {
        var tag = new Tag(1, new List<string> { "Category 1" });
        var postTag = new PostTag(DateTime.Now);

        tag.RemovePostTag(postTag); // Should not throw
        Assert.Empty(tag.PostTags);
    }

}
