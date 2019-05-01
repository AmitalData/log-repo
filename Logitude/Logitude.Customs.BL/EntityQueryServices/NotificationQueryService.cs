using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class NotificationQueryService : EntityQueryService<Notification, NotificationKeys, NotificationPM, object, NotificationKeys>
    {

        public override void GetComposition(Simplog.Server.Infrastructure.EntityKeyFields entityKeys, NotificationPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            NotificationKeys notificationKeys = entityKeys as NotificationKeys;

            NotificationReplyQueryService notificationReplyQueryService = new NotificationReplyQueryService(context);
            entityPM.NotificationRplies = notificationReplyQueryService.GetMulti(notificationKeys, false);




            if (entityPM.NotificationRplies.Count > 0)
            {
                entityPM.NotificationReplyLastLineNumber = entityPM.NotificationRplies.Max(m => m.Line);
            }

            

            base.GetComposition(entityKeys, entityPM);
        }

        public List<NotificationList> GetNotificationwithManaement(int tenant)
        {

            List<NotificationList> notifications = (from a in context.Notifications

                                                    select new NotificationList()
                                                         {
                                                             Id = a.Id,
                                                             AssigneToId = a.AssigneToId,
                                                             AssigneToNotificationTypeCode = a.AssigneToNotificationTypeCode,
                                                             CreateDate = a.CreateDate,
                                                             CreatedByRequestID = a.CreatedByRequestID,
                                                             DeclarationOfficeCode = a.DeclarationOfficeCode,
                                                            
                                                             DueDate = a.DueDate,
                                                             EntityId = a.EntityId,
                                                             IsClosedBCustomOffice = a.IsClosedBCustomOffice,
                                                             IsClosedByAssignee = a.IsClosedByAssignee,
                                                             IsSeenByAssignee = a.IsSeenByAssignee,
                                                             IsHandledByCustomOffice = a.IsHandledByCustomOffice,
                                                             NotificationDefinitionCode = a.NotificationDefinitionCode,
                                                             NotificationDefinitionName = a.NotificationDefinition != null? a.NotificationDefinition.LocalName : null,
                                                             ObjectTableId = a.ObjectTableId,
                                                              Description = a.Description,
                                                             Tenant = a.Tenant,
                                                             AssigneToName = a.AssigneTo != null? a.AssigneTo.Contact.EnglishName : null,
                                                             ObjectTableName = a.ObjectTable != null? a.ObjectTable.Name : null,
                                                              
                                                             Reference1Number = a.Reference1Number,
                                                             Reference2Number = a.Reference2Number,
                                                             DepartmentId = a.DepartmentId,
                                                             ResponseNotes = a.ResponseNotes,
                                                             DepartmentName = a.Department != null? a.Department.LocalName : null,

                                                         }).ToList();


            return notifications;

        }

        public List<NotificationPM> GetOpenNotificationList(int tenant, string ObjectTableId, string EntityId, string NotificationDefinitionCode, string Reference2Number)
        {
            if (!string.IsNullOrWhiteSpace(NotificationDefinitionCode))
            {
                var allOpen = (this.repository as NotificationRepository).GetAll(tenant)
                    .Where(rec => rec.ObjectTableId == ObjectTableId && rec.EntityId == EntityId && rec.IsClosedByAssignee == false && rec.NotificationDefinitionCode.StartsWith(NotificationDefinitionCode));
                    //.ToList();
                if (!string.IsNullOrWhiteSpace(Reference2Number)) // Mirit 17/05/15 Task 13336
                {
                    if (allOpen.ToList().Count > 0)
                    {
                        allOpen = allOpen.Where(a => ((a.Reference2Number ?? "_IsNull") == (Reference2Number ?? "_IsNull")));
                    }
                    else
                    {
                        allOpen = (this.repository as NotificationRepository).GetAll(tenant)
                        .Where(rec => rec.IsClosedByAssignee == false && rec.NotificationDefinitionCode.StartsWith(NotificationDefinitionCode) && rec.Reference2Number == Reference2Number);
                    }
                }
                allOpen.ToList();
                var allOpenPM = allOpen.ToList().Select(rec => this.GetEntityPM(rec)).ToList();
                return allOpenPM;
            }
            else
            {
                var allOpen = (this.repository as NotificationRepository).GetAll(tenant)
                    .Where(rec => rec.ObjectTableId == ObjectTableId && rec.EntityId == EntityId && rec.IsClosedByAssignee == false)
                    .ToList();
                var allOpenPM = allOpen.ToList().Select(rec => this.GetEntityPM(rec)).ToList();
                return allOpenPM;
            }
        }

        public List<NotificationPM> GetNotificationByDefinitionCode( string ObjectTableId, string EntityId,int tenant)
        {

            var allNotifications = (this.repository as NotificationRepository).GetAll(tenant)
                .Where(rec => rec.ObjectTableId == ObjectTableId && rec.EntityId == EntityId && (rec.NotificationDefinitionCode == "5101N" || rec.NotificationDefinitionCode == "5101E" || rec.NotificationDefinitionCode == "5101R" || rec.NotificationDefinitionCode == "5101A") && !string.IsNullOrEmpty(rec.Reference2Number))
                .ToList();
                var result = allNotifications.ToList().Select(rec => this.GetSingle(rec.Id, true, false)).ToList();
                return result;
           
        }

        public NotificationPM GetNotification(int tenant, string ObjectTableId, string EntityId, string NotificationDefinitionCode)
        {
            var allNotification = (this.repository as NotificationRepository).GetAll(tenant)
                .Where(rec => rec.ObjectTableId == ObjectTableId && rec.EntityId == EntityId && rec.NotificationDefinitionCode == NotificationDefinitionCode);
            var notificationListPM = allNotification.ToList().Select(rec => this.GetEntityPM(rec)).ToList();
            return notificationListPM.FirstOrDefault();
        }

        public List<NotificationPM> GetAllNotificationPMs(int tenant)
        {

            var allNotifications = (this.repository as NotificationRepository).GetAll(tenant)
                .ToList();
            var result = allNotifications.ToList().Select(rec => this.GetEntityPM(rec)).ToList();
            return result;

        }

        public IQueryable<NotificationPM> GetAllNotifications(int tenant)
        {
             
            var allNotifications = (this.repository as NotificationRepository).GetAll(tenant).OrderBy(d => d.IsSeenByAssignee).ThenBy(d => d.DueDate);

            IQueryable<NotificationPM> notifications = (from a in allNotifications

                                                        select new NotificationPM()
                                                        {
                                                            Id = a.Id,
                                                            AssigneToId = a.AssigneToId,
                                                            AssigneToNotificationTypeCode = a.AssigneToNotificationTypeCode,
                                                            CreateDate = a.CreateDate,
                                                            CreatedByRequestID = a.CreatedByRequestID,
                                                            DeclarationOfficeCode = a.DeclarationOfficeCode,

                                                            DueDate = a.DueDate,
                                                            EntityId = a.EntityId,
                                                            IsClosedBCustomOffice = a.IsClosedBCustomOffice,
                                                            IsClosedByAssignee = a.IsClosedByAssignee,
                                                            IsSeenByAssignee = a.IsSeenByAssignee,
                                                            IsHandledByCustomOffice = a.IsHandledByCustomOffice,
                                                            NotificationDefinitionCode = a.NotificationDefinitionCode,
                                                            NotificationDefinitionName = a.NotificationDefinition != null ? a.NotificationDefinition.LocalName : null,
                                                            ObjectTableId = a.ObjectTableId,
                                                            Description = a.Description,
                                                            Tenant = a.Tenant,
                                                           // AssigneToName = a.AssigneTo != null ? a.AssigneTo.Contact.EnglishName : null,
                                                            ObjectTableName = a.ObjectTable != null ? a.ObjectTable.Name : null,
                                                            ClosedByAssigneeName = a.User != null ? (!string.IsNullOrEmpty(a.User.Contact.LocalName) ? a.User.Contact.LocalName : a.User.Contact.EnglishName) : null,
                                                            ClosedByAssignee = a.ClosedByAssignee,
                                                            Reference1Number = a.Reference1Number,
                                                            Reference2Number = a.Reference2Number,
                                                            DepartmentId = a.DepartmentId,
                                                            ResponseNotes = a.ResponseNotes,
                                                            DepartmentName = a.Department != null ? a.Department.LocalName : null,

                                                        });


            return notifications;



        }

        public List<NotificationPM> GetNotificationsPMsByIds(List<string> ids, int tenant)
        {

            var allNotifications = (this.repository as NotificationRepository).GetNotificationsByIds(ids,tenant)
                .ToList();
            //var result = allNotifications.ToList().Select(rec => this.GetEntityPM(rec)).ToList();
            //return result;

            List<NotificationPM> notifications = (from a in allNotifications

                                                    select new NotificationPM()
                                                    {
                                                        Id = a.Id,
                                                        AssigneToId = a.AssigneToId,
                                                        AssigneToNotificationTypeCode = a.AssigneToNotificationTypeCode,
                                                        CreateDate = a.CreateDate,
                                                        CreatedByRequestID = a.CreatedByRequestID,
                                                        DeclarationOfficeCode = a.DeclarationOfficeCode,

                                                        DueDate = a.DueDate,
                                                        EntityId = a.EntityId,
                                                        IsClosedBCustomOffice = a.IsClosedBCustomOffice,
                                                        IsClosedByAssignee = a.IsClosedByAssignee,
                                                        IsSeenByAssignee = a.IsSeenByAssignee,
                                                        IsHandledByCustomOffice = a.IsHandledByCustomOffice,
                                                        NotificationDefinitionCode = a.NotificationDefinitionCode,
                                                        NotificationDefinitionName = a.NotificationDefinition != null ? a.NotificationDefinition.LocalName : null,
                                                        ObjectTableId = a.ObjectTableId,
                                                        Description = a.Description,
                                                        Tenant = a.Tenant,
                                                        AssigneToName = a.AssigneTo != null ? a.AssigneTo.Contact.EnglishName : null,
                                                        ObjectTableName = a.ObjectTable != null ? a.ObjectTable.Name : null,
                                                        ClosedByAssigneeName = a.User != null ? (!string.IsNullOrEmpty(a.User.Contact.LocalName) ? a.User.Contact.LocalName : a.User.Contact.EnglishName) : null,
                                                        ClosedByAssignee = a.ClosedByAssignee,
                                                        Reference1Number = a.Reference1Number,
                                                        Reference2Number = a.Reference2Number,
                                                        DepartmentId = a.DepartmentId,
                                                        ResponseNotes = a.ResponseNotes,
                                                        DepartmentName = a.Department != null ? a.Department.LocalName : null,

                                                    }).ToList();


            return notifications;


        }
        public List<NotificationPM> GetNotificationsPMsBySearchField(string searchField, int tenant)
        {

            var notifications = (this.repository as NotificationRepository).GetAll(tenant)
                .Where(rec => rec.SearchFields.Contains(searchField)).ToList();
            //var result = allNotifications.ToList().Select(rec => this.GetEntityPM(rec)).ToList();
            //return result;

            List<NotificationPM> result = (from a in notifications

                                                  select new NotificationPM()
                                                  {
                                                      Id = a.Id,
                                                      AssigneToId = a.AssigneToId,
                                                      AssigneToNotificationTypeCode = a.AssigneToNotificationTypeCode,
                                                      CreateDate = a.CreateDate,
                                                      CreatedByRequestID = a.CreatedByRequestID,
                                                      DeclarationOfficeCode = a.DeclarationOfficeCode,

                                                      DueDate = a.DueDate,
                                                      EntityId = a.EntityId,
                                                      IsClosedBCustomOffice = a.IsClosedBCustomOffice,
                                                      IsClosedByAssignee = a.IsClosedByAssignee,
                                                      IsSeenByAssignee = a.IsSeenByAssignee,
                                                      IsHandledByCustomOffice = a.IsHandledByCustomOffice,
                                                      NotificationDefinitionCode = a.NotificationDefinitionCode,
                                                      NotificationDefinitionName = a.NotificationDefinition != null ? a.NotificationDefinition.LocalName : null,
                                                      ObjectTableId = a.ObjectTableId,
                                                      Description = a.Description,
                                                      Tenant = a.Tenant,
                                                      AssigneToName = a.AssigneTo != null ? a.AssigneTo.Contact.EnglishName : null,
                                                      ObjectTableName = a.ObjectTable != null ? a.ObjectTable.Name : null,
                                                      ClosedByAssigneeName = a.User != null ? (!string.IsNullOrEmpty(a.User.Contact.LocalName) ? a.User.Contact.LocalName : a.User.Contact.EnglishName) : null,
                                                      ClosedByAssignee = a.ClosedByAssignee,
                                                      Reference1Number = a.Reference1Number,
                                                      Reference2Number = a.Reference2Number,
                                                      DepartmentId = a.DepartmentId,
                                                      ResponseNotes = a.ResponseNotes,
                                                      DepartmentName = a.Department != null ? a.Department.LocalName : null,

                                                  }).ToList();


            return result;


        }



        public List<NotificationPM> GetTopTenNotificationPMs(string userId,int tenant)
        {

            var allNotifications = (this.repository as NotificationRepository).GetTopTenNotifications(userId,tenant).ToList();
            List<NotificationPM> result = allNotifications.OrderByDescending(d => d.CreateDate).Take(10).ToList().Select(rec => this.GetEntityPM(rec)).ToList();
            return result;

        }

        public List<NotificationPM> GetNotificationsWithBadj(string userId, int tenant)
        {

            var allNotifications = (this.repository as NotificationRepository).GetNotificationsWithBadjCount(userId, tenant).ToList();
            List<NotificationPM> result = allNotifications.OrderByDescending(d => d.CreateDate).ToList().Select(rec => this.GetEntityPM(rec)).ToList();
            return result;

        }

        public int GetBadjCount(string userId, int tenant)
        {
            return repository.GetBadjCount(userId, tenant);
        }

        public int GetOpenNotificationCountForUser(string userId, int tenant)
        {
            return repository.GetOpenNotificationCountForUser(userId, tenant);
        }

    }
}
