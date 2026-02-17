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

    public partial class DeclarationErrorMappingListQueryService
    {
	    private IQueryable<DeclarationErrorMappingList> GetIqueryableList(IQueryable<DeclarationErrorMapping> iQueryable)
        {
            IQueryable<DeclarationErrorMappingList> query = (from a in iQueryable
                                                             select new DeclarationErrorMappingList()
                                                     {
                                                         DocumentSectionCode = a.DocumentSectionCode,
                                                         Entity = a.Entity,
                                                         Field = a.Field,
                                                         Id = a.Id,
                                                         Skip = a.Skip,
                                                         TagID = a.TagID,


                                                     });
            return query;
		}

        private IQueryable<DeclarationErrorMapping> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<DeclarationErrorMapping> iQueryable)
        {
            return iQueryable;
        }
	}


}
	