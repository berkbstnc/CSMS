using CSMS.Models;
using CSMS.Models.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CSMS.Web.Models.Service
{
    public class FaultRecord
    {
        [Key]
        public int RecordId { get; set; }
        public int CarId { get; set; }
        public int CarKm { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string Record { get; set; }
        public bool Status { get; set; }
        public virtual Car CustomerCar { get; set; }
    }
}