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

    public partial class PartyRelationshipTypeListQueryService
    {
	    private IQueryable<PartyRelationshipTypeList> GetIqueryableList(IQueryable<PartyRelationshipType> iQueryable)
        {
		IQueryable<PartyRelationshipTypeList> query = (from a in iQueryable
                                            select new PartyRelationshipTypeList()
											{
                     Code= a.Code,
                     LocalName= a.LocalName,
                     EnglishName= a.EnglishName,
					                          SearchFields = a.SearchFields,
					
					                          Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<PartyRelationshipType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<PartyRelationshipType> iQueryable)
        {
            return iQueryable;
        }
			}


}
	