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
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityLists;

namespace Logitude.Infrastructure.Data.EntityListQueryServices
{
    public partial class BIReportFolderListQueryService
    {
        private IQueryable<BIReportFolderList> GetIqueryableList(IQueryable<BIReportFolder> iQueryable)
        {
            IQueryable<BIReportFolderList> query = (from a in iQueryable
                                                    select new BIReportFolderList()
                                                    {
                                                        Id = a.Id,
                                                        Tenant = a.Tenant,
                                                        CreateDate = a.CreateDate,
                                                        CreatedByUserId = a.CreatedByUserId,
                                                        UpdateDate = a.UpdateDate,
                                                        UpdatedByUserId = a.UpdatedByUserId,
                                                        SearchFields = a.SearchFields,
                                                        Name = a.Name,
                                                        Description = a.Description,
                                                        Index = a.Index,
                                                    });
            return query;
        }

        private IQueryable<BIReportFolder> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<BIReportFolder> iQueryable, int tenant)
        {
            return iQueryable;
        }

        private IQueryable<BIReportFolder> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<BIReportFolder> iQueryable, int tenant)
        {
            return iQueryable;
        }
    }
}
	