	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class CertificateOfOriginListQueryService
    {
	    private IQueryable<CertificateOfOriginList> GetIqueryableList(IQueryable<CertificateOfOrigin> iQueryable)
        {
		IQueryable<CertificateOfOriginList> query = (from a in iQueryable
                                            select new CertificateOfOriginList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          SearchFields = a.SearchFields,
					
					                          Counter = a.Counter,
					
					                          CooTypeCode = a.CooTypeCode,
					
					                          RequestReasonCode = a.RequestReasonCode,
					
					                          COONumber = a.COONumber,
					
					                          COONumberToCancel = a.COONumberToCancel,
					
					                          ReplacementReason = a.ReplacementReason,
					
					                          DeclarationId = a.DeclarationId,
					
					                          ExporterVat = a.ExporterVat,
					
					                          ExporterName = a.ExporterName,
					
					                          ExporterAddress = a.ExporterAddress,
					
					                          ExporterCountry = a.ExporterCountry,
					
					                          TradeAgreementCountry1 = a.TradeAgreementCountry1,
					
					                          TradeAgreementCountry2 = a.TradeAgreementCountry2,
					
					                          TradeAgreementGroupOfCountries = a.TradeAgreementGroupOfCountries,
					
					                          ConsigneeName = a.ConsigneeName,
					
					                          ConsigneeAddress = a.ConsigneeAddress,
					
					                          ConsigneeCountry = a.ConsigneeCountry,
					
					                          ConsigneeRemarks = a.ConsigneeRemarks,
					
					                          IsConsigneeForPrint = a.IsConsigneeForPrint,
					
					                          OriginCountry = a.OriginCountry,
					
					                          OriginGroupOfCountry = a.OriginGroupOfCountry,
					
					                          DestinationCountry = a.DestinationCountry,
					
					                          DestinationGroupOfCountries = a.DestinationGroupOfCountries,
					
					                          Transport = a.Transport,
					
					                          PortOfShipment = a.PortOfShipment,
					
					                          IsCumulation = a.IsCumulation,
					
					                          CumulationCountry = a.CumulationCountry,
					
					                          CumulationGroupOfCountries = a.CumulationGroupOfCountries,
					
					                          PlaceOfManufacture = a.PlaceOfManufacture,
					
					                          ZipCodeOfManufacture = a.ZipCodeOfManufacture,
					
					                          Observations = a.Observations,
					
					                          IsExportDecForPrint = a.IsExportDecForPrint,
					
					                          IsUnitedInvoices = a.IsUnitedInvoices,
					
					                          CustomsHouse = a.CustomsHouse,
					
					                          IssuingCountry = a.IssuingCountry,
					
					                          CityOfDeclaration = a.CityOfDeclaration,
					
					                          CountryOfDeclaration = a.CountryOfDeclaration,
					
					                          DateOfDeclaration = a.DateOfDeclaration,
					
					                          IsDeclaredByManufacture = a.IsDeclaredByManufacture,
					
					                          IsDeclaredByExporter = a.IsDeclaredByExporter,
					
					                          IsAttachedList = a.IsAttachedList,
					
					                          InsufficentWorkingInd = a.InsufficentWorkingInd,
					
					                          InsufficentWorkingText = a.InsufficentWorkingText,
					
					                          NonExportDate = a.NonExportDate,
					
					                          NonExportCountry = a.NonExportCountry,
					
					                          NonImportBillOfLadingNum = a.NonImportBillOfLadingNum,
					
					                          NonExportPort = a.NonExportPort,
					
					                          NonImportDate = a.NonImportDate,
					
					                          NonExportBillOfLadingNum = a.NonExportBillOfLadingNum,
					
					                          NonTransirCountry = a.NonTransirCountry,
					
					                          NonPortOfEntrance = a.NonPortOfEntrance,
					
					                          NonExpectedExitDate = a.NonExpectedExitDate,
					
					                          NonExitPort = a.NonExitPort,
					
					                          NonGoodsDescription = a.NonGoodsDescription,
					
					                          NonDeclaringCompany = a.NonDeclaringCompany,
					
					                          NonDeclaringPerson = a.NonDeclaringPerson,
					
					                          NonDeclaringPosition = a.NonDeclaringPosition,
					
					                          NonManifestNum = a.NonManifestNum,
					
					                          ErrXml = a.ErrXml,
					
					                          CooStatusCode = a.CooStatusCode,
					
					                          FeedbackRemark = a.FeedbackRemark,
					
					                          RejectCancelReason = a.RejectCancelReason,
					
					                          IssueDateIfReleased = a.IssueDateIfReleased,
					
					                          QueryUrl = a.QueryUrl,
					
					                          CooPdf = a.CooPdf,
					
					                          CoodPdf1 = a.CoodPdf1,
					
					                          OpenByUser = a.OpenByUser,

											  OpenByUserName = a.CreateByUser.Code,
                                              
											  CooTypeCodeName = a.CertificateOfOriginTypeCodeEnum.LocalName,

                                              RequestReasonCodeName = a.RequestReasonCodeEnum.LocalName,

											  CooStatusCodeName = a.CertificateOfOriginStatusCodeEnum.LocalName,

                                              IsSubmitted = a.IsSubmitted,
					
		                    	            });
            return query;
		}

		private IQueryable<CertificateOfOrigin> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CertificateOfOrigin> iQueryable, int tenant)
        {
            return iQueryable;
        }
    }


}
	