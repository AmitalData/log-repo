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

    public partial class ClientsAddressCommTypeListQueryService
    {
	    private IQueryable<ClientsAddressCommTypeList> GetIqueryableList(IQueryable<ClientsAddressCommType> iQueryable)
        {
            IQueryable<ClientsAddressCommTypeList> query = (from a in iQueryable
                                                                     select new ClientsAddressCommTypeList()
                                          {
                                             AddressId = a.AddressId ,
                                             ClientId = a.ClientId,
                                             CommunicationAddress = a.CommunicationAddress,
                                             CommunicationTypeCode = a.CommunicationTypeCode,
                                             Line = a.Line,
                                             Tenant = a.Tenant,
                                            
                                             

                                          });
            return query;
		}

        private IQueryable<ClientsAddressCommType> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ClientsAddressCommType> iQueryable,int tenant)
        {
            return iQueryable;
        }
	}


}
	