using System;
using System.Collections.Generic;
using System.Linq;


namespace Logitude.CustomsMessaging.ResponseData
{
    public class CH_NG_192_MSG3_ApproveChangeTimeResponseData : ResponseDataBase
    {
        public CH_NG_192_MSG3_ApproveChangeTimeResponseData()
        {
            PiscalCheckItems = new List<DateTime?>();
            XrayItems = new List<DateTime?>();
        }
        List<DateTime?> _XrayItems;
        List<DateTime?> _PiscalCheckItems;
        public List<DateTime?> PiscalCheckItems
        {
            get { return _PiscalCheckItems; }
            set
            {
                _PiscalCheckItems = value;
            }
        }

        public List<DateTime?> XrayItems
        {
            get { return _XrayItems; }
            set
            {
                _XrayItems = value;
            }
        }

       
    }
}