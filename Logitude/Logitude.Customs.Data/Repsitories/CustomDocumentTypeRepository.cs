 
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
   public partial class CustomDocumentTypeRepository:IRepository<CustomDocumentType>
   {
        
		public List<CustomDocumentType> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
		public CustomDocumentType GetSingleCustomDocumentType(EntityKeyFields entityKeys)
		{
			CustomDocumentTypeKeys keys = entityKeys as CustomDocumentTypeKeys;
			return (from a in context.CustomDocumentTypes
					where a.Code == keys.Code
					select a).FirstOrDefault();
		}

		public List<CustomDocumentType> GetCustomDocumentTypesIsCourierManadatoryByTenant(int tenant)
		{
			var CustomDocumentTypeData =
				(from a in context.CustomDocumentTypes
				 join c in context.CustomDocumentTypeTenants
					 on a.Code equals c.Code into tenantGroup
				 from c in tenantGroup.DefaultIfEmpty()
				 where c == null || c.Tenant == tenant
				 select new
				 {
					 Code = a.Code,
					 IsCourierManadatory = c != null ? c.IsCourierManadatory : a.IsCourierManadatory
				 })
				 .ToList(); 

			return CustomDocumentTypeData
				.Select(x => new CustomDocumentType
				{
					Code = x.Code,
					IsCourierManadatory = x.IsCourierManadatory
				})
				.ToList();
		}

	}

}
   