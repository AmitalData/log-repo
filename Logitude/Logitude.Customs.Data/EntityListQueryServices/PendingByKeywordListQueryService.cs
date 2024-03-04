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
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.Customs.Data.EntityListQueryServices
{

    public partial class PendingByKeywordListQueryService
    {
        private IQueryable<PendingByKeywordList> GetIqueryableList(IQueryable<PendingByKeyword> iQueryable)
        {
            IQueryable<PendingByKeywordList> query = (from a in iQueryable
                                                      join cpr in context.CourierPendingReasons
                                                      on new { Code = a.CourierPendingReasonCode, Tenant = a.Tenant } equals new { Code = cpr.Code, Tenant = cpr.Tenant } into joined
                                                      from cpr in joined.DefaultIfEmpty()
                                                      select new PendingByKeywordList()
                                                      {

                                                          Id = a.Id,

                                                          Tenant = a.Tenant,
                                                          CourierPendingReasonCode = cpr != null ? cpr.Code  : null,
                                                          CourierPendingReasonName = cpr != null ? cpr.LocalName : null,
                                                          KeywordsList = a.KeywordsList, 
                                                           SearchByFieldCode = a.SearchByFieldCode,
                                                            SearchByFieldName = a.SearchByFieldCode!=null ? (a.SearchByFieldCode=="1" ?
                                                            "תאור טובין"//"���� �����" 
                                                            :
                                                            "שם יבואן"///"�� �����"
                                                            ) :null,
                                                            SearchType=a.SearchType,
                                                          SearchTypesName = a.SearchType != null ? (a.SearchType == "1" ?
                                                            "מילה" 
                                                            :
                                                             "חלק ממילה"
                                                            ) : null,
                                                          Remove = "",
                                                          ExceptKeywords=a.ExceptKeywords,

                                                      });
            return query;
        }

        private IQueryable<PendingByKeyword> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<PendingByKeyword> iQueryable, int tenant)
        {
            return iQueryable;
        }
    }


}
	