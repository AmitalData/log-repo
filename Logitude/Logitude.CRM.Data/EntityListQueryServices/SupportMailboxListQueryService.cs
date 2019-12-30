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

    public partial class SupportMailboxListQueryService
    {
        private IQueryable<SupportMailboxList> GetIqueryableList(IQueryable<SupportMailbox> iQueryable)
        {
            IQueryable<SupportMailboxList> query = (from a in iQueryable
                                                    select new SupportMailboxList()
                                                    {

                                                        Id = a.Id,

                                                        Tenant = a.Tenant,

                                                        CreateDate = a.CreateDate,

                                                        CreatedByUserId = a.CreatedByUserId,

                                                        UpdateDate = a.UpdateDate,

                                                        UpdatedByUserId = a.UpdatedByUserId,

                                                        Mailbox = a.Mailbox,

                                                        Inactive = a.Inactive,

                                                        IsDefault = a.IsDefault,

                                                    });
            return query;
        }

        private IQueryable<SupportMailbox> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<SupportMailbox> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<SupportMailbox> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<SupportMailbox> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }


}
	