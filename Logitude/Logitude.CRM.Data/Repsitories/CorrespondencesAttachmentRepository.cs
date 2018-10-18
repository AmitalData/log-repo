using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class CorrespondencesAttachmentRepository:IRepository<CorrespondencesAttachment>
   {
        
		public List<CorrespondencesAttachment> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public List<CorrespondencesAttachment> GetAllCorrespondencesAttachmentByListOfIds(List<string> ids, int tenant)
        {
            List<CorrespondencesAttachment> query = (from a in context.CorrespondencesAttachments.Include("DocumentsFiling").Include("Correspondence")
                                                       where a.Tenant == tenant && ids.Contains(a.CorrespondenceId)
                                                       select a).ToList();
            return query;
        }

        public List<CorrespondencesAttachment> GetAllCorrespondencesAttachmentByCorrespondenceIdAndTenant(string id, int tenant)
        {
            List<CorrespondencesAttachment> query = (from a in context.CorrespondencesAttachments.Include("DocumentsFiling").Include("Correspondence")
                                                     where a.Tenant == tenant && a.CorrespondenceId == id
                                                     select a).ToList();
            return query;
        }
   }
}
   