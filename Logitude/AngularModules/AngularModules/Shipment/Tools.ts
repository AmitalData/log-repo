import {AppTool, DateTool, ArrayTool} from '../Infrastructure/Tools';
import {Validator} from '../Infrastructure/Validators/Validator';
import {FeatureLocator} from '../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../Infrastructure/Utilities/TextCodeTranslator';
import {ShipmentPM} from './EntityPMs/ShipmentPM';
import {ShipmentPayablePM} from './EntityPMs/ShipmentPayablePM';
import {ShipmentReceivablePM} from './EntityPMs/ShipmentReceivablePM';
import {ShipmentAWBPrintOnlyPM} from './EntityPMs/ShipmentAWBPrintOnlyPM';
import {ShipmentOrderPackagePM} from './EntityPMs/ShipmentOrderPackagePM';
import {ShipmentCommodityPM} from './EntityPMs/ShipmentCommodityPM';
import {ShipmentPackagePM} from './EntityPMs/ShipmentPackagePM';
import {InsideShipmentPackagePM} from './EntityPMs/InsideShipmentPackagePM';
import {QuotePM} from '../Quote/EntityPMs/QuotePM';
import {QuoteChargePM} from '../Quote/EntityPMs/QuoteChargePM';
import {QuotePriceStepsPM} from '../Quote/EntityPMs/QuotePriceStepsPM';
import {PortList} from '../Common/EntityLists/PortList';
import {CardList} from '../Common/EntityLists/CardList';
import {CurrencyList} from '../Common/EntityLists/CurrencyList';
import {PackageTypeList} from '../Common/EntityLists/PackageTypeList';
import {ChargesTypeList} from '../Common/EntityLists/ChargesTypeList';
import {CurrencyListService} from '../Common/Services/StandardLists/CurrencyListService';
import {PackageTypeListService} from '../Common/Services/StandardLists/PackageTypeListService';
import {ChargesTypeListService} from '../Common/Services/StandardLists/ChargesTypeListService';
import {LastRate} from '../Common/Services/CurrencyRatesService';
import {ServiceResponse} from '../Infrastructure/DataContracts/ServiceResponse';
import {ShipmentPickUpPM} from './EntityPMs/ShipmentPickUpPM';
import {ShipmentDeliveryPM} from './EntityPMs/ShipmentDeliveryPM';
import {VatTypeList} from '../Common/EntityLists/VatTypeList';

export class ShipmentTool {
    private static CurrentSession = SessionLocator.SelectedSession;
    public static IsEditingEnabled(entityPM: ShipmentPM) {
        var myResult: boolean = true;

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
    }
    public static IsInlandDomestic(entityPM: ShipmentPM) {
        var isInlandDomestic = false;

        if (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I") {
            isInlandDomestic = true;
        }

        return isInlandDomestic;
    }
    public static IsLCL(entityPM: ShipmentPM) {

        var result: boolean = false;

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
    }
    public static ComputeSCI(entityPM: ShipmentPM) {
        var myResult: string = null;

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
    }
    public static BuildAWBPlaceField(entityPM: ShipmentPM) {
        var myResult: string = null;

        if (entityPM.MainCarriageFromPortId != null) {
            myResult = entityPM.MainCarriageFromPortName + " " + entityPM.MainCarriageFromPortCountryName;
        }

        if (entityPM.AWBPlace != myResult) {
            entityPM.AWBPlace = myResult;
        }
    }
    public static GetLongMasterField(myTransportModeId: string, myAirlinePrefix: string, myMaster: string) {
        var myField: string = null;

        if (myTransportModeId == "A") {
            if (!AppTool.IsNullOrEmpty(myAirlinePrefix) && !AppTool.IsNullOrEmpty(myMaster)) {
                myField = myAirlinePrefix + "-" + myMaster;
            }
        }

        else {
            myField = myMaster;
        }

        return myField;
    }
    public static IsFullAWBWizard(DirectionId: string) {
        var isFullWizard = false;

        if (DirectionId == "E" || DirectionId == "R") {
            isFullWizard = true;
        }

        else if (DirectionId == "D") {
            if (!FeatureLocator.IsPackage_EAWB()) {
                isFullWizard = true;
            }
        }

        return isFullWizard;
    }
    public static GetAWBWizardHeader(ShipmentLevelCode: string, DirectionId: string) {
        var myResult: string = null;

        var isFullWizard = this.IsFullAWBWizard(DirectionId);

        if (isFullWizard) {
            switch (ShipmentLevelCode) {
                case "D": { myResult = "Direct AWB Wizard"; break; }
                case "H": { myResult = "House AWB Wizard"; break; }
                case "C": { myResult = "Master AWB Wizard"; break; }
                default: { break; }
            }
        }

        else {
            myResult = "Airline statuses";
        }

        return myResult;
    }
    public static MapTenantZeroAirline(entityPM: ShipmentPM, myAirlinePM: any) {
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
    }
    public static CopyShipment(shipmentPM: ShipmentPM, oldShipment: ShipmentPM) {

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
        oldShipment.ShipmentAWBPrintOnlies.forEach(item => {
            var newItem: ShipmentAWBPrintOnlyPM = new ShipmentAWBPrintOnlyPM(shipmentPM);
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
    }
    public static CopyShipmentPackages(shipmentPM: ShipmentPM, oldShipment: ShipmentPM, copyOtherProperties: boolean) {
        if (copyOtherProperties) {
            shipmentPM.IsDangerous = oldShipment.IsDangerous;
            shipmentPM.DescriptionOfGoods = oldShipment.DescriptionOfGoods;
            shipmentPM.BookingVolume = oldShipment.BookingVolume;
            shipmentPM.OrderVolumetricWeight = oldShipment.OrderVolumetricWeight;
            shipmentPM.OrderGrossWeight = oldShipment.OrderGrossWeight;
            shipmentPM.OrderChargeableWeight = oldShipment.OrderChargeableWeight;
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

        oldShipment.ShipmentOrderPackages.forEach(item => {
            var newItem: ShipmentOrderPackagePM = new ShipmentOrderPackagePM(shipmentPM);
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
            oldShipment.ShipmentPackages.forEach(item => {
                var newItem: ShipmentPackagePM = new ShipmentPackagePM(shipmentPM);
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

                item.InsideShipmentPackages.forEach(inside => {
                    var newItemInside: InsideShipmentPackagePM = new InsideShipmentPackagePM(item);
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
    }
    public static CopyFlights(shipmentPM: ShipmentPM, oldShipment: ShipmentPM) {
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
    }
    public static MapBookingShipment(shipmentPM: ShipmentPM, bookingShipment: ShipmentPM) {
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
        shipmentPM.ViaColoader = (bookingShipment.IssuingCarrierAgentId == SessionLocator.TenantPM.AgentId) ? false : true;

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
        bookingShipment.ShipmentPackages.forEach(item => {
            var newItem: ShipmentPackagePM = new ShipmentPackagePM(shipmentPM);

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
    }

    public static IsGLSHK() {
        if (SessionLocator.TenantManagementJS.AWBMessagesCCSTypeCode == "GLSHK") {
            return true;
        }

        else {
            return false;
        }
    }
    public static GetTenantZeroAirlineField(shipmentPM: ShipmentPM) {
        var myResult: string = null;

        if (SessionLocator.TenantManagementJS.AWBMessagesCCSTypeCode == "GLSHK") {
            myResult = shipmentPM.TenantZeroAirlinePIMA;
        }

        else {
            myResult = shipmentPM.TenantZeroAirlineTTY;
        }

        return myResult;
    }
    public static OnRegulatedAgentFieldChanged(entityPM: ShipmentPM) {
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
    }
    public static ComputeAWBPrintingRANumber(entityPM: ShipmentPM): string {
        var myResult = null;

        if (entityPM.IsKnownCargo) {
            if (!AppTool.IsNullOrEmpty(entityPM.ColoaderRANumber)) {
                myResult = entityPM.ColoaderRANumber;
            }

            else {
                if (!AppTool.IsNullOrEmpty(entityPM.RegulatedAgentRANumber) && !AppTool.IsNullOrEmpty(entityPM.KnownConsignorNumber)) {
                    myResult = entityPM.RegulatedAgentRANumber;
                }
            }
        }

        return myResult;
    }
    public static ComputeAdditionalHandlingInfo(entityPM: ShipmentPM): string {
        var myCode = null;
        var myResult = null;

        if (AppTool.IsNullOrEmpty(entityPM.KnownConsignorNumber)) {
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
    }
    public static ComputeAWBPrintingSecurityStatus(entityPM: ShipmentPM): string {
        var myResult = null;

        if (entityPM.IsKnownCargo) {
            var myCode = null;

            if (!AppTool.IsNullOrEmpty(entityPM.ColoaderRANumber)) {
                if (!AppTool.IsNullOrEmpty(entityPM.RegulatedAgentRANumber)) {
                    myCode = "SPX";
                }
            }

            else {
                if (!AppTool.IsNullOrEmpty(entityPM.RegulatedAgentRANumber) && !AppTool.IsNullOrEmpty(entityPM.KnownConsignorNumber)) {
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
    }
    public static OnSecurityCodeChanged(entityPM: ShipmentPM) {
        entityPM.AWBSpecialHandlingCodeId9 = entityPM.AWBPrintingSecurityStatusId;
    }
    public static OnAdditionalHandlingInfoChanged(entityPM: ShipmentPM, oldValue: string, newValue: string) {
        if (AppTool.IsNullOrEmpty(entityPM.AWBHandlingInformation)) {
            entityPM.AWBHandlingInformation = newValue;
        }

        else {
            if (!AppTool.IsNullOrEmpty(oldValue)) {
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

            if (AppTool.IsNullOrEmpty(entityPM.AWBHandlingInformation)) {
                entityPM.AWBHandlingInformation = newValue;
            }

            else if (!AppTool.IsNullOrEmpty(newValue)) {
                entityPM.AWBHandlingInformation = newValue + '\n\r' + entityPM.AWBHandlingInformation;
            }
        }
    }
    public static GetRateClassGroupCode(rateClassCode: string): string {
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

            default: { break; }
        }

        return code;
    }
    public static BuildAWBChargesCodeCode(entityPM: ShipmentPM) {
        if (entityPM.FreightPrepaidCollectId == "P" && entityPM.OtherPrepaidCollectId == "P") {
            entityPM.AWBChargesCodeCode = "PP";
        }

        else if (entityPM.FreightPrepaidCollectId == "C" && entityPM.OtherPrepaidCollectId == "C") {
            entityPM.AWBChargesCodeCode = "CC";
        }

        else {
            entityPM.AWBChargesCodeCode = "PC";
        }
    }
    public static ComputeAWBChargeAmount(myRateClassCode: string, myChargeRate: number, myChargeableWeight: number) {
        var myResult: number = null;

        var groupCode = this.GetRateClassGroupCode(myRateClassCode);

        if (groupCode == "M") {
            myResult = myChargeRate;
        }

        else if (groupCode == "R") {
            myResult = myChargeRate * myChargeableWeight;
        }

        return myResult;
    }
    public static ValidateAddedPackagesCount(entityPM: ShipmentPM): string {
        var myResult = "";

        if (!SessionLocator.TenantPM.AllowEAWBMoreThanTenPackages) {
            if (entityPM.IsMultipleCommodities) {
                var allowdCount = 10;

                var myCount1 = entityPM.ShipmentCommodities.length;
                var myCount2 = 0;

                entityPM.ShipmentCommodities.forEach((item) => {
                    if (item.CommodityPackages != null) {
                        myCount2 += item.CommodityPackages.length;
                    }
                })

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
    }
    public static ValidateAddingPackagesCount(currentCount: number): string {
        var myResult = "";

        if (!SessionLocator.TenantPM.AllowEAWBMoreThanTenPackages) {
            var allowdCount = 9;

            if (currentCount >= allowdCount) {
                myResult = "You have reached the limit of " + allowdCount + " lines of packages";
            }
        }

        return myResult;
    }
    public static ValidateAddingCommoditiesCount(entityPM: ShipmentPM): string {
        var myResult = "";

        if (!SessionLocator.TenantPM.AllowEAWBMoreThanTenPackages) {
            var allowdCount = 10;

            var myCount1 = entityPM.ShipmentCommodities.length;
            var myCount2 = 0;

            entityPM.ShipmentCommodities.forEach((item) => {
                if (item.CommodityPackages != null) {
                    myCount2 += item.CommodityPackages.length;
                }
            })

            var totalCount = myCount1 + myCount2;

            if (totalCount >= allowdCount) {
                myResult = "You have reached the limit of " + allowdCount + " lines of commodities and packages";
            }
        }

        return myResult;
    }
    public static IsAirlineRuleFieldValid(myRule: any, myFieldValue: any): boolean {
        var myResult = true;

        if (myRule != null) {

            if (myFieldValue == null || isNaN(myFieldValue)) {
                if (myRule.IsMandatoryForSending) {
                    myResult = false;
                }
            }

            else if (typeof (myFieldValue) == "string") {
                if (AppTool.IsNullOrEmpty(myFieldValue)) {
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
                if (AppTool.IsNullOrZero(myFieldValue)) {
                    if (myRule.IsMandatoryForSending) {
                        myResult = false;
                    }
                }
            }
        }

        return myResult;
    }
    public static IsAdvancedAccountingInformation(entityPM: ShipmentPM): boolean {
        var myResult = false;

        if (!AppTool.IsNullOrEmpty(entityPM.AccountingInformation1)) {
            myResult = true;
        }

        else if (!AppTool.IsNullOrEmpty(entityPM.AccountingInformation2)) {
            myResult = true;
        }

        else if (!AppTool.IsNullOrEmpty(entityPM.AccountingInformation3)) {
            myResult = true;
        }

        else if (!AppTool.IsNullOrEmpty(entityPM.AccountingInformation4)) {
            myResult = true;
        }

        else if (!AppTool.IsNullOrEmpty(entityPM.AccountingInformation5)) {
            myResult = true;
        }

        else if (!AppTool.IsNullOrEmpty(entityPM.AccountingInformation6)) {
            myResult = true;
        }

        return myResult;
    }
    public static IsParticipant1Filled(entityPM: ShipmentPM): boolean {
        var myResult = false;

        if (!AppTool.IsNullOrEmpty(entityPM.OtherParticipantIdCode1)) {
            myResult = true;
        }

        else if (!AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationCode1)) {
            myResult = true;
        }

        else if (!AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationPortCode1)) {
            myResult = true;
        }

        else if (!AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationName1)) {
            myResult = true;
        }

        else if (!AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationReference1)) {
            myResult = true;
        }

        return myResult;
    }
    public static IsParticipant2Filled(entityPM: ShipmentPM): boolean {
        var myResult = false;

        if (!AppTool.IsNullOrEmpty(entityPM.OtherParticipantIdCode2)) {
            myResult = true;
        }

        else if (!AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationCode2)) {
            myResult = true;
        }

        else if (!AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationPortCode2)) {
            myResult = true;
        }

        else if (!AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationName2)) {
            myResult = true;
        }

        else if (!AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationReference2)) {
            myResult = true;
        }

        return myResult;
    }
    public static IsParticipant3Filled(entityPM: ShipmentPM): boolean {
        var myResult = false;

        if (!AppTool.IsNullOrEmpty(entityPM.OtherParticipantIdCode3)) {
            myResult = true;
        }

        else if (!AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationCode3)) {
            myResult = true;
        }

        else if (!AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationPortCode3)) {
            myResult = true;
        }

        else if (!AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationName3)) {
            myResult = true;
        }

        else if (!AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationReference3)) {
            myResult = true;
        }

        return myResult;
    }
    public static IsParticipant1MissingData(entityPM: ShipmentPM): boolean {
        var myResult = false;

        if (AppTool.IsNullOrEmpty(entityPM.OtherParticipantIdCode1)) {
            myResult = true;
        }

        else if (AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationCode1)) {
            myResult = true;
        }

        else if (AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationPortCode1)) {
            myResult = true;
        }

        else if (AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationName1)) {
            myResult = true;
        }

        else if (AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationReference1)) {
            myResult = true;
        }

        return myResult;
    }
    public static IsParticipant2MissingData(entityPM: ShipmentPM): boolean {
        var myResult = false;

        if (AppTool.IsNullOrEmpty(entityPM.OtherParticipantIdCode2)) {
            myResult = true;
        }

        else if (AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationCode2)) {
            myResult = true;
        }

        else if (AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationPortCode2)) {
            myResult = true;
        }

        else if (AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationName2)) {
            myResult = true;
        }

        else if (AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationReference2)) {
            myResult = true;
        }

        return myResult;
    }
    public static IsParticipant3MissingData(entityPM: ShipmentPM): boolean {
        var myResult = false;

        if (AppTool.IsNullOrEmpty(entityPM.OtherParticipantIdCode3)) {
            myResult = true;
        }

        else if (AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationCode3)) {
            myResult = true;
        }

        else if (AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationPortCode3)) {
            myResult = true;
        }

        else if (AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationName3)) {
            myResult = true;
        }

        else if (AppTool.IsNullOrEmpty(entityPM.OtherParticipantInformationReference3)) {
            myResult = true;
        }

        return myResult;
    }
    public static GetFromPortTextCode(TransportModeId: string, levelCode: string) {
        var myResult: string = null;

        if (levelCode == "C") {
            switch (TransportModeId) {
                case "A": { myResult = "Master.S.NewMaster.Gateway"; break; }
                case "O": { myResult = "Master.S.NewMaster.LoadingPort"; break; }
                case "I": { myResult = "Master.S.NewMaster.From"; break; }
                default: { myResult = "Master.S.NewMaster.From"; break; }
            }
        }

        else {
            switch (TransportModeId) {
                case "A": { myResult = "Shipment.S.NewShipment.Gateway"; break; }
                case "O": { myResult = "Shipment.S.NewShipment.LoadingPort"; break; }
                case "I": { myResult = "Shipment.S.NewShipment.From"; break; }
                default: { myResult = "Shipment.S.NewShipment.From"; break; }
            }
        }

        return myResult;
    }
    public static GetToPortTextCode(TransportModeId: string, levelCode: string) {
        var myResult: string = null;

        if (levelCode == "C") {
            switch (TransportModeId) {
                case "A": { myResult = "Master.S.NewMaster.Destination"; break; }
                case "O": { myResult = "Master.S.NewMaster.DischargePort"; break; }
                case "I": { myResult = "Master.S.NewMaster.To"; break; }
                default: { myResult = "Master.S.NewMaster.To"; break; }
            }
        }

        else {
            switch (TransportModeId) {
                case "A": { myResult = "Shipment.S.NewShipment.Destination"; break; }
                case "O": { myResult = "Shipment.S.NewShipment.DischargePort"; break; }
                case "I": { myResult = "Shipment.S.NewShipment.To"; break; }
                default: { myResult = "Shipment.S.NewShipment.To"; break; }
            }
        }

        return myResult;
    }
    public static SetPayableUnitPriceBySteps(entityPM: ShipmentPayablePM, myBaseQuote: QuotePM) {
        if (myBaseQuote != null) {
            var myCharge: QuoteChargePM = myBaseQuote.QuoteCharges.filter(d => d.Id == entityPM.QuoteChargeId)[0];
            if (myCharge != null) {
                var mySteps: QuotePriceStepsPM[] = myCharge.QuoteChargePriceSteps;

                if (mySteps.length > 0) {
                    var price: number = null;

                    mySteps.sort((a, b) => { return a.Step - b.Step }).forEach((itemPriceStep) => {
                        if (!AppTool.IsNullOrEmpty(entityPM.Quantity) && entityPM.Quantity >= itemPriceStep.Step) {
                            price = itemPriceStep.CostUnitPrice;
                        }
                    });

                    var entitySmallest: QuotePriceStepsPM = mySteps.sort((a, b) => { return a.Step - b.Step })[0];
                    if (entitySmallest != null) {
                        if (AppTool.IsNullOrEmpty(entityPM.Quantity) || entityPM.Quantity < entitySmallest.Step) {
                            price = entitySmallest.CostUnitPrice;
                        }
                    }

                    entityPM.UnitPrice = AppTool.Round(price, 3);
                }
            }
        }
    }
    public static SetReceivableUnitPriceBySteps(entityPM: ShipmentReceivablePM, myBaseQuote: QuotePM) {
        if (myBaseQuote != null) {
            var myCharge: QuoteChargePM = myBaseQuote.QuoteCharges.filter(d => d.Id == entityPM.QuoteChargeId)[0];
            if (myCharge != null) {
                var mySteps: QuotePriceStepsPM[] = myCharge.QuoteChargePriceSteps;

                if (mySteps.length > 0) {
                    var price: number = null;

                    mySteps.sort((a, b) => { return a.Step - b.Step }).forEach((itemPriceStep) => {
                        if (!AppTool.IsNullOrEmpty(entityPM.Quantity) && entityPM.Quantity >= itemPriceStep.Step) {
                            price = itemPriceStep.SaleUnitPrice;
                        }
                    });

                    var entitySmallest: QuotePriceStepsPM = mySteps.sort((a, b) => { return a.Step - b.Step })[0];
                    if (entitySmallest != null) {
                        if (AppTool.IsNullOrEmpty(entityPM.Quantity) || entityPM.Quantity < entitySmallest.Step) {
                            price = entitySmallest.SaleUnitPrice;
                        }
                    }

                    entityPM.UnitPrice = AppTool.Round(price, 3);
                }
            }
        }
    }
    public static SetReceivableLineStatus(entityPM: ShipmentReceivablePM) {
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
    }
    public static SetPayableLineStatus(entityPM: ShipmentPayablePM) {
        if (entityPM.IsDirty) {

            var StatusCode: string = "EMPT";

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
    }
    public static GetByPckageTypeGrouped(entityPM: ShipmentPM) {
        var myResult: ByPckageType[] = [];

        if (entityPM.ShipmentPackages.length > 0) {
            var myService = new PackageTypeListService();

            entityPM.ShipmentPackages.filter(f => f.IsContainer == true).forEach(item => {
                var itemGrouped = myResult.filter(f => f.PackageTypeId == item.PackageTypeId)[0];
                if (itemGrouped == null) {
                    itemGrouped = new ByPckageType();
                    itemGrouped.PackageTypeId = item.PackageTypeId;
                    itemGrouped.Quantity = item.Quantity;

                    if (AppTool.IsNullOrEmpty(itemGrouped.Quantity)) {
                        itemGrouped.Quantity = 0;
                    }

                    if (item.PackageTypeId) {
                        myService.getSingleFromCache(item.PackageTypeId).subscribe((myResponse: any) => {
                            if (!myResponse.HasError) {
                                var list: PackageTypeList = myResponse.Result;
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
                    if (!AppTool.IsNullOrEmpty(item.Quantity)) {
                        itemGrouped.Quantity += item.Quantity;
                    }
                }
            });
        }

        return myResult;
    }
    public static ComputeTotals(entityPM: ShipmentPM) {

        // Payables
        var openPayablesLocal: number = null;
        var acctPayablesLocal: number = null;
        var openPayablesProfit: number = null;
        var acctPayablesProfit: number = null;
        if (entityPM.ShipmentLevelCode == "C" && entityPM.ShipmentConsoleShipments.length > 0) {
            openPayablesLocal = ArrayTool.Sum(entityPM.ShipmentConsoleShipments, "OAMTPayables_Local");
            acctPayablesLocal = ArrayTool.Sum(entityPM.ShipmentConsoleShipments, "ACCTPayables_Local");
            openPayablesProfit = ArrayTool.Sum(entityPM.ShipmentConsoleShipments, "OAMTPayables_Profit");
            acctPayablesProfit = ArrayTool.Sum(entityPM.ShipmentConsoleShipments, "ACCTPayables_Profit");
        }

        else {
            openPayablesLocal = ArrayTool.Sum(entityPM.ShipmentPayables, "OpenAmountInLocalCurrency");
            acctPayablesLocal = ArrayTool.Sum(entityPM.ShipmentPayables, "AccountedAmountInLocalCurrency");
            openPayablesProfit = ArrayTool.Sum(entityPM.ShipmentPayables, "OpenAmountInProfitCurrency");
            acctPayablesProfit = ArrayTool.Sum(entityPM.ShipmentPayables, "AccountedAmountInProfitCurrency");
        }

        // Receivables
        var openReceivablesLocal: number = null;
        var acctReceivablesLocal: number = null;
        var openReceivablesProfit: number = null;
        var acctReceivablesProfit: number = null;
        if (entityPM.ShipmentLevelCode == "C" && entityPM.ShipmentConsoleShipments.length > 0) {

            if (entityPM.ProrateReceivables) {
                openReceivablesLocal = ArrayTool.Sum(entityPM.ShipmentConsoleShipments, "OAMTReceivables_Local");
                acctReceivablesLocal = ArrayTool.Sum(entityPM.ShipmentConsoleShipments, "ACCTReceivables_Local");
                openReceivablesProfit = ArrayTool.Sum(entityPM.ShipmentConsoleShipments, "OAMTReceivables_Profit");
                acctReceivablesProfit = ArrayTool.Sum(entityPM.ShipmentConsoleShipments, "ACCTReceivables_Profit");
            }

            else {
                //openReceivablesLocal = ArrayTool.Sum(entityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "EMPT" || f.ShipmentReceivableLineStatusCode == "OAMT"), "TotalAmountLocal");
                //acctReceivablesLocal = ArrayTool.Sum(entityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "DRFT" || f.ShipmentReceivableLineStatusCode == "ACCT"), "TotalAmountLocal");
                //openReceivablesProfit = ArrayTool.Sum(entityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "EMPT" || f.ShipmentReceivableLineStatusCode == "OAMT"), "AmountInProfitCurrency");
                //acctReceivablesProfit = ArrayTool.Sum(entityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "DRFT" || f.ShipmentReceivableLineStatusCode == "ACCT"), "AmountInProfitCurrency");

                openReceivablesLocal = ArrayTool.Sum(entityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode != "ACCT"), "TotalAmountLocal");
                acctReceivablesLocal = ArrayTool.Sum(entityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "ACCT"), "TotalAmountLocal");
                openReceivablesProfit = ArrayTool.Sum(entityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode != "ACCT"), "AmountInProfitCurrency");
                acctReceivablesProfit = ArrayTool.Sum(entityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "ACCT"), "AmountInProfitCurrency");


                // need to add houses receivables that are not connected to parent receivable
                openReceivablesLocal += ArrayTool.Sum(entityPM.ShipmentConsoleShipments, "OAMTReceivables_Local_NoParent");
                acctReceivablesLocal += ArrayTool.Sum(entityPM.ShipmentConsoleShipments, "ACCTReceivables_Local_NoParent");
                openReceivablesProfit += ArrayTool.Sum(entityPM.ShipmentConsoleShipments, "OAMTReceivables_Profit_NoParent");
                acctReceivablesProfit += ArrayTool.Sum(entityPM.ShipmentConsoleShipments, "ACCTReceivables_Profit_NoParent");
            }
        }

        else {
            //openReceivablesLocal = ArrayTool.Sum(entityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "EMPT" || f.ShipmentReceivableLineStatusCode == "OAMT"), "TotalAmountLocal");
            //acctReceivablesLocal = ArrayTool.Sum(entityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "DRFT" || f.ShipmentReceivableLineStatusCode == "ACCT"), "TotalAmountLocal");
            //openReceivablesProfit = ArrayTool.Sum(entityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "EMPT" || f.ShipmentReceivableLineStatusCode == "OAMT"), "AmountInProfitCurrency");
            //acctReceivablesProfit = ArrayTool.Sum(entityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "DRFT" || f.ShipmentReceivableLineStatusCode == "ACCT"), "AmountInProfitCurrency");

            openReceivablesLocal = ArrayTool.Sum(entityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode != "ACCT"), "TotalAmountLocal");
            acctReceivablesLocal = ArrayTool.Sum(entityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "ACCT"), "TotalAmountLocal");
            openReceivablesProfit = ArrayTool.Sum(entityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode != "ACCT"), "AmountInProfitCurrency");
            acctReceivablesProfit = ArrayTool.Sum(entityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "ACCT"), "AmountInProfitCurrency");
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
            entityPM.OpenPayablesInLocalCurrency = (openPayablesLocal == null) ? 0 : AppTool.Round(openPayablesLocal, 2);
        }

        if (entityPM.OpenPayablesInProfitCurrency != openPayablesProfit) {
            entityPM.OpenPayablesInProfitCurrency = (openPayablesProfit == null) ? 0 : AppTool.Round(openPayablesProfit, 2);
        }

        if (entityPM.AccountedPayablesInLocalCurrency != acctPayablesLocal) {
            entityPM.AccountedPayablesInLocalCurrency = (acctPayablesLocal == null) ? 0 : AppTool.Round(acctPayablesLocal, 2);
        }

        if (entityPM.AccountedPayablesInProfitCurrency != acctPayablesProfit) {
            entityPM.AccountedPayablesInProfitCurrency = (acctPayablesProfit == null) ? 0 : AppTool.Round(acctPayablesProfit, 2);
        }

        /* Receivables */
        if (entityPM.OpenReceivablesInLocalCurrency != openReceivablesLocal) {
            entityPM.OpenReceivablesInLocalCurrency = (openReceivablesLocal == null) ? 0 : AppTool.Round(openReceivablesLocal, 2);
        }

        if (entityPM.OpenReceivablesInProfitCurrency != openReceivablesProfit) {
            entityPM.OpenReceivablesInProfitCurrency = (openReceivablesProfit == null) ? 0 : AppTool.Round(openReceivablesProfit, 2);
        }

        if (entityPM.AccountedReceivablesInLocalCurrency != acctReceivablesLocal) {
            entityPM.AccountedReceivablesInLocalCurrency = (acctReceivablesLocal == null) ? 0 : AppTool.Round(acctReceivablesLocal, 2);
        }

        if (entityPM.AccountedReceivablesInProfitCurrency != acctReceivablesProfit) {
            entityPM.AccountedReceivablesInProfitCurrency = (acctReceivablesProfit == null) ? 0 : AppTool.Round(acctReceivablesProfit, 2);
        }

        /* Profit */
        if (entityPM.ProfitInLocalCurrency != profitInLocal) {
            entityPM.ProfitInLocalCurrency = (profitInLocal == null) ? 0 : AppTool.Round(profitInLocal, 2);
        }

        if (entityPM.ProfitInProfitCurrency != profitInProfit) {
            entityPM.ProfitInProfitCurrency = (profitInProfit == null) ? 0 : AppTool.Round(profitInProfit, 2);
        }
    }
    public static RecalculateShipmentFields(shipmentPM: ShipmentPM) {

        if (shipmentPM != null) {

            if (shipmentPM.Ratio == null) {
                shipmentPM.Ratio = AppTool.GetRatio(shipmentPM.DirectionId, shipmentPM.TransportModeId, shipmentPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
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

                var myQuantity: number = 0;
                var myVolume: number = 0;
                var myGrossWeight: number = 0;
                var myVolumetricWeight: number = 0;

                shipmentPM.ShipmentPackages.forEach((item) => {

                    item.InsideShipmentPackages.forEach((insideItem) => {
                        insideItem.Volume = AppTool.ComputePackageVolume(insideItem.Quantity, insideItem.Width, insideItem.Height, insideItem.Length, insideItem.Weight, shipmentPM.Ratio, shipmentPM.DimensionsUnitCode, shipmentPM.VolumeUnitCode, shipmentPM.GrossWeightUnitCode);
                        insideItem.VolumetricWeight = AppTool.ComputePackageVolumetricWeight(insideItem.Quantity, insideItem.Width, insideItem.Height, insideItem.Length, insideItem.Volume, insideItem.Weight, shipmentPM.Ratio, shipmentPM.DimensionsUnitCode, shipmentPM.VolumeUnitCode, shipmentPM.GrossWeightUnitCode, shipmentPM.ChargeableWeightUnitCode);
                    })

                    item.Volume = AppTool.ComputePackageVolume(item.Quantity, item.Width, item.Height, item.Length, item.Weight, shipmentPM.Ratio, shipmentPM.DimensionsUnitCode, shipmentPM.VolumeUnitCode, shipmentPM.GrossWeightUnitCode);
                    item.VolumetricWeight = AppTool.ComputePackageVolumetricWeight(item.Quantity, item.Width, item.Height, item.Length, item.Volume, item.Weight, shipmentPM.Ratio, shipmentPM.DimensionsUnitCode, shipmentPM.VolumeUnitCode, shipmentPM.GrossWeightUnitCode, shipmentPM.ChargeableWeightUnitCode);

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
                })

                shipmentPM.NumberOfPackages = myQuantity;
                shipmentPM.Volume = AppTool.Round(myVolume, 3);
                shipmentPM.VolumetricWeight = AppTool.Round(myVolumetricWeight, 3);

                if (!shipmentPM.GrossWeightEdited) {
                    shipmentPM.GrossWeight = AppTool.Round(myGrossWeight, 3);
                }

                if (!shipmentPM.ChargeableWeightEdited) {
                    shipmentPM.ChargeableWeight = AppTool.CalculateChargeableWeight(shipmentPM.GrossWeight, shipmentPM.VolumetricWeight, shipmentPM.GrossWeightUnitCode, shipmentPM.ChargeableWeightUnitCode, shipmentPM.DirectionId, shipmentPM.TransportModeId);
                }
            }

            var orderVolumetricWeight: number = null;

            if (shipmentPM.BookingVolume != null) {
                orderVolumetricWeight = AppTool.GetWeightFromVolume(shipmentPM.VolumeUnitCode, shipmentPM.ChargeableWeightUnitCode, shipmentPM.BookingVolume, shipmentPM.Ratio);
            }

            else if (shipmentPM.OrderGrossWeight != null) {
                orderVolumetricWeight = AppTool.GetWeightFromWeight(shipmentPM.GrossWeightUnitCode, shipmentPM.ChargeableWeightUnitCode, shipmentPM.OrderGrossWeight);
            }

            shipmentPM.OrderVolumetricWeight = AppTool.Round(orderVolumetricWeight, 3);
            shipmentPM.OrderChargeableWeight = AppTool.CalculateChargeableWeight(shipmentPM.OrderGrossWeight, shipmentPM.OrderVolumetricWeight, shipmentPM.GrossWeightUnitCode, shipmentPM.ChargeableWeightUnitCode, shipmentPM.DirectionId, shipmentPM.TransportModeId);
        }
    }
    public static OnShipmentRatioChanged(shipmentPM: ShipmentPM) {
        if (shipmentPM) {
            if (shipmentPM.Ratio == null) {
                shipmentPM.Ratio = AppTool.GetRatio(shipmentPM.DirectionId, shipmentPM.TransportModeId, shipmentPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
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
                shipmentPM.ShipmentPackages.forEach((item) => {

                    item.InsideShipmentPackages.forEach((insideItem) => {
                        if (insideItem.Volume) {
                            insideItem.VolumetricWeight = AppTool.GetWeightFromVolume(shipmentPM.VolumeUnitCode, shipmentPM.ChargeableWeightUnitCode, insideItem.Volume, shipmentPM.Ratio);
                        }
                    })

                    if (item.Volume) {
                        item.VolumetricWeight = AppTool.GetWeightFromVolume(shipmentPM.VolumeUnitCode, shipmentPM.ChargeableWeightUnitCode, item.Volume, shipmentPM.Ratio);
                    }
                });

                shipmentPM.VolumetricWeight = AppTool.Round(ArrayTool.Sum(shipmentPM.ShipmentPackages, "VolumetricWeight"), 3);

                if (!shipmentPM.ChargeableWeightEdited) {
                    shipmentPM.ChargeableWeight = AppTool.CalculateChargeableWeight(shipmentPM.GrossWeight, shipmentPM.VolumetricWeight, shipmentPM.GrossWeightUnitCode, shipmentPM.ChargeableWeightUnitCode, shipmentPM.DirectionId, shipmentPM.TransportModeId);
                }
            }

            // ShipmentOrderPackages
            if (shipmentPM.ShipmentOrderPackages.length > 0) {
                shipmentPM.ShipmentOrderPackages.forEach((item) => {
                    if (item.Volume) {
                        item.VolumetricWeight = AppTool.GetWeightFromVolume(shipmentPM.VolumeUnitCode, shipmentPM.ChargeableWeightUnitCode, item.Volume, shipmentPM.Ratio);
                    }
                });

                shipmentPM.OrderVolumetricWeight = AppTool.Round(ArrayTool.Sum(shipmentPM.ShipmentOrderPackages, "VolumetricWeight"), 3);
                shipmentPM.OrderChargeableWeight = AppTool.CalculateChargeableWeight(shipmentPM.OrderGrossWeight, shipmentPM.OrderVolumetricWeight, shipmentPM.GrossWeightUnitCode, shipmentPM.ChargeableWeightUnitCode, shipmentPM.DirectionId, shipmentPM.TransportModeId);
            }

            else {                
                if (shipmentPM.BookingVolume) {
                    var orderVolumetricWeight = AppTool.GetWeightFromVolume(shipmentPM.VolumeUnitCode, shipmentPM.ChargeableWeightUnitCode, shipmentPM.BookingVolume, shipmentPM.Ratio);

                    shipmentPM.OrderVolumetricWeight = AppTool.Round(orderVolumetricWeight, 3);
                    shipmentPM.OrderChargeableWeight = AppTool.CalculateChargeableWeight(shipmentPM.OrderGrossWeight, shipmentPM.OrderVolumetricWeight, shipmentPM.GrossWeightUnitCode, shipmentPM.ChargeableWeightUnitCode, shipmentPM.DirectionId, shipmentPM.TransportModeId);
                }
            }
        }
    }
    public static GetNewShipmentPM() {
        var todayDate: Date = DateTool.GetCurrentDateTimeAsUtc();

        var entityPM: ShipmentPM = new ShipmentPM();

        entityPM.FHLStatusCode = "NSEN";
        entityPM.FWBStatusCode = "NSEN";
        entityPM.FHLStatusName = "Not Sent";
        entityPM.FWBStatusName = "Not Sent";
        entityPM.ManifestStatusCode = "NSEN";
        entityPM.IsOperationalClosed = false;
        entityPM.CreateDateTime = todayDate;
        entityPM.LastUpdateDate = todayDate;
        entityPM.StatusDate = todayDate;
        entityPM.Tenant = SessionLocator.TenantPM.Id;
        entityPM.AWBCurrencyId = SessionLocator.TenantPM.FreightCurrencyId;
        entityPM.ProfitCurrencyId = SessionLocator.TenantPM.ProfitCurrencyId;
        entityPM.VolumeUnitCode = SessionLocator.TenantPM.VolumeUnitCode;
        entityPM.DimensionsUnitCode = SessionLocator.TenantPM.DimensionsUnitCode;
        entityPM.GrossWeightUnitCode = SessionLocator.TenantPM.GrossWeightUnitCode;
        entityPM.ChargeableWeightUnitCode = SessionLocator.TenantPM.ChargeableWeightUnitCode;
        entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        entityPM.BranchId = SessionLocator.LoggedUserPM.BranchId;
        entityPM.DepartmentId = SessionLocator.LoggedUserPM.DepartmentId;
        entityPM.NewConcurrencyGUID = AppTool.GetNewGuid();

        return entityPM;
    }
    public static OnShipmentQuantitiesChanged(entityPM: ShipmentPM) {
        if (entityPM) {
            var isLCL = this.IsLCL(entityPM);
            if (isLCL) {

                entityPM.ShipmentPayables.forEach(itemPayable => {
                    if (AppTool.IsNullOrEmpty(itemPayable.UnitPrice)) {
                        if (AppTool.IsNullOrEmpty(itemPayable.ShipmentPayableParentId)) {
                            if (itemPayable.ShipmentPayableAmountTypeCode != "NEXP" && itemPayable.ShipmentPayableLineStatusCode != "ACCT" && itemPayable.ShipmentPayableLineStatusCode != "PACC") {

                                var myQuantity: number = null;
                                switch (itemPayable.MeasurementCode) {
                                    case "GRWT": { myQuantity = entityPM.GrossWeight; break; }
                                    case "CHWT": { myQuantity = entityPM.ChargeableWeight; break; }
                                    case "VOLU": { myQuantity = entityPM.Volume; break; }
                                    case "BTEU": { myQuantity = entityPM.TEU; break; }
                                    case "FIXD": { myQuantity = 1; break; }
                                    case "GWTN": { myQuantity = entityPM.GrossWeightPerTon; break; }
                                    case "PRVL": { myQuantity = entityPM.ValueOfGoods; break; }
                                    case "PRFR": { myQuantity = ArrayTool.Sum(entityPM.ShipmentPayables.filter(d => d.ChargesGroupCode == "FRT" && AppTool.IsNullOrEmpty(d.ShipmentPayableParentId)), "ExpectedAmount"); break; }
                                    case "QTY": { myQuantity = entityPM.NumberOfPackages; break; }
                                    default: { break; }
                                }

                                if (itemPayable.Quantity != myQuantity) {
                                    itemPayable.Quantity = AppTool.Round(myQuantity, 2);

                                    if (itemPayable.IsChargeBySteps) {
                                        //this.SetPayableUnitPriceBySteps(itemPayable, this.fatherComponent.BaseQuote);
                                    }
                                }
                            }
                        }
                    }
                });

                entityPM.ShipmentReceivables.forEach(itemReceivable => {
                    if (AppTool.IsNullOrEmpty(itemReceivable.UnitPrice)) {
                        if (AppTool.IsNullOrEmpty(itemReceivable.ShipmentReceivableParentId)) {
                            if (AppTool.IsNullOrEmpty(itemReceivable.ARInvoiceId)) {

                                var myQuantity: number = null;
                                switch (itemReceivable.MeasurementCode) {
                                    case "GRWT": { myQuantity = entityPM.GrossWeight; break; }
                                    case "CHWT": { myQuantity = entityPM.ChargeableWeight; break; }
                                    case "VOLU": { myQuantity = entityPM.Volume; break; }
                                    case "BTEU": { myQuantity = entityPM.TEU; break; }
                                    case "FIXD": { myQuantity = 1; break; }
                                    case "GWTN": { myQuantity = entityPM.GrossWeightPerTon; break; }
                                    case "PRVL": { myQuantity = entityPM.ValueOfGoods; break; }
                                    case "PRFR": { myQuantity = ArrayTool.Sum(entityPM.ShipmentReceivables.filter(d => d.ChargesGroupCode == "FRT" && AppTool.IsNullOrEmpty(d.ShipmentReceivableParentId)), "TotalAmount"); break; }
                                    case "QTY": { myQuantity = entityPM.NumberOfPackages; break; }
                                    default: { break; }
                                }

                                if (itemReceivable.Quantity != myQuantity) {
                                    itemReceivable.Quantity = AppTool.Round(myQuantity, 2);

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
    }

}
export class ByPckageType {
    public Quantity: number;
    public PackageTypeId: string;
    public MeasurementId: string;
    public MeasurementCode: string;
    public MeasurementShortName: string;   
}
export class ShipmentGenerator {
    private EntityPM: ShipmentPM = null;
    private OriginShipment: ShipmentPM = null;
    private BaseQuote: QuotePM = null;
    private IsAdhoc: boolean = false;
    private IsRoutingRate: boolean = false;
    private AllRates: LastRate[] = [];
    private IsLCLEntity: boolean = false;
    private BCNTGrouped: ByPckageType[] = [];
    private myCurrencyListService: CurrencyListService;
    private myChargesTypeListService: ChargesTypeListService;
    constructor(entityPM: ShipmentPM, allRates: LastRate[]) {
        this.EntityPM = entityPM;
        this.AllRates = allRates;
        this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
        this.myCurrencyListService = new CurrencyListService();
        this.myChargesTypeListService = new ChargesTypeListService();
        this.BCNTGrouped = ShipmentTool.GetByPckageTypeGrouped(this.EntityPM);

        if (this.AllRates == null) {
            this.AllRates = [];
        }
    }

    // Payables
    public GeneratePayablesAutoDisplay() {
        this.myChargesTypeListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var allChargesTypes: ChargesTypeList[] = myResponse.Result;
                if (allChargesTypes != null) {
                    allChargesTypes = allChargesTypes.filter(d => d.IsPayable == true && d.InActive == false);

                    if (this.EntityPM.ShipmentLevelCode == "C") {
                        allChargesTypes = allChargesTypes.filter(d => d.IsAutoDisplayInConsolidation);
                    }

                    else {
                        if (this.EntityPM.IncludesCustoms) {
                            allChargesTypes = allChargesTypes.filter(d => d.IsAutoDisplayInShipment == true || d.IsAutoDisplayInCustoms);
                        }

                        else {
                            allChargesTypes = allChargesTypes.filter(d => d.IsAutoDisplayInShipment == true);
                        }
                    }

                    switch (this.EntityPM.TransportModeId) {
                        case "A":
                            {
                                allChargesTypes = allChargesTypes.filter(r => r.IsAir);
                                break;
                            }

                        case "O":
                            {
                                allChargesTypes = allChargesTypes.filter(r => r.IsOcean);
                                break;
                            }

                        case "I":
                            {
                                allChargesTypes = allChargesTypes.filter(r => r.IsInland);
                                break;
                            }
                    }

                    switch (this.EntityPM.DirectionId) {
                        case "E":
                            {
                                allChargesTypes = allChargesTypes.filter(r => r.IsExport);
                                break;
                            }

                        case "I":
                            {
                                allChargesTypes = allChargesTypes.filter(r => r.IsImport);
                                break;
                            }

                        case "D":
                            {
                                allChargesTypes = allChargesTypes.filter(r => r.IsDomestic);
                                break;
                            }

                        case "R":
                            {
                                allChargesTypes = allChargesTypes.filter(r => r.IsDrop);
                                break;
                            }
                    }

                    if (this.IsLCLEntity) {
                        allChargesTypes.sort((a, b) => { return a.ViewOrder - b.ViewOrder }).forEach((item) => {
                            var newRecord: ShipmentPayablePM = new ShipmentPayablePM(this.EntityPM);
                            newRecord.Tenant = SessionLocator.Tenant;
                            newRecord.ShipmentId = this.EntityPM.Id;
                            newRecord.ShipmentNumber = this.EntityPM.ShipmentNumber;
                            newRecord.ShipmentPayableLineStatusCode = "EMPT";
                            newRecord.ShipmentPayableAmountTypeCode = "ACCU";
                            newRecord.ShipmentPayableAmountTypeName = "Accrual";
                            newRecord.CreatedByUserId = SessionLocator.LoggedUserId;
                            newRecord.UpdateByUserId = SessionLocator.LoggedUserId;
                            newRecord.CreateDate = DateTool.GetCurrentDateAsUtc();
                            newRecord.UpdateDate = DateTool.GetCurrentDateAsUtc();
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
                                case "GRWT": { newRecord.Quantity = this.EntityPM.GrossWeight; break; }
                                case "CHWT": { newRecord.Quantity = this.EntityPM.ChargeableWeight; break; }
                                case "VOLU": { newRecord.Quantity = this.EntityPM.Volume; break; }
                                case "BTEU": { newRecord.Quantity = this.EntityPM.TEU; break; }
                                case "FIXD": { newRecord.Quantity = 1; break; }
                                case "PRVL": { newRecord.Quantity = this.EntityPM.ValueOfGoods; break; }
                                case "GWTN": { newRecord.Quantity = this.EntityPM.GrossWeightPerTon; break; }
                                case "QTY": { newRecord.Quantity = this.EntityPM.NumberOfPackages; break; }
                                default: { break; }
                            }

                            if (item.ChargesGroupCode == "FRT" || item.ChargesGroupCode == "SCH") {
                                newRecord.CurrencyId = SessionLocator.TenantPM.FreightCurrencyId;
                            }

                            else {
                                newRecord.CurrencyId = SessionLocator.TenantPM.OtherChargesCurrencyId;
                            }

                            this.GetCurrencyCode(newRecord);
                            newRecord.Rate = this.GetCurrencyRate(newRecord.CurrencyId);
                            newRecord.ProfitCurrencyExchangeRate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);
                            this.EntityPM.AddPayable(newRecord);
                        });

                        this.EntityPM.ShipmentPayables.filter(f => f.MeasurementCode == "PRFR").forEach(item => {
                            item.Quantity = ArrayTool.Sum(this.EntityPM.ShipmentPayables.filter(d => d.ChargesGroupCode == "FRT" && AppTool.IsNullOrEmpty(d.ShipmentPayableParentId)), "ExpectedAmount");
                        });
                    }

                    else {
                        if (this.BCNTGrouped.length > 0) {
                            allChargesTypes.filter(f => f.ContainerMeasurementCode == "BCNT").sort((a, b) => { return a.ViewOrder - b.ViewOrder }).forEach(myChargeType => {
                                this.BCNTGrouped.forEach(itemGrouped => {
                                    var newRecord: ShipmentPayablePM = new ShipmentPayablePM(this.EntityPM);
                                    newRecord.Tenant = SessionLocator.Tenant;
                                    newRecord.ShipmentId = this.EntityPM.Id;
                                    newRecord.ShipmentNumber = this.EntityPM.ShipmentNumber;
                                    newRecord.ShipmentPayableLineStatusCode = "EMPT";
                                    newRecord.ShipmentPayableAmountTypeCode = "ACCU";
                                    newRecord.ShipmentPayableAmountTypeName = "Accrual";
                                    newRecord.CreatedByUserId = SessionLocator.LoggedUserId;
                                    newRecord.UpdateByUserId = SessionLocator.LoggedUserId;
                                    newRecord.CreateDate = DateTool.GetCurrentDateAsUtc();
                                    newRecord.UpdateDate = DateTool.GetCurrentDateAsUtc();
                                    newRecord.ChargesTypeId = myChargeType.Id;
                                    newRecord.ChargesTypeCode = myChargeType.Code;
                                    newRecord.ChargesTypeName = myChargeType.EnglishName;
                                    newRecord.VatTypeId = myChargeType.VatTypeId;
                                    newRecord.ChargesGroupCode = myChargeType.ChargesGroupCode;
                                    newRecord.DueTypeCode = myChargeType.DueTypeCode;
                                    newRecord.DueTypeName = myChargeType.DueTypeName;
                                    newRecord.IATACodeId = myChargeType.IATACodeId;
                                    newRecord.ViewOrder = myChargeType.ViewOrder;
                                    newRecord.PrepaidCollectId = myChargeType.ChargesGroupCode == "FRT" ? this.EntityPM.FreightPrepaidCollectId : this.EntityPM.OtherPrepaidCollectId;
                                    newRecord.Quantity = itemGrouped.Quantity;
                                    newRecord.MeasurementId = itemGrouped.MeasurementId;
                                    newRecord.MeasurementCode = itemGrouped.MeasurementCode;
                                    newRecord.MeasurementShortName = itemGrouped.MeasurementShortName;
                                    newRecord.IsBackToBack = myChargeType.IsBackToBack;
                                    if (myChargeType.ChargesGroupCode == "FRT" || myChargeType.ChargesGroupCode == "SCH") {
                                        newRecord.CurrencyId = SessionLocator.TenantPM.FreightCurrencyId;
                                    }

                                    else {
                                        newRecord.CurrencyId = SessionLocator.TenantPM.OtherChargesCurrencyId;
                                    }

                                    this.GetCurrencyCode(newRecord);
                                    newRecord.Rate = this.GetCurrencyRate(newRecord.CurrencyId);
                                    newRecord.ProfitCurrencyExchangeRate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);

                                    var existsPayable: ShipmentPayablePM = this.EntityPM.ShipmentPayables.filter(d => d.ChargesTypeId == newRecord.ChargesTypeId && d.MeasurementId == newRecord.MeasurementId)[0];
                                    if (existsPayable == null) {
                                        this.EntityPM.AddPayable(newRecord);
                                    }

                                    else {
                                        var acctPayable: ShipmentPayablePM = this.EntityPM.ShipmentPayables.filter(d => d.ChargesTypeId == newRecord.ChargesTypeId && d.MeasurementId == newRecord.MeasurementId && (d.ShipmentPayableLineStatusCode == "ACCT" || d.ShipmentPayableLineStatusCode == "PACC"))[0];
                                        var openPayable: ShipmentPayablePM = this.EntityPM.ShipmentPayables.filter(d => d.ChargesTypeId == newRecord.ChargesTypeId && d.MeasurementId == newRecord.MeasurementId && (d.ShipmentPayableLineStatusCode == "EMPT" || d.ShipmentPayableLineStatusCode == "OAMT"))[0];

                                        if (acctPayable == null) {
                                            openPayable.Quantity = newRecord.Quantity;
                                        }

                                        if (acctPayable != null && newRecord.Quantity > acctPayable.Quantity) {
                                            if (openPayable == null) {
                                                this.EntityPM.AddPayable(newRecord);
                                            }

                                            else {
                                                openPayable.Quantity = newRecord.Quantity;
                                            }
                                        }
                                    }
                                });
                            });
                        }
                    }
                }
            }
        });
    }
    public GeneratePayablesFromQuote(baseQuote: QuotePM) {
        this.BaseQuote = baseQuote;

        if (this.EntityPM && this.BaseQuote) {

            this.IsAdhoc = this.BaseQuote.QuoteTypeCode == "A" ? true : false;
            this.IsRoutingRate = !this.IsAdhoc;            

            var rate: number = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);
            var quoteProfitInLocalCurrency: number = null;
            var quoteProfitInProfitCurrency: number = null;
            if (this.BaseQuote.QuoteTypeCode == "A") {
                quoteProfitInLocalCurrency = this.BaseQuote.EstimateProfit * this.BaseQuote.ExchangeRate;
                quoteProfitInProfitCurrency = quoteProfitInLocalCurrency / rate;
            }

            this.EntityPM.EstimateProfitInLocalCurrency = AppTool.Round(quoteProfitInLocalCurrency, 2);
            this.EntityPM.EstimateProfitInProfitCurrency = AppTool.Round(quoteProfitInProfitCurrency, 2);

            if (this.IsLCLEntity) {
                this.GeneratePayablesFromQuote_LCL();
            }

            else {
                this.GeneratePayablesFromQuote_FCL();
            }            
        }
    }
    public GeneratePayablesFromOriginShipment(originShipment: ShipmentPM) {
        this.OriginShipment = originShipment;

        if (this.EntityPM && this.OriginShipment) {
            if (this.IsLCLEntity) {
                this.OriginShipment.ShipmentPayables.forEach(item => {
                    this.CreateNewPayableFromOriginShipment(item);
                });
            }

            else {
                this.OriginShipment.ShipmentPayables.forEach(item => {
                    switch (item.MeasurementCode) {
                        case "GRWT":
                        case "CHWT":
                        case "VOLU":
                        case "BTEU":
                        case "FIXD":
                        case "PRVL":
                        case "PRFR":
                        case "GWTN":
                        case "QTY":
                            {
                                this.CreateNewPayableFromOriginShipment(item);
                                break;
                            }

                        case "BCNT": {
                            // No such case
                            break;
                        }

                        default: {

                            var itemGrouped = this.BCNTGrouped.filter(f => f.MeasurementId == item.MeasurementId)[0];
                            if (itemGrouped != null) {
                                this.CreateNewPayableFromOriginShipment(item);
                            }

                            break;
                        }
                    }
                });
            }            
        }
    }
    private GeneratePayablesFromQuote_LCL() {
        this.BaseQuote.QuoteCharges.sort((a, b) => { return a.ViewOrder - b.ViewOrder }).forEach(item => {
            if (item.IsChargeBySteps) {
                var newPayablePM: ShipmentPayablePM = this.CreateNewPayableFromQuoteCharge(item);
                this.EntityPM.AddPayable(newPayablePM);
            }

            else if (!AppTool.IsNullOrEmpty(item.CostUnitPrice)) {
                var newPayablePM: ShipmentPayablePM = this.CreateNewPayableFromQuoteCharge(item);
                this.EntityPM.AddPayable(newPayablePM);
            }
        });

        this.ComputeAllPayablesQuote_LCL();
    }
    private GeneratePayablesFromQuote_FCL() {
        this.BaseQuote.QuoteCharges.sort((a, b) => { return a.ViewOrder - b.ViewOrder }).forEach(item => {
            if (item.CostMeasurementCode == "BCNT") {
                this.CreateNewPayableFromQuoteCharge_FCL(this.BaseQuote.PackageType1Id, item.CostContainerType1UnitPrice, item);
                this.CreateNewPayableFromQuoteCharge_FCL(this.BaseQuote.PackageType2Id, item.CostContainerType2UnitPrice, item);
                this.CreateNewPayableFromQuoteCharge_FCL(this.BaseQuote.PackageType3Id, item.CostContainerType3UnitPrice, item);
                this.CreateNewPayableFromQuoteCharge_FCL(this.BaseQuote.PackageType4Id, item.CostContainerType4UnitPrice, item);
                this.CreateNewPayableFromQuoteCharge_FCL(this.BaseQuote.PackageType5Id, item.CostContainerType5UnitPrice, item);
            }

            else if (!AppTool.IsNullOrEmpty(item.CostUnitPrice)) {
                var newPayablePM: ShipmentPayablePM = this.CreateNewPayableFromQuoteCharge(item);
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
                    case "QTY":
                        {
                            break;
                        }

                    default: {
                        if (item.CostMeasurementId) {
                            var itemGrouped: ByPckageType = this.BCNTGrouped.filter(f => f.MeasurementId == item.CostMeasurementId)[0];
                            if (itemGrouped) {
                                this.ComputePayableQuoteAmounts_FCL(newPayablePM, itemGrouped.Quantity);
                            }
                        }

                        break;
                    }
                }

                this.EntityPM.AddPayable(newPayablePM);
            }
        });

        // Add Other Shipment Containers on Quote Records
        this.BCNTGrouped.forEach(itemGrouped => {
            var myRecord: ShipmentPayablePM = this.EntityPM.ShipmentPayables.filter(f => f.MeasurementId == itemGrouped.MeasurementId)[0];
            if (myRecord == null) {
                this.BaseQuote.QuoteCharges.filter(f => f.IsAllIN == false).forEach(item => {
                    if (item.CostMeasurementCode == "BCNT") {

                        var newRecord = new ShipmentPayablePM(null);
                        newRecord.Tenant = this.EntityPM.Tenant;
                        newRecord.ShipmentId = this.EntityPM.Id;
                        newRecord.ShipmentNumber = this.EntityPM.ShipmentNumber;
                        newRecord.ShipmentPayableLineStatusCode = "EMPT";
                        newRecord.ShipmentPayableAmountTypeCode = "ACCU";
                        newRecord.ShipmentPayableAmountTypeName = "Accrual";
                        newRecord.CreateDate = DateTool.GetCurrentDateAsUtc();
                        newRecord.CreatedByUserId = SessionLocator.LoggedUserId;
                        newRecord.UpdateDate = DateTool.GetCurrentDateAsUtc();
                        newRecord.UpdateByUserId = SessionLocator.LoggedUserId;
                        newRecord.Quantity = itemGrouped.Quantity;
                        newRecord.MeasurementId = itemGrouped.MeasurementId;
                        newRecord.MeasurementCode = itemGrouped.MeasurementCode;
                        newRecord.MeasurementShortName = itemGrouped.MeasurementShortName;
 
                        this.myChargesTypeListService.getSingleFromCache(item.ChargesTypeId).subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError) {
                                var chargesType: ChargesTypeList = myResponse.Result;
                                if (chargesType) {
                                    newRecord.ChargesTypeId = chargesType.Id;
                                    newRecord.ChargesTypeCode = chargesType.Code;
                                    newRecord.ChargesTypeName = chargesType.EnglishName;
                                    newRecord.VatTypeId = chargesType.VatTypeId;
                                    newRecord.ChargesGroupCode = chargesType.ChargesGroupCode;
                                    newRecord.DueTypeCode = chargesType.DueTypeCode;
                                    newRecord.DueTypeName = chargesType.DueTypeName;
                                    newRecord.IATACodeId = chargesType.IATACodeId;
                                    newRecord.PrepaidCollectId = chargesType.ChargesGroupCode == "FRT" ? this.EntityPM.FreightPrepaidCollectId : this.EntityPM.OtherPrepaidCollectId;
                                    newRecord.IsBackToBack = chargesType.IsBackToBack;


                                    if (chargesType.ChargesGroupCode == "FRT" || chargesType.ChargesGroupCode == "SCH") {
                                        newRecord.CurrencyId = SessionLocator.TenantPM.FreightCurrencyId;
                                    }

                                    else {
                                        newRecord.CurrencyId = SessionLocator.TenantPM.OtherChargesCurrencyId;
                                    }

                                    this.GetCurrencyCode(newRecord);
                                    //newRecord.ProfitCurrencyExchangeRate
                                    newRecord.Rate = this.GetCurrencyRate(newRecord.CurrencyId);
                                }
                            }
                        });

                        this.EntityPM.AddPayable(newRecord);
                    }
                });
            }
        });

        this.ComputeAllPayablesQuote_LCL();
    }
    private ComputeAllPayablesQuote_LCL() {

        this.EntityPM.ShipmentPayables.filter(f => !AppTool.IsNullOrEmpty(f.QuoteChargeId)).forEach(item => {
            switch (item.MeasurementCode) {
                case "GRWT":
                case "CHWT":
                case "VOLU":
                case "BTEU":
                case "FIXD":
                case "PRVL":
                case "GWTN":
                case "QTY":
                    {
                        var itemCharge: QuoteChargePM = this.BaseQuote.QuoteCharges.filter(f => f.Id == item.QuoteChargeId)[0];
                        this.ComputePayableQuoteAmounts_LCL(item, itemCharge);
                        break;
                    }
            }
        });

        this.EntityPM.ShipmentPayables.filter(f => !AppTool.IsNullOrEmpty(f.QuoteChargeId)).forEach(item => {
            switch (item.MeasurementCode) {
                case "PRFR":
                    {
                        var itemCharge: QuoteChargePM = this.BaseQuote.QuoteCharges.filter(f => f.Id == item.QuoteChargeId)[0];
                        this.ComputePayableQuoteAmounts_LCL(item, itemCharge);
                        break;
                    }
            }
        }); 
    }
    private ComputePayableQuoteAmounts_LCL(myRecordPM: ShipmentPayablePM, item: QuoteChargePM) {

        // Quantity
        var myQuantity: number = null;
        switch (myRecordPM.MeasurementCode) {
            case "GRWT": { myQuantity = this.EntityPM.GrossWeight; break; }
            case "CHWT": { myQuantity = this.EntityPM.ChargeableWeight; break; }
            case "VOLU": { myQuantity = this.EntityPM.Volume; break; }
            case "BTEU": { myQuantity = this.EntityPM.TEU; break; }
            case "FIXD": { myQuantity = 1; break; }
            case "PRVL": { myQuantity = this.EntityPM.ValueOfGoods; break; }
            case "PRFR": { myQuantity = ArrayTool.Sum(this.EntityPM.ShipmentPayables.filter(d => d.ChargesGroupCode == "FRT" && AppTool.IsNullOrEmpty(d.ShipmentPayableParentId)), "ExpectedAmount"); break; }
            case "GWTN": { myQuantity = this.EntityPM.GrossWeightPerTon; break; }
            case "QTY": { myQuantity = this.EntityPM.NumberOfPackages; break; }
            default: { break; }
        }
        myRecordPM.Quantity = AppTool.Round(myQuantity, 2);

        // UnitPrice
        var myUnitPrice = item.CostUnitPrice;
        if (this.IsRoutingRate) {
            if (item.IsChargeBySteps) {
                item.QuoteChargePriceSteps.sort((a, b) => { return a.Step - b.Step }).forEach(step => {
                    if (!AppTool.IsNullOrEmpty(myQuantity) && myQuantity >= step.Step) {
                        myUnitPrice = step.CostUnitPrice;
                    }
                });

                var entitySmallest: QuotePriceStepsPM = item.QuoteChargePriceSteps.sort((a, b) => { return a.Step - b.Step })[0];
                if (entitySmallest != null) {
                    if (AppTool.IsNullOrEmpty(myQuantity) || myQuantity < entitySmallest.Step) {
                        myUnitPrice = entitySmallest.CostUnitPrice;
                    }
                }
            }
        }
        myRecordPM.UnitPrice = AppTool.Round(myUnitPrice, 3);

        // ComputedAmount
        var myComputedAmount: number = null;
        if (!AppTool.IsNullOrEmpty(myQuantity) && !AppTool.IsNullOrEmpty(myUnitPrice)) {
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

        if (!AppTool.IsNullOrEmpty(myComputedAmount)) {
            myRecordPM.ExpectedAmount = AppTool.Round(myComputedAmount, 2);

            if (!AppTool.IsNullOrEmpty(myRecordPM.Rate)) {
                myRecordPM.ExpectedAmountLocal = AppTool.Round(myRecordPM.ExpectedAmount * myRecordPM.Rate, 2);

                if (!AppTool.IsNullOrEmpty(myRecordPM.ProfitCurrencyExchangeRate)) {
                    myRecordPM.ExpectedAmountInProfitCurrency = AppTool.Round(myRecordPM.ExpectedAmountLocal / myRecordPM.ProfitCurrencyExchangeRate, 2);
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
    }
    private ComputePayableQuoteAmounts_FCL(itemPM: ShipmentPayablePM, quantity: number) {
        if (itemPM.ProfitCurrencyExchangeRate == 0) {
            itemPM.ProfitCurrencyExchangeRate = 1;
        }

        var nweQuantity = quantity;
        var expectedAmount = itemPM.UnitPrice * nweQuantity;
        var expectedAmountLocal = expectedAmount * itemPM.Rate;
        var expectedAmountProfit = expectedAmountLocal / itemPM.ProfitCurrencyExchangeRate;

        itemPM.Quantity = AppTool.Round(nweQuantity, 3);
        itemPM.ExpectedAmount = AppTool.Round(expectedAmount, 2);
        itemPM.ExpectedAmountLocal = AppTool.Round(expectedAmountLocal, 2);
        itemPM.ExpectedAmountInProfitCurrency = AppTool.Round(expectedAmountProfit, 2);

        itemPM.OpenAmount = itemPM.ExpectedAmount;
        itemPM.OpenAmountInLocalCurrency = itemPM.ExpectedAmountLocal;
        itemPM.OpenAmountInProfitCurrency = itemPM.ExpectedAmountInProfitCurrency;
        itemPM.AccountedAmount = 0;
        itemPM.AccountedAmountInLocalCurrency = 0;
        itemPM.AccountedAmountInProfitCurrency = 0;
        itemPM.ShipmentPayableLineStatusCode = (itemPM.Quantity != null && itemPM.UnitPrice != null) ? "OAMT" : "EMPT";
    }
    private CreateNewPayableFromQuoteCharge(item: QuoteChargePM) {
        var myRecordPM = new ShipmentPayablePM(null);
        myRecordPM.Tenant = SessionLocator.Tenant;
        myRecordPM.ShipmentId = this.EntityPM.Id;
        myRecordPM.ShipmentNumber = this.EntityPM.ShipmentNumber;
        myRecordPM.CreateDate = DateTool.GetCurrentDateAsUtc();
        myRecordPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
        myRecordPM.CreatedByUserId = SessionLocator.LoggedUserId;
        myRecordPM.UpdateByUserId = SessionLocator.LoggedUserId;
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
        if (!AppTool.IsNullOrEmpty(item.ChargesTypeId)) {
            this.myChargesTypeListService.getSingleFromCache(item.ChargesTypeId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var chargesType: ChargesTypeList = myResponse.Result;
                    if (chargesType) {
                        myRecordPM.ChargesTypeCode = chargesType.Code;
                        myRecordPM.ChargesTypeName = chargesType.EnglishName;
                        myRecordPM.ChargesGroupCode = chargesType.ChargesGroupCode;
                        myRecordPM.DueTypeCode = chargesType.DueTypeCode;
                        myRecordPM.DueTypeName = chargesType.DueTypeName;
                        myRecordPM.IATACodeId = chargesType.IATACodeId;
                        myRecordPM.PrepaidCollectId = chargesType.ChargesGroupCode == "FRT" ? this.EntityPM.FreightPrepaidCollectId : this.EntityPM.OtherPrepaidCollectId;
                        myRecordPM.ViewOrder = chargesType.ViewOrder;

                        if (AppTool.IsNullOrEmpty(myRecordPM.VatTypeId)) {
                            myRecordPM.VatTypeId = chargesType.VatTypeId;
                        }
                    }
                }
            });
        }

        return myRecordPM;
    }
    private CreateNewPayableFromQuoteCharge_FCL(myPackageTypeId: string, myUnitPrice: number, item: QuoteChargePM) {
        if (!AppTool.IsNullOrEmpty(myPackageTypeId)) {
            if (!AppTool.IsNullOrEmpty(myUnitPrice)) {
                var itemGrouped: ByPckageType = this.BCNTGrouped.filter(f => f.PackageTypeId == myPackageTypeId)[0];
                if (itemGrouped != null) {
                    var newItemPM: ShipmentPayablePM = this.CreateNewPayableFromQuoteCharge(item);
                    newItemPM.UnitPrice = myUnitPrice;
                    newItemPM.MeasurementId = itemGrouped.MeasurementId;
                    newItemPM.MeasurementCode = itemGrouped.MeasurementCode;
                    newItemPM.MeasurementShortName = itemGrouped.MeasurementShortName;
                    this.ComputePayableQuoteAmounts_FCL(newItemPM, itemGrouped.Quantity);
                    this.EntityPM.AddPayable(newItemPM);
                }
            }
        }
    }
    private CreateNewPayableFromOriginShipment(OriginItemPM: ShipmentPayablePM) {
        if (OriginItemPM) {
            if (OriginItemPM.ShipmentPayableAmountTypeCode == "ACCU") {
                var myRecordPM = new ShipmentPayablePM(null);
                myRecordPM.Tenant = SessionLocator.Tenant;
                myRecordPM.ShipmentId = this.EntityPM.Id;
                myRecordPM.ShipmentNumber = this.EntityPM.ShipmentNumber;
                myRecordPM.CreateDate = DateTool.GetCurrentDateAsUtc();
                myRecordPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
                myRecordPM.CreatedByUserId = SessionLocator.LoggedUserId;
                myRecordPM.UpdateByUserId = SessionLocator.LoggedUserId;
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
    }

    // Receivables
    public GenerateReceivablesAutoDisplay() {
        this.myChargesTypeListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var allChargesTypes: ChargesTypeList[] = myResponse.Result;
                if (allChargesTypes != null) {

                    allChargesTypes = allChargesTypes.filter(d => d.IsReceivable == true && d.InActive == false);

                    if (this.EntityPM.ShipmentLevelCode == "C") {
                        allChargesTypes = allChargesTypes.filter(d => d.IsAutoDisplayInConsolidation);
                    }

                    else {
                        if (this.EntityPM.IncludesCustoms) {
                            allChargesTypes = allChargesTypes.filter(d => d.IsAutoDisplayInShipment == true || d.IsAutoDisplayInCustoms);
                        }

                        else {
                            allChargesTypes = allChargesTypes.filter(d => d.IsAutoDisplayInShipment == true);
                        }
                    }

                    switch (this.EntityPM.TransportModeId) {
                        case "A":
                            {
                                allChargesTypes = allChargesTypes.filter(r => r.IsAir);
                                break;
                            }

                        case "O":
                            {
                                allChargesTypes = allChargesTypes.filter(r => r.IsOcean);
                                break;
                            }

                        case "I":
                            {
                                allChargesTypes = allChargesTypes.filter(r => r.IsInland);
                                break;
                            }
                    }

                    switch (this.EntityPM.DirectionId) {
                        case "E":
                            {
                                allChargesTypes = allChargesTypes.filter(r => r.IsExport);
                                break;
                            }

                        case "I":
                            {
                                allChargesTypes = allChargesTypes.filter(r => r.IsImport);
                                break;
                            }

                        case "D":
                            {
                                allChargesTypes = allChargesTypes.filter(r => r.IsDomestic);
                                break;
                            }

                        case "R":
                            {
                                allChargesTypes = allChargesTypes.filter(r => r.IsDrop);
                                break;
                            }
                    }

                    if (allChargesTypes.length > 0) {
                        if (this.IsLCLEntity) {
                            allChargesTypes.sort((a, b) => { return a.ViewOrder - b.ViewOrder }).forEach((item) => {

                                var newRecord: ShipmentReceivablePM = new ShipmentReceivablePM(this.EntityPM);
                                newRecord.Tenant = SessionLocator.Tenant;
                                newRecord.ShipmentId = this.EntityPM.Id;
                                newRecord.ShipmentNumber = this.EntityPM.ShipmentNumber;
                                newRecord.ShipmentReceivableLineStatusCode = "EMPT";
                                newRecord.CreatedByUserId = SessionLocator.LoggedUserId;
                                newRecord.UpdateByUserId = SessionLocator.LoggedUserId;
                                newRecord.CreateDate = DateTool.GetCurrentDateAsUtc();
                                newRecord.UpdateDate = DateTool.GetCurrentDateAsUtc();
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
                                    case "GRWT": { newRecord.Quantity = this.EntityPM.GrossWeight; break; }
                                    case "CHWT": { newRecord.Quantity = this.EntityPM.ChargeableWeight; break; }
                                    case "VOLU": { newRecord.Quantity = this.EntityPM.Volume; break; }
                                    case "BTEU": { newRecord.Quantity = this.EntityPM.TEU; break; }
                                    case "FIXD": { newRecord.Quantity = 1; break; }
                                    case "PRVL": { newRecord.Quantity = this.EntityPM.ValueOfGoods; break; }
                                    case "GWTN": { newRecord.Quantity = this.EntityPM.GrossWeightPerTon; }
                                    case "QTY": { newRecord.Quantity = this.EntityPM.NumberOfPackages; }
                                    default: { break; }
                                }

                                if (item.ChargesGroupCode == "FRT" || item.ChargesGroupCode == "SCH") {
                                    newRecord.CurrencyId = SessionLocator.TenantPM.FreightCurrencyId;
                                }

                                else {
                                    newRecord.CurrencyId = SessionLocator.TenantPM.OtherChargesCurrencyId;
                                }

                                this.GetCurrencyCode(newRecord);
                                newRecord.Rate = this.GetCurrencyRate(newRecord.CurrencyId);
                                newRecord.ProfitCurrencyExchangeRate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);
                                this.EntityPM.AddReceivable(newRecord);
                            })

                            this.EntityPM.ShipmentReceivables.filter(f => f.MeasurementCode == "PRFR").forEach(item => {
                                item.Quantity = ArrayTool.Sum(this.EntityPM.ShipmentReceivables.filter(d => d.ChargesGroupCode == "FRT" && AppTool.IsNullOrEmpty(d.ShipmentReceivableParentId)), "TotalAmount");
                            });
                        }

                        else {
                            if (this.BCNTGrouped.length > 0) {
                                allChargesTypes.filter(f => f.ContainerMeasurementCode == "BCNT").sort((a, b) => { return a.ViewOrder - b.ViewOrder }).forEach(myChargeType => {
                                    this.BCNTGrouped.forEach(itemGrouped => {
                                        var newReceivable: ShipmentReceivablePM = new ShipmentReceivablePM(this.EntityPM);
                                        newReceivable.Tenant = SessionLocator.Tenant;
                                        newReceivable.ShipmentId = this.EntityPM.Id;
                                        newReceivable.ShipmentNumber = this.EntityPM.ShipmentNumber;
                                        newReceivable.ShipmentReceivableLineStatusCode = "EMPT";
                                        newReceivable.CreatedByUserId = SessionLocator.LoggedUserId;
                                        newReceivable.UpdateByUserId = SessionLocator.LoggedUserId;
                                        newReceivable.CreateDate = DateTool.GetCurrentDateAsUtc();
                                        newReceivable.UpdateDate = DateTool.GetCurrentDateAsUtc();
                                        newReceivable.ChargesTypeId = myChargeType.Id;
                                        newReceivable.ChargesTypeCode = myChargeType.Code;
                                        newReceivable.ChargesTypeName = myChargeType.EnglishName;
                                        newReceivable.VatTypeId = myChargeType.VatTypeId;
                                        newReceivable.ChargesGroupCode = myChargeType.ChargesGroupCode;
                                        newReceivable.DueTypeCode = myChargeType.DueTypeCode;
                                        newReceivable.DueTypeName = myChargeType.DueTypeName;
                                        newReceivable.IATACodeId = myChargeType.IATACodeId;
                                        newReceivable.ViewOrder = myChargeType.ViewOrder;
                                        newReceivable.PrepaidCollectId = myChargeType.ChargesGroupCode == "FRT" ? this.EntityPM.FreightPrepaidCollectId : this.EntityPM.OtherPrepaidCollectId;
                                        newReceivable.Quantity = itemGrouped.Quantity;
                                        newReceivable.MeasurementId = itemGrouped.MeasurementId;
                                        newReceivable.MeasurementCode = itemGrouped.MeasurementCode;
                                        newReceivable.MeasurementShortName = itemGrouped.MeasurementShortName;
                                        newReceivable.IsBackToBack = myChargeType.IsBackToBack;
                                        newReceivable.IsExpense = myChargeType.IsExpense;

                                        if (myChargeType.ChargesGroupCode == "FRT" || myChargeType.ChargesGroupCode == "SCH") {
                                            newReceivable.CurrencyId = SessionLocator.TenantPM.FreightCurrencyId;
                                        }

                                        else {
                                            newReceivable.CurrencyId = SessionLocator.TenantPM.OtherChargesCurrencyId;
                                        }
                                        
                                        this.GetCurrencyCode(newReceivable);
                                        newReceivable.ProfitCurrencyExchangeRate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);
                                        newReceivable.Rate = this.GetCurrencyRate(newReceivable.CurrencyId);
                                        newReceivable.ProfitCurrencyExchangeRate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);

                                        var existsReceivable: ShipmentReceivablePM = this.EntityPM.ShipmentReceivables.filter(d => d.ChargesTypeId == newReceivable.ChargesTypeId && d.MeasurementId == newReceivable.MeasurementId)[0];
                                        if (existsReceivable == null) {
                                            this.EntityPM.AddReceivable(newReceivable);
                                        }

                                        else {
                                            //var acctReceivable: ShipmentReceivablePM = this.EntityPM.ShipmentReceivables.filter(d => d.ChargesTypeId == newReceivable.ChargesTypeId && d.MeasurementId == newReceivable.MeasurementId && (d.ShipmentReceivableLineStatusCode == "ACCT" || d.ShipmentReceivableLineStatusCode == "DRFT"))[0];
                                            //var openReceivable: ShipmentReceivablePM = this.EntityPM.ShipmentReceivables.filter(d => d.ChargesTypeId == newReceivable.ChargesTypeId && d.MeasurementId == newReceivable.MeasurementId && (d.ShipmentReceivableLineStatusCode == "EMPT" || d.ShipmentReceivableLineStatusCode == "OAMT"))[0];

                                            var acctReceivable: ShipmentReceivablePM = this.EntityPM.ShipmentReceivables.filter(d => d.ChargesTypeId == newReceivable.ChargesTypeId && d.MeasurementId == newReceivable.MeasurementId && d.ShipmentReceivableLineStatusCode == "ACCT")[0];
                                            var openReceivable: ShipmentReceivablePM = this.EntityPM.ShipmentReceivables.filter(d => d.ChargesTypeId == newReceivable.ChargesTypeId && d.MeasurementId == newReceivable.MeasurementId && d.ShipmentReceivableLineStatusCode != "ACCT")[0];

                                            if (acctReceivable == null) {
                                                openReceivable.Quantity = newReceivable.Quantity;
                                            }

                                            if (acctReceivable != null && newReceivable.Quantity > acctReceivable.Quantity) {
                                                if (openReceivable == null) {
                                                    this.EntityPM.AddReceivable(newReceivable);
                                                }

                                                else {
                                                    openReceivable.Quantity = newReceivable.Quantity;
                                                }
                                            }
                                        }
                                    });
                                });
                            }
                        }
                    }
                }
            }
        });
    }
    public GenerateReceivablesFromQuote(baseQuote: QuotePM) {
        this.BaseQuote = baseQuote;

        if (this.EntityPM && this.BaseQuote) {

            this.IsAdhoc = this.BaseQuote.QuoteTypeCode == "A" ? true : false;
            this.IsRoutingRate = !this.IsAdhoc; 

            var rate: number = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);
            var quoteProfitInLocalCurrency: number = null;
            var quoteProfitInProfitCurrency: number = null;
            if (this.BaseQuote.QuoteTypeCode == "A") {
                quoteProfitInLocalCurrency = this.BaseQuote.EstimateProfit * this.BaseQuote.ExchangeRate;
                quoteProfitInProfitCurrency = quoteProfitInLocalCurrency / rate;
            }

            this.EntityPM.EstimateProfitInLocalCurrency = AppTool.Round(quoteProfitInLocalCurrency, 2);
            this.EntityPM.EstimateProfitInProfitCurrency = AppTool.Round(quoteProfitInProfitCurrency, 2);

            if (this.IsLCLEntity) {
                this.GenerateReceivablesFromQuote_LCL();
            }

            else {
                this.GenerateReceivablesFromQuote_FCL();
            }
        }        
    }
    public GenerateReceivablesFromOriginShipment(originShipment: ShipmentPM) {
        this.OriginShipment = originShipment;

        if (this.EntityPM && this.OriginShipment) {
            if (this.IsLCLEntity) {
                this.OriginShipment.ShipmentReceivables.forEach(item => {
                    this.CreateNewReceivableFromOriginShipment(item);
                });
            }

            else {
                this.OriginShipment.ShipmentReceivables.forEach(item => {
                    switch (item.MeasurementCode) {
                        case "GRWT":
                        case "CHWT":
                        case "VOLU":
                        case "BTEU":
                        case "FIXD":
                        case "PRVL":
                        case "PRFR":
                        case "GWTN":
                        case "QTY":
                            {
                                this.CreateNewReceivableFromOriginShipment(item);
                                break;
                            }

                        case "BCNT": {
                            // No such case
                            break;
                        }

                        default: {

                            var itemGrouped = this.BCNTGrouped.filter(f => f.MeasurementId == item.MeasurementId)[0];
                            if (itemGrouped != null) {
                                this.CreateNewReceivableFromOriginShipment(item);
                            }

                            break;
                        }
                    }
                });
            }
        }
    }
    private GenerateReceivablesFromQuote_LCL() {
        this.BaseQuote.QuoteCharges.sort((a, b) => { return a.ViewOrder - b.ViewOrder }).filter(f => f.IsAllIN == false).forEach(item => {
            if (item.IsChargeBySteps) {
                var newReceivablePM: ShipmentReceivablePM = this.CreateNewReceivableFromQuoteCharge(item);
                this.EntityPM.AddReceivable(newReceivablePM);
            }

            else if (!AppTool.IsNullOrEmpty(item.SaleUnitPrice)) {
                var newReceivablePM: ShipmentReceivablePM = this.CreateNewReceivableFromQuoteCharge(item);
                this.EntityPM.AddReceivable(newReceivablePM);
            }
        });

        this.ComputeAllReceivablesQuote_LCL();
    }
    private GenerateReceivablesFromQuote_FCL() {
        this.BaseQuote.QuoteCharges.sort((a, b) => { return a.ViewOrder - b.ViewOrder }).filter(f => f.IsAllIN == false).forEach(item => {
            if (item.SaleMeasurementCode == "BCNT") {
                this.CreateNewReceivableFromQuoteCharge_FCL(this.BaseQuote.PackageType1Id, item.SaleContainerType1UnitPrice, item);
                this.CreateNewReceivableFromQuoteCharge_FCL(this.BaseQuote.PackageType2Id, item.SaleContainerType2UnitPrice, item);
                this.CreateNewReceivableFromQuoteCharge_FCL(this.BaseQuote.PackageType3Id, item.SaleContainerType3UnitPrice, item);
                this.CreateNewReceivableFromQuoteCharge_FCL(this.BaseQuote.PackageType4Id, item.SaleContainerType4UnitPrice, item);
                this.CreateNewReceivableFromQuoteCharge_FCL(this.BaseQuote.PackageType5Id, item.SaleContainerType5UnitPrice, item);
            }

            else if (!AppTool.IsNullOrEmpty(item.SaleUnitPrice)) {
                var newReceivablePM: ShipmentReceivablePM = this.CreateNewReceivableFromQuoteCharge(item);

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
                    case "QTY":
                        {
                            break;
                        }

                    default: {
                        if (item.CostMeasurementId) {
                            var itemGrouped: ByPckageType = this.BCNTGrouped.filter(f => f.MeasurementId == item.CostMeasurementId)[0];
                            if (itemGrouped) {
                                this.ComputeReceivableQuoteAmounts_FCL(newReceivablePM, itemGrouped.Quantity);
                            }
                        }

                        break;
                    }
                }

                this.EntityPM.AddReceivable(newReceivablePM);
            }
        });

        // Add Other Shipment Containers on Quote Records
        this.BCNTGrouped.forEach(itemGrouped => {
            var myRecord: ShipmentReceivablePM = this.EntityPM.ShipmentReceivables.filter(f => f.MeasurementId == itemGrouped.MeasurementId)[0];
            if (myRecord == null) {
                this.BaseQuote.QuoteCharges.filter(f => f.IsAllIN == false).forEach(item => {
                    if (item.SaleMeasurementCode == "BCNT") {
                        var newRecord = new ShipmentReceivablePM(null);
                        newRecord.Tenant = this.EntityPM.Tenant;
                        newRecord.ShipmentId = this.EntityPM.Id;
                        newRecord.ShipmentNumber = this.EntityPM.ShipmentNumber;
                        newRecord.ShipmentReceivableLineStatusCode = "EMPT";
                        newRecord.CreateDate = DateTool.GetCurrentDateAsUtc();
                        newRecord.CreatedByUserId = SessionLocator.LoggedUserId;
                        newRecord.UpdateDate = DateTool.GetCurrentDateAsUtc();
                        newRecord.UpdateByUserId = SessionLocator.LoggedUserId;
                        newRecord.Quantity = itemGrouped.Quantity;
                        newRecord.MeasurementId = itemGrouped.MeasurementId;
                        newRecord.MeasurementCode = itemGrouped.MeasurementCode;
                        newRecord.MeasurementShortName = itemGrouped.MeasurementShortName;

                        this.myChargesTypeListService.getSingleFromCache(item.ChargesTypeId).subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError) {
                                var chargesType: ChargesTypeList = myResponse.Result;
                                if (chargesType) {
                                    newRecord.ChargesTypeId = chargesType.Id;
                                    newRecord.ChargesTypeCode = chargesType.Code;
                                    newRecord.ChargesTypeName = chargesType.EnglishName;
                                    newRecord.VatTypeId = chargesType.VatTypeId;
                                    newRecord.ChargesGroupCode = chargesType.ChargesGroupCode;
                                    newRecord.DueTypeCode = chargesType.DueTypeCode;
                                    newRecord.DueTypeName = chargesType.DueTypeName;
                                    newRecord.IATACodeId = chargesType.IATACodeId;
                                    newRecord.PrepaidCollectId = chargesType.ChargesGroupCode == "FRT" ? this.EntityPM.FreightPrepaidCollectId : this.EntityPM.OtherPrepaidCollectId;
                                    newRecord.IsBackToBack = chargesType.IsBackToBack;
                                    if (chargesType.ChargesGroupCode == "FRT" || chargesType.ChargesGroupCode == "SCH") {
                                        newRecord.CurrencyId = SessionLocator.TenantPM.FreightCurrencyId;
                                    }

                                    else {
                                        newRecord.CurrencyId = SessionLocator.TenantPM.OtherChargesCurrencyId;
                                    }

                                    this.GetCurrencyCode(newRecord);
                                    newRecord.Rate = this.GetCurrencyRate(newRecord.CurrencyId);
                                    newRecord.ProfitCurrencyExchangeRate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);
                                }
                            }
                        });

                        this.EntityPM.AddReceivable(newRecord);
                    }
                });
            }
        });

        this.ComputeAllReceivablesQuote_LCL();
    }
    private ComputeAllReceivablesQuote_LCL() {
        this.EntityPM.ShipmentReceivables.filter(f => !AppTool.IsNullOrEmpty(f.QuoteChargeId)).forEach(item => {
            switch (item.MeasurementCode) {
                case "GRWT":
                case "CHWT":
                case "VOLU":
                case "BTEU":
                case "FIXD":
                case "PRVL":
                case "GWTN":
                case "QTY":
                    {
                        var itemCharge: QuoteChargePM = this.BaseQuote.QuoteCharges.filter(f => f.Id == item.QuoteChargeId)[0];
                        this.ComputeReceivableQuoteAmounts_LCL(item, itemCharge);
                        break;
                    }
            }
        });

        this.EntityPM.ShipmentReceivables.filter(f => !AppTool.IsNullOrEmpty(f.QuoteChargeId)).forEach(item => {
            switch (item.MeasurementCode) {
                case "PRFR":
                    {
                        var itemCharge: QuoteChargePM = this.BaseQuote.QuoteCharges.filter(f => f.Id == item.QuoteChargeId)[0];
                        this.ComputeReceivableQuoteAmounts_LCL(item, itemCharge);
                        break;
                    }
            }
        });
    }
    private ComputeReceivableQuoteAmounts_LCL(myRecordPM: ShipmentReceivablePM, item: QuoteChargePM) {

        // Quantity
        var myQuantity: number = null;
        switch (myRecordPM.MeasurementCode) {
            case "GRWT": { myQuantity = this.EntityPM.GrossWeight; break; }
            case "CHWT": { myQuantity = this.EntityPM.ChargeableWeight; break; }
            case "VOLU": { myQuantity = this.EntityPM.Volume; break; }
            case "BTEU": { myQuantity = this.EntityPM.TEU; break; }
            case "FIXD": { myQuantity = 1; break; }
            case "PRVL": { myQuantity = this.EntityPM.ValueOfGoods; break; }
            case "PRFR": { myQuantity = ArrayTool.Sum(this.EntityPM.ShipmentReceivables.filter(d => d.ChargesGroupCode == "FRT" && AppTool.IsNullOrEmpty(d.ShipmentReceivableParentId)), "TotalAmount"); break; }
            case "GWTN": { myQuantity = this.EntityPM.GrossWeightPerTon; break; }
            case "QTY": { myQuantity = this.EntityPM.NumberOfPackages; break; }
            default: { break; }
        }
        myRecordPM.Quantity = AppTool.Round(myQuantity, 2);

        // UnitPrice
        var myUnitPrice = item.SaleUnitPrice;
        if (this.IsRoutingRate) {
            if (item.IsChargeBySteps) {
                item.QuoteChargePriceSteps.sort((a, b) => { return a.Step - b.Step }).forEach(step => {
                    if (!AppTool.IsNullOrEmpty(myQuantity) && myQuantity >= step.Step) {
                        myUnitPrice = step.SaleUnitPrice;
                    }
                });

                var entitySmallest: QuotePriceStepsPM = item.QuoteChargePriceSteps.sort((a, b) => { return a.Step - b.Step })[0];
                if (entitySmallest != null) {
                    if (AppTool.IsNullOrEmpty(myQuantity) || myQuantity < entitySmallest.Step) {
                        myUnitPrice = entitySmallest.SaleUnitPrice;
                    }
                }
            }
        }
        myRecordPM.UnitPrice = AppTool.Round(myUnitPrice, 3);

        // ComputedAmount
        var myComputedAmount: number = null;
        if (!AppTool.IsNullOrEmpty(myQuantity) && !AppTool.IsNullOrEmpty(myUnitPrice)) {
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

            if (!AppTool.IsNullOrEmpty(myRecordPM.ProfitCurrencyExchangeRate)) {
                myRecordPM.AmountInProfitCurrency = AppTool.Round(myRecordPM.TotalAmountLocal / myRecordPM.ProfitCurrencyExchangeRate, 2);
            }
        }

        else if (!AppTool.IsNullOrEmpty(myComputedAmount)) {
            myRecordPM.TotalAmount = AppTool.Round(myComputedAmount, 2);

            if (!AppTool.IsNullOrEmpty(myRecordPM.Rate)) {
                myRecordPM.TotalAmountLocal = AppTool.Round(myRecordPM.TotalAmount * myRecordPM.Rate, 2);

                if (!AppTool.IsNullOrEmpty(myRecordPM.ProfitCurrencyExchangeRate)) {
                    myRecordPM.AmountInProfitCurrency = AppTool.Round(myRecordPM.TotalAmountLocal / myRecordPM.ProfitCurrencyExchangeRate, 2);
                }
            }
        }
        
        myRecordPM.ShipmentReceivableLineStatusCode = (myQuantity != null && myUnitPrice != null) ? "OAMT" : "EMPT";
    }
    private ComputeReceivableQuoteAmounts_FCL(itemPM: ShipmentReceivablePM, quantity: number) {
        if (itemPM.ProfitCurrencyExchangeRate == 0) {
            itemPM.ProfitCurrencyExchangeRate = 1;
        }

        var newQuantity = quantity;
        var amount = itemPM.UnitPrice * newQuantity;
        var amountInLocal = amount * itemPM.Rate;
        var amountInProfit = amountInLocal / itemPM.ProfitCurrencyExchangeRate;

        itemPM.Quantity = AppTool.Round(newQuantity, 2);
        itemPM.TotalAmount = AppTool.Round(amount, 2);
        itemPM.TotalAmountLocal = AppTool.Round(amountInLocal, 2);
        itemPM.AmountInProfitCurrency = AppTool.Round(amountInProfit, 2);
        itemPM.ShipmentReceivableLineStatusCode = (itemPM.Quantity != null && itemPM.UnitPrice != null) ? "OAMT" : "EMPT";  
    }
    private CreateNewReceivableFromQuoteCharge(QuoteCharge: QuoteChargePM) {
        var myRecordPM = new ShipmentReceivablePM(null);
        myRecordPM.Tenant = SessionLocator.Tenant;
        myRecordPM.ShipmentId = this.EntityPM.Id;
        myRecordPM.ShipmentNumber = this.EntityPM.ShipmentNumber;
        myRecordPM.CreateDate = DateTool.GetCurrentDateAsUtc();
        myRecordPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
        myRecordPM.CreatedByUserId = SessionLocator.LoggedUserId;
        myRecordPM.UpdateByUserId = SessionLocator.LoggedUserId;
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
        this.myChargesTypeListService.getSingleFromCache(QuoteCharge.ChargesTypeId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var chargesType: ChargesTypeList = myResponse.Result;
                if (chargesType) {
                    myRecordPM.ChargesGroupCode = chargesType.ChargesGroupCode;
                    myRecordPM.DueTypeCode = chargesType.DueTypeCode;
                    myRecordPM.DueTypeName = chargesType.DueTypeName;
                    myRecordPM.IATACodeId = chargesType.IATACodeId;
                    myRecordPM.PrepaidCollectId = chargesType.ChargesGroupCode == "FRT" ? this.EntityPM.FreightPrepaidCollectId : this.EntityPM.OtherPrepaidCollectId;                    
                    myRecordPM.ViewOrder = chargesType.ViewOrder;
                    myRecordPM.IsExpense = chargesType.IsExpense;

                    if (AppTool.IsNullOrEmpty(myRecordPM.VatTypeId)) {
                        myRecordPM.VatTypeId = chargesType.VatTypeId;
                    }
                }
            }
        });

        return myRecordPM;
    }
    private CreateNewReceivableFromQuoteCharge_FCL(myPackageTypeId: string, myUnitPrice: number, item: QuoteChargePM) {
        if (!AppTool.IsNullOrEmpty(myPackageTypeId)) {
            if (!AppTool.IsNullOrEmpty(myUnitPrice)) {
                var itemGrouped: ByPckageType = this.BCNTGrouped.filter(f => f.PackageTypeId == myPackageTypeId)[0];
                if (itemGrouped != null) {
                    var newItemPM: ShipmentReceivablePM = this.CreateNewReceivableFromQuoteCharge(item);
                    newItemPM.UnitPrice = myUnitPrice;
                    newItemPM.MeasurementId = itemGrouped.MeasurementId;
                    newItemPM.MeasurementCode = itemGrouped.MeasurementCode;
                    newItemPM.MeasurementShortName = itemGrouped.MeasurementShortName;
                    this.ComputeReceivableQuoteAmounts_FCL(newItemPM, itemGrouped.Quantity);
                    this.EntityPM.AddReceivable(newItemPM);
                }
            }
        }
    }
    private CreateNewReceivableFromOriginShipment(OriginItemPM: ShipmentReceivablePM) {
        if (OriginItemPM) {
            var myRecordPM = new ShipmentReceivablePM(null);
            myRecordPM.Tenant = SessionLocator.Tenant;
            myRecordPM.ShipmentId = this.EntityPM.Id;
            myRecordPM.ShipmentNumber = this.EntityPM.ShipmentNumber;
            myRecordPM.CreateDate = DateTool.GetCurrentDateAsUtc();
            myRecordPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
            myRecordPM.CreatedByUserId = SessionLocator.LoggedUserId;
            myRecordPM.UpdateByUserId = SessionLocator.LoggedUserId;
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
    }
    private GetCurrencyRate(CurrencyId: string) {
        var myResult: number = null;

        if (CurrencyId == SessionLocator.LocalCurrencyId) {
            myResult = 1;
        }

        else {
            var lastRate: LastRate = this.AllRates.filter(f => f.ForeignCurrencyId == CurrencyId)[0];
            if (lastRate != null) {
                myResult = lastRate.Rate;
            }
        }

        return myResult;
    }
    private GetCurrencyCode(entity: any) {
        if (entity) {
            this.myCurrencyListService.getSingleFromCache(entity.CurrencyId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: CurrencyList = myResponse.Result;
                    if (list != null) {
                        entity.CurrencyCode = list.Code;
                    }
                }
            });
        }
    }
}
export class AWBHelper {
    public static ValidateAWBCCS(shipmentPM: ShipmentPM) {
        var myResult = new AWBCCSValidator();

        if (SessionLocator.TenantManagementJS.AWBMessagesCCSTypeCode == "GLSHK") {
            myResult.FWB = shipmentPM.TenantZeroAirlineGLSHKFWB;
            myResult.FHL = shipmentPM.TenantZeroAirlineGLSHKFHL;
            myResult.FSU = shipmentPM.TenantZeroAirlineGLSHKFSU;
            myResult.FSRFSA = shipmentPM.TenantZeroAirlineGLSHKFSRFSA;
            myResult.FVRFVA = shipmentPM.TenantZeroAirlineGLSHKFVRFVA;

            if (AppTool.IsNullOrEmpty(shipmentPM.TenantZeroAirlinePIMA)) {
                myResult.IsValid = false;
                myResult.AirlineFieldHasError = true;
                myResult.AirlineFieldErrorMessage = "Airline communication parameter (PIMA) is missing";
            }

            if (AppTool.IsNullOrEmpty(SessionLocator.TenantManagementJS.PIMA)) {
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

            if (AppTool.IsNullOrEmpty(shipmentPM.TenantZeroAirlineTTY)) {
                myResult.IsValid = false;
                myResult.AirlineFieldHasError = true;
                myResult.AirlineFieldErrorMessage = "This Airline doesn't support transmitting messages";
            }

            if (AppTool.IsNullOrEmpty(SessionLocator.TenantManagementJS.TTY)) {
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
    }
    public static ValidateShipment(shipmentPM: ShipmentPM) {
        var errors: string[] = [];

        if (shipmentPM != null) {

            var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            var table = (shipmentPM.ShipmentLevelCode == "C") ? "Master" : "Shipment";

            Validator.TryValidateObject(shipmentPM, table, errors);

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

            if (AppTool.IsNullOrEmpty(shipmentPM.ShipperId)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate(table + ".F.ShipperId")));
            }

            this.ValidateRoutings(shipmentPM, errors);

            if (shipmentPM.DirectionId.toUpperCase() == "D") {
                if (!AppTool.IsNullOrEmpty(shipmentPM.MainCarriageFromPortId) && !AppTool.IsNullOrEmpty(shipmentPM.MainCarriageToPortId)) {
                    if (shipmentPM.FromCountryId != shipmentPM.ToCountryId) {
                        if (shipmentPM.FromCountryIsEC == false || shipmentPM.ToCountryIsEC == false) {
                            if (shipmentPM.TransportModeId == "I") {
                                errors.push("Main Carriage Addresses must be in the same country since the direction is Domestic");
                            }

                            else {
                                errors.push("Main Carriage Ports must be in the same country since the direction is Domestic");
                            }
                        }
                    }
                }
            }

            if (shipmentPM.ShipmentAWBPrintOnlies != null) {
                shipmentPM.ShipmentAWBPrintOnlies.forEach(item => {
                    Validator.TryValidateObject(item, "ShipmentAWBPrintOnly", errors);

                    if (AppTool.IsNullOrEmpty(item.IATACodeId)) {
                        errors.push("IATA code field is required");
                    }

                    if (AppTool.IsNullOrEmpty(item.PrepaidCollectId)) {
                        errors.push("P/C field is required");
                    }

                    if (item.CurrencyId != shipmentPM.AWBCurrencyId) {
                        errors.push("Currency is not matching the shipment awb currency");
                    }
                });
            }

            if (shipmentPM.ShipmentPackages != null) {
                shipmentPM.ShipmentPackages.forEach(item => {
                    Validator.TryValidateObject(item, "ShipmentPackage", errors);
                });
            }

            if (shipmentPM.ShipmentCommodities != null) {
                shipmentPM.ShipmentCommodities.forEach(item => {
                    Validator.TryValidateObject(item, "ShipmentCommodity", errors);
                });
            }

            //if (!SessionLocator.TenantPM.AllowEAWBMoreThanTenPackages) {
            //    var myError: string = ShipmentTool.ValidateAddedPackagesCount(shipmentPM);

            //    if (!AppTool.IsNullOrEmpty(myError)) {
            //        errors.push(myError);
            //    }
            //}

            if (shipmentPM.AWBOCIPMs != null) {
                shipmentPM.AWBOCIPMs.forEach(item => {
                    Validator.TryValidateObject(item, "AWBOCI", errors);

                    if (AppTool.IsNullOrEmpty(item.CountryId) && AppTool.IsNullOrEmpty(item.AWBCustomsInformationCode) && AppTool.IsNullOrEmpty(item.AWBInformationCode)) {
                        errors.push("You must fill one of the fields (Country or Information or CustomsInformation)");
                    }
                });
            }

            if (!AppTool.IsNullOrEmpty(shipmentPM.OtherParticipantInformationName1) && AppTool.IsNullOrEmpty(shipmentPM.OtherParticipantInformationReference1)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.OtherParticipantInformationReference1")));
            }

            if (!AppTool.IsNullOrEmpty(shipmentPM.OtherParticipantInformationName2) && AppTool.IsNullOrEmpty(shipmentPM.OtherParticipantInformationReference2)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.OtherParticipantInformationReference2")));
            }

            if (!AppTool.IsNullOrEmpty(shipmentPM.OtherParticipantInformationName3) && AppTool.IsNullOrEmpty(shipmentPM.OtherParticipantInformationReference3)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.OtherParticipantInformationReference3")));
            }
        }

        return errors;
    }
    private static ValidateRoutings(shipmentPM: ShipmentPM, errors: string[]) {

        if (shipmentPM.ShipmentLevelCode == "H" && AppTool.IsNullOrEmpty(shipmentPM.MasterShipmentDataId)) {

            var ValidationText = TextCodeTranslator.Translate("General.M.FieldIsRequired");

            if (AppTool.IsNullOrEmpty(shipmentPM.FromPortId)) {

                if (shipmentPM.TransportModeId == "A") {
                    errors.push(ValidationText.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.Departure")));
                }

                else {
                    errors.push("From Port is required");
                }
            }

            if (AppTool.IsNullOrEmpty(shipmentPM.ToPortId)) {

                if (shipmentPM.TransportModeId == "A") {
                    errors.push(ValidationText.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.Destination")));
                }

                else {
                    errors.push("To Port is required");
                }
            }
        }

        else {
            var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            var table = (shipmentPM.ShipmentLevelCode == "C") ? "Master" : "Shipment";

            //if (AppTool.IsNullOrEmpty(shipmentPM.MainCarriageFromPortId)) {
            //    errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate(table + ".F.MainCarriageFromPortId")));
            //}

            //if (AppTool.IsNullOrEmpty(shipmentPM.MainCarriageToPortId)) {
            //    errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate(table + ".F.MainCarriageToPortId")));
            //}

            if (!AppTool.IsNullOrEmpty(shipmentPM.Transshipment2FromPortId)) {
                if (AppTool.IsNullOrEmpty(shipmentPM.Transshipment1FromPortId)) {
                    errors.push("To Add Transshipment2 you need to add Transshipment1");
                }
            }

            RoutingHelper.ValidateRoutingsActualDates(shipmentPM, errors);
            RoutingHelper.ValidateRoutingsSeriesDates(shipmentPM, errors, "MainCarriage");

            // followups remove legs
        }
    }
}
export class AWBCCSValidator {
    public IsValid: boolean;
    public AirlineFieldHasError: boolean;
    public AirlineRegistrationHasError: boolean;
    public TenantManagementFieldHasError: boolean;
    public AirlineFieldErrorMessage: string;
    public AirlineRegistrationErrorMessage: string;
    public TenantManagementFieldErrorMessage: string;
    public FWB: boolean;
    public FHL: boolean;
    public FSU: boolean;
    public FSRFSA: boolean;
    public FVRFVA: boolean;
    constructor() {
        this.IsValid = true;
        this.AirlineFieldHasError = false;
        this.AirlineRegistrationHasError = false;
        this.TenantManagementFieldHasError = false;
        this.AirlineFieldErrorMessage = null;
        this.AirlineRegistrationErrorMessage = null;
        this.TenantManagementFieldErrorMessage = null;
    }
}
export class RoutingHelper {
    private static CurrentSession = SessionLocator.SelectedSession;
    public static MainCarriageFromPortChanged(entityPM: ShipmentPM, list: PortList) {
        if (entityPM != null) {
            var myPortId: string = null;
            var myPortCode: string = null;
            var myPortName: string = null;
            var myPortCountryId: string = null;
            var myPortCountryCode: string = null;
            var myPortCountryName: string = null;
            var myPortCountryEC: boolean = false;
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
            entityPM.MainCarriageFromPortCountryEC = myPortCountryEC;

            ShipmentTool.ComputeSCI(entityPM);
            ShipmentTool.BuildAWBPlaceField(entityPM);

            // Previous.To == this.From
            if (!AppTool.IsNullOrEmpty(entityPM.PreCarriageFromPortId)) {
                entityPM.PreCarriageToPortId = myPortId;
                entityPM.PreCarriageToPortCode = myPortCode;
                entityPM.PreCarriageToPortName = myPortName;
                entityPM.PreCarriageToPortCountryCode = myPortCountryCode;
                entityPM.PreCarriageToPortCountryName = myPortCountryName;
            }
        }
    }
    public static Transshipment1FromPortChanged(entityPM: ShipmentPM, list: PortList) {
        if (entityPM != null) {
            var myPortId: string = null;
            var myPortCode: string = null;
            var myPortName: string = null;
            var myPortCountryId: string = null;
            var myPortCountryCode: string = null;
            var myPortCountryName: string = null;
            var myPortCountryEC: boolean = false;
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
            entityPM.Transshipment1FromPortId = myPortId;
            entityPM.Transshipment1FromPortCode = myPortCode;
            entityPM.Transshipment1FromPortName = myPortName;
            entityPM.Transshipment1FromPortCountryCode = myPortCountryCode;
            entityPM.Transshipment1FromPortCountryName = myPortCountryName;

            // Previous.To == this.From OR Next.From
            if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1FromPortId)) {
                entityPM.MainCarriageToPortId = entityPM.Transshipment1FromPortId;
                entityPM.MainCarriageToPortCode = entityPM.Transshipment1FromPortCode;
                entityPM.MainCarriageToPortName = entityPM.Transshipment1FromPortName;
                entityPM.MainCarriageToPortCountryCode = entityPM.Transshipment1FromPortCountryCode;
                entityPM.MainCarriageToPortCountryName = entityPM.Transshipment1FromPortCountryName;
                entityPM.MainCarriageToPortCountryEC = myPortCountryEC;
            }

            else if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2FromPortId)) {
                entityPM.MainCarriageToPortId = entityPM.Transshipment2FromPortId;
                entityPM.MainCarriageToPortCode = entityPM.Transshipment2FromPortCode;
                entityPM.MainCarriageToPortName = entityPM.Transshipment2FromPortName;
                entityPM.MainCarriageToPortCountryCode = entityPM.Transshipment2FromPortCountryCode;
                entityPM.MainCarriageToPortCountryName = entityPM.Transshipment2FromPortCountryName;
                //entityPM.MainCarriageToPortCountryEC = entityPM.Transshipment2FromPortCountryEC;                    
            }

            else if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3FromPortId)) {
                entityPM.MainCarriageToPortId = entityPM.Transshipment3FromPortId;
                entityPM.MainCarriageToPortCode = entityPM.Transshipment3FromPortCode;
                entityPM.MainCarriageToPortName = entityPM.Transshipment3FromPortName;
                entityPM.MainCarriageToPortCountryCode = entityPM.Transshipment3FromPortCountryCode;
                entityPM.MainCarriageToPortCountryName = entityPM.Transshipment3FromPortCountryName;
                //entityPM.MainCarriageToPortCountryEC = entityPM.Transshipment3FromPortCountryEC;                    
            }

            else {
                entityPM.MainCarriageToPortId = entityPM.MainCarriageFinalDestinationPortId;
                entityPM.MainCarriageToPortCode = entityPM.MainCarriageFinalDestinationPortCode;
                entityPM.MainCarriageToPortName = entityPM.MainCarriageFinalDestinationPortName;
                entityPM.MainCarriageToPortCountryCode = entityPM.MainCarriageFinalDestinationPortCountryCode;
                entityPM.MainCarriageToPortCountryName = entityPM.MainCarriageFinalDestinationPortCountryName;
                entityPM.MainCarriageToPortCountryEC = entityPM.ToCountryIsEC;
            }

            // this.To == thisDeleted OR next.From
            if (AppTool.IsNullOrEmpty(entityPM.Transshipment1FromPortId)) {
                entityPM.Transshipment1ToPortId = null;
                entityPM.Transshipment1ToPortCode = null;
                entityPM.Transshipment1ToPortName = null;
                entityPM.Transshipment1ToPortCountryCode = null;
                entityPM.Transshipment1ToPortCountryName = null;
            }

            else {
                if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2FromPortId)) {
                    entityPM.Transshipment1ToPortId = entityPM.Transshipment2FromPortId;
                    entityPM.Transshipment1ToPortCode = entityPM.Transshipment2FromPortCode;
                    entityPM.Transshipment1ToPortName = entityPM.Transshipment2FromPortName;
                    entityPM.Transshipment1ToPortCountryCode = entityPM.Transshipment2FromPortCountryCode;
                    entityPM.Transshipment1ToPortCountryName = entityPM.Transshipment2FromPortCountryName;
                }

                else if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3FromPortId)) {
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
    }
    public static Transshipment2FromPortChanged(entityPM: ShipmentPM, list: PortList) {
        if (entityPM != null) {
            var myPortId: string = null;
            var myPortCode: string = null;
            var myPortName: string = null;
            var myPortCountryId: string = null;
            var myPortCountryCode: string = null;
            var myPortCountryName: string = null;
            var myPortCountryEC: boolean = false;
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
                if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3FromPortId)) {
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
                if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1FromPortId)) {
                    entityPM.Transshipment1ToPortId = myPortId;
                    entityPM.Transshipment1ToPortCode = myPortCode;
                    entityPM.Transshipment1ToPortName = myPortName;
                    entityPM.Transshipment1ToPortCountryCode = myPortCountryCode;
                    entityPM.Transshipment1ToPortCountryName = myPortCountryName;
                    entityPM.Transshipment1ToPortCountryEC = myPortCountryEC;
                }

                else {
                    entityPM.MainCarriageToPortId = myPortId;
                    entityPM.MainCarriageToPortCode = myPortCode;
                    entityPM.MainCarriageToPortName = myPortName;
                    entityPM.MainCarriageToPortCountryCode = myPortCountryCode;
                    entityPM.MainCarriageToPortCountryName = myPortCountryName;
                    entityPM.MainCarriageToPortCountryEC = myPortCountryEC;
                }
            }

            else {
                myPortId = entityPM.MainCarriageFinalDestinationPortId;
                myPortCode = entityPM.MainCarriageFinalDestinationPortCode;
                myPortName = entityPM.MainCarriageFinalDestinationPortName;
                myPortCountryCode = entityPM.MainCarriageFinalDestinationPortCountryCode;
                myPortCountryName = entityPM.MainCarriageFinalDestinationPortCountryName;
                myPortCountryEC = false;

                if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3FromPortId)) {
                    myPortId = entityPM.Transshipment3FromPortId;
                    myPortCode = entityPM.Transshipment3FromPortCode;
                    myPortName = entityPM.Transshipment3FromPortName;
                    myPortCountryCode = entityPM.Transshipment3FromPortCountryCode;
                    myPortCountryName = entityPM.Transshipment3FromPortCountryName;
                    //myPortCountryEC = entityPM.Transshipment3FromPortCountryEC; 
                }

                if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1FromPortId)) {
                    entityPM.Transshipment1ToPortId = myPortId;
                    entityPM.Transshipment1ToPortCode = myPortCode;
                    entityPM.Transshipment1ToPortName = myPortName;
                    entityPM.Transshipment1ToPortCountryCode = myPortCountryCode;
                    entityPM.Transshipment1ToPortCountryName = myPortCountryName;
                    entityPM.Transshipment1ToPortCountryEC = myPortCountryEC;
                }

                else {
                    entityPM.MainCarriageToPortId = myPortId;
                    entityPM.MainCarriageToPortCode = myPortCode;
                    entityPM.MainCarriageToPortName = myPortName;
                    entityPM.MainCarriageToPortCountryCode = myPortCountryCode;
                    entityPM.MainCarriageToPortCountryName = myPortCountryName;
                    entityPM.MainCarriageToPortCountryEC = myPortCountryEC;
                }
            }
        }
    }
    public static Transshipment3FromPortChanged(entityPM: ShipmentPM, list: PortList) {
        if (entityPM != null) {
            var myPortId: string = null;
            var myPortCode: string = null;
            var myPortName: string = null;
            var myPortCountryId: string = null;
            var myPortCountryCode: string = null;
            var myPortCountryName: string = null;
            var myPortCountryEC: boolean = false;
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
                if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2FromPortId)) {
                    entityPM.Transshipment2ToPortId = myPortId;
                    entityPM.Transshipment2ToPortCode = myPortCode;
                    entityPM.Transshipment2ToPortName = myPortName;
                    entityPM.Transshipment2ToPortCountryCode = myPortCountryCode;
                    entityPM.Transshipment2ToPortCountryName = myPortCountryName;
                    entityPM.Transshipment2ToPortCountryEC = myPortCountryEC;
                }

                else if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1FromPortId)) {
                    entityPM.Transshipment1ToPortId = myPortId;
                    entityPM.Transshipment1ToPortCode = myPortCode;
                    entityPM.Transshipment1ToPortName = myPortName;
                    entityPM.Transshipment1ToPortCountryCode = myPortCountryCode;
                    entityPM.Transshipment1ToPortCountryName = myPortCountryName;
                    entityPM.Transshipment1ToPortCountryEC = myPortCountryEC;
                }

                else {
                    entityPM.MainCarriageToPortId = myPortId;
                    entityPM.MainCarriageToPortCode = myPortCode;
                    entityPM.MainCarriageToPortName = myPortName;
                    entityPM.MainCarriageToPortCountryCode = myPortCountryCode;
                    entityPM.MainCarriageToPortCountryName = myPortCountryName;
                    entityPM.MainCarriageToPortCountryEC = myPortCountryEC;
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
                if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2FromPortId)) {
                    entityPM.Transshipment2ToPortId = entityPM.MainCarriageFinalDestinationPortId;;
                    entityPM.Transshipment2ToPortCode = entityPM.MainCarriageFinalDestinationPortCode;
                    entityPM.Transshipment2ToPortName = entityPM.MainCarriageFinalDestinationPortName;
                    entityPM.Transshipment2ToPortCountryCode = entityPM.MainCarriageFinalDestinationPortCountryCode;
                    entityPM.Transshipment2ToPortCountryName = entityPM.MainCarriageFinalDestinationPortCountryName;
                    //entityPM.Transshipment2ToPortCountryEC = entityPM.maincarriagefi
                }

                else if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1FromPortId)) {
                    entityPM.Transshipment1ToPortId = entityPM.MainCarriageFinalDestinationPortId;;
                    entityPM.Transshipment1ToPortCode = entityPM.MainCarriageFinalDestinationPortCode;
                    entityPM.Transshipment1ToPortName = entityPM.MainCarriageFinalDestinationPortName;
                    entityPM.Transshipment1ToPortCountryCode = entityPM.MainCarriageFinalDestinationPortCountryCode;
                    entityPM.Transshipment1ToPortCountryName = entityPM.MainCarriageFinalDestinationPortCountryName;
                    //entityPM.Transshipment1ToPortCountryEC = myPortCountryEC;
                }

                else {
                    entityPM.MainCarriageToPortId = entityPM.MainCarriageFinalDestinationPortId;;
                    entityPM.MainCarriageToPortCode = entityPM.MainCarriageFinalDestinationPortCode;
                    entityPM.MainCarriageToPortName = entityPM.MainCarriageFinalDestinationPortName;
                    entityPM.MainCarriageToPortCountryCode = entityPM.MainCarriageFinalDestinationPortCountryCode;
                    entityPM.MainCarriageToPortCountryName = entityPM.MainCarriageFinalDestinationPortCountryName;
                    //entityPM.MainCarriageToPortCountryEC = myPortCountryEC;
                }
            }




            //// Previous.To == this.From
            //if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3FromPortId)) {
            //    entityPM.Transshipment2ToPortId = entityPM.Transshipment3FromPortId;
            //    entityPM.Transshipment2ToPortCode = entityPM.Transshipment3FromPortCode;
            //    entityPM.Transshipment2ToPortName = entityPM.Transshipment3FromPortName;
            //    entityPM.Transshipment2ToPortCountryCode = entityPM.Transshipment3FromPortCountryCode;
            //    entityPM.Transshipment2ToPortCountryName = entityPM.Transshipment3FromPortCountryName;
            //    //entityPM.MainCarriageToPortCountryEC = entityPM.Transshipment3FromPortCountryEC;                    
            //}

            //else {
            //    entityPM.Transshipment2ToPortId = entityPM.MainCarriageFinalDestinationPortId;
            //    entityPM.Transshipment2ToPortCode = entityPM.MainCarriageFinalDestinationPortCode;
            //    entityPM.Transshipment2ToPortName = entityPM.MainCarriageFinalDestinationPortName;
            //    entityPM.Transshipment2ToPortCountryCode = entityPM.MainCarriageFinalDestinationPortCountryCode;
            //    entityPM.Transshipment2ToPortCountryName = entityPM.MainCarriageFinalDestinationPortCountryName;
            //    entityPM.Transshipment2ToPortCountryEC = entityPM.ToCountryIsEC;
            //}

            //// this.To == next.From
            //if (AppTool.IsNullOrEmpty(entityPM.Transshipment3FromPortId)) {
            //    entityPM.Transshipment3ToPortId = null;
            //    entityPM.Transshipment3ToPortCode = null;
            //    entityPM.Transshipment3ToPortName = null;
            //    entityPM.Transshipment3ToPortCountryCode = null;
            //    entityPM.Transshipment3ToPortCountryName = null;
            //}

            //else {
            //    entityPM.Transshipment3ToPortId = entityPM.MainCarriageFinalDestinationPortId;
            //    entityPM.Transshipment3ToPortCode = entityPM.MainCarriageFinalDestinationPortCode;
            //    entityPM.Transshipment3ToPortName = entityPM.MainCarriageFinalDestinationPortName;
            //    entityPM.Transshipment3ToPortCountryCode = entityPM.MainCarriageFinalDestinationPortCountryCode;
            //    entityPM.Transshipment3ToPortCountryName = entityPM.MainCarriageFinalDestinationPortCountryName;
            //}
        }
    }
    public static FinalDestinationPortChanged(entityPM: ShipmentPM, list: PortList) {
        if (entityPM != null) {
            var myPortId: string = null;
            var myPortCode: string = null;
            var myPortName: string = null;
            var myPortCountryId: string = null;
            var myPortCountryCode: string = null;
            var myPortCountryName: string = null;
            var myPortCountryEC: boolean = false;
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
            if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3FromPortId)) {
                entityPM.Transshipment3ToPortId = myPortId;
                entityPM.Transshipment3ToPortCode = myPortCode;
                entityPM.Transshipment3ToPortName = myPortName;
                entityPM.Transshipment3ToPortCountryCode = myPortCountryCode;
                entityPM.Transshipment3ToPortCountryName = myPortCountryName;
                entityPM.Transshipment3ToPortCountryEC = myPortCountryEC;
            }

            else if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2FromPortId)) {
                entityPM.Transshipment2ToPortId = myPortId;
                entityPM.Transshipment2ToPortCode = myPortCode;
                entityPM.Transshipment2ToPortName = myPortName;
                entityPM.Transshipment2ToPortCountryCode = myPortCountryCode;
                entityPM.Transshipment2ToPortCountryName = myPortCountryName;
                entityPM.Transshipment2ToPortCountryEC = myPortCountryEC;
            }

            else if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1FromPortId)) {
                entityPM.Transshipment1ToPortId = myPortId;
                entityPM.Transshipment1ToPortCode = myPortCode;
                entityPM.Transshipment1ToPortName = myPortName;
                entityPM.Transshipment1ToPortCountryCode = myPortCountryCode;
                entityPM.Transshipment1ToPortCountryName = myPortCountryName;
                entityPM.Transshipment1ToPortCountryEC = myPortCountryEC;
            }

            else {
                entityPM.MainCarriageToPortId = myPortId;
                entityPM.MainCarriageToPortCode = myPortCode;
                entityPM.MainCarriageToPortName = myPortName;
                entityPM.MainCarriageToPortCountryCode = myPortCountryCode;
                entityPM.MainCarriageToPortCountryName = myPortCountryName;
                entityPM.MainCarriageToPortCountryEC = myPortCountryEC;
            }

            // next.From = this.To
            if (!AppTool.IsNullOrEmpty(entityPM.OnCarriageToPortId)) {
                entityPM.OnCarriageFromPortId = myPortId;
                entityPM.OnCarriageFromPortCode = myPortCode;
                entityPM.OnCarriageFromPortName = myPortName;
                entityPM.OnCarriageFromPortCountryCode = myPortCountryCode;
                entityPM.OnCarriageFromPortCountryName = myPortCountryName;
            }
        }
    }
    public static PreCarriageToPortChanged(entityPM: ShipmentPM, list: PortList) {
        if (entityPM != null) {
            var myPortId: string = null;
            var myPortCode: string = null;
            var myPortName: string = null;
            var myPortCountryId: string = null;
            var myPortCountryCode: string = null;
            var myPortCountryName: string = null;
            var myPortCountryEC: boolean = false;
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
            entityPM.PreCarriageToPortName = myPortName
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
            entityPM.MainCarriageFromPortCountryEC = myPortCountryEC;

            ShipmentTool.ComputeSCI(entityPM);
            ShipmentTool.BuildAWBPlaceField(entityPM);
        }
    }
    public static OnCarriageFromPortChanged(entityPM: ShipmentPM, list: PortList) {
        if (entityPM != null) {
            var myPortId: string = null;
            var myPortCode: string = null;
            var myPortName: string = null;
            var myPortCountryId: string = null;
            var myPortCountryCode: string = null;
            var myPortCountryName: string = null;
            var myPortCountryEC: boolean = false;
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
            entityPM.OnCarriageFromPortId = myPortId;
            entityPM.OnCarriageFromPortCode = myPortCode;
            entityPM.OnCarriageFromPortName = myPortName
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
                entityPM.MainCarriageToPortCountryEC = myPortCountryEC;
            }

            entityPM.ToCountryId = myPortCountryId;
            entityPM.ToCountryIsEC = myPortCountryEC;
            entityPM.FinalDistenationPortId = myPortId;
            entityPM.MainCarriageFinalDestinationPortId = myPortId;
            entityPM.MainCarriageFinalDestinationPortCode = myPortCode;
            entityPM.MainCarriageFinalDestinationPortName = myPortName;
            entityPM.MainCarriageFinalDestinationPortCountryCode = myPortCountryCode;
            entityPM.MainCarriageFinalDestinationPortCountryName = myPortCountryName;
        }
    }

    public static MainCarriageCarrierChanged(entityPM: ShipmentPM, list: CardList) {
        var myCardCode: string = null;
        var myCardName: string = null;
        var myCardPrefix: string = null;
        var myCardWebSite: string = null;

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
    }
    public static Transshipment1CarrierChanged(entityPM: ShipmentPM, list: CardList) {
        var myCardCode: string = null;
        var myCardName: string = null;
        var myCardPrefix: string = null;
        var myCardWebSite: string = null;

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
    }
    public static Transshipment2CarrierChanged(entityPM: ShipmentPM, list: CardList) {
        var myCardCode: string = null;
        var myCardName: string = null;
        var myCardPrefix: string = null;
        var myCardWebSite: string = null;

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
    }
    public static Transshipment3CarrierChanged(entityPM: ShipmentPM, list: CardList) {
        var myCardCode: string = null;
        var myCardName: string = null;
        var myCardPrefix: string = null;
        var myCardWebSite: string = null;

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
    }

    public static RemovePreCarriageLeg(entityPM: ShipmentPM) {
        if (entityPM.HasPreCarriage == true) {
            entityPM.HasPreCarriage = false;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.PreCarriageTransportModeId)) {
            entityPM.PreCarriageTransportModeId = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.PreCarriageCarrierId)) {
            entityPM.PreCarriageCarrierId = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.PreCarriageCarrierCode)) {
            entityPM.PreCarriageCarrierCode = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.PreCarriageCarrierName)) {
            entityPM.PreCarriageCarrierName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.PreCarriageCarrierWebSite)) {
            entityPM.PreCarriageCarrierWebSite = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.PreCarriageCarrierNumber)) {
            entityPM.PreCarriageCarrierNumber = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.PreCarriageVesselId)) {
            entityPM.PreCarriageVesselId = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.PreCarriageVesselName)) {
            entityPM.PreCarriageVesselName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.PreCarriageFromPortId)) {
            entityPM.PreCarriageFromPortId = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.PreCarriageFromPortCode)) {
            entityPM.PreCarriageFromPortCode = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.PreCarriageFromPortName)) {
            entityPM.PreCarriageFromPortName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.PreCarriageFromPortCountryCode)) {
            entityPM.PreCarriageFromPortCountryCode = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.PreCarriageFromPortCountryName)) {
            entityPM.PreCarriageFromPortCountryName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.PreCarriageToPortId)) {
            entityPM.PreCarriageToPortId = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.PreCarriageToPortCode)) {
            entityPM.PreCarriageToPortCode = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.PreCarriageToPortName)) {
            entityPM.PreCarriageToPortName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.PreCarriageToPortCountryCode)) {
            entityPM.PreCarriageToPortCountryCode = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.PreCarriageToPortCountryName)) {
            entityPM.PreCarriageToPortCountryName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.PreCarriageETD)) {
            entityPM.PreCarriageETD = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.PreCarriageETA)) {
            entityPM.PreCarriageETA = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.PreCarriageATD)) {
            entityPM.PreCarriageATD = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.PreCarriageATA)) {
            entityPM.PreCarriageATA = null;
        }

        var followups = entityPM.FollowUps.filter(f => f.LegType != null);
        followups = followups.filter(f => f.LegType.indexOf("PreCarriage") > -1);

        if (followups.length > 0) {
            followups.forEach(item => {
                entityPM.RemoveShipmentFollowUp(item);
            });

            this.CurrentSession.FireEvent("FollowupsChanged");
        }
    }
    public static RemoveOnCarriageLeg(entityPM: ShipmentPM) {
        if (entityPM.HasOnCarriage == true) {
            entityPM.HasOnCarriage = false;
        }

        if (!AppTool.IsNullOrEmpty(entityPM.OnCarriageTransportModeId)) {
            entityPM.OnCarriageTransportModeId = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.OnCarriageCarrierId)) {
            entityPM.OnCarriageCarrierId = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.OnCarriageCarrierCode)) {
            entityPM.OnCarriageCarrierCode = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.OnCarriageCarrierName)) {
            entityPM.OnCarriageCarrierName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.OnCarriageCarrierWebSite)) {
            entityPM.OnCarriageCarrierWebSite = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.OnCarriageCarrierNumber)) {
            entityPM.OnCarriageCarrierNumber = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.OnCarriageVesselId)) {
            entityPM.OnCarriageVesselId = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.OnCarriageVesselName)) {
            entityPM.OnCarriageVesselName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.OnCarriageFromPortId)) {
            entityPM.OnCarriageFromPortId = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.OnCarriageFromPortCode)) {
            entityPM.OnCarriageFromPortCode = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.OnCarriageFromPortName)) {
            entityPM.OnCarriageFromPortName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.OnCarriageFromPortCountryCode)) {
            entityPM.OnCarriageFromPortCountryCode = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.OnCarriageFromPortCountryName)) {
            entityPM.OnCarriageFromPortCountryName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.OnCarriageToPortId)) {
            entityPM.OnCarriageToPortId = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.OnCarriageToPortCode)) {
            entityPM.OnCarriageToPortCode = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.OnCarriageToPortName)) {
            entityPM.OnCarriageToPortName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.OnCarriageToPortCountryCode)) {
            entityPM.OnCarriageToPortCountryCode = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.OnCarriageToPortCountryName)) {
            entityPM.OnCarriageToPortCountryName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.OnCarriageETD)) {
            entityPM.OnCarriageETD = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.OnCarriageETA)) {
            entityPM.OnCarriageETA = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.OnCarriageATD)) {
            entityPM.OnCarriageATD = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.OnCarriageATA)) {
            entityPM.OnCarriageATA = null;
        }

        var followups = entityPM.FollowUps.filter(f => f.LegType != null);
        followups = followups.filter(f => f.LegType.indexOf("OnCarriage") > -1);

        if (followups.length > 0) {
            followups.forEach(item => {
                entityPM.RemoveShipmentFollowUp(item);
            });

            this.CurrentSession.FireEvent("FollowupsChanged");
        }
    }
    public static RemoveTransshipment1Leg(entityPM: ShipmentPM) {
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1CarrierId)) {
            entityPM.Transshipment1CarrierId = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1CarrierCode)) {
            entityPM.Transshipment1CarrierCode = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1CarrierName)) {
            entityPM.Transshipment1CarrierName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1CarrierPrefix)) {
            entityPM.Transshipment1CarrierPrefix = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1CarrierWebSite)) {
            entityPM.Transshipment1CarrierWebSite = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1CarrierNumber)) {
            entityPM.Transshipment1CarrierNumber = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1FullCarrierNumber)) {
            entityPM.Transshipment1FullCarrierNumber = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1AdditionalMAWBOBLBL)) {
            entityPM.Transshipment1AdditionalMAWBOBLBL = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1VesselId)) {
            entityPM.Transshipment1VesselId = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1VesselName)) {
            entityPM.Transshipment1VesselName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1FromPortId)) {
            entityPM.Transshipment1FromPortId = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1FromPortCode)) {
            entityPM.Transshipment1FromPortCode = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1FromPortName)) {
            entityPM.Transshipment1FromPortName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1FromPortCountryCode)) {
            entityPM.Transshipment1FromPortCountryCode = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1FromPortCountryName)) {
            entityPM.Transshipment1FromPortCountryName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1ToPortId)) {
            entityPM.Transshipment1ToPortId = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1ToPortCode)) {
            entityPM.Transshipment1ToPortCode = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1ToPortName)) {
            entityPM.Transshipment1ToPortName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1ToPortCountryCode)) {
            entityPM.Transshipment1ToPortCountryCode = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1ToPortCountryName)) {
            entityPM.Transshipment1ToPortCountryName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1ETD)) {
            entityPM.Transshipment1ETD = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1ETA)) {
            entityPM.Transshipment1ETA = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1ATD)) {
            entityPM.Transshipment1ATD = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1ATA)) {
            entityPM.Transshipment1ATA = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1STD)) {
            entityPM.Transshipment1STD = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment1STA)) {
            entityPM.Transshipment1STA = null;
        }

        var followups = entityPM.FollowUps.filter(f => f.LegType != null);
        followups = followups.filter(f => f.LegType.indexOf("Transshipment1") > -1);

        if (followups.length > 0) {
            followups.forEach(item => {
                entityPM.RemoveShipmentFollowUp(item);
            });

            this.CurrentSession.FireEvent("FollowupsChanged");
        }
    }
    public static RemoveTransshipment2Leg(entityPM: ShipmentPM) {
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2CarrierId)) {
            entityPM.Transshipment2CarrierId = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2CarrierCode)) {
            entityPM.Transshipment2CarrierCode = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2CarrierName)) {
            entityPM.Transshipment2CarrierName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2CarrierPrefix)) {
            entityPM.Transshipment2CarrierPrefix = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2CarrierWebSite)) {
            entityPM.Transshipment2CarrierWebSite = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2CarrierNumber)) {
            entityPM.Transshipment2CarrierNumber = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2FullCarrierNumber)) {
            entityPM.Transshipment2FullCarrierNumber = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2AdditionalMAWBOBLBL)) {
            entityPM.Transshipment2AdditionalMAWBOBLBL = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2VesselId)) {
            entityPM.Transshipment2VesselId = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2VesselName)) {
            entityPM.Transshipment2VesselName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2FromPortId)) {
            entityPM.Transshipment2FromPortId = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2FromPortCode)) {
            entityPM.Transshipment2FromPortCode = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2FromPortName)) {
            entityPM.Transshipment2FromPortName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2FromPortCountryCode)) {
            entityPM.Transshipment2FromPortCountryCode = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2FromPortCountryName)) {
            entityPM.Transshipment2FromPortCountryName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2ToPortId)) {
            entityPM.Transshipment2ToPortId = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2ToPortCode)) {
            entityPM.Transshipment2ToPortCode = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2ToPortName)) {
            entityPM.Transshipment2ToPortName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2ToPortCountryCode)) {
            entityPM.Transshipment2ToPortCountryCode = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2ToPortCountryName)) {
            entityPM.Transshipment2ToPortCountryName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2ETD)) {
            entityPM.Transshipment2ETD = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2ETA)) {
            entityPM.Transshipment2ETA = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2ATD)) {
            entityPM.Transshipment2ATD = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2ATA)) {
            entityPM.Transshipment2ATA = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2STD)) {
            entityPM.Transshipment2STD = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment2STA)) {
            entityPM.Transshipment2STA = null;
        }

        var followups = entityPM.FollowUps.filter(f => f.LegType != null);
        followups = followups.filter(f => f.LegType.indexOf("Transshipment2") > -1);

        if (followups.length > 0) {
            followups.forEach(item => {
                entityPM.RemoveShipmentFollowUp(item);
            });

            this.CurrentSession.FireEvent("FollowupsChanged");
        }
    }
    public static RemoveTransshipment3Leg(entityPM: ShipmentPM) {
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3CarrierId)) {
            entityPM.Transshipment3CarrierId = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3CarrierCode)) {
            entityPM.Transshipment3CarrierCode = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3CarrierName)) {
            entityPM.Transshipment3CarrierName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3CarrierPrefix)) {
            entityPM.Transshipment3CarrierPrefix = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3CarrierWebSite)) {
            entityPM.Transshipment3CarrierWebSite = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3CarrierNumber)) {
            entityPM.Transshipment3CarrierNumber = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3FullCarrierNumber)) {
            entityPM.Transshipment3FullCarrierNumber = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3AdditionalMAWBOBLBL)) {
            entityPM.Transshipment3AdditionalMAWBOBLBL = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3VesselId)) {
            entityPM.Transshipment3VesselId = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3VesselName)) {
            entityPM.Transshipment3VesselName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3FromPortId)) {
            entityPM.Transshipment3FromPortId = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3FromPortCode)) {
            entityPM.Transshipment3FromPortCode = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3FromPortName)) {
            entityPM.Transshipment3FromPortName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3FromPortCountryCode)) {
            entityPM.Transshipment3FromPortCountryCode = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3FromPortCountryName)) {
            entityPM.Transshipment3FromPortCountryName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3ToPortId)) {
            entityPM.Transshipment3ToPortId = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3ToPortCode)) {
            entityPM.Transshipment3ToPortCode = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3ToPortName)) {
            entityPM.Transshipment3ToPortName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3ToPortCountryCode)) {
            entityPM.Transshipment3ToPortCountryCode = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3ToPortCountryName)) {
            entityPM.Transshipment3ToPortCountryName = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3ETD)) {
            entityPM.Transshipment3ETD = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3ETA)) {
            entityPM.Transshipment3ETA = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3ATD)) {
            entityPM.Transshipment3ATD = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3ATA)) {
            entityPM.Transshipment3ATA = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3STD)) {
            entityPM.Transshipment3STD = null;
        }
        if (!AppTool.IsNullOrEmpty(entityPM.Transshipment3STA)) {
            entityPM.Transshipment3STA = null;
        }

        var followups = entityPM.FollowUps.filter(f => f.LegType != null);
        followups = followups.filter(f => f.LegType.indexOf("Transshipment3") > -1);

        if (followups.length > 0) {
            followups.forEach(item => {
                entityPM.RemoveShipmentFollowUp(item);
            });

            this.CurrentSession.FireEvent("FollowupsChanged");
        }
    }

    public static ValidateRoutingsActualDates(entityPM: ShipmentPM, errors: string[]) {
        if (entityPM != null) {
            if (!entityPM.IsHybrid && !SessionLocator.TenantPM.IsDocumentsArchive) {

                if (errors == null) {
                    errors = [];
                }

                var message: string = DateTool.ActualDateMessage;

                // Pickups
                entityPM.ShipmentPickUps.forEach(itemPickup => {
                    if (!DateTool.IsActualDateValid(itemPickup.ATD)) {
                        errors.push(message.replace("Field", "Pickup ATD"));
                    }
                    if (!DateTool.IsActualDateValid(itemPickup.ATA)) {
                        errors.push(message.replace("Field", "Pickup ATA"));
                    }
                });

                // PreCarriage
                if (!DateTool.IsActualDateValid(entityPM.PreCarriageATD)) {
                    errors.push(message.replace("Field", "Pre Carriage ATD"));
                }
                if (!DateTool.IsActualDateValid(entityPM.PreCarriageATA)) {
                    errors.push(message.replace("Field", "Pre Carriage ATA"));
                }

                // Main
                if (!DateTool.IsActualDateValid(entityPM.MainCarriageATD)) {
                    var textCode: string = entityPM.TransportModeId == "O" ? "Shipment.O.Routings.MainCarriage" : "Shipment.O.Routings.MainCarriageLeg1";
                    var field: string = TextCodeTranslator.Translate(textCode) + " ATD";
                    errors.push(message.replace("Field", field));
                }
                if (!DateTool.IsActualDateValid(entityPM.MainCarriageATA)) {
                    var textCode: string = entityPM.TransportModeId == "O" ? "Shipment.O.Routings.MainCarriage" : "Shipment.O.Routings.MainCarriageLeg1";
                    var field: string = TextCodeTranslator.Translate(textCode) + " ATA";
                    errors.push(message.replace("Field", field));
                }

                // TR1
                if (!DateTool.IsActualDateValid(entityPM.Transshipment1ATD)) {
                    var textCode: string = entityPM.TransportModeId == "O" ? "Shipment.O.Routings.Transshipment1" : "Shipment.O.Routings.MainCarriageLeg2";
                    var field: string = TextCodeTranslator.Translate(textCode) + " ATD";
                    errors.push(message.replace("Field", field));
                }
                if (!DateTool.IsActualDateValid(entityPM.Transshipment1ATA)) {
                    var textCode: string = entityPM.TransportModeId == "O" ? "Shipment.O.Routings.Transshipment1" : "Shipment.O.Routings.MainCarriageLeg2";
                    var field: string = TextCodeTranslator.Translate(textCode) + " ATA";
                    errors.push(message.replace("Field", field));
                }

                // TR2
                if (!DateTool.IsActualDateValid(entityPM.Transshipment2ATD)) {
                    var textCode: string = entityPM.TransportModeId == "O" ? "Shipment.O.Routings.Transshipment2" : "Shipment.O.Routings.MainCarriageLeg3";
                    var field: string = TextCodeTranslator.Translate(textCode) + " ATD";
                    errors.push(message.replace("Field", field));
                }
                if (!DateTool.IsActualDateValid(entityPM.Transshipment2ATA)) {
                    var textCode: string = entityPM.TransportModeId == "O" ? "Shipment.O.Routings.Transshipment2" : "Shipment.O.Routings.MainCarriageLeg3";
                    var field: string = TextCodeTranslator.Translate(textCode) + " ATA";
                    errors.push(message.replace("Field", field));
                }

                // TR3
                if (!DateTool.IsActualDateValid(entityPM.Transshipment3ATD)) {
                    var textCode: string = entityPM.TransportModeId == "O" ? "Shipment.O.Routings.Transshipment3" : "Shipment.O.Routings.MainCarriageLeg4";
                    var field: string = TextCodeTranslator.Translate(textCode) + " ATD";
                    errors.push(message.replace("Field", field));
                }
                if (!DateTool.IsActualDateValid(entityPM.Transshipment3ATA)) {
                    var textCode: string = entityPM.TransportModeId == "O" ? "Shipment.O.Routings.Transshipment3" : "Shipment.O.Routings.MainCarriageLeg4";
                    var field: string = TextCodeTranslator.Translate(textCode) + " ATA";
                    errors.push(message.replace("Field", field));
                }

                // OnCarriage
                if (!DateTool.IsActualDateValid(entityPM.OnCarriageATD)) {
                    errors.push(message.replace("Field", "On Carriage ATD"));
                }
                if (!DateTool.IsActualDateValid(entityPM.OnCarriageATA)) {
                    errors.push(message.replace("Field", "On Carriage ATA"));
                }

                // Warehouse
                if (!DateTool.IsActualDateValid(entityPM.WarehouseLegActualReleaseDate)) {
                    errors.push(message.replace("Field", "Warehouse Release Date"));
                }
                if (!DateTool.IsActualDateValid(entityPM.WarehouseLegActualEntryDate)) {
                    errors.push(message.replace("Field", "Warehouse Entry Date"));
                }

                // Deliveries
                entityPM.ShipmentDeliveries.forEach(itemDelivery => {
                    if (!DateTool.IsActualDateValid(itemDelivery.ATD)) {
                        errors.push(message.replace("Field", "Delivery ATD"));
                    }
                    if (!DateTool.IsActualDateValid(itemDelivery.ATA)) {
                        errors.push(message.replace("Field", "Delivery ATA"));
                    }
                });
            }
        }
    }
    public static ValidateRoutingsSeriesDates(entityPM: ShipmentPM, errors: string[], legCode: string = null) {
        if (entityPM != null) {

            if (errors == null) {
                errors = [];
            }

            var PreCarriageErrors: string[] = [];
            var isPreCarriageExists: boolean = (entityPM.PreCarriageFromPortId != null && entityPM.PreCarriageToPortId != null) ? true : false;
            var PreCarriageETD: number = isPreCarriageExists ? DateTool.GetDateParts(entityPM.PreCarriageETD).DateTicks : 0;
            var PreCarriageETA: number = isPreCarriageExists ? DateTool.GetDateParts(entityPM.PreCarriageETA).DateTicks : 0;
            var PreCarriageATD: number = isPreCarriageExists ? DateTool.GetDateParts(entityPM.PreCarriageATD).DateTicks : 0;
            var PreCarriageATA: number = isPreCarriageExists ? DateTool.GetDateParts(entityPM.PreCarriageATA).DateTicks : 0;

            var MainCarriageErrors: string[] = [];
            var isMainCarriageExists: boolean = true;
            var MainCarriageETD: number = DateTool.GetDateParts(entityPM.MainCarriageETD).DateTicks;
            var MainCarriageETA: number = DateTool.GetDateParts(entityPM.MainCarriageETA).DateTicks;
            var MainCarriageATD: number = DateTool.GetDateParts(entityPM.MainCarriageATD).DateTicks;
            var MainCarriageATA: number = DateTool.GetDateParts(entityPM.MainCarriageATA).DateTicks;

            var isTransshipment1Exists: boolean = (entityPM.Transshipment1FromPortId != null && entityPM.Transshipment1ToPortId != null) ? true : false;
            var Transshipment1ETD: number = isTransshipment1Exists ? DateTool.GetDateParts(entityPM.Transshipment1ETD).DateTicks : 0;
            var Transshipment1ETA: number = isTransshipment1Exists ? DateTool.GetDateParts(entityPM.Transshipment1ETA).DateTicks : 0;
            var Transshipment1ATD: number = isTransshipment1Exists ? DateTool.GetDateParts(entityPM.Transshipment1ATD).DateTicks : 0;
            var Transshipment1ATA: number = isTransshipment1Exists ? DateTool.GetDateParts(entityPM.Transshipment1ATA).DateTicks : 0;

            var isTransshipment2Exists: boolean = (entityPM.Transshipment2FromPortId != null && entityPM.Transshipment2ToPortId != null) ? true : false;
            var Transshipment2ETD: number = isTransshipment2Exists ? DateTool.GetDateParts(entityPM.Transshipment2ETD).DateTicks : 0;
            var Transshipment2ETA: number = isTransshipment2Exists ? DateTool.GetDateParts(entityPM.Transshipment2ETA).DateTicks : 0;
            var Transshipment2ATD: number = isTransshipment2Exists ? DateTool.GetDateParts(entityPM.Transshipment2ATD).DateTicks : 0;
            var Transshipment2ATA: number = isTransshipment2Exists ? DateTool.GetDateParts(entityPM.Transshipment2ATA).DateTicks : 0;

            var isTransshipment3Exists: boolean = (entityPM.Transshipment3FromPortId != null && entityPM.Transshipment3ToPortId != null) ? true : false;
            var Transshipment3ETD: number = isTransshipment3Exists ? DateTool.GetDateParts(entityPM.Transshipment3ETD).DateTicks : 0;
            var Transshipment3ETA: number = isTransshipment3Exists ? DateTool.GetDateParts(entityPM.Transshipment3ETA).DateTicks : 0;
            var Transshipment3ATD: number = isTransshipment3Exists ? DateTool.GetDateParts(entityPM.Transshipment3ATD).DateTicks : 0;
            var Transshipment3ATA: number = isTransshipment3Exists ? DateTool.GetDateParts(entityPM.Transshipment3ATA).DateTicks : 0;

            var OnCarriageErrors: string[] = [];
            var isOnCarriageExists: boolean = (entityPM.OnCarriageFromPortId != null && entityPM.OnCarriageToPortId != null) ? true : false;
            var OnCarriageETD: number = isOnCarriageExists ? DateTool.GetDateParts(entityPM.OnCarriageETD).DateTicks : 0;
            var OnCarriageETA: number = isOnCarriageExists ? DateTool.GetDateParts(entityPM.OnCarriageETA).DateTicks : 0;
            var OnCarriageATD: number = isOnCarriageExists ? DateTool.GetDateParts(entityPM.OnCarriageATD).DateTicks : 0;
            var OnCarriageATA: number = isOnCarriageExists ? DateTool.GetDateParts(entityPM.OnCarriageATA).DateTicks : 0;

            var WarehouseLegErrors: string[] = [];
            var isWarehouseLegExists: boolean = (entityPM.WarehouseLegWarehouseId != null) ? true : false;
            var isWarehousePickupsLegExists = isWarehouseLegExists == true && entityPM.DirectionId != "I" ? true : false;
            var isWarehouseDeliveriesLegExists = isWarehouseLegExists == true && entityPM.DirectionId == "I" ? true : false;
            var WarehouseLegEED: number = isWarehouseLegExists ? DateTool.GetDateParts(entityPM.WarehouseLegExpectedEntryDate).DateTicks : 0;
            var WarehouseLegERD: number = isWarehouseLegExists ? DateTool.GetDateParts(entityPM.WarehouseLegExpectedReleaseDate).DateTicks : 0;
            var WarehouseLegAED: number = isWarehouseLegExists ? DateTool.GetDateParts(entityPM.WarehouseLegActualEntryDate).DateTicks : 0;
            var WarehouseLegARD: number = isWarehouseLegExists ? DateTool.GetDateParts(entityPM.WarehouseLegActualReleaseDate).DateTicks : 0;

            // Pickups
            var allPickupsETA: number = 0;
            var allPickupsATA: number = 0;
            var allPickupsErrors: string[] = [];
            var isPickupsExists: boolean = entityPM.ShipmentPickUps.length > 0 ? true : false;
            if (isPickupsExists) {
                entityPM.ShipmentPickUps.forEach(itemPickup => {
                    var ETD: number = DateTool.GetDateParts(itemPickup.ETD).DateTicks;
                    var ETA: number = DateTool.GetDateParts(itemPickup.ETA).DateTicks;
                    var ATD: number = DateTool.GetDateParts(itemPickup.ATD).DateTicks;
                    var ATA: number = DateTool.GetDateParts(itemPickup.ATA).DateTicks;

                    if (ETA > allPickupsETA) {
                        allPickupsETA = ETA;
                    }

                    if (ATA > allPickupsATA) {
                        allPickupsATA = ATA;
                    }

                    // Self
                    if (!this.IsRoutingLegDatesValid(ETD, ETA)) {
                        allPickupsErrors.push("Pick up expected departure must be less than pick up expected arrival");
                    }

                    if (!this.IsRoutingLegDatesValid(ATD, ATA)) {
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
                        if (this.IsDateSeriesBiggerNotEqual(ETA, WarehouseLegEED)) {
                            allPickupsErrors.push("Pick up expected arrival must be equal or less than Warehouse expected entry");
                        }

                        if (this.IsDateSeriesBiggerNotEqual(ATA, WarehouseLegAED)) {
                            allPickupsErrors.push("Pick up actual arrival must be equal or less than Warehouse actual entry");
                        }
                    }

                    else if (isPreCarriageExists) {
                        if (this.IsDateSeriesBigger(ETA, PreCarriageETD)) {
                            allPickupsErrors.push("Pick up expected arrival must be less than pre carriage expected departure");
                        }

                        if (this.IsDateSeriesBigger(ATA, PreCarriageATD)) {
                            allPickupsErrors.push("Pick up actual arrival must be less than pre carriage actual departure");
                        }
                    }

                    else if (isMainCarriageExists) {
                        if (this.IsDateSeriesBigger(ETA, MainCarriageETD)) {
                            allPickupsErrors.push("Pick up expected arrival must be less than main carriage expected departure");
                        }

                        if (this.IsDateSeriesBigger(ATA, MainCarriageATD)) {
                            allPickupsErrors.push("Pick up actual arrival must be less than main carriage actual departure");
                        }
                    }
                });
            }

            // Deliveries
            var allDeliveriesETD: number = 0;
            var allDeliveriesATD: number = 0;
            var allDeliveriesErrors: string[] = [];
            var isDeliveriesExists: boolean = entityPM.ShipmentDeliveries.length > 0 ? true : false;
            if (isDeliveriesExists) {
                entityPM.ShipmentDeliveries.forEach(itemDelivery => {
                    var ETD: number = DateTool.GetDateParts(itemDelivery.ETD).DateTicks;
                    var ETA: number = DateTool.GetDateParts(itemDelivery.ETA).DateTicks;
                    var ATD: number = DateTool.GetDateParts(itemDelivery.ATD).DateTicks;
                    var ATA: number = DateTool.GetDateParts(itemDelivery.ATA).DateTicks;

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
                    if (!this.IsRoutingLegDatesValid(ETD, ETA)) {
                        allDeliveriesErrors.push("Delivery expected departure must be less than Delivery expected arrival");
                    }

                    if (!this.IsRoutingLegDatesValid(ATD, ATA)) {
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
                        if (this.IsDateSeriesSmaller(ETD, WarehouseLegERD)) {
                            allDeliveriesErrors.push("Delivery expected departure must be bigger than Warehouse expected release");
                        }

                        if (this.IsDateSeriesSmaller(ATD, WarehouseLegARD)) {
                            allDeliveriesErrors.push("Delivery actual departure must be bigger than Warehouse actual release");
                        }
                    }

                    else if (isOnCarriageExists) {
                        if (this.IsDateSeriesSmaller(ETD, OnCarriageETA)) {
                            allDeliveriesErrors.push("Delivery expected departure must be bigger than On-Carriage expected arrival");
                        }

                        if (this.IsDateSeriesSmaller(ATD, OnCarriageATA)) {
                            allDeliveriesErrors.push("Delivery actual departure must be bigger than On-Carriage actual arrival");
                        }
                    }

                    else if (isTransshipment3Exists) {
                        if (this.IsDateSeriesSmaller(ETD, Transshipment3ETA)) {
                            allDeliveriesErrors.push("Delivery expected departure must be bigger than Transshipment3 expected arrival");
                        }

                        if (this.IsDateSeriesSmaller(ATD, Transshipment3ATA)) {
                            allDeliveriesErrors.push("Delivery actual departure must be bigger than Transshipment3 actual arrival");
                        }
                    }

                    else if (isTransshipment2Exists) {
                        if (this.IsDateSeriesSmaller(ETD, Transshipment2ETA)) {
                            allDeliveriesErrors.push("Delivery expected departure must be bigger than Transshipment2 expected arrival");
                        }

                        if (this.IsDateSeriesSmaller(ATD, Transshipment2ATA)) {
                            allDeliveriesErrors.push("Delivery actual departure must be bigger than Transshipment2 actual arrival");
                        }
                    }

                    else if (isTransshipment1Exists) {
                        if (this.IsDateSeriesSmaller(ETD, Transshipment1ETA)) {
                            allDeliveriesErrors.push("Delivery expected departure must be bigger than Transshipment1 expected arrival");
                        }

                        if (this.IsDateSeriesSmaller(ATD, Transshipment1ATA)) {
                            allDeliveriesErrors.push("Delivery actual departure must be bigger than Transshipment1 actual arrival");
                        }
                    }

                    else {
                        if (this.IsDateSeriesSmaller(ETD, MainCarriageETA)) {
                            allDeliveriesErrors.push("Delivery expected departure must be bigger than Main-Carriage expected arrival");
                        }

                        if (this.IsDateSeriesSmaller(ATD, MainCarriageATA)) {
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
                    PreCarriageErrors.forEach(item => {
                        errors.push(item);
                    });
                    break;
                }

                case "MainCarriage": {
                    MainCarriageErrors.forEach(item => {
                        errors.push(item);
                    });
                    break;
                }

                case "OnCarriage": {
                    OnCarriageErrors.forEach(item => {
                        errors.push(item);
                    });
                    break;
                }

                case "WarehouseLeg":
                case "WarehouseLeg_Pickups":
                    {
                        WarehouseLegErrors.forEach(item => {
                            errors.push(item);
                        });

                        break;
                    }

                default: {
                    allPickupsErrors.forEach(item => {
                        errors.push(item);
                    });

                    PreCarriageErrors.forEach(item => {
                        errors.push(item);
                    });

                    MainCarriageErrors.forEach(item => {
                        errors.push(item);
                    });

                    OnCarriageErrors.forEach(item => {
                        errors.push(item);
                    });

                    allDeliveriesErrors.forEach(item => {
                        errors.push(item);
                    });

                    WarehouseLegErrors.forEach(item => {
                        errors.push(item);
                    });

                    break;
                }
            }
        }
    }
    public static ValidateRoutingsSeriesDates_PackageFollowup(entityPM: ShipmentPM, myPackagePM: ShipmentPackagePM, errors: string[], legCode: string) {
        if (entityPM != null && myPackagePM != null) {

            if (errors == null) {
                errors = [];
            }

            var isPreCarriageExists: boolean = (entityPM.PreCarriageFromPortId != null && entityPM.PreCarriageToPortId != null) ? true : false;
            var PreCarriageETD: number = isPreCarriageExists ? DateTool.GetDateParts(entityPM.PreCarriageETD).DateTicks : 0;
            var PreCarriageETA: number = isPreCarriageExists ? DateTool.GetDateParts(entityPM.PreCarriageETA).DateTicks : 0;
            var PreCarriageATD: number = isPreCarriageExists ? DateTool.GetDateParts(entityPM.PreCarriageATD).DateTicks : 0;
            var PreCarriageATA: number = isPreCarriageExists ? DateTool.GetDateParts(entityPM.PreCarriageATA).DateTicks : 0;

            var isMainCarriageExists: boolean = true;
            var MainCarriageETD: number = DateTool.GetDateParts(entityPM.MainCarriageETD).DateTicks;
            var MainCarriageETA: number = DateTool.GetDateParts(entityPM.MainCarriageETA).DateTicks;
            var MainCarriageATD: number = DateTool.GetDateParts(entityPM.MainCarriageATD).DateTicks;
            var MainCarriageATA: number = DateTool.GetDateParts(entityPM.MainCarriageATA).DateTicks;

            var isTransshipment1Exists: boolean = (entityPM.Transshipment1FromPortId != null && entityPM.Transshipment1ToPortId != null) ? true : false;
            var Transshipment1ETD: number = isTransshipment1Exists ? DateTool.GetDateParts(entityPM.Transshipment1ETD).DateTicks : 0;
            var Transshipment1ETA: number = isTransshipment1Exists ? DateTool.GetDateParts(entityPM.Transshipment1ETA).DateTicks : 0;
            var Transshipment1ATD: number = isTransshipment1Exists ? DateTool.GetDateParts(entityPM.Transshipment1ATD).DateTicks : 0;
            var Transshipment1ATA: number = isTransshipment1Exists ? DateTool.GetDateParts(entityPM.Transshipment1ATA).DateTicks : 0;

            var isTransshipment2Exists: boolean = (entityPM.Transshipment2FromPortId != null && entityPM.Transshipment2ToPortId != null) ? true : false;
            var Transshipment2ETD: number = isTransshipment2Exists ? DateTool.GetDateParts(entityPM.Transshipment2ETD).DateTicks : 0;
            var Transshipment2ETA: number = isTransshipment2Exists ? DateTool.GetDateParts(entityPM.Transshipment2ETA).DateTicks : 0;
            var Transshipment2ATD: number = isTransshipment2Exists ? DateTool.GetDateParts(entityPM.Transshipment2ATD).DateTicks : 0;
            var Transshipment2ATA: number = isTransshipment2Exists ? DateTool.GetDateParts(entityPM.Transshipment2ATA).DateTicks : 0;

            var isTransshipment3Exists: boolean = (entityPM.Transshipment3FromPortId != null && entityPM.Transshipment3ToPortId != null) ? true : false;
            var Transshipment3ETD: number = isTransshipment3Exists ? DateTool.GetDateParts(entityPM.Transshipment3ETD).DateTicks : 0;
            var Transshipment3ETA: number = isTransshipment3Exists ? DateTool.GetDateParts(entityPM.Transshipment3ETA).DateTicks : 0;
            var Transshipment3ATD: number = isTransshipment3Exists ? DateTool.GetDateParts(entityPM.Transshipment3ATD).DateTicks : 0;
            var Transshipment3ATA: number = isTransshipment3Exists ? DateTool.GetDateParts(entityPM.Transshipment3ATA).DateTicks : 0;

            var isOnCarriageExists: boolean = (entityPM.OnCarriageFromPortId != null && entityPM.OnCarriageToPortId != null) ? true : false;
            var OnCarriageETD: number = isOnCarriageExists ? DateTool.GetDateParts(entityPM.OnCarriageETD).DateTicks : 0;
            var OnCarriageETA: number = isOnCarriageExists ? DateTool.GetDateParts(entityPM.OnCarriageETA).DateTicks : 0;
            var OnCarriageATD: number = isOnCarriageExists ? DateTool.GetDateParts(entityPM.OnCarriageATD).DateTicks : 0;
            var OnCarriageATA: number = isOnCarriageExists ? DateTool.GetDateParts(entityPM.OnCarriageATA).DateTicks : 0;


            // Package Follow up
            var ETD: number = 0;
            var ETA: number = 0;
            var ATD: number = 0;
            var ATA: number = 0;
            switch (legCode) {
                case "D": {
                    ETD = DateTool.GetDateParts(myPackagePM.DeliveryETD).DateTicks;
                    ETA = DateTool.GetDateParts(myPackagePM.DeliveryETA).DateTicks;
                    ATD = DateTool.GetDateParts(myPackagePM.DeliveryATD).DateTicks;
                    ATA = DateTool.GetDateParts(myPackagePM.DeliveryATA).DateTicks;
                    break;
                }

                case "R": {
                    ETD = DateTool.GetDateParts(myPackagePM.EmptyContainerReturnETD).DateTicks;
                    ETA = DateTool.GetDateParts(myPackagePM.EmptyContainerReturnETA).DateTicks;
                    ATD = DateTool.GetDateParts(myPackagePM.EmptyContainerReturnATD).DateTicks;
                    ATA = DateTool.GetDateParts(myPackagePM.EmptyContainerReturnATA).DateTicks;
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
    }

    public static IsRoutingLegDatesValid(Date1Ticks: any, Date2Ticks: any) {
        var myResult: boolean = true;

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
    }

    public static CompairDateSeries(Date1Ticks: number, Date2Ticks: number, Operator: string) {
        var myResult: boolean = false;

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
    }
    public static IsDateSeriesBigger(Date1Ticks: number, Date2Ticks: number) {
        var myResult: boolean = false;

        if (Date1Ticks != 0 && Date2Ticks != 0) {
            if (Date1Ticks >= Date2Ticks) {
                myResult = true;
            }
        }

        return myResult;
    }
    public static IsDateSeriesBiggerNotEqual(Date1Ticks: number, Date2Ticks: number) {
        var myResult: boolean = false;

        if (Date1Ticks != 0 && Date2Ticks != 0) {
            if (Date1Ticks > Date2Ticks) {
                myResult = true;
            }
        }

        return myResult;
    }
    public static IsDateSeriesSmaller(Date1Ticks: number, Date2Ticks: number) {
        var myResult: boolean = false;

        if (Date1Ticks != 0 && Date2Ticks != 0) {
            if (Date1Ticks <= Date2Ticks) {
                myResult = true;
            }
        }

        return myResult;
    }
    public static IsDateSeriesSmallerNotEqual(Date1Ticks: number, Date2Ticks: number) {
        var myResult: boolean = false;

        if (Date1Ticks != 0 && Date2Ticks != 0) {
            if (Date1Ticks < Date2Ticks) {
                myResult = true;
            }
        }

        return myResult;
    }
    public static ValidateLegDates(shipmentPM: ShipmentPM, pickUpPM: ShipmentPickUpPM, deliveryPM: ShipmentDeliveryPM, errors: string[], legCode: string)
    {
        if (!AppTool.IsNullOrEmpty(legCode)) {
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
            default: { break; }
        }
    }
    }     
}
