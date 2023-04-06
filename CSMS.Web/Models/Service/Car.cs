using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Models.Service
{
    public class Car
    {
        [Key]
        public int CarId { get; set; }
        public string ApplicationUserId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }
        public string Plate { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
