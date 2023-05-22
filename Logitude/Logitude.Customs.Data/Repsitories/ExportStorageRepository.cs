 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using System.Data.Entity;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class ExportStorageRepository:IRepository<ExportStorage>
   {
        
		public List<ExportStorage> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public string GetIDByStorageNo(string storageNo, int tenant)
        {
            var q= from a in context.ExportStorages
                   where 
                   a.Tenant == tenant &&
                   a.StorageNo == storageNo
                   select a.Id;
            return q.FirstOrDefault();

        }

        public ExportStorage GetIDByCargoKeys(string firstCargoID, string secondCargoID, string thirdCargoID, int cargoIdentifierType, int tenant)
        {
            var q = from a in context.ExportStorages
                    where
                    a.Tenant == tenant &&
                    a.FirstCargoID == firstCargoID &&
                    (a.SecondCargoID == secondCargoID || secondCargoID == null) &&
                    (a.ThirdCargoID == thirdCargoID || thirdCargoID == null) &&
                    a.CargoTypeCode == cargoIdentifierType.ToString()
                    select a;
            return q.FirstOrDefault();

        }

        public List<ExportStorage> GetExportStorageListByDeclarationIdAndExportFile(string declarationId, string exportFileNo, int tenant)
        {
            var q = from a in context.ExportStorages.Include("ExportLogisticPermitAction")
                    where
                    a.Tenant == tenant && (
                    a.ExportFileNo == exportFileNo
                    || a.DeclarationId== declarationId )
                    select a;
           
            return q.Distinct().ToList();

        }

        public List<ExportStorage> GetExportStorageListByDeclarationId(string declarationId, int tenant)
        {
            var q = from a in context.ExportStorages.Include("ExportLogisticPermitAction")
                    where
                    a.Tenant == tenant && 
                    a.DeclarationId == declarationId
                    select a;

            return q.Distinct().ToList();

        }

        public List<ExportStorage> GetConnectToFileNoNotToDeclaration(string exportFileNo, int tenant)
        {
            var q = from a in context.ExportStorages
                    where
                    a.Tenant == tenant && 
                    a.ExportFileNo == exportFileNo
                    && a.DeclarationId == null &&
                    a.StorageStatus != "Cancel"
                    select a;

            return q.ToList();

        }
    }

}
   