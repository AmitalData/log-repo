using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.Data;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Unifreight.Data.AmitalModel;
using Unifreight.BL.EntityUpdateServices;
using Unifreight.BL.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.EntityPMs.UGenerated;
using System.Xml.Linq;
using Logitude.Customs.BL.EntityDataMappings;

namespace Logitude.Customs.BL.EntityUpdateServices
{
   public partial class ImporterDespositionUpdateService
    {
        public CustomsVendor Vendor { get; private set; }

        private Boolean toSendTask = false;
        private AmitalContext _AmitalContext;

        protected override void OnCreating(ImporterDespositionPM entityPM, EntityPM entityParentPM)
       {
          entityPM.Id = IdCounter.GetNumber("Customs.ImporterDesposition", entityPM.Tenant);
       }

       protected override void OnUpdating(ImporterDespositionPM entityPM)
       {
           UpdateNotification(entityPM);
       }

       private void UpdateNotification(ImporterDespositionPM dirtyEntityPM)
       {
           string loggingUserId = AuthenticationUtil.ResolveUserId(dirtyEntityPM.Tenant);

           string notificationDefinitionCode = "";
           var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
           if (eventContextTagModel != null)
           {
               switch (eventContextTagModel.CallProccessID)
               {
                   case EventContextTagModel.ProccessEnum.None:
                       break;
                   case EventContextTagModel.ProccessEnum.VE_3700_ImporterPeriodicDeclarationReplyResponseServiceNew:
                       notificationDefinitionCode = "3700N";
                       break;
                   case EventContextTagModel.ProccessEnum.VE_3700_ImporterPeriodicDeclarationReplyResponseServiceUpdate:
                       notificationDefinitionCode = "3700U";
                       break;
               }
           }

           if (!string.IsNullOrWhiteSpace(notificationDefinitionCode))
           {
               DoUpdateNotification(dirtyEntityPM, loggingUserId, notificationDefinitionCode);
           }
       }

       private void DoUpdateNotification(ImporterDespositionPM dirtyEntityPM, string loggingUserId, string notificationDefinitionCode)
       {
           ICustomContext dbContext = CustomContext.GetContext(dirtyEntityPM.Tenant);
           this.currentContext = dbContext;
           string desc = "";
           var notificationUpdateService = new NotificationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), dirtyEntityPM.Tenant);
           var notificationQueryService = new NotificationQueryService(dbContext);

           var newNotificationPM = new NotificationPM();
           newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
           newNotificationPM.Tenant = dirtyEntityPM.Tenant;

           switch (notificationDefinitionCode)
           {
               case "3700N":
                   newNotificationPM.NotificationDefinitionCode = "3700N";
                   newNotificationPM.AssigneToNotificationTypeCode = "I";
                   desc = "תצהיר תקופתי חדש לספק ";
                   break;
               case "3700U":
                   newNotificationPM.NotificationDefinitionCode = "3700U";
                   newNotificationPM.AssigneToNotificationTypeCode = "I";
                   desc = "עדכון תצהיר תקופתי לספק ";
                   break;
           }

           if (!string.IsNullOrWhiteSpace(dirtyEntityPM.VendorID))
           {
               desc = desc + "\n" + dirtyEntityPM.VendorID + "(" + dirtyEntityPM.VendorName + ")";
           }

           if (!string.IsNullOrWhiteSpace(dirtyEntityPM.ImporterlId))
           {
               desc = desc + "\n" + "יבואן " + dirtyEntityPM.ImporterCode + "(" + dirtyEntityPM.ImporterName + ")";
           }

           if (!string.IsNullOrWhiteSpace(dirtyEntityPM.ImporterDepositionStatusCode))
           {
               ImporterPeriodicDeclarStatusQueryService statusQueryService = new ImporterPeriodicDeclarStatusQueryService(dirtyEntityPM.Tenant);
               ImporterPeriodicDeclarStatusPM importerPeriodicDeclarationStatusPM = statusQueryService.GetSingle(dirtyEntityPM.ImporterDepositionStatusCode, false, true);
               desc = desc + "\n" + "סטטוס " + dirtyEntityPM.ImporterDepositionStatusCode + "(" + importerPeriodicDeclarationStatusPM.LocalName + ")";
           }

           newNotificationPM.EntityId = dirtyEntityPM.Id;
           newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.ImporterDesposition");
           newNotificationPM.Reference1Number = dirtyEntityPM.DepositionNumber;
           newNotificationPM.ResponseNotes = dirtyEntityPM.ErrorMessage + ", " + dirtyEntityPM.NotesToAgent;
           newNotificationPM.CreateDate = DateTime.Now;
           newNotificationPM.DueDate = DateTime.Now;
           newNotificationPM.Description = desc;
           if (dirtyEntityPM != null && !string.IsNullOrWhiteSpace(dirtyEntityPM.ImporterlId)) // newNotificationPM.CustomerId = dirtyEntityPM.ImporterlId; // moran 20.6.16 - Task 20789
           { // moran 13.9.16 - Bug 22397 - change handle
               var clientQueryService = new ClientQueryService(dirtyEntityPM.Tenant);
               var client = new ClientPM();

               client = clientQueryService.GetSingle(dirtyEntityPM.ImporterlId, true, false);
               if (client != null)
               {

                   ICommonDataContext CommonContext = CommonDataContext.GetContext(dirtyEntityPM.Tenant);
                   var cardRepository = new CardRepository(CommonContext);
                   Card card = cardRepository.GetSingleCardByVatNumber(client.Code, dirtyEntityPM.Tenant);
                   if (card != null && !String.IsNullOrWhiteSpace(card.Id))
                   {
                       newNotificationPM.CustomerId = card.Id;
                   }
               }
           }
           NotificationBase.CloseAllRelatedNotification(dbContext, newNotificationPM, newNotificationPM.NotificationDefinitionCode);
           notificationUpdateService.Update(newNotificationPM, true);
       }

        protected override void OnUpdating(ImporterDespositionPM entityPM, ImporterDesposition entityPOCO)
        {
            CustomsSettingQueryService settingsQuery = new CustomsSettingQueryService(entityPM.Tenant);
            var setting = CustomsSettingQueryService.GetSettingByTenant(entityPM.Tenant);
            if (setting.IsConnectedToUniFreight)
            {
                string defValue = GetDefault("ISRAEL", "CGG_SHARE_DESPO", "NON", "NON", entityPM.Tenant);
                if (defValue == "Y")
                {
                    this.Vendor = entityPOCO.Vendor;
                    this.toSendTask = true;
                }
            }
        }


        private string GetDefault(string DISTRID, string DEFID, string BRANCHID, string CARDID, int tenant)
        {
            AmitalContext amitalContext = AmitalContext.GetContext(tenant);
            var myGDFDATAQueryService = new GDFDATAQueryService(amitalContext);

            if (DISTRID == null || DEFID == null || BRANCHID == null || CARDID == null)
            {
                return ("");
            }

            GDFDATAPM myGDFDATAPM = myGDFDATAQueryService.GetSingle(DISTRID, DEFID, BRANCHID, CARDID, false, true);
            if (myGDFDATAPM == null)
            {
                return ("");
            }
            return (myGDFDATAPM.DEFDATA);
        }


        protected override void AfterUpdating(ImporterDespositionPM entityPM, EntityPM entityParentPM)
        {
            if (this.toSendTask == true)
            {
                OpenUnifreighTask(entityPM, "LD2C", "", false, "");
            }
        }


        private void OpenUnifreighTask(ImporterDespositionPM dirtyImporterDespositionPM, string taskType, string status, bool raiseStatus, string xmlStatus)
        {
            var sw = Stopwatch.StartNew();
            TransactionScope scope = null;
            var statusDateTime = DateTime.Now;

            if (!DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
            {
                scope = TransactionFactory.GetNewOracleReadCommittedTransaction();
            }
            try
            {
                using (_AmitalContext = AmitalContext.GetContext(dirtyImporterDespositionPM.Tenant))
                {
                    var myGGGQUpdateService = new GGGQUpdateService(_AmitalContext);
                    myGGGQUpdateService.DontAddTransaction = true;
                    var myYCULTASKUpdateService = new YCULTASKUpdateService(_AmitalContext);
                    myYCULTASKUpdateService.DontAddTransaction = true;
                    var requestData = "";
                    string importerCode = dirtyImporterDespositionPM.ImporterCode;
                    if(importerCode == null && dirtyImporterDespositionPM.ImporterlId != null) importerCode = TranslateClient(dirtyImporterDespositionPM.ImporterlId, dirtyImporterDespositionPM.Tenant);
                    if(this.Vendor == null && !string.IsNullOrWhiteSpace(dirtyImporterDespositionPM.VendorID))
                    {
                        if(this.currentContext == null) this.currentContext = CustomContext.GetContext(dirtyImporterDespositionPM.Tenant);
                        var myQueryService = new CustomsVendorQueryService(this.currentContext);
                        var custVendor = myQueryService.GetSingle(dirtyImporterDespositionPM.VendorID, false, true);
                        if(custVendor != null)
                        {
                            this.Vendor = new CustomsVendor();
                            var mapping = new CustomsVendorDataMapping();
                            mapping.PMToPOCO(custVendor, this.Vendor);
                        }
                    }
                    var XMLData = new XDocument(
                        new XElement("ImporterDepositionPM",
                            new XElement("ShipperCode", this.Vendor != null ? this.Vendor.VendorNumber : null),
                            new XElement("ShipperName", this.Vendor != null ? this.Vendor.VendorName : null),
                            new XElement("ShipperCountry", this.Vendor != null ? this.Vendor.CountryCode : null),
                            new XElement("ShipperVAT", this.Vendor != null ? this.Vendor.VATNumber : null),
                            new XElement("DepositionNumber", dirtyImporterDespositionPM.DepositionNumber),
                            new XElement("ValidityStartDate", dirtyImporterDespositionPM.StartDate != null ? dirtyImporterDespositionPM.StartDate.Value.ToString("o") : null),
                            new XElement("ValidityEndDate", dirtyImporterDespositionPM.EndDate != null ? dirtyImporterDespositionPM.EndDate.Value.ToString("o") : null),
                            new XElement("ImporterVat", importerCode)
                            )
                            );
        
   
                    requestData = XMLData.ToString(SaveOptions.None);
                    string unifreightUser = null;

                    if (String.IsNullOrWhiteSpace(unifreightUser))
                    {
                        unifreightUser = AuthenticationUtil.ResolveUnifreightUserId(dirtyImporterDespositionPM.Tenant);
                    }

                    var myYCULTASKPM = new YCULTASKPM()
                    {
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        STATUS = "W",
                        REQUESTDATA = requestData,
                        ENTNAME = "DEPOSITION",
                        PRIMARYNUM = dirtyImporterDespositionPM.Id,
                        PRIORITY = YCULTASKPM.calcPriority(taskType),
                        TYPE = taskType,
                        USRCODE = unifreightUser,
                        ARCHIVE = "F",
                    };

                    myYCULTASKUpdateService.Update(myYCULTASKPM, true);

                    var myGGGQPM = new GGGQPM()
                    {
                        ChangeSetOp = ChangeSetOperation.Insert,
                        ORIGINQUE = "LGT", 
                        STATUS = "1",
                        EXPTASKTIME = 5,
                        EXECDATE = (new DualQueryService(_AmitalContext as AmitalContext)).GetServerDateTime() ?? DateTime.Now.AddMinutes(-20), 
                        TRY = 9,
                        PRIORITY = 8,
                        ENTNAME = "DEPOSITION",
                        PRIMARYNUM = "0",
                        FORMID = "LGT_UPDATE_FCI",
                        DEBUG = "F",
                        DONEOPERATION = "D",
                        //GSTRING1 = myYCULTASKPM.TASKID,
                    };
                    myGGGQUpdateService.Update(myGGGQPM, true);

                    if (scope != null)
                    {
                        scope.Complete();
                    }
                }
            }
            finally
            {
                if (scope != null)
                {
                    scope.Dispose();
                }
            }

            LogMessagingUtil.Instance.AppendLine("OpenUnifreighTask:Took:" + sw.ElapsedMilliseconds);
        }


        private string TranslateClient(string amitalImporterId, int tenant)
        {
            if (String.IsNullOrWhiteSpace(amitalImporterId))
            {
                return null;
            }
            {
                ClientQueryService clientQueryService = new ClientQueryService(tenant);

                var clientId = clientQueryService.GetSingle(amitalImporterId, false, true);

                if (clientId == null)
                {
                    return null;
                }
                return clientId.Code;
            }
        }
    }
}
