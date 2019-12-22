using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.BL.DataContract;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class CashBookQueryService : EntityQueryService<CashBook, CashBookKeys, CashBookPM, object, CashBookKeys>
    {
        public override void GetComposition(EntityKeyFields entityKeys, CashBookPM entityPM)
        {
            IAccountingContext context = MainContext as AccountingContext;
            CashBookKeys cashBookKeys = entityKeys as CashBookKeys;

            CashBookLineQueryService cashBookLineQueryService = new CashBookLineQueryService(context);



            //******getting all compositionTables for response service purposes only *****///

            entityPM.CashBookLines = cashBookLineQueryService.GetMulti(cashBookKeys, true);

            // entityPM.DeclarationErrorViews = this.GetDeclarationErrors(declarationKeys.Id, entityPM.Tenant, null);
            //****************************************************************************//

        }

        public CashBookPM GetByPaymentAndCurrency(string currency, string paymentMethod, int tenant)
        {
            CashBookRepository cashBookQuery = new CashBookRepository(tenant);
            var cashBook = cashBookQuery.GetByPaymentAndCurrency(currency, paymentMethod, tenant);

            if (cashBook != null)
            {
                EntityPM = new CashBookPM();
                mapping.CustomPOCOToPM(EntityPM, cashBook);
                mapping.POCOToPM(EntityPM, cashBook);
            }

            return EntityPM;
        }
        public CashBookPM GetByPaymentAndCurrencyAndBranch(string currency, string paymentMethod,string branch, int tenant)
        {
            CashBookRepository cashBookQuery = new CashBookRepository(tenant);
            var cashBook = cashBookQuery.GetByPaymentAndCurrencyAndBranch(currency, paymentMethod, branch, tenant);

            if (cashBook != null)
            {
                EntityPM = new CashBookPM();
                mapping.CustomPOCOToPM(EntityPM, cashBook);
                mapping.POCOToPM(EntityPM, cashBook);
            }

            return EntityPM;
        }
        public List<CashBookPM> GetListByPaymentAndCurrencyAndBranch(string code, string currencyId, string branch, int tenant)
        {
            CashBookRepository cashBookQuery = new CashBookRepository(tenant);
            List<CashBook> cashBook = cashBookQuery.GetListByPaymentAndCurrencyAndBranch(code,currencyId, branch, tenant);
            return cashBook.Select(rec => this.GetEntityPM(rec)).ToList();
        }

        public CashbookChequesCounter GetCashbookChequesCounter(string cashbookId, int tenant)
        {
            CashBookRepository cashBookQuery = new CashBookRepository(tenant);
            CashbookChequesCounter chequesCounters = new CashbookChequesCounter();

            chequesCounters.CashChequesCount = cashBookQuery.GetCashChequesTotalsForCashbook(cashbookId, tenant);
            chequesCounters.PostdatedChequesCount = cashBookQuery.GetPostdatedChequesTotalsForCashbook(cashbookId, tenant);

            return chequesCounters;
        }
        public int GetUndepositedChequesCount(string cashbookId, int tenant)
        {
            CashBookRepository cashBookQuery = new CashBookRepository(tenant);
            return cashBookQuery.GetUndepositedChequesCount(cashbookId, tenant);
        }
        public decimal GetCashbookChequesTotal(string cashbookId, string chequeFilterType, int tenant)
        {
            CashBookRepository cashBookQuery = new CashBookRepository(tenant);
            return cashBookQuery.GetChequesTotal(cashbookId, chequeFilterType, tenant);
        }


        public List<CashBookPM> GetAll(int tenant)
        {
            CashBookRepository repo = new CashBookRepository(tenant);
            List<CashBook> cashBook = repo.GetAll(tenant).ToList();
            return cashBook.Select(rec => GetEntityPM(rec,true, new CashBookKeys() { Id=rec.Id})).ToList();
        }


        public CashBookPM GetLightCashbook(string id, int tenant)
        {
            CashBook cashBook = repository.GetSingle(id, tenant);
            CashBookPM entityPM = new CashBookPM();

            if (cashBook != null)
            {
                mapping.POCOToPM(entityPM, cashBook);
            }

            return entityPM;
        }
    }

}
