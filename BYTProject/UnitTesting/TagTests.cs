using System;
using System.Collections.Generic;
using BYTProject.Models;
using Xunit;

public class TagTests
{
    [Fact]
    public void TagID_ShouldThrowException_WhenValueIsNonPositive()
    {
        var tag = new Tag(1, new List<string> { "Category 1" });
        Assert.Throws<ArgumentException>(() => tag.TagId = 0);
    }

    [Fact]
    public void Categories_ShouldThrowException_WhenValueIsEmpty()
    {
        var tag = new Tag(1, new List<string> { "Category 1" });
        Assert.Throws<ArgumentException>(() => tag.Categories = new List<string> { "" });
    }

    [Fact]
    public void AddTag_ShouldThrowException_WhenTagIsNull()
    {
        Assert.Throws<ArgumentException>(() => Tag.AddTag(null));
    }

    [Fact]
    public void TagConstructor_ShouldThrowException_WhenCategoriesContainMoreThan10Items()
    {
        var longCategoryList = new List<string> { "Cat1", "Cat2", "Cat3", "Cat4", "Cat5", "Cat6", "Cat7", "Cat8", "Cat9", "Cat10", "Cat11" };
        Assert.Throws<ArgumentException>(() => new Tag(1, longCategoryList));
    }

    [Fact]
    public void SaveAndLoadTags_ShouldPersistDataCorrectly()
    {
        // Arrange
        var tag1 = new Tag(1, new List<string> { "Category 1" });
        var tag2 = new Tag(2, new List<string> { "Category 2" });

        // Act
        Tag.SaveTags();
        Tag.LoadTags();

        // Assert
        var tags = Tag.GetTags();
        Assert.Equal(2, tags.Count);
        Assert.Equal(1, tags[0].TagId);
        Assert.Equal(new List<string> { "Category 1" }, tags[0].Categories);
        Assert.Equal(2, tags[1].TagId);
        Assert.Equal(new List<string> { "Category 2" }, tags[1].Categories);
    }
}