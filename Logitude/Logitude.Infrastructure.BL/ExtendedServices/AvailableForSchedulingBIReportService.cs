using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Infrastructure.BL.ExtendedServices
{
    public class AvailableForSchedulingBIReportService
    {
        private List<DWObjectTable> dWObjectTables;
        private int tenant;
        public AvailableForSchedulingBIReportService(int tenant)
        {
            DWObjectTableRepository dWObjectTableRepository = new DWObjectTableRepository(0);
            dWObjectTables = dWObjectTableRepository.GetDWObjectTables(0).ToList();
            this.tenant = tenant;
        }
        public bool IsAvailableForScheduling(string factTableName)
        {
            DWObjectTable selectedDWObjectTable = dWObjectTables.Where(dWObjectTable => dWObjectTable.Code == factTableName).FirstOrDefault();
            if(selectedDWObjectTable != null)
            {
                return CheckBIFactFeatureToggle(selectedDWObjectTable.Code);
            }
            return false;
        }

        private bool CheckBIFactFeatureToggle(string factTableCode)
        {
            switch (factTableCode)
            {
                case "Fact_ARInvoices": return FeatureToggleHelper.HasFeatureToggle("BAR", tenant);
                case "Fact_Charges": return FeatureToggleHelper.HasFeatureToggle("BCH", tenant);
                case "Fact_Containers": return FeatureToggleHelper.HasFeatureToggle("BCO", tenant);
                case "Fact_InlandDomesticShipments": return FeatureToggleHelper.HasFeatureToggle("BID", tenant);
                case "Fact_Invoices": return FeatureToggleHelper.HasFeatureToggle("BIN", tenant);
                case "Fact_MasterCharges": return FeatureToggleHelper.HasFeatureToggle("BMC", tenant);
                case "Fact_Masters": return FeatureToggleHelper.HasFeatureToggle("BMA", tenant);
                case "Fact_Quotes": return FeatureToggleHelper.HasFeatureToggle("BQU", tenant);
                case "Fact_Shipments": return FeatureToggleHelper.HasFeatureToggle("BSH", tenant);
                default: return false;
            }
        }
    }
}
