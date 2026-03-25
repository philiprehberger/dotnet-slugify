using Xunit;
namespace Philiprehberger.Slugify.Tests;

public class SlugTests
{
    [Fact]
    public void Generate_SimpleText_ReturnsLowercaseSlug()
    {
        var result = Slug.Generate("Hello World");

        Assert.Equal("hello-world", result);
    }

    [Fact]
    public void Generate_AccentedCharacters_RemovesDiacritics()
    {
        var result = Slug.Generate("cafe resume");

        Assert.Equal("cafe-resume", result);
    }

    [Fact]
    public void Generate_SpecialCharacters_ReplacesWithSeparator()
    {
        var result = Slug.Generate("hello & world! @ 2024");

        Assert.Equal("hello-world-2024", result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Generate_NullOrWhitespace_ReturnsEmpty(string? input)
    {
        var result = Slug.Generate(input!);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Generate_CustomSeparator_UsesSeparator()
    {
        var options = new SlugOptions { Separator = "_" };
        var result = Slug.Generate("Hello World", options);

        Assert.Equal("hello_world", result);
    }

    [Fact]
    public void Generate_LowercaseFalse_PreservesCase()
    {
        var options = new SlugOptions { Lowercase = false };
        var result = Slug.Generate("Hello World", options);

        Assert.Equal("Hello-World", result);
    }

    [Fact]
    public void Generate_MaxLength_TruncatesResult()
    {
        var options = new SlugOptions { MaxLength = 5 };
        var result = Slug.Generate("Hello World", options);

        Assert.True(result.Length <= 5);
    }

    [Fact]
    public void Generate_ConsecutiveSpecialChars_CollapsesToSingleSeparator()
    {
        var result = Slug.Generate("hello---world");

        Assert.Equal("hello-world", result);
    }

    [Fact]
    public void Generate_LeadingTrailingSpecialChars_TrimsEnds()
    {
        var result = Slug.Generate("--hello world--");

        Assert.Equal("hello-world", result);
    }

    [Theory]
    [InlineData("Hello World", "hello-world")]
    [InlineData("My Blog Post!", "my-blog-post")]
    [InlineData("  spaced  out  ", "spaced-out")]
    public void Generate_VariousInputs_ReturnsExpected(string input, string expected)
    {
        var result = Slug.Generate(input);

        Assert.Equal(expected, result);
    }
}
