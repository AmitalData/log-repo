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
 
    public class OcrDocumentMap : EntityTypeConfiguration<OcrDocument>
    {
	    string dbms;
        public OcrDocumentMap()
        { 
			  this.ToTable("OcrDocuments", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.Process).HasColumnName("Process").HasMaxLength(5).IsUnicode(false);

            this.Property(t => t.JsonData).HasColumnName("JsonData").HasMaxLength(500).IsUnicode(false);

            this.Property(t => t.Score).HasColumnName("Score").HasPrecision(3, 2);

            this.Property(t => t.JsonTif).HasColumnName("JsonTif").HasMaxLength(500).IsUnicode(false);

            this.Property(t => t.ErrorMsg).HasColumnName("ErrorMsg").HasMaxLength(1000).IsUnicode(false);

            this.Property(t => t.StatusCode).HasColumnName("StatusCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.OcrId).HasColumnName("OcrId").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.DocId).HasColumnName("DocId").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.Reference).HasColumnName("Reference").HasMaxLength(30).IsUnicode(false);
        }
    }
}
	 