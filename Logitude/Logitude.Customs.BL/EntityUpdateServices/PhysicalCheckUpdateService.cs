using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.NotificationBL;
using System.Diagnostics;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Customs.BL.Models;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging;
using System.Data;
using System.Data.Entity.Core;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class PhysicalCheckUpdateService : EntityUpdateService<PhysicalCheck, PhysicalCheckPM,EntityPM>
    {
        protected override void OnCreating(PhysicalCheckPM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Customs.PhysicalCheck", entityPM.Tenant);
        }
        protected override void OnUpdating(PhysicalCheckPM entityPM)
        {
            //var setting = CustomsSettingQueryService.GetSettingByTenant(entityPM.Tenant);
            //if (setting.IsConnectedToUniFreight)
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(entityPM.Tenant);
            DeclarationPM declarationPM = declarationQueryService.GetSingle(entityPM.DeclarationId, false, false);
            if (declarationPM != null && (declarationPM.IsConnectedToUnifreight || declarationPM.IsAmendment==true))
            {
                UpdateUnifreight(entityPM);
            }
            UpdateNotification(entityPM);
        }
        protected override void Trace(PhysicalCheckPM entityPM, PhysicalCheck entityPOCO, string changesXml)
        {
            
        }

        private void UpdateNotification(PhysicalCheckPM dirtyEntityPM) // moran 2.9.14 - Task 7092
        {

            DeclarationPM connectedDeclarationPM = null;

            string errMessage = null;
            string loggingUserId = AuthenticationUtil.ResolveUserId(dirtyEntityPM.Tenant);

            string notificationDefinitionCode = "";
            var dbOccPhysicalCheckPM = GetDBEntity(dirtyEntityPM);
            var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;

            //<--- Yuval Chalup 10.10.2016 TASK-23098 - Check if this is a Multi notification EventContextTagModel
            var eventContextTagModelList = dirtyEntityPM.CurrentContextTag as List<EventContextTagModel>;
            if (eventContextTagModelList != null)
            {
                eventContextTagModel = eventContextTagModelList.FirstOrDefault();
            }
            //Yuval Chalup 10.10.2016 TASK-23098 --->

            if (eventContextTagModel != null)
            {
                switch (eventContextTagModel.CallProccessID)
                {
                    case EventContextTagModel.ProccessEnum.None:
                        break;
                    case EventContextTagModel.ProccessEnum.CH_NG_196_MSG7_CargoExitFromCheckSiteResponseServiceUpdate:
                        notificationDefinitionCode = "196E";
                        break;
                    case EventContextTagModel.ProccessEnum.CH_NG_190_MSG1_NoticeToClientResponseServiceInsert:
                        notificationDefinitionCode = "190N";
                        break;
                    case EventContextTagModel.ProccessEnum.CH_NG_190_MSG1_NoticeToClientResponseServiceUpdate:
                        notificationDefinitionCode = "190U";
                        break;
                    case EventContextTagModel.ProccessEnum.CH_NG_190_MSG1_NoticeToClientResponseServiceDelete:
                        notificationDefinitionCode = "190C";
                        break;
                    default:
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(notificationDefinitionCode))
            {
                if (String.IsNullOrWhiteSpace(dirtyEntityPM.DeclarationId))
                {
                    errMessage = "Piscal check is not connected to Declaration ";
                    Debug.WriteLine(errMessage);
                    return;
                }
                else
                {
                    connectedDeclarationPM = GetConnectedDeclarationPM(dirtyEntityPM);
                    if (connectedDeclarationPM == null)
                    {
                        errMessage = "Can not found connected declaration" + dirtyEntityPM.DeclarationId;
                        Debug.WriteLine(errMessage);
                        return;
                    }                   
                }
                DoUpdateNotification(dirtyEntityPM, connectedDeclarationPM, loggingUserId, notificationDefinitionCode);
            }
        }

        // moran 11.8.14 - Task 7092 -->
        private void DoUpdateNotification(PhysicalCheckPM dirtyEntityPM, DeclarationPM connectedDeclarationPM, string loggingUserId, string notificationDefinitionCode)
        {
            ICustomContext dbContext = CustomContext.GetContext(dirtyEntityPM.Tenant);
            this.currentContext = dbContext;
            string desc = "";
            var notificationUpdateService = new NotificationUpdateService(this.currentContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), dirtyEntityPM.Tenant);
            var notificationQueryService = new NotificationQueryService(this.currentContext);

            var newNotificationPM = new NotificationPM();
            newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
            newNotificationPM.Tenant = dirtyEntityPM.Tenant;
            newNotificationPM.CreatedByRequestID = dirtyEntityPM.CustomsRequestsSheetId;

            LogMessagingUtil.Instance.AppendLine("New Physical Check Notification");
            newNotificationPM.NotificationDefinitionCode = notificationDefinitionCode;
            string convertedDate = null; // moran 2.4.15 - Task 11561 - add Initiator -->
            if (dirtyEntityPM.LimitDate != null)
            {
                DateTime limitDate = (DateTime)dirtyEntityPM.LimitDate;
                convertedDate = limitDate.ToString("O").Substring(0, 19);
            } // moran 2.4.15 - Task 11561 - add Initiator <--

            //<--- Yuval Chalup 19.11.2015 TASK-17450
            string DeclarationConvertionText = null;
            //If this is a Converted Declaration
            if (connectedDeclarationPM.IsConvertedDeclaration)
            {
                DeclarationConvertionText = connectedDeclarationPM.UserNotes;
            }
            //Yuval Chalup 19.11.2015 TASK-17450 --->
            switch (notificationDefinitionCode)
            {
                case "190N":
                    //desc = "בדיקה פיזית לתיק " + connectedDeclarationPM.CustomFileNo + "\n" + "מספר בדיקה - " + dirtyEntityPM.CheckId + "\n" + "תאריך הבדיקה -" + convertedDate;
                    //<--- Yuval Chalup 19.11.2015 TASK-17450
                    if (string.IsNullOrWhiteSpace(DeclarationConvertionText))
                    {
                        desc = "בדיקה פיזית לתיק " + connectedDeclarationPM.CustomFileNo + "\n" + "מספר בדיקה - " + dirtyEntityPM.CheckId + "\n" + "תאריך הבדיקה -" + convertedDate;
                    }
                    else
                    {
                        desc = "בדיקה פיזית ל " + DeclarationConvertionText + "\n" + "מספר בדיקה - " + dirtyEntityPM.CheckId + "\n" + "תאריך הבדיקה -" + convertedDate;
                    }
                    //Yuval Chalup 19.11.2015 TASK-17450 --->
                    if (!string.IsNullOrWhiteSpace(dirtyEntityPM.CheckSiteCode))
                    {
                        SiteLookupQueryService siteLookupQueryService = new SiteLookupQueryService(dirtyEntityPM.Tenant);
                        SiteLookupPM siteLookup = siteLookupQueryService.GetSingle(dirtyEntityPM.CheckSiteCode, false, true);
                        string checkSite = "\n" + "אתר בדיקה -" + dirtyEntityPM.CheckSiteCode + "(" + siteLookup.LocalName + ")";
                        desc = string.Concat(desc,checkSite);
                    }
                    break;
                case "190U":
                    //desc = "עודכנו נתוני בדיקה פיזית לתיק " + connectedDeclarationPM.CustomFileNo + "\n" + "מספר בדיקה - " + dirtyEntityPM.CheckId + "\n" + "תאריך הבדיקה -" + convertedDate;
                    //<--- Yuval Chalup 19.11.2015 TASK-17450
                    if (string.IsNullOrWhiteSpace(DeclarationConvertionText))
                    {
                        desc = "עודכנו נתוני בדיקה פיזית לתיק " + connectedDeclarationPM.CustomFileNo + "\n" + "מספר בדיקה - " + dirtyEntityPM.CheckId + "\n" + "תאריך הבדיקה -" + convertedDate;
                    }
                    else
                    {
                        desc = "עודכנו נתוני בדיקה פיזית ל " + DeclarationConvertionText + "\n" + "מספר בדיקה - " + dirtyEntityPM.CheckId + "\n" + "תאריך הבדיקה -" + convertedDate;
                    }
                    //Yuval Chalup 19.11.2015 TASK-17450 --->
                    if (!string.IsNullOrWhiteSpace(dirtyEntityPM.CheckSiteCode))
                    {
                        SiteLookupQueryService siteLookupQueryService = new SiteLookupQueryService(dirtyEntityPM.Tenant);
                        SiteLookupPM siteLookup = siteLookupQueryService.GetSingle(dirtyEntityPM.CheckSiteCode, false, true);
                        string checkSite = "\n" + "אתר בדיקה -" + dirtyEntityPM.CheckSiteCode + "(" + siteLookup.LocalName + ")";
                        desc = string.Concat(desc,checkSite);
                    }
                    break;
                case "196E":
                    //desc = "הסתיימה בדיקה פיזית לתיק " + connectedDeclarationPM.CustomFileNo + "\n" + "מספר בדיקה - " + dirtyEntityPM.CheckId;
                    //<--- Yuval Chalup 19.11.2015 TASK-17450
                    if (string.IsNullOrWhiteSpace(DeclarationConvertionText))
                    {
                        desc = "הסתיימה בדיקה פיזית לתיק " + connectedDeclarationPM.CustomFileNo + "\n" + "מספר בדיקה - " + dirtyEntityPM.CheckId;
                    }
                    else
                    {
                        desc = "הסתיימה בדיקה פיזית ל " + DeclarationConvertionText + "\n" + "מספר בדיקה - " + dirtyEntityPM.CheckId + "\n" + "תאריך הבדיקה -" + convertedDate;
                    }
                    //Yuval Chalup 19.11.2015 TASK-17450 --->
                    break;
                case "190C":
                    //desc = "בוטלה בדיקה פיזית לתיק " + connectedDeclarationPM.CustomFileNo + "\n" + "מספר בדיקה - " + dirtyEntityPM.CheckId;
                    //<--- Yuval Chalup 19.11.2015 TASK-17450
                    if (string.IsNullOrWhiteSpace(DeclarationConvertionText))
                    {
                        desc = "בוטלה בדיקה פיזית לתיק " + connectedDeclarationPM.CustomFileNo + "\n" + "מספר בדיקה - " + dirtyEntityPM.CheckId;
                    }
                    else
                    {
                        desc = "בוטלה בדיקה פיזית ל " + DeclarationConvertionText + "\n" + "מספר בדיקה - " + dirtyEntityPM.CheckId + "\n" + "תאריך הבדיקה -" + convertedDate;
                    }
                    //Yuval Chalup 19.11.2015 TASK-17450 --->
                break;
            }
            
            newNotificationPM.EntityId = dirtyEntityPM.Id;
            newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.PhysicalCheck");
            newNotificationPM.CreateDate = DateTime.Now;
            newNotificationPM.Description = desc;
            newNotificationPM.Reference2Number = dirtyEntityPM.CheckId;
           
            string oldassigneId = null;
            string customerId = null;
            string referentUserId = null;
            if (connectedDeclarationPM != null)
            {
                newNotificationPM.EntityId = connectedDeclarationPM.Id;
                newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                newNotificationPM.Reference1Number = connectedDeclarationPM.CustomFileNo;
                newNotificationPM.DepartmentId = connectedDeclarationPM.DepartmentId;
                newNotificationPM.DeclarationOfficeCode = connectedDeclarationPM.DeclarationOfficeCode;
                customerId = connectedDeclarationPM.CustomerId;
                referentUserId = connectedDeclarationPM.ReferentUserId;
            }
            if (connectedDeclarationPM != null && !string.IsNullOrWhiteSpace(connectedDeclarationPM.CustomerId)) newNotificationPM.CustomerId = connectedDeclarationPM.CustomerId; // moran 20.6.16 - Task 20789

            var oldNotificationPM = new NotificationPM();
            oldNotificationPM = GetNotification(notificationQueryService, newNotificationPM, "190N");       

            if (oldNotificationPM != null)
            {
                oldassigneId = oldNotificationPM.AssigneToId;
            }
            if (notificationDefinitionCode == "190N" || notificationDefinitionCode == "190U")
            {
                newNotificationPM.DueDate = dirtyEntityPM.LimitDate;
                newNotificationPM.AssigneToNotificationTypeCode = "A";
            }
            else
            {
                newNotificationPM.DueDate = DateTime.Now;
                newNotificationPM.AssigneToNotificationTypeCode = "I";
            }
            newNotificationPM.IsHandledByCustomOffice = true;
            newNotificationPM.AssigneToId = NotificationBase.CalcAssigneToId(newNotificationPM.Tenant, customerId, referentUserId, notificationDefinitionCode, oldassigneId);
         
            NotificationBase.CloseAllRelatedNotification(this.currentContext, newNotificationPM, "190");
            notificationUpdateService.Update(newNotificationPM, true);
        }

        private NotificationPM GetNotification(NotificationQueryService notificationQueryService, NotificationPM newNotificationPM, string notificationDefinitionCode)
        {
            var oldNotificationPM = notificationQueryService.GetNotification(newNotificationPM.Tenant, newNotificationPM.ObjectTableId, newNotificationPM.EntityId, notificationDefinitionCode);
            return oldNotificationPM;
        }
        // moran 11.8.14 - Task 7092 <--

        protected override void CheckConcurrency(PhysicalCheckPM entityPM, PhysicalCheck entityPOCO)
        {
            //if (!entityPM.ConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID) && !entityPM.NewConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID))
            if (entityPM.ConcurrencyGUID != entityPOCO.ConcurrencyGUID && entityPM.NewConcurrencyGUID != entityPOCO.ConcurrencyGUID)
            {
                string msg = TranslateTextsClass.Translate("General.M.CantUpdateRecord", entityPM.Tenant);
                throw new OptimisticConcurrencyException(msg);
            }

        }
    }
}
