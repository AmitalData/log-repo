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
 
    public class DeclarationPendingMap : EntityTypeConfiguration<DeclarationPending>
    {
	    string dbms;
        public DeclarationPendingMap()
        { 
			  this.ToTable("DeclarationPendings", "Customs");
		
		    this.HasKey(t => new { t.DeclarationID, t.CourierPendingReasonCode });
	 
            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.DeclarationID).HasColumnName("DeclarationID").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CourierPendingReasonCode).HasColumnName("CourierPendingReasonCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.PendingRemarks).HasColumnName("PendingRemarks").HasMaxLength(1024).IsUnicode(true);

            this.Property(t => t.Status).HasColumnName("Status").HasMaxLength(1).IsUnicode(true);
        }
    }
}
	 