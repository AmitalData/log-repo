using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class ExportStorageQueryService : EntityQueryService<ExportStorage, ExportStorageKeys, ExportStoragePM, object, ExportStorageKeys>
    {
        public ExportStoragePM GetByStorageNo(string storageNo, int tenant)
        {
            string id=this.repository.GetIDByStorageNo(storageNo,tenant);
            if (String.IsNullOrEmpty(id))
            {
                return null;
            }
            return this.GetSingle(id, true, false);
        }

        public ExportStoragePM GetByCargoKeys(string firstCargoID, string secondCargoID, string thirdCargoID, int cargoIdentifierType, int tenant)
        {
            var exportStorage = this.repository.GetIDByCargoKeys(firstCargoID, secondCargoID, thirdCargoID, cargoIdentifierType, tenant);
            ExportStoragePM exportStoragePM = null;
            if (exportStorage != null)
            {

                exportStoragePM = this.GetEntityPM(exportStorage, true, null);
                //exportStoragePM = new ExportStoragePM()
                //{
                //    Id = exportStorage.Id,
                //    DeclarationId = exportStorage.DeclarationId,
                //    Tenant = exportStorage.Tenant,
                //    CargoType = exportStorage.CargoType,
                //    CargoTypeCode = exportStorage.CargoTypeCode,
                //    ThirdCargoID = exportStorage.ThirdCargoID,
                //    SecondCargoID = exportStorage.SecondCargoID,
                //    FirstCargoID = exportStorage.FirstCargoID,
                //    CustomsStatus = exportStorage.CustomsStatus,
                //    ExporterID = exportStorage.ExporterID,
                //    ExportFileNo = exportStorage.ExportFileNo
                //};
            }
            return exportStoragePM;
        }


        public List<ExportStoragePM> GetDeclarationExportStoragesList(string declarationId,string  exportFile ,int tenant)
        {

            

            List<ExportStorage> DeclarationExportStorages = repository.GetExportStorageListByDeclarationIdAndExportFile(declarationId, exportFile,tenant);
            List<ExportStoragePM> DeclarationExportStorageList = new List<ExportStoragePM>();
            if (DeclarationExportStorages != null)
            {
                /*
                foreach (var DeclarationCargoSplitItem in DeclarationCargoSplits)
                {
                    DeclarationCargoSplitPM DeclarationCargoSplitPM = this.GetSingle(DeclarationCargoSplitItem.Id,true,false);
                    DeclarationCargoSplitList.Add(DeclarationCargoSplitPM);
                }
                */
                var pocos = DeclarationExportStorages.ToList();
                var pmList = pocos.Select(poco => this.GetEntityPM(poco, true, new ExportStorageKeys() { Id = poco.Id }))
                   .ToList();
                DeclarationExportStorageList = pmList;
            }


            return DeclarationExportStorageList;
        }
    }
}
