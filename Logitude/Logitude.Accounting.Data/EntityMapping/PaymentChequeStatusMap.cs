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
 
    public class PaymentChequeStatusMap : EntityTypeConfiguration<PaymentChequeStatus>
    {
	    string dbms;
        public PaymentChequeStatusMap()
        { 
				this.ToTable("PaymentChequeStatuses");
		
		    this.HasKey(t => new { t.Code });
	 
            this.Property(t => t.Code).HasColumnName("Code").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EnglishName).HasColumnName("EnglishName").HasMaxLength(120).IsUnicode(true);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.LocalName).HasColumnName("LocalName").HasMaxLength(60).IsUnicode(true);
        }
    }
}
	 