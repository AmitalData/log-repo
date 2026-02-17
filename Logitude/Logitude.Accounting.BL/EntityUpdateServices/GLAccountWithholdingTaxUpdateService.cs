using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
  public partial  class GLAccountWithholdingTaxUpdateService
    {
        public const string RaiseEventWBLKConst = "RaiseEventWBLK";
        public const string RaiseEventWLDAConst = "RaiseEventWLDA";
        public const string RaiseEventAWNCConst = "RaiseEventAWNC";


        protected override void OnCreating(GLAccountWithholdingTaxPM entityPM, GLAccountPM entityParentPM)
        {
            entityPM.GLAccountId = entityParentPM.Id;
            entityPM.Id = IdCounter.GetNumber("GLAccountWithholdingTax", entityPM.Tenant);
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
            entityPM.FromDate = new DateTime(entityPM.FromDate.Year, entityPM.FromDate.Month, entityPM.FromDate.Day, 00, 00, 00);
            entityPM.ToDate = new DateTime(entityPM.ToDate.Year, entityPM.ToDate.Month, entityPM.ToDate.Day, 00, 00, 00);
            //    var currentContextTag = entityPM.CurrentContextTag ?? "";
            //    if (currentContextTag.ToString() == RaiseEventWBLKConst)
            //        {
            //            ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
            //        string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
            //        Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
            //        String notes = TranslateTextsClass.Translate("Accounting.O.WithholdingBlocked", entityPM.Tenant);
            //        EventTracer.CreateTraceEvent(new EventTracerArgs()
            //        {
            //            EntityId = entityPM.Id,
            //            Tenant = entityPM.Tenant,
            //            UserId = contact.Id,
            //            ObjectTableName = "GLAccountWithholdingTax",
            //            IsAddedManually = false,
            //            EventTypeCode = "WBLK",
            //            Notes = notes,
            //        });
            //    }
            //    if (currentContextTag.ToString() == RaiseEventWLDAConst)
            //    {
            //        ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
            //        string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
            //        Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
            //        String notes = TranslateTextsClass.Translate("Accounting.O.WithholdingLineDisabled", entityPM.Tenant);
            //        EventTracer.CreateTraceEvent(new EventTracerArgs()
            //        {
            //            EntityId = entityPM.Id,
            //            Tenant = entityPM.Tenant,
            //            UserId = contact.Id,
            //            ObjectTableName = "GLAccountWithholdingTax",
            //            IsAddedManually = false,
            //            EventTypeCode = "WLDA",
            //            Notes = notes,
            //        });
            //    }
            base.OnUpdating(entityPM);
        }

        protected override void Trace(GLAccountWithholdingTaxPM entityPM, GLAccountWithholdingTax entityPOCO, string changesXml)
        {
           
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update) {
               

                PropertyInfo[] pmProperties = EntityPM.GetType().GetProperties();
                PropertyInfo[] pocoProperties = EntityPOCO.GetType().GetProperties();
                foreach (PropertyInfo property in pmProperties)
                {
                    string notes = GetTraceEventNotes(pmProperties, pocoProperties, property);
                    if (notes != null)
                    {
                        CreateTraceEvent(notes);
                    }



                }

            }

        }
        public string GetTraceEventNotes(PropertyInfo[] pmProperties, PropertyInfo[] pocoProperties, PropertyInfo property)
        {
            string notes = null;
            PropertyInfo pmProperty = pmProperties.Where(d => d.Name == property.Name).FirstOrDefault();//[0];
            PropertyInfo pocoProperty = pocoProperties.Where(d => d.Name == property.Name).FirstOrDefault();//[0];
            if (pmProperty != null && pocoProperty != null)
            {
                var pmPropertyValue = pmProperty.GetValue(EntityPM, null);
                var pocoPropertyValue = pocoProperty.GetValue(EntityPOCO, null);
                if (!pocoPropertyValue.Equals(pmPropertyValue))
                {
                   notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + pocoPropertyValue + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + pmPropertyValue;

                  
                }

            }
            return notes;
        }

        private void CreateTraceEvent(string notes)
        {
            Contact contact = GetLoggedContact();
            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                EntityId = EntityParentPM.Id,
                Tenant = EntityParentPM.Tenant,
                UserId = contact.Id,
                ObjectTableName = "GLAccount",
                IsAddedManually = false,
               EventTypeCode = "WTDU",
               Notes = notes,
            });

        }

        public Contact GetLoggedContact()
        {

            ContactRepository contactRep = new ContactRepository(EntityPM.Tenant);
            string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(EntityPM.Tenant);
         return contactRep.GetSingleContactByEmail(resolveLoggingUserId,EntityPM.Tenant);
        }

    }
}
