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

    public partial class TariffLinesContainersPriceListQueryService
    {
	    private IQueryable<TariffLinesContainersPriceList> GetIqueryableList(IQueryable<TariffLinesContainersPrice> iQueryable)
        {
		IQueryable<TariffLinesContainersPriceList> query = (from a in iQueryable
                                            select new TariffLinesContainersPriceList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          TariffId = a.TariffId,
					
					                          TariffLineId = a.TariffLineId,
					
					                          SurchargeId = a.SurchargeId,
					
					                          Price1 = a.Price1,
					
					                          Price2 = a.Price2,
					
					                          Price3 = a.Price3,
					
					                          Price4 = a.Price4,
					
					                          Price5 = a.Price5,
					
		                    	            });
            return query;
		}

		private IQueryable<TariffLinesContainersPrice> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TariffLinesContainersPrice> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<TariffLinesContainersPrice> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TariffLinesContainersPrice> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	