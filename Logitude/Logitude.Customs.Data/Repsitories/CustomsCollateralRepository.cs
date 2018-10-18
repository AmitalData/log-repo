
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
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CustomsCollateralRepository:IRepository<CustomsCollateral>
   {
        
		public List<CustomsCollateral> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public string GetIdByCollateralRequestNumber(string collateralRequestNumber, int tenant)
        {
            if (String.IsNullOrWhiteSpace(collateralRequestNumber)) return "";
            return
                  (
                  from rec in context.CustomsCollaterals
                  where rec.CollateralRequestNumber == collateralRequestNumber && rec.Tenant == tenant
                  select rec.Id
                  )
                  .FirstOrDefault();
        }

        public List<CustomsCollateral> GetDeclarationCollateralsList(string declarationId, int tenant)
        {
            return (from a in context.CustomsCollaterals
                    where a.DeclarationId == declarationId && a.Tenant == tenant
                    select a).ToList();
        }

        public List<CustomsCollateral> GetCollateralsListByDeclarationConstraint(string customsEntityTypeCode, string entityIdKey1, string entityIdKey2, int tenant)
        {
            return (from a in context.CustomsCollaterals
                    where a.CustomsEntityTypeCode == customsEntityTypeCode && a.Tenant == tenant
                    && a.EntityIdKey1 == entityIdKey1 && a.EntityIdKey2 == entityIdKey2
                    select a).OrderByDescending(d => d.CollateralRequestNumber).ToList();
        }
    }

}
   