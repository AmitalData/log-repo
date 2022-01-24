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

    public partial class IncotemrsFileValidationListQueryService
    {
	    private IQueryable<IncotemrsFileValidationList> GetIqueryableList(IQueryable<IncotemrsFileValidation> iQueryable)
        {
		IQueryable<IncotemrsFileValidationList> query = (from a in iQueryable
                                            select new IncotemrsFileValidationList()
											{
												Code = a.Code,
												LocalName = a.LocalName,
												SearchFields = a.SearchFields,
												Inactive = a.Inactive,
												LeadDocumentTypeID=a.LeadDocumentTypeID,
												LeadDocumentTypeName=a.LeadDocumentTypeName,
												EnglishName=a.EnglishName,
												CargoIdentifierTypeID=a.CargoIdentifierTypeID,
												CargoIdentifierTypeName=a.CargoIdentifierTypeName,
												TermsOfSaleTypeID=a.TermsOfSaleTypeID,
												IsFreightCharge=a.IsFreightCharge,
												IsInsurance	= a.IsInsurance,
												IsPortIsraelCharge=a.IsPortIsraelCharge,

											});
            return query;
		}

		private IQueryable<IncotemrsFileValidation> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<IncotemrsFileValidation> iQueryable)
        {
			return iQueryable;
		}
			}


}
	