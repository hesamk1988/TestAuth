namespace TestAuth.Authorization;

[AttributeUsage(AttributeTargets.Method)]
public class CustomAllowAnonymousAttribute : Attribute
{ }