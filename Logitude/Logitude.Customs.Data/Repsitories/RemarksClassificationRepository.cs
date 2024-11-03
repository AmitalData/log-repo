 
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
using Logitude.Customs.Data.EntityMapping;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class RemarksClassificationRepository:IRepository<RemarksClassification>
   {
        
		public List<RemarksClassification> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public List<RemarksClassificationList> GetAllCommentsByCustomsItemId(int customsItemId, int tenant)
        {

            List<RemarksClassification> RemarksClassificationPocoList = (from a in context.RemarksClassifications
                    where a.CustomsItemsID == customsItemId && a.Tenant == tenant
                    select a).ToList();

            // convert the list of POCO to list of List
            List<RemarksClassificationList> RemarksClassificationList = new List<RemarksClassificationList>();
            if (RemarksClassificationPocoList?.Count() <= 0) return RemarksClassificationList;
            
            foreach (RemarksClassification RemarksClassificationPoco in RemarksClassificationPocoList)
            {
                RemarksClassificationList.Add(new RemarksClassificationList
                {
                    Id = RemarksClassificationPoco.Id,
                    Tenant = RemarksClassificationPoco.Tenant,
                    CustomsItemsID = RemarksClassificationPoco.CustomsItemsID,
                    RemarkDescription = RemarksClassificationPoco.RemarkDescription
                });
            }
            return RemarksClassificationList;
        }

   }

}
   