using CCEasy.Services;
using CCEasy.Test.Framework.TestData;
using System;
using Xunit;

namespace CCEasy.Test.Tests.Units;

public class TestTypeBinder
{
    [Theory]
    [MemberData(nameof(TypeData.ReferenceTypes), MemberType = typeof(TypeData))]
    public void CanHoldNull_ReferenceType_ReturnsTrue(Type type)
    {
        Assert.True(TypeBinder.CanHoldNull(type));
    }

    [Theory]
    [MemberData(nameof(TypeData.NullableTypes), MemberType = typeof(TypeData))]
    public void CanHoldNull_NullableType_ReturnsTrue(Type type)
    {
        Assert.True(TypeBinder.CanHoldNull(type));
    }



    [Theory]
    [MemberData(nameof(TypeData.ValueTypes), MemberType = typeof(TypeData))]
    [MemberData(nameof(TypeData.ReferenceTypes), MemberType = typeof(TypeData))]
    [MemberData(nameof(TypeData.NullableTypes), MemberType = typeof(TypeData))]
    public void CanBind_SameTypes_ReturnsTrue(Type type)
    {
        Assert.True(TypeBinder.CanBind(type, type));
    }

    [Theory]
    [MemberData(nameof(TypeData.ValueTypes), MemberType = typeof(TypeData))]
    [MemberData(nameof(TypeData.NullableTypes), MemberType = typeof(TypeData))]
    public void CanBind_ChildAndParentType_ReturnsTrue(Type type)
    {
        Assert.True(TypeBinder.CanBind(type, typeof(object)));
    }
}
