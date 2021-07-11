using Logitude.Test.Base.Models.PartnersPreparation;
using Logitude.Test.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRMTests.Models.Builders
{
    public class ActivityBuilder
    {
        private List<ActivityPriorityPM> activityPriorities;
        private ActivityPM _activity;
        public ActivityBuilder()
        {
            this.SetActivityProiorities();
            this.Reset();
        }

        private void SetActivityProiorities()
        {
            activityPriorities = new List<ActivityPriorityPM>
            {
                new ActivityPriorityPM{Code ="01", Name ="Low"},
                new ActivityPriorityPM{Code ="02", Name ="Normal"},
                new ActivityPriorityPM{Code ="03", Name ="High"}
            };
        }

        private void Reset()
        {
            _activity = new ActivityPM();
        }

        public ActivityBuilder Subject(string subject)
        {
            _activity.Subject = subject;
            return this;
        }

        public ActivityBuilder Location(string location)
        {
            _activity.Location = location;
            return this;
        }


        public ActivityBuilder Description(string description)
        {
            _activity.Description = description;
            return this;
        }

        public ActivityBuilder ActivityTimeTypeCode(string activityTimeTypeCode)
        {
            _activity.ActivityTimeTypeCode = activityTimeTypeCode;
            return this;
        }      

        public ActivityBuilder Duration(int? duration)
        {
            _activity.Duration = duration;
            return this;
        }

        public ActivityBuilder StartDateTime(DateTime? dateTime)
        {
            _activity.StartDateTime = dateTime;
            return this;
        }

        public ActivityBuilder DueDate(DateTime? dueDate)
        {
            _activity.DueDate = dueDate;
            return this;
        }   
        public ActivityBuilder EndDateTime(DateTime? endDateTime)
        {
            _activity.EndDateTime = endDateTime;
            return this;
        }

        public ActivityBuilder PriorityCode(string priority)
        {
            _activity.PriorityCode = activityPriorities.FirstOrDefault(x => x.Name == priority)?.Code;
            return this;
        }
        
        public ActivityBuilder ActivityStatusCode(string activityStatusCode)
        {
            _activity.ActivityStatusCode = activityStatusCode;
            return this;
        }

        public ActivityBuilder ActivityTypeCode(string activityTypeCode)
        {
            _activity.ActivityTypeCode = activityTypeCode;
            return this;
        }

        public ActivityBuilder BusinessUnitId(string businessUnitId)
        {
            _activity.BusinessUnitId = businessUnitId;
            return this;
        }

        public ActivityPM Build()
        {
            ActivityPM result = _activity;
            this.Reset();
            return result;
        }

        public ActivityBuilder WithModel(ActivityPM activity)
        {
            _activity = activity;
            return this;
        }

        public ActivityBuilder WithDefualtValues()
        {
            _activity = new ActivityPM
            {
                Tenant = UserTenant.Tenant,
                CustomerId = PartnersData.CustomerId,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                OwnerId = UserTenant.UserId,
                BranchId = UserTenant.BranchId,
                BusinessUnitId = UserTenant.BusinessUnitId,
                CallWithId = PartnersData.ContactId
            };
            return this;
        }
    }
}
