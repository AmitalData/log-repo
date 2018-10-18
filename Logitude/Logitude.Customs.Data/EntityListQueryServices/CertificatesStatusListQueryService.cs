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

    public partial class CertificatesStatusListQueryService
    {
	    private IQueryable<CertificatesStatusList> GetIqueryableList(IQueryable<CertificatesStatus> iQueryable)
        {
		IQueryable<CertificatesStatusList> query = (from a in iQueryable
                                            select new CertificatesStatusList()
											{
                     
					                          Code = a.Code,
					
					                          LocalName = a.LocalName,
					
					                          EnglishName = a.EnglishName,
					
		                    	            });
            return query;
		}

		private IQueryable<CertificatesStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CertificatesStatus> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	