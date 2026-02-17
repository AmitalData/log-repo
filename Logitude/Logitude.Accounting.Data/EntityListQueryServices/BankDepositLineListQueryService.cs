	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{

    public partial class BankDepositLineListQueryService
    {
        private IQueryable<BankDepositLineList> GetIqueryableList(IQueryable<BankDepositLine> iQueryable)
        {
            IQueryable<BankDepositLineList> query = (from a in iQueryable.Include("BankDeposit").Include("ARPaymentCheque")
                                                     select new BankDepositLineList()
                                                     {
                                                         ARPaymentChequeId = a.ARPaymentCheque.Id,
                                                         ChequeNumber = a.ARPaymentCheque.ChequeNumber,
                                                         DueDate = a.ARPaymentCheque.ValueDate,
                                                         LocalAmount = a.ARPaymentCheque.LocalAmount,
                                                         Currency = a.ARPaymentCheque.Currency.Code,
                                                         ForeignAmount = a.ARPaymentCheque.ForeignAmount,
                                                         AccountNumber = a.ARPaymentCheque.BankAccount,
                                                         //Bank = a.ARPaymentCheque.Bank.EnglishName,
                                                         Branch = a.ARPaymentCheque.BankBranch,
                                                         DepositId = a.BankDeposit.Id,
                                                         IsOutOfDeposit = a.IsOutOfDeposit,
                                                         Line = a.Line,
                                                         Notes = a.Notes,
                                                         OutOfDepositeDate = a.OutOfDepositeDate,
                                                         Tenant = a.Tenant,

                                                     });
            return query;
        }

        private IQueryable<BankDepositLine> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<BankDepositLine> iQueryable, int tenant)
        {
            throw new NotImplementedException();
        }
        private IQueryable<BankDepositLine> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<BankDepositLine> iQueryable, int tenant)
        {
            return iQueryable;
        }

        public List<BankDepositLineList> GetByARPaymentChequeId(string aRPaymentChequeId, string searchFields, int tenant)
        {
            IQueryable<BankDepositLine> linesQuery = (from a in context.BankDepositLines
                                                         where a.Tenant == tenant && a.ARPaymentChequeId == aRPaymentChequeId && searchFields.Contains(searchFields)
                                                         select a);

            IQueryable<BankDepositLineList> linesListQuery = this.GetIqueryableList(linesQuery);
            List<BankDepositLineList> linesList = linesListQuery.ToList();
            return linesList;
        }

    }


}
	