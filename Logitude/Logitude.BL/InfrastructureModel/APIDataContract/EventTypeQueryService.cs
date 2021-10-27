using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.QuoteModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;

using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.InfrastructureModel;

namespace Logitude.BL.InfrastructureModel.APIDataContract.ApiV1
{
	public partial class EventTypeQueryService
	{
		  
		public List<EventType> GetEventTypeByObjectTable(string objectTableId, int Tenant)
		{
			try
			{
				var temp = query.GetEventTypesByObjectTable(objectTableId, Tenant);
				if (temp == null)
					throw new ApplicationException("EventType with Object Table Id " + objectTableId + " doesn't exist");

				List<EventType> EventTypeList = new List<EventType>();
				foreach (var EventType in temp)
				{
					EventTypeList.Add(EventTypeDataMapping(EventType, Tenant));
				}

				return EventTypeList;
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}

	 

		 

	}
}
