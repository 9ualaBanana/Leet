using CCHelper.Services;
using CCHelper.Test.Framework.TestData;
using System;
using Xunit;

namespace CCHelper.Test.Tests.Units;

public class TestTypeBinder
{
    [Theory]
    [InlineData(typeof(object))]
    [InlineData(typeof(string))]
    public void ShouldBeAbleToHoldNull_WhenTypeIsReferenceType(Type type)
    {
        Assert.True(TypeBinder.CanHoldNull(type));
    }

    [Theory]
    [MemberData(nameof(TypeData.NullableTypes), MemberType = typeof(TypeData))]
    public void ShouldBeAbleToHoldNull_WhenTypeIsNullable(Type type)
    {
        Assert.True(TypeBinder.CanHoldNull(type));
    }

    [Theory]
    [MemberData(nameof(TypeData.Types), MemberType = typeof(TypeData))]
    public void ShouldBeAbleToBind_WhenTypesAreSame(Type type)
    {
        Assert.True(TypeBinder.CanBind(type, type));
    }

    [Theory]
    [MemberData(nameof(TypeData.Types), MemberType = typeof(TypeData))]
    public void ShouldBeAbleToBind_WhenTypeIsSubtypeOfAnotherType(Type type)
    {
        Assert.True(TypeBinder.CanBind(type, typeof(object)));
    }
}
