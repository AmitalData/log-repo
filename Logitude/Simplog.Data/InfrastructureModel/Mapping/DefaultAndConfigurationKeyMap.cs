using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class DefaultAndConfigurationKeyMap : EntityTypeConfiguration<DefaultAndConfigurationKey>
    {
        public DefaultAndConfigurationKeyMap()
        {
            // Primary Key
            this.HasKey(t => new { t.SetKey, t.Tenant, t.SetType });

            // Properties



            this.Property(t => t.SetType)
                .IsRequired()
                .HasMaxLength(128)
                .IsUnicode(false);


            this.Property(t => t.SetKey)
                .IsRequired()
                .HasMaxLength(36)
                .IsUnicode(false);

            this.Property(t => t.ShortDescription)
                 .HasMaxLength(4000)
                .IsUnicode(false);

            this.Property(t => t.FullDesctiption)
                .HasMaxLength(4000)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("DefaultAndConfigurationKeys");

            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CreateDateTime).HasColumnName("CreateDateTime");
            this.Property(t => t.SetType).HasColumnName("SetType");
            this.Property(t => t.SetKey).HasColumnName("SetKey");
            this.Property(t => t.ShortDescription).HasColumnName("ShortDescription");
            this.Property(t => t.FullDesctiption).HasColumnName("FullDesctiption");

        }
    }
}