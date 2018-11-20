 
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

namespace Logitude.Customs.Data.Repsitories
{
   public partial class DeclarationCargoSplitRepository:IRepository<DeclarationCargoSplit>
   {
        
		public List<DeclarationCargoSplit> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public List<DeclarationCargoSplit> GetDeclarationCargoSplitsList(string declarationId, int tenant)
        {
            return (from a in context.DeclarationCargoSplits
                    where a.DeclarationId == declarationId && a.Tenant == tenant
                    select a).ToList();
        }

        public string GetIdByDeclarationCargoSplitRequestNumber(string declarationCargoSplitRequestNumber, int tenant)
        {
            if (String.IsNullOrWhiteSpace(declarationCargoSplitRequestNumber)) return "";
            return
                  (
                  from rec in context.DeclarationCargoSplits
                  where rec.RequestNumber == declarationCargoSplitRequestNumber && rec.Tenant == tenant
                  select rec.Id
                  )
                  .FirstOrDefault();
        }

        public string GetIdByCargoIdentifiers(string cargoIdentifierKey1, string cargoIdentifierKey2, string cargoIdentifierKey3, int cargoIdentifierType, int tenant)
        {
            if (String.IsNullOrWhiteSpace(cargoIdentifierKey1) || String.IsNullOrWhiteSpace(cargoIdentifierKey2) || String.IsNullOrWhiteSpace(cargoIdentifierKey3)) return "";
            string cargoTypeCode = null;
            if (cargoIdentifierType > 0) cargoTypeCode = cargoIdentifierType.ToString();
            return
                  (
                  from rec in context.DeclarationCargoSplits
                  where rec.ManifestNumber == cargoIdentifierKey1 && rec.SecondCargoID == cargoIdentifierKey2 && rec.ThirdCargoID == cargoIdentifierKey3 && rec.CargoTypeCode == cargoTypeCode && rec.Tenant == tenant
                  select rec.Id
                  )
                  .FirstOrDefault();
        }
    }

}
   