using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CSMS.Models;
using CSMS.Models.Service;
using CSMS.Web.Models.Service;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CSMS.Web
{
    public class DataContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<FaultRecord> FaultRecords { get; set; }
    }
}
