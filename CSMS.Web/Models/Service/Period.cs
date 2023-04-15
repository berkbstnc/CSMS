using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CSMS.Web.Models.Service
{
    public class Period
    {
        [Key]
        public int PeriodId { get; set; }
        [ForeignKey("Fault")]
        public int FaultId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public virtual FaultRecord Fault { get; set; }
    }
}