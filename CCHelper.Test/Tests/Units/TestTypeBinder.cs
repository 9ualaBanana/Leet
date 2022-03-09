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
    public void WhenTypeIsReferenceType_ShouldBeAbleToHoldNull(Type type)
    {
        Assert.True(TypeBinder.CanHoldNull(type));
    }

    [Theory]
    [MemberData(nameof(TypeData.NullableTypes), MemberType = typeof(TypeData))]
    public void WhenTypeIsNullable_ShouldBeAbleToHoldNull(Type type)
    {
        Assert.True(TypeBinder.CanHoldNull(type));
    }

    [Theory]
    [MemberData(nameof(TypeData.Types), MemberType = typeof(TypeData))]
    public void WhenTypesAreSame_ShouldBeAbleToBind(Type type)
    {
        Assert.True(TypeBinder.CanBind(type, type));
    }

    [Theory]
    [MemberData(nameof(TypeData.Types), MemberType = typeof(TypeData))]
    public void WhenTypeIsSubtype_ShouldBeAbleToBind(Type type)
    {
        Assert.True(TypeBinder.CanBind(type, typeof(object)));
    }
}
