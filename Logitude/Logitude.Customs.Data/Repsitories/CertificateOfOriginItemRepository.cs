 
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
   public partial class CertificateOfOriginItemRepository:IRepository<CertificateOfOriginItem>
   {
        
		public List<CertificateOfOriginItem> GetMulti(EntityKeyFields entityKeys)
        {

			CertificateOfOriginKeys certificateOfOriginKeys = entityKeys as CertificateOfOriginKeys;

			return (from a in context.CertificateOfOriginItems
					where a.CertificateOfOriginId == certificateOfOriginKeys.Id
					select a).ToList();
		}

   }

}
   