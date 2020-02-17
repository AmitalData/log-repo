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

    public partial class CargoSealIdentifierListQueryService
    {
	    private IQueryable<CargoSealIdentifierList> GetIqueryableList(IQueryable<CargoSealIdentifier> iQueryable)
        {
		IQueryable<CargoSealIdentifierList> query = (from a in iQueryable
                                            select new CargoSealIdentifierList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          DeclarationId = a.DeclarationId,
					
					                          CargoRowNumber = a.CargoRowNumber,
					
					                          ContainerNumber = a.ContainerNumber,
					
					                          UpdateDate = a.UpdateDate,
					
					                          ImporterId = a.ImporterId,
					
					                          CargoIdentifierTypeCode = a.CargoIdentifierTypeCode,
					
					                          CargoIdentifierKey1 = a.CargoIdentifierKey1,
					
					                          CargoIdentifierKey3 = a.CargoIdentifierKey3,
					
					                          Status = a.Status,
					
		                    	            });
            return query;
		}

		private IQueryable<CargoSealIdentifier> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CargoSealIdentifier> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	