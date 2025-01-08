using BillingApplication.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplication
{
    public class UserConfiguration : IEntityTypeConfiguration<SubscriberEntity>
    {
        public void Configure(EntityTypeBuilder<SubscriberEntity> builder)
        {
            builder.HasKey(x=>x.Id);
            builder.Property(x => x.Email)
                .IsRequired();
            builder.Property(x => x.Password)
                .IsRequired();
        }
    }
}
