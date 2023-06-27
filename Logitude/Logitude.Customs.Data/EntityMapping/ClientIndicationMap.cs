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
 
    public class ClientIndicationMap : EntityTypeConfiguration<ClientIndication>
    {
	    string dbms;
        public ClientIndicationMap()
        { 
			  this.ToTable("ClientIndications", "Customs");
		
		    this.HasKey(t => new { t.IndicationId, t.ClientId });
	 
            this.Property(t => t.IndicationId).HasColumnName("IndicationId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.ClientId).HasColumnName("ClientId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CustomerIndicationTypeID).HasColumnName("CustomerIndicationTypeID").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.IsActive).HasColumnName("IsActive");

            this.Property(t => t.StartDate).HasColumnName("StartDate");

            this.Property(t => t.EndDate).HasColumnName("EndDate");

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
        }
    }
}
	 