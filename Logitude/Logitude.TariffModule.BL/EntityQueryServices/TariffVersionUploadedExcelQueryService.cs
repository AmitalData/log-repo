using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.EntityQueryServices
{
    public partial class TariffVersionUploadedExcelQueryService
    {
        public List<TariffVersionUploadedExcelPM> GetUploadedExcelByTariffAndVersion(string tariffId, int version, int tenant)
        {
            List<TariffVersionUploadedExcel> myList = (from a in context.TariffVersionUploadedExcels
                                                 where a.TariffId == tariffId && a.Tenant == tenant && a.Version == version
                                                 select a).ToList();

            List<TariffVersionUploadedExcelPM> PMs = new List<TariffVersionUploadedExcelPM>();
            foreach (TariffVersionUploadedExcel entityPOCO in myList)
            {
                TariffVersionUploadedExcelPM entityPM = new TariffVersionUploadedExcelPM();
                mapping.CustomPOCOToPM(entityPM, entityPOCO);
                mapping.POCOToPM(entityPM, entityPOCO);
                PMs.Add(entityPM);
            }

            return PMs;
        }
    }
}
