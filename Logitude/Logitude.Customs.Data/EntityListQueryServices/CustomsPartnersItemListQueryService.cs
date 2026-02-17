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
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class CustomsPartnersItemListQueryService
    {
	    private IQueryable<CustomsPartnersItemList> GetIqueryableList(IQueryable<CustomsPartnersItem> iQueryable)
        {
            IQueryable<CustomsPartnersItemList> query = (from a in iQueryable.Include("Client").Include("CustomsVendor")
                                                         select new CustomsPartnersItemList()
                                                                     {
                                                                        Id = a.Id,
                                                                        ClassificationCode = a.ClassificationCode,
                                                                        CustomerId = a.CustomerId,
                                                                        ItemCode = a.ItemCode,
                                                                        Name = a.Name,
                                                                        SearchFields = a.SearchFields,
                                                                        VendorId = a.VendorId,
                                                                        CustomerName = a.Client != null? a.Client.FullName: null,
                                                                        VendorName = a.CustomsVendor != null? a.CustomsVendor.VendorName : null,
                                                                         Tenant = a.Tenant,



                                                                     });
            return query;
		}

        private IQueryable<CustomsPartnersItem> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CustomsPartnersItem> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	