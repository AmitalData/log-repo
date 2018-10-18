using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
   public partial class AutomaticReconcileQueryService
    {
        public AutomaticReconcilePM GetSinglePM(string code, int tenant)
        {
            AutomaticReconcilePM entityPM = null;

            entityPM =
                (from a in repository.GetAll()
                 where a.Code == code 
                 select new AutomaticReconcilePM()
                 {
                     Code = a.Code,
                     LocalName = a.LocalName,
                     EnglishName = a.EnglishName,
                     Inactive = a.Inactive,
                  
                     SearchFields = a.SearchFields,
                 }).FirstOrDefault();

            return entityPM;
        }

        public AutomaticReconcilePM GetSinglePM(string code)
        {
            AutomaticReconcilePM entityPM = null;

            entityPM =
                (from a in repository.GetAll()
                 where a.Code == code
                 select new AutomaticReconcilePM()
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
