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

    public partial class ExportLogisticPermitActionListQueryService
    {
        private IQueryable<ExportLogisticPermitActionList> GetIqueryableList(IQueryable<ExportLogisticPermitAction> iQueryable)
        {
            IQueryable<ExportLogisticPermitActionList> query = (from a in iQueryable
                                                                select new ExportLogisticPermitActionList()
                                                                {
                                                                    Code = a.Code,
                                                                    EnglishName = a.EnglishName,
                                                                    LocalName = a.LocalName,
                                                                    SearchFields = a.SearchFields,
                                                                    Inactive = a.Inactive,
                                                                });
            return query;
        }

        private IQueryable<ExportLogisticPermitAction> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ExportLogisticPermitAction> iQueryable)
        {
            return iQueryable; //throw new NotImplementedException();
        }
    }
}
