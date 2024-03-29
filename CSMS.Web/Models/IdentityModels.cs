﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using CSMS.Models.Service;
using CSMS.Web.Models.Service;
using System.Collections.Generic;
using System;

namespace CSMS.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public List<FaultRecord> FaultRecords { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DataContext", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
            modelBuilder.Entity<Car>()
             .HasRequired(n => n.ApplicationUser)
             .WithMany(a => a.Cars)
             .HasForeignKey(n => n.ApplicationUserId)
             .WillCascadeOnDelete(false);
        }
        public DbSet<Car> Cars { get; set; }
        public DbSet<FaultRecord> FaultRecords { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Period> Periods { get; set; }
    }
}