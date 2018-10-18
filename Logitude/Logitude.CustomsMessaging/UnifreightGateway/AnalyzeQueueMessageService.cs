


using Logitude.CustomsMessaging.MessageAnalyzer;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.Utils;
using System.Collections;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.AmitalMessaging.Utils;

namespace Logitude.CustomsMessaging.UnifreightGateway
{
   public class AnalyzeQueueMessageService: UnifreightGatewayProxy
    {
       
        //public enum MethodsEnum
        //{
        //    //None,
        //    InsertImportDeclaration
        //}


        public AnalyzeQueueMessageService()
            :base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, // "ImportDeclarationService";
            true 
            )

        {
            //base.UniVersion = "1.000.000001";
            //base.UniDescription = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;// "ImportDeclarationService";
            //UniProduction = false;
            MessagingServiceFactoryHelper.InitContainer();
        }

   

        
        public override void ProccessRequest(
            string DataIn1,
            string DataIn2,
            out string DataOut1,
            out string DataOut2,
            out string SUCCESS,
            ref string MoreParams,
            out string MessageOut
            )
        {
            DataOut1 = DataOut2 = MessageOut = "";
            SUCCESS = false.ToString();
            try
            {


                AppendLogLine("AnalyzeQueueMessageService.ProccessRequest");
                AppendLogLine("Deserialize(DataIn1) ..");
                LogMessagingUtil.Instance.Clear();

                //DF_NG_2754_MSG10004_ImportDeclarationResponse

                var analyzeQueueMessage = new AnalyzeQueueMessage();
                analyzeQueueMessage.AnalyzeQueueCustomMessage(DataIn1,DataIn2);
                
                //var responseData = analyzeQueueMessage.MessageAnalyzerService.ResponseData  ;
                var responseData = analyzeQueueMessage.ResponseData;
                DataOut1 = XmlGenericUtil<INF_MSG_GenericResponseData>.SerializeObject(responseData);

            }
            catch (Exception e)
            {

                throw;
            }
        }

        
 
 

        public override string GetAssemblyQualifiedName()
        {
            return this.GetType().Name;
            //return this.GetType().AssemblyQualifiedName;
            //"UnifreightGatewayServer.BL.TaskYam.LogIn.TYLoginService, UnifreightGatewayServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        }

        public override string GetExampleDataIn1()
        {
         //tenant, string selectedFile   
            var h = new Hashtable();
            h["tenant"]=1;
            h["selectedFile"]="SendDF_MSG2470_ReleaseGoodsMessage_Out.IL941079089.2014-07-27_11-05-05-767.110d58a0-a5b2-4c74-9f67-1d6c3ae8f6b9.TST.xml";
            var xml=UnifreightListsUtil.Serialize(h);
            return xml; 
        }

        public override string GetExampleDataIn2()
        {
            return
@"<ns0:ESBResponse xmlns:ns0=""http://MalamTeam.Inf.ESB.Schemas.ESBResponse"">
  <ns1:ResponseHeader xmlns:ns1=""http://MalamTeam.Inf.ESB.Schemas.ResponseHeader"">
    <ns1:CorrelationId>e00b37a2-4be9-4cc0-b7db-97def841b4aa</ns1:CorrelationId>
    <ns1:Status>Success</ns1:Status>
    <ns1:ErrorDescription></ns1:ErrorDescription>
    <ns1:ErrorCode>None</ns1:ErrorCode>
    <ns1:ExternalId>110d58a0-a5b2-4c74-9f67-1d6c3ae8f6b9</ns1:ExternalId>
  </ns1:ResponseHeader>
  <Body>
    <DF_NG_2470_DF_MSG16001_ReleaseGoodsMessage xmlns=""http://malam.com/customs/DealFile/Common/DF_NG_2470_DF_MSG16001_ReleaseGoodsMessage"">
      <RequestContentHeader>
        <TransmitionDateTime xmlns=""http://malam.com/customs/EAICommon.xsd"">2014-07-27T11:04:57.5833744+03:00</TransmitionDateTime>
        <SenderID xmlns=""http://malam.com/customs/EAICommon.xsd"">0</SenderID>
        <Convertor xsi:nil=""true"" xmlns=""http://malam.com/customs/EAICommon.xsd"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" />
      </RequestContentHeader>
      <GeneralData>
        <declarationID>14020213773781</declarationID>
        <type>1</type>
        <governmentProcedureType>4000001</governmentProcedureType>
        <ReleaseMessageCode>1</ReleaseMessageCode>
        <currentDate>2014-07-27T11:04:56.397</currentDate>
        <releaseDate>2014-07-27T11:04:56.217</releaseDate>
        <isBOLNotExists>false</isBOLNotExists>
      </GeneralData>
      <Customers>
        <importerExpoterExternalID>40845497</importerExpoterExternalID>
        <importerExpoterName>אורלי כהן</importerExpoterName>
        <transportingImporterExpoterName xsi:nil=""true"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" />
        <agentExternalID>510120041</agentExternalID>
        <agentFileReferenceID>41320070</agentFileReferenceID>
      </Customers>
      <Sites>
        <deliverySitenumber>ILASH</deliverySitenumber>
        <loadingSiteNumber>INDMU</loadingSiteNumber>
        <unloadingSiteNumber>ILASH</unloadingSiteNumber>
        <storingSiteNumber xsi:nil=""true"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" />
      </Sites>
      <Consignment>
        <cargoDescription>גכ</cargoDescription>
        <cargoIdentifier>
          <cargoIdentifierType xmlns=""http://malam.com/customs/EAICommon.xsd"">11</cargoIdentifierType>
          <cargoIdentifierKey1 xmlns=""http://malam.com/customs/EAICommon.xsd"">779112</cargoIdentifierKey1>
          <cargoIdentifierKey2 xmlns=""http://malam.com/customs/EAICommon.xsd"">I063235600</cargoIdentifierKey2>
          <cargoIdentifierKey3 xsi:nil=""true"" xmlns=""http://malam.com/customs/EAICommon.xsd"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" />
        </cargoIdentifier>
        <arrivalDateTime>2014-03-20T00:00:00</arrivalDateTime>
        <AgentId xsi:nil=""true"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" />
      </Consignment>
      <PackagesInDeliverySite>
        <packageType>D5</packageType>
        <packageQuantity>1</packageQuantity>
        <packagesWeight>9800.000</packagesWeight>
        <marksAndNumbers xsi:nil=""true"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" />
      </PackagesInDeliverySite>
    </DF_NG_2470_DF_MSG16001_ReleaseGoodsMessage>
  </Body>
</ns0:ESBResponse>";
        }

        public override string GetExampleDataout1()
        {

            return "1-9"; //new entity  ...import dec.
        }
        public override string GetExampleDataout2()
        {

            return "";
        }

        public override void ProccessBASE64Request(string BASE64DataIn1, string BASE64DataIn2, string BASE64DataIn3, out string BASE64DataOut1, out string BASE64DataOut2, out string BASE64DataOut3, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }

        #region IDisposable Members

        //public   void Dispose()
        //{


        //}
        public override void Dispose()
        {
            
        }
        #endregion
    }
  
 
}
