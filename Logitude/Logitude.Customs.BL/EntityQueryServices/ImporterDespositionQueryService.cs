using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class ImporterDespositionQueryService
    {
        public string GetImporterDespositionByDepositionNumber(string depositionNumber, int tenant)
        {
            if (string.IsNullOrWhiteSpace(depositionNumber)) return null;
            return repository.GetImporterDespositionByDepositionNumber(depositionNumber, tenant);
        }

        public ImporterDespositionPM GetImporterDespositionByDepositionNumberImporterVendor(string depositionNumber, string importerId, string vendorID, int tenant)
        {
            if (string.IsNullOrWhiteSpace(depositionNumber) || string.IsNullOrWhiteSpace(depositionNumber) || string.IsNullOrWhiteSpace(depositionNumber)) return null;

            List<ImporterDesposition> importerDespositionList = repository.GetImporterDespositionByDepositionNumberImporterVendor(depositionNumber, importerId, vendorID, tenant);
            if (importerDespositionList == null) return null;

            var ImporterDespositionPM = importerDespositionList.Select(poco => this.GetEntityPM(poco)).FirstOrDefault();
            return ImporterDespositionPM;
        }
    }
}
