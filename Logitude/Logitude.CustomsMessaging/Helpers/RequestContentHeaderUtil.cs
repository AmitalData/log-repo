using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.CommonIIGInterface;

namespace Logitude.CustomsMessaging.Helpers
{
    public class RequestContentHeaderUtil
    {
        private static void GetDefault(IRequestContentHeader myRequestContentHeader)
        {
            myRequestContentHeader.TransmitionDateTime = DateTime.Now; 
            myRequestContentHeader.SenderID = 1;
            myRequestContentHeader.RecieverID = new int[] { 1 };
            
        }
        
    }
}
