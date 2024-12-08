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

    public partial class CertificateOfOriginConnectionListQueryService
    {
	    private IQueryable<CertificateOfOriginConnectionList> GetIqueryableList(IQueryable<CertificateOfOriginConnection> iQueryable)
        {
		IQueryable<CertificateOfOriginConnectionList> query = (from a in iQueryable
                                            select new CertificateOfOriginConnectionList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          SearchFields = a.SearchFields,
					
					                          CooStatus = a.CooStatus,
					
					                          CooReason = a.CooReason,
					
		                    	            });
            return query;
		}

		private IQueryable<CertificateOfOriginConnection> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CertificateOfOriginConnection> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	