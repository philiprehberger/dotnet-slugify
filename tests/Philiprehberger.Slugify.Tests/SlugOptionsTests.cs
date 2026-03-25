using Xunit;
namespace Philiprehberger.Slugify.Tests;

public class SlugOptionsTests
{
    [Fact]
    public void DefaultOptions_MaxLength_Is200()
    {
        var options = new SlugOptions();

        Assert.Equal(200, options.MaxLength);
    }

    [Fact]
    public void DefaultOptions_Separator_IsDash()
    {
        var options = new SlugOptions();

        Assert.Equal("-", options.Separator);
    }

    [Fact]
    public void DefaultOptions_Lowercase_IsTrue()
    {
        var options = new SlugOptions();

        Assert.True(options.Lowercase);
    }

    [Fact]
    public void WithInit_OverridesDefaults()
    {
        var options = new SlugOptions
        {
            MaxLength = 50,
            Separator = "_",
            Lowercase = false
        };

        Assert.Equal(50, options.MaxLength);
        Assert.Equal("_", options.Separator);
        Assert.False(options.Lowercase);
    }
}
