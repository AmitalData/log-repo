using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Data.DataMapping
{
    public class DocumentsFilingMetaDataValueMapping
    {
        public static void MapEntity(DocumentsFilingMetaDataValuePM entityPM, DocumentsFilingMetaDataValue entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.DocumentsFilingId = entityPM.DocumentsFilingId;
            }

            entityPOCO.DocumentsMetaDataTypeId = entityPM.DocumentsMetaDataTypeId;
            entityPOCO.MetaDataValue = entityPM.MetaDataValue;


        }
    }

}
