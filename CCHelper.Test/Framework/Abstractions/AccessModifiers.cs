using System.Reflection;

namespace CCHelper.Test.Framework.Abstractions;

public enum AccessModifier
{
    Public = MethodAttributes.Public,
    Internal = MethodAttributes.Assembly,
    Protected = MethodAttributes.Family,
    Private = MethodAttributes.Private,
}
