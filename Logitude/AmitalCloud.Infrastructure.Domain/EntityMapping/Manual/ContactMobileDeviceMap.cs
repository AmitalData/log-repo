using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class ContactMobileDeviceMap : EntityTypeConfiguration<ContactMobileDevice>
    {
        public ContactMobileDeviceMap()
        {
            // Primary Key
            this.HasKey(t => t.DeviceId);
         //   this.HasKey(t => new { t.Email, t.DeviceId });

            // Properties
            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(70)  
                 .IsUnicode(false);
           

            this.Property(t => t.DeviceId)
                   .IsRequired()
                .HasMaxLength(500)
                 .IsUnicode(false);

            this.Property(t => t.NotificationUniqueKey)
                   .IsRequired()
                .HasMaxLength(50)
                 .IsUnicode(false);
             


            // Properties
            this.Property(t => t.Platform)
                .IsRequired()
                .HasMaxLength(10)
                 .IsUnicode(false);


            this.Property(t => t.Devicetype)
                
                .HasMaxLength(200)
                 .IsUnicode(false);


            this.Property(t => t.Version)
              .HasMaxLength(10)
               .IsUnicode(false);


            this.Property(t => t.AppVersion)
              .HasMaxLength(10)
               .IsUnicode(false);
            
             

            this.Property(t => t.CreateDate)
                 .IsRequired();



            this.Property(t => t.UpdateDate);


            // Table & Column Mappings
            this.ToTable("ContactMobileDevices");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.DeviceId).HasColumnName("DeviceId");
            this.Property(t => t.Devicetype).HasColumnName("Devicetype");
            this.Property(t => t.Platform).HasColumnName("Platform");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.IsSignOut).HasColumnName("IsSignOut");
      

        }



    }
}
