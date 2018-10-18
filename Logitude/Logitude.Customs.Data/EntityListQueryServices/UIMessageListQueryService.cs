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

    public partial class UIMessageListQueryService
    {
	    private IQueryable<UIMessageList> GetIqueryableList(IQueryable<UIMessage> iQueryable)
        {
		IQueryable<UIMessageList> query = (from a in iQueryable
                                           join d in context.UIMessageAdditionals
                                           on a.Code equals d.Code
                                           select new UIMessageList()
											{
                                                Code  = a.Code,
					                            EnglishName = a.EnglishName,
                                                LocalName = a.LocalName,
                                                SearchFields = a.SearchFields,
					                            Inactive = a.Inactive,
                                                Sort = d.Sort,
		                    	            });
            return query;
		}

		private IQueryable<UIMessage> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<UIMessage> iQueryable)
        {
            return iQueryable;
        }
	}


}
	