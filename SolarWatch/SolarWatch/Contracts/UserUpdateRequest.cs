using System.ComponentModel.DataAnnotations;

namespace SolarWatch.Contracts;

public record UserUpdateRequest([Required]string Email, string NewEmail, string NewPassword, string NewName);