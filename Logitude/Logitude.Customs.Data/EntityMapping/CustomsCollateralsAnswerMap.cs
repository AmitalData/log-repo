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
 
    public class CustomsCollateralsAnswerMap : EntityTypeConfiguration<CustomsCollateralsAnswer>
    {
	    string dbms;
        public CustomsCollateralsAnswerMap()
        { 
			  this.ToTable("CustomsCollateralsAnswers", "Customs");
		
		    this.HasKey(t => new { t.CustomsCollateralId, t.LineNumber });
	 
            this.Property(t => t.CustomsCollateralId).HasColumnName("CustomsCollateralId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LineNumber).HasColumnName("LineNumber").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.AnswerEntityTypeCode).HasColumnName("AnswerEntityTypeCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.AllocatedAmount).HasColumnName("AllocatedAmount");

            this.Property(t => t.Remarks).HasColumnName("Remarks").HasMaxLength(512).IsUnicode(true);

            this.Property(t => t.CustomsTapgFile).HasColumnName("CustomsTapgFile").HasMaxLength(25).IsUnicode(false);

            this.Property(t => t.CustomsNumeral).HasColumnName("CustomsNumeral").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.AnswerForCollateralStatusCode).HasColumnName("AnswerForCollateralStatusCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.Errors).HasColumnName("Errors").HasMaxLength(1024).IsUnicode(true);

            this.Property(t => t.TapagId).HasColumnName("TapagId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.NewFileRequest).HasColumnName("NewFileRequest");

            this.Property(t => t.RequestFileTypeCode).HasColumnName("RequestFileTypeCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.RequestFileAmount).HasColumnName("RequestFileAmount").HasPrecision(18, 2);

            this.Property(t => t.IsClosed).HasColumnName("IsClosed");

            this.Property(t => t.RequestedTapagNumeral).HasColumnName("RequestedTapagNumeral").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.RequestedTapagFile).HasColumnName("RequestedTapagFile").HasMaxLength(25).IsUnicode(false);

            this.Property(t => t.PaymentOrderId).HasColumnName("PaymentOrderId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 