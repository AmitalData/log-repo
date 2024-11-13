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

    public partial class SupplierInvioceItemCertificatDefaultListQueryService
    {
	    private IQueryable<SupplierInvioceItemCertificatDefaultList> GetIqueryableList(IQueryable<SupplierInvioceItemCertificatDefault> iQueryable)
        {
		IQueryable<SupplierInvioceItemCertificatDefaultList> query = (from a in iQueryable
                                            select new SupplierInvioceItemCertificatDefaultList()
											{
                     
					                          SequenceNumeric = a.SequenceNumeric,
					
		                    	            });
            return query;
		}

		private IQueryable<SupplierInvioceItemCertificatDefault> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<SupplierInvioceItemCertificatDefault> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	