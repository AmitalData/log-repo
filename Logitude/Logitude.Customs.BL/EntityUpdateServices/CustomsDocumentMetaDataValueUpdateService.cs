using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CustomsDocumentMetaDataValueUpdateService 
 
    {

        protected override void OnCreating(CustomsDocumentMetaDataValuePM entityPM,CustomsDocumentPM entityParentPM)
        {
            entityPM.CustomsDocumentId = entityParentPM.DocumentsFilingId;

            base.OnCreating(entityPM, entityParentPM);
        }

    }
}
