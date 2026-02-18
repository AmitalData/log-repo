	using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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

    public partial class CertificateOfOriginTypeCodeEnumListQueryService
    {
	    private IQueryable<CertificateOfOriginTypeCodeEnumList> GetIqueryableList(IQueryable<CertificateOfOriginTypeCodeEnum> iQueryable)
        {
		IQueryable<CertificateOfOriginTypeCodeEnumList> query = (from a in iQueryable
                                            select new CertificateOfOriginTypeCodeEnumList()
											{
                     
					                          Code = a.Code,
					
					                          LocalName = a.LocalName,
					
					                          SearchFields = a.SearchFields,
					
					                          EnglishName = a.EnglishName,
					
					                          Inactive = a.Inactive,
					
					                          IsCustomApprovalRequired = a.IsCustomApprovalRequired,
					
					                          IsCriterionMandatory = a.IsCriterionMandatory,
					
		                    	            });
            return query;
		}

		private IQueryable<CertificateOfOriginTypeCodeEnum> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CertificateOfOriginTypeCodeEnum> iQueryable)
        {
            return iQueryable;
        }
    }


}
	