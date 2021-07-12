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

    public partial class CustomsDocumentsDefinitionListQueryService
    {
	    private IQueryable<CustomsDocumentsDefinitionList> GetIqueryableList(IQueryable<CustomsDocumentsDefinition> iQueryable)
        {
		IQueryable<CustomsDocumentsDefinitionList> query = (from a in iQueryable.Include("CustomDocumentType").Include("CustomsTransportMode").Include("GovernmentProcedureType").Include("CargoIdentifireType").Include("LeadDocumentType")
                                                            select new CustomsDocumentsDefinitionList()
											                {
					                                            Id = a.Id,
					                                            Tenant = a.Tenant,
					                                            DocumentTypeCode = a.DocumentTypeCode,
                                                                DocumentTypeName = a.CustomDocumentType != null ? a.CustomDocumentType.LocalName : null,
                                                                CargoTypeCode = a.CargoTypeCode,
                                                                CargoTypeName = a.CargoIdentifireType != null ? a.CargoIdentifireType.LocalName : null,
                                                                ProcessTypeCode = a.ProcessTypeCode,
                                                                ProcessTypeName = a.GovernmentProcedureType != null ? a.GovernmentProcedureType.LocalName : null,
                                                                TransportationTypeCode = a.TransportationTypeCode,
                                                                TransportationTypeName = a.CustomsTransportMode != null ? a.CustomsTransportMode.LocalName : null,
                                                                Mandatory = a.Mandatory,
                                                                Inactive = a.Inactive,
                                                                DeclarationTypeCode = a.DeclarationTypeCode,
                                                                DeclarationTypeName = a.LeadDocumentType != null ? a.LeadDocumentType.LocalName : null,

                                                            });
            return query;
		}

		private IQueryable<CustomsDocumentsDefinition> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CustomsDocumentsDefinition> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	