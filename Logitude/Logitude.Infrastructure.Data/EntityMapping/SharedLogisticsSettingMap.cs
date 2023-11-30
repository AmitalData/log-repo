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
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data;
 
namespace Logitude.Infrastructure.Data.EntityMapping
{
 
    public class SharedLogisticsSettingMap : EntityTypeConfiguration<SharedLogisticsSetting>
    {
	    string dbms;
        public SharedLogisticsSettingMap()
        { 
				this.ToTable("SharedLogisticsSettings");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.IsAgentShared).HasColumnName("IsAgentShared");

            this.Property(t => t.IsShipperNotExporterShared).HasColumnName("IsShipperNotExporterShared");

            this.Property(t => t.IsNotify1Shared).HasColumnName("IsNotify1Shared");

            this.Property(t => t.IsNotify2Shared).HasColumnName("IsNotify2Shared");

            this.Property(t => t.IsFreightForwarderShared).HasColumnName("IsFreightForwarderShared");

            this.Property(t => t.IsColoaderShared).HasColumnName("IsColoaderShared");

            this.Property(t => t.IsConsigneeNotImporterShared).HasColumnName("IsConsigneeNotImporterShared");

            this.Property(t => t.IsMainCarrierShared).HasColumnName("IsMainCarrierShared");

            this.Property(t => t.IsPickDelivCarriesShared).HasColumnName("IsPickDelivCarriesShared");

            this.Property(t => t.IsInvoicesMenuEnabled).HasColumnName("IsInvoicesMenuEnabled");

            this.Property(t => t.IsMoneyTabEnabled).HasColumnName("IsMoneyTabEnabled");

            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsIssuingCarrierAgentShared).HasColumnName("IsIssuingCarrierAgentShared");

            this.Property(t => t.IsCustomsAgentExportShared).HasColumnName("IsCustomsAgentExportShared");

            this.Property(t => t.IsCustomsAgentImportShared).HasColumnName("IsCustomsAgentImportShared");

            this.Property(t => t.IsCustomClearancePoinShared).HasColumnName("IsCustomClearancePoinShared");

            this.Property(t => t.IsConsolidatorShared).HasColumnName("IsConsolidatorShared");

            this.Property(t => t.IsReleasingAgentShared).HasColumnName("IsReleasingAgentShared");

            this.Property(t => t.IsShipperShared).HasColumnName("IsShipperShared");

            this.Property(t => t.IsConsigneeShared).HasColumnName("IsConsigneeShared");

            this.Property(t => t.IsShowAmountLocalCurrency).HasColumnName("IsShowAmountLocalCurrency");

            this.Property(t => t.IsShipperShowContactTS).HasColumnName("IsShipperShowContactTS");

            this.Property(t => t.IsConsigneeShowContactTS).HasColumnName("IsConsigneeShowContactTS");

            this.Property(t => t.IsAgentShowContactTS).HasColumnName("IsAgentShowContactTS");

            this.Property(t => t.IsShipperNotExShowContactTS).HasColumnName("IsShipperNotExShowContactTS");

            this.Property(t => t.IsConsigneeNotImShowContactTS).HasColumnName("IsConsigneeNotImShowContactTS");

            this.Property(t => t.IsNotify1ShowContactTS).HasColumnName("IsNotify1ShowContactTS");

            this.Property(t => t.IsNotify2ShowContactTS).HasColumnName("IsNotify2ShowContactTS");

            this.Property(t => t.IsFreightForwardShowContactTS).HasColumnName("IsFreightForwardShowContactTS");

            this.Property(t => t.IsColoaderShowContactTS).HasColumnName("IsColoaderShowContactTS");

            this.Property(t => t.IsCustomAgentExShowContactTS).HasColumnName("IsCustomAgentExShowContactTS");

            this.Property(t => t.IsCustomAgentImShowContactTS).HasColumnName("IsCustomAgentImShowContactTS");

            this.Property(t => t.IsCustomCleaPointShowContactTS).HasColumnName("IsCustomCleaPointShowContactTS");

            this.Property(t => t.IsConsolidatorShowContactTS).HasColumnName("IsConsolidatorShowContactTS");

            this.Property(t => t.IsReleasingAgentShowContactTS).HasColumnName("IsReleasingAgentShowContactTS");

            this.Property(t => t.IsIssuingCarAgentShowContactTS).HasColumnName("IsIssuingCarAgentShowContactTS");

            this.Property(t => t.IsCustomerShared).HasColumnName("IsCustomerShared");

            this.Property(t => t.IsCustomerShowContactTS).HasColumnName("IsCustomerShowContactTS");

            this.Property(t => t.IsAccountManagerShared).HasColumnName("IsAccountManagerShared");

            this.Property(t => t.IsAccountManagerShowContactTS).HasColumnName("IsAccountManagerShowContactTS");

            this.Property(t => t.IsSalesmanShared).HasColumnName("IsSalesmanShared");

            this.Property(t => t.IsSalesmanShowContactTS).HasColumnName("IsSalesmanShowContactTS");

            this.Property(t => t.IsCollectorShared).HasColumnName("IsCollectorShared");

            this.Property(t => t.IsCollectorShowContactTS).HasColumnName("IsCollectorShowContactTS");

            this.Property(t => t.IsPickDelivCarShowContactTS).HasColumnName("IsPickDelivCarShowContactTS");

            this.Property(t => t.IsMainCarShowContactTS).HasColumnName("IsMainCarShowContactTS");
        }
    }
}
	 