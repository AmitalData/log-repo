using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class ChangePasswordLogMap : EntityTypeConfiguration<ChangePasswordLog>
    {
     public ChangePasswordLogMap()
        {

            // Primary Key


            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
                  .IsRequired()
                  .HasMaxLength(40)
                  .IsUnicode(false);


            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(70)
                .IsUnicode(false);

            this.Property(t => t.EnteredPassword)
                .HasMaxLength(60)
                .IsUnicode(false);

            this.Property(t => t.CurrentPassword)
                .HasMaxLength(60)
                .IsUnicode(false);

            this.Property(t => t.log)
              .IsRequired()
              .HasMaxLength(500)
              .IsUnicode(false);


            this.Property(t => t.CreateDate)
                 .IsRequired();


            this.Property(t => t.IP)
                .HasMaxLength(15)
                .IsUnicode(false);


            // Table & Column Mappings
            this.ToTable("ChangePasswordLogs");

            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.EnteredPassword).HasColumnName("EnteredPassword");
            this.Property(t => t.CurrentPassword).HasColumnName("CurrentPassword");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.IP).HasColumnName("IP");
            this.Property(t => t.log).HasColumnName("log");


        }

    }
}
