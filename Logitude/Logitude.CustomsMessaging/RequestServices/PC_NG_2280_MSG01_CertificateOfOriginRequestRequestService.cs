using Logitude.AmitalMessaging.Infrastructure.FuStatus;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.Server.Tools;
using Microsoft.Practices.ObjectBuilder2;
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
			var RequestReasonCodeList = new List<string> { "10", "13", "14" };
			var myMsg = new PC_NG_2280_MSG01_CertificateOfOriginRequest();
			CertificateOfOriginQueryService certificateOfOriginQueryService = new CertificateOfOriginQueryService(requestParams.Tenant);
			var certificateOfOrigin = certificateOfOriginQueryService.GetSingle(requestParams.CertificateOfOriginId, true, false);
			DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParams.Tenant);
			var declarationPM = declarationQueryService.GetSingle(certificateOfOrigin.DeclarationId, true, false);

			myMsg.AgentRequest = new PC_NG_2280_MSG01_CertificateOfOriginRequestAgentRequest()
			{
				internalApplication = certificateOfOrigin.Counter,
				certificateOfOriginTypeCode = Convert.ToInt32(certificateOfOrigin.CooTypeCode),
				requestReasonCode = requestParams.RequestReasonCode,
				certificateID = certificateOfOrigin.COONumber,
				certificateIdToCancel = certificateOfOrigin.COONumberToCancel,
				replacementReason = certificateOfOrigin.ReplacementReason,
                exportDeclarationNum = declarationPM?.DeclarationNumber,

            };

			string decId = declarationPM.IsAmendment != true ? declarationPM.Id : declarationPM.AmendmentOriginalDeclartation;
            if (string.IsNullOrEmpty(myMsg.AgentRequest.exportDeclarationNum) && !string.IsNullOrEmpty(decId))
			{
                string decNo = declarationQueryService.GetDeclarationNumberByDecId(decId, requestParams.Tenant);
                if (!string.IsNullOrEmpty(decNo)) myMsg.AgentRequest.exportDeclarationNum = decNo;
            }
           
			if (certificateOfOrigin.CooTypeCode != "5" && !RequestReasonCodeList.Contains(requestParams.RequestReasonCode.ToString())) 
			{

			      myMsg.CertificateOfOrigin = GetCertificateOfOrigin(certificateOfOrigin, declarationPM);
			      myMsg.CertificateOfOrigin.CertificateOfOriginRequestInvoiceDetail = certificateOfOrigin.IsUnitedInvoices ? 
			      	GetCertificateOfOriginRequestInvoiceDetailUnitedInvoices(certificateOfOrigin.CertificateOriginInvoiceItems, certificateOfOrigin.CertificateOriginItemItems) :
			      	GetCertificateOfOriginRequestInvoiceDetail(certificateOfOrigin.CertificateOriginInvoiceItems, certificateOfOrigin.CertificateOriginItemItems);
            }
			if (certificateOfOrigin.CooTypeCode == "5" && !RequestReasonCodeList.Contains(requestParams.RequestReasonCode.ToString()))
			{
				myMsg.NonManipulationCertificate = new PC_NG_2280_MSG01_CertificateOfOriginRequestNonManipulationCertificate()
				{
					ExportDate = Convert.ToDateTime(certificateOfOrigin.NonExportDate),
					ExportCountry = certificateOfOrigin.NonExportCountry,
					ImportBillOfLadingNum = certificateOfOrigin.NonImportBillOfLadingNum,
					ExportPort = certificateOfOrigin.NonExportPort,
					ImportDate = Convert.ToDateTime(certificateOfOrigin.NonImportDate),
					ExportBillOFLadingNum = certificateOfOrigin.NonExportBillOfLadingNum,
					TransirCountry = string.IsNullOrEmpty(certificateOfOrigin.NonTransirCountry)? "IL" : certificateOfOrigin.NonTransirCountry,
					PortOfEntrance = certificateOfOrigin.NonPortOfEntrance,
					ExpectedExitDate = Convert.ToDateTime(certificateOfOrigin.NonExpectedExitDate),
					ExitPort = certificateOfOrigin.NonExitPort,
					GoodsDescription = certificateOfOrigin.NonGoodsDescription,
					DeclaringCompany = certificateOfOrigin.NonDeclaringCompany,
					DeclaringPerson = certificateOfOrigin.NonDeclaringPerson,
					DeclaringPosition = certificateOfOrigin.NonDeclaringPosition,
					ManifestNum = certificateOfOrigin.NonManifestNum,

				};
			}

			this.MyRequestSheetParam = new RequestSheetParam();
			this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
			this.MyRequestSheetParam.EntityId1 = requestParams.DeclarationId;
			this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.CertificateOfOrigin");
			this.MyRequestSheetParam.EntityId2 = requestParams.CertificateOfOriginId;
			this.MyRequestSheetParam.CustomFileNo = requestParams.CustomFileNo;
            this.MyRequestSheetParam.RequestDescription = (requestParams.RequestReasonCode == 13 ? "סטטוס תעודת מקור: " : "בקשת תעודת מקור: " )+ certificateOfOrigin.Counter;
			return myMsg;
		}
        public PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOrigin GetCertificateOfOrigin(CertificateOfOriginPM certificateOfOrigin,DeclarationPM declarationPM)
        {
			var CooTypeCodeList = new List<string> { "1", "2" };


            if (declarationPM?.TransportModeId == "A")
            {
                certificateOfOrigin.Transport = "Air";
            }
            else if (declarationPM?.TransportModeId == "O")
            {
                certificateOfOrigin.Transport = "Ocean";
            }

            PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOrigin PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOrigin = new PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOrigin() {
				ExporterId = declarationPM?.ImporterCode,
				ExporterName = certificateOfOrigin.ExporterName,
				ExporterAddress = certificateOfOrigin.ExporterAddress,
				ExporterCountry = certificateOfOrigin.ExporterCountry,
				TradeAgreementCountry1 = !string.IsNullOrEmpty(certificateOfOrigin.TradeAgreementCountry1) ? certificateOfOrigin.TradeAgreementCountry1 : CooTypeCodeList.Contains(certificateOfOrigin.CooTypeCode) ? "IL" : certificateOfOrigin.TradeAgreementCountry1,
				//TradeAgreementCountry1 = CooTypeCodeList.Contains(certificateOfOrigin.CooTypeCode) ? "IL" : certificateOfOrigin.TradeAgreementCountry1,
				TradeAgreementCountry2 = certificateOfOrigin.TradeAgreementCountry2,
				TradeAgreementGroupOfCountries = string.IsNullOrEmpty(certificateOfOrigin.TradeAgreementGroupOfCountries) ? null : (int?)Convert.ToInt32(certificateOfOrigin.TradeAgreementGroupOfCountries),
				TradeAgreementGroupOfCountriesSpecified = true,

				ConsigneeName = certificateOfOrigin.ConsigneeName,
				ConsigneeAddress = certificateOfOrigin.ConsigneeAddress,
				ConsigneeCountry = certificateOfOrigin.ConsigneeCountry,
				ConsigneeRemarks = certificateOfOrigin.ConsigneeRemarks,
				IsConsigneeForPrint = certificateOfOrigin.IsConsigneeForPrint,
				IsConsigneeForPrintSpecified = true,
				OriginCountry = certificateOfOrigin.OriginCountry,
				OriginGroupOfCountries = string.IsNullOrEmpty(certificateOfOrigin.OriginGroupOfCountry) ? null : (int?)Convert.ToInt32(certificateOfOrigin.OriginGroupOfCountry),
				OriginGroupOfCountriesSpecified = true,
				DestinationCountry = certificateOfOrigin.DestinationCountry,
				DestinationGroupOfCountries = string.IsNullOrEmpty(certificateOfOrigin.DestinationGroupOfCountries) ? null : (int?)Convert.ToInt32(certificateOfOrigin.DestinationGroupOfCountries),
				DestinationGroupOfCountriesSpecified = true,
				Transport = certificateOfOrigin.Transport,
                PortOfShipment = certificateOfOrigin.CooTypeCode == "3" && declarationPM.TransportModeId == "O" ? certificateOfOrigin.PortOfShipment : null,
                IsCumulation = certificateOfOrigin.IsCumulation,
				IsCumulationSpecified = true,
				CumulationCountry = certificateOfOrigin.CumulationCountry,
				CumulationGroupOfCountries = string.IsNullOrEmpty(certificateOfOrigin.CumulationGroupOfCountries) ? null : (int?)Convert.ToInt32(certificateOfOrigin.CumulationGroupOfCountries),
				CumulationGroupOfCountriesSpecified = true,
                Observations = certificateOfOrigin.Observations,
				IsExportDecForPrint = certificateOfOrigin.IsExportDecForPrint,
				IsExportDecForPrintSpecified = true,
				CustomsHouse = !string.IsNullOrEmpty(certificateOfOrigin.CustomsHouse)? certificateOfOrigin.CustomsHouse:declarationPM?.DeclarationOfficeHandlerCode,
				IssuingCountry = "IL",
				CityOfDeclaration = string.IsNullOrEmpty(certificateOfOrigin.CityOfDeclaration) ? null : (int?)Convert.ToInt32(certificateOfOrigin.CityOfDeclaration),
				CityOfDeclarationSpecified = true,
				CountryOfDeclaration = CooTypeCodeList.Contains(certificateOfOrigin.CooTypeCode) ? "IL" : certificateOfOrigin.CountryOfDeclaration,
				DateOfDeclaration = Convert.ToDateTime(certificateOfOrigin.DateOfDeclaration),
				IsDeclaredByManufacturer = certificateOfOrigin.IsDeclaredByManufacture,
				IsDeclaredByManufacturerSpecified = true,
				IsDeclaredByExporter = certificateOfOrigin.IsDeclaredByExporter,
				IsDeclaredByExporterSpecified = true,
				IsAttachedList = certificateOfOrigin.IsAttachedList,
				IsAttachedListSpecified = true,
				InSufficentworkingInd = certificateOfOrigin.InsufficentWorkingInd,
				InSufficentworkingIndSpecified = true,
				InsufficentWorkingText = certificateOfOrigin.InsufficentWorkingText,
			
			};

            if (certificateOfOrigin.OriginCountry == "IL" && (certificateOfOrigin.CooTypeCode == "1" || certificateOfOrigin.CooTypeCode == "2" || certificateOfOrigin.CooTypeCode == "7" || certificateOfOrigin.CooTypeCode == "9"))
			{
				PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOrigin.PlaceOfManufacture = string.IsNullOrEmpty(certificateOfOrigin.PlaceOfManufacture) ? null : (int?)Convert.ToInt32(certificateOfOrigin.PlaceOfManufacture);
				PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOrigin.PlaceOfManufactureSpecified = true;
				PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOrigin.ZipCodeOfManufacture = string.IsNullOrWhiteSpace(certificateOfOrigin.ZipCodeOfManufacture) ? null : (int?)Convert.ToInt32(certificateOfOrigin.ZipCodeOfManufacture);
				PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOrigin.ZipCodeOfManufactureSpecified = true;
			}
		
            return PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOrigin;
		}
		public PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOriginCertificateOfOriginRequestInvoiceDetail[] GetCertificateOfOriginRequestInvoiceDetail(List<CertificateOfOriginInvoicePM> CertificateOfOriginInvoices, List<CertificateOfOriginItemPM> CertificateOfOriginItems)
        {
            var CertificateOfOriginInvoicesDetails = new List<PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOriginCertificateOfOriginRequestInvoiceDetail>();
			CertificateOfOriginInvoices = CertificateOfOriginInvoices.FindAll(x => x.IsInvoiceConnected == true);
			foreach (var item in CertificateOfOriginInvoices)
			{
				var CertificateOfOriginInvoiceDetail = new PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOriginCertificateOfOriginRequestInvoiceDetail()
				{
					InvoiceNum = item.InvoiceNumber,
					InvoiceDate = Convert.ToDateTime(item.InvoiceDate),
					InvoiceSum = item.InvoiceSum != null ? decimal.Parse(item.InvoiceSum) : 0,
					InvoiceSumSpecified = true,
					CurrencyType = item.CurrencyTypeCode,
					DescriptionOfInvoice = item.DescriptionOfInvoice,
					IsInvoicesForPrint = item.IsInvoicesForPrint,
				};
				List<PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOriginCertificateOfOriginRequestInvoiceDetailCertificateOfOriginRequestItemDetail> CertificateOfOriginRequestItemDetailList = new List<PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOriginCertificateOfOriginRequestInvoiceDetailCertificateOfOriginRequestItemDetail>();
                foreach (var item1 in CertificateOfOriginItems)
                {
					
					if (CertificateOfOriginItems.Count() == 1 || string.IsNullOrEmpty(item1.InvoiceConnect) || item1.InvoiceConnect.Split(',').Contains(item.InvoicesIdUry.ToString())) { 
					    var CertificateOfOriginRequestItemDetail = new PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOriginCertificateOfOriginRequestInvoiceDetailCertificateOfOriginRequestItemDetail()
					    {
					    	ItemSerial = Convert.ToInt32(item1.ItemSerial),
							ItemSerialSpecified = true,
                            ItemId = !string.IsNullOrEmpty(item1.ItemId) ? item1.ItemId : "",
                            OriginCriterion = item1.OriginCriterionCodeName,
					    	MarksAndNumbers = item1.MarksAndNumbers,
					    	PackageQuantity = Convert.ToInt32(item1.PackageQuantity),
							PackageQuantitySpecified = true,
					    	PackageType = item1.PackageType,
					    	ContainerISOCode = item1.ContainerIsoCode,
					    	ItemDescription = item1.ItemDescription,
					    	Weight = Convert.ToDecimal(item1.Weight),
					    	MeasureType = item1.MeasureType,
					    
					    };

						CertificateOfOriginRequestItemDetailList.Add(CertificateOfOriginRequestItemDetail);
					}
					
				}
				CertificateOfOriginInvoiceDetail.CertificateOfOriginRequestItemDetail = CertificateOfOriginRequestItemDetailList.ToArray();

				CertificateOfOriginInvoicesDetails.Add(CertificateOfOriginInvoiceDetail);

			}


            return CertificateOfOriginInvoicesDetails.ToArray();
        }
		public PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOriginCertificateOfOriginRequestInvoiceDetail[] GetCertificateOfOriginRequestInvoiceDetailUnitedInvoices(List<CertificateOfOriginInvoicePM> CertificateOfOriginInvoices, List<CertificateOfOriginItemPM> CertificateOfOriginItems)
		{
			var CertificateOfOriginInvoicesDetails = new List<PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOriginCertificateOfOriginRequestInvoiceDetail>();
			var CertificateOfOriginInvoiceDetail = new PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOriginCertificateOfOriginRequestInvoiceDetail();

			CertificateOfOriginInvoices = CertificateOfOriginInvoices.FindAll(x => x.IsInvoiceConnected == true);
			foreach (var item in CertificateOfOriginInvoices)
			{
				if (CertificateOfOriginInvoices.First() == item)
				{
					CertificateOfOriginInvoiceDetail.InvoiceNum = item.InvoiceNumber;
					CertificateOfOriginInvoiceDetail.InvoiceDate = Convert.ToDateTime(item.InvoiceDate);
					CertificateOfOriginInvoiceDetail.InvoiceSum = Convert.ToDecimal(item.InvoiceSum);
					CertificateOfOriginInvoiceDetail.InvoiceSumSpecified = true;
					CertificateOfOriginInvoiceDetail.CurrencyType = item.CurrencyTypeCode;
					CertificateOfOriginInvoiceDetail.DescriptionOfInvoice = item.DescriptionOfInvoice;
					CertificateOfOriginInvoiceDetail.IsInvoicesForPrint = item.IsInvoicesForPrint;

				}
				else
				{
					CertificateOfOriginInvoiceDetail.InvoiceNum += ("," + item.InvoiceNumber);
					CertificateOfOriginInvoiceDetail.InvoiceSum += Convert.ToDecimal(item.InvoiceSum);

				}

			}
			List<PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOriginCertificateOfOriginRequestInvoiceDetailCertificateOfOriginRequestItemDetail> CertificateOfOriginRequestItemDetailList = new List<PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOriginCertificateOfOriginRequestInvoiceDetailCertificateOfOriginRequestItemDetail>();

			foreach (var item1 in CertificateOfOriginItems)
			{
				var invoicesConnect = string.IsNullOrEmpty(item1.InvoiceConnect)?new string[0] :item1.InvoiceConnect?.Split(',');
				var IsInvoiceConnect = CertificateOfOriginInvoices.Exists(x=> invoicesConnect.Contains(x.InvoicesIdUry.ToString()));

				if (CertificateOfOriginItems.Count() == 1 || string.IsNullOrEmpty(item1.InvoiceConnect) || IsInvoiceConnect)
				{
					var CertificateOfOriginRequestItemDetail = new PC_NG_2280_MSG01_CertificateOfOriginRequestCertificateOfOriginCertificateOfOriginRequestInvoiceDetailCertificateOfOriginRequestItemDetail()
					{
						ItemSerial = Convert.ToInt32(item1.ItemSerial),
						ItemSerialSpecified = true,
						ItemId = !string.IsNullOrEmpty(item1.ItemId) ? item1.ItemId : "",
						OriginCriterion = item1.OriginCriterionCodeName,
						MarksAndNumbers = item1.MarksAndNumbers,
						PackageQuantity = Convert.ToInt32(item1.PackageQuantity),
						PackageQuantitySpecified = true,
						PackageType = item1.PackageType,
						ContainerISOCode = item1.ContainerIsoCode,
						ItemDescription = item1.ItemDescription,
						Weight = Convert.ToDecimal(item1.Weight),
						MeasureType = item1.MeasureType,

					};

                    CertificateOfOriginRequestItemDetailList.Add(CertificateOfOriginRequestItemDetail);
				}
			}
			CertificateOfOriginInvoiceDetail.CertificateOfOriginRequestItemDetail = CertificateOfOriginRequestItemDetailList.ToArray();

			CertificateOfOriginInvoicesDetails.Add(CertificateOfOriginInvoiceDetail);

			return CertificateOfOriginInvoicesDetails.ToArray();
		}
	}
}
