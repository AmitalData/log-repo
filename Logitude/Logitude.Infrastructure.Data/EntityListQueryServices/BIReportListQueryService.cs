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
    public partial class BIReportListQueryService
    {
        private IQueryable<BIReportList> GetIqueryableList(IQueryable<BIReport> iQueryable)
        {
            IQueryable<BIReportList> query = (from a in iQueryable.Include("UpdatedByUser").Include("UpdatedByUser.Contact").Include("CreatedByUser").Include("CreatedByUser.Contact")
                                              select new BIReportList()
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
                                                  DWQueryId = a.DWQueryId,
                                                  Inactive = a.Inactive,
                                                  TypeCode = a.TypeCode,
                                                  AGGridOptionsXML = a.AGGridOptionsXML,
                                                  BIReportFolderId = a.BIReportFolderId,
                                                  UpdatedByUserName = a.UpdatedByUser == null ? null : (a.UpdatedByUser.Contact == null ? null : a.UpdatedByUser.Contact.EnglishName),
                                                  CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                                              });
            return query;
        }

        private IQueryable<BIReport> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<BIReport> iQueryable, int tenant)
        {
            return iQueryable;
        }

        private IQueryable<BIReport> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<BIReport> iQueryable, int tenant)
        {
            return iQueryable;
        }
    }
}
	