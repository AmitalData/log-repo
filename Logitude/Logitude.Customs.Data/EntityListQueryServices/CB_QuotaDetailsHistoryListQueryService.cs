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

    public partial class CB_QuotaDetailsHistoryListQueryService
    {
	    private IQueryable<CB_QuotaDetailsHistoryList> GetIqueryableList(IQueryable<CB_QuotaDetailsHistory> iQueryable)
        {
		IQueryable<CB_QuotaDetailsHistoryList> query = (from a in iQueryable
                                            select new CB_QuotaDetailsHistoryList()
											{
                     
					                          ID = a.ID,
					
					                          CreateDate = a.CreateDate,
					
					                          UpdateDate = a.UpdateDate,
					
					                          StartDate = a.StartDate,
					
					                          EndDate = a.EndDate,
					
					                          EntityStatusID = a.EntityStatusID,
					
					                          IsImportLicenseRequired = a.IsImportLicenseRequired,
					
					                          MeasurementUnitID = a.MeasurementUnitID,
					
					                          Quantity = a.Quantity,
					
					                          QuotaComputationBasisID = a.QuotaComputationBasisID,
					
					                          QuotaIncrementID = a.QuotaIncrementID,
					
					                          RenewalMethodID = a.RenewalMethodID,
					
					                          RenewalUntilDate = a.RenewalUntilDate,
					
					                          QuotaID = a.QuotaID,
					
					                          ChangeRequestTypePriority = a.ChangeRequestTypePriority,
					
					                          QuotaValueIncrement = a.QuotaValueIncrement,
					
					                          PerYearFrequency = a.PerYearFrequency,
					
					                          CurrencyTypeID = a.CurrencyTypeID,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_QuotaDetailsHistory> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_QuotaDetailsHistory> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	