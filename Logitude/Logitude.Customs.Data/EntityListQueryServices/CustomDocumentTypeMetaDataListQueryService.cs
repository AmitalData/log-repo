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

    public partial class CustomDocumentTypeMetaDataListQueryService
    {
	    private IQueryable<CustomDocumentTypeMetaDataList> GetIqueryableList(IQueryable<CustomDocumentTypeMetaData> iQueryable)
        {
            IQueryable<CustomDocumentTypeMetaDataList> query = (from a in iQueryable.Include("CustomMetaDataType").Include("CustomDocumentType")
                                                                select new CustomDocumentTypeMetaDataList()
                                                        {
                                                           DocumentTypeCode =a.DocumentTypeCode,
                                                           Format =a.Format,
                                                           Mandatory =a.Mandatory,
                                                           MetaDataTypeCode = a.MetaDataTypeCode,
                                                           ValuesTable = a.ValuesTable,
                                                           MetaDataTypeName=a.CustomMetaDataType.LocalName,
                                                           DocumentTypeName=a.CustomDocumentType.LocalName,
                                                           IsLeading=a.IsLeading,
                                                        });
            return query;
		}

		private IQueryable<CustomDocumentTypeMetaData> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CustomDocumentTypeMetaData> iQueryable)
        {
            return iQueryable;
		}
	}


}
	