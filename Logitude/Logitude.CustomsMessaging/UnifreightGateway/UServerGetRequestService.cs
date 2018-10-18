using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.UnifreightGateway
{
    public class UServerGetRequestService: UnifreightGatewayProxy
    {
       

        public UServerGetRequestService()
            :base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, // "ImportDeclarationService";
            true 
            )

        {
            
          
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
                AppendLogLine("UServerGetRequest.ProccessRequest");
                //Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO.DBTester.test();
                //Logitude.CustomsMessaging.Testers.SendUnifreightCustomInterfaceTester.Tester();
                string xmlRequest="";
                   string serializeReference="";
                var myUServerQueueManager = new UServerQueueManager();
                myUServerQueueManager.UnifreightGetRequest(out xmlRequest, out serializeReference);
                
                DataOut1 =serializeReference;
                DataOut2 = xmlRequest;
                SUCCESS = true.ToString(); 
                //DF_NG_2754_MSG10004_ImportDeclarationResponse

               

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

            return @"How  Care From that .."; 
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
