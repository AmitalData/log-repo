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
 
    public class OpportunityProductLocationMap : EntityTypeConfiguration<OpportunityProductLocation>
    {
	    string dbms;
        public OpportunityProductLocationMap()
        { 
				this.ToTable("OpportunityProductLocations");
		
		    this.HasKey(t => new { t.OpportunityId, t.OpportunityProductTypeCode, t.LineNumber });
	 
            this.Property(t => t.OpportunityId).HasColumnName("OpportunityId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.OpportunityProductTypeCode).HasColumnName("OpportunityProductTypeCode").IsRequired().HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.LineNumber).HasColumnName("LineNumber").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CountryId).HasColumnName("CountryId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TEU).HasColumnName("TEU").HasPrecision(18, 2);

            this.Property(t => t.NumberOfShipments).HasColumnName("NumberOfShipments");

            this.Property(t => t.ChargeableWeight).HasColumnName("ChargeableWeight").HasPrecision(18, 2);

            this.Property(t => t.Revenue).HasColumnName("Revenue").HasPrecision(18, 2);
        }
    }
}
	 