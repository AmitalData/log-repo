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

    public partial class CB_TariffComputedDataListQueryService
    {
	    private IQueryable<CB_TariffComputedDataList> GetIqueryableList(IQueryable<CB_TariffComputedData> iQueryable)
        {
		IQueryable<CB_TariffComputedDataList> query = (from a in iQueryable
                                            select new CB_TariffComputedDataList()
											{
                     
					                          CB_ID = a.CB_ID,
					
					                          ID = a.ID,
					
					                          TariffID = a.TariffID,
					
					                          TDH_IDNum = a.TDH_IDNum,
					
					                          StartDate = a.StartDate,
					
					                          EndDate = a.EndDate,
					
					                          WithoutQuota_ComputationID = a.WithoutQuota_ComputationID,
					
					                          WithinQuota_ComputationID = a.WithinQuota_ComputationID,
					
					                          CustomsItemIDNum = a.CustomsItemIDNum,
					
					                          TradeAgreementID = a.TradeAgreementID,
					
					                          QuotaID = a.QuotaID,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_TariffComputedData> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_TariffComputedData> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	