using Logitude.BL.QuoteModel.EntityPMs;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    class QuoteFollowUpUpdateService
    {
        private QuotePM entityPM;
        private int tenant;
        public QuoteFollowUpUpdateService(QuotePM entityPM, int tenant)
        {
            this.entityPM = entityPM;
            this.tenant = tenant;
        }
        public void RefreshFollowUps()
        {
            IWebFreightContext freightContext = WebFreightContext.GetContext(tenant);
            FollowUpRepository followUpsRepository = new FollowUpRepository(freightContext);
            List<FollowUp> allFollowupLists = null;
            List<FollowUp> followupLists = null;

            if (entityPM.IsRefreshQuoteFollowUps)
            {
                allFollowupLists = followUpsRepository.GetFollowUpsByQuoteId(entityPM.Id, entityPM.Tenant);
                followupLists = allFollowupLists.Where(d => !string.IsNullOrEmpty(d.DateFieldName)).ToList();

                UpdateQuoteFollowUps(followUpsRepository, followupLists);

                if (entityPM.IsRefreshQuoteFollowUps && allFollowupLists.Count > 0) RefreshQuoteFollowUps(allFollowupLists);

            }
        }

        private void UpdateQuoteFollowUps(FollowUpRepository followUpsRepository, List<FollowUp> followupLists)
        {
            if (followupLists.Count > 0)
            {
                bool isAnyOneChange = false;
                isAnyOneChange = RefreshFollowUpsDateFieldList(followUpsRepository, followupLists, isAnyOneChange);
                if (isAnyOneChange) followUpsRepository.SubmitChanges();

            }
        }

        private bool RefreshFollowUpsDateFieldList(FollowUpRepository followUpsRepository, List<FollowUp> followupLists, bool isAnyOneChange)
        {
            foreach (FollowUp follow in followupLists)
            {
                bool isChange = false;

                UpdateFollowUpsDateField(followUpsRepository, ref isAnyOneChange, follow, ref isChange);
            }

            return isAnyOneChange;
        }

        private void UpdateFollowUpsDateField(FollowUpRepository followUpsRepository, ref bool isAnyOneChange, FollowUp follow, ref bool isChange)
        {
            if (!string.IsNullOrEmpty(follow.DateFieldName))
            {
                PropertyInfo propInfo = entityPM.GetType().GetProperty(follow.DateFieldName);
                if (propInfo != null)
                {
                    object fieldValue = propInfo.GetValue(entityPM);

                    UpdateDateField(ref isAnyOneChange, follow, ref isChange, fieldValue);

                    if (isChange) followUpsRepository.Update(follow);
                }
            }
        }

        private void UpdateDateField(ref bool isAnyOneChange, FollowUp follow, ref bool isChange, object fieldValue)
        {
            if (fieldValue != null)
            {
                DateTime? fieldValuedate = (DateTime?)fieldValue;

                fieldValuedate = UpdateDateFieldDays(follow, fieldValuedate);

                UpdateQuoteFollowUpsDate(ref isAnyOneChange, follow, ref isChange, fieldValuedate);

            }
        }

        private void UpdateQuoteFollowUpsDate(ref bool isAnyOneChange, FollowUp follow, ref bool isChange, DateTime? fieldValuedate)
        {
            if (follow.Date != fieldValuedate)
            {
                follow.Date = fieldValuedate;

                if (!entityPM.IsRefreshQuoteFollowUps)
                {
                    QuoteFollowUpPM followUpPM = entityPM.FollowUps.Where(d => d.Id == follow.Id).FirstOrDefault();
                    if (followUpPM != null) followUpPM.Date = follow.Date;
                }

                isChange = true;
                isAnyOneChange = true;
            }
        }

        private static DateTime? UpdateDateFieldDays(FollowUp follow, DateTime? fieldValuedate)
        {
            if (follow.DateEscalationActionTimeIndicatorCode != "IM" && follow.DateEscalationTime != 0)
            {
                int dateEscalationTime = follow.DateEscalationActionTimeIndicatorCode == "AF" ? follow.DateEscalationTime : follow.DateEscalationTime * -1;
                fieldValuedate = fieldValuedate.Value.AddDays(dateEscalationTime);

            }

            return fieldValuedate;
        }

        private void RefreshQuoteFollowUps(List<FollowUp> followupList = null)
        {
            entityPM.IsRefreshQuoteFollowUps = false;
            entityPM.IsRefreshFollowUp = true;
            IWebFreightContext myFreightContext = WebFreightContext.GetContext(tenant);
            FollowUpRepository followUpsRepository = new FollowUpRepository(myFreightContext);

            ClearFollowUps();
            followupList = RefreshQuoteFollowUpPMList(followupList, followUpsRepository);
            RefreshQuoteFollowUpPM(followupList);
        }

        private void RefreshQuoteFollowUpPM(List<FollowUp> followupList)
        {
            foreach (FollowUp follow in followupList)
            {
                QuoteFollowUpPM followUpPM = new QuoteFollowUpPM()
                {
                    Tenant = follow.Tenant,
                    Date = follow.Date,
                    Done = follow.Done,
                    DoneDateTime = follow.DoneDateTime,
                    DoneNote = follow.DoneNote,
                    ExternalDocumentId = follow.DocumentsFilingId,
                    Id = follow.Id,
                    InternalDocumentId = follow.InternalDocumentId,
                    IsNew = follow.IsNew,
                    JobId = follow.JobId,
                    LegType = follow.LegType,
                    Note = follow.Notes,
                    ShipmentId = follow.ShipmentId,
                    QuoteId = follow.QuoteId,
                    EventTypeId = follow.EventTypeId,
                    EventTypeFollowUpName = follow.EventType.FollowUpEnglishName,
                    ManualActivatedFollowUp = follow.EventType.ManualActivatedFollowUp,
                    OwnerUserId = follow.OwnerUserId,
                    OwnerUserName = follow.OwnerUser.Contact.EnglishName,
                    Area = follow.Area,
                    DocumentTypeId = follow.DocumentTypeId,
                    AutomationId = follow.AutomationId,

                };
                entityPM.FollowUps.Add(followUpPM);
            }
        }

        private List<FollowUp> RefreshQuoteFollowUpPMList(List<FollowUp> followupList, FollowUpRepository followUpsRepository)
        {
            if (followupList == null)
            {
                followupList = followUpsRepository.GetFollowUpsByQuoteId(entityPM.Id, entityPM.Tenant);
            }

            return followupList;
        }
          
        private void ClearFollowUps()
        {
            if (entityPM.FollowUps.Count != 0)
            {
                entityPM.FollowUps.Clear();
            }
        }
    }
}
