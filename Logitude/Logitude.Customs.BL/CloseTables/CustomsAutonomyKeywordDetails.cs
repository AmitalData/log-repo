using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.CloseTables
{
    class CustomsAutonomyKeywordDetails
    {
        public List<InterfaceDetails> GetAllCustomsAutonomyKeywords()
        {
            var all = new List<InterfaceDetails>() {
            //all.Add(new KeyValuePair<string, string>("", ""));
             
             new InterfaceDetails()
            {
                Code = "1",
                Name = "עיר",
                TypeCode = "1",
                Partner = "",
                ViaMethod = ""
            }
,
            new InterfaceDetails()
            {
                Code = "2",
                Name = "טלפון",
                TypeCode = "2",
                Partner = "",
                ViaMethod = ""
            }
        
            };
            ///

            //all.Add(new KeyValuePair<string, string>("TST", "Test"));
            return all;
        }
    }

}
