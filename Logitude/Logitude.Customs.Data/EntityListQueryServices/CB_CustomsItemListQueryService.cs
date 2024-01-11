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

    public partial class CB_CustomsItemListQueryService
    {
	    private IQueryable<CB_CustomsItemList> GetIqueryableList(IQueryable<CB_CustomsItem> iQueryable)
        {
		IQueryable<CB_CustomsItemList> query = (from a in iQueryable
                                            select new CB_CustomsItemList()
											{
                     
					                          ID = a.ID,
					
					                          CreateDate = a.CreateDate,
					
					                          UpdateDate = a.UpdateDate,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_CustomsItem> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_CustomsItem> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	