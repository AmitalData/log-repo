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

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class PaymentOrderMethodListQueryService
    {
	    private IQueryable<PaymentOrderMethodList> GetIqueryableList(IQueryable<PaymentOrderMethod> iQueryable)
        {
            IQueryable<PaymentOrderMethodList> query = (from a in iQueryable
                                                        select new PaymentOrderMethodList()
                                                       {
                                                           AccountNumber = a.AccountNumber,
                                                           Amount = a.Amount,
                                                           BankCode = a.BankCode,
                                                           BranchCode = a.BranchCode,
                                                           Line = a.Line,
                                                           PaymentMethodStatusCode = a.PaymentMethodStatusCode,
                                                           PaymentOrderId = a.PaymentOrderId,
                                                           Tenant = a.Tenant,
                                                           TypeCode = a.TypeCode,
                                                           InternalBankId = a.InternalBankId,
                                                           CustomerActivityTypeCode = a.CustomerActivityTypeCode,
                                                           
                                                       });
            return query;
		}

        private IQueryable<PaymentOrderMethod> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<PaymentOrderMethod> iQueryable, int tenant)
        {
            return iQueryable;
        }

	}


}
	