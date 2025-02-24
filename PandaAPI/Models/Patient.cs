using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PandaAPI.Models
{
    [Index(nameof(NhsNumber), IsUnique = true)]
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string NhsNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        [StringLength(8)]
        public string Postcode { get; set; } = string.Empty;
    }
}