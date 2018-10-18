 
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
   public partial class CustomsDocumentRepository:IRepository<CustomsDocument>
   {
        
		public List<CustomsDocument> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

       public string GetDocumentInIdByCustomsDocId(string customsDocId, int tenant)
        {
            return
                  (from rec in context.CustomsDocuments
                   where rec.CustomsDocId == customsDocId && rec.Tenant == tenant
                   select rec.DocumentsFilingId)
                  .FirstOrDefault();
        }

       public List<CustomsDocument> GetCustomsDocumentListByPointerList(List<string> pointerIdList, int tenant)
       {
           return
                 (from rec in context.CustomsDocuments
                  where pointerIdList.Contains(rec.CustomsDocId) && rec.Tenant == tenant
                  select rec)
                 .ToList();
       }
   }

}
   