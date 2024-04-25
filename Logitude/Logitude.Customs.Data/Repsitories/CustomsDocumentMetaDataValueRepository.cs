 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Customs.Data.Repsitories
{
    public partial class CustomsDocumentMetaDataValueRepository : IRepository<CustomsDocumentMetaDataValue>
    {

        public List<CustomsDocumentMetaDataValue> GetMulti(EntityKeyFields entityKeys)
        {

            CustomsDocumentKeys keys = entityKeys as CustomsDocumentKeys;
            List<CustomsDocumentMetaDataValue> values = (from a in context.CustomsDocumentMetaDataValues
                                                         where a.CustomsDocumentId == keys.DocumentsFilingId
                                                         select a).ToList();
            return values;


        }

        public List<CustomsDocumentMetaDataValue> GetCustomsDocumentMetaDataValuesByCustomDocument(string customDocumentId, int tenant)
        {
            List<CustomsDocumentMetaDataValue> values = (from a in context.CustomsDocumentMetaDataValues
                                                         where a.CustomsDocumentId == customDocumentId && a.Tenant == tenant
                                                         select a).ToList();
            return values;
        }

        public CustomsDocumentMetaDataValue GetCustomsDocumentMetaDataValuesByCustomDocumentAndMetaDateValue(string customDocumentId, int tenant,string MetaDataTypeCode)
        {
            CustomsDocumentMetaDataValue value = (from a in context.CustomsDocumentMetaDataValues
                                                         where a.CustomsDocumentId == customDocumentId && a.Tenant == tenant && a.MetaDataTypeCode== MetaDataTypeCode
                                                         select a).FirstOrDefault();
            return value;
        }

        public List<CustomsDocumentMetaDataValue> GetCustomsDocumentMetaDataValuesByEntity_BADBAD(string entityId, int tenant)
        {
            List<CustomsDocumentMetaDataValue> values = (from a in context.CustomsDocumentMetaDataValues.Include("CustomsDocument.DocumentsFiling")
                                                         where a.CustomsDocument.DocumentsFiling.EntityId == entityId && a.Tenant == tenant
                                                         select a).ToList();
            return values;
        }

        public List<CustomsDocumentMetaDataValue> GetCustomsDocumentMetaDataValuesByCustomsDocumentFilingIds(string customsDocumentFilingIds, int tenant)
        {
            if (customsDocumentFilingIds != null)
            {
                string[] ids = customsDocumentFilingIds.Split(',');

                List<CustomsDocumentMetaDataValue> values = (from a in context.CustomsDocumentMetaDataValues
                                                             where ids.Contains(a.CustomsDocumentId) && a.Tenant == tenant
                                                             select a).ToList();

                return values;
            }
            else
            {
                return null;
            }

        }

    }

}
   