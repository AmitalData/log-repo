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

    public partial class CustomDocumentTypeListQueryService
    {
	    private IQueryable<CustomDocumentTypeList> GetIqueryableList(IQueryable<CustomDocumentType> iQueryable)
        {
            IQueryable<CustomDocumentTypeList> query = (from a in iQueryable.Include("Pointer")
                                                        select new CustomDocumentTypeList()
                                                        {
                                                            Code = a.Code,
                                                            EnglishName = a.EnglishName,
                                                            LocalName = a.LocalName,
                                                            SearchFields = a.SearchFields,
                                                            Inactive = a.Inactive,
                                                            PointerLevel = a.PointerLevel,
                                                            PointerLevelName = a.Pointer.LocalName != null ? a.Pointer.LocalName : null,
                                                            AutoSetOriginalDocumentTrue = a.AutoSetOriginalDocumentTrue,
                                                            IsCourierManadatory = a.IsCourierManadatory,
                                                        });

            return query;
		}

		private IQueryable<CustomDocumentType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CustomDocumentType> iQueryable)
        {
            return iQueryable;
		}
	}


}
	