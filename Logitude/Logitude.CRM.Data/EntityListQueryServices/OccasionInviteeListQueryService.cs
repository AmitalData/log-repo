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

    public partial class OccasionInviteeListQueryService
    {
        private IQueryable<OccasionInviteeList> GetIqueryableList(IQueryable<OccasionInvitee> iQueryable)
        {
            IQueryable<OccasionInviteeList> query = (from a in iQueryable
                                                     select new OccasionInviteeList()
                                                     {

                                                         Id = a.Id,

                                                         Tenant = a.Tenant,

                                                         AddedDate = a.AddedDate,

                                                         AddedByUserId = a.AddedByUserId,

                                                         UpdateDate = a.UpdateDate,

                                                         UpdatedByUserId = a.UpdatedByUserId,

                                                         SearchFields = a.SearchFields,

                                                         Notes = a.Notes,

                                                         OccasionId = a.OccasionId,
                                                         ContactId = a.ContactId,
                                                         ContactName = a.Contact != null ? a.Contact.EnglishName : "",
                                                         CustomerName = a.Contact != null ? a.Contact.CompanyName : "",
                                                         OccasionName = a.Occasion != null ? a.Occasion.Name : "",
                                                         ContactEmail = a.Contact != null ? a.Contact.Email : "",
                                                         ContactMobile = a.Contact != null ? a.Contact.Mobile : "",
                                                         ContactPhone = a.Contact != null ? a.Contact.BusinessPhone : "",
                                                         ContactPosition = a.Contact != null ? a.Contact.Position : "",
                                                         ContactTel  = a.Contact != null ? a.Contact.BusinessPhone : "",
                                                     });
            return query;
        }

        private IQueryable<OccasionInvitee> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<OccasionInvitee> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<OccasionInvitee> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<OccasionInvitee> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }
}
	