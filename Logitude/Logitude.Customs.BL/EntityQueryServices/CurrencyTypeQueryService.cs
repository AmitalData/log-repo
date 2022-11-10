
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CurrencyTypeQueryService : EntityQueryService<CurrencyType, CurrencyTypeKeys, CurrencyTypePM, object, CurrencyTypeKeys>
    {
        
        public IQueryable<CurrencyTypePM> GetCurrencyTypePMsByTenant(int tenant)
        {
            IQueryable<CurrencyTypePM> currencyTypes = from a in repository.customsDataContext.CurrencyTypes
                                         
                                         select new CurrencyTypePM()
                                         {
                                             Code = a.Code,
                                             EnglishName = a.EnglishName,
                                             LocalName = a.LocalName,
                                             Inactive = a.Inactive,
                                             SearchFields = a.SearchFields,
                                         };

            return currencyTypes;
        }
    }
}
