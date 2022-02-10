namespace CCHelper;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class SolutionAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
public class ResultAttribute : Attribute
{ 
}