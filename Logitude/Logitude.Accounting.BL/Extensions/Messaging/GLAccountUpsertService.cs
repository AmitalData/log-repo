using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.Data;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.Messaging
{
    public class GLAccountUpsertService : UnifreightGenericService
    {
        private Stopwatch _Stopwatch;


        public GLAccountUpsertService()
            : base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, // "CustomsRequestsSheetInProgressService";
            true
            )
        {


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

            var myGLAccountPMExample = new GLAccountPM()
            {
                Tenant = 1,
                DisplayNumber = "SPDE12333",
                EnglishName = "EnglishName : ",
                AccountTypeCode = "4",// Sped ?!@?!?
            };


            var xml = LogitudeXmlSerializer.SerializeObjectToXmlString<GLAccountPM>(myGLAccountPMExample); //XmlGenericUtil<GLAccountPM>.SerializeObject(myGLAccountPMExample);
            return xml;
            //var myXML= UnifreightListsUtil.Serialize(h);
            //return myXML; 
        }

        public override string GetExampleDataIn2()
        {
            //Put in More Param
            return @"<!--  Put in More Param
<?xml version=""1.0"" encoding=""utf-8"" ?>
<ArrayOfEntry xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
<!-- Put in More Param -->
 <Entry>
  <Key>TENANT</Key>
  <Value>1</Value>
 </Entry>
 <Entry>
  <Key>UNIFREIGHT_USER_ID</Key>
  <Value>ITZIK</Value>
 </Entry>
</ArrayOfEntry>
-->";
        }

        public override string GetExampleDataout1()
        {
            MyGenericResponseObj.ApplicationId = "InternalNumber.ToString()";
            MyGenericResponseObj.Stage = "Done All ";
            MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;

            
                var xml = LogitudeXmlSerializer.SerializeObjectToXmlString<GenericResponseObj >(MyGenericResponseObj); //XmlGenericUtil<GLAccountPM>.SerializeObject(myGLAccountPMExample);
            return xml;
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

        public override void ProccessGenericRequest(string DataIn, ref string MoreParams, out string MessageOut)
        {

            MessageOut = "";
            _Stopwatch = Stopwatch.StartNew();
            MyCommunicationsParams.Subject = "GLAccountUpsertService";

            var MessageError = "";
            string InternalNumber = "";
            var DispayMessage = "";// "Please note  RequestInProgress ( 444,444,442342)  ";


            AppendLogLine("ProccessRequest");

            IAccountingContext  dbContext = AccountingContext.GetContext(ResolvedTenant());
                
            AppendLogLine("Deserialize(DataIn1) ..");


            var unifaceGLAccountPM = LogitudeXmlSerializer.DeserializeObject<GLAccountPM>(DataIn);
            if (String.IsNullOrWhiteSpace(unifaceGLAccountPM.DisplayNumber))
            {

                MyGenericResponseObj.Message="";
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                return;
            }
            MyGenericResponseObj.Stage = "GetByDisplayNumber";
            var queryService = new GLAccountQueryService(dbContext);
            var myGLAccountPM = queryService.GetByDisplayNumber(unifaceGLAccountPM.DisplayNumber, unifaceGLAccountPM.Tenant).FirstOrDefault();
            
            MapIt(unifaceGLAccountPM, ref myGLAccountPM);

            MyGenericResponseObj.Stage = "Validator";
            var validationResult = GLAccountValidator.IsGLAccountValid(myGLAccountPM);
            //AccountingClassLevelValidator.ValidateClass(

            MyGenericResponseObj.Stage = "GLAccountUpdateService";
            var updateService = new GLAccountUpdateService(dbContext, new Dictionary<string, IContext>(), ResolvedTenant());
            updateService.Update(myGLAccountPM, true);
            InternalNumber = myGLAccountPM.InternalNumber;
            MyGenericResponseObj.ApplicationId = InternalNumber.ToString();
            MyGenericResponseObj.Stage = "Done All ";
            MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;

        }



        private void MapIt(GLAccountPM unifaceGLAccountPM, ref GLAccountPM myGLAccountPM)
        {
            MyGenericResponseObj.Stage = "Mapping";
            if (myGLAccountPM == null)
            {
                myGLAccountPM = new GLAccountPM();
                myGLAccountPM.Tenant = unifaceGLAccountPM.Tenant;
                myGLAccountPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            }
            else
            {
                myGLAccountPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            }
            
            myGLAccountPM.EnglishName = unifaceGLAccountPM.EnglishName;
            myGLAccountPM.AccountTypeCode = unifaceGLAccountPM.AccountTypeCode;
        }

        public override void ProccessRequest(string DataIn1, string DataIn2, out string DataOut1, out string DataOut2, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }
    }


}
