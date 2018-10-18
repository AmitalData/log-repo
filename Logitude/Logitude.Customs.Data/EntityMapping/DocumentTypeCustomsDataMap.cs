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
 
    public class DocumentTypeCustomsDataMap : EntityTypeConfiguration<DocumentTypeCustomsData>
    {
	    string dbms;
        public DocumentTypeCustomsDataMap()
        { 
			  this.ToTable("DocumentTypeCustomsData", "Customs");
		
		    this.HasKey(t => new { t.DocumentTypeId });
	 
            this.Property(t => t.DocumentTypeId).HasColumnName("DocumentTypeId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CustomsDoucumentTypeCode).HasColumnName("CustomsDoucumentTypeCode").IsRequired().HasMaxLength(7).IsUnicode(false);
        }
    }
}
	 