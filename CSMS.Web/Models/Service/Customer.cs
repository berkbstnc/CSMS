using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Models.Service
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        //[ForeignKey]
        //public virtual CSM ApplicationUser
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Plate { get; set; }
        public string Address { get; set; }
    }
}
