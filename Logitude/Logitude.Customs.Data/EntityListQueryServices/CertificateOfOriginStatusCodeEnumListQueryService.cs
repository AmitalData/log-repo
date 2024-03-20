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

    public partial class CertificateOfOriginStatusCodeEnumListQueryService
    {
	    private IQueryable<CertificateOfOriginStatusCodeEnumList> GetIqueryableList(IQueryable<CertificateOfOriginStatusCodeEnum> iQueryable)
        {
		IQueryable<CertificateOfOriginStatusCodeEnumList> query = (from a in iQueryable
                                            select new CertificateOfOriginStatusCodeEnumList()
											{
                     
					                          Code = a.Code,
					
					                          LocalName = a.LocalName,
					
					                          SearchFields = a.SearchFields,
					
					                          EnglishName = a.EnglishName,
					
					                          Inactive = a.Inactive,
					
					                          RecordEditable = a.RecordEditable,
					
		                    	            });
            return query;
		}

		private IQueryable<CertificateOfOriginStatusCodeEnum> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CertificateOfOriginStatusCodeEnum> iQueryable)
        {
            return iQueryable;
        }
    }


}
	