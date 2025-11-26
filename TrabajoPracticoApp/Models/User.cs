using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Identity.Client;

namespace TrabajoPracticoApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool  EmailConfirmed { get; set; }
        public string? EmailConfirmationToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpiryTime { get; set; }
        
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }



    }
}
