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
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data;
 
namespace Logitude.CRM.Data.EntityMapping
{
 
    public class OccasionMap : EntityTypeConfiguration<Occasion>
    {
	    string dbms;
        public OccasionMap()
        { 
				this.ToTable("Occasions");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.StartDateTime).HasColumnName("StartDateTime");

            this.Property(t => t.EndDateTime).HasColumnName("EndDateTime");

            this.Property(t => t.Goal).HasColumnName("Goal").HasMaxLength(500).IsUnicode(true);

            this.Property(t => t.Location).HasColumnName("Location").HasMaxLength(500).IsUnicode(true);

            this.Property(t => t.OwnerId).HasColumnName("OwnerId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IndustryId).HasColumnName("IndustryId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.OccasionTypeId).HasColumnName("OccasionTypeId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.OccasionStatusId).HasColumnName("OccasionStatusId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 