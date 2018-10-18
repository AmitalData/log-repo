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
 
    public class TapagMap : EntityTypeConfiguration<Tapag>
    {
	    string dbms;
        public TapagMap()
        { 
			  this.ToTable("Tapags", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.TapagNumber).HasColumnName("TapagNumber").HasMaxLength(12).IsUnicode(false);

            this.Property(t => t.LeadingFileNumber).HasColumnName("LeadingFileNumber").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TapagTypeCode).HasColumnName("TapagTypeCode").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.CustomerId).HasColumnName("CustomerId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ImporterId).HasColumnName("ImporterId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CustomsBranchCode).HasColumnName("CustomsBranchCode").HasMaxLength(17).IsUnicode(false);

            this.Property(t => t.ProfessionUnitTypeCode).HasColumnName("ProfessionUnitTypeCode").HasMaxLength(5).IsUnicode(false);

            this.Property(t => t.SpecializationTypeCode).HasColumnName("SpecializationTypeCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.Property(t => t.FollowDate).HasColumnName("FollowDate");

            this.Property(t => t.ValidityDate).HasColumnName("ValidityDate");

            this.Property(t => t.IsClosed).HasColumnName("IsClosed");

            this.Property(t => t.ReferantId).HasColumnName("ReferantId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 