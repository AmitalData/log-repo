using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.U2L.Courier
{
    public class CourierPendingReasonService : UnifreightGenericService
    {

        public CourierPendingReasonService()
            : base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
            true
            )
        {

        }

        public override string GetAssemblyQualifiedName()
        {
            throw new NotImplementedException();
        }

        public override string GetExampleDataIn1()
        {
            throw new NotImplementedException();
        }

        public override string GetExampleDataIn2()
        {
            return "";
        }

        public override string GetExampleDataout1()
        {
            return "";
        }

        public override string GetExampleDataout2()
        {
            return "";
        }

        public override void ProccessBASE64Request(string BASE64DataIn1, string BASE64DataIn2, string BASE64DataIn3, out string BASE64DataOut1, out string BASE64DataOut2, out string BASE64DataOut3, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }

        public override void ProccessGenericRequest(string DataIn, ref string MoreParams, out string MessageOut)
        {
            MessageOut = "";
            try
            {
                AppendLogLine("CustomsRequestsSheetInProgressService.ProccessRequest");
                AppendLogLine("Deserialize(DataIn1) ..");

                var hDataIn1 = UnifreightListsUtil.Deserialize(DataIn);
                AppendLogLine("DataIn=" + hDataIn1.Count.ToString());
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

                ICustomContext context = CustomContext.GetContext(ResolvedTenant());
                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
                DeclarationCourierStatusPM currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(DeclarationId, false, false);
                if (currentDeclarationCourierStatusPM != null && currentDeclarationCourierStatusPM.CourierPendingReasonCode == "900")
                {
                    DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), tenant);
                    currentDeclarationCourierStatusPM.CourierPendingReasonCode = null;
                    currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                    declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);
                    LogMessagingUtil.Instance.AppendLine("Set Courier Pending Reason Code To null");
                }
                //SUCCESS = true.ToString();
            }
            catch (Exception eeee)
            {
                //SUCCESS = false.ToString();
                //MessageError = eeee.ToString();
            }
        }

        public override void ProccessRequest(string DataIn1, string DataIn2, out string DataOut1, out string DataOut2, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }
    }
}
