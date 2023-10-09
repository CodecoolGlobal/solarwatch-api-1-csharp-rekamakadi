namespace SolarWatch.Services.Authentication;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(string email, string username, string password, string role);
    Task<AuthResult> LoginAsync(string username, string password);
    Task<AuthResult> ManageUserRoleAsync(string userId, string roleName, bool addToRole);
    Task<AuthResult> UpdateUserAsync(string email, string newEmail, string newPassword, string newName);

}