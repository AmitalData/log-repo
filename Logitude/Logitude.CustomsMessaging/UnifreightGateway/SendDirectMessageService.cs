using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Logitude.Customs.BL.Messaging.Customs;
//'using UnifreightIIG.Common.Utils;

namespace Logitude.CustomsMessaging.UnifreightGateway
{
    public class SendDirectMessageService: UnifreightGatewayProxy
    {
       
        //public enum MethodsEnum
        //{
        //    //None,
        //    InsertImportDeclaration
        //}

        
        public  SendDirectMessageService()
            :base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, // "ImportDeclarationService";
            true 
            )

        {
            //base.UniVersion = "1.000.000001";
            //base.UniDescription = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;// "ImportDeclarationService";
            //UniProduction = false;
          
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
                AppendLogLine("ImportDeclarationService.ProccessRequest");
                AppendLogLine("Deserialize(DataIn1) ..");


                //DF_NG_2754_MSG10004_ImportDeclarationResponse
                var myName = XDocument.Parse(DataIn1).Root.Name.LocalName.ToString();
                myName += "MessagingService";
                AppendLogLine("Root.Name =" + myName);
                IMessagingServiceInterfaceType  messagingService=null;
                IUnityContainer unityContainer = this.MyUnity as IUnityContainer;
                messagingService = unityContainer.Resolve<IMessagingServiceInterfaceType>(myName);
                AppendLogLine("Resolved");

                var exceptionMessage="";
                DataOut1 = messagingService.TestSendXml(DataIn1, out exceptionMessage);
                if (!String.IsNullOrWhiteSpace(exceptionMessage))
                {
                    InsertLogLine(0, "Error message " + exceptionMessage);
                }
                
                

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
            var h = new Dictionary<string, string>();
            
            ///h["AssemblyQualifiedName".ToUpper()] = GetAssemblyQualifiedName(); 
            //h["Method"] = MethodsEnum.InsertImportDeclaration.ToString() ;
            //var d = Enum.GetNames(typeof(MethodsEnum)).ToList<string>();
            //h["AllMethod"] = string.Join<string>(",", d);

            h["Tenant"] = "1";

            
            var myXML= UnifreightListsUtil.Serialize(h);
            return myXML; 
        }

        public override string GetExampleDataIn2()
        {

            

            var xmlIn = @"<?xml version=""1.0""?>
<DeclarationPM xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <ChangeSetOp>None</ChangeSetOp>
  <DeclarationConsignments />
  <Tenant>0</Tenant>
  <CustomFileNo>120210210</CustomFileNo>
  <CustomerId>1-1004</CustomerId>
  <IssueDateTime xsi:nil=""true"" />
  <TaxationDateTime xsi:nil=""true"" />
  <IsChanged>false</IsChanged>
  <PaymentDate xsi:nil=""true"" />
  <HatraDate xsi:nil=""true"" />
  <LoadingFactor xsi:nil=""true"" />
  <DealValue xsi:nil=""true"" />
  <CIFValue xsi:nil=""true"" />
  <TotalTax xsi:nil=""true"" />
  <Consignments />
  <DeletedConsignments />
  <SupplierInvoices />
  <DeletedSupplierInvoices />
</DeclarationPM>";
            return xmlIn;
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
