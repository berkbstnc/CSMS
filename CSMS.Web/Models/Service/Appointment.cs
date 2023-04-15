using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSMS.Models.Service
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required]
        [ForeignKey("Car")]
        public int CarId { get; set; }

        [Required]
        [ForeignKey("MechanicUser")]
        public string MechanicUserId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        public string Description { get; set; }

        [Required]
        public int Status { get; set; }


        public virtual Car Car { get; set; }
        public virtual ApplicationUser MechanicUser { get; set; }
    }
}
