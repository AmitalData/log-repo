using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using UnifreightIIG.Common.Utils;

namespace Logitude.CustomsMessaging.UnifreightGateway
{
    public class InsertImportDeclarationService : UnifreightGatewayProxy
    {
       
        //public enum MethodsEnum
        //{
        //    //None,
        //    InsertImportDeclaration
        //}

        
        
        public  InsertImportDeclarationService()
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


                
                var hDataIn1 = UnifreightListsUtil.Deserialize(DataIn1);
                AppendLogLine("DataIn1=" + hDataIn1.Count.ToString());
                AppendLogLine("Deserialize(DataIn2) ..");


                string stenant = UnifreightListsUtil.GetValue(ref hDataIn1, "Tenant");
                if (String.IsNullOrWhiteSpace(stenant))
                {
                    throw new Exception("Tenant is missing !!!");
                }
                int tenant ;
                if (!int.TryParse(stenant,out tenant))
                {
                    throw new Exception("Tenant is not int  !!!");
                }
                //string sMethod = UnifreightListsUtil.GetValue(ref hDataIn1, "Method");
                //if (string.IsNullOrWhiteSpace(sMethod))
                //{
                //    throw new Exception("Method is missing !!!");
                //}                
                //AppendLogLine("sMethod=" + sMethod);
                //MethodsEnum myMethod;
                //if (!Enum.TryParse<MethodsEnum>(sMethod, out myMethod))
                //{
                //    throw new Exception("sMethod is bad !!!");
                //}



                if (string.IsNullOrWhiteSpace(DataIn2))
                {
                    throw new Exception("XmlIn is missing !!!");
                }
                if (DataIn2.Length > 1000)
                {
                    AppendLogLine("XmlIn=" + DataIn2.Substring(0, 1000));
                    AppendLogLine(".Substring(0, 1000)");
                }
                else
                {
                    AppendLogLine("XmlIn=" + DataIn2);
                }
                AppendLogLine("Tring DeserilazeObject" );
                var declarationPM = XmlGenericUtil<DeclarationPM>.DeSerializeObject(DataIn2);
                

                if (String.IsNullOrWhiteSpace(declarationPM.CustomFileNo))
                {
                    throw new Exception("declarationPM.CustomFileNo  is must  !");
                }
                string logitudeRef="";
                AppendLogLine("InsertImportDeclaration..");
                logitudeRef = InsertImportDeclaration(tenant, declarationPM);
                

                string xmlResult = "";


                
                if (!string.IsNullOrEmpty(logitudeRef))
                {
                    DataOut1 = logitudeRef;
                    AppendLogLine("SUCCESS");
                    SUCCESS = true.ToString();
                }
                else
                {
                    AppendLogLine("Failed !!");
                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        private string InsertImportDeclaration(int tenant, DeclarationPM declarationPM)
        {
            if (!String.IsNullOrWhiteSpace(declarationPM.Id))
            {
                throw new Exception("declarationPM.Id  is must  be null (adding ) !");
            }
            declarationPM.Tenant =tenant ;

            
            InsertDeclaration(declarationPM);

            return declarationPM.Id;
        }
        public void InsertDeclaration(DeclarationPM entityPm) //WebFreight.Web.CustomModel.DomainServices.CustomDomainService
        {

            
            //SecurityUtility.CheckContactFeature("Customs.Declaration", "NEW", entityPm.Tenant);



            ICustomContext customContext=null;
            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }


            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            DeclarationUpdateService service = new DeclarationUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);

            service.Update(entityPm, true);
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

            var myDeclarationPM = new DeclarationPM();
            myDeclarationPM.CustomerId = "1-1004";
            myDeclarationPM.CustomFileNo = "120210210";
            string xmlIn = "";//  xmlIn = XmlGenericUtil<DeclarationPM>.SerializeObject(myDeclarationPM);

            xmlIn = @"<?xml version=""1.0""?>
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
