using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
   public class OneTimePasswordMap : EntityTypeConfiguration<OneTimePassword>
    {

       public OneTimePasswordMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
         //   this.HasKey(t => new { t.Email, t.DeviceId });

            // Properties



            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);


            this.Property(t => t.UserEmail)
                .IsRequired()
                .HasMaxLength(40)  
                .IsUnicode(false);
          

            this.Property(t => t.UserId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);


            this.Property(t => t.Tenant)
                   .IsRequired();
        



            this.Property(t => t.CreateDate)
                 .IsRequired();
       
           
           this.Property(t => t.ExpirationDate)
            .IsRequired();


            this.Property(t => t.UsageDate);


            // Table & Column Mappings
            this.ToTable("OneTimePasswords");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.UserEmail).HasColumnName("UserEmail");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.UsageDate).HasColumnName("UsageDate");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.ExpirationDate).HasColumnName("ExpirationDate");
            this.Property(t => t.IsUsed).HasColumnName("IsUsed");
      

        }
    }
}
