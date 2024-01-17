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

    public partial class CB_CustomsBookAdditionListQueryService
    {
	    private IQueryable<CB_CustomsBookAdditionList> GetIqueryableList(IQueryable<CB_CustomsBookAddition> iQueryable)
        {
		IQueryable<CB_CustomsBookAdditionList> query = (from a in iQueryable
                                            select new CB_CustomsBookAdditionList()
											{
                     
					                          ID = a.ID,
					
					                          CreateDate = a.CreateDate,
					
					                          UpdateDate = a.UpdateDate,
					
					                          TypeID = a.TypeID,
					
					                          Title = a.Title,
					
					                          CustomsBookTypeID = a.CustomsBookTypeID,
					
					                          AdditionCode = a.AdditionCode,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_CustomsBookAddition> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_CustomsBookAddition> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	