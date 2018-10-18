using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data.EntityLists;
using Logitude.CRM.Data.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityQueryServices
{
    public partial class SLAHeaderQueryService
    {
        public override void GetComposition(EntityKeyFields entityKeys, SLAHeaderPM entityPM)
        {
            ICRMContext context = MainContext as ICRMContext;
            SLAHeaderKeys SLAHeaderKeys = entityKeys as SLAHeaderKeys;

            SLALineQueryService service = new SLALineQueryService(context);
            entityPM.SLALines = service.GetMulti(SLAHeaderKeys, true);

            SLAEscalationQueryService escalationsService = new SLAEscalationQueryService(context);
            entityPM.SLAEscalations = escalationsService.GetMulti(SLAHeaderKeys, true);
        }
        public SLAHeaderPM GetSinglePMByTenant(int tenant)
        {
            ICRMContext context = MainContext as ICRMContext;
            SLAHeader entity = repository.GetSingleByTenant(tenant);

            if (entity != null)
            {
                EntityPM = new SLAHeaderPM();
                mapping.CustomPOCOToPM(EntityPM, entity);
                mapping.POCOToPM(EntityPM, entity);
                this.GetComposition(new SLAHeaderKeys() { Id = EntityPM.Id }, EntityPM);
            }

            return EntityPM;
        }
        public List<SLAHeaderPM> GetActiveSLAbyTenant(int tenant)
        {
            List<SLAHeaderPM> query = (from a in context.SLAHeaders
                                       where a.Tenant == tenant 
                                           select new SLAHeaderPM()
                                           {
                                               Id = a.Id,
                                               Tenant = a.Tenant,
                                               CreateDate = a.CreateDate,
                                               CreatedByUserId = a.CreatedByUserId,
                                               UpdateDate = a.UpdateDate,
                                               UpdatedByUserId = a.UpdatedByUserId,
                                               Name = a.Name,
                                               Description = a.Description,
                                               Inactive = a.Inactive,
                                               UpdatedByUserName = a.UpdatedByUser != null ? a.UpdatedByUser.Contact.EnglishName : null,
                                               CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                           }).ToList();

            //foreach (SLAHeaderPM item in query)
            //{
            //    item.SLALines = (from a in context.SLALines
            //                               where a.SLAHeaderId == item.Id && a.Tenant == item.Tenant
            //                               select new SLALinePM()
            //                               {
            //                                   Id = a.Id,
            //                                   Tenant = a.Tenant,
            //                                   SLAHeaderId = a.SLAHeaderId,
            //                                   SeverityId = a.SeverityId,
            //                                   BusinessHoursId = a.BusinessHoursId,
            //                                   FirstResponseTime = a.FirstResponseTime,
            //                                   FirstResponseTimeUnit = a.FirstResponseTimeUnit,
            //                                   FirstResponseTimeInMinute = a.FirstResponseTimeInMinute,
            //                                   ResolveWithinTime = a.ResolveWithinTime,
            //                                   ResolveWithinTimeUnit = a.ResolveWithinTimeUnit,
            //                                   ResolveWithinTimeInMinute = a.ResolveWithinTimeInMinute,
            //                                   ResolveWithinEscalate = a.ResolveWithinEscalate,
            //                                   FirstResponseEscalate = a.FirstResponseEscalate,
            //                                   SeverityName = a.TicketSeverity == null ? "" : a.TicketSeverity.Name,
            //                               }).ToList();

            //    item.SLAEscalations =  (from a in context.SLAEscalations
            //                             where a.SLAHeaderId == item.Id && a.Tenant == item.Tenant
            //                             select new SLAEscalationPM()
            //                             {
            //                                 Id = a.Id,
            //                                 Tenant = a.Tenant,
            //                                 SLAHeaderId = a.SLAHeaderId,
            //                                 LineNumber = a.LineNumber,
            //                                 EscalationFor = a.EscalationFor,
            //                                 EscalationActionTimeIndicator = a.EscalationActionTimeIndicator,
            //                                 EscalationTime = a.EscalationTime,
            //                                 EscalationTimeUnit = a.EscalationTimeUnit,
            //                                 EscalaitonTimeInMinutes = a.EscalaitonTimeInMinutes,
            //                                 TimeIndicator = a.EscalationActionTime == null ? "" : a.EscalationActionTime.Name,
            //                                 TimeUnitName = a.TimeUnit == null ? "" : a.TimeUnit.Name,
            //                             }).ToList();
            //}

            return query;
        }
    }
}
