using BillingApplication.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplication
{
    public class BillingAppDbContext : DbContext
    {
        public BillingAppDbContext(DbContextOptions<BillingAppDbContext> options)
            :base(options)
        {
            
        }

        public DbSet<UserEntity> users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<UserBoost>()
            //    .HasOne(ub => ub.User)
            //    .WithMany()
            //    .HasForeignKey(p => p.UserId);

            //builder.Entity<UserBoost>()
            //    .HasOne(ub => ub.Boost)
            //    .WithMany()
            //    .HasForeignKey(p => p.BoostId);
        }
    }
}
