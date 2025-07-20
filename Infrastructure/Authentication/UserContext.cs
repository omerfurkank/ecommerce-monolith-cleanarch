namespace Infrastructure.Authentication;

internal sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public Guid UserId => User?.GetUserId() ?? throw new ApplicationException("User ID not found.");
    public string? UserName => User?.GetUsername();
    public string? Email => User?.GetEmail();
    public List<string> Roles => User?.GetRoles() ?? [];
}
