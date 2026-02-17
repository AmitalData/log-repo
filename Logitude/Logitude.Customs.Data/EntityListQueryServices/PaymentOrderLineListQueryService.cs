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

    public partial class PaymentOrderLineListQueryService
    {
	    private IQueryable<PaymentOrderLineList> GetIqueryableList(IQueryable<PaymentOrderLine> iQueryable)
        {
            IQueryable<PaymentOrderLineList> query = (from a in iQueryable
                                                      select new PaymentOrderLineList()
                                                          {
                                                              Amount = a.Amount,
                                                              ParagraphTypeCode = a.ParagraphTypeCode,
                                                              PaymentOrderId = a.PaymentOrderId,
                                                              Tenant = a.Tenant,
                                                              


                                                          });
            return query;
		}

        private IQueryable<PaymentOrderLine> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<PaymentOrderLine> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	