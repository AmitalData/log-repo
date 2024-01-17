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

    public partial class CB_VendorListQueryService
    {
	    private IQueryable<CB_VendorList> GetIqueryableList(IQueryable<CB_Vendor> iQueryable)
        {
		IQueryable<CB_VendorList> query = (from a in iQueryable
                                            select new CB_VendorList()
											{
                     
					                          ID = a.ID,
					
					                          Title = a.Title,
					
					                          State = a.State,
					
					                          EnglishCountryName = a.EnglishCountryName,
					
					                          VendorSingleStringAddress = a.VendorSingleStringAddress,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_Vendor> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_Vendor> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	