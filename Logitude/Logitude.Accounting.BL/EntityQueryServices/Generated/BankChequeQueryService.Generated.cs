
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.BL.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class BankChequeQueryService : EntityQueryService<BankCheque, BankChequeKeys, BankChequePM, object, BankChequeKeys>
    {

        BankChequeRepository repository;
        IAccountingContext context;
        public BankChequeQueryService(int tenant)
        {
            context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new BankChequeRepository(context);
            Repository = repository;
            mapping = new BankChequeDataMapping();
        }

        public BankChequeQueryService(BankChequeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new BankChequeDataMapping();
        }

        public BankChequeQueryService(IAccountingContext context)
        {
            this.repository = new BankChequeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new BankChequeDataMapping();
        }

        public BankChequePM GetSingle(string id, bool getComposition, bool getFromCache)
        {
            EntityKeys = new BankChequeKeys() { Id = id };

            return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }


        protected override EntityKeyFields GetKeys(BankCheque entityPOCO)
        {
            BankChequeKeys entityKeys = new BankChequeKeys() { Id = entityPOCO.Id, };
            return entityKeys;
        }


    }

}
