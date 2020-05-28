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
 
    public class InterestBasesPeriodMap : EntityTypeConfiguration<InterestBasesPeriod>
    {
	    string dbms;
        public InterestBasesPeriodMap()
        { 
				this.ToTable("InterestBasesPeriods");
		
		    this.HasKey(t => new { t.InterestBaseTypeId, t.LineNumber });
	 
            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.InterestBaseTypeId).HasColumnName("InterestBaseTypeId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LineNumber).HasColumnName("LineNumber").HasDatabaseGeneratedOption(null);

            this.Property(t => t.InterestBaseStartDate).HasColumnName("InterestBaseStartDate").IsRequired();

            this.Property(t => t.InterestRate).HasColumnName("InterestRate").IsRequired().HasPrecision(4, 2);
        }
    }
}
	 