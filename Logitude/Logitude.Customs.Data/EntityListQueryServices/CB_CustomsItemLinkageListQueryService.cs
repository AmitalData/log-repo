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

    public partial class CB_CustomsItemLinkageListQueryService
    {
	    private IQueryable<CB_CustomsItemLinkageList> GetIqueryableList(IQueryable<CB_CustomsItemLinkage> iQueryable)
        {
		IQueryable<CB_CustomsItemLinkageList> query = (from a in iQueryable
                                            select new CB_CustomsItemLinkageList()
											{
                     
					                          ID = a.ID,
					
					                          ChangeTypeID = a.ChangeTypeID,
					
					                          ChangeDate = a.ChangeDate,
					
					                          CustomsItemDetailsHistoryID = a.CustomsItemDetailsHistoryID,
					
					                          Connect_CustItemDetailsHistID = a.Connect_CustItemDetailsHistID,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_CustomsItemLinkage> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_CustomsItemLinkage> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	