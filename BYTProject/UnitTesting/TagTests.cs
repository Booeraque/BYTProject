using System;
using System.Collections.Generic;
using Xunit;

public class TagTests
{
    [Fact]
    public void TagID_ShouldThrowException_WhenValueIsNonPositive()
    {
        var tag = new Tag(1, "Category 1");
        Assert.Throws<ArgumentException>(() => tag.TagID = 0);
    }

    [Fact]
    public void Category_ShouldThrowException_WhenValueIsEmpty()
    {
        var tag = new Tag(1, "Category 1");
        Assert.Throws<ArgumentException>(() => tag.Category = "");
    }
    
    [Fact]
    public void AddTag_ShouldThrowException_WhenTagIsNull()
    {
        Assert.Throws<ArgumentException>(() => Tag.AddTag(null));
    }

    [Fact]
    public void TagConstructor_ShouldThrowException_WhenCategoryIsNotProvided()
    {
        Assert.Throws<ArgumentException>(() => new Tag(1, null));
    }

    [Fact]
    public void SaveAndLoadTags_ShouldPersistDataCorrectly()
    {
        // Arrange
        var tag1 = new Tag(1, "Category 1");
        var tag2 = new Tag(2, "Category 2");

        // Act
        Tag.SaveTags();
        Tag.LoadTags();

        // Assert
        var tags = Tag.GetTags();
        Assert.Equal(2, tags.Count);
        Assert.Equal(1, tags[0].TagID);
        Assert.Equal("Category 1", tags[0].Category);
        Assert.Equal(2, tags[1].TagID);
        Assert.Equal("Category 2", tags[1].Category);
    }
}