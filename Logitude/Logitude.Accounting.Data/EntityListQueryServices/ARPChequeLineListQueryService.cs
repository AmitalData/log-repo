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

    public partial class ARPChequeLineListQueryService
    {
        private IQueryable<ARPChequeLineList> GetIqueryableList(IQueryable<ARPChequeLine> iQueryable)
        {
            IQueryable<ARPChequeLineList> query = (from a in iQueryable
                                                   select new ARPChequeLineList()
                                                   {
                                                       Id = a.Id,
                                                       Tenant = a.Tenant,
                                                       SearchFields = a.SearchFields,
                                                       LineNumber = a.LineNumber,
                                                       ChequeNumber = a.ChequeNumber,
                                                       CurrencyId = a.CurrencyId,
                                                       CurrencyCode = a.Currency != null ? a.Currency.Code : null,
                                                       CurrencyName = a.Currency != null ? a.Currency.EnglishName : null,
                                                       ValueDate = a.ValueDate,
                                                       LocalAmount = a.LocalAmount,
                                                       ForeignAmount = a.ForeignAmount,
                                                       BankId = a.BankId,
                                                       BankBranch = a.BankBranch,
                                                       BankAccount = a.BankAccount,
                                                       BankNumber = a.Bank != null ? a.Bank.Code : null,
                                                       BankName = a.Bank != null ? a.Bank.EnglishName : null,
                                                       PaymentId = a.PaymentId,
                                                       PaymentNumber = a.Payment != null ? a.Payment.PaymentNo : null,
                                                   });
            return query;
        }

        private IQueryable<ARPChequeLine> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ARPChequeLine> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<ARPChequeLine> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<ARPChequeLine> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }


}
	