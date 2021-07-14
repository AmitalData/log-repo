using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{

    public partial class PoaStatusTypeLookUpListQueryService
    {
        private IQueryable<PoaStatusTypeLookUpList> GetIqueryableList(IQueryable<PoaStatusTypeLookUp> iQueryable)
        {
            IQueryable<PoaStatusTypeLookUpList> query = (from a in iQueryable
                                                         select new PoaStatusTypeLookUpList()
                                                         {
                                                             Code = a.Code,
                                                             LocalName = a.LocalName,
                                                             SearchFields = a.SearchFields,
                                                             Inactive = a.Inactive,
                                                             EnglishName = a.EnglishName,

                                                         });
            return query;
        }

        private IQueryable<PoaStatusTypeLookUp> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<PoaStatusTypeLookUp> iQueryable)
        {
            return iQueryable;
        }
    }
}
