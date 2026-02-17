using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.CRM.Data.Helpers;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class CorrespondenceRepository:IRepository<Correspondence>
   {
        
		public List<Correspondence> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }


        public List<InboundLineContactClass> GetCorrespondenceContacts(List<string> ids)
        {
            List<InboundLineContactClass> myResult = new List<InboundLineContactClass>();

            if (ids.Count > 0)
            {
                myResult = (from d in context.Correspondences.Include("CreatedByContact")
                            where ids.Contains(d.Id)
                            select new InboundLineContactClass
                            {
                                Id = d.Id,
                                Name = d.CreatedByContact == null ? "" : d.CreatedByContact.EnglishName,
                                IsInternal = d.IsInternal ,
                                RightToLeft = d.RightToLeft,
                            }).ToList();
            }

            return myResult;
        }

        public List<Correspondence> GetCorrespondenceByEntityId(string entityId, int tenant)
        {
           return (from d in context.Correspondences
                        where d.EntityId == entityId && d.Tenant == tenant
                        select d).ToList();
        }
   }
}
   