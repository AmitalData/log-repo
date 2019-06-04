using Logitude.BL.Helpers;
using Logitude.TimeManagement.BL.EntityPMs;
using Logitude.TimeManagement.BL.EntityUpdateServices;
using Logitude.TimeManagement.Data;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.Data.Repositories;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Logitude.Server.Tools.QueueService;

namespace WebFreight.Web.Helpers
{
    public class TFSParseWebhook
    {
        public TFSResponse Details;
        public string AnalyzeQueueId;
        public int Tenant = 0;
        public bool IsAnalyzeQueueFaild = false;
        public object Description = "";
        public string projectNo = "";

        public TFSParseWebhook()
        {

        }
        public TFSParseWebhook(TFSResponse details, string AnalyzeQueueId, int tenant)
        {
            this.Details = details;
            this.AnalyzeQueueId = AnalyzeQueueId;
            this.Tenant = tenant;

            foreach (var item in this.Details.Relations)
            {
                string url = item.Url;

                if (item.Rel == "System.LinkTypes.Hierarchy-Reverse")
                {
                    string last = url.Split('/').Last();
                    projectNo = this.GetWorkItemById(Int32.Parse(last));
                    this.Details.ProjectNumber = projectNo;
                    this.CheckComputingPartners();
                    break;
                }
            }
        }

        bool isFirst = true;
        public string GetWorkItemById(int wi, bool isOutSide = true)
        {
            // Create a connection to the account
            string accountUri = "https://logitudeteam.visualstudio.com";
            var personalAccessToken = "qsxsy6j454xpslikiuzc5oynhh5djttgxj4gmnlzpuaeypbuyc3q";
            int workItemId = wi;

            // new VssOAuthAccessTokenCredential(personalAccessToken)
            VssConnection connection = new VssConnection(new Uri(accountUri), new VssBasicCredential("logitudo@live.com", personalAccessToken));
            // Get an instance of the work item tracking client
            WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();
            object projectNo = "";

            try
            {
                // Get the specified work item
                WorkItem workitem = witClient.GetWorkItemAsync(workItemId, null, null, WorkItemExpand.Relations).Result;
                var s = new StringBuilder();

                // Output the work item's field values
                projectNo = workitem.Fields.Where(a => a.Key == "LogitudeProcess.ProjectNumber").Select(a => a.Value).FirstOrDefault();
                if (isFirst && isOutSide)
                {
                    WorkItem workitem_description = witClient.GetWorkItemAsync(Int32.Parse(this.Details.WorkItemId), null, null, WorkItemExpand.Relations).Result;
                    Description = workitem_description.Fields.Where(a => a.Key == "System.Title").Select(a => a.Value).FirstOrDefault();
                }
                if (projectNo == null)
                {
                    if (workitem.Relations != null)
                    {
                        var relation = workitem.Relations.Where(a => a.Rel == "System.LinkTypes.Hierarchy-Reverse").FirstOrDefault();

                        if (relation != null)
                        {
                            isFirst = false;
                            string last = relation.Url.Split('/').Last();
                            return this.GetWorkItemById(Int32.Parse(last));
                        }
                    }
                }
            }
            catch (AggregateException aex)
            {
                VssServiceException vssex = aex.InnerException as VssServiceException;
                if (vssex != null)
                {
                    //Console.WriteLine(vssex.Message);
                    throw new Exception(vssex.Message);
                }
            }

            return projectNo != null ? projectNo.ToString() : "";
        }
        private void CheckComputingPartners()
        {
            ICommonDataContext context = CommonDataContext.GetContext(Tenant); ;
            ComputingPartnerRepository computingRepository = new ComputingPartnerRepository(context);
            ComputingPartnerTableRepository computingTableRepository = new ComputingPartnerTableRepository(context);
            ComputingPartnerTranslationRepository computingTranslationRepository = new ComputingPartnerTranslationRepository(context);
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(Tenant);

            ComputingPartner computingPartner = computingRepository.GetSingleComputingPartnerByCode("G-TFS"); // Computing Partner for TimeSheet = "TFS"
            if (computingPartner != null)
            {
                //var objectTable = objectTableRepository.GetObjectTableByName("User", Tenant, false);
                //ComputingPartnerTable computingTable = computingTableRepository.GetSingleComputingPartnerTable(Tenant, objectTable.Id, computingPartner.Id);
                //if (computingTable != null)
                //{
                //    checkPartner = true;
                //    this.InsertTMEmployeeTime(checkPartner);
                //}
                //else
                //{
                //    this.InsertTMEmployeeTime(checkPartner);
                //}

                this.InsertTMEmployeeTime(true);
            }
            else
            {
                this.InsertTMEmployeeTime(false);
            }
        }
        private void InsertTMEmployeeTime(bool checkPartner)
        {
            ITimeManagementContext myContext = TimeManagementContext.GetContext(Tenant);
            TMEmployeeTimeRepository tmEmployeeTimeRepository = new TMEmployeeTimeRepository(myContext);
            TMProjectRepository tmProjectRepository = new TMProjectRepository(myContext);
            ComputingPartnerTranslationHelper computingPartnerHelper = new ComputingPartnerTranslationHelper(Tenant);
            UserRepository userRepository = new UserRepository(Tenant);
            TMEmployeeTimeUpdateService service = new TMEmployeeTimeUpdateService(myContext, new Dictionary<string, IContext>(), Tenant);

            User assignedToUser = null;
            User updatedByUser = null;
            User createdByUser = null;

            string assignedToUserEmail = "";
            string updatedByUserEmail = "";
            string createdByUserEmail = "";

            if (checkPartner)
            {
                assignedToUserEmail = computingPartnerHelper.GetLogitudeCodeTranslation(Details.AssignedTo, "G-TFS", "User");
                updatedByUserEmail = computingPartnerHelper.GetLogitudeCodeTranslation(Details.ChangedBy, "G-TFS", "User");
                createdByUserEmail = computingPartnerHelper.GetLogitudeCodeTranslation(Details.CreatedBy, "G-TFS", "User");
            }
            else
            {
                assignedToUserEmail = Details.AssignedTo;
                updatedByUserEmail = Details.ChangedBy;
                createdByUserEmail = Details.CreatedBy;
            }
            if (!string.IsNullOrEmpty(assignedToUserEmail))
            {
                assignedToUser = userRepository.GetSingleUserByCodeOrEmail(null, assignedToUserEmail, Tenant, true);
            }

            if (!string.IsNullOrEmpty(updatedByUserEmail))
            {
                updatedByUser = userRepository.GetSingleUserByCodeOrEmail(null, updatedByUserEmail, Tenant, true);
            }

            if (!string.IsNullOrEmpty(createdByUserEmail))
            {
                createdByUser = userRepository.GetSingleUserByCodeOrEmail(null, createdByUserEmail, Tenant, true);
            }

            var projectId = tmProjectRepository.GetTMProjectByNumber(Details.ProjectNumber, Tenant);
            if (assignedToUser != null && updatedByUser != null)
            {
                if ((assignedToUser.Id == updatedByUser.Id) && Details.RemainingWork != null && (Details.TaskState == "In Progress" || Details.TaskState == "Committed" || Details.TaskState == "Done"))
                {
                    if (!CheckTMLineDuplication(this.Details.WorkItemId, Tenant)) {
                        var newItem = new TMEmployeeTimePM();
                        //newItem.Id = IdCounter.GetNumber("TMEmployeeTime", Tenant);
                        newItem.Tenant = Tenant;
                        newItem.DateOfWork = this.Details.ChangedDate != null ? this.Details.ChangedDate.Date : this.Details.ChangedDate;
                        newItem.CreateDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                        newItem.UpdateDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                        newItem.ProjectId = projectId;
                        newItem.Description = Description + "";
                        newItem.WINumber = this.Details.WorkItemId;
                        newItem.EmployeeUserId = assignedToUser.Id;
                        newItem.LocationCode = "O";
                        newItem.UpdatedByUserId = updatedByUser.Id;
                        newItem.CreatedByUserId = updatedByUser.Id;
                        newItem.AnalyzeQueueId = this.AnalyzeQueueId;
                        newItem.NeedsProrating = true;
                        var sprint = computingPartnerHelper.GetLogitudeCodeTranslation(Details.IterationPath, "G-TFS", "Sprint");
                        SprintRepository sprintRepository = new SprintRepository(Tenant);
                        var sprintPOCO = sprintRepository.GetSprintByName(sprint, Tenant);
                        newItem.SprintId = sprintPOCO != null ? sprintPOCO.Id : null;
                        newItem.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                        service.Update(newItem, true);
                    }
                }
            }
            else
            {
                this.IsAnalyzeQueueFaild = true;
            }
        }

        public bool CheckTMLineDuplication(string workItemId, int tenant)
        {
            var isDuplicate = false;
            var todayDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
            ITimeManagementContext myContext = TimeManagementContext.GetContext(Tenant);
            IQueryable<TMEmployeeTime> iQueryable;

            iQueryable = (from d in myContext.TMEmployeeTimes
                          where d.Tenant == tenant && d.WINumber == workItemId && System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) == System.Data.Entity.DbFunctions.TruncateTime(todayDate)
                          select d);

            isDuplicate = (iQueryable != null && iQueryable.Count() == 0) ? false : true;

            return isDuplicate;
        }

        public List<TMEmployeeTime> GetProjects(List<TMEmployeeTime> list, int tenant)
        {
            ITimeManagementContext myContext = TimeManagementContext.GetContext(tenant);
            TMEmployeeTimeRepository iTMEmployeeTimeRepository = new TMEmployeeTimeRepository(myContext);
            TMProjectRepository iTMProjectRepository = new TMProjectRepository(myContext);
            int count = 0;

            foreach (var item in list)
            {
                int iWorkItemNumber;

                if (Int32.TryParse(item.WINumber, out iWorkItemNumber))
                {
                    string iProjectNumber = GetWorkItemById(iWorkItemNumber, false);
                    if (!string.IsNullOrEmpty(iProjectNumber))
                    {
                        string iProjectId = iTMProjectRepository.GetTMProjectByNumber(iProjectNumber, tenant);

                        if (!string.IsNullOrEmpty(iProjectId))
                        {
                            if (item.ProjectId != iProjectId)
                            {
                                item.ProjectId = iProjectId;
                                iTMEmployeeTimeRepository.Update(item);
                                count++;
                            }
                        }
                    }
                }

                if (count >= 100)
                {
                    count = 0;
                    iTMEmployeeTimeRepository.SubmitChanges();
                }
            }

            if (count > 0)
            {
                iTMEmployeeTimeRepository.SubmitChanges();
            }
            return list;
        }

        public void CalculateCompletedWorkHours(int tenant)
        {
            ITimeManagementContext myContext = TimeManagementContext.GetContext(tenant);
            List<TMEmployeeTime> tmEmployeelist = (from d in myContext.TMEmployeeTimes
                                                   where d.Tenant == tenant && d.DateOfWork != null && d.WINumber != null
                                                   select d).ToList();

            var groupedItems = (from d in tmEmployeelist
                                group d by new { d.WINumber, d.Tenant } into g
                                select new
                                {
                                    Tenant = g.Key.Tenant,
                                    WINumber = g.Key.WINumber,
                                    CompletedWork = (g.Sum(s => s.TimeInMinutes)) / 60.00,
                                });
            int counter = 0;
            int currentIndex = 0;
            int groupSize = groupedItems.Count();
            foreach (var item in groupedItems)
            {
                counter += 1;
                currentIndex += 1;
                SendQueueMessage(item.WINumber, item.CompletedWork, item.Tenant);

                if (counter == 10)
                {
                    counter = 0;
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }
        private void SendQueueMessage(string wINumber, double completedWork, int tenant)
        {
            string queueName = "timemanagementqueue";
            if (!string.IsNullOrEmpty(wINumber))
            {
                try
                {
                    DbQueueService queueservice = new DbQueueService(queueName, tenant);
                    Dictionary<string, string> message = new Dictionary<string, string>()
                    {
                        { "Tenant", tenant.ToString() },
                        { "WorkItemNumber",  wINumber },
                        { "CompletedWork", completedWork.ToString("0.##")},
                    };

                    queueservice.Send(message);
                }
                catch (Exception ex)
                {
                    string ip = "";
                    if (HttpContext.Current != null && HttpContext.Current.Request != null)
                    {
                        string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                        if (string.IsNullOrEmpty(currentIP))
                        {
                            currentIP = HttpContext.Current.Request.UserHostAddress;
                        }
                        ip = currentIP;
                    }
                    throw new Exception(ex + "\t" + ip);
                }
            }
        }
    }
}