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

    public partial class DeclarationPaymentMethodListQueryService
    {
	    private IQueryable<DeclarationPaymentMethodList> GetIqueryableList(IQueryable<DeclarationPaymentMethod> iQueryable)
        {
            IQueryable<DeclarationPaymentMethodList> query = (from a in iQueryable
                                                              select new DeclarationPaymentMethodList()
                                                        {
                                                            DeclarationId = a.DeclarationId,
                                                            AccountNumber = a.AccountNumber,
                                                            Amount = a.Amount,
                                                            BankCode = a.BankCode,
                                                            BranchCode = a.BranchCode,
                                                            Line = a.Line,
                                                            MethodTypeCode = a.MethodTypeCode,
                                                            PayerActivityTypeCode = a.PayerActivityTypeCode,
                                                            SequenceNumeric = a.SequenceNumeric,
                                                            Tenant = a.Tenant,
                                                            InternalBankId = a.InternalBankId,
                                                            CustomsBranchId = a.CustomsBranchId,
                                                        });
            return query;
		}

        private IQueryable<DeclarationPaymentMethod> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<DeclarationPaymentMethod> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	