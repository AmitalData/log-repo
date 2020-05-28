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
 
    public class CustomsRequiredFieldMap : EntityTypeConfiguration<CustomsRequiredField>
    {
	    string dbms;
        public CustomsRequiredFieldMap()
        { 
			  this.ToTable("CustomsRequiredFields", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ObjectfieldId).HasColumnName("ObjectfieldId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ObjectfieldCode).HasColumnName("ObjectfieldCode").HasMaxLength(100).IsUnicode(false);
        }
    }
}
	 