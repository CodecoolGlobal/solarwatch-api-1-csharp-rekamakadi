using System.ComponentModel.DataAnnotations;

namespace SolarWatch.Contracts;

public record UserRoleModificationRequest([Required]string Email, [Required]string RoleName);