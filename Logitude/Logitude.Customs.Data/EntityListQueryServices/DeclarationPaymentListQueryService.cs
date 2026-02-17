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

    public partial class DeclarationPaymentListQueryService
    {
        private IQueryable<DeclarationPaymentList> GetIqueryableList(IQueryable<DeclarationPayment> iQueryable)
        {
            IQueryable<DeclarationPaymentList> query = (from a in iQueryable
                                                        select new DeclarationPaymentList()
                                                       {
                                                           CreatedByUserId = a.CreatedByUserId,
                                                           DeclarationId = a.DeclarationId,
                                                           IsProcessA = a.IsProcessA,
                                                           PaymentDate = a.PaymentDate,
                                                           ProcessADescription = a.ProcessADescription,
                                                           SignatoryIdentification = a.SignatoryIdentification,

                                                           Tenant=a.Tenant,
                                                       });
            return query;
		}

        private IQueryable<DeclarationPayment> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<DeclarationPayment> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	