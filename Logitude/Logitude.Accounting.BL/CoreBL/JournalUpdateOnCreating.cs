using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.Accounting.BL.CoreBL
{
    public class JournalUpdateOnCreating : IJournalUpdateInsert
    {
        private IAccountingContext _MainContext;
        public JournalUpdateOnCreating(IAccountingContext mainContext)
        {
            this._MainContext = mainContext;
        }
        
        public void OnCreating(JournalPM entityPM, EntityPM entityParentPM)
        {

            if (!String.IsNullOrWhiteSpace(entityPM.ExternalNo) && !String.IsNullOrWhiteSpace(entityPM.ExternalSystem))
            {
                JournalQueryService journalQuery = new JournalQueryService(entityPM.Tenant);
                String oldjournalNumber = "";
                if (journalQuery.CheckIfExternalNoAndSystemExist(entityPM.ExternalNo, entityPM.ExternalSystem, out oldjournalNumber, entityPM.Tenant))
                {
                    string basic_text_ExternalExist =
                        //"There is a Journal ("+ journalNumber + ") with the same ExternalNo And ExternalSystem";
                        JournalValidator.M_ExternalNoAlreadyExists_1 + oldjournalNumber + JournalValidator.M_ExternalNoAlreadyExists_2;
                    throw new Exception(basic_text_ExternalExist);
                }
            }
    
            entityPM.Id = //IdCounter.GetNumber(

                // new IdCounterWrapper().GetNumber(
                //GetNumberJournal(), 
                IdCounterWrapperGetNumber(entityPM.Tenant);


            //entityPM.JournalNumber =
            //    //(new CodeCounterWrapper()).GetNumber(GetCodeNumberJournal(), 
            //    CodeCounterWrapperGetNumber(
            //    entityPM.Tenant).ToString();


            string loggedContactId = GetLogContactId(entityPM);
            if (string.IsNullOrWhiteSpace(loggedContactId))
            {
                throw new ApplicationException("Logged Contact Id is required ");
            }
            string ObjectTableId = GetObjectTableId(entityPM);

            AddAcitivityLog(entityPM, loggedContactId, ObjectTableId);


            var myAccountingEntityDetails = new AccountingEntityDetails();
            var myAccEntityReconciliation10 = myAccountingEntityDetails
                .GetAll()
                .FirstOrDefault(r => r.EnglishName =="Adjustment");


            //if (entityPM.TypeCode == "0" && entityPM.AccountingEntityReference == null) // Manual
            //{
            //    if (myAccEntityReconciliation10.Code == entityPM.AccountingEntityCode)
            //    {
            //        //entityPM.AccountingEntityReference = will be enter WhileStreaming ;
 
            //    }
            //    else
            //    {
            //        entityPM.AccountingEntityReference = entityPM.JournalNumber;
            //    }
            //}
            var DateTimeNow = GetDateTimeNow();

            if (entityPM.CreateDate == null)
                entityPM.CreateDate = DateTimeNow;

            if (entityPM.UpdateDate == null)
                entityPM.UpdateDate = DateTimeNow;

            ClearDMYByUserId(entityPM, loggedContactId);
            if (entityPM.UpdatedByUserId == null)
                entityPM.UpdatedByUserId = loggedContactId;

            if (entityPM.CreatedByUserId == null)
                entityPM.CreatedByUserId = loggedContactId;

            entityPM.IsVoided = entityPM.IsVoided ?? false;
            if (String.IsNullOrWhiteSpace(entityPM.AccountingEntityId))
            {
                if (myAccEntityReconciliation10.Code == entityPM.AccountingEntityCode)
                {
                    //do not set  entityPM.AccountingEntityId!!! will be enter WhileStreaming 
 
                }
                else
                {
                    entityPM.AccountingEntityId = entityPM.Id;
                }
                
            }
            if (String.IsNullOrWhiteSpace(entityPM.TypeCode)) entityPM.TypeCode = "0"; //Manual
            if (String.IsNullOrWhiteSpace(entityPM.AccountingEntityCode)) entityPM.AccountingEntityCode = "1"; //Journal
            if (String.IsNullOrWhiteSpace(entityPM.CreatedByUserId)) entityPM.CreatedByUserId = AuthenticationUtil.GetAuthenticatedUser();// "1-14733"; //Alex //COMPILE//
            if (entityPM.StatusCode == "6" && String.IsNullOrWhiteSpace(entityPM.ApprovedByUserId)) entityPM.ApprovedByUserId = entityPM.CreatedByUserId;
            if (entityPM.JournalLines != null)
            {
                //if (entityPM.CreateDate == DateTime.MinValue) entityPM.CreateDate = DateTime.Now;
                entityPM.CreateDate = DateTime.Now; //eyal 

                var originalLines = entityPM.JournalLines.ToList();
                for (int i = 0; i < originalLines.Count; i++)
                {
                    JournalLinePM item = originalLines[i];

                    JournalUpdateService journalUpdateService = new JournalUpdateService(this._MainContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                    var newJournalLinePM = journalUpdateService.CheckJournalActionCodeAndSplitedIt(item, entityPM.JournalLines);
                    OnCreateLine(entityPM, item);

                    if (newJournalLinePM != null)
                    {
                        entityPM.JournalLines.Add(newJournalLinePM);
                        OnCreateLine(entityPM, newJournalLinePM);
                    }
                }

                foreach (var item in entityPM.JournalReconciles)
                {

                    item.JournalId = entityPM.Id;

                }
                foreach (var item in entityPM.JournalExternalReconciles)
                {

                    item.JournalId = entityPM.Id;

                }
                var renumber = true;
                if (renumber)
                {
                    int i = 1;
                    foreach (JournalLinePM item in entityPM.JournalLines)
                    {
                        item.Line = i++;

                    }
                }

            }



            //if (entityPM.StatusCode == "2")
            //{
            //    Case_2(entityPM);
            //}

        }
  


        public virtual void ClearDMYByUserId(JournalPM entityPM, string loggedContactId)
        {
            ContactRepository contactRep = new ContactRepository(entityPM.Tenant);


            var contact = contactRep.GetSingleContact(entityPM.UpdatedByUserId, entityPM.Tenant);
            if (contact == null)
            {
                entityPM.UpdatedByUserId = null;
            }

            contact = contactRep.GetSingleContact(entityPM.CreatedByUserId, entityPM.Tenant);
            if (contact == null)
            {
                entityPM.CreatedByUserId = null;
            }


            
        }

        public virtual void AddAcitivityLog(JournalPM entityPM, string loggedContactId, string ObjectTableId)
        {
            var myActivityLogger = ContainerAccessor.Container.ResolveSafe<IActivityLogger>() ?? 
                new ActivityLoggerWrapper();
            myActivityLogger.AddAcitivityLog(entityPM.Id, ObjectTableId, entityPM.Tenant, "N", loggedContactId);
        }

        public virtual string IdCounterWrapperGetNumber(int Tenant)
        {
            return (new IdCounterWrapper()).GetNumber(
                    GetNumberJournal(), Tenant);
        }
        //public virtual int CodeCounterWrapperGetNumber(int Tenant)
        //{
        //    return CodeCounter.GetNumber(GetCodeNumberJournal(), Tenant, false);
        //}
        public virtual void OnCreateLine(JournalPM entityPM, JournalLinePM journalLinePM)
        {
            var journalLineUpdateInsert = new JournalLineOnUpdate(this._MainContext);
            journalLineUpdateInsert.OnUpdate(journalLinePM, entityPM);
            journalLinePM.ChangeSetOp = ChangeSetOperation.Insert;
        }

        public virtual DateTime GetDateTimeNow()
        {
            return DateTime.Now;
        }
        public static string GetCodeNumberJournal()
        {
            return "Journal.JournalNumber";
        }

        public static string GetNumberJournal()
        {
            return "Journal";
        }

        public virtual string GetLogContactId(JournalPM entityPM)
        {

            return AuthenticationUtil.ResolveUserId(entityPM.Tenant);
            //string email = HttpContext.Current.User.Identity.Name;


            //var contactRepository = ContainerAccessor.Container.ResolveSafe<IContactRepository>() ?? new ContactRepository(entityPM.Tenant);
            //Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            //if (loggedContact != null)
            //{
            //    return loggedContact.Id;
            //}
            //return null;


        }
        public virtual string GetObjectTableId(JournalPM entityPM)
        {

            var objectTableRepository = ContainerAccessor.Container.ResolveSafe<IObjectTableRepository>() ?? new ObjectTableRepository(entityPM.Tenant);
            var pocoObjectTable = //this.UpdateServiceProvider.GetObjectTableId(entityPM.Tenant);
                objectTableRepository.GetObjectTableByName(
                GetNumberJournal()///=="Journal"
            , 0, true);
            return pocoObjectTable.Id;
        }

        public class Factory
        {
            private IJournalUpdateInsert customManager = null;
            public IJournalUpdateInsert Create(IAccountingContext mainContext)
            {
                if (customManager != null) return customManager;

                return new JournalUpdateOnCreating(mainContext) as IJournalUpdateInsert;
            }
            public void SetManager(IJournalUpdateInsert mgr)
            {
                customManager = mgr;
            }
        }

    }
    public interface IJournalUpdateInsert
    {
        void OnCreating(JournalPM entityPM, EntityPM entityParentPM);
        string GetObjectTableId(JournalPM entityPM);
        string GetLogContactId(JournalPM entityPM);
    }
}
