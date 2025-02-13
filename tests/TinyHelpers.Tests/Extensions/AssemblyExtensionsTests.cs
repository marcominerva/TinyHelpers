using System.Reflection;
using TinyHelpers.Extensions;
using static TinyHelpers.Tests.Extensions.AssemblyExtensionsTests;

[assembly: AssemblyInfo("This is a test assembly for custom attributes.")]

namespace TinyHelpers.Tests.Extensions;

public class AssemblyExtensionsTests
{
    // Custom test class with a custom attribute
    [AttributeUsage(AttributeTargets.Assembly)]
    public class AssemblyInfoAttribute : Attribute
    {
        public string Description { get; }

        public AssemblyInfoAttribute(string description)
        {
            Description = description;
        }
    }

    [Fact]
    public void GetAttribute_ReturnsCorrectValue_WhenCustomAttributeIsPresent()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();

        // Act
        var description = assembly.GetAttribute<AssemblyInfoAttribute, string>(attr => attr.Description);

        // Assert
        Assert.NotNull(description);
        Assert.Equal("This is a test assembly for custom attributes.", description);
    }
}
