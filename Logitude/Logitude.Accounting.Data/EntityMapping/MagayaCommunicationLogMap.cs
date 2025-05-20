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
 
    public class MagayaCommunicationLogMap : EntityTypeConfiguration<MagayaCommunicationLog>
    {
	    string dbms;
        public MagayaCommunicationLogMap()
        { 
				this.ToTable("MagayaCommunicationLogs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.CommunicationId).HasColumnName("CommunicationId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Step).HasColumnName("Step").IsRequired().HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.StatusCode).HasColumnName("StatusCode").IsRequired().HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.Exception).HasColumnName("Exception").HasMaxLength(1000).IsUnicode(true);
        }
    }
}
	 