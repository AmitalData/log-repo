using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
  public partial  class GLAccountWithholdingTaxUpdateService
    {
  //      public bool RaiseEventWBLK { get; internal set; }
  //      public bool RaiseEventWLDA { get; internal set; }
        public const string RaiseEventWBLKConst = "RaiseEventWBLK";
        public const string RaiseEventWLDAConst = "RaiseEventWLDA";


        protected override void OnCreating(GLAccountWithholdingTaxPM entityPM, GLAccountPM entityParentPM)
        {
            entityPM.GLAccountId = entityParentPM.Id;
            entityPM.Id= IdCounter.GetNumber("GLAccountWithholdingTax", entityPM.Tenant);
            entityParentPM.TaxWithholdingLastLine += 1;
            entityPM.LineNumber = entityParentPM.TaxWithholdingLastLine;
         
            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            string email = "";
            if (AuthenticationUtil.IsAuthenticatedUserExists())
            {
                email = AuthenticationUtil.GetAuthenticatedUser();
            }
            else
            {
                email = "system@tenant" + entityPM.Tenant + ".com";
            }
            Contact contact = contactRep.GetSingleContactByEmail(email, entityPM.Tenant);
            entityPM.CreatedByUserId = contact.Id;
        }

        protected override void OnUpdating(GLAccountWithholdingTaxPM entityPM)
        {
            var currentContextTag = entityPM.CurrentContextTag ?? "";
            //if (RaiseEventWBLK)
            if (currentContextTag.ToString() == RaiseEventWBLKConst)
                {
                    ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                String notes = TranslateTextsClass.Translate("Accounting.O.WithholdingBlocked", entityPM.Tenant);
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    UserId = contact.Id,
                    ObjectTableName = "GLAccountWithholdingTax",
                    IsAddedManually = false,
                    EventTypeCode = "WBLK",
                    Notes = notes,
                });
       //         RaiseEventWBLK = false;
            }
            if (currentContextTag.ToString() == RaiseEventWLDAConst)
//                if (RaiseEventWLDA)
            {
                ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                String notes = TranslateTextsClass.Translate("Accounting.O.WithholdingLineDisabled", entityPM.Tenant);
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    UserId = contact.Id,
                    ObjectTableName = "GLAccountWithholdingTax",
                    IsAddedManually = false,
                    EventTypeCode = "WLDA",
                    Notes = notes,
                });
     //           RaiseEventWLDA = false;
            }
            base.OnUpdating(entityPM);
        }
    }
}
