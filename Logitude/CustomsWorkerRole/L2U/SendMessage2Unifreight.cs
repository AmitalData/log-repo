
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Amital;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.UServer;

namespace CustomsWorkerRole.L2U
{
    class SendMessage2Unifreight : ReceivedBMessageAction
    {
        public SendMessage2Unifreight(BrokeredMessage receivedBrokeredMessage)
            : base(receivedBrokeredMessage) 
        {

                }

        public override bool DoAction(string urouterParams)
                {
            //implement the code to send the xml file to amital;

            string P_MOREPARAMS = "";
            string P_XML_DATA = "";
            string P_MESSAGE = "";
            if (String.IsNullOrWhiteSpace(urouterParams))
            {

                throw new ArgumentNullException("SendFileToAmitalService():xmlfile is null");
            }
            string uniTester = "";
            P_XML_DATA = SendMessageToUServerUtil.SendMessageToUServer(_Tenant, urouterParams, out P_MESSAGE, out uniTester);
#if false
		  

            var setting = CustomsSettingQueryService.GetSettingByTenant(_Tenant);
            if (setting==null)
            {
                throw new Exception("no definition !!  CustomsSettingQueryService.GetSettingByTenant " + _Tenant ); 
            }
            if (string.IsNullOrWhiteSpace(setting.UServerServiceAddress ))
            {
                throw new Exception("no UServerServiceAddress definition !!  CustomsSettingQueryService.GetSettingByTenant " + _Tenant ); 
            }


            var myUServerDNS = "UNIV55";// setting.UServerDNS;
            var myUServerPort = "8055";// setting.UServerPort;
            //http://univ55:8055/
            var myUServerUtil = new UServerUtil(//myUServerDNS, myUServerPort); ;
                setting.UServerServiceAddress ,setting.


            myUServerUtil.DoIt(urouterParams, ref P_MOREPARAMS, out P_XML_DATA, out P_MESSAGE);
#endif
            //_WaitingCommLog.Logs += "UServer did not return response ";
            _WaitingCommLog.Logs += P_XML_DATA + Environment.NewLine;
            _WaitingCommLog.Logs += uniTester;
            if (String.IsNullOrWhiteSpace(P_XML_DATA))
            {
                _WaitingCommLog.Logs += "UServer did not return response ";
                return false;    
            }
            else
            {
                //todo: Create New commincation/ANALYZQUEUE ?? 
            }

            
            // var UNIQUE_ENVIRONMENT_ID = UnifaceAssociativeListUtil.GetValue(P_XML_DATA, "UNIQUE_ENVIRONMENT_ID");

            return true;
        }
    }
}
