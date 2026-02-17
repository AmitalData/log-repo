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

    public partial class VendorCommunicationListQueryService
    {
	    private IQueryable<VendorCommunicationList> GetIqueryableList(IQueryable<VendorCommunication> iQueryable)
        {
            IQueryable<VendorCommunicationList> query = (from a in iQueryable
                                                         select new VendorCommunicationList()
                                                {
                                                  
                                                    SearchFields = a.SearchFields,
                                                    CommunicationAddress = a.CommunicationAddress,
                                                    CommunicationTypeCode= a.CommunicationTypeCode,
                                                    LineNumber = a.LineNumber,
                                                    Tenant= a.Tenant,
                                                    VendorId = a.VendorId,
                                                    



                                                });
            return query;
		}

        private IQueryable<VendorCommunication> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<VendorCommunication> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	