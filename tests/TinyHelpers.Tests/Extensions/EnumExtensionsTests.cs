using System.ComponentModel.DataAnnotations;
using TinyHelpers.Extensions;

namespace TinyHelpers.Tests.Extensions;

public class EnumExtensionsTests
{
    [Fact]
    public void GetDescription_DisplayAndFallbackValues_ReturnsDescriptions()
    {
        Assert.Equal("First item", SampleFlags.First.GetDescription());
        Assert.Equal("Both", (SampleFlags.First | SampleFlags.Second).GetDescription());
        Assert.Equal("None", SampleFlags.None.GetDescription());
    }

    [Fact]
    public void GetFlags_DeclaredCompositeValue_ReturnsCompositeMember()
    {
        Assert.Equal([SampleFlags.Both], (SampleFlags.First | SampleFlags.Second).GetFlags());
    }

    [Fact]
    public void GetFlags_ZeroWithDeclaredZero_ReturnsZeroMember()
    {
        Assert.Equal([SampleFlags.None], SampleFlags.None.GetFlags());
    }

    [Fact]
    public void GetFlags_UnknownBits_ReturnsEmptySequence()
    {
        Assert.Empty(((SampleFlags)8).GetFlags());
    }

    [Fact]
    public void GetDescriptions_GenericAndTypeOverloads_ReturnAllValues()
    {
        var generic = EnumExtensions.GetDescriptions<SampleFlags>();
        var byType = typeof(SampleFlags).GetDescriptions();

        Assert.Equal("First item", generic[1]);
        Assert.Equal("Second", generic[2]);
        Assert.Equal(generic, byType);
    }

    [Flags]
    private enum SampleFlags
    {
        None = 0,
        [Display(Name = "First item")]
        First = 1,
        Second = 2,
        Both = First | Second
    }
}
