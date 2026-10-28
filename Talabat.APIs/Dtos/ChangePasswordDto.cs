using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.Dtos
{
    public class ChangePasswordDto
    {
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Current password is required")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm password does not match new password.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
