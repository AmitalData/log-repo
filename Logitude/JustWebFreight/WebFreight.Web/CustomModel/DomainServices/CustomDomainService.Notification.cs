using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public NotificationPM GetSingleNotificationPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            notificationQuery = new NotificationQueryService(customContext);
            NotificationPM Notification = notificationQuery.GetSingle(id, true, false);
            return Notification;
        }

        public NotificationList GetSingleNotificationList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           // SecurityUtility.CheckContactFeature("Customs.Notification", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            NotificationListQueryService listService = new NotificationListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<NotificationPM> GetTopTenNotifications(string userId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            NotificationQueryService queryService = new NotificationQueryService(customContext);
            List<NotificationPM> list = queryService.GetTopTenNotificationPMs(userId, tenant);
           
            return list;
           

        }

        [Invoke]
        public int GetNotificationsBadjCount(string userId, int tenant)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);

                customContext = CustomContext.GetContext(tenant);
                NotificationQueryService queryService = new NotificationQueryService(customContext);
                return queryService.GetBadjCount(userId, tenant);
            }
            catch (Exception ex)
            {
                string authenticateduser = "";

                try
                {
                    authenticateduser = Security.SecurityUtility.GetAuthenticatedUser();
                }

                catch
                {
                    authenticateduser = "UnKnown";
                }
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
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", authenticateduser, "", ip);

                return 0;
                
            }

        }

        [Invoke]
        public int GetOpenNotificationsCount(string userId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            NotificationQueryService queryService = new NotificationQueryService(customContext);
            return queryService.GetOpenNotificationCountForUser(userId, tenant);

        }


        public List<NotificationList> GetNotificationLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           // SecurityUtility.CheckContactFeature("Customs.Notification", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            NotificationListQueryService listService = new NotificationListQueryService(customContext);
            return listService.GetList(tenant);
         
        }



        //public List<NotificationPM> GetTopTenNotifications(strinng assigneeToId,int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    // SecurityUtility.CheckContactFeature("Customs.Notification", "READ", tenant);
        //    customContext = CustomContext.GetContext(tenant);
        //    NotificationQueryService queryService = new NotificationQueryService(customContext);
        //    return queryService.gettop(tenant);
        //    return new List<NotificationList>();
        //}



        public List<NotificationList> GetNotificationwithManagement(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("Customs.Notification", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            notificationQuery = new NotificationQueryService(customContext);
            List<NotificationList> notifications = notificationQuery.GetNotificationwithManaement(tenant);
            return notifications;
        }

        public List<NotificationPM> GetNotificationsByDefinitionCode(string objectTableId, string entityId, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            notificationQuery = new NotificationQueryService(customContext);
            List<NotificationPM> notifications = notificationQuery.GetNotificationByDefinitionCode(objectTableId, entityId,tenant);
            return notifications;
        }

        [Query(HasSideEffects = true)] 
        public List<NotificationList> GetNotificationFilters(byte[] xmlFilters, int tenant)
        {


            SecurityUtility.AuthenticationOnTenant(tenant);
           // SecurityUtility.CheckContactFeature("Customs.Notification", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            NotificationListQueryService listService = new NotificationListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetNotificationLists(queryOperations, tenant);

        }
        [Query(HasSideEffects = true)]
        public List<NotificationTabsDataCount> GetNotificationTabsCountFilters(byte[] xmlFilters, int tenant)
        {
            List<NotificationTabsDataCount> list = new List<NotificationTabsDataCount>();
            customContext = CustomContext.GetContext(tenant);
            NotificationListQueryService listService = new NotificationListQueryService(customContext);
   
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            int allNotificationsCount = listService.GetListCount(queryOperations, tenant);
         
       
            QueryFilterItem allDateItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "Dates" && d.Operator == "Between").FirstOrDefault();
            NotificationTabsDataCount count;
            QueryFilterItem CreateDateFilterItem = null;
            QueryFilterItem DueDateFilterItem = null;

          
            if (allDateItem != null)
            {
                DateTime? dueDate = null;
                if (allDateItem.FieldValue3 != null)
                {
                     dueDate = (DateTime?)allDateItem.FieldValue3;

                    dueDate = dueDate.Value.Date;
                    dueDate = dueDate.Value.AddHours(23).AddMinutes(59);
                }
               

                CreateDateFilterItem = new QueryFilterItem() { FieldName = "CreateDate", FieldValue = allDateItem.FieldValue, Operator = "Between", FieldValue2 = allDateItem.FieldValue2, DisplayInList = true };
                DueDateFilterItem = new QueryFilterItem() { FieldName = "DueDate", FieldValue = dueDate, Operator = "LessThanOrEqual", DisplayInList = true };
                queryOperations.QueryFilterItems.Remove(allDateItem);
                 queryOperations.QueryFilterItems.Add(CreateDateFilterItem);
            }



          
                int createDateNotificationsCount = listService.GetListCount(queryOperations, tenant);

                //if (CreateDateFilterItem.FieldValue == null && CreateDateFilterItem.FieldValue2 == null)
                //{
                //    createDateNotificationsCount = allNotificationsCount;
                //}

                if (CreateDateFilterItem != null && DueDateFilterItem != null)
                {
                    queryOperations.QueryFilterItems.Remove(CreateDateFilterItem);
                    queryOperations.QueryFilterItems.Add(DueDateFilterItem);
                }
                int dueDateNotificationsCount = listService.GetListCount(queryOperations, tenant);
                //if (DueDateFilterItem.FieldValue == null)
                //{
                //    dueDateNotificationsCount = allNotificationsCount;
                //}
                count = new NotificationTabsDataCount() { Id = Guid.NewGuid().ToString(), AllCount = allNotificationsCount, CreateDateCount = createDateNotificationsCount, DueDateCount = dueDateNotificationsCount };
                list.Add(count);
            

           
            return list;
           


        }

        [Query(HasSideEffects = true)]
        public List<NotificationFiltersDataCount> GetNotificationFiltersDataCount(byte[] xmlFilters, int tenant)
        {
           List<NotificationFiltersDataCount> countsList = new List<NotificationFiltersDataCount>();
            customContext = CustomContext.GetContext(tenant);
            NotificationListQueryService listService = new NotificationListQueryService(customContext);
            NotificationFiltersDataCount count = null;
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);

            QueryFilterItem notificationTypeFilterItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "AssigneToNotificationTypeCode" && d.Operator == "Equals").FirstOrDefault();
            string notificationType = null;
            if (notificationTypeFilterItem != null)
            {
                 notificationType = notificationTypeFilterItem.FieldValue.ToString();
                queryOperations.QueryFilterItems.Remove(notificationTypeFilterItem);
            }

            QueryFilterItem IsClosedByAssigneeFilterItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IsClosedByAssignee" && d.Operator == "Equals").FirstOrDefault();
            bool IsClosedByAssignee = false;
            if (IsClosedByAssigneeFilterItem != null)
            {
                IsClosedByAssignee = (bool)IsClosedByAssigneeFilterItem.FieldValue;
                queryOperations.QueryFilterItems.Remove(IsClosedByAssigneeFilterItem);
            }



           // int notificationCount = listService.GetListCount(queryOperations, tenant);
            queryOperations.GetAll = true;
           // List<NotificationList> allNotifications = listService.GetList(queryOperations, tenant);

          
            //All Count notification Type
            if (IsClosedByAssigneeFilterItem != null)
            {
                queryOperations.QueryFilterItems.Add(IsClosedByAssigneeFilterItem);
            }
            int allTypeNotifications = listService.GetListCount(queryOperations, tenant);
            //int allTypeNotifications = allNotifications.Where(d => d.IsClosedByAssignee == IsClosedByAssignee).Count();

            // info notifications
            QueryFilterItem infoFilterItem = new QueryFilterItem() { FieldName = "AssigneToNotificationTypeCode", Operator = "Equals", FieldValue = "I" };
            queryOperations.QueryFilterItems.Add(infoFilterItem);
            int allInfoNotifications = listService.GetListCount(queryOperations, tenant);
            queryOperations.QueryFilterItems.Remove(infoFilterItem);
            //int allInfoNotifications = allNotifications.Where(d => d.AssigneToNotificationTypeCode == "I" && d.IsClosedByAssignee == IsClosedByAssignee).Count();

            //Action notifications

            QueryFilterItem actionFilterItem = new QueryFilterItem() { FieldName = "AssigneToNotificationTypeCode", Operator = "Equals", FieldValue = "A" };
            queryOperations.QueryFilterItems.Add(actionFilterItem);
            int allActionNotifications = listService.GetListCount(queryOperations, tenant);
            queryOperations.QueryFilterItems.Remove(actionFilterItem);


            queryOperations.QueryFilterItems.Remove(IsClosedByAssigneeFilterItem);
            //int allActionNotifications = allNotifications.Where(d => d.AssigneToNotificationTypeCode == "A" && d.IsClosedByAssignee == IsClosedByAssignee).Count();
            int OpenCount = 0;
            int closedCount = 0;
            if (notificationType == null)
            {
                //open count
                QueryFilterItem openFilterItem = new QueryFilterItem() { FieldName ="IsClosedByAssignee",Operator="Equals",FieldValue=false};
                queryOperations.QueryFilterItems.Add(openFilterItem);
                OpenCount = listService.GetListCount(queryOperations, tenant);
                queryOperations.QueryFilterItems.Remove(openFilterItem);

               //closed Count
                QueryFilterItem closedFilterItem = new QueryFilterItem() { FieldName = "IsClosedByAssignee", Operator = "Equals", FieldValue = true };
                queryOperations.QueryFilterItems.Add(closedFilterItem);
                closedCount = listService.GetListCount(queryOperations, tenant);
                queryOperations.QueryFilterItems.Remove(closedFilterItem);
                //OpenCount = allNotifications.Where(d => !d.IsClosedByAssignee ).Count();
            }
            else
            {
                QueryFilterItem openFilterItem = new QueryFilterItem() { FieldName = "IsClosedByAssignee", Operator = "Equals", FieldValue = false };
                queryOperations.QueryFilterItems.Add(openFilterItem);
                queryOperations.QueryFilterItems.Add(notificationTypeFilterItem);
                OpenCount = listService.GetListCount(queryOperations, tenant);
                queryOperations.QueryFilterItems.Remove(openFilterItem);

                QueryFilterItem closedFilterItem = new QueryFilterItem() { FieldName = "IsClosedByAssignee", Operator = "Equals", FieldValue = true };
                queryOperations.QueryFilterItems.Add(closedFilterItem);
                queryOperations.QueryFilterItems.Add(notificationTypeFilterItem);
                closedCount = listService.GetListCount(queryOperations, tenant);

                queryOperations.QueryFilterItems.Remove(closedFilterItem);
                queryOperations.QueryFilterItems.Remove(notificationTypeFilterItem);

               // OpenCount = allNotifications.Where(d => d.AssigneToNotificationTypeCode == notificationType && !d.IsClosedByAssignee ).Count();
            }

            //int allOpenNotificationTypes = allNotifications.Where(d => !d.IsClosedByAssignee).Count();
            //int infoOpenNotificationTypes = allNotifications.Where(d => d.AssigneToNotificationTypeCode == "I" && !d.IsClosedByAssignee).Count();
            //int actionOpenNotificationTypes = allNotifications.Where(d => d.AssigneToNotificationTypeCode == "A" && !d.IsClosedByAssignee).Count();
            //int closedNoifications = allNotifications.Where(d => d.IsClosedByAssignee).Count();
            //int allClosedNotificationTypes = allNotifications.Where(d => d.IsClosedByAssignee).Count();
            //int infoClosedNotificationTypes = allNotifications.Where(d => d.IsClosedByAssignee && d.AssigneToNotificationTypeCode == "I").Count();
            //int actionClosedNotificationTypes = allNotifications.Where(d => d.IsClosedByAssignee && d.AssigneToNotificationTypeCode == "A").Count();
           


            //if (notificationType != null)
            //{
            //    notificationTypeFilterItem = new QueryFilterItem() { FieldName = "AssigneToNotificationTypeCode", FieldValue = notificationType, Operator = "Equals" };
            //    queryOperations.QueryFilterItems.Add(notificationTypeFilterItem);


            //}

            //notificationCount = listService.GetListCount(queryOperations, tenant);
            //queryOperations.PageSize = notificationCount;
            //allNotifications = listService.GetList(queryOperations, tenant);
            //int openNotifications = allNotifications.Where(d => !d.IsClosedByAssignee).Count();

            count = new NotificationFiltersDataCount() { Id = Guid.NewGuid().ToString() , OpenCount = OpenCount , AllCount = allTypeNotifications, ActionCount = allActionNotifications, InfoCount = allInfoNotifications, ClosedCount = closedCount};
            countsList.Add(count);
            return countsList;
           
        }




        public int GetNotificationFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          //  SecurityUtility.CheckContactFeature("Customs.Notification", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            NotificationListQueryService queryService = new NotificationListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertNotification(NotificationPM entityPm)
        {
          //  SecurityUtility.CheckContactFeature("Customs.Notification", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            NotificationUpdateService service = new NotificationUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            foreach (NotificationReplyPM reply in entityPm.NotificationRplies)
            {
                reply.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            }

            service.Update(entityPm, true);

        }

        public void UpdateNotification(NotificationPM currententityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.Notification", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            NotificationUpdateService service = new NotificationUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            SetNotificationRepliesChangeSet(currententityPm);
            service.Update(currententityPm, true);

        }

        private void SetNotificationRepliesChangeSet(NotificationPM currententityPm)
        {
            List<NotificationReplyPM> notificationReplychangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.NotificationRplies).Cast<NotificationReplyPM>().ToList();
            foreach (NotificationReplyPM itemPM in notificationReplychangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            NotificationReplyPM currentItemPM = currententityPm.NotificationRplies.Where(d => d.NotificationId == itemPM.NotificationId && d.Line == itemPM.Line).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            NotificationReplyPM currentItemPM = currententityPm.NotificationRplies.Where(d => d.NotificationId == itemPM.NotificationId && d.Line == itemPM.Line).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;


                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            NotificationReplyPM currentItemPM = new NotificationReplyPM() { ChangeSetOp = ChangeSetOperation.Delete, NotificationId = itemPM.NotificationId, Line = itemPM.Line };

                            currententityPm.DeletedNotificationRplies.Add(currentItemPM);

                            break;
                        }
                    default:
                        {
                            NotificationReplyPM currentItemPM = currententityPm.NotificationRplies.Where(d => d.NotificationId == itemPM.NotificationId && d.Line == itemPM.Line).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        public void UpdateNotificationList(NotificationList list)
        {

        }

        [Invoke]
        public void SetNotificationsStatus(List<string> Ids, string status,int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            notificationQuery = new NotificationQueryService(customContext);

            NotificationUpdateService service = new NotificationUpdateService(customContext, new Dictionary<string, IContext>(), tenant);
            List<NotificationPM> notifications = new List<NotificationPM>();

            notifications = notificationQuery.GetNotificationsPMsByIds(Ids, tenant);

            foreach (NotificationPM notificationPM in notifications)
            {
        
            if (status == "Read")
            {
                notificationPM.IsSeenByAssignee = true;
            }

            else if (status == "Unread")
            {
                notificationPM.IsSeenByAssignee = false;
            }

            else if (status == "Close")
            {
                notificationPM.IsClosedByAssignee = true;
            }

            else if (status == "Open")
            {
                notificationPM.IsClosedByAssignee = false;
            }
            notificationPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

            service.Update(notificationPM, true);
            }
        
            //}


        }

        [Invoke]
        public void SetNotificationBadjCount(string userId, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            NotificationQueryService queryService = new NotificationQueryService(customContext);
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {

                List<NotificationPM> result = queryService.GetNotificationsWithBadj(userId, tenant);
            foreach (NotificationPM item in result)
            {
                item.BadjCount = false;
                    NotificationUpdateService service = new NotificationUpdateService(customContext, new Dictionary<string, IContext>(), item.Tenant);
                    item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                    service.Update(item, false);
                }

                customContext.SaveChanges();
                scope.Complete();
            }

          

        }
    }
}