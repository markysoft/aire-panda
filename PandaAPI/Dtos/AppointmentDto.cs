using PandaAPI.Validators;
using System.ComponentModel.DataAnnotations;

namespace PandaAPI.Dtos
{
    public class AppointmentDto
    {
        [Required]
        public Guid Id { get; set; }

        public string Patient { get; set; } = string.Empty;

        [Required]
        [RegularExpression(Constants.StatusRegex, ErrorMessage = Constants.StatusErrorMessage)]
        public string Status { get; set; } = string.Empty;

        [Required]
        public DateTime Time { get; set; }

        [Required]
        [DurationValidation]
        public string Duration { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Clinician { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Department { get; set; } = string.Empty;

        [Required]
        [StringLength(8)]
        public string Postcode { get; set; } = string.Empty;
    }
}