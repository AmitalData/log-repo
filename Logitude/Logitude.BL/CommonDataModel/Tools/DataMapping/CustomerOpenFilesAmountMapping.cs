using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
   public class CustomerOpenFilesAmountMapping
    {
        public static void MapEntity(CustomerOpenFilesAmountPM customerOpenFilesAmountPM, CustomerOpenFilesAmount customerOpenFilesAmount, bool isNewState)
        {
            customerOpenFilesAmount.CustomerId = customerOpenFilesAmountPM.CustomerId;
            customerOpenFilesAmount.Tenant = customerOpenFilesAmountPM.Tenant;
            customerOpenFilesAmount.TotalOpenFilesAmount = customerOpenFilesAmountPM.TotalOpenFilesAmount;

        }
    }
}
