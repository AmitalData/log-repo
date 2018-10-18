using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.SystemLogs.POCOs;

namespace Logitude.SystemLogs.Mapping
{
    public class ErrorLogMap : EntityTypeConfiguration<ErrorLog>
    {

        public ErrorLogMap()
        {
         
            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);
            
            this.Property(t => t.UserName)
                .HasMaxLength(100)
                .IsRequired()
                .IsUnicode(false);

            this.Property(t => t.LogDate).IsRequired();

            this.Property(t => t.ClientDate).IsRequired();

            this.Property(t => t.Tier)
                .HasMaxLength(40)
                .IsRequired()
                .IsUnicode(false);

            this.Property(t => t.Exception)
                .HasMaxLength(7000)
                .IsRequired()
                .IsUnicode(false);

            this.Property(t => t.StackTrace)
                .HasMaxLength(7000)
                .IsUnicode(false);

            this.Property(t => t.SearchFields)
                .HasMaxLength(8000)
                .IsUnicode(false);

            this.Property(t => t.IP)
              .HasMaxLength(15)
              .IsUnicode(false);


            this.ToTable("ErrorLogs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.LogDate).HasColumnName("LogDate");
            this.Property(t => t.ClientDate).HasColumnName("ClientDate");
            this.Property(t => t.Tier).HasColumnName("Tier");
            this.Property(t => t.Exception).HasColumnName("Exception");
            this.Property(t => t.StackTrace).HasColumnName("StackTrace");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.IP).HasColumnName("IP");

        }

    }
}
