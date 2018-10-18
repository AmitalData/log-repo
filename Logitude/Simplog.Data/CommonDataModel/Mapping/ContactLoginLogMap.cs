using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class ContactLoginLogMap : EntityTypeConfiguration<ContactLoginLog>
    {
        public ContactLoginLogMap()
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

            this.Property(t => t.ContactId)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.ContactAgent)
                .HasMaxLength(400);

            this.Property(t => t.ComputerId)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);



                this.Property(t => t.Via)
                .HasMaxLength(20)
                .IsUnicode(false);
            

            // Table & Column Mappings
            this.ToTable("ContactLoginLogs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.IP).HasColumnName("IP");
            this.Property(t => t.Browser).HasColumnName("Browser");
            this.Property(t => t.ContactId).HasColumnName("ContactId");
            this.Property(t => t.LocalDateTime).HasColumnName("LocalDateTime");
            this.Property(t => t.GMTDateTime).HasColumnName("GMTDateTime");
            this.Property(t => t.ContactAgent).HasColumnName("ContactAgent");
            this.Property(t => t.ComputerId).HasColumnName("ComputerId");
            this.Property(t => t.Via).HasColumnName("Via");
            // Relationships
            this.HasRequired(t => t.Contact)
                .WithMany()
                .HasForeignKey(d => d.ContactId);

        }
    }
}
