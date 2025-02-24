using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PandaAPI.Models
{
    [Index(nameof(AppointmentId), IsUnique = true)]
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        public Guid AppointmentId { get; set; } = Guid.NewGuid();

        [Required]
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; } = new Patient();

        public string Status { get; set; } = string.Empty;

        [Required]
        public DateTime Time { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

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