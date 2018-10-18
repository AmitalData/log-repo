using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
   public partial  class RevenueExpenseTypeQueryService
    {

        public RevenueExpenseTypePM GetSinglePM(string code, int tenant)
        {
            RevenueExpenseTypePM entityPM = null;

            entityPM =
                (from a in repository.All()
                 where a.Code == code 
                 select new RevenueExpenseTypePM()
                 {
                     Code = a.Code,
                     LocalName = a.LocalName,
                     EnglishName = a.EnglishName,
                     Inactive = a.Inactive,
                    

                    
                 }).FirstOrDefault();

            return entityPM;
        }

        public RevenueExpenseTypePM GetSinglePM(string code  )
        {
            RevenueExpenseTypePM entityPM = null;

            entityPM =
                (from a in repository.All()
                 where a.Code == code
                 select new RevenueExpenseTypePM()
                 {
                     Code = a.Code,
                     LocalName = a.LocalName,
                     EnglishName = a.EnglishName,
                     Inactive = a.Inactive,



                 }).FirstOrDefault();

            return entityPM;
        }





    }
}
