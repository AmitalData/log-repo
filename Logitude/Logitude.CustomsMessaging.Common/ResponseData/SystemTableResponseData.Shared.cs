using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class SystemTableResponseData : INF_MSG_GenericResponseData
    {
        //public List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData> MyProperty { get; set; }
        [XmlIgnore]
        public object MySystemTableResponse { get; set; }
    }
}
