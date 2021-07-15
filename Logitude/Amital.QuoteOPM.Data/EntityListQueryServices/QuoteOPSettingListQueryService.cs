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

    public partial class QuoteOPSettingListQueryService
    {
	    private IQueryable<QuoteOPSettingList> GetIqueryableList(IQueryable<QuoteOPSetting> iQueryable)
        {
		IQueryable<QuoteOPSettingList> query = (from a in iQueryable
                                            select new QuoteOPSettingList()
											{
                     
		                    	            });
            return query;
		}

		private IQueryable<QuoteOPSetting> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteOPSetting> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<QuoteOPSetting> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteOPSetting> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	