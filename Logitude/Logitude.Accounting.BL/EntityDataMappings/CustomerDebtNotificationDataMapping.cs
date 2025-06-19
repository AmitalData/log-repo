
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class CustomerDebtNotificationDataMapping: IMapping<CustomerDebtNotificationPM, CustomerDebtNotification>
   {

        public void CustomPMToPOCO(CustomerDebtNotificationPM entityPM, CustomerDebtNotification entityPOCO)
        {
			CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);


			if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
			{
				entityPOCO.Id = entityPM.Id;

			}
		}

        public void CustomPOCOToPM(CustomerDebtNotificationPM entityPM, CustomerDebtNotification entityPOCO)
        {
        }
   }


}
   