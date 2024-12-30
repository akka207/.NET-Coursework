using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StaffManagerModels;

namespace PM.API.Data
{
    public class PMAPIContext : DbContext
    {
        public PMAPIContext (DbContextOptions<PMAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().Ignore(i => i.IsSelected);
            base.OnModelCreating(modelBuilder);
        }
    }
}
