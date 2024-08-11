using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class ExternalSystemsTablesCodeMap : EntityTypeConfiguration<ExternalSystemsTablesCode>
    {

        public ExternalSystemsTablesCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);
            this.Property(t => t.LogitudeTable)
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.Code)
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .HasMaxLength(120)
                .IsUnicode(true);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("ExternalSystemsTablesCodes");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.LogitudeTable).HasColumnName("LogitudeTable");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
        }

    }
}
