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

    public partial class CertificateOfOriginMandatoryFieldsListQueryService
    {
	    private IQueryable<CertificateOfOriginMandatoryFieldsList> GetIqueryableList(IQueryable<CertificateOfOriginMandatoryFields> iQueryable)
        {
			IQueryable<CertificateOfOriginMandatoryFieldsList> query = (from a in iQueryable.Include("objectfields")
																		select new CertificateOfOriginMandatoryFieldsList()
																		{

																			Code = a.Code,

																			LocalName = a.LocalName,

																			SearchFields = a.SearchFields,

																			EnglishName = a.EnglishName,

																			Inactive = a.Inactive,

																			IsMandatory = a.IsMandatory,

																			Location = a.Location,

																			LastUpdatedDate = a.LastUpdatedDate,

																			MappedCertificateFields = a.MappedCertificateFields,
                                                                            ///MappedCertificateFieldsName = a.MappedCertificateFieldsName,
                                                                            CertificateOfOriginTypeCodeID = a.CertificateOfOriginTypeCodeID,

                                                                            CertificateOfOriginTypeName = a.CertificateOfOriginTypeName,
						

																		}); ;
            return query;
		}

		private IQueryable<CertificateOfOriginMandatoryFields> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CertificateOfOriginMandatoryFields> iQueryable)
        {
            return iQueryable;
        }
	}
}
	