﻿using FluentAssertions;
using TinyHelpers.Extensions;

namespace TinyHelpers.Tests.Extensions;

public class GuidExtensionsTests
{
    [Fact]
    public void GuidIsEmpty_IsEmpty_Should_Return_True()
    {
        // Arrange
        var input = Guid.Empty;

        // Act
        var hasValue = input.IsEmpty();

        // Assert
        hasValue.Should().BeTrue();
    }

    [Fact]
    public void GuidIsNotEmpty_IsEmpty_Should_Return_False()
    {
        // Arrange
        var input = Guid.NewGuid();

        // Act
        var hasValue = input.IsEmpty();

        // Assert
        hasValue.Should().BeFalse();
    }

    [Fact]
    public void GuidIsEmpty_IsNotEmpty_Should_Return_False()
    {
        // Arrange
        var input = Guid.Empty;

        // Act
        var hasValue = input.IsNotEmpty();

        // Assert
        hasValue.Should().BeFalse();
    }

    [Fact]
    public void GuidIsNotEmpty_IsNotEmpty_Should_Return_True()
    {
        // Arrange
        var input = Guid.NewGuid();

        // Act
        var hasValue = input.IsNotEmpty();

        // Assert
        hasValue.Should().BeTrue();
    }

    [Fact]
    public void NullableGuidIsEmpty_IsEmpty_Should_Return_True()
    {
        // Arrange
        Guid? input = Guid.Empty;

        // Act
        var hasValue = input.IsEmpty();

        // Assert
        hasValue.Should().BeTrue();
    }

    [Fact]
    public void NullableGuidIsNotEmpty_IsEmpty_Should_Return_False()
    {
        // Arrange
        Guid? input = Guid.NewGuid();

        // Act
        var hasValue = input.IsEmpty();

        // Assert
        hasValue.Should().BeFalse();
    }

    [Fact]
    public void NullableGuidIsEmpty_IsNotEmpty_Should_Return_False()
    {
        // Arrange
        Guid? input = Guid.Empty;

        // Act
        var hasValue = input.IsNotEmpty();

        // Assert
        hasValue.Should().BeFalse();
    }

    [Fact]
    public void NullableGuidIsNotEmpty_IsNotEmpty_Should_Return_True()
    {
        // Arrange
        Guid? input = Guid.NewGuid();

        // Act
        var hasValue = input.IsNotEmpty();

        // Assert
        hasValue.Should().BeTrue();
    }

    [Fact]
    public void NullableGuidIsNull_IsEmpty_Should_Return_True()
    {
        // Arrange
        Guid? input = null;

        // Act
        var hasValue = input.IsEmpty();

        // Assert
        hasValue.Should().BeTrue();
    }

    [Fact]
    public void NullableGuidIsNull_IsNotEmpty_Should_Return_False()
    {
        // Arrange
        Guid? input = null;

        // Act
        var hasValue = input.IsNotEmpty();

        // Assert
        hasValue.Should().BeFalse();
    }

    [Fact]
    public void GuidIsEmpty_GetValueOrCreateNew_Should_Return_NewGuid()
    {
        // Arrange
        var input = Guid.Empty;

        // Act
        var value = input.GetValueOrCreateNew();

        // Assert
        value.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void GuidIsNotEmpty_GetValueOrCreateNew_Should_Return_TheSameGuid()
    {
        // Arrange
        var input = Guid.NewGuid();

        // Act
        var value = input.GetValueOrCreateNew();

        // Assert
        value.Should().Be(input);
    }

    [Fact]
    public void NullableGuidIsEmpty_GetValueOrCreateNew_Should_Return_NewGuid()
    {
        // Arrange
        Guid? input = Guid.Empty;

        // Act
        var value = input.GetValueOrCreateNew();

        // Assert
        value.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void NullableGuidIsNull_GetValueOrCreateNew_Should_Return_NewGuid()
    {
        // Arrange
        Guid? input = null;

        // Act
        var value = input.GetValueOrCreateNew();

        // Assert
        value.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void NullableGuidIsNotEmpty_GetValueOrCreateNew_Should_Return_TheSameGuid()
    {
        // Arrange
        Guid? input = Guid.NewGuid();

        // Act
        var value = input.GetValueOrCreateNew();

        // Assert
        value.Should().Be((Guid)input);
    }
}