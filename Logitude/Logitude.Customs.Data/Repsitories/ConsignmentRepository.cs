
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
using System.Diagnostics;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class ConsignmentRepository:IRepository<Consignment>
   {
        
		public List<Consignment> GetMulti(EntityKeyFields entityKeys)
        {
   
            DeclarationKeys declarationKeys = entityKeys as DeclarationKeys;

            return (from a in context.Consignments
                    where a.DeclarationId == declarationKeys.Id
                    select a).ToList();
       
        }

        public Consignment GetConsignmentByIdentifiers(string cargoTypeCode, string manifestNumber, string secondCargoID, int tenant)
        {
            return (from a in context.Consignments
                    where a.CargoTypeCode == cargoTypeCode & a.ManifestNumber == manifestNumber & a.SecondCargoID == secondCargoID
                    select a).ToList().FirstOrDefault();
        }
        public int? GetMaxCounterKey(string declarationId, int tenant)
        {
            return (from a in context.Consignments
                    where a.DeclarationId == declarationId && a.Tenant == tenant
                    select a).Max(d => (int?)d.ConsignmentNumber) ?? 0;
        }

        public string GetDeclarationIdByConsignmentCargoId(string manifestNumber, string secondCargoID, string thirdCargoID, int tenant)
        {
            var q = GetAll(tenant);
            q = q.Where(a => a.ManifestNumber == manifestNumber );
            q = q.Where(a => ((a.SecondCargoID ?? "_IsNull") == (secondCargoID ?? "_IsNull")) && ((a.SecondCargoID ?? "_IsNull") == (secondCargoID ?? "_IsNull")));
            q = q.Where(a => ((a.ThirdCargoID ?? "_IsNull") == (thirdCargoID ?? "_IsNull")) && ((a.ThirdCargoID ?? "_IsNull") == (thirdCargoID ?? "_IsNull")));
            q = q.Distinct();

            if (q.FirstOrDefault() != null)
            {
                return q.FirstOrDefault().DeclarationId;
            }

            return null;
        }


        public string GetDeclarationIdBythirdCargoID(string thirdCargoID, int tenant, List<string> idList = null)
        {
            var q = GetAll(tenant)
                .Where(r => r.ThirdCargoID == thirdCargoID);

            if (idList != null && idList.Count > 0)
            {
                q = q.Where(r => idList.Contains(r.DeclarationId));
            }

            return q.Select(r => r.DeclarationId).FirstOrDefault();
        }


        public void FastDeleteMulti(DeclarationKeys entityKeyFields)
        {
            //throw new Exception("preventing Clear Consignments - Validation (CALL#291407)");
            LogitudeSettings.HandleLogMe("DeclarationId:" + entityKeyFields.GetFullKey() + Environment.NewLine + Environment.StackTrace.ToString(), false, "ConsignmentRepositoryFastDeleteMulti", new DateTime(2017, 11, 1));

            (context as DbContextBase)
                .DeleteWhere<Consignment>(rec => rec.DeclarationId == entityKeyFields.Id);
        }

        //partial void onRemove(Consignment entity)
        //{
        //    //entity.DeclarationId

        //    LogitudeSettings.HandleLogMe("DeclarationId:" + entity.DeclarationId + Environment.NewLine + Environment.StackTrace.ToString(), false, "ConsignmentRepositoryonRemove", new DateTime(2017, 11, 1));
        //    return;


        //    this.SubmitChanges();
        //    var q = (from a in context.Consignments
        //             where
        //             a.DeclarationId == entity.DeclarationId &&
        //             a.ConsignmentNumber != entity.ConsignmentNumber

        //             select a);
        //    if (q.Any())
        //    {
        //        return;
        //    }
        //    throw new Exception("preventing Clear Consignments - Validation (CALL#291407)");
        //}
   }

}
   