using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.EntityChanges.AutomationResult
{
    public class AutomationQueuedTaskResultService : GeneralAutomationResultService, IAutomationResultService
    {
        AutomationResultArgs automationResultArgs { get; set; }
        public void Run(AutomationResultArgs automationResultArgs)
        {
            this.automationResultArgs = automationResultArgs;
            List<Automation> queueautomationsList = automationResultArgs.AutomationLists.Where(d => d.ResultCode == "QUEUE").ToList();
            if (queueautomationsList.Count > 0)
            {

                var otherObjectTableIdWithLastUpdate = (automationResultArgs.OtherAutomationObjectTable != null && !string.IsNullOrEmpty(automationResultArgs.OtherAutomationObjectTable.AutomationLastUpdate)) ? (automationResultArgs.OtherAutomationObjectTable.Id + "@" + automationResultArgs.OtherAutomationObjectTable.AutomationLastUpdate) : "";

                ApplyQueuedTaskAutomation(automationResultArgs.EntityChangeArgs.EntityPM, queueautomationsList, automationResultArgs.EntityChange, automationResultArgs.AutomationFieldLists, automationResultArgs.AutomationObjectTable.AutomationLastUpdate, automationResultArgs.EntityChangeArgs.OldEntityPM, automationResultArgs.EntityChangeArgs.ProcessType, automationResultArgs.EntityChangeArgs.EntityId, otherObjectTableIdWithLastUpdate);
            }
        }


        private void ApplyQueuedTaskAutomation(Object entityPM, List<Automation> automations, EntityChange entityChange, List<Field> automationFieldLists, string lastupdateautomation, Object oldentityPM, string processtype, string entityId, string otherLastupdateautomation)
        {
            if (automations.Count > 0)
            {
                foreach (Automation automation in automations)
                {
                    DateTime dateNow = DateTime.Now;
                    EntityChangeAutomation entityChangesAutomation = CreateEntityChangeAutomation(automation);
                    entityChangesAutomation.ResultCode = "Queued Task";

                    string lastUpdate = GetLastAuomationUpdateDate(automationResultArgs.AutomationObjectTable, automationResultArgs.OtherAutomationObjectTable, automation);

                    ValidateAutomationResultClass validateResult = this.ValidateAutomation(automation, entityChange, automationFieldLists, lastUpdate, "");
                    entityChangesAutomation.ConditionsList = validateResult.ConditionsList;

                    if (validateResult.Type == "Delayed")
                    {
                        automationResultArgs.MainEntityChangeService.IsDelayAutomation = true;
                    }

                    entityChangesAutomation.type = validateResult.IsAutomationValid ? "QueuedTaskCreatedSsucceed" : "QueuedTaskCreatedFailed";

                    if (validateResult.IsAutomationValid)
                    {
                        if (validateResult.Type == "Delayed")
                        {
                            DelaytimeDetails delaytimeDetails = new DelaytimeDetails() { Type = validateResult.Type, Delaytime = validateResult.Delaytime, DelaytimeIndicator = validateResult.DelaytimeIndicator, DelaytimeOp = validateResult.DelaytimeOp, SelectedDelaytimeFieldCode = validateResult.SelectedDelaytimeFieldCode };
                            AddAutomationQueue(new AutomationQueueArgs() { EntityChangeId = entityChange.Id, AutomationId = automation.Id, AutomationType = processtype, EntityId = entityId, Tenant = automation.Tenant, AutomationDelayTime = GetAutomationDelayTime(delaytimeDetails, automationFieldLists, entityChange.Tenant) });
                        }

                        else
                        {
                            this.AddAutomationQueuedTask(entityPM, entityChange, automationFieldLists, lastUpdate, automationResultArgs.MainEntityChangeService.EntityChangesAutomationsSsucceedList, automation, entityChangesAutomation, dateNow);
                        }

                    }
                    else
                    { 
                        entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(entityChange.Tenant);
                        automationResultArgs.MainEntityChangeService.EntityChangesAutomationsFailedList.Add(entityChangesAutomation);
                        entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - dateNow.Ticks) / TimeSpan.TicksPerMillisecond);
                    }
                }
            }
        }

        public void AddAutomationQueuedTask(Object entityPM, EntityChange entityChange, List<Field> automationFieldLists, string lastupdateautomation, List<EntityChangeAutomation> EntityChangesAutomationsSsucceedList, Automation automation, EntityChangeAutomation entityChangesAutomation, DateTime dateNow)
        {
            int tenant = automation.Tenant;
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);

            ContactRepository contactRepository = new ContactRepository(commonContext);
            UserRepository userRepository = new UserRepository(commonContext);

            AutomationQueuedTask automationQueuedTask = null;

            #region  Get AutomationQueuedTask From Cache

            string automationQueuedTaskName = "AutomationQueuedTask" + lastupdateautomation + automation.Id + tenant;

            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(automationQueuedTaskName) == null)
                {
                    if (!string.IsNullOrEmpty(automation.AutomationXML))
                    {
                        AutomatedBackup AutomatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automation.AutomationXML);
                        automationQueuedTask = AutomatedBackup.AutomationQueuedTask;
                        CacheManager.CacheWrapper.Insert(automationQueuedTaskName, automationQueuedTask, null, System.DateTime.UtcNow.AddHours(12), TimeSpan.Zero);
                    }
                }
                else
                {
                    automationQueuedTask = (AutomationQueuedTask)CacheManager.CacheWrapper.Get(automationQueuedTaskName);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(automation.AutomationXML))
                {
                    AutomatedBackup AutomatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automation.AutomationXML);
                    automationQueuedTask = AutomatedBackup.AutomationQueuedTask;
                }
            }

            #endregion

            if (automationQueuedTask != null)
            {
                string dueDateFieldName = automationQueuedTask.DateFieldValue;
                string offsetType = automationQueuedTask.OffsetTypeValue;
                string timeUnit = automationQueuedTask.TaskTimeUnitValue;
                string queueId = automationQueuedTask.QueueId;
                string teamId = automationQueuedTask.TeamId;
                int? offset = automationQueuedTask.TaskOffset;
                string ownerId = automationQueuedTask.TaskOwnerId;
                string customerId = automationQueuedTask.TaskCustomerId;
                string subject = automationQueuedTask.TaskSubject;
                string description = automationQueuedTask.TaskDescription;
                string priorityId = automationQueuedTask.TaskPriorityId;
                DateTime? dueDate = null;

                if (!string.IsNullOrEmpty(queueId))
                {
                    string businessRoleId = "";

                    IInfrastructureContext infraContext = InfrastructureContext.GetContext(tenant);
                    BusinessProcessQueueRepository businessProcessQueueRepository = new BusinessProcessQueueRepository(infraContext);

                    BusinessProcessQueue queue = businessProcessQueueRepository.GetSingle(queueId, tenant);
                    if (queue != null)
                    {
                        businessRoleId = queue.BusinessRoleId;
                    }

                    if (!string.IsNullOrEmpty(businessRoleId))
                    {
                        TeamMemberBusinessRoleRepository teamMemberBusinessRoleRepository = new TeamMemberBusinessRoleRepository(infraContext);
                        IQueryable<TeamMemberBusinessRole> teamMemberBusinessRoles = teamMemberBusinessRoleRepository.GetAllByBusinessRoleId(businessRoleId, tenant);

                        string loggedUserBranchId = "";
                        string loggedUserBusinessUnitId = "";

                        User loggedUser = userRepository.GetSingleUser(entityChange.CreateByUserId, tenant);
                        if (loggedUser != null)
                        {
                            loggedUserBranchId = loggedUser.BranchId;
                            loggedUserBusinessUnitId = loggedUser.BusinessUnitId;
                        }

                        if (string.IsNullOrEmpty(ownerId))
                        {
                            TeamMemberBusinessRole myMember = teamMemberBusinessRoles.FirstOrDefault();

                            if (myMember != null && myMember.LBPTeamMember != null)
                            {
                                ownerId = myMember.LBPTeamMember.MemberUserId;
                            }
                        }

                        else
                        {
                            if (string.IsNullOrEmpty(teamId))
                            {
                                TeamMemberBusinessRole myMember = teamMemberBusinessRoles.FirstOrDefault();

                                if (myMember != null && myMember.LBPTeamMember != null)
                                {
                                    teamId = myMember.LBPTeamMember.TeamId;
                                }
                            }
                        }

                        Type type = entityPM.GetType();
                        string entityId = string.Empty;
                        Object value = GetPropertyValue(entityPM, type, "Id");
                        if (value != null)
                        {
                            entityId = value.ToString();
                        }

                        ActivityRepository activityRepository = new ActivityRepository(tenant);
                        Activity activity = new Activity();
                        activity.Id = IdCounter.GetNumber("Activity", tenant);
                        activity.Tenant = tenant;
                        activity.ConcurrencyGUID = Guid.NewGuid().ToString();
                        activity.ShipmentId = entityId;
                        activity.IsOpen = true;
                        activity.ActivityStatusCode = "N";
                        activity.ActivityTypeCode = "TX";
                        activity.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                        activity.CreatedByUserId = entityChange.CreateByUserId;
                        activity.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                        activity.UpdatedByUserId = entityChange.CreateByUserId;
                        activity.BranchId = loggedUserBranchId;
                        activity.BusinessUnitId = loggedUserBusinessUnitId;
                        activity.PriorityCode = !string.IsNullOrEmpty(priorityId) ? priorityId : "02";
                        activity.OwnerId = string.IsNullOrEmpty(ownerId) ? null : ownerId;
                        activity.CustomerId = string.IsNullOrEmpty(customerId) ? null : customerId;
                        activity.BusinessProcessQueueId = queueId;
                        activity.TeamId = string.IsNullOrEmpty(teamId) ? null : teamId;
                        activity.Description = description;
                        activity.Subject = subject;

                        if (offset != null)
                        {
                            int? myOffset = null;
                            switch (offsetType)
                            {
                                case "A":
                                    {
                                        myOffset = offset;
                                        break;
                                    }

                                case "B":
                                    {
                                        myOffset = offset * -1;
                                        break;
                                    }
                            }

                            if (timeUnit == "D")
                            {
                                activity.DueDateOffset = myOffset * 24;
                            }
                            else
                            {
                                activity.DueDateOffset = myOffset;
                            }
                        }

                        if (!string.IsNullOrEmpty(dueDateFieldName))
                        {
                            activity.DueDateDateField = dueDateFieldName;

                            Type dateType = entityPM.GetType();
                            if (dateType != null)
                            {
                                PropertyInfo propInfo = dateType.GetProperty(dueDateFieldName);

                                if (propInfo != null)
                                {
                                    object dateObject = propInfo.GetValue(entityPM);
                                    if (dateObject != null)
                                    {
                                        DateTime? dateValue = (DateTime)dateObject;
                                        if (dateValue != null)
                                        {
                                            if (activity.DueDateOffset == null)
                                            {
                                                dueDate = dateValue;
                                            }

                                            else
                                            {
                                                dueDate = dateValue.Value.AddHours((double)activity.DueDateOffset);
                                            }

                                            activity.DueDate = dueDate;
                                        }
                                    }
                                }
                            }
                        }

                        activityRepository.Add(activity);
                        activityRepository.SubmitChanges();

                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = tenant,
                            EventTypeCode = "CRAV",
                            UserId = entityChange.CreateByUserId,
                            EntityId = activity.Id,
                            ObjectTableName = "Activity",
                        });
                    }
                }

                entityChange.HasExecutedRecord = true;
                entityChangesAutomation.IsConditionTrue = true;
                entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(entityChange.Tenant);
                entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - dateNow.Ticks) / TimeSpan.TicksPerMillisecond);
                EntityChangesAutomationsSsucceedList.Add(entityChangesAutomation);
            }


        }
        private object GetPropertyValue(Object entityPM, Type type, string fieldName)
        {
            PropertyInfo propertyInf = type.GetProperty(fieldName);
            object value = null;
            if (propertyInf != null)
            {
                value = propertyInf.GetValue(entityPM, null);
            }
            return value;
        }


    }
}
