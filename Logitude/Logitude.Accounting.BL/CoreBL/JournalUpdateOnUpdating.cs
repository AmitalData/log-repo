using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.EntityUpdateServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Accounting.BL.Validators;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.BL.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;

namespace Logitude.Accounting.BL
{
    public class JournalUpdateOnUpdating
    {
        public const string M_CreateStornoAndSaveFailed = "CreateStornoAndSave Failed ";
        //public const string M_CannotvoidApprovedJournalthatdidnotstramtoAccounting= "Can not void Approved Journal that did not stram to Accounting";
        private IAccountingContext _MainContext;

        private IJournalStornoService _JournalStornoService;
        


        public JournalUpdateOnUpdating(IAccountingContext mainContext)
        {
            this._MainContext = mainContext;
        }

        public JournalUpdateOnUpdating(IAccountingContext accountingContext, IJournalStornoService myJournalStornoService)
        {
            // TODO: Complete member initialization
            this._MainContext = accountingContext;
            this._JournalStornoService = myJournalStornoService;
        }

        public virtual string GetLogContactId(JournalPM entityPM)
        {
            return AuthenticationUtil.ResolveUserId(entityPM.Tenant);
            //string email = HttpContext.Current.User.Identity.Name;


            //var contactRepository = ContainerAccessor.Container.ResolveSafe<IContactRepository>() ?? new ContactRepository(entityPM.Tenant);
            //var loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            //if (loggedContact != null)
            //{
            //    return loggedContact.Id;
            //}
            //return null;


        }
        public void OnUpdating(JournalPM journalPM, Journal JournalPOCO, EntityPM changeTrackingEntityPM)
        {



            if (journalPM.ChangeSetOp == ChangeSetOperation.None)
            {
                throw new ApplicationException("Don't Update Nothing");
            }
            if (journalPM.ChangeSetOp == ChangeSetOperation.Delete)
            {
                throw new ApplicationException("I Don't think its good idea to delete Journal (ask yaron)");
            }
            if (JournalPOCO.StatusCode == ((int)JournalStatusTypePM.StatusCodeEnum.Voided).ToString())
            {

                throw new ApplicationException("Journal is voided (Change is not Allowed)");
            }

                /// 
                foreach (var jl in journalPM.JournalLines)
            {
                var JournalUpdateOnCreatingLine = CreateJournalLineOnUpdate();
                //jl.EnsureAllDecimalPrecisionIfChangeChangeUpdate();
                JournalUpdateOnCreatingLine.OnUpdate(jl, journalPM);
            }


            string loggedContactId = //AddActivityGetLogContactId(entityPM.Tenant, entityPM.Id, "N");
                //this.UpdateServiceProvider.
               GetLogContactId(journalPM);
            string ObjectTableId = GetObjectTableId(journalPM);

            var myActivityLogger = CreateActivityLogger();
            myActivityLogger.AddAcitivityLog(journalPM.Id, ObjectTableId, journalPM.Tenant, "U", loggedContactId);



            if (journalPM.CreatedByUserId == null)
            {
                journalPM.CreatedByUserId = loggedContactId;
            }
            if (journalPM.UpdatedByUserId == null)
            {
                journalPM.UpdatedByUserId = loggedContactId;
            }
            journalPM.UpdateDate = DateTime.Now;

            journalPM.AccountingDate = journalPM.AccountingDate.Date; //Eyal:Time No Meaning (create+update Have  Time have Meaning )

            // The FullAccountingSettings,IsSecurityLevelActivated is not checked here - INTENTIONALLY 
            // Also, the CheckFeature("Journal", "Journal.Feature.ManageSecurity") is not performed 
            if (!String.IsNullOrEmpty(loggedContactId) && 
                ((journalPM.SecurityLevel.HasValue && !JournalPOCO.SecurityLevel.HasValue) ||
             // (!journalPM.SecurityLevel.HasValue && JournalPOCO.SecurityLevel.HasValue) ||
                (journalPM.SecurityLevel.HasValue && JournalPOCO.SecurityLevel.HasValue && journalPM.SecurityLevel.Value != JournalPOCO.SecurityLevel.Value)))
            {
                UserQuery userQuery = new UserQuery(JournalPOCO.Tenant);
                
                var loggedUser = userQuery.GetSinglePM(loggedContactId, JournalPOCO.Tenant);
                if (loggedUser != null)
                {
                    if (!loggedUser.SecurityLevel.HasValue || journalPM.SecurityLevel.Value > loggedUser.SecurityLevel.Value)
                    {
                        bool useLocal = true;
                        string msg = TranslateTextsClassTranslate("Accounting.O.CantVoidJouranlItDidntTurnedToTransactions", 0, useLocal);
                        // לא ניתן לתת רמת אבטחת לצפיה בפקודת היומן שגבוהה מרמת האבטחה שקיימת למשתמש

                        if (String.IsNullOrEmpty(msg))
                            msg = "Cannot set Journal Viewing Security Level higher than the User Security Level";

                        string name = !String.IsNullOrEmpty(loggedUser.LocalName) ? loggedUser.LocalName : loggedUser.EnglishName;
                        msg = msg + " - " + name;

                        throw new ApplicationException(msg);

                    }
                }

            }


            if (JournalPOCO.StatusCode == journalPM.StatusCode)
            {
                return;
            }
            //



            if (journalPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                if (journalPM.StatusCode == ((int)JournalStatusTypePM.StatusCodeEnum.Voided).ToString())
                {
                    throw new ApplicationException("I Don't think its good idea to insert Journal and immediatlly to voided him ?!?!?(ask yaron)");
                }
            }
            if (journalPM.StatusCode == ((int)JournalStatusTypePM.StatusCodeEnum.Voided).ToString())
            {
                
                if (
                    JournalPOCO.StatusCode == ((int)JournalStatusTypePM.StatusCodeEnum.Voided).ToString() ||
                    JournalPOCO.StatusCode == ((int)JournalStatusTypePM.StatusCodeEnum.Failed).ToString()
                    )
                {
                    throw new ApplicationException("BLException :Void ACTION can only affect Approved journal");
                }
            }

            if (JournalPOCO.StatusCode == ((int)JournalStatusTypePM.StatusCodeEnum.Approved).ToString())
            {
                if (String.IsNullOrWhiteSpace(JournalPOCO.QueueId))
                {
                    if (journalPM.StatusCode != ((int)JournalStatusTypePM.StatusCodeEnum.Failed).ToString())
                    {

                        //bool useLocal = !(GetLoggedContact(JournalPOCO.Tenant).DontShowLocal);
                        bool useLocal = true;
                        var user = GetLoggedContact(JournalPOCO.Tenant);
                        if (user != null) useLocal = !(GetLoggedContact(JournalPOCO.Tenant).DontShowLocal);

                        string msg = TranslateTextsClassTranslate("Accounting.O.CantVoidJouranlItDidntTurnedToTransactions", 0, useLocal);

                        throw new ApplicationException(msg);
                    }
                }
                else
                {
                    if (journalPM.StatusCode != ((int)JournalStatusTypePM.StatusCodeEnum.Voided).ToString())
                    {
                        throw new ApplicationException("BLException :Approved Streamed Journal Can Only Change To Voided");
                    }
                }

                if (!String.IsNullOrWhiteSpace(journalPM.OriginalJournalId))
                {
                    throw new ApplicationException("BLException :Can not create Strono Journal to Strono Journal ");
                }

                if (JournalPOCO.IsVoided.GetValueOrDefault())
                {
                    throw new ApplicationException("already entityPOCO.IsVoided.GetValueOrDefault() ?!?!?");
                }
                var voidedProp = new List<string>(){
                        
                    JournalDataMapping.PMPropertyNames.StatusCode.ToString(),
                    JournalDataMapping.PMPropertyNames.IsVoided.ToString(),
                    JournalDataMapping.PMPropertyNames.VoidDate.ToString(),
                     JournalDataMapping.PMPropertyNames.VoidedByJournalId.ToString(),
                     JournalDataMapping.PMPropertyNames.VoidedByUserId.ToString(),
                     JournalDataMapping.PMPropertyNames.VoidedByUserName.ToString()
                     };



                var propChanged = changeTrackingEntityPM.ChangedProperties.Where(f =>
                    !voidedProp.Contains(f.PropertyName)).ToList();
                //update 
                if (propChanged.Any())
                {
                    throw new ApplicationException("BLException :Approved Journal Can Only Change To Voided Property");
                }
                if (journalPM.JournalLines.Any(jl => jl.ChangeSetOp != ChangeSetOperation.None))
                {
                    var jl1 = journalPM.JournalLines.First(jl => jl.ChangeSetOp != ChangeSetOperation.None);
                    //    throw new ApplicationException($"BLException :Approved Journal Can Only Change To Voided Property (Change JournalLines fix credrit or debit) line={jl1.Line} ");
                    bool useLocal = true;
                    string msg = TranslateTextsClassTranslate("Accounting.O.ChangeMadeToConnectedAccounts", 0, useLocal);
                    if (String.IsNullOrEmpty(msg)) msg = "A change was made in the connected accounts. The connected accounts must be returned to their state as it was when the original journal was prodiced. Line ";
                    throw new ApplicationException($"{msg}{jl1.Line} ");
                }

            }

            switch (journalPM.StatusCodeEnum)
            {

                case JournalStatusTypePM.StatusCodeEnum.WaitingforApprove:
                    break;
                case JournalStatusTypePM.StatusCodeEnum.InProcessing:
                    var journalApproveParser = NewJournalApproveParser(journalPM);
                    journalApproveParser.OnApproveUpdatingFillArrangeJournalPMResetControlAccount();
                    journalApproveParser.ParseIt();//throw exception if not valid !!!!

                    break;

                case JournalStatusTypePM.StatusCodeEnum.Voided:
                    //var JournalStornoService = new JournalStornoService(journalPM);
                    if (string.IsNullOrWhiteSpace(JournalPOCO.QueueId))
                    {
                        //bool useLocal = !(GetLoggedContact(JournalPOCO.Tenant).DontShowLocal);
                        
                        bool useLocal = true;
                        var user = GetLoggedContact(JournalPOCO.Tenant);
                        if (user != null) useLocal = !(GetLoggedContact(JournalPOCO.Tenant).DontShowLocal);

                        string msg = TranslateTextsClassTranslate("Accounting.O.CantVoidJouranlItDidntTurnedToTransactions", 0,useLocal);
                        throw new ApplicationException(msg);
                    }
                    if (_JournalStornoService==null)
                    {
                        throw new ApplicationException("Only JournalVoidUpdateService init _JournalStornoService !!");
                    }
                    JournalPM Storno = _JournalStornoService.CreateStornoAndCommitUpdate();
                    if (string.IsNullOrWhiteSpace(Storno.Id))
                    {
                        throw new ApplicationException(M_CreateStornoAndSaveFailed);
                    }
                    journalPM.VoidedByUserId = journalPM.UpdatedByUserId =
                        this.GetLogContactId(journalPM); // AuthenticationUtil.ResolveUserId(journalPM.Tenant);
                    journalPM.VoidedByJournalId = Storno.Id;
                    journalPM.IsVoided = true;
                    journalPM.VoidDate = DateTime.UtcNow;

                    //throw new ApplicationException("entityPM.VoidedBy = Storno.Id;// Add this line after VoidedBy convert from bool? to VC(15)");

                    break;

                case JournalStatusTypePM.StatusCodeEnum.Cancelled:
                    {
                        CancelJournal(journalPM);
                        break;
                    }

                default:
                    break;
            }

        }

        private void CancelJournal(JournalPM journalPM)
        {
            journalPM.IsVoided = true;
            journalPM.VoidedByUserId = journalPM.UpdatedByUserId = this.GetLogContactId(journalPM);
            journalPM.VoidDate = DateTime.UtcNow;
        }

        public virtual string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
        }



        public virtual IJournalApproveParser NewJournalApproveParser(JournalPM journalPM)
        {
            bool SuppressCheckGLAccountIsMultiCurrencyWI40640 = false;
            var journalApproveParser = new JournalApproveParser(journalPM, false,
                AccountingValidationContextServiceProvider
                .NewJournalValidatorContextByAContext(this._MainContext as IAccountingContext, journalPM, SuppressCheckGLAccountIsMultiCurrencyWI40640)
                );
            return journalApproveParser;
        }

        public virtual CoreBL.JournalLineOnUpdate CreateJournalLineOnUpdate()
        {
            var JournalUpdateOnCreatingLine = new JournalLineOnUpdate(this._MainContext as IAccountingContext);
            return JournalUpdateOnCreatingLine;
        }

        public virtual IActivityLogger CreateActivityLogger()
        {
            var myActivityLogger = ContainerAccessor.Container.ResolveSafe<IActivityLogger>() ?? new ActivityLoggerWrapper();
            return myActivityLogger;
        }






        public virtual List<GLAccountPM> GLAccountGetByGLAccountsIdList(int Tenant, List<string> GLAccountsIdList)
        {
            var qs = ContainerAccessor.Container.ResolveSafe<IGLAccountQueryService>() ??
                new GLAccountQueryService(this._MainContext as IAccountingContext);
            var GLAccountsList = qs.GetByGLAccountsIdList(GLAccountsIdList, Tenant);
            return GLAccountsList;
        }




        public virtual string GetObjectTableId(JournalPM entityPM)
        {

            var objectTableRepository = ContainerAccessor.Container.ResolveSafe<IObjectTableRepository>() ?? new ObjectTableRepository(entityPM.Tenant);
            var pocoObjectTable = //this.UpdateServiceProvider.GetObjectTableId(entityPM.Tenant);
                objectTableRepository.GetObjectTableByName("Journal", 0, true);
            return pocoObjectTable.Id;
        }


        public virtual Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }



        public virtual ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }

            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }




    }
}
