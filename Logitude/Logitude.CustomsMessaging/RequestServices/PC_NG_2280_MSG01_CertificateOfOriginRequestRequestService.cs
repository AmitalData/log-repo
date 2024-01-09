using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.Server.Tools;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using UnifreightIIG.Common.CertificateOfOriginRequestServiceReference;
using UnifreightIIG.Common.LogisticActionRequestMessageServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class PC_NG_2280_MSG01_CertificateOfOriginRequestRequestService : RequestServiceBase<PC_NG_2280_MSG01_CertificateOfOriginRequest, CertificateOfOriginRequestRequestParams>
    {
        public override void OnRequestFail(CertificateOfOriginRequestRequestParams requestParams)
        {
            base.OnRequestFail(requestParams);
        }

        public override PC_NG_2280_MSG01_CertificateOfOriginRequest GetRequest(CertificateOfOriginRequestRequestParams requestParams)
        {
            var myMsg = new PC_NG_2280_MSG01_CertificateOfOriginRequest();
			CertificateOfOriginQueryService certificateOfOriginQueryService =  new CertificateOfOriginQueryService(requestParams.Tenant);
           var certificateOfOrigin =  certificateOfOriginQueryService.GetSingle(requestParams.CertificateOfOriginId,true,false);

			requestParams.RequestReasonCode = Convert.ToInt32(certificateOfOrigin.RequestReasonCode);

			myMsg.AgentRequest = new PC_NG_2280_MSG01_CertificateOfOriginRequestAgentRequest()
            {
				internalApplication = certificateOfOrigin.Counter,
				certificateOfOriginTypeCode = Convert.ToInt32( certificateOfOrigin.CooTypeCode),
				requestReasonCode = Convert.ToInt32(certificateOfOrigin.RequestReasonCode),
				certificateID = certificateOfOrigin.COONumber,
				certificateIdToCancel = certificateOfOrigin.COONumberToCancel,
				replacementReason = certificateOfOrigin.ReplacementReason,
				exportDeclarationNum = certificateOfOrigin.DeclarationId,

			};


			myMsg.CertificateOfOrigin = GetCertificateOfOrigin(certificateOfOrigin);
			myMsg.CertificateOfOrigin.CertificateOfOriginRequestInvoiceDetail = GetCertificateOfOriginRequestInvoiceDetail(certificateOfOrigin.CertificateOriginInvoiceItems,certificateOfOrigin.CertificateOriginItemItems);
			myMsg.NonManipulationCertificate = new PC_NG_2280_MSG01_CertificateOfOriginRequestNonManipulationCertificate()
			{
				ExportDate = Convert.ToDateTime(certificateOfOrigin.NonExportDate),
				ExportCountry = certificateOfOrigin.NonExportCountry,
				ImportBillOfLadingNum = certificateOfOrigin.NonImportBillOfLadingNum,
				ExportPort = certificateOfOrigin.NonExportPort,
				ImportDate = Convert.ToDateTime(certificateOfOrigin.NonImportDate),
				ExportBillOFLadingNum = certificateOfOrigin.NonExportBillOfLadingNum,
				TransirCountry = certificateOfOrigin.NonTransirCountry,
				PortOfEntrance = certificateOfOrigin.NonPortOfEntrance,
				ExpectedExitDate = Convert.ToDateTime(certificateOfOrigin.NonExpectedExitDate),
				ExitPort = certificateOfOrigin.NonExitPort,
				GoodsDescription = certificateOfOrigin.NonGoodsDescription,
				DeclaringCompany = certificateOfOrigin.NonDeclaringCompany,
				DeclaringPerson = certificateOfOrigin.NonDeclaringPerson,
				DeclaringPosition = certificateOfOrigin.NonDeclaringPosition,
				ManifestNum = certificateOfOrigin.NonManifestNum,

			};

			this.MyRequestSheetParam = new RequestSheetParam();
			this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
			this.MyRequestSheetParam.EntityId1 = requestParams.DeclarationId;
			this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.CertificateOfOrigin");
			this.MyRequestSheetParam.EntityId2 = requestParams.CertificateOfOriginId;
			this.MyRequestSheetParam.CustomFileNo = requestParams.CustomFileNo;
            this.MyRequestSheetParam.RequestDescription = "בקשת תעודת מקור : " + certificateOfOrigin.COONumber;
			return myMsg;
		}
        public PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOrigin GetCertificateOfOrigin(CertificateOfOriginPM certificateOfOrigin)
        {
            PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOrigin PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOrigin = new PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOrigin() {
			ExporterId = certificateOfOrigin.ExporterVat,
				ExporterName = certificateOfOrigin.ExporterName,
				ExporterAddress = certificateOfOrigin.ExporterAddress,
				ExporterCountry = certificateOfOrigin.ExporterVat,
				TradeAgreementCountry1 = certificateOfOrigin.TradeAgreementCountry1,
				TradeAgreementCountry2 = certificateOfOrigin.TradeAgreementCountry2,
				TradeAgreementGroupOfCountries = Convert.ToInt32(certificateOfOrigin.TradeAgreementGroupOfCountries),
				ConsigneeName = certificateOfOrigin.ConsigneeName,
				ConsigneeAddress = certificateOfOrigin.ConsigneeAddress,
				ConsigneeCountry = certificateOfOrigin.ConsigneeCountry,
				ConsigneeRemarks = certificateOfOrigin.ConsigneeRemarks,
				IsConsigneeForPrint = certificateOfOrigin.IsConsigneeForPrint,
				OriginCountry = certificateOfOrigin.OriginCountry,
				OriginGroupOfCountries = Convert.ToInt32(certificateOfOrigin.OriginGroupOfCountry),
				DestinationCountry = certificateOfOrigin.DestinationCountry,
				DestinationGroupOfCountries = Convert.ToInt32(certificateOfOrigin.DestinationGroupOfCountries),
				Transport = certificateOfOrigin.Transport,
				PortOfShipment = certificateOfOrigin.PortOfShipment,
				IsCumulation = certificateOfOrigin.IsCumulation,
				CumulationCountry = certificateOfOrigin.CumulationCountry,
				CumulationGroupOfCountries = Convert.ToInt32(certificateOfOrigin.CumulationGroupOfCountries),
				PlaceOfManufacture = Convert.ToInt32(certificateOfOrigin.PlaceOfManufacture),
				ZipCodeOfManufacture = Convert.ToInt32(certificateOfOrigin.ZipCodeOfManufacture),
				Observations = certificateOfOrigin.Observations,
				IsExportDecForPrint = certificateOfOrigin.IsExportDecForPrint,
				CustomsHouse = certificateOfOrigin.CustomsHouse,
				IssuingCountry = certificateOfOrigin.IssuingCountry,
				CityOfDeclaration = Convert.ToInt32(certificateOfOrigin.CityOfDeclaration),
				CountryOfDeclaration = certificateOfOrigin.CountryOfDeclaration,
				DateOfDeclaration = Convert.ToDateTime(certificateOfOrigin.DateOfDeclaration),
				IsDeclaredByManufacturer = certificateOfOrigin.IsDeclaredByManufacture,
				IsDeclaredByExporter = certificateOfOrigin.IsDeclaredByExporter,
				IsAttachedList = certificateOfOrigin.IsAttachedList,
				InSufficentworkingInd = certificateOfOrigin.InsufficentWorkingInd,
				InsufficentWorkingText = certificateOfOrigin.InsufficentWorkingText,
			
			};
           
            
            
            
            return PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOrigin;
		}
		public PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOriginCertificateOfOriginRequestInvoiceDetail[] GetCertificateOfOriginRequestInvoiceDetail(List<CertificateOfOriginInvoicePM> CertificateOfOriginInvoices, List<CertificateOfOriginItemPM> CertificateOfOriginItems)
        {
            var CertificateOfOriginInvoicesDetails = new List<PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOriginCertificateOfOriginRequestInvoiceDetail>();
			CertificateOfOriginInvoices = CertificateOfOriginInvoices.FindAll(x => x.IsInvoicesForPrint == true);
			foreach (var item in CertificateOfOriginInvoices)
			{
				var CertificateOfOriginInvoiceDetail = new PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOriginCertificateOfOriginRequestInvoiceDetail()
				{
                   InvoiceNum = item.InvoiceNumber,
					InvoiceDate =Convert.ToDateTime(item.InvoiceDate),
					InvoiceSum = Convert.ToInt32(item.InvoiceSum),
					CurrencyType = item.CurrencyTypeCode,
					DescriptionOfInvoice = item.DescriptionOfInvoice,
					IsInvoicesForPrint = item.IsInvoicesForPrint,
				};
                foreach (var item1 in CertificateOfOriginItems)
                {
					
					if (CertificateOfOriginItems.Count() == 1 || string.IsNullOrEmpty(item1.InvoiceConnect) || item1.InvoiceConnect.Split(',').Contains(item.InvoicesIdUry.ToString())) { 
					    var CertificateOfOriginRequestItemDetail = new PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOriginCertificateOfOriginRequestInvoiceDetailCertificateOfOriginRequestItemDetail()
					    {
					    	ItemSerial = Convert.ToInt32(item1.ItemSerial),
					    	ItemId = item1.ItemId,
					    	OriginCriterion = item1.OriginCriterionCode,
					    	MarksAndNumbers = item1.MarksAndNumbers,
					    	PackageQuantity = Convert.ToInt32(item1.PackageQuantity),
					    	PackageType = item1.PackageType,
					    	ContainerISOCode = item1.ContainerIsoCode,
					    	ItemDescription = item1.ItemDescription,
					    	Weight = Convert.ToDecimal(item1.Weight),
					    	MeasureType = item1.MeasureType,
					    
					    };
					    
                        CertificateOfOriginInvoiceDetail.CertificateOfOriginRequestItemDetail.Append(CertificateOfOriginRequestItemDetail);
					}
				}
				CertificateOfOriginInvoicesDetails.Add(CertificateOfOriginInvoiceDetail);

			}


            return CertificateOfOriginInvoicesDetails.ToArray();
        }

	}
}
