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

    public partial class DeclarationPaymentProtestListQueryService
    {
	    private IQueryable<DeclarationPaymentProtestList> GetIqueryableList(IQueryable<DeclarationPaymentProtest> iQueryable)
        {
            IQueryable<DeclarationPaymentProtestList> query = (from a in iQueryable
                                                               select new DeclarationPaymentProtestList()
                                                              {
                                                                  DeclarationId = a.DeclarationId,
                                                                  Line = a.Line,
                                                                  AmountInDispute = a.AmountInDispute,
                                                                  CustomsAgentExplanation = a.CustomsAgentExplanation,
                                                                  GoodsItemClassification = a.GoodsItemClassification,
                                                                  GoodsItemLineNumber = a.GoodsItemLineNumber,
                                                                  InvoiceNumber = a.InvoiceNumber,
                                                                  ProtestTypeCode = a.ProtestTypeCode,
                                                                  Tenant = a.Tenant,
                                                              });
            return query;
		}

        private IQueryable<DeclarationPaymentProtest> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<DeclarationPaymentProtest> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	