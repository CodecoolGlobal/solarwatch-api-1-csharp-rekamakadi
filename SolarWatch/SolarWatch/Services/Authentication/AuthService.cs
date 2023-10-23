using Microsoft.AspNetCore.Identity;

namespace SolarWatch.Services.Authentication;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private ITokenService _tokenService;

    public AuthService(UserManager<IdentityUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<AuthResult> RegisterAsync(string email, string username, string password, string role)
    {
        var user = new IdentityUser { UserName = username, Email = email };
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return FailedRegistration(result, email, username);
        }

        await _userManager.AddToRoleAsync(user, role);
        return new AuthResult(true, email, username, "");
    }

    public async Task<AuthResult> LoginAsync(string email, string password)
    {
        var managedUser = await _userManager.FindByEmailAsync(email);

        if (managedUser == null)
        {
            return InvalidEmail(email);
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, password);

        if (!isPasswordValid)
        {
            return InvalidPassword(email, managedUser.UserName);
        }

        var userRoles = await _userManager.GetRolesAsync(managedUser);
        var accessToken = _tokenService.CreateToken(managedUser, userRoles[0]);

        return new AuthResult(true, managedUser.Email, managedUser.UserName, accessToken);
    }
    
    public async Task<AuthResult> ManageUserRoleAsync(string email, string roleName, bool addToRole)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return UserNotFound();
        }

        var roleExists = await _userManager.GetRolesAsync(user);
    
        if (addToRole)
        {
            if (roleExists.All(r => r != roleName))
            {
                // Role does not exist, create it
                var roleCreationResult = await _userManager.AddToRoleAsync(user, roleName);

                if (!roleCreationResult.Succeeded)
                {
                    return RoleAdditionFailed(email, roleName);
                }
            }
        }
        else
        {
            if (roleExists.Any(r => r == roleName))
            {
                // Remove the user from the role
                var roleRemovalResult = await _userManager.RemoveFromRoleAsync(user, roleName);

                if (!roleRemovalResult.Succeeded)
                {
                    return RoleRemovalFailed(email, roleName);
                }
            }
        }

        return new AuthResult(true, user.Email, user.UserName, "");
    }
    
    public async Task<AuthResult> UpdateUserAsync(string email, string newEmail, string newPassword, string newName)
    {
        var managedUser = await _userManager.FindByEmailAsync(email);

        if (managedUser == null)
        {
            return UserNotFound();
        }
        
        if (!string.IsNullOrEmpty(newEmail))
        {
            managedUser.Email = newEmail;
        }

        if (!string.IsNullOrEmpty(newPassword))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(managedUser);
            var resetResult = await _userManager.ResetPasswordAsync(managedUser, token, newPassword);
        
            if (!resetResult.Succeeded)
            {
                return PasswordResetFailed(email);
            }
        }

        if (!string.IsNullOrEmpty(newName))
        {
            managedUser.UserName = newName;
        }
       
        var updateResult = await _userManager.UpdateAsync(managedUser);

        if (!updateResult.Succeeded)
        {
            return UserUpdateFailed(email);
        }

        return new AuthResult(true, managedUser.Email, managedUser.UserName, "");
    }

    private AuthResult PasswordResetFailed(string email)
    {
        var result = new AuthResult(false, email, "", "");
        result.ErrorMessages.Add("Password reset failed", "Unable to reset the user's password.");
        return result;
    }

    private AuthResult UserUpdateFailed(string email)
    {
        var result = new AuthResult(false, email, "", "");
        result.ErrorMessages.Add("User update failed", "Failed to update the user's information.");
        return result;
    }

    private AuthResult UserNotFound()
    {
        var result = new AuthResult(false, "", "", "");
        result.ErrorMessages.Add("User not found", "User with the provided data was not found.");
        return result;
    }

    private AuthResult RoleAdditionFailed(string email, string roleName)
    {
        var result = new AuthResult(false, email, "", "");
        result.ErrorMessages.Add("Role addition failed", $"Failed to add the user to the role '{roleName}'.");
        return result;
    }

    private AuthResult RoleRemovalFailed(string email, string roleName)
    {
        var result = new AuthResult(false, email, "", "");
        result.ErrorMessages.Add("Role removal failed", $"Failed to remove the user from the role '{roleName}'.");
        return result;
    }

    private AuthResult InvalidPassword(string email, string userName)
    {
        var result = new AuthResult(false, email, userName, "");
        result.ErrorMessages.Add("Bad credentials", "Invalid password");
        return result;
    }

    private AuthResult InvalidEmail(string email)
    {
        var result = new AuthResult(false, email, "", "");
        result.ErrorMessages.Add("Bad credentials", "Invalid email");
        return result;
    }

    private static AuthResult FailedRegistration(IdentityResult result, string email, string username)
    {
        var authResult = new AuthResult(false, email, username, "");

        foreach (var error in result.Errors)
        {
            authResult.ErrorMessages.Add(error.Code, error.Description);
        }

        return authResult;
    }
}