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
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class CustomDocumentTypeListQueryService
    {
	    private IQueryable<CustomDocumentTypeList> GetIqueryableList(IQueryable<CustomDocumentType> iQueryable)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            var tenant = authToken.Tenant;

            CustomDocumentTypeTenantRepository documentTypeTenantRep = new CustomDocumentTypeTenantRepository(context);
            IQueryable<CustomDocumentTypeTenant> documentTypeTenants = documentTypeTenantRep.GetAll(tenant);

            IQueryable<CustomDocumentTypeList> query = (from a in iQueryable
                                                        join d in documentTypeTenants.Include("Pointer").Include("CustomsDocumentUploadT")
                                                         on a.Code equals d.Code into xy
                                                        from s in xy.DefaultIfEmpty()
                                                        select new CustomDocumentTypeList()
                                                        {
                                                            Code = a.Code,
                                                            EnglishName = a.EnglishName,
                                                            LocalName = a.LocalName,
                                                            SearchFields = a.SearchFields,
                                                            Inactive = a.Inactive,
                                                            PointerLevel = s != null ? s.PointerLevel : null,
                                                            PointerLevelName = s != null ?  (s.Pointer.LocalName != null ? s.Pointer.LocalName : null) : null,
                                                            AutoSetOriginalDocumentTrue = s != null ? s.AutoSetOriginalDocumentTrue : false,
                                                            IsCourierManadatory= s != null ? s.IsCourierManadatory : false,
                                                            IsDiamondManadatory= s != null ? s.IsDiamondManadatory : false,
                                                            CustomsDocumentUpload = s != null ? s.CustomsDocumentUpload : null,
                                                            CustomsDocumentUploadName = s != null ? (s.CustomsDocumentUploadT.LocalName != null ? s.CustomsDocumentUploadT.LocalName : null) : null,
                                                        });

            return query;
		}

		private IQueryable<CustomDocumentType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CustomDocumentType> iQueryable)
        {
            return iQueryable;
		}
	}


}
	