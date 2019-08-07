"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../Infrastructure/Tools");
var Validator_1 = require("../Infrastructure/Validators/Validator");
var FeatureLocator_1 = require("../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../Infrastructure/Utilities/TextCodeTranslator");
var ShipmentPM_1 = require("./EntityPMs/ShipmentPM");
var ShipmentPayablePM_1 = require("./EntityPMs/ShipmentPayablePM");
var ShipmentReceivablePM_1 = require("./EntityPMs/ShipmentReceivablePM");
var ShipmentAWBPrintOnlyPM_1 = require("./EntityPMs/ShipmentAWBPrintOnlyPM");
var ShipmentOrderPackagePM_1 = require("./EntityPMs/ShipmentOrderPackagePM");
var ShipmentPackagePM_1 = require("./EntityPMs/ShipmentPackagePM");
var InsideShipmentPackagePM_1 = require("./EntityPMs/InsideShipmentPackagePM");
var CurrencyListService_1 = require("../Common/Services/StandardLists/CurrencyListService");
var PackageTypeListService_1 = require("../Common/Services/StandardLists/PackageTypeListService");
var ChargesTypeListService_1 = require("../Common/Services/StandardLists/ChargesTypeListService");
var ShipmentTool = /** @class */ (function () {
    function ShipmentTool() {
    }
    ShipmentTool.IsEditingEnabled = function (entityPM) {
        var myResult = true;
        var d = "";
        if (entityPM.IsOperationalClosed) {
            myResult = false;
        }
        else if (entityPM.IsAccountingClosed) {
            myResult = false;
        }
        else if (entityPM.IsCancelled) {
            myResult = false;
        }
        return myResult;
    };
    ShipmentTool.IsInlandDomestic = function (entityPM) {
        var isInlandDomestic = false;
        if (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I") {
            isInlandDomestic = true;
        }
        return isInlandDomestic;
    };
    ShipmentTool.IsLCL = function (entityPM) {
        var result = false;
        if (entityPM != null) {
            if (entityPM.TransportModeId == "A") {
                result = true;
            }
            else if (entityPM.TransportModeId == "O" && entityPM.ShipmentTypeId == "LCLD") {
                result = true;
            }
            else if (entityPM.TransportModeId == "I" && entityPM.ShipmentTypeId == "LTL") {
                result = true;
            }
        }
        return result;
    };
    ShipmentTool.ComputeSCI = function (entityPM) {
        var myResult = null;
        if (entityPM.MainCarriageFromPortId != null && entityPM.FinalDistenationPortId != null) {
            if (entityPM.FromCountryIsEC == true && entityPM.ToCountryIsEC == true) {
                myResult = "C";
            }
            else if (entityPM.FromCountryIsEC == false && entityPM.ToCountryIsEC == true) {
                myResult = "T1";
            }
            else {
                myResult = "X";
            }
        }
        entityPM.SCI = myResult;
    };
    ShipmentTool.BuildAWBPlaceField = function (entityPM) {
        var myResult = null;
        if (entityPM.MainCarriageFromPortId != null) {
            myResult = entityPM.MainCarriageFromPortName + " " + entityPM.MainCarriageFromPortCountryName;
        }
        if (entityPM.AWBPlace != myResult) {
            entityPM.AWBPlace = myResult;
        }
    };
    ShipmentTool.GetLongMasterField = function (myTransportModeId, myAirlinePrefix, myMaster) {
        var myField = null;
        if (myTransportModeId == "A") {
            if (!Tools_1.AppTool.IsNullOrEmpty(myAirlinePrefix) && !Tools_1.AppTool.IsNullOrEmpty(myMaster)) {
                myField = myAirlinePrefix + "-" + myMaster;
            }
        }
        else {
            myField = myMaster;
        }
        return myField;
    };
    ShipmentTool.IsFullAWBWizard = function (DirectionId) {
        var isFullWizard = false;
        if (DirectionId == "E" || DirectionId == "R") {
            isFullWizard = true;
        }
        else if (DirectionId == "D") {
            if (!FeatureLocator_1.FeatureLocator.IsPackage_EAWB()) {
                isFullWizard = true;
            }
        }
        return isFullWizard;
    };
    ShipmentTool.GetAWBWizardHeader = function (ShipmentLevelCode, DirectionId) {
        var myResult = null;
        var isFullWizard = this.IsFullAWBWizard(DirectionId);
        if (isFullWizard) {
            switch (ShipmentLevelCode) {
                case "D": {
                    myResult = "Direct AWB Wizard";
                    break;
                }
                case "H": {
                    myResult = "House AWB Wizard";
                    break;
                }
                case "C": {
                    myResult = "Master AWB Wizard";
                    break;
                }
                default: {
                    break;
                }
            }
        }
        else {
            myResult = "Airline statuses";
        }
        return myResult;
    };
    ShipmentTool.MapTenantZeroAirline = function (entityPM, myAirlinePM) {
        if (myAirlinePM == null || myAirlinePM === undefined) {
            if (entityPM.TenantZeroAirlineId != null) {
                entityPM.TenantZeroAirlineId = null;
            }
            if (entityPM.TenantZeroAirlineTTY != null) {
                entityPM.TenantZeroAirlineTTY = null;
            }
            if (entityPM.TenantZeroAirlinePIMA != null) {
                entityPM.TenantZeroAirlinePIMA = null;
            }
            if (entityPM.TenantZeroAirlineChampFWB != false) {
                entityPM.TenantZeroAirlineChampFWB = false;
            }
            if (entityPM.TenantZeroAirlineChampFHL != false) {
                entityPM.TenantZeroAirlineChampFHL = false;
            }
            if (entityPM.TenantZeroAirlineChampFSU != false) {
                entityPM.TenantZeroAirlineChampFSU = false;
            }
            if (entityPM.TenantZeroAirlineChampFVRFVA != false) {
                entityPM.TenantZeroAirlineChampFVRFVA = false;
            }
            if (entityPM.TenantZeroAirlineChampFSRFSA != false) {
                entityPM.TenantZeroAirlineChampFSRFSA = false;
            }
            if (entityPM.TenantZeroAirlineChampNeedsRegistration != false) {
                entityPM.TenantZeroAirlineChampNeedsRegistration = false;
            }
            if (entityPM.TenantZeroAirlineGLSHKFWB != false) {
                entityPM.TenantZeroAirlineGLSHKFWB = false;
            }
            if (entityPM.TenantZeroAirlineGLSHKFHL != false) {
                entityPM.TenantZeroAirlineGLSHKFHL = false;
            }
            if (entityPM.TenantZeroAirlineGLSHKFSU != false) {
                entityPM.TenantZeroAirlineGLSHKFSU = false;
            }
            if (entityPM.TenantZeroAirlineGLSHKFVRFVA != false) {
                entityPM.TenantZeroAirlineGLSHKFVRFVA = false;
            }
            if (entityPM.TenantZeroAirlineGLSHKFSRFSA != false) {
                entityPM.TenantZeroAirlineGLSHKFSRFSA = false;
            }
            if (entityPM.TenantZeroAirlineGLSHKNeedsRegistration != false) {
                entityPM.TenantZeroAirlineGLSHKNeedsRegistration = false;
            }
        }
        else {
            if (entityPM.TenantZeroAirlineId != myAirlinePM.Id) {
                entityPM.TenantZeroAirlineId = myAirlinePM.Id;
            }
            if (entityPM.TenantZeroAirlineTTY != myAirlinePM.TTY) {
                entityPM.TenantZeroAirlineTTY = myAirlinePM.TTY;
            }
            if (entityPM.TenantZeroAirlinePIMA != myAirlinePM.GLSHKPIMA) {
                entityPM.TenantZeroAirlinePIMA = myAirlinePM.GLSHKPIMA;
            }
            if (entityPM.TenantZeroAirlineChampFWB != myAirlinePM.ChampFWB) {
                entityPM.TenantZeroAirlineChampFWB = myAirlinePM.ChampFWB;
            }
            if (entityPM.TenantZeroAirlineChampFHL != myAirlinePM.ChampFHL) {
                entityPM.TenantZeroAirlineChampFHL = myAirlinePM.ChampFHL;
            }
            if (entityPM.TenantZeroAirlineChampFSU != myAirlinePM.ChampFSU) {
                entityPM.TenantZeroAirlineChampFSU = myAirlinePM.ChampFSU;
            }
            if (entityPM.TenantZeroAirlineChampFVRFVA != myAirlinePM.ChampFVRFVA) {
                entityPM.TenantZeroAirlineChampFVRFVA = myAirlinePM.ChampFVRFVA;
            }
            if (entityPM.TenantZeroAirlineChampFSRFSA != myAirlinePM.ChampFSRFSA) {
                entityPM.TenantZeroAirlineChampFSRFSA = myAirlinePM.ChampFSRFSA;
            }
            if (entityPM.TenantZeroAirlineChampNeedsRegistration != myAirlinePM.ChampNeedsRegistration) {
                entityPM.TenantZeroAirlineChampNeedsRegistration = myAirlinePM.ChampNeedsRegistration;
            }
            if (entityPM.TenantZeroAirlineGLSHKFWB != myAirlinePM.GLSHKFWB) {
                entityPM.TenantZeroAirlineGLSHKFWB = myAirlinePM.GLSHKFWB;
            }
            if (entityPM.TenantZeroAirlineGLSHKFHL != myAirlinePM.GLSHKFHL) {
                entityPM.TenantZeroAirlineGLSHKFHL = myAirlinePM.GLSHKFHL;
            }
            if (entityPM.TenantZeroAirlineGLSHKFSU != myAirlinePM.GLSHKFSU) {
                entityPM.TenantZeroAirlineGLSHKFSU = myAirlinePM.GLSHKFSU;
            }
            if (entityPM.TenantZeroAirlineGLSHKFVRFVA != myAirlinePM.GLSHKFVRFVA) {
                entityPM.TenantZeroAirlineGLSHKFVRFVA = myAirlinePM.GLSHKFVRFVA;
            }
            if (entityPM.TenantZeroAirlineGLSHKFSRFSA != myAirlinePM.GLSHKFSRFSA) {
                entityPM.TenantZeroAirlineGLSHKFSRFSA = myAirlinePM.GLSHKFSRFSA;
            }
            if (entityPM.TenantZeroAirlineGLSHKNeedsRegistration != myAirlinePM.GLSHKNeedsRegistration) {
                entityPM.TenantZeroAirlineGLSHKNeedsRegistration = myAirlinePM.GLSHKNeedsRegistration;
            }
        }
    };
    ShipmentTool.CopyShipment = function (shipmentPM, oldShipment) {
        // Main Fields
        shipmentPM.CopyFromShipmentId = oldShipment.Id;
        shipmentPM.IsCopyFromShipment = oldShipment.Id == null ? false : true;
        shipmentPM.BaseShipmentNumber = oldShipment.ShipmentNumber;
        shipmentPM.ShipmentLevelCode = oldShipment.ShipmentLevelCode;
        shipmentPM.DirectionId = oldShipment.DirectionId;
        shipmentPM.TransportModeId = oldShipment.TransportModeId;
        shipmentPM.ShipmentTypeId = oldShipment.ShipmentTypeId;
        shipmentPM.BranchId = oldShipment.BranchId;
        shipmentPM.DepartmentId = oldShipment.DepartmentId;
        shipmentPM.ProfitCurrencyId = oldShipment.ProfitCurrencyId;
        shipmentPM.ProfitExchangeRate = oldShipment.ProfitExchangeRate;
        shipmentPM.SalesmanUserId = oldShipment.SalesmanUserId;
        shipmentPM.AWBCurrencyId = oldShipment.AWBCurrencyId;
        shipmentPM.QuoteId = oldShipment.QuoteId;
        shipmentPM.IncotermId = oldShipment.IncotermId;
        shipmentPM.FreightPrepaidCollectId = oldShipment.FreightPrepaidCollectId;
        shipmentPM.OtherPrepaidCollectId = oldShipment.OtherPrepaidCollectId;
        shipmentPM.MoveTypeId = oldShipment.MoveTypeId;
        // FWB CCS Dummy fields
        shipmentPM.TenantZeroAirlineId = oldShipment.TenantZeroAirlineId;
        shipmentPM.TenantZeroAirlineTTY = oldShipment.TenantZeroAirlineTTY;
        shipmentPM.TenantZeroAirlinePIMA = oldShipment.TenantZeroAirlinePIMA;
        shipmentPM.TenantZeroAirlineChampFWB = oldShipment.TenantZeroAirlineChampFWB;
        shipmentPM.TenantZeroAirlineChampFHL = oldShipment.TenantZeroAirlineChampFHL;
        shipmentPM.TenantZeroAirlineChampFSU = oldShipment.TenantZeroAirlineChampFSU;
        shipmentPM.TenantZeroAirlineChampFSRFSA = oldShipment.TenantZeroAirlineChampFSRFSA;
        shipmentPM.TenantZeroAirlineChampFVRFVA = oldShipment.TenantZeroAirlineChampFVRFVA;
        shipmentPM.CarrierIsChampRegistered = oldShipment.CarrierIsChampRegistered;
        shipmentPM.TenantZeroAirlineChampNeedsRegistration = oldShipment.TenantZeroAirlineChampNeedsRegistration;
        shipmentPM.TenantZeroAirlineGLSHKFWB = oldShipment.TenantZeroAirlineGLSHKFWB;
        shipmentPM.TenantZeroAirlineGLSHKFHL = oldShipment.TenantZeroAirlineGLSHKFHL;
        shipmentPM.TenantZeroAirlineGLSHKFSU = oldShipment.TenantZeroAirlineGLSHKFSU;
        shipmentPM.TenantZeroAirlineGLSHKFSRFSA = oldShipment.TenantZeroAirlineGLSHKFSRFSA;
        shipmentPM.TenantZeroAirlineGLSHKFVRFVA = oldShipment.TenantZeroAirlineGLSHKFVRFVA;
        shipmentPM.CarrierIsGLSHKRegistered = oldShipment.CarrierIsGLSHKRegistered;
        shipmentPM.TenantZeroAirlineGLSHKNeedsRegistration = oldShipment.TenantZeroAirlineGLSHKNeedsRegistration;
        shipmentPM.CarrierIsCheckDigit = oldShipment.CarrierIsCheckDigit;
        shipmentPM.CarrierIsLimitedLength = oldShipment.CarrierIsLimitedLength;
        // AWB General Tab
        shipmentPM.SCI = oldShipment.SCI;
        shipmentPM.MainHarmonize = oldShipment.MainHarmonize;
        shipmentPM.AWBDeclaredValueForCarriage = oldShipment.AWBDeclaredValueForCarriage;
        shipmentPM.AWBDeclaredValueForCustoms = oldShipment.AWBDeclaredValueForCustoms;
        shipmentPM.AWBInsurrenceValue = oldShipment.AWBInsurrenceValue;
        shipmentPM.AWBCarrierTarrifReference = oldShipment.AWBCarrierTarrifReference;
        shipmentPM.ReferenceNumber = oldShipment.ReferenceNumber;
        shipmentPM.SupplementaryShipmentInformation1 = oldShipment.SupplementaryShipmentInformation1;
        shipmentPM.SupplementaryShipmentInformation2 = oldShipment.SupplementaryShipmentInformation2;
        shipmentPM.AWBSignature = oldShipment.AWBSignature;
        shipmentPM.AWBPlace = oldShipment.AWBPlace;
        shipmentPM.CASSCode = oldShipment.CASSCode;
        shipmentPM.AWBAccountingInformation = oldShipment.AWBAccountingInformation;
        shipmentPM.AWBHandlingInformation = oldShipment.AWBHandlingInformation;
        shipmentPM.AWBComments = oldShipment.AWBComments;
        shipmentPM.AWBSpecialHandlingCodeId1 = oldShipment.AWBSpecialHandlingCodeId1;
        shipmentPM.AWBSpecialHandlingCodeId2 = oldShipment.AWBSpecialHandlingCodeId2;
        shipmentPM.AWBSpecialHandlingCodeId3 = oldShipment.AWBSpecialHandlingCodeId3;
        shipmentPM.AWBSpecialHandlingCodeId4 = oldShipment.AWBSpecialHandlingCodeId4;
        shipmentPM.AWBSpecialHandlingCodeId5 = oldShipment.AWBSpecialHandlingCodeId5;
        shipmentPM.AWBSpecialHandlingCodeId6 = oldShipment.AWBSpecialHandlingCodeId6;
        shipmentPM.AWBSpecialHandlingCodeId7 = oldShipment.AWBSpecialHandlingCodeId7;
        shipmentPM.AWBSpecialHandlingCodeId8 = oldShipment.AWBSpecialHandlingCodeId8;
        shipmentPM.AWBSpecialHandlingCodeId9 = oldShipment.AWBSpecialHandlingCodeId9;
        shipmentPM.AccountingInformation1 = oldShipment.AccountingInformation1;
        shipmentPM.AccountingInformation2 = oldShipment.AccountingInformation2;
        shipmentPM.AccountingInformation3 = oldShipment.AccountingInformation3;
        shipmentPM.AccountingInformation4 = oldShipment.AccountingInformation4;
        shipmentPM.AccountingInformation5 = oldShipment.AccountingInformation5;
        shipmentPM.AccountingInformation6 = oldShipment.AccountingInformation6;
        shipmentPM.AccountingInformationIdentifierCode1 = oldShipment.AccountingInformationIdentifierCode1;
        shipmentPM.AccountingInformationIdentifierCode2 = oldShipment.AccountingInformationIdentifierCode2;
        shipmentPM.AccountingInformationIdentifierCode3 = oldShipment.AccountingInformationIdentifierCode3;
        shipmentPM.AccountingInformationIdentifierCode4 = oldShipment.AccountingInformationIdentifierCode4;
        shipmentPM.AccountingInformationIdentifierCode5 = oldShipment.AccountingInformationIdentifierCode5;
        shipmentPM.AccountingInformationIdentifierCode6 = oldShipment.AccountingInformationIdentifierCode6;
        shipmentPM.AWBChargesCodeCode = oldShipment.AWBChargesCodeCode;
        shipmentPM.AWBFreightAmountPrepaid = oldShipment.AWBFreightAmountPrepaid;
        shipmentPM.AWBFreightAmountCollect = oldShipment.AWBFreightAmountCollect;
        // Measurment
        shipmentPM.Ratio = oldShipment.Ratio;
        shipmentPM.DimFactor = oldShipment.DimFactor;
        shipmentPM.VolumeUnitCode = oldShipment.VolumeUnitCode;
        shipmentPM.GrossWeightUnitCode = oldShipment.GrossWeightUnitCode;
        shipmentPM.DimensionsUnitCode = oldShipment.DimensionsUnitCode;
        shipmentPM.ChargeableWeightUnitCode = oldShipment.ChargeableWeightUnitCode;
        // PickUpDelivery
        shipmentPM.IncludePickUp = oldShipment.IncludePickUp;
        shipmentPM.FromAddressCity = oldShipment.FromAddressCity;
        shipmentPM.FromAddressZipCode = oldShipment.FromAddressZipCode;
        shipmentPM.FromAddressCountryId = oldShipment.FromAddressCountryId;
        shipmentPM.PickUpAddressId = oldShipment.PickUpAddressId;
        shipmentPM.IncludeDelivery = oldShipment.IncludeDelivery;
        shipmentPM.ToAddressCity = oldShipment.ToAddressCity;
        shipmentPM.ToAddressZipCode = oldShipment.ToAddressZipCode;
        shipmentPM.ToAddressCountryId = oldShipment.ToAddressCountryId;
        shipmentPM.DeliveryAddressId = oldShipment.DeliveryAddressId;
        // AWBPrintOnlies
        oldShipment.ShipmentAWBPrintOnlies.forEach(function (item) {
            var newItem = new ShipmentAWBPrintOnlyPM_1.ShipmentAWBPrintOnlyPM(shipmentPM);
            newItem.ShipmentId = shipmentPM.Id;
            newItem.Tenant = shipmentPM.Tenant;
            newItem.CurrencyId = item.CurrencyId;
            newItem.CurrencyCode = item.CurrencyCode;
            newItem.ExchangeRate = item.ExchangeRate;
            newItem.DueTypeCode = item.DueTypeCode;
            newItem.DueTypeName = item.DueTypeName;
            newItem.IATACodeId = item.IATACodeId;
            newItem.IATACodeName = item.IATACodeName;
            newItem.MeasurementId = item.MeasurementId;
            newItem.MeasurementCode = item.MeasurementCode;
            newItem.PrepaidCollectId = item.PrepaidCollectId;
            shipmentPM.ShipmentAWBPrintOnlies.push(newItem);
        });
        if (oldShipment.IsBuildFromQuote) {
            this.CopyShipmentPackages(shipmentPM, oldShipment, true);
        }
        // Routing
        shipmentPM.InterlineId = oldShipment.InterlineId;
        shipmentPM.AirlinePrefix = oldShipment.AirlinePrefix;
        shipmentPM.MainCarriageVesselId = oldShipment.MainCarriageVesselId;
        shipmentPM.MainCarriageFromPartnerId = oldShipment.MainCarriageFromPartnerId;
        shipmentPM.MainCarriageToPartnerId = oldShipment.MainCarriageToPartnerId;
        shipmentPM.MainCarriageToAddressId = oldShipment.MainCarriageToAddressId;
        shipmentPM.MainCarriageFromAddressId = oldShipment.MainCarriageFromAddressId;
        shipmentPM.Driver = oldShipment.Driver;
        shipmentPM.TrailerNumber = oldShipment.TrailerNumber;
        shipmentPM.TruckNumber = oldShipment.TruckNumber;
        shipmentPM.AccountNumber = oldShipment.AccountNumber;
        shipmentPM.MainCarriageCarrierId = oldShipment.MainCarriageCarrierId;
        shipmentPM.MainCarriageCarrierNumber = oldShipment.MainCarriageCarrierNumber;
        shipmentPM.MainCarriageCarrierCode = oldShipment.MainCarriageCarrierCode;
        shipmentPM.MainCarriageCarrierPrefix = oldShipment.MainCarriageCarrierPrefix;
        shipmentPM.FromPortId = oldShipment.MainCarriageFromPortId;
        shipmentPM.MainCarriageFromPortId = oldShipment.MainCarriageFromPortId;
        shipmentPM.MainCarriageFromPortCode = oldShipment.MainCarriageFromPortCode;
        shipmentPM.MainCarriageFromPortName = oldShipment.MainCarriageFromPortName;
        shipmentPM.MainCarriageFromPortCountryCode = oldShipment.MainCarriageFromPortCountryCode;
        shipmentPM.MainCarriageFromPortCountryName = oldShipment.MainCarriageFromPortCountryName;
        shipmentPM.ToPortId = oldShipment.MainCarriageToPortId;
        shipmentPM.MainCarriageToPortId = oldShipment.MainCarriageToPortId;
        shipmentPM.MainCarriageToPortCode = oldShipment.MainCarriageToPortCode;
        shipmentPM.MainCarriageToPortName = oldShipment.MainCarriageToPortName;
        shipmentPM.MainCarriageToPortCountryCode = oldShipment.MainCarriageToPortCountryCode;
        shipmentPM.MainCarriageToPortCountryName = oldShipment.MainCarriageToPortCountryName;
        shipmentPM.FinalDistenationPortId = oldShipment.FinalDistenationPortId;
        shipmentPM.MainCarriageFinalDestinationPortId = oldShipment.MainCarriageFinalDestinationPortId;
        shipmentPM.ValueOfGoods = oldShipment.ValueOfGoods;
        shipmentPM.ValueOfGoodsCurrencyId = oldShipment.ValueOfGoodsCurrencyId;
    };
    ShipmentTool.CopyShipmentPackages = function (shipmentPM, oldShipment, copyOtherProperties) {
        if (copyOtherProperties) {
            shipmentPM.IsDangerous = oldShipment.IsDangerous;
            shipmentPM.DescriptionOfGoods = oldShipment.DescriptionOfGoods;
            shipmentPM.BookingVolume = oldShipment.BookingVolume;
            shipmentPM.OrderVolumetricWeight = oldShipment.OrderVolumetricWeight;
            shipmentPM.OrderGrossWeight = oldShipment.OrderGrossWeight;
            shipmentPM.OrderChargeableWeight = oldShipment.OrderChargeableWeight;
            shipmentPM.OrderGrossWeightEdited = oldShipment.OrderGrossWeightEdited;
            shipmentPM.OrderChargeableWeightEdited = oldShipment.OrderChargeableWeightEdited;
            shipmentPM.BookingNumberOfPackages = oldShipment.BookingNumberOfPackages;
            shipmentPM.OrderIsDangerouseGoods = oldShipment.OrderIsDangerouseGoods;
            shipmentPM.GrossWeight = oldShipment.GrossWeight;
            shipmentPM.GrossWeightInKG = oldShipment.GrossWeightInKG;
            shipmentPM.GrossWeightPerTon = oldShipment.GrossWeightPerTon;
            shipmentPM.Volume = oldShipment.Volume;
            shipmentPM.VolumeInCBM = oldShipment.VolumeInCBM;
            shipmentPM.ChargeableWeight = oldShipment.ChargeableWeight;
            shipmentPM.ChargeableWeightInKG = oldShipment.ChargeableWeightInKG;
            shipmentPM.VolumetricWeight = oldShipment.VolumetricWeight;
            shipmentPM.NumberOfContainers = oldShipment.NumberOfContainers;
            shipmentPM.NumberOfPackages = oldShipment.NumberOfPackages;
            shipmentPM.TEU = oldShipment.TEU;
            shipmentPM.Quantity1 = oldShipment.Quantity1;
            shipmentPM.Quantity2 = oldShipment.Quantity2;
            shipmentPM.Quantity3 = oldShipment.Quantity3;
            shipmentPM.Quantity4 = oldShipment.Quantity4;
            shipmentPM.Quantity5 = oldShipment.Quantity5;
            shipmentPM.PackageTypeId1 = oldShipment.PackageTypeId1;
            shipmentPM.PackageTypeId2 = oldShipment.PackageTypeId2;
            shipmentPM.PackageTypeId3 = oldShipment.PackageTypeId3;
            shipmentPM.PackageTypeId4 = oldShipment.PackageTypeId4;
            shipmentPM.PackageTypeId5 = oldShipment.PackageTypeId5;
            shipmentPM.IsMultipleCommodities = oldShipment.IsMultipleCommodities;
            shipmentPM.AWBCommodityItemNumber = oldShipment.AWBCommodityItemNumber;
            shipmentPM.AWBChargeAmount = oldShipment.AWBChargeAmount;
            shipmentPM.AWBChargeRate = oldShipment.AWBChargeRate;
            shipmentPM.RateClassCode = oldShipment.RateClassCode;
        }
        oldShipment.ShipmentOrderPackages.forEach(function (item) {
            var newItem = new ShipmentOrderPackagePM_1.ShipmentOrderPackagePM(shipmentPM);
            newItem.Tenant = shipmentPM.Tenant;
            newItem.Quantity = item.Quantity;
            newItem.ContainerTypeId = item.ContainerTypeId;
            newItem.GrossWeight = item.GrossWeight;
            newItem.Height = item.Height;
            newItem.IsContainer = item.IsContainer;
            newItem.Length = item.Length;
            newItem.PackageTypeId = item.PackageTypeId;
            newItem.PackageTypeName = item.PackageTypeName;
            newItem.Volume = item.Volume;
            newItem.VolumetricWeight = item.VolumetricWeight;
            newItem.Width = item.Width;
            shipmentPM.ShipmentOrderPackages.push(newItem);
        });
        if (oldShipment.IsMultipleCommodities) {
            //foreach(ShipmentCommodityPM oldCommodity in oldShipment.ShipmentCommodities)
            //{
            //    ShipmentCommodityPM newCommodity = new ShipmentCommodityPM()
            //    {
            //        ChargeableWeight = oldCommodity.ChargeableWeight,
            //            ChargeAmount = oldCommodity.ChargeAmount,
            //            ChargeRate = oldCommodity.ChargeRate,
            //            GrossWeight = oldCommodity.GrossWeight,
            //            CommodityNumber = oldCommodity.CommodityNumber,
            //            NumberOfPackages = oldCommodity.NumberOfPackages,
            //            RateClassCode = oldCommodity.RateClassCode,
            //            Tenant = oldCommodity.Tenant,
            //            Volume = oldCommodity.Volume,
            //            VolumetricWeight = oldCommodity.VolumetricWeight,
            //            DescriptionOfGoods = oldCommodity.DescriptionOfGoods,
            //        };
            //    shipmentPM.ShipmentCommodities.Add(newCommodity);
            //    foreach(CommodityPackagePM oldPackage in oldCommodity.CommodityPackages)
            //    {
            //        CommodityPackagePM newPackage = new CommodityPackagePM()
            //        {
            //            Description = oldPackage.Description,
            //                Height = oldPackage.Height,
            //                Length = oldPackage.Length,
            //                PackageTypeId = oldPackage.PackageTypeId,
            //                Quantity = oldPackage.Quantity,
            //                Tenant = oldPackage.Tenant,
            //                Volume = oldPackage.Volume,
            //                Width = oldPackage.Width,
            //                Weight = oldPackage.Weight,
            //                VolumetricWeight = oldPackage.VolumetricWeight,
            //            };
            //        newCommodity.CommodityPackages.Add(newPackage);
            //    }
            //}
        }
        else {
            oldShipment.ShipmentPackages.forEach(function (item) {
                var newItem = new ShipmentPackagePM_1.ShipmentPackagePM(shipmentPM);
                newItem.Tenant = item.Tenant;
                newItem.ClassNumber = item.ClassNumber;
                newItem.Description = item.Description;
                newItem.FlashPoint = item.FlashPoint;
                newItem.Harmonize = item.Harmonize;
                newItem.Height = item.Height;
                newItem.IMDGCode = item.IMDGCode;
                newItem.IsContainer = item.IsContainer;
                newItem.IsDangerous = item.IsDangerous;
                newItem.Length = item.Length;
                newItem.MarksAndNumbers = item.MarksAndNumbers;
                newItem.MaterialDescription = item.MaterialDescription;
                newItem.PackageTypeId = item.PackageTypeId;
                newItem.PackageTypeName = item.PackageTypeName;
                newItem.PackagingGroup = item.PackagingGroup;
                newItem.Quantity = item.Quantity;
                newItem.ShipperSeal = item.ShipperSeal;
                newItem.CarrierSeal = item.CarrierSeal;
                newItem.SOC = item.SOC;
                newItem.Tare = item.Tare;
                newItem.Temperature = item.Temperature;
                newItem.UnNumber = item.UnNumber;
                newItem.Ventilation = item.Ventilation;
                newItem.Volume = item.Volume;
                newItem.VolumetricWeight = item.VolumetricWeight;
                newItem.Weight = item.Weight;
                newItem.Width = item.Width;
                shipmentPM.ShipmentPackages.push(newItem);
                item.InsideShipmentPackages.forEach(function (inside) {
                    var newItemInside = new InsideShipmentPackagePM_1.InsideShipmentPackagePM(item);
                    newItemInside.Tenant = inside.Tenant;
                    newItemInside.Description = inside.Description;
                    newItemInside.Height = inside.Height;
                    newItemInside.Length = inside.Length;
                    newItemInside.PackageTypeId = inside.PackageTypeId;
                    newItemInside.PackageTypeName = inside.PackageTypeName;
                    newItemInside.Quantity = inside.Quantity;
                    newItemInside.Volume = inside.Volume;
                    newItemInside.Weight = inside.Weight;
                    newItemInside.Width = inside.Width;
                    newItem.InsideShipmentPackages.push(newItemInside);
                });
            });
        }
    };
    ShipmentTool.CopyFlights = function (shipmentPM, oldShipment) {
        shipmentPM.Transshipment1FromPortId = oldShipment.Transshipment1FromPortId;
        shipmentPM.Transshipment1FromPortCode = oldShipment.Transshipment1FromPortCode;
        shipmentPM.Transshipment1FromPortName = oldShipment.Transshipment1FromPortName;
        shipmentPM.Transshipment1FromPortCountryCode = oldShipment.Transshipment1FromPortCountryCode;
        shipmentPM.Transshipment1FromPortCountryName = oldShipment.Transshipment1FromPortCountryName;
        shipmentPM.Transshipment1ToPortId = oldShipment.Transshipment1ToPortId;
        shipmentPM.Transshipment1ToPortCode = oldShipment.Transshipment1ToPortCode;
        shipmentPM.Transshipment1ToPortName = oldShipment.Transshipment1ToPortName;
        shipmentPM.Transshipment1ToPortCountryCode = oldShipment.Transshipment1ToPortCountryCode;
        shipmentPM.Transshipment1ToPortCountryName = oldShipment.Transshipment1ToPortCountryName;
        shipmentPM.Transshipment1CarrierId = oldShipment.Transshipment1CarrierId;
        shipmentPM.Transshipment1CarrierNumber = oldShipment.Transshipment1CarrierNumber;
        shipmentPM.Transshipment1CarrierCode = oldShipment.Transshipment1CarrierCode;
        shipmentPM.Transshipment1CarrierPrefix = oldShipment.Transshipment1CarrierPrefix;
        shipmentPM.Transshipment2FromPortId = oldShipment.Transshipment2FromPortId;
        shipmentPM.Transshipment2FromPortCode = oldShipment.Transshipment2FromPortCode;
        shipmentPM.Transshipment2FromPortName = oldShipment.Transshipment2FromPortName;
        shipmentPM.Transshipment2FromPortCountryCode = oldShipment.Transshipment2FromPortCountryCode;
        shipmentPM.Transshipment2FromPortCountryName = oldShipment.Transshipment2FromPortCountryName;
        shipmentPM.Transshipment2ToPortId = oldShipment.Transshipment2ToPortId;
        shipmentPM.Transshipment2ToPortCode = oldShipment.Transshipment2ToPortCode;
        shipmentPM.Transshipment2ToPortName = oldShipment.Transshipment2ToPortName;
        shipmentPM.Transshipment2ToPortCountryCode = oldShipment.Transshipment2ToPortCountryCode;
        shipmentPM.Transshipment2ToPortCountryName = oldShipment.Transshipment2ToPortCountryName;
        shipmentPM.Transshipment2CarrierId = oldShipment.Transshipment2CarrierId;
        shipmentPM.Transshipment2CarrierNumber = oldShipment.Transshipment2CarrierNumber;
        shipmentPM.Transshipment2CarrierCode = oldShipment.Transshipment2CarrierCode;
        shipmentPM.Transshipment2CarrierPrefix = oldShipment.Transshipment2CarrierPrefix;
        shipmentPM.Transshipment3FromPortId = oldShipment.Transshipment3FromPortId;
        shipmentPM.Transshipment3FromPortCode = oldShipment.Transshipment3FromPortCode;
        shipmentPM.Transshipment3FromPortName = oldShipment.Transshipment3FromPortName;
        shipmentPM.Transshipment3FromPortCountryCode = oldShipment.Transshipment3FromPortCountryCode;
        shipmentPM.Transshipment3FromPortCountryName = oldShipment.Transshipment3FromPortCountryName;
        shipmentPM.Transshipment3ToPortId = oldShipment.Transshipment3ToPortId;
        shipmentPM.Transshipment3ToPortCode = oldShipment.Transshipment3ToPortCode;
        shipmentPM.Transshipment3ToPortName = oldShipment.Transshipment3ToPortName;
        shipmentPM.Transshipment3ToPortCountryCode = oldShipment.Transshipment3ToPortCountryCode;
        shipmentPM.Transshipment3ToPortCountryName = oldShipment.Transshipment3ToPortCountryName;
        shipmentPM.Transshipment3CarrierId = oldShipment.Transshipment3CarrierId;
        shipmentPM.Transshipment3CarrierNumber = oldShipment.Transshipment3CarrierNumber;
        shipmentPM.Transshipment3CarrierCode = oldShipment.Transshipment3CarrierCode;
        shipmentPM.Transshipment3CarrierPrefix = oldShipment.Transshipment3CarrierPrefix;
    };
    ShipmentTool.MapBookingShipment = function (shipmentPM, bookingShipment) {
        //General
        shipmentPM.IsBuildFromBooking = true;
        shipmentPM.BookingId = bookingShipment.BookingId;
        shipmentPM.DirectionId = bookingShipment.DirectionId;
        shipmentPM.TransportModeId = bookingShipment.TransportModeId;
        shipmentPM.ShipmentTypeId = bookingShipment.ShipmentTypeId;
        shipmentPM.ShipmentLevelCode = bookingShipment.ShipmentLevelCode;
        shipmentPM.Notes = bookingShipment.Notes;
        shipmentPM.AirlinePrefix = bookingShipment.AirlinePrefix;
        shipmentPM.InterlineId = bookingShipment.InterlineId;
        shipmentPM.MAWBOBLDate = bookingShipment.MAWBOBLDate;
        shipmentPM.AWBPlace = bookingShipment.AWBPlace;
        shipmentPM.AccountNumber = bookingShipment.AccountNumber;
        //Totals
        shipmentPM.GrossWeightEdited = bookingShipment.GrossWeightEdited;
        shipmentPM.ChargeableWeightEdited = bookingShipment.ChargeableWeightEdited;
        shipmentPM.NumberOfPackages = bookingShipment.NumberOfPackages;
        shipmentPM.DescriptionOfGoods = bookingShipment.DescriptionOfGoods;
        shipmentPM.AWBCommodityItemNumber = bookingShipment.AWBCommodityItemNumber;
        shipmentPM.Ratio = bookingShipment.Ratio;
        shipmentPM.Volume = bookingShipment.Volume;
        shipmentPM.DimFactor = bookingShipment.DimFactor;
        shipmentPM.GrossWeight = bookingShipment.GrossWeight;
        shipmentPM.VolumetricWeight = bookingShipment.VolumetricWeight;
        shipmentPM.ChargeableWeight = bookingShipment.ChargeableWeight;
        shipmentPM.GrossWeightInKG = bookingShipment.GrossWeightInKG;
        shipmentPM.GrossWeightPerTon = bookingShipment.GrossWeightPerTon;
        shipmentPM.ChargeableWeightInKG = bookingShipment.ChargeableWeightInKG;
        shipmentPM.VolumeUnitCode = bookingShipment.VolumeUnitCode;
        shipmentPM.DimensionsUnitCode = bookingShipment.DimensionsUnitCode;
        shipmentPM.GrossWeightUnitCode = bookingShipment.GrossWeightUnitCode;
        shipmentPM.ChargeableWeightUnitCode = bookingShipment.ChargeableWeightUnitCode;
        //Routing
        shipmentPM.Master = bookingShipment.Master;
        shipmentPM.Routing = bookingShipment.Routing;
        shipmentPM.MainCarriageCarrierId = bookingShipment.MainCarriageCarrierId;
        shipmentPM.MainCarriageIsFromStack = bookingShipment.MainCarriageIsFromStack;
        shipmentPM.MainCarriageFromPortId = bookingShipment.MainCarriageFromPortId;
        shipmentPM.MainCarriageToPortId = bookingShipment.MainCarriageToPortId;
        shipmentPM.MainCarriageFinalDestinationPortId = bookingShipment.MainCarriageFinalDestinationPortId;
        shipmentPM.MainCarriageCarrierPrefix = bookingShipment.MainCarriageCarrierPrefix;
        shipmentPM.MainCarriageCarrierNumber = bookingShipment.MainCarriageCarrierNumber;
        shipmentPM.MainCarriageETD = bookingShipment.MainCarriageETD;
        shipmentPM.Transshipment1FromPortId = bookingShipment.Transshipment1FromPortId;
        shipmentPM.Transshipment1ToPortId = bookingShipment.Transshipment1ToPortId;
        shipmentPM.Transshipment1CarrierId = bookingShipment.Transshipment1CarrierId;
        shipmentPM.Transshipment1ETD = bookingShipment.Transshipment1ETD;
        shipmentPM.Transshipment1CarrierPrefix = bookingShipment.Transshipment1CarrierPrefix;
        shipmentPM.Transshipment1CarrierNumber = bookingShipment.Transshipment1CarrierNumber;
        shipmentPM.Transshipment2FromPortId = bookingShipment.Transshipment2FromPortId;
        shipmentPM.Transshipment2ToPortId = bookingShipment.Transshipment2ToPortId;
        shipmentPM.Transshipment2CarrierId = bookingShipment.Transshipment2CarrierId;
        shipmentPM.Transshipment2ETD = bookingShipment.Transshipment2ETD;
        shipmentPM.Transshipment2CarrierPrefix = bookingShipment.Transshipment2CarrierPrefix;
        shipmentPM.Transshipment2CarrierNumber = bookingShipment.Transshipment2CarrierNumber;
        //Partners
        shipmentPM.ShipperId = bookingShipment.ShipperId;
        shipmentPM.ShipperName = bookingShipment.ShipperName;
        shipmentPM.ShipperReference1 = bookingShipment.ShipperReference1;
        shipmentPM.ConsigneeId = bookingShipment.ConsigneeId;
        shipmentPM.ConsigneeName = bookingShipment.ConsigneeName;
        shipmentPM.ConsigneeReference1 = bookingShipment.ConsigneeReference1;
        shipmentPM.CustomerId = bookingShipment.ShipperId;
        shipmentPM.CustomerReference1 = bookingShipment.CustomerReference1;
        shipmentPM.CustomerName = bookingShipment.ShipperName;
        shipmentPM.ShipmentCustomerTypeCode = "SHI";
        shipmentPM.IssuingCarrierAgentId = bookingShipment.IssuingCarrierAgentId;
        shipmentPM.IssuingCarrierAddressId = bookingShipment.IssuingCarrierAddressId;
        shipmentPM.CASSCode = bookingShipment.CASSCode;
        shipmentPM.IssuingCarrierIATACode = bookingShipment.IssuingCarrierIATACode;
        shipmentPM.ViaColoader = (bookingShipment.IssuingCarrierAgentId == SessionLocator_1.SessionLocator.TenantPM.AgentId) ? false : true;
        //AWB Fields
        shipmentPM.AWBComments = bookingShipment.AWBComments;
        shipmentPM.AWBSpecialHandlingCodeId1 = bookingShipment.AWBSpecialHandlingCodeId1;
        shipmentPM.AWBSpecialHandlingCodeId2 = bookingShipment.AWBSpecialHandlingCodeId2;
        shipmentPM.AWBSpecialHandlingCodeId3 = bookingShipment.AWBSpecialHandlingCodeId3;
        shipmentPM.AWBSpecialHandlingCodeId4 = bookingShipment.AWBSpecialHandlingCodeId4;
        shipmentPM.AWBSpecialHandlingCodeId5 = bookingShipment.AWBSpecialHandlingCodeId5;
        shipmentPM.AWBSpecialHandlingCodeId6 = bookingShipment.AWBSpecialHandlingCodeId6;
        shipmentPM.AWBSpecialHandlingCodeId7 = bookingShipment.AWBSpecialHandlingCodeId7;
        shipmentPM.AWBSpecialHandlingCodeId8 = bookingShipment.AWBSpecialHandlingCodeId8;
        shipmentPM.AWBSpecialHandlingCodeId9 = bookingShipment.AWBSpecialHandlingCodeId9;
        shipmentPM.AWBCarrierTarrifReference = bookingShipment.AWBCarrierTarrifReference;
        //IsDangerous
        shipmentPM.IsDangerous = bookingShipment.IsDangerous;
        shipmentPM.DangerousClassNumber = bookingShipment.DangerousClassNumber;
        shipmentPM.DangerousUnNumber = bookingShipment.DangerousUnNumber;
        shipmentPM.DangerousPackagingGroup = bookingShipment.DangerousPackagingGroup;
        shipmentPM.DangerousIMDGCode = bookingShipment.DangerousIMDGCode;
        shipmentPM.DangerousFlashPoint = bookingShipment.DangerousFlashPoint;
        shipmentPM.DangerousMaterialDescription = bookingShipment.DangerousMaterialDescription;
        shipmentPM.MainHarmonize = bookingShipment.MainHarmonize;
        //Packages
        bookingShipment.ShipmentPackages.forEach(function (item) {
            var newItem = new ShipmentPackagePM_1.ShipmentPackagePM(shipmentPM);
            newItem.ShipmentId = shipmentPM.Id;
            newItem.ShipmentNumber = shipmentPM.ShipmentNumber;
            //newItem.ShipmentPM = shipmentPM;
            newItem.Tenant = item.Tenant;
            newItem.PackageTypeId = item.PackageTypeId;
            newItem.PackageTypeName = item.PackageTypeName;
            newItem.Description = item.Description;
            newItem.ContainerNumber = item.ContainerNumber;
            newItem.ShipperSeal = item.ShipperSeal;
            newItem.CarrierSeal = item.CarrierSeal;
            newItem.Quantity = item.Quantity;
            newItem.Weight = item.Weight;
            newItem.Volume = item.Volume;
            newItem.Height = item.Height;
            newItem.Length = item.Length;
            newItem.Width = item.Width;
            newItem.Tare = item.Tare;
            newItem.VolumetricWeight = item.VolumetricWeight;
            newItem.UnNumber = item.UnNumber;
            newItem.ClassNumber = item.ClassNumber;
            newItem.Temperature = item.Temperature;
            newItem.Ventilation = item.Ventilation;
            newItem.SOC = item.SOC;
            newItem.MarksAndNumbers = item.MarksAndNumbers;
            newItem.PackagingGroup = item.PackagingGroup;
            newItem.IMDGCode = item.IMDGCode;
            newItem.FlashPoint = item.FlashPoint;
            newItem.Harmonize = item.Harmonize;
            newItem.MaterialDescription = item.MaterialDescription;
            newItem.IsDangerous = item.IsDangerous;
            newItem.CommodityId = item.CommodityId;
            shipmentPM.ShipmentPackages.push(newItem);
        });
    };
    ShipmentTool.IsGLSHK = function () {
        if (SessionLocator_1.SessionLocator.TenantManagementJS.AWBMessagesCCSTypeCode == "GLSHK") {
            return true;
        }
        else {
            return false;
        }
    };
    ShipmentTool.GetTenantZeroAirlineField = function (shipmentPM) {
        var myResult = null;
        if (SessionLocator_1.SessionLocator.TenantManagementJS.AWBMessagesCCSTypeCode == "GLSHK") {
            myResult = shipmentPM.TenantZeroAirlinePIMA;
        }
        else {
            myResult = shipmentPM.TenantZeroAirlineTTY;
        }
        return myResult;
    };
    ShipmentTool.OnRegulatedAgentFieldChanged = function (entityPM) {
        if (entityPM != null) {
            if (!entityPM.AWBPrintingRANumberEdited) {
                var myComputedValue = this.ComputeAWBPrintingRANumber(entityPM);
                if (entityPM.AWBPrintingRANumber != myComputedValue) {
                    entityPM.AWBPrintingRANumber = myComputedValue;
                    entityPM.AWBPrintingRANumberEdited = false;
                }
            }
            if (!entityPM.AdditionalHandlingInfoEdited) {
                var myComputedValue = this.ComputeAdditionalHandlingInfo(entityPM);
                if (entityPM.AdditionalHandlingInfo != myComputedValue) {
                    var myOldValue = entityPM.AdditionalHandlingInfo;
                    entityPM.AdditionalHandlingInfo = myComputedValue;
                    entityPM.AdditionalHandlingInfoEdited = false;
                    this.OnAdditionalHandlingInfoChanged(entityPM, myOldValue, entityPM.AdditionalHandlingInfo);
                }
            }
            if (!entityPM.AWBPrintingSecurityStatusEdited) {
                var myComputedValue = this.ComputeAWBPrintingSecurityStatus(entityPM);
                if (entityPM.AWBPrintingSecurityStatusId != myComputedValue) {
                    entityPM.AWBPrintingSecurityStatusId = myComputedValue;
                    entityPM.AWBPrintingSecurityStatusEdited = false;
                    this.OnSecurityCodeChanged(entityPM);
                }
            }
        }
    };
    ShipmentTool.ComputeAWBPrintingRANumber = function (entityPM) {
        var myResult = null;
        if (entityPM.IsKnownCargo) {
            if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.ColoaderRANumber)) {
                myResult = entityPM.ColoaderRANumber;
            }
            else {
                if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.RegulatedAgentRANumber) && !Tools_1.AppTool.IsNullOrEmpty(entityPM.KnownConsignorNumber)) {
                    myResult = entityPM.RegulatedAgentRANumber;
                }
            }
        }
        return myResult;
    };
    ShipmentTool.ComputeAdditionalHandlingInfo = function (entityPM) {
        var myCode = null;
        var myResult = null;
        if (Tools_1.AppTool.IsNullOrEmpty(entityPM.KnownConsignorNumber)) {
            myCode = "UNK";
        }
        else {
            if (entityPM.IsKnownCargo) {
                myCode = "KCKC";
            }
            else {
                myCode = "UKKC";
            }
        }
        //AWBAdditionalHandlingInfoList myHandlingInfo = AWBAdditionalHandlingInfoDataProvider.GetCachedList<AWBAdditionalHandlingInfoList>().Where(d => d.Code == myCode).FirstOrDefault();
        //if (myHandlingInfo != null) {
        //    myResult = myHandlingInfo.PrintDescription;
        //}
        return myResult;
    };
    ShipmentTool.ComputeAWBPrintingSecurityStatus = function (entityPM) {
        var myResult = null;
        if (entityPM.IsKnownCargo) {
            var myCode = null;
            if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.ColoaderRANumber)) {
                if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.RegulatedAgentRANumber)) {
                    myCode = "SPX";
                }
            }
            else {
                if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.RegulatedAgentRANumber) && !Tools_1.AppTool.IsNullOrEmpty(entityPM.KnownConsignorNumber)) {
                    myCode = "SPX";
                }
            }
            //if (!AppTool.IsNullOrEmpty(myCode)) {
            //    AWBSpecialHandlingCodeList list = AWBSpecialHandlingCodeDataProvider.GetCachedList<AWBSpecialHandlingCodeList>().Where(d => d.Code == myCode).FirstOrDefault();
            //    if (list != null) {
            //        myResult = list.Id;
            //    }
            //}
        }
        return myResult;
    };
    ShipmentTool.OnSecurityCodeChanged = function (entityPM) {
        entityPM.AWBSpecialHandlingCodeId9 = entityPM.AWBPrintingSecurityStatusId;
    };
    ShipmentTool.OnAdditionalHandlingInfoChanged = function (entityPM, oldValue, newValue) {
        if (Tools_1.AppTool.IsNullOrEmpty(entityPM.AWBHandlingInformation)) {
            entityPM.AWBHandlingInformation = newValue;
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(oldValue)) {
                var index = entityPM.AWBHandlingInformation.indexOf(oldValue);
                if (index == 0) {
                    entityPM.AWBHandlingInformation = entityPM.AWBHandlingInformation.replace(entityPM.AWBHandlingInformation.substring(0, oldValue.length), "");
                }
            }
            if (entityPM.AWBHandlingInformation.startsWith('\n') || entityPM.AWBHandlingInformation.startsWith('\r')) {
                var index1 = entityPM.AWBHandlingInformation.indexOf('\n');
                if (index1 == 0 || index1 == 1) {
                    entityPM.AWBHandlingInformation = entityPM.AWBHandlingInformation.replace(entityPM.AWBHandlingInformation.substring(index1, 1), "");
                }
                var index2 = entityPM.AWBHandlingInformation.indexOf('\r');
                if (index2 == 0 || index2 == 1) {
                    entityPM.AWBHandlingInformation = entityPM.AWBHandlingInformation.replace(entityPM.AWBHandlingInformation.substring(index2, 1), "");
                }
            }
            if (Tools_1.AppTool.IsNullOrEmpty(entityPM.AWBHandlingInformation)) {
                entityPM.AWBHandlingInformation = newValue;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                entityPM.AWBHandlingInformation = newValue + '\n\r' + entityPM.AWBHandlingInformation;
            }
        }
    };
    ShipmentTool.GetRateClassGroupCode = function (rateClassCode) {
        var code = "";
        switch (rateClassCode) {
            case "M":
            case "B":
                {
                    code = "M";
                    break;
                }
            case "R":
            case "S":
            case "X":
            case "Y":
                {
                    code = "S";
                    break;
                }
            case "C":
            case "E":
            case "K":
            case "N":
            case "P":
            case "Q":
            case "U":
                {
                    code = "R";
                    break;
                }
            default: {
                break;
            }
        }
        return code;
    };
    ShipmentTool.BuildAWBChargesCodeCode = function (entityPM) {
        if (entityPM.FreightPrepaidCollectId == "P" && entityPM.OtherPrepaidCollectId == "P") {
            entityPM.AWBChargesCodeCode = "PP";
        }
        else if (entityPM.FreightPrepaidCollectId == "C" && entityPM.OtherPrepaidCollectId == "C") {
            entityPM.AWBChargesCodeCode = "CC";
        }
        else {
            entityPM.AWBChargesCodeCode = "PC";
        }
    };
    ShipmentTool.ComputeAWBChargeAmount = function (myRateClassCode, myChargeRate, myChargeableWeight) {
        var myResult = null;
        var groupCode = this.GetRateClassGroupCode(myRateClassCode);
        if (groupCode == "M") {
            myResult = myChargeRate;
        }
        else if (groupCode == "R") {
            myResult = myChargeRate * myChargeableWeight;
        }
        return myResult;
    };
    ShipmentTool.ValidateAddedPackagesCount = function (entityPM) {
        var myResult = "";
        if (!SessionLocator_1.SessionLocator.TenantPM.AllowEAWBMoreThanTenPackages) {
            if (entityPM.IsMultipleCommodities) {
                var allowdCount = 10;
                var myCount1 = entityPM.ShipmentCommodities.length;
                var myCount2 = 0;
                entityPM.ShipmentCommodities.forEach(function (item) {
                    if (item.CommodityPackages != null) {
                        myCount2 += item.CommodityPackages.length;
                    }
                });
                var totalCount = myCount1 + myCount2;
                if (totalCount > allowdCount) {
                    myResult = "You have exceeded the allowable limit of " + allowdCount + " lines of commodities and packages";
                }
            }
            else {
                var allowdCount = 9;
                if (entityPM.ShipmentPackages.length > allowdCount) {
                    myResult = "You have exceeded the allowable limit of " + allowdCount + " lines of packages";
                }
            }
        }
        return myResult;
    };
    ShipmentTool.ValidateAddingPackagesCount = function (currentCount) {
        var myResult = "";
        if (!SessionLocator_1.SessionLocator.TenantPM.AllowEAWBMoreThanTenPackages) {
            var allowdCount = 9;
            if (currentCount >= allowdCount) {
                myResult = "You have reached the limit of " + allowdCount + " lines of packages";
            }
        }
        return myResult;
    };
    ShipmentTool.ValidateAddingCommoditiesCount = function (entityPM) {
        var myResult = "";
        if (!SessionLocator_1.SessionLocator.TenantPM.AllowEAWBMoreThanTenPackages) {
            var allowdCount = 10;
            var myCount1 = entityPM.ShipmentCommodities.length;
            var myCount2 = 0;
            entityPM.ShipmentCommodities.forEach(function (item) {
                if (item.CommodityPackages != null) {
                    myCount2 += item.CommodityPackages.length;
                }
            });
            var totalCount = myCount1 + myCount2;
            if (totalCount >= allowdCount) {
                myResult = "You have reached the limit of " + allowdCount + " lines of commodities and packages";
            }
        }
        return myResult;
    };
    ShipmentTool.IsAirlineRuleFieldValid = function (myRule, myFieldValue) {
        var myResult = true;
        if (myRule != null) {
            if (myFieldValue == null || isNaN(myFieldValue)) {
                if (myRule.IsMandatoryForSending) {
                    myResult = false;
                }
            }
            else if (typeof (myFieldValue) == "string") {
                if (Tools_1.AppTool.IsNullOrEmpty(myFieldValue)) {
                    if (myRule.IsMandatoryForSending) {
                        myResult = false;
                    }
                }
                else if (myRule.MaxSize > 0) {
                    if (myFieldValue.length > myRule.MaxSize) {
                        myResult = false;
                    }
                }
            }
            else if (typeof (myFieldValue) == "number") {
                if (Tools_1.AppTool.IsNullOrZero(myFieldValue)) {
                    if (myRule.IsMandatoryForSending) {
                        myResult = false;
                    }
                }
            }
        }
        return myResult;
    };
    ShipmentTool.IsAdvancedAccountingInformation = function (entityPM) {
        var myResult = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.AccountingInformation1)) {
            myResult = true;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.AccountingInformation2)) {
            myResult = true;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.AccountingInformation3)) {
            myResult = true;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.AccountingInformation4)) {
            myResult = true;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.AccountingInformation5)) {
            myResult = true;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.AccountingInformation6)) {
            myResult = true;
        }
        return myResult;
    };
    ShipmentTool.IsParticipant1Filled = function (entityPM) {
        var myResult = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantIdCode1)) {
            myResult = true;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationCode1)) {
            myResult = true;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationPortCode1)) {
            myResult = true;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationName1)) {
            myResult = true;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationReference1)) {
            myResult = true;
        }
        return myResult;
    };
    ShipmentTool.IsParticipant2Filled = function (entityPM) {
        var myResult = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantIdCode2)) {
            myResult = true;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationCode2)) {
            myResult = true;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationPortCode2)) {
            myResult = true;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationName2)) {
            myResult = true;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationReference2)) {
            myResult = true;
        }
        return myResult;
    };
    ShipmentTool.IsParticipant3Filled = function (entityPM) {
        var myResult = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantIdCode3)) {
            myResult = true;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationCode3)) {
            myResult = true;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationPortCode3)) {
            myResult = true;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationName3)) {
            myResult = true;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationReference3)) {
            myResult = true;
        }
        return myResult;
    };
    ShipmentTool.IsParticipant1MissingData = function (entityPM) {
        var myResult = false;
        if (Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantIdCode1)) {
            myResult = true;
        }
        else if (Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationCode1)) {
            myResult = true;
        }
        else if (Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationPortCode1)) {
            myResult = true;
        }
        else if (Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationName1)) {
            myResult = true;
        }
        else if (Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationReference1)) {
            myResult = true;
        }
        return myResult;
    };
    ShipmentTool.IsParticipant2MissingData = function (entityPM) {
        var myResult = false;
        if (Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantIdCode2)) {
            myResult = true;
        }
        else if (Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationCode2)) {
            myResult = true;
        }
        else if (Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationPortCode2)) {
            myResult = true;
        }
        else if (Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationName2)) {
            myResult = true;
        }
        else if (Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationReference2)) {
            myResult = true;
        }
        return myResult;
    };
    ShipmentTool.IsParticipant3MissingData = function (entityPM) {
        var myResult = false;
        if (Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantIdCode3)) {
            myResult = true;
        }
        else if (Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationCode3)) {
            myResult = true;
        }
        else if (Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationPortCode3)) {
            myResult = true;
        }
        else if (Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationName3)) {
            myResult = true;
        }
        else if (Tools_1.AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationReference3)) {
            myResult = true;
        }
        return myResult;
    };
    ShipmentTool.GetFromPortTextCode = function (TransportModeId, levelCode) {
        var myResult = null;
        if (levelCode == "C") {
            switch (TransportModeId) {
                case "A": {
                    myResult = "Master.S.NewMaster.Gateway";
                    break;
                }
                case "O": {
                    myResult = "Master.S.NewMaster.LoadingPort";
                    break;
                }
                case "I": {
                    myResult = "Master.S.NewMaster.From";
                    break;
                }
                default: {
                    myResult = "Master.S.NewMaster.From";
                    break;
                }
            }
        }
        else {
            switch (TransportModeId) {
                case "A": {
                    myResult = "Shipment.S.NewShipment.Gateway";
                    break;
                }
                case "O": {
                    myResult = "Shipment.S.NewShipment.LoadingPort";
                    break;
                }
                case "I": {
                    myResult = "Shipment.S.NewShipment.From";
                    break;
                }
                default: {
                    myResult = "Shipment.S.NewShipment.From";
                    break;
                }
            }
        }
        return myResult;
    };
    ShipmentTool.GetToPortTextCode = function (TransportModeId, levelCode) {
        var myResult = null;
        if (levelCode == "C") {
            switch (TransportModeId) {
                case "A": {
                    myResult = "Master.S.NewMaster.Destination";
                    break;
                }
                case "O": {
                    myResult = "Master.S.NewMaster.DischargePort";
                    break;
                }
                case "I": {
                    myResult = "Master.S.NewMaster.To";
                    break;
                }
                default: {
                    myResult = "Master.S.NewMaster.To";
                    break;
                }
            }
        }
        else {
            switch (TransportModeId) {
                case "A": {
                    myResult = "Shipment.S.NewShipment.Destination";
                    break;
                }
                case "O": {
                    myResult = "Shipment.S.NewShipment.DischargePort";
                    break;
                }
                case "I": {
                    myResult = "Shipment.S.NewShipment.To";
                    break;
                }
                default: {
                    myResult = "Shipment.S.NewShipment.To";
                    break;
                }
            }
        }
        return myResult;
    };
    ShipmentTool.SetPayableUnitPriceBySteps = function (entityPM, myBaseQuote) {
        if (myBaseQuote != null) {
            var myCharge = myBaseQuote.QuoteCharges.filter(function (d) { return d.Id == entityPM.QuoteChargeId; })[0];
            if (myCharge != null) {
                var mySteps = myCharge.QuoteChargePriceSteps;
                if (mySteps.length > 0) {
                    var price = null;
                    mySteps.sort(function (a, b) { return a.Step - b.Step; }).forEach(function (itemPriceStep) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Quantity) && entityPM.Quantity >= itemPriceStep.Step) {
                            price = itemPriceStep.CostUnitPrice;
                        }
                    });
                    var entitySmallest = mySteps.sort(function (a, b) { return a.Step - b.Step; })[0];
                    if (entitySmallest != null) {
                        if (Tools_1.AppTool.IsNullOrEmpty(entityPM.Quantity) || entityPM.Quantity < entitySmallest.Step) {
                            price = entitySmallest.CostUnitPrice;
                        }
                    }
                    entityPM.UnitPrice = Tools_1.AppTool.Round(price, 3);
                }
            }
        }
    };
    ShipmentTool.SetReceivableUnitPriceBySteps = function (entityPM, myBaseQuote) {
        if (myBaseQuote != null) {
            var myCharge = myBaseQuote.QuoteCharges.filter(function (d) { return d.Id == entityPM.QuoteChargeId; })[0];
            if (myCharge != null) {
                var mySteps = myCharge.QuoteChargePriceSteps;
                if (mySteps.length > 0) {
                    var price = null;
                    mySteps.sort(function (a, b) { return a.Step - b.Step; }).forEach(function (itemPriceStep) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Quantity) && entityPM.Quantity >= itemPriceStep.Step) {
                            price = itemPriceStep.SaleUnitPrice;
                        }
                    });
                    var entitySmallest = mySteps.sort(function (a, b) { return a.Step - b.Step; })[0];
                    if (entitySmallest != null) {
                        if (Tools_1.AppTool.IsNullOrEmpty(entityPM.Quantity) || entityPM.Quantity < entitySmallest.Step) {
                            price = entitySmallest.SaleUnitPrice;
                        }
                    }
                    entityPM.UnitPrice = Tools_1.AppTool.Round(price, 3);
                }
            }
        }
    };
    ShipmentTool.SetReceivableLineStatus = function (entityPM) {
        if (entityPM.ShipmentReceivableLineStatusCode == "APPD" || entityPM.ShipmentReceivableLineStatusCode == "ACCT" || entityPM.ShipmentReceivableLineStatusCode == "DRFT") {
        }
        else {
            if (entityPM.Quantity != null && entityPM.UnitPrice != null) {
                if (entityPM.ShipmentReceivableLineStatusCode != "OAMT") {
                    entityPM.ShipmentReceivableLineStatusCode = "OAMT";
                }
            }
            else {
                if (entityPM.ShipmentReceivableLineStatusCode != "EMPT") {
                    entityPM.ShipmentReceivableLineStatusCode = "EMPT";
                }
            }
        }
    };
    ShipmentTool.SetPayableLineStatus = function (entityPM) {
        if (entityPM.IsDirty) {
            var StatusCode = "EMPT";
            if (entityPM.ShipmentPayableAmountTypeCode == "NEXP") {
                StatusCode = "ACCT";
            }
            else if (entityPM.Quantity == null || entityPM.UnitPrice == null) {
                StatusCode = "EMPT";
            }
            else {
                if (entityPM.OpenAmount == null) {
                    entityPM.OpenAmount = 0;
                }
                if (entityPM.AccountedAmount == null) {
                    entityPM.AccountedAmount = 0;
                }
                if (entityPM.OpenAmount != 0 && entityPM.AccountedAmount != 0) {
                    StatusCode = "PACC";
                }
                else if (entityPM.OpenAmount != 0) {
                    StatusCode = "OAMT";
                }
                else if (entityPM.AccountedAmount != 0) {
                    StatusCode = "ACCT";
                }
            }
            if (StatusCode == "EMPT") {
                if (entityPM.Quantity != null && entityPM.UnitPrice != null) {
                    StatusCode = "OAMT";
                }
            }
            entityPM.ShipmentPayableLineStatusCode = StatusCode;
        }
    };
    ShipmentTool.GetByPckageTypeGrouped = function (entityPM) {
        var myResult = [];
        if (entityPM.ShipmentPackages.length > 0) {
            var myService = new PackageTypeListService_1.PackageTypeListService();
            entityPM.ShipmentPackages.filter(function (f) { return f.IsContainer == true; }).forEach(function (item) {
                var itemGrouped = myResult.filter(function (f) { return f.PackageTypeId == item.PackageTypeId; })[0];
                if (itemGrouped == null) {
                    itemGrouped = new ByPckageType();
                    itemGrouped.PackageTypeId = item.PackageTypeId;
                    itemGrouped.Quantity = item.Quantity;
                    if (Tools_1.AppTool.IsNullOrEmpty(itemGrouped.Quantity)) {
                        itemGrouped.Quantity = 0;
                    }
                    if (item.PackageTypeId) {
                        myService.getSingleFromCache(item.PackageTypeId).subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                var list = myResponse.Result;
                                if (list) {
                                    itemGrouped.MeasurementId = list.MeasurementId;
                                    itemGrouped.MeasurementCode = list.MeasurementCode;
                                    itemGrouped.MeasurementShortName = list.MeasurementShortName;
                                }
                            }
                        });
                    }
                    myResult.push(itemGrouped);
                }
                else {
                    if (!Tools_1.AppTool.IsNullOrEmpty(item.Quantity)) {
                        itemGrouped.Quantity += item.Quantity;
                    }
                }
            });
        }
        return myResult;
    };
    ShipmentTool.ComputeTotals = function (entityPM) {
        // Payables
        var openPayablesLocal = null;
        var acctPayablesLocal = null;
        var openPayablesProfit = null;
        var acctPayablesProfit = null;
        if (entityPM.ShipmentLevelCode == "C" && entityPM.ShipmentConsoleShipments.length > 0) {
            openPayablesLocal = Tools_1.ArrayTool.Sum(entityPM.ShipmentConsoleShipments, "OAMTPayables_Local");
            acctPayablesLocal = Tools_1.ArrayTool.Sum(entityPM.ShipmentConsoleShipments, "ACCTPayables_Local");
            openPayablesProfit = Tools_1.ArrayTool.Sum(entityPM.ShipmentConsoleShipments, "OAMTPayables_Profit");
            acctPayablesProfit = Tools_1.ArrayTool.Sum(entityPM.ShipmentConsoleShipments, "ACCTPayables_Profit");
        }
        else {
            openPayablesLocal = Tools_1.ArrayTool.Sum(entityPM.ShipmentPayables, "OpenAmountInLocalCurrency");
            acctPayablesLocal = Tools_1.ArrayTool.Sum(entityPM.ShipmentPayables, "AccountedAmountInLocalCurrency");
            openPayablesProfit = Tools_1.ArrayTool.Sum(entityPM.ShipmentPayables, "OpenAmountInProfitCurrency");
            acctPayablesProfit = Tools_1.ArrayTool.Sum(entityPM.ShipmentPayables, "AccountedAmountInProfitCurrency");
        }
        // Receivables
        var openReceivablesLocal = null;
        var acctReceivablesLocal = null;
        var openReceivablesProfit = null;
        var acctReceivablesProfit = null;
        if (entityPM.ShipmentLevelCode == "C" && entityPM.ShipmentConsoleShipments.length > 0) {
            if (entityPM.ProrateReceivables) {
                openReceivablesLocal = Tools_1.ArrayTool.Sum(entityPM.ShipmentConsoleShipments, "OAMTReceivables_Local");
                acctReceivablesLocal = Tools_1.ArrayTool.Sum(entityPM.ShipmentConsoleShipments, "ACCTReceivables_Local");
                openReceivablesProfit = Tools_1.ArrayTool.Sum(entityPM.ShipmentConsoleShipments, "OAMTReceivables_Profit");
                acctReceivablesProfit = Tools_1.ArrayTool.Sum(entityPM.ShipmentConsoleShipments, "ACCTReceivables_Profit");
            }
            else {
                //openReceivablesLocal = ArrayTool.Sum(entityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "EMPT" || f.ShipmentReceivableLineStatusCode == "OAMT"), "TotalAmountLocal");
                //acctReceivablesLocal = ArrayTool.Sum(entityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "DRFT" || f.ShipmentReceivableLineStatusCode == "ACCT"), "TotalAmountLocal");
                //openReceivablesProfit = ArrayTool.Sum(entityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "EMPT" || f.ShipmentReceivableLineStatusCode == "OAMT"), "AmountInProfitCurrency");
                //acctReceivablesProfit = ArrayTool.Sum(entityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "DRFT" || f.ShipmentReceivableLineStatusCode == "ACCT"), "AmountInProfitCurrency");
                openReceivablesLocal = Tools_1.ArrayTool.Sum(entityPM.ShipmentReceivables.filter(function (f) { return f.ShipmentReceivableLineStatusCode != "ACCT"; }), "TotalAmountLocal");
                acctReceivablesLocal = Tools_1.ArrayTool.Sum(entityPM.ShipmentReceivables.filter(function (f) { return f.ShipmentReceivableLineStatusCode == "ACCT"; }), "TotalAmountLocal");
                openReceivablesProfit = Tools_1.ArrayTool.Sum(entityPM.ShipmentReceivables.filter(function (f) { return f.ShipmentReceivableLineStatusCode != "ACCT"; }), "AmountInProfitCurrency");
                acctReceivablesProfit = Tools_1.ArrayTool.Sum(entityPM.ShipmentReceivables.filter(function (f) { return f.ShipmentReceivableLineStatusCode == "ACCT"; }), "AmountInProfitCurrency");
                // need to add houses receivables that are not connected to parent receivable
                openReceivablesLocal += Tools_1.ArrayTool.Sum(entityPM.ShipmentConsoleShipments, "OAMTReceivables_Local_NoParent");
                acctReceivablesLocal += Tools_1.ArrayTool.Sum(entityPM.ShipmentConsoleShipments, "ACCTReceivables_Local_NoParent");
                openReceivablesProfit += Tools_1.ArrayTool.Sum(entityPM.ShipmentConsoleShipments, "OAMTReceivables_Profit_NoParent");
                acctReceivablesProfit += Tools_1.ArrayTool.Sum(entityPM.ShipmentConsoleShipments, "ACCTReceivables_Profit_NoParent");
            }
        }
        else {
            //openReceivablesLocal = ArrayTool.Sum(entityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "EMPT" || f.ShipmentReceivableLineStatusCode == "OAMT"), "TotalAmountLocal");
            //acctReceivablesLocal = ArrayTool.Sum(entityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "DRFT" || f.ShipmentReceivableLineStatusCode == "ACCT"), "TotalAmountLocal");
            //openReceivablesProfit = ArrayTool.Sum(entityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "EMPT" || f.ShipmentReceivableLineStatusCode == "OAMT"), "AmountInProfitCurrency");
            //acctReceivablesProfit = ArrayTool.Sum(entityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "DRFT" || f.ShipmentReceivableLineStatusCode == "ACCT"), "AmountInProfitCurrency");
            openReceivablesLocal = Tools_1.ArrayTool.Sum(entityPM.ShipmentReceivables.filter(function (f) { return f.ShipmentReceivableLineStatusCode != "ACCT"; }), "TotalAmountLocal");
            acctReceivablesLocal = Tools_1.ArrayTool.Sum(entityPM.ShipmentReceivables.filter(function (f) { return f.ShipmentReceivableLineStatusCode == "ACCT"; }), "TotalAmountLocal");
            openReceivablesProfit = Tools_1.ArrayTool.Sum(entityPM.ShipmentReceivables.filter(function (f) { return f.ShipmentReceivableLineStatusCode != "ACCT"; }), "AmountInProfitCurrency");
            acctReceivablesProfit = Tools_1.ArrayTool.Sum(entityPM.ShipmentReceivables.filter(function (f) { return f.ShipmentReceivableLineStatusCode == "ACCT"; }), "AmountInProfitCurrency");
        }
        var allPayablesLocal = openPayablesLocal + acctPayablesLocal;
        var allPayablesProfit = openPayablesProfit + acctPayablesProfit;
        var allReceivablesLocal = openReceivablesLocal + acctReceivablesLocal;
        var allReceivablesProfit = openReceivablesProfit + acctReceivablesProfit;
        // New Design
        var profitInLocal = allReceivablesLocal - allPayablesLocal;
        var profitInProfit = allReceivablesProfit - allPayablesProfit;
        // Old Design
        //var profitInLocal = 0;
        //var profitInProfit = 0;
        //if (allReceivablesLocal > 0) {
        //    profitInLocal = allReceivablesLocal - allPayablesLocal;
        //    profitInProfit = allReceivablesProfit - allPayablesProfit;
        //}
        /* Payables */
        if (entityPM.OpenPayablesInLocalCurrency != openPayablesLocal) {
            entityPM.OpenPayablesInLocalCurrency = (openPayablesLocal == null) ? 0 : Tools_1.AppTool.Round(openPayablesLocal, 2);
        }
        if (entityPM.OpenPayablesInProfitCurrency != openPayablesProfit) {
            entityPM.OpenPayablesInProfitCurrency = (openPayablesProfit == null) ? 0 : Tools_1.AppTool.Round(openPayablesProfit, 2);
        }
        if (entityPM.AccountedPayablesInLocalCurrency != acctPayablesLocal) {
            entityPM.AccountedPayablesInLocalCurrency = (acctPayablesLocal == null) ? 0 : Tools_1.AppTool.Round(acctPayablesLocal, 2);
        }
        if (entityPM.AccountedPayablesInProfitCurrency != acctPayablesProfit) {
            entityPM.AccountedPayablesInProfitCurrency = (acctPayablesProfit == null) ? 0 : Tools_1.AppTool.Round(acctPayablesProfit, 2);
        }
        /* Receivables */
        if (entityPM.OpenReceivablesInLocalCurrency != openReceivablesLocal) {
            entityPM.OpenReceivablesInLocalCurrency = (openReceivablesLocal == null) ? 0 : Tools_1.AppTool.Round(openReceivablesLocal, 2);
        }
        if (entityPM.OpenReceivablesInProfitCurrency != openReceivablesProfit) {
            entityPM.OpenReceivablesInProfitCurrency = (openReceivablesProfit == null) ? 0 : Tools_1.AppTool.Round(openReceivablesProfit, 2);
        }
        if (entityPM.AccountedReceivablesInLocalCurrency != acctReceivablesLocal) {
            entityPM.AccountedReceivablesInLocalCurrency = (acctReceivablesLocal == null) ? 0 : Tools_1.AppTool.Round(acctReceivablesLocal, 2);
        }
        if (entityPM.AccountedReceivablesInProfitCurrency != acctReceivablesProfit) {
            entityPM.AccountedReceivablesInProfitCurrency = (acctReceivablesProfit == null) ? 0 : Tools_1.AppTool.Round(acctReceivablesProfit, 2);
        }
        /* Profit */
        if (entityPM.ProfitInLocalCurrency != profitInLocal) {
            entityPM.ProfitInLocalCurrency = (profitInLocal == null) ? 0 : Tools_1.AppTool.Round(profitInLocal, 2);
        }
        if (entityPM.ProfitInProfitCurrency != profitInProfit) {
            entityPM.ProfitInProfitCurrency = (profitInProfit == null) ? 0 : Tools_1.AppTool.Round(profitInProfit, 2);
        }
    };
    ShipmentTool.RecalculateShipmentFields = function (shipmentPM) {
        if (shipmentPM != null) {
            if (shipmentPM.Ratio == null) {
                shipmentPM.Ratio = Tools_1.AppTool.GetRatio(shipmentPM.DirectionId, shipmentPM.TransportModeId, shipmentPM.ShipmentTypeId, SessionLocator_1.SessionLocator.TenantPM.CountryCode);
            }
            if (shipmentPM.ShipmentPackages.length == 0) {
                shipmentPM.NumberOfPackages = null;
                shipmentPM.GrossWeight = null;
                shipmentPM.Volume = null;
                shipmentPM.VolumetricWeight = null;
                shipmentPM.ChargeableWeight = null;
                shipmentPM.AWBCommodityItemNumber = null;
                shipmentPM.GrossWeightEdited = false;
                shipmentPM.ChargeableWeightEdited = false;
            }
            else {
                var myQuantity = 0;
                var myVolume = 0;
                var myGrossWeight = 0;
                var myVolumetricWeight = 0;
                shipmentPM.ShipmentPackages.forEach(function (item) {
                    item.InsideShipmentPackages.forEach(function (insideItem) {
                        insideItem.Volume = Tools_1.AppTool.ComputePackageVolume(insideItem.Quantity, insideItem.Width, insideItem.Height, insideItem.Length, insideItem.Weight, shipmentPM.Ratio, shipmentPM.DimensionsUnitCode, shipmentPM.VolumeUnitCode, shipmentPM.GrossWeightUnitCode);
                        insideItem.VolumetricWeight = Tools_1.AppTool.ComputePackageVolumetricWeight(insideItem.Quantity, insideItem.Width, insideItem.Height, insideItem.Length, insideItem.Volume, insideItem.Weight, shipmentPM.Ratio, shipmentPM.DimensionsUnitCode, shipmentPM.VolumeUnitCode, shipmentPM.GrossWeightUnitCode, shipmentPM.ChargeableWeightUnitCode);
                    });
                    item.Volume = Tools_1.AppTool.ComputePackageVolume(item.Quantity, item.Width, item.Height, item.Length, item.Weight, shipmentPM.Ratio, shipmentPM.DimensionsUnitCode, shipmentPM.VolumeUnitCode, shipmentPM.GrossWeightUnitCode);
                    item.VolumetricWeight = Tools_1.AppTool.ComputePackageVolumetricWeight(item.Quantity, item.Width, item.Height, item.Length, item.Volume, item.Weight, shipmentPM.Ratio, shipmentPM.DimensionsUnitCode, shipmentPM.VolumeUnitCode, shipmentPM.GrossWeightUnitCode, shipmentPM.ChargeableWeightUnitCode);
                    if (item.Quantity != null) {
                        myQuantity += item.Quantity;
                    }
                    if (item.Volume != null) {
                        myVolume += item.Volume;
                    }
                    if (item.VolumetricWeight != null) {
                        myVolumetricWeight += item.VolumetricWeight;
                    }
                    if (item.Weight != null) {
                        myGrossWeight += item.Weight;
                    }
                });
                shipmentPM.NumberOfPackages = myQuantity;
                shipmentPM.Volume = Tools_1.AppTool.Round(myVolume, 3);
                shipmentPM.VolumetricWeight = Tools_1.AppTool.Round(myVolumetricWeight, 3);
                if (!shipmentPM.GrossWeightEdited) {
                    shipmentPM.GrossWeight = Tools_1.AppTool.Round(myGrossWeight, 3);
                }
                if (!shipmentPM.ChargeableWeightEdited) {
                    shipmentPM.ChargeableWeight = Tools_1.AppTool.CalculateChargeableWeight(shipmentPM.GrossWeight, shipmentPM.VolumetricWeight, shipmentPM.GrossWeightUnitCode, shipmentPM.ChargeableWeightUnitCode, shipmentPM.DirectionId, shipmentPM.TransportModeId);
                }
            }
            var orderVolumetricWeight = null;
            if (shipmentPM.BookingVolume != null) {
                orderVolumetricWeight = Tools_1.AppTool.GetWeightFromVolume(shipmentPM.VolumeUnitCode, shipmentPM.ChargeableWeightUnitCode, shipmentPM.BookingVolume, shipmentPM.Ratio);
            }
            else if (shipmentPM.OrderGrossWeight != null) {
                orderVolumetricWeight = Tools_1.AppTool.GetWeightFromWeight(shipmentPM.GrossWeightUnitCode, shipmentPM.ChargeableWeightUnitCode, shipmentPM.OrderGrossWeight);
            }
            shipmentPM.OrderVolumetricWeight = Tools_1.AppTool.Round(orderVolumetricWeight, 3);
            shipmentPM.OrderChargeableWeight = Tools_1.AppTool.CalculateChargeableWeight(shipmentPM.OrderGrossWeight, shipmentPM.OrderVolumetricWeight, shipmentPM.GrossWeightUnitCode, shipmentPM.ChargeableWeightUnitCode, shipmentPM.DirectionId, shipmentPM.TransportModeId);
        }
    };
    ShipmentTool.OnShipmentRatioChanged = function (shipmentPM) {
        if (shipmentPM) {
            if (shipmentPM.Ratio == null) {
                shipmentPM.Ratio = Tools_1.AppTool.GetRatio(shipmentPM.DirectionId, shipmentPM.TransportModeId, shipmentPM.ShipmentTypeId, SessionLocator_1.SessionLocator.TenantPM.CountryCode);
            }
            // ShipmentPackages
            if (shipmentPM.ShipmentPackages.length == 0) {
                shipmentPM.NumberOfPackages = null;
                shipmentPM.GrossWeight = null;
                shipmentPM.Volume = null;
                shipmentPM.VolumetricWeight = null;
                shipmentPM.ChargeableWeight = null;
                shipmentPM.AWBCommodityItemNumber = null;
                shipmentPM.GrossWeightEdited = false;
                shipmentPM.ChargeableWeightEdited = false;
            }
            else {
                shipmentPM.ShipmentPackages.forEach(function (item) {
                    item.InsideShipmentPackages.forEach(function (insideItem) {
                        if (insideItem.Volume) {
                            insideItem.VolumetricWeight = Tools_1.AppTool.GetWeightFromVolume(shipmentPM.VolumeUnitCode, shipmentPM.ChargeableWeightUnitCode, insideItem.Volume, shipmentPM.Ratio);
                        }
                    });
                    if (item.Volume) {
                        item.VolumetricWeight = Tools_1.AppTool.GetWeightFromVolume(shipmentPM.VolumeUnitCode, shipmentPM.ChargeableWeightUnitCode, item.Volume, shipmentPM.Ratio);
                    }
                });
                shipmentPM.VolumetricWeight = Tools_1.AppTool.Round(Tools_1.ArrayTool.Sum(shipmentPM.ShipmentPackages, "VolumetricWeight"), 3);
                if (!shipmentPM.ChargeableWeightEdited) {
                    shipmentPM.ChargeableWeight = Tools_1.AppTool.CalculateChargeableWeight(shipmentPM.GrossWeight, shipmentPM.VolumetricWeight, shipmentPM.GrossWeightUnitCode, shipmentPM.ChargeableWeightUnitCode, shipmentPM.DirectionId, shipmentPM.TransportModeId);
                }
            }
            // ShipmentOrderPackages
            if (shipmentPM.ShipmentOrderPackages.length > 0) {
                shipmentPM.ShipmentOrderPackages.forEach(function (item) {
                    if (item.Volume) {
                        item.VolumetricWeight = Tools_1.AppTool.GetWeightFromVolume(shipmentPM.VolumeUnitCode, shipmentPM.ChargeableWeightUnitCode, item.Volume, shipmentPM.Ratio);
                    }
                });
                shipmentPM.OrderVolumetricWeight = Tools_1.AppTool.Round(Tools_1.ArrayTool.Sum(shipmentPM.ShipmentOrderPackages, "VolumetricWeight"), 3);
                shipmentPM.OrderChargeableWeight = Tools_1.AppTool.CalculateChargeableWeight(shipmentPM.OrderGrossWeight, shipmentPM.OrderVolumetricWeight, shipmentPM.GrossWeightUnitCode, shipmentPM.ChargeableWeightUnitCode, shipmentPM.DirectionId, shipmentPM.TransportModeId);
            }
            else {
                if (shipmentPM.BookingVolume) {
                    var orderVolumetricWeight = Tools_1.AppTool.GetWeightFromVolume(shipmentPM.VolumeUnitCode, shipmentPM.ChargeableWeightUnitCode, shipmentPM.BookingVolume, shipmentPM.Ratio);
                    shipmentPM.OrderVolumetricWeight = Tools_1.AppTool.Round(orderVolumetricWeight, 3);
                    shipmentPM.OrderChargeableWeight = Tools_1.AppTool.CalculateChargeableWeight(shipmentPM.OrderGrossWeight, shipmentPM.OrderVolumetricWeight, shipmentPM.GrossWeightUnitCode, shipmentPM.ChargeableWeightUnitCode, shipmentPM.DirectionId, shipmentPM.TransportModeId);
                }
            }
        }
    };
    ShipmentTool.GetNewShipmentPM = function () {
        var todayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        var entityPM = new ShipmentPM_1.ShipmentPM();
        entityPM.FHLStatusCode = "NSEN";
        entityPM.FWBStatusCode = "NSEN";
        entityPM.FHLStatusName = "Not Sent";
        entityPM.FWBStatusName = "Not Sent";
        entityPM.ManifestStatusCode = "NSEN";
        entityPM.IsOperationalClosed = false;
        entityPM.CreateDateTime = todayDate;
        entityPM.LastUpdateDate = todayDate;
        entityPM.StatusDate = todayDate;
        entityPM.Tenant = SessionLocator_1.SessionLocator.TenantPM.Id;
        entityPM.AWBCurrencyId = SessionLocator_1.SessionLocator.TenantPM.FreightCurrencyId;
        entityPM.ProfitCurrencyId = SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyId;
        entityPM.VolumeUnitCode = SessionLocator_1.SessionLocator.TenantPM.VolumeUnitCode;
        entityPM.DimensionsUnitCode = SessionLocator_1.SessionLocator.TenantPM.DimensionsUnitCode;
        entityPM.GrossWeightUnitCode = SessionLocator_1.SessionLocator.TenantPM.GrossWeightUnitCode;
        entityPM.ChargeableWeightUnitCode = SessionLocator_1.SessionLocator.TenantPM.ChargeableWeightUnitCode;
        entityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        entityPM.BranchId = SessionLocator_1.SessionLocator.LoggedUserPM.BranchId;
        entityPM.DepartmentId = SessionLocator_1.SessionLocator.LoggedUserPM.DepartmentId;
        entityPM.NewConcurrencyGUID = Tools_1.AppTool.GetNewGuid();
        return entityPM;
    };
    ShipmentTool.OnShipmentQuantitiesChanged = function (entityPM) {
        if (entityPM) {
            var isLCL = this.IsLCL(entityPM);
            if (isLCL) {
                entityPM.ShipmentPayables.forEach(function (itemPayable) {
                    if (Tools_1.AppTool.IsNullOrEmpty(itemPayable.UnitPrice)) {
                        if (Tools_1.AppTool.IsNullOrEmpty(itemPayable.ShipmentPayableParentId)) {
                            if (itemPayable.ShipmentPayableAmountTypeCode != "NEXP" && itemPayable.ShipmentPayableLineStatusCode != "ACCT" && itemPayable.ShipmentPayableLineStatusCode != "PACC") {
                                var myQuantity = null;
                                switch (itemPayable.MeasurementCode) {
                                    case "GRWT": {
                                        myQuantity = entityPM.GrossWeight;
                                        break;
                                    }
                                    case "CHWT": {
                                        myQuantity = entityPM.ChargeableWeight;
                                        break;
                                    }
                                    case "VOLU": {
                                        myQuantity = entityPM.Volume;
                                        break;
                                    }
                                    case "BTEU": {
                                        myQuantity = entityPM.TEU;
                                        break;
                                    }
                                    case "FIXD": {
                                        myQuantity = 1;
                                        break;
                                    }
                                    case "GWTN": {
                                        myQuantity = entityPM.GrossWeightPerTon;
                                        break;
                                    }
                                    case "PRVL": {
                                        myQuantity = entityPM.ValueOfGoods;
                                        break;
                                    }
                                    case "PRFR": {
                                        myQuantity = Tools_1.ArrayTool.Sum(entityPM.ShipmentPayables.filter(function (d) { return d.ChargesGroupCode == "FRT" && Tools_1.AppTool.IsNullOrEmpty(d.ShipmentPayableParentId); }), "ExpectedAmount");
                                        break;
                                    }
                                    case "CWKG": {
                                        myQuantity = entityPM.ChargeableWeightInKG;
                                        break;
                                    }
                                    case "GWKG": {
                                        myQuantity = entityPM.GrossWeightInKG;
                                        break;
                                    }
                                    case "QTY": {
                                        myQuantity = entityPM.NumberOfPackages;
                                        break;
                                    }
                                    case "VCBM": {
                                        myQuantity = entityPM.VolumeInCBM;
                                        break;
                                    }
                                    default: {
                                        break;
                                    }
                                }
                                if (itemPayable.Quantity != myQuantity) {
                                    itemPayable.Quantity = Tools_1.AppTool.Round(myQuantity, 3);
                                    if (itemPayable.IsChargeBySteps) {
                                        //this.SetPayableUnitPriceBySteps(itemPayable, this.fatherComponent.BaseQuote);
                                    }
                                }
                            }
                        }
                    }
                });
                entityPM.ShipmentReceivables.forEach(function (itemReceivable) {
                    if (Tools_1.AppTool.IsNullOrEmpty(itemReceivable.UnitPrice)) {
                        if (Tools_1.AppTool.IsNullOrEmpty(itemReceivable.ShipmentReceivableParentId)) {
                            if (Tools_1.AppTool.IsNullOrEmpty(itemReceivable.ARInvoiceId)) {
                                var myQuantity = null;
                                switch (itemReceivable.MeasurementCode) {
                                    case "GRWT": {
                                        myQuantity = entityPM.GrossWeight;
                                        break;
                                    }
                                    case "CHWT": {
                                        myQuantity = entityPM.ChargeableWeight;
                                        break;
                                    }
                                    case "VOLU": {
                                        myQuantity = entityPM.Volume;
                                        break;
                                    }
                                    case "BTEU": {
                                        myQuantity = entityPM.TEU;
                                        break;
                                    }
                                    case "FIXD": {
                                        myQuantity = 1;
                                        break;
                                    }
                                    case "GWTN": {
                                        myQuantity = entityPM.GrossWeightPerTon;
                                        break;
                                    }
                                    case "PRVL": {
                                        myQuantity = entityPM.ValueOfGoods;
                                        break;
                                    }
                                    case "PRFR": {
                                        myQuantity = Tools_1.ArrayTool.Sum(entityPM.ShipmentReceivables.filter(function (d) { return d.ChargesGroupCode == "FRT" && Tools_1.AppTool.IsNullOrEmpty(d.ShipmentReceivableParentId); }), "TotalAmount");
                                        break;
                                    }
                                    case "QTY": {
                                        myQuantity = entityPM.NumberOfPackages;
                                        break;
                                    }
                                    case "CWKG": {
                                        myQuantity = entityPM.ChargeableWeightInKG;
                                        break;
                                    }
                                    case "GWKG": {
                                        myQuantity = entityPM.GrossWeightInKG;
                                        break;
                                    }
                                    case "VCBM": {
                                        myQuantity = entityPM.VolumeInCBM;
                                        break;
                                    }
                                    default: {
                                        break;
                                    }
                                }
                                if (itemReceivable.Quantity != myQuantity) {
                                    itemReceivable.Quantity = Tools_1.AppTool.Round(myQuantity, 3);
                                    if (itemReceivable.IsChargeBySteps) {
                                        //this.SetReceivableUnitPriceBySteps(itemReceivable, this.fatherComponent.BaseQuote);
                                    }
                                }
                            }
                        }
                    }
                });
            }
            else {
            }
        }
    };
    ShipmentTool.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    return ShipmentTool;
}());
exports.ShipmentTool = ShipmentTool;
var ByPckageType = /** @class */ (function () {
    function ByPckageType() {
    }
    return ByPckageType;
}());
exports.ByPckageType = ByPckageType;
var ShipmentGenerator = /** @class */ (function () {
    function ShipmentGenerator(entityPM, allRates) {
        this.EntityPM = null;
        this.OriginShipment = null;
        this.BaseQuote = null;
        this.IsAdhoc = false;
        this.IsRoutingRate = false;
        this.AllRates = [];
        this.IsLCLEntity = false;
        this.BCNTGrouped = [];
        this.EntityPM = entityPM;
        this.AllRates = allRates;
        this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
        this.myCurrencyListService = new CurrencyListService_1.CurrencyListService();
        this.myChargesTypeListService = new ChargesTypeListService_1.ChargesTypeListService();
        this.BCNTGrouped = ShipmentTool.GetByPckageTypeGrouped(this.EntityPM);
        if (this.AllRates == null) {
            this.AllRates = [];
        }
    }
    // Payables
    ShipmentGenerator.prototype.GeneratePayablesAutoDisplay = function () {
        var _this = this;
        this.myChargesTypeListService.getAllFromCache().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var allChargesTypes = myResponse.Result;
                if (allChargesTypes != null) {
                    allChargesTypes = allChargesTypes.filter(function (d) { return d.IsPayable == true && d.InActive == false; });
                    if (_this.EntityPM.ShipmentLevelCode == "C") {
                        allChargesTypes = allChargesTypes.filter(function (d) { return d.IsAutoDisplayInConsolidation; });
                    }
                    else {
                        if (_this.EntityPM.IncludesCustoms) {
                            allChargesTypes = allChargesTypes.filter(function (d) { return d.IsAutoDisplayInShipment == true || d.IsAutoDisplayInCustoms; });
                        }
                        else {
                            allChargesTypes = allChargesTypes.filter(function (d) { return d.IsAutoDisplayInShipment == true; });
                        }
                    }
                    switch (_this.EntityPM.TransportModeId) {
                        case "A":
                            {
                                allChargesTypes = allChargesTypes.filter(function (r) { return r.IsAir; });
                                break;
                            }
                        case "O":
                            {
                                allChargesTypes = allChargesTypes.filter(function (r) { return r.IsOcean; });
                                break;
                            }
                        case "I":
                            {
                                allChargesTypes = allChargesTypes.filter(function (r) { return r.IsInland; });
                                break;
                            }
                    }
                    switch (_this.EntityPM.DirectionId) {
                        case "E":
                            {
                                allChargesTypes = allChargesTypes.filter(function (r) { return r.IsExport; });
                                break;
                            }
                        case "I":
                            {
                                allChargesTypes = allChargesTypes.filter(function (r) { return r.IsImport; });
                                break;
                            }
                        case "D":
                            {
                                allChargesTypes = allChargesTypes.filter(function (r) { return r.IsDomestic; });
                                break;
                            }
                        case "R":
                            {
                                allChargesTypes = allChargesTypes.filter(function (r) { return r.IsDrop; });
                                break;
                            }
                    }
                    if (_this.IsLCLEntity) {
                        allChargesTypes.sort(function (a, b) { return a.ViewOrder - b.ViewOrder; }).forEach(function (item) {
                            _this.EntityPM.AddPayable(_this.CreateNewPayableFromLCLChargesType(item));
                        });
                        _this.EntityPM.ShipmentPayables.filter(function (f) { return f.MeasurementCode == "PRFR"; }).forEach(function (item) {
                            item.Quantity = Tools_1.ArrayTool.Sum(_this.EntityPM.ShipmentPayables.filter(function (d) { return d.ChargesGroupCode == "FRT" && Tools_1.AppTool.IsNullOrEmpty(d.ShipmentPayableParentId); }), "ExpectedAmount");
                        });
                    }
                    else {
                        if (_this.BCNTGrouped.length > 0) {
                            allChargesTypes.filter(function (f) { return f.ContainerMeasurementCode == "BCNT"; }).sort(function (a, b) { return a.ViewOrder - b.ViewOrder; }).forEach(function (myChargeType) {
                                _this.BCNTGrouped.forEach(function (itemGrouped) {
                                    var newRecord = new ShipmentPayablePM_1.ShipmentPayablePM(_this.EntityPM);
                                    newRecord.Tenant = SessionLocator_1.SessionLocator.Tenant;
                                    newRecord.ShipmentId = _this.EntityPM.Id;
                                    newRecord.ShipmentNumber = _this.EntityPM.ShipmentNumber;
                                    newRecord.ShipmentPayableLineStatusCode = "EMPT";
                                    newRecord.ShipmentPayableAmountTypeCode = "ACCU";
                                    newRecord.ShipmentPayableAmountTypeName = "Accrual";
                                    newRecord.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                                    newRecord.UpdateByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                                    newRecord.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                                    newRecord.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                                    newRecord.ChargesTypeId = myChargeType.Id;
                                    newRecord.ChargesTypeCode = myChargeType.Code;
                                    newRecord.ChargesTypeName = myChargeType.EnglishName;
                                    newRecord.VatTypeId = myChargeType.VatTypeId;
                                    newRecord.ChargesGroupCode = myChargeType.ChargesGroupCode;
                                    newRecord.DueTypeCode = myChargeType.DueTypeCode;
                                    newRecord.DueTypeName = myChargeType.DueTypeName;
                                    newRecord.IATACodeId = myChargeType.IATACodeId;
                                    newRecord.ViewOrder = myChargeType.ViewOrder;
                                    newRecord.PrepaidCollectId = myChargeType.ChargesGroupCode == "FRT" ? _this.EntityPM.FreightPrepaidCollectId : _this.EntityPM.OtherPrepaidCollectId;
                                    newRecord.Quantity = itemGrouped.Quantity;
                                    newRecord.MeasurementId = itemGrouped.MeasurementId;
                                    newRecord.MeasurementCode = itemGrouped.MeasurementCode;
                                    newRecord.MeasurementShortName = itemGrouped.MeasurementShortName;
                                    newRecord.IsBackToBack = myChargeType.IsBackToBack;
                                    if (!Tools_1.AppTool.IsNullOrEmpty(myChargeType.PayablesDefaultCurrencyId)) {
                                        newRecord.CurrencyId = myChargeType.PayablesDefaultCurrencyId;
                                    }
                                    else {
                                        if (myChargeType.ChargesGroupCode == "FRT" || myChargeType.ChargesGroupCode == "SCH") {
                                            newRecord.CurrencyId = SessionLocator_1.SessionLocator.TenantPM.FreightCurrencyId;
                                        }
                                        else {
                                            newRecord.CurrencyId = SessionLocator_1.SessionLocator.TenantPM.OtherChargesCurrencyId;
                                        }
                                    }
                                    _this.GetCurrencyCode(newRecord);
                                    newRecord.Rate = _this.GetCurrencyRate(newRecord.CurrencyId);
                                    newRecord.ProfitCurrencyExchangeRate = _this.GetCurrencyRate(_this.EntityPM.ProfitCurrencyId);
                                    var existsPayable = _this.EntityPM.ShipmentPayables.filter(function (d) { return d.ChargesTypeId == newRecord.ChargesTypeId && d.MeasurementId == newRecord.MeasurementId; })[0];
                                    if (existsPayable == null) {
                                        _this.EntityPM.AddPayable(newRecord);
                                    }
                                    else {
                                        var acctPayable = _this.EntityPM.ShipmentPayables.filter(function (d) { return d.ChargesTypeId == newRecord.ChargesTypeId && d.MeasurementId == newRecord.MeasurementId && (d.ShipmentPayableLineStatusCode == "ACCT" || d.ShipmentPayableLineStatusCode == "PACC"); })[0];
                                        var openPayable = _this.EntityPM.ShipmentPayables.filter(function (d) { return d.ChargesTypeId == newRecord.ChargesTypeId && d.MeasurementId == newRecord.MeasurementId && (d.ShipmentPayableLineStatusCode == "EMPT" || d.ShipmentPayableLineStatusCode == "OAMT"); })[0];
                                        if (acctPayable == null) {
                                            openPayable.Quantity = newRecord.Quantity;
                                        }
                                        if (acctPayable != null && newRecord.Quantity > acctPayable.Quantity) {
                                            if (openPayable == null) {
                                                _this.EntityPM.AddPayable(newRecord);
                                            }
                                            else {
                                                openPayable.Quantity = newRecord.Quantity;
                                            }
                                        }
                                    }
                                });
                            });
                        }
                        allChargesTypes.filter(function (f) { return Tools_1.AppTool.IsNullOrEmpty(f.ContainerMeasurementId); }).sort(function (a, b) { return a.ViewOrder - b.ViewOrder; }).forEach(function (myChargeType) {
                            _this.EntityPM.AddPayable(_this.CreateNewPayableFromLCLChargesType(myChargeType));
                        });
                    }
                }
            }
        });
    };
    ShipmentGenerator.prototype.GeneratePayablesFromQuote = function (baseQuote) {
        this.BaseQuote = baseQuote;
        if (this.EntityPM && this.BaseQuote) {
            this.IsAdhoc = this.BaseQuote.QuoteTypeCode == "A" ? true : false;
            this.IsRoutingRate = !this.IsAdhoc;
            var rate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);
            var quoteProfitInLocalCurrency = null;
            var quoteProfitInProfitCurrency = null;
            if (this.BaseQuote.QuoteTypeCode == "A") {
                quoteProfitInLocalCurrency = this.BaseQuote.EstimateProfit * this.BaseQuote.ExchangeRate;
                quoteProfitInProfitCurrency = quoteProfitInLocalCurrency / rate;
            }
            this.EntityPM.EstimateProfitInLocalCurrency = Tools_1.AppTool.Round(quoteProfitInLocalCurrency, 2);
            this.EntityPM.EstimateProfitInProfitCurrency = Tools_1.AppTool.Round(quoteProfitInProfitCurrency, 2);
            if (this.IsLCLEntity) {
                this.GeneratePayablesFromQuote_LCL();
            }
            else {
                this.GeneratePayablesFromQuote_FCL();
            }
        }
    };
    ShipmentGenerator.prototype.GeneratePayablesFromOriginShipment = function (originShipment) {
        var _this = this;
        this.OriginShipment = originShipment;
        if (this.EntityPM && this.OriginShipment) {
            if (this.IsLCLEntity) {
                this.OriginShipment.ShipmentPayables.forEach(function (item) {
                    _this.CreateNewPayableFromOriginShipment(item);
                });
            }
            else {
                this.OriginShipment.ShipmentPayables.forEach(function (item) {
                    switch (item.MeasurementCode) {
                        case "GRWT":
                        case "CHWT":
                        case "VOLU":
                        case "BTEU":
                        case "FIXD":
                        case "PRVL":
                        case "PRFR":
                        case "GWTN":
                        case "CWKG":
                        case "GWKG":
                        case "VCBM":
                        case "QTY":
                            {
                                _this.CreateNewPayableFromOriginShipment(item);
                                break;
                            }
                        case "BCNT": {
                            // No such case
                            break;
                        }
                        default: {
                            var itemGrouped = _this.BCNTGrouped.filter(function (f) { return f.MeasurementId == item.MeasurementId; })[0];
                            if (itemGrouped != null) {
                                _this.CreateNewPayableFromOriginShipment(item);
                            }
                            break;
                        }
                    }
                });
            }
        }
    };
    ShipmentGenerator.prototype.GeneratePayablesFromQuote_LCL = function () {
        var _this = this;
        this.BaseQuote.QuoteCharges.sort(function (a, b) { return a.ViewOrder - b.ViewOrder; }).forEach(function (item) {
            if (item.IsChargeBySteps) {
                var newPayablePM = _this.CreateNewPayableFromQuoteCharge(item);
                _this.EntityPM.AddPayable(newPayablePM);
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(item.CostUnitPrice)) {
                var newPayablePM = _this.CreateNewPayableFromQuoteCharge(item);
                _this.EntityPM.AddPayable(newPayablePM);
            }
        });
        this.ComputeAllPayablesQuote_LCL();
    };
    ShipmentGenerator.prototype.GeneratePayablesFromQuote_FCL = function () {
        var _this = this;
        this.BaseQuote.QuoteCharges.sort(function (a, b) { return a.ViewOrder - b.ViewOrder; }).forEach(function (item) {
            if (item.CostMeasurementCode == "BCNT") {
                _this.CreateNewPayableFromQuoteCharge_FCL(_this.BaseQuote.PackageType1Id, item.CostContainerType1UnitPrice, item);
                _this.CreateNewPayableFromQuoteCharge_FCL(_this.BaseQuote.PackageType2Id, item.CostContainerType2UnitPrice, item);
                _this.CreateNewPayableFromQuoteCharge_FCL(_this.BaseQuote.PackageType3Id, item.CostContainerType3UnitPrice, item);
                _this.CreateNewPayableFromQuoteCharge_FCL(_this.BaseQuote.PackageType4Id, item.CostContainerType4UnitPrice, item);
                _this.CreateNewPayableFromQuoteCharge_FCL(_this.BaseQuote.PackageType5Id, item.CostContainerType5UnitPrice, item);
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(item.CostUnitPrice)) {
                var newPayablePM = _this.CreateNewPayableFromQuoteCharge(item);
                newPayablePM.UnitPrice = item.CostUnitPrice;
                switch (item.CostMeasurementCode) {
                    case "BCNT":
                    case "GRWT":
                    case "CHWT":
                    case "VOLU":
                    case "BTEU":
                    case "FIXD":
                    case "PRVL":
                    case "PRFR":
                    case "GWTN":
                    case "CWKG":
                    case "GWKG":
                    case "VCBM":
                    case "QTY":
                        {
                            break;
                        }
                    default: {
                        if (item.CostMeasurementId) {
                            var itemGrouped = _this.BCNTGrouped.filter(function (f) { return f.MeasurementId == item.CostMeasurementId; })[0];
                            if (itemGrouped) {
                                _this.ComputePayableQuoteAmounts_FCL(newPayablePM, itemGrouped.Quantity);
                            }
                        }
                        break;
                    }
                }
                _this.EntityPM.AddPayable(newPayablePM);
            }
        });
        // Add Other Shipment Containers on Quote Records
        this.BCNTGrouped.forEach(function (itemGrouped) {
            var myRecord = _this.EntityPM.ShipmentPayables.filter(function (f) { return f.MeasurementId == itemGrouped.MeasurementId; })[0];
            if (myRecord == null) {
                _this.BaseQuote.QuoteCharges.filter(function (f) { return f.IsAllIN == false; }).forEach(function (item) {
                    if (item.CostMeasurementCode == "BCNT") {
                        var newRecord = new ShipmentPayablePM_1.ShipmentPayablePM(null);
                        newRecord.Tenant = _this.EntityPM.Tenant;
                        newRecord.ShipmentId = _this.EntityPM.Id;
                        newRecord.ShipmentNumber = _this.EntityPM.ShipmentNumber;
                        newRecord.ShipmentPayableLineStatusCode = "EMPT";
                        newRecord.ShipmentPayableAmountTypeCode = "ACCU";
                        newRecord.ShipmentPayableAmountTypeName = "Accrual";
                        newRecord.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                        newRecord.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                        newRecord.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                        newRecord.UpdateByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                        newRecord.Quantity = itemGrouped.Quantity;
                        newRecord.MeasurementId = itemGrouped.MeasurementId;
                        newRecord.MeasurementCode = itemGrouped.MeasurementCode;
                        newRecord.MeasurementShortName = itemGrouped.MeasurementShortName;
                        _this.myChargesTypeListService.getSingleFromCache(item.ChargesTypeId).subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                var chargesType = myResponse.Result;
                                if (chargesType) {
                                    newRecord.ChargesTypeId = chargesType.Id;
                                    newRecord.ChargesTypeCode = chargesType.Code;
                                    newRecord.ChargesTypeName = chargesType.EnglishName;
                                    newRecord.VatTypeId = chargesType.VatTypeId;
                                    newRecord.ChargesGroupCode = chargesType.ChargesGroupCode;
                                    newRecord.DueTypeCode = chargesType.DueTypeCode;
                                    newRecord.DueTypeName = chargesType.DueTypeName;
                                    newRecord.IATACodeId = chargesType.IATACodeId;
                                    newRecord.PrepaidCollectId = chargesType.ChargesGroupCode == "FRT" ? _this.EntityPM.FreightPrepaidCollectId : _this.EntityPM.OtherPrepaidCollectId;
                                    newRecord.IsBackToBack = chargesType.IsBackToBack;
                                    if (chargesType.ChargesGroupCode == "FRT" || chargesType.ChargesGroupCode == "SCH") {
                                        newRecord.CurrencyId = SessionLocator_1.SessionLocator.TenantPM.FreightCurrencyId;
                                    }
                                    else {
                                        newRecord.CurrencyId = SessionLocator_1.SessionLocator.TenantPM.OtherChargesCurrencyId;
                                    }
                                    _this.GetCurrencyCode(newRecord);
                                    //newRecord.ProfitCurrencyExchangeRate
                                    newRecord.Rate = _this.GetCurrencyRate(newRecord.CurrencyId);
                                }
                            }
                        });
                        _this.EntityPM.AddPayable(newRecord);
                    }
                });
            }
        });
        this.ComputeAllPayablesQuote_LCL();
    };
    ShipmentGenerator.prototype.ComputeAllPayablesQuote_LCL = function () {
        var _this = this;
        this.EntityPM.ShipmentPayables.filter(function (f) { return !Tools_1.AppTool.IsNullOrEmpty(f.QuoteChargeId); }).forEach(function (item) {
            switch (item.MeasurementCode) {
                case "GRWT":
                case "CHWT":
                case "VOLU":
                case "BTEU":
                case "FIXD":
                case "PRVL":
                case "GWTN":
                case "CWKG":
                case "VCBM":
                case "GWKG":
                case "QTY":
                    {
                        var itemCharge = _this.BaseQuote.QuoteCharges.filter(function (f) { return f.Id == item.QuoteChargeId; })[0];
                        _this.ComputePayableQuoteAmounts_LCL(item, itemCharge);
                        break;
                    }
            }
        });
        this.EntityPM.ShipmentPayables.filter(function (f) { return !Tools_1.AppTool.IsNullOrEmpty(f.QuoteChargeId); }).forEach(function (item) {
            switch (item.MeasurementCode) {
                case "PRFR":
                    {
                        var itemCharge = _this.BaseQuote.QuoteCharges.filter(function (f) { return f.Id == item.QuoteChargeId; })[0];
                        _this.ComputePayableQuoteAmounts_LCL(item, itemCharge);
                        break;
                    }
            }
        });
    };
    ShipmentGenerator.prototype.ComputePayableQuoteAmounts_LCL = function (myRecordPM, item) {
        // Quantity
        var myQuantity = null;
        switch (myRecordPM.MeasurementCode) {
            case "GRWT": {
                myQuantity = this.EntityPM.GrossWeight;
                break;
            }
            case "CHWT": {
                myQuantity = this.EntityPM.ChargeableWeight;
                break;
            }
            case "VOLU": {
                myQuantity = this.EntityPM.Volume;
                break;
            }
            case "BTEU": {
                myQuantity = this.EntityPM.TEU;
                break;
            }
            case "FIXD": {
                myQuantity = 1;
                break;
            }
            case "PRVL": {
                myQuantity = this.EntityPM.ValueOfGoods;
                break;
            }
            case "PRFR": {
                myQuantity = Tools_1.ArrayTool.Sum(this.EntityPM.ShipmentPayables.filter(function (d) { return d.ChargesGroupCode == "FRT" && Tools_1.AppTool.IsNullOrEmpty(d.ShipmentPayableParentId); }), "ExpectedAmount");
                break;
            }
            case "GWTN": {
                myQuantity = this.EntityPM.GrossWeightPerTon;
                break;
            }
            case "CWKG": {
                myQuantity = this.EntityPM.ChargeableWeightInKG;
                break;
            }
            case "GWKG": {
                myQuantity = this.EntityPM.GrossWeightInKG;
                break;
            }
            case "QTY": {
                myQuantity = this.EntityPM.NumberOfPackages;
                break;
            }
            case "VCBM": {
                myQuantity = this.EntityPM.VolumeInCBM;
                break;
            }
            default: {
                break;
            }
        }
        myRecordPM.Quantity = Tools_1.AppTool.Round(myQuantity, 2);
        // UnitPrice
        var myUnitPrice = item.CostUnitPrice;
        if (this.IsRoutingRate) {
            if (item.IsChargeBySteps) {
                item.QuoteChargePriceSteps.sort(function (a, b) { return a.Step - b.Step; }).forEach(function (step) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(myQuantity) && myQuantity >= step.Step) {
                        myUnitPrice = step.CostUnitPrice;
                    }
                });
                var entitySmallest = item.QuoteChargePriceSteps.sort(function (a, b) { return a.Step - b.Step; })[0];
                if (entitySmallest != null) {
                    if (Tools_1.AppTool.IsNullOrEmpty(myQuantity) || myQuantity < entitySmallest.Step) {
                        myUnitPrice = entitySmallest.CostUnitPrice;
                    }
                }
            }
        }
        myRecordPM.UnitPrice = Tools_1.AppTool.Round(myUnitPrice, 3);
        // ComputedAmount
        var myComputedAmount = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(myQuantity) && !Tools_1.AppTool.IsNullOrEmpty(myUnitPrice)) {
            switch (myRecordPM.MeasurementCode) {
                case "PRVL":
                case "PRFR": {
                    myComputedAmount = myQuantity * myUnitPrice / 100;
                    break;
                }
                default: {
                    myComputedAmount = myQuantity * myUnitPrice;
                    break;
                }
            }
        }
        // MinMax
        if (myComputedAmount != null) {
            if (myRecordPM.QuoteCostMinAmount != null) {
                if (myComputedAmount < myRecordPM.QuoteCostMinAmount) {
                    myComputedAmount = myRecordPM.QuoteCostMinAmount;
                }
            }
            if (myRecordPM.QuoteCostMaxAmount != null) {
                if (myComputedAmount > myRecordPM.QuoteCostMaxAmount) {
                    myComputedAmount = myRecordPM.QuoteCostMaxAmount;
                }
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(myComputedAmount)) {
            myRecordPM.ExpectedAmount = Tools_1.AppTool.Round(myComputedAmount, 2);
            if (!Tools_1.AppTool.IsNullOrEmpty(myRecordPM.Rate)) {
                myRecordPM.ExpectedAmountLocal = Tools_1.AppTool.Round(myRecordPM.ExpectedAmount * myRecordPM.Rate, 2);
                if (!Tools_1.AppTool.IsNullOrEmpty(myRecordPM.ProfitCurrencyExchangeRate)) {
                    myRecordPM.ExpectedAmountInProfitCurrency = Tools_1.AppTool.Round(myRecordPM.ExpectedAmountLocal / myRecordPM.ProfitCurrencyExchangeRate, 2);
                }
            }
        }
        // Other Amounts
        myRecordPM.OpenAmount = myRecordPM.ExpectedAmount;
        myRecordPM.OpenAmountInLocalCurrency = myRecordPM.ExpectedAmountLocal;
        myRecordPM.OpenAmountInProfitCurrency = myRecordPM.ExpectedAmountInProfitCurrency;
        myRecordPM.AccountedAmount = 0;
        myRecordPM.AccountedAmountInLocalCurrency = 0;
        myRecordPM.AccountedAmountInProfitCurrency = 0;
        myRecordPM.ShipmentPayableLineStatusCode = (myQuantity != null && myUnitPrice != null) ? "OAMT" : "EMPT";
    };
    ShipmentGenerator.prototype.ComputePayableQuoteAmounts_FCL = function (itemPM, quantity) {
        if (itemPM.ProfitCurrencyExchangeRate == 0) {
            itemPM.ProfitCurrencyExchangeRate = 1;
        }
        var nweQuantity = quantity;
        var expectedAmount = itemPM.UnitPrice * nweQuantity;
        var expectedAmountLocal = expectedAmount * itemPM.Rate;
        var expectedAmountProfit = expectedAmountLocal / itemPM.ProfitCurrencyExchangeRate;
        itemPM.Quantity = Tools_1.AppTool.Round(nweQuantity, 3);
        itemPM.ExpectedAmount = Tools_1.AppTool.Round(expectedAmount, 2);
        itemPM.ExpectedAmountLocal = Tools_1.AppTool.Round(expectedAmountLocal, 2);
        itemPM.ExpectedAmountInProfitCurrency = Tools_1.AppTool.Round(expectedAmountProfit, 2);
        itemPM.OpenAmount = itemPM.ExpectedAmount;
        itemPM.OpenAmountInLocalCurrency = itemPM.ExpectedAmountLocal;
        itemPM.OpenAmountInProfitCurrency = itemPM.ExpectedAmountInProfitCurrency;
        itemPM.AccountedAmount = 0;
        itemPM.AccountedAmountInLocalCurrency = 0;
        itemPM.AccountedAmountInProfitCurrency = 0;
        itemPM.ShipmentPayableLineStatusCode = (itemPM.Quantity != null && itemPM.UnitPrice != null) ? "OAMT" : "EMPT";
    };
    ShipmentGenerator.prototype.CreateNewPayableFromQuoteCharge = function (item) {
        var _this = this;
        var myRecordPM = new ShipmentPayablePM_1.ShipmentPayablePM(null);
        myRecordPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        myRecordPM.ShipmentId = this.EntityPM.Id;
        myRecordPM.ShipmentNumber = this.EntityPM.ShipmentNumber;
        myRecordPM.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        myRecordPM.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        myRecordPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        myRecordPM.UpdateByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        myRecordPM.IsFromQuote = true;
        myRecordPM.ShipmentPayableLineStatusCode = "EMPT";
        myRecordPM.ShipmentPayableAmountTypeCode = "ACCU";
        myRecordPM.ShipmentPayableAmountTypeName = "Accrual";
        myRecordPM.ChargesTypeId = item.ChargesTypeId;
        myRecordPM.VatTypeId = item.VatTypeId;
        myRecordPM.CurrencyId = item.CostCurrencyId;
        myRecordPM.CurrencyCode = item.CostCurrencyCode;
        myRecordPM.MeasurementId = item.CostMeasurementId;
        myRecordPM.MeasurementCode = item.CostMeasurementCode;
        myRecordPM.MeasurementShortName = item.CostMeasurementShortName;
        myRecordPM.Notes = item.Notes;
        myRecordPM.VendorId = item.VendorId;
        myRecordPM.VendorName = item.VendorName;
        myRecordPM.QuoteChargeId = item.Id;
        myRecordPM.QuoteCostMinAmount = item.CostMinAmount;
        myRecordPM.QuoteCostMaxAmount = item.CostMaxAmount;
        myRecordPM.IsChargeBySteps = item.IsChargeBySteps;
        myRecordPM.Rate = this.GetCurrencyRate(item.CostCurrencyId);
        myRecordPM.ProfitCurrencyExchangeRate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);
        // ChargeType
        if (!Tools_1.AppTool.IsNullOrEmpty(item.ChargesTypeId)) {
            this.myChargesTypeListService.getSingleFromCache(item.ChargesTypeId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var chargesType = myResponse.Result;
                    if (chargesType) {
                        myRecordPM.ChargesTypeCode = chargesType.Code;
                        myRecordPM.ChargesTypeName = chargesType.EnglishName;
                        myRecordPM.ChargesGroupCode = chargesType.ChargesGroupCode;
                        myRecordPM.DueTypeCode = chargesType.DueTypeCode;
                        myRecordPM.DueTypeName = chargesType.DueTypeName;
                        myRecordPM.IATACodeId = chargesType.IATACodeId;
                        myRecordPM.PrepaidCollectId = chargesType.ChargesGroupCode == "FRT" ? _this.EntityPM.FreightPrepaidCollectId : _this.EntityPM.OtherPrepaidCollectId;
                        myRecordPM.ViewOrder = chargesType.ViewOrder;
                        if (Tools_1.AppTool.IsNullOrEmpty(myRecordPM.VatTypeId)) {
                            myRecordPM.VatTypeId = chargesType.VatTypeId;
                        }
                    }
                }
            });
        }
        return myRecordPM;
    };
    ShipmentGenerator.prototype.CreateNewPayableFromQuoteCharge_FCL = function (myPackageTypeId, myUnitPrice, item) {
        if (!Tools_1.AppTool.IsNullOrEmpty(myPackageTypeId)) {
            if (!Tools_1.AppTool.IsNullOrEmpty(myUnitPrice)) {
                var itemGrouped = this.BCNTGrouped.filter(function (f) { return f.PackageTypeId == myPackageTypeId; })[0];
                if (itemGrouped != null) {
                    var newItemPM = this.CreateNewPayableFromQuoteCharge(item);
                    newItemPM.UnitPrice = myUnitPrice;
                    newItemPM.MeasurementId = itemGrouped.MeasurementId;
                    newItemPM.MeasurementCode = itemGrouped.MeasurementCode;
                    newItemPM.MeasurementShortName = itemGrouped.MeasurementShortName;
                    this.ComputePayableQuoteAmounts_FCL(newItemPM, itemGrouped.Quantity);
                    this.EntityPM.AddPayable(newItemPM);
                }
            }
        }
    };
    ShipmentGenerator.prototype.CreateNewPayableFromOriginShipment = function (OriginItemPM) {
        if (OriginItemPM) {
            if (OriginItemPM.ShipmentPayableAmountTypeCode == "ACCU") {
                var myRecordPM = new ShipmentPayablePM_1.ShipmentPayablePM(null);
                myRecordPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                myRecordPM.ShipmentId = this.EntityPM.Id;
                myRecordPM.ShipmentNumber = this.EntityPM.ShipmentNumber;
                myRecordPM.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                myRecordPM.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                myRecordPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                myRecordPM.UpdateByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                myRecordPM.ShipmentPayableAmountTypeCode = "ACCU";
                myRecordPM.ShipmentPayableAmountTypeName = "Accrual";
                myRecordPM.ShipmentPayableLineStatusCode = "EMPT";
                myRecordPM.ShipmentPayableLineStatusName = "Empty";
                myRecordPM.ChargesTypeId = OriginItemPM.ChargesTypeId;
                myRecordPM.ChargesTypeCode = OriginItemPM.ChargesTypeCode;
                myRecordPM.ChargesTypeName = OriginItemPM.ChargesTypeName;
                myRecordPM.ChargesGroupCode = OriginItemPM.ChargesGroupCode;
                myRecordPM.CurrencyId = OriginItemPM.CurrencyId;
                myRecordPM.CurrencyCode = OriginItemPM.CurrencyCode;
                myRecordPM.DueTypeCode = OriginItemPM.DueTypeCode;
                myRecordPM.DueTypeName = OriginItemPM.DueTypeName;
                myRecordPM.MeasurementId = OriginItemPM.MeasurementId;
                myRecordPM.MeasurementCode = OriginItemPM.MeasurementCode;
                myRecordPM.MeasurementShortName = OriginItemPM.MeasurementShortName;
                myRecordPM.IATACodeId = OriginItemPM.IATACodeId;
                myRecordPM.PrepaidCollectId = OriginItemPM.PrepaidCollectId;
                myRecordPM.ViewOrder = OriginItemPM.ViewOrder;
                myRecordPM.AWBPrint = OriginItemPM.AWBPrint;
                myRecordPM.Notes = OriginItemPM.Notes;
                myRecordPM.VendorId = OriginItemPM.VendorId;
                myRecordPM.VendorName = OriginItemPM.VendorName;
                myRecordPM.ValueDate = OriginItemPM.ValueDate;
                myRecordPM.UOMPercentage = OriginItemPM.UOMPercentage;
                myRecordPM.UnitPrice = OriginItemPM.UnitPrice;
                myRecordPM.MaxAmount = OriginItemPM.MaxAmount;
                myRecordPM.MinAmount = OriginItemPM.MinAmount;
                myRecordPM.IsBackToBack = OriginItemPM.IsBackToBack;
                myRecordPM.ProfitCurrencyExchangeRate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);
                myRecordPM.VatTypeId = OriginItemPM.VatTypeId;
                myRecordPM.IsFromQuote = OriginItemPM.IsFromQuote;
                myRecordPM.QuoteChargeId = OriginItemPM.QuoteChargeId;
                myRecordPM.IsChargeBySteps = OriginItemPM.IsChargeBySteps;
                myRecordPM.QuoteCostMinAmount = OriginItemPM.QuoteCostMinAmount;
                myRecordPM.QuoteCostMaxAmount = OriginItemPM.QuoteCostMaxAmount;
                //if (OriginItemPM.IsFromQuote) {
                //    myRecordPM.VatTypeId = OriginItemPM.VatTypeId;
                //}
                this.EntityPM.AddPayable(myRecordPM);
            }
        }
    };
    ShipmentGenerator.prototype.CreateNewPayableFromLCLChargesType = function (item) {
        var newRecord = new ShipmentPayablePM_1.ShipmentPayablePM(this.EntityPM);
        newRecord.Tenant = SessionLocator_1.SessionLocator.Tenant;
        newRecord.ShipmentId = this.EntityPM.Id;
        newRecord.ShipmentNumber = this.EntityPM.ShipmentNumber;
        newRecord.ShipmentPayableLineStatusCode = "EMPT";
        newRecord.ShipmentPayableAmountTypeCode = "ACCU";
        newRecord.ShipmentPayableAmountTypeName = "Accrual";
        newRecord.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newRecord.UpdateByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newRecord.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        newRecord.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        newRecord.ChargesTypeId = item.Id;
        newRecord.ChargesTypeCode = item.Code;
        newRecord.ChargesTypeName = item.EnglishName;
        newRecord.MeasurementId = item.MeasurementId;
        newRecord.MeasurementCode = item.MeasurementCode;
        newRecord.MeasurementShortName = item.MeasurementShortName;
        newRecord.VatTypeId = item.VatTypeId;
        newRecord.ChargesGroupCode = item.ChargesGroupCode;
        newRecord.DueTypeCode = item.DueTypeCode;
        newRecord.DueTypeName = item.DueTypeName;
        newRecord.IATACodeId = item.IATACodeId;
        newRecord.ViewOrder = item.ViewOrder;
        newRecord.IsBackToBack = item.IsBackToBack;
        newRecord.PrepaidCollectId = item.ChargesGroupCode == "FRT" ? this.EntityPM.FreightPrepaidCollectId : this.EntityPM.OtherPrepaidCollectId;
        switch (item.MeasurementCode) {
            case "GRWT": {
                newRecord.Quantity = this.EntityPM.GrossWeight;
                break;
            }
            case "CHWT": {
                newRecord.Quantity = this.EntityPM.ChargeableWeight;
                break;
            }
            case "VOLU": {
                newRecord.Quantity = this.EntityPM.Volume;
                break;
            }
            case "BTEU": {
                newRecord.Quantity = this.EntityPM.TEU;
                break;
            }
            case "FIXD": {
                newRecord.Quantity = 1;
                break;
            }
            case "PRVL": {
                newRecord.Quantity = this.EntityPM.ValueOfGoods;
                break;
            }
            case "GWTN": {
                newRecord.Quantity = this.EntityPM.GrossWeightPerTon;
                break;
            }
            case "CWKG": {
                newRecord.Quantity = this.EntityPM.ChargeableWeightInKG;
                break;
            }
            case "GWKG": {
                newRecord.Quantity = this.EntityPM.GrossWeightInKG;
                break;
            }
            case "QTY": {
                newRecord.Quantity = this.EntityPM.NumberOfPackages;
                break;
            }
            case "VCBM": {
                newRecord.Quantity = this.EntityPM.VolumeInCBM;
                break;
            }
            default: {
                break;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(item.PayablesDefaultCurrencyId)) {
            newRecord.CurrencyId = item.PayablesDefaultCurrencyId;
        }
        else {
            if (item.ChargesGroupCode == "FRT" || item.ChargesGroupCode == "SCH") {
                newRecord.CurrencyId = SessionLocator_1.SessionLocator.TenantPM.FreightCurrencyId;
            }
            else {
                newRecord.CurrencyId = SessionLocator_1.SessionLocator.TenantPM.OtherChargesCurrencyId;
            }
        }
        this.GetCurrencyCode(newRecord);
        newRecord.Rate = this.GetCurrencyRate(newRecord.CurrencyId);
        newRecord.ProfitCurrencyExchangeRate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);
        return newRecord;
    };
    // Receivables
    ShipmentGenerator.prototype.GenerateReceivablesAutoDisplay = function () {
        var _this = this;
        this.myChargesTypeListService.getAllFromCache().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var allChargesTypes = myResponse.Result;
                if (allChargesTypes != null) {
                    allChargesTypes = allChargesTypes.filter(function (d) { return d.IsReceivable == true && d.InActive == false; });
                    if (_this.EntityPM.ShipmentLevelCode == "C") {
                        allChargesTypes = allChargesTypes.filter(function (d) { return d.IsAutoDisplayInConsolidation; });
                    }
                    else {
                        if (_this.EntityPM.IncludesCustoms) {
                            allChargesTypes = allChargesTypes.filter(function (d) { return d.IsAutoDisplayInShipment == true || d.IsAutoDisplayInCustoms; });
                        }
                        else {
                            allChargesTypes = allChargesTypes.filter(function (d) { return d.IsAutoDisplayInShipment == true; });
                        }
                    }
                    switch (_this.EntityPM.TransportModeId) {
                        case "A":
                            {
                                allChargesTypes = allChargesTypes.filter(function (r) { return r.IsAir; });
                                break;
                            }
                        case "O":
                            {
                                allChargesTypes = allChargesTypes.filter(function (r) { return r.IsOcean; });
                                break;
                            }
                        case "I":
                            {
                                allChargesTypes = allChargesTypes.filter(function (r) { return r.IsInland; });
                                break;
                            }
                    }
                    switch (_this.EntityPM.DirectionId) {
                        case "E":
                            {
                                allChargesTypes = allChargesTypes.filter(function (r) { return r.IsExport; });
                                break;
                            }
                        case "I":
                            {
                                allChargesTypes = allChargesTypes.filter(function (r) { return r.IsImport; });
                                break;
                            }
                        case "D":
                            {
                                allChargesTypes = allChargesTypes.filter(function (r) { return r.IsDomestic; });
                                break;
                            }
                        case "R":
                            {
                                allChargesTypes = allChargesTypes.filter(function (r) { return r.IsDrop; });
                                break;
                            }
                    }
                    if (allChargesTypes.length > 0) {
                        if (_this.IsLCLEntity) {
                            allChargesTypes.sort(function (a, b) { return a.ViewOrder - b.ViewOrder; }).forEach(function (item) {
                                _this.EntityPM.AddReceivable(_this.CreateNewReceivableFromLCLChargesType(item));
                            });
                            _this.EntityPM.ShipmentReceivables.filter(function (f) { return f.MeasurementCode == "PRFR"; }).forEach(function (item) {
                                item.Quantity = Tools_1.ArrayTool.Sum(_this.EntityPM.ShipmentReceivables.filter(function (d) { return d.ChargesGroupCode == "FRT" && Tools_1.AppTool.IsNullOrEmpty(d.ShipmentReceivableParentId); }), "TotalAmount");
                            });
                        }
                        else {
                            if (_this.BCNTGrouped.length > 0) {
                                allChargesTypes.filter(function (f) { return f.ContainerMeasurementCode == "BCNT"; }).sort(function (a, b) { return a.ViewOrder - b.ViewOrder; }).forEach(function (myChargeType) {
                                    _this.BCNTGrouped.forEach(function (itemGrouped) {
                                        var newReceivable = new ShipmentReceivablePM_1.ShipmentReceivablePM(_this.EntityPM);
                                        newReceivable.Tenant = SessionLocator_1.SessionLocator.Tenant;
                                        newReceivable.ShipmentId = _this.EntityPM.Id;
                                        newReceivable.ShipmentNumber = _this.EntityPM.ShipmentNumber;
                                        newReceivable.ShipmentReceivableLineStatusCode = "EMPT";
                                        newReceivable.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                                        newReceivable.UpdateByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                                        newReceivable.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                                        newReceivable.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                                        newReceivable.ChargesTypeId = myChargeType.Id;
                                        newReceivable.ChargesTypeCode = myChargeType.Code;
                                        newReceivable.ChargesTypeName = myChargeType.EnglishName;
                                        newReceivable.VatTypeId = myChargeType.VatTypeId;
                                        newReceivable.ChargesGroupCode = myChargeType.ChargesGroupCode;
                                        newReceivable.DueTypeCode = myChargeType.DueTypeCode;
                                        newReceivable.DueTypeName = myChargeType.DueTypeName;
                                        newReceivable.IATACodeId = myChargeType.IATACodeId;
                                        newReceivable.ViewOrder = myChargeType.ViewOrder;
                                        newReceivable.PrepaidCollectId = myChargeType.ChargesGroupCode == "FRT" ? _this.EntityPM.FreightPrepaidCollectId : _this.EntityPM.OtherPrepaidCollectId;
                                        newReceivable.Quantity = itemGrouped.Quantity;
                                        newReceivable.MeasurementId = itemGrouped.MeasurementId;
                                        newReceivable.MeasurementCode = itemGrouped.MeasurementCode;
                                        newReceivable.MeasurementShortName = itemGrouped.MeasurementShortName;
                                        newReceivable.IsBackToBack = myChargeType.IsBackToBack;
                                        newReceivable.IsExpense = myChargeType.IsExpense;
                                        if (!Tools_1.AppTool.IsNullOrEmpty(myChargeType.ReceivablesDefaultCurrencyId)) {
                                            newReceivable.CurrencyId = myChargeType.ReceivablesDefaultCurrencyId;
                                        }
                                        else {
                                            if (myChargeType.ChargesGroupCode == "FRT" || myChargeType.ChargesGroupCode == "SCH") {
                                                newReceivable.CurrencyId = SessionLocator_1.SessionLocator.TenantPM.FreightCurrencyId;
                                            }
                                            else {
                                                newReceivable.CurrencyId = SessionLocator_1.SessionLocator.TenantPM.OtherChargesCurrencyId;
                                            }
                                        }
                                        _this.GetCurrencyCode(newReceivable);
                                        newReceivable.ProfitCurrencyExchangeRate = _this.GetCurrencyRate(_this.EntityPM.ProfitCurrencyId);
                                        newReceivable.Rate = _this.GetCurrencyRate(newReceivable.CurrencyId);
                                        newReceivable.ProfitCurrencyExchangeRate = _this.GetCurrencyRate(_this.EntityPM.ProfitCurrencyId);
                                        var existsReceivable = _this.EntityPM.ShipmentReceivables.filter(function (d) { return d.ChargesTypeId == newReceivable.ChargesTypeId && d.MeasurementId == newReceivable.MeasurementId; })[0];
                                        if (existsReceivable == null) {
                                            _this.EntityPM.AddReceivable(newReceivable);
                                        }
                                        else {
                                            //var acctReceivable: ShipmentReceivablePM = this.EntityPM.ShipmentReceivables.filter(d => d.ChargesTypeId == newReceivable.ChargesTypeId && d.MeasurementId == newReceivable.MeasurementId && (d.ShipmentReceivableLineStatusCode == "ACCT" || d.ShipmentReceivableLineStatusCode == "DRFT"))[0];
                                            //var openReceivable: ShipmentReceivablePM = this.EntityPM.ShipmentReceivables.filter(d => d.ChargesTypeId == newReceivable.ChargesTypeId && d.MeasurementId == newReceivable.MeasurementId && (d.ShipmentReceivableLineStatusCode == "EMPT" || d.ShipmentReceivableLineStatusCode == "OAMT"))[0];
                                            var acctReceivable = _this.EntityPM.ShipmentReceivables.filter(function (d) { return d.ChargesTypeId == newReceivable.ChargesTypeId && d.MeasurementId == newReceivable.MeasurementId && d.ShipmentReceivableLineStatusCode == "ACCT"; })[0];
                                            var openReceivable = _this.EntityPM.ShipmentReceivables.filter(function (d) { return d.ChargesTypeId == newReceivable.ChargesTypeId && d.MeasurementId == newReceivable.MeasurementId && d.ShipmentReceivableLineStatusCode != "ACCT"; })[0];
                                            if (acctReceivable == null) {
                                                openReceivable.Quantity = newReceivable.Quantity;
                                            }
                                            if (acctReceivable != null && newReceivable.Quantity > acctReceivable.Quantity) {
                                                if (openReceivable == null) {
                                                    _this.EntityPM.AddReceivable(newReceivable);
                                                }
                                                else {
                                                    openReceivable.Quantity = newReceivable.Quantity;
                                                }
                                            }
                                        }
                                    });
                                });
                            }
                            allChargesTypes.filter(function (f) { return Tools_1.AppTool.IsNullOrEmpty(f.ContainerMeasurementId); }).sort(function (a, b) { return a.ViewOrder - b.ViewOrder; }).forEach(function (myChargeType) {
                                _this.EntityPM.AddReceivable(_this.CreateNewReceivableFromLCLChargesType(myChargeType));
                            });
                        }
                    }
                }
            }
        });
    };
    ShipmentGenerator.prototype.GenerateReceivablesFromQuote = function (baseQuote) {
        this.BaseQuote = baseQuote;
        if (this.EntityPM && this.BaseQuote) {
            this.IsAdhoc = this.BaseQuote.QuoteTypeCode == "A" ? true : false;
            this.IsRoutingRate = !this.IsAdhoc;
            var rate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);
            var quoteProfitInLocalCurrency = null;
            var quoteProfitInProfitCurrency = null;
            if (this.BaseQuote.QuoteTypeCode == "A") {
                quoteProfitInLocalCurrency = this.BaseQuote.EstimateProfit * this.BaseQuote.ExchangeRate;
                quoteProfitInProfitCurrency = quoteProfitInLocalCurrency / rate;
            }
            this.EntityPM.EstimateProfitInLocalCurrency = Tools_1.AppTool.Round(quoteProfitInLocalCurrency, 2);
            this.EntityPM.EstimateProfitInProfitCurrency = Tools_1.AppTool.Round(quoteProfitInProfitCurrency, 2);
            if (this.IsLCLEntity) {
                this.GenerateReceivablesFromQuote_LCL();
            }
            else {
                this.GenerateReceivablesFromQuote_FCL();
            }
        }
    };
    ShipmentGenerator.prototype.GenerateReceivablesFromOriginShipment = function (originShipment) {
        var _this = this;
        this.OriginShipment = originShipment;
        if (this.EntityPM && this.OriginShipment) {
            if (this.IsLCLEntity) {
                this.OriginShipment.ShipmentReceivables.forEach(function (item) {
                    _this.CreateNewReceivableFromOriginShipment(item);
                });
            }
            else {
                this.OriginShipment.ShipmentReceivables.forEach(function (item) {
                    switch (item.MeasurementCode) {
                        case "GRWT":
                        case "CHWT":
                        case "VOLU":
                        case "BTEU":
                        case "FIXD":
                        case "PRVL":
                        case "PRFR":
                        case "GWTN":
                        case "CWKG":
                        case "GWKG":
                        case "VCBM":
                        case "QTY":
                            {
                                _this.CreateNewReceivableFromOriginShipment(item);
                                break;
                            }
                        case "BCNT": {
                            // No such case
                            break;
                        }
                        default: {
                            var itemGrouped = _this.BCNTGrouped.filter(function (f) { return f.MeasurementId == item.MeasurementId; })[0];
                            if (itemGrouped != null) {
                                _this.CreateNewReceivableFromOriginShipment(item);
                            }
                            break;
                        }
                    }
                });
            }
        }
    };
    ShipmentGenerator.prototype.GenerateReceivablesFromQuote_LCL = function () {
        var _this = this;
        this.BaseQuote.QuoteCharges.sort(function (a, b) { return a.ViewOrder - b.ViewOrder; }).filter(function (f) { return f.IsAllIN == false; }).forEach(function (item) {
            if (item.IsChargeBySteps) {
                var newReceivablePM = _this.CreateNewReceivableFromQuoteCharge(item);
                _this.EntityPM.AddReceivable(newReceivablePM);
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(item.SaleUnitPrice)) {
                var newReceivablePM = _this.CreateNewReceivableFromQuoteCharge(item);
                _this.EntityPM.AddReceivable(newReceivablePM);
            }
        });
        this.ComputeAllReceivablesQuote_LCL();
    };
    ShipmentGenerator.prototype.GenerateReceivablesFromQuote_FCL = function () {
        var _this = this;
        this.BaseQuote.QuoteCharges.sort(function (a, b) { return a.ViewOrder - b.ViewOrder; }).filter(function (f) { return f.IsAllIN == false; }).forEach(function (item) {
            if (item.SaleMeasurementCode == "BCNT") {
                _this.CreateNewReceivableFromQuoteCharge_FCL(_this.BaseQuote.PackageType1Id, item.SaleContainerType1UnitPrice, item);
                _this.CreateNewReceivableFromQuoteCharge_FCL(_this.BaseQuote.PackageType2Id, item.SaleContainerType2UnitPrice, item);
                _this.CreateNewReceivableFromQuoteCharge_FCL(_this.BaseQuote.PackageType3Id, item.SaleContainerType3UnitPrice, item);
                _this.CreateNewReceivableFromQuoteCharge_FCL(_this.BaseQuote.PackageType4Id, item.SaleContainerType4UnitPrice, item);
                _this.CreateNewReceivableFromQuoteCharge_FCL(_this.BaseQuote.PackageType5Id, item.SaleContainerType5UnitPrice, item);
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(item.SaleUnitPrice)) {
                var newReceivablePM = _this.CreateNewReceivableFromQuoteCharge(item);
                newReceivablePM.UnitPrice = item.SaleUnitPrice;
                switch (item.CostMeasurementCode) {
                    case "BCNT":
                    case "GRWT":
                    case "CHWT":
                    case "VOLU":
                    case "BTEU":
                    case "FIXD":
                    case "PRVL":
                    case "PRFR":
                    case "GWTN":
                    case "CWKG":
                    case "GWKG":
                    case "VCBM":
                    case "QTY":
                        {
                            break;
                        }
                    default: {
                        if (item.CostMeasurementId) {
                            var itemGrouped = _this.BCNTGrouped.filter(function (f) { return f.MeasurementId == item.CostMeasurementId; })[0];
                            if (itemGrouped) {
                                _this.ComputeReceivableQuoteAmounts_FCL(newReceivablePM, itemGrouped.Quantity);
                            }
                        }
                        break;
                    }
                }
                _this.EntityPM.AddReceivable(newReceivablePM);
            }
        });
        // Add Other Shipment Containers on Quote Records
        this.BCNTGrouped.forEach(function (itemGrouped) {
            var myRecord = _this.EntityPM.ShipmentReceivables.filter(function (f) { return f.MeasurementId == itemGrouped.MeasurementId; })[0];
            if (myRecord == null) {
                _this.BaseQuote.QuoteCharges.filter(function (f) { return f.IsAllIN == false; }).forEach(function (item) {
                    if (item.SaleMeasurementCode == "BCNT") {
                        var newRecord = new ShipmentReceivablePM_1.ShipmentReceivablePM(null);
                        newRecord.Tenant = _this.EntityPM.Tenant;
                        newRecord.ShipmentId = _this.EntityPM.Id;
                        newRecord.ShipmentNumber = _this.EntityPM.ShipmentNumber;
                        newRecord.ShipmentReceivableLineStatusCode = "EMPT";
                        newRecord.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                        newRecord.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                        newRecord.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                        newRecord.UpdateByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                        newRecord.Quantity = itemGrouped.Quantity;
                        newRecord.MeasurementId = itemGrouped.MeasurementId;
                        newRecord.MeasurementCode = itemGrouped.MeasurementCode;
                        newRecord.MeasurementShortName = itemGrouped.MeasurementShortName;
                        _this.myChargesTypeListService.getSingleFromCache(item.ChargesTypeId).subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                var chargesType = myResponse.Result;
                                if (chargesType) {
                                    newRecord.ChargesTypeId = chargesType.Id;
                                    newRecord.ChargesTypeCode = chargesType.Code;
                                    newRecord.ChargesTypeName = chargesType.EnglishName;
                                    newRecord.VatTypeId = chargesType.VatTypeId;
                                    newRecord.ChargesGroupCode = chargesType.ChargesGroupCode;
                                    newRecord.DueTypeCode = chargesType.DueTypeCode;
                                    newRecord.DueTypeName = chargesType.DueTypeName;
                                    newRecord.IATACodeId = chargesType.IATACodeId;
                                    newRecord.PrepaidCollectId = chargesType.ChargesGroupCode == "FRT" ? _this.EntityPM.FreightPrepaidCollectId : _this.EntityPM.OtherPrepaidCollectId;
                                    newRecord.IsBackToBack = chargesType.IsBackToBack;
                                    if (chargesType.ChargesGroupCode == "FRT" || chargesType.ChargesGroupCode == "SCH") {
                                        newRecord.CurrencyId = SessionLocator_1.SessionLocator.TenantPM.FreightCurrencyId;
                                    }
                                    else {
                                        newRecord.CurrencyId = SessionLocator_1.SessionLocator.TenantPM.OtherChargesCurrencyId;
                                    }
                                    _this.GetCurrencyCode(newRecord);
                                    newRecord.Rate = _this.GetCurrencyRate(newRecord.CurrencyId);
                                    newRecord.ProfitCurrencyExchangeRate = _this.GetCurrencyRate(_this.EntityPM.ProfitCurrencyId);
                                }
                            }
                        });
                        _this.EntityPM.AddReceivable(newRecord);
                    }
                });
            }
        });
        this.ComputeAllReceivablesQuote_LCL();
    };
    ShipmentGenerator.prototype.ComputeAllReceivablesQuote_LCL = function () {
        var _this = this;
        this.EntityPM.ShipmentReceivables.filter(function (f) { return !Tools_1.AppTool.IsNullOrEmpty(f.QuoteChargeId); }).forEach(function (item) {
            switch (item.MeasurementCode) {
                case "GRWT":
                case "CHWT":
                case "VOLU":
                case "BTEU":
                case "FIXD":
                case "PRVL":
                case "GWTN":
                case "CWKG":
                case "GWKG":
                case "VCBM":
                case "QTY":
                    {
                        var itemCharge = _this.BaseQuote.QuoteCharges.filter(function (f) { return f.Id == item.QuoteChargeId; })[0];
                        _this.ComputeReceivableQuoteAmounts_LCL(item, itemCharge);
                        break;
                    }
            }
        });
        this.EntityPM.ShipmentReceivables.filter(function (f) { return !Tools_1.AppTool.IsNullOrEmpty(f.QuoteChargeId); }).forEach(function (item) {
            switch (item.MeasurementCode) {
                case "PRFR":
                    {
                        var itemCharge = _this.BaseQuote.QuoteCharges.filter(function (f) { return f.Id == item.QuoteChargeId; })[0];
                        _this.ComputeReceivableQuoteAmounts_LCL(item, itemCharge);
                        break;
                    }
            }
        });
    };
    ShipmentGenerator.prototype.ComputeReceivableQuoteAmounts_LCL = function (myRecordPM, item) {
        // Quantity
        var myQuantity = null;
        switch (myRecordPM.MeasurementCode) {
            case "GRWT": {
                myQuantity = this.EntityPM.GrossWeight;
                break;
            }
            case "CHWT": {
                myQuantity = this.EntityPM.ChargeableWeight;
                break;
            }
            case "VOLU": {
                myQuantity = this.EntityPM.Volume;
                break;
            }
            case "BTEU": {
                myQuantity = this.EntityPM.TEU;
                break;
            }
            case "FIXD": {
                myQuantity = 1;
                break;
            }
            case "PRVL": {
                myQuantity = this.EntityPM.ValueOfGoods;
                break;
            }
            case "PRFR": {
                myQuantity = Tools_1.ArrayTool.Sum(this.EntityPM.ShipmentReceivables.filter(function (d) { return d.ChargesGroupCode == "FRT" && Tools_1.AppTool.IsNullOrEmpty(d.ShipmentReceivableParentId); }), "TotalAmount");
                break;
            }
            case "GWTN": {
                myQuantity = this.EntityPM.GrossWeightPerTon;
                break;
            }
            case "CWKG": {
                myQuantity = this.EntityPM.ChargeableWeightInKG;
                break;
            }
            case "GWKG": {
                myQuantity = this.EntityPM.GrossWeightInKG;
                break;
            }
            case "QTY": {
                myQuantity = this.EntityPM.NumberOfPackages;
                break;
            }
            case "VCBM": {
                myQuantity = this.EntityPM.VolumeInCBM;
                break;
            }
            default: {
                break;
            }
        }
        myRecordPM.Quantity = Tools_1.AppTool.Round(myQuantity, 2);
        // UnitPrice
        var myUnitPrice = item.SaleUnitPrice;
        if (this.IsRoutingRate) {
            if (item.IsChargeBySteps) {
                item.QuoteChargePriceSteps.sort(function (a, b) { return a.Step - b.Step; }).forEach(function (step) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(myQuantity) && myQuantity >= step.Step) {
                        myUnitPrice = step.SaleUnitPrice;
                    }
                });
                var entitySmallest = item.QuoteChargePriceSteps.sort(function (a, b) { return a.Step - b.Step; })[0];
                if (entitySmallest != null) {
                    if (Tools_1.AppTool.IsNullOrEmpty(myQuantity) || myQuantity < entitySmallest.Step) {
                        myUnitPrice = entitySmallest.SaleUnitPrice;
                    }
                }
            }
        }
        myRecordPM.UnitPrice = Tools_1.AppTool.Round(myUnitPrice, 3);
        // ComputedAmount
        var myComputedAmount = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(myQuantity) && !Tools_1.AppTool.IsNullOrEmpty(myUnitPrice)) {
            switch (myRecordPM.MeasurementCode) {
                case "PRVL":
                case "PRFR": {
                    myComputedAmount = myQuantity * myUnitPrice / 100;
                    break;
                }
                default: {
                    myComputedAmount = myQuantity * myUnitPrice;
                    break;
                }
            }
        }
        // MinMax
        if (myComputedAmount != null) {
            if (myRecordPM.QuoteSaleMinAmount != null) {
                if (myComputedAmount < myRecordPM.QuoteSaleMinAmount) {
                    myComputedAmount = myRecordPM.QuoteSaleMinAmount;
                }
            }
            if (myRecordPM.QuoteSaleMaxAmount != null) {
                if (myComputedAmount > myRecordPM.QuoteSaleMaxAmount) {
                    myComputedAmount = myRecordPM.QuoteSaleMaxAmount;
                }
            }
        }
        if (myRecordPM.IsFixedPrice) {
            myRecordPM.TotalAmount = item.SaleTotalAmount;
            myRecordPM.TotalAmountLocal = item.SaleTotalAmountLocal;
            if (!Tools_1.AppTool.IsNullOrEmpty(myRecordPM.ProfitCurrencyExchangeRate)) {
                myRecordPM.AmountInProfitCurrency = Tools_1.AppTool.Round(myRecordPM.TotalAmountLocal / myRecordPM.ProfitCurrencyExchangeRate, 2);
            }
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(myComputedAmount)) {
            myRecordPM.TotalAmount = Tools_1.AppTool.Round(myComputedAmount, 2);
            if (!Tools_1.AppTool.IsNullOrEmpty(myRecordPM.Rate)) {
                myRecordPM.TotalAmountLocal = Tools_1.AppTool.Round(myRecordPM.TotalAmount * myRecordPM.Rate, 2);
                if (!Tools_1.AppTool.IsNullOrEmpty(myRecordPM.ProfitCurrencyExchangeRate)) {
                    myRecordPM.AmountInProfitCurrency = Tools_1.AppTool.Round(myRecordPM.TotalAmountLocal / myRecordPM.ProfitCurrencyExchangeRate, 2);
                }
            }
        }
        myRecordPM.ShipmentReceivableLineStatusCode = (myQuantity != null && myUnitPrice != null) ? "OAMT" : "EMPT";
    };
    ShipmentGenerator.prototype.ComputeReceivableQuoteAmounts_FCL = function (itemPM, quantity) {
        if (itemPM.ProfitCurrencyExchangeRate == 0) {
            itemPM.ProfitCurrencyExchangeRate = 1;
        }
        var newQuantity = quantity;
        var amount = itemPM.UnitPrice * newQuantity;
        var amountInLocal = amount * itemPM.Rate;
        var amountInProfit = amountInLocal / itemPM.ProfitCurrencyExchangeRate;
        itemPM.Quantity = Tools_1.AppTool.Round(newQuantity, 2);
        itemPM.TotalAmount = Tools_1.AppTool.Round(amount, 2);
        itemPM.TotalAmountLocal = Tools_1.AppTool.Round(amountInLocal, 2);
        itemPM.AmountInProfitCurrency = Tools_1.AppTool.Round(amountInProfit, 2);
        itemPM.ShipmentReceivableLineStatusCode = (itemPM.Quantity != null && itemPM.UnitPrice != null) ? "OAMT" : "EMPT";
    };
    ShipmentGenerator.prototype.CreateNewReceivableFromQuoteCharge = function (QuoteCharge) {
        var _this = this;
        var myRecordPM = new ShipmentReceivablePM_1.ShipmentReceivablePM(null);
        myRecordPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        myRecordPM.ShipmentId = this.EntityPM.Id;
        myRecordPM.ShipmentNumber = this.EntityPM.ShipmentNumber;
        myRecordPM.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        myRecordPM.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        myRecordPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        myRecordPM.UpdateByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        myRecordPM.IsFromQuote = true;
        myRecordPM.IsFixedPrice = this.BaseQuote.IsFixedPrice;
        myRecordPM.ChargesTypeId = QuoteCharge.ChargesTypeId;
        myRecordPM.ChargesTypeCode = QuoteCharge.ChargesTypeCode;
        myRecordPM.ChargesTypeName = QuoteCharge.ChargesTypeName;
        myRecordPM.VatTypeId = QuoteCharge.VatTypeId;
        myRecordPM.CurrencyId = QuoteCharge.SaleCurrencyId;
        myRecordPM.CurrencyCode = QuoteCharge.SaleCurrencyCode;
        myRecordPM.MeasurementId = QuoteCharge.SaleMeasurementId;
        myRecordPM.MeasurementCode = QuoteCharge.SaleMeasurementCode;
        myRecordPM.MeasurementShortName = QuoteCharge.SaleMeasurementShortName;
        myRecordPM.Notes = QuoteCharge.Notes;
        myRecordPM.IsExchangeRateFixed = QuoteCharge.SaleIsFixedRate;
        myRecordPM.QuoteSaleMinAmount = QuoteCharge.SaleMinAmount;
        myRecordPM.QuoteSaleMaxAmount = QuoteCharge.SaleMaxAmount;
        myRecordPM.QuoteChargeId = QuoteCharge.Id;
        myRecordPM.IsChargeBySteps = QuoteCharge.IsChargeBySteps;
        myRecordPM.ProfitCurrencyExchangeRate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);
        myRecordPM.ShipmentReceivableLineStatusCode = "EMPT";
        if (QuoteCharge.SaleIsFixedRate) {
            myRecordPM.Rate = QuoteCharge.SaleExchangeRate;
        }
        else {
            myRecordPM.Rate = this.GetCurrencyRate(QuoteCharge.SaleCurrencyId);
        }
        // ChargeType
        this.myChargesTypeListService.getSingleFromCache(QuoteCharge.ChargesTypeId).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var chargesType = myResponse.Result;
                if (chargesType) {
                    myRecordPM.ChargesGroupCode = chargesType.ChargesGroupCode;
                    myRecordPM.DueTypeCode = chargesType.DueTypeCode;
                    myRecordPM.DueTypeName = chargesType.DueTypeName;
                    myRecordPM.IATACodeId = chargesType.IATACodeId;
                    myRecordPM.PrepaidCollectId = chargesType.ChargesGroupCode == "FRT" ? _this.EntityPM.FreightPrepaidCollectId : _this.EntityPM.OtherPrepaidCollectId;
                    myRecordPM.ViewOrder = chargesType.ViewOrder;
                    myRecordPM.IsExpense = chargesType.IsExpense;
                    if (Tools_1.AppTool.IsNullOrEmpty(myRecordPM.VatTypeId)) {
                        myRecordPM.VatTypeId = chargesType.VatTypeId;
                    }
                }
            }
        });
        return myRecordPM;
    };
    ShipmentGenerator.prototype.CreateNewReceivableFromQuoteCharge_FCL = function (myPackageTypeId, myUnitPrice, item) {
        if (!Tools_1.AppTool.IsNullOrEmpty(myPackageTypeId)) {
            if (!Tools_1.AppTool.IsNullOrEmpty(myUnitPrice)) {
                var itemGrouped = this.BCNTGrouped.filter(function (f) { return f.PackageTypeId == myPackageTypeId; })[0];
                if (itemGrouped != null) {
                    var newItemPM = this.CreateNewReceivableFromQuoteCharge(item);
                    newItemPM.UnitPrice = myUnitPrice;
                    newItemPM.MeasurementId = itemGrouped.MeasurementId;
                    newItemPM.MeasurementCode = itemGrouped.MeasurementCode;
                    newItemPM.MeasurementShortName = itemGrouped.MeasurementShortName;
                    this.ComputeReceivableQuoteAmounts_FCL(newItemPM, itemGrouped.Quantity);
                    this.EntityPM.AddReceivable(newItemPM);
                }
            }
        }
    };
    ShipmentGenerator.prototype.CreateNewReceivableFromOriginShipment = function (OriginItemPM) {
        if (OriginItemPM) {
            var myRecordPM = new ShipmentReceivablePM_1.ShipmentReceivablePM(null);
            myRecordPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            myRecordPM.ShipmentId = this.EntityPM.Id;
            myRecordPM.ShipmentNumber = this.EntityPM.ShipmentNumber;
            myRecordPM.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            myRecordPM.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            myRecordPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            myRecordPM.UpdateByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            myRecordPM.ShipmentReceivableLineStatusCode = "EMPT";
            myRecordPM.ShipmentReceivableLineStatusName = "Empty";
            myRecordPM.AWBPrint = OriginItemPM.AWBPrint;
            myRecordPM.ChargesTypeId = OriginItemPM.ChargesTypeId;
            myRecordPM.ChargesTypeName = OriginItemPM.ChargesTypeName;
            myRecordPM.ChargesTypeCode = OriginItemPM.ChargesTypeCode;
            myRecordPM.ChargesGroupCode = OriginItemPM.ChargesGroupCode;
            myRecordPM.CurrencyId = OriginItemPM.CurrencyId;
            myRecordPM.CurrencyCode = OriginItemPM.CurrencyCode;
            myRecordPM.DueTypeCode = OriginItemPM.DueTypeCode;
            myRecordPM.DueTypeName = OriginItemPM.DueTypeName;
            myRecordPM.IATACodeId = OriginItemPM.IATACodeId;
            myRecordPM.MeasurementId = OriginItemPM.MeasurementId;
            myRecordPM.MeasurementCode = OriginItemPM.MeasurementCode;
            myRecordPM.MeasurementShortName = OriginItemPM.MeasurementShortName;
            myRecordPM.Notes = OriginItemPM.Notes;
            myRecordPM.PrepaidCollectId = OriginItemPM.PrepaidCollectId;
            myRecordPM.UOMPercentage = OriginItemPM.UOMPercentage;
            myRecordPM.ViewOrder = OriginItemPM.ViewOrder;
            myRecordPM.IsExchangeRateFixed = OriginItemPM.IsExchangeRateFixed;
            myRecordPM.IsFixedPrice = OriginItemPM.IsFixedPrice;
            myRecordPM.UnitPrice = OriginItemPM.UnitPrice;
            myRecordPM.QuoteSaleMinAmount = OriginItemPM.QuoteSaleMinAmount;
            myRecordPM.QuoteSaleMaxAmount = OriginItemPM.QuoteSaleMaxAmount;
            myRecordPM.IsBackToBack = OriginItemPM.IsBackToBack;
            myRecordPM.ProfitCurrencyExchangeRate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);
            myRecordPM.IsExpense = OriginItemPM.IsExpense;
            myRecordPM.VatTypeId = OriginItemPM.VatTypeId;
            myRecordPM.IsFromQuote = OriginItemPM.IsFromQuote;
            myRecordPM.QuoteChargeId = OriginItemPM.QuoteChargeId;
            myRecordPM.IsChargeBySteps = OriginItemPM.IsChargeBySteps;
            myRecordPM.QuoteSaleMinAmount = OriginItemPM.QuoteSaleMinAmount;
            myRecordPM.QuoteSaleMaxAmount = OriginItemPM.QuoteSaleMaxAmount;
            //if (OriginItemPM.IsFromQuote) {
            //    myRecordPM.VatTypeId = OriginItemPM.VatTypeId;
            //}
            this.EntityPM.AddReceivable(myRecordPM);
        }
    };
    ShipmentGenerator.prototype.GetCurrencyRate = function (CurrencyId) {
        var myResult = null;
        if (CurrencyId == SessionLocator_1.SessionLocator.LocalCurrencyId) {
            myResult = 1;
        }
        else {
            var lastRate = this.AllRates.filter(function (f) { return f.ForeignCurrencyId == CurrencyId; })[0];
            if (lastRate != null) {
                myResult = lastRate.Rate;
            }
        }
        return myResult;
    };
    ShipmentGenerator.prototype.GetCurrencyCode = function (entity) {
        if (entity) {
            this.myCurrencyListService.getSingleFromCache(entity.CurrencyId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list != null) {
                        entity.CurrencyCode = list.Code;
                    }
                }
            });
        }
    };
    ShipmentGenerator.prototype.CreateNewReceivableFromLCLChargesType = function (item) {
        var newRecord = new ShipmentReceivablePM_1.ShipmentReceivablePM(this.EntityPM);
        newRecord.Tenant = SessionLocator_1.SessionLocator.Tenant;
        newRecord.ShipmentId = this.EntityPM.Id;
        newRecord.ShipmentNumber = this.EntityPM.ShipmentNumber;
        newRecord.ShipmentReceivableLineStatusCode = "EMPT";
        newRecord.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newRecord.UpdateByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newRecord.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        newRecord.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        newRecord.ChargesTypeId = item.Id;
        newRecord.ChargesTypeCode = item.Code;
        newRecord.ChargesTypeName = item.EnglishName;
        newRecord.MeasurementId = item.MeasurementId;
        newRecord.MeasurementCode = item.MeasurementCode;
        newRecord.MeasurementShortName = item.MeasurementShortName;
        newRecord.VatTypeId = item.VatTypeId;
        newRecord.ChargesGroupCode = item.ChargesGroupCode;
        newRecord.DueTypeCode = item.DueTypeCode;
        newRecord.DueTypeName = item.DueTypeName;
        newRecord.IATACodeId = item.IATACodeId;
        newRecord.ViewOrder = item.ViewOrder;
        newRecord.PrepaidCollectId = item.ChargesGroupCode == "FRT" ? this.EntityPM.FreightPrepaidCollectId : this.EntityPM.OtherPrepaidCollectId;
        newRecord.IsExpense = item.IsExpense;
        newRecord.IsBackToBack = item.IsBackToBack;
        switch (item.MeasurementCode) {
            case "GRWT": {
                newRecord.Quantity = this.EntityPM.GrossWeight;
                break;
            }
            case "CHWT": {
                newRecord.Quantity = this.EntityPM.ChargeableWeight;
                break;
            }
            case "VOLU": {
                newRecord.Quantity = this.EntityPM.Volume;
                break;
            }
            case "BTEU": {
                newRecord.Quantity = this.EntityPM.TEU;
                break;
            }
            case "FIXD": {
                newRecord.Quantity = 1;
                break;
            }
            case "PRVL": {
                newRecord.Quantity = this.EntityPM.ValueOfGoods;
                break;
            }
            case "GWTN": {
                newRecord.Quantity = this.EntityPM.GrossWeightPerTon;
            }
            case "QTY": {
                newRecord.Quantity = this.EntityPM.NumberOfPackages;
            }
            case "CWKG": {
                newRecord.Quantity = this.EntityPM.ChargeableWeightInKG;
                break;
            }
            case "GWKG": {
                newRecord.Quantity = this.EntityPM.GrossWeightInKG;
                break;
            }
            case "VCBM": {
                newRecord.Quantity = this.EntityPM.VolumeInCBM;
                break;
            }
            default: {
                break;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(item.ReceivablesDefaultCurrencyId)) {
            newRecord.CurrencyId = item.ReceivablesDefaultCurrencyId;
        }
        else {
            if (item.ChargesGroupCode == "FRT" || item.ChargesGroupCode == "SCH") {
                newRecord.CurrencyId = SessionLocator_1.SessionLocator.TenantPM.FreightCurrencyId;
            }
            else {
                newRecord.CurrencyId = SessionLocator_1.SessionLocator.TenantPM.OtherChargesCurrencyId;
            }
        }
        this.GetCurrencyCode(newRecord);
        newRecord.Rate = this.GetCurrencyRate(newRecord.CurrencyId);
        newRecord.ProfitCurrencyExchangeRate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);
        return newRecord;
    };
    return ShipmentGenerator;
}());
exports.ShipmentGenerator = ShipmentGenerator;
var AWBHelper = /** @class */ (function () {
    function AWBHelper() {
    }
    AWBHelper.ValidateAWBCCS = function (shipmentPM) {
        var myResult = new AWBCCSValidator();
        if (SessionLocator_1.SessionLocator.TenantManagementJS.AWBMessagesCCSTypeCode == "GLSHK") {
            myResult.FWB = shipmentPM.TenantZeroAirlineGLSHKFWB;
            myResult.FHL = shipmentPM.TenantZeroAirlineGLSHKFHL;
            myResult.FSU = shipmentPM.TenantZeroAirlineGLSHKFSU;
            myResult.FSRFSA = shipmentPM.TenantZeroAirlineGLSHKFSRFSA;
            myResult.FVRFVA = shipmentPM.TenantZeroAirlineGLSHKFVRFVA;
            if (Tools_1.AppTool.IsNullOrEmpty(shipmentPM.TenantZeroAirlinePIMA)) {
                myResult.IsValid = false;
                myResult.AirlineFieldHasError = true;
                myResult.AirlineFieldErrorMessage = "Airline communication parameter (PIMA) is missing";
            }
            if (Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantManagementJS.PIMA)) {
                myResult.IsValid = false;
                myResult.TenantManagementFieldHasError = true;
                myResult.TenantManagementFieldErrorMessage = "Tenant communication parameter (PIMA) is missing";
            }
            if (shipmentPM.TenantZeroAirlineGLSHKNeedsRegistration && !shipmentPM.CarrierIsGLSHKRegistered) {
                myResult.AirlineRegistrationHasError = true;
                myResult.AirlineRegistrationErrorMessage = "Can’t send this message, the airline needs GLSHK registration. Please contact your account manager";
            }
        }
        else {
            myResult.FWB = shipmentPM.TenantZeroAirlineChampFWB;
            myResult.FHL = shipmentPM.TenantZeroAirlineChampFHL;
            myResult.FSU = shipmentPM.TenantZeroAirlineChampFSU;
            myResult.FSRFSA = shipmentPM.TenantZeroAirlineChampFSRFSA;
            myResult.FVRFVA = shipmentPM.TenantZeroAirlineChampFVRFVA;
            if (Tools_1.AppTool.IsNullOrEmpty(shipmentPM.TenantZeroAirlineTTY)) {
                myResult.IsValid = false;
                myResult.AirlineFieldHasError = true;
                myResult.AirlineFieldErrorMessage = "This Airline doesn't support transmitting messages";
            }
            if (Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantManagementJS.TTY)) {
                myResult.IsValid = false;
                myResult.TenantManagementFieldHasError = true;
                myResult.TenantManagementFieldErrorMessage = "Tenant communication parameter (TTY) is missing";
            }
            if (shipmentPM.TenantZeroAirlineChampNeedsRegistration && !shipmentPM.CarrierIsChampRegistered) {
                myResult.AirlineRegistrationHasError = true;
                myResult.AirlineRegistrationErrorMessage = "Can’t send this message, the airline needs Champ registration. Please contact your account manager";
            }
        }
        return myResult;
    };
    AWBHelper.ValidateShipment = function (shipmentPM) {
        var errors = [];
        if (shipmentPM != null) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            var table = (shipmentPM.ShipmentLevelCode == "C") ? "Master" : "Shipment";
            Validator_1.Validator.TryValidateObject(shipmentPM, table, errors);
            if (shipmentPM.MainCarriageFinalDestinationPortId == null) {
                shipmentPM.MainCarriageFinalDestinationPortId = shipmentPM.MainCarriageToPortId;
            }
            //if (shipmentPM.ShipmentLevelCode != "C") {
            //    if (shipmentPM.ShipmentCustomerTypeCode != "SHI") {
            //        shipmentPM.ShipmentCustomerTypeCode = "SHI";
            //    }
            //    if (shipmentPM.CustomerId != shipmentPM.ShipperId) {
            //        shipmentPM.CustomerId = shipmentPM.ShipperId;
            //    }
            //    if (shipmentPM.CustomerName != shipmentPM.ShipperName) {
            //        shipmentPM.CustomerName = shipmentPM.ShipperName;
            //    }
            //    if (shipmentPM.CustomerAddressId != shipmentPM.ShipperAddressId) {
            //        shipmentPM.CustomerAddressId = shipmentPM.ShipperAddressId;
            //    }
            //    if (shipmentPM.CustomerContactId != shipmentPM.ShipperContactId) {
            //        shipmentPM.CustomerContactId = shipmentPM.ShipperContactId;
            //    }
            //}
            if (Tools_1.AppTool.IsNullOrEmpty(shipmentPM.ShipperId) && shipmentPM.DirectionId != "I") {
                errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate(table + ".F.ShipperId")));
            }
            if (Tools_1.AppTool.IsNullOrEmpty(shipmentPM.ConsigneeId) && shipmentPM.DirectionId == "I") {
                errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate(table + ".F.ConsigneeId")));
            }
            this.ValidateRoutings(shipmentPM, errors);
            if (shipmentPM.ShipmentAWBPrintOnlies != null) {
                shipmentPM.ShipmentAWBPrintOnlies.forEach(function (item) {
                    Validator_1.Validator.TryValidateObject(item, "ShipmentAWBPrintOnly", errors);
                    if (Tools_1.AppTool.IsNullOrEmpty(item.IATACodeId)) {
                        errors.push("IATA code field is required");
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(item.PrepaidCollectId)) {
                        errors.push("P/C field is required");
                    }
                    if (item.CurrencyId != shipmentPM.AWBCurrencyId) {
                        errors.push("Currency is not matching the shipment awb currency");
                    }
                });
            }
            if (shipmentPM.ShipmentPackages != null) {
                shipmentPM.ShipmentPackages.forEach(function (item) {
                    Validator_1.Validator.TryValidateObject(item, "ShipmentPackage", errors);
                });
            }
            if (shipmentPM.ShipmentCommodities != null) {
                shipmentPM.ShipmentCommodities.forEach(function (item) {
                    Validator_1.Validator.TryValidateObject(item, "ShipmentCommodity", errors);
                });
            }
            //if (!SessionLocator.TenantPM.AllowEAWBMoreThanTenPackages) {
            //    var myError: string = ShipmentTool.ValidateAddedPackagesCount(shipmentPM);
            //    if (!AppTool.IsNullOrEmpty(myError)) {
            //        errors.push(myError);
            //    }
            //}
            if (shipmentPM.AWBOCIPMs != null) {
                shipmentPM.AWBOCIPMs.forEach(function (item) {
                    Validator_1.Validator.TryValidateObject(item, "AWBOCI", errors);
                    if (Tools_1.AppTool.IsNullOrEmpty(item.CountryId) && Tools_1.AppTool.IsNullOrEmpty(item.AWBCustomsInformationCode) && Tools_1.AppTool.IsNullOrEmpty(item.AWBInformationCode)) {
                        errors.push("You must fill one of the fields (Country or Information or CustomsInformation)");
                    }
                });
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(shipmentPM.OtherParticipantInformationName1) && Tools_1.AppTool.IsNullOrEmpty(shipmentPM.OtherParticipantInformationReference1)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.OtherParticipantInformationReference1")));
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(shipmentPM.OtherParticipantInformationName2) && Tools_1.AppTool.IsNullOrEmpty(shipmentPM.OtherParticipantInformationReference2)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.OtherParticipantInformationReference2")));
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(shipmentPM.OtherParticipantInformationName3) && Tools_1.AppTool.IsNullOrEmpty(shipmentPM.OtherParticipantInformationReference3)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.OtherParticipantInformationReference3")));
            }
        }
        return errors;
    };
    AWBHelper.ValidateRoutings = function (shipmentPM, errors) {
        if (shipmentPM.ShipmentLevelCode == "H" && Tools_1.AppTool.IsNullOrEmpty(shipmentPM.MasterShipmentDataId)) {
            var ValidationText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            if (Tools_1.AppTool.IsNullOrEmpty(shipmentPM.FromPortId)) {
                if (shipmentPM.TransportModeId == "A") {
                    errors.push(ValidationText.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.Departure")));
                }
                else {
                    errors.push("From Port is required");
                }
            }
            if (Tools_1.AppTool.IsNullOrEmpty(shipmentPM.ToPortId)) {
                if (shipmentPM.TransportModeId == "A") {
                    errors.push(ValidationText.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.Destination")));
                }
                else {
                    errors.push("To Port is required");
                }
            }
        }
        else {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            var table = (shipmentPM.ShipmentLevelCode == "C") ? "Master" : "Shipment";
            //if (AppTool.IsNullOrEmpty(shipmentPM.MainCarriageFromPortId)) {
            //    errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate(table + ".F.MainCarriageFromPortId")));
            //}
            //if (AppTool.IsNullOrEmpty(shipmentPM.MainCarriageToPortId)) {
            //    errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate(table + ".F.MainCarriageToPortId")));
            //}
            if (!Tools_1.AppTool.IsNullOrEmpty(shipmentPM.Transshipment2FromPortId)) {
                if (Tools_1.AppTool.IsNullOrEmpty(shipmentPM.Transshipment1FromPortId)) {
                    errors.push("To Add Transshipment2 you need to add Transshipment1");
                }
            }
            RoutingHelper.ValidateRoutingsActualDates(shipmentPM, errors);
            RoutingHelper.ValidateRoutingsSeriesDates(shipmentPM, errors, "MainCarriage");
            // followups remove legs
        }
    };
    return AWBHelper;
}());
exports.AWBHelper = AWBHelper;
var AWBCCSValidator = /** @class */ (function () {
    function AWBCCSValidator() {
        this.IsValid = true;
        this.AirlineFieldHasError = false;
        this.AirlineRegistrationHasError = false;
        this.TenantManagementFieldHasError = false;
        this.AirlineFieldErrorMessage = null;
        this.AirlineRegistrationErrorMessage = null;
        this.TenantManagementFieldErrorMessage = null;
    }
    return AWBCCSValidator;
}());
exports.AWBCCSValidator = AWBCCSValidator;
var RoutingHelper = /** @class */ (function () {
    function RoutingHelper() {
    }
    RoutingHelper.MainCarriageFromPortChanged = function (entityPM, list) {
        if (entityPM != null) {
            var myPortId = null;
            var myPortCode = null;
            var myPortName = null;
            var myPortCountryId = null;
            var myPortCountryCode = null;
            var myPortCountryName = null;
            var myPortCountryEC = false;
            if (list != null) {
                myPortId = list.Id;
                myPortCode = list.Code;
                myPortName = list.EnglishName;
                myPortCountryId = list.CountryId;
                myPortCountryCode = list.CountryCode;
                myPortCountryName = list.CountryName;
                myPortCountryEC = list.CountryEC;
            }
            // this
            entityPM.FromCountryId = myPortCountryId;
            entityPM.FromCountryIsEC = myPortCountryEC;
            entityPM.MainCarriageFromPortId = myPortId;
            entityPM.MainCarriageFromPortCode = myPortCode;
            entityPM.MainCarriageFromPortName = myPortName;
            entityPM.MainCarriageFromPortCountryCode = myPortCountryCode;
            entityPM.MainCarriageFromPortCountryName = myPortCountryName;
            ShipmentTool.ComputeSCI(entityPM);
            ShipmentTool.BuildAWBPlaceField(entityPM);
            // Previous.To == this.From
            if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PreCarriageFromPortId)) {
                entityPM.PreCarriageToPortId = myPortId;
                entityPM.PreCarriageToPortCode = myPortCode;
                entityPM.PreCarriageToPortName = myPortName;
                entityPM.PreCarriageToPortCountryCode = myPortCountryCode;
                entityPM.PreCarriageToPortCountryName = myPortCountryName;
            }
        }
    };
    RoutingHelper.Transshipment1FromPortChanged = function (entityPM, list) {
        if (entityPM != null) {
            var myPortId = null;
            var myPortCode = null;
            var myPortName = null;
            var myPortCountryId = null;
            var myPortCountryCode = null;
            var myPortCountryName = null;
            if (list != null) {
                myPortId = list.Id;
                myPortCode = list.Code;
                myPortName = list.EnglishName;
                myPortCountryId = list.CountryId;
                myPortCountryCode = list.CountryCode;
                myPortCountryName = list.CountryName;
            }
            // this
            entityPM.Transshipment1FromPortId = myPortId;
            entityPM.Transshipment1FromPortCode = myPortCode;
            entityPM.Transshipment1FromPortName = myPortName;
            entityPM.Transshipment1FromPortCountryCode = myPortCountryCode;
            entityPM.Transshipment1FromPortCountryName = myPortCountryName;
            // Previous.To == this.From OR Next.From
            if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1FromPortId)) {
                entityPM.MainCarriageToPortId = entityPM.Transshipment1FromPortId;
                entityPM.MainCarriageToPortCode = entityPM.Transshipment1FromPortCode;
                entityPM.MainCarriageToPortName = entityPM.Transshipment1FromPortName;
                entityPM.MainCarriageToPortCountryCode = entityPM.Transshipment1FromPortCountryCode;
                entityPM.MainCarriageToPortCountryName = entityPM.Transshipment1FromPortCountryName;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2FromPortId)) {
                entityPM.MainCarriageToPortId = entityPM.Transshipment2FromPortId;
                entityPM.MainCarriageToPortCode = entityPM.Transshipment2FromPortCode;
                entityPM.MainCarriageToPortName = entityPM.Transshipment2FromPortName;
                entityPM.MainCarriageToPortCountryCode = entityPM.Transshipment2FromPortCountryCode;
                entityPM.MainCarriageToPortCountryName = entityPM.Transshipment2FromPortCountryName;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3FromPortId)) {
                entityPM.MainCarriageToPortId = entityPM.Transshipment3FromPortId;
                entityPM.MainCarriageToPortCode = entityPM.Transshipment3FromPortCode;
                entityPM.MainCarriageToPortName = entityPM.Transshipment3FromPortName;
                entityPM.MainCarriageToPortCountryCode = entityPM.Transshipment3FromPortCountryCode;
                entityPM.MainCarriageToPortCountryName = entityPM.Transshipment3FromPortCountryName;
            }
            else {
                entityPM.MainCarriageToPortId = entityPM.MainCarriageFinalDestinationPortId;
                entityPM.MainCarriageToPortCode = entityPM.MainCarriageFinalDestinationPortCode;
                entityPM.MainCarriageToPortName = entityPM.MainCarriageFinalDestinationPortName;
                entityPM.MainCarriageToPortCountryCode = entityPM.MainCarriageFinalDestinationPortCountryCode;
                entityPM.MainCarriageToPortCountryName = entityPM.MainCarriageFinalDestinationPortCountryName;
            }
            // this.To == thisDeleted OR next.From
            if (Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1FromPortId)) {
                entityPM.Transshipment1ToPortId = null;
                entityPM.Transshipment1ToPortCode = null;
                entityPM.Transshipment1ToPortName = null;
                entityPM.Transshipment1ToPortCountryCode = null;
                entityPM.Transshipment1ToPortCountryName = null;
            }
            else {
                if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2FromPortId)) {
                    entityPM.Transshipment1ToPortId = entityPM.Transshipment2FromPortId;
                    entityPM.Transshipment1ToPortCode = entityPM.Transshipment2FromPortCode;
                    entityPM.Transshipment1ToPortName = entityPM.Transshipment2FromPortName;
                    entityPM.Transshipment1ToPortCountryCode = entityPM.Transshipment2FromPortCountryCode;
                    entityPM.Transshipment1ToPortCountryName = entityPM.Transshipment2FromPortCountryName;
                }
                else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3FromPortId)) {
                    entityPM.Transshipment1ToPortId = entityPM.Transshipment3FromPortId;
                    entityPM.Transshipment1ToPortCode = entityPM.Transshipment3FromPortCode;
                    entityPM.Transshipment1ToPortName = entityPM.Transshipment3FromPortName;
                    entityPM.Transshipment1ToPortCountryCode = entityPM.Transshipment3FromPortCountryCode;
                    entityPM.Transshipment1ToPortCountryName = entityPM.Transshipment3FromPortCountryName;
                }
                else {
                    entityPM.Transshipment1ToPortId = entityPM.MainCarriageFinalDestinationPortId;
                    entityPM.Transshipment1ToPortCode = entityPM.MainCarriageFinalDestinationPortCode;
                    entityPM.Transshipment1ToPortName = entityPM.MainCarriageFinalDestinationPortName;
                    entityPM.Transshipment1ToPortCountryCode = entityPM.MainCarriageFinalDestinationPortCountryCode;
                    entityPM.Transshipment1ToPortCountryName = entityPM.MainCarriageFinalDestinationPortCountryName;
                }
            }
        }
    };
    RoutingHelper.Transshipment2FromPortChanged = function (entityPM, list) {
        if (entityPM != null) {
            var myPortId = null;
            var myPortCode = null;
            var myPortName = null;
            var myPortCountryId = null;
            var myPortCountryCode = null;
            var myPortCountryName = null;
            var myPortCountryEC = false;
            if (list != null) {
                myPortId = list.Id;
                myPortCode = list.Code;
                myPortName = list.EnglishName;
                myPortCountryId = list.CountryId;
                myPortCountryCode = list.CountryCode;
                myPortCountryName = list.CountryName;
                myPortCountryEC = list.CountryEC;
            }
            // this
            entityPM.Transshipment2FromPortId = myPortId;
            entityPM.Transshipment2FromPortCode = myPortCode;
            entityPM.Transshipment2FromPortName = myPortName;
            entityPM.Transshipment2FromPortCountryCode = myPortCountryCode;
            entityPM.Transshipment2FromPortCountryName = myPortCountryName;
            // this.To
            if (list) {
                if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3FromPortId)) {
                    entityPM.Transshipment2ToPortId = entityPM.Transshipment3FromPortId;
                    entityPM.Transshipment2ToPortCode = entityPM.Transshipment3FromPortCode;
                    entityPM.Transshipment2ToPortName = entityPM.Transshipment3FromPortName;
                    entityPM.Transshipment2ToPortCountryCode = entityPM.Transshipment3FromPortCountryCode;
                    entityPM.Transshipment2ToPortCountryName = entityPM.Transshipment3FromPortCountryName;
                }
                else {
                    entityPM.Transshipment2ToPortId = entityPM.MainCarriageFinalDestinationPortId;
                    entityPM.Transshipment2ToPortCode = entityPM.MainCarriageFinalDestinationPortCode;
                    entityPM.Transshipment2ToPortName = entityPM.MainCarriageFinalDestinationPortName;
                    entityPM.Transshipment2ToPortCountryCode = entityPM.MainCarriageFinalDestinationPortCountryCode;
                    entityPM.Transshipment2ToPortCountryName = entityPM.MainCarriageFinalDestinationPortCountryName;
                }
            }
            else {
                entityPM.Transshipment2ToPortId = null;
                entityPM.Transshipment2ToPortCode = null;
                entityPM.Transshipment2ToPortName = null;
                entityPM.Transshipment2ToPortCountryCode = null;
                entityPM.Transshipment2ToPortCountryName = null;
            }
            // Previous.From
            if (list) {
                if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1FromPortId)) {
                    entityPM.Transshipment1ToPortId = myPortId;
                    entityPM.Transshipment1ToPortCode = myPortCode;
                    entityPM.Transshipment1ToPortName = myPortName;
                    entityPM.Transshipment1ToPortCountryCode = myPortCountryCode;
                    entityPM.Transshipment1ToPortCountryName = myPortCountryName;
                }
                else {
                    entityPM.MainCarriageToPortId = myPortId;
                    entityPM.MainCarriageToPortCode = myPortCode;
                    entityPM.MainCarriageToPortName = myPortName;
                    entityPM.MainCarriageToPortCountryCode = myPortCountryCode;
                    entityPM.MainCarriageToPortCountryName = myPortCountryName;
                }
            }
            else {
                myPortId = entityPM.MainCarriageFinalDestinationPortId;
                myPortCode = entityPM.MainCarriageFinalDestinationPortCode;
                myPortName = entityPM.MainCarriageFinalDestinationPortName;
                myPortCountryCode = entityPM.MainCarriageFinalDestinationPortCountryCode;
                myPortCountryName = entityPM.MainCarriageFinalDestinationPortCountryName;
                myPortCountryEC = false;
                if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3FromPortId)) {
                    myPortId = entityPM.Transshipment3FromPortId;
                    myPortCode = entityPM.Transshipment3FromPortCode;
                    myPortName = entityPM.Transshipment3FromPortName;
                    myPortCountryCode = entityPM.Transshipment3FromPortCountryCode;
                    myPortCountryName = entityPM.Transshipment3FromPortCountryName;
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1FromPortId)) {
                    entityPM.Transshipment1ToPortId = myPortId;
                    entityPM.Transshipment1ToPortCode = myPortCode;
                    entityPM.Transshipment1ToPortName = myPortName;
                    entityPM.Transshipment1ToPortCountryCode = myPortCountryCode;
                    entityPM.Transshipment1ToPortCountryName = myPortCountryName;
                }
                else {
                    entityPM.MainCarriageToPortId = myPortId;
                    entityPM.MainCarriageToPortCode = myPortCode;
                    entityPM.MainCarriageToPortName = myPortName;
                    entityPM.MainCarriageToPortCountryCode = myPortCountryCode;
                    entityPM.MainCarriageToPortCountryName = myPortCountryName;
                }
            }
        }
    };
    RoutingHelper.Transshipment3FromPortChanged = function (entityPM, list) {
        if (entityPM != null) {
            var myPortId = null;
            var myPortCode = null;
            var myPortName = null;
            var myPortCountryId = null;
            var myPortCountryCode = null;
            var myPortCountryName = null;
            var myPortCountryEC = false;
            if (list != null) {
                myPortId = list.Id;
                myPortCode = list.Code;
                myPortName = list.EnglishName;
                myPortCountryId = list.CountryId;
                myPortCountryCode = list.CountryCode;
                myPortCountryName = list.CountryName;
                myPortCountryEC = list.CountryEC;
            }
            // this
            entityPM.Transshipment3FromPortId = myPortId;
            entityPM.Transshipment3FromPortCode = myPortCode;
            entityPM.Transshipment3FromPortName = myPortName;
            entityPM.Transshipment3FromPortCountryCode = myPortCountryCode;
            entityPM.Transshipment3FromPortCountryName = myPortCountryName;
            if (list) {
                // Previous
                if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2FromPortId)) {
                    entityPM.Transshipment2ToPortId = myPortId;
                    entityPM.Transshipment2ToPortCode = myPortCode;
                    entityPM.Transshipment2ToPortName = myPortName;
                    entityPM.Transshipment2ToPortCountryCode = myPortCountryCode;
                    entityPM.Transshipment2ToPortCountryName = myPortCountryName;
                }
                else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1FromPortId)) {
                    entityPM.Transshipment1ToPortId = myPortId;
                    entityPM.Transshipment1ToPortCode = myPortCode;
                    entityPM.Transshipment1ToPortName = myPortName;
                    entityPM.Transshipment1ToPortCountryCode = myPortCountryCode;
                    entityPM.Transshipment1ToPortCountryName = myPortCountryName;
                }
                else {
                    entityPM.MainCarriageToPortId = myPortId;
                    entityPM.MainCarriageToPortCode = myPortCode;
                    entityPM.MainCarriageToPortName = myPortName;
                    entityPM.MainCarriageToPortCountryCode = myPortCountryCode;
                    entityPM.MainCarriageToPortCountryName = myPortCountryName;
                }
                // Next
                entityPM.Transshipment3ToPortId = entityPM.MainCarriageFinalDestinationPortId;
                entityPM.Transshipment3ToPortCode = entityPM.MainCarriageFinalDestinationPortCode;
                entityPM.Transshipment3ToPortName = entityPM.MainCarriageFinalDestinationPortName;
                entityPM.Transshipment3ToPortCountryCode = entityPM.MainCarriageFinalDestinationPortCountryCode;
                entityPM.Transshipment3ToPortCountryName = entityPM.MainCarriageFinalDestinationPortCountryName;
            }
            else {
                entityPM.Transshipment3ToPortId = null;
                entityPM.Transshipment3ToPortCode = null;
                entityPM.Transshipment3ToPortName = null;
                entityPM.Transshipment3ToPortCountryCode = null;
                entityPM.Transshipment3ToPortCountryName = null;
                // Previous
                if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2FromPortId)) {
                    entityPM.Transshipment2ToPortId = entityPM.MainCarriageFinalDestinationPortId;
                    ;
                    entityPM.Transshipment2ToPortCode = entityPM.MainCarriageFinalDestinationPortCode;
                    entityPM.Transshipment2ToPortName = entityPM.MainCarriageFinalDestinationPortName;
                    entityPM.Transshipment2ToPortCountryCode = entityPM.MainCarriageFinalDestinationPortCountryCode;
                    entityPM.Transshipment2ToPortCountryName = entityPM.MainCarriageFinalDestinationPortCountryName;
                }
                else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1FromPortId)) {
                    entityPM.Transshipment1ToPortId = entityPM.MainCarriageFinalDestinationPortId;
                    ;
                    entityPM.Transshipment1ToPortCode = entityPM.MainCarriageFinalDestinationPortCode;
                    entityPM.Transshipment1ToPortName = entityPM.MainCarriageFinalDestinationPortName;
                    entityPM.Transshipment1ToPortCountryCode = entityPM.MainCarriageFinalDestinationPortCountryCode;
                    entityPM.Transshipment1ToPortCountryName = entityPM.MainCarriageFinalDestinationPortCountryName;
                }
                else {
                    entityPM.MainCarriageToPortId = entityPM.MainCarriageFinalDestinationPortId;
                    ;
                    entityPM.MainCarriageToPortCode = entityPM.MainCarriageFinalDestinationPortCode;
                    entityPM.MainCarriageToPortName = entityPM.MainCarriageFinalDestinationPortName;
                    entityPM.MainCarriageToPortCountryCode = entityPM.MainCarriageFinalDestinationPortCountryCode;
                    entityPM.MainCarriageToPortCountryName = entityPM.MainCarriageFinalDestinationPortCountryName;
                }
            }
        }
    };
    RoutingHelper.FinalDestinationPortChanged = function (entityPM, list) {
        if (entityPM != null) {
            var myPortId = null;
            var myPortCode = null;
            var myPortName = null;
            var myPortCountryId = null;
            var myPortCountryCode = null;
            var myPortCountryName = null;
            var myPortCountryEC = false;
            if (list != null) {
                myPortId = list.Id;
                myPortCode = list.Code;
                myPortName = list.EnglishName;
                myPortCountryId = list.CountryId;
                myPortCountryCode = list.CountryCode;
                myPortCountryName = list.CountryName;
                myPortCountryEC = list.CountryEC;
            }
            // this
            entityPM.ToCountryId = myPortCountryId;
            entityPM.ToCountryIsEC = myPortCountryEC;
            entityPM.FinalDistenationPortId = myPortId;
            entityPM.MainCarriageFinalDestinationPortId = myPortId;
            entityPM.MainCarriageFinalDestinationPortCode = myPortCode;
            entityPM.MainCarriageFinalDestinationPortName = myPortName;
            entityPM.MainCarriageFinalDestinationPortCountryCode = myPortCountryCode;
            entityPM.MainCarriageFinalDestinationPortCountryName = myPortCountryName;
            ShipmentTool.ComputeSCI(entityPM);
            // Previous.To == this.From
            if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3FromPortId)) {
                entityPM.Transshipment3ToPortId = myPortId;
                entityPM.Transshipment3ToPortCode = myPortCode;
                entityPM.Transshipment3ToPortName = myPortName;
                entityPM.Transshipment3ToPortCountryCode = myPortCountryCode;
                entityPM.Transshipment3ToPortCountryName = myPortCountryName;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2FromPortId)) {
                entityPM.Transshipment2ToPortId = myPortId;
                entityPM.Transshipment2ToPortCode = myPortCode;
                entityPM.Transshipment2ToPortName = myPortName;
                entityPM.Transshipment2ToPortCountryCode = myPortCountryCode;
                entityPM.Transshipment2ToPortCountryName = myPortCountryName;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1FromPortId)) {
                entityPM.Transshipment1ToPortId = myPortId;
                entityPM.Transshipment1ToPortCode = myPortCode;
                entityPM.Transshipment1ToPortName = myPortName;
                entityPM.Transshipment1ToPortCountryCode = myPortCountryCode;
                entityPM.Transshipment1ToPortCountryName = myPortCountryName;
            }
            else {
                entityPM.MainCarriageToPortId = myPortId;
                entityPM.MainCarriageToPortCode = myPortCode;
                entityPM.MainCarriageToPortName = myPortName;
                entityPM.MainCarriageToPortCountryCode = myPortCountryCode;
                entityPM.MainCarriageToPortCountryName = myPortCountryName;
            }
            // next.From = this.To
            if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OnCarriageToPortId)) {
                entityPM.OnCarriageFromPortId = myPortId;
                entityPM.OnCarriageFromPortCode = myPortCode;
                entityPM.OnCarriageFromPortName = myPortName;
                entityPM.OnCarriageFromPortCountryCode = myPortCountryCode;
                entityPM.OnCarriageFromPortCountryName = myPortCountryName;
            }
        }
    };
    RoutingHelper.PreCarriageToPortChanged = function (entityPM, list) {
        if (entityPM != null) {
            var myPortId = null;
            var myPortCode = null;
            var myPortName = null;
            var myPortCountryId = null;
            var myPortCountryCode = null;
            var myPortCountryName = null;
            var myPortCountryEC = false;
            if (list != null) {
                myPortId = list.Id;
                myPortCode = list.Code;
                myPortName = list.EnglishName;
                myPortCountryId = list.CountryId;
                myPortCountryCode = list.CountryCode;
                myPortCountryName = list.CountryName;
                myPortCountryEC = list.CountryEC;
            }
            // this
            entityPM.PreCarriageToPortId = myPortId;
            entityPM.PreCarriageToPortCode = myPortCode;
            entityPM.PreCarriageToPortName = myPortName;
            entityPM.PreCarriageToPortCountryCode = myPortCountryCode;
            entityPM.PreCarriageToPortCountryName = myPortCountryName;
            // next.From == this.To
            entityPM.FromCountryId = myPortCountryId;
            entityPM.FromCountryIsEC = myPortCountryEC;
            entityPM.MainCarriageFromPortId = myPortId;
            entityPM.MainCarriageFromPortCode = myPortCode;
            entityPM.MainCarriageFromPortName = myPortName;
            entityPM.MainCarriageFromPortCountryCode = myPortCountryCode;
            entityPM.MainCarriageFromPortCountryName = myPortCountryName;
            ShipmentTool.ComputeSCI(entityPM);
            ShipmentTool.BuildAWBPlaceField(entityPM);
        }
    };
    RoutingHelper.OnCarriageFromPortChanged = function (entityPM, list) {
        if (entityPM != null) {
            var myPortId = null;
            var myPortCode = null;
            var myPortName = null;
            var myPortCountryId = null;
            var myPortCountryCode = null;
            var myPortCountryName = null;
            if (list != null) {
                myPortId = list.Id;
                myPortCode = list.Code;
                myPortName = list.EnglishName;
                myPortCountryId = list.CountryId;
                myPortCountryCode = list.CountryCode;
                myPortCountryName = list.CountryName;
            }
            // this
            entityPM.OnCarriageFromPortId = myPortId;
            entityPM.OnCarriageFromPortCode = myPortCode;
            entityPM.OnCarriageFromPortName = myPortName;
            entityPM.OnCarriageFromPortCountryCode = myPortCountryCode;
            entityPM.OnCarriageFromPortCountryName = myPortCountryName;
            // Previous.To == this.From
            if (entityPM.Transshipment3FromPortId != null && entityPM.Transshipment3ToPortId != null) {
                entityPM.Transshipment3ToPortId = myPortId;
                entityPM.Transshipment3ToPortCode = myPortCode;
                entityPM.Transshipment3ToPortName = myPortName;
                entityPM.Transshipment3ToPortCountryCode = myPortCountryCode;
                entityPM.Transshipment3ToPortCountryName = myPortCountryName;
            }
            else if (entityPM.Transshipment2FromPortId != null && entityPM.Transshipment2ToPortId != null) {
                entityPM.Transshipment2ToPortId = myPortId;
                entityPM.Transshipment2ToPortCode = myPortCode;
                entityPM.Transshipment2ToPortName = myPortName;
                entityPM.Transshipment2ToPortCountryCode = myPortCountryCode;
                entityPM.Transshipment2ToPortCountryName = myPortCountryName;
            }
            else if (entityPM.Transshipment1FromPortId != null && entityPM.Transshipment1ToPortId != null) {
                entityPM.Transshipment1ToPortId = myPortId;
                entityPM.Transshipment1ToPortCode = myPortCode;
                entityPM.Transshipment1ToPortName = myPortName;
                entityPM.Transshipment1ToPortCountryCode = myPortCountryCode;
                entityPM.Transshipment1ToPortCountryName = myPortCountryName;
            }
            else {
                entityPM.MainCarriageToPortId = myPortId;
                entityPM.MainCarriageToPortCode = myPortCode;
                entityPM.MainCarriageToPortName = myPortName;
                entityPM.MainCarriageToPortCountryCode = myPortCountryCode;
                entityPM.MainCarriageToPortCountryName = myPortCountryName;
            }
            entityPM.ToCountryId = myPortCountryId;
            entityPM.FinalDistenationPortId = myPortId;
            entityPM.MainCarriageFinalDestinationPortId = myPortId;
            entityPM.MainCarriageFinalDestinationPortCode = myPortCode;
            entityPM.MainCarriageFinalDestinationPortName = myPortName;
            entityPM.MainCarriageFinalDestinationPortCountryCode = myPortCountryCode;
            entityPM.MainCarriageFinalDestinationPortCountryName = myPortCountryName;
        }
    };
    RoutingHelper.MainCarriageCarrierChanged = function (entityPM, list) {
        var myCardCode = null;
        var myCardName = null;
        var myCardPrefix = null;
        var myCardWebSite = null;
        if (list == null) {
            if (entityPM.MainCarriageCarrierNumber != null) {
                entityPM.MainCarriageCarrierNumber = null;
            }
            if (entityPM.TransportModeId == "A") {
                if (entityPM.Master) {
                    entityPM.Master = null;
                }
            }
        }
        else {
            myCardCode = list.Code;
            myCardName = list.EnglishName;
            myCardWebSite = list.WebSite;
            if (entityPM.TransportModeId == "A") {
                myCardPrefix = list.Code;
            }
        }
        if (entityPM.MainCarriageCarrierCode != myCardCode) {
            entityPM.MainCarriageCarrierCode = myCardCode;
        }
        if (entityPM.MainCarriageCarrierName != myCardName) {
            entityPM.MainCarriageCarrierName = myCardName;
        }
        if (entityPM.MainCarriageCarrierPrefix != myCardPrefix) {
            entityPM.MainCarriageCarrierPrefix = myCardPrefix;
        }
        if (entityPM.MainCarriageCarrierWebSite != myCardWebSite) {
            entityPM.MainCarriageCarrierWebSite = myCardWebSite;
        }
    };
    RoutingHelper.Transshipment1CarrierChanged = function (entityPM, list) {
        var myCardCode = null;
        var myCardName = null;
        var myCardPrefix = null;
        var myCardWebSite = null;
        if (list == null) {
            entityPM.Transshipment1CarrierNumber = null;
            entityPM.Transshipment1AdditionalMAWBOBLBL = null;
        }
        else {
            myCardCode = list.Code;
            myCardName = list.EnglishName;
            myCardWebSite = list.WebSite;
            if (entityPM.TransportModeId == "A") {
                myCardPrefix = list.Code;
            }
        }
        entityPM.Transshipment1CarrierCode = myCardCode;
        entityPM.Transshipment1CarrierName = myCardName;
        entityPM.Transshipment1CarrierPrefix = myCardPrefix;
        entityPM.Transshipment1CarrierWebSite = myCardWebSite;
    };
    RoutingHelper.Transshipment2CarrierChanged = function (entityPM, list) {
        var myCardCode = null;
        var myCardName = null;
        var myCardPrefix = null;
        var myCardWebSite = null;
        if (list == null) {
            entityPM.Transshipment2CarrierNumber = null;
            entityPM.Transshipment2AdditionalMAWBOBLBL = null;
        }
        else {
            myCardCode = list.Code;
            myCardName = list.EnglishName;
            myCardWebSite = list.WebSite;
            if (entityPM.TransportModeId == "A") {
                myCardPrefix = list.Code;
            }
        }
        entityPM.Transshipment2CarrierCode = myCardCode;
        entityPM.Transshipment2CarrierName = myCardName;
        entityPM.Transshipment2CarrierPrefix = myCardPrefix;
        entityPM.Transshipment2CarrierWebSite = myCardWebSite;
    };
    RoutingHelper.Transshipment3CarrierChanged = function (entityPM, list) {
        var myCardCode = null;
        var myCardName = null;
        var myCardPrefix = null;
        var myCardWebSite = null;
        if (list == null) {
            entityPM.Transshipment3CarrierNumber = null;
            entityPM.Transshipment3AdditionalMAWBOBLBL = null;
        }
        else {
            myCardCode = list.Code;
            myCardName = list.EnglishName;
            myCardWebSite = list.WebSite;
            if (entityPM.TransportModeId == "A") {
                myCardPrefix = list.Code;
            }
        }
        entityPM.Transshipment3CarrierCode = myCardCode;
        entityPM.Transshipment3CarrierName = myCardName;
        entityPM.Transshipment3CarrierPrefix = myCardPrefix;
        entityPM.Transshipment3CarrierWebSite = myCardWebSite;
    };
    RoutingHelper.RemovePreCarriageLeg = function (entityPM) {
        if (entityPM.HasPreCarriage == true) {
            entityPM.HasPreCarriage = false;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PreCarriageTransportModeId)) {
            entityPM.PreCarriageTransportModeId = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PreCarriageCarrierId)) {
            entityPM.PreCarriageCarrierId = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PreCarriageCarrierCode)) {
            entityPM.PreCarriageCarrierCode = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PreCarriageCarrierName)) {
            entityPM.PreCarriageCarrierName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PreCarriageCarrierWebSite)) {
            entityPM.PreCarriageCarrierWebSite = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PreCarriageCarrierNumber)) {
            entityPM.PreCarriageCarrierNumber = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PreCarriageVesselId)) {
            entityPM.PreCarriageVesselId = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PreCarriageVesselName)) {
            entityPM.PreCarriageVesselName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PreCarriageFromPortId)) {
            entityPM.PreCarriageFromPortId = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PreCarriageFromPortCode)) {
            entityPM.PreCarriageFromPortCode = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PreCarriageFromPortName)) {
            entityPM.PreCarriageFromPortName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PreCarriageFromPortCountryCode)) {
            entityPM.PreCarriageFromPortCountryCode = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PreCarriageFromPortCountryName)) {
            entityPM.PreCarriageFromPortCountryName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PreCarriageToPortId)) {
            entityPM.PreCarriageToPortId = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PreCarriageToPortCode)) {
            entityPM.PreCarriageToPortCode = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PreCarriageToPortName)) {
            entityPM.PreCarriageToPortName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PreCarriageToPortCountryCode)) {
            entityPM.PreCarriageToPortCountryCode = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PreCarriageToPortCountryName)) {
            entityPM.PreCarriageToPortCountryName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PreCarriageETD)) {
            entityPM.PreCarriageETD = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PreCarriageETA)) {
            entityPM.PreCarriageETA = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PreCarriageATD)) {
            entityPM.PreCarriageATD = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PreCarriageATA)) {
            entityPM.PreCarriageATA = null;
        }
        var followups = entityPM.FollowUps.filter(function (f) { return f.LegType != null; });
        followups = followups.filter(function (f) { return f.LegType.indexOf("PreCarriage") > -1; });
        if (followups.length > 0) {
            followups.forEach(function (item) {
                entityPM.RemoveShipmentFollowUp(item);
            });
            this.CurrentSession.FireEvent("FollowupsChanged");
        }
    };
    RoutingHelper.RemoveOnCarriageLeg = function (entityPM) {
        if (entityPM.HasOnCarriage == true) {
            entityPM.HasOnCarriage = false;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OnCarriageTransportModeId)) {
            entityPM.OnCarriageTransportModeId = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OnCarriageCarrierId)) {
            entityPM.OnCarriageCarrierId = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OnCarriageCarrierCode)) {
            entityPM.OnCarriageCarrierCode = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OnCarriageCarrierName)) {
            entityPM.OnCarriageCarrierName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OnCarriageCarrierWebSite)) {
            entityPM.OnCarriageCarrierWebSite = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OnCarriageCarrierNumber)) {
            entityPM.OnCarriageCarrierNumber = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OnCarriageVesselId)) {
            entityPM.OnCarriageVesselId = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OnCarriageVesselName)) {
            entityPM.OnCarriageVesselName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OnCarriageFromPortId)) {
            entityPM.OnCarriageFromPortId = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OnCarriageFromPortCode)) {
            entityPM.OnCarriageFromPortCode = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OnCarriageFromPortName)) {
            entityPM.OnCarriageFromPortName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OnCarriageFromPortCountryCode)) {
            entityPM.OnCarriageFromPortCountryCode = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OnCarriageFromPortCountryName)) {
            entityPM.OnCarriageFromPortCountryName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OnCarriageToPortId)) {
            entityPM.OnCarriageToPortId = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OnCarriageToPortCode)) {
            entityPM.OnCarriageToPortCode = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OnCarriageToPortName)) {
            entityPM.OnCarriageToPortName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OnCarriageToPortCountryCode)) {
            entityPM.OnCarriageToPortCountryCode = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OnCarriageToPortCountryName)) {
            entityPM.OnCarriageToPortCountryName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OnCarriageETD)) {
            entityPM.OnCarriageETD = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OnCarriageETA)) {
            entityPM.OnCarriageETA = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OnCarriageATD)) {
            entityPM.OnCarriageATD = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.OnCarriageATA)) {
            entityPM.OnCarriageATA = null;
        }
        var followups = entityPM.FollowUps.filter(function (f) { return f.LegType != null; });
        followups = followups.filter(function (f) { return f.LegType.indexOf("OnCarriage") > -1; });
        if (followups.length > 0) {
            followups.forEach(function (item) {
                entityPM.RemoveShipmentFollowUp(item);
            });
            this.CurrentSession.FireEvent("FollowupsChanged");
        }
    };
    RoutingHelper.RemoveTransshipment1Leg = function (entityPM) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1CarrierId)) {
            entityPM.Transshipment1CarrierId = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1CarrierCode)) {
            entityPM.Transshipment1CarrierCode = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1CarrierName)) {
            entityPM.Transshipment1CarrierName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1CarrierPrefix)) {
            entityPM.Transshipment1CarrierPrefix = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1CarrierWebSite)) {
            entityPM.Transshipment1CarrierWebSite = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1CarrierNumber)) {
            entityPM.Transshipment1CarrierNumber = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1FullCarrierNumber)) {
            entityPM.Transshipment1FullCarrierNumber = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1AdditionalMAWBOBLBL)) {
            entityPM.Transshipment1AdditionalMAWBOBLBL = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1VesselId)) {
            entityPM.Transshipment1VesselId = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1VesselName)) {
            entityPM.Transshipment1VesselName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1FromPortId)) {
            entityPM.Transshipment1FromPortId = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1FromPortCode)) {
            entityPM.Transshipment1FromPortCode = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1FromPortName)) {
            entityPM.Transshipment1FromPortName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1FromPortCountryCode)) {
            entityPM.Transshipment1FromPortCountryCode = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1FromPortCountryName)) {
            entityPM.Transshipment1FromPortCountryName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1ToPortId)) {
            entityPM.Transshipment1ToPortId = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1ToPortCode)) {
            entityPM.Transshipment1ToPortCode = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1ToPortName)) {
            entityPM.Transshipment1ToPortName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1ToPortCountryCode)) {
            entityPM.Transshipment1ToPortCountryCode = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1ToPortCountryName)) {
            entityPM.Transshipment1ToPortCountryName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1ETD)) {
            entityPM.Transshipment1ETD = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1ETA)) {
            entityPM.Transshipment1ETA = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1ATD)) {
            entityPM.Transshipment1ATD = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1ATA)) {
            entityPM.Transshipment1ATA = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1STD)) {
            entityPM.Transshipment1STD = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment1STA)) {
            entityPM.Transshipment1STA = null;
        }
        var followups = entityPM.FollowUps.filter(function (f) { return f.LegType != null; });
        followups = followups.filter(function (f) { return f.LegType.indexOf("Transshipment1") > -1; });
        if (followups.length > 0) {
            followups.forEach(function (item) {
                entityPM.RemoveShipmentFollowUp(item);
            });
            this.CurrentSession.FireEvent("FollowupsChanged");
        }
    };
    RoutingHelper.RemoveTransshipment2Leg = function (entityPM) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2CarrierId)) {
            entityPM.Transshipment2CarrierId = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2CarrierCode)) {
            entityPM.Transshipment2CarrierCode = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2CarrierName)) {
            entityPM.Transshipment2CarrierName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2CarrierPrefix)) {
            entityPM.Transshipment2CarrierPrefix = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2CarrierWebSite)) {
            entityPM.Transshipment2CarrierWebSite = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2CarrierNumber)) {
            entityPM.Transshipment2CarrierNumber = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2FullCarrierNumber)) {
            entityPM.Transshipment2FullCarrierNumber = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2AdditionalMAWBOBLBL)) {
            entityPM.Transshipment2AdditionalMAWBOBLBL = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2VesselId)) {
            entityPM.Transshipment2VesselId = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2VesselName)) {
            entityPM.Transshipment2VesselName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2FromPortId)) {
            entityPM.Transshipment2FromPortId = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2FromPortCode)) {
            entityPM.Transshipment2FromPortCode = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2FromPortName)) {
            entityPM.Transshipment2FromPortName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2FromPortCountryCode)) {
            entityPM.Transshipment2FromPortCountryCode = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2FromPortCountryName)) {
            entityPM.Transshipment2FromPortCountryName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2ToPortId)) {
            entityPM.Transshipment2ToPortId = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2ToPortCode)) {
            entityPM.Transshipment2ToPortCode = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2ToPortName)) {
            entityPM.Transshipment2ToPortName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2ToPortCountryCode)) {
            entityPM.Transshipment2ToPortCountryCode = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2ToPortCountryName)) {
            entityPM.Transshipment2ToPortCountryName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2ETD)) {
            entityPM.Transshipment2ETD = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2ETA)) {
            entityPM.Transshipment2ETA = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2ATD)) {
            entityPM.Transshipment2ATD = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2ATA)) {
            entityPM.Transshipment2ATA = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2STD)) {
            entityPM.Transshipment2STD = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment2STA)) {
            entityPM.Transshipment2STA = null;
        }
        var followups = entityPM.FollowUps.filter(function (f) { return f.LegType != null; });
        followups = followups.filter(function (f) { return f.LegType.indexOf("Transshipment2") > -1; });
        if (followups.length > 0) {
            followups.forEach(function (item) {
                entityPM.RemoveShipmentFollowUp(item);
            });
            this.CurrentSession.FireEvent("FollowupsChanged");
        }
    };
    RoutingHelper.RemoveTransshipment3Leg = function (entityPM) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3CarrierId)) {
            entityPM.Transshipment3CarrierId = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3CarrierCode)) {
            entityPM.Transshipment3CarrierCode = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3CarrierName)) {
            entityPM.Transshipment3CarrierName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3CarrierPrefix)) {
            entityPM.Transshipment3CarrierPrefix = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3CarrierWebSite)) {
            entityPM.Transshipment3CarrierWebSite = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3CarrierNumber)) {
            entityPM.Transshipment3CarrierNumber = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3FullCarrierNumber)) {
            entityPM.Transshipment3FullCarrierNumber = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3AdditionalMAWBOBLBL)) {
            entityPM.Transshipment3AdditionalMAWBOBLBL = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3VesselId)) {
            entityPM.Transshipment3VesselId = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3VesselName)) {
            entityPM.Transshipment3VesselName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3FromPortId)) {
            entityPM.Transshipment3FromPortId = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3FromPortCode)) {
            entityPM.Transshipment3FromPortCode = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3FromPortName)) {
            entityPM.Transshipment3FromPortName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3FromPortCountryCode)) {
            entityPM.Transshipment3FromPortCountryCode = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3FromPortCountryName)) {
            entityPM.Transshipment3FromPortCountryName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3ToPortId)) {
            entityPM.Transshipment3ToPortId = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3ToPortCode)) {
            entityPM.Transshipment3ToPortCode = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3ToPortName)) {
            entityPM.Transshipment3ToPortName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3ToPortCountryCode)) {
            entityPM.Transshipment3ToPortCountryCode = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3ToPortCountryName)) {
            entityPM.Transshipment3ToPortCountryName = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3ETD)) {
            entityPM.Transshipment3ETD = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3ETA)) {
            entityPM.Transshipment3ETA = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3ATD)) {
            entityPM.Transshipment3ATD = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3ATA)) {
            entityPM.Transshipment3ATA = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3STD)) {
            entityPM.Transshipment3STD = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Transshipment3STA)) {
            entityPM.Transshipment3STA = null;
        }
        var followups = entityPM.FollowUps.filter(function (f) { return f.LegType != null; });
        followups = followups.filter(function (f) { return f.LegType.indexOf("Transshipment3") > -1; });
        if (followups.length > 0) {
            followups.forEach(function (item) {
                entityPM.RemoveShipmentFollowUp(item);
            });
            this.CurrentSession.FireEvent("FollowupsChanged");
        }
    };
    RoutingHelper.ValidateRoutingsActualDates = function (entityPM, errors) {
        if (entityPM != null) {
            if (!entityPM.IsHybrid && !SessionLocator_1.SessionLocator.TenantPM.IsDocumentsArchive) {
                if (errors == null) {
                    errors = [];
                }
                var message = Tools_1.DateTool.ActualDateMessage;
                // Pickups
                entityPM.ShipmentPickUps.forEach(function (itemPickup) {
                    if (!Tools_1.DateTool.IsActualDateValid(itemPickup.ATD)) {
                        errors.push(message.replace("Field", "Pickup ATD"));
                    }
                    if (!Tools_1.DateTool.IsActualDateValid(itemPickup.ATA)) {
                        errors.push(message.replace("Field", "Pickup ATA"));
                    }
                });
                // PreCarriage
                if (!Tools_1.DateTool.IsActualDateValid(entityPM.PreCarriageATD)) {
                    errors.push(message.replace("Field", "Pre Carriage ATD"));
                }
                if (!Tools_1.DateTool.IsActualDateValid(entityPM.PreCarriageATA)) {
                    errors.push(message.replace("Field", "Pre Carriage ATA"));
                }
                // Main
                if (!Tools_1.DateTool.IsActualDateValid(entityPM.MainCarriageATD)) {
                    var textCode = entityPM.TransportModeId == "O" ? "Shipment.O.Routings.MainCarriage" : "Shipment.O.Routings.MainCarriageLeg1";
                    var field = TextCodeTranslator_1.TextCodeTranslator.Translate(textCode) + " ATD";
                    errors.push(message.replace("Field", field));
                }
                if (!Tools_1.DateTool.IsActualDateValid(entityPM.MainCarriageATA)) {
                    var textCode = entityPM.TransportModeId == "O" ? "Shipment.O.Routings.MainCarriage" : "Shipment.O.Routings.MainCarriageLeg1";
                    var field = TextCodeTranslator_1.TextCodeTranslator.Translate(textCode) + " ATA";
                    errors.push(message.replace("Field", field));
                }
                // TR1
                if (!Tools_1.DateTool.IsActualDateValid(entityPM.Transshipment1ATD)) {
                    var textCode = entityPM.TransportModeId == "O" ? "Shipment.O.Routings.Transshipment1" : "Shipment.O.Routings.MainCarriageLeg2";
                    var field = TextCodeTranslator_1.TextCodeTranslator.Translate(textCode) + " ATD";
                    errors.push(message.replace("Field", field));
                }
                if (!Tools_1.DateTool.IsActualDateValid(entityPM.Transshipment1ATA)) {
                    var textCode = entityPM.TransportModeId == "O" ? "Shipment.O.Routings.Transshipment1" : "Shipment.O.Routings.MainCarriageLeg2";
                    var field = TextCodeTranslator_1.TextCodeTranslator.Translate(textCode) + " ATA";
                    errors.push(message.replace("Field", field));
                }
                // TR2
                if (!Tools_1.DateTool.IsActualDateValid(entityPM.Transshipment2ATD)) {
                    var textCode = entityPM.TransportModeId == "O" ? "Shipment.O.Routings.Transshipment2" : "Shipment.O.Routings.MainCarriageLeg3";
                    var field = TextCodeTranslator_1.TextCodeTranslator.Translate(textCode) + " ATD";
                    errors.push(message.replace("Field", field));
                }
                if (!Tools_1.DateTool.IsActualDateValid(entityPM.Transshipment2ATA)) {
                    var textCode = entityPM.TransportModeId == "O" ? "Shipment.O.Routings.Transshipment2" : "Shipment.O.Routings.MainCarriageLeg3";
                    var field = TextCodeTranslator_1.TextCodeTranslator.Translate(textCode) + " ATA";
                    errors.push(message.replace("Field", field));
                }
                // TR3
                if (!Tools_1.DateTool.IsActualDateValid(entityPM.Transshipment3ATD)) {
                    var textCode = entityPM.TransportModeId == "O" ? "Shipment.O.Routings.Transshipment3" : "Shipment.O.Routings.MainCarriageLeg4";
                    var field = TextCodeTranslator_1.TextCodeTranslator.Translate(textCode) + " ATD";
                    errors.push(message.replace("Field", field));
                }
                if (!Tools_1.DateTool.IsActualDateValid(entityPM.Transshipment3ATA)) {
                    var textCode = entityPM.TransportModeId == "O" ? "Shipment.O.Routings.Transshipment3" : "Shipment.O.Routings.MainCarriageLeg4";
                    var field = TextCodeTranslator_1.TextCodeTranslator.Translate(textCode) + " ATA";
                    errors.push(message.replace("Field", field));
                }
                // OnCarriage
                if (!Tools_1.DateTool.IsActualDateValid(entityPM.OnCarriageATD)) {
                    errors.push(message.replace("Field", "On Carriage ATD"));
                }
                if (!Tools_1.DateTool.IsActualDateValid(entityPM.OnCarriageATA)) {
                    errors.push(message.replace("Field", "On Carriage ATA"));
                }
                // Warehouse
                if (!Tools_1.DateTool.IsActualDateValid(entityPM.WarehouseLegActualReleaseDate)) {
                    errors.push(message.replace("Field", "Warehouse Release Date"));
                }
                if (!Tools_1.DateTool.IsActualDateValid(entityPM.WarehouseLegActualEntryDate)) {
                    errors.push(message.replace("Field", "Warehouse Entry Date"));
                }
                // Deliveries
                entityPM.ShipmentDeliveries.forEach(function (itemDelivery) {
                    if (!Tools_1.DateTool.IsActualDateValid(itemDelivery.ATD)) {
                        errors.push(message.replace("Field", "Delivery ATD"));
                    }
                    if (!Tools_1.DateTool.IsActualDateValid(itemDelivery.ATA)) {
                        errors.push(message.replace("Field", "Delivery ATA"));
                    }
                });
            }
        }
    };
    RoutingHelper.ValidateRoutingsSeriesDates = function (entityPM, errors, legCode) {
        var _this = this;
        if (legCode === void 0) { legCode = null; }
        if (entityPM != null) {
            if (errors == null) {
                errors = [];
            }
            var PreCarriageErrors = [];
            var isPreCarriageExists = (entityPM.PreCarriageFromPortId != null && entityPM.PreCarriageToPortId != null) ? true : false;
            var PreCarriageETD = isPreCarriageExists ? Tools_1.DateTool.GetDateParts(entityPM.PreCarriageETD).DateTicks : 0;
            var PreCarriageETA = isPreCarriageExists ? Tools_1.DateTool.GetDateParts(entityPM.PreCarriageETA).DateTicks : 0;
            var PreCarriageATD = isPreCarriageExists ? Tools_1.DateTool.GetDateParts(entityPM.PreCarriageATD).DateTicks : 0;
            var PreCarriageATA = isPreCarriageExists ? Tools_1.DateTool.GetDateParts(entityPM.PreCarriageATA).DateTicks : 0;
            var MainCarriageErrors = [];
            var isMainCarriageExists = true;
            var MainCarriageETD = Tools_1.DateTool.GetDateParts(entityPM.MainCarriageETD).DateTicks;
            var MainCarriageETA = Tools_1.DateTool.GetDateParts(entityPM.MainCarriageETA).DateTicks;
            var MainCarriageATD = Tools_1.DateTool.GetDateParts(entityPM.MainCarriageATD).DateTicks;
            var MainCarriageATA = Tools_1.DateTool.GetDateParts(entityPM.MainCarriageATA).DateTicks;
            var isTransshipment1Exists = (entityPM.Transshipment1FromPortId != null && entityPM.Transshipment1ToPortId != null) ? true : false;
            var Transshipment1ETD = isTransshipment1Exists ? Tools_1.DateTool.GetDateParts(entityPM.Transshipment1ETD).DateTicks : 0;
            var Transshipment1ETA = isTransshipment1Exists ? Tools_1.DateTool.GetDateParts(entityPM.Transshipment1ETA).DateTicks : 0;
            var Transshipment1ATD = isTransshipment1Exists ? Tools_1.DateTool.GetDateParts(entityPM.Transshipment1ATD).DateTicks : 0;
            var Transshipment1ATA = isTransshipment1Exists ? Tools_1.DateTool.GetDateParts(entityPM.Transshipment1ATA).DateTicks : 0;
            var isTransshipment2Exists = (entityPM.Transshipment2FromPortId != null && entityPM.Transshipment2ToPortId != null) ? true : false;
            var Transshipment2ETD = isTransshipment2Exists ? Tools_1.DateTool.GetDateParts(entityPM.Transshipment2ETD).DateTicks : 0;
            var Transshipment2ETA = isTransshipment2Exists ? Tools_1.DateTool.GetDateParts(entityPM.Transshipment2ETA).DateTicks : 0;
            var Transshipment2ATD = isTransshipment2Exists ? Tools_1.DateTool.GetDateParts(entityPM.Transshipment2ATD).DateTicks : 0;
            var Transshipment2ATA = isTransshipment2Exists ? Tools_1.DateTool.GetDateParts(entityPM.Transshipment2ATA).DateTicks : 0;
            var isTransshipment3Exists = (entityPM.Transshipment3FromPortId != null && entityPM.Transshipment3ToPortId != null) ? true : false;
            var Transshipment3ETD = isTransshipment3Exists ? Tools_1.DateTool.GetDateParts(entityPM.Transshipment3ETD).DateTicks : 0;
            var Transshipment3ETA = isTransshipment3Exists ? Tools_1.DateTool.GetDateParts(entityPM.Transshipment3ETA).DateTicks : 0;
            var Transshipment3ATD = isTransshipment3Exists ? Tools_1.DateTool.GetDateParts(entityPM.Transshipment3ATD).DateTicks : 0;
            var Transshipment3ATA = isTransshipment3Exists ? Tools_1.DateTool.GetDateParts(entityPM.Transshipment3ATA).DateTicks : 0;
            var OnCarriageErrors = [];
            var isOnCarriageExists = (entityPM.OnCarriageFromPortId != null && entityPM.OnCarriageToPortId != null) ? true : false;
            var OnCarriageETD = isOnCarriageExists ? Tools_1.DateTool.GetDateParts(entityPM.OnCarriageETD).DateTicks : 0;
            var OnCarriageETA = isOnCarriageExists ? Tools_1.DateTool.GetDateParts(entityPM.OnCarriageETA).DateTicks : 0;
            var OnCarriageATD = isOnCarriageExists ? Tools_1.DateTool.GetDateParts(entityPM.OnCarriageATD).DateTicks : 0;
            var OnCarriageATA = isOnCarriageExists ? Tools_1.DateTool.GetDateParts(entityPM.OnCarriageATA).DateTicks : 0;
            var WarehouseLegErrors = [];
            var isWarehouseLegExists = (entityPM.WarehouseLegWarehouseId != null) ? true : false;
            var isWarehousePickupsLegExists = isWarehouseLegExists == true && entityPM.DirectionId != "I" ? true : false;
            var isWarehouseDeliveriesLegExists = isWarehouseLegExists == true && entityPM.DirectionId == "I" ? true : false;
            var WarehouseLegEED = isWarehouseLegExists ? Tools_1.DateTool.GetDateParts(entityPM.WarehouseLegExpectedEntryDate).DateTicks : 0;
            var WarehouseLegERD = isWarehouseLegExists ? Tools_1.DateTool.GetDateParts(entityPM.WarehouseLegExpectedReleaseDate).DateTicks : 0;
            var WarehouseLegAED = isWarehouseLegExists ? Tools_1.DateTool.GetDateParts(entityPM.WarehouseLegActualEntryDate).DateTicks : 0;
            var WarehouseLegARD = isWarehouseLegExists ? Tools_1.DateTool.GetDateParts(entityPM.WarehouseLegActualReleaseDate).DateTicks : 0;
            // Pickups
            var allPickupsETA = 0;
            var allPickupsATA = 0;
            var allPickupsErrors = [];
            var isPickupsExists = entityPM.ShipmentPickUps.length > 0 ? true : false;
            if (isPickupsExists) {
                entityPM.ShipmentPickUps.forEach(function (itemPickup) {
                    var ETD = Tools_1.DateTool.GetDateParts(itemPickup.ETD).DateTicks;
                    var ETA = Tools_1.DateTool.GetDateParts(itemPickup.ETA).DateTicks;
                    var ATD = Tools_1.DateTool.GetDateParts(itemPickup.ATD).DateTicks;
                    var ATA = Tools_1.DateTool.GetDateParts(itemPickup.ATA).DateTicks;
                    if (ETA > allPickupsETA) {
                        allPickupsETA = ETA;
                    }
                    if (ATA > allPickupsATA) {
                        allPickupsATA = ATA;
                    }
                    // Self
                    if (!_this.IsRoutingLegDatesValid(ETD, ETA)) {
                        allPickupsErrors.push("Pick up expected departure must be less than pick up expected arrival");
                    }
                    if (!_this.IsRoutingLegDatesValid(ATD, ATA)) {
                        allPickupsErrors.push("Pick up actual departure must be less than pick up actual arrival");
                    }
                    //if (this.CompairDateSeries(ETD, ETA, ">")) {
                    //    allPickupsErrors.push("Pick up expected departure must be less than pick up expected arrival");
                    //}
                    //if (this.CompairDateSeries(ATD, ATA, ">")) {
                    //    allPickupsErrors.push("Pick up actual departure must be less than pick up actual arrival");
                    //}
                    // Next
                    if (isWarehousePickupsLegExists) {
                        if (_this.IsDateSeriesBiggerNotEqual(ETA, WarehouseLegEED)) {
                            allPickupsErrors.push("Pick up expected arrival must be equal or less than Warehouse expected entry");
                        }
                        if (_this.IsDateSeriesBiggerNotEqual(ATA, WarehouseLegAED)) {
                            allPickupsErrors.push("Pick up actual arrival must be equal or less than Warehouse actual entry");
                        }
                    }
                    else if (isPreCarriageExists) {
                        if (_this.IsDateSeriesBigger(ETA, PreCarriageETD)) {
                            allPickupsErrors.push("Pick up expected arrival must be less than pre carriage expected departure");
                        }
                        if (_this.IsDateSeriesBigger(ATA, PreCarriageATD)) {
                            allPickupsErrors.push("Pick up actual arrival must be less than pre carriage actual departure");
                        }
                    }
                    else if (isMainCarriageExists) {
                        if (_this.IsDateSeriesBigger(ETA, MainCarriageETD)) {
                            allPickupsErrors.push("Pick up expected arrival must be less than main carriage expected departure");
                        }
                        if (_this.IsDateSeriesBigger(ATA, MainCarriageATD)) {
                            allPickupsErrors.push("Pick up actual arrival must be less than main carriage actual departure");
                        }
                    }
                });
            }
            // Deliveries
            var allDeliveriesETD = 0;
            var allDeliveriesATD = 0;
            var allDeliveriesErrors = [];
            var isDeliveriesExists = entityPM.ShipmentDeliveries.length > 0 ? true : false;
            if (isDeliveriesExists) {
                entityPM.ShipmentDeliveries.forEach(function (itemDelivery) {
                    var ETD = Tools_1.DateTool.GetDateParts(itemDelivery.ETD).DateTicks;
                    var ETA = Tools_1.DateTool.GetDateParts(itemDelivery.ETA).DateTicks;
                    var ATD = Tools_1.DateTool.GetDateParts(itemDelivery.ATD).DateTicks;
                    var ATA = Tools_1.DateTool.GetDateParts(itemDelivery.ATA).DateTicks;
                    if (ETD > 0) {
                        if (allDeliveriesETD == 0) {
                            allDeliveriesETD = ETD;
                        }
                        else if (allDeliveriesETD > ETD) {
                            allDeliveriesETD = ETD;
                        }
                    }
                    if (ATD > 0) {
                        if (allDeliveriesATD == 0) {
                            allDeliveriesATD = ATD;
                        }
                        else if (allDeliveriesATD > ATD) {
                            allDeliveriesATD = ATD;
                        }
                    }
                    // Self
                    if (!_this.IsRoutingLegDatesValid(ETD, ETA)) {
                        allDeliveriesErrors.push("Delivery expected departure must be less than Delivery expected arrival");
                    }
                    if (!_this.IsRoutingLegDatesValid(ATD, ATA)) {
                        allDeliveriesErrors.push("Delivery actual departure must be less than Delivery actual arrival");
                    }
                    //if (this.CompairDateSeries(ETD, ETA, ">")) {
                    //    allDeliveriesErrors.push("Delivery expected departure must be less than Delivery expected arrival");
                    //}
                    //if (this.CompairDateSeries(ATD, ATA, ">")) {
                    //    allDeliveriesErrors.push("Delivery actual departure must be less than Delivery actual arrival");
                    //}
                    // Previous
                    if (isWarehouseDeliveriesLegExists) {
                        if (_this.IsDateSeriesSmaller(ETD, WarehouseLegERD)) {
                            allDeliveriesErrors.push("Delivery expected departure must be bigger than Warehouse expected release");
                        }
                        if (_this.IsDateSeriesSmaller(ATD, WarehouseLegARD)) {
                            allDeliveriesErrors.push("Delivery actual departure must be bigger than Warehouse actual release");
                        }
                    }
                    else if (isOnCarriageExists) {
                        if (_this.IsDateSeriesSmaller(ETD, OnCarriageETA)) {
                            allDeliveriesErrors.push("Delivery expected departure must be bigger than On-Carriage expected arrival");
                        }
                        if (_this.IsDateSeriesSmaller(ATD, OnCarriageATA)) {
                            allDeliveriesErrors.push("Delivery actual departure must be bigger than On-Carriage actual arrival");
                        }
                    }
                    else if (isTransshipment3Exists) {
                        if (_this.IsDateSeriesSmaller(ETD, Transshipment3ETA)) {
                            allDeliveriesErrors.push("Delivery expected departure must be bigger than Transshipment3 expected arrival");
                        }
                        if (_this.IsDateSeriesSmaller(ATD, Transshipment3ATA)) {
                            allDeliveriesErrors.push("Delivery actual departure must be bigger than Transshipment3 actual arrival");
                        }
                    }
                    else if (isTransshipment2Exists) {
                        if (_this.IsDateSeriesSmaller(ETD, Transshipment2ETA)) {
                            allDeliveriesErrors.push("Delivery expected departure must be bigger than Transshipment2 expected arrival");
                        }
                        if (_this.IsDateSeriesSmaller(ATD, Transshipment2ATA)) {
                            allDeliveriesErrors.push("Delivery actual departure must be bigger than Transshipment2 actual arrival");
                        }
                    }
                    else if (isTransshipment1Exists) {
                        if (_this.IsDateSeriesSmaller(ETD, Transshipment1ETA)) {
                            allDeliveriesErrors.push("Delivery expected departure must be bigger than Transshipment1 expected arrival");
                        }
                        if (_this.IsDateSeriesSmaller(ATD, Transshipment1ATA)) {
                            allDeliveriesErrors.push("Delivery actual departure must be bigger than Transshipment1 actual arrival");
                        }
                    }
                    else {
                        if (_this.IsDateSeriesSmaller(ETD, MainCarriageETA)) {
                            allDeliveriesErrors.push("Delivery expected departure must be bigger than Main-Carriage expected arrival");
                        }
                        if (_this.IsDateSeriesSmaller(ATD, MainCarriageATA)) {
                            allDeliveriesErrors.push("Delivery actual departure must be bigger than Main-Carriage actual arrival");
                        }
                    }
                });
            }
            // Pre Carriage
            if (isPreCarriageExists) {
                // Self
                if (!this.IsRoutingLegDatesValid(PreCarriageETD, PreCarriageETA)) {
                    PreCarriageErrors.push("Pre-Carriage expected departure must be less than Pre-Carriage expected arrival");
                }
                if (!this.IsRoutingLegDatesValid(PreCarriageATD, PreCarriageATA)) {
                    PreCarriageErrors.push("Pre-Carriage actual departure must be less than Pre-Carriage actual arrival");
                }
                //if (this.CompairDateSeries(PreCarriageETD, PreCarriageETA, ">")) {
                //    PreCarriageErrors.push("Pre-Carriage expected departure must be less than Pre-Carriage expected arrival");
                //}
                //if (this.CompairDateSeries(PreCarriageATD, PreCarriageATA, ">")) {
                //    PreCarriageErrors.push("Pre-Carriage actual departure must be less than Pre-Carriage actual arrival");
                //}
                // Next
                if (isMainCarriageExists) {
                    if (this.IsDateSeriesBigger(PreCarriageETA, MainCarriageETD)) {
                        PreCarriageErrors.push("Pre-Carriage expected arrival must be less than Main-Carriage expected departure");
                    }
                    if (this.IsDateSeriesBigger(PreCarriageATA, MainCarriageATD)) {
                        PreCarriageErrors.push("Pre-Carriage actual arrival must be less than Main-Carriage actual departure");
                    }
                }
                // Previous
                if (isWarehousePickupsLegExists) {
                    if (this.IsDateSeriesSmaller(PreCarriageETD, WarehouseLegERD)) {
                        PreCarriageErrors.push("Pre-Carriage expected departure must be bigger than Warehouse expected release");
                    }
                    if (this.IsDateSeriesSmaller(PreCarriageATD, WarehouseLegARD)) {
                        PreCarriageErrors.push("Pre-Carriage actual departure must be bigger than Warehouse actual release");
                    }
                }
                else if (isPickupsExists) {
                    if (this.IsDateSeriesSmaller(PreCarriageETD, allPickupsETA)) {
                        PreCarriageErrors.push("Pre-Carriage expected departure must be bigger than all pick ups expected arrival");
                    }
                    if (this.IsDateSeriesSmaller(PreCarriageATD, allPickupsATA)) {
                        PreCarriageErrors.push("Pre-Carriage actual departure must be bigger than all pick ups actual arrival");
                    }
                }
            }
            // On Carriage
            if (isOnCarriageExists) {
                // Self
                if (!this.IsRoutingLegDatesValid(OnCarriageETD, OnCarriageETA)) {
                    OnCarriageErrors.push("On-Carriage expected departure must be less than On-Carriage expected arrival");
                }
                if (!this.IsRoutingLegDatesValid(OnCarriageATD, OnCarriageATA)) {
                    OnCarriageErrors.push("On-Carriage actual departure must be less than On-Carriage actual arrival");
                }
                //if (this.CompairDateSeries(OnCarriageETD, OnCarriageETA, ">")) {
                //    OnCarriageErrors.push("On-Carriage expected departure must be less than On-Carriage expected arrival");
                //}
                //if (this.CompairDateSeries(OnCarriageATD, OnCarriageATA, ">")) {
                //    OnCarriageErrors.push("On-Carriage actual departure must be less than On-Carriage actual arrival");
                //}
                // Previous
                if (isTransshipment3Exists) {
                    if (this.IsDateSeriesSmaller(OnCarriageETD, Transshipment3ETA)) {
                        OnCarriageErrors.push("On-Carriage expected departure must be bigger than Transshipment3 expected arrival");
                    }
                    if (this.IsDateSeriesSmaller(OnCarriageATD, Transshipment3ATA)) {
                        OnCarriageErrors.push("On-Carriage actual departure must be bigger than Transshipment3 actual arrival");
                    }
                }
                else if (isTransshipment2Exists) {
                    if (this.IsDateSeriesSmaller(OnCarriageETD, Transshipment2ETA)) {
                        OnCarriageErrors.push("On-Carriage expected departure must be bigger than Transshipment2 expected arrival");
                    }
                    if (this.IsDateSeriesSmaller(OnCarriageATD, Transshipment2ATA)) {
                        OnCarriageErrors.push("On-Carriage actual departure must be bigger than Transshipment2 actual arrival");
                    }
                }
                else if (isTransshipment1Exists) {
                    if (this.IsDateSeriesSmaller(OnCarriageETD, Transshipment1ETA)) {
                        OnCarriageErrors.push("On-Carriage expected departure must be bigger than Transshipment1 expected arrival");
                    }
                    if (this.IsDateSeriesSmaller(OnCarriageATD, Transshipment1ATA)) {
                        OnCarriageErrors.push("On-Carriage actual departure must be bigger than Transshipment1 actual arrival");
                    }
                }
                else {
                    if (this.IsDateSeriesSmaller(OnCarriageETD, MainCarriageETA)) {
                        OnCarriageErrors.push("On-Carriage expected departure must be bigger than Main-Carriage expected arrival");
                    }
                    if (this.IsDateSeriesSmaller(OnCarriageATD, MainCarriageATA)) {
                        OnCarriageErrors.push("On-Carriage actual departure must be bigger than Main-Carriage actual arrival");
                    }
                }
                // Next
                if (isWarehouseDeliveriesLegExists) {
                    if (this.IsDateSeriesBigger(OnCarriageETA, WarehouseLegEED)) {
                        OnCarriageErrors.push("On-Carriage expected arrival must be less than Warehouse expected entry");
                    }
                    if (this.IsDateSeriesBigger(OnCarriageATA, WarehouseLegAED)) {
                        OnCarriageErrors.push("On-Carriage actual arrival must be less than Warehouse actual entry");
                    }
                }
                else if (isDeliveriesExists) {
                    if (this.IsDateSeriesBigger(OnCarriageETA, allDeliveriesETD)) {
                        OnCarriageErrors.push("On-Carriage expected arrival must be less than all deliveries expected departure");
                    }
                    if (this.IsDateSeriesBigger(OnCarriageATA, allDeliveriesATD)) {
                        OnCarriageErrors.push("On-Carriage actual arrival must be less than all deliveries actual departure");
                    }
                }
            }
            // Main Carriage
            if (isMainCarriageExists) {
                // Self
                if (!this.IsRoutingLegDatesValid(MainCarriageETD, MainCarriageETA)) {
                    MainCarriageErrors.push("Main-Carriage expected departure must be less than Main-Carriage expected arrival");
                }
                if (!this.IsRoutingLegDatesValid(MainCarriageATD, MainCarriageATA)) {
                    MainCarriageErrors.push("Main-Carriage actual departure must be less than Main-Carriage actual arrival");
                }
                //if (this.CompairDateSeries(MainCarriageETD, MainCarriageETA, ">")) {
                //    MainCarriageErrors.push("Main-Carriage expected departure must be less than Main-Carriage expected arrival");
                //}
                //if (this.CompairDateSeries(MainCarriageATD, MainCarriageATA, ">")) {
                //    MainCarriageErrors.push("Main-Carriage actual departure must be less than Main-Carriage actual arrival");
                //}
                // Previous
                if (isPreCarriageExists) {
                    if (this.IsDateSeriesSmaller(MainCarriageETD, PreCarriageETA)) {
                        MainCarriageErrors.push("Main-Carriage expected departure must be bigger than Pre-Carriage expected arrival");
                    }
                    if (this.IsDateSeriesSmaller(MainCarriageATD, PreCarriageATA)) {
                        MainCarriageErrors.push("Main-Carriage actual departure must be bigger than Pre-Carriage actual arrival");
                    }
                }
                else if (isWarehousePickupsLegExists) {
                    if (this.IsDateSeriesSmaller(MainCarriageETD, WarehouseLegERD)) {
                        MainCarriageErrors.push("Main-Carriage expected departure must be bigger than Warehouse expected release");
                    }
                    if (this.IsDateSeriesSmaller(MainCarriageATD, WarehouseLegARD)) {
                        MainCarriageErrors.push("Main-Carriage actual departure must be bigger than Warehouse actual release");
                    }
                }
                else if (isPickupsExists) {
                    if (this.IsDateSeriesSmaller(MainCarriageETD, allPickupsETA)) {
                        MainCarriageErrors.push("Main-Carriage expected departure must be bigger than all pick ups expected arrival");
                    }
                    if (this.IsDateSeriesSmaller(MainCarriageATD, allPickupsATA)) {
                        MainCarriageErrors.push("Main-Carriage actual departure must be bigger than all pick ups actual arrival");
                    }
                }
                // Next
                if (isTransshipment1Exists) {
                    if (this.IsDateSeriesBigger(MainCarriageETA, Transshipment1ETD)) {
                        MainCarriageErrors.push("Main-Carriage expected arrival must be less than Via1 expected departure");
                    }
                    if (this.IsDateSeriesBigger(MainCarriageATA, Transshipment1ATD)) {
                        MainCarriageErrors.push("Main-Carriage actual arrival must be less than Via1 actual departure");
                    }
                }
                else if (isTransshipment2Exists) {
                    if (this.IsDateSeriesBigger(MainCarriageETA, Transshipment2ETD)) {
                        MainCarriageErrors.push("Main-Carriage expected arrival must be less than Via2 expected departure");
                    }
                    if (this.IsDateSeriesBigger(MainCarriageATA, Transshipment2ATD)) {
                        MainCarriageErrors.push("Main-Carriage actual arrival must be less than Via2 actual departure");
                    }
                }
                else if (isTransshipment3Exists) {
                    if (this.IsDateSeriesBigger(MainCarriageETA, Transshipment3ETD)) {
                        MainCarriageErrors.push("Main-Carriage expected arrival must be less than Via3 expected departure");
                    }
                    if (this.IsDateSeriesBigger(MainCarriageATA, Transshipment3ATD)) {
                        MainCarriageErrors.push("Main-Carriage actual arrival must be less than Via3 actual departure");
                    }
                }
                else if (isOnCarriageExists) {
                    if (this.IsDateSeriesBigger(MainCarriageETA, OnCarriageETD)) {
                        MainCarriageErrors.push("Main-Carriage expected arrival must be less than On-Carriage expected departure");
                    }
                    if (this.IsDateSeriesBigger(MainCarriageATA, OnCarriageATD)) {
                        MainCarriageErrors.push("Main-Carriage actual arrival must be less than On-Carriage actual departure");
                    }
                }
                else if (isWarehouseDeliveriesLegExists) {
                    if (this.IsDateSeriesBigger(MainCarriageETA, WarehouseLegEED)) {
                        MainCarriageErrors.push("Main-Carriage expected arrival must be less than Warehouse expected entry");
                    }
                    if (this.IsDateSeriesBigger(MainCarriageATA, WarehouseLegAED)) {
                        MainCarriageErrors.push("Main-Carriage actual arrival must be less than Warehouse actual entry");
                    }
                }
                else if (isDeliveriesExists) {
                    if (this.IsDateSeriesBigger(MainCarriageETA, allDeliveriesETD)) {
                        MainCarriageErrors.push("Main-Carriage expected arrival must be less than all deliveries expected departure");
                    }
                    if (this.IsDateSeriesBigger(MainCarriageATA, allDeliveriesATD)) {
                        MainCarriageErrors.push("Main-Carriage actual arrival must be less than all deliveries actual departure");
                    }
                }
            }
            // Transshipment1
            if (isTransshipment1Exists) {
                // Self
                if (!this.IsRoutingLegDatesValid(Transshipment1ETD, Transshipment1ETA)) {
                    MainCarriageErrors.push("Via1 expected departure must be less than Via1 expected arrival");
                }
                if (!this.IsRoutingLegDatesValid(Transshipment1ATD, Transshipment1ATA)) {
                    MainCarriageErrors.push("Via1 actual departure must be less than Via1 actual arrival");
                }
                //if (this.CompairDateSeries(Transshipment1ETD, Transshipment1ETA, ">")) {
                //    MainCarriageErrors.push("Via1 expected departure must be less than Via1 expected arrival");
                //}
                //if (this.CompairDateSeries(Transshipment1ATD, Transshipment1ATA, ">")) {
                //    MainCarriageErrors.push("Via1 actual departure must be less than Via1 actual arrival");
                //}
                // Next
                if (isTransshipment2Exists) {
                    if (this.IsDateSeriesBigger(Transshipment1ETA, Transshipment2ETD)) {
                        MainCarriageErrors.push("Via1 expected arrival must be less than Via2 expected departure");
                    }
                    if (this.IsDateSeriesBigger(Transshipment1ATA, Transshipment2ATD)) {
                        MainCarriageErrors.push("Via1 actual arrival must be less than Via2 actual departure");
                    }
                }
                else if (isTransshipment3Exists) {
                    if (this.IsDateSeriesBigger(Transshipment1ETA, Transshipment3ETD)) {
                        MainCarriageErrors.push("Via1 expected arrival must be less than Via3 expected departure");
                    }
                    if (this.IsDateSeriesBigger(Transshipment1ATA, Transshipment3ATD)) {
                        MainCarriageErrors.push("Via1 actual arrival must be less than Via3 actual departure");
                    }
                }
                else if (isOnCarriageExists) {
                    if (this.IsDateSeriesBigger(Transshipment1ETA, OnCarriageETD)) {
                        MainCarriageErrors.push("Via1 expected arrival must be less than On-Carriage expected departure");
                    }
                    if (this.IsDateSeriesBigger(Transshipment1ATA, OnCarriageATD)) {
                        MainCarriageErrors.push("Via1 actual arrival must be less than On-Carriage actual departure");
                    }
                }
                else if (isWarehouseDeliveriesLegExists) {
                    if (this.IsDateSeriesBigger(Transshipment1ETA, WarehouseLegEED)) {
                        MainCarriageErrors.push("Via1 expected arrival must be less than Warehouse expected entry");
                    }
                    if (this.IsDateSeriesBigger(Transshipment1ATA, WarehouseLegAED)) {
                        MainCarriageErrors.push("Via1 actual arrival must be less than Warehouse actual entry");
                    }
                }
                else if (isDeliveriesExists) {
                    if (this.IsDateSeriesBigger(Transshipment1ETA, allDeliveriesETD)) {
                        MainCarriageErrors.push("Via1 expected arrival must be less than all deliveries expected departure");
                    }
                    if (this.IsDateSeriesBigger(Transshipment1ATA, allDeliveriesATD)) {
                        MainCarriageErrors.push("Via1 actual arrival must be less than all deliveries actual departure");
                    }
                }
            }
            // Transshipment2
            if (isTransshipment2Exists) {
                // Self
                if (!this.IsRoutingLegDatesValid(Transshipment2ETD, Transshipment2ETA)) {
                    MainCarriageErrors.push("Via2 expected departure must be less than Via2 expected arrival");
                }
                if (!this.IsRoutingLegDatesValid(Transshipment2ATD, Transshipment2ATA)) {
                    MainCarriageErrors.push("Via2 actual departure must be less than Via2 actual arrival");
                }
                //if (this.CompairDateSeries(Transshipment2ETD, Transshipment2ETA, ">")) {
                //    MainCarriageErrors.push("Via2 expected departure must be less than Via2 expected arrival");
                //}
                //if (this.CompairDateSeries(Transshipment2ATD, Transshipment2ATA, ">")) {
                //    MainCarriageErrors.push("Via2 actual departure must be less than Via2 actual arrival");
                //}
                // Next
                if (isTransshipment3Exists) {
                    if (this.IsDateSeriesBigger(Transshipment2ETA, Transshipment3ETD)) {
                        MainCarriageErrors.push("Via2 expected arrival must be less than Via3 expected departure");
                    }
                    if (this.IsDateSeriesBigger(Transshipment2ATA, Transshipment3ATD)) {
                        MainCarriageErrors.push("Via2 actual arrival must be less than Via3 actual departure");
                    }
                }
                else if (isOnCarriageExists) {
                    if (this.IsDateSeriesBigger(Transshipment2ETA, OnCarriageETD)) {
                        MainCarriageErrors.push("Via2 expected arrival must be less than On-Carriage expected departure");
                    }
                    if (this.IsDateSeriesBigger(Transshipment2ATA, OnCarriageATD)) {
                        MainCarriageErrors.push("Via2 actual arrival must be less than On-Carriage actual departure");
                    }
                }
                else if (isWarehouseDeliveriesLegExists) {
                    if (this.IsDateSeriesBigger(Transshipment2ETA, WarehouseLegEED)) {
                        MainCarriageErrors.push("Via2 expected arrival must be less than Warehouse expected entry");
                    }
                    if (this.IsDateSeriesBigger(Transshipment2ATA, WarehouseLegAED)) {
                        MainCarriageErrors.push("Via2 actual arrival must be less than Warehouse actual entry");
                    }
                }
                else if (isDeliveriesExists) {
                    if (this.IsDateSeriesBigger(Transshipment2ETA, allDeliveriesETD)) {
                        MainCarriageErrors.push("Via2 expected arrival must be less than all deliveries expected departure");
                    }
                    if (this.IsDateSeriesBigger(Transshipment2ATA, allDeliveriesATD)) {
                        MainCarriageErrors.push("Via2 actual arrival must be less than all deliveries actual departure");
                    }
                }
            }
            // Transshipment3
            if (isTransshipment3Exists) {
                // Self
                if (!this.IsRoutingLegDatesValid(Transshipment3ETD, Transshipment3ETA)) {
                    MainCarriageErrors.push("Via3 expected departure must be less than Via3 expected arrival");
                }
                if (!this.IsRoutingLegDatesValid(Transshipment3ATD, Transshipment3ATA)) {
                    MainCarriageErrors.push("Via3 actual departure must be less than Via3 actual arrival");
                }
                //if (this.CompairDateSeries(Transshipment3ETD, Transshipment3ETA, ">")) {
                //    MainCarriageErrors.push("Via3 expected departure must be less than Via3 expected arrival");
                //}
                //if (this.CompairDateSeries(Transshipment3ATD, Transshipment3ATA, ">")) {
                //    MainCarriageErrors.push("Via3 actual departure must be less than Via3 actual arrival");
                //}
                // Next
                if (isOnCarriageExists) {
                    if (this.IsDateSeriesBigger(Transshipment3ETA, OnCarriageETD)) {
                        MainCarriageErrors.push("Via3 expected arrival must be less than On-Carriage expected departure");
                    }
                    if (this.IsDateSeriesBigger(Transshipment3ATA, OnCarriageATD)) {
                        MainCarriageErrors.push("Via3 actual arrival must be less than On-Carriage actual departure");
                    }
                }
                else if (isWarehouseDeliveriesLegExists) {
                    if (this.IsDateSeriesBigger(Transshipment3ETA, WarehouseLegEED)) {
                        MainCarriageErrors.push("Via3 expected arrival must be less than Warehouse expected entry");
                    }
                    if (this.IsDateSeriesBigger(Transshipment3ATA, WarehouseLegAED)) {
                        MainCarriageErrors.push("Via3 actual arrival must be less than Warehouse actual entry");
                    }
                }
                else if (isDeliveriesExists) {
                    if (this.IsDateSeriesBigger(Transshipment3ETA, allDeliveriesETD)) {
                        MainCarriageErrors.push("Via3 expected arrival must be less than all deliveries expected departure");
                    }
                    if (this.IsDateSeriesBigger(Transshipment3ATA, allDeliveriesATD)) {
                        MainCarriageErrors.push("Via3 actual arrival must be less than all deliveries actual departure");
                    }
                }
            }
            // WarehouseLeg
            if (isWarehouseLegExists) {
                // Self
                if (!this.IsRoutingLegDatesValid(WarehouseLegEED, WarehouseLegERD)) {
                    WarehouseLegErrors.push("Warehouse expected entry must be less than Warehouse expected release");
                }
                if (!this.IsRoutingLegDatesValid(WarehouseLegAED, WarehouseLegARD)) {
                    WarehouseLegErrors.push("Warehouse actual entry must be less than Warehouse actual release");
                }
                //if (this.CompairDateSeries(WarehouseLegEED, WarehouseLegERD, ">")) {
                //    WarehouseLegErrors.push("Warehouse expected entry must be less than Warehouse expected release");
                //}
                //if (this.CompairDateSeries(WarehouseLegAED, WarehouseLegARD, ">")) {
                //    WarehouseLegErrors.push("Warehouse actual entry must be less than Warehouse actual release");
                //}
                if (entityPM.DirectionId == "I") {
                    // Previous
                    if (isOnCarriageExists) {
                        if (this.IsDateSeriesSmaller(WarehouseLegEED, OnCarriageETA)) {
                            WarehouseLegErrors.push("Warehouse expected entry must be bigger than On-Carriage expected arrival");
                        }
                        if (this.IsDateSeriesSmaller(WarehouseLegAED, OnCarriageATA)) {
                            WarehouseLegErrors.push("Warehouse actual entry must be bigger than On-Carriage actual arrival");
                        }
                    }
                    else if (isTransshipment3Exists) {
                        if (this.IsDateSeriesSmaller(WarehouseLegEED, Transshipment3ETA)) {
                            WarehouseLegErrors.push("Warehouse expected entry must be bigger than Transshipment3 expected arrival");
                        }
                        if (this.IsDateSeriesSmaller(WarehouseLegAED, Transshipment3ATA)) {
                            WarehouseLegErrors.push("Warehouse actual entry must be bigger than Transshipment3 actual arrival");
                        }
                    }
                    else if (isTransshipment2Exists) {
                        if (this.IsDateSeriesSmaller(WarehouseLegEED, Transshipment2ETA)) {
                            WarehouseLegErrors.push("Warehouse expected entry must be bigger than Transshipment2 expected arrival");
                        }
                        if (this.IsDateSeriesSmaller(WarehouseLegAED, Transshipment2ATA)) {
                            WarehouseLegErrors.push("Warehouse actual entry must be bigger than Transshipment2 actual arrival");
                        }
                    }
                    else if (isTransshipment1Exists) {
                        if (this.IsDateSeriesSmaller(WarehouseLegEED, Transshipment1ETA)) {
                            WarehouseLegErrors.push("Warehouse expected entry must be bigger than Transshipment1 expected arrival");
                        }
                        if (this.IsDateSeriesSmaller(WarehouseLegAED, Transshipment1ATA)) {
                            WarehouseLegErrors.push("Warehouse actual entry must be bigger than Transshipment1 actual arrival");
                        }
                    }
                    else {
                        if (this.IsDateSeriesSmaller(WarehouseLegEED, MainCarriageETA)) {
                            WarehouseLegErrors.push("Warehouse expected entry must be bigger than Main-Carriage expected arrival");
                        }
                        if (this.IsDateSeriesSmaller(WarehouseLegAED, MainCarriageATA)) {
                            WarehouseLegErrors.push("Warehouse actual entry must be bigger than Main-Carriage actual arrival");
                        }
                    }
                    // Next
                    if (isDeliveriesExists) {
                        if (this.IsDateSeriesBigger(WarehouseLegERD, allDeliveriesETD)) {
                            WarehouseLegErrors.push("Warehouse expected release must be less than all deliveries expected departure");
                        }
                        if (this.IsDateSeriesBigger(WarehouseLegARD, allDeliveriesATD)) {
                            WarehouseLegErrors.push("Warehouse actual release must be less than all deliveries actual departure");
                        }
                    }
                }
                else {
                    // Previous
                    if (isPickupsExists) {
                        if (this.IsDateSeriesSmallerNotEqual(WarehouseLegEED, allPickupsETA)) {
                            WarehouseLegErrors.push("Warehouse expected entry must be equal or bigger than all pick ups expected arrival");
                        }
                        if (this.IsDateSeriesSmallerNotEqual(WarehouseLegAED, allPickupsATA)) {
                            WarehouseLegErrors.push("Warehouse actual entry must be equal or bigger than all pick ups actual arrival");
                        }
                    }
                    // Next
                    if (isPreCarriageExists) {
                        if (this.IsDateSeriesBigger(WarehouseLegERD, PreCarriageETD)) {
                            WarehouseLegErrors.push("Warehouse expected release must be less than pre carriage expected departure");
                        }
                        if (this.IsDateSeriesBigger(WarehouseLegARD, PreCarriageATD)) {
                            WarehouseLegErrors.push("Warehouse actual release must be less than pre carriage actual departure");
                        }
                    }
                    else if (isMainCarriageExists) {
                        if (this.IsDateSeriesBigger(WarehouseLegERD, MainCarriageETD)) {
                            WarehouseLegErrors.push("Warehouse expected release must be less than main carriage expected departure");
                        }
                        if (this.IsDateSeriesBigger(WarehouseLegARD, MainCarriageATD)) {
                            WarehouseLegErrors.push("Warehouse actual release must be less than main carriage actual departure");
                        }
                    }
                }
            }
            switch (legCode) {
                case "PreCarriage": {
                    PreCarriageErrors.forEach(function (item) {
                        errors.push(item);
                    });
                    break;
                }
                case "MainCarriage": {
                    MainCarriageErrors.forEach(function (item) {
                        errors.push(item);
                    });
                    break;
                }
                case "OnCarriage": {
                    OnCarriageErrors.forEach(function (item) {
                        errors.push(item);
                    });
                    break;
                }
                case "WarehouseLeg":
                case "WarehouseLeg_Pickups":
                    {
                        WarehouseLegErrors.forEach(function (item) {
                            errors.push(item);
                        });
                        break;
                    }
                default: {
                    allPickupsErrors.forEach(function (item) {
                        errors.push(item);
                    });
                    PreCarriageErrors.forEach(function (item) {
                        errors.push(item);
                    });
                    MainCarriageErrors.forEach(function (item) {
                        errors.push(item);
                    });
                    OnCarriageErrors.forEach(function (item) {
                        errors.push(item);
                    });
                    allDeliveriesErrors.forEach(function (item) {
                        errors.push(item);
                    });
                    WarehouseLegErrors.forEach(function (item) {
                        errors.push(item);
                    });
                    break;
                }
            }
        }
    };
    RoutingHelper.ValidateRoutingsSeriesDates_PackageFollowup = function (entityPM, myPackagePM, errors, legCode) {
        if (entityPM != null && myPackagePM != null) {
            if (errors == null) {
                errors = [];
            }
            var isPreCarriageExists = (entityPM.PreCarriageFromPortId != null && entityPM.PreCarriageToPortId != null) ? true : false;
            var PreCarriageETD = isPreCarriageExists ? Tools_1.DateTool.GetDateParts(entityPM.PreCarriageETD).DateTicks : 0;
            var PreCarriageETA = isPreCarriageExists ? Tools_1.DateTool.GetDateParts(entityPM.PreCarriageETA).DateTicks : 0;
            var PreCarriageATD = isPreCarriageExists ? Tools_1.DateTool.GetDateParts(entityPM.PreCarriageATD).DateTicks : 0;
            var PreCarriageATA = isPreCarriageExists ? Tools_1.DateTool.GetDateParts(entityPM.PreCarriageATA).DateTicks : 0;
            var isMainCarriageExists = true;
            var MainCarriageETD = Tools_1.DateTool.GetDateParts(entityPM.MainCarriageETD).DateTicks;
            var MainCarriageETA = Tools_1.DateTool.GetDateParts(entityPM.MainCarriageETA).DateTicks;
            var MainCarriageATD = Tools_1.DateTool.GetDateParts(entityPM.MainCarriageATD).DateTicks;
            var MainCarriageATA = Tools_1.DateTool.GetDateParts(entityPM.MainCarriageATA).DateTicks;
            var isTransshipment1Exists = (entityPM.Transshipment1FromPortId != null && entityPM.Transshipment1ToPortId != null) ? true : false;
            var Transshipment1ETD = isTransshipment1Exists ? Tools_1.DateTool.GetDateParts(entityPM.Transshipment1ETD).DateTicks : 0;
            var Transshipment1ETA = isTransshipment1Exists ? Tools_1.DateTool.GetDateParts(entityPM.Transshipment1ETA).DateTicks : 0;
            var Transshipment1ATD = isTransshipment1Exists ? Tools_1.DateTool.GetDateParts(entityPM.Transshipment1ATD).DateTicks : 0;
            var Transshipment1ATA = isTransshipment1Exists ? Tools_1.DateTool.GetDateParts(entityPM.Transshipment1ATA).DateTicks : 0;
            var isTransshipment2Exists = (entityPM.Transshipment2FromPortId != null && entityPM.Transshipment2ToPortId != null) ? true : false;
            var Transshipment2ETD = isTransshipment2Exists ? Tools_1.DateTool.GetDateParts(entityPM.Transshipment2ETD).DateTicks : 0;
            var Transshipment2ETA = isTransshipment2Exists ? Tools_1.DateTool.GetDateParts(entityPM.Transshipment2ETA).DateTicks : 0;
            var Transshipment2ATD = isTransshipment2Exists ? Tools_1.DateTool.GetDateParts(entityPM.Transshipment2ATD).DateTicks : 0;
            var Transshipment2ATA = isTransshipment2Exists ? Tools_1.DateTool.GetDateParts(entityPM.Transshipment2ATA).DateTicks : 0;
            var isTransshipment3Exists = (entityPM.Transshipment3FromPortId != null && entityPM.Transshipment3ToPortId != null) ? true : false;
            var Transshipment3ETD = isTransshipment3Exists ? Tools_1.DateTool.GetDateParts(entityPM.Transshipment3ETD).DateTicks : 0;
            var Transshipment3ETA = isTransshipment3Exists ? Tools_1.DateTool.GetDateParts(entityPM.Transshipment3ETA).DateTicks : 0;
            var Transshipment3ATD = isTransshipment3Exists ? Tools_1.DateTool.GetDateParts(entityPM.Transshipment3ATD).DateTicks : 0;
            var Transshipment3ATA = isTransshipment3Exists ? Tools_1.DateTool.GetDateParts(entityPM.Transshipment3ATA).DateTicks : 0;
            var isOnCarriageExists = (entityPM.OnCarriageFromPortId != null && entityPM.OnCarriageToPortId != null) ? true : false;
            var OnCarriageETD = isOnCarriageExists ? Tools_1.DateTool.GetDateParts(entityPM.OnCarriageETD).DateTicks : 0;
            var OnCarriageETA = isOnCarriageExists ? Tools_1.DateTool.GetDateParts(entityPM.OnCarriageETA).DateTicks : 0;
            var OnCarriageATD = isOnCarriageExists ? Tools_1.DateTool.GetDateParts(entityPM.OnCarriageATD).DateTicks : 0;
            var OnCarriageATA = isOnCarriageExists ? Tools_1.DateTool.GetDateParts(entityPM.OnCarriageATA).DateTicks : 0;
            // Package Follow up
            var ETD = 0;
            var ETA = 0;
            var ATD = 0;
            var ATA = 0;
            switch (legCode) {
                case "D": {
                    ETD = Tools_1.DateTool.GetDateParts(myPackagePM.DeliveryETD).DateTicks;
                    ETA = Tools_1.DateTool.GetDateParts(myPackagePM.DeliveryETA).DateTicks;
                    ATD = Tools_1.DateTool.GetDateParts(myPackagePM.DeliveryATD).DateTicks;
                    ATA = Tools_1.DateTool.GetDateParts(myPackagePM.DeliveryATA).DateTicks;
                    break;
                }
                case "R": {
                    ETD = Tools_1.DateTool.GetDateParts(myPackagePM.EmptyContainerReturnETD).DateTicks;
                    ETA = Tools_1.DateTool.GetDateParts(myPackagePM.EmptyContainerReturnETA).DateTicks;
                    ATD = Tools_1.DateTool.GetDateParts(myPackagePM.EmptyContainerReturnATD).DateTicks;
                    ATA = Tools_1.DateTool.GetDateParts(myPackagePM.EmptyContainerReturnATA).DateTicks;
                    break;
                }
            }
            // Self
            if (!this.IsRoutingLegDatesValid(ETD, ETA)) {
                errors.push("Expected departure must be less than expected arrival");
            }
            if (!this.IsRoutingLegDatesValid(ATD, ATA)) {
                errors.push("Actual departure must be less than actual arrival");
            }
            //if (this.CompairDateSeries(ETD, ETA, ">")) {
            //    errors.push("Expected departure must be less than expected arrival");
            //}
            //if (this.CompairDateSeries(ATD, ATA, ">")) {
            //    errors.push("Actual departure must be less than actual arrival");
            //}
            // Previous
            if (isOnCarriageExists) {
                if (this.IsDateSeriesSmaller(ETD, OnCarriageETA)) {
                    errors.push("Expected departure must be bigger than On-Carriage expected arrival");
                }
                if (this.IsDateSeriesSmaller(ATD, OnCarriageATA)) {
                    errors.push("Actual departure must be bigger than On-Carriage actual arrival");
                }
            }
            else if (isTransshipment3Exists) {
                if (this.IsDateSeriesSmaller(ETD, Transshipment3ETA)) {
                    errors.push("Expected departure must be bigger than Transshipment3 expected arrival");
                }
                if (this.IsDateSeriesSmaller(ATD, Transshipment3ATA)) {
                    errors.push("Actual departure must be bigger than Transshipment3 actual arrival");
                }
            }
            else if (isTransshipment2Exists) {
                if (this.IsDateSeriesSmaller(ETD, Transshipment2ETA)) {
                    errors.push("Expected departure must be bigger than Transshipment2 expected arrival");
                }
                if (this.IsDateSeriesSmaller(ATD, Transshipment2ATA)) {
                    errors.push("Actual departure must be bigger than Transshipment2 actual arrival");
                }
            }
            else if (isTransshipment1Exists) {
                if (this.IsDateSeriesSmaller(ETD, Transshipment1ETA)) {
                    errors.push("Expected departure must be bigger than Transshipment1 expected arrival");
                }
                if (this.IsDateSeriesSmaller(ATD, Transshipment1ATA)) {
                    errors.push("Actual departure must be bigger than Transshipment1 actual arrival");
                }
            }
            else {
                if (this.IsDateSeriesSmaller(ETD, MainCarriageETA)) {
                    errors.push("Expected departure must be bigger than Main-Carriage expected arrival");
                }
                if (this.IsDateSeriesSmaller(ATD, MainCarriageATA)) {
                    errors.push("Actual departure must be bigger than Main-Carriage actual arrival");
                }
            }
        }
    };
    RoutingHelper.IsRoutingLegDatesValid = function (Date1Ticks, Date2Ticks) {
        var myResult = true;
        if (Date1Ticks && Date2Ticks) {
            if (Date1Ticks > 0 && Date2Ticks > 0) {
                if (Date1Ticks > Date2Ticks) {
                    var ticks = Date1Ticks - Date2Ticks;
                    var seconds = ticks / 1000;
                    var minutes = seconds / 60;
                    if (minutes > (24 * 60)) {
                        myResult = false;
                    }
                }
            }
        }
        return myResult;
    };
    RoutingHelper.CompairDateSeries = function (Date1Ticks, Date2Ticks, Operator) {
        var myResult = false;
        if (Date1Ticks != 0 && Date2Ticks != 0) {
            switch (Operator) {
                case ">": {
                    if (Date1Ticks > Date2Ticks) {
                        myResult = true;
                    }
                    break;
                }
                case ">=": {
                    if (Date1Ticks >= Date2Ticks) {
                        myResult = true;
                    }
                    break;
                }
                case "<=": {
                    if (Date1Ticks <= Date2Ticks) {
                        myResult = true;
                    }
                    break;
                }
            }
        }
        return myResult;
    };
    RoutingHelper.IsDateSeriesBigger = function (Date1Ticks, Date2Ticks) {
        var myResult = false;
        if (Date1Ticks != 0 && Date2Ticks != 0) {
            if (Date1Ticks >= Date2Ticks) {
                myResult = true;
            }
        }
        return myResult;
    };
    RoutingHelper.IsDateSeriesBiggerNotEqual = function (Date1Ticks, Date2Ticks) {
        var myResult = false;
        if (Date1Ticks != 0 && Date2Ticks != 0) {
            if (Date1Ticks > Date2Ticks) {
                myResult = true;
            }
        }
        return myResult;
    };
    RoutingHelper.IsDateSeriesSmaller = function (Date1Ticks, Date2Ticks) {
        var myResult = false;
        if (Date1Ticks != 0 && Date2Ticks != 0) {
            if (Date1Ticks <= Date2Ticks) {
                myResult = true;
            }
        }
        return myResult;
    };
    RoutingHelper.IsDateSeriesSmallerNotEqual = function (Date1Ticks, Date2Ticks) {
        var myResult = false;
        if (Date1Ticks != 0 && Date2Ticks != 0) {
            if (Date1Ticks < Date2Ticks) {
                myResult = true;
            }
        }
        return myResult;
    };
    RoutingHelper.ValidateLegDates = function (shipmentPM, pickUpPM, deliveryPM, errors, legCode) {
        if (!Tools_1.AppTool.IsNullOrEmpty(legCode)) {
            switch (legCode) {
                case "Pick Up":
                    {
                        if (pickUpPM != null) {
                            /* with it self */
                            if (pickUpPM.ETD > pickUpPM.ETA) {
                                errors.push("Pick up expected departure must be less than pick up expected arrival");
                            }
                            if (pickUpPM.ATD > pickUpPM.ATA) {
                                errors.push("Pick up actual departure must be less than pick up actual arrival");
                            }
                            /* flow up */
                            if (shipmentPM.PreCarriageFromPortId != null && shipmentPM.PreCarriageToPortId != null) {
                                if (pickUpPM.ETA >= shipmentPM.PreCarriageETD) {
                                    errors.push("Pick up expected arrival must be less than pre carriage expected departure");
                                }
                                if (pickUpPM.ATA >= shipmentPM.PreCarriageATD) {
                                    errors.push("Pick up actual arrival must be less than pre carriage actual departure");
                                }
                            }
                            else {
                                if (pickUpPM.ETA >= shipmentPM.MainCarriageETD) {
                                    errors.push("Pick up expected arrival must be less than main carriage expected departure");
                                }
                                if (pickUpPM.ATA >= shipmentPM.MainCarriageATD) {
                                    errors.push("Pick up actual arrival must be less than main carriage actual departure");
                                }
                            }
                        }
                        break;
                    }
                case "Delivery":
                    {
                        if (deliveryPM != null) {
                            /* with it self */
                            if (deliveryPM.ETD > deliveryPM.ETA) {
                                errors.push("Delivery expected departure must be less than Delivery expected arrival");
                            }
                            if (deliveryPM.ATD > deliveryPM.ATA) {
                                errors.push("Delivery actual departure must be less than Delivery actual arrival");
                            }
                            /* flow down */
                            if (shipmentPM.OnCarriageFromPortId != null && shipmentPM.OnCarriageToPortId != null) {
                                if (deliveryPM.ETD <= shipmentPM.OnCarriageETA) {
                                    errors.push("Delivery expected departure must be bigger than On-Carriage expected arrival");
                                }
                                if (deliveryPM.ATD <= shipmentPM.OnCarriageATA) {
                                    errors.push("Delivery actual departure must be bigger than On-Carriage actual arrival");
                                }
                            }
                            else if (shipmentPM.Transshipment3FromPortId != null && shipmentPM.Transshipment3ToPortId != null) {
                                if (deliveryPM.ETD <= shipmentPM.Transshipment3ETA) {
                                    errors.push("Delivery expected departure must be bigger than Transshipment3 expected arrival");
                                }
                                if (deliveryPM.ATD <= shipmentPM.Transshipment3ATA) {
                                    errors.push("Delivery actual departure must be bigger than Transshipment3 actual arrival");
                                }
                            }
                            else if (shipmentPM.Transshipment2FromPortId != null && shipmentPM.Transshipment2ToPortId != null) {
                                if (deliveryPM.ETD <= shipmentPM.Transshipment2ETA) {
                                    errors.push("Delivery expected departure must be bigger than Transshipment2 expected arrival");
                                }
                                if (deliveryPM.ATD <= shipmentPM.Transshipment2ATA) {
                                    errors.push("Delivery actual departure must be bigger than Transshipment2 actual arrival");
                                }
                            }
                            else if (shipmentPM.Transshipment1FromPortId != null && shipmentPM.Transshipment1ToPortId != null) {
                                if (deliveryPM.ETD <= shipmentPM.Transshipment1ETA) {
                                    errors.push("Delivery expected departure must be bigger than Transshipment1 expected arrival");
                                }
                                if (deliveryPM.ATD <= shipmentPM.Transshipment1ATA) {
                                    errors.push("Delivery actual departure must be bigger than Transshipment1 actual arrival");
                                }
                            }
                            else {
                                if (deliveryPM.ETD <= shipmentPM.MainCarriageETA) {
                                    errors.push("Delivery expected departure must be bigger than Main-Carriage expected arrival");
                                }
                                if (deliveryPM.ATD <= shipmentPM.MainCarriageATA) {
                                    errors.push("Delivery actual departure must be bigger than Main-Carriage actual arrival");
                                }
                            }
                        }
                        break;
                    }
                case "Pre Carriage":
                    {
                        if (shipmentPM.PreCarriageFromPortId != null && shipmentPM.PreCarriageToPortId != null) {
                            /* with it self */
                            if (shipmentPM.PreCarriageETD > shipmentPM.PreCarriageETA) {
                                errors.push("Pre-Carriage expected departure must be less than Pre-Carriage expected arrival");
                            }
                            if (shipmentPM.PreCarriageATD > shipmentPM.PreCarriageATA) {
                                errors.push("Pre-Carriage actual departure must be less than Pre-Carriage actual arrival");
                            }
                            /* flow up */
                            if (shipmentPM.PreCarriageETA >= shipmentPM.MainCarriageETD) {
                                errors.push("Pre-Carriage expected arrival must be less than Main-Carriage expected departure");
                            }
                            if (shipmentPM.PreCarriageATA >= shipmentPM.MainCarriageATD) {
                                errors.push("Pre-Carriage actual arrival must be less than Main-Carriage actual departure");
                            }
                            /* flow down */
                            //List < ShipmentPickUpPM > shipmentPickups = shipmentPM.ShipmentPickUps.ToList();
                            //if (shipmentPickups.Count > 0) {
                            //    DateTime ? shipmentPicksETA = shipmentPickups.Max(m => m.ETA);
                            //    DateTime ? shipmentPicksATA = shipmentPickups.Max(m => m.ATA);
                            //    if (shipmentPM.PreCarriageETD <= shipmentPicksETA) {
                            //        errors.push("Pre-Carriage expected departure must be bigger than all pick ups expected arrival");
                            //    }
                            //    if (shipmentPM.PreCarriageATD <= shipmentPicksATA) {
                            //        errors.push("Pre-Carriage actual departure must be bigger than all pick ups actual arrival");
                            //    }
                            //}
                        }
                        break;
                    }
                case "On Carriage":
                    {
                        if (shipmentPM.OnCarriageFromPortId != null && shipmentPM.OnCarriageToPortId != null) {
                            /* with it self */
                            if (shipmentPM.OnCarriageETD > shipmentPM.OnCarriageETA) {
                                errors.push("On-Carriage expected departure must be less than On-Carriage expected arrival");
                            }
                            if (shipmentPM.OnCarriageATD > shipmentPM.OnCarriageATA) {
                                errors.push("On-Carriage actual departure must be less than On-Carriage actual arrival");
                            }
                            /* flow down */
                            if (shipmentPM.Transshipment3FromPortId != null && shipmentPM.Transshipment3ToPortId != null) {
                                if (shipmentPM.OnCarriageETD <= shipmentPM.Transshipment3ETA) {
                                    errors.push("On-Carriage expected departure must be bigger than Transshipment3 expected arrival");
                                }
                                if (shipmentPM.OnCarriageATD <= shipmentPM.Transshipment3ATA) {
                                    errors.push("On-Carriage actual departure must be bigger than Transshipment3 actual arrival");
                                }
                            }
                            else if (shipmentPM.Transshipment2FromPortId != null && shipmentPM.Transshipment2ToPortId != null) {
                                if (shipmentPM.OnCarriageETD <= shipmentPM.Transshipment2ETA) {
                                    errors.push("On-Carriage expected departure must be bigger than Transshipment2 expected arrival");
                                }
                                if (shipmentPM.OnCarriageATD <= shipmentPM.Transshipment2ATA) {
                                    errors.push("On-Carriage actual departure must be bigger than Transshipment2 actual arrival");
                                }
                            }
                            else if (shipmentPM.Transshipment1FromPortId != null && shipmentPM.Transshipment1ToPortId != null) {
                                if (shipmentPM.OnCarriageETD <= shipmentPM.Transshipment1ETA) {
                                    errors.push("On-Carriage expected departure must be bigger than Transshipment1 expected arrival");
                                }
                                if (shipmentPM.OnCarriageATD <= shipmentPM.Transshipment1ATA) {
                                    errors.push("On-Carriage actual departure must be bigger than Transshipment1 actual arrival");
                                }
                            }
                            else {
                                if (shipmentPM.OnCarriageETD <= shipmentPM.MainCarriageETA) {
                                    errors.push("On-Carriage expected departure must be bigger than Main-Carriage expected arrival");
                                }
                                if (shipmentPM.OnCarriageATD <= shipmentPM.MainCarriageATA) {
                                    errors.push("On-Carriage actual departure must be bigger than Main-Carriage actual arrival");
                                }
                            }
                            /* flow up */
                            //List < ShipmentDeliveryPM > shipmentDeliveries = shipmentPM.ShipmentDeliveries.ToList();
                            //if (shipmentDeliveries.Count > 0) {
                            //    DateTime ? shipmentDeliveriesETD = shipmentDeliveries.Min(m => m.ETD);
                            //    DateTime ? shipmentDeliveriesATD = shipmentDeliveries.Min(m => m.ATD);
                            //    if (shipmentPM.OnCarriageETA >= shipmentDeliveriesETD) {
                            //        errors.push("On-Carriage expected arrival must be less than all deliveries expected departure");
                            //    }
                            //    if (shipmentPM.OnCarriageATA >= shipmentDeliveriesATD) {
                            //        errors.push("On-Carriage actual departure must be less than all deliveries actual arrival");
                            //    }
                            //}
                        }
                        break;
                    }
                case "Main Carriage":
                    {
                        /* with it self */
                        if (shipmentPM.MainCarriageETD > shipmentPM.MainCarriageETA) {
                            errors.push("Main-Carriage expected departure must be less than Main-Carriage expected arrival");
                        }
                        if (shipmentPM.MainCarriageATD > shipmentPM.MainCarriageATA) {
                            errors.push("Main-Carriage actual departure must be less than Main-Carriage actual arrival");
                        }
                        /* flow down */
                        if (shipmentPM.PreCarriageFromPortId != null && shipmentPM.PreCarriageToPortId != null) {
                            if (shipmentPM.MainCarriageETD <= shipmentPM.PreCarriageETA) {
                                errors.push("Main-Carriage expected departure must be bigger than Pre-Carriage expected arrival");
                            }
                            if (shipmentPM.MainCarriageATD <= shipmentPM.PreCarriageATA) {
                                errors.push("Main-Carriage actual departure must be bigger than Pre-Carriage actual arrival");
                            }
                        }
                        else {
                            //List < ShipmentPickUpPM > shipmentPickups = shipmentPM.ShipmentPickUps.ToList();
                            //if (shipmentPickups.Count > 0) {
                            //    DateTime ? shipmentPicksETA = shipmentPickups.Max(m => m.ETA);
                            //    DateTime ? shipmentPicksATA = shipmentPickups.Max(m => m.ATA);
                            //    if (shipmentPM.MainCarriageETD <= shipmentPicksETA) {
                            //        errors.push("Main-Carriage expected departure must be bigger than all pick ups expected arrival");
                            //    }
                            //    if (shipmentPM.MainCarriageATD <= shipmentPicksATA) {
                            //        errors.push("Main-Carriage actual departure must be bigger than all pick ups actual arrival");
                            //    }
                            //}
                        }
                        /* flow up */
                        if (shipmentPM.Transshipment1FromPortId != null && shipmentPM.Transshipment1ToPortId != null) {
                            if (shipmentPM.MainCarriageETA >= shipmentPM.Transshipment1ETD) {
                                errors.push("Main-Carriage expected arrival must be less than Via1 expected departure");
                            }
                            if (shipmentPM.MainCarriageATA >= shipmentPM.Transshipment1ATD) {
                                errors.push("Main-Carriage actual arrival must be less than Via1 actual departure");
                            }
                        }
                        else if (shipmentPM.Transshipment2FromPortId != null && shipmentPM.Transshipment2ToPortId != null) {
                            if (shipmentPM.MainCarriageETA >= shipmentPM.Transshipment2ETD) {
                                errors.push("Main-Carriage expected arrival must be less than Via2 expected departure");
                            }
                            if (shipmentPM.MainCarriageATA >= shipmentPM.Transshipment2ATD) {
                                errors.push("Main-Carriage actual arrival must be less than Via2 actual departure");
                            }
                        }
                        else if (shipmentPM.Transshipment3FromPortId != null && shipmentPM.Transshipment3ToPortId != null) {
                            if (shipmentPM.MainCarriageETA >= shipmentPM.Transshipment3ETD) {
                                errors.push("Main-Carriage expected arrival must be less than Via3 expected departure");
                            }
                            if (shipmentPM.MainCarriageATA >= shipmentPM.Transshipment3ATD) {
                                errors.push("Main-Carriage actual arrival must be less than Via3 actual departure");
                            }
                        }
                        else if (shipmentPM.OnCarriageFromPortId != null && shipmentPM.OnCarriageToPortId != null) {
                            if (shipmentPM.MainCarriageETA >= shipmentPM.OnCarriageETD) {
                                errors.push("Main-Carriage expected arrival must be less than On-Carriage expected departure");
                            }
                            if (shipmentPM.MainCarriageATA >= shipmentPM.OnCarriageATD) {
                                errors.push("Main-Carriage actual arrival must be less than On-Carriage actual departure");
                            }
                        }
                        else {
                            //List < ShipmentDeliveryPM > shipmentDeliveries = shipmentPM.ShipmentDeliveries.ToList();
                            //if (shipmentDeliveries.Count > 0) {
                            //    DateTime ? shipmentDeliveriesETD = shipmentDeliveries.Min(m => m.ETD);
                            //    DateTime ? shipmentDeliveriesATD = shipmentDeliveries.Min(m => m.ATD);
                            //    if (shipmentPM.MainCarriageETA >= shipmentDeliveriesETD) {
                            //        errors.push("Main-Carriage expected arrival must be less than all deliveries expected departure");
                            //    }
                            //    if (shipmentPM.MainCarriageATA >= shipmentDeliveriesATD) {
                            //        errors.push("Main-Carriage actual departure must be less than all deliveries actual arrival");
                            //    }
                            //}
                        }
                        break;
                    }
                case "Transshipment1":
                    {
                        if (shipmentPM.Transshipment1FromPortId != null && shipmentPM.Transshipment1ToPortId != null) {
                            /* with it self */
                            if (shipmentPM.Transshipment1ETD > shipmentPM.Transshipment1ETA) {
                                errors.push("Via1 expected departure must be less than Via1 expected arrival");
                            }
                            if (shipmentPM.Transshipment1ATD > shipmentPM.Transshipment1ATA) {
                                errors.push("Via1 actual departure must be less than Via1 actual arrival");
                            }
                            /* flow up */
                            if (shipmentPM.Transshipment2FromPortId != null && shipmentPM.Transshipment2ToPortId != null) {
                                if (shipmentPM.Transshipment1ETA >= shipmentPM.Transshipment2ETD) {
                                    errors.push("Via1 expected arrival must be less than Via2 expected departure");
                                }
                                if (shipmentPM.Transshipment1ATA >= shipmentPM.Transshipment2ATD) {
                                    errors.push("Via1 actual arrival must be less than Via2 actual departure");
                                }
                            }
                            else if (shipmentPM.Transshipment3FromPortId != null && shipmentPM.Transshipment3ToPortId != null) {
                                if (shipmentPM.Transshipment1ETA >= shipmentPM.Transshipment3ETD) {
                                    errors.push("Via1 expected arrival must be less than Via3 expected departure");
                                }
                                if (shipmentPM.Transshipment1ATA >= shipmentPM.Transshipment3ATD) {
                                    errors.push("Via1 actual arrival must be less than Via3 actual departure");
                                }
                            }
                            else if (shipmentPM.OnCarriageFromPortId != null && shipmentPM.OnCarriageToPortId != null) {
                                if (shipmentPM.Transshipment1ETA >= shipmentPM.OnCarriageETD) {
                                    errors.push("Via1 expected arrival must be less than On-Carriage expected departure");
                                }
                                if (shipmentPM.Transshipment1ATA >= shipmentPM.OnCarriageATD) {
                                    errors.push("Via1 actual arrival must be less than On-Carriage actual departure");
                                }
                            }
                            else {
                                //List < ShipmentDeliveryPM > shipmentDeliveries = shipmentPM.ShipmentDeliveries.ToList();
                                //if (shipmentDeliveries.Count > 0) {
                                //    DateTime ? shipmentDeliveriesETD = shipmentDeliveries.Min(m => m.ETD);
                                //    DateTime ? shipmentDeliveriesATD = shipmentDeliveries.Min(m => m.ATD);
                                //    if (shipmentPM.Transshipment1ETA >= shipmentDeliveriesETD) {
                                //        errors.push("Via1 expected arrival must be less than all deliveries expected departure");
                                //    }
                                //    if (shipmentPM.Transshipment1ATA >= shipmentDeliveriesATD) {
                                //        errors.push("Via1 actual departure must be less than all deliveries actual arrival");
                                //    }
                                //}
                            }
                        }
                        break;
                    }
                case "Transshipment2":
                    {
                        if (shipmentPM.Transshipment2FromPortId != null && shipmentPM.Transshipment2ToPortId != null) {
                            /* with it self */
                            if (shipmentPM.Transshipment2ETD > shipmentPM.Transshipment2ETA) {
                                errors.push("Via2 expected departure must be less than Via2 expected arrival");
                            }
                            if (shipmentPM.Transshipment2ATD > shipmentPM.Transshipment2ATA) {
                                errors.push("Via2 actual departure must be less than Via2 actual arrival");
                            }
                            /* flow up */
                            if (shipmentPM.Transshipment3FromPortId != null && shipmentPM.Transshipment3ToPortId != null) {
                                if (shipmentPM.Transshipment2ETA >= shipmentPM.Transshipment3ETD) {
                                    errors.push("Via2 expected arrival must be less than Via3 expected departure");
                                }
                                if (shipmentPM.Transshipment2ATA >= shipmentPM.Transshipment3ATD) {
                                    errors.push("Via2 actual arrival must be less than Via3 actual departure");
                                }
                            }
                            else if (shipmentPM.OnCarriageFromPortId != null && shipmentPM.OnCarriageToPortId != null) {
                                if (shipmentPM.Transshipment2ETA >= shipmentPM.OnCarriageETD) {
                                    errors.push("Via2 expected arrival must be less than On-Carriage expected departure");
                                }
                                if (shipmentPM.Transshipment2ATA >= shipmentPM.OnCarriageATD) {
                                    errors.push("Via2 actual arrival must be less than On-Carriage actual departure");
                                }
                            }
                            else {
                                //List < ShipmentDeliveryPM > shipmentDeliveries = shipmentPM.ShipmentDeliveries.ToList();
                                //if (shipmentDeliveries.Count > 0) {
                                //    DateTime ? shipmentDeliveriesETD = shipmentDeliveries.Min(m => m.ETD);
                                //    DateTime ? shipmentDeliveriesATD = shipmentDeliveries.Min(m => m.ATD);
                                //    if (shipmentPM.Transshipment2ETA >= shipmentDeliveriesETD) {
                                //        errors.push("Via2 expected arrival must be less than all deliveries expected departure");
                                //    }
                                //    if (shipmentPM.Transshipment2ATA >= shipmentDeliveriesATD) {
                                //        errors.push("Via2 actual departure must be less than all deliveries actual arrival");
                                //    }
                                //}
                            }
                        }
                        break;
                    }
                case "Transshipment3":
                    {
                        if (shipmentPM.Transshipment3FromPortId != null && shipmentPM.Transshipment3ToPortId != null) {
                            /* with it self */
                            if (shipmentPM.Transshipment3ETD > shipmentPM.Transshipment3ETA) {
                                errors.push("Via3 expected departure must be less than Via3 expected arrival");
                            }
                            if (shipmentPM.Transshipment3ATD > shipmentPM.Transshipment3ATA) {
                                errors.push("Via3 actual departure must be less than Via3 actual arrival");
                            }
                            /* flow up */
                            if (shipmentPM.OnCarriageFromPortId != null && shipmentPM.OnCarriageToPortId != null) {
                                if (shipmentPM.Transshipment3ETA >= shipmentPM.OnCarriageETD) {
                                    errors.push("Via3 expected arrival must be less than On-Carriage expected departure");
                                }
                                if (shipmentPM.Transshipment3ATA >= shipmentPM.OnCarriageATD) {
                                    errors.push("Via3 actual arrival must be less than On-Carriage actual departure");
                                }
                            }
                            else {
                                //List < ShipmentDeliveryPM > shipmentDeliveries = shipmentPM.ShipmentDeliveries.ToList();
                                //if (shipmentDeliveries.Count > 0) {
                                //    DateTime ? shipmentDeliveriesETD = shipmentDeliveries.Min(m => m.ETD);
                                //    DateTime ? shipmentDeliveriesATD = shipmentDeliveries.Min(m => m.ATD);
                                //    if (shipmentPM.Transshipment3ETA >= shipmentDeliveriesETD) {
                                //        errors.push("Via3 expected arrival must be less than all deliveries expected departure");
                                //    }
                                //    if (shipmentPM.Transshipment3ATA >= shipmentDeliveriesATD) {
                                //        errors.push("Via3 actual departure must be less than all deliveries actual arrival");
                                //    }
                                //}
                            }
                        }
                        break;
                    }
                default: {
                    break;
                }
            }
        }
    };
    RoutingHelper.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    return RoutingHelper;
}());
exports.RoutingHelper = RoutingHelper;
//# sourceMappingURL=Tools.js.map