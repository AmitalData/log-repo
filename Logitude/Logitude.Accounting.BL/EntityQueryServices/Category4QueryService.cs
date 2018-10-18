using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
     public partial class Category4QueryService
    {

        public Category4PM GetSinglePM(string id, int tenant)
        {
            Category4PM entityPM = null;

            entityPM =
                (from a in repository.All()
                 where a.Id == id && a.Tenant== tenant
                 select new Category4PM()
                 {
                     Id = a.Id,
                     LocalName = a.LocalName,
                     EnglishName = a.EnglishName,
                     Inactive = a.Inactive,
                     Tenant = a.Tenant,

                     SearchFields = a.SearchFields,
                 }).FirstOrDefault();

            return entityPM;
        }

    }
}
