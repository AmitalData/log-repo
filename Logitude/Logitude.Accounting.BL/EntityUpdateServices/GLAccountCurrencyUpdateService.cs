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
using Logitude.Accounting.Data.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.BL.EntityQueryServices;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class GLAccountCurrencyUpdateService  
    {
    
        protected override void OnUpdating(GLAccountCurrencyPM entityPM)
        {
            if (string.IsNullOrEmpty(entityPM.Id))
            {
                GLAccountCurrencyRepository gLAccountCurrencyRepository = new GLAccountCurrencyRepository(entityPM.Tenant);
                GLAccountCurrency accountCurrency = gLAccountCurrencyRepository.GetEntityByCurrencyAndGLAccountId(entityPM.MainGLAccountId, entityPM.CurrencyId , entityPM.Tenant);
                entityPM.Id = accountCurrency.Id;

            }

            CreateGLAccountCurrencyEvents(entityPM);


        }



        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }
        public static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }

        public void ValidateSplitGLAccount(GLAccountCurrencyPM entityPM)
        {
            GLAccountQueryService query = new GLAccountQueryService(entityPM.Tenant);
            GLAccountPM parentPM = query.GetSingle(entityPM.MainGLAccountId, false, false);
            CheckIfTheSplitGLAccountIsACustomerGLAccount(entityPM.GLAccountId, parentPM);
        }
        private void CheckIfTheSplitGLAccountIsACustomerGLAccount(string glaccountId, GLAccountPM parentPM)
        {
            if (parentPM.IsMultiCurrency == true && glaccountId == parentPM.CustomerGLAccountId)
                throw new ApplicationException(new ValidationResult(TextCodesTranslator.TranslateText("GLAccounts.O.CustomerGLaccountDefinedSplit", parentPM.Tenant)).ErrorMessage);
        }
        private void CreateGLAccountCurrencyEvents(GLAccountCurrencyPM entityPM)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert  || entityPM.ChangeSetOp == ChangeSetOperation.Delete)
            {
                ContactPM contact = GetLoggedContact(entityPM.Tenant);
                bool showLocals = !contact.DontShowLocal;
                GLAccount MainGLAccount = GetGLAccount(entityPM.MainGLAccountId, entityPM.Tenant);
                GLAccount CurrencyGLAccount = GetGLAccount(entityPM.GLAccountId, entityPM.Tenant);
                if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
                {
                    String notes = TranslateTextsClass.Translate("GLAccount.O.Glaccount.Currency", entityPM.Tenant, showLocals) + " (" + CurrencyGLAccount.DisplayNumber.ToString() + ") " + TranslateTextsClass.Translate("GLAccount.O.WasConnected", entityPM.Tenant, showLocals) + " (" + MainGLAccount.DisplayNumber.ToString() + ")";
                  
                    CreateGLAccountEvent("GLCC", notes, entityPM.GLAccountId, entityPM.Tenant, contact.Id);
                    CreateGLAccountEvent("GLCC", notes, entityPM.MainGLAccountId, entityPM.Tenant, contact.Id);

                }

                if (entityPM.ChangeSetOp == ChangeSetOperation.Delete)
                {
                    String notes = TranslateTextsClass.Translate("GLAccount.O.Glaccount.Currency", entityPM.Tenant, showLocals) + " (" + CurrencyGLAccount.DisplayNumber.ToString() + ") " + TranslateTextsClass.Translate("GLAccount.O.WasDisconnected", entityPM.Tenant, showLocals) + " (" + MainGLAccount.DisplayNumber.ToString() + ")";
                    CreateGLAccountEvent("GLDD", notes, entityPM.GLAccountId, entityPM.Tenant, contact.Id);
                    CreateGLAccountEvent("GLDD", notes, entityPM.MainGLAccountId, entityPM.Tenant, contact.Id);
                }
            }

         }

        private void CreateGLAccountEvent(string eventCode, string Note, string GLAccountId, int Tenant,string ContactId)
        {
            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                EntityId = GLAccountId,
                Tenant = Tenant,
                UserId = ContactId,
                ObjectTableName = "GLAccount",
                IsAddedManually = false,
                EventTypeCode = eventCode,
                Notes = Note,
            });
        }

        private GLAccount GetGLAccount(string GLAccountId,int Tenant)
        {
            GLAccountRepository gLAccountRepository = new GLAccountRepository(Tenant);
            GLAccount gLAccount = gLAccountRepository.GetSingle(GLAccountId, Tenant);

            return gLAccount;
        }

    }
}
