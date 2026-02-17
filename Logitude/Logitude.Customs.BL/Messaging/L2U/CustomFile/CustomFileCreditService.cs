using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.L2U.CustomFile
{
    public partial class CustomFileCreditService
    {
        private CustomFileCreditRequestParams _CustomFileCreditModel;

        public CustomFileCreditService(CustomFileCreditRequestParams customFileCreditModel)
        {
            _CustomFileCreditModel = customFileCreditModel;
        }

        public CUSTOMCREDIT_UL CheckFileCredit()
        {
            var context = CustomContext.GetContext(_CustomFileCreditModel.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);

            var declarationPM = myDeclarationQueryService.GetSingle(_CustomFileCreditModel.AppicationId, false, false);
            if (declarationPM == null)
            {
                throw new Exception("Declaration is null:" + _CustomFileCreditModel.AppicationId);
            }

            var amitalCustomFileCommunicationModel = new Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase(
               Logitude.Server.Tools.Models.AmitalStandardCommunicationModel.OperationMethod.DataAccess,
               "CWSFCREDITFILE", "DeclarationCheckCredit")
            {
                Tenant = declarationPM.Tenant,
                objectTableName = "Customs.Declaration",
                CommunicationLoggingEntityReference = declarationPM.DeclarationNumber,
                EntityId = declarationPM.Id,
                UserId = _CustomFileCreditModel.LoggingUserId,
                CommunicationSubject = "Logitude Declaration check File Credit",
            };

            var myCreditFile = new CustomFileCreditRequest();
            myCreditFile.CustomsFile = new CustomsFile[] { new CustomsFile() };
            myCreditFile.CustomsFile[0].FileNo = declarationPM.CustomFileNo;
            myCreditFile.CustomsFile[0].Mode = _CustomFileCreditModel.Mode;
            myCreditFile.CustomsFile[0].TotalTax = declarationPM.TotalTax.ToString();
            myCreditFile.CustomsFile[0].UpdatedByUser = _CustomFileCreditModel.LoggingUserId;
            
            var myUServerCommunicationService = new Logitude.Customs.BL.Messaging.Amital.UServerCommunicationService
                <Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase, CustomFileCreditRequest>(
                amitalCustomFileCommunicationModel, myCreditFile);
            bool myImmediately = true;

            var info = myUServerCommunicationService.Send(myImmediately);
            if (String.IsNullOrWhiteSpace(info.ImmediatelyResponse))
            {
                throw new Exception("ImmediatelyResponse is null");
            }
            var GenericResponse = XmlGenericUtil<GenericResponse>.DeSerializeObject(info.ImmediatelyResponse);
            var genericResponseObj = GenericResponse.GenericResponseObj.FirstOrDefault();
            if (genericResponseObj == null)
            {
                throw new Exception("GenericResponse.GenericResponseObj is null");
            }

            if (genericResponseObj.ResponseXml == null && genericResponseObj.ResponseXml == "")
            {
                throw new Exception("genericResponseObj.ResponseXml is null");
            }

            var unifreightResponse = XmlGenericUtil<CUSTOMCREDIT_UL>.DeSerializeObject(genericResponseObj.ResponseXml);


            return unifreightResponse;

        }

    }
}
