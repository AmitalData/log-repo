using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class FailedTokenLogMap : EntityTypeConfiguration<FailedTokenLog>
    {
        
        public FailedTokenLogMap()
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

            // Properties

            this.Property(t => t.Token)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);


            // Table & Column Mappings
            this.ToTable("FailedTokenLogs");
            this.Property(t => t.Id).HasColumnName("Id");

            this.Property(t => t.IP).HasColumnName("IP");
            this.Property(t => t.Browser).HasColumnName("Browser");

            this.Property(t => t.GMTDateTime).HasColumnName("GMTDateTime");
            this.Property(t => t.Token).HasColumnName("Token");

        }
    }
}
