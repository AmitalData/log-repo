using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.ShipmentsModel
{
    public class SplitShipmentController : ApiController
    {
        public HttpResponseMessage Put(SplitShipmentHelper helper)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;
                    string loggedEmail = authToken.Email;

                    SecurityUtility.AuthenticationOnTenant(tenant);
                    SecurityUtility.CheckContactFeature("Shipment", "NEW", tenant);
                    SecurityUtility.CheckContactFeature("Shipment", "UPDATE", tenant);

                    if (helper != null)
                    {
                        if (helper.SplitPackages.Count == 0)
                        {
                            throw new ApplicationException("Please Choose Packages");
                        }

                        else
                        {
                            ShipmentPM oldShipmentPM = helper.Shipment;
                            ShipmentPM newShipmentPM = this.CopyShipment(oldShipmentPM);

                            int SplitIndex = 0;
                            foreach (SplitPackage item in helper.SplitPackages)
                            {
                                if (item.IsSplit)
                                {
                                    ShipmentPackagePM oldPackagePM = oldShipmentPM.ShipmentPackages.Where(d => d.Id == item.Id).FirstOrDefault();

                                    if(oldPackagePM != null)
                                    {
                                        ShipmentPackagePM newPackagePM = this.CopyPackage(oldPackagePM, true);

                                        SplitIndex++;
                                        newPackagePM.IsFromSplit = true;
                                        newPackagePM.SplitIndex = SplitIndex;

                                        oldPackagePM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;

                                        newShipmentPM.ShipmentPackages.Add(newPackagePM);

                                        if (oldPackagePM.IsDeliveryFU && oldPackagePM.DeliveryId != null)
                                        {
                                            this.CopyRoutingDelivery(oldShipmentPM, newShipmentPM, oldPackagePM.DeliveryId, SplitIndex);
                                        }

                                        if (oldPackagePM.IsEmptyContainerReturnFU && oldPackagePM.EmptyContainerReturnId != null)
                                        {
                                            this.CopyRoutingDelivery(oldShipmentPM, newShipmentPM, oldPackagePM.EmptyContainerReturnId, SplitIndex);
                                        }
                                    }

                                }

                                else if (item.IsPartialSplit)
                                {
                                    ShipmentPackagePM oldPackagePM = oldShipmentPM.ShipmentPackages.Where(d => d.Id == item.ParentId).FirstOrDefault();

                                    if (oldPackagePM != null)
                                    {
                                        ShipmentPackagePM newPackagePM = this.CopyPackage(oldPackagePM, false);

                                        newPackagePM.Quantity = item.Quantity;
                                        newPackagePM.Volume = item.Volume;
                                        newPackagePM.Weight = item.Weight;
                                        this.ComputeVolumetricWeight(newPackagePM, newShipmentPM);

                                        if (oldPackagePM.Quantity != null && item.Quantity != null)
                                        {
                                            oldPackagePM.Quantity -= item.Quantity;
                                        }

                                        if (oldPackagePM.Volume != null && item.Volume != null)
                                        {
                                            oldPackagePM.Volume -= item.Volume;
                                        }

                                        if (oldPackagePM.Weight != null && item.Weight != null)
                                        {
                                            oldPackagePM.Weight -= item.Weight;
                                        }

                                        this.ComputeVolumetricWeight(oldPackagePM, oldShipmentPM);

                                        oldPackagePM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                                        newShipmentPM.ShipmentPackages.Add(newPackagePM);
                                    }
                                }
                            }

                            this.CalculateShipmentAmounts(oldShipmentPM);
                            this.CalculateShipmentAmounts(newShipmentPM);

                            IShipmentsContext objectContext = ShipmentsContext.GetContext(tenant);
                            ShipmentService oldShipmentService = new ShipmentService(objectContext, oldShipmentPM, loggedEmail);
                            ShipmentService newShipmentService = new ShipmentService(objectContext, newShipmentPM, loggedEmail);
                            oldShipmentService.Update(true);
                            newShipmentService.Create();
                            objectContext.SaveChanges();

                            List<ShipmentDeliveryPM> SplittedDeliveries = newShipmentPM.ShipmentDeliveries.Where(d => d.IsFromSplit).ToList();
                            if (SplittedDeliveries.Count > 0)
                            {
                                foreach (ShipmentDeliveryPM itemDeliveryPM in SplittedDeliveries)
                                {
                                    ShipmentPackagePM itemPackagePM = newShipmentPM.ShipmentPackages.Where(d => d.IsFromSplit && d.SplitIndex == itemDeliveryPM.SplitIndex).FirstOrDefault();
                                    if (itemPackagePM != null)
                                    {
                                        itemDeliveryPM.ConnectedPackageId = itemPackagePM.Id;

                                        if (itemDeliveryPM.PickUpDeliveryTypeCode == "EMPT")
                                        {
                                            itemPackagePM.EmptyContainerReturnId = itemDeliveryPM.Id;
                                        }

                                        else
                                        {
                                            itemPackagePM.DeliveryId = itemDeliveryPM.Id;
                                        }

                                        ShipmentPackageRepository shipmentPackageRepository = new ShipmentPackageRepository(objectContext);
                                        ShipmentPackage itemPackagePOCO = shipmentPackageRepository.GetSingleShipmentPackage(itemPackagePM.Id, tenant);
                                        if (itemPackagePOCO != null)
                                        {
                                            itemPackagePOCO.DeliveryId = itemPackagePM.DeliveryId;
                                            itemPackagePOCO.EmptyContainerReturnId = itemPackagePM.EmptyContainerReturnId;
                                        }
                                    }
                                }

                                objectContext.SaveChanges();
                            }

                            helper.NewShipmentId = newShipmentPM.Id;
                        }
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, helper);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private ShipmentPM CopyShipment(ShipmentPM oldEntity)
        {
            ShipmentPM entityPM = new ShipmentPM()
            {
                Tenant = oldEntity.Tenant,
                ShipmentLevelCode = oldEntity.ShipmentLevelCode,
                DirectionId = oldEntity.DirectionId,
                TransportModeId = oldEntity.TransportModeId,
                ShipmentTypeId = oldEntity.ShipmentTypeId,
                BranchId = oldEntity.BranchId,
                DepartmentId = oldEntity.DepartmentId,
                ProfitCurrencyId = oldEntity.ProfitCurrencyId,
                ProfitExchangeRate = oldEntity.ProfitExchangeRate,
                SalesmanUserId = oldEntity.SalesmanUserId,
                AWBCurrencyId = oldEntity.AWBCurrencyId,
                IncotermId = oldEntity.IncotermId,
                FreightPrepaidCollectId = oldEntity.FreightPrepaidCollectId,
                OtherPrepaidCollectId = oldEntity.OtherPrepaidCollectId,
                SCI = oldEntity.SCI,
                MainHarmonize = oldEntity.MainHarmonize,
                AWBDeclaredValueForCarriage = oldEntity.AWBDeclaredValueForCarriage,
                AWBDeclaredValueForCustoms = oldEntity.AWBDeclaredValueForCustoms,
                AWBInsurrenceValue = oldEntity.AWBInsurrenceValue,
                AWBCarrierTarrifReference = oldEntity.AWBCarrierTarrifReference,
                ReferenceNumber = oldEntity.ReferenceNumber,
                SupplementaryShipmentInformation1 = oldEntity.SupplementaryShipmentInformation1,
                SupplementaryShipmentInformation2 = oldEntity.SupplementaryShipmentInformation2,
                AWBSignature = oldEntity.AWBSignature,
                AWBPlace = oldEntity.AWBPlace,
                CASSCode = oldEntity.CASSCode,
                AWBAccountingInformation = oldEntity.AWBAccountingInformation,
                AWBHandlingInformation = oldEntity.AWBHandlingInformation,
                AWBComments = oldEntity.AWBComments,
                AWBSpecialHandlingCodeId1 = oldEntity.AWBSpecialHandlingCodeId1,
                AWBSpecialHandlingCodeId2 = oldEntity.AWBSpecialHandlingCodeId2,
                AWBSpecialHandlingCodeId3 = oldEntity.AWBSpecialHandlingCodeId3,
                AWBSpecialHandlingCodeId4 = oldEntity.AWBSpecialHandlingCodeId4,
                AWBSpecialHandlingCodeId5 = oldEntity.AWBSpecialHandlingCodeId5,
                AWBSpecialHandlingCodeId6 = oldEntity.AWBSpecialHandlingCodeId6,
                AWBSpecialHandlingCodeId7 = oldEntity.AWBSpecialHandlingCodeId7,
                AWBSpecialHandlingCodeId8 = oldEntity.AWBSpecialHandlingCodeId8,
                AWBSpecialHandlingCodeId9 = oldEntity.AWBSpecialHandlingCodeId9,
                AccountingInformation1 = oldEntity.AccountingInformation1,
                AccountingInformation2 = oldEntity.AccountingInformation2,
                AccountingInformation3 = oldEntity.AccountingInformation3,
                AccountingInformation4 = oldEntity.AccountingInformation4,
                AccountingInformation5 = oldEntity.AccountingInformation5,
                AccountingInformation6 = oldEntity.AccountingInformation6,
                AccountingInformationIdentifierCode1 = oldEntity.AccountingInformationIdentifierCode1,
                AccountingInformationIdentifierCode2 = oldEntity.AccountingInformationIdentifierCode2,
                AccountingInformationIdentifierCode3 = oldEntity.AccountingInformationIdentifierCode3,
                AccountingInformationIdentifierCode4 = oldEntity.AccountingInformationIdentifierCode4,
                AccountingInformationIdentifierCode5 = oldEntity.AccountingInformationIdentifierCode5,
                AccountingInformationIdentifierCode6 = oldEntity.AccountingInformationIdentifierCode6,
                AWBChargesCodeCode = oldEntity.AWBChargesCodeCode,
                AWBFreightAmountPrepaid = oldEntity.AWBFreightAmountPrepaid,
                AWBFreightAmountCollect = oldEntity.AWBFreightAmountCollect,
                Ratio = oldEntity.Ratio,
                DimFactor = oldEntity.DimFactor,
                VolumeUnitCode = oldEntity.VolumeUnitCode,
                GrossWeightUnitCode = oldEntity.GrossWeightUnitCode,
                DimensionsUnitCode = oldEntity.DimensionsUnitCode,
                ChargeableWeightUnitCode = oldEntity.ChargeableWeightUnitCode,
                IncludePickUp = oldEntity.IncludePickUp,
                FromAddressCity = oldEntity.FromAddressCity,
                FromAddressZipCode = oldEntity.FromAddressZipCode,
                FromAddressCountryId = oldEntity.FromAddressCountryId,
                PickUpAddressId = oldEntity.PickUpAddressId,
                IncludeDelivery = oldEntity.IncludeDelivery,
                ToAddressCity = oldEntity.ToAddressCity,
                ToAddressZipCode = oldEntity.ToAddressZipCode,
                ToAddressCountryId = oldEntity.ToAddressCountryId,
                DeliveryAddressId = oldEntity.DeliveryAddressId,
                AccountNumber = oldEntity.AccountNumber,
                ValueOfGoods = oldEntity.ValueOfGoods,
                ValueOfGoodsCurrencyId = oldEntity.ValueOfGoodsCurrencyId,
                AccountManagerUserId = oldEntity.AccountManagerUserId,
                AccountManagerUserName = oldEntity.AccountManagerUserName,
                AdditionalHandlingInfo = oldEntity.AdditionalHandlingInfo,
                AdditionalHandlingInfoEdited = oldEntity.AdditionalHandlingInfoEdited,

                // Main Carriage
                InterlineId = oldEntity.InterlineId,
                AirlinePrefix = oldEntity.AirlinePrefix,
                MainCarriageVesselId = oldEntity.MainCarriageVesselId,
                MainCarriageFromPartnerId = oldEntity.MainCarriageFromPartnerId,
                MainCarriageToPartnerId = oldEntity.MainCarriageToPartnerId,
                MainCarriageToAddressId = oldEntity.MainCarriageToAddressId,
                MainCarriageFromAddressId = oldEntity.MainCarriageFromAddressId,
                Driver = oldEntity.Driver,
                TrailerNumber = oldEntity.TrailerNumber,
                TruckNumber = oldEntity.TruckNumber,
                MainCarriageCarrierId = oldEntity.MainCarriageCarrierId,
                MainCarriageCarrierNumber = oldEntity.MainCarriageCarrierNumber,
                MainCarriageCarrierCode = oldEntity.MainCarriageCarrierCode,
                MainCarriageCarrierPrefix = oldEntity.MainCarriageCarrierPrefix,
                FromPortId = oldEntity.MainCarriageFromPortId,
                MainCarriageFromPortId = oldEntity.MainCarriageFromPortId,
                MainCarriageFromPortCode = oldEntity.MainCarriageFromPortCode,
                MainCarriageFromPortName = oldEntity.MainCarriageFromPortName,
                MainCarriageFromPortCountryCode = oldEntity.MainCarriageFromPortCountryCode,
                MainCarriageFromPortCountryName = oldEntity.MainCarriageFromPortCountryName,
                ToPortId = oldEntity.MainCarriageToPortId,
                MainCarriageToPortId = oldEntity.MainCarriageToPortId,
                MainCarriageToPortCode = oldEntity.MainCarriageToPortCode,
                MainCarriageToPortName = oldEntity.MainCarriageToPortName,
                MainCarriageToPortCountryCode = oldEntity.MainCarriageToPortCountryCode,
                MainCarriageToPortCountryName = oldEntity.MainCarriageToPortCountryName,
                FinalDistenationPortId = oldEntity.FinalDistenationPortId,
                MainCarriageFinalDestinationPortId = oldEntity.MainCarriageFinalDestinationPortId,

                // Flights
                Transshipment1FromPortId = oldEntity.Transshipment1FromPortId,
                Transshipment1FromPortCode = oldEntity.Transshipment1FromPortCode,
                Transshipment1FromPortName = oldEntity.Transshipment1FromPortName,
                Transshipment1FromPortCountryCode = oldEntity.Transshipment1FromPortCountryCode,
                Transshipment1FromPortCountryName = oldEntity.Transshipment1FromPortCountryName,
                Transshipment1ToPortId = oldEntity.Transshipment1ToPortId,
                Transshipment1ToPortCode = oldEntity.Transshipment1ToPortCode,
                Transshipment1ToPortName = oldEntity.Transshipment1ToPortName,
                Transshipment1ToPortCountryCode = oldEntity.Transshipment1ToPortCountryCode,
                Transshipment1ToPortCountryName = oldEntity.Transshipment1ToPortCountryName,
                Transshipment1CarrierId = oldEntity.Transshipment1CarrierId,
                Transshipment1CarrierNumber = oldEntity.Transshipment1CarrierNumber,
                Transshipment1CarrierCode = oldEntity.Transshipment1CarrierCode,
                Transshipment1CarrierPrefix = oldEntity.Transshipment1CarrierPrefix,

                Transshipment2FromPortId = oldEntity.Transshipment2FromPortId,
                Transshipment2FromPortCode = oldEntity.Transshipment2FromPortCode,
                Transshipment2FromPortName = oldEntity.Transshipment2FromPortName,
                Transshipment2FromPortCountryCode = oldEntity.Transshipment2FromPortCountryCode,
                Transshipment2FromPortCountryName = oldEntity.Transshipment2FromPortCountryName,
                Transshipment2ToPortId = oldEntity.Transshipment2ToPortId,
                Transshipment2ToPortCode = oldEntity.Transshipment2ToPortCode,
                Transshipment2ToPortName = oldEntity.Transshipment2ToPortName,
                Transshipment2ToPortCountryCode = oldEntity.Transshipment2ToPortCountryCode,
                Transshipment2ToPortCountryName = oldEntity.Transshipment2ToPortCountryName,
                Transshipment2CarrierId = oldEntity.Transshipment2CarrierId,
                Transshipment2CarrierNumber = oldEntity.Transshipment2CarrierNumber,
                Transshipment2CarrierCode = oldEntity.Transshipment2CarrierCode,
                Transshipment2CarrierPrefix = oldEntity.Transshipment2CarrierPrefix,

                Transshipment3FromPortId = oldEntity.Transshipment3FromPortId,
                Transshipment3FromPortCode = oldEntity.Transshipment3FromPortCode,
                Transshipment3FromPortName = oldEntity.Transshipment3FromPortName,
                Transshipment3FromPortCountryCode = oldEntity.Transshipment3FromPortCountryCode,
                Transshipment3FromPortCountryName = oldEntity.Transshipment3FromPortCountryName,
                Transshipment3ToPortId = oldEntity.Transshipment3ToPortId,
                Transshipment3ToPortCode = oldEntity.Transshipment3ToPortCode,
                Transshipment3ToPortName = oldEntity.Transshipment3ToPortName,
                Transshipment3ToPortCountryCode = oldEntity.Transshipment3ToPortCountryCode,
                Transshipment3ToPortCountryName = oldEntity.Transshipment3ToPortCountryName,
                Transshipment3CarrierId = oldEntity.Transshipment3CarrierId,
                Transshipment3CarrierNumber = oldEntity.Transshipment3CarrierNumber,
                Transshipment3CarrierCode = oldEntity.Transshipment3CarrierCode,
                Transshipment3CarrierPrefix = oldEntity.Transshipment3CarrierPrefix,

                // Partners
                ShipperId = oldEntity.ShipperId,
                ShipperAddress1 = oldEntity.ShipperAddress1,
                ShipperAddress2 = oldEntity.ShipperAddress2,
                ShipperAddressCountryCode = oldEntity.ShipperAddressCountryCode,
                ShipperAddressId = oldEntity.ShipperAddressId,
                ShipperAddressOneTime = oldEntity.ShipperAddressOneTime,
                ShipperAddressText = oldEntity.ShipperAddressText,
                ShipperCity = oldEntity.ShipperCity,
                ShipperContactId = oldEntity.ShipperContactId,
                ShipperCountryId = oldEntity.ShipperCountryId,
                ShipperMainAddressId = oldEntity.ShipperMainAddressId,
                ShipperName = oldEntity.ShipperName,
                ShipperNote = oldEntity.ShipperNote,
                ShipperStateId = oldEntity.ShipperStateId,
                ShipperZipCode = oldEntity.ShipperZipCode,
                ShipperPickAddressId = oldEntity.ShipperPickAddressId,

                ConsigneeAddress1 = oldEntity.ConsigneeAddress1,
                ConsigneeAddress2 = oldEntity.ConsigneeAddress2,
                ConsigneeAddressCountryCode = oldEntity.ConsigneeAddressCountryCode,
                ConsigneeAddressId = oldEntity.ConsigneeAddressId,
                ConsigneeAddressOneTime = oldEntity.ConsigneeAddressOneTime,
                ConsigneeAddressText = oldEntity.ConsigneeAddressText,
                ConsigneeCity = oldEntity.ConsigneeCity,
                ConsigneeContactId = oldEntity.ConsigneeContactId,
                ConsigneeCountryId = oldEntity.ConsigneeCountryId,
                ConsigneeId = oldEntity.ConsigneeId,
                ConsigneeMainAddressId = oldEntity.ConsigneeMainAddressId,
                ConsigneeName = oldEntity.ConsigneeName,
                ConsigneeNote = oldEntity.ConsigneeNote,
                ConsigneePickAddressId = oldEntity.ConsigneePickAddressId,
                ConsigneeStateId = oldEntity.ConsigneeStateId,
                ConsigneeZipCode = oldEntity.ConsigneeZipCode,

                CustomerAddressId = oldEntity.CustomerAddressId,
                CustomerContactId = oldEntity.CustomerContactId,
                CustomerId = oldEntity.CustomerId,
                CustomerName = oldEntity.CustomerName,
                CustomerNote = oldEntity.CustomerNote,
                CustomerRankName = oldEntity.CustomerRankName,
                ShipmentCustomerTypeCode = oldEntity.ShipmentCustomerTypeCode,
                Field1 = oldEntity.Field1,
                Field2 = oldEntity.Field2,
                Field3 = oldEntity.Field3,
                Field4 = oldEntity.Field4,
                Field5 = oldEntity.Field5,
                Field6 = oldEntity.Field6,
                Field7 = oldEntity.Field7,
                Field8 = oldEntity.Field8,
                Field9 = oldEntity.Field9,
                Field10 = oldEntity.Field10,
                Field11 = oldEntity.Field11,
                Field12 = oldEntity.Field12,
                Field13 = oldEntity.Field13,
                Field14 = oldEntity.Field14,
                Field15 = oldEntity.Field15,
                Field16 = oldEntity.Field16,
                Field17 = oldEntity.Field17,
                Field18 = oldEntity.Field18,
                Field19 = oldEntity.Field19,
                Field20 = oldEntity.Field20,
                Field21 = oldEntity.Field21,
                Field22 = oldEntity.Field22,
                Field23 = oldEntity.Field23,
                Field24 = oldEntity.Field24,
                Field25 = oldEntity.Field25,
                Field26 = oldEntity.Field26,
                Field27 = oldEntity.Field27,
                Field28 = oldEntity.Field28,
                Field29 = oldEntity.Field29,
                Field30 = oldEntity.Field30,
                Field31 = oldEntity.Field31,
                Field32 = oldEntity.Field32,
                Field33 = oldEntity.Field33,
                Field34 = oldEntity.Field34,
                Field35 = oldEntity.Field35,
                Field36 = oldEntity.Field36,
                Field37 = oldEntity.Field37,
                Field38 = oldEntity.Field38,
                Field39 = oldEntity.Field39,
                Field40 = oldEntity.Field40,
            };

            entityPM.NewConcurrencyGUID = Guid.NewGuid().ToString();
            entityPM.SplitFromShipmentNo = oldEntity.ShipmentNumber;

            int tenant = oldEntity.Tenant;
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;

            if (!string.IsNullOrEmpty(entityPM.ProfitCurrencyId))
            {
                Tenant loggedTenant = TenantRepository.GetSingleTenant(tenant, true);

                if (entityPM.ProfitCurrencyId == loggedTenant.CurrencyId)
                {
                    entityPM.ProfitExchangeRate = 1;
                }

                else
                {
                    RatesTableQuery lastRateQuery = new RatesTableQuery(tenant);
                    LastRate lastRate = lastRateQuery.GetLastRecordByValueDate(tenant, entityPM.ProfitCurrencyId, loggedTenant.CurrencyId, entityPM.CreateDateTime);
                    if (lastRate != null)
                    {
                        entityPM.ProfitExchangeRate = lastRate.Rate;
                    }
                }
            }


            return entityPM;
        }
        private ShipmentPackagePM CopyPackage(ShipmentPackagePM oldEntity, bool isFullSplit)
        {
            ShipmentPackagePM entityPM = new ShipmentPackagePM()
            {
                ContainerNumber = null,
                Tenant = oldEntity.Tenant,
                CeficClass = oldEntity.CeficClass,
                ClassNumber = oldEntity.ClassNumber,
                CommodityId = oldEntity.CommodityId,
                CommodityNumber = oldEntity.CommodityNumber,
                CommodityName = oldEntity.CommodityName,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                ContainerSize = oldEntity.ContainerSize,
                Description = oldEntity.Description,
                EMS = oldEntity.EMS,
                FlashPoint = oldEntity.FlashPoint,
                Harmonize = oldEntity.Harmonize,
                Height = oldEntity.Height,
                IMDGCode = oldEntity.IMDGCode,
                IsContainer = oldEntity.IsContainer,
                IsDangerous = oldEntity.IsDangerous,
                KelmerCode = oldEntity.KelmerCode,
                Length = oldEntity.Length,
                MarinePollutant = oldEntity.MarinePollutant,
                MarksAndNumbers = oldEntity.MarksAndNumbers,
                MaterialDescription = oldEntity.MaterialDescription,
                MethodUsed = oldEntity.MethodUsed,
                Notes = oldEntity.Notes,
                PackageTypeCode = oldEntity.PackageTypeCode,
                PackageTypeId = oldEntity.PackageTypeId,
                PackageTypeIsAir = oldEntity.PackageTypeIsAir,
                PackageTypeIsInland = oldEntity.PackageTypeIsInland,
                PackageTypeIsOcean = oldEntity.PackageTypeIsOcean,
                PackageTypeLocalName = oldEntity.PackageTypeLocalName,
                PackageTypeName = oldEntity.PackageTypeName,
                PackageTypeNote = oldEntity.PackageTypeNote,
                PackageTypeVolume = oldEntity.PackageTypeVolume,
                PackagingGroup = oldEntity.PackagingGroup,
                PrintAs = oldEntity.PrintAs,
                ProperShippingName = oldEntity.ProperShippingName,
                Quantity = oldEntity.Quantity,
                Reference1 = oldEntity.Reference1,
                Reference2 = oldEntity.Reference2,
                Reference3 = oldEntity.Reference3,
                ShipperSeal = oldEntity.ShipperSeal,
                CarrierSeal = oldEntity.CarrierSeal,
                SOC = oldEntity.SOC,
                Tare = oldEntity.Tare,
                Temperature = oldEntity.Temperature,
                TEU = oldEntity.TEU,
                UnNumber = oldEntity.UnNumber,
                Ventilation = oldEntity.Ventilation,
                VGM = oldEntity.VGM,
                Volume = oldEntity.Volume,
                VolumetricWeight = oldEntity.VolumetricWeight,
                Weight = oldEntity.Weight,
                Width = oldEntity.Width,

                DeliveryId = null,
                EmptyContainerReturnId = null,
                IsDeliveryFU = oldEntity.IsDeliveryFU,
                IsEmptyContainerReturnFU = oldEntity.IsEmptyContainerReturnFU,
                DeliveryATA = oldEntity.DeliveryATA,
                DeliveryATD = oldEntity.DeliveryATD,
                DeliveryETA = oldEntity.DeliveryETA,
                DeliveryETD = oldEntity.DeliveryETD,
                DeliveryFrom = oldEntity.DeliveryFrom,
                DeliveryTo = oldEntity.DeliveryTo,
                EmptyContainerReturnATA = oldEntity.EmptyContainerReturnATA,
                EmptyContainerReturnATD = oldEntity.EmptyContainerReturnATD,
                EmptyContainerReturnETA = oldEntity.EmptyContainerReturnETA,
                EmptyContainerReturnETD = oldEntity.EmptyContainerReturnETD,
                EmptyContainerReturnFrom = oldEntity.EmptyContainerReturnFrom,
                EmptyContainerReturnTo = oldEntity.EmptyContainerReturnTo,


                Id = null,
                DummyIdGuid = null,
                IsAWBWizardDefault = false,
                IsPackageAddedManually = false,
                ShipmentId = null,
                ShipmentNumber = null,
                ShipmentPM = null,
                ShipmentPMId = null,
                OriginalShipmentPackageId = null,
                NumberOfInsidePackages = 0,
                NumberOfInsidePackagesDetails = null,
            };

            if (isFullSplit)
            {
                foreach (InsideShipmentPackagePM item in oldEntity.InsideShipmentPackages)
                {
                    #region
                    entityPM.InsideShipmentPackages.Add(new InsideShipmentPackagePM()
                    {
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        Tenant = item.Tenant,
                        CommodityNumber = item.CommodityNumber,                       
                        ContainerSize = item.ContainerSize,
                        Description = item.Description,
                        Height = item.Height,
                        IsContainer = item.IsContainer,
                        Length = item.Length,
                        PackageTypeCode = item.PackageTypeCode,
                        PackageTypeId = item.PackageTypeId,
                        PackageTypeIsAir = item.PackageTypeIsAir,
                        PackageTypeIsInland = item.PackageTypeIsInland,
                        PackageTypeIsOcean = item.PackageTypeIsOcean,
                        PackageTypeLocalName = item.PackageTypeLocalName,
                        PackageTypeName = item.PackageTypeName,
                        PackageTypeNote = item.PackageTypeNote,
                        PackageTypeVolume = item.PackageTypeVolume,
                        PrintAs = item.PrintAs,
                        Quantity = item.Quantity,
                        Reference1 = item.Reference1,
                        Reference2 = item.Reference2,
                        Reference3 = item.Reference3,
                        TEU = item.TEU,
                        Volume = item.Volume,
                        VolumetricWeight = item.VolumetricWeight,
                        Weight = item.Weight,
                        Width = item.Width,

                        Id = null,
                        IsPackageAddedManually = false,
                        OriginalInsideShipmentPackageId = null,
                        OriginalShipmentPackageId = null,
                        ShipmentPackageId = null,
                    });
                    #endregion
                }

                foreach (ShipmentPackageItemPM item in oldEntity.ShipmentPackageItems)
                {
                    #region
                    entityPM.ShipmentPackageItems.Add(new ShipmentPackageItemPM()
                    {
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        Tenant = item.Tenant,
                        Description = item.Description,
                        GoodsValue = item.GoodsValue,
                        LineNumber = item.LineNumber,
                        Quantity = item.Quantity,
                    });
                    #endregion
                }               
            }

            return entityPM;
        }
        private void ComputeVolumetricWeight(ShipmentPackagePM PackagePM, ShipmentPM ShipmentPM)
        {
            double? myResult = null;

            if (PackagePM.Width == null || PackagePM.Height == null || PackagePM.Length == null || PackagePM.Quantity == null)
            {
                if (PackagePM.Volume != null)
                {
                    myResult = this.GetWeightFromVolume(ShipmentPM.VolumeUnitCode, ShipmentPM.ChargeableWeightUnitCode, PackagePM.Volume, ShipmentPM.Ratio);
                }

                else if (PackagePM.Weight != null)
                {
                    myResult = this.GetWeightFromWeight(ShipmentPM.GrossWeightUnitCode, ShipmentPM.ChargeableWeightUnitCode, PackagePM.Weight);
                }
            }

            else
            {
                myResult = this.GetWeightFromDimentions(PackagePM.Width, PackagePM.Height, PackagePM.Length, PackagePM.Quantity, ShipmentPM.DimensionsUnitCode, ShipmentPM.ChargeableWeightUnitCode, ShipmentPM.Ratio);
            }

            if (myResult != null)
            {
                myResult = MethodHelper.Round(myResult, 3);
            }

            PackagePM.VolumetricWeight = myResult;
        }



        private void CopyRoutingDelivery(ShipmentPM oldShipmentPM, ShipmentPM newShipmentPM, string DeliveryId, int SplitIndex)
        {
            ShipmentDeliveryPM oldDeliveryPM = oldShipmentPM.ShipmentDeliveries.Where(d => d.Id == DeliveryId).FirstOrDefault();
            if (oldDeliveryPM != null)
            {
                oldDeliveryPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;

                ShipmentDeliveryPM newDeliveryPM = new ShipmentDeliveryPM()
                {
                    Id = null,
                    ShipmentId = null,
                    ShipmentNumber = null,
                    MasterNumber = null,
                    ConnectedPackageId = null,
                    PickUpDeliveryNumber = null,
                    AgentId = oldDeliveryPM.AgentId,
                    AgentName = oldDeliveryPM.AgentName,
                    ATA = oldDeliveryPM.ATA,
                    ATD = oldDeliveryPM.ATD,
                    CarrierCode = oldDeliveryPM.CarrierCode,
                    CarrierId = oldDeliveryPM.CarrierId,
                    CarrierName = oldDeliveryPM.CarrierName,
                    CarrierNumber = oldDeliveryPM.CarrierNumber,
                    CarrierWebSite = oldDeliveryPM.CarrierWebSite,
                    ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                    CustomerId = oldDeliveryPM.CustomerId,
                    DirectionId = oldDeliveryPM.DirectionId,
                    Driver = oldDeliveryPM.Driver,
                    EmptyDeliveryContainerPartnerId = oldDeliveryPM.EmptyDeliveryContainerPartnerId,
                    EmptyDeliveryDepotReference = oldDeliveryPM.EmptyDeliveryDepotReference,
                    EmptyPickupContainerPartnerId = oldDeliveryPM.EmptyPickupContainerPartnerId,
                    EmptyPickupDepotReference = oldDeliveryPM.EmptyPickupDepotReference,
                    ETA = oldDeliveryPM.ETA,
                    ETD = oldDeliveryPM.ETD,
                    FromAddress = oldDeliveryPM.FromAddress,
                    FromAddressCity = oldDeliveryPM.FromAddressCity,
                    FromAddressCity_Dummy = oldDeliveryPM.FromAddressCity_Dummy,
                    FromAddressCountryCode = oldDeliveryPM.FromAddressCountryCode,
                    FromAddressCountryId = oldDeliveryPM.FromAddressCountryId,
                    FromAddressCountryName = oldDeliveryPM.FromAddressCountryName,
                    FromAddressId = oldDeliveryPM.FromAddressId,
                    FromAddressZipCode = oldDeliveryPM.FromAddressZipCode,
                    FromLocation = oldDeliveryPM.FromLocation,
                    FromPartnerCardId = oldDeliveryPM.FromPartnerCardId,
                    FromPortCode = oldDeliveryPM.FromPortCode,
                    FromPortCountryCode = oldDeliveryPM.FromPortCountryCode,
                    FromPortCountryName = oldDeliveryPM.FromPortCountryName,
                    FromPortId = oldDeliveryPM.FromPortId,
                    FromPortName = oldDeliveryPM.FromPortName,
                    FullResponsibility = oldDeliveryPM.FullResponsibility,
                    IsCancelled = oldDeliveryPM.IsCancelled,
                    Notes = oldDeliveryPM.Notes,
                    PackageTEU = oldDeliveryPM.PackageTEU,
                    PickUpDeliveryFromTypeCode = oldDeliveryPM.PickUpDeliveryFromTypeCode,
                    PickUpDeliveryToTypeCode = oldDeliveryPM.PickUpDeliveryToTypeCode,
                    PickUpDeliveryTypeCode = oldDeliveryPM.PickUpDeliveryTypeCode,
                    Tenant = oldDeliveryPM.Tenant,
                    ToAddress = oldDeliveryPM.ToAddress,
                    ToAddressCity = oldDeliveryPM.ToAddressCity,
                    ToAddressCity_Dummy = oldDeliveryPM.ToAddressCity_Dummy,
                    ToAddressCountryCode = oldDeliveryPM.ToAddressCountryCode,
                    ToAddressCountryId = oldDeliveryPM.ToAddressCountryId,
                    ToAddressCountryName = oldDeliveryPM.ToAddressCountryName,
                    ToAddressId = oldDeliveryPM.ToAddressId,
                    ToAddressZipCode = oldDeliveryPM.ToAddressZipCode,
                    ToLocation = oldDeliveryPM.ToLocation,
                    ToPartnerCardId = oldDeliveryPM.ToPartnerCardId,
                    ToPortCode = oldDeliveryPM.ToPortCode,
                    ToPortCountryCode = oldDeliveryPM.ToPortCountryCode,
                    ToPortCountryName = oldDeliveryPM.ToPortCountryName,
                    ToPortId = oldDeliveryPM.ToPortId,
                    ToPortName = oldDeliveryPM.ToPortName,
                    TrailerNumber = oldDeliveryPM.TrailerNumber,
                    TruckNumber = oldDeliveryPM.TruckNumber,
                };

                foreach(ShipmentPickUpDeliveryPackagePM itemPM in oldDeliveryPM.ShipmentPickUpDeliveryPackages)
                {
                    newDeliveryPM.ShipmentPickUpDeliveryPackages.Add(new ShipmentPickUpDeliveryPackagePM()
                    {
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        ContainerNumber = null,
                        Description = itemPM.Description,
                        Harmonize = itemPM.Harmonize,
                        Height = itemPM.Height,
                        Id = null,
                        Length = itemPM.Length,
                        PackageTypeId = itemPM.PackageTypeId,
                        PackageTypeName = itemPM.PackageTypeName,
                        PackageTypeTEU = itemPM.PackageTypeTEU,
                        Quantity = itemPM.Quantity,
                        ShipperSeal = itemPM.ShipperSeal,
                        ShipmentPickUpDeliveryId = null,
                        Tenant = itemPM.Tenant,
                        Volume = itemPM.Volume,
                        Weight = itemPM.Weight,
                        Width = itemPM.Width,
                    });
                }

                newDeliveryPM.IsFromSplit = true;
                newDeliveryPM.SplitIndex = SplitIndex;
                newShipmentPM.ShipmentDeliveries.Add(newDeliveryPM);
            }
        }
        private void CalculateShipmentAmounts(ShipmentPM entityPM)
        {
            if (entityPM != null)
            {
                List<ShipmentPackagePM> list = entityPM.ShipmentPackages.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).ToList();
                if (list.Count == 0)
                {
                    entityPM.TEU = null;
                    entityPM.NumberOfPackages = null;
                    entityPM.Volume = null;
                    entityPM.GrossWeight = null;
                    entityPM.VolumetricWeight = null;
                    entityPM.ChargeableWeight = null;
                    entityPM.AWBCommodityItemNumber = null;
                    entityPM.GrossWeightEdited = false;
                    entityPM.ChargeableWeightEdited = false;
                }

                else
                {
                    entityPM.TEU = list.Sum(s => s.TEU);
                    entityPM.NumberOfPackages = list.Sum(s => s.Quantity);
                    entityPM.Volume = MethodHelper.Round(list.Sum(s => s.Volume), 3);
                    entityPM.VolumetricWeight = MethodHelper.Round(list.Sum(s => s.VolumetricWeight), 3);

                    if (!entityPM.GrossWeightEdited)
                    {
                        entityPM.GrossWeight = MethodHelper.Round(list.Sum(s => s.Weight), 3);
                    }

                    if (!entityPM.ChargeableWeightEdited)
                    {
                        entityPM.ChargeableWeight = this.CalculateChargeableWeight(entityPM.GrossWeight, entityPM.VolumetricWeight, entityPM.GrossWeightUnitCode, entityPM.ChargeableWeightUnitCode, entityPM.DirectionId, entityPM.TransportModeId);
                    }
                }
            }
        }
        private double? CalculateChargeableWeight(object grossWeight, object volumetricWeight, string grossWeightUnitCode, string chargeableWeightUnitCode, string directionId, string transportModeId)
        {
            double? myGrossWeight = null;
            double? myVolumetricWeight = null;

            if (grossWeight != null)
            {
                myGrossWeight = Convert.ToDouble(grossWeight);
            }

            if (volumetricWeight != null)
            {
                myVolumetricWeight = Convert.ToDouble(volumetricWeight);
            }

            double? myResult = null;

            if (myGrossWeight != null || myVolumetricWeight != null)
            {

                double? grossWeightInVolumetricUnit = this.GetWeightFromWeight(grossWeightUnitCode, chargeableWeightUnitCode, myGrossWeight);

                if (grossWeightInVolumetricUnit != null && myVolumetricWeight == null)
                {
                    myResult = grossWeightInVolumetricUnit;
                }

                if (grossWeightInVolumetricUnit == null && myVolumetricWeight != null)
                {
                    myResult = myVolumetricWeight;
                }

                if (grossWeightInVolumetricUnit != null && myVolumetricWeight != null)
                {
                    myResult = grossWeightInVolumetricUnit > myVolumetricWeight ? grossWeightInVolumetricUnit : myVolumetricWeight;
                }
            }

            if (myResult != null)
            {
                myResult = Round(myResult, chargeableWeightUnitCode, directionId, transportModeId);
            }

            return myResult;
        }
        private double? Round(object args, string chargeableWeightUnitCode, string directionId, string transportModeId)
        {
            double? myArgs = null;

            if (args != null)
            {
                myArgs = Convert.ToDouble(args);
            }

            double? result = myArgs;

            if (chargeableWeightUnitCode != "MT")
            {
                if (directionId == "E" && transportModeId == "A")
                {
                    if (result != null)
                    {
                        string toString = result.ToString();
                        string[] r = toString.Split('.');

                        if (r.Length > 1)
                        {
                            string strDigits = "0." + r[1];
                            double? digits = Convert.ToDouble(strDigits);
                            double? integer = Convert.ToDouble(r[0]);

                            if (digits <= 0.5)
                            {
                                result = integer + 0.5;
                            }

                            else
                            {
                                result = integer + 1;
                            }
                        }
                    }
                }
            }

            return result;
        }
        private double? GetWeightFromWeight(string fromWeightCode, string toWeightCode, object weight)
        {
            double? myWeight = null;

            if (weight != null)
            {
                myWeight = Convert.ToDouble(weight);
            }

            double? myResult = null;
            double? factorOfConvert = 1;

            if (weight != null)
            {
                if (string.IsNullOrEmpty(fromWeightCode))
                {
                    factorOfConvert = 1;
                }

                else
                {
                    switch (fromWeightCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }     // 1 LB = 0.45359237 KG
                        case "MT": { factorOfConvert = 1000; break; }           // 1 mt = 1000 KG
                    }
                }

                double? weightInKilograms = myWeight * factorOfConvert;

                if (string.IsNullOrEmpty(toWeightCode))
                {
                    factorOfConvert = 1;
                }

                else
                {
                    switch (toWeightCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }     // 1 LB = 0.45359237 KG
                        case "MT": { factorOfConvert = 1000; break; }           // 1 mt = 1000 KG
                    }
                }

                myResult = weightInKilograms / factorOfConvert;
            }

            if (myResult != null)
            {
                myResult = MethodHelper.Round(myResult.Value, 3);
            }

            return myResult;
        }
        private double? GetWeightFromVolume(string volumeCode, string weightCode, object volume, object ratio)
        {
            double? myVolume = null;
            double? myRatio = null;

            if (volume != null)
            {
                myVolume = Convert.ToDouble(volume);
            }

            if (ratio != null)
            {
                myRatio = Convert.ToDouble(ratio);
            }

            double? myResult = null;
            double? factorOfConvert = 1;

            if (volume != null)
            {
                double? volumeInCBM = null;
                double? weightInKilograms = null;

                if (string.IsNullOrEmpty(volumeCode))
                {
                    factorOfConvert = 1;
                }

                else
                {
                    switch (volumeCode.ToUpper())
                    {
                        case "CBM": { factorOfConvert = 1; break; }
                        case "CBI": { factorOfConvert = 61024; break; }      // 1m³ = 61024in³
                        case "CBF": { factorOfConvert = 35.315; break; }     // 1m³ = 35.315ft³
                    }
                }

                volumeInCBM = myVolume / factorOfConvert;
                weightInKilograms = (volumeInCBM * 1000) / myRatio;

                if (string.IsNullOrEmpty(weightCode))
                {
                    factorOfConvert = 1;
                }

                else
                {
                    switch (weightCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }     // 1 LB = 0.45359237 KG
                        case "MT": { factorOfConvert = 1000; break; }           // 1 mt = 1000 KG
                    }
                }

                myResult = weightInKilograms / factorOfConvert;
            }

            if (myResult != null)
            {
                myResult = MethodHelper.Round(myResult.Value, 3);
            }

            return myResult;
        }
        private double? GetWeightFromDimentions(object width, object height, object length, object quantity, string dimentionCode, string weightCode, object ratio)
        {
            double? myWidth = null;
            double? myHeight = null;
            double? myLength = null;
            double? myRatio = null;
            double? myQuantity = null;

            if (width != null)
            {
                myWidth = Convert.ToDouble(width);
            }

            if (height != null)
            {
                myHeight = Convert.ToDouble(height);
            }

            if (length != null)
            {
                myLength = Convert.ToDouble(length);
            }

            if (ratio != null)
            {
                myRatio = Convert.ToDouble(ratio);
            }

            if (quantity != null)
            {
                myQuantity = Convert.ToDouble(quantity);
            }

            double? myResult = null;
            double factorOfConvert = 1;

            if (string.IsNullOrEmpty(dimentionCode))
            {
                factorOfConvert = 1;
            }

            else
            {
                switch (dimentionCode.ToUpper())
                {
                    case "CM": { factorOfConvert = 1; break; }
                    case "INC": { factorOfConvert = 2.54; break; }      // 1 inch = 2.54 centimeters
                    case "FT": { factorOfConvert = 30.48; break; }      // 1ft = 30.48 centimeters
                }
            }

            double? volumeInCentimeters = myQuantity * (myWidth * myHeight * myLength) * Math.Pow(factorOfConvert, 3);
            double? volumeInCBM = volumeInCentimeters / Math.Pow(100, 3);

            double? weightInKilograms = (volumeInCBM * 1000) / myRatio;

            if (string.IsNullOrEmpty(weightCode))
            {
                factorOfConvert = 1;
            }

            else
            {
                switch (weightCode.ToUpper())
                {
                    case "KG": { factorOfConvert = 1; break; }
                    case "LB": { factorOfConvert = 0.45359237; break; }     // 1 LB = 0.45359237 KG
                    case "MT": { factorOfConvert = 1000; break; }           // 1 mt = 1000 KG
                }
            }

            myResult = weightInKilograms / factorOfConvert;

            if (myResult != null)
            {
                myResult = MethodHelper.Round(myResult.Value, 3);
            }

            return myResult;
        }
    }
    public class SplitShipmentHelper
    {
        public string OldShipmentId { get; set; }
        public string NewShipmentId { get; set; }
        public ShipmentPM Shipment { get; set; }
        public List<SplitPackage> SplitPackages { get; set; }
    }
    public class SplitPackage
    {
        public string Id { get; set; }
        public bool IsSplit { get; set; }
        public bool IsPartialSplit { get; set; }
        public string ParentId { get; set; }
        public int? Quantity { get; set; }
        public double? Weight { get; set; }
        public double? Volume { get; set; }
    }
}