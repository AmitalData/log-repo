using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
   public partial  class ReconcileMethodQueryService
    {

        public ReconcileMethodPM GetSinglePM(string code, int tenant)
        {
            ReconcileMethodPM entityPM = null;

            entityPM =
                (from a in repository.All()
                 where a.Code == code 
                 select new ReconcileMethodPM()
                 {
                     Code = a.Code,
                     LocalName = a.LocalName,
                     EnglishName = a.EnglishName,
                     Inactive = a.Inactive,
                     

                     SearchFields = a.SearchFields,
                 }).FirstOrDefault();

            return entityPM;
        }

        public ReconcileMethodPM GetSinglePM(string code )
        {
            ReconcileMethodPM entityPM = null;

            entityPM =
                (from a in repository.All()
                 where a.Code == code
                 select new ReconcileMethodPM()
                 {
                     Code = a.Code,
                     LocalName = a.LocalName,
                     EnglishName = a.EnglishName,
                     Inactive = a.Inactive,


                     SearchFields = a.SearchFields,
                 }).FirstOrDefault();

            return entityPM;
        }
    }
}
