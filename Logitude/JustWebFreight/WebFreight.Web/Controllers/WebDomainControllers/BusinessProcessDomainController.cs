using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Repsitories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class BusinessProcessDomainController : ApiController
    {
        public HttpResponseMessage GetQueuesWithCounts(string myFilter)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int myTenant = authToken.Tenant;

                IInfrastructureContext context = InfrastructureContext.GetContext(myTenant);
                List<QueueData> myResult = new List<QueueData>();
                BusinessProcessQueueRepository businessProcessQueueRepository = new BusinessProcessQueueRepository(context);
                ActivityRepository activityRepository = new ActivityRepository(myTenant);

                IQueryable<BusinessProcessQueue> queues = businessProcessQueueRepository.GetAll(myTenant);
                List<Activity> activities = activityRepository.GetExtendedActivities(myTenant).ToList();

                if (activities.Count > 0)
                {
                    ContactRepository contactRep = new ContactRepository(myTenant);
                    Contact loggedContact = contactRep.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), myTenant);
                    string loggedContactId = loggedContact == null ? "" : loggedContact.Id;

                    switch (myFilter)
                    {
                        case "My":
                            {
                                if (!string.IsNullOrEmpty(loggedContactId))
                                {
                                    activities = activities.Where(d => d.OwnerId == loggedContactId).ToList();
                                }

                                break;
                            }

                        case "Team":
                            {
                                if (!string.IsNullOrEmpty(loggedContactId))
                                {
                                    LBPTeamMemberRepository LBPTeamMemberRepository = new LBPTeamMemberRepository(context);
                                    IQueryable<LBPTeamMember> members = LBPTeamMemberRepository.GetAll(myTenant).Where(d => d.MemberUserId != null && d.MemberUserId == loggedContactId);

                                    List<string> ids = members.Select(s => s.TeamId).ToList();
                                    activities = activities.Where(d => ids.Contains(d.TeamId)).ToList();
                                }

                                break;
                            }

                        case "All":
                            {

                                break;
                            }
                    }

                    myResult.Add(new QueueData()
                    {
                        QueueId = null,
                        QueueName = "All Queues",
                        Count = activities.Count(),
                    });

                    foreach (BusinessProcessQueue item in queues)
                    {
                        QueueData myData = new QueueData();
                        myData.QueueId = item.Id;
                        myData.QueueName = item.Name;
                        myData.Count = activities.Where(d => d.BusinessProcessQueueId == item.Id).Count();

                        myResult.Add(myData);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetTeamsForLoggedUser(string loggedUserId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int myTenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                LBPTeamMemberRepository LBPTeamMemberRepository = new LBPTeamMemberRepository(myTenant);
                IQueryable<LBPTeamMember> members = LBPTeamMemberRepository.GetAll(myTenant).Where(d => d.MemberUserId != null && d.MemberUserId == loggedUserId);

                List<string> teamsIds = members.Select(s => s.TeamId).ToList();

                string teamsIdsString = "";
                foreach(string id in teamsIds)
                {
                    if(string.IsNullOrEmpty(teamsIdsString))
                    {
                        teamsIdsString = id;
                    }
                    else
                    {
                        teamsIdsString += "," + id;
                    }
                }
                
                return Request.CreateResponse(HttpStatusCode.OK, teamsIdsString);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }

    public class QueueData
    {
        public int Count { get; set; }
        public string QueueId { get; set; }
        public string QueueName { get; set; }
    }
}