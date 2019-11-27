using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data.Repsitories;
using System.Linq;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.DataContracts;
using Logitude.CRM.Data;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data.BusinessUnitFilters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools;
using Logitude.CRM.BL.EntityDws;

namespace Logitude.CRM.BL.EntityQueryServices
{
    public partial class OccasionQueryService
    {
        public override void GetComposition(EntityKeyFields entityKeys, OccasionPM entityPM)
        {
            ICRMContext context = MainContext as ICRMContext;
            OccasionKeys occasionKeys = entityKeys as OccasionKeys;
            OccasionInviteeQueryService queryService = new OccasionInviteeQueryService(context);
            entityPM.OccasionInvitees = queryService.GetMulti(occasionKeys, true);
        }

        public List<OccasionPM> GetContactOccasions(int tenant, ICRMContext context, string contactId)
        {
            List<string> occasionIds = (from item in context.OccasionInvitees
                                        where item.ContactId == contactId
                                        select item.OccasionId).ToList();

            IQueryable<OccasionPM> iQueryable = from occasion in context.Occasions
                                              where occasion.Tenant == tenant
                                              && occasionIds.Contains(occasion.Id)
                                              select new OccasionPM {
                                                  Id = occasion.Id,
                                                  Name = occasion.Name,
                                                  TypeName = occasion.OccasionType.Name,
                                                  IndustryName = occasion.Industry.Name,
                                                  OccasionStatusName = occasion.OccasionStatus.Name,
                                                  StartDateTime = occasion.StartDateTime,
                                                  EndDateTime = occasion.EndDateTime,
                                                  Location = occasion.Location
                                              };
            return iQueryable.ToList();
        }
    }
}
