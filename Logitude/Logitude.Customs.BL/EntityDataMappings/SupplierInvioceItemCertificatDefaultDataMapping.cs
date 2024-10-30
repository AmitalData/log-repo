
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class SupplierInvioceItemCertificatDefaultDataMapping: IMapping<SupplierInvioceItemCertificatDefaultPM, SupplierInvioceItemCertificatDefault>
   {

        public void CustomPMToPOCO(SupplierInvioceItemCertificatDefaultPM entityPM, SupplierInvioceItemCertificatDefault entityPOCO)
        {
			CustomMappedPOCOProperties.Add(POCOPropertyNames.SupplierInvioceExportDefaultId);
			CustomMappedPOCOProperties.Add(POCOPropertyNames.SequenceNumeric);			
			if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
			{

				entityPOCO.SupplierInvioceExportDefaultId = entityPM.SupplierInvioceExportDefaultId;
				entityPOCO.SequenceNumeric = entityPM.SequenceNumeric;
			
			}
		}

        public void CustomPOCOToPM(SupplierInvioceItemCertificatDefaultPM entityPM, SupplierInvioceItemCertificatDefault entityPOCO)
        {
			//throw new NotImplementedException();
		}
	}


}
   