using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Accounting.BL.EntityQueryServices;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class BankCodeUpdateService : EntityUpdateService<BankCode, BankCodePM, EntityPM>
    {
        protected override void OnCreating(BankCodePM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.Id == null || entityPM.Id == "") entityPM.Id = IdCounter.GetNumber("BankCode", entityPM.Tenant);
            entityPM.SearchFields = entityPM.Code + "," + entityPM.EnglishName + "," + entityPM.LocalName;
        }

        protected override void OnUpdating(BankCodePM entityPM)
        {
            entityPM.SearchFields = entityPM.Code + "," + entityPM.EnglishName + "," + entityPM.LocalName;
            if(entityPM.ChangeSetOp == ChangeSetOperation.Insert && entityPM.Tenant == 0)
            {
                
                ImageDetailRepository imageDetailRepository = new ImageDetailRepository(entityPM.Tenant);
                string extension = imageDetailRepository.GetImageExtensionbyId(entityPM.Tenant, entityPM.LogoId);
                Uploader uploaderService = new Uploader();
                byte[] filedata = uploaderService.DownloadFile(filename, documentExtension, fileLocation, tenant);

                if (filedata != null)
                {
                    if (filename.Contains("logo") || type == "Base64")
                    {
                        result = "data:image/" + documentExtension + ";base64," + Convert.ToBase64String(filedata);
                    }
                    else result = UTF8Encoding.UTF8.GetString(filedata, 0, filedata.Length);
                }

            }
        }

        protected override void Trace(BankCodePM entityPM, BankCode entityPOCO, string changesXml)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                //create trace event with created type.
                ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                // ContactPM loggedContact = LoggedContact(entityPM.Tenant);
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    UserId = contact.Id,
                    ObjectTableName = "BankCode",
                    IsAddedManually = false,
                    EventTypeCode = "CREV",


                });

            }
            else if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                String notes = "";
                if (entityPM.EnglishName != entityPOCO.EnglishName && (!String.IsNullOrEmpty(entityPM.EnglishName) || !String.IsNullOrEmpty(entityPOCO.EnglishName)))
                {
                    notes +=  " -English Name Changed." + Environment.NewLine + TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + entityPOCO.EnglishName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + entityPM.EnglishName;
                }
                if (entityPM.LocalName != entityPOCO.LocalName && (!String.IsNullOrEmpty(entityPM.LocalName) || !String.IsNullOrEmpty(entityPOCO.LocalName)))
                {
                    notes += " -Local Name Changed. " + Environment.NewLine + TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + entityPOCO.LocalName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + entityPM.LocalName;
                }
                if (entityPM.Code != entityPOCO.Code && (!String.IsNullOrEmpty(entityPM.Code) || !String.IsNullOrEmpty(entityPOCO.Code)))
                {
                    notes += " -Code Changed. " + Environment.NewLine + TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + entityPOCO.Code + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + entityPM.Code;
                }
                if (entityPM.Inactive != entityPOCO.Inactive)
                {
                    if (entityPM.Inactive == true)
                    {
                        notes += " -Inactivated";
                    }
                    else
                    {
                        notes += " -Deactivated";
                    }
                }

                ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    UserId = contact.Id,
                    ObjectTableName = "BankCode",
                    IsAddedManually = false,
                    EventTypeCode = "UPEV",
                    Notes = notes,

                });
               
            }
            base.Trace(entityPM, entityPOCO, changesXml);
        }

    }
}
