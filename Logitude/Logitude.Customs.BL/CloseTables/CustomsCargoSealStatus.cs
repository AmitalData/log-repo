using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.CloseTables
{
    class CustomsCargoSealStatus
    {
        public List<InterfaceDetails> GetAllCustomsCargoSealStatus()
        {
            var all = new List<InterfaceDetails>() { 
 
            new InterfaceDetails()
            {
                Code = "1",
                Name = "תקין",
                TypeCode = "1",
                Partner = "",
                ViaMethod = ""
            }
          ,
             new InterfaceDetails()
            {
                Code = "2",
                Name = "שגוי",
                TypeCode = "2",
                Partner = "",
                ViaMethod = ""
            }

            };
            ///

             return all;
        }
    }

}
