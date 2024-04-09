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

    public partial class ClientItemListQueryService
    {
	    private IQueryable<ClientItemList> GetIqueryableList(IQueryable<ClientItem> iQueryable)
        {
		IQueryable<ClientItemList> query = (from a in iQueryable.Include("OriginCountry")
                                            select new ClientItemList()
											{
											  Id = a.Id,	
					                          Tenant = a.Tenant,
					                          ClientCode=a.ClientCode,
					                          ClassificationCode=a.ClassificationCode,
											  ItemDescription=a.ItemDescription,
											  ItemCode=a.ItemCode,
					                          SearchFields = a.SearchFields,
                                              OriginCountryName = a.OriginCountry.LocalName,
                                               OriginCountryCode = a.OriginCountryCode


                                            });
            return query;
		}

		private IQueryable<ClientItem> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ClientItem> iQueryable, int tenant)
        {
            return iQueryable;
        }
			}


}
	