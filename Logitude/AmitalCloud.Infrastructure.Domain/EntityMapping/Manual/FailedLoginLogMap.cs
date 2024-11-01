using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    
     
    public class FailedLoginLogMap : EntityTypeConfiguration<FailedLoginLog>
    {
        public FailedLoginLogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.IP)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Browser)
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.Email).HasMaxLength(70).IsUnicode(false);


            this.Property(t => t.UserAgent)
                .HasMaxLength(400)
                .IsUnicode(true);

            this.Property(t => t.Reason)
                .HasMaxLength(200)
                .IsUnicode(false);


            // Table & Column Mappings
            this.ToTable("FailedLoginLogs");
            this.Property(t => t.Id).HasColumnName("Id");

            this.Property(t => t.IP).HasColumnName("IP");
            this.Property(t => t.Browser).HasColumnName("Browser");

            this.Property(t => t.GMTDateTime).HasColumnName("GMTDateTime");
            this.Property(t => t.UserAgent).HasColumnName("UserAgent");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Reason).HasColumnName("Reason");


        }
    }



}
