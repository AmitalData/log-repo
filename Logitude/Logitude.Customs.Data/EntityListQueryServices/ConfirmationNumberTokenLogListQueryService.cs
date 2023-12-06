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
    public partial class ConfirmationNumberTokenLogListQueryService
    {
        private IQueryable<ConfirmationNumberTokenLogList> GetIqueryableList(IQueryable<ConfirmationNumberTokenLog> iQueryable)
        {
            IQueryable<ConfirmationNumberTokenLogList> query = (from a in iQueryable
                                                                select new ConfirmationNumberTokenLogList()
                                                                {

                                                                    Id = a.Id,

                                                                });
            return query;
        }

        private IQueryable<ConfirmationNumberTokenLog> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ConfirmationNumberTokenLog> iQueryable, int tenant)
        {
            return iQueryable;
        }
    }


}
