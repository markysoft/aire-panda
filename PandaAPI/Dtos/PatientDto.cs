using System.ComponentModel.DataAnnotations;

namespace PandaAPI.Dtos
{
    public class PatientDto
    {
        [Required]
        [StringLength(10)]
        public string NhsNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string DateOfBirth { get; set; } = string.Empty;

        [Required]
        [StringLength(8)]
        public string Postcode { get; set; } = string.Empty;
    }
}