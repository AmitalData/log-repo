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

    public partial class ExceptionReasonListQueryService
    {
        private IQueryable<ExceptionReasonList> GetIqueryableList(IQueryable<ExceptionReason> iQueryable)
        {
            IQueryable<ExceptionReasonList> query = (from a in iQueryable
                                                     select new ExceptionReasonList()
                                                     {

                                                         Code = a.Code,

                                                         EnglishName = a.EnglishName,

                                                         LocalName = a.LocalName,

                                                         IsActive = a.IsActive,

                                                     });
            return query;
        }

        private IQueryable<ExceptionReason> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ExceptionReason> iQueryable, int tenant)
        {
            return iQueryable;
        }
    }


}
