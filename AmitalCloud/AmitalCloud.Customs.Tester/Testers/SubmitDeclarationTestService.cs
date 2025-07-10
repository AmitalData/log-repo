using AmitalCloud.Infrastructure.Data.Helpers;
using Logitude.AmitalMessaging.Utils;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using System;
using System.Collections.Generic;
using AmitalCloud.Infrastructure.APITools.DataContracts;
using AmitalCloud.Infrastructure.Domain.Helpers;

namespace Logitude.CustomsMessaging.Testers
{
    public class SubmitDeclarationTestService : UnifreightGatewayProxy
    {


        public SubmitDeclarationTestService()
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





                //var declarationPM = XmlGenericUtil<DeclarationPM>.DeSerializeObject(DataIn2);


                //if (String.IsNullOrWhiteSpace(declarationPM.CustomFileNo))
                //{
                //    throw new Exception("declarationPM.CustomFileNo  is must  !");
                //}
                string logitudeRef = "";
                AppendLogLine("paymentTest..");
                //logitudeRef = paymentTest(tenant, declarationPM);



                var ms = new DF_NG_2755_MSG12001_SubmitDeclarationMessagingService();
                var resParam = ms.Send(new GenericRequestParams() { Tenant = 1, AppicationId = "1-3" });

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
