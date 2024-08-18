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
 
    public class OpportunityClosingReasonMap : EntityTypeConfiguration<OpportunityClosingReason>
    {
	    string dbms;
        public OpportunityClosingReasonMap()
        { 
				this.ToTable("OpportunityClosingReasons");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.Code).HasColumnName("Code").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(60).IsUnicode(false);

            this.Property(t => t.LocalName).HasColumnName("LocalName").HasMaxLength(60).IsUnicode(true);

            this.Property(t => t.IsClosedLost).HasColumnName("IsClosedLost").IsRequired();

            this.Property(t => t.AddedManually).HasColumnName("AddedManually").IsRequired();

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.InActive).HasColumnName("InActive").IsRequired();
        }
    }
}
	 