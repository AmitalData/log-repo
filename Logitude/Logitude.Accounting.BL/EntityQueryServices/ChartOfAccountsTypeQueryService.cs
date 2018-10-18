using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class ChartOfAccountsTypeQueryService : EntityQueryService<ChartOfAccountsType, ChartOfAccountsTypeKeys, ChartOfAccountsTypePM, object, ChartOfAccountsTypeKeys>
    {

        public ChartOfAccountsTypePM GetSinglePM(string code, int tenant)
        {
            ChartOfAccountsTypePM entityPM = null;

            entityPM =
                (from a in repository.All()
                 where a.Code == code 
                 select new ChartOfAccountsTypePM()
                 {
                     Code = a.Code,
                     LocalName = a.LocalName,
                     EnglishName = a.EnglishName,
                     Inactive = a.Inactive,
                 

                     SearchFields = a.SearchFields,
                 }).FirstOrDefault();

            return entityPM;
        }
        public ChartOfAccountsTypePM GetSinglePM(string code)
        {
            ChartOfAccountsTypePM entityPM = null;

            entityPM =
                (from a in repository.All()
                 where a.Code == code
                 select new ChartOfAccountsTypePM()
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
