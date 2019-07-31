using Logitude.TimeManagement.BL.EntityPMs;
using Logitude.TimeManagement.BL.EntityQueryServices;
using Logitude.TimeManagement.Data;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.Data.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.APIHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class TimeOfficeHourDomainController : ApiController
    {

        public HttpResponseMessage GetTimeOfficeClock(string employeeUserId, string FromDate, string ToDate)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("TMOfficeHour", "READ", tenant);

                List<TMOfficeHourPM> myResult = this.GetTimeOffice(employeeUserId, FromDate, ToDate, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        private List<TMOfficeHourPM> GetTimeOffice(string employeeUserId, string FromDate, string ToDate, int tenant)
        {

            DateTime? myStartDate = DateHelper.GetDate(FromDate);

            if (myStartDate == null)
            {
                throw new ApplicationException("Please select from date");
            }

            DateTime? myEndDate = DateHelper.GetDate(ToDate);

            if (myEndDate == null)
            {
                throw new ApplicationException("Please select to date");
            }

            TMOfficeHourQuery query = new TMOfficeHourQuery(tenant);

            List<TMOfficeHourPM> myResult = query.GetTMOfficeHoursByUserIdAndDate(employeeUserId, myStartDate, myEndDate, tenant).ToList();
            foreach(TMOfficeHourPM item in myResult)
            {
                if (item.ExitTime != null && item.EntryTime != null)
                {
                    item.Minutes = (item.ExitTime.Value - item.EntryTime.Value).TotalMinutes;
                }
            }

            return myResult;

        }

        public HttpResponseMessage post(List<TMOfficeHourPM> args)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                ITimeManagementContext myContext = TimeManagementContext.GetContext(tenant);
                TMOfficeHourRepository repository = new TMOfficeHourRepository(myContext);

                foreach (var item in args)
                {
                    TMOfficeHour OfficeHourItem = repository.GetSingle(item.Id, tenant);
                    OfficeHourItem.Inactive = item.Inactive;
                    OfficeHourItem.ExitTime = item.ExitTime;
                    OfficeHourItem.EntryTime = item.EntryTime;
                    OfficeHourItem.Description = item.Description;
                    OfficeHourItem.UpdatedByUserId = item.UpdatedByUserId;
                    OfficeHourItem.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    repository.Update(OfficeHourItem);
                }

                repository.SubmitChanges();
                return Request.CreateResponse(HttpStatusCode.OK, args);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}