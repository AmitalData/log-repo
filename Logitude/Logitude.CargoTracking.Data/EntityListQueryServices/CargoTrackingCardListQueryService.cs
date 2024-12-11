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

using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Data.EntityLists;

namespace Logitude.CargoTracking.Data.EntityListQueryServices
{ 

    public partial class CargoTrackingCardListQueryService
    {
	    private IQueryable<CargoTrackingCardList> GetIqueryableList(IQueryable<CargoTrackingCard> iQueryable)
        {
		IQueryable<CargoTrackingCardList> query = (from a in iQueryable
                                            select new CargoTrackingCardList()
											{
                     
					                          Id = a.Id,
					
					                          Code = a.Code,
					
					                          EnglishName = a.EnglishName,
					
					                          LocalName = a.LocalName,
					
		                    	            });
            return query;
		}

		private IQueryable<CargoTrackingCard> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CargoTrackingCard> iQueryable,int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<CargoTrackingCard> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<CargoTrackingCard> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	