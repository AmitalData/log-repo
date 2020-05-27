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
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data;
 
namespace Logitude.Accounting.Data.EntityMapping
{
 
    public class InterestBasesTypeMap : EntityTypeConfiguration<InterestBasesType>
    {
	    string dbms;
        public InterestBasesTypeMap()
        { 
				this.ToTable("InterestBasesTypes");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.Code).HasColumnName("Code").IsRequired().HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.LocalName).HasColumnName("LocalName").HasMaxLength(256).IsUnicode(true);

            this.Property(t => t.EnglishName).HasColumnName("EnglishName").HasMaxLength(256).IsUnicode(false);

            this.Property(t => t.Description).HasColumnName("Description").HasMaxLength(1024).IsUnicode(true);

            this.Property(t => t.InActive).HasColumnName("InActive");
        }
    }
}
	 