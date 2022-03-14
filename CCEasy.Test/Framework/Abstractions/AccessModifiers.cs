using System.Reflection;

namespace CCEasy.Test.Framework.Abstractions;

public enum AccessModifier
{
    Public = MethodAttributes.Public,
    Internal = MethodAttributes.Assembly,
    Protected = MethodAttributes.Family,
    Private = MethodAttributes.Private,
}
