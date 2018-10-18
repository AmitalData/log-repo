
                                            //Yuval Chalup 29.10.2015 TASK-16002
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class CourierBOLQueryResponseData : ResponseDataBase
    {
        public List<CourierBOLDetailsResult> CourierBOLDetailsList { get; set; }
        public string ResponseStatusXML { get; set; }

        public class CourierBOLDetailsResult
        {
            public string cargoIdentifierKey1 { get; set; }
            public string cargoIdentifierKey2 { get; set; }
            public string cargoIdentifierKey3 { get; set; }
            public int cargoIdentifierType { get; set; }
        }
    }
}
