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
 
    public class CustomsSettingMap : EntityTypeConfiguration<CustomsSetting>
    {
	    string dbms;
        public CustomsSettingMap()
        { 
			  this.ToTable("CustomsSettings", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.IsConnectedToUniFreight).HasColumnName("IsConnectedToUniFreight");

            this.Property(t => t.CustomsAgentId).HasColumnName("CustomsAgentId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SignServiceAddress).HasColumnName("SignServiceAddress").HasMaxLength(2048).IsUnicode(false);

            this.Property(t => t.IIGServiceAddress).HasColumnName("IIGServiceAddress").HasMaxLength(2048).IsUnicode(false);

            this.Property(t => t.DCAServiceAddress).HasColumnName("DCAServiceAddress").HasMaxLength(2048).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.DCAPartnerVault).HasColumnName("DCAPartnerVault").HasMaxLength(6).IsUnicode(false);

            this.Property(t => t.UServerServiceAddress).HasColumnName("UServerServiceAddress").HasMaxLength(2048).IsUnicode(false);

            this.Property(t => t.DefaultNotificationAssignee).HasColumnName("DefaultNotificationAssignee").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.CustomsEnvoirmentTypeCode).HasColumnName("CustomsEnvoirmentTypeCode").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.OnPremiseFillingService).HasColumnName("OnPremiseFillingService").HasMaxLength(2048).IsUnicode(false);

            this.Property(t => t.UnfConnectionString).HasColumnName("UnfConnectionString").HasMaxLength(512).IsUnicode(false);

            this.Property(t => t.TehilaDca).HasColumnName("TehilaDca");

            this.Property(t => t.BlockAgentBankForMasab).HasColumnName("BlockAgentBankForMasab");

            this.Property(t => t.PaymentOrderAccCard).HasColumnName("PaymentOrderAccCard").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.UnifreightCertificateActivated).HasColumnName("UnifreightCertificateActivated");

            this.Property(t => t.AutoFillPaymentScreen).HasColumnName("AutoFillPaymentScreen");

            this.Property(t => t.AutoFillAccountType).HasColumnName("AutoFillAccountType");

            this.Property(t => t.AutoUnitMeasurement).HasColumnName("AutoUnitMeasurement");

            this.Property(t => t.CompanyType).HasColumnName("CompanyType").IsRequired().HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.IsMessagesPending).HasColumnName("IsMessagesPending");

            this.Property(t => t.QtyFeedbackInPendingMessage).HasColumnName("QtyFeedbackInPendingMessage");

            this.Property(t => t.LastRunningDCAWS).HasColumnName("LastRunningDCAWS");

            this.Property(t => t.LastNumOfMessagesDCAWS).HasColumnName("LastNumOfMessagesDCAWS");

            this.Property(t => t.SuppressIIGMessageFromDate).HasColumnName("SuppressIIGMessageFromDate");

            this.Property(t => t.SuppressIIGMessageToDate).HasColumnName("SuppressIIGMessageToDate");

            this.Property(t => t.HSMCompanyId).HasColumnName("HSMCompanyId").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.HSMToken).HasColumnName("HSMToken").HasMaxLength(1024).IsUnicode(false);

            this.Property(t => t.StandAlone).HasColumnName("StandAlone");

            this.Property(t => t.MaxItemsSendInteractive).HasColumnName("MaxItemsSendInteractive");

            this.Property(t => t.MaxSISendInteractive).HasColumnName("MaxSISendInteractive");
        }
    }
}
	 