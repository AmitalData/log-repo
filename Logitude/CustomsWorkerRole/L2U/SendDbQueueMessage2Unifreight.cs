

using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Amital;
using Logitude.Server.Tools.QueueService;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.UServer;

namespace CustomsWorkerRole.L2U
{
    class SendDbQueueMessage2Unifreight : ReceivedDbMessageAction
    {
        

        public SendDbQueueMessage2Unifreight(CustomDBQueueMessage receivedCustomDBQueueResponse)
            : base(receivedCustomDBQueueResponse) 
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
