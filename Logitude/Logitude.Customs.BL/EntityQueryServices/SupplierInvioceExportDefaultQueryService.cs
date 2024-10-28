using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Customs.Data.Repsitories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
   public partial class SupplierInvioceExportDefaultQueryService
    {
		public override void GetComposition(EntityKeyFields entityKeys, SupplierInvioceExportDefaultPM entityPM)
		{
			SupplierInvioceItemCertificatDefaultQueryService supplierInvoiceItemsPriceQueryService = new SupplierInvioceItemCertificatDefaultQueryService(context);
			SupplierInvioceExportDefaultKeys supplierInvioceExportDefaultKeys = entityKeys as SupplierInvioceExportDefaultKeys;

			entityPM.SupplierInvItemCertificatDefs = supplierInvoiceItemsPriceQueryService.GetMulti(supplierInvioceExportDefaultKeys, false);
		}

	   public SupplierInvioceExportDefault GetSupplierInvoiceExportDefaultByTenant(int tenant)
       {
            SupplierInvioceExportDefault supplierInvioceExportDefault = null;
           if (tenant != null)
           {

                SupplierInvioceExportDefaultRepository supplierInvioceExportDefaultRepository = new SupplierInvioceExportDefaultRepository(context);
                 supplierInvioceExportDefault = supplierInvioceExportDefaultRepository.GetSupplierInvoiceExportDefaultByTenant( tenant);


			}
			return supplierInvioceExportDefault;
       }

      

     
      

       
    }
}
