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
            this.Property(t => t.DataViewName).HasMaxLength(200).IsUnicode(true);
            this.Property(t => t.PivotFieldCode).HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.AdditionalFactCode).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.AdditionalFactForeignKey).HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.ParentFactCode).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.RecordType).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.DisplayName).HasMaxLength(100).IsUnicode(false);
           
            this.Property(t => t.IndexesXml).IsMaxLength().IsUnicode(true);

            this.Property(t => t.ObjectTableName).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.AdditionalFactRelationType).HasMaxLength(100).IsUnicode(false);

            this.ToTable("DWObjectTables");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.TypeCode).HasColumnName("TypeCode");
            this.Property(t => t.IsClosed).HasColumnName("IsClosed");
            this.Property(t => t.DefaultFilterBy).HasColumnName("DefaultFilterBy");
            this.Property(t => t.DataViewName).HasColumnName("DataViewName");
            this.Property(t => t.HasPivotColumn).HasColumnName("HasPivotColumn");
            this.Property(t => t.PivotFieldCode).HasColumnName("PivotFieldCode");

            this.Property(t => t.AdditionalFactCode).HasColumnName("AdditionalFactCode");

            this.Property(t => t.AdditionalFactForeignKey).HasColumnName("AdditionalFactForeignKey");

            this.Property(t => t.ParentFactCode).HasColumnName("ParentFactCode");
            this.Property(t => t.RecordType).HasColumnName("RecordType");
            this.Property(t => t.DisplayName).HasColumnName("DisplayName");
            this.Property(t => t.IndexesXml).HasColumnName("IndexesXml");

            this.Property(t => t.HasCustomFields).HasColumnName("HasCustomFields");
            this.Property(t => t.ObjectTableName).HasColumnName("ObjectTableName");
            this.Property(t => t.MaxNumberOfCustomFields).HasColumnName("MaxNumberOfCustomFields");
            this.Property(t => t.AdditionalFactRelationType).HasColumnName("AdditionalFactRelationType");




        }
    }
}
