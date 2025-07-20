namespace Application.Common.Interfaces.Authentication;
public interface IUserContext
{
    Guid UserId { get; }
    string? UserName { get; }
    string? Email { get; }
    List<string> Roles { get; }
}
