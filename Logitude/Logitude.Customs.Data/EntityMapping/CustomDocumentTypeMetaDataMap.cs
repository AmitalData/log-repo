using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data;
 
namespace Logitude.Customs.Data.EntityMapping
{
 
    public class CustomDocumentTypeMetaDataMap : EntityTypeConfiguration<CustomDocumentTypeMetaData>
    {
	    string dbms;
        public CustomDocumentTypeMetaDataMap()
        { 
			  this.ToTable("CustomDocumentTypeMetaData", "Customs");
		
		    this.HasKey(t => new { t.MetaDataTypeCode, t.DocumentTypeCode });
	 
            this.Property(t => t.MetaDataTypeCode).HasColumnName("MetaDataTypeCode").HasMaxLength(6).IsUnicode(false);

            this.Property(t => t.Mandatory).HasColumnName("Mandatory");

            this.Property(t => t.Format).HasColumnName("Format").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.DocumentTypeCode).HasColumnName("DocumentTypeCode").IsRequired().HasMaxLength(7).IsUnicode(false);

            this.Property(t => t.ValuesTable).HasColumnName("ValuesTable").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.IsLeading).HasColumnName("IsLeading");

            this.Property(t => t.Inactive).HasColumnName("Inactive");
        }
    }
}
	 