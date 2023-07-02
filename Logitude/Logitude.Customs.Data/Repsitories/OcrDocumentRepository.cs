 
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

namespace Logitude.Customs.Data.Repsitories
{
   public partial class OcrDocumentRepository:IRepository<OcrDocument>
   {
        
		public List<OcrDocument> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public OcrDocument GetSingleByDocId(string docId, int tenant)
        {
            return (from a in context.OcrDocuments
                    where a.DocId == docId && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

    }

}
   