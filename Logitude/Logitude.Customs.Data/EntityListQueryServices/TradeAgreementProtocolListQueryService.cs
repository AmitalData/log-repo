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

    public partial class TradeAgreementProtocolListQueryService
    {
	    private IQueryable<TradeAgreementProtocolList> GetIqueryableList(IQueryable<TradeAgreementProtocol> iQueryable)
        {
		IQueryable<TradeAgreementProtocolList> query = (from a in iQueryable
                                            select new TradeAgreementProtocolList()
											{
                     
					                          Code = a.Code,
					
					                          LocalName = a.LocalName,
					
					                          SearchFields = a.SearchFields,
											 
											  EnglishName = a.EnglishName  , 
											  InActive = a.InActive
					
		                    	            });
            return query;
		}

		private IQueryable<TradeAgreementProtocol> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TradeAgreementProtocol> iQueryable)
        {
			return iQueryable;
		}
			}


}
	