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
 
    public class SIIRequestMap : EntityTypeConfiguration<SIIRequest>
    {
	    string dbms;
        public SIIRequestMap()
        { 
			  this.ToTable("SIIRequests", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.RequestNo).HasColumnName("RequestNo").HasMaxLength(9).IsUnicode(false);

            this.Property(t => t.DeclarationId).HasColumnName("DeclarationId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Status).HasColumnName("Status").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.WareHouseAddress).HasColumnName("WareHouseAddress").HasMaxLength(60).IsUnicode(true);

            this.Property(t => t.WareHouseCity).HasColumnName("WareHouseCity").HasMaxLength(4).IsUnicode(true);

            this.Property(t => t.IsClosed).HasColumnName("IsClosed");
        }
    }
}
	 