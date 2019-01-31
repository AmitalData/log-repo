using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
   public partial class AccountingEntityQueryService
    {

        public AccountingEntityPM GetSinglePM(string code, int tenant)
        {
            AccountingEntityPM entityPM = null;

            entityPM =
                (from a in repository.All()
                 where a.Code == code 
                 select new AccountingEntityPM()
                 {
                     Code = a.Code,
                     LocalName = a.LocalName,
                     EnglishName = a.EnglishName,
                    
                 }).FirstOrDefault();

            return entityPM;
        }

        public AccountingEntityPM GetSinglePM(string code)
        {
            AccountingEntityPM entityPM = null;

            entityPM =
                (from a in repository.All()
                 where a.Code == code
                 select new AccountingEntityPM()
                 {
                     Code = a.Code,
                     LocalName = a.LocalName,
                     EnglishName = a.EnglishName,

                 }).FirstOrDefault();

            return entityPM;
        }

    }
}
