using Logitude.Customs.BL.EntityUpdateServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.Repsitories;

namespace Logitude.CustomsMessaging.Testers
{
    public class EcomTesterService
    {
        public static void Test()
        {
            SupplierInvoiceUpdateService.RealSetDeclarationChanged(3, "1-7779");


            var dualRepository = new DualRepository(3);
            var res1= DateTime.Now;
            //$"update  {UserId}.CFIFILEM set  LOGITUDE_FILE ='{myLOGITUDE_FILE}' where FILE_NO={fileNo}"
            var cFIFILEMRepository = new CFIFILEMRepository(3);
            var res = cFIFILEMRepository.UpdateLOGITUDE_FILE(3, 800000012, "tst-1");
        }
    }
}
