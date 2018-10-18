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

using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityLists;

namespace Logitude.CRM.Data.EntityListQueryServices
{

    public partial class CorrespondencesAttachmentListQueryService
    {
        private IQueryable<CorrespondencesAttachmentList> GetIqueryableList(IQueryable<CorrespondencesAttachment> iQueryable)
        {
            IQueryable<CorrespondencesAttachmentList> query = (from a in iQueryable
                                                               select new CorrespondencesAttachmentList()
                                                               {

                                                                   Id = a.Id,

                                                                   Tenant = a.Tenant,

                                                                   DocumentFilingId = a.DocumentFilingId,

                                                                   CorrespondenceId = a.CorrespondenceId,

                                                               });
            return query;
        }

        private IQueryable<CorrespondencesAttachment> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CorrespondencesAttachment> iQueryable, int tenant)
        {
            throw new NotImplementedException();
        }
        private IQueryable<CorrespondencesAttachment> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<CorrespondencesAttachment> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }
}
