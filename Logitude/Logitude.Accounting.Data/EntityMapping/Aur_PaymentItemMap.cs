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
 
    public class Aur_PaymentItemMap : EntityTypeConfiguration<Aur_PaymentItem>
    {
	    string dbms;
        public Aur_PaymentItemMap()
        { 
				this.ToTable("Aur_PaymentItems");
		
		    this.HasKey(t => new { t.PaymentId, t.Line });
	 
            this.Property(t => t.PaymentId).HasColumnName("PaymentId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Line).HasColumnName("Line").HasDatabaseGeneratedOption(null);

            this.Property(t => t.PaymentSequence).HasColumnName("PaymentSequence");

            this.Property(t => t.QuoteId).HasColumnName("QuoteId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Project).HasColumnName("Project").HasMaxLength(20).IsUnicode(true);

            this.Property(t => t.ProjectNumber).HasColumnName("ProjectNumber").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SectionType).HasColumnName("SectionType").HasMaxLength(20).IsUnicode(true);

            this.Property(t => t.BaseAmount).HasColumnName("BaseAmount").HasPrecision(10, 2);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();
        }
    }
}
	 