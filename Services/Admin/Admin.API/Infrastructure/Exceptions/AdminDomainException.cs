namespace SkillTracker.Services.Admin.API.Infrastructure.Exceptions;

/// <summary>
/// Exception type for app exceptions
/// </summary>
public class AdminDomainException : Exception
{
    public AdminDomainException()
    { }

    public AdminDomainException(string message)
        : base(message)
    { }

    public AdminDomainException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
