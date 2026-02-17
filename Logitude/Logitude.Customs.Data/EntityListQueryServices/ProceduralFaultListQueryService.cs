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

    public partial class ProceduralFaultListQueryService
    {
	    private IQueryable<ProceduralFaultList> GetIqueryableList(IQueryable<ProceduralFault> iQueryable)
        {
		IQueryable<ProceduralFaultList> query = (from a in iQueryable
                                            select new ProceduralFaultList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          ProceduralFaultNumber = a.ProceduralFaultNumber,
					
					                          ProceduralFaultStatusCode = a.ProceduralFaultStatusCode,
					
					                          CreateDate = a.CreateDate,
					
					                          InputTypeCode = a.InputTypeCode,
					
					                          InspectionTypeCode = a.InspectionTypeCode,
					
					                          ProceduralFaultCode = a.ProceduralFaultCode,
					
					                          ProceduralFaultInputProcesCode = a.ProceduralFaultInputProcesCode,
					
					                          RansomViolationTypeCode = a.RansomViolationTypeCode,
					
					                          RansomViolationSum = a.RansomViolationSum,
					
					                          Remarks = a.Remarks,
					
					                          IsCustomerResponsibility = a.IsCustomerResponsibility,
					
					                          IsAgentProceduralFaultCountabl = a.IsAgentProceduralFaultCountabl,
					
					                          IsCustProceduralFaultCountabl = a.IsCustProceduralFaultCountabl,
					
					                          IsAgentResponsibility = a.IsAgentResponsibility,
					
					                          UpdateDate = a.UpdateDate,
					
					                          LeadingDocumentVersion = a.LeadingDocumentVersion,
					
					                          Notes = a.Notes,
					
					                          IsCancelled = a.IsCancelled,
					
					                          CancellationDate = a.CancellationDate,
					
					                          SearchFields = a.SearchFields,
					
					                          DeclarationId = a.DeclarationId,
					
		                    	            });
            return query;
		}

		private IQueryable<ProceduralFault> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ProceduralFault> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	