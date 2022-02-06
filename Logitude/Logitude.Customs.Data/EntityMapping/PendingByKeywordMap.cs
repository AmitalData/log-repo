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
 
    public class PendingByKeywordMap : EntityTypeConfiguration<PendingByKeyword>
    {
	    string dbms;
        public PendingByKeywordMap()
        { 
			  this.ToTable("PendingByKeywords", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CourierPendingReasonCode).HasColumnName("CourierPendingReasonCode").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.KeywordsList).HasColumnName("KeywordsList").HasMaxLength(2000).IsUnicode(true);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(2000).IsUnicode(true);

            this.Property(t => t.SearchByFieldCode).HasColumnName("SearchByFieldCode").IsRequired().HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.SearchType).HasColumnName("SearchType").IsRequired().HasMaxLength(2).IsUnicode(false);
        }
    }
}
	 