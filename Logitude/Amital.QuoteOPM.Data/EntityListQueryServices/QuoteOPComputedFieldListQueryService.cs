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

using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Data.EntityLists;

namespace Amital.QuoteOPM.Data.EntityListQueryServices
{ 

    public partial class QuoteOPComputedFieldListQueryService
    {
	    private IQueryable<QuoteOPComputedFieldList> GetIqueryableList(IQueryable<QuoteOPComputedField> iQueryable)
        {
		IQueryable<QuoteOPComputedFieldList> query = (from a in iQueryable
                                            select new QuoteOPComputedFieldList()
											{
                     
					                          Id = a.Id,
					
					                          ConnectedToShipment = a.ConnectedToShipment,
					
					                          ConnectedToTicket = a.ConnectedToTicket,
					
					                          ToLocation = a.ToLocation,
					
					                          FromLocation = a.FromLocation,
					
					                          DeliveryTo = a.DeliveryTo,
					
					                          PickupFrom = a.PickupFrom,
					
					                          EstimatedPayablesInSales = a.EstimatedPayablesInSales,
					
					                          EstimatedPayablesInLocal = a.EstimatedPayablesInLocal,
					
					                          EstimatedReceivablesInLocal = a.EstimatedReceivablesInLocal,
					
					                          EstimatedReceivablesInSales = a.EstimatedReceivablesInSales,
					
		                    	            });
            return query;
		}

		private IQueryable<QuoteOPComputedField> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteOPComputedField> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<QuoteOPComputedField> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteOPComputedField> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	