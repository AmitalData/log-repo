using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ImporterDeclarationsDetailServiceReference;
using Logitude.Customs.Data.Repsitories;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class VE_8327_ImporterDeclarationResponseService : ResponseServiceBase
        <ImporterDeclarationResponseData, VE_NG_8327_Web02_ImporterDeclarationsDetail, ImporterDeclarationRequestParams>
    {
        ICustomContext _DBContext;
        List<PeriodDeclarationResult> periodDeclarationResultList = null;
        List<LoiDeclarationResult> loiDeclarationResultList = null;
        List<SecurityDeclarationResult> securityDeclarationList = null;

        public override void Update(VE_NG_8327_Web02_ImporterDeclarationsDetail customResponse, ImporterDeclarationRequestParams requestParams)
        {
            _DBContext = CustomContext.GetContext(requestParams.Tenant);

            var test = false;
            if (test)
            {
                PeriodDeclarationResult periodDeclarationResult = new PeriodDeclarationResult()
                {

                    PeriodDeclarationID = "222222",
                    VendorID = "2419847",
                    VendorName = "UNIVERSAL ELECTRIC COR",
                    CreateDate = "01/11/2017",
                    ValidityFrom = "01/11/2017",
                    ExpirationDate = "31/10/2018",
                    Status = "wtf",
                    StatusName = "1",
                    DocumentID = "562775218",
                };
                periodDeclarationResultList = new List<PeriodDeclarationResult>();
                periodDeclarationResultList.Add(periodDeclarationResult);
                this.MyResponseData = new ImporterDeclarationResponseData();
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.UserMessage = "התקבלה רשימת תצהירים ליבואן " + requestParams.ImporterNumber;
                
                
                this.MyResponseData.PeriodDeclarationList = periodDeclarationResultList;
                return;
            }

            if (customResponse.ResponseContentHeader.Exception != null)
            {
                LogMessagingUtil.Instance.AppendLine(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription);
                this.MyResponseData = new ImporterDeclarationResponseData();
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                return;
            }

            if (customResponse.PeriodDeclaration != null)
            {
               periodDeclarationResultList = new List<PeriodDeclarationResult>();
               foreach (var periodDeclarationItem in customResponse.PeriodDeclaration)
                {
                    PeriodDeclarationResult periodDeclarationResult = new PeriodDeclarationResult()
                    {
                        PeriodDeclarationID = periodDeclarationItem.periodDeclarationID.ToString(),
                        VendorID = periodDeclarationItem.vendorID.ToString(),
                        VendorName = periodDeclarationItem.name,
                        CreateDate = periodDeclarationItem.createDate.Date.ToString("dd/MM/yyyy"),
                        ValidityFrom = periodDeclarationItem.validityFrom.Date.ToString("dd/MM/yyyy"),
                        ExpirationDate = periodDeclarationItem.expirationDate.Date.ToString("dd/MM/yyyy"),
                        Status = periodDeclarationItem.status.ToString(),
                        StatusName = periodDeclarationItem.statusName,
                        DocumentID = periodDeclarationItem.documentID.ToString(),
                    };
                    periodDeclarationResultList.Add(periodDeclarationResult);

                    UpsertImporterDesposition(periodDeclarationItem, customResponse, requestParams);
                }
            }

            if (customResponse.LoiDeclaration != null)
            {
                loiDeclarationResultList = new List<LoiDeclarationResult>();
                foreach (var loiDeclarationItem in customResponse.LoiDeclaration)
                {
                    LoiDeclarationResult loiDeclarationResult = new LoiDeclarationResult()
                    {
                        DeclarationID = loiDeclarationItem.declarationID,
                        LoiDeclarationID = loiDeclarationItem.loiDeclarationID.ToString(),
                        VendorID = loiDeclarationItem.vendorID.ToString(),
                        VendorName = loiDeclarationItem.name,
                        CreateDate = loiDeclarationItem.createDate.Date.ToString("dd/MM/yyyy"),
                        DocumentID = loiDeclarationItem.documentID.ToString(),
                    };
                    loiDeclarationResultList.Add(loiDeclarationResult);
                }
            }

            if (customResponse.SecurityDeclaration != null)
            {
                securityDeclarationList = new List<SecurityDeclarationResult>();
                foreach (var securityDeclarationItem in customResponse.SecurityDeclaration)
                {
                    SecurityDeclarationResult securityDeclarationResult = new SecurityDeclarationResult()
                    {
                        SecurityDeclarationType = securityDeclarationItem.securityDeclarationType.ToString(),
                        SecurityDeclarationName = securityDeclarationItem.csecurityDeclarationName,
                        SecurityImporterDeclarationId = securityDeclarationItem.securityImporterDeclarationId.ToString(),
                        DeclarationDate = securityDeclarationItem.declarationDate.Date.ToString("dd/MM/yyyy"),
                        ExpirationDate = securityDeclarationItem.expirationDate.Date.ToString("dd/MM/yyyy"),
                        StatusName = securityDeclarationItem.statusName,
                        DocumentID = securityDeclarationItem.documentID.ToString(),
                    };
                    securityDeclarationList.Add(securityDeclarationResult);
                }
            }

            this.MyResponseData = new ImporterDeclarationResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.UserMessage = "התקבלה רשימת תצהירים ליבואן " + requestParams.ImporterNumber;
            this.MyResponseData.PeriodDeclarationList = periodDeclarationResultList;
            this.MyResponseData.LoiDeclarationList = loiDeclarationResultList;
            this.MyResponseData.SecurityDeclarationList = securityDeclarationList;
        }

        private void UpsertImporterDesposition(VE_NG_8327_Web02_ImporterDeclarationsDetailList periodDeclarationItem, VE_NG_8327_Web02_ImporterDeclarationsDetail customResponse, ImporterDeclarationRequestParams requestParams)
        {
            bool importerDespositionExists = false;
            ImporterDespositionQueryService importerDespositionQueryService = new ImporterDespositionQueryService(requestParams.Tenant);
            ImporterDespositionUpdateService importerDespositionUpdateService = new ImporterDespositionUpdateService(_DBContext, new Dictionary<string, IContext>(), requestParams.Tenant);

            importerDespositionExists = false;
            ImporterDespositionPM importerDespositionPM = null;

            //Find vendor ID
            CustomsVendorQueryService customsVendorQueryService = new CustomsVendorQueryService(requestParams.Tenant);
            CustomsVendorPM customsVendorPM = customsVendorQueryService.GetVendorByNumber(periodDeclarationItem.vendorID.ToString(), requestParams.Tenant);

            //Find imporetr ID
            ClientQueryService clientQueryService = new ClientQueryService(requestParams.Tenant);
            var importerPM = clientQueryService.GetClientByCode(requestParams.ImporterNumber, requestParams.Tenant);

            //Find importerDesposition (If exists)
            if (customsVendorPM != null && importerPM != null)
            {
                importerDespositionPM = importerDespositionQueryService.GetImporterDespositionByDepositionNumberImporterVendor(periodDeclarationItem.periodDeclarationID.ToString(), importerPM.Id, customsVendorPM.Id, requestParams.Tenant);

                if (importerDespositionPM != null)
                {
                    importerDespositionExists = true;
                    importerDespositionPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    importerDespositionPM.StartDate = periodDeclarationItem.validityFrom.Date;
                    importerDespositionPM.EndDate = periodDeclarationItem.expirationDate;
                    importerDespositionPM.ImporterDepositionStatusCode = periodDeclarationItem.status.ToString();
                }


                if (!importerDespositionExists)
                {
                    importerDespositionPM = new ImporterDespositionPM()
                    {
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        DepositionNumber = periodDeclarationItem.periodDeclarationID.ToString(),
                        ImporterDepositionStatusCode = periodDeclarationItem.status.ToString(),
                        ImporterlId = importerPM.Id,
                        VendorID = customsVendorPM.Id,
                        StartDate = periodDeclarationItem.validityFrom,
                        EndDate = periodDeclarationItem.expirationDate,
                        NotesToAgent = "",
                        ErrorMessage = "" ,
                        Tenant = requestParams.Tenant
                        
                    };
                }

                importerDespositionUpdateService.Update(importerDespositionPM, true);
            }
        }

        public override ImporterDeclarationResponseData GetResponse(VE_NG_8327_Web02_ImporterDeclarationsDetail customResponse, ImporterDeclarationRequestParams requestParams)
        {

            if (requestParams.JoinCustomsVendors.GetValueOrDefault())
            {
                if(this.MyResponseData.PeriodDeclarationList!= null  && this.MyResponseData.PeriodDeclarationList.Count>0)
                {
                      var mehesVendorIdS= this.MyResponseData.PeriodDeclarationList.Select(r => r.VendorID).ToList();
                    var myCustomsVendorRepository = new CustomsVendorRepository(requestParams.Tenant);
                    var listCustomsVendor= myCustomsVendorRepository.GetVendorByNumberList(mehesVendorIdS, requestParams.Tenant);
                    this.MyResponseData.PeriodDeclarationList.ForEach(
                        periodDec => {
                            var dbVendor=listCustomsVendor.FirstOrDefault(r => r.VendorNumber == periodDec.VendorID);
                            if (dbVendor != null)
                            {
                                periodDec.DBVendorID = dbVendor.Id;
                                periodDec.DBCountryCode = dbVendor.CountryCode;
                            }
                    });
                    
                }
            }
            //ImporterDespositionClass
            return this.MyResponseData;
        }
    }
}
