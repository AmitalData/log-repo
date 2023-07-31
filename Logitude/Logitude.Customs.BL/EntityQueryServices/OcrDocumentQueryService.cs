using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class OcrDocumentQueryService
    {
             
        public OcrDocument GetOcrDocumentByDocumentFilingId(string documentFilingId, int tenant)
        {

            OcrDocumentRepository ocrDocumentRepository = new OcrDocumentRepository(tenant);

            return ocrDocumentRepository.GetSingleByDocId(documentFilingId, tenant);
        }

     



       


     


       
    }
}
