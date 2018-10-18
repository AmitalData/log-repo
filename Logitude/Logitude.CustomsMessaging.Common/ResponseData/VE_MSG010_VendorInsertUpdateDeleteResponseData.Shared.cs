using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class VE_MSG010_VendorInsertUpdateDeleteResponseData : ResponseDataBase
    {
        public string ApplicationID { get; set; }
        public int ExeptionType { get; set; }
        public bool IsCustomWarning { get; set; }
    }
}
