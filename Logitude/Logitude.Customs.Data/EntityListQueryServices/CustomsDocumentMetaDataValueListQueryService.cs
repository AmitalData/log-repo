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

    public partial class CustomsDocumentMetaDataValueListQueryService
    {
	    private IQueryable<CustomsDocumentMetaDataValueList> GetIqueryableList(IQueryable<CustomsDocumentMetaDataValue> iQueryable)
        {
            IQueryable<CustomsDocumentMetaDataValueList> query = (from a in iQueryable
                                                                  select new CustomsDocumentMetaDataValueList()
                                                       {
                                                         CustomsDocumentId = a.CustomsDocumentId,
                                                         MetaDataTypeCode = a.MetaDataTypeCode,
                                                         MetaDataValue = a.MetaDataValue,
                                                         Tenant = a.Tenant,
                                                         


                                                       });
            return query;
		}

        private IQueryable<CustomsDocumentMetaDataValue> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CustomsDocumentMetaDataValue> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	