 
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
   public partial class SupplierInvioceItemCertificatDefaultRepository:IRepository<SupplierInvioceItemCertificatDefault>
   {
        
		public List<SupplierInvioceItemCertificatDefault> GetMulti(EntityKeyFields entityKeys)
        {
			SupplierInvioceExportDefaultKeys supplierInvioceExportDefaultKeys = entityKeys as SupplierInvioceExportDefaultKeys;

			return (from a in context.SupplierInvioceItemCertificatDefaults
					where a.SupplierInvioceExportDefaultId == supplierInvioceExportDefaultKeys.Id
					select a).ToList();
		}
		public void FastDeleteMulti(SupplierInvioceExportDefaultKeys entityKeyFields)
		{
			(context as DbContextBase)
				.DeleteWhere<SupplierInvioceItemCertificatDefault>(rec => rec.SupplierInvioceExportDefaultId == entityKeyFields.Id);
		}

	}

}
   