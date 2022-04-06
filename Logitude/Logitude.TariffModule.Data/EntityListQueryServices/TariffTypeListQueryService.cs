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

using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.EntityLists;

namespace Logitude.TariffModule.Data.EntityListQueryServices
{ 

    public partial class TariffTypeListQueryService
    {
	    private IQueryable<TariffTypeList> GetIqueryableList(IQueryable<TariffType> iQueryable)
        {
		IQueryable<TariffTypeList> query = (from a in iQueryable
                                            select new TariffTypeList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
											  TransportModeCode = a.TransportModeCode,
											  DirectionCode = a.DirectionCode
					
		                    	            });
            return query;
		}

		private IQueryable<TariffType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TariffType> iQueryable)
        {
			throw new NotImplementedException();
		}
				private IQueryable<TariffType> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TariffType> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	