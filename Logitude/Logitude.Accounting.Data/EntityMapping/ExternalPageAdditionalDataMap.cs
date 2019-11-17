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
 
    public class ExternalPageAdditionalDataMap : EntityTypeConfiguration<ExternalPageAdditionalData>
    {
	    string dbms;
        public ExternalPageAdditionalDataMap()
        { 
				this.ToTable("ExternalPageAdditionalDatas");
		
		    this.HasKey(t => new { t.ObjectTableId, t.EntityId });
	 
            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EntityId).HasColumnName("EntityId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LastPageNumber).HasColumnName("LastPageNumber").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LastPageEndDate).HasColumnName("LastPageEndDate");

            this.Property(t => t.LastPageCloseBalance).HasColumnName("LastPageCloseBalance").HasPrecision(16, 2);
        }
    }
}
	 