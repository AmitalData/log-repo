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

    public partial class DeclarationTaxListQueryService
    {
	    private IQueryable<DeclarationTaxList> GetIqueryableList(IQueryable<DeclarationTax> iQueryable)
        {
            IQueryable<DeclarationTaxList> query = (from a in iQueryable
                                                    select new DeclarationTaxList()
                                                     {
                                                      DeclarationId = a.DeclarationId,
                                                      DeferredTaxAmount = a.DeferredTaxAmount,
                                                      TotalAmount = a.TotalAmount,
                                                      TaxTypeCode = a.TaxTypeCode,

                                                     });
            return query;
		}

        private IQueryable<DeclarationTax> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<DeclarationTax> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	