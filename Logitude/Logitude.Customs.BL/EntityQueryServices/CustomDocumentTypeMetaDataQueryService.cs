using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CustomDocumentTypeMetaDataQueryService
    {
        public List<CustomDocumentTypeMetaDataPM> GetCustomDocumentTypeMetaDataByType(string customDocumentTypeCode)
        {
            string entityKeyString = $"GetCustomDocumentTypeMetaDataByType({customDocumentTypeCode})";
            var res = CacheManager.GetOrInsertNewObject<List<CustomDocumentTypeMetaDataPM>>(entityKeyString, () =>
            {
                ICustomContext context = MainContext as CustomContext;
                List<CustomDocumentTypeMetaDataPM> metaData = (from a in context.CustomDocumentTypeMetaData.Include("CustomDocumentType").Include("CustomMetaDataType")
                                                               where a.DocumentTypeCode == customDocumentTypeCode
                                                               select new CustomDocumentTypeMetaDataPM()
                                                               {
                                                                   DocumentTypeCode = a.DocumentTypeCode,
                                                                   Format = a.Format,
                                                                   Mandatory = a.Mandatory,
                                                                   MetaDataTypeCode = a.MetaDataTypeCode,
                                                                   ValuesTable = a.ValuesTable,
                                                                   DocumentTypeName = a.CustomDocumentType != null ? a.CustomDocumentType.LocalName : null,
                                                                   MetaDataTypeName = a.CustomMetaDataType != null ? a.CustomMetaDataType.LocalName : null,
                                                                   IsLeading = a.IsLeading,
                                                                   Inactive = a.Inactive
                                                               }).ToList();

                return metaData;
            });
            return res;


        }
    }
}
