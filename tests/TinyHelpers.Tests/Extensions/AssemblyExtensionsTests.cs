using System.Reflection;
using TinyHelpers.Extensions;

namespace TinyHelpers.Tests.Extensions;

public class AssemblyExtensionsTests
{
    // Custom test class with an Obsolete attribute
    [Obsolete("This class is obsolete.")]
    public class MyClass
    {
        public void MyMethod()
        {
        }
    }

    [Fact]
    public void GetAttribute_ReturnsCorrectValue_WhenObsoleteAttributeIsPresent()
    {
        // Arrange
        var assembly = Assembly.GetAssembly(typeof(MyClass));

        // Act
        var attribute = assembly.GetAttribute<ObsoleteAttribute>(attr => attr.Message);

        // Assert
        Assert.NotNull(attribute);
        Assert.Equal("This class is obsolete.", attribute);
    }
}
