using Logitude.Infrastructure.Data.Repsitories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Infrastructure.Data.ExtendedServices
{
    public class AvailableForSchedulingBIReportService
    {
        private List<DWObjectTable> dWObjectTables;
        public AvailableForSchedulingBIReportService()
        {
            DWObjectTableRepository dWObjectTableRepository = new DWObjectTableRepository(0);
            dWObjectTables = dWObjectTableRepository.GetDWObjectTables(0).ToList();
        }
        public bool IsAvailableForScheduling(string factTableName, int tenant)
        {
            DWObjectTable selectedDWObjectTable = dWObjectTables.Where(dWObjectTable => dWObjectTable.Code == factTableName).FirstOrDefault();
            if(selectedDWObjectTable != null)
            {
                return CheckBIFactFeatureToggle(selectedDWObjectTable.Code, tenant);
            }
            return false;
        }

        private bool CheckBIFactFeatureToggle(string factTableCode, int tenant)
        {
            switch (factTableCode)
            {
                case "Fact_ARInvoices": return HasFeatureToggle("BAR", tenant);
                case "Fact_Charges": return HasFeatureToggle("BCH", tenant);
                case "Fact_Containers": return HasFeatureToggle("BCO", tenant);
                case "Fact_InlandDomesticShipments": return HasFeatureToggle("BID", tenant);
                case "Fact_Invoices": return HasFeatureToggle("BIN", tenant);
                case "Fact_MasterCharges": return HasFeatureToggle("BMC", tenant);
                case "Fact_Masters": return HasFeatureToggle("BMA", tenant);
                case "Fact_Quotes": return HasFeatureToggle("BQU", tenant);
                case "Fact_Shipments": return HasFeatureToggle("BSH", tenant);
                default: return false;
            }
        }

        private bool HasFeatureToggle(string toggleCode, int tenant)
        {
            FeatureToggleRepository featureToggleRepository = new FeatureToggleRepository(tenant);
            return featureToggleRepository.HasFeatureToggle(toggleCode, tenant);
        }
    }
}
