using Logitude.Accounting.BL.CoreBL.BankDeposit;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public class BankDepositOnCreatingService: IBankDepositOnCreatingService
    {
        private IAccountingContext _MainContext;
        bool showLocals = false;
        public BankDepositOnCreatingService(IAccountingContext mainContext, int tenant)
        {
            _MainContext = mainContext;
            showLocals = LoggedContactResolver.GetLoggedContactShowLocal(tenant);
        }
        public BankDepositOnCreatingService()
        {

        }
        // Main Method
        public void OnCreating(BankDepositPM depositPM)
        {
            CashBookPM cashBookPM = GetCashbookById(depositPM.Tenant, depositPM.CashBookId);
            try
            {
                SetEntityId(depositPM);
                SetEntityCode(depositPM);
                SetUserFields(depositPM);
                depositPM.UpdateDate = GetCurrentDateTime(depositPM.Tenant);
                depositPM.CreateDate = GetCurrentDateTime(depositPM.Tenant);

                CopyDepositIdToLines(depositPM);

                CheckCashbookAmount(depositPM, depositPM.Tenant, cashBookPM);

                CreateJournalForBankDeposit(depositPM);

                if (!depositPM.IsCashDeposit)
                {
                    DepositChequesForBankDeposit(depositPM);
                }

                UpdateCashbookTotals(depositPM, cashBookPM);
                SubmitCashbook(depositPM.Tenant, cashBookPM);

                LogActivity(depositPM);
            }
            catch (Exception exc) {
                cashBookPM.InDepositingProgress = false;
                SubmitCashbook(depositPM.Tenant, cashBookPM);
                throw new ApplicationException(exc.Message);
            }

        }
        public virtual void CreateJournalForBankDeposit(BankDepositPM depositPM)
        {
            BankDepositJournalCreator depositJournalCreator = new BankDepositJournalCreator(depositPM);
            depositJournalCreator.CreateJounal();


        }


        public virtual void DepositChequesForBankDeposit(BankDepositPM depositPM)
        {
            BankDepositingService depositor = new BankDepositingService(depositPM);
            depositor.DepositCheques();
        }
        private static void UpdateCashbookTotals(BankDepositPM depositPM, CashBookPM cashBookPM)
        {
            cashBookPM.TotalAmount = cashBookPM.TotalAmount - Math.Round(depositPM.ForeignAmount, 2);
            cashBookPM.InDepositingProgress = false;
        }

        public virtual void SubmitCashbook(int tenant, CashBookPM cashBookPM)
        {
            cashBookPM.ChangeSetOp = ChangeSetOperation.Update;

            IAccountingContext MyContext2 = AccountingContext.GetContext(tenant);
            var myCashBookUpdateService = new CashBookUpdateService(MyContext2, new Dictionary<string, IContext>(), tenant);
            myCashBookUpdateService.Update(cashBookPM, true);
        }
        private static void CheckCashbookAmount(BankDepositPM entityPM, int tenant, CashBookPM cashBook)
        {
            /* canceled validation
            if (entityPM.ForeignAmount > cashBook.TotalAmount)
            {
                //showlocal
                bool showLocal = false;
                ContactPM user = LoggedContactResolver.GetLoggedContact(tenant);//GetLoggedContact(entityPM.Tenant);
                if (user != null)
                    showLocal = !user.DontShowLocal;

                throw new ApplicationException(TextCodesTranslator.TranslateText("BankDeposit.O.DepositAmountmustbelessthanCashbook", 0, showLocal));
            }*/
        }
        public virtual CashBookPM GetCashbookById(int tenant, string id)
        {
            CashBookQueryService cashBookQueryService = new CashBookQueryService(tenant);
            CashBookPM cashBook = cashBookQueryService.GetSingle(id, false, false);
            return cashBook;
        }
        private static void CopyDepositIdToLines(BankDepositPM entityPM)
        {
            foreach (BankDepositLinePM item in entityPM.BankDepositLines)
            {
                item.DepositId = entityPM.Id;
            }
        }

        private void SetUserFields(BankDepositPM entityPM)
        {
            ContactPM user = GetLoggedContact(entityPM.Tenant);
            if (user != null)
            {

                entityPM.UpdatedByUserId = user.Id;

                if (entityPM.CreatedByUserId == null)
                {
                    entityPM.CreatedByUserId = user.Id;
                }
            }
        }

        private void SetEntityCode(BankDepositPM entityPM)
        {
            if (entityPM.DepositNumber == 0) entityPM.DepositNumber = CodeCounterWrapperGetNumber(entityPM.Tenant);
        }

        private void SetEntityId(BankDepositPM entityPM)
        {
            if (entityPM.Id == null || entityPM.Id == "")
            {
                entityPM.Id = IdCounterWrapperGetNumber(entityPM.Tenant);
            }
        }

        #region Logic
        // Create journal and its lines for cashbook and bank

        #endregion

        #region Others functions

        public virtual DateTime GetCurrentDateTime(int tenant)
        {
            return TenantServerConfigration.GetCurrentDateTime(tenant);
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


        public virtual string IdCounterWrapperGetNumber(int Tenant)
        {
            return (new IdCounterWrapper()).GetNumber(
                    "BankDeposit", Tenant);
        }

        public virtual int CodeCounterWrapperGetNumber(int Tenant)
        {
            return (new CodeCounterWrapper(false)).GetNumber(
                    "BankDeposit", Tenant);
        }

        public virtual void LogActivity(BankDepositPM entityPM)
        {
            //Activity Log
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("BankDeposit", 0, true);
            var myLoggedUser = GetLoggedContact(entityPM.Tenant);
            if (myLoggedUser != null)
            {
                ActivityLogger.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "N", myLoggedUser.Id);
            }
        }

        #endregion

    }


    public interface IBankDepositOnCreatingService {
        void OnCreating(BankDepositPM entityPM);
        DateTime GetCurrentDateTime(int tenant);
        ContactPM GetLoggedContact(int tenant);
        string IdCounterWrapperGetNumber(int Tenant);
        int CodeCounterWrapperGetNumber(int Tenant);
        void LogActivity(BankDepositPM entityPM);
    }
}
