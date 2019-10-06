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
 
    public class OccasionInviteeMap : EntityTypeConfiguration<OccasionInvitee>
    {
	    string dbms;
        public OccasionInviteeMap()
        { 
				this.ToTable("OccasionInvitees");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.AddedDate).HasColumnName("AddedDate").IsRequired();

            this.Property(t => t.AddedByUserId).HasColumnName("AddedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
              this.Property(t => t.Notes).HasMaxLength(2000);
			}
            else
            {
              this.Property(t => t.Notes).HasMaxLength(4000);
			}


            this.Property(t => t.Notes).HasColumnName("Notes").IsUnicode(true);

            this.Property(t => t.OccasionId).HasColumnName("OccasionId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ContactId).HasColumnName("ContactId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Invited).HasColumnName("Invited");

            this.Property(t => t.Participated).HasColumnName("Participated");
        }
    }
}
	 