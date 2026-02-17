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

    public partial class InternationalSiteListQueryService
    {
	    private IQueryable<InternationalSiteList> GetIqueryableList(IQueryable<InternationalSite> iQueryable)
        {
            IQueryable<InternationalSiteList> query = (from a in iQueryable.Include("CustomsCountry")
                                                       //join d in context.CustomsCountries
                                                       // on a.Code equals d.Code
                                                       select new InternationalSiteList()
                                                       {
                                                           Code = a.Code,
                                                           EnglishName = a.EnglishName,
                                                           LocalName = a.LocalName,
                                                           SearchFields = a.SearchFields,
                                                           CountryTypeCode = a.CountryTypeCode,
                                                           CountryTypeName = a.CustomsCountry != null ? a.CustomsCountry.LocalName : null,
                                                           Inactive = a.Inactive
                                                       });


            return query;
		}

        private IQueryable<InternationalSite> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<InternationalSite> iQueryable)
        {
            return iQueryable;
        }
	}


}
	