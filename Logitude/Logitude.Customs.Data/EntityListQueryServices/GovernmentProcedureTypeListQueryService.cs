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
using Logitude.Customs.Data.Repsitories;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class GovernmentProcedureTypeListQueryService
    {
	    private IQueryable<GovernmentProcedureTypeList> GetIqueryableList(IQueryable<GovernmentProcedureType> iQueryable)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            var tenant = authToken.Tenant;

            GovernmentProcTypeTenantRepository documentTypeTenantRep = new GovernmentProcTypeTenantRepository(context);
            IQueryable<GovernmentProcTypeTenant> documentTypeTenants = documentTypeTenantRep.GetAll(tenant);


            IQueryable<GovernmentProcedureTypeList> query = (from a in iQueryable
                                                             join d in documentTypeTenants on a.Code equals d.Code into xy
                                                             from s in xy.DefaultIfEmpty()
                                                             select new GovernmentProcedureTypeList()
                                                             {
                                                                 Code = a.Code,
                                                                 EnglishName = a.EnglishName,
                                                                 LocalName = a.LocalName,
                                                                 SearchFields = a.SearchFields,
                                                                 IsImport = s != null ? s.IsImport : a.IsImport,
                                                                 Inactive = a.Inactive,
                                                                 IndexOrder = s != null ? s.IndexOrder : a.IndexOrder,
                                                                 IsExport = s != null ? s.IsExport : a.IsExport,
                                                                 ShortProcedure = a.ShortProcedure,

                                                             });
            return query;//.Where(d => !d.Code.StartsWith("1") );
		}

        private IQueryable<GovernmentProcedureType> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<GovernmentProcedureType> iQueryable)
        {
            return iQueryable;
        }
	}


}
	