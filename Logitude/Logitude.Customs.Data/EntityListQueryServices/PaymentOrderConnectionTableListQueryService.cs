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

    public partial class PaymentOrderConnectionTableListQueryService
    {
	    private IQueryable<PaymentOrderConnectionTableList> GetIqueryableList(IQueryable<PaymentOrderConnectionTable> iQueryable)
        {

            IQueryable<PaymentOrderConnectionTableList> query = (from a in iQueryable
                                                                 select new PaymentOrderConnectionTableList()
                                                       {
                                                         ConnectedEntityCode = a.ConnectedEntityCode,
                                                         ConnectedEntityId = a.ConnectedEntityId,
                                                         PaymentOrderId = a.PaymentOrderId,
                                                         Tenant = a.Tenant,


                                                       });
            return query;
		}

        private IQueryable<PaymentOrderConnectionTable> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<PaymentOrderConnectionTable> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	