using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CustomsDocumentMetaDataValueQueryService
    {
        public List<CustomsDocumentMetaDataValuePM> GetCustomDocumentMetaDataValues(string customDocumentId, int tenant)
        {
            ICustomContext context=MainContext as CustomContext;
            CustomsDocumentMetaDataValueRepository metaDataValueRepository = new CustomsDocumentMetaDataValueRepository(context);
            List<CustomsDocumentMetaDataValue> values = metaDataValueRepository.GetCustomsDocumentMetaDataValuesByCustomDocument(customDocumentId, tenant);
            List<CustomsDocumentMetaDataValuePM> valuePms = new List<CustomsDocumentMetaDataValuePM>();
            foreach (CustomsDocumentMetaDataValue value in values)
            {
                CustomsDocumentMetaDataValuePM valuepm = new CustomsDocumentMetaDataValuePM();
                mapping.CustomPOCOToPM(valuepm, value);
                mapping.POCOToPM(valuepm, value);
                valuePms.Add(valuepm);
            }

            return valuePms;
        }

        public List<CustomsDocumentMetaDataValuePM> GetCustomsDocumentMetaDataValuesByConnectedEntity(string entityId, int tenant)
        {
            ICustomContext context = MainContext as CustomContext;
            var metaDataValueRepository = new CustomsDocumentMetaDataValueRepository(context);
            var documentInRep = new DocumentsFilingRepository(tenant);
            var listDF= documentInRep.GetDocumentsFilingsByEntityId(entityId, tenant);
            if (!listDF.Any())
            {
                return new List<CustomsDocumentMetaDataValuePM>();
            }
            var DocFilingIds =listDF.Select(r => r.Id);

            List<CustomsDocumentMetaDataValue> values =
                //metaDataValueRepository.GetCustomsDocumentMetaDataValuesByEntity(entityId, tenant);
                (from mdv in metaDataValueRepository.GetAll(tenant)
                 where //mdv.CustomsDocument.DocumentsFiling.EntityId == entityId &&
                 mdv.Tenant == tenant
                 && DocFilingIds.Contains(mdv.CustomsDocumentId)
                 select mdv
                 ).ToList();




            List<CustomsDocumentMetaDataValuePM> valuePms = new List<CustomsDocumentMetaDataValuePM>();
            foreach (CustomsDocumentMetaDataValue value in values)
            {
                CustomsDocumentMetaDataValuePM valuepm = new CustomsDocumentMetaDataValuePM();
                mapping.CustomPOCOToPM(valuepm, value);
                mapping.POCOToPM(valuepm, value);
                valuePms.Add(valuepm);
            }

            return valuePms;
        }

        public List<CustomsDocumentMetaDataValuePM> GetCustomsDocumentMetaDataValuesByCustomsDocumentFilingIds(string entityId, int tenant)
        {
            ICustomContext context = MainContext as CustomContext;
            CustomsDocumentMetaDataValueRepository metaDataValueRepository = new CustomsDocumentMetaDataValueRepository(context);
            List<CustomsDocumentMetaDataValue> values = metaDataValueRepository.GetCustomsDocumentMetaDataValuesByCustomsDocumentFilingIds(entityId, tenant);
            List<CustomsDocumentMetaDataValuePM> valuePms = new List<CustomsDocumentMetaDataValuePM>();
            foreach (CustomsDocumentMetaDataValue value in values)
            {
                CustomsDocumentMetaDataValuePM valuepm = new CustomsDocumentMetaDataValuePM();
                mapping.CustomPOCOToPM(valuepm, value);
                mapping.POCOToPM(valuepm, value);
                valuePms.Add(valuepm);
            }

            return valuePms;
        }
    }
}
