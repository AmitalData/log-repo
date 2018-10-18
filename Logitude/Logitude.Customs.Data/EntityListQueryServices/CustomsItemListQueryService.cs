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

    public partial class CustomsItemListQueryService
    {
	    private IQueryable<CustomsItemList> GetIqueryableList(IQueryable<CustomsItem> iQueryable)
        {
		IQueryable<CustomsItemList> query = (from a in iQueryable
                                            select new CustomsItemList()
											{
                                                ID = a.ID,
                                                FullClassification = a.FullClassification,
                                                ComputedCheckDigit = a.ComputedCheckDigit,
                                                CustomsBookTypeID = a.CustomsBookTypeID,
                                                CustomsItemCategoryID = a.CustomsItemCategoryID,
                                                CustomsItemHierarchicLocationID = a.CustomsItemHierarchicLocationID, 
		                    	            });
            return query;
		}

        private IQueryable<CustomsItem> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CustomsItem> iQueryable)
        {
            return iQueryable;
        }
	}


}
	