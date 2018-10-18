using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.L2U.CustomFile
{
    public partial class CustomCertificatesService
    {
        private string _DeclarationId;
        private int _Tenant;
        
        public CustomCertificatesService(string declarationId, int tenant)
        {
            _Tenant = tenant;
            _DeclarationId = declarationId;
        }

        private CUSTOMCERTIFICATES_LU ConvertData(List<CertificateGroupItems> customCertificatesRequest, DeclarationPM declarationPM)
        {
            CUSTOMCERTIFICATES_LU customCertificatesRequestModel = new CUSTOMCERTIFICATES_LU();
            CUSTOMCERTIFICATEAMITAL customCertificate = new CUSTOMCERTIFICATEAMITAL();
            customCertificate.DeclarationId = _DeclarationId;
            customCertificate.Tenant = _Tenant.ToString();
            customCertificate.CustomFileNo = declarationPM.CustomFileNo;
            customCertificate.CustomerId = declarationPM.CustomerId;
            customCertificate.TaxationDate = declarationPM.TaxationDateTime.ToString();

            List<CertificateGroup> certificateGroupList = new List<CertificateGroup>();
            foreach (var certificateItem in customCertificatesRequest)
            {
                CertificateGroup certificateGroup = new CertificateGroup();
                certificateGroup.ClassificationCode = certificateItem.ClassificationCode;
                certificateGroup.ItemCode = certificateItem.ItemCode;
                certificateGroup.OriginCountryCode = certificateItem.OriginCountryCode;
                certificateGroup.VendorNumber = certificateItem.VendorNumber;
                certificateGroupList.Add(certificateGroup);
            }
            customCertificate.CertificateGroup = certificateGroupList.ToArray();

            List<CUSTOMCERTIFICATEAMITAL> certificatesRequest = new List<CUSTOMCERTIFICATEAMITAL>();
            certificatesRequest.Add(customCertificate);

            customCertificatesRequestModel.CUSTOMCERTIFICATEAMITAL = certificatesRequest.ToArray();

            return customCertificatesRequestModel;
        }

        public CUSTOMCERTIFICATES_UL GetUnifreightCertificateForDeclaration(List<CertificateGroupItems> customCertificatesRequest)
        {
            var context = CustomContext.GetContext(_Tenant);

            var myDeclarationQueryService = new DeclarationQueryService(context);
            DeclarationPM declarationPM = myDeclarationQueryService.GetSingle(_DeclarationId, false, false);
            if (declarationPM == null)
            {
                throw new Exception("Declaration is null:" + _DeclarationId);
            }

            CUSTOMCERTIFICATES_LU customCertificatesRequestModel = ConvertData(customCertificatesRequest, declarationPM);

            var amitalCustomFileCommunicationModel = new Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase(
               Logitude.Server.Tools.Models.AmitalStandardCommunicationModel.OperationMethod.DataAccess,"CWSFCERTIFICATES", "DeclarationGetCertificate")
            {
                Tenant = _Tenant,
                objectTableName = "Customs.Declaration",
                CommunicationLoggingEntityReference = declarationPM.DeclarationNumber,
                EntityId = _DeclarationId,
                //UserId = _CustomFileCreditModel.LoggingUserId,
                CommunicationSubject = "Logitude Declaration get File certificates",
            };

            var myUServerCommunicationService = new Logitude.Customs.BL.Messaging.Amital.UServerCommunicationService
                <Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase, CUSTOMCERTIFICATES_LU>(
                amitalCustomFileCommunicationModel, customCertificatesRequestModel);
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
            if (genericResponseObj.Status != "0")
            {
                return null; // to do?!
            }
            if (genericResponseObj.ResponseXml == null && genericResponseObj.ResponseXml == "")
            {
                throw new Exception("genericResponseObj.ResponseXml is null");
            }

            var unifreightResponse = XmlGenericUtil<CUSTOMCERTIFICATES_UL>.DeSerializeObject(genericResponseObj.ResponseXml);


            return unifreightResponse;

        }
    }
}
