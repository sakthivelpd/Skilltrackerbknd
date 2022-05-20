namespace SkillTracker.Services.Profile.API.Infrastructure.Exceptions;

/// <summary>
/// Exception type for app exceptions
/// </summary>
public class ProfileDomainException : Exception
{
    public ProfileDomainException()
    { }

    public ProfileDomainException(string message)
        : base(message)
    { }

    public ProfileDomainException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
