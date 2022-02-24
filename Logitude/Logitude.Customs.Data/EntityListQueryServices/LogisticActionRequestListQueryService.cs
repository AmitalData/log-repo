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

    public partial class LogisticActionRequestListQueryService
    {
	    private IQueryable<LogisticActionRequestList> GetIqueryableList(IQueryable<LogisticActionRequest> iQueryable)
        {
		IQueryable<LogisticActionRequestList> query = (from a in iQueryable
                                            select new LogisticActionRequestList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          SearchFields = a.SearchFields,
					
					                          RequestDate = a.RequestDate,
					
					                          ExportFileNo = a.ExportFileNo,
					
					                          ExporterIdentifierType = a.ExporterIdentifierType,
					
					                          ExporterNumber = a.ExporterNumber,
					
					                          PassportCountry = a.PassportCountry,
					
					                          PassportNumber = a.PassportNumber,
					
					                          RequestType = a.RequestType,
					
					                          RequestReason = a.RequestReason,
					
					                          DeliverySiteID = a.DeliverySiteID,
					
					                          CargoIdentifierType = a.CargoIdentifierType,
					
					                          CargoIdentifierKey1 = a.CargoIdentifierKey1,
					
					                          CargoIdentifierKey2 = a.CargoIdentifierKey2,
					
					                          CargoIdentifierKey3 = a.CargoIdentifierKey3,
					
					                          PackagingTypeCode = a.PackagingTypeCode,
					
					                          Quantity = a.Quantity,
					
					                          RequestNumber = a.RequestNumber,
					
					                          ResponseStatusCode = a.ResponseStatusCode,
					
					                          OperationalStatus = a.OperationalStatus,
					
					                          Direction = a.Direction,
					
					                          TransportmodeId = a.TransportmodeId,
					
					                          DecisionRmarks = a.DecisionRmarks,
					
					                          CustomsUserName = a.CustomsUserName,
					
					                          IsClosed = a.IsClosed,
					
					                          DeclarationId = a.DeclarationId,
					
		                    	            });
            return query;
		}

		private IQueryable<LogisticActionRequest> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<LogisticActionRequest> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	