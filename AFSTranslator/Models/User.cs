using System.ComponentModel.DataAnnotations;

namespace AFSTranslator.Models
{
    public class User
    {
        public int Id { get; set; }

        // [Required]
        [StringLength(100)]
        public required string Username { get; set; }

        // [Required]
        [StringLength(256)]
        public required string PasswordHash { get; set; }
    }
}