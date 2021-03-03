using Logitude.BL.QuoteModel.EntityPMs;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection; 

namespace Logitude.BL.QuoteModel.Tools.EntityService
{
    class QuoteFollowUpUpdateService
    {
        private QuotePM entityPM;
        private int tenant;
        private bool isChange = false;
        public QuoteFollowUpUpdateService(QuotePM entityPM, int tenant)
        {
            this.entityPM = entityPM;
            this.tenant = tenant;
        }
        public void RefreshFollowUps()
        { 
            if (entityPM.IsRefreshQuoteFollowUps)
            {
                IWebFreightContext freightContext = WebFreightContext.GetContext(tenant);
                FollowUpRepository followUpsRepository = new FollowUpRepository(freightContext);
                List<FollowUp> allFollowupLists = null;
                List<FollowUp> followupLists = null;

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
                bool isAnyOneChange = RefreshFollowUpsDateFieldList(followUpsRepository, followupLists);
                if (isAnyOneChange) followUpsRepository.SubmitChanges();

            }
        }

        private bool RefreshFollowUpsDateFieldList(FollowUpRepository followUpsRepository, List<FollowUp> followupLists)
        {
            bool isAnyOneChange = false;
            foreach (FollowUp follow in followupLists)
            {  
                isAnyOneChange =  UpdateFollowUpsDateField(followUpsRepository, follow);
            }

            return isAnyOneChange;
        }

        private bool UpdateFollowUpsDateField(FollowUpRepository followUpsRepository, FollowUp follow)
        {
            bool isAnyOneChange = false;
            if (!string.IsNullOrEmpty(follow.DateFieldName))
            {
                PropertyInfo propInfo = entityPM.GetType().GetProperty(follow.DateFieldName);
                if (propInfo != null)
                {
                    object fieldValue = propInfo.GetValue(entityPM);

                    isAnyOneChange = UpdateDateField(follow, fieldValue);

                    if (isChange) followUpsRepository.Update(follow);
                }
            }
            return isAnyOneChange;
        }

        private bool UpdateDateField( FollowUp follow, object fieldValue)
        {
            bool isAnyOneChange = false;
            if (fieldValue != null)
            {
                DateTime? fieldValuedate = (DateTime?)fieldValue;

                fieldValuedate = AddFollowupDateEscalationTimeDays(follow, fieldValuedate);

                isAnyOneChange = SetQuoteFollowUpsDate(follow, fieldValuedate);

            }
            return isAnyOneChange;
        }

        private bool SetQuoteFollowUpsDate(FollowUp follow, DateTime? fieldValuedate)
        {
            bool isAnyOneChange = false;
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
            return isAnyOneChange;
        }

        private static DateTime? AddFollowupDateEscalationTimeDays(FollowUp follow, DateTime? fieldValuedate)
        {
            DateTime? theFieldValuedate = fieldValuedate; 
            if (follow.DateEscalationActionTimeIndicatorCode != "IM" && follow.DateEscalationTime != 0)
            {
                int dateEscalationTime = follow.DateEscalationActionTimeIndicatorCode == "AF" ? follow.DateEscalationTime : follow.DateEscalationTime * -1;
                theFieldValuedate = fieldValuedate.Value.AddDays(dateEscalationTime);

            }

            return theFieldValuedate;
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


 