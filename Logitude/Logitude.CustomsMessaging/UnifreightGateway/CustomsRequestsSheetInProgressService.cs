
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
using Logitude.Customs.BL.EntityQueryServices;
//using UnifreightIIG.Common.Utils;

namespace Logitude.CustomsMessaging.UnifreightGateway
{
    public class CustomsRequestsSheetInProgressService : UnifreightGatewayProxy
    {
       
        //public enum MethodsEnum
        //{
        //    //None,
        //    InsertImportDeclaration
        //}

        
        
        public  CustomsRequestsSheetInProgressService()
            :base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, // "CustomsRequestsSheetInProgressService";
            true 
            )

        {
            //base.UniVersion = "1.000.000001";
            //base.UniDescription = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;// "CustomsRequestsSheetInProgressService";
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

            var MessageError= "";
            var RequestInProgress = false;
            var listRequestInProgress= "";
            var DispayMessage = "";// "Please note  RequestInProgress ( 444,444,442342)  ";

            try
            {
                AppendLogLine("CustomsRequestsSheetInProgressService.ProccessRequest");
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

                string CustomFileNo = UnifreightListsUtil.GetValue(ref hDataIn1, "CustomFileNo");
                if (String.IsNullOrWhiteSpace(CustomFileNo))
                {
                    throw new Exception("CustomFileNo  is must  !");
                }

                string DeclarationId = UnifreightListsUtil.GetValue(ref hDataIn1, "DeclarationId");
                if (String.IsNullOrWhiteSpace(DeclarationId))
                {
                    throw new Exception("DeclarationId  is must  !");
                }

               var customsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(tenant);
               List<CustomsRequestsSheetPM> customsRequestsSheetPMList = customsRequestsSheetQueryService.GetRequestInProgress(tenant, "2750", "", "", null, null, CustomFileNo, true);
                if (customsRequestsSheetPMList != null)
                {
                    if (customsRequestsSheetPMList.Count > 0)
                    {
                        var first=customsRequestsSheetPMList.First();
                        var RequestInProgressInterfaceTypeName = first.InterfaceTypeName;
                        listRequestInProgress = first.Id;
                        var text = TranslateTextsClass.Translate("Customs.General.RequestInProgress", tenant, true);
                        DispayMessage = String.Format(text, RequestInProgressInterfaceTypeName);
                        
                        RequestInProgress = true ;
                    }
                }
                SUCCESS = true.ToString();
            }
            catch (Exception eeee)
            {
                SUCCESS = false.ToString();
                MessageError = eeee.ToString() ;
            }
            finally
            {
                var hResponse = new Dictionary<string, string>();

                ///h["AssemblyQualifiedName".ToUpper()] = GetAssemblyQualifiedName(); 
                //h["Method"] = MethodsEnum.InsertImportDeclaration.ToString() ;
                //var d = Enum.GetNames(typeof(MethodsEnum)).ToList<string>();
                //h["AllMethod"] = string.Join<string>(",", d);

                hResponse["MessageError"] = MessageError;
                hResponse["RequestInProgress"] = RequestInProgress.ToString();
                hResponse["listRequestInProgress"] = listRequestInProgress;
                hResponse["DispayMessage"] = DispayMessage;
                DataOut1 = UnifreightListsUtil.Serialize(hResponse);
                
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

            h["Tenant"] = "208";
            h["CustomFileNo"] = "106152073";
            h["DeclarationId"] = "1-1004";

            
            var myXML= UnifreightListsUtil.Serialize(h);
            return myXML; 
        }

        public override string GetExampleDataIn2()
        {
            return "";
        }

        public override string GetExampleDataout1()
        {
            var h = new Dictionary<string, string>();

            ///h["AssemblyQualifiedName".ToUpper()] = GetAssemblyQualifiedName(); 
            //h["Method"] = MethodsEnum.InsertImportDeclaration.ToString() ;
            //var d = Enum.GetNames(typeof(MethodsEnum)).ToList<string>();
            //h["AllMethod"] = string.Join<string>(",", d);

            h["MessageError"] = "";
            h["RequestInProgress"] = "True";
            h["listRequestInProgress"] = "1-1004";
            h["DispayMessage"] = "Please note  RequestInProgress ( 444,444,442342)  ";

            var myXML = UnifreightListsUtil.Serialize(h);
            return myXML; 
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
