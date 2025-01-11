using AmitalCloud.Infrastructure.Data.Helpers;
using Logitude.AmitalMessaging.Utils;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using System;
using System.Collections.Generic;
using AmitalCloud.Infrastructure.APITools.DataContracts;

namespace Logitude.CustomsMessaging.Testers
{

    //Logitude.CustomsMessaging.Testers.PaymentTestService
    public class PaymentTestService : UnifreightGatewayProxy
    {


        public PaymentTestService()
            : base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, // "ImportDeclarationService";
            true
            )
        { }




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
                AppendLogLine("ImportDeclarationService.ProccessRequest");
                AppendLogLine("Deserialize(DataIn1) ..");



                var hDataIn1 = UnifreightListsUtil.Deserialize(DataIn1);
                AppendLogLine("DataIn1=" + hDataIn1.Count.ToString());
                AppendLogLine("Deserialize(DataIn2) ..");


                string stenant = UnifreightListsUtil.GetValue(ref hDataIn1, "Tenant");
                if (String.IsNullOrWhiteSpace(stenant))
                {
                    throw new Exception("Tenant is missing !!!");
                }
                int tenant;
                if (!int.TryParse(stenant, out tenant))
                {
                    throw new Exception("Tenant is not int  !!!");
                }



                //var declarationPM = XmlGenericUtil<DeclarationPM>.DeSerializeObject(DataIn2);


                //if (String.IsNullOrWhiteSpace(declarationPM.CustomFileNo))
                //{
                //    throw new Exception("declarationPM.CustomFileNo  is must  !");
                //}
                string logitudeRef = "";
                AppendLogLine("paymentTest..");
                //logitudeRef = paymentTest(tenant, declarationPM);



                var reqParam = new NewPaymentRequestParams();
                reqParam.Tenant = tenant;// UnifreightListsUtil.GetValue(ref hDataIn1, "Tenant");
                reqParam.PaymentNumber = UnifreightListsUtil.GetValue(ref hDataIn1, "paymentID");
                reqParam.ExternalId = UnifreightListsUtil.GetValue(ref hDataIn1, "externalID");


                var messageService = new TSH_NG_3053_MSG8_AgentPaymentRequestMessageService();
                var resParam = messageService.Send(reqParam);


                var xml = XmlGenericUtil<INF_MSG_GenericResponseData>.SerializeObject(resParam);

                //h["paymentID"] = "351686551";
                //h["externalID"] = "510120041";


                DataOut1 = xml;

                string xmlResult = "";



                AppendLogLine("SUCCESS");
                SUCCESS = true.ToString();




            }
            catch (Exception)
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
            var h = new Dictionary<string, string>();

            ///h["AssemblyQualifiedName".ToUpper()] = GetAssemblyQualifiedName(); 
            //h["Method"] = MethodsEnum.paymentTest.ToString() ;
            //var d = Enum.GetNames(typeof(MethodsEnum)).ToList<string>();
            //h["AllMethod"] = string.Join<string>(",", d);
            //<paymentID xmlns="http://malam.com/customs/EAICommon.xsd">351686551</paymentID> <externalID xmlns="http://malam.com/customs/EAICommon.xsd">510120041</externalID>

            h["Tenant"] = "1";
            h["paymentID"] = "351686551";
            h["externalID"] = "510120041";


            var myXML = UnifreightListsUtil.Serialize(h);
            return myXML;
        }

        public override string GetExampleDataIn2()
        {


            return "";
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
