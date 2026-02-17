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
 
    public class CustomsDocumentMetaDataValueMap : EntityTypeConfiguration<CustomsDocumentMetaDataValue>
    {
	    string dbms;
        public CustomsDocumentMetaDataValueMap()
        { 
			  this.ToTable("CustomsDocumentMetaDataValues", "Customs");
		
		    this.HasKey(t => new { t.CustomsDocumentId, t.MetaDataTypeCode });
	 
            this.Property(t => t.CustomsDocumentId).HasColumnName("CustomsDocumentId").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.MetaDataTypeCode).HasColumnName("MetaDataTypeCode").HasMaxLength(6).IsUnicode(false);

            this.Property(t => t.MetaDataValue).HasColumnName("MetaDataValue").HasMaxLength(128).IsUnicode(false);
        }
    }
}
	 