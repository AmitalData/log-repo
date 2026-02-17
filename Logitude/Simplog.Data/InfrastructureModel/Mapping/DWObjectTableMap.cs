using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class DWObjectTableMap : EntityTypeConfiguration<DWObjectTable>
    {
        
        public DWObjectTableMap()
        {
            this.HasKey(t => t.Code);
            this.Property(t => t.Code).IsRequired().HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Name).IsRequired().HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.TypeCode).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.DefaultFilterBy).HasMaxLength(50).IsUnicode(false); 

            this.ToTable("DWObjectTables");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.TypeCode).HasColumnName("TypeCode");
            this.Property(t => t.IsClosed).HasColumnName("IsClosed");
            this.Property(t => t.DefaultFilterBy).HasColumnName("DefaultFilterBy");


        }
    }
}
