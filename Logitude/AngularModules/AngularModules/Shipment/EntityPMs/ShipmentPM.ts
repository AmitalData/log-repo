
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceLocator} from '../../Infrastructure/Locators/ServiceLocator';
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ShipmentFollowUpPM} from './ShipmentFollowUpPM';
import {AWBOCIPM} from './AWBOCIPM';
import {ShipmentPackagePM} from './ShipmentPackagePM';
import {ShipmentCommodityPM} from './ShipmentCommodityPM';
import {ShipmentOrderPackagePM} from './ShipmentOrderPackagePM';
import {ShipmentPayablePM} from './ShipmentPayablePM';
import {ShipmentReceivablePM} from './ShipmentReceivablePM';
import {ShipmentAWBPrintOnlyPM} from './ShipmentAWBPrintOnlyPM';
import {ConsoleShipmentPM} from './ConsoleShipmentPM';
import {ShipmentPickUpPM} from './ShipmentPickUpPM';
import {ShipmentDeliveryPM} from './ShipmentDeliveryPM';
import { ShipmentPMCustomCode } from '../EntityPMCustomCode/ShipmentPMCustomCode';
import { Output, EventEmitter } from '@angular/core';
import { PropertyChangedArgs } from '../../Infrastructure/EventEmitterArgs/PropertyChangedArgs';
import {CustomFieldClass} from '../../Infrastructure/DataContracts/CustomFieldClass';
import { ShipmentAssemblyPM } from './ShipmentAssemblyPM';
import { ShipmentStoragePricingPM } from './ShipmentStoragePricingPM';
import { ShipmentProductItemPM } from './ShipmentProductItemPM';
import { ShipmentUnassignedFieldPM } from './ShipmentUnassignedFieldPM';
import { CustomChildEntity } from '../../Infrastructure/EntityPMs/CustomChildEntity';
import { ShipmentAdditionalData } from '../DataContract/ShipmentAdditionalData';
import { ShipmentReferancePM } from './ShipmentReferancePM';
import { FreightForwarderReferencePM } from './FreightForwarderReferencePM';

export class ShipmentPM {
    public UIProperties: UIProperties;
    @Output() PropertyChanged: EventEmitter<PropertyChangedArgs> = new EventEmitter<PropertyChangedArgs>();
    constructor() {

        //for (var property in this) {
        //    if (this.hasOwnProperty(property)) {
        //        this[property] = null;
        //    }
        //}

        this.UIProperties = new UIProperties(this);
        this.IsDirty = false;
    }

    public IsDirty: boolean;


    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { if (this.id != newValue) { this.id = newValue; this.MarkAsDirty("Id"); } }

 
    private isHybrid: boolean;
    public get IsHybrid() { return this.isHybrid; }
    public set IsHybrid(newValue: boolean) { if (this.isHybrid != newValue) { this.isHybrid = newValue; this.MarkAsDirty("IsHybrid"); } }


    private ismultiUpdate: boolean;
    public get IsMultiUpdate() { return this.ismultiUpdate; }
    public set IsMultiUpdate(newValue: boolean) { if (this.ismultiUpdate != newValue) { this.ismultiUpdate = newValue; this.MarkAsDirty("IsMultiUpdate"); } }

    private isDocumentsNeedApprove: boolean;
    public get IsDocumentsNeedApprove() { return this.isDocumentsNeedApprove; }
    public set IsDocumentsNeedApprove(newValue: boolean) { if (this.isDocumentsNeedApprove != newValue) { this.isDocumentsNeedApprove = newValue; this.MarkAsDirty("IsDocumentsNeedApprove"); } }

    private baseShipmentNumber: string;
    public get BaseShipmentNumber() { return this.baseShipmentNumber; }
    public set BaseShipmentNumber(newValue: string) { if (this.baseShipmentNumber != newValue) { this.baseShipmentNumber = newValue; this.MarkAsDirty("BaseShipmentNumber"); } }

    private concurrencyGUID: string;
    public get ConcurrencyGUID() { return this.concurrencyGUID; }
    public set ConcurrencyGUID(newValue: string) { if (this.concurrencyGUID != newValue) { this.concurrencyGUID = newValue; this.MarkAsDirty("ConcurrencyGUID"); } }


    private cASSCode: string;
    public get CASSCode() { return this.cASSCode; }
    public set CASSCode(newValue: string) { if (this.cASSCode != newValue) { this.cASSCode = newValue; this.MarkAsDirty("CASSCode"); } }


    private deliveryOrder: string;
    public get DeliveryOrder() { return this.deliveryOrder; }
    public set DeliveryOrder(newValue: string) { if (this.deliveryOrder != newValue) { this.deliveryOrder = newValue; this.MarkAsDirty("DeliveryOrder"); } }


    private importManifest: string;
    public get ImportManifest() { return this.importManifest; }
    public set ImportManifest(newValue: string) { if (this.importManifest != newValue) { this.importManifest = newValue; this.MarkAsDirty("ImportManifest"); } }


    private freightLocationId: string;
    public get FreightLocationId() { return this.freightLocationId; }
    public set FreightLocationId(newValue: string) { if (this.freightLocationId != newValue) { this.freightLocationId = newValue; this.MarkAsDirty("FreightLocationId"); } }


    private transportDocumentNumber: string;
    public get TransportDocumentNumber() { return this.transportDocumentNumber; }
    public set TransportDocumentNumber(newValue: string) { if (this.transportDocumentNumber != newValue) { this.transportDocumentNumber = newValue; this.MarkAsDirty("TransportDocumentNumber"); } }


    private carrierTransportDocumentNumber: string;
    public get CarrierTransportDocumentNumber() { return this.carrierTransportDocumentNumber; }
    public set CarrierTransportDocumentNumber(newValue: string) { if (this.carrierTransportDocumentNumber != newValue) { this.carrierTransportDocumentNumber = newValue; this.MarkAsDirty("CarrierTransportDocumentNumber"); } }

    private notInvoicedReceivablesAmount: number;
    public get NotInvoicedReceivablesAmount() { return this.notInvoicedReceivablesAmount; }
    public set NotInvoicedReceivablesAmount(newValue: number) { if (this.notInvoicedReceivablesAmount != newValue) { this.notInvoicedReceivablesAmount = newValue; this.MarkAsDirty("NotInvoicedReceivablesAmount"); } }


    private openReceivablesInLocalCurrency: number;
    public get OpenReceivablesInLocalCurrency() { return this.openReceivablesInLocalCurrency; }
    public set OpenReceivablesInLocalCurrency(newValue: number) { if (this.openReceivablesInLocalCurrency != newValue) { this.openReceivablesInLocalCurrency = newValue; this.MarkAsDirty("OpenReceivablesInLocalCurrency"); } }


    private accountedReceivablesInLocalCurrency: number;
    public get AccountedReceivablesInLocalCurrency() { return this.accountedReceivablesInLocalCurrency; }
    public set AccountedReceivablesInLocalCurrency(newValue: number) { if (this.accountedReceivablesInLocalCurrency != newValue) { this.accountedReceivablesInLocalCurrency = newValue; this.MarkAsDirty("AccountedReceivablesInLocalCurrency"); } }


    private profitInLocalCurrency: number;
    public get ProfitInLocalCurrency() { return this.profitInLocalCurrency; }
    public set ProfitInLocalCurrency(newValue: number) { if (this.profitInLocalCurrency != newValue) { this.profitInLocalCurrency = newValue; this.MarkAsDirty("ProfitInLocalCurrency"); } }


    private estimateProfitInLocalCurrency: number;
    public get EstimateProfitInLocalCurrency() { return this.estimateProfitInLocalCurrency; }
    public set EstimateProfitInLocalCurrency(newValue: number) { if (this.estimateProfitInLocalCurrency != newValue) { this.estimateProfitInLocalCurrency = newValue; this.MarkAsDirty("EstimateProfitInLocalCurrency"); } }


    private openReceivablesInProfitCurrency: number;
    public get OpenReceivablesInProfitCurrency() { return this.openReceivablesInProfitCurrency; }
    public set OpenReceivablesInProfitCurrency(newValue: number) { if (this.openReceivablesInProfitCurrency != newValue) { this.openReceivablesInProfitCurrency = newValue; this.MarkAsDirty("OpenReceivablesInProfitCurrency"); } }


    private accountedReceivablesInProfitCurrency: number;
    public get AccountedReceivablesInProfitCurrency() { return this.accountedReceivablesInProfitCurrency; }
    public set AccountedReceivablesInProfitCurrency(newValue: number) { if (this.accountedReceivablesInProfitCurrency != newValue) { this.accountedReceivablesInProfitCurrency = newValue; this.MarkAsDirty("AccountedReceivablesInProfitCurrency"); } }


    private profitInProfitCurrency: number;
    public get ProfitInProfitCurrency() { return this.profitInProfitCurrency; }
    public set ProfitInProfitCurrency(newValue: number) { if (this.profitInProfitCurrency != newValue) { this.profitInProfitCurrency = newValue; this.MarkAsDirty("ProfitInProfitCurrency"); } }


    private estimateProfitInProfitCurrency: number;
    public get EstimateProfitInProfitCurrency() { return this.estimateProfitInProfitCurrency; }
    public set EstimateProfitInProfitCurrency(newValue: number) { if (this.estimateProfitInProfitCurrency != newValue) { this.estimateProfitInProfitCurrency = newValue; this.MarkAsDirty("EstimateProfitInProfitCurrency"); } }


    private openPayablesInLocalCurrency: number;
    public get OpenPayablesInLocalCurrency() { return this.openPayablesInLocalCurrency; }
    public set OpenPayablesInLocalCurrency(newValue: number) { if (this.openPayablesInLocalCurrency != newValue) { this.openPayablesInLocalCurrency = newValue; this.MarkAsDirty("OpenPayablesInLocalCurrency"); } }


    private accountedPayablesInLocalCurrency: number;
    public get AccountedPayablesInLocalCurrency() { return this.accountedPayablesInLocalCurrency; }
    public set AccountedPayablesInLocalCurrency(newValue: number) { if (this.accountedPayablesInLocalCurrency != newValue) { this.accountedPayablesInLocalCurrency = newValue; this.MarkAsDirty("AccountedPayablesInLocalCurrency"); } }


    private openPayablesInProfitCurrency: number;
    public get OpenPayablesInProfitCurrency() { return this.openPayablesInProfitCurrency; }
    public set OpenPayablesInProfitCurrency(newValue: number) { if (this.openPayablesInProfitCurrency != newValue) { this.openPayablesInProfitCurrency = newValue; this.MarkAsDirty("OpenPayablesInProfitCurrency"); } }


    private accountedPayablesInProfitCurrency: number;
    public get AccountedPayablesInProfitCurrency() { return this.accountedPayablesInProfitCurrency; }
    public set AccountedPayablesInProfitCurrency(newValue: number) { if (this.accountedPayablesInProfitCurrency != newValue) { this.accountedPayablesInProfitCurrency = newValue; this.MarkAsDirty("AccountedPayablesInProfitCurrency"); } }


    private countryForStatisticsId: string;
    public get CountryForStatisticsId() { return this.countryForStatisticsId; }
    public set CountryForStatisticsId(newValue: string) { if (this.countryForStatisticsId != newValue) { this.countryForStatisticsId = newValue; this.MarkAsDirty("CountryForStatisticsId"); } }


    private mainCarriageCarrierId: string;
    public get MainCarriageCarrierId() { return this.mainCarriageCarrierId; }
    public set MainCarriageCarrierId(newValue: string) { if (this.mainCarriageCarrierId != newValue) { this.mainCarriageCarrierId = newValue; this.MarkAsDirty("MainCarriageCarrierId"); } }

    private mainCarriageCarrierName:string;
    public get MainCarriageCarrierName() { return this.mainCarriageCarrierName; }
    public set MainCarriageCarrierName(newValue: string) { if (this.mainCarriageCarrierName != newValue) { this.mainCarriageCarrierName = newValue; this.MarkAsDirty("MainCarriageCarrierName"); } }
       
	 
    private mainCarriageCarrierCode: string;
    public get MainCarriageCarrierCode() { return this.mainCarriageCarrierCode; }
    public set MainCarriageCarrierCode(newValue: string) { if (this.mainCarriageCarrierCode != newValue) { this.mainCarriageCarrierCode = newValue; this.MarkAsDirty("MainCarriageCarrierCode"); } }
       
	 
    private mainCarriageCarrierNumber: string;
    public get MainCarriageCarrierNumber() { return this.mainCarriageCarrierNumber; }
    public set MainCarriageCarrierNumber(newValue: string) { if (this.mainCarriageCarrierNumber != newValue) { this.mainCarriageCarrierNumber = newValue; this.MarkAsDirty("MainCarriageCarrierNumber"); } }
       
	 
    private mainCarriageCarrierAddressId: string;
    public get MainCarriageCarrierAddressId() { return this.mainCarriageCarrierAddressId; }
    public set MainCarriageCarrierAddressId(newValue: string) { if (this.mainCarriageCarrierAddressId != newValue) { this.mainCarriageCarrierAddressId = newValue; this.MarkAsDirty("MainCarriageCarrierAddressId"); } }
       
	 
    private mainCarriageCarrierWebSite: string;
    public get MainCarriageCarrierWebSite() { return this.mainCarriageCarrierWebSite; }
    public set MainCarriageCarrierWebSite(newValue: string) { if (this.mainCarriageCarrierWebSite != newValue) { this.mainCarriageCarrierWebSite = newValue; this.MarkAsDirty("MainCarriageCarrierWebSite"); } }
       
	 
   
    private isFSRSent: boolean;
    public get IsFSRSent() { return this.isFSRSent; }
    public set IsFSRSent(newValue: boolean) { if (this.isFSRSent != newValue) { this.isFSRSent = newValue; this.MarkAsDirty("IsFSRSent"); } }
       
	 
    private lastFSRStatusRequestDate: Date;
    public get LastFSRStatusRequestDate() { return this.lastFSRStatusRequestDate; }
    public set LastFSRStatusRequestDate(newValue: Date) { if (this.lastFSRStatusRequestDate != newValue) { this.lastFSRStatusRequestDate = newValue; this.MarkAsDirty("LastFSRStatusRequestDate"); } }
       
	  
    private fHLStatusDate: Date;
    public get FHLStatusDate() { return this.fHLStatusDate; }
    public set FHLStatusDate(newValue: Date) { if (this.fHLStatusDate != newValue) { this.fHLStatusDate = newValue; this.MarkAsDirty("FHLStatusDate"); } }
       
	 
    private fWBStatusDate: Date;
    public get FWBStatusDate() { return this.fWBStatusDate; }
    public set FWBStatusDate(newValue: Date) { if (this.fWBStatusDate != newValue) { this.fWBStatusDate = newValue; this.MarkAsDirty("FWBStatusDate"); } }
       
	 
    private carrierLastStatusCode: string;
    public get CarrierLastStatusCode() { return this.carrierLastStatusCode; }
    public set CarrierLastStatusCode(newValue: string) { if (this.carrierLastStatusCode != newValue) { this.carrierLastStatusCode = newValue; this.MarkAsDirty("CarrierLastStatusCode"); } }
       
	 
    private carrierLastStatusName: string;
    public get CarrierLastStatusName() { return this.carrierLastStatusName; }
    public set CarrierLastStatusName(newValue: string) { if (this.carrierLastStatusName != newValue) { this.carrierLastStatusName = newValue; this.MarkAsDirty("CarrierLastStatusName"); } }
       
	 
    private carrierLastStatusDate: Date;
    public get CarrierLastStatusDate() { return this.carrierLastStatusDate; }
    public set CarrierLastStatusDate(newValue: Date) { if (this.carrierLastStatusDate != newValue) { this.carrierLastStatusDate = newValue; this.MarkAsDirty("CarrierLastStatusDate"); } }
       
	 
    private fNAReason: string;
    public get FNAReason() { return this.fNAReason; }
    public set FNAReason(newValue: string) { if (this.fNAReason != newValue) { this.fNAReason = newValue; this.MarkAsDirty("FNAReason"); } }
       
	 
    private aWBSpecialHandlingCodeId1: string;
    public get AWBSpecialHandlingCodeId1() { return this.aWBSpecialHandlingCodeId1; }
    public set AWBSpecialHandlingCodeId1(newValue: string) { if (this.aWBSpecialHandlingCodeId1 != newValue) { this.aWBSpecialHandlingCodeId1 = newValue; this.MarkAsDirty("AWBSpecialHandlingCodeId1"); } }
       
	 
    private aWBSpecialHandlingCodeId2: string;
    public get AWBSpecialHandlingCodeId2() { return this.aWBSpecialHandlingCodeId2; }
    public set AWBSpecialHandlingCodeId2(newValue: string) { if (this.aWBSpecialHandlingCodeId2 != newValue) { this.aWBSpecialHandlingCodeId2 = newValue; this.MarkAsDirty("AWBSpecialHandlingCodeId2"); } }
       
	 
    private aWBSpecialHandlingCodeId3: string;
    public get AWBSpecialHandlingCodeId3() { return this.aWBSpecialHandlingCodeId3; }
    public set AWBSpecialHandlingCodeId3(newValue: string) { if (this.aWBSpecialHandlingCodeId3 != newValue) { this.aWBSpecialHandlingCodeId3 = newValue; this.MarkAsDirty("AWBSpecialHandlingCodeId3"); } }
       
	 
    private aWBSpecialHandlingCodeId4: string;
    public get AWBSpecialHandlingCodeId4() { return this.aWBSpecialHandlingCodeId4; }
    public set AWBSpecialHandlingCodeId4(newValue: string) { if (this.aWBSpecialHandlingCodeId4 != newValue) { this.aWBSpecialHandlingCodeId4 = newValue; this.MarkAsDirty("AWBSpecialHandlingCodeId4"); } }
       
	 
    private aWBSpecialHandlingCodeId5: string;
    public get AWBSpecialHandlingCodeId5() { return this.aWBSpecialHandlingCodeId5; }
    public set AWBSpecialHandlingCodeId5(newValue: string) { if (this.aWBSpecialHandlingCodeId5 != newValue) { this.aWBSpecialHandlingCodeId5 = newValue; this.MarkAsDirty("AWBSpecialHandlingCodeId5"); } }
       
	 
    private aWBSpecialHandlingCodeId6: string;
    public get AWBSpecialHandlingCodeId6() { return this.aWBSpecialHandlingCodeId6; }
    public set AWBSpecialHandlingCodeId6(newValue: string) { if (this.aWBSpecialHandlingCodeId6 != newValue) { this.aWBSpecialHandlingCodeId6 = newValue; this.MarkAsDirty("AWBSpecialHandlingCodeId6"); } }
       
	 
    private aWBSpecialHandlingCodeId7: string;
    public get AWBSpecialHandlingCodeId7() { return this.aWBSpecialHandlingCodeId7; }
    public set AWBSpecialHandlingCodeId7(newValue: string) { if (this.aWBSpecialHandlingCodeId7 != newValue) { this.aWBSpecialHandlingCodeId7 = newValue; this.MarkAsDirty("AWBSpecialHandlingCodeId7"); } }
       
	 
    private aWBSpecialHandlingCodeId8: string;
    public get AWBSpecialHandlingCodeId8() { return this.aWBSpecialHandlingCodeId8; }
    public set AWBSpecialHandlingCodeId8(newValue: string) { if (this.aWBSpecialHandlingCodeId8 != newValue) { this.aWBSpecialHandlingCodeId8 = newValue; this.MarkAsDirty("AWBSpecialHandlingCodeId8"); } }
       
	 
    private aWBSpecialHandlingCodeId9: string;
    public get AWBSpecialHandlingCodeId9() { return this.aWBSpecialHandlingCodeId9; }
    public set AWBSpecialHandlingCodeId9(newValue: string) { if (this.aWBSpecialHandlingCodeId9 != newValue) { this.aWBSpecialHandlingCodeId9 = newValue; this.MarkAsDirty("AWBSpecialHandlingCodeId9"); } }
       
	 
    private aWBChargeRate: number;
    public get AWBChargeRate() { return this.aWBChargeRate; }
    public set AWBChargeRate(newValue: number) { if (this.aWBChargeRate != newValue) { this.aWBChargeRate = newValue; this.MarkAsDirty("AWBChargeRate"); } }
       
	 
    private aWBChargeAmount: number;
    public get AWBChargeAmount() { return this.aWBChargeAmount; }
    public set AWBChargeAmount(newValue: number) { if (this.aWBChargeAmount != newValue) { this.aWBChargeAmount = newValue; this.MarkAsDirty("AWBChargeAmount"); } }
       
	 
    private aWBCommodityItemNumber: string;
    public get AWBCommodityItemNumber() { return this.aWBCommodityItemNumber; }
    public set AWBCommodityItemNumber(newValue: string) { if (this.aWBCommodityItemNumber != newValue) { this.aWBCommodityItemNumber = newValue; this.MarkAsDirty("AWBCommodityItemNumber"); } }
       
	 
    private pPCC: string;
    public get PPCC() { return this.pPCC; }
    public set PPCC(newValue: string) { if (this.pPCC != newValue) { this.pPCC = newValue; this.MarkAsDirty("PPCC"); } }
       
	 
    private flightDate: Date;
    public get FlightDate() { return this.flightDate; }
    public set FlightDate(newValue: Date) { if (this.flightDate != newValue) { this.flightDate = newValue; this.MarkAsDirty("FlightDate"); } }
       
	 
    private isFlightDateActual: boolean;
    public get IsFlightDateActual() { return this.isFlightDateActual; }
    public set IsFlightDateActual(newValue: boolean) { if (this.isFlightDateActual != newValue) { this.isFlightDateActual = newValue; this.MarkAsDirty("IsFlightDateActual"); } }
       
	 
    private connectedShipments: number;
    public get ConnectedShipments() { return this.connectedShipments; }
    public set ConnectedShipments(newValue: number) { if (this.connectedShipments != newValue) { this.connectedShipments = newValue; this.MarkAsDirty("ConnectedShipments"); } }
       
	 
    private masterShipmentNumber: string;
    public get MasterShipmentNumber() { return this.masterShipmentNumber; }
    public set MasterShipmentNumber(newValue: string) { if (this.masterShipmentNumber != newValue) { this.masterShipmentNumber = newValue; this.MarkAsDirty("MasterShipmentNumber"); } }
       
	 
    private chargeableWeightInKG: number;
    public get ChargeableWeightInKG() { return this.chargeableWeightInKG; }
    public set ChargeableWeightInKG(newValue: number) { if (this.chargeableWeightInKG != newValue) { this.chargeableWeightInKG = newValue; this.MarkAsDirty("ChargeableWeightInKG"); } }
       
	 
    private grossWeightInKG: number;
    public get GrossWeightInKG() { return this.grossWeightInKG; }
    public set GrossWeightInKG(newValue: number) { if (this.grossWeightInKG != newValue) { this.grossWeightInKG = newValue; this.MarkAsDirty("GrossWeightInKG"); } }
       

    private grossWeightPerStorageDays: number;
    public get GrossWeightPerStorageDays() { return this.grossWeightPerStorageDays; }
    public set GrossWeightPerStorageDays(newValue: number) { if (this.grossWeightPerStorageDays != newValue) { this.grossWeightPerStorageDays = newValue; this.MarkAsDirty("GrossWeightPerStorageDays"); } }


    private chargeableWeight: number;
    public get ChargeableWeight() { return this.chargeableWeight; }
    public set ChargeableWeight(newValue: number) { if (this.chargeableWeight != newValue) { this.chargeableWeight = newValue; this.MarkAsDirty("ChargeableWeight"); } }
       
	 
    private grossWeight: number;
    public get GrossWeight() { return this.grossWeight; }
    public set GrossWeight(newValue: number) { if (this.grossWeight != newValue) { this.grossWeight = newValue; this.MarkAsDirty("GrossWeight"); } }
       
	 
    private currentUserId: string;
    public get CurrentUserId() { return this.currentUserId; }
    public set CurrentUserId(newValue: string) { if (this.currentUserId != newValue) { this.currentUserId = newValue; this.MarkAsDirty("CurrentUserId"); } }
       
	 
    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { if (this.tenant != newValue) { this.tenant = newValue; this.MarkAsDirty("Tenant"); } }
       
	 
    private basketId: string;
    public get BasketId() { return this.basketId; }
    public set BasketId(newValue: string) { if (this.basketId != newValue) { this.basketId = newValue; this.MarkAsDirty("BasketId"); } }
       
	 
    private shipmentNumber: string;
    public get ShipmentNumber() { return this.shipmentNumber; }
    public set ShipmentNumber(newValue: string) { if (this.shipmentNumber != newValue) { this.shipmentNumber = newValue; this.MarkAsDirty("ShipmentNumber"); } }
       
	 
    private directionId: string;
    public get DirectionId() { return this.directionId; }
    public set DirectionId(newValue: string) { if (this.directionId != newValue) { this.directionId = newValue; this.MarkAsDirty("DirectionId"); } }
       
	 
    private directionName: string;
    public get DirectionName() { return this.directionName; }
    public set DirectionName(newValue: string) { if (this.directionName != newValue) { this.directionName = newValue; this.MarkAsDirty("DirectionName"); } }
       
	 
    private transportModeId: string;
    public get TransportModeId() { return this.transportModeId; }
    public set TransportModeId(newValue: string) { if (this.transportModeId != newValue) { this.transportModeId = newValue; this.MarkAsDirty("TransportModeId"); } }
       
	 
    private transportModeName: string;
    public get TransportModeName() { return this.transportModeName; }
    public set TransportModeName(newValue: string) { if (this.transportModeName != newValue) { this.transportModeName = newValue; this.MarkAsDirty("TransportModeName"); } }
       
	 
    private shipmentTypeId: string;
    public get ShipmentTypeId() { return this.shipmentTypeId; }
    public set ShipmentTypeId(newValue: string) { if (this.shipmentTypeId != newValue) { this.shipmentTypeId = newValue; this.MarkAsDirty("ShipmentTypeId"); } }
       
	 
    private shipmentTypeName: string;
    public get ShipmentTypeName() { return this.shipmentTypeName; }
    public set ShipmentTypeName(newValue: string) { if (this.shipmentTypeName != newValue) { this.shipmentTypeName = newValue; this.MarkAsDirty("ShipmentTypeName"); } }
       
	 
    private house: string;
    public get House() { return this.house; }
    public set House(newValue: string) { if (this.house != newValue) { this.house = newValue; this.MarkAsDirty("House"); } }
       
	 
    private createDateTime: Date;
    public get CreateDateTime() { return this.createDateTime; }
    public set CreateDateTime(newValue: Date) { if (this.createDateTime != newValue) { this.createDateTime = newValue; this.MarkAsDirty("CreateDateTime"); } }
       
	 
    private mAWBTakenFromStack: boolean;
    public get MAWBTakenFromStack() { return this.mAWBTakenFromStack; }
    public set MAWBTakenFromStack(newValue: boolean) { if (this.mAWBTakenFromStack != newValue) { this.mAWBTakenFromStack = newValue; this.MarkAsDirty("MAWBTakenFromStack"); } }
       
	 
    private mAWBReturnedToStack: boolean;
    public get MAWBReturnedToStack() { return this.mAWBReturnedToStack; }
    public set MAWBReturnedToStack(newValue: boolean) { if (this.mAWBReturnedToStack != newValue) { this.mAWBReturnedToStack = newValue; this.MarkAsDirty("MAWBReturnedToStack"); } }
       
	 
    private branchId: string;
    public get BranchId() { return this.branchId; }
    public set BranchId(newValue: string) { if (this.branchId != newValue) { this.branchId = newValue; this.MarkAsDirty("BranchId"); } }
       
	 
    private branchName: string;
    public get BranchName() { return this.branchName; }
    public set BranchName(newValue: string) { if (this.branchName != newValue) { this.branchName = newValue; this.MarkAsDirty("BranchName"); } }
       
	 
    private branchAddress: string;
    public get BranchAddress() { return this.branchAddress; }
    public set BranchAddress(newValue: string) { if (this.branchAddress != newValue) { this.branchAddress = newValue; this.MarkAsDirty("BranchAddress"); } }
       
	 
    private incotermId: string;
    public get IncotermId() { return this.incotermId; }
    public set IncotermId(newValue: string) { if (this.incotermId != newValue) { this.incotermId = newValue; this.MarkAsDirty("IncotermId"); } }
       
	 
    private incotermCode: string;
    public get IncotermCode() { return this.incotermCode; }
    public set IncotermCode(newValue: string) { if (this.incotermCode != newValue) { this.incotermCode = newValue; this.MarkAsDirty("IncotermCode"); } }
       
	 
    private incotermName: string;
    public get IncotermName() { return this.incotermName; }
    public set IncotermName(newValue: string) { if (this.incotermName != newValue) { this.incotermName = newValue; this.MarkAsDirty("IncotermName"); } }
       
	 
    private routing: string;
    public get Routing() { return this.routing; }
    public set Routing(newValue: string) { if (this.routing != newValue) { this.routing = newValue; this.MarkAsDirty("Routing"); } }
       
	 
    private salesmanUserId: string;
    public get SalesmanUserId() { return this.salesmanUserId; }
    public set SalesmanUserId(newValue: string) { if (this.salesmanUserId != newValue) { this.salesmanUserId = newValue; this.MarkAsDirty("SalesmanUserId"); } }
       
	 
    private salesmanUserName: string;
    public get SalesmanUserName() { return this.salesmanUserName; }
    public set SalesmanUserName(newValue: string) { if (this.salesmanUserName != newValue) { this.salesmanUserName = newValue; this.MarkAsDirty("SalesmanUserName"); } }
       
	 
    private createdByUserId: string;
    public get CreatedByUserId() { return this.createdByUserId; }
    public set CreatedByUserId(newValue: string) { if (this.createdByUserId != newValue) { this.createdByUserId = newValue; this.MarkAsDirty("CreatedByUserId"); } }
       
	 
    private departmentId: string;
    public get DepartmentId() { return this.departmentId; }
    public set DepartmentId(newValue: string) { if (this.departmentId != newValue) { this.departmentId = newValue; this.MarkAsDirty("DepartmentId"); } }
       
	 
    private notes: string;
    public get Notes() { return this.notes; }
    public set Notes(newValue: string) { if (this.notes != newValue) { this.notes = newValue; this.MarkAsDirty("Notes"); } }
      
    private notesSharedWithCustomer: string;
    public get NotesSharedWithCustomer() { return this.notesSharedWithCustomer; }
    public set NotesSharedWithCustomer(newValue: string) { if (this.notesSharedWithCustomer != newValue) { this.notesSharedWithCustomer = newValue; this.MarkAsDirty("Notes"); } }
       
	 
    private descriptionOfGoods: string;
    public get DescriptionOfGoods() { return this.descriptionOfGoods; }
    public set DescriptionOfGoods(newValue: string) { if (this.descriptionOfGoods != newValue) { this.descriptionOfGoods = newValue; this.MarkAsDirty("DescriptionOfGoods"); } }
       
	 
    private lastModified: string;
    public get LastModified() { return this.lastModified; }
    public set LastModified(newValue: string) { if (this.lastModified != newValue) { this.lastModified = newValue; this.MarkAsDirty("LastModified"); } }
       
	 
    private hAWBDate: Date;
    public get HAWBDate() { return this.hAWBDate; }
    public set HAWBDate(newValue: Date) { if (this.hAWBDate != newValue) { this.hAWBDate = newValue; this.MarkAsDirty("HAWBDate"); } }
       
	 
    private mAWBStackNumber: string;
    public get MAWBStackNumber() { return this.mAWBStackNumber; }
    public set MAWBStackNumber(newValue: string) { if (this.mAWBStackNumber != newValue) { this.mAWBStackNumber = newValue; this.MarkAsDirty("MAWBStackNumber"); } }
       
	 
    private isOperationalClosed: boolean;
    public get IsOperationalClosed() { return this.isOperationalClosed; }
    public set IsOperationalClosed(newValue: boolean) { if (this.isOperationalClosed != newValue) { this.isOperationalClosed = newValue; this.MarkAsDirty("IsOperationalClosed"); } }
       
	 
    private field1: CustomFieldClass;
    public get Field1() {if(!this.field1){ this.field1 = new CustomFieldClass(null, "Field1", "Shipment");} return this.field1; }
    public set Field1(newValue: CustomFieldClass) {  this.field1 = newValue; this.MarkAsDirty("Field1");  }
       
	 
    private field2: CustomFieldClass;
    public get Field2() {if(!this.field2){ this.field2 = new CustomFieldClass(null, "Field2", "Shipment");} return this.field2; }
    public set Field2(newValue: CustomFieldClass) {  this.field2 = newValue; this.MarkAsDirty("Field2");  }
       
	 
    private field3: CustomFieldClass;
    public get Field3() {if(!this.field3){ this.field3 = new CustomFieldClass(null, "Field3", "Shipment");} return this.field3; }
    public set Field3(newValue: CustomFieldClass) {  this.field3 = newValue; this.MarkAsDirty("Field3");  }
       
	 
    private field4: CustomFieldClass;
    public get Field4() {if(!this.field4){ this.field4 = new CustomFieldClass(null, "Field4", "Shipment");} return this.field4; }
    public set Field4(newValue: CustomFieldClass) {  this.field4 = newValue; this.MarkAsDirty("Field4");  }
       
	 
    private field5: CustomFieldClass;
    public get Field5() {if(!this.field5){ this.field5 = new CustomFieldClass(null, "Field5", "Shipment");} return this.field5; }
    public set Field5(newValue: CustomFieldClass) {  this.field5 = newValue; this.MarkAsDirty("Field5");  }
       
	 
    private field6: CustomFieldClass;
    public get Field6() {if(!this.field6){ this.field6 = new CustomFieldClass(null, "Field6", "Shipment");} return this.field6; }
    public set Field6(newValue: CustomFieldClass) {  this.field6 = newValue; this.MarkAsDirty("Field6");  }
       
	 
    private field7: CustomFieldClass;
    public get Field7() {if(!this.field7){ this.field7 = new CustomFieldClass(null, "Field7", "Shipment");} return this.field7; }
    public set Field7(newValue: CustomFieldClass) {  this.field7 = newValue; this.MarkAsDirty("Field7");  }
       
	 
    private field8: CustomFieldClass;
    public get Field8() {if(!this.field8){ this.field8 = new CustomFieldClass(null, "Field8", "Shipment");} return this.field8; }
    public set Field8(newValue: CustomFieldClass) {  this.field8 = newValue; this.MarkAsDirty("Field8");  }
       
	 
    private field9: CustomFieldClass;
    public get Field9() {if(!this.field9){ this.field9 = new CustomFieldClass(null, "Field9", "Shipment");} return this.field9; }
    public set Field9(newValue: CustomFieldClass) {  this.field9 = newValue; this.MarkAsDirty("Field9");  }
       
	 
    private field10: CustomFieldClass;
    public get Field10() {if(!this.field10){ this.field10 = new CustomFieldClass(null, "Field10", "Shipment");} return this.field10; }
    public set Field10(newValue: CustomFieldClass) {  this.field10 = newValue; this.MarkAsDirty("Field10");  }
       
	 
    private field11: CustomFieldClass;
    public get Field11() {if(!this.field11){ this.field11 = new CustomFieldClass(null, "Field11", "Shipment");} return this.field11; }
    public set Field11(newValue: CustomFieldClass) {  this.field11 = newValue; this.MarkAsDirty("Field11");  }
       
	 
    private field12: CustomFieldClass;
    public get Field12() {if(!this.field12){ this.field12 = new CustomFieldClass(null, "Field12", "Shipment");} return this.field12; }
    public set Field12(newValue: CustomFieldClass) {  this.field12 = newValue; this.MarkAsDirty("Field12");  }
       
	 
    private field13: CustomFieldClass;
    public get Field13() {if(!this.field13){ this.field13 = new CustomFieldClass(null, "Field13", "Shipment");} return this.field13; }
    public set Field13(newValue: CustomFieldClass) {  this.field13 = newValue; this.MarkAsDirty("Field13");  }
       
	 
    private field14: CustomFieldClass;
    public get Field14() {if(!this.field14){ this.field14 = new CustomFieldClass(null, "Field14", "Shipment");} return this.field14; }
    public set Field14(newValue: CustomFieldClass) {  this.field14 = newValue; this.MarkAsDirty("Field14");  }
       
	 
    private field15: CustomFieldClass;
    public get Field15() {if(!this.field15){ this.field15 = new CustomFieldClass(null, "Field15", "Shipment");} return this.field15; }
    public set Field15(newValue: CustomFieldClass) {  this.field15 = newValue; this.MarkAsDirty("Field15");  }
       
	 
    private field16: CustomFieldClass;
    public get Field16() {if(!this.field16){ this.field16 = new CustomFieldClass(null, "Field16", "Shipment");} return this.field16; }
    public set Field16(newValue: CustomFieldClass) {  this.field16 = newValue; this.MarkAsDirty("Field16");  }
       
	 
    private field17: CustomFieldClass;
    public get Field17() {if(!this.field17){ this.field17 = new CustomFieldClass(null, "Field17", "Shipment");} return this.field17; }
    public set Field17(newValue: CustomFieldClass) {  this.field17 = newValue; this.MarkAsDirty("Field17");  }
       
	 
    private field18: CustomFieldClass;
    public get Field18() {if(!this.field18){ this.field18 = new CustomFieldClass(null, "Field18", "Shipment");} return this.field18; }
    public set Field18(newValue: CustomFieldClass) {  this.field18 = newValue; this.MarkAsDirty("Field18");  }
       
	 
    private field19: CustomFieldClass;
    public get Field19() {if(!this.field19){ this.field19 = new CustomFieldClass(null, "Field19", "Shipment");} return this.field19; }
    public set Field19(newValue: CustomFieldClass) {  this.field19 = newValue; this.MarkAsDirty("Field19");  }
       
    private field20: CustomFieldClass;
    public get Field20() {if(!this.field20){ this.field20 = new CustomFieldClass(null, "Field20", "Shipment");} return this.field20; }
    public set Field20(newValue: CustomFieldClass) { this.field20 = newValue; this.MarkAsDirty("Field20"); }

    private field21: CustomFieldClass;
    public get Field21() { if (!this.field21) { this.field21 = new CustomFieldClass(null, "Field21", "Shipment"); } return this.field21; }
    public set Field21(newValue: CustomFieldClass) { this.field21 = newValue; this.MarkAsDirty("Field21"); }

    private field22: CustomFieldClass;
    public get Field22() { if (!this.field22) { this.field22 = new CustomFieldClass(null, "Field22", "Shipment"); } return this.field22; }
    public set Field22(newValue: CustomFieldClass) { this.field22 = newValue; this.MarkAsDirty("Field22"); }

    private field23: CustomFieldClass;
    public get Field23() { if (!this.field23) { this.field23 = new CustomFieldClass(null, "Field23", "Shipment"); } return this.field23; }
    public set Field23(newValue: CustomFieldClass) { this.field23 = newValue; this.MarkAsDirty("Field23"); }

    private field24: CustomFieldClass;
    public get Field24() { if (!this.field24) { this.field24 = new CustomFieldClass(null, "Field24", "Shipment"); } return this.field24; }
    public set Field24(newValue: CustomFieldClass) { this.field24 = newValue; this.MarkAsDirty("Field24"); }

    private field25: CustomFieldClass;
    public get Field25() { if (!this.field25) { this.field25 = new CustomFieldClass(null, "Field25", "Shipment"); } return this.field25; }
    public set Field25(newValue: CustomFieldClass) { this.field25 = newValue; this.MarkAsDirty("Field25"); }

    private field26: CustomFieldClass;
    public get Field26() { if (!this.field26) { this.field26 = new CustomFieldClass(null, "Field26", "Shipment"); } return this.field26; }
    public set Field26(newValue: CustomFieldClass) { this.field26 = newValue; this.MarkAsDirty("Field26"); }

    private field27: CustomFieldClass;
    public get Field27() { if (!this.field27) { this.field27 = new CustomFieldClass(null, "Field27", "Shipment"); } return this.field27; }
    public set Field27(newValue: CustomFieldClass) { this.field27 = newValue; this.MarkAsDirty("Field27"); }

    private field28: CustomFieldClass;
    public get Field28() { if (!this.field28) { this.field28 = new CustomFieldClass(null, "Field28", "Shipment"); } return this.field28; }
    public set Field28(newValue: CustomFieldClass) { this.field28 = newValue; this.MarkAsDirty("Field28"); }

    private field29: CustomFieldClass;
    public get Field29() { if (!this.field29) { this.field29 = new CustomFieldClass(null, "Field29", "Shipment"); } return this.field29; }
    public set Field29(newValue: CustomFieldClass) { this.field29 = newValue; this.MarkAsDirty("Field29"); }

    private field30: CustomFieldClass;
    public get Field30() { if (!this.field30) { this.field30 = new CustomFieldClass(null, "Field30", "Shipment"); } return this.field30; }
    public set Field30(newValue: CustomFieldClass) { this.field30 = newValue; this.MarkAsDirty("Field30"); }

    private field31: CustomFieldClass;
    public get Field31() { if (!this.field31) { this.field31 = new CustomFieldClass(null, "Field31", "Shipment"); } return this.field31; }
    public set Field31(newValue: CustomFieldClass) { this.field31 = newValue; this.MarkAsDirty("Field31"); }

    private field32: CustomFieldClass;
    public get Field32() { if (!this.field32) { this.field32 = new CustomFieldClass(null, "Field32", "Shipment"); } return this.field32; }
    public set Field32(newValue: CustomFieldClass) { this.field32 = newValue; this.MarkAsDirty("Field32"); }

    private field33: CustomFieldClass;
    public get Field33() { if (!this.field33) { this.field33 = new CustomFieldClass(null, "Field33", "Shipment"); } return this.field33; }
    public set Field33(newValue: CustomFieldClass) { this.field33 = newValue; this.MarkAsDirty("Field33"); }

    private field34: CustomFieldClass;
    public get Field34() { if (!this.field34) { this.field34 = new CustomFieldClass(null, "Field34", "Shipment"); } return this.field34; }
    public set Field34(newValue: CustomFieldClass) { this.field34 = newValue; this.MarkAsDirty("Field34"); }

    private field35: CustomFieldClass;
    public get Field35() { if (!this.field35) { this.field35 = new CustomFieldClass(null, "Field35", "Shipment"); } return this.field35; }
    public set Field35(newValue: CustomFieldClass) { this.field35 = newValue; this.MarkAsDirty("Field35"); }

    private field36: CustomFieldClass;
    public get Field36() { if (!this.field36) { this.field36 = new CustomFieldClass(null, "Field36", "Shipment"); } return this.field36; }
    public set Field36(newValue: CustomFieldClass) { this.field36 = newValue; this.MarkAsDirty("Field36"); }

    private field37: CustomFieldClass;
    public get Field37() { if (!this.field37) { this.field37 = new CustomFieldClass(null, "Field37", "Shipment"); } return this.field37; }
    public set Field37(newValue: CustomFieldClass) { this.field37 = newValue; this.MarkAsDirty("Field37"); }

    private field38: CustomFieldClass;
    public get Field38() { if (!this.field38) { this.field38 = new CustomFieldClass(null, "Field38", "Shipment"); } return this.field38; }
    public set Field38(newValue: CustomFieldClass) { this.field38 = newValue; this.MarkAsDirty("Field38"); }

    private field39: CustomFieldClass;
    public get Field39() { if (!this.field39) { this.field39 = new CustomFieldClass(null, "Field39", "Shipment"); } return this.field39; }
    public set Field39(newValue: CustomFieldClass) { this.field39 = newValue; this.MarkAsDirty("Field39"); }

    private field40: CustomFieldClass;
    public get Field40() { if (!this.field40) { this.field40 = new CustomFieldClass(null, "Field40", "Shipment"); } return this.field40; }
    public set Field40(newValue: CustomFieldClass) { this.field40 = newValue; this.MarkAsDirty("Field40"); }


    private field41: CustomFieldClass;
    public get Field41() { if (!this.field41) { this.field41 = new CustomFieldClass(null, "Field41", "Shipment"); } return this.field41; }
    public set Field41(newValue: CustomFieldClass) { this.field41 = newValue; this.MarkAsDirty("Field41"); }


    private field42: CustomFieldClass;
    public get Field42() { if (!this.field42) { this.field42 = new CustomFieldClass(null, "Field42", "Shipment"); } return this.field42; }
    public set Field42(newValue: CustomFieldClass) { this.field42 = newValue; this.MarkAsDirty("Field42"); }


    private field43: CustomFieldClass;
    public get Field43() { if (!this.field43) { this.field43 = new CustomFieldClass(null, "Field43", "Shipment"); } return this.field43; }
    public set Field43(newValue: CustomFieldClass) { this.field43 = newValue; this.MarkAsDirty("Field43"); }


    private field44: CustomFieldClass;
    public get Field44() { if (!this.field44) { this.field44 = new CustomFieldClass(null, "Field44", "Shipment"); } return this.field44; }
    public set Field44(newValue: CustomFieldClass) { this.field44 = newValue; this.MarkAsDirty("Field44"); }


    private field45: CustomFieldClass;
    public get Field45() { if (!this.field45) { this.field45 = new CustomFieldClass(null, "Field45", "Shipment"); } return this.field45; }
    public set Field45(newValue: CustomFieldClass) { this.field45 = newValue; this.MarkAsDirty("Field45"); }


    private field46: CustomFieldClass;
    public get Field46() { if (!this.field46) { this.field46 = new CustomFieldClass(null, "Field46", "Shipment"); } return this.field46; }
    public set Field46(newValue: CustomFieldClass) { this.field46 = newValue; this.MarkAsDirty("Field46"); }


    private field47: CustomFieldClass;
    public get Field47() { if (!this.field47) { this.field47 = new CustomFieldClass(null, "Field47", "Shipment"); } return this.field47; }
    public set Field47(newValue: CustomFieldClass) { this.field47 = newValue; this.MarkAsDirty("Field47"); }


    private field48: CustomFieldClass;
    public get Field48() { if (!this.field48) { this.field48 = new CustomFieldClass(null, "Field48", "Shipment"); } return this.field48; }
    public set Field48(newValue: CustomFieldClass) { this.field48 = newValue; this.MarkAsDirty("Field48"); }


    private field49: CustomFieldClass;
    public get Field49() { if (!this.field49) { this.field49 = new CustomFieldClass(null, "Field49", "Shipment"); } return this.field49; }
    public set Field49(newValue: CustomFieldClass) { this.field49 = newValue; this.MarkAsDirty("Field49"); }


    private field50: CustomFieldClass;
    public get Field50() { if (!this.field50) { this.field50 = new CustomFieldClass(null, "Field50", "Shipment"); } return this.field50; }
    public set Field50(newValue: CustomFieldClass) { this.field50 = newValue; this.MarkAsDirty("Field50"); }


    private field51: CustomFieldClass;
    public get Field51() { if (!this.field51) { this.field51 = new CustomFieldClass(null, "Field51", "Shipment"); } return this.field51; }
    public set Field51(newValue: CustomFieldClass) { this.field51 = newValue; this.MarkAsDirty("Field51"); }


    private field52: CustomFieldClass;
    public get Field52() { if (!this.field52) { this.field52 = new CustomFieldClass(null, "Field52", "Shipment"); } return this.field52; }
    public set Field52(newValue: CustomFieldClass) { this.field52 = newValue; this.MarkAsDirty("Field52"); }


    private field53: CustomFieldClass;
    public get Field53() { if (!this.field53) { this.field53 = new CustomFieldClass(null, "Field53", "Shipment"); } return this.field53; }
    public set Field53(newValue: CustomFieldClass) { this.field53 = newValue; this.MarkAsDirty("Field53"); }


    private field54: CustomFieldClass;
    public get Field54() { if (!this.field54) { this.field54 = new CustomFieldClass(null, "Field54", "Shipment"); } return this.field54; }
    public set Field54(newValue: CustomFieldClass) { this.field54 = newValue; this.MarkAsDirty("Field54"); }


    private field55: CustomFieldClass;
    public get Field55() { if (!this.field55) { this.field55 = new CustomFieldClass(null, "Field55", "Shipment"); } return this.field55; }
    public set Field55(newValue: CustomFieldClass) { this.field55 = newValue; this.MarkAsDirty("Field55"); }


    private field56: CustomFieldClass;
    public get Field56() { if (!this.field56) { this.field56 = new CustomFieldClass(null, "Field56", "Shipment"); } return this.field56; }
    public set Field56(newValue: CustomFieldClass) { this.field56 = newValue; this.MarkAsDirty("Field56"); }


    private field57: CustomFieldClass;
    public get Field57() { if (!this.field57) { this.field57 = new CustomFieldClass(null, "Field57", "Shipment"); } return this.field57; }
    public set Field57(newValue: CustomFieldClass) { this.field57 = newValue; this.MarkAsDirty("Field57"); }


    private field58: CustomFieldClass;
    public get Field58() { if (!this.field58) { this.field58 = new CustomFieldClass(null, "Field58", "Shipment"); } return this.field58; }
    public set Field58(newValue: CustomFieldClass) { this.field58 = newValue; this.MarkAsDirty("Field58"); }


    private field59: CustomFieldClass;
    public get Field59() { if (!this.field59) { this.field59 = new CustomFieldClass(null, "Field59", "Shipment"); } return this.field59; }
    public set Field59(newValue: CustomFieldClass) { this.field59 = newValue; this.MarkAsDirty("Field59"); }


    private field60: CustomFieldClass;
    public get Field60() { if (!this.field60) { this.field60 = new CustomFieldClass(null, "Field60", "Shipment"); } return this.field60; }
    public set Field60(newValue: CustomFieldClass) { this.field60 = newValue; this.MarkAsDirty("Field60"); }


    private field61: CustomFieldClass;
    public get Field61() { if (!this.field61) { this.field61 = new CustomFieldClass(null, "Field61", "Shipment"); } return this.field61; }
    public set Field61(newValue: CustomFieldClass) { this.field61 = newValue; this.MarkAsDirty("Field61"); }


    private field62: CustomFieldClass;
    public get Field62() { if (!this.field62) { this.field62 = new CustomFieldClass(null, "Field62", "Shipment"); } return this.field62; }
    public set Field62(newValue: CustomFieldClass) { this.field62 = newValue; this.MarkAsDirty("Field62"); }


    private field63: CustomFieldClass;
    public get Field63() { if (!this.field63) { this.field63 = new CustomFieldClass(null, "Field63", "Shipment"); } return this.field63; }
    public set Field63(newValue: CustomFieldClass) { this.field63 = newValue; this.MarkAsDirty("Field63"); }


    private field64: CustomFieldClass;
    public get Field64() { if (!this.field64) { this.field64 = new CustomFieldClass(null, "Field64", "Shipment"); } return this.field64; }
    public set Field64(newValue: CustomFieldClass) { this.field64 = newValue; this.MarkAsDirty("Field64"); }


    private field65: CustomFieldClass;
    public get Field65() { if (!this.field65) { this.field65 = new CustomFieldClass(null, "Field65", "Shipment"); } return this.field65; }
    public set Field65(newValue: CustomFieldClass) { this.field65 = newValue; this.MarkAsDirty("Field65"); }


    private field66: CustomFieldClass;
    public get Field66() { if (!this.field66) { this.field66 = new CustomFieldClass(null, "Field66", "Shipment"); } return this.field66; }
    public set Field66(newValue: CustomFieldClass) { this.field66 = newValue; this.MarkAsDirty("Field66"); }


    private field67: CustomFieldClass;
    public get Field67() { if (!this.field67) { this.field67 = new CustomFieldClass(null, "Field67", "Shipment"); } return this.field67; }
    public set Field67(newValue: CustomFieldClass) { this.field67 = newValue; this.MarkAsDirty("Field67"); }


    private field68: CustomFieldClass;
    public get Field68() { if (!this.field68) { this.field68 = new CustomFieldClass(null, "Field68", "Shipment"); } return this.field68; }
    public set Field68(newValue: CustomFieldClass) { this.field68 = newValue; this.MarkAsDirty("Field68"); }


    private field69: CustomFieldClass;
    public get Field69() { if (!this.field69) { this.field69 = new CustomFieldClass(null, "Field69", "Shipment"); } return this.field69; }
    public set Field69(newValue: CustomFieldClass) { this.field69 = newValue; this.MarkAsDirty("Field69"); }


    private field70: CustomFieldClass;
    public get Field70() { if (!this.field70) { this.field70 = new CustomFieldClass(null, "Field70", "Shipment"); } return this.field70; }
    public set Field70(newValue: CustomFieldClass) { this.field70 = newValue; this.MarkAsDirty("Field70"); }


    private searchFields: string;
    public get SearchFields() { return this.searchFields; }
    public set SearchFields(newValue: string) { if (this.searchFields != newValue) { this.searchFields = newValue; this.MarkAsDirty("SearchFields"); } }
       
	 
    private isSecured: boolean;
    public get IsSecured() { return this.isSecured; }
    public set IsSecured(newValue: boolean) { if (this.isSecured != newValue) { this.isSecured = newValue; this.MarkAsDirty("IsSecured"); } }
       
	 
    private master: string;
    public get Master() { return this.master; }
    public set Master(newValue: string) { if (this.master != newValue) { this.master = newValue; this.MarkAsDirty("Master"); } }
       
	 
    private shipmentTypeViewField: string;
    public get ShipmentTypeViewField() { return this.shipmentTypeViewField; }
    public set ShipmentTypeViewField(newValue: string) { if (this.shipmentTypeViewField != newValue) { this.shipmentTypeViewField = newValue; this.MarkAsDirty("ShipmentTypeViewField"); } }
       
	 
    private longMaster: string;
    public get LongMaster() { return this.longMaster; }
    public set LongMaster(newValue: string) { if (this.longMaster != newValue) { this.longMaster = newValue; this.MarkAsDirty("LongMaster"); } }
       
	 
    private freightPrepaidCollectId: string;
    public get FreightPrepaidCollectId() { return this.freightPrepaidCollectId; }
    public set FreightPrepaidCollectId(newValue: string) { if (this.freightPrepaidCollectId != newValue) { this.freightPrepaidCollectId = newValue; this.MarkAsDirty("FreightPrepaidCollectId"); } }
       
	 
    private otherPrepaidCollectId: string;
    public get OtherPrepaidCollectId() { return this.otherPrepaidCollectId; }
    public set OtherPrepaidCollectId(newValue: string) { if (this.otherPrepaidCollectId != newValue) { this.otherPrepaidCollectId = newValue; this.MarkAsDirty("OtherPrepaidCollectId"); } }
       
	 
    private grossWeightUnitCode: string;
    public get GrossWeightUnitCode() { return this.grossWeightUnitCode; }
    public set GrossWeightUnitCode(newValue: string) { if (this.grossWeightUnitCode != newValue) { this.grossWeightUnitCode = newValue; this.MarkAsDirty("GrossWeightUnitCode"); } }
       
	 
    private chargeableWeightUnitCode: string;
    public get ChargeableWeightUnitCode() { return this.chargeableWeightUnitCode; }
    public set ChargeableWeightUnitCode(newValue: string) { if (this.chargeableWeightUnitCode != newValue) { this.chargeableWeightUnitCode = newValue; this.MarkAsDirty("ChargeableWeightUnitCode"); } }
       
	 
    private dimensionsUnitCode: string;
    public get DimensionsUnitCode() { return this.dimensionsUnitCode; }
    public set DimensionsUnitCode(newValue: string) { if (this.dimensionsUnitCode != newValue) { this.dimensionsUnitCode = newValue; this.MarkAsDirty("DimensionsUnitCode"); } }
       
	 
    private rateClassCode: string;
    public get RateClassCode() { return this.rateClassCode; }
    public set RateClassCode(newValue: string) { if (this.rateClassCode != newValue) { this.rateClassCode = newValue; this.MarkAsDirty("RateClassCode"); } }
       
	 
    private volumetricWeight: number;
    public get VolumetricWeight() { return this.volumetricWeight; }
    public set VolumetricWeight(newValue: number) { if (this.volumetricWeight != newValue) { this.volumetricWeight = newValue; this.MarkAsDirty("VolumetricWeight"); } }
       
	 
    private volumeInCBM: number;
    public get VolumeInCBM() { return this.volumeInCBM; }
    public set VolumeInCBM(newValue: number) { if (this.volumeInCBM != newValue) { this.volumeInCBM = newValue; this.MarkAsDirty("VolumeInCBM"); } }
       
	 
    private volume: number;
    public get Volume() { return this.volume; }
    public set Volume(newValue: number) { if (this.volume != newValue) { this.volume = newValue; this.MarkAsDirty("Volume"); } }
       
	 
    private packagesQuantity: number;
    public get PackagesQuantity() { return this.packagesQuantity; }
    public set PackagesQuantity(newValue: number) { if (this.packagesQuantity != newValue) { this.packagesQuantity = newValue; this.MarkAsDirty("PackagesQuantity"); } }
       
	 
    private numberOfPackages: number;
    public get NumberOfPackages() { return this.numberOfPackages; }
    public set NumberOfPackages(newValue: number) { if (this.numberOfPackages != newValue) { this.numberOfPackages = newValue; this.MarkAsDirty("NumberOfPackages"); } }
       
	 
    private numberOfContainers: number;
    public get NumberOfContainers() { return this.numberOfContainers; }
    public set NumberOfContainers(newValue: number) { if (this.numberOfContainers != newValue) { this.numberOfContainers = newValue; this.MarkAsDirty("NumberOfContainers"); } }
       
	 
    private ratio: number;
    public get Ratio() { return this.ratio; }
    public set Ratio(newValue: number) { if (this.ratio != newValue) { this.ratio = newValue; this.MarkAsDirty("Ratio"); } }
       
	 
    private dimFactor: number;
    public get DimFactor() { return this.dimFactor; }
    public set DimFactor(newValue: number) { if (this.dimFactor != newValue) { this.dimFactor = newValue; this.MarkAsDirty("DimFactor"); } }
       
	 
    private mAWBOBLDate: Date;
    public get MAWBOBLDate() { return this.mAWBOBLDate; }
    public set MAWBOBLDate(newValue: Date) { if (this.mAWBOBLDate != newValue) { this.mAWBOBLDate = newValue; this.MarkAsDirty("MAWBOBLDate"); } }
       
	 
    private grossWeightEdited: boolean;
    public get GrossWeightEdited() { return this.grossWeightEdited; }
    public set GrossWeightEdited(newValue: boolean) { if (this.grossWeightEdited != newValue) { this.grossWeightEdited = newValue; this.MarkAsDirty("GrossWeightEdited"); } }
       
    private orderGrossWeightEdited: boolean;
    public get OrderGrossWeightEdited() { return this.orderGrossWeightEdited; }
    public set OrderGrossWeightEdited(newValue: boolean) { if (this.orderGrossWeightEdited != newValue) { this.orderGrossWeightEdited = newValue; this.MarkAsDirty("OrderGrossWeightEdited"); } }

    private chargeableWeightEdited: boolean;
    public get ChargeableWeightEdited() { return this.chargeableWeightEdited; }
    public set ChargeableWeightEdited(newValue: boolean) { if (this.chargeableWeightEdited != newValue) { this.chargeableWeightEdited = newValue; this.MarkAsDirty("ChargeableWeightEdited"); } }
       
    private orderChargeableWeightEdited: boolean;
    public get OrderChargeableWeightEdited() { return this.orderChargeableWeightEdited; }
    public set OrderChargeableWeightEdited(newValue: boolean) { if (this.orderChargeableWeightEdited != newValue) { this.orderChargeableWeightEdited = newValue; this.MarkAsDirty("OrderChargeableWeightEdited"); } }

    private volumeUnitCode: string;
    public get VolumeUnitCode() { return this.volumeUnitCode; }
    public set VolumeUnitCode(newValue: string) { if (this.volumeUnitCode != newValue) { this.volumeUnitCode = newValue; this.MarkAsDirty("VolumeUnitCode"); } }
       
	 
    private statusId: string;
    public get StatusId() { return this.statusId; }
    public set StatusId(newValue: string) { if (this.statusId != newValue) { this.statusId = newValue; this.MarkAsDirty("StatusId"); } }
       
	 
    private statusName: string;
    public get StatusName() { return this.statusName; }
    public set StatusName(newValue: string) { if (this.statusName != newValue) { this.statusName = newValue; this.MarkAsDirty("StatusName"); } }
       
	 
    private statusDate: Date;
    public get StatusDate() { return this.statusDate; }
    public set StatusDate(newValue: Date) { if (this.statusDate != newValue) { this.statusDate = newValue; this.MarkAsDirty("StatusDate"); } }
       
	 
    private statusLocation: string;
    public get StatusLocation() { return this.statusLocation; }
    public set StatusLocation(newValue: string) { if (this.statusLocation != newValue) { this.statusLocation = newValue; this.MarkAsDirty("PartialStatusAmount"); } }

    private partialStatusAmount: string;
    public get PartialStatusAmount() { return this.partialStatusAmount; }
    public set PartialStatusAmount(newValue: string) { if (this.partialStatusAmount != newValue) { this.partialStatusAmount = newValue; this.MarkAsDirty("StatusLocation"); } }

    private statusWeight: number;
    public get StatusWeight() { return this.statusWeight; }
    public set StatusWeight(newValue: number) { if (this.statusWeight != newValue) { this.statusWeight = newValue; this.MarkAsDirty("StatusWeight"); } }

    private quoteId: string;
    public get QuoteId() { return this.quoteId; }
    public set QuoteId(newValue: string) { if (this.quoteId != newValue) { this.quoteId = newValue; this.MarkAsDirty("QuoteId"); } }
       
	 
    private bookingId: string;
    public get BookingId() { return this.bookingId; }
    public set BookingId(newValue: string) { if (this.bookingId != newValue) { this.bookingId = newValue; this.MarkAsDirty("BookingId"); } }
       
	 
    private bookingNumber: string;
    public get BookingNumber() { return this.bookingNumber; }
    public set BookingNumber(newValue: string) { if (this.bookingNumber != newValue) { this.bookingNumber = newValue; this.MarkAsDirty("BookingNumber"); } }
       
	 
    private totalContainers: string;
    public get TotalContainers() { return this.totalContainers; }
    public set TotalContainers(newValue: string) { if (this.totalContainers != newValue) { this.totalContainers = newValue; this.MarkAsDirty("TotalContainers"); } }
       
	 
    private shipmentType: string;
    public get ShipmentType() { return this.shipmentType; }
    public set ShipmentType(newValue: string) { if (this.shipmentType != newValue) { this.shipmentType = newValue; this.MarkAsDirty("ShipmentType"); } }

 
    private shipmentPMId: string;
    public get ShipmentPMId() { return this.shipmentPMId; }
    public set ShipmentPMId(newValue: string) { if (this.shipmentPMId != newValue) { this.shipmentPMId = newValue; this.MarkAsDirty("ShipmentPMId"); } }
       
	 
    private lastUpdate: Date;
    public get LastUpdate() { return this.lastUpdate; }
    public set LastUpdate(newValue: Date) { if (this.lastUpdate != newValue) { this.lastUpdate = newValue; this.MarkAsDirty("LastUpdate"); } }
       
	 
    private newMessage: boolean;
    public get NewMessage() { return this.newMessage; }
    public set NewMessage(newValue: boolean) { if (this.newMessage != newValue) { this.newMessage = newValue; this.MarkAsDirty("NewMessage"); } }
       
	
    private isAnyConversation: boolean;
    public get IsAnyConversation() { return this.isAnyConversation; }
    public set IsAnyConversation(newValue: boolean) { if (this.isAnyConversation != newValue) { this.isAnyConversation = newValue; this.MarkAsDirty("IsAnyConversation"); } }
       
	 
    private numberOfShipments: number;
    public get NumberOfShipments() { return this.numberOfShipments; }
    public set NumberOfShipments(newValue: number) { if (this.numberOfShipments != newValue) { this.numberOfShipments = newValue; this.MarkAsDirty("NumberOfShipments"); } }
       
	 
    private mainCarriageFinalDestinationPortId: string;
    public get MainCarriageFinalDestinationPortId() { return this.mainCarriageFinalDestinationPortId; }
    public set MainCarriageFinalDestinationPortId(newValue: string) { if (this.mainCarriageFinalDestinationPortId != newValue) { this.mainCarriageFinalDestinationPortId = newValue; this.MarkAsDirty("MainCarriageFinalDestinationPortId"); } }
       
	 
    private mainCarriageFinalDestinationPortCode: string;
    public get MainCarriageFinalDestinationPortCode() { return this.mainCarriageFinalDestinationPortCode; }
    public set MainCarriageFinalDestinationPortCode(newValue: string) { if (this.mainCarriageFinalDestinationPortCode != newValue) { this.mainCarriageFinalDestinationPortCode = newValue; this.MarkAsDirty("MainCarriageFinalDestinationPortCode"); } }
       
	 
    private mainCarriageFinalDestinationPortName: string;
    public get MainCarriageFinalDestinationPortName() { return this.mainCarriageFinalDestinationPortName; }
    public set MainCarriageFinalDestinationPortName(newValue: string) { if (this.mainCarriageFinalDestinationPortName != newValue) { this.mainCarriageFinalDestinationPortName = newValue; this.MarkAsDirty("MainCarriageFinalDestinationPortName"); } }

    private mainCarriageFinalDestinationPortCountryId: string;
    public get MainCarriageFinalDestinationPortCountryId() { return this.mainCarriageFinalDestinationPortCountryId; }
    public set MainCarriageFinalDestinationPortCountryId(newValue: string) { if (this.mainCarriageFinalDestinationPortCountryId != newValue) { this.mainCarriageFinalDestinationPortCountryId = newValue; this.MarkAsDirty("MainCarriageFinalDestinationPortCountryId"); } }

	 
    private mainCarriageFinalDestinationPortCountryCode: string;
    public get MainCarriageFinalDestinationPortCountryCode() { return this.mainCarriageFinalDestinationPortCountryCode; }
    public set MainCarriageFinalDestinationPortCountryCode(newValue: string) { if (this.mainCarriageFinalDestinationPortCountryCode != newValue) { this.mainCarriageFinalDestinationPortCountryCode = newValue; this.MarkAsDirty("MainCarriageFinalDestinationPortCountryCode"); } }
       
	 
    private mainCarriageFinalDestinationPortCountryName: string;
    public get MainCarriageFinalDestinationPortCountryName() { return this.mainCarriageFinalDestinationPortCountryName; }
    public set MainCarriageFinalDestinationPortCountryName(newValue: string) { if (this.mainCarriageFinalDestinationPortCountryName != newValue) { this.mainCarriageFinalDestinationPortCountryName = newValue; this.MarkAsDirty("MainCarriageFinalDestinationPortCountryName"); } }
       
	 
    private mainHarmonize: string;
    public get MainHarmonize() { return this.mainHarmonize; }
    public set MainHarmonize(newValue: string) { if (this.mainHarmonize != newValue) { this.mainHarmonize = newValue; this.MarkAsDirty("MainHarmonize"); } }
       
	 
    private isDangerous: boolean;
    public get IsDangerous() { return this.isDangerous; }
    public set IsDangerous(newValue: boolean) { if (this.isDangerous != newValue) { this.isDangerous = newValue; this.MarkAsDirty("IsDangerous"); } }
       
	 
    private dangerousClassNumber: string;
    public get DangerousClassNumber() { return this.dangerousClassNumber; }
    public set DangerousClassNumber(newValue: string) { if (this.dangerousClassNumber != newValue) { this.dangerousClassNumber = newValue; this.MarkAsDirty("DangerousClassNumber"); } }
       
	 
    private dangerousUnNumber: string;
    public get DangerousUnNumber() { return this.dangerousUnNumber; }
    public set DangerousUnNumber(newValue: string) { if (this.dangerousUnNumber != newValue) { this.dangerousUnNumber = newValue; this.MarkAsDirty("DangerousUnNumber"); } }
       
	 
    private dangerousPackagingGroup: string;
    public get DangerousPackagingGroup() { return this.dangerousPackagingGroup; }
    public set DangerousPackagingGroup(newValue: string) { if (this.dangerousPackagingGroup != newValue) { this.dangerousPackagingGroup = newValue; this.MarkAsDirty("DangerousPackagingGroup"); } }
       
	 
    private dangerousIMDGCode: string;
    public get DangerousIMDGCode() { return this.dangerousIMDGCode; }
    public set DangerousIMDGCode(newValue: string) { if (this.dangerousIMDGCode != newValue) { this.dangerousIMDGCode = newValue; this.MarkAsDirty("DangerousIMDGCode"); } }
       
	 
    private dangerousFlashPoint: string;
    public get DangerousFlashPoint() { return this.dangerousFlashPoint; }
    public set DangerousFlashPoint(newValue: string) { if (this.dangerousFlashPoint != newValue) { this.dangerousFlashPoint = newValue; this.MarkAsDirty("DangerousFlashPoint"); } }
       
	 
    private dangerousMaterialDescription: string;
    public get DangerousMaterialDescription() { return this.dangerousMaterialDescription; }
    public set DangerousMaterialDescription(newValue: string) { if (this.dangerousMaterialDescription != newValue) { this.dangerousMaterialDescription = newValue; this.MarkAsDirty("DangerousMaterialDescription"); } }
       
	 
    private lTCWEdited: boolean;
    public get LTCWEdited() { return this.lTCWEdited; }
    public set LTCWEdited(newValue: boolean) { if (this.lTCWEdited != newValue) { this.lTCWEdited = newValue; this.MarkAsDirty("LTCWEdited"); } }
       
	 
    private shipmentPickUpIndex: number;
    public get ShipmentPickUpIndex() { return this.shipmentPickUpIndex; }
    public set ShipmentPickUpIndex(newValue: number) { if (this.shipmentPickUpIndex != newValue) { this.shipmentPickUpIndex = newValue; this.MarkAsDirty("ShipmentPickUpIndex"); } }
       
	 
    private shipmentDeliveryIndex: number;
    public get ShipmentDeliveryIndex() { return this.shipmentDeliveryIndex; }
    public set ShipmentDeliveryIndex(newValue: number) { if (this.shipmentDeliveryIndex != newValue) { this.shipmentDeliveryIndex = newValue; this.MarkAsDirty("ShipmentDeliveryIndex"); } }
       
	 
    private shipmentContainerReturnIndex: number;
    public get ShipmentContainerReturnIndex() { return this.shipmentContainerReturnIndex; }
    public set ShipmentContainerReturnIndex(newValue: number) { if (this.shipmentContainerReturnIndex != newValue) { this.shipmentContainerReturnIndex = newValue; this.MarkAsDirty("ShipmentContainerReturnIndex"); } }
       
	 
    private shipmentPayableStatusCode: string;
    public get ShipmentPayableStatusCode() { return this.shipmentPayableStatusCode; }
    public set ShipmentPayableStatusCode(newValue: string) { if (this.shipmentPayableStatusCode != newValue) { this.shipmentPayableStatusCode = newValue; this.MarkAsDirty("ShipmentPayableStatusCode"); } }
       
	 
    private shipmentReceivableStatusCode: string;
    public get ShipmentReceivableStatusCode() { return this.shipmentReceivableStatusCode; }
    public set ShipmentReceivableStatusCode(newValue: string) { if (this.shipmentReceivableStatusCode != newValue) { this.shipmentReceivableStatusCode = newValue; this.MarkAsDirty("ShipmentReceivableStatusCode"); } }
       
	 
    private shipmentReceivableStatusName: string;
    public get ShipmentReceivableStatusName() { return this.shipmentReceivableStatusName; }
    public set ShipmentReceivableStatusName(newValue: string) { if (this.shipmentReceivableStatusName != newValue) { this.shipmentReceivableStatusName = newValue; this.MarkAsDirty("ShipmentReceivableStatusName"); } }
       
	 
    private shipmentPayableStatusName: string;
    public get ShipmentPayableStatusName() { return this.shipmentPayableStatusName; }
    public set ShipmentPayableStatusName(newValue: string) { if (this.shipmentPayableStatusName != newValue) { this.shipmentPayableStatusName = newValue; this.MarkAsDirty("ShipmentPayableStatusName"); } }
       
	 
    private isCancelled: boolean;
    public get IsCancelled() { return this.isCancelled; }
    public set IsCancelled(newValue: boolean) { if (this.isCancelled != newValue) { this.isCancelled = newValue; this.MarkAsDirty("IsCancelled"); } }
       
	 
    private cancelledDate: Date;
    public get CancelledDate() { return this.cancelledDate; }
    public set CancelledDate(newValue: Date) { if (this.cancelledDate != newValue) { this.cancelledDate = newValue; this.MarkAsDirty("CancelledDate"); } }
       
	 
    private isAccountingClosed: boolean;
    public get IsAccountingClosed() { return this.isAccountingClosed; }
    public set IsAccountingClosed(newValue: boolean) { if (this.isAccountingClosed != newValue) { this.isAccountingClosed = newValue; this.MarkAsDirty("IsAccountingClosed"); } }
       
	 
    private accessDate: Date;
    public get AccessDate() { return this.accessDate; }
    public set AccessDate(newValue: Date) { if (this.accessDate != newValue) { this.accessDate = newValue; this.MarkAsDirty("AccessDate"); } }
       
	 
    private updatedByUserId: string;
    public get UpdatedByUserId() { return this.updatedByUserId; }
    public set UpdatedByUserId(newValue: string) { if (this.updatedByUserId != newValue) { this.updatedByUserId = newValue; this.MarkAsDirty("UpdatedByUserId"); } }
       
	 
    private lastUpdateDate: Date;
    public get LastUpdateDate() { return this.lastUpdateDate; }
    public set LastUpdateDate(newValue: Date) { if (this.lastUpdateDate != newValue) { this.lastUpdateDate = newValue; this.MarkAsDirty("LastUpdateDate"); } }
       
	 
    private profitCurrencyId: string;
    public get ProfitCurrencyId() { return this.profitCurrencyId; }
    public set ProfitCurrencyId(newValue: string) { if (this.profitCurrencyId != newValue) { this.profitCurrencyId = newValue; this.MarkAsDirty("ProfitCurrencyId"); } }
       
	 
    private profitCurrencyCode: string;
    public get ProfitCurrencyCode() { return this.profitCurrencyCode; }
    public set ProfitCurrencyCode(newValue: string) { if (this.profitCurrencyCode != newValue) { this.profitCurrencyCode = newValue; this.MarkAsDirty("ProfitCurrencyCode"); } }
       
	 
    private profitExchangeRate: number;
    public get ProfitExchangeRate() { return this.profitExchangeRate; }
    public set ProfitExchangeRate(newValue: number) { if (this.profitExchangeRate != newValue) { this.profitExchangeRate = newValue; this.MarkAsDirty("ProfitExchangeRate"); } }
       
	 
    private updatedByUserName: string;
    public get UpdatedByUserName() { return this.updatedByUserName; }
    public set UpdatedByUserName(newValue: string) { if (this.updatedByUserName != newValue) { this.updatedByUserName = newValue; this.MarkAsDirty("UpdatedByUserName"); } }
       
	 
    private eventNote: string;
    public get EventNote() { return this.eventNote; }
    public set EventNote(newValue: string) { if (this.eventNote != newValue) { this.eventNote = newValue; this.MarkAsDirty("EventNote"); } }
       
	 
    private nextLegCode: string;
    public get NextLegCode() { return this.nextLegCode; }
    public set NextLegCode(newValue: string) { if (this.nextLegCode != newValue) { this.nextLegCode = newValue; this.MarkAsDirty("NextLegCode"); } }
       
	 
    private nextLegName: string;
    public get NextLegName() { return this.nextLegName; }
    public set NextLegName(newValue: string) { if (this.nextLegName != newValue) { this.nextLegName = newValue; this.MarkAsDirty("NextLegName"); } }
       
	 
    private nextETD: Date;
    public get NextETD() { return this.nextETD; }
    public set NextETD(newValue: Date) { if (this.nextETD != newValue) { this.nextETD = newValue; this.MarkAsDirty("NextETD"); } }
       
	 
    private nextETA: Date;
    public get NextETA() { return this.nextETA; }
    public set NextETA(newValue: Date) { if (this.nextETA != newValue) { this.nextETA = newValue; this.MarkAsDirty("NextETA"); } }
       
	 
    private shipmentLevelCode: string;
    public get ShipmentLevelCode() { return this.shipmentLevelCode; }
    public set ShipmentLevelCode(newValue: string) { if (this.shipmentLevelCode != newValue) { this.shipmentLevelCode = newValue; this.MarkAsDirty("ShipmentLevelCode"); } }
       
	 
    private shipmentLevelName: string;
    public get ShipmentLevelName() { return this.shipmentLevelName; }
    public set ShipmentLevelName(newValue: string) { if (this.shipmentLevelName != newValue) { this.shipmentLevelName = newValue; this.MarkAsDirty("ShipmentLevelName"); } }
       
	 
    private masterShipmentDataId: string;
    public get MasterShipmentDataId() { return this.masterShipmentDataId; }
    public set MasterShipmentDataId(newValue: string) { if (this.masterShipmentDataId != newValue) { this.masterShipmentDataId = newValue; this.MarkAsDirty("MasterShipmentDataId"); } }
       
	 
    private numberOfFollowUps: number;
    public get NumberOfFollowUps() { return this.numberOfFollowUps; }
    public set NumberOfFollowUps(newValue: number) { if (this.numberOfFollowUps != newValue) { this.numberOfFollowUps = newValue; this.MarkAsDirty("NumberOfFollowUps"); } }
       
	 
    private quoteNumber: string;
    public get QuoteNumber() { return this.quoteNumber; }
    public set QuoteNumber(newValue: string) { if (this.quoteNumber != newValue) { this.quoteNumber = newValue; this.MarkAsDirty("QuoteNumber"); } }
       
	 
    private shipmentCustomerTypeCode: string;
    public get ShipmentCustomerTypeCode() { return this.shipmentCustomerTypeCode; }
    public set ShipmentCustomerTypeCode(newValue: string) { if (this.shipmentCustomerTypeCode != newValue) { this.shipmentCustomerTypeCode = newValue; this.MarkAsDirty("ShipmentCustomerTypeCode"); } }
       
	 
    private consigneeAddressOneTime: boolean;
    public get ConsigneeAddressOneTime() { return this.consigneeAddressOneTime; }
    public set ConsigneeAddressOneTime(newValue: boolean) { if (this.consigneeAddressOneTime != newValue) { this.consigneeAddressOneTime = newValue; this.MarkAsDirty("ConsigneeAddressOneTime"); } }
       
	 
    private shipperAddressOneTime: boolean;
    public get ShipperAddressOneTime() { return this.shipperAddressOneTime; }
    public set ShipperAddressOneTime(newValue: boolean) { if (this.shipperAddressOneTime != newValue) { this.shipperAddressOneTime = newValue; this.MarkAsDirty("ShipperAddressOneTime"); } }
       
	 
    private customerId: string;
    public get CustomerId() { return this.customerId; }
    public set CustomerId(newValue: string) { if (this.customerId != newValue) { this.customerId = newValue; this.MarkAsDirty("CustomerId"); } }
       
	 
    private customerAddressId: string;
    public get CustomerAddressId() { return this.customerAddressId; }
    public set CustomerAddressId(newValue: string) { if (this.customerAddressId != newValue) { this.customerAddressId = newValue; this.MarkAsDirty("CustomerAddressId"); } }
       
	 
    private customerContactId: string;
    public get CustomerContactId() { return this.customerContactId; }
    public set CustomerContactId(newValue: string) { if (this.customerContactId != newValue) { this.customerContactId = newValue; this.MarkAsDirty("CustomerContactId"); } }
       
	 
    private customerReference1: string;
    public get CustomerReference1() { return this.customerReference1; }
    public set CustomerReference1(newValue: string) { if (this.customerReference1 != newValue) { this.customerReference1 = newValue; this.MarkAsDirty("CustomerReference1"); } }
       
	 
    private customerReference2: string;
    public get CustomerReference2() { return this.customerReference2; }
    public set CustomerReference2(newValue: string) { if (this.customerReference2 != newValue) { this.customerReference2 = newValue; this.MarkAsDirty("CustomerReference2"); } }


    private customerReference3: string;
    public get CustomerReference3() { return this.customerReference3; }
    public set CustomerReference3(newValue: string) { if (this.customerReference3 != newValue) { this.customerReference3 = newValue; this.MarkAsDirty("CustomerReference3"); } }
       
	 
    private customerName: string;
    public get CustomerName() { return this.customerName; }
    public set CustomerName(newValue: string) { if (this.customerName != newValue) { this.customerName = newValue; this.MarkAsDirty("CustomerName"); } }
       
	 
    private customerNote: string;
    public get CustomerNote() { return this.customerNote; }
    public set CustomerNote(newValue: string) { if (this.customerNote != newValue) { this.customerNote = newValue; this.MarkAsDirty("CustomerNote"); } }
       
	 
    private freelancerId: string;
    public get FreelancerId() { return this.freelancerId; }
    public set FreelancerId(newValue: string) { if (this.freelancerId != newValue) { this.freelancerId = newValue; this.MarkAsDirty("FreelancerId"); } }
       
	 
    private freelancerAddressId: string;
    public get FreelancerAddressId() { return this.freelancerAddressId; }
    public set FreelancerAddressId(newValue: string) { if (this.freelancerAddressId != newValue) { this.freelancerAddressId = newValue; this.MarkAsDirty("FreelancerAddressId"); } }
       
	 
    private freelancerContactId: string;
    public get FreelancerContactId() { return this.freelancerContactId; }
    public set FreelancerContactId(newValue: string) { if (this.freelancerContactId != newValue) { this.freelancerContactId = newValue; this.MarkAsDirty("FreelancerContactId"); } }
       
	 
    private freelancerName: string;
    public get FreelancerName() { return this.freelancerName; }
    public set FreelancerName(newValue: string) { if (this.freelancerName != newValue) { this.freelancerName = newValue; this.MarkAsDirty("FreelancerName"); } }
       
	 
    private issuingCarrierAgentId: string;
    public get IssuingCarrierAgentId() { return this.issuingCarrierAgentId; }
    public set IssuingCarrierAgentId(newValue: string) { if (this.issuingCarrierAgentId != newValue) { this.issuingCarrierAgentId = newValue; this.MarkAsDirty("IssuingCarrierAgentId"); } }
       
	 
    private issuingCarrierAddressId: string;
    public get IssuingCarrierAddressId() { return this.issuingCarrierAddressId; }
    public set IssuingCarrierAddressId(newValue: string) { if (this.issuingCarrierAddressId != newValue) { this.issuingCarrierAddressId = newValue; this.MarkAsDirty("IssuingCarrierAddressId"); } }
       
	 
    private issuingCarrierAgentName: string;
    public get IssuingCarrierAgentName() { return this.issuingCarrierAgentName; }
    public set IssuingCarrierAgentName(newValue: string) { if (this.issuingCarrierAgentName != newValue) { this.issuingCarrierAgentName = newValue; this.MarkAsDirty("IssuingCarrierAgentName"); } }
       
	 
    private issuingCarrierAgentNote: string;
    public get IssuingCarrierAgentNote() { return this.issuingCarrierAgentNote; }
    public set IssuingCarrierAgentNote(newValue: string) { if (this.issuingCarrierAgentNote != newValue) { this.issuingCarrierAgentNote = newValue; this.MarkAsDirty("IssuingCarrierAgentNote"); } }
       
	 
    private issuingCarrierIATACode: string;
    public get IssuingCarrierIATACode() { return this.issuingCarrierIATACode; }
    public set IssuingCarrierIATACode(newValue: string) { if (this.issuingCarrierIATACode != newValue) { this.issuingCarrierIATACode = newValue; this.MarkAsDirty("IssuingCarrierIATACode"); } }
       
	 
    private freightForwarderId: string;
    public get FreightForwarderId() { return this.freightForwarderId; }
    public set FreightForwarderId(newValue: string) { if (this.freightForwarderId != newValue) { this.freightForwarderId = newValue; this.MarkAsDirty("FreightForwarderId"); } }
       
	 
    private freightForwarderAddressId: string;
    public get FreightForwarderAddressId() { return this.freightForwarderAddressId; }
    public set FreightForwarderAddressId(newValue: string) { if (this.freightForwarderAddressId != newValue) { this.freightForwarderAddressId = newValue; this.MarkAsDirty("FreightForwarderAddressId"); } }
       
	 
    private freightForwarderContactId: string;
    public get FreightForwarderContactId() { return this.freightForwarderContactId; }
    public set FreightForwarderContactId(newValue: string) { if (this.freightForwarderContactId != newValue) { this.freightForwarderContactId = newValue; this.MarkAsDirty("FreightForwarderContactId"); } }
       
	 
    private freightForwarderReference: string;
    public get FreightForwarderReference() { return this.freightForwarderReference; }
    public set FreightForwarderReference(newValue: string) { if (this.freightForwarderReference != newValue) { this.freightForwarderReference = newValue; this.MarkAsDirty("FreightForwarderReference"); } }
       
	 
    private freightForwarderName: string;
    public get FreightForwarderName() { return this.freightForwarderName; }
    public set FreightForwarderName(newValue: string) { if (this.freightForwarderName != newValue) { this.freightForwarderName = newValue; this.MarkAsDirty("FreightForwarderName"); } }
       
	 
    private freightForwarderNote: string;
    public get FreightForwarderNote() { return this.freightForwarderNote; }
    public set FreightForwarderNote(newValue: string) { if (this.freightForwarderNote != newValue) { this.freightForwarderNote = newValue; this.MarkAsDirty("FreightForwarderNote"); } }
       
	 
    private shipperId: string;
    public get ShipperId() { return this.shipperId; }
    public set ShipperId(newValue: string) { if (this.shipperId != newValue) { this.shipperId = newValue; this.MarkAsDirty("ShipperId"); } }
       
	 
    private shipperAddressId: string;
    public get ShipperAddressId() { return this.shipperAddressId; }
    public set ShipperAddressId(newValue: string) { if (this.shipperAddressId != newValue) { this.shipperAddressId = newValue; this.MarkAsDirty("ShipperAddressId"); } }
       
	 
    private shipperContactId: string;
    public get ShipperContactId() { return this.shipperContactId; }
    public set ShipperContactId(newValue: string) { if (this.shipperContactId != newValue) { this.shipperContactId = newValue; this.MarkAsDirty("ShipperContactId"); } }
       
	 
    private shipperReference1: string;
    public get ShipperReference1() { return this.shipperReference1; }
    public set ShipperReference1(newValue: string) { if (this.shipperReference1 != newValue) { this.shipperReference1 = newValue; this.MarkAsDirty("ShipperReference1"); } }
       
	 
    private shipperReference2: string;
    public get ShipperReference2() { return this.shipperReference2; }
    public set ShipperReference2(newValue: string) { if (this.shipperReference2 != newValue) { this.shipperReference2 = newValue; this.MarkAsDirty("ShipperReference2"); } }
       
    private shipperReference3: string;
    public get ShipperReference3() { return this.shipperReference3; }
    public set ShipperReference3(newValue: string) { if (this.shipperReference3 != newValue) { this.shipperReference3 = newValue; this.MarkAsDirty("ShipperReference3"); } }

    private shipperName: string;
    public get ShipperName() { return this.shipperName; }
    public set ShipperName(newValue: string) { if (this.shipperName != newValue) { this.shipperName = newValue; this.MarkAsDirty("ShipperName"); } }
       
     

    private shipperNote: string;
    public get ShipperNote() { return this.shipperNote; }
    public set ShipperNote(newValue: string) { if (this.shipperNote != newValue) { this.shipperNote = newValue; this.MarkAsDirty("ShipperNote"); } }
       
	 
    private shipperAddressText: string;
    public get ShipperAddressText() { return this.shipperAddressText; }
    public set ShipperAddressText(newValue: string) { if (this.shipperAddressText != newValue) { this.shipperAddressText = newValue; this.MarkAsDirty("ShipperAddressText"); } }
       
	 
    private shipperAddressCountryCode: string;
    public get ShipperAddressCountryCode() { return this.shipperAddressCountryCode; }
    public set ShipperAddressCountryCode(newValue: string) { if (this.shipperAddressCountryCode != newValue) { this.shipperAddressCountryCode = newValue; this.MarkAsDirty("ShipperAddressCountryCode"); } }
       
	 
    private consigneeId: string;
    public get ConsigneeId() { return this.consigneeId; }
    public set ConsigneeId(newValue: string) { if (this.consigneeId != newValue) { this.consigneeId = newValue; this.MarkAsDirty("ConsigneeId"); } }
       
	 
    private consigneeAddressId: string;
    public get ConsigneeAddressId() { return this.consigneeAddressId; }
    public set ConsigneeAddressId(newValue: string) { if (this.consigneeAddressId != newValue) { this.consigneeAddressId = newValue; this.MarkAsDirty("ConsigneeAddressId"); } }
       
	 
    private consigneeContactId: string;
    public get ConsigneeContactId() { return this.consigneeContactId; }
    public set ConsigneeContactId(newValue: string) { if (this.consigneeContactId != newValue) { this.consigneeContactId = newValue; this.MarkAsDirty("ConsigneeContactId"); } }
       
	 
    private consigneeReference1: string;
    public get ConsigneeReference1() { return this.consigneeReference1; }
    public set ConsigneeReference1(newValue: string) { if (this.consigneeReference1 != newValue) { this.consigneeReference1 = newValue; this.MarkAsDirty("ConsigneeReference1"); } }
       
	 
    private consigneeReference2: string;
    public get ConsigneeReference2() { return this.consigneeReference2; }
    public set ConsigneeReference2(newValue: string) { if (this.consigneeReference2 != newValue) { this.consigneeReference2 = newValue; this.MarkAsDirty("ConsigneeReference2"); } }


    private consigneeReference3: string;
    public get ConsigneeReference3() { return this.consigneeReference3; }
    public set ConsigneeReference3(newValue: string) { if (this.consigneeReference3 != newValue) { this.consigneeReference3 = newValue; this.MarkAsDirty("ConsigneeReference3"); } }

    private shippingAgent: string;
    public get ShippingAgent() { return this.shippingAgent; }
    public set ShippingAgent(newValue: string) { if (this.shippingAgent != newValue) { this.shippingAgent = newValue; this.MarkAsDirty("ShippingAgent"); } }


    private consigneeName: string;
    public get ConsigneeName() { return this.consigneeName; }
    public set ConsigneeName(newValue: string) { if (this.consigneeName != newValue) { this.consigneeName = newValue; this.MarkAsDirty("ConsigneeName"); } }
       
	 
    private consigneeNote: string;
    public get ConsigneeNote() { return this.consigneeNote; }
    public set ConsigneeNote(newValue: string) { if (this.consigneeNote != newValue) { this.consigneeNote = newValue; this.MarkAsDirty("ConsigneeNote"); } }
       
	 
    private consigneeAddressText: string;
    public get ConsigneeAddressText() { return this.consigneeAddressText; }
    public set ConsigneeAddressText(newValue: string) { if (this.consigneeAddressText != newValue) { this.consigneeAddressText = newValue; this.MarkAsDirty("ConsigneeAddressText"); } }
       
	 
    private consigneeAddressCountryCode: string;
    public get ConsigneeAddressCountryCode() { return this.consigneeAddressCountryCode; }
    public set ConsigneeAddressCountryCode(newValue: string) { if (this.consigneeAddressCountryCode != newValue) { this.consigneeAddressCountryCode = newValue; this.MarkAsDirty("ConsigneeAddressCountryCode"); } }
       
	 
    private agentId: string;
    public get AgentId() { return this.agentId; }
    public set AgentId(newValue: string) { if (this.agentId != newValue) { this.agentId = newValue; this.MarkAsDirty("AgentId"); } }
       
	 
    private agentAddressId: string;
    public get AgentAddressId() { return this.agentAddressId; }
    public set AgentAddressId(newValue: string) { if (this.agentAddressId != newValue) { this.agentAddressId = newValue; this.MarkAsDirty("AgentAddressId"); } }
       
	 
    private agentContactId: string;
    public get AgentContactId() { return this.agentContactId; }
    public set AgentContactId(newValue: string) { if (this.agentContactId != newValue) { this.agentContactId = newValue; this.MarkAsDirty("AgentContactId"); } }
       
	 
    private agentReference1: string;
    public get AgentReference1() { return this.agentReference1; }
    public set AgentReference1(newValue: string) { if (this.agentReference1 != newValue) { this.agentReference1 = newValue; this.MarkAsDirty("AgentReference1"); } }
       
	 
    private agentReference2: string;
    public get AgentReference2() { return this.agentReference2; }
    public set AgentReference2(newValue: string) { if (this.agentReference2 != newValue) { this.agentReference2 = newValue; this.MarkAsDirty("AgentReference2"); } }
       
	 
    private agentName: string;
    public get AgentName() { return this.agentName; }
    public set AgentName(newValue: string) { if (this.agentName != newValue) { this.agentName = newValue; this.MarkAsDirty("AgentName"); } }
       
	 
    private agentNote: string;
    public get AgentNote() { return this.agentNote; }
    public set AgentNote(newValue: string) { if (this.agentNote != newValue) { this.agentNote = newValue; this.MarkAsDirty("AgentNote"); } }
       
	 
    private agentAddressText: string;
    public get AgentAddressText() { return this.agentAddressText; }
    public set AgentAddressText(newValue: string) { if (this.agentAddressText != newValue) { this.agentAddressText = newValue; this.MarkAsDirty("AgentAddressText"); } }
       
	 
    private agentAddressCountryCode: string;
    public get AgentAddressCountryCode() { return this.agentAddressCountryCode; }
    public set AgentAddressCountryCode(newValue: string) { if (this.agentAddressCountryCode != newValue) { this.agentAddressCountryCode = newValue; this.MarkAsDirty("AgentAddressCountryCode"); } }
       
	 
    private customAgentExportId: string;
    public get CustomAgentExportId() { return this.customAgentExportId; }
    public set CustomAgentExportId(newValue: string) { if (this.customAgentExportId != newValue) { this.customAgentExportId = newValue; this.MarkAsDirty("CustomAgentExportId"); } }
       
	 
    private customAgentExportAddressId: string;
    public get CustomAgentExportAddressId() { return this.customAgentExportAddressId; }
    public set CustomAgentExportAddressId(newValue: string) { if (this.customAgentExportAddressId != newValue) { this.customAgentExportAddressId = newValue; this.MarkAsDirty("CustomAgentExportAddressId"); } }
       
	 
    private customAgentExportContactId: string;
    public get CustomAgentExportContactId() { return this.customAgentExportContactId; }
    public set CustomAgentExportContactId(newValue: string) { if (this.customAgentExportContactId != newValue) { this.customAgentExportContactId = newValue; this.MarkAsDirty("CustomAgentExportContactId"); } }
       
	 
    private customAgentExportReference: string;
    public get CustomAgentExportReference() { return this.customAgentExportReference; }
    public set CustomAgentExportReference(newValue: string) { if (this.customAgentExportReference != newValue) { this.customAgentExportReference = newValue; this.MarkAsDirty("CustomAgentExportReference"); } }
       
	 
    private customAgentExportName: string;
    public get CustomAgentExportName() { return this.customAgentExportName; }
    public set CustomAgentExportName(newValue: string) { if (this.customAgentExportName != newValue) { this.customAgentExportName = newValue; this.MarkAsDirty("CustomAgentExportName"); } }
       
	 
    private customAgentExportNote: string;
    public get CustomAgentExportNote() { return this.customAgentExportNote; }
    public set CustomAgentExportNote(newValue: string) { if (this.customAgentExportNote != newValue) { this.customAgentExportNote = newValue; this.MarkAsDirty("CustomAgentExportNote"); } }
       
	 
    private customAgentImportId: string;
    public get CustomAgentImportId() { return this.customAgentImportId; }
    public set CustomAgentImportId(newValue: string) { if (this.customAgentImportId != newValue) { this.customAgentImportId = newValue; this.MarkAsDirty("CustomAgentImportId"); } }
       
	 
    private customAgentImportAddressId: string;
    public get CustomAgentImportAddressId() { return this.customAgentImportAddressId; }
    public set CustomAgentImportAddressId(newValue: string) { if (this.customAgentImportAddressId != newValue) { this.customAgentImportAddressId = newValue; this.MarkAsDirty("CustomAgentImportAddressId"); } }
       
	 
    private customAgentImportContactId: string;
    public get CustomAgentImportContactId() { return this.customAgentImportContactId; }
    public set CustomAgentImportContactId(newValue: string) { if (this.customAgentImportContactId != newValue) { this.customAgentImportContactId = newValue; this.MarkAsDirty("CustomAgentImportContactId"); } }
       
	 
    private customAgentImportReference: string;
    public get CustomAgentImportReference() { return this.customAgentImportReference; }
    public set CustomAgentImportReference(newValue: string) { if (this.customAgentImportReference != newValue) { this.customAgentImportReference = newValue; this.MarkAsDirty("CustomAgentImportReference"); } }
       
	 
    private customAgentImportName: string;
    public get CustomAgentImportName() { return this.customAgentImportName; }
    public set CustomAgentImportName(newValue: string) { if (this.customAgentImportName != newValue) { this.customAgentImportName = newValue; this.MarkAsDirty("CustomAgentImportName"); } }
       
	 
    private customAgentImportNote: string;
    public get CustomAgentImportNote() { return this.customAgentImportNote; }
    public set CustomAgentImportNote(newValue: string) { if (this.customAgentImportNote != newValue) { this.customAgentImportNote = newValue; this.MarkAsDirty("CustomAgentImportNote"); } }
       
	 
    private notify1Id: string;
    public get Notify1Id() { return this.notify1Id; }
    public set Notify1Id(newValue: string) { if (this.notify1Id != newValue) { this.notify1Id = newValue; this.MarkAsDirty("Notify1Id"); } }
       
	 
    private notify1AddressId: string;
    public get Notify1AddressId() { return this.notify1AddressId; }
    public set Notify1AddressId(newValue: string) { if (this.notify1AddressId != newValue) { this.notify1AddressId = newValue; this.MarkAsDirty("Notify1AddressId"); } }
       
	 
    private notify1ContactId: string;
    public get Notify1ContactId() { return this.notify1ContactId; }
    public set Notify1ContactId(newValue: string) { if (this.notify1ContactId != newValue) { this.notify1ContactId = newValue; this.MarkAsDirty("Notify1ContactId"); } }
       
	 
    private notify1Name: string;
    public get Notify1Name() { return this.notify1Name; }
    public set Notify1Name(newValue: string) { if (this.notify1Name != newValue) { this.notify1Name = newValue; this.MarkAsDirty("Notify1Name"); } }
       
	 
    private notify1Note: string;
    public get Notify1Note() { return this.notify1Note; }
    public set Notify1Note(newValue: string) { if (this.notify1Note != newValue) { this.notify1Note = newValue; this.MarkAsDirty("Notify1Note"); } }
       
	 
    private notify2Id: string;
    public get Notify2Id() { return this.notify2Id; }
    public set Notify2Id(newValue: string) { if (this.notify2Id != newValue) { this.notify2Id = newValue; this.MarkAsDirty("Notify2Id"); } }
       
	 
    private notify2AddressId: string;
    public get Notify2AddressId() { return this.notify2AddressId; }
    public set Notify2AddressId(newValue: string) { if (this.notify2AddressId != newValue) { this.notify2AddressId = newValue; this.MarkAsDirty("Notify2AddressId"); } }
       
	 
    private notify2ContactId: string;
    public get Notify2ContactId() { return this.notify2ContactId; }
    public set Notify2ContactId(newValue: string) { if (this.notify2ContactId != newValue) { this.notify2ContactId = newValue; this.MarkAsDirty("Notify2ContactId"); } }
       
	 
    private notify2Name: string;
    public get Notify2Name() { return this.notify2Name; }
    public set Notify2Name(newValue: string) { if (this.notify2Name != newValue) { this.notify2Name = newValue; this.MarkAsDirty("Notify2Name"); } }
       
	 
    private notify2Note: string;
    public get Notify2Note() { return this.notify2Note; }
    public set Notify2Note(newValue: string) { if (this.notify2Note != newValue) { this.notify2Note = newValue; this.MarkAsDirty("Notify2Note"); } }
       
	 
    private shipperNotExporterId: string;
    public get ShipperNotExporterId() { return this.shipperNotExporterId; }
    public set ShipperNotExporterId(newValue: string) { if (this.shipperNotExporterId != newValue) { this.shipperNotExporterId = newValue; this.MarkAsDirty("ShipperNotExporterId"); } }
       
	 
    private shipperNotExporterAddressId: string;
    public get ShipperNotExporterAddressId() { return this.shipperNotExporterAddressId; }
    public set ShipperNotExporterAddressId(newValue: string) { if (this.shipperNotExporterAddressId != newValue) { this.shipperNotExporterAddressId = newValue; this.MarkAsDirty("ShipperNotExporterAddressId"); } }
       
	 
    private shipperNotExporterContactId: string;
    public get ShipperNotExporterContactId() { return this.shipperNotExporterContactId; }
    public set ShipperNotExporterContactId(newValue: string) { if (this.shipperNotExporterContactId != newValue) { this.shipperNotExporterContactId = newValue; this.MarkAsDirty("ShipperNotExporterContactId"); } }
       
	 
    private shipperNotExporterName: string;
    public get ShipperNotExporterName() { return this.shipperNotExporterName; }
    public set ShipperNotExporterName(newValue: string) { if (this.shipperNotExporterName != newValue) { this.shipperNotExporterName = newValue; this.MarkAsDirty("ShipperNotExporterName"); } }
       
	 
    private shipperNotExporterNote: string;
    public get ShipperNotExporterNote() { return this.shipperNotExporterNote; }
    public set ShipperNotExporterNote(newValue: string) { if (this.shipperNotExporterNote != newValue) { this.shipperNotExporterNote = newValue; this.MarkAsDirty("ShipperNotExporterNote"); } }
       
	 
    private consigneeNotImporterId: string;
    public get ConsigneeNotImporterId() { return this.consigneeNotImporterId; }
    public set ConsigneeNotImporterId(newValue: string) { if (this.consigneeNotImporterId != newValue) { this.consigneeNotImporterId = newValue; this.MarkAsDirty("ConsigneeNotImporterId"); } }
       
	 
    private consigneeNotImporterAddressId: string;
    public get ConsigneeNotImporterAddressId() { return this.consigneeNotImporterAddressId; }
    public set ConsigneeNotImporterAddressId(newValue: string) { if (this.consigneeNotImporterAddressId != newValue) { this.consigneeNotImporterAddressId = newValue; this.MarkAsDirty("ConsigneeNotImporterAddressId"); } }
       
	 
    private consigneeNotImporterContactId: string;
    public get ConsigneeNotImporterContactId() { return this.consigneeNotImporterContactId; }
    public set ConsigneeNotImporterContactId(newValue: string) { if (this.consigneeNotImporterContactId != newValue) { this.consigneeNotImporterContactId = newValue; this.MarkAsDirty("ConsigneeNotImporterContactId"); } }
       
	 
    private consigneeNotImporterName: string;
    public get ConsigneeNotImporterName() { return this.consigneeNotImporterName; }
    public set ConsigneeNotImporterName(newValue: string) { if (this.consigneeNotImporterName != newValue) { this.consigneeNotImporterName = newValue; this.MarkAsDirty("ConsigneeNotImporterName"); } }
       
	 
    private consigneeNotImporterNote: string;
    public get ConsigneeNotImporterNote() { return this.consigneeNotImporterNote; }
    public set ConsigneeNotImporterNote(newValue: string) { if (this.consigneeNotImporterNote != newValue) { this.consigneeNotImporterNote = newValue; this.MarkAsDirty("ConsigneeNotImporterNote"); } }
       
	 
    private customClearancePointId: string;
    public get CustomClearancePointId() { return this.customClearancePointId; }
    public set CustomClearancePointId(newValue: string) { if (this.customClearancePointId != newValue) { this.customClearancePointId = newValue; this.MarkAsDirty("CustomClearancePointId"); } }
       
	 
    private customClearancePointAddressId: string;
    public get CustomClearancePointAddressId() { return this.customClearancePointAddressId; }
    public set CustomClearancePointAddressId(newValue: string) { if (this.customClearancePointAddressId != newValue) { this.customClearancePointAddressId = newValue; this.MarkAsDirty("CustomClearancePointAddressId"); } }
       
	 
    private customClearancePointContactId: string;
    public get CustomClearancePointContactId() { return this.customClearancePointContactId; }
    public set CustomClearancePointContactId(newValue: string) { if (this.customClearancePointContactId != newValue) { this.customClearancePointContactId = newValue; this.MarkAsDirty("CustomClearancePointContactId"); } }
       
	 
    private customClearancePointReference1: string;
    public get CustomClearancePointReference1() { return this.customClearancePointReference1; }
    public set CustomClearancePointReference1(newValue: string) { if (this.customClearancePointReference1 != newValue) { this.customClearancePointReference1 = newValue; this.MarkAsDirty("CustomClearancePointReference1"); } }
       
	 
    private customClearancePointName: string;
    public get CustomClearancePointName() { return this.customClearancePointName; }
    public set CustomClearancePointName(newValue: string) { if (this.customClearancePointName != newValue) { this.customClearancePointName = newValue; this.MarkAsDirty("CustomClearancePointName"); } }
       
	 
    private customClearancePointNote: string;
    public get CustomClearancePointNote() { return this.customClearancePointNote; }
    public set CustomClearancePointNote(newValue: string) { if (this.customClearancePointNote != newValue) { this.customClearancePointNote = newValue; this.MarkAsDirty("CustomClearancePointNote"); } }
       
	 
    private coloaderId: string;
    public get ColoaderId() { return this.coloaderId; }
    public set ColoaderId(newValue: string) { if (this.coloaderId != newValue) { this.coloaderId = newValue; this.MarkAsDirty("ColoaderId"); } }
       
	 
    private coloaderAddressId: string;
    public get ColoaderAddressId() { return this.coloaderAddressId; }
    public set ColoaderAddressId(newValue: string) { if (this.coloaderAddressId != newValue) { this.coloaderAddressId = newValue; this.MarkAsDirty("ColoaderAddressId"); } }
       
	 
    private coloaderContactId: string;
    public get ColoaderContactId() { return this.coloaderContactId; }
    public set ColoaderContactId(newValue: string) { if (this.coloaderContactId != newValue) { this.coloaderContactId = newValue; this.MarkAsDirty("ColoaderContactId"); } }
       
	 
    private coloaderReference1: string;
    public get ColoaderReference1() { return this.coloaderReference1; }
    public set ColoaderReference1(newValue: string) { if (this.coloaderReference1 != newValue) { this.coloaderReference1 = newValue; this.MarkAsDirty("ColoaderReference1"); } }
       
	 
    private coloaderName: string;
    public get ColoaderName() { return this.coloaderName; }
    public set ColoaderName(newValue: string) { if (this.coloaderName != newValue) { this.coloaderName = newValue; this.MarkAsDirty("ColoaderName"); } }
       
	 
    private coloaderNote: string;
    public get ColoaderNote() { return this.coloaderNote; }
    public set ColoaderNote(newValue: string) { if (this.coloaderNote != newValue) { this.coloaderNote = newValue; this.MarkAsDirty("ColoaderNote"); } }
       
	 
    private consolidatorId: string;
    public get ConsolidatorId() { return this.consolidatorId; }
    public set ConsolidatorId(newValue: string) { if (this.consolidatorId != newValue) { this.consolidatorId = newValue; this.MarkAsDirty("ConsolidatorId"); } }
       
	 
    private consolidatorAddressId: string;
    public get ConsolidatorAddressId() { return this.consolidatorAddressId; }
    public set ConsolidatorAddressId(newValue: string) { if (this.consolidatorAddressId != newValue) { this.consolidatorAddressId = newValue; this.MarkAsDirty("ConsolidatorAddressId"); } }
       
	 
    private consolidatorContactId: string;
    public get ConsolidatorContactId() { return this.consolidatorContactId; }
    public set ConsolidatorContactId(newValue: string) { if (this.consolidatorContactId != newValue) { this.consolidatorContactId = newValue; this.MarkAsDirty("ConsolidatorContactId"); } }
       
	 
    private consolidatorReference: string;
    public get ConsolidatorReference() { return this.consolidatorReference; }
    public set ConsolidatorReference(newValue: string) { if (this.consolidatorReference != newValue) { this.consolidatorReference = newValue; this.MarkAsDirty("ConsolidatorReference"); } }
       
	 
    private consolidatorName: string;
    public get ConsolidatorName() { return this.consolidatorName; }
    public set ConsolidatorName(newValue: string) { if (this.consolidatorName != newValue) { this.consolidatorName = newValue; this.MarkAsDirty("ConsolidatorName"); } }
       
	 
    private consolidatorNote: string;
    public get ConsolidatorNote() { return this.consolidatorNote; }
    public set ConsolidatorNote(newValue: string) { if (this.consolidatorNote != newValue) { this.consolidatorNote = newValue; this.MarkAsDirty("ConsolidatorNote"); } }
       
	 
    private forwarderShipmentNumber: string;
    public get ForwarderShipmentNumber() { return this.forwarderShipmentNumber; }
    public set ForwarderShipmentNumber(newValue: string) { if (this.forwarderShipmentNumber != newValue) { this.forwarderShipmentNumber = newValue; this.MarkAsDirty("ForwarderShipmentNumber"); } }
       
	 
    private computedForwarderShipmentNumber: string;
    public get ComputedForwarderShipmentNumber() { return this.computedForwarderShipmentNumber; }
    public set ComputedForwarderShipmentNumber(newValue: string) { if (this.computedForwarderShipmentNumber != newValue) { this.computedForwarderShipmentNumber = newValue; this.MarkAsDirty("ComputedForwarderShipmentNumber"); } }
       
	 
    private customerShipmentNumber: string;
    public get CustomerShipmentNumber() { return this.customerShipmentNumber; }
    public set CustomerShipmentNumber(newValue: string) { if (this.customerShipmentNumber != newValue) { this.customerShipmentNumber = newValue; this.MarkAsDirty("CustomerShipmentNumber"); } }
       
	 
    private customsDeclarationNumber: string;
    public get CustomsDeclarationNumber() { return this.customsDeclarationNumber; }
    public set CustomsDeclarationNumber(newValue: string) { if (this.customsDeclarationNumber != newValue) { this.customsDeclarationNumber = newValue; this.MarkAsDirty("CustomsDeclarationNumber"); } }
       
	 
    private releasingAgentId: string;
    public get ReleasingAgentId() { return this.releasingAgentId; }
    public set ReleasingAgentId(newValue: string) { if (this.releasingAgentId != newValue) { this.releasingAgentId = newValue; this.MarkAsDirty("ReleasingAgentId"); } }
       
	 
    private releasingAgentAddressId: string;
    public get ReleasingAgentAddressId() { return this.releasingAgentAddressId; }
    public set ReleasingAgentAddressId(newValue: string) { if (this.releasingAgentAddressId != newValue) { this.releasingAgentAddressId = newValue; this.MarkAsDirty("ReleasingAgentAddressId"); } }
       
	 
    private releasingAgentContactId: string;
    public get ReleasingAgentContactId() { return this.releasingAgentContactId; }
    public set ReleasingAgentContactId(newValue: string) { if (this.releasingAgentContactId != newValue) { this.releasingAgentContactId = newValue; this.MarkAsDirty("ReleasingAgentContactId"); } }
       
	 
    private releasingAgentReference1: string;
    public get ReleasingAgentReference1() { return this.releasingAgentReference1; }
    public set ReleasingAgentReference1(newValue: string) { if (this.releasingAgentReference1 != newValue) { this.releasingAgentReference1 = newValue; this.MarkAsDirty("ReleasingAgentReference1"); } }
       
	 
    private releasingAgentReference2: string;
    public get ReleasingAgentReference2() { return this.releasingAgentReference2; }
    public set ReleasingAgentReference2(newValue: string) { if (this.releasingAgentReference2 != newValue) { this.releasingAgentReference2 = newValue; this.MarkAsDirty("ReleasingAgentReference2"); } }
       
	 
    private releasingAgentName: string;
    public get ReleasingAgentName() { return this.releasingAgentName; }
    public set ReleasingAgentName(newValue: string) { if (this.releasingAgentName != newValue) { this.releasingAgentName = newValue; this.MarkAsDirty("ReleasingAgentName"); } }
       
	 
    private releasingAgentNote: string;
    public get ReleasingAgentNote() { return this.releasingAgentNote; }
    public set ReleasingAgentNote(newValue: string) { if (this.releasingAgentNote != newValue) { this.releasingAgentNote = newValue; this.MarkAsDirty("ReleasingAgentNote"); } }
       
	 
    private noFreightFile: boolean;
    public get NoFreightFile() { return this.noFreightFile; }
    public set NoFreightFile(newValue: boolean) { if (this.noFreightFile != newValue) { this.noFreightFile = newValue; this.MarkAsDirty("NoFreightFile"); } }
       
	 
    private isExceptionResolved: boolean;
    public get IsExceptionResolved() { return this.isExceptionResolved; }
    public set IsExceptionResolved(newValue: boolean) { if (this.isExceptionResolved != newValue) { this.isExceptionResolved = newValue; this.MarkAsDirty("IsExceptionResolved"); } }

    private airlinePrefix: string;
    public get AirlinePrefix() { return this.airlinePrefix; }
    public set AirlinePrefix(newValue: string) { if (this.airlinePrefix != newValue) { this.airlinePrefix = newValue; this.MarkAsDirty("AirlinePrefix"); } }
       
	 
    private mainCarriageCarrierPrefix: string;
    public get MainCarriageCarrierPrefix() { return this.mainCarriageCarrierPrefix; }
    public set MainCarriageCarrierPrefix(newValue: string) { if (this.mainCarriageCarrierPrefix != newValue) { this.mainCarriageCarrierPrefix = newValue; this.MarkAsDirty("MainCarriageCarrierPrefix"); } }
       
	 
    private transshipment1CarrierPrefix: string;
    public get Transshipment1CarrierPrefix() { return this.transshipment1CarrierPrefix; }
    public set Transshipment1CarrierPrefix(newValue: string) { if (this.transshipment1CarrierPrefix != newValue) { this.transshipment1CarrierPrefix = newValue; this.MarkAsDirty("Transshipment1CarrierPrefix"); } }
       
	 
    private transshipment2CarrierPrefix: string;
    public get Transshipment2CarrierPrefix() { return this.transshipment2CarrierPrefix; }
    public set Transshipment2CarrierPrefix(newValue: string) { if (this.transshipment2CarrierPrefix != newValue) { this.transshipment2CarrierPrefix = newValue; this.MarkAsDirty("Transshipment2CarrierPrefix"); } }
       
	 
    private transshipment3CarrierPrefix: string;
    public get Transshipment3CarrierPrefix() { return this.transshipment3CarrierPrefix; }
    public set Transshipment3CarrierPrefix(newValue: string) { if (this.transshipment3CarrierPrefix != newValue) { this.transshipment3CarrierPrefix = newValue; this.MarkAsDirty("Transshipment3CarrierPrefix"); } }
       
	 
    private fromPortId: string;
    public get FromPortId() { return this.fromPortId; }
    public set FromPortId(newValue: string) { if (this.fromPortId != newValue) { this.fromPortId = newValue; this.MarkAsDirty("FromPortId"); } }
       
	 
    private toPortId: string;
    public get ToPortId() { return this.toPortId; }
    public set ToPortId(newValue: string) { if (this.toPortId != newValue) { this.toPortId = newValue; this.MarkAsDirty("ToPortId"); } }
       
	 
    private preCarriageTransportModeId: string;
    public get PreCarriageTransportModeId() { return this.preCarriageTransportModeId; }
    public set PreCarriageTransportModeId(newValue: string) { if (this.preCarriageTransportModeId != newValue) { this.preCarriageTransportModeId = newValue; this.MarkAsDirty("PreCarriageTransportModeId"); } }
       
	 
    private preCarriageFromPortId: string;
    public get PreCarriageFromPortId() { return this.preCarriageFromPortId; }
    public set PreCarriageFromPortId(newValue: string) { if (this.preCarriageFromPortId != newValue) { this.preCarriageFromPortId = newValue; this.MarkAsDirty("PreCarriageFromPortId"); } }
       
	 
    private preCarriageToPortId: string;
    public get PreCarriageToPortId() { return this.preCarriageToPortId; }
    public set PreCarriageToPortId(newValue: string) { if (this.preCarriageToPortId != newValue) { this.preCarriageToPortId = newValue; this.MarkAsDirty("PreCarriageToPortId"); } }
       
	 
    private preCarriageCarrierId: string;
    public get PreCarriageCarrierId() { return this.preCarriageCarrierId; }
    public set PreCarriageCarrierId(newValue: string) { if (this.preCarriageCarrierId != newValue) { this.preCarriageCarrierId = newValue; this.MarkAsDirty("PreCarriageCarrierId"); } }
       
	 
    private preCarriageCarrierNumber: string;
    public get PreCarriageCarrierNumber() { return this.preCarriageCarrierNumber; }
    public set PreCarriageCarrierNumber(newValue: string) { if (this.preCarriageCarrierNumber != newValue) { this.preCarriageCarrierNumber = newValue; this.MarkAsDirty("PreCarriageCarrierNumber"); } }
       
	 
    private preCarriageCarrierName: string;
    public get PreCarriageCarrierName() { return this.preCarriageCarrierName; }
    public set PreCarriageCarrierName(newValue: string) { if (this.preCarriageCarrierName != newValue) { this.preCarriageCarrierName = newValue; this.MarkAsDirty("PreCarriageCarrierName"); } }
       
	 
    private preCarriageCarrierCode: string;
    public get PreCarriageCarrierCode() { return this.preCarriageCarrierCode; }
    public set PreCarriageCarrierCode(newValue: string) { if (this.preCarriageCarrierCode != newValue) { this.preCarriageCarrierCode = newValue; this.MarkAsDirty("PreCarriageCarrierCode"); } }
       
	 
    private preCarriageFromPortCode: string;
    public get PreCarriageFromPortCode() { return this.preCarriageFromPortCode; }
    public set PreCarriageFromPortCode(newValue: string) { if (this.preCarriageFromPortCode != newValue) { this.preCarriageFromPortCode = newValue; this.MarkAsDirty("PreCarriageFromPortCode"); } }
       
	 
    private preCarriageFromPortName: string;
    public get PreCarriageFromPortName() { return this.preCarriageFromPortName; }
    public set PreCarriageFromPortName(newValue: string) { if (this.preCarriageFromPortName != newValue) { this.preCarriageFromPortName = newValue; this.MarkAsDirty("PreCarriageFromPortName"); } }
       
	 
    private preCarriageFromPortCountryCode: string;
    public get PreCarriageFromPortCountryCode() { return this.preCarriageFromPortCountryCode; }
    public set PreCarriageFromPortCountryCode(newValue: string) { if (this.preCarriageFromPortCountryCode != newValue) { this.preCarriageFromPortCountryCode = newValue; this.MarkAsDirty("PreCarriageFromPortCountryCode"); } }
       
	 
    private preCarriageFromPortCountryName: string;
    public get PreCarriageFromPortCountryName() { return this.preCarriageFromPortCountryName; }
    public set PreCarriageFromPortCountryName(newValue: string) { if (this.preCarriageFromPortCountryName != newValue) { this.preCarriageFromPortCountryName = newValue; this.MarkAsDirty("PreCarriageFromPortCountryName"); } }
       
	 
    private preCarriageToPortCode: string;
    public get PreCarriageToPortCode() { return this.preCarriageToPortCode; }
    public set PreCarriageToPortCode(newValue: string) { if (this.preCarriageToPortCode != newValue) { this.preCarriageToPortCode = newValue; this.MarkAsDirty("PreCarriageToPortCode"); } }
       
	 
    private preCarriageToPortName: string;
    public get PreCarriageToPortName() { return this.preCarriageToPortName; }
    public set PreCarriageToPortName(newValue: string) { if (this.preCarriageToPortName != newValue) { this.preCarriageToPortName = newValue; this.MarkAsDirty("PreCarriageToPortName"); } }
       
	 
    private preCarriageToPortCountryCode: string;
    public get PreCarriageToPortCountryCode() { return this.preCarriageToPortCountryCode; }
    public set PreCarriageToPortCountryCode(newValue: string) { if (this.preCarriageToPortCountryCode != newValue) { this.preCarriageToPortCountryCode = newValue; this.MarkAsDirty("PreCarriageToPortCountryCode"); } }
       
	 
    private preCarriageToPortCountryName: string;
    public get PreCarriageToPortCountryName() { return this.preCarriageToPortCountryName; }
    public set PreCarriageToPortCountryName(newValue: string) { if (this.preCarriageToPortCountryName != newValue) { this.preCarriageToPortCountryName = newValue; this.MarkAsDirty("PreCarriageToPortCountryName"); } }
       
	 
    private preCarriageETD: Date;
    public get PreCarriageETD() { return this.preCarriageETD; }
    public set PreCarriageETD(newValue: Date) { if (this.preCarriageETD != newValue) { this.preCarriageETD = newValue; this.MarkAsDirty("PreCarriageETD"); } }
       
	 
    private preCarriageATD: Date;
    public get PreCarriageATD() { return this.preCarriageATD; }
    public set PreCarriageATD(newValue: Date) { if (this.preCarriageATD != newValue) { this.preCarriageATD = newValue; this.MarkAsDirty("PreCarriageATD"); } }
       
	 
    private preCarriageETA: Date;
    public get PreCarriageETA() { return this.preCarriageETA; }
    public set PreCarriageETA(newValue: Date) { if (this.preCarriageETA != newValue) { this.preCarriageETA = newValue; this.MarkAsDirty("PreCarriageETA"); } }
       
	 
    private preCarriageATA: Date;
    public get PreCarriageATA() { return this.preCarriageATA; }
    public set PreCarriageATA(newValue: Date) { if (this.preCarriageATA != newValue) { this.preCarriageATA = newValue; this.MarkAsDirty("PreCarriageATA"); } }
       
	 
    private preCarriageCarrierWebSite: string;
    public get PreCarriageCarrierWebSite() { return this.preCarriageCarrierWebSite; }
    public set PreCarriageCarrierWebSite(newValue: string) { if (this.preCarriageCarrierWebSite != newValue) { this.preCarriageCarrierWebSite = newValue; this.MarkAsDirty("PreCarriageCarrierWebSite"); } }
       
	 
    private onCarriageTransportModeId: string;
    public get OnCarriageTransportModeId() { return this.onCarriageTransportModeId; }
    public set OnCarriageTransportModeId(newValue: string) { if (this.onCarriageTransportModeId != newValue) { this.onCarriageTransportModeId = newValue; this.MarkAsDirty("OnCarriageTransportModeId"); } }
       
	 
    private onCarriageFromPortId: string;
    public get OnCarriageFromPortId() { return this.onCarriageFromPortId; }
    public set OnCarriageFromPortId(newValue: string) { if (this.onCarriageFromPortId != newValue) { this.onCarriageFromPortId = newValue; this.MarkAsDirty("OnCarriageFromPortId"); } }
       
	 
    private onCarriageToPortId: string;
    public get OnCarriageToPortId() { return this.onCarriageToPortId; }
    public set OnCarriageToPortId(newValue: string) { if (this.onCarriageToPortId != newValue) { this.onCarriageToPortId = newValue; this.MarkAsDirty("OnCarriageToPortId"); } }
       
	 
    private onCarriageCarrierId: string;
    public get OnCarriageCarrierId() { return this.onCarriageCarrierId; }
    public set OnCarriageCarrierId(newValue: string) { if (this.onCarriageCarrierId != newValue) { this.onCarriageCarrierId = newValue; this.MarkAsDirty("OnCarriageCarrierId"); } }
       
	 
    private onCarriageCarrierNumber: string;
    public get OnCarriageCarrierNumber() { return this.onCarriageCarrierNumber; }
    public set OnCarriageCarrierNumber(newValue: string) { if (this.onCarriageCarrierNumber != newValue) { this.onCarriageCarrierNumber = newValue; this.MarkAsDirty("OnCarriageCarrierNumber"); } }
       
	 
    private onCarriageCarrierName: string;
    public get OnCarriageCarrierName() { return this.onCarriageCarrierName; }
    public set OnCarriageCarrierName(newValue: string) { if (this.onCarriageCarrierName != newValue) { this.onCarriageCarrierName = newValue; this.MarkAsDirty("OnCarriageCarrierName"); } }
       
	 
    private onCarriageCarrierCode: string;
    public get OnCarriageCarrierCode() { return this.onCarriageCarrierCode; }
    public set OnCarriageCarrierCode(newValue: string) { if (this.onCarriageCarrierCode != newValue) { this.onCarriageCarrierCode = newValue; this.MarkAsDirty("OnCarriageCarrierCode"); } }
       
	 
    private onCarriageFromPortCode: string;
    public get OnCarriageFromPortCode() { return this.onCarriageFromPortCode; }
    public set OnCarriageFromPortCode(newValue: string) { if (this.onCarriageFromPortCode != newValue) { this.onCarriageFromPortCode = newValue; this.MarkAsDirty("OnCarriageFromPortCode"); } }
       
	 
    private onCarriageFromPortName: string;
    public get OnCarriageFromPortName() { return this.onCarriageFromPortName; }
    public set OnCarriageFromPortName(newValue: string) { if (this.onCarriageFromPortName != newValue) { this.onCarriageFromPortName = newValue; this.MarkAsDirty("OnCarriageFromPortName"); } }
       
	 
    private onCarriageFromPortCountryCode: string;
    public get OnCarriageFromPortCountryCode() { return this.onCarriageFromPortCountryCode; }
    public set OnCarriageFromPortCountryCode(newValue: string) { if (this.onCarriageFromPortCountryCode != newValue) { this.onCarriageFromPortCountryCode = newValue; this.MarkAsDirty("OnCarriageFromPortCountryCode"); } }
       
	 
    private onCarriageFromPortCountryName: string;
    public get OnCarriageFromPortCountryName() { return this.onCarriageFromPortCountryName; }
    public set OnCarriageFromPortCountryName(newValue: string) { if (this.onCarriageFromPortCountryName != newValue) { this.onCarriageFromPortCountryName = newValue; this.MarkAsDirty("OnCarriageFromPortCountryName"); } }
       
	 
    private onCarriageToPortCode: string;
    public get OnCarriageToPortCode() { return this.onCarriageToPortCode; }
    public set OnCarriageToPortCode(newValue: string) { if (this.onCarriageToPortCode != newValue) { this.onCarriageToPortCode = newValue; this.MarkAsDirty("OnCarriageToPortCode"); } }
       
	 
    private onCarriageToPortName: string;
    public get OnCarriageToPortName() { return this.onCarriageToPortName; }
    public set OnCarriageToPortName(newValue: string) { if (this.onCarriageToPortName != newValue) { this.onCarriageToPortName = newValue; this.MarkAsDirty("OnCarriageToPortName"); } }
       
	 
    private onCarriageToPortCountryCode: string;
    public get OnCarriageToPortCountryCode() { return this.onCarriageToPortCountryCode; }
    public set OnCarriageToPortCountryCode(newValue: string) { if (this.onCarriageToPortCountryCode != newValue) { this.onCarriageToPortCountryCode = newValue; this.MarkAsDirty("OnCarriageToPortCountryCode"); } }
       
	 
    private onCarriageToPortCountryName: string;
    public get OnCarriageToPortCountryName() { return this.onCarriageToPortCountryName; }
    public set OnCarriageToPortCountryName(newValue: string) { if (this.onCarriageToPortCountryName != newValue) { this.onCarriageToPortCountryName = newValue; this.MarkAsDirty("OnCarriageToPortCountryName"); } }
       
	 
    private onCarriageETD: Date;
    public get OnCarriageETD() { return this.onCarriageETD; }
    public set OnCarriageETD(newValue: Date) { if (this.onCarriageETD != newValue) { this.onCarriageETD = newValue; this.MarkAsDirty("OnCarriageETD"); } }
       
	 
    private onCarriageATD: Date;
    public get OnCarriageATD() { return this.onCarriageATD; }
    public set OnCarriageATD(newValue: Date) { if (this.onCarriageATD != newValue) { this.onCarriageATD = newValue; this.MarkAsDirty("OnCarriageATD"); } }
       
	 
    private onCarriageETA: Date;
    public get OnCarriageETA() { return this.onCarriageETA; }
    public set OnCarriageETA(newValue: Date) { if (this.onCarriageETA != newValue) { this.onCarriageETA = newValue; this.MarkAsDirty("OnCarriageETA"); } }
       
	 
    private onCarriageATA: Date;
    public get OnCarriageATA() { return this.onCarriageATA; }
    public set OnCarriageATA(newValue: Date) { if (this.onCarriageATA != newValue) { this.onCarriageATA = newValue; this.MarkAsDirty("OnCarriageATA"); } }
       
	 
    private onCarriageCarrierWebSite: string;
    public get OnCarriageCarrierWebSite() { return this.onCarriageCarrierWebSite; }
    public set OnCarriageCarrierWebSite(newValue: string) { if (this.onCarriageCarrierWebSite != newValue) { this.onCarriageCarrierWebSite = newValue; this.MarkAsDirty("OnCarriageCarrierWebSite"); } }
       
	 
    private mainCarriageTransportModeId: string;
    public get MainCarriageTransportModeId() { return this.mainCarriageTransportModeId; }
    public set MainCarriageTransportModeId(newValue: string) { if (this.mainCarriageTransportModeId != newValue) { this.mainCarriageTransportModeId = newValue; this.MarkAsDirty("MainCarriageTransportModeId"); } }
       
	 
    private mainCarriageFromPortId: string;
    public get MainCarriageFromPortId() { return this.mainCarriageFromPortId; }
    public set MainCarriageFromPortId(newValue: string) { if (this.mainCarriageFromPortId != newValue) { this.mainCarriageFromPortId = newValue; this.MarkAsDirty("MainCarriageFromPortId"); } }
       
	 
    private mainCarriageToPortId: string;
    public get MainCarriageToPortId() { return this.mainCarriageToPortId; }
    public set MainCarriageToPortId(newValue: string) { if (this.mainCarriageToPortId != newValue) { this.mainCarriageToPortId = newValue; this.MarkAsDirty("MainCarriageToPortId"); } }
       
	 
    private mainCarriageFromPortCode: string;
    public get MainCarriageFromPortCode() { return this.mainCarriageFromPortCode; }
    public set MainCarriageFromPortCode(newValue: string) { if (this.mainCarriageFromPortCode != newValue) { this.mainCarriageFromPortCode = newValue; this.MarkAsDirty("MainCarriageFromPortCode"); } }
       
	 
    private mainCarriageFromPortName: string;
    public get MainCarriageFromPortName() { return this.mainCarriageFromPortName; }
    public set MainCarriageFromPortName(newValue: string) { if (this.mainCarriageFromPortName != newValue) { this.mainCarriageFromPortName = newValue; this.MarkAsDirty("MainCarriageFromPortName"); } }
       
	 
    private mainCarriageFromPortCountryName: string;
    public get MainCarriageFromPortCountryName() { return this.mainCarriageFromPortCountryName; }
    public set MainCarriageFromPortCountryName(newValue: string) { if (this.mainCarriageFromPortCountryName != newValue) { this.mainCarriageFromPortCountryName = newValue; this.MarkAsDirty("MainCarriageFromPortCountryName"); } }
       
	 
    private mainCarriageFromPortCountryCode: string;
    public get MainCarriageFromPortCountryCode() { return this.mainCarriageFromPortCountryCode; }
    public set MainCarriageFromPortCountryCode(newValue: string) { if (this.mainCarriageFromPortCountryCode != newValue) { this.mainCarriageFromPortCountryCode = newValue; this.MarkAsDirty("MainCarriageFromPortCountryCode"); } }
       
	 
    private mainCarriageToPortCode: string;
    public get MainCarriageToPortCode() { return this.mainCarriageToPortCode; }
    public set MainCarriageToPortCode(newValue: string) { if (this.mainCarriageToPortCode != newValue) { this.mainCarriageToPortCode = newValue; this.MarkAsDirty("MainCarriageToPortCode"); } }
       
	 
    private mainCarriageToPortName: string;
    public get MainCarriageToPortName() { return this.mainCarriageToPortName; }
    public set MainCarriageToPortName(newValue: string) { if (this.mainCarriageToPortName != newValue) { this.mainCarriageToPortName = newValue; this.MarkAsDirty("MainCarriageToPortName"); } }
       
	 
    private mainCarriageToPortCountryCode: string;
    public get MainCarriageToPortCountryCode() { return this.mainCarriageToPortCountryCode; }
    public set MainCarriageToPortCountryCode(newValue: string) { if (this.mainCarriageToPortCountryCode != newValue) { this.mainCarriageToPortCountryCode = newValue; this.MarkAsDirty("MainCarriageToPortCountryCode"); } }
       
	 
    private mainCarriageToPortCountryName: string;
    public get MainCarriageToPortCountryName() { return this.mainCarriageToPortCountryName; }
    public set MainCarriageToPortCountryName(newValue: string) { if (this.mainCarriageToPortCountryName != newValue) { this.mainCarriageToPortCountryName = newValue; this.MarkAsDirty("MainCarriageToPortCountryName"); } }
       
	 

    private mainCarriageVesselId: string;
    public get MainCarriageVesselId() { return this.mainCarriageVesselId; }
    public set MainCarriageVesselId(newValue: string) { if (this.mainCarriageVesselId != newValue) { this.mainCarriageVesselId = newValue; this.MarkAsDirty("MainCarriageVesselId"); } }
       
	 
    private preCarriageVesselId: string;
    public get PreCarriageVesselId() { return this.preCarriageVesselId; }
    public set PreCarriageVesselId(newValue: string) { if (this.preCarriageVesselId != newValue) { this.preCarriageVesselId = newValue; this.MarkAsDirty("PreCarriageVesselId"); } }
       
	 
    private onCarriageVesselId: string;
    public get OnCarriageVesselId() { return this.onCarriageVesselId; }
    public set OnCarriageVesselId(newValue: string) { if (this.onCarriageVesselId != newValue) { this.onCarriageVesselId = newValue; this.MarkAsDirty("OnCarriageVesselId"); } }
       
	 
    private transshipment1VesselId: string;
    public get Transshipment1VesselId() { return this.transshipment1VesselId; }
    public set Transshipment1VesselId(newValue: string) { if (this.transshipment1VesselId != newValue) { this.transshipment1VesselId = newValue; this.MarkAsDirty("Transshipment1VesselId"); } }
       
	 
    private transshipment2VesselId: string;
    public get Transshipment2VesselId() { return this.transshipment2VesselId; }
    public set Transshipment2VesselId(newValue: string) { if (this.transshipment2VesselId != newValue) { this.transshipment2VesselId = newValue; this.MarkAsDirty("Transshipment2VesselId"); } }
       
	 
    private transshipment3VesselId: string;
    public get Transshipment3VesselId() { return this.transshipment3VesselId; }
    public set Transshipment3VesselId(newValue: string) { if (this.transshipment3VesselId != newValue) { this.transshipment3VesselId = newValue; this.MarkAsDirty("Transshipment3VesselId"); } }
       
	 
    private mainCarriageVesselName: string;
    public get MainCarriageVesselName() { return this.mainCarriageVesselName; }
    public set MainCarriageVesselName(newValue: string) { if (this.mainCarriageVesselName != newValue) { this.mainCarriageVesselName = newValue; this.MarkAsDirty("MainCarriageVesselName"); } }
       
	 
    private preCarriageVesselName: string;
    public get PreCarriageVesselName() { return this.preCarriageVesselName; }
    public set PreCarriageVesselName(newValue: string) { if (this.preCarriageVesselName != newValue) { this.preCarriageVesselName = newValue; this.MarkAsDirty("PreCarriageVesselName"); } }
       
	 
    private onCarriageVesselName: string;
    public get OnCarriageVesselName() { return this.onCarriageVesselName; }
    public set OnCarriageVesselName(newValue: string) { if (this.onCarriageVesselName != newValue) { this.onCarriageVesselName = newValue; this.MarkAsDirty("OnCarriageVesselName"); } }
       
	 
    private transshipment1VesselName: string;
    public get Transshipment1VesselName() { return this.transshipment1VesselName; }
    public set Transshipment1VesselName(newValue: string) { if (this.transshipment1VesselName != newValue) { this.transshipment1VesselName = newValue; this.MarkAsDirty("Transshipment1VesselName"); } }
       
	 
    private transshipment2VesselName: string;
    public get Transshipment2VesselName() { return this.transshipment2VesselName; }
    public set Transshipment2VesselName(newValue: string) { if (this.transshipment2VesselName != newValue) { this.transshipment2VesselName = newValue; this.MarkAsDirty("Transshipment2VesselName"); } }
       
	 
    private transshipment3VesselName: string;
    public get Transshipment3VesselName() { return this.transshipment3VesselName; }
    public set Transshipment3VesselName(newValue: string) { if (this.transshipment3VesselName != newValue) { this.transshipment3VesselName = newValue; this.MarkAsDirty("Transshipment3VesselName"); } }
       
	 
    private mainCarriageIsFromStack: boolean;
    public get MainCarriageIsFromStack() { return this.mainCarriageIsFromStack; }
    public set MainCarriageIsFromStack(newValue: boolean) { if (this.mainCarriageIsFromStack != newValue) { this.mainCarriageIsFromStack = newValue; this.MarkAsDirty("MainCarriageIsFromStack"); } }
       
	 
    private mainCarriageATD: Date;
    public get MainCarriageATD() { return this.mainCarriageATD; }
    public set MainCarriageATD(newValue: Date) { if (this.mainCarriageATD != newValue) { this.mainCarriageATD = newValue; this.MarkAsDirty("MainCarriageATD"); } }
       
	 
    private mainCarriageATA: Date;
    public get MainCarriageATA() { return this.mainCarriageATA; }
    public set MainCarriageATA(newValue: Date) { if (this.mainCarriageATA != newValue) { this.mainCarriageATA = newValue; this.MarkAsDirty("MainCarriageATA"); } }
       
	 
    private mainCarriageETD: Date;
    public get MainCarriageETD() { return this.mainCarriageETD; }
    public set MainCarriageETD(newValue: Date) { if (this.mainCarriageETD != newValue) { this.mainCarriageETD = newValue; this.MarkAsDirty("MainCarriageETD"); } }
       
	 
    private mainCarriageETA: Date;
    public get MainCarriageETA() { return this.mainCarriageETA; }
    public set MainCarriageETA(newValue: Date) { if (this.mainCarriageETA != newValue) { this.mainCarriageETA = newValue; this.MarkAsDirty("MainCarriageETA"); } }
       
	 
    private transshipment1FromPortId: string;
    public get Transshipment1FromPortId() { return this.transshipment1FromPortId; }
    public set Transshipment1FromPortId(newValue: string) { if (this.transshipment1FromPortId != newValue) { this.transshipment1FromPortId = newValue; this.MarkAsDirty("Transshipment1FromPortId"); } }
       
	 
    private transshipment1ToPortId: string;
    public get Transshipment1ToPortId() { return this.transshipment1ToPortId; }
    public set Transshipment1ToPortId(newValue: string) { if (this.transshipment1ToPortId != newValue) { this.transshipment1ToPortId = newValue; this.MarkAsDirty("Transshipment1ToPortId"); } }
       
	 
    private transshipment1ATD: Date;
    public get Transshipment1ATD() { return this.transshipment1ATD; }
    public set Transshipment1ATD(newValue: Date) { if (this.transshipment1ATD != newValue) { this.transshipment1ATD = newValue; this.MarkAsDirty("Transshipment1ATD"); } }
       
	 
    private transshipment1ATA: Date;
    public get Transshipment1ATA() { return this.transshipment1ATA; }
    public set Transshipment1ATA(newValue: Date) { if (this.transshipment1ATA != newValue) { this.transshipment1ATA = newValue; this.MarkAsDirty("Transshipment1ATA"); } }
       
	 
    private transshipment1ETD: Date;
    public get Transshipment1ETD() { return this.transshipment1ETD; }
    public set Transshipment1ETD(newValue: Date) { if (this.transshipment1ETD != newValue) { this.transshipment1ETD = newValue; this.MarkAsDirty("Transshipment1ETD"); } }
       
	 
    private transshipment1ETA: Date;
    public get Transshipment1ETA() { return this.transshipment1ETA; }
    public set Transshipment1ETA(newValue: Date) { if (this.transshipment1ETA != newValue) { this.transshipment1ETA = newValue; this.MarkAsDirty("Transshipment1ETA"); } }
       
	 
    private transshipment1CarrierNumber: string;
    public get Transshipment1CarrierNumber() { return this.transshipment1CarrierNumber; }
    public set Transshipment1CarrierNumber(newValue: string) { if (this.transshipment1CarrierNumber != newValue) { this.transshipment1CarrierNumber = newValue; this.MarkAsDirty("Transshipment1CarrierNumber"); } }
       
	 
    private transshipment1CarrierId: string;
    public get Transshipment1CarrierId() { return this.transshipment1CarrierId; }
    public set Transshipment1CarrierId(newValue: string) { if (this.transshipment1CarrierId != newValue) { this.transshipment1CarrierId = newValue; this.MarkAsDirty("Transshipment1CarrierId"); } }
       
	 
    private transshipment1CarrierName: string;
    public get Transshipment1CarrierName() { return this.transshipment1CarrierName; }
    public set Transshipment1CarrierName(newValue: string) { if (this.transshipment1CarrierName != newValue) { this.transshipment1CarrierName = newValue; this.MarkAsDirty("Transshipment1CarrierName"); } }
       
	 
    private transshipment1CarrierCode: string;
    public get Transshipment1CarrierCode() { return this.transshipment1CarrierCode; }
    public set Transshipment1CarrierCode(newValue: string) { if (this.transshipment1CarrierCode != newValue) { this.transshipment1CarrierCode = newValue; this.MarkAsDirty("Transshipment1CarrierCode"); } }
       
	 
    private transshipment1FromPortCode: string;
    public get Transshipment1FromPortCode() { return this.transshipment1FromPortCode; }
    public set Transshipment1FromPortCode(newValue: string) { if (this.transshipment1FromPortCode != newValue) { this.transshipment1FromPortCode = newValue; this.MarkAsDirty("Transshipment1FromPortCode"); } }
       
	 
    private transshipment1FromPortName: string;
    public get Transshipment1FromPortName() { return this.transshipment1FromPortName; }
    public set Transshipment1FromPortName(newValue: string) { if (this.transshipment1FromPortName != newValue) { this.transshipment1FromPortName = newValue; this.MarkAsDirty("Transshipment1FromPortName"); } }
       
	 
    private transshipment1FromPortCountryCode: string;
    public get Transshipment1FromPortCountryCode() { return this.transshipment1FromPortCountryCode; }
    public set Transshipment1FromPortCountryCode(newValue: string) { if (this.transshipment1FromPortCountryCode != newValue) { this.transshipment1FromPortCountryCode = newValue; this.MarkAsDirty("Transshipment1FromPortCountryCode"); } }
       
	 
    private transshipment1FromPortCountryName: string;
    public get Transshipment1FromPortCountryName() { return this.transshipment1FromPortCountryName; }
    public set Transshipment1FromPortCountryName(newValue: string) { if (this.transshipment1FromPortCountryName != newValue) { this.transshipment1FromPortCountryName = newValue; this.MarkAsDirty("Transshipment1FromPortCountryName"); } }
       
	 
    private transshipment1ToPortCode: string;
    public get Transshipment1ToPortCode() { return this.transshipment1ToPortCode; }
    public set Transshipment1ToPortCode(newValue: string) { if (this.transshipment1ToPortCode != newValue) { this.transshipment1ToPortCode = newValue; this.MarkAsDirty("Transshipment1ToPortCode"); } }
       
	 
    private transshipment1ToPortName: string;
    public get Transshipment1ToPortName() { return this.transshipment1ToPortName; }
    public set Transshipment1ToPortName(newValue: string) { if (this.transshipment1ToPortName != newValue) { this.transshipment1ToPortName = newValue; this.MarkAsDirty("Transshipment1ToPortName"); } }
       
	 
    private transshipment1ToPortCountryCode: string;
    public get Transshipment1ToPortCountryCode() { return this.transshipment1ToPortCountryCode; }
    public set Transshipment1ToPortCountryCode(newValue: string) { if (this.transshipment1ToPortCountryCode != newValue) { this.transshipment1ToPortCountryCode = newValue; this.MarkAsDirty("Transshipment1ToPortCountryCode"); } }
       
	 
    private transshipment1ToPortCountryName: string;
    public get Transshipment1ToPortCountryName() { return this.transshipment1ToPortCountryName; }
    public set Transshipment1ToPortCountryName(newValue: string) { if (this.transshipment1ToPortCountryName != newValue) { this.transshipment1ToPortCountryName = newValue; this.MarkAsDirty("Transshipment1ToPortCountryName"); } }
       
	 
    private transshipment1CarrierWebSite: string;
    public get Transshipment1CarrierWebSite() { return this.transshipment1CarrierWebSite; }
    public set Transshipment1CarrierWebSite(newValue: string) { if (this.transshipment1CarrierWebSite != newValue) { this.transshipment1CarrierWebSite = newValue; this.MarkAsDirty("Transshipment1CarrierWebSite"); } }
       
	 
    private transshipment2CarrierWebSite: string;
    public get Transshipment2CarrierWebSite() { return this.transshipment2CarrierWebSite; }
    public set Transshipment2CarrierWebSite(newValue: string) { if (this.transshipment2CarrierWebSite != newValue) { this.transshipment2CarrierWebSite = newValue; this.MarkAsDirty("Transshipment2CarrierWebSite"); } }
       
	 
    private transshipment3CarrierWebSite: string;
    public get Transshipment3CarrierWebSite() { return this.transshipment3CarrierWebSite; }
    public set Transshipment3CarrierWebSite(newValue: string) { if (this.transshipment3CarrierWebSite != newValue) { this.transshipment3CarrierWebSite = newValue; this.MarkAsDirty("Transshipment3CarrierWebSite"); } }
       
	 
    private transshipment2FromPortId: string;
    public get Transshipment2FromPortId() { return this.transshipment2FromPortId; }
    public set Transshipment2FromPortId(newValue: string) { if (this.transshipment2FromPortId != newValue) { this.transshipment2FromPortId = newValue; this.MarkAsDirty("Transshipment2FromPortId"); } }
       
	 
    private transshipment2ToPortId: string;
    public get Transshipment2ToPortId() { return this.transshipment2ToPortId; }
    public set Transshipment2ToPortId(newValue: string) { if (this.transshipment2ToPortId != newValue) { this.transshipment2ToPortId = newValue; this.MarkAsDirty("Transshipment2ToPortId"); } }
       
	 
    private transshipment2ATD: Date;
    public get Transshipment2ATD() { return this.transshipment2ATD; }
    public set Transshipment2ATD(newValue: Date) { if (this.transshipment2ATD != newValue) { this.transshipment2ATD = newValue; this.MarkAsDirty("Transshipment2ATD"); } }
       
	 
    private transshipment2ATA: Date;
    public get Transshipment2ATA() { return this.transshipment2ATA; }
    public set Transshipment2ATA(newValue: Date) { if (this.transshipment2ATA != newValue) { this.transshipment2ATA = newValue; this.MarkAsDirty("Transshipment2ATA"); } }
       
	 
    private transshipment2ETD: Date;
    public get Transshipment2ETD() { return this.transshipment2ETD; }
    public set Transshipment2ETD(newValue: Date) { if (this.transshipment2ETD != newValue) { this.transshipment2ETD = newValue; this.MarkAsDirty("Transshipment2ETD"); } }
       
	 
    private transshipment2ETA: Date;
    public get Transshipment2ETA() { return this.transshipment2ETA; }
    public set Transshipment2ETA(newValue: Date) { if (this.transshipment2ETA != newValue) { this.transshipment2ETA = newValue; this.MarkAsDirty("Transshipment2ETA"); } }
       
	 
    private transshipment2CarrierNumber: string;
    public get Transshipment2CarrierNumber() { return this.transshipment2CarrierNumber; }
    public set Transshipment2CarrierNumber(newValue: string) { if (this.transshipment2CarrierNumber != newValue) { this.transshipment2CarrierNumber = newValue; this.MarkAsDirty("Transshipment2CarrierNumber"); } }
       
	 
    private transshipment2CarrierId: string;
    public get Transshipment2CarrierId() { return this.transshipment2CarrierId; }
    public set Transshipment2CarrierId(newValue: string) { if (this.transshipment2CarrierId != newValue) { this.transshipment2CarrierId = newValue; this.MarkAsDirty("Transshipment2CarrierId"); } }
       
	 
    private transshipment2CarrierName: string;
    public get Transshipment2CarrierName() { return this.transshipment2CarrierName; }
    public set Transshipment2CarrierName(newValue: string) { if (this.transshipment2CarrierName != newValue) { this.transshipment2CarrierName = newValue; this.MarkAsDirty("Transshipment2CarrierName"); } }
       
	 
    private transshipment2CarrierCode: string;
    public get Transshipment2CarrierCode() { return this.transshipment2CarrierCode; }
    public set Transshipment2CarrierCode(newValue: string) { if (this.transshipment2CarrierCode != newValue) { this.transshipment2CarrierCode = newValue; this.MarkAsDirty("Transshipment2CarrierCode"); } }
       
	 
    private transshipment2FromPortCode: string;
    public get Transshipment2FromPortCode() { return this.transshipment2FromPortCode; }
    public set Transshipment2FromPortCode(newValue: string) { if (this.transshipment2FromPortCode != newValue) { this.transshipment2FromPortCode = newValue; this.MarkAsDirty("Transshipment2FromPortCode"); } }
       
	 
    private transshipment2FromPortName: string;
    public get Transshipment2FromPortName() { return this.transshipment2FromPortName; }
    public set Transshipment2FromPortName(newValue: string) { if (this.transshipment2FromPortName != newValue) { this.transshipment2FromPortName = newValue; this.MarkAsDirty("Transshipment2FromPortName"); } }
       
	 
    private transshipment2FromPortCountryCode: string;
    public get Transshipment2FromPortCountryCode() { return this.transshipment2FromPortCountryCode; }
    public set Transshipment2FromPortCountryCode(newValue: string) { if (this.transshipment2FromPortCountryCode != newValue) { this.transshipment2FromPortCountryCode = newValue; this.MarkAsDirty("Transshipment2FromPortCountryCode"); } }
       
	 
    private transshipment2FromPortCountryName: string;
    public get Transshipment2FromPortCountryName() { return this.transshipment2FromPortCountryName; }
    public set Transshipment2FromPortCountryName(newValue: string) { if (this.transshipment2FromPortCountryName != newValue) { this.transshipment2FromPortCountryName = newValue; this.MarkAsDirty("Transshipment2FromPortCountryName"); } }
       
	 
    private transshipment2ToPortCode: string;
    public get Transshipment2ToPortCode() { return this.transshipment2ToPortCode; }
    public set Transshipment2ToPortCode(newValue: string) { if (this.transshipment2ToPortCode != newValue) { this.transshipment2ToPortCode = newValue; this.MarkAsDirty("Transshipment2ToPortCode"); } }
       
	 
    private transshipment2ToPortName: string;
    public get Transshipment2ToPortName() { return this.transshipment2ToPortName; }
    public set Transshipment2ToPortName(newValue: string) { if (this.transshipment2ToPortName != newValue) { this.transshipment2ToPortName = newValue; this.MarkAsDirty("Transshipment2ToPortName"); } }
       
	 
    private transshipment2ToPortCountryCode: string;
    public get Transshipment2ToPortCountryCode() { return this.transshipment2ToPortCountryCode; }
    public set Transshipment2ToPortCountryCode(newValue: string) { if (this.transshipment2ToPortCountryCode != newValue) { this.transshipment2ToPortCountryCode = newValue; this.MarkAsDirty("Transshipment2ToPortCountryCode"); } }
       
	 
    private transshipment2ToPortCountryName: string;
    public get Transshipment2ToPortCountryName() { return this.transshipment2ToPortCountryName; }
    public set Transshipment2ToPortCountryName(newValue: string) { if (this.transshipment2ToPortCountryName != newValue) { this.transshipment2ToPortCountryName = newValue; this.MarkAsDirty("Transshipment2ToPortCountryName"); } }
       
	 
    private transshipment3FromPortId: string;
    public get Transshipment3FromPortId() { return this.transshipment3FromPortId; }
    public set Transshipment3FromPortId(newValue: string) { if (this.transshipment3FromPortId != newValue) { this.transshipment3FromPortId = newValue; this.MarkAsDirty("Transshipment3FromPortId"); } }
       
	 
    private transshipment3ToPortId: string;
    public get Transshipment3ToPortId() { return this.transshipment3ToPortId; }
    public set Transshipment3ToPortId(newValue: string) { if (this.transshipment3ToPortId != newValue) { this.transshipment3ToPortId = newValue; this.MarkAsDirty("Transshipment3ToPortId"); } }
       
	 
    private transshipment3ATD: Date;
    public get Transshipment3ATD() { return this.transshipment3ATD; }
    public set Transshipment3ATD(newValue: Date) { if (this.transshipment3ATD != newValue) { this.transshipment3ATD = newValue; this.MarkAsDirty("Transshipment3ATD"); } }
       
	 
    private transshipment3ATA: Date;
    public get Transshipment3ATA() { return this.transshipment3ATA; }
    public set Transshipment3ATA(newValue: Date) { if (this.transshipment3ATA != newValue) { this.transshipment3ATA = newValue; this.MarkAsDirty("Transshipment3ATA"); } }
       
	 
    private transshipment3ETD: Date;
    public get Transshipment3ETD() { return this.transshipment3ETD; }
    public set Transshipment3ETD(newValue: Date) { if (this.transshipment3ETD != newValue) { this.transshipment3ETD = newValue; this.MarkAsDirty("Transshipment3ETD"); } }
       
	 
    private transshipment3ETA: Date;
    public get Transshipment3ETA() { return this.transshipment3ETA; }
    public set Transshipment3ETA(newValue: Date) { if (this.transshipment3ETA != newValue) { this.transshipment3ETA = newValue; this.MarkAsDirty("Transshipment3ETA"); } }
       
	 
    private transshipment3CarrierNumber: string;
    public get Transshipment3CarrierNumber() { return this.transshipment3CarrierNumber; }
    public set Transshipment3CarrierNumber(newValue: string) { if (this.transshipment3CarrierNumber != newValue) { this.transshipment3CarrierNumber = newValue; this.MarkAsDirty("Transshipment3CarrierNumber"); } }
       
	 
    private transshipment3CarrierId: string;
    public get Transshipment3CarrierId() { return this.transshipment3CarrierId; }
    public set Transshipment3CarrierId(newValue: string) { if (this.transshipment3CarrierId != newValue) { this.transshipment3CarrierId = newValue; this.MarkAsDirty("Transshipment3CarrierId"); } }
       
	 
    private transshipment3CarrierName: string;
    public get Transshipment3CarrierName() { return this.transshipment3CarrierName; }
    public set Transshipment3CarrierName(newValue: string) { if (this.transshipment3CarrierName != newValue) { this.transshipment3CarrierName = newValue; this.MarkAsDirty("Transshipment3CarrierName"); } }
       
	 
    private transshipment3CarrierCode: string;
    public get Transshipment3CarrierCode() { return this.transshipment3CarrierCode; }
    public set Transshipment3CarrierCode(newValue: string) { if (this.transshipment3CarrierCode != newValue) { this.transshipment3CarrierCode = newValue; this.MarkAsDirty("Transshipment3CarrierCode"); } }
       
	 
    private transshipment3FromPortCode: string;
    public get Transshipment3FromPortCode() { return this.transshipment3FromPortCode; }
    public set Transshipment3FromPortCode(newValue: string) { if (this.transshipment3FromPortCode != newValue) { this.transshipment3FromPortCode = newValue; this.MarkAsDirty("Transshipment3FromPortCode"); } }
       
	 
    private transshipment3FromPortName: string;
    public get Transshipment3FromPortName() { return this.transshipment3FromPortName; }
    public set Transshipment3FromPortName(newValue: string) { if (this.transshipment3FromPortName != newValue) { this.transshipment3FromPortName = newValue; this.MarkAsDirty("Transshipment3FromPortName"); } }
       
	 
    private transshipment3FromPortCountryCode: string;
    public get Transshipment3FromPortCountryCode() { return this.transshipment3FromPortCountryCode; }
    public set Transshipment3FromPortCountryCode(newValue: string) { if (this.transshipment3FromPortCountryCode != newValue) { this.transshipment3FromPortCountryCode = newValue; this.MarkAsDirty("Transshipment3FromPortCountryCode"); } }
       
	 
    private transshipment3FromPortCountryName: string;
    public get Transshipment3FromPortCountryName() { return this.transshipment3FromPortCountryName; }
    public set Transshipment3FromPortCountryName(newValue: string) { if (this.transshipment3FromPortCountryName != newValue) { this.transshipment3FromPortCountryName = newValue; this.MarkAsDirty("Transshipment3FromPortCountryName"); } }
       
	 
    private transshipment3ToPortCode: string;
    public get Transshipment3ToPortCode() { return this.transshipment3ToPortCode; }
    public set Transshipment3ToPortCode(newValue: string) { if (this.transshipment3ToPortCode != newValue) { this.transshipment3ToPortCode = newValue; this.MarkAsDirty("Transshipment3ToPortCode"); } }
       
	 
    private transshipment3ToPortName: string;
    public get Transshipment3ToPortName() { return this.transshipment3ToPortName; }
    public set Transshipment3ToPortName(newValue: string) { if (this.transshipment3ToPortName != newValue) { this.transshipment3ToPortName = newValue; this.MarkAsDirty("Transshipment3ToPortName"); } }
       
	 
    private transshipment3ToPortCountryCode: string;
    public get Transshipment3ToPortCountryCode() { return this.transshipment3ToPortCountryCode; }
    public set Transshipment3ToPortCountryCode(newValue: string) { if (this.transshipment3ToPortCountryCode != newValue) { this.transshipment3ToPortCountryCode = newValue; this.MarkAsDirty("Transshipment3ToPortCountryCode"); } }
       
	 
    private transshipment3ToPortCountryName: string;
    public get Transshipment3ToPortCountryName() { return this.transshipment3ToPortCountryName; }
    public set Transshipment3ToPortCountryName(newValue: string) { if (this.transshipment3ToPortCountryName != newValue) { this.transshipment3ToPortCountryName = newValue; this.MarkAsDirty("Transshipment3ToPortCountryName"); } }
     
    private fromCountryIsEC: boolean;
    public get FromCountryIsEC() { return this.fromCountryIsEC; }
    public set FromCountryIsEC(newValue: boolean) { if (this.fromCountryIsEC != newValue) { this.fromCountryIsEC = newValue; this.MarkAsDirty("FromCountryIsEC"); } }
       
    private toCountryIsEC: boolean;
    public get ToCountryIsEC() { return this.toCountryIsEC; }
    public set ToCountryIsEC(newValue: boolean) { if (this.toCountryIsEC != newValue) { this.toCountryIsEC = newValue; this.MarkAsDirty("ToCountryIsEC"); } }

    private transshipment1ToCountryIsEC: boolean;
    public get Transshipment1ToCountryIsEC() { return this.transshipment1ToCountryIsEC; }
    public set Transshipment1ToCountryIsEC(newValue: boolean) { if (this.transshipment1ToCountryIsEC != newValue) { this.transshipment1ToCountryIsEC = newValue; this.MarkAsDirty("Transshipment1ToCountryIsEC"); } }

    private transshipment2ToCountryIsEC: boolean;
    public get Transshipment2ToCountryIsEC() { return this.transshipment2ToCountryIsEC; }
    public set Transshipment2ToCountryIsEC(newValue: boolean) { if (this.transshipment2ToCountryIsEC != newValue) { this.transshipment2ToCountryIsEC = newValue; this.MarkAsDirty("Transshipment2ToCountryIsEC"); } }

    private transshipment3ToCountryIsEC: boolean;
    public get Transshipment3ToCountryIsEC() { return this.transshipment3ToCountryIsEC; }
    public set Transshipment3ToCountryIsEC(newValue: boolean) { if (this.transshipment3ToCountryIsEC != newValue) { this.transshipment3ToCountryIsEC = newValue; this.MarkAsDirty("Transshipment3ToCountryIsEC"); } }

    private finalDistenationPortId: string;
    public get FinalDistenationPortId() { return this.finalDistenationPortId; }
    public set FinalDistenationPortId(newValue: string) { if (this.finalDistenationPortId != newValue) { this.finalDistenationPortId = newValue; this.MarkAsDirty("FinalDistenationPortId"); } }
       
	 
    private transshipment1AdditionalMAWBOBLBL: string;
    public get Transshipment1AdditionalMAWBOBLBL() { return this.transshipment1AdditionalMAWBOBLBL; }
    public set Transshipment1AdditionalMAWBOBLBL(newValue: string) { if (this.transshipment1AdditionalMAWBOBLBL != newValue) { this.transshipment1AdditionalMAWBOBLBL = newValue; this.MarkAsDirty("Transshipment1AdditionalMAWBOBLBL"); } }
       
	 
    private transshipment2AdditionalMAWBOBLBL: string;
    public get Transshipment2AdditionalMAWBOBLBL() { return this.transshipment2AdditionalMAWBOBLBL; }
    public set Transshipment2AdditionalMAWBOBLBL(newValue: string) { if (this.transshipment2AdditionalMAWBOBLBL != newValue) { this.transshipment2AdditionalMAWBOBLBL = newValue; this.MarkAsDirty("Transshipment2AdditionalMAWBOBLBL"); } }
       
	 
    private transshipment3AdditionalMAWBOBLBL: string;
    public get Transshipment3AdditionalMAWBOBLBL() { return this.transshipment3AdditionalMAWBOBLBL; }
    public set Transshipment3AdditionalMAWBOBLBL(newValue: string) { if (this.transshipment3AdditionalMAWBOBLBL != newValue) { this.transshipment3AdditionalMAWBOBLBL = newValue; this.MarkAsDirty("Transshipment3AdditionalMAWBOBLBL"); } }
       
	 
    private fromPort: string;
    public get FromPort() { return this.fromPort; }
    public set FromPort(newValue: string) { if (this.fromPort != newValue) { this.fromPort = newValue; this.MarkAsDirty("FromPort"); } }
       
	 
    private fromPortName: string;
    public get FromPortName() { return this.fromPortName; }
    public set FromPortName(newValue: string) { if (this.fromPortName != newValue) { this.fromPortName = newValue; this.MarkAsDirty("FromPortName"); } }
       
	 
    private fromPortCountry: string;
    public get FromPortCountry() { return this.fromPortCountry; }
    public set FromPortCountry(newValue: string) { if (this.fromPortCountry != newValue) { this.fromPortCountry = newValue; this.MarkAsDirty("FromPortCountry"); } }
       
	 
    private fromPortCountryName: string;
    public get FromPortCountryName() { return this.fromPortCountryName; }
    public set FromPortCountryName(newValue: string) { if (this.fromPortCountryName != newValue) { this.fromPortCountryName = newValue; this.MarkAsDirty("FromPortCountryName"); } }
       
	 
    private toPort: string;
    public get ToPort() { return this.toPort; }
    public set ToPort(newValue: string) { if (this.toPort != newValue) { this.toPort = newValue; this.MarkAsDirty("ToPort"); } }
       
	 
    private toPortName: string;
    public get ToPortName() { return this.toPortName; }
    public set ToPortName(newValue: string) { if (this.toPortName != newValue) { this.toPortName = newValue; this.MarkAsDirty("ToPortName"); } }
       
	 
    private toPortCountry: string;
    public get ToPortCountry() { return this.toPortCountry; }
    public set ToPortCountry(newValue: string) { if (this.toPortCountry != newValue) { this.toPortCountry = newValue; this.MarkAsDirty("ToPortCountry"); } }
       
	 
    private toPortCountryName: string;
    public get ToPortCountryName() { return this.toPortCountryName; }
    public set ToPortCountryName(newValue: string) { if (this.toPortCountryName != newValue) { this.toPortCountryName = newValue; this.MarkAsDirty("ToPortCountryName"); } }
       
	 
    private orderGrossWeight: number;
    public get OrderGrossWeight() { return this.orderGrossWeight; }
    public set OrderGrossWeight(newValue: number) { if (this.orderGrossWeight != newValue) { this.orderGrossWeight = newValue; this.MarkAsDirty("OrderGrossWeight"); } }
       
	 
    private bookingVolume: number;
    public get BookingVolume() { return this.bookingVolume; }
    public set BookingVolume(newValue: number) { if (this.bookingVolume != newValue) { this.bookingVolume = newValue; this.MarkAsDirty("BookingVolume"); } }
       
	 
    private bookingNumberOfPackages: number;
    public get BookingNumberOfPackages() { return this.bookingNumberOfPackages; }
    public set BookingNumberOfPackages(newValue: number) { if (this.bookingNumberOfPackages != newValue) { this.bookingNumberOfPackages = newValue; this.MarkAsDirty("BookingNumberOfPackages"); } }
       
	 
    private orderIsDangerouseGoods: boolean;
    public get OrderIsDangerouseGoods() { return this.orderIsDangerouseGoods; }
    public set OrderIsDangerouseGoods(newValue: boolean) { if (this.orderIsDangerouseGoods != newValue) { this.orderIsDangerouseGoods = newValue; this.MarkAsDirty("OrderIsDangerouseGoods"); } }
       
	 
    private bookingConfirmationNumber: string;
    public get BookingConfirmationNumber() { return this.bookingConfirmationNumber; }
    public set BookingConfirmationNumber(newValue: string) { if (this.bookingConfirmationNumber != newValue) { this.bookingConfirmationNumber = newValue; this.MarkAsDirty("BookingConfirmationNumber"); } }
       
	 
    private bookingConfirmedBy: string;
    public get BookingConfirmedBy() { return this.bookingConfirmedBy; }
    public set BookingConfirmedBy(newValue: string) { if (this.bookingConfirmedBy != newValue) { this.bookingConfirmedBy = newValue; this.MarkAsDirty("BookingConfirmedBy"); } }
       
	 
    private bookingConfirmationNotes: string;
    public get BookingConfirmationNotes() { return this.bookingConfirmationNotes; }
    public set BookingConfirmationNotes(newValue: string) { if (this.bookingConfirmationNotes != newValue) { this.bookingConfirmationNotes = newValue; this.MarkAsDirty("BookingConfirmationNotes"); } }
       
	 
    private orderVolumetricWeight: number;
    public get OrderVolumetricWeight() { return this.orderVolumetricWeight; }
    public set OrderVolumetricWeight(newValue: number) { if (this.orderVolumetricWeight != newValue) { this.orderVolumetricWeight = newValue; this.MarkAsDirty("OrderVolumetricWeight"); } }
       
	 
    private orderChargeableWeight: number;
    public get OrderChargeableWeight() { return this.orderChargeableWeight; }
    public set OrderChargeableWeight(newValue: number) { if (this.orderChargeableWeight != newValue) { this.orderChargeableWeight = newValue; this.MarkAsDirty("OrderChargeableWeight"); } }
       
	 
    private cutoffDate: Date;
    public get CutoffDate() { return this.cutoffDate; }
    public set CutoffDate(newValue: Date) { if (this.cutoffDate != newValue) { this.cutoffDate = newValue; this.MarkAsDirty("CutoffDate"); } }
       
	 
    private aWBPrint: boolean;
    public get AWBPrint() { return this.aWBPrint; }
    public set AWBPrint(newValue: boolean) { if (this.aWBPrint != newValue) { this.aWBPrint = newValue; this.MarkAsDirty("AWBPrint"); } }
       
	 
    private fWBStatusCode: string;
    public get FWBStatusCode() { return this.fWBStatusCode; }
    public set FWBStatusCode(newValue: string) { if (this.fWBStatusCode != newValue) { this.fWBStatusCode = newValue; this.MarkAsDirty("FWBStatusCode"); } }
       
	 
    private fWBStatusName: string;
    public get FWBStatusName() { return this.fWBStatusName; }
    public set FWBStatusName(newValue: string) { if (this.fWBStatusName != newValue) { this.fWBStatusName = newValue; this.MarkAsDirty("FWBStatusName"); } }
       
	 
    private fHLStatusCode: string;
    public get FHLStatusCode() { return this.fHLStatusCode; }
    public set FHLStatusCode(newValue: string) { if (this.fHLStatusCode != newValue) { this.fHLStatusCode = newValue; this.MarkAsDirty("FHLStatusCode"); } }
       
	 
    private fHLStatusName: string;
    public get FHLStatusName() { return this.fHLStatusName; }
    public set FHLStatusName(newValue: string) { if (this.fHLStatusName != newValue) { this.fHLStatusName = newValue; this.MarkAsDirty("FHLStatusName"); } }
       
	 
    private aWBCurrencyId: string;
    public get AWBCurrencyId() { return this.aWBCurrencyId; }
    public set AWBCurrencyId(newValue: string) { if (this.aWBCurrencyId != newValue) { this.aWBCurrencyId = newValue; this.MarkAsDirty("AWBCurrencyId"); } }
       
	 
    private aWBCurrencyCode: string;
    public get AWBCurrencyCode() { return this.aWBCurrencyCode; }
    public set AWBCurrencyCode(newValue: string) { if (this.aWBCurrencyCode != newValue) { this.aWBCurrencyCode = newValue; this.MarkAsDirty("AWBCurrencyCode"); } }
       
	 
    private aWBFreightAmountPrepaid: number;
    public get AWBFreightAmountPrepaid() { return this.aWBFreightAmountPrepaid; }
    public set AWBFreightAmountPrepaid(newValue: number) { if (this.aWBFreightAmountPrepaid != newValue) { this.aWBFreightAmountPrepaid = newValue; this.MarkAsDirty("AWBFreightAmountPrepaid"); } }
       
	 
    private aWBFreightAmountCollect: number;
    public get AWBFreightAmountCollect() { return this.aWBFreightAmountCollect; }
    public set AWBFreightAmountCollect(newValue: number) { if (this.aWBFreightAmountCollect != newValue) { this.aWBFreightAmountCollect = newValue; this.MarkAsDirty("AWBFreightAmountCollect"); } }
       
	 
    private aWBCarrierTarrifReference: string;
    public get AWBCarrierTarrifReference() { return this.aWBCarrierTarrifReference; }
    public set AWBCarrierTarrifReference(newValue: string) { if (this.aWBCarrierTarrifReference != newValue) { this.aWBCarrierTarrifReference = newValue; this.MarkAsDirty("AWBCarrierTarrifReference"); } }
       
	 
    private aWBDeclaredValueForCarriage: string;
    public get AWBDeclaredValueForCarriage() { return this.aWBDeclaredValueForCarriage; }
    public set AWBDeclaredValueForCarriage(newValue: string) { if (this.aWBDeclaredValueForCarriage != newValue) { this.aWBDeclaredValueForCarriage = newValue; this.MarkAsDirty("AWBDeclaredValueForCarriage"); } }
       
	 
    private aWBDeclaredValueForCustoms: string;
    public get AWBDeclaredValueForCustoms() { return this.aWBDeclaredValueForCustoms; }
    public set AWBDeclaredValueForCustoms(newValue: string) { if (this.aWBDeclaredValueForCustoms != newValue) { this.aWBDeclaredValueForCustoms = newValue; this.MarkAsDirty("AWBDeclaredValueForCustoms"); } }
       
	 
    private aWBAccountingInformation: string;
    public get AWBAccountingInformation() { return this.aWBAccountingInformation; }
    public set AWBAccountingInformation(newValue: string) { if (this.aWBAccountingInformation != newValue) { this.aWBAccountingInformation = newValue; this.MarkAsDirty("AWBAccountingInformation"); } }
       
	 
    private aWBInsurrenceValue: string;
    public get AWBInsurrenceValue() { return this.aWBInsurrenceValue; }
    public set AWBInsurrenceValue(newValue: string) { if (this.aWBInsurrenceValue != newValue) { this.aWBInsurrenceValue = newValue; this.MarkAsDirty("AWBInsurrenceValue"); } }
       
	 
    private aWBHandlingInformation: string;
    public get AWBHandlingInformation() { return this.aWBHandlingInformation; }
    public set AWBHandlingInformation(newValue: string) { if (this.aWBHandlingInformation != newValue) { this.aWBHandlingInformation = newValue; this.MarkAsDirty("AWBHandlingInformation"); } }
       
	 
    private sCI: string;
    public get SCI() { return this.sCI; }
    public set SCI(newValue: string) { if (this.sCI != newValue) { this.sCI = newValue; this.MarkAsDirty("SCI"); } }
       
	 
    private aWBComments: string;
    public get AWBComments() { return this.aWBComments; }
    public set AWBComments(newValue: string) { if (this.aWBComments != newValue) { this.aWBComments = newValue; this.MarkAsDirty("AWBComments"); } }
       
    private aWBPrintingComments: string;
    public get AWBPrintingComments() { return this.aWBPrintingComments; }
    public set AWBPrintingComments(newValue: string) { if (this.aWBPrintingComments != newValue) { this.aWBPrintingComments = newValue; this.MarkAsDirty("AWBPrintingComments"); } }

    private aWBSignature: string;
    public get AWBSignature() { return this.aWBSignature; }
    public set AWBSignature(newValue: string) { if (this.aWBSignature != newValue) { this.aWBSignature = newValue; this.MarkAsDirty("AWBSignature"); } }
       
	 
    private aWBPlace: string;
    public get AWBPlace() { return this.aWBPlace; }
    public set AWBPlace(newValue: string) { if (this.aWBPlace != newValue) { this.aWBPlace = newValue; this.MarkAsDirty("AWBPlace"); } }
       
	 
    private aWBChargesCodeCode: string;
    public get AWBChargesCodeCode() { return this.aWBChargesCodeCode; }
    public set AWBChargesCodeCode(newValue: string) { if (this.aWBChargesCodeCode != newValue) { this.aWBChargesCodeCode = newValue; this.MarkAsDirty("AWBChargesCodeCode"); } }
       
	 
    private tenantZeroAirlineId: string;
    public get TenantZeroAirlineId() { return this.tenantZeroAirlineId; }
    public set TenantZeroAirlineId(newValue: string) { if (this.tenantZeroAirlineId != newValue) { this.tenantZeroAirlineId = newValue; this.MarkAsDirty("TenantZeroAirlineId"); } }
       
	 
    private tenantZeroAirlineTTY: string;
    public get TenantZeroAirlineTTY() { return this.tenantZeroAirlineTTY; }
    public set TenantZeroAirlineTTY(newValue: string) { if (this.tenantZeroAirlineTTY != newValue) { this.tenantZeroAirlineTTY = newValue; this.MarkAsDirty("TenantZeroAirlineTTY"); } }
       
	 
    private tenantZeroAirlinePIMA: string;
    public get TenantZeroAirlinePIMA() { return this.tenantZeroAirlinePIMA; }
    public set TenantZeroAirlinePIMA(newValue: string) { if (this.tenantZeroAirlinePIMA != newValue) { this.tenantZeroAirlinePIMA = newValue; this.MarkAsDirty("TenantZeroAirlinePIMA"); } }
       
	 
    private tenantZeroAirlineChampFWB: boolean;
    public get TenantZeroAirlineChampFWB() { return this.tenantZeroAirlineChampFWB; }
    public set TenantZeroAirlineChampFWB(newValue: boolean) { if (this.tenantZeroAirlineChampFWB != newValue) { this.tenantZeroAirlineChampFWB = newValue; this.MarkAsDirty("TenantZeroAirlineChampFWB"); } }
       
	 
    private tenantZeroAirlineChampFHL: boolean;
    public get TenantZeroAirlineChampFHL() { return this.tenantZeroAirlineChampFHL; }
    public set TenantZeroAirlineChampFHL(newValue: boolean) { if (this.tenantZeroAirlineChampFHL != newValue) { this.tenantZeroAirlineChampFHL = newValue; this.MarkAsDirty("TenantZeroAirlineChampFHL"); } }
       
	 
    private tenantZeroAirlineChampFSU: boolean;
    public get TenantZeroAirlineChampFSU() { return this.tenantZeroAirlineChampFSU; }
    public set TenantZeroAirlineChampFSU(newValue: boolean) { if (this.tenantZeroAirlineChampFSU != newValue) { this.tenantZeroAirlineChampFSU = newValue; this.MarkAsDirty("TenantZeroAirlineChampFSU"); } }
       
	 
    private tenantZeroAirlineChampFSRFSA: boolean;
    public get TenantZeroAirlineChampFSRFSA() { return this.tenantZeroAirlineChampFSRFSA; }
    public set TenantZeroAirlineChampFSRFSA(newValue: boolean) { if (this.tenantZeroAirlineChampFSRFSA != newValue) { this.tenantZeroAirlineChampFSRFSA = newValue; this.MarkAsDirty("TenantZeroAirlineChampFSRFSA"); } }
       
	 
    private tenantZeroAirlineChampFVRFVA: boolean;
    public get TenantZeroAirlineChampFVRFVA() { return this.tenantZeroAirlineChampFVRFVA; }
    public set TenantZeroAirlineChampFVRFVA(newValue: boolean) { if (this.tenantZeroAirlineChampFVRFVA != newValue) { this.tenantZeroAirlineChampFVRFVA = newValue; this.MarkAsDirty("TenantZeroAirlineChampFVRFVA"); } }
       
	 
    private carrierIsChampRegistered: boolean;
    public get CarrierIsChampRegistered() { return this.carrierIsChampRegistered; }
    public set CarrierIsChampRegistered(newValue: boolean) { if (this.carrierIsChampRegistered != newValue) { this.carrierIsChampRegistered = newValue; this.MarkAsDirty("CarrierIsChampRegistered"); } }
       
	 
    private tenantZeroAirlineChampNeedsRegistration: boolean;
    public get TenantZeroAirlineChampNeedsRegistration() { return this.tenantZeroAirlineChampNeedsRegistration; }
    public set TenantZeroAirlineChampNeedsRegistration(newValue: boolean) { if (this.tenantZeroAirlineChampNeedsRegistration != newValue) { this.tenantZeroAirlineChampNeedsRegistration = newValue; this.MarkAsDirty("TenantZeroAirlineChampNeedsRegistration"); } }
       
	 
    private tenantZeroAirlineGLSHKFWB: boolean;
    public get TenantZeroAirlineGLSHKFWB() { return this.tenantZeroAirlineGLSHKFWB; }
    public set TenantZeroAirlineGLSHKFWB(newValue: boolean) { if (this.tenantZeroAirlineGLSHKFWB != newValue) { this.tenantZeroAirlineGLSHKFWB = newValue; this.MarkAsDirty("TenantZeroAirlineGLSHKFWB"); } }
       
	 
    private tenantZeroAirlineGLSHKFHL: boolean;
    public get TenantZeroAirlineGLSHKFHL() { return this.tenantZeroAirlineGLSHKFHL; }
    public set TenantZeroAirlineGLSHKFHL(newValue: boolean) { if (this.tenantZeroAirlineGLSHKFHL != newValue) { this.tenantZeroAirlineGLSHKFHL = newValue; this.MarkAsDirty("TenantZeroAirlineGLSHKFHL"); } }
       
	 
    private tenantZeroAirlineGLSHKFSU: boolean;
    public get TenantZeroAirlineGLSHKFSU() { return this.tenantZeroAirlineGLSHKFSU; }
    public set TenantZeroAirlineGLSHKFSU(newValue: boolean) { if (this.tenantZeroAirlineGLSHKFSU != newValue) { this.tenantZeroAirlineGLSHKFSU = newValue; this.MarkAsDirty("TenantZeroAirlineGLSHKFSU"); } }
       
	 
    private tenantZeroAirlineGLSHKFSRFSA: boolean;
    public get TenantZeroAirlineGLSHKFSRFSA() { return this.tenantZeroAirlineGLSHKFSRFSA; }
    public set TenantZeroAirlineGLSHKFSRFSA(newValue: boolean) { if (this.tenantZeroAirlineGLSHKFSRFSA != newValue) { this.tenantZeroAirlineGLSHKFSRFSA = newValue; this.MarkAsDirty("TenantZeroAirlineGLSHKFSRFSA"); } }
       
	 
    private tenantZeroAirlineGLSHKFVRFVA: boolean;
    public get TenantZeroAirlineGLSHKFVRFVA() { return this.tenantZeroAirlineGLSHKFVRFVA; }
    public set TenantZeroAirlineGLSHKFVRFVA(newValue: boolean) { if (this.tenantZeroAirlineGLSHKFVRFVA != newValue) { this.tenantZeroAirlineGLSHKFVRFVA = newValue; this.MarkAsDirty("TenantZeroAirlineGLSHKFVRFVA"); } }
       
	 
    private carrierIsGLSHKRegistered: boolean;
    public get CarrierIsGLSHKRegistered() { return this.carrierIsGLSHKRegistered; }
    public set CarrierIsGLSHKRegistered(newValue: boolean) { if (this.carrierIsGLSHKRegistered != newValue) { this.carrierIsGLSHKRegistered = newValue; this.MarkAsDirty("CarrierIsGLSHKRegistered"); } }
       
	 
    private tenantZeroAirlineGLSHKNeedsRegistration: boolean;
    public get TenantZeroAirlineGLSHKNeedsRegistration() { return this.tenantZeroAirlineGLSHKNeedsRegistration; }
    public set TenantZeroAirlineGLSHKNeedsRegistration(newValue: boolean) { if (this.tenantZeroAirlineGLSHKNeedsRegistration != newValue) { this.tenantZeroAirlineGLSHKNeedsRegistration = newValue; this.MarkAsDirty("TenantZeroAirlineGLSHKNeedsRegistration"); } }
       
	 
    private carrierIsCheckDigit: boolean;
    public get CarrierIsCheckDigit() { return this.carrierIsCheckDigit; }
    public set CarrierIsCheckDigit(newValue: boolean) { if (this.carrierIsCheckDigit != newValue) { this.carrierIsCheckDigit = newValue; this.MarkAsDirty("CarrierIsCheckDigit"); } }
       
	 
    private carrierIsLimitedLength: boolean;
    public get CarrierIsLimitedLength() { return this.carrierIsLimitedLength; }
    public set CarrierIsLimitedLength(newValue: boolean) { if (this.carrierIsLimitedLength != newValue) { this.carrierIsLimitedLength = newValue; this.MarkAsDirty("CarrierIsLimitedLength"); } }
       
	 
    private localCustomsTransmissionsStatusCode: string;
    public get LocalCustomsTransmissionsStatusCode() { return this.localCustomsTransmissionsStatusCode; }
    public set LocalCustomsTransmissionsStatusCode(newValue: string) { if (this.localCustomsTransmissionsStatusCode != newValue) { this.localCustomsTransmissionsStatusCode = newValue; this.MarkAsDirty("LocalCustomsTransmissionsStatusCode"); } }
       
	 
    private localCustomsTransmissionsStatusName: string;
    public get LocalCustomsTransmissionsStatusName() { return this.localCustomsTransmissionsStatusName; }
    public set LocalCustomsTransmissionsStatusName(newValue: string) { if (this.localCustomsTransmissionsStatusName != newValue) { this.localCustomsTransmissionsStatusName = newValue; this.MarkAsDirty("LocalCustomsTransmissionsStatusName"); } }
       
	 
    private localCustomsTransmissionsStatusError: string;
    public get LocalCustomsTransmissionsStatusError() { return this.localCustomsTransmissionsStatusError; }
    public set LocalCustomsTransmissionsStatusError(newValue: string) { if (this.localCustomsTransmissionsStatusError != newValue) { this.localCustomsTransmissionsStatusError = newValue; this.MarkAsDirty("LocalCustomsTransmissionsStatusError"); } }
       
	 
    private localCustomsTransmissionsStatusDate: Date;
    public get LocalCustomsTransmissionsStatusDate() { return this.localCustomsTransmissionsStatusDate; }
    public set LocalCustomsTransmissionsStatusDate(newValue: Date) { if (this.localCustomsTransmissionsStatusDate != newValue) { this.localCustomsTransmissionsStatusDate = newValue; this.MarkAsDirty("LocalCustomsTransmissionsStatusDate"); } }

    private localCustomsSentByUserId: string;
    public get LocalCustomsSentByUserId() { return this.localCustomsSentByUserId; }
    public set LocalCustomsSentByUserId(newValue: string) { if (this.localCustomsSentByUserId != newValue) { this.localCustomsSentByUserId = newValue; this.MarkAsDirty("LocalCustomsSentByUserId"); } }

    private localCustomsSentByUserName: string;
    public get LocalCustomsSentByUserName() { return this.localCustomsSentByUserName; }
    public set LocalCustomsSentByUserName(newValue: string) { if (this.localCustomsSentByUserName != newValue) { this.localCustomsSentByUserName = newValue; this.MarkAsDirty("LocalCustomsSentByUserName"); } }


    private includesCustoms: boolean;
    public get IncludesCustoms() { return this.includesCustoms; }
    public set IncludesCustoms(newValue: boolean) { if (this.includesCustoms != newValue) { this.includesCustoms = newValue; this.MarkAsDirty("IncludesCustoms"); } }
       
	 
    private isUpdateByAutomation: boolean;
    public get IsUpdateByAutomation() { return this.isUpdateByAutomation; }
    public set IsUpdateByAutomation(newValue: boolean) { if (this.isUpdateByAutomation != newValue) { this.isUpdateByAutomation = newValue; this.MarkAsDirty("IsUpdateByAutomation"); } }
       
	 
    private declarationNumber: string;
    public get DeclarationNumber() { return this.declarationNumber; }
    public set DeclarationNumber(newValue: string) { if (this.declarationNumber != newValue) { this.declarationNumber = newValue; this.MarkAsDirty("DeclarationNumber"); } }
       
	 
    private declarationDate: Date;
    public get DeclarationDate() { return this.declarationDate; }
    public set DeclarationDate(newValue: Date) { if (this.declarationDate != newValue) { this.declarationDate = newValue; this.MarkAsDirty("DeclarationDate"); } }
       
	 
    private customsClearanceDate: Date;
    public get CustomsClearanceDate() { return this.customsClearanceDate; }
    public set CustomsClearanceDate(newValue: Date) { if (this.customsClearanceDate != newValue) { this.customsClearanceDate = newValue; this.MarkAsDirty("CustomsClearanceDate"); } }
       
	 
    private productCode: string;
    public get ProductCode() { return this.productCode; }
    public set ProductCode(newValue: string) { if (this.productCode != newValue) { this.productCode = newValue; this.MarkAsDirty("ProductCode"); } }
       
	 
    private securityKey: string;
    public get SecurityKey() { return this.securityKey; }
    public set SecurityKey(newValue: string) { if (this.securityKey != newValue) { this.securityKey = newValue; this.MarkAsDirty("SecurityKey"); } }
       
	 
    private tEU: number;
    public get TEU() { return this.tEU; }
    public set TEU(newValue: number) { if (this.tEU != newValue) { this.tEU = newValue; this.MarkAsDirty("TEU"); } }
       
	 
    private convertFromHouseToDirect: boolean;
    public get ConvertFromHouseToDirect() { return this.convertFromHouseToDirect; }
    public set ConvertFromHouseToDirect(newValue: boolean) { if (this.convertFromHouseToDirect != newValue) { this.convertFromHouseToDirect = newValue; this.MarkAsDirty("ConvertFromHouseToDirect"); } }
       
	 
    private convertFromDirectToHouse: boolean;
    public get ConvertFromDirectToHouse() { return this.convertFromDirectToHouse; }
    public set ConvertFromDirectToHouse(newValue: boolean) { if (this.convertFromDirectToHouse != newValue) { this.convertFromDirectToHouse = newValue; this.MarkAsDirty("ConvertFromDirectToHouse"); } }
       
	 
    private isRefreshShipmentFollowUps: boolean;
    public get IsRefreshShipmentFollowUps() { return this.isRefreshShipmentFollowUps; }
    public set IsRefreshShipmentFollowUps(newValue: boolean) { if (this.isRefreshShipmentFollowUps != newValue) { this.isRefreshShipmentFollowUps = newValue; this.MarkAsDirty("IsRefreshShipmentFollowUps"); } }
       
	 
    private customerRankName: string;
    public get CustomerRankName() { return this.customerRankName; }
    public set CustomerRankName(newValue: string) { if (this.customerRankName != newValue) { this.customerRankName = newValue; this.MarkAsDirty("CustomerRankName"); } }
       
	 
    private finalArrivalDate: Date;
    public get FinalArrivalDate() { return this.finalArrivalDate; }
    public set FinalArrivalDate(newValue: Date) { if (this.finalArrivalDate != newValue) { this.finalArrivalDate = newValue; this.MarkAsDirty("FinalArrivalDate"); } }
       
    private estimatedFinalArrivalDate: Date;
    public get EstimatedFinalArrivalDate() { return this.estimatedFinalArrivalDate; }
    public set EstimatedFinalArrivalDate(newValue: Date) { if (this.estimatedFinalArrivalDate != newValue) { this.estimatedFinalArrivalDate = newValue; this.MarkAsDirty("EstimatedFinalArrivalDate"); } }

    private actualFinalArrivalDate: Date;
    public get ActualFinalArrivalDate() { return this.actualFinalArrivalDate; }
    public set ActualFinalArrivalDate(newValue: Date) { if (this.actualFinalArrivalDate != newValue) { this.actualFinalArrivalDate = newValue; this.MarkAsDirty("ActualFinalArrivalDate"); } }

    private foreignPartnerCountryCode: string;
    public get ForeignPartnerCountryCode() { return this.foreignPartnerCountryCode; }
    public set ForeignPartnerCountryCode(newValue: string) { if (this.foreignPartnerCountryCode != newValue) { this.foreignPartnerCountryCode = newValue; this.MarkAsDirty("ForeignPartnerCountryCode"); } }
       
	 
    private lastStatusLogDate: Date;
    public get LastStatusLogDate() { return this.lastStatusLogDate; }
    public set LastStatusLogDate(newValue: Date) { if (this.lastStatusLogDate != newValue) { this.lastStatusLogDate = newValue; this.MarkAsDirty("LastStatusLogDate"); } }
       
	 
    private mainCarriageFullCarrierNumber: string;
    public get MainCarriageFullCarrierNumber() { return this.mainCarriageFullCarrierNumber; }
    public set MainCarriageFullCarrierNumber(newValue: string) { if (this.mainCarriageFullCarrierNumber != newValue) { this.mainCarriageFullCarrierNumber = newValue; this.MarkAsDirty("MainCarriageFullCarrierNumber"); } }
       
	 
    private transshipment1FullCarrierNumber: string;
    public get Transshipment1FullCarrierNumber() { return this.transshipment1FullCarrierNumber; }
    public set Transshipment1FullCarrierNumber(newValue: string) { if (this.transshipment1FullCarrierNumber != newValue) { this.transshipment1FullCarrierNumber = newValue; this.MarkAsDirty("Transshipment1FullCarrierNumber"); } }
       
	 
    private transshipment2FullCarrierNumber: string;
    public get Transshipment2FullCarrierNumber() { return this.transshipment2FullCarrierNumber; }
    public set Transshipment2FullCarrierNumber(newValue: string) { if (this.transshipment2FullCarrierNumber != newValue) { this.transshipment2FullCarrierNumber = newValue; this.MarkAsDirty("Transshipment2FullCarrierNumber"); } }
       
	 
    private transshipment3FullCarrierNumber: string;
    public get Transshipment3FullCarrierNumber() { return this.transshipment3FullCarrierNumber; }
    public set Transshipment3FullCarrierNumber(newValue: string) { if (this.transshipment3FullCarrierNumber != newValue) { this.transshipment3FullCarrierNumber = newValue; this.MarkAsDirty("Transshipment3FullCarrierNumber"); } }
       
	 
    private exceptionDescription: string;
    public get ExceptionDescription() { return this.exceptionDescription; }
    public set ExceptionDescription(newValue: string) { if (this.exceptionDescription != newValue) { this.exceptionDescription = newValue; this.MarkAsDirty("ExceptionDescription"); } }
       
	 
    private exceptionResolvedDescription: string;
    public get ExceptionResolvedDescription() { return this.exceptionResolvedDescription; }
    public set ExceptionResolvedDescription(newValue: string) { if (this.exceptionResolvedDescription != newValue) { this.exceptionResolvedDescription = newValue; this.MarkAsDirty("ExceptionResolvedDescription"); } }
       
	 
    private lastExceptionDescription: string;
    public get LastExceptionDescription() { return this.lastExceptionDescription; }
    public set LastExceptionDescription(newValue: string) { if (this.lastExceptionDescription != newValue) { this.lastExceptionDescription = newValue; this.MarkAsDirty("LastExceptionDescription"); } }
       
	 
    private exceptionDate: Date;
    public get ExceptionDate() { return this.exceptionDate; }
    public set ExceptionDate(newValue: Date) { if (this.exceptionDate != newValue) { this.exceptionDate = newValue; this.MarkAsDirty("ExceptionDate"); } }
       
	 
    private hasException: boolean;
    public get HasException() { return this.hasException; }
    public set HasException(newValue: boolean) { if (this.hasException != newValue) { this.hasException = newValue; this.MarkAsDirty("HasException"); } }
       
	 
    private hasExceptionMessage: string;
    public get HasExceptionMessage() { return this.hasExceptionMessage; }
    public set HasExceptionMessage(newValue: string) { if (this.hasExceptionMessage != newValue) { this.hasExceptionMessage = newValue; this.MarkAsDirty("HasExceptionMessage"); } }
       

       
	 
    private fromLocation: string;
    public get FromLocation() { return this.fromLocation; }
    public set FromLocation(newValue: string) { if (this.fromLocation != newValue) { this.fromLocation = newValue; this.MarkAsDirty("FromLocation"); } }
       
	 
    private toLocation: string;
    public get ToLocation() { return this.toLocation; }
    public set ToLocation(newValue: string) { if (this.toLocation != newValue) { this.toLocation = newValue; this.MarkAsDirty("ToLocation"); } }
       
	 
    private moveTypeId: string;
    public get MoveTypeId() { return this.moveTypeId; }
    public set MoveTypeId(newValue: string) { if (this.moveTypeId != newValue) { this.moveTypeId = newValue; this.MarkAsDirty("MoveTypeId"); } }
       
	 
    private moveTypeCode: string;
    public get MoveTypeCode() { return this.moveTypeCode; }
    public set MoveTypeCode(newValue: string) { if (this.moveTypeCode != newValue) { this.moveTypeCode = newValue; this.MarkAsDirty("MoveTypeCode"); } }
       
	 
    private moveTypeName: string;
    public get MoveTypeName() { return this.moveTypeName; }
    public set MoveTypeName(newValue: string) { if (this.moveTypeName != newValue) { this.moveTypeName = newValue; this.MarkAsDirty("MoveTypeName"); } }
       
	 
    private isCreatedFromAgentSharedManifest: boolean;
    public get IsCreatedFromAgentSharedManifest() { return this.isCreatedFromAgentSharedManifest; }
    public set IsCreatedFromAgentSharedManifest(newValue: boolean) { if (this.isCreatedFromAgentSharedManifest != newValue) { this.isCreatedFromAgentSharedManifest = newValue; this.MarkAsDirty("IsCreatedFromAgentSharedManifest"); } }
       
	 
    private aMSBL: string;
    public get AMSBL() { return this.aMSBL; }
    public set AMSBL(newValue: string) { if (this.aMSBL != newValue) { this.aMSBL = newValue; this.MarkAsDirty("AMSBL"); } }
       
	 
    private customFileId: string;
    public get CustomFileId() { return this.customFileId; }
    public set CustomFileId(newValue: string) { if (this.customFileId != newValue) { this.customFileId = newValue; this.MarkAsDirty("CustomFileId"); } }
       
	 
    private customFileNumber: string;
    public get CustomFileNumber() { return this.customFileNumber; }
    public set CustomFileNumber(newValue: string) { if (this.customFileNumber != newValue) { this.customFileNumber = newValue; this.MarkAsDirty("CustomFileNumber"); } }
       
	 
    private mainCarriageSTD: Date;
    public get MainCarriageSTD() { return this.mainCarriageSTD; }
    public set MainCarriageSTD(newValue: Date) { if (this.mainCarriageSTD != newValue) { this.mainCarriageSTD = newValue; this.MarkAsDirty("MainCarriageSTD"); } }
       
	 
    private mainCarriageSTA: Date;
    public get MainCarriageSTA() { return this.mainCarriageSTA; }
    public set MainCarriageSTA(newValue: Date) { if (this.mainCarriageSTA != newValue) { this.mainCarriageSTA = newValue; this.MarkAsDirty("MainCarriageSTA"); } }
       
	 
    private transshipment1STD: Date;
    public get Transshipment1STD() { return this.transshipment1STD; }
    public set Transshipment1STD(newValue: Date) { if (this.transshipment1STD != newValue) { this.transshipment1STD = newValue; this.MarkAsDirty("Transshipment1STD"); } }
       
	 
    private transshipment1STA: Date;
    public get Transshipment1STA() { return this.transshipment1STA; }
    public set Transshipment1STA(newValue: Date) { if (this.transshipment1STA != newValue) { this.transshipment1STA = newValue; this.MarkAsDirty("Transshipment1STA"); } }
       
	 
    private transshipment2STD: Date;
    public get Transshipment2STD() { return this.transshipment2STD; }
    public set Transshipment2STD(newValue: Date) { if (this.transshipment2STD != newValue) { this.transshipment2STD = newValue; this.MarkAsDirty("Transshipment2STD"); } }
       
	 
    private transshipment2STA: Date;
    public get Transshipment2STA() { return this.transshipment2STA; }
    public set Transshipment2STA(newValue: Date) { if (this.transshipment2STA != newValue) { this.transshipment2STA = newValue; this.MarkAsDirty("Transshipment2STA"); } }
       
	 
    private transshipment3STD: Date;
    public get Transshipment3STD() { return this.transshipment3STD; }
    public set Transshipment3STD(newValue: Date) { if (this.transshipment3STD != newValue) { this.transshipment3STD = newValue; this.MarkAsDirty("Transshipment3STD"); } }
       
	 
    private transshipment3STA: Date;
    public get Transshipment3STA() { return this.transshipment3STA; }
    public set Transshipment3STA(newValue: Date) { if (this.transshipment3STA != newValue) { this.transshipment3STA = newValue; this.MarkAsDirty("Transshipment3STA"); } }
       
	 
    private isMultipleCommodities: boolean;
    public get IsMultipleCommodities() { return this.isMultipleCommodities; }
    public set IsMultipleCommodities(newValue: boolean) { if (this.isMultipleCommodities != newValue) { this.isMultipleCommodities = newValue; this.MarkAsDirty("IsMultipleCommodities"); } }
       
	 
    private nominatedHandlingPartyId: string;
    public get NominatedHandlingPartyId() { return this.nominatedHandlingPartyId; }
    public set NominatedHandlingPartyId(newValue: string) { if (this.nominatedHandlingPartyId != newValue) { this.nominatedHandlingPartyId = newValue; this.MarkAsDirty("NominatedHandlingPartyId"); } }
       
	 
    private otherParticipantIdCode1: string;
    public get OtherParticipantIdCode1() { return this.otherParticipantIdCode1; }
    public set OtherParticipantIdCode1(newValue: string) { if (this.otherParticipantIdCode1 != newValue) { this.otherParticipantIdCode1 = newValue; this.MarkAsDirty("OtherParticipantIdCode1"); } }
       
	 
    private otherParticipantIdCode2: string;
    public get OtherParticipantIdCode2() { return this.otherParticipantIdCode2; }
    public set OtherParticipantIdCode2(newValue: string) { if (this.otherParticipantIdCode2 != newValue) { this.otherParticipantIdCode2 = newValue; this.MarkAsDirty("OtherParticipantIdCode2"); } }
       
	 
    private otherParticipantIdCode3: string;
    public get OtherParticipantIdCode3() { return this.otherParticipantIdCode3; }
    public set OtherParticipantIdCode3(newValue: string) { if (this.otherParticipantIdCode3 != newValue) { this.otherParticipantIdCode3 = newValue; this.MarkAsDirty("OtherParticipantIdCode3"); } }
       
	 
    private otherParticipantInformationCode1: string;
    public get OtherParticipantInformationCode1() { return this.otherParticipantInformationCode1; }
    public set OtherParticipantInformationCode1(newValue: string) { if (this.otherParticipantInformationCode1 != newValue) { this.otherParticipantInformationCode1 = newValue; this.MarkAsDirty("OtherParticipantInformationCode1"); } }
       
	 
    private otherParticipantInformationCode2: string;
    public get OtherParticipantInformationCode2() { return this.otherParticipantInformationCode2; }
    public set OtherParticipantInformationCode2(newValue: string) { if (this.otherParticipantInformationCode2 != newValue) { this.otherParticipantInformationCode2 = newValue; this.MarkAsDirty("OtherParticipantInformationCode2"); } }
       
	 
    private otherParticipantInformationCode3: string;
    public get OtherParticipantInformationCode3() { return this.otherParticipantInformationCode3; }
    public set OtherParticipantInformationCode3(newValue: string) { if (this.otherParticipantInformationCode3 != newValue) { this.otherParticipantInformationCode3 = newValue; this.MarkAsDirty("OtherParticipantInformationCode3"); } }
       
	 
    private otherParticipantInformationPortCode1: string;
    public get OtherParticipantInformationPortCode1() { return this.otherParticipantInformationPortCode1; }
    public set OtherParticipantInformationPortCode1(newValue: string) { if (this.otherParticipantInformationPortCode1 != newValue) { this.otherParticipantInformationPortCode1 = newValue; this.MarkAsDirty("OtherParticipantInformationPortCode1"); } }
       
	 
    private otherParticipantInformationPortCode2: string;
    public get OtherParticipantInformationPortCode2() { return this.otherParticipantInformationPortCode2; }
    public set OtherParticipantInformationPortCode2(newValue: string) { if (this.otherParticipantInformationPortCode2 != newValue) { this.otherParticipantInformationPortCode2 = newValue; this.MarkAsDirty("OtherParticipantInformationPortCode2"); } }
       
	 
    private otherParticipantInformationPortCode3: string;
    public get OtherParticipantInformationPortCode3() { return this.otherParticipantInformationPortCode3; }
    public set OtherParticipantInformationPortCode3(newValue: string) { if (this.otherParticipantInformationPortCode3 != newValue) { this.otherParticipantInformationPortCode3 = newValue; this.MarkAsDirty("OtherParticipantInformationPortCode3"); } }
       
	 
    private otherParticipantInformationName1: string;
    public get OtherParticipantInformationName1() { return this.otherParticipantInformationName1; }
    public set OtherParticipantInformationName1(newValue: string) { if (this.otherParticipantInformationName1 != newValue) { this.otherParticipantInformationName1 = newValue; this.MarkAsDirty("OtherParticipantInformationName1"); } }
       
	 
    private otherParticipantInformationName2: string;
    public get OtherParticipantInformationName2() { return this.otherParticipantInformationName2; }
    public set OtherParticipantInformationName2(newValue: string) { if (this.otherParticipantInformationName2 != newValue) { this.otherParticipantInformationName2 = newValue; this.MarkAsDirty("OtherParticipantInformationName2"); } }
       
	 
    private otherParticipantInformationName3: string;
    public get OtherParticipantInformationName3() { return this.otherParticipantInformationName3; }
    public set OtherParticipantInformationName3(newValue: string) { if (this.otherParticipantInformationName3 != newValue) { this.otherParticipantInformationName3 = newValue; this.MarkAsDirty("OtherParticipantInformationName3"); } }
       
	 
    private otherParticipantInformationReference1: string;
    public get OtherParticipantInformationReference1() { return this.otherParticipantInformationReference1; }
    public set OtherParticipantInformationReference1(newValue: string) { if (this.otherParticipantInformationReference1 != newValue) { this.otherParticipantInformationReference1 = newValue; this.MarkAsDirty("OtherParticipantInformationReference1"); } }
       
	 
    private otherParticipantInformationReference2: string;
    public get OtherParticipantInformationReference2() { return this.otherParticipantInformationReference2; }
    public set OtherParticipantInformationReference2(newValue: string) { if (this.otherParticipantInformationReference2 != newValue) { this.otherParticipantInformationReference2 = newValue; this.MarkAsDirty("OtherParticipantInformationReference2"); } }
       
	 
    private otherParticipantInformationReference3: string;
    public get OtherParticipantInformationReference3() { return this.otherParticipantInformationReference3; }
    public set OtherParticipantInformationReference3(newValue: string) { if (this.otherParticipantInformationReference3 != newValue) { this.otherParticipantInformationReference3 = newValue; this.MarkAsDirty("OtherParticipantInformationReference3"); } }
       
	 
    private accountingInformation1: string;
    public get AccountingInformation1() { return this.accountingInformation1; }
    public set AccountingInformation1(newValue: string) { if (this.accountingInformation1 != newValue) { this.accountingInformation1 = newValue; this.MarkAsDirty("AccountingInformation1"); } }
       
	 
    private accountingInformation2: string;
    public get AccountingInformation2() { return this.accountingInformation2; }
    public set AccountingInformation2(newValue: string) { if (this.accountingInformation2 != newValue) { this.accountingInformation2 = newValue; this.MarkAsDirty("AccountingInformation2"); } }
       
	 
    private accountingInformation3: string;
    public get AccountingInformation3() { return this.accountingInformation3; }
    public set AccountingInformation3(newValue: string) { if (this.accountingInformation3 != newValue) { this.accountingInformation3 = newValue; this.MarkAsDirty("AccountingInformation3"); } }
       
	 
    private accountingInformation4: string;
    public get AccountingInformation4() { return this.accountingInformation4; }
    public set AccountingInformation4(newValue: string) { if (this.accountingInformation4 != newValue) { this.accountingInformation4 = newValue; this.MarkAsDirty("AccountingInformation4"); } }
       
	 
    private accountingInformation5: string;
    public get AccountingInformation5() { return this.accountingInformation5; }
    public set AccountingInformation5(newValue: string) { if (this.accountingInformation5 != newValue) { this.accountingInformation5 = newValue; this.MarkAsDirty("AccountingInformation5"); } }
       
	 
    private accountingInformation6: string;
    public get AccountingInformation6() { return this.accountingInformation6; }
    public set AccountingInformation6(newValue: string) { if (this.accountingInformation6 != newValue) { this.accountingInformation6 = newValue; this.MarkAsDirty("AccountingInformation6"); } }
       
	 
    private accountingInformationIdentifierCode1: string;
    public get AccountingInformationIdentifierCode1() { return this.accountingInformationIdentifierCode1; }
    public set AccountingInformationIdentifierCode1(newValue: string) { if (this.accountingInformationIdentifierCode1 != newValue) { this.accountingInformationIdentifierCode1 = newValue; this.MarkAsDirty("AccountingInformationIdentifierCode1"); } }
       
	 
    private accountingInformationIdentifierCode2: string;
    public get AccountingInformationIdentifierCode2() { return this.accountingInformationIdentifierCode2; }
    public set AccountingInformationIdentifierCode2(newValue: string) { if (this.accountingInformationIdentifierCode2 != newValue) { this.accountingInformationIdentifierCode2 = newValue; this.MarkAsDirty("AccountingInformationIdentifierCode2"); } }
       
	 
    private accountingInformationIdentifierCode3: string;
    public get AccountingInformationIdentifierCode3() { return this.accountingInformationIdentifierCode3; }
    public set AccountingInformationIdentifierCode3(newValue: string) { if (this.accountingInformationIdentifierCode3 != newValue) { this.accountingInformationIdentifierCode3 = newValue; this.MarkAsDirty("AccountingInformationIdentifierCode3"); } }
       
	 
    private accountingInformationIdentifierCode4: string;
    public get AccountingInformationIdentifierCode4() { return this.accountingInformationIdentifierCode4; }
    public set AccountingInformationIdentifierCode4(newValue: string) { if (this.accountingInformationIdentifierCode4 != newValue) { this.accountingInformationIdentifierCode4 = newValue; this.MarkAsDirty("AccountingInformationIdentifierCode4"); } }
       
	 
    private accountingInformationIdentifierCode5: string;
    public get AccountingInformationIdentifierCode5() { return this.accountingInformationIdentifierCode5; }
    public set AccountingInformationIdentifierCode5(newValue: string) { if (this.accountingInformationIdentifierCode5 != newValue) { this.accountingInformationIdentifierCode5 = newValue; this.MarkAsDirty("AccountingInformationIdentifierCode5"); } }
       
	 
    private accountingInformationIdentifierCode6: string;
    public get AccountingInformationIdentifierCode6() { return this.accountingInformationIdentifierCode6; }
    public set AccountingInformationIdentifierCode6(newValue: string) { if (this.accountingInformationIdentifierCode6 != newValue) { this.accountingInformationIdentifierCode6 = newValue; this.MarkAsDirty("AccountingInformationIdentifierCode6"); } }
       
	 
    private referenceNumber: string;
    public get ReferenceNumber() { return this.referenceNumber; }
    public set ReferenceNumber(newValue: string) { if (this.referenceNumber != newValue) { this.referenceNumber = newValue; this.MarkAsDirty("ReferenceNumber"); } }
       
	 
    private supplementaryShipmentInformation1: string;
    public get SupplementaryShipmentInformation1() { return this.supplementaryShipmentInformation1; }
    public set SupplementaryShipmentInformation1(newValue: string) { if (this.supplementaryShipmentInformation1 != newValue) { this.supplementaryShipmentInformation1 = newValue; this.MarkAsDirty("SupplementaryShipmentInformation1"); } }
       
	 
    private supplementaryShipmentInformation2: string;
    public get SupplementaryShipmentInformation2() { return this.supplementaryShipmentInformation2; }
    public set SupplementaryShipmentInformation2(newValue: string) { if (this.supplementaryShipmentInformation2 != newValue) { this.supplementaryShipmentInformation2 = newValue; this.MarkAsDirty("SupplementaryShipmentInformation2"); } }
       
	 
    private lastSentByUserId: string;
    public get LastSentByUserId() { return this.lastSentByUserId; }
    public set LastSentByUserId(newValue: string) { if (this.lastSentByUserId != newValue) { this.lastSentByUserId = newValue; this.MarkAsDirty("LastSentByUserId"); } }
       
	 
    private valueOfGoods: number;
    public get ValueOfGoods() { return this.valueOfGoods; }
    public set ValueOfGoods(newValue: number) { if (this.valueOfGoods != newValue) { this.valueOfGoods = newValue; this.MarkAsDirty("ValueOfGoods"); } }
       
	 
    private valueOfGoodsCurrencyId: string;
    public get ValueOfGoodsCurrencyId() { return this.valueOfGoodsCurrencyId; }
    public set ValueOfGoodsCurrencyId(newValue: string) { if (this.valueOfGoodsCurrencyId != newValue) { this.valueOfGoodsCurrencyId = newValue; this.MarkAsDirty("ValueOfGoodsCurrencyId"); } }
       
	 
    private operationalDate: Date;
    public get OperationalDate() { return this.operationalDate; }
    public set OperationalDate(newValue: Date) { if (this.operationalDate != newValue) { this.operationalDate = newValue; this.MarkAsDirty("OperationalDate"); } }
       
	 
    private fBLIsFromStock: boolean;
    public get FBLIsFromStock() { return this.fBLIsFromStock; }
    public set FBLIsFromStock(newValue: boolean) { if (this.fBLIsFromStock != newValue) { this.fBLIsFromStock = newValue; this.MarkAsDirty("FBLIsFromStock"); } }
       
	 
    private fBLReturnedToStock: boolean;
    public get FBLReturnedToStock() { return this.fBLReturnedToStock; }
    public set FBLReturnedToStock(newValue: boolean) { if (this.fBLReturnedToStock != newValue) { this.fBLReturnedToStock = newValue; this.MarkAsDirty("FBLReturnedToStock"); } }
       
	 
    private fBLTakenFromStock: boolean;
    public get FBLTakenFromStock() { return this.fBLTakenFromStock; }
    public set FBLTakenFromStock(newValue: boolean) { if (this.fBLTakenFromStock != newValue) { this.fBLTakenFromStock = newValue; this.MarkAsDirty("FBLTakenFromStock"); } }
       
	 
    private fBLStockNumber: string;
    public get FBLStockNumber() { return this.fBLStockNumber; }
    public set FBLStockNumber(newValue: string) { if (this.fBLStockNumber != newValue) { this.fBLStockNumber = newValue; this.MarkAsDirty("FBLStockNumber"); } }
       
	 
    private fBLReturnedToStockWithCancel: boolean;
    public get FBLReturnedToStockWithCancel() { return this.fBLReturnedToStockWithCancel; }
    public set FBLReturnedToStockWithCancel(newValue: boolean) { if (this.fBLReturnedToStockWithCancel != newValue) { this.fBLReturnedToStockWithCancel = newValue; this.MarkAsDirty("FBLReturnedToStockWithCancel"); } }
       
	 
    
     private mainCarriageFromPartnerId: string;
    public get MainCarriageFromPartnerId() { return this.mainCarriageFromPartnerId; }
    public set MainCarriageFromPartnerId(newValue: string) { if (this.mainCarriageFromPartnerId != newValue) { this.mainCarriageFromPartnerId = newValue; this.MarkAsDirty("MainCarriageFromPartnerId"); } }
       
	 
    private mainCarriageFromAddressId: string;
    public get MainCarriageFromAddressId() { return this.mainCarriageFromAddressId; }
    public set MainCarriageFromAddressId(newValue: string) { if (this.mainCarriageFromAddressId != newValue) { this.mainCarriageFromAddressId = newValue; this.MarkAsDirty("MainCarriageFromAddressId"); } }
       
	 
    private mainCarriageToPartnerId: string;
    public get MainCarriageToPartnerId() { return this.mainCarriageToPartnerId; }
    public set MainCarriageToPartnerId(newValue: string) { if (this.mainCarriageToPartnerId != newValue) { this.mainCarriageToPartnerId = newValue; this.MarkAsDirty("MainCarriageToPartnerId"); } }
       
	 
    private mainCarriageToAddressId: string;
    public get MainCarriageToAddressId() { return this.mainCarriageToAddressId; }
    public set MainCarriageToAddressId(newValue: string) { if (this.mainCarriageToAddressId != newValue) { this.mainCarriageToAddressId = newValue; this.MarkAsDirty("MainCarriageToAddressId"); } }
       
	 
    private driver: string;
    public get Driver() { return this.driver; }
    public set Driver(newValue: string) { if (this.driver != newValue) { this.driver = newValue; this.MarkAsDirty("Driver"); } }
       
	 
    private truckNumber: string;
    public get TruckNumber() { return this.truckNumber; }
    public set TruckNumber(newValue: string) { if (this.truckNumber != newValue) { this.truckNumber = newValue; this.MarkAsDirty("TruckNumber"); } }
       
	 
    private trailerNumber: string;
    public get TrailerNumber() { return this.trailerNumber; }
    public set TrailerNumber(newValue: string) { if (this.trailerNumber != newValue) { this.trailerNumber = newValue; this.MarkAsDirty("TrailerNumber"); } }


    private transshipment1TrailerNumber: string;
    public get Transshipment1TrailerNumber() { return this.transshipment1TrailerNumber; }
    public set Transshipment1TrailerNumber(newValue: string) { if (this.transshipment1TrailerNumber != newValue) { this.transshipment1TrailerNumber = newValue; this.MarkAsDirty("Transshipment1TrailerNumber"); } }


    private transshipment2TrailerNumber: string;
    public get Transshipment2TrailerNumber() { return this.transshipment2TrailerNumber; }
    public set Transshipment2TrailerNumber(newValue: string) { if (this.transshipment2TrailerNumber != newValue) { this.transshipment2TrailerNumber = newValue; this.MarkAsDirty("Transshipment2TrailerNumber"); } }


    private transshipment3TrailerNumber: string;
    public get Transshipment3TrailerNumber() { return this.transshipment3TrailerNumber; }
    public set Transshipment3TrailerNumber(newValue: string) { if (this.transshipment3TrailerNumber != newValue) { this.transshipment3TrailerNumber = newValue; this.MarkAsDirty("Transshipment3TrailerNumber"); } }
      	 
    private asAgreedFreight: boolean;
    public get AsAgreedFreight() { return this.asAgreedFreight; }
    public set AsAgreedFreight(newValue: boolean) { if (this.asAgreedFreight != newValue) { this.asAgreedFreight = newValue; this.MarkAsDirty("AsAgreedFreight"); } }
       
	 
    private asAgreedOtherCharges: boolean;
    public get AsAgreedOtherCharges() { return this.asAgreedOtherCharges; }
    public set AsAgreedOtherCharges(newValue: boolean) { if (this.asAgreedOtherCharges != newValue) { this.asAgreedOtherCharges = newValue; this.MarkAsDirty("AsAgreedOtherCharges"); } }
       
	 
    private aRInvoiceIssued: boolean;
    public get ARInvoiceIssued() { return this.aRInvoiceIssued; }
    public set ARInvoiceIssued(newValue: boolean) { if (this.aRInvoiceIssued != newValue) { this.aRInvoiceIssued = newValue; this.MarkAsDirty("ARInvoiceIssued"); } }
       
	 
    private creditNoteIssued: boolean;
    public get CreditNoteIssued() { return this.creditNoteIssued; }
    public set CreditNoteIssued(newValue: boolean) { if (this.creditNoteIssued != newValue) { this.creditNoteIssued = newValue; this.MarkAsDirty("CreditNoteIssued"); } }
       
	 
    private accountNumber: string;
    public get AccountNumber() { return this.accountNumber; }
    public set AccountNumber(newValue: string) { if (this.accountNumber != newValue) { this.accountNumber = newValue; this.MarkAsDirty("AccountNumber"); } }
       
	 
    private cargonautFHLStatusCode: string;
    public get CargonautFHLStatusCode() { return this.cargonautFHLStatusCode; }
    public set CargonautFHLStatusCode(newValue: string) { if (this.cargonautFHLStatusCode != newValue) { this.cargonautFHLStatusCode = newValue; this.MarkAsDirty("CargonautFHLStatusCode"); } }
       
	 
    private cargonautFHLStatusName: string;
    public get CargonautFHLStatusName() { return this.cargonautFHLStatusName; }
    public set CargonautFHLStatusName(newValue: string) { if (this.cargonautFHLStatusName != newValue) { this.cargonautFHLStatusName = newValue; this.MarkAsDirty("CargonautFHLStatusName"); } }
       
	 
    private cargonautFHLStatusDate: Date;
    public get CargonautFHLStatusDate() { return this.cargonautFHLStatusDate; }
    public set CargonautFHLStatusDate(newValue: Date) { if (this.cargonautFHLStatusDate != newValue) { this.cargonautFHLStatusDate = newValue; this.MarkAsDirty("CargonautFHLStatusDate"); } }
       
	 
    private cargonautFWBStatusCode: string;
    public get CargonautFWBStatusCode() { return this.cargonautFWBStatusCode; }
    public set CargonautFWBStatusCode(newValue: string) { if (this.cargonautFWBStatusCode != newValue) { this.cargonautFWBStatusCode = newValue; this.MarkAsDirty("CargonautFWBStatusCode"); } }
       
	 
    private cargonautFWBStatusName: string;
    public get CargonautFWBStatusName() { return this.cargonautFWBStatusName; }
    public set CargonautFWBStatusName(newValue: string) { if (this.cargonautFWBStatusName != newValue) { this.cargonautFWBStatusName = newValue; this.MarkAsDirty("CargonautFWBStatusName"); } }
       
	 
    private cargonautFWBStatusDate: Date;
    public get CargonautFWBStatusDate() { return this.cargonautFWBStatusDate; }
    public set CargonautFWBStatusDate(newValue: Date) { if (this.cargonautFWBStatusDate != newValue) { this.cargonautFWBStatusDate = newValue; this.MarkAsDirty("CargonautFWBStatusDate"); } }
       
	 
    private markFollowUpsAsDone: boolean;
    public get MarkFollowUpsAsDone() { return this.markFollowUpsAsDone; }
    public set MarkFollowUpsAsDone(newValue: boolean) { if (this.markFollowUpsAsDone != newValue) { this.markFollowUpsAsDone = newValue; this.MarkAsDirty("MarkFollowUpsAsDone"); } }
       
	 
    private calculateProfit: boolean;
    public get CalculateProfit() { return this.calculateProfit; }
    public set CalculateProfit(newValue: boolean) { if (this.calculateProfit != newValue) { this.calculateProfit = newValue; this.MarkAsDirty("CalculateProfit"); } }
       
	 
    private calculateStatus: boolean;
    public get CalculateStatus() { return this.calculateStatus; }
    public set CalculateStatus(newValue: boolean) { if (this.calculateStatus != newValue) { this.calculateStatus = newValue; this.MarkAsDirty("CalculateStatus"); } }
       
	 
    private computedStatusId: string;
    public get ComputedStatusId() { return this.computedStatusId; }
    public set ComputedStatusId(newValue: string) { if (this.computedStatusId != newValue) { this.computedStatusId = newValue; this.MarkAsDirty("ComputedStatusId"); } }
       
	 
    private computedStatusDate: Date;
    public get ComputedStatusDate() { return this.computedStatusDate; }
    public set ComputedStatusDate(newValue: Date) { if (this.computedStatusDate != newValue) { this.computedStatusDate = newValue; this.MarkAsDirty("ComputedStatusDate"); } }
       
	 
    private computedStatusName: string;
    public get ComputedStatusName() { return this.computedStatusName; }
    public set ComputedStatusName(newValue: string) { if (this.computedStatusName != newValue) { this.computedStatusName = newValue; this.MarkAsDirty("ComputedStatusName"); } }
       
	 
    private customFilePocoId: string;
    public get CustomFilePocoId() { return this.customFilePocoId; }
    public set CustomFilePocoId(newValue: string) { if (this.customFilePocoId != newValue) { this.customFilePocoId = newValue; this.MarkAsDirty("CustomFilePocoId"); } }
       
	 
    private calculatePayables: boolean;
    public get CalculatePayables() { return this.calculatePayables; }
    public set CalculatePayables(newValue: boolean) { if (this.calculatePayables != newValue) { this.calculatePayables = newValue; this.MarkAsDirty("CalculatePayables"); } }
       
	 
    private calculateReceivables: boolean;
    public get CalculateReceivables() { return this.calculateReceivables; }
    public set CalculateReceivables(newValue: boolean) { if (this.calculateReceivables != newValue) { this.calculateReceivables = newValue; this.MarkAsDirty("CalculateReceivables"); } }
       
	 
    private isAddingStackEvents: boolean;
    public get IsAddingStackEvents() { return this.isAddingStackEvents; }
    public set IsAddingStackEvents(newValue: boolean) { if (this.isAddingStackEvents != newValue) { this.isAddingStackEvents = newValue; this.MarkAsDirty("IsAddingStackEvents"); } }
       
	 
    private isRemovingStackEvents: boolean;
    public get IsRemovingStackEvents() { return this.isRemovingStackEvents; }
    public set IsRemovingStackEvents(newValue: boolean) { if (this.isRemovingStackEvents != newValue) { this.isRemovingStackEvents = newValue; this.MarkAsDirty("IsRemovingStackEvents"); } }
       
	 
    private stackAirlineId: string;
    public get StackAirlineId() { return this.stackAirlineId; }
    public set StackAirlineId(newValue: string) { if (this.stackAirlineId != newValue) { this.stackAirlineId = newValue; this.MarkAsDirty("StackAirlineId"); } }
       
	 
    private fromPartnerCity: string;
    public get FromPartnerCity() { return this.fromPartnerCity; }
    public set FromPartnerCity(newValue: string) { if (this.fromPartnerCity != newValue) { this.fromPartnerCity = newValue; this.MarkAsDirty("FromPartnerCity"); } }
       
	 
    private fromPartnerCountryCode: string;
    public get FromPartnerCountryCode() { return this.fromPartnerCountryCode; }
    public set FromPartnerCountryCode(newValue: string) { if (this.fromPartnerCountryCode != newValue) { this.fromPartnerCountryCode = newValue; this.MarkAsDirty("FromPartnerCountryCode"); } }
       
	 
    private fromPartnerCountryName: string;
    public get FromPartnerCountryName() { return this.fromPartnerCountryName; }
    public set FromPartnerCountryName(newValue: string) { if (this.fromPartnerCountryName != newValue) { this.fromPartnerCountryName = newValue; this.MarkAsDirty("FromPartnerCountryName"); } }
       
	 
    private toPartnerCity: string;
    public get ToPartnerCity() { return this.toPartnerCity; }
    public set ToPartnerCity(newValue: string) { if (this.toPartnerCity != newValue) { this.toPartnerCity = newValue; this.MarkAsDirty("ToPartnerCity"); } }
       
	 
    private toPartnerCountryCode: string;
    public get ToPartnerCountryCode() { return this.toPartnerCountryCode; }
    public set ToPartnerCountryCode(newValue: string) { if (this.toPartnerCountryCode != newValue) { this.toPartnerCountryCode = newValue; this.MarkAsDirty("ToPartnerCountryCode"); } }
       
	 
    private toPartnerCountryName: string;
    public get ToPartnerCountryName() { return this.toPartnerCountryName; }
    public set ToPartnerCountryName(newValue: string) { if (this.toPartnerCountryName != newValue) { this.toPartnerCountryName = newValue; this.MarkAsDirty("ToPartnerCountryName"); } }
       
	 
    private isSendFSRCreatingShipment: boolean;
    public get IsSendFSRCreatingShipment() { return this.isSendFSRCreatingShipment; }
    public set IsSendFSRCreatingShipment(newValue: boolean) { if (this.isSendFSRCreatingShipment != newValue) { this.isSendFSRCreatingShipment = newValue; this.MarkAsDirty("IsSendFSRCreatingShipment"); } }
       
	 
    private toCountryId: string;
    public get ToCountryId() { return this.toCountryId; }
    public set ToCountryId(newValue: string) { if (this.toCountryId != newValue) { this.toCountryId = newValue; this.MarkAsDirty("ToCountryId"); } }
       
	 
    private fromCountryId: string;
    public get FromCountryId() { return this.fromCountryId; }
    public set FromCountryId(newValue: string) { if (this.fromCountryId != newValue) { this.fromCountryId = newValue; this.MarkAsDirty("FromCountryId"); } }
       
	 
    private copyFromShipmentId: string;
    public get CopyFromShipmentId() { return this.copyFromShipmentId; }
    public set CopyFromShipmentId(newValue: string) { if (this.copyFromShipmentId != newValue) { this.copyFromShipmentId = newValue; this.MarkAsDirty("CopyFromShipmentId"); } }
       
	 
    private isCopyFromShipment: boolean;
    public get IsCopyFromShipment() { return this.isCopyFromShipment; }
    public set IsCopyFromShipment(newValue: boolean) { if (this.isCopyFromShipment != newValue) { this.isCopyFromShipment = newValue; this.MarkAsDirty("IsCopyFromShipment"); } }
       
	 
    private isBuildFromQuote: boolean;
    public get IsBuildFromQuote() { return this.isBuildFromQuote; }
    public set IsBuildFromQuote(newValue: boolean) { if (this.isBuildFromQuote != newValue) { this.isBuildFromQuote = newValue; this.MarkAsDirty("IsBuildFromQuote"); } }
       
	 
    private isBuildFromBooking: boolean;
    public get IsBuildFromBooking() { return this.isBuildFromBooking; }
    public set IsBuildFromBooking(newValue: boolean) { if (this.isBuildFromBooking != newValue) { this.isBuildFromBooking = newValue; this.MarkAsDirty("IsBuildFromBooking"); } }
       
	 
    private shipperMainAddressId: string;
    public get ShipperMainAddressId() { return this.shipperMainAddressId; }
    public set ShipperMainAddressId(newValue: string) { if (this.shipperMainAddressId != newValue) { this.shipperMainAddressId = newValue; this.MarkAsDirty("ShipperMainAddressId"); } }
       
	 
    private shipperPickAddressId: string;
    public get ShipperPickAddressId() { return this.shipperPickAddressId; }
    public set ShipperPickAddressId(newValue: string) { if (this.shipperPickAddressId != newValue) { this.shipperPickAddressId = newValue; this.MarkAsDirty("ShipperPickAddressId"); } }
       
	 
    private consigneeMainAddressId: string;
    public get ConsigneeMainAddressId() { return this.consigneeMainAddressId; }
    public set ConsigneeMainAddressId(newValue: string) { if (this.consigneeMainAddressId != newValue) { this.consigneeMainAddressId = newValue; this.MarkAsDirty("ConsigneeMainAddressId"); } }
       
	 
    private consigneePickAddressId: string;
    public get ConsigneePickAddressId() { return this.consigneePickAddressId; }
    public set ConsigneePickAddressId(newValue: string) { if (this.consigneePickAddressId != newValue) { this.consigneePickAddressId = newValue; this.MarkAsDirty("ConsigneePickAddressId"); } }
       
	 
    private hasPreCarriage: boolean;
    public get HasPreCarriage() { return this.hasPreCarriage; }
    public set HasPreCarriage(newValue: boolean) { if (this.hasPreCarriage != newValue) { this.hasPreCarriage = newValue; this.MarkAsDirty("HasPreCarriage"); } }
       
	 
    private hasOnCarriage: boolean;
    public get HasOnCarriage() { return this.hasOnCarriage; }
    public set HasOnCarriage(newValue: boolean) { if (this.hasOnCarriage != newValue) { this.hasOnCarriage = newValue; this.MarkAsDirty("HasOnCarriage"); } }
       
	 
    private includePickUp: boolean;
    public get IncludePickUp() { return this.includePickUp; }
    public set IncludePickUp(newValue: boolean) { if (this.includePickUp != newValue) { this.includePickUp = newValue; this.MarkAsDirty("IncludePickUp"); } }
       
	 
    private fromAddressCity: string;
    public get FromAddressCity() { return this.fromAddressCity; }
    public set FromAddressCity(newValue: string) { if (this.fromAddressCity != newValue) { this.fromAddressCity = newValue; this.MarkAsDirty("FromAddressCity"); } }
       
	 
    private fromAddressZipCode: string;
    public get FromAddressZipCode() { return this.fromAddressZipCode; }
    public set FromAddressZipCode(newValue: string) { if (this.fromAddressZipCode != newValue) { this.fromAddressZipCode = newValue; this.MarkAsDirty("FromAddressZipCode"); } }
       
	 
    private fromAddressCountryId: string;
    public get FromAddressCountryId() { return this.fromAddressCountryId; }
    public set FromAddressCountryId(newValue: string) { if (this.fromAddressCountryId != newValue) { this.fromAddressCountryId = newValue; this.MarkAsDirty("FromAddressCountryId"); } }
       
	 
    private pickUpAddressId: string;
    public get PickUpAddressId() { return this.pickUpAddressId; }
    public set PickUpAddressId(newValue: string) { if (this.pickUpAddressId != newValue) { this.pickUpAddressId = newValue; this.MarkAsDirty("PickUpAddressId"); } }
       
	 
    private customConnectToShipment: boolean;
    public get CustomConnectToShipment() { return this.customConnectToShipment; }
    public set CustomConnectToShipment(newValue: boolean) { if (this.customConnectToShipment != newValue) { this.customConnectToShipment = newValue; this.MarkAsDirty("CustomConnectToShipment"); } }
       
	 
    private includeDelivery: boolean;
    public get IncludeDelivery() { return this.includeDelivery; }
    public set IncludeDelivery(newValue: boolean) { if (this.includeDelivery != newValue) { this.includeDelivery = newValue; this.MarkAsDirty("IncludeDelivery"); } }
       
	 
    private toAddressCity: string;
    public get ToAddressCity() { return this.toAddressCity; }
    public set ToAddressCity(newValue: string) { if (this.toAddressCity != newValue) { this.toAddressCity = newValue; this.MarkAsDirty("ToAddressCity"); } }
       
	 
    private toAddressZipCode: string;
    public get ToAddressZipCode() { return this.toAddressZipCode; }
    public set ToAddressZipCode(newValue: string) { if (this.toAddressZipCode != newValue) { this.toAddressZipCode = newValue; this.MarkAsDirty("ToAddressZipCode"); } }
       
	 
    private toAddressCountryId: string;
    public get ToAddressCountryId() { return this.toAddressCountryId; }
    public set ToAddressCountryId(newValue: string) { if (this.toAddressCountryId != newValue) { this.toAddressCountryId = newValue; this.MarkAsDirty("ToAddressCountryId"); } }
       
	 
    private deliveryAddressId: string;
    public get DeliveryAddressId() { return this.deliveryAddressId; }
    public set DeliveryAddressId(newValue: string) { if (this.deliveryAddressId != newValue) { this.deliveryAddressId = newValue; this.MarkAsDirty("DeliveryAddressId"); } }
       
	 
    private specialServicesTypeId: string;
    public get SpecialServicesTypeId() { return this.specialServicesTypeId; }
    public set SpecialServicesTypeId(newValue: string) { if (this.specialServicesTypeId != newValue) { this.specialServicesTypeId = newValue; this.MarkAsDirty("SpecialServicesTypeId"); } }
       
	 
    private specialServicesTypeName: string;
    public get SpecialServicesTypeName() { return this.specialServicesTypeName; }
    public set SpecialServicesTypeName(newValue: string) { if (this.specialServicesTypeName != newValue) { this.specialServicesTypeName = newValue; this.MarkAsDirty("SpecialServicesTypeName"); } }
       

  
    private containerNumber1: string;
    public get ContainerNumber1() { return this.containerNumber1; }
    public set ContainerNumber1(newValue: string) { if (this.containerNumber1 != newValue) { this.containerNumber1 = newValue; this.MarkAsDirty("ContainerNumber1"); } }



    private containerNumber2: string;
    public get ContainerNumber2() { return this.containerNumber2; }
    public set ContainerNumber2(newValue: string) { if (this.containerNumber2 != newValue) { this.containerNumber2 = newValue; this.MarkAsDirty("ContainerNumber2"); } }


    private containerNumber3: string;
    public get ContainerNumber3() { return this.containerNumber3; }
    public set ContainerNumber3(newValue: string) { if (this.containerNumber3 != newValue) { this.containerNumber3 = newValue; this.MarkAsDirty("ContainerNumber3"); } }


    private containerNumber4: string;
    public get ContainerNumber4() { return this.containerNumber4; }
    public set ContainerNumber4(newValue: string) { if (this.containerNumber4 != newValue) { this.containerNumber4 = newValue; this.MarkAsDirty("ContainerNumber4"); } }

    private containerNumber5: string;
    public get ContainerNumber5() { return this.containerNumber5; }
    public set ContainerNumber5(newValue: string) { if (this.containerNumber5 != newValue) { this.containerNumber5 = newValue; this.MarkAsDirty("ContainerNumber5"); } }



    private quantity1: number;
    public get Quantity1() { return this.quantity1; }
    public set Quantity1(newValue: number) { if (this.quantity1 != newValue) { this.quantity1 = newValue; this.MarkAsDirty("Quantity1"); } }
       
	 
    private quantity2: number;
    public get Quantity2() { return this.quantity2; }
    public set Quantity2(newValue: number) { if (this.quantity2 != newValue) { this.quantity2 = newValue; this.MarkAsDirty("Quantity2"); } }
       
	 
    private quantity3: number;
    public get Quantity3() { return this.quantity3; }
    public set Quantity3(newValue: number) { if (this.quantity3 != newValue) { this.quantity3 = newValue; this.MarkAsDirty("Quantity3"); } }
       
	 
    private quantity4: number;
    public get Quantity4() { return this.quantity4; }
    public set Quantity4(newValue: number) { if (this.quantity4 != newValue) { this.quantity4 = newValue; this.MarkAsDirty("Quantity4"); } }
       
	 
    private quantity5: number;
    public get Quantity5() { return this.quantity5; }
    public set Quantity5(newValue: number) { if (this.quantity5 != newValue) { this.quantity5 = newValue; this.MarkAsDirty("Quantity5"); } }
       
	 
    private packageTypeId1: string;
    public get PackageTypeId1() { return this.packageTypeId1; }
    public set PackageTypeId1(newValue: string) { if (this.packageTypeId1 != newValue) { this.packageTypeId1 = newValue; this.MarkAsDirty("PackageTypeId1"); } }
       
	 
    private packageTypeId2: string;
    public get PackageTypeId2() { return this.packageTypeId2; }
    public set PackageTypeId2(newValue: string) { if (this.packageTypeId2 != newValue) { this.packageTypeId2 = newValue; this.MarkAsDirty("PackageTypeId2"); } }
       
	 
    private packageTypeId3: string;
    public get PackageTypeId3() { return this.packageTypeId3; }
    public set PackageTypeId3(newValue: string) { if (this.packageTypeId3 != newValue) { this.packageTypeId3 = newValue; this.MarkAsDirty("PackageTypeId3"); } }
       
	 
    private packageTypeId4: string;
    public get PackageTypeId4() { return this.packageTypeId4; }
    public set PackageTypeId4(newValue: string) { if (this.packageTypeId4 != newValue) { this.packageTypeId4 = newValue; this.MarkAsDirty("PackageTypeId4"); } }
       
	 
    private packageTypeId5: string;
    public get PackageTypeId5() { return this.packageTypeId5; }
    public set PackageTypeId5(newValue: string) { if (this.packageTypeId5 != newValue) { this.packageTypeId5 = newValue; this.MarkAsDirty("PackageTypeId5"); } }
       
	 
    private newConcurrencyGUID: string;
    public get NewConcurrencyGUID() { return this.newConcurrencyGUID; }
    public set NewConcurrencyGUID(newValue: string) { if (this.newConcurrencyGUID != newValue) { this.newConcurrencyGUID = newValue; this.MarkAsDirty("NewConcurrencyGUID"); } }
       
	 
    private connectedShipmentsPayablesCount: number;
    public get ConnectedShipmentsPayablesCount() { return this.connectedShipmentsPayablesCount; }
    public set ConnectedShipmentsPayablesCount(newValue: number) { if (this.connectedShipmentsPayablesCount != newValue) { this.connectedShipmentsPayablesCount = newValue; this.MarkAsDirty("ConnectedShipmentsPayablesCount"); } }
       
	 
    private connectedShipmentsReceivablesCount: number;
    public get ConnectedShipmentsReceivablesCount() { return this.connectedShipmentsReceivablesCount; }
    public set ConnectedShipmentsReceivablesCount(newValue: number) { if (this.connectedShipmentsReceivablesCount != newValue) { this.connectedShipmentsReceivablesCount = newValue; this.MarkAsDirty("ConnectedShipmentsReceivablesCount"); } }
       
	 
    private numberOfInsidePackages: number;
    public get NumberOfInsidePackages() { return this.numberOfInsidePackages; }
    public set NumberOfInsidePackages(newValue: number) { if (this.numberOfInsidePackages != newValue) { this.numberOfInsidePackages = newValue; this.MarkAsDirty("NumberOfInsidePackages"); } }
       
	 
    private numberOfInsidePackagesDetails: string;
    public get NumberOfInsidePackagesDetails() { return this.numberOfInsidePackagesDetails; }
    public set NumberOfInsidePackagesDetails(newValue: string) { if (this.numberOfInsidePackagesDetails != newValue) { this.numberOfInsidePackagesDetails = newValue; this.MarkAsDirty("NumberOfInsidePackagesDetails"); } }
       
	 
    private accountManagerUserId: string;
    public get AccountManagerUserId() { return this.accountManagerUserId; }
    public set AccountManagerUserId(newValue: string) { if (this.accountManagerUserId != newValue) { this.accountManagerUserId = newValue; this.MarkAsDirty("AccountManagerUserId"); } }
       
	 
    private accountManagerUserName: string;
    public get AccountManagerUserName() { return this.accountManagerUserName; }
    public set AccountManagerUserName(newValue: string) { if (this.accountManagerUserName != newValue) { this.accountManagerUserName = newValue; this.MarkAsDirty("AccountManagerUserName"); } }
       
	 
    private manifestReason: string;
    public get ManifestReason() { return this.manifestReason; }
    public set ManifestReason(newValue: string) { if (this.manifestReason != newValue) { this.manifestReason = newValue; this.MarkAsDirty("ManifestReason"); } }
       
	 
    private manifestStatusCode: string;
    public get ManifestStatusCode() { return this.manifestStatusCode; }
    public set ManifestStatusCode(newValue: string) { if (this.manifestStatusCode != newValue) { this.manifestStatusCode = newValue; this.MarkAsDirty("ManifestStatusCode"); } }
       
	 
    private isKnownCargo: boolean;
    public get IsKnownCargo() { return this.isKnownCargo; }
    public set IsKnownCargo(newValue: boolean) { if (this.isKnownCargo != newValue) { this.isKnownCargo = newValue; this.MarkAsDirty("IsKnownCargo"); } }
       
	 
    private regulatedAgentRANumber: string;
    public get RegulatedAgentRANumber() { return this.regulatedAgentRANumber; }
    public set RegulatedAgentRANumber(newValue: string) { if (this.regulatedAgentRANumber != newValue) { this.regulatedAgentRANumber = newValue; this.MarkAsDirty("RegulatedAgentRANumber"); } }
       
	 
    private knownConsignorNumber: string;
    public get KnownConsignorNumber() { return this.knownConsignorNumber; }
    public set KnownConsignorNumber(newValue: string) { if (this.knownConsignorNumber != newValue) { this.knownConsignorNumber = newValue; this.MarkAsDirty("KnownConsignorNumber"); } }
       
	 
    private kCExpirationDate: Date;
    public get KCExpirationDate() { return this.kCExpirationDate; }
    public set KCExpirationDate(newValue: Date) { if (this.kCExpirationDate != newValue) { this.kCExpirationDate = newValue; this.MarkAsDirty("KCExpirationDate"); } }
       
	 
    private coloaderRANumber: string;
    public get ColoaderRANumber() { return this.coloaderRANumber; }
    public set ColoaderRANumber(newValue: string) { if (this.coloaderRANumber != newValue) { this.coloaderRANumber = newValue; this.MarkAsDirty("ColoaderRANumber"); } }
       
	 
    private aWBPrintingSecurityStatusId: string;
    public get AWBPrintingSecurityStatusId() { return this.aWBPrintingSecurityStatusId; }
    public set AWBPrintingSecurityStatusId(newValue: string) { if (this.aWBPrintingSecurityStatusId != newValue) { this.aWBPrintingSecurityStatusId = newValue; this.MarkAsDirty("AWBPrintingSecurityStatusId"); } }
       
	 
    private aWBPrintingRANumber: string;
    public get AWBPrintingRANumber() { return this.aWBPrintingRANumber; }
    public set AWBPrintingRANumber(newValue: string) { if (this.aWBPrintingRANumber != newValue) { this.aWBPrintingRANumber = newValue; this.MarkAsDirty("AWBPrintingRANumber"); } }
       
	 
    private additionalHandlingInfo: string;
    public get AdditionalHandlingInfo() { return this.additionalHandlingInfo; }
    public set AdditionalHandlingInfo(newValue: string) { if (this.additionalHandlingInfo != newValue) { this.additionalHandlingInfo = newValue; this.MarkAsDirty("AdditionalHandlingInfo"); } }
       
	 
    private aWBPrintingSecurityStatusEdited: boolean;
    public get AWBPrintingSecurityStatusEdited() { return this.aWBPrintingSecurityStatusEdited; }
    public set AWBPrintingSecurityStatusEdited(newValue: boolean) { if (this.aWBPrintingSecurityStatusEdited != newValue) { this.aWBPrintingSecurityStatusEdited = newValue; this.MarkAsDirty("AWBPrintingSecurityStatusEdited"); } }
       
	 
    private aWBPrintingRANumberEdited: boolean;
    public get AWBPrintingRANumberEdited() { return this.aWBPrintingRANumberEdited; }
    public set AWBPrintingRANumberEdited(newValue: boolean) { if (this.aWBPrintingRANumberEdited != newValue) { this.aWBPrintingRANumberEdited = newValue; this.MarkAsDirty("AWBPrintingRANumberEdited"); } }
       
	 
    private additionalHandlingInfoEdited: boolean;
    public get AdditionalHandlingInfoEdited() { return this.additionalHandlingInfoEdited; }
    public set AdditionalHandlingInfoEdited(newValue: boolean) { if (this.additionalHandlingInfoEdited != newValue) { this.additionalHandlingInfoEdited = newValue; this.MarkAsDirty("AdditionalHandlingInfoEdited"); } }
       
	 
    private viaColoader: boolean;
    public get ViaColoader() { return this.viaColoader; }
    public set ViaColoader(newValue: boolean) { if (this.viaColoader != newValue) { this.viaColoader = newValue; this.MarkAsDirty("ViaColoader"); } }
       
	 
    private issuingCarrierReference1: string;
    public get IssuingCarrierReference1() { return this.issuingCarrierReference1; }
    public set IssuingCarrierReference1(newValue: string) { if (this.issuingCarrierReference1 != newValue) { this.issuingCarrierReference1 = newValue; this.MarkAsDirty("IssuingCarrierReference1"); } }
       
	 
    private isMissingDocument: boolean;
    public get IsMissingDocument() { return this.isMissingDocument; }
    public set IsMissingDocument(newValue: boolean) { if (this.isMissingDocument != newValue) { this.isMissingDocument = newValue; this.MarkAsDirty("IsMissingDocument"); } }
       
	 
    private documentsSearchFields: string;
    public get DocumentsSearchFields() { return this.documentsSearchFields; }
    public set DocumentsSearchFields(newValue: string) { if (this.documentsSearchFields != newValue) { this.documentsSearchFields = newValue; this.MarkAsDirty("DocumentsSearchFields"); } }
       
	 
    private interlineId: string;
    public get InterlineId() { return this.interlineId; }
    public set InterlineId(newValue: string) { if (this.interlineId != newValue) { this.interlineId = newValue; this.MarkAsDirty("InterlineId"); } }
       
	 
    private shipperAddress1: string;
    public get ShipperAddress1() { return this.shipperAddress1; }
    public set ShipperAddress1(newValue: string) { if (this.shipperAddress1 != newValue) { this.shipperAddress1 = newValue; this.MarkAsDirty("ShipperAddress1"); } }
       
	 
    private shipperAddress2: string;
    public get ShipperAddress2() { return this.shipperAddress2; }
    public set ShipperAddress2(newValue: string) { if (this.shipperAddress2 != newValue) { this.shipperAddress2 = newValue; this.MarkAsDirty("ShipperAddress2"); } }
       
	 
    private shipperZipCode: string;
    public get ShipperZipCode() { return this.shipperZipCode; }
    public set ShipperZipCode(newValue: string) { if (this.shipperZipCode != newValue) { this.shipperZipCode = newValue; this.MarkAsDirty("ShipperZipCode"); } }
       
	 
    private shipperStateId: string;
    public get ShipperStateId() { return this.shipperStateId; }
    public set ShipperStateId(newValue: string) { if (this.shipperStateId != newValue) { this.shipperStateId = newValue; this.MarkAsDirty("ShipperStateId"); } }
       
	 
    private shipperCountryId: string;
    public get ShipperCountryId() { return this.shipperCountryId; }
    public set ShipperCountryId(newValue: string) { if (this.shipperCountryId != newValue) { this.shipperCountryId = newValue; this.MarkAsDirty("ShipperCountryId"); } }       
	 
    private shipperCity: string;
    public get ShipperCity() { return this.shipperCity; }
    public set ShipperCity(newValue: string) { if (this.shipperCity != newValue) { this.shipperCity = newValue; this.MarkAsDirty("ShipperCity"); } }
       
    private shipperPhoneNumber: string;
    public get ShipperPhoneNumber() { return this.shipperPhoneNumber; }
    public set ShipperPhoneNumber(newValue: string) { if (this.shipperPhoneNumber != newValue) { this.shipperPhoneNumber = newValue; this.MarkAsDirty("ShipperPhoneNumber"); } }

    private shipperFaxNumber: string;
    public get ShipperFaxNumber() { return this.shipperFaxNumber; }
    public set ShipperFaxNumber(newValue: string) { if (this.shipperFaxNumber != newValue) { this.shipperFaxNumber = newValue; this.MarkAsDirty("ShipperFaxNumber"); } }

    private consigneeAddress1: string;
    public get ConsigneeAddress1() { return this.consigneeAddress1; }
    public set ConsigneeAddress1(newValue: string) { if (this.consigneeAddress1 != newValue) { this.consigneeAddress1 = newValue; this.MarkAsDirty("ConsigneeAddress1"); } }
       
	 
    private consigneeAddress2: string;
    public get ConsigneeAddress2() { return this.consigneeAddress2; }
    public set ConsigneeAddress2(newValue: string) { if (this.consigneeAddress2 != newValue) { this.consigneeAddress2 = newValue; this.MarkAsDirty("ConsigneeAddress2"); } }
       
	 
    private consigneeZipCode: string;
    public get ConsigneeZipCode() { return this.consigneeZipCode; }
    public set ConsigneeZipCode(newValue: string) { if (this.consigneeZipCode != newValue) { this.consigneeZipCode = newValue; this.MarkAsDirty("ConsigneeZipCode"); } }
       
	 
    private consigneeStateId: string;
    public get ConsigneeStateId() { return this.consigneeStateId; }
    public set ConsigneeStateId(newValue: string) { if (this.consigneeStateId != newValue) { this.consigneeStateId = newValue; this.MarkAsDirty("ConsigneeStateId"); } }
       
	 
    private consigneeCountryId: string;
    public get ConsigneeCountryId() { return this.consigneeCountryId; }
    public set ConsigneeCountryId(newValue: string) { if (this.consigneeCountryId != newValue) { this.consigneeCountryId = newValue; this.MarkAsDirty("ConsigneeCountryId"); } }       
	 
    private consigneeCity: string;
    public get ConsigneeCity() { return this.consigneeCity; }
    public set ConsigneeCity(newValue: string) { if (this.consigneeCity != newValue) { this.consigneeCity = newValue; this.MarkAsDirty("ConsigneeCity"); } }

    private consigneePhoneNumber: string;
    public get ConsigneePhoneNumber() { return this.consigneePhoneNumber; }
    public set ConsigneePhoneNumber(newValue: string) { if (this.consigneePhoneNumber != newValue) { this.consigneePhoneNumber = newValue; this.MarkAsDirty("ConsigneePhoneNumber"); } }

    private consigneeFaxNumber: string;
    public get ConsigneeFaxNumber() { return this.consigneeFaxNumber; }
    public set ConsigneeFaxNumber(newValue: string) { if (this.consigneeFaxNumber != newValue) { this.consigneeFaxNumber = newValue; this.MarkAsDirty("ConsigneeFaxNumber"); } }

    private notify1Address1: string;
    public get Notify1Address1() { return this.notify1Address1; }
    public set Notify1Address1(newValue: string) { if (this.notify1Address1 != newValue) { this.notify1Address1 = newValue; this.MarkAsDirty("Notify1Address1"); } }
       
	 
    private notify1Address2: string;
    public get Notify1Address2() { return this.notify1Address2; }
    public set Notify1Address2(newValue: string) { if (this.notify1Address2 != newValue) { this.notify1Address2 = newValue; this.MarkAsDirty("Notify1Address2"); } }
       
	 
    private notify1ZipCode: string;
    public get Notify1ZipCode() { return this.notify1ZipCode; }
    public set Notify1ZipCode(newValue: string) { if (this.notify1ZipCode != newValue) { this.notify1ZipCode = newValue; this.MarkAsDirty("Notify1ZipCode"); } }
       
	 
    private notify1StateId: string;
    public get Notify1StateId() { return this.notify1StateId; }
    public set Notify1StateId(newValue: string) { if (this.notify1StateId != newValue) { this.notify1StateId = newValue; this.MarkAsDirty("Notify1StateId"); } }
       
	 
    private notify1CountryId: string;
    public get Notify1CountryId() { return this.notify1CountryId; }
    public set Notify1CountryId(newValue: string) { if (this.notify1CountryId != newValue) { this.notify1CountryId = newValue; this.MarkAsDirty("Notify1CountryId"); } }
       
	 
    private notify1City: string;
    public get Notify1City() { return this.notify1City; }
    public set Notify1City(newValue: string) { if (this.notify1City != newValue) { this.notify1City = newValue; this.MarkAsDirty("Notify1City"); } }
       
    private notify1PhoneNumber: string;
    public get Notify1PhoneNumber() { return this.notify1PhoneNumber; }
    public set Notify1PhoneNumber(newValue: string) { if (this.notify1PhoneNumber != newValue) { this.notify1PhoneNumber = newValue; this.MarkAsDirty("Notify1PhoneNumber"); } }

    private notify1FaxNumber: string;
    public get Notify1FaxNumber() { return this.notify1FaxNumber; }
    public set Notify1FaxNumber(newValue: string) { if (this.notify1FaxNumber != newValue) { this.notify1FaxNumber = newValue; this.MarkAsDirty("Notify1FaxNumber"); } }

    private issuingCarrierCity: string;
    public get IssuingCarrierCity() { return this.issuingCarrierCity; }
    public set IssuingCarrierCity(newValue: string) { if (this.issuingCarrierCity != newValue) { this.issuingCarrierCity = newValue; this.MarkAsDirty("IssuingCarrierCity"); } }
       
	 
    private mAWBReturnedToStackWithCancel: boolean;
    public get MAWBReturnedToStackWithCancel() { return this.mAWBReturnedToStackWithCancel; }
    public set MAWBReturnedToStackWithCancel(newValue: boolean) { if (this.mAWBReturnedToStackWithCancel != newValue) { this.mAWBReturnedToStackWithCancel = newValue; this.MarkAsDirty("MAWBReturnedToStackWithCancel"); } }
       
	 
    private mAWBStackAirlineId: string;
    public get MAWBStackAirlineId() { return this.mAWBStackAirlineId; }
    public set MAWBStackAirlineId(newValue: string) { if (this.mAWBStackAirlineId != newValue) { this.mAWBStackAirlineId = newValue; this.MarkAsDirty("MAWBStackAirlineId"); } }
     	 
    private dontAddToImportersQueue: boolean;
    public get DontAddToImportersQueue() { return this.dontAddToImportersQueue; }
    public set DontAddToImportersQueue(newValue: boolean) { if (this.dontAddToImportersQueue != newValue) { this.dontAddToImportersQueue = newValue; this.MarkAsDirty("DontAddToImportersQueue"); } }

    private dontAddToForwarderQueue: boolean;
    public get DontAddToForwarderQueue() { return this.dontAddToForwarderQueue; }
    public set DontAddToForwarderQueue(newValue: boolean) { if (this.dontAddToForwarderQueue != newValue) { this.dontAddToForwarderQueue = newValue; this.MarkAsDirty("DontAddToForwarderQueue"); } }
	 
    private forwarderPartnerId: string;
    public get ForwarderPartnerId() { return this.forwarderPartnerId; }
    public set ForwarderPartnerId(newValue: string) { if (this.forwarderPartnerId != newValue) { this.forwarderPartnerId = newValue; this.MarkAsDirty("ForwarderPartnerId"); } }

    private forwardingPartnerId: string;
    public get ForwardingPartnerId() { return this.forwardingPartnerId; }
    public set ForwardingPartnerId(newValue: string) { if (this.forwardingPartnerId != newValue) { this.forwardingPartnerId = newValue; this.MarkAsDirty("ForwardingPartnerId"); } }
	 
    private operationalCloseDate: Date;
    public get OperationalCloseDate() { return this.operationalCloseDate; }
    public set OperationalCloseDate(newValue: Date) { if (this.operationalCloseDate != newValue) { this.operationalCloseDate = newValue; this.MarkAsDirty("OperationalCloseDate"); } }
       
    private firstOperationalCloseDate: Date;
    public get FirstOperationalCloseDate() { return this.firstOperationalCloseDate; }
    public set FirstOperationalCloseDate(newValue: Date) { if (this.firstOperationalCloseDate != newValue) { this.firstOperationalCloseDate = newValue; this.MarkAsDirty("FirstOperationalCloseDate"); } }
    
    private firstAccountingCloseDate: Date;
    public get FirstAccountingCloseDate() { return this.firstAccountingCloseDate; }
    public set FirstAccountingCloseDate(newValue: Date) { if (this.firstAccountingCloseDate != newValue) { this.firstAccountingCloseDate = newValue; this.MarkAsDirty("FirstAccountingCloseDate"); } }

    private accountingCloseDate: Date;
    public get AccountingCloseDate() { return this.accountingCloseDate; }
    public set AccountingCloseDate(newValue: Date) { if (this.accountingCloseDate != newValue) { this.accountingCloseDate = newValue; this.MarkAsDirty("AccountingCloseDate"); } }
      	 
    private fromCountryCode: string;
    public get FromCountryCode() { return this.fromCountryCode; }
    public set FromCountryCode(newValue: string) { if (this.fromCountryCode != newValue) { this.fromCountryCode = newValue; this.MarkAsDirty("FromCountryCode"); } }
     	 
    private toCountryCode: string;
    public get ToCountryCode() { return this.toCountryCode; }
    public set ToCountryCode(newValue: string) { if (this.toCountryCode != newValue) { this.toCountryCode = newValue; this.MarkAsDirty("ToCountryCode"); } }
     	 
    private convertToCustomFile: boolean;
    public get ConvertToCustomFile() { return this.convertToCustomFile; }
    public set ConvertToCustomFile(newValue: boolean) { if (this.convertToCustomFile != newValue) { this.convertToCustomFile = newValue; this.MarkAsDirty("ConvertToCustomFile"); } }
     	 
    private customerTenantNumber: number;
    public get CustomerTenantNumber() { return this.customerTenantNumber; }
    public set CustomerTenantNumber(newValue: number) { if (this.customerTenantNumber != newValue) { this.customerTenantNumber = newValue; this.MarkAsDirty("CustomerTenantNumber"); } }
       
	 
    private mainCarriageFinalDestinationETA: Date;
    public get MainCarriageFinalDestinationETA() { return this.mainCarriageFinalDestinationETA; }
    public set MainCarriageFinalDestinationETA(newValue: Date) { if (this.mainCarriageFinalDestinationETA != newValue) { this.mainCarriageFinalDestinationETA = newValue; this.MarkAsDirty("MainCarriageFinalDestinationETA"); } }
       
	 
    private mainCarriageFinalDestinationATA: Date;
    public get MainCarriageFinalDestinationATA() { return this.mainCarriageFinalDestinationATA; }
    public set MainCarriageFinalDestinationATA(newValue: Date) { if (this.mainCarriageFinalDestinationATA != newValue) { this.mainCarriageFinalDestinationATA = newValue; this.MarkAsDirty("MainCarriageFinalDestinationATA"); } }
       
	 
    private departureArrivalFromDate: Date;
    public get DepartureArrivalFromDate() { return this.departureArrivalFromDate; }
    public set DepartureArrivalFromDate(newValue: Date) { if (this.departureArrivalFromDate != newValue) { this.departureArrivalFromDate = newValue; this.MarkAsDirty("DepartureArrivalFromDate"); } }
       
	 
    private departureArrivalToDate: Date;
    public get DepartureArrivalToDate() { return this.departureArrivalToDate; }
    public set DepartureArrivalToDate(newValue: Date) { if (this.departureArrivalToDate != newValue) { this.departureArrivalToDate = newValue; this.MarkAsDirty("DepartureArrivalToDate"); } }
       
	 
    private firstPickupLocation: string;
    public get FirstPickupLocation() { return this.firstPickupLocation; }
    public set FirstPickupLocation(newValue: string) { if (this.firstPickupLocation != newValue) { this.firstPickupLocation = newValue; this.MarkAsDirty("FirstPickupLocation"); } }
       
	 
    private finalDeliveryLocation: string;
    public get FinalDeliveryLocation() { return this.finalDeliveryLocation; }
    public set FinalDeliveryLocation(newValue: string) { if (this.finalDeliveryLocation != newValue) { this.finalDeliveryLocation = newValue; this.MarkAsDirty("FinalDeliveryLocation"); } }
       
	 
    private isImporterShipment: boolean;
    public get IsImporterShipment() { return this.isImporterShipment; }
    public set IsImporterShipment(newValue: boolean) { if (this.isImporterShipment != newValue) { this.isImporterShipment = newValue; this.MarkAsDirty("IsImporterShipment"); } }
       
	 
    private isUpdatedByAnalyzer: boolean;
    public get IsUpdatedByAnalyzer() { return this.isUpdatedByAnalyzer; }
    public set IsUpdatedByAnalyzer(newValue: boolean) { if (this.isUpdatedByAnalyzer != newValue) { this.isUpdatedByAnalyzer = newValue; this.MarkAsDirty("IsUpdatedByAnalyzer"); } }
       
	 
    private isCreatedFromCustomerOverview: boolean;
    public get IsCreatedFromCustomerOverview() { return this.isCreatedFromCustomerOverview; }
    public set IsCreatedFromCustomerOverview(newValue: boolean) { if (this.isCreatedFromCustomerOverview != newValue) { this.isCreatedFromCustomerOverview = newValue; this.MarkAsDirty("IsCreatedFromCustomerOverview"); } }
       
	 
    private declarationXMLData: string;
    public get DeclarationXMLData() { return this.declarationXMLData; }
    public set DeclarationXMLData(newValue: string) { if (this.declarationXMLData != newValue) { this.declarationXMLData = newValue; this.MarkAsDirty("DeclarationXMLData"); } }

    private documentInspection: Date;
    public get DocumentInspection() { return this.documentInspection; }
    public set DocumentInspection(newValue: Date) { if (this.documentInspection != newValue) { this.documentInspection = newValue; this.MarkAsDirty("DocumentInspection"); } }

    private gatepassDocumentsReady: Date;
    public get GatepassDocumentsReady() { return this.gatepassDocumentsReady; }
    public set GatepassDocumentsReady(newValue: Date) { if (this.gatepassDocumentsReady != newValue) { this.gatepassDocumentsReady = newValue; this.MarkAsDirty("GatepassDocumentsReady"); } }

    private goodsClassification: Date;
    public get GoodsClassification() { return this.goodsClassification; }
    public set GoodsClassification(newValue: Date) { if (this.goodsClassification != newValue) { this.goodsClassification = newValue; this.MarkAsDirty("GoodsClassification"); } }

    private isImporterApprovalRequired: boolean;
    public get IsImporterApprovalRequired() { return this.isImporterApprovalRequired; }
    public set IsImporterApprovalRequired(newValue: boolean) { if (this.isImporterApprovalRequired != newValue) { this.isImporterApprovalRequired = newValue; this.MarkAsDirty("IsImporterApprovalRequired"); } }

    private docsSentToAgent: boolean;
    public get DocsSentToAgent() { return this.docsSentToAgent; }
    public set DocsSentToAgent(newValue: boolean) { if (this.docsSentToAgent != newValue) { this.docsSentToAgent = newValue; this.MarkAsDirty("DocsSentToAgent"); } }

	 
    private versionApproved: string;
    public get VersionApproved() { return this.versionApproved; }
    public set VersionApproved(newValue: string) { if (this.versionApproved != newValue) { this.versionApproved = newValue; this.MarkAsDirty("VersionApproved"); } }
       
	 
    private approveDateTime: Date;
    public get ApproveDateTime() { return this.approveDateTime; }
    public set ApproveDateTime(newValue: Date) { if (this.approveDateTime != newValue) { this.approveDateTime = newValue; this.MarkAsDirty("ApproveDateTime"); } }
       
	 
    private isNewARInvoiceBlocked: boolean;
    public get IsNewARInvoiceBlocked() { return this.isNewARInvoiceBlocked; }
    public set IsNewARInvoiceBlocked(newValue: boolean) { if (this.isNewARInvoiceBlocked != newValue) { this.isNewARInvoiceBlocked = newValue; this.MarkAsDirty("IsNewARInvoiceBlocked"); } }
       
	 
    private shipmentAddtionalDataXML: string;
    public get ShipmentAddtionalDataXML() { return this.shipmentAddtionalDataXML; }
    public set ShipmentAddtionalDataXML(newValue: string) { if (this.shipmentAddtionalDataXML != newValue) { this.shipmentAddtionalDataXML = newValue; this.MarkAsDirty("ShipmentAddtionalDataXML"); } }


    private shipmentAdditionalData: ShipmentAdditionalData;
    public get ShipmentAdditionalData() { return this.shipmentAdditionalData; }
    public set ShipmentAdditionalData(newValue: ShipmentAdditionalData) { if (this.shipmentAdditionalData != newValue) { this.shipmentAdditionalData = newValue; this.MarkAsDirty("ShipmentAdditionalData"); } }

	 
    private originShipmentId: string;
    public get OriginShipmentId() { return this.originShipmentId; }
    public set OriginShipmentId(newValue: string) { if (this.originShipmentId != newValue) { this.originShipmentId = newValue; this.MarkAsDirty("OriginShipmentId"); } }
       
	 
    private prorateReceivables: boolean;
    public get ProrateReceivables() { return this.prorateReceivables; }
    public set ProrateReceivables(newValue: boolean) { if (this.prorateReceivables != newValue) { this.prorateReceivables = newValue; this.MarkAsDirty("ProrateReceivables"); } }
       
	 
    private freightRelease: Date;
    public get FreightRelease() { return this.freightRelease; }
    public set FreightRelease(newValue: Date) { if (this.freightRelease != newValue) { this.freightRelease = newValue; this.MarkAsDirty("FreightRelease"); } }
       
	 
    private terminalAvailable: Date;
    public get TerminalAvailable() { return this.terminalAvailable; }
    public set TerminalAvailable(newValue: Date) { if (this.terminalAvailable != newValue) { this.terminalAvailable = newValue; this.MarkAsDirty("TerminalAvailable"); } }
       
    private terminal2Available: Date;
    public get Terminal2Available() { return this.terminal2Available; }
    public set Terminal2Available(newValue: Date) { if (this.terminal2Available != newValue) { this.terminal2Available = newValue; this.MarkAsDirty("Terminal2Available"); } }

    private iSFNumber: string;
    public get ISFNumber() { return this.iSFNumber; }
    public set ISFNumber(newValue: string) { if (this.iSFNumber != newValue) { this.iSFNumber = newValue; this.MarkAsDirty("ISFNumber"); } }
       
	 
    private iSFDate: Date;
    public get ISFDate() { return this.iSFDate; }
    public set ISFDate(newValue: Date) { if (this.iSFDate != newValue) { this.iSFDate = newValue; this.MarkAsDirty("ISFDate"); } }
       
	 
    private iTNumber: string;
    public get ITNumber() { return this.iTNumber; }
    public set ITNumber(newValue: string) { if (this.iTNumber != newValue) { this.iTNumber = newValue; this.MarkAsDirty("ITNumber"); } }
       
	 
    private iTDate: Date;
    public get ITDate() { return this.iTDate; }
    public set ITDate(newValue: Date) { if (this.iTDate != newValue) { this.iTDate = newValue; this.MarkAsDirty("ITDate"); } }
       
	 
    private documentsClosingDate: Date;
    public get DocumentsClosingDate() { return this.documentsClosingDate; }
    public set DocumentsClosingDate(newValue: Date) { if (this.documentsClosingDate != newValue) { this.documentsClosingDate = newValue; this.MarkAsDirty("DocumentsClosingDate"); } }
       
	 
    private oBLTypeCode: string;
    public get OBLTypeCode() { return this.oBLTypeCode; }
    public set OBLTypeCode(newValue: string) { if (this.oBLTypeCode != newValue) { this.oBLTypeCode = newValue; this.MarkAsDirty("OBLTypeCode"); } }
       
	 
    private eNSNumber: string;
    public get ENSNumber() { return this.eNSNumber; }
    public set ENSNumber(newValue: string) { if (this.eNSNumber != newValue) { this.eNSNumber = newValue; this.MarkAsDirty("ENSNumber"); } }
       
	 
    private eNSDate: Date;
    public get ENSDate() { return this.eNSDate; }
    public set ENSDate(newValue: Date) { if (this.eNSDate != newValue) { this.eNSDate = newValue; this.MarkAsDirty("ENSDate"); } }
       
	 
    private registryDate: Date;
    public get RegistryDate() { return this.registryDate; }
    public set RegistryDate(newValue: Date) { if (this.registryDate != newValue) { this.registryDate = newValue; this.MarkAsDirty("RegistryDate"); } }


    private warehouseLegExpectedEntryDate: Date;
    public get WarehouseLegExpectedEntryDate() { return this.warehouseLegExpectedEntryDate; }
    public set WarehouseLegExpectedEntryDate(newValue: Date) { this.warehouseLegExpectedEntryDate = newValue; this.MarkAsDirty("WarehouseLegExpectedEntryDate"); }

    private warehouseLegActualEntryDate: Date;
    public get WarehouseLegActualEntryDate() { return this.warehouseLegActualEntryDate; }
    public set WarehouseLegActualEntryDate(newValue: Date) { this.warehouseLegActualEntryDate = newValue; this.MarkAsDirty("WarehouseLegActualEntryDate"); }

    private warehouseLegExpectedReleaseDate: Date;
    public get WarehouseLegExpectedReleaseDate() { return this.warehouseLegExpectedReleaseDate; }
    public set WarehouseLegExpectedReleaseDate(newValue: Date) { this.warehouseLegExpectedReleaseDate = newValue; this.MarkAsDirty("WarehouseLegExpectedReleaseDate"); }

    private warehouseLegActualReleaseDate: Date;
    public get WarehouseLegActualReleaseDate() { return this.warehouseLegActualReleaseDate; }
    public set WarehouseLegActualReleaseDate(newValue: Date) { this.warehouseLegActualReleaseDate = newValue; this.MarkAsDirty("WarehouseLegActualReleaseDate"); }

    private warehouseLegLastFreeDate: Date;
    public get WarehouseLegLastFreeDate() { return this.warehouseLegLastFreeDate; }
    public set WarehouseLegLastFreeDate(newValue: Date) { this.warehouseLegLastFreeDate = newValue; this.MarkAsDirty("WarehouseLegLastFreeDate"); }

    private warehouseLegWarehouseId: string;
    public get WarehouseLegWarehouseId() { return this.warehouseLegWarehouseId; }
    public set WarehouseLegWarehouseId(newValue: string) { this.warehouseLegWarehouseId = newValue; this.MarkAsDirty(); }

    private isUpdateWarehouseLegData: boolean;
    public get IsUpdateWarehouseLegData() { return this.isUpdateWarehouseLegData; }
    public set IsUpdateWarehouseLegData(newValue: boolean) { this.isUpdateWarehouseLegData = newValue; this.MarkAsDirty("IsUpdateWarehouseLegData"); }

    private warehouseLegAddressId: string;
    public get WarehouseLegAddressId() { return this.warehouseLegAddressId; }
    public set WarehouseLegAddressId(newValue: string) { this.warehouseLegAddressId = newValue; this.MarkAsDirty(); }

    private warehouseLegRemarks: string;
    public get WarehouseLegRemarks() { return this.warehouseLegRemarks; }
    public set WarehouseLegRemarks(newValue: string) { this.warehouseLegRemarks = newValue; this.MarkAsDirty(); }

    private warehouseLegTerminalCode: string;
    public get WarehouseLegTerminalCode() { return this.warehouseLegTerminalCode; }
    public set WarehouseLegTerminalCode(newValue: string) { this.warehouseLegTerminalCode = newValue; this.MarkAsDirty(); }

    private warehouseLegReference: string;
    public get WarehouseLegReference() { return this.warehouseLegReference; }
    public set WarehouseLegReference(newValue: string) { this.warehouseLegReference = newValue; this.MarkAsDirty(); }

    private warehouseLegAddressCountryName: string;
    public get WarehouseLegAddressCountryName() { return this.warehouseLegAddressCountryName; }
    public set WarehouseLegAddressCountryName(newValue: string) { this.warehouseLegAddressCountryName = newValue; this.MarkAsDirty(); }

    private warehouseLegAddressCountryCode: string;
    public get WarehouseLegAddressCountryCode() { return this.warehouseLegAddressCountryCode; }
    public set WarehouseLegAddressCountryCode(newValue: string) { this.warehouseLegAddressCountryCode = newValue; this.MarkAsDirty(); }
    private warehouseLeg2ExpectedEntryDate: Date;
    public get WarehouseLeg2ExpectedEntryDate() { return this.warehouseLeg2ExpectedEntryDate; }
    public set WarehouseLeg2ExpectedEntryDate(newValue: Date) { this.warehouseLeg2ExpectedEntryDate = newValue; this.MarkAsDirty("WarehouseLeg2ExpectedEntryDate"); }

    private warehouseLeg2ActualEntryDate: Date;
    public get WarehouseLeg2ActualEntryDate() { return this.warehouseLeg2ActualEntryDate; }
    public set WarehouseLeg2ActualEntryDate(newValue: Date) { this.warehouseLeg2ActualEntryDate = newValue; this.MarkAsDirty("WarehouseLeg2ActualEntryDate"); }

    private warehouseLeg2ExpectedReleaseDate: Date;
    public get WarehouseLeg2ExpectedReleaseDate() { return this.warehouseLeg2ExpectedReleaseDate; }
    public set WarehouseLeg2ExpectedReleaseDate(newValue: Date) { this.warehouseLeg2ExpectedReleaseDate = newValue; this.MarkAsDirty("WarehouseLeg2ExpectedReleaseDate"); }

    private warehouseLeg2ActualReleaseDate: Date;
    public get WarehouseLeg2ActualReleaseDate() { return this.warehouseLeg2ActualReleaseDate; }
    public set WarehouseLeg2ActualReleaseDate(newValue: Date) { this.warehouseLeg2ActualReleaseDate = newValue; this.MarkAsDirty("WarehouseLeg2ActualReleaseDate"); }

    private warehouseLeg2WarehouseId: string;
    public get WarehouseLeg2WarehouseId() { return this.warehouseLeg2WarehouseId; }
    public set WarehouseLeg2WarehouseId(newValue: string) { this.warehouseLeg2WarehouseId = newValue; this.MarkAsDirty(); }

    private isUpdateWarehouseLeg2Data: boolean;
    public get IsUpdateWarehouseLeg2Data() { return this.isUpdateWarehouseLeg2Data; }
    public set IsUpdateWarehouseLeg2Data(newValue: boolean) { this.isUpdateWarehouseLeg2Data = newValue; this.MarkAsDirty("IsUpdateWarehouseLeg2Data"); }

    private warehouseLeg2AddressId: string;
    public get WarehouseLeg2AddressId() { return this.warehouseLeg2AddressId; }
    public set WarehouseLeg2AddressId(newValue: string) { this.warehouseLeg2AddressId = newValue; this.MarkAsDirty(); }

    private warehouseLeg2Remarks: string;
    public get WarehouseLeg2Remarks() { return this.warehouseLeg2Remarks; }
    public set WarehouseLeg2Remarks(newValue: string) { this.warehouseLeg2Remarks = newValue; this.MarkAsDirty(); }

    private warehouseLeg2TerminalCode: string;
    public get WarehouseLeg2TerminalCode() { return this.warehouseLeg2TerminalCode; }
    public set WarehouseLeg2TerminalCode(newValue: string) { this.warehouseLeg2TerminalCode = newValue; this.MarkAsDirty(); }

    private warehouseLeg2Reference: string;
    public get WarehouseLeg2Reference() { return this.warehouseLeg2Reference; }
    public set WarehouseLeg2Reference(newValue: string) { this.warehouseLeg2Reference = newValue; this.MarkAsDirty(); }

    private warehouseLeg2AddressCountryName: string;
    public get WarehouseLeg2AddressCountryName() { return this.warehouseLeg2AddressCountryName; }
    public set WarehouseLeg2AddressCountryName(newValue: string) { this.warehouseLeg2AddressCountryName = newValue; this.MarkAsDirty(); }

    private warehouseLeg2AddressCountryCode: string;
    public get WarehouseLeg2AddressCountryCode() { return this.warehouseLeg2AddressCountryCode; }
    public set WarehouseLeg2AddressCountryCode(newValue: string) { this.warehouseLeg2AddressCountryCode = newValue; this.MarkAsDirty(); }

    private isAssembly: boolean;
    public get IsAssembly() { return this.isAssembly; }
    public set IsAssembly(newValue: boolean) { if (this.isAssembly != newValue) { this.isAssembly = newValue; this.MarkAsDirty("IsAssembly"); } }

    private lastSharedEventId: string;
    public get LastSharedEventId() { return this.lastSharedEventId; }
    public set LastSharedEventId(newValue: string) { if (this.lastSharedEventId != newValue) { this.lastSharedEventId = newValue; this.MarkAsDirty("LastSharedEventId"); } }

    private lastSharedEventLocation: string;
    public get LastSharedEventLocation() { return this.lastSharedEventLocation; }
    public set LastSharedEventLocation(newValue: string) { if (this.lastSharedEventLocation != newValue) { this.lastSharedEventLocation = newValue; this.MarkAsDirty("LastSharedEventLocation"); } }

    private lastSharedEventNotes: string;
    public get LastSharedEventNotes() { return this.lastSharedEventNotes; }
    public set LastSharedEventNotes(newValue: string) { if (this.lastSharedEventNotes != newValue) { this.lastSharedEventNotes = newValue; this.MarkAsDirty("LastSharedEventNotes"); } }

    private lastSharedEventDate: Date;
    public get LastSharedEventDate() { return this.lastSharedEventDate; }
    public set LastSharedEventDate(newValue: Date) { if (this.lastSharedEventDate != newValue) { this.lastSharedEventDate = newValue; this.MarkAsDirty("LastSharedEventDate"); } }

    private lastSharedEventName: string;
    public get LastSharedEventName() { return this.lastSharedEventName; }
    public set LastSharedEventName(newValue: string) { if (this.lastSharedEventName != newValue) { this.lastSharedEventName = newValue; this.MarkAsDirty("LastSharedEventName"); } }

    private grossWeightPerTon: number;
    public get GrossWeightPerTon() { return this.grossWeightPerTon; }
    public set GrossWeightPerTon(newValue: number) { if (this.grossWeightPerTon != newValue) { this.grossWeightPerTon = newValue; this.MarkAsDirty("GrossWeightPerTon"); } }


    //Abed 
    //properties withOut MarkAsDirty()
    //start
    private isRefreshFollowUp: boolean;
    public get IsRefreshFollowUp() { return this.isRefreshFollowUp; }
    public set IsRefreshFollowUp(newValue: boolean) { if (this.isRefreshFollowUp != newValue) { this.isRefreshFollowUp = newValue;; } }

    private isManifestSentToAgent: boolean;
    public get IsManifestSentToAgent() { return this.isManifestSentToAgent; }
    public set IsManifestSentToAgent(newValue: boolean) { if (this.isManifestSentToAgent != newValue) { this.isManifestSentToAgent = newValue; } }


    private agentSharedManifestRef: string;
    public get AgentSharedManifestRef() { return this.agentSharedManifestRef; }
    public set AgentSharedManifestRef(newValue: string) { if (this.agentSharedManifestRef != newValue) { this.agentSharedManifestRef = newValue; } }

    //end

    private warehouseLegTerminalName: string;
    public get WarehouseLegTerminalName() { return this.warehouseLegTerminalName; }
    public set WarehouseLegTerminalName(newValue: string) { this.warehouseLegTerminalName = newValue; this.MarkAsDirty(); }

    private warehouseLegEntryDate: Date;
    public get WarehouseLegEntryDate() { return this.warehouseLegEntryDate; }
    public set WarehouseLegEntryDate(newValue: Date) { this.warehouseLegEntryDate = newValue; this.MarkAsDirty(); }

    private warehouseLegReleaseDate: Date;
    public get WarehouseLegReleaseDate() { return this.warehouseLegReleaseDate; }
    public set WarehouseLegReleaseDate(newValue: Date) { this.warehouseLegReleaseDate = newValue; this.MarkAsDirty(); }

    private manifestLastSharingDate: Date;
    public get ManifestLastSharingDate() { return this.manifestLastSharingDate; }
    public set ManifestLastSharingDate(newValue: Date) { if (this.manifestLastSharingDate != newValue) { this.manifestLastSharingDate = newValue; this.MarkAsDirty("ManifestLastSharingDate"); } }

    private warehouseLegCutOffDate: Date;
    public get WarehouseLegCutOffDate() { return this.warehouseLegCutOffDate; }
    public set WarehouseLegCutOffDate(newValue: Date) { this.warehouseLegCutOffDate = newValue; this.MarkAsDirty("WarehouseLegCutOffDate"); }

    private warehouseLegVGMCutOffDate: Date;
    public get WarehouseLegVGMCutOffDate() { return this.warehouseLegVGMCutOffDate; }
    public set WarehouseLegVGMCutOffDate(newValue: Date) { this.warehouseLegVGMCutOffDate = newValue; this.MarkAsDirty("WarehouseLegVGMCutOffDate"); }

    private warehouseLeg2TerminalName: string;
    public get WarehouseLeg2TerminalName() { return this.warehouseLeg2TerminalName; }
    public set WarehouseLeg2TerminalName(newValue: string) { this.warehouseLeg2TerminalName = newValue; this.MarkAsDirty(); }

    private warehouseLeg2EntryDate: Date;
    public get WarehouseLeg2EntryDate() { return this.warehouseLeg2EntryDate; }
    public set WarehouseLeg2EntryDate(newValue: Date) { this.warehouseLeg2EntryDate = newValue; this.MarkAsDirty(); }

    private warehouseLeg2ReleaseDate: Date;
    public get WarehouseLeg2ReleaseDate() { return this.warehouseLeg2ReleaseDate; }
    public set WarehouseLeg2ReleaseDate(newValue: Date) { this.warehouseLeg2ReleaseDate = newValue; this.MarkAsDirty(); }

    private warehouseLeg2CutOffDate: Date;
    public get WarehouseLeg2CutOffDate() { return this.warehouseLeg2CutOffDate; }
    public set WarehouseLeg2CutOffDate(newValue: Date) { this.warehouseLeg2CutOffDate = newValue; this.MarkAsDirty("WarehouseLeg2CutOffDate"); }

    private warehouseLeg2VGMCutOffDate: Date;
    public get WarehouseLeg2VGMCutOffDate() { return this.warehouseLeg2VGMCutOffDate; }
    public set WarehouseLeg2VGMCutOffDate(newValue: Date) { this.warehouseLeg2VGMCutOffDate = newValue; this.MarkAsDirty("WarehouseLeg2VGMCutOffDate"); }

    private aMSClosingDate: Date;
    public get AMSClosingDate() { return this.aMSClosingDate; }
    public set AMSClosingDate(newValue: Date) { this.aMSClosingDate = newValue; this.MarkAsDirty("AMSClosingDate"); }

    private updatedByPartner: string;
    public get UpdatedByPartner() { return this.updatedByPartner; }
    public set UpdatedByPartner(newValue: string) { this.updatedByPartner = newValue; this.MarkAsDirty(); }

    private sendUpdatesToAgentEnabled: boolean;
    public get SendUpdatesToAgentEnabled() { return this.sendUpdatesToAgentEnabled; }
    public set SendUpdatesToAgentEnabled(newValue: boolean) { if (this.sendUpdatesToAgentEnabled != newValue) { this.sendUpdatesToAgentEnabled = newValue; } }

    private updateSendUpdatesToAgentEnabledField: boolean;
    public get UpdateSendUpdatesToAgentEnabledField() { return this.updateSendUpdatesToAgentEnabledField; }
    public set UpdateSendUpdatesToAgentEnabledField(newValue: boolean) { if (this.updateSendUpdatesToAgentEnabledField != newValue) { this.updateSendUpdatesToAgentEnabledField = newValue; } }

    private iNTTRASIError: string;
    public get INTTRASIError() { return this.iNTTRASIError; }
    public set INTTRASIError(newValue: string) { this.iNTTRASIError = newValue; this.MarkAsDirty(); }

    private iNTTRASIStatusCode: string;
    public get INTTRASIStatusCode() { return this.iNTTRASIStatusCode; }
    public set INTTRASIStatusCode(newValue: string) { this.iNTTRASIStatusCode = newValue; this.MarkAsDirty(); }

    private iNTTRASIStatusName: string;
    public get INTTRASIStatusName() { return this.iNTTRASIStatusName; }
    public set INTTRASIStatusName(newValue: string) { this.iNTTRASIStatusName = newValue; this.MarkAsDirty(); }


    private iNTTRABookingStatusCode: string;
    public get INTTRABookingStatusCode() { return this.iNTTRABookingStatusCode; }
    public set INTTRABookingStatusCode(newValue: string) { this.iNTTRABookingStatusCode = newValue; this.MarkAsDirty(); }

    private iNTTRABookingStatusName: string;
    public get INTTRABookingStatusName() { return this.iNTTRABookingStatusName; }
    public set INTTRABookingStatusName(newValue: string) { this.iNTTRABookingStatusName = newValue; this.MarkAsDirty(); }

    private iNTTRABookingTransStatusCode: string;
    public get INTTRABookingTransStatusCode() { return this.iNTTRABookingTransStatusCode; }
    public set INTTRABookingTransStatusCode(newValue: string) { this.iNTTRABookingTransStatusCode = newValue; this.MarkAsDirty(); }

    private iNTTRABookingTransStatusName: string;
    public get INTTRABookingTransStatusName() { return this.iNTTRABookingTransStatusName; }
    public set INTTRABookingTransStatusName(newValue: string) { this.iNTTRABookingTransStatusName = newValue; this.MarkAsDirty(); }


    private iNTTRASIStatusDate: string;
    public get INTTRASIStatusDate() { return this.iNTTRASIStatusDate; }
    public set INTTRASIStatusDate(newValue: string) { this.iNTTRASIStatusDate = newValue; this.MarkAsDirty(); }

    private emergencyContactId: string;
    public get EmergencyContactId() { return this.emergencyContactId; }
    public set EmergencyContactId(newValue: string) { this.emergencyContactId = newValue; this.MarkAsDirty(); }

    private iNTTRAContractNumber: string;
    public get INTTRAContractNumber() { return this.iNTTRAContractNumber; }
    public set INTTRAContractNumber(newValue: string) { this.iNTTRAContractNumber = newValue; this.MarkAsDirty(); }

    private iNTTRAInstructions: string;
    public get INTTRAInstructions() { return this.iNTTRAInstructions; }
    public set INTTRAInstructions(newValue: string) { this.iNTTRAInstructions = newValue; this.MarkAsDirty(); }

    private iNTTRAComments: string;
    public get INTTRAComments() { return this.iNTTRAComments; }
    public set INTTRAComments(newValue: string) { this.iNTTRAComments = newValue; this.MarkAsDirty(); }

    private onCarriageAdditionalTransportModeCode: string;
    public get OnCarriageAdditionalTransportModeCode() { return this.onCarriageAdditionalTransportModeCode; }
    public set OnCarriageAdditionalTransportModeCode(newValue: string) { this.onCarriageAdditionalTransportModeCode = newValue; this.MarkAsDirty(); }

    private iNTTRADocumentQTY: number;
    public get INTTRADocumentQTY() { return this.iNTTRADocumentQTY; }
    public set INTTRADocumentQTY(newValue: number) { this.iNTTRADocumentQTY = newValue; this.MarkAsDirty(); }

    private sIHasAttachList: boolean;
    public get SIHasAttachList() { return this.sIHasAttachList; }
    public set SIHasAttachList(newValue: boolean) { this.sIHasAttachList = newValue; this.MarkAsDirty(); }

    private iNTTRAIsFreighted: boolean;
    public get INTTRAIsFreighted() { return this.iNTTRAIsFreighted; }
    public set INTTRAIsFreighted(newValue: boolean) { this.iNTTRAIsFreighted = newValue; this.MarkAsDirty(); }

    private iNTTRADocumentTypeCode: string;
    public get INTTRADocumentTypeCode() { return this.iNTTRADocumentTypeCode; }
    public set INTTRADocumentTypeCode(newValue: string) { this.iNTTRADocumentTypeCode = newValue; this.MarkAsDirty(); }
    
    private splitOnCarriage: boolean;
    public get SplitOnCarriage() { return this.splitOnCarriage; }
    public set SplitOnCarriage(newValue: boolean) { if (this.splitOnCarriage != newValue) { this.splitOnCarriage = newValue; this.MarkAsDirty(); } }        

    private iNTTRALastStatusDate: Date;
    public get INTTRALastStatusDate() { return this.iNTTRALastStatusDate; }
    public set INTTRALastStatusDate(newValue: Date) { if (this.iNTTRALastStatusDate != newValue) { this.iNTTRALastStatusDate = newValue; this.MarkAsDirty("INTTRALastStatusDate"); } }

    private projectNumber: string;
    public get ProjectNumber() { return this.projectNumber; }
    public set ProjectNumber(newValue: string) { if (this.projectNumber != newValue) { this.projectNumber = newValue; this.MarkAsDirty("ProjectNumber"); } }



    private privateLabelInvoiceNumber: string;
    public get PrivateLabelInvoiceNumber() { return this.privateLabelInvoiceNumber; }
    public set PrivateLabelInvoiceNumber(newValue: string) { if (this.privateLabelInvoiceNumber != newValue) { this.privateLabelInvoiceNumber = newValue; this.MarkAsDirty(); } }

    private privateLabelIncludePickup: boolean;
    public get PrivateLabelIncludePickup() { return this.privateLabelIncludePickup; }
    public set PrivateLabelIncludePickup(newValue: boolean) { if (this.privateLabelIncludePickup != newValue) { this.privateLabelIncludePickup = newValue; this.MarkAsDirty(); } }

    private privateLabelIncludeDelivery: boolean;
    public get PrivateLabelIncludeDelivery() { return this.privateLabelIncludeDelivery; }
    public set PrivateLabelIncludeDelivery(newValue: boolean) { if (this.privateLabelIncludeDelivery != newValue) { this.privateLabelIncludeDelivery = newValue; this.MarkAsDirty(); } }

    private requestedFlightDate: Date;
    public get RequestedFlightDate() { return this.requestedFlightDate; }
    public set RequestedFlightDate(newValue: Date) { if (this.requestedFlightDate != newValue) { this.requestedFlightDate = newValue; this.MarkAsDirty(); } }

    private hasUnassignedData: boolean;
    public get HasUnassignedData() { return this.hasUnassignedData; }
    public set HasUnassignedData(newValue: boolean) { if (this.hasUnassignedData != newValue) { this.hasUnassignedData = newValue; this.MarkAsDirty("HasUnassignedData"); } }

    private privateLabelAgentName: string;
    public get PrivateLabelAgentName() { return this.privateLabelAgentName; }
    public set PrivateLabelAgentName(newValue: string) { if (this.privateLabelAgentName != newValue) { this.privateLabelAgentName = newValue; this.MarkAsDirty("PrivateLabelAgentName"); } }


    private notify1Reference: string;
    public get Notify1Reference() { return this.notify1Reference; }
    public set Notify1Reference(newValue: string) {
        if (this.notify1Reference != newValue) {
            this.notify1Reference = newValue;
            this.MarkAsDirty("Notify1Reference");
        }
    }

    private notify1Reference2: string;
    public get Notify1Reference2() { return this.notify1Reference2; }
    public set Notify1Reference2(newValue: string) {
        if (this.notify1Reference2 != newValue) {
            this.notify1Reference2 = newValue;
            this.MarkAsDirty("Notify1Reference2");
        }
    }


    private notify2Reference: string;
    public get Notify2Reference() { return this.notify2Reference; }
    public set Notify2Reference(newValue: string) {
        if (this.notify2Reference != newValue) {
            this.notify2Reference = newValue;
            this.MarkAsDirty("Notify2Reference");
        }
    }

    private warehouseStorageFreeDays: number;
    public get WarehouseStorageFreeDays() { return this.warehouseStorageFreeDays; }
    public set WarehouseStorageFreeDays(newValue: number) {
        if (this.warehouseStorageFreeDays != newValue) {
            this.warehouseStorageFreeDays = newValue;
            this.MarkAsDirty("WarehouseStorageFreeDays");
        }
    }

    private shipperNotExporterReference1: string;
    public get ShipperNotExporterReference1() { return this.shipperNotExporterReference1; }
    public set ShipperNotExporterReference1(newValue: string) {
        if (this.shipperNotExporterReference1 != newValue) {
            this.shipperNotExporterReference1 = newValue;
            this.MarkAsDirty("ShipperNotExporterReference1");
        }
    }

    private shipperNotExporterReference2: string;
    public get ShipperNotExporterReference2() { return this.shipperNotExporterReference2; }
    public set ShipperNotExporterReference2(newValue: string) {
        if (this.shipperNotExporterReference2 != newValue) {
            this.shipperNotExporterReference2 = newValue;
            this.MarkAsDirty("ShipperNotExporterReference2");
        }
    }

    private consigneeNotImporterReference: string;
    public get ConsigneeNotImporterReference() { return this.consigneeNotImporterReference; }
    public set ConsigneeNotImporterReference(newValue: string) {
        if (this.consigneeNotImporterReference != newValue) {
            this.consigneeNotImporterReference = newValue;
            this.MarkAsDirty("ConsigneeNotImporterReference");
        }
    }

    private basicFreightId: string;
    public get BasicFreightId() { return this.basicFreightId; }
    public set BasicFreightId(newValue: string) {
        if (this.basicFreightId != newValue) {
            this.basicFreightId = newValue;
            this.MarkAsDirty("BasicFreightId");
        }
    }

    private destinationPortChargesId: string;
    public get DestinationPortChargesId() { return this.destinationPortChargesId; }
    public set DestinationPortChargesId(newValue: string) {
        if (this.destinationPortChargesId != newValue) {
            this.destinationPortChargesId = newValue;
            this.MarkAsDirty("DestinationPortChargesId");
        }
    }

    private destinationHaulageChargesId: string;
    public get DestinationHaulageChargesId() { return this.destinationHaulageChargesId; }
    public set DestinationHaulageChargesId(newValue: string) {
        if (this.destinationHaulageChargesId != newValue) {
            this.destinationHaulageChargesId = newValue;
            this.MarkAsDirty("DestinationHaulageChargesId");
        }
    }

    private additionalChargesId: string;
    public get AdditionalChargesId() { return this.additionalChargesId; }
    public set AdditionalChargesId(newValue: string) {
        if (this.additionalChargesId != newValue) {
            this.additionalChargesId = newValue;
            this.MarkAsDirty("AdditionalChargesId");
        }
    }

    private freightPayerId: string;
    public get FreightPayerId() { return this.freightPayerId; }
    public set FreightPayerId(newValue: string) {
        if (this.freightPayerId != newValue) {
            this.freightPayerId = newValue;
            this.MarkAsDirty("FreightPayerId");
        }
    }

    private freightPayerAddressId: string;
    public get FreightPayerAddressId() { return this.freightPayerAddressId; }
    public set FreightPayerAddressId(newValue: string) {
        if (this.freightPayerAddressId != newValue) {
            this.freightPayerAddressId = newValue;
            this.MarkAsDirty("FreightPayerAddressId");
        }
    }

    private hasContainerException: boolean;
    public get HasContainerException() { return this.hasContainerException; }
    public set HasContainerException(newValue: boolean) {
        if (this.hasContainerException != newValue) {
            this.hasContainerException = newValue;
            this.MarkAsDirty("HasContainerException");
        }
    }


    private originMainCarriageFromPortId: string;
    public get OriginMainCarriageFromPortId() { return this.originMainCarriageFromPortId; }
    public set OriginMainCarriageFromPortId(newValue: string) {
        if (this.originMainCarriageFromPortId != newValue) {
            this.originMainCarriageFromPortId = newValue;
            this.MarkAsDirty("OriginMainCarriageFromPortId");
        }
    }

    private originFinalDestinationPortId: string;
    public get OriginFinalDestinationPortId() { return this.originFinalDestinationPortId; }
    public set OriginFinalDestinationPortId(newValue: string) {
        if (this.originFinalDestinationPortId != newValue) {
            this.originFinalDestinationPortId = newValue;
            this.MarkAsDirty("HasContainerException");
        }
    }

    private aRInvoices: string;
    public get ARInvoices() { return this.aRInvoices; }
    public set ARInvoices(newValue: string) {
        if (this.aRInvoices != newValue) {
            this.aRInvoices = newValue;
            this.MarkAsDirty("ARInvoices");
        }
    }

    private masterCreatedFromHouseId: string;
    public get MasterCreatedFromHouseId() { return this.masterCreatedFromHouseId; }
    public set MasterCreatedFromHouseId(newValue: string) {
        if (this.masterCreatedFromHouseId != newValue) {
            this.masterCreatedFromHouseId = newValue;
            this.MarkAsDirty("MasterCreatedFromHouseId");
        }
    }

    private convertShipmentToLCL: boolean;
    public get ConvertShipmentToLCL() { return this.convertShipmentToLCL; }
    public set ConvertShipmentToLCL(newValue: boolean) { if (this.convertShipmentToLCL != newValue) { this.convertShipmentToLCL = newValue; this.MarkAsDirty("ConvertShipmentToLCL"); } }
    
    private convertShipmentToFCL: boolean;
    public get ConvertShipmentToFCL() { return this.convertShipmentToFCL; }
    public set ConvertShipmentToFCL(newValue: boolean) { if (this.convertShipmentToFCL != newValue) { this.convertShipmentToFCL = newValue; this.MarkAsDirty("ConvertShipmentToFCL"); } }

    private convertShipmentToLTL: boolean;
    public get ConvertShipmentToLTL() { return this.convertShipmentToLTL; }
    public set ConvertShipmentToLTL(newValue: boolean) { if (this.convertShipmentToLTL != newValue) { this.convertShipmentToLTL = newValue; this.MarkAsDirty("ConvertShipmentToLTL"); } }

    private convertShipmentToFTL: boolean;
    public get ConvertShipmentToFTL() { return this.convertShipmentToFTL; }
    public set ConvertShipmentToFTL(newValue: boolean) { if (this.convertShipmentToFTL != newValue) { this.convertShipmentToFTL = newValue; this.MarkAsDirty("ConvertShipmentToFTL"); } }


    private shipmentDirectionConverted: boolean;
    public get ShipmentDirectionConverted() { return this.shipmentDirectionConverted; }
    public set ShipmentDirectionConverted(newValue: boolean) { if (this.shipmentDirectionConverted != newValue) { this.shipmentDirectionConverted = newValue; this.MarkAsDirty("ShipmentDirectionConverted"); } }

    private shipmentConvertedNewNumber: boolean;
    public get ShipmentConvertedNewNumber() { return this.shipmentConvertedNewNumber; }
    public set ShipmentConvertedNewNumber(newValue: boolean) { if (this.shipmentConvertedNewNumber != newValue) { this.shipmentConvertedNewNumber = newValue; this.MarkAsDirty("ShipmentConvertedNewNumber"); } }

    private packagesDeleted: boolean;
    public get PackagesDeleted() { return this.packagesDeleted; }
    public set PackagesDeleted(newValue: boolean) { if (this.packagesDeleted != newValue) { this.packagesDeleted = newValue; this.MarkAsDirty("PackagesDeleted"); } }

    private isDeletingAllPayables: boolean;
    public get IsDeletingAllPayables() { return this.isDeletingAllPayables; }
    public set IsDeletingAllPayables(newValue: boolean) { if (this.isDeletingAllPayables != newValue) { this.isDeletingAllPayables = newValue; this.MarkAsDirty("IsDeletingAllPayables"); } }


    private iNTTRABookingResponse_Voyage: string;
    public get INTTRABookingResponse_Voyage() { return this.iNTTRABookingResponse_Voyage; }
    public set INTTRABookingResponse_Voyage(newValue: string) { if (this.iNTTRABookingResponse_Voyage != newValue) { this.iNTTRABookingResponse_Voyage = newValue; this.MarkAsDirty("INTTRABookingResponse_Voyage"); } }

    private iNTTRABookingResponse_Vessel: string;
    public get INTTRABookingResponse_Vessel() { return this.iNTTRABookingResponse_Vessel; }
    public set INTTRABookingResponse_Vessel(newValue: string) { if (this.iNTTRABookingResponse_Vessel != newValue) { this.iNTTRABookingResponse_Vessel = newValue; this.MarkAsDirty("INTTRABookingResponse_Vessel"); } }

    private iNTTRABookingResponse_VesselId: string;
    public get INTTRABookingResponse_VesselId() { return this.iNTTRABookingResponse_VesselId; }
    public set INTTRABookingResponse_VesselId(newValue: string) { if (this.iNTTRABookingResponse_VesselId != newValue) { this.iNTTRABookingResponse_VesselId = newValue; this.MarkAsDirty("INTTRABookingResponse_VesselId"); } }


    private iNTTRABookingResponse_POLDate: Date;
    public get INTTRABookingResponse_POLDate() { return this.iNTTRABookingResponse_POLDate; }
    public set INTTRABookingResponse_POLDate(newValue: Date) { if (this.iNTTRABookingResponse_POLDate != newValue) { this.iNTTRABookingResponse_POLDate = newValue; this.MarkAsDirty("INTTRABookingResponse_POLDate"); } }


    private iNTTRABookingResponse_POFPort: string;
    public get INTTRABookingResponse_POFPort() { return this.iNTTRABookingResponse_POFPort; }
    public set INTTRABookingResponse_POFPort(newValue: string) { if (this.iNTTRABookingResponse_POFPort != newValue) { this.iNTTRABookingResponse_POFPort = newValue; this.MarkAsDirty("INTTRABookingResponse_POFPort"); } }

    private iNTTRABookingResponse_POFCCode: string;
    public get INTTRABookingResponse_POFCCode() { return this.iNTTRABookingResponse_POFCCode; }
    public set INTTRABookingResponse_POFCCode(newValue: string) { if (this.iNTTRABookingResponse_POFCCode != newValue) { this.iNTTRABookingResponse_POFCCode = newValue; this.MarkAsDirty("INTTRABookingResponse_POFCCode"); } }

    private iNTTRABookingResponse_POFCName: string;
    public get INTTRABookingResponse_POFCName() { return this.iNTTRABookingResponse_POFCName; }
    public set INTTRABookingResponse_POFCName(newValue: string) { if (this.iNTTRABookingResponse_POFCName != newValue) { this.iNTTRABookingResponse_POFCName = newValue; this.MarkAsDirty("INTTRABookingResponse_POFCName"); } }

    private iNTTRABookingResponse_PODDate: Date;
    public get INTTRABookingResponse_PODDate() { return this.iNTTRABookingResponse_PODDate; }
    public set INTTRABookingResponse_PODDate(newValue: Date) { if (this.iNTTRABookingResponse_PODDate != newValue) { this.iNTTRABookingResponse_PODDate = newValue; this.MarkAsDirty("INTTRABookingResponse_PODDate"); } }

    private iNTTRABookingResponse_PODPort: string;
    public get INTTRABookingResponse_PODPort() { return this.iNTTRABookingResponse_PODPort; }
    public set INTTRABookingResponse_PODPort(newValue: string) { if (this.iNTTRABookingResponse_PODPort != newValue) { this.iNTTRABookingResponse_PODPort = newValue; this.MarkAsDirty("INTTRABookingResponse_PODPort"); } }

    private iNTTRABookingResponse_PODCCode: string;
    public get INTTRABookingResponse_PODCCode() { return this.iNTTRABookingResponse_PODCCode; }
    public set INTTRABookingResponse_PODCCode(newValue: string) { if (this.iNTTRABookingResponse_PODCCode != newValue) { this.iNTTRABookingResponse_PODCCode = newValue; this.MarkAsDirty("INTTRABookingResponse_PODCCode"); } }

    private iNTTRABookingResponse_PODCName: string;
    public get INTTRABookingResponse_PODCName() { return this.iNTTRABookingResponse_PODCName; }
    public set INTTRABookingResponse_PODCName(newValue: string) { if (this.iNTTRABookingResponse_PODCName != newValue) { this.iNTTRABookingResponse_PODCName = newValue; this.MarkAsDirty("INTTRABookingResponse_PODCName"); } }


    private iNTTRABookingResponse_POFPortCode: string;
    public get INTTRABookingResponse_POFPortCode() { return this.iNTTRABookingResponse_POFPortCode; }
    public set INTTRABookingResponse_POFPortCode(newValue: string) { if (this.iNTTRABookingResponse_POFPortCode != newValue) { this.iNTTRABookingResponse_POFPortCode = newValue; this.MarkAsDirty("INTTRABookingResponse_POFPortCode"); } }

    private iNTTRABookingResponse_PODPortCode: string;
    public get INTTRABookingResponse_PODPortCode() { return this.iNTTRABookingResponse_PODPortCode; }
    public set INTTRABookingResponse_PODPortCode(newValue: string) { if (this.iNTTRABookingResponse_PODPortCode != newValue) { this.iNTTRABookingResponse_PODPortCode = newValue; this.MarkAsDirty("INTTRABookingResponse_PODPortCode"); } }

    private iNTTRABookingResponse_ShippingLine: string;
    public get INTTRABookingResponse_ShippingLine() { return this.iNTTRABookingResponse_ShippingLine; }
    public set INTTRABookingResponse_ShippingLine(newValue: string) { if (this.iNTTRABookingResponse_ShippingLine != newValue) { this.iNTTRABookingResponse_ShippingLine = newValue; this.MarkAsDirty("INTTRABookingResponse_ShippingLine"); } }

    private fWBStatusCode_Original: string;
    public get FWBStatusCode_Original() { return this.fWBStatusCode_Original; }
    public set FWBStatusCode_Original(newValue: string) { if (this.fWBStatusCode_Original != newValue) { this.fWBStatusCode_Original = newValue; this.MarkAsDirty("FWBStatusCode_Original"); } }

    private fHLStatusCode_Original: string;
    public get FHLStatusCode_Original() { return this.fHLStatusCode_Original; }
    public set FHLStatusCode_Original(newValue: string) { if (this.fHLStatusCode_Original != newValue) { this.fHLStatusCode_Original = newValue; this.MarkAsDirty("FHLStatusCode_Original"); } }

    private fHLStatusDate_Original: Date;
    public get FHLStatusDate_Original() { return this.fHLStatusDate_Original; }
    public set FHLStatusDate_Original(newValue: Date) { if (this.fHLStatusDate_Original != newValue) { this.fHLStatusDate_Original = newValue; this.MarkAsDirty("FHLStatusDate_Original"); } }

    private fWBStatusDate_Original: Date;
    public get FWBStatusDate_Original() { return this.fWBStatusDate_Original; }
    public set FWBStatusDate_Original(newValue: Date) { if (this.fWBStatusDate_Original != newValue) { this.fWBStatusDate_Original = newValue; this.MarkAsDirty("FWBStatusDate_Original"); } }

    private carrierLastStatusCode_Original: string;
    public get CarrierLastStatusCode_Original() { return this.carrierLastStatusCode_Original; }
    public set CarrierLastStatusCode_Original(newValue: string) { if (this.carrierLastStatusCode_Original != newValue) { this.carrierLastStatusCode_Original = newValue; this.MarkAsDirty("CarrierLastStatusCode_Original"); } }

    private carrierLastStatusDate_Original: Date;
    public get CarrierLastStatusDate_Original() { return this.carrierLastStatusDate_Original; }
    public set CarrierLastStatusDate_Original(newValue: Date) { if (this.carrierLastStatusDate_Original != newValue) { this.carrierLastStatusDate_Original = newValue; this.MarkAsDirty("CarrierLastStatusDate_Original"); } }

    private mainCarriageToPortId_Original: string;
    public get MainCarriageToPortId_Original() { return this.mainCarriageToPortId_Original; }
    public set MainCarriageToPortId_Original(newValue: string) { if (this.mainCarriageToPortId_Original != newValue) { this.mainCarriageToPortId_Original = newValue; this.MarkAsDirty("MainCarriageToPortId_Original"); } }

    private numberOfPackages_Original: number;
    public get NumberOfPackages_Original() { return this.numberOfPackages_Original; }
    public set NumberOfPackages_Original(newValue: number) { if (this.numberOfPackages_Original != newValue) { this.numberOfPackages_Original = newValue; this.MarkAsDirty("NumberOfPackages_Original"); } }

    private chargeableWeight_Original: number;
    public get ChargeableWeight_Original() { return this.chargeableWeight_Original; }
    public set ChargeableWeight_Original(newValue: number) { if (this.chargeableWeight_Original != newValue) { this.chargeableWeight_Original = newValue; this.MarkAsDirty("ChargeableWeight_Original"); } }

    private grossWeight_Original: number;
    public get GrossWeight_Original() { return this.grossWeight_Original; }
    public set GrossWeight_Original(newValue: number) { if (this.grossWeight_Original != newValue) { this.grossWeight_Original = newValue; this.MarkAsDirty("GrossWeight_Original"); } }

    private grossWeightUnitCode_Original: string;
    public get GrossWeightUnitCode_Original() { return this.grossWeightUnitCode_Original; }
    public set GrossWeightUnitCode_Original(newValue: string) { if (this.grossWeightUnitCode_Original != newValue) { this.grossWeightUnitCode_Original = newValue; this.MarkAsDirty("GrossWeightUnitCode_Original"); } }

    private mainCarriageATD_Original: Date;
    public get MainCarriageATD_Original() { return this.mainCarriageATD_Original; }
    public set MainCarriageATD_Original(newValue: Date) { if (this.mainCarriageATD_Original != newValue) { this.mainCarriageATD_Original = newValue; this.MarkAsDirty("MainCarriageATD_Original"); } }


    private mainCarriageATA_Original: Date;
    public get MainCarriageATA_Original() { return this.mainCarriageATA_Original; }
    public set MainCarriageATA_Original(newValue: Date) { if (this.mainCarriageATA_Original != newValue) { this.mainCarriageATA_Original = newValue; this.MarkAsDirty("MainCarriageATA_Original"); } }


    private mainCarriageETD_Original: Date;
    public get MainCarriageETD_Original() { return this.mainCarriageETD_Original; }
    public set MainCarriageETD_Original(newValue: Date) { if (this.mainCarriageETD_Original != newValue) { this.mainCarriageETD_Original = newValue; this.MarkAsDirty("MainCarriageETD_Original"); } }


    private mainCarriageETA_Original: Date;
    public get MainCarriageETA_Original() { return this.mainCarriageETA_Original; }
    public set MainCarriageETA_Original(newValue: Date) { if (this.mainCarriageETA_Original != newValue) { this.mainCarriageETA_Original = newValue; this.MarkAsDirty("MainCarriageETA_Original"); } }

    private mainCarriageSTD_Original: Date;
    public get MainCarriageSTD_Original() { return this.mainCarriageSTD_Original; }
    public set MainCarriageSTD_Original(newValue: Date) { if (this.mainCarriageSTD_Original != newValue) { this.mainCarriageSTD_Original = newValue; this.MarkAsDirty("MainCarriageSTD_Original"); } }

    private mainCarriageSTA_Original: Date;
    public get MainCarriageSTA_Original() { return this.mainCarriageSTA_Original; }
    public set MainCarriageSTA_Original(newValue: Date) { if (this.mainCarriageSTA_Original != newValue) { this.mainCarriageSTA_Original = newValue; this.MarkAsDirty("MainCarriageSTA_Original"); } }

    private transshipment1ATD_Original: Date;
    public get Transshipment1ATD_Original() { return this.transshipment1ATD_Original; }
    public set Transshipment1ATD_Original(newValue: Date) { if (this.transshipment1ATD_Original != newValue) { this.transshipment1ATD_Original = newValue; this.MarkAsDirty("Transshipment1ATD_Original"); } }


    private transshipment1ATA_Original: Date;
    public get Transshipment1ATA_Original() { return this.transshipment1ATA_Original; }
    public set Transshipment1ATA_Original(newValue: Date) { if (this.transshipment1ATA_Original != newValue) { this.transshipment1ATA_Original = newValue; this.MarkAsDirty("Transshipment1ATA_Original"); } }


    private transshipment1ETD_Original: Date;
    public get Transshipment1ETD_Original() { return this.transshipment1ETD_Original; }
    public set Transshipment1ETD_Original(newValue: Date) { if (this.transshipment1ETD_Original != newValue) { this.transshipment1ETD_Original = newValue; this.MarkAsDirty("Transshipment1ETD_Original"); } }


    private transshipment1ETA_Original: Date;
    public get Transshipment1ETA_Original() { return this.transshipment1ETA_Original; }
    public set Transshipment1ETA_Original(newValue: Date) { if (this.transshipment1ETA_Original != newValue) { this.transshipment1ETA_Original = newValue; this.MarkAsDirty("Transshipment1ETA_Original"); } }

    private transshipment1STD_Original: Date;
    public get Transshipment1STD_Original() { return this.transshipment1STD_Original; }
    public set Transshipment1STD_Original(newValue: Date) { if (this.transshipment1STD_Original != newValue) { this.transshipment1STD_Original = newValue; this.MarkAsDirty("Transshipment1STD_Original"); } }


    private transshipment1STA_Original: Date;
    public get Transshipment1STA_Original() { return this.transshipment1STA_Original; }
    public set Transshipment1STA_Original(newValue: Date) { if (this.transshipment1STA_Original != newValue) { this.transshipment1STA_Original = newValue; this.MarkAsDirty("Transshipment1STA_Original"); } }

    private transshipment2ATD_Original: Date;
    public get Transshipment2ATD_Original() { return this.transshipment2ATD_Original; }
    public set Transshipment2ATD_Original(newValue: Date) { if (this.transshipment2ATD_Original != newValue) { this.transshipment2ATD_Original = newValue; this.MarkAsDirty("Transshipment2ATD_Original"); } }


    private transshipment2ATA_Original: Date;
    public get Transshipment2ATA_Original() { return this.transshipment2ATA_Original; }
    public set Transshipment2ATA_Original(newValue: Date) { if (this.transshipment2ATA_Original != newValue) { this.transshipment2ATA_Original = newValue; this.MarkAsDirty("Transshipment2ATA_Original"); } }


    private transshipment2ETD_Original: Date;
    public get Transshipment2ETD_Original() { return this.transshipment2ETD_Original; }
    public set Transshipment2ETD_Original(newValue: Date) { if (this.transshipment2ETD_Original != newValue) { this.transshipment2ETD_Original = newValue; this.MarkAsDirty("Transshipment2ETD_Original"); } }


    private transshipment2ETA_Original: Date;
    public get Transshipment2ETA_Original() { return this.transshipment2ETA_Original; }
    public set Transshipment2ETA_Original(newValue: Date) { if (this.transshipment2ETA_Original != newValue) { this.transshipment2ETA_Original = newValue; this.MarkAsDirty("Transshipment2ETA_Original"); } }

    private transshipment2STD_Original: Date;
    public get Transshipment2STD_Original() { return this.transshipment2STD_Original; }
    public set Transshipment2STD_Original(newValue: Date) { if (this.transshipment2STD_Original != newValue) { this.transshipment2STD_Original = newValue; this.MarkAsDirty("Transshipment2STD_Original"); } }


    private transshipment2STA_Original: Date;
    public get Transshipment2STA_Original() { return this.transshipment2STA_Original; }
    public set Transshipment2STA_Original(newValue: Date) { if (this.transshipment2STA_Original != newValue) { this.transshipment2STA_Original = newValue; this.MarkAsDirty("Transshipment2STA_Original"); } }

    private transshipment3ATD_Original: Date;
    public get Transshipment3ATD_Original() { return this.transshipment3ATD_Original; }
    public set Transshipment3ATD_Original(newValue: Date) { if (this.transshipment3ATD_Original != newValue) { this.transshipment3ATD_Original = newValue; this.MarkAsDirty("Transshipment3ATD_Original"); } }


    private transshipment3ATA_Original: Date;
    public get Transshipment3ATA_Original() { return this.transshipment3ATA_Original; }
    public set Transshipment3ATA_Original(newValue: Date) { if (this.transshipment3ATA_Original != newValue) { this.transshipment3ATA_Original = newValue; this.MarkAsDirty("Transshipment3ATA_Original"); } }


    private transshipment3ETD_Original: Date;
    public get Transshipment3ETD_Original() { return this.transshipment3ETD_Original; }
    public set Transshipment3ETD_Original(newValue: Date) { if (this.transshipment3ETD_Original != newValue) { this.transshipment3ETD_Original = newValue; this.MarkAsDirty("Transshipment3ETD_Original"); } }


    private transshipment3ETA_Original: Date;
    public get Transshipment3ETA_Original() { return this.transshipment3ETA_Original; }
    public set Transshipment3ETA_Original(newValue: Date) { if (this.transshipment3ETA_Original != newValue) { this.transshipment3ETA_Original = newValue; this.MarkAsDirty("Transshipment3ETA_Original"); } }

    private transshipment3STD_Original: Date;
    public get Transshipment3STD_Original() { return this.transshipment3STD_Original; }
    public set Transshipment3STD_Original(newValue: Date) { if (this.transshipment3STD_Original != newValue) { this.transshipment3STD_Original = newValue; this.MarkAsDirty("Transshipment3STD_Original"); } }


    private transshipment3STA_Original: Date;
    public get Transshipment3STA_Original() { return this.transshipment3STA_Original; }
    public set Transshipment3STA_Original(newValue: Date) { if (this.transshipment3STA_Original != newValue) { this.transshipment3STA_Original = newValue; this.MarkAsDirty("Transshipment3STA_Original"); } }

    private preCarriageETD_Original: Date;
    public get PreCarriageETD_Original() { return this.preCarriageETD_Original; }
    public set PreCarriageETD_Original(newValue: Date) { if (this.preCarriageETD_Original != newValue) { this.preCarriageETD_Original = newValue; this.MarkAsDirty("PreCarriageETD_Original"); } }


    private preCarriageATD_Original: Date;
    public get PreCarriageATD_Original() { return this.preCarriageATD_Original; }
    public set PreCarriageATD_Original(newValue: Date) { if (this.preCarriageATD_Original != newValue) { this.preCarriageATD_Original = newValue; this.MarkAsDirty("PreCarriageATD_Original"); } }


    private preCarriageETA_Original: Date;
    public get PreCarriageETA_Original() { return this.preCarriageETA_Original; }
    public set PreCarriageETA_Original(newValue: Date) { if (this.preCarriageETA_Original != newValue) { this.preCarriageETA_Original = newValue; this.MarkAsDirty("PreCarriageETA_Original"); } }


    private preCarriageATA_Original: Date;
    public get PreCarriageATA_Original() { return this.preCarriageATA_Original; }
    public set PreCarriageATA_Original(newValue: Date) { if (this.preCarriageATA_Original != newValue) { this.preCarriageATA_Original = newValue; this.MarkAsDirty("PreCarriageATA_Original"); } }

    private onCarriageETD_Original: Date;
    public get OnCarriageETD_Original() { return this.onCarriageETD_Original; }
    public set OnCarriageETD_Original(newValue: Date) { if (this.onCarriageETD_Original != newValue) { this.onCarriageETD_Original = newValue; this.MarkAsDirty("OnCarriageETD_Original"); } }


    private onCarriageATD_Original: Date;
    public get OnCarriageATD_Original() { return this.onCarriageATD_Original; }
    public set OnCarriageATD_Original(newValue: Date) { if (this.onCarriageATD_Original != newValue) { this.onCarriageATD_Original = newValue; this.MarkAsDirty("OnCarriageATD_Original"); } }

    private onCarriageETA_Original: Date;
    public get OnCarriageETA_Original() { return this.onCarriageETA_Original; }
    public set OnCarriageETA_Original(newValue: Date) { if (this.onCarriageETA_Original != newValue) { this.onCarriageETA_Original = newValue; this.MarkAsDirty("OnCarriageETA_Original"); } }

    private onCarriageATA_Original: Date;
    public get OnCarriageATA_Original() { return this.onCarriageATA_Original; }
    public set OnCarriageATA_Original(newValue: Date) { if (this.onCarriageATA_Original != newValue) { this.onCarriageATA_Original = newValue; this.MarkAsDirty("OnCarriageATA_Original"); } }

    private iNTTRABookingStatusCode_Original: string;
    public get INTTRABookingStatusCode_Original() { return this.iNTTRABookingStatusCode_Original; }
    public set INTTRABookingStatusCode_Original(newValue: string) { this.iNTTRABookingStatusCode_Original = newValue; this.MarkAsDirty(); }

    private bookingConfirmedBy_Original: string;
    public get BookingConfirmedBy_Original() { return this.bookingConfirmedBy_Original; }
    public set BookingConfirmedBy_Original(newValue: string) { if (this.bookingConfirmedBy_Original != newValue) { this.bookingConfirmedBy_Original = newValue; this.MarkAsDirty("BookingConfirmedBy_Original"); } }   

    private mAN_FromPortId_Original: string;
    public get MAN_FromPortId_Original() { return this.mAN_FromPortId_Original; }
    public set MAN_FromPortId_Original(newValue: string) { if (this.mAN_FromPortId_Original != newValue) { this.mAN_FromPortId_Original = newValue; this.MarkAsDirty("MAN_FromPortId_Original"); } }

    private fIN_PortId_Original: string;
    public get FIN_PortId_Original() { return this.fIN_PortId_Original; }
    public set FIN_PortId_Original(newValue: string) { if (this.fIN_PortId_Original != newValue) { this.fIN_PortId_Original = newValue; this.MarkAsDirty("FIN_PortId_Original"); } }

    private bookingConfNumber_Original: string;
    public get BookingConfNumber_Original() { return this.bookingConfNumber_Original; }
    public set BookingConfNumber_Original(newValue: string) { if (this.bookingConfNumber_Original != newValue) { this.bookingConfNumber_Original = newValue; this.MarkAsDirty("BookingConfNumber_Original"); } }

    private mAN_CarrierNumber_Original: string;
    public get MAN_CarrierNumber_Original() { return this.mAN_CarrierNumber_Original; }
    public set MAN_CarrierNumber_Original(newValue: string) { if (this.mAN_CarrierNumber_Original != newValue) { this.mAN_CarrierNumber_Original = newValue; this.MarkAsDirty("MAN_CarrierNumber_Original"); } }

    private tR1_ToPortId_Original: string;
    public get TR1_ToPortId_Original() { return this.tR1_ToPortId_Original; }
    public set TR1_ToPortId_Original(newValue: string) { if (this.tR1_ToPortId_Original != newValue) { this.tR1_ToPortId_Original = newValue; this.MarkAsDirty("TR1_ToPortId_Original"); } }

    private tR2_ToPortId_Original: string;
    public get TR2_ToPortId_Original() { return this.tR2_ToPortId_Original; }
    public set TR2_ToPortId_Original(newValue: string) { if (this.tR2_ToPortId_Original != newValue) { this.tR2_ToPortId_Original = newValue; this.MarkAsDirty("TR2_ToPortId_Original"); } }

    private tR3_ToPortId_Original: string;
    public get TR3_ToPortId_Original() { return this.tR3_ToPortId_Original; }
    public set TR3_ToPortId_Original(newValue: string) {
        if (this.tR3_ToPortId_Original != newValue) {
            this.tR3_ToPortId_Original = newValue;
            this.MarkAsDirty("TR3_ToPortId_Original");
        }
    }
    
    private storageDays: number;
    public get StorageDays() { return this.storageDays; }
    public set StorageDays(newValue: number) { if (this.storageDays != newValue) { this.storageDays = newValue; this.MarkAsDirty("StorageDays"); } }

    private warehouseReleasesIds: string;
    public get WarehouseReleasesIds() { return this.warehouseReleasesIds; }
    public set WarehouseReleasesIds(newValue: string) { if (this.warehouseReleasesIds != newValue) { this.warehouseReleasesIds = newValue; this.MarkAsDirty("WarehouseReleasesIds"); } }

    private sLAC: string;
    public get SLAC() { return this.sLAC; }
    public set SLAC(newValue: string) { if (this.sLAC != newValue) { this.sLAC = newValue; this.MarkAsDirty("SLAC"); } }

    private shipmentSubTypeId: string;
    public get ShipmentSubTypeId() { return this.shipmentSubTypeId; }
    public set ShipmentSubTypeId(newValue: string) { if (this.shipmentSubTypeId != newValue) { this.shipmentSubTypeId = newValue; this.MarkAsDirty("ShipmentSubTypeId"); } }

    private shipmentSubTypeName: string;
    public get ShipmentSubTypeName() { return this.shipmentSubTypeName; }
    public set ShipmentSubTypeName(newValue: string) { if (this.shipmentSubTypeName != newValue) { this.shipmentSubTypeName = newValue; this.MarkAsDirty("ShipmentSubTypeName"); } }

    private isCFSWarehouse: boolean;
    public get IsCFSWarehouse() { return this.isCFSWarehouse; }
    public set IsCFSWarehouse(newValue: boolean) { if (this.isCFSWarehouse != newValue) { this.isCFSWarehouse = newValue; this.MarkAsDirty("IsCFSWarehouse"); } }

    private isCFSWarehouseChanged: boolean;
    public get IsCFSWarehouseChanged() { return this.isCFSWarehouseChanged; }
    public set IsCFSWarehouseChanged(newValue: boolean) { if (this.isCFSWarehouseChanged != newValue) { this.isCFSWarehouseChanged = newValue; this.MarkAsDirty("IsCFSWarehouseChanged"); } }

    private chargeStorage: boolean;
    public get ChargeStorage() { return this.chargeStorage; }
    public set ChargeStorage(newValue: boolean) { if (this.chargeStorage != newValue) { this.chargeStorage = newValue; this.MarkAsDirty("ChargeStorage"); } }

    private chargeStorageCurrencyId: string;
    public get ChargeStorageCurrencyId() { return this.chargeStorageCurrencyId; }
    public set ChargeStorageCurrencyId(newValue: string) { if (this.chargeStorageCurrencyId != newValue) { this.chargeStorageCurrencyId = newValue; this.MarkAsDirty("ChargeStorageCurrencyId"); } }

    private chargeStorageCurrencyCode: string;
    public get ChargeStorageCurrencyCode() { return this.chargeStorageCurrencyCode; }
    public set ChargeStorageCurrencyCode(newValue: string) { if (this.chargeStorageCurrencyCode != newValue) { this.chargeStorageCurrencyCode = newValue; this.MarkAsDirty("ChargeStorageCurrencyCode"); } }

    private weightMeasurementCode: string;
    public get WeightMeasurementCode() { return this.weightMeasurementCode; }
    public set WeightMeasurementCode(newValue: string) { if (this.weightMeasurementCode != newValue) { this.weightMeasurementCode = newValue; this.MarkAsDirty("WeightMeasurementCode"); } }

    private weightRoundingCode: string;
    public get WeightRoundingCode() { return this.weightRoundingCode; }
    public set WeightRoundingCode(newValue: string) { if (this.weightRoundingCode != newValue) { this.weightRoundingCode = newValue; this.MarkAsDirty("WeightRoundingCode"); } }

    private viewSharedDocuments: string;
    public get ViewSharedDocuments() { return this.viewSharedDocuments; }
    public set ViewSharedDocuments(newValue: string) { if (this.viewSharedDocuments != newValue) { this.viewSharedDocuments = newValue; this.MarkAsDirty("ViewSharedDocuments"); } }

    private isAccrualsApproved: boolean;
    public get IsAccrualsApproved() { return this.isAccrualsApproved; }
    public set IsAccrualsApproved(newValue: boolean) { if (this.isAccrualsApproved != newValue) { this.isAccrualsApproved = newValue; this.MarkAsDirty("IsAccrualsApproved"); } }

    private accrualsApprovalDate: Date;
    public get AccrualsApprovalDate() { return this.accrualsApprovalDate; }
    public set AccrualsApprovalDate(newValue: Date) { if (this.accrualsApprovalDate != newValue) { this.accrualsApprovalDate = newValue; this.MarkAsDirty("AccrualsApprovalDate"); } }

    private isGroupageHousesUpdated: boolean;
    public get IsGroupageHousesUpdated() { return this.isGroupageHousesUpdated; }
    public set IsGroupageHousesUpdated(newValue: boolean) { if (this.isGroupageHousesUpdated != newValue) { this.isGroupageHousesUpdated = newValue; this.MarkAsDirty("IsGroupageHousesUpdated"); } }

    private preForwardingTransportModeId: string;
    public get PreForwardingTransportModeId() { return this.preForwardingTransportModeId; }
    public set PreForwardingTransportModeId(newValue: string) { if (this.preForwardingTransportModeId != newValue) { this.preForwardingTransportModeId = newValue; this.MarkAsDirty("PreForwardingTransportModeId"); } }

    private preForwardingFromPortId: string;
    public get PreForwardingFromPortId() { return this.preForwardingFromPortId; }
    public set PreForwardingFromPortId(newValue: string) { if (this.preForwardingFromPortId != newValue) { this.preForwardingFromPortId = newValue; this.MarkAsDirty("PreForwardingFromPortId"); } }

    private preForwardingToPortId: string;
    public get PreForwardingToPortId() { return this.preForwardingToPortId; }
    public set PreForwardingToPortId(newValue: string) { if (this.preForwardingToPortId != newValue) { this.preForwardingToPortId = newValue; this.MarkAsDirty("PreForwardingToPortId"); } }

    private preForwardingCarrierId: string;
    public get PreForwardingCarrierId() { return this.preForwardingCarrierId; }
    public set PreForwardingCarrierId(newValue: string) { if (this.preForwardingCarrierId != newValue) { this.preForwardingCarrierId = newValue; this.MarkAsDirty("PreForwardingCarrierId"); } }

    private preForwardingCarrierNumber: string;
    public get PreForwardingCarrierNumber() { return this.preForwardingCarrierNumber; }
    public set PreForwardingCarrierNumber(newValue: string) { if (this.preForwardingCarrierNumber != newValue) { this.preForwardingCarrierNumber = newValue; this.MarkAsDirty("PreForwardingCarrierNumber"); } }

    private preForwardingCarrierName: string;
    public get PreForwardingCarrierName() { return this.preForwardingCarrierName; }
    public set PreForwardingCarrierName(newValue: string) { if (this.preForwardingCarrierName != newValue) { this.preForwardingCarrierName = newValue; this.MarkAsDirty("PreForwardingCarrierName"); } }

    private preForwardingCarrierCode: string;
    public get PreForwardingCarrierCode() { return this.preForwardingCarrierCode; }
    public set PreForwardingCarrierCode(newValue: string) { if (this.preForwardingCarrierCode != newValue) { this.preForwardingCarrierCode = newValue; this.MarkAsDirty("PreForwardingCarrierCode"); } }

    private preForwardingFromPortCode: string;
    public get PreForwardingFromPortCode() { return this.preForwardingFromPortCode; }
    public set PreForwardingFromPortCode(newValue: string) { if (this.preForwardingFromPortCode != newValue) { this.preForwardingFromPortCode = newValue; this.MarkAsDirty("PreForwardingFromPortCode"); } }

    private preForwardingFromPortName: string;
    public get PreForwardingFromPortName() { return this.preForwardingFromPortName; }
    public set PreForwardingFromPortName(newValue: string) { if (this.preForwardingFromPortName != newValue) { this.preForwardingFromPortName = newValue; this.MarkAsDirty("PreForwardingFromPortName"); } }

    private preForwardingFromPortCountryCode: string;
    public get PreForwardingFromPortCountryCode() { return this.preForwardingFromPortCountryCode; }
    public set PreForwardingFromPortCountryCode(newValue: string) { if (this.preForwardingFromPortCountryCode != newValue) { this.preForwardingFromPortCountryCode = newValue; this.MarkAsDirty("PreForwardingFromPortCountryCode"); } }

    private preForwardingFromPortCountryName: string;
    public get PreForwardingFromPortCountryName() { return this.preForwardingFromPortCountryName; }
    public set PreForwardingFromPortCountryName(newValue: string) { if (this.preForwardingFromPortCountryName != newValue) { this.preForwardingFromPortCountryName = newValue; this.MarkAsDirty("PreForwardingFromPortCountryName"); } }

    private preForwardingToPortCode: string;
    public get PreForwardingToPortCode() { return this.preForwardingToPortCode; }
    public set PreForwardingToPortCode(newValue: string) { if (this.preForwardingToPortCode != newValue) { this.preForwardingToPortCode = newValue; this.MarkAsDirty("PreForwardingToPortCode"); } }

    private preForwardingToPortName: string;
    public get PreForwardingToPortName() { return this.preForwardingToPortName; }
    public set PreForwardingToPortName(newValue: string) { if (this.preForwardingToPortName != newValue) { this.preForwardingToPortName = newValue; this.MarkAsDirty("PreForwardingToPortName"); } }

    private preForwardingToPortCountryCode: string;
    public get PreForwardingToPortCountryCode() { return this.preForwardingToPortCountryCode; }
    public set PreForwardingToPortCountryCode(newValue: string) { if (this.preForwardingToPortCountryCode != newValue) { this.preForwardingToPortCountryCode = newValue; this.MarkAsDirty("PreForwardingToPortCountryCode"); } }

    private preForwardingToPortCountryName: string;
    public get PreForwardingToPortCountryName() { return this.preForwardingToPortCountryName; }
    public set PreForwardingToPortCountryName(newValue: string) { if (this.preForwardingToPortCountryName != newValue) { this.preForwardingToPortCountryName = newValue; this.MarkAsDirty("PreForwardingToPortCountryName"); } }

    private preForwardingETD: Date;
    public get PreForwardingETD() { return this.preForwardingETD; }
    public set PreForwardingETD(newValue: Date) { if (this.preForwardingETD != newValue) { this.preForwardingETD = newValue; this.MarkAsDirty("PreForwardingETD"); } }

    private preForwardingATD: Date;
    public get PreForwardingATD() { return this.preForwardingATD; }
    public set PreForwardingATD(newValue: Date) { if (this.preForwardingATD != newValue) { this.preForwardingATD = newValue; this.MarkAsDirty("PreForwardingATD"); } }

    private preForwardingETA: Date;
    public get PreForwardingETA() { return this.preForwardingETA; }
    public set PreForwardingETA(newValue: Date) { if (this.preForwardingETA != newValue) { this.preForwardingETA = newValue; this.MarkAsDirty("PreForwardingETA"); } }

    private preForwardingATA: Date;
    public get PreForwardingATA() { return this.preForwardingATA; }
    public set PreForwardingATA(newValue: Date) { if (this.preForwardingATA != newValue) { this.preForwardingATA = newValue; this.MarkAsDirty("PreForwardingATA"); } }

    private preForwardingCarrierWebSite: string;
    public get PreForwardingCarrierWebSite() { return this.preForwardingCarrierWebSite; }
    public set PreForwardingCarrierWebSite(newValue: string) { if (this.preForwardingCarrierWebSite != newValue) { this.preForwardingCarrierWebSite = newValue; this.MarkAsDirty("PreForwardingCarrierWebSite"); } }

    private preForwardingVesselId: string;
    public get PreForwardingVesselId() { return this.preForwardingVesselId; }
    public set PreForwardingVesselId(newValue: string) { if (this.preForwardingVesselId != newValue) { this.preForwardingVesselId = newValue; this.MarkAsDirty("PreForwardingVesselId"); } }

    private preForwardingVesselName: string;
    public get PreForwardingVesselName() { return this.preForwardingVesselName; }
    public set PreForwardingVesselName(newValue: string) { if (this.preForwardingVesselName != newValue) { this.preForwardingVesselName = newValue; this.MarkAsDirty("PreForwardingVesselName"); } }

    private hasPreForwarding: boolean;
    public get HasPreForwarding() { return this.hasPreForwarding; }
    public set HasPreForwarding(newValue: boolean) { if (this.hasPreForwarding != newValue) { this.hasPreForwarding = newValue; this.MarkAsDirty("HasPreForwarding"); } }

    private preForwardingETD_Original: Date;
    public get PreForwardingETD_Original() { return this.preForwardingETD_Original; }
    public set PreForwardingETD_Original(newValue: Date) { if (this.preForwardingETD_Original != newValue) { this.preForwardingETD_Original = newValue; this.MarkAsDirty("PreForwardingETD_Original"); } }

    private preForwardingATD_Original: Date;
    public get PreForwardingATD_Original() { return this.preForwardingATD_Original; }
    public set PreForwardingATD_Original(newValue: Date) { if (this.preForwardingATD_Original != newValue) { this.preForwardingATD_Original = newValue; this.MarkAsDirty("PreForwardingATD_Original"); } }

    private preForwardingETA_Original: Date;
    public get PreForwardingETA_Original() { return this.preForwardingETA_Original; }
    public set PreForwardingETA_Original(newValue: Date) { if (this.preForwardingETA_Original != newValue) { this.preForwardingETA_Original = newValue; this.MarkAsDirty("PreForwardingETA_Original"); } }

    private preForwardingATA_Original: Date;
    public get PreForwardingATA_Original() { return this.preForwardingATA_Original; }
    public set PreForwardingATA_Original(newValue: Date) { if (this.preForwardingATA_Original != newValue) { this.preForwardingATA_Original = newValue; this.MarkAsDirty("PreForwardingATA_Original"); } }

    private onForwardingTransportModeId: string;
    public get OnForwardingTransportModeId() { return this.onForwardingTransportModeId; }
    public set OnForwardingTransportModeId(newValue: string) { if (this.onForwardingTransportModeId != newValue) { this.onForwardingTransportModeId = newValue; this.MarkAsDirty("OnForwardingTransportModeId"); } }

    private onForwardingFromPortId: string;
    public get OnForwardingFromPortId() { return this.onForwardingFromPortId; }
    public set OnForwardingFromPortId(newValue: string) { if (this.onForwardingFromPortId != newValue) { this.onForwardingFromPortId = newValue; this.MarkAsDirty("OnForwardingFromPortId"); } }

    private onForwardingToPortId: string;
    public get OnForwardingToPortId() { return this.onForwardingToPortId; }
    public set OnForwardingToPortId(newValue: string) { if (this.onForwardingToPortId != newValue) { this.onForwardingToPortId = newValue; this.MarkAsDirty("OnForwardingToPortId"); } }

    private onForwardingCarrierId: string;
    public get OnForwardingCarrierId() { return this.onForwardingCarrierId; }
    public set OnForwardingCarrierId(newValue: string) { if (this.onForwardingCarrierId != newValue) { this.onForwardingCarrierId = newValue; this.MarkAsDirty("OnForwardingCarrierId"); } }

    private onForwardingCarrierNumber: string;
    public get OnForwardingCarrierNumber() { return this.onForwardingCarrierNumber; }
    public set OnForwardingCarrierNumber(newValue: string) { if (this.onForwardingCarrierNumber != newValue) { this.onForwardingCarrierNumber = newValue; this.MarkAsDirty("OnForwardingCarrierNumber"); } }

    private onForwardingCarrierName: string;
    public get OnForwardingCarrierName() { return this.onForwardingCarrierName; }
    public set OnForwardingCarrierName(newValue: string) { if (this.onForwardingCarrierName != newValue) { this.onForwardingCarrierName = newValue; this.MarkAsDirty("OnForwardingCarrierName"); } }

    private onForwardingCarrierCode: string;
    public get OnForwardingCarrierCode() { return this.onForwardingCarrierCode; }
    public set OnForwardingCarrierCode(newValue: string) { if (this.onForwardingCarrierCode != newValue) { this.onForwardingCarrierCode = newValue; this.MarkAsDirty("OnForwardingCarrierCode"); } }

    private onForwardingFromPortCode: string;
    public get OnForwardingFromPortCode() { return this.onForwardingFromPortCode; }
    public set OnForwardingFromPortCode(newValue: string) { if (this.onForwardingFromPortCode != newValue) { this.onForwardingFromPortCode = newValue; this.MarkAsDirty("OnForwardingFromPortCode"); } }

    private onForwardingFromPortName: string;
    public get OnForwardingFromPortName() { return this.onForwardingFromPortName; }
    public set OnForwardingFromPortName(newValue: string) { if (this.onForwardingFromPortName != newValue) { this.onForwardingFromPortName = newValue; this.MarkAsDirty("OnForwardingFromPortName"); } }

    private onForwardingFromPortCountryCode: string;
    public get OnForwardingFromPortCountryCode() { return this.onForwardingFromPortCountryCode; }
    public set OnForwardingFromPortCountryCode(newValue: string) { if (this.onForwardingFromPortCountryCode != newValue) { this.onForwardingFromPortCountryCode = newValue; this.MarkAsDirty("OnForwardingFromPortCountryCode"); } }

    private onForwardingFromPortCountryName: string;
    public get OnForwardingFromPortCountryName() { return this.onForwardingFromPortCountryName; }
    public set OnForwardingFromPortCountryName(newValue: string) { if (this.onForwardingFromPortCountryName != newValue) { this.onForwardingFromPortCountryName = newValue; this.MarkAsDirty("OnForwardingFromPortCountryName"); } }

    private onForwardingToPortCode: string;
    public get OnForwardingToPortCode() { return this.onForwardingToPortCode; }
    public set OnForwardingToPortCode(newValue: string) { if (this.onForwardingToPortCode != newValue) { this.onForwardingToPortCode = newValue; this.MarkAsDirty("OnForwardingToPortCode"); } }

    private onForwardingToPortName: string;
    public get OnForwardingToPortName() { return this.onForwardingToPortName; }
    public set OnForwardingToPortName(newValue: string) { if (this.onForwardingToPortName != newValue) { this.onForwardingToPortName = newValue; this.MarkAsDirty("OnForwardingToPortName"); } }

    private onForwardingToPortCountryCode: string;
    public get OnForwardingToPortCountryCode() { return this.onForwardingToPortCountryCode; }
    public set OnForwardingToPortCountryCode(newValue: string) { if (this.onForwardingToPortCountryCode != newValue) { this.onForwardingToPortCountryCode = newValue; this.MarkAsDirty("OnForwardingToPortCountryCode"); } }

    private onForwardingToPortCountryName: string;
    public get OnForwardingToPortCountryName() { return this.onForwardingToPortCountryName; }
    public set OnForwardingToPortCountryName(newValue: string) { if (this.onForwardingToPortCountryName != newValue) { this.onForwardingToPortCountryName = newValue; this.MarkAsDirty("OnForwardingToPortCountryName"); } }

    private onForwardingETD: Date;
    public get OnForwardingETD() { return this.onForwardingETD; }
    public set OnForwardingETD(newValue: Date) { if (this.onForwardingETD != newValue) { this.onForwardingETD = newValue; this.MarkAsDirty("OnForwardingETD"); } }

    private onForwardingATD: Date;
    public get OnForwardingATD() { return this.onForwardingATD; }
    public set OnForwardingATD(newValue: Date) { if (this.onForwardingATD != newValue) { this.onForwardingATD = newValue; this.MarkAsDirty("OnForwardingATD"); } }

    private onForwardingETA: Date;
    public get OnForwardingETA() { return this.onForwardingETA; }
    public set OnForwardingETA(newValue: Date) { if (this.onForwardingETA != newValue) { this.onForwardingETA = newValue; this.MarkAsDirty("OnForwardingETA"); } }

    private onForwardingATA: Date;
    public get OnForwardingATA() { return this.onForwardingATA; }
    public set OnForwardingATA(newValue: Date) { if (this.onForwardingATA != newValue) { this.onForwardingATA = newValue; this.MarkAsDirty("OnForwardingATA"); } }

    private onForwardingCarrierWebSite: string;
    public get OnForwardingCarrierWebSite() { return this.onForwardingCarrierWebSite; }
    public set OnForwardingCarrierWebSite(newValue: string) { if (this.onForwardingCarrierWebSite != newValue) { this.onForwardingCarrierWebSite = newValue; this.MarkAsDirty("OnForwardingCarrierWebSite"); } }

    private onForwardingVesselId: string;
    public get OnForwardingVesselId() { return this.onForwardingVesselId; }
    public set OnForwardingVesselId(newValue: string) { if (this.onForwardingVesselId != newValue) { this.onForwardingVesselId = newValue; this.MarkAsDirty("OnForwardingVesselId"); } }

    private onForwardingVesselName: string;
    public get OnForwardingVesselName() { return this.onForwardingVesselName; }
    public set OnForwardingVesselName(newValue: string) { if (this.onForwardingVesselName != newValue) { this.onForwardingVesselName = newValue; this.MarkAsDirty("OnForwardingVesselName"); } }

    private hasOnForwarding: boolean;
    public get HasOnForwarding() { return this.hasOnForwarding; }
    public set HasOnForwarding(newValue: boolean) { if (this.hasOnForwarding != newValue) { this.hasOnForwarding = newValue; this.MarkAsDirty("HasOnForwarding"); } }

    private onForwardingAdditionalTransportModeCode: string;
    public get OnForwardingAdditionalTransportModeCode() { return this.onForwardingAdditionalTransportModeCode; }
    public set OnForwardingAdditionalTransportModeCode(newValue: string) { this.onForwardingAdditionalTransportModeCode = newValue; this.MarkAsDirty(); }

    private splitOnForwarding: boolean;
    public get SplitOnForwarding() { return this.splitOnForwarding; }
    public set SplitOnForwarding(newValue: boolean) { if (this.splitOnForwarding != newValue) { this.splitOnForwarding = newValue; this.MarkAsDirty(); } }      

    private onForwardingETD_Original: Date;
    public get OnForwardingETD_Original() { return this.onForwardingETD_Original; }
    public set OnForwardingETD_Original(newValue: Date) { if (this.onForwardingETD_Original != newValue) { this.onForwardingETD_Original = newValue; this.MarkAsDirty("OnForwardingETD_Original"); } }

    private onForwardingATD_Original: Date;
    public get OnForwardingATD_Original() { return this.onForwardingATD_Original; }
    public set OnForwardingATD_Original(newValue: Date) { if (this.onForwardingATD_Original != newValue) { this.onForwardingATD_Original = newValue; this.MarkAsDirty("OnForwardingATD_Original"); } }

    private onForwardingETA_Original: Date;
    public get OnForwardingETA_Original() { return this.onForwardingETA_Original; }
    public set OnForwardingETA_Original(newValue: Date) { if (this.onForwardingETA_Original != newValue) { this.onForwardingETA_Original = newValue; this.MarkAsDirty("OnForwardingETA_Original"); } }

    private onForwardingATA_Original: Date;
    public get OnForwardingATA_Original() { return this.onForwardingATA_Original; }
    public set OnForwardingATA_Original(newValue: Date) { if (this.onForwardingATA_Original != newValue) { this.onForwardingATA_Original = newValue; this.MarkAsDirty("OnForwardingATA_Original"); } }

    private assignedToTruckerDate: Date;
    public get AssignedToTruckerDate() { return this.assignedToTruckerDate; }
    public set AssignedToTruckerDate(newValue: Date) { if (this.assignedToTruckerDate != newValue) { this.assignedToTruckerDate = newValue; this.MarkAsDirty("AssignedToTruckerDate"); } }

    private truckerId: string;
    public get TruckerId() { return this.truckerId; }
    public set TruckerId(newValue: string) { if (this.truckerId != newValue) { this.truckerId = newValue; this.MarkAsDirty("TruckerId"); } }
     
    private truckerAddressId: string;
    public get TruckerAddressId() { return this.truckerAddressId; }
    public set TruckerAddressId(newValue: string) { if (this.truckerAddressId != newValue) { this.truckerAddressId = newValue; this.MarkAsDirty("TruckerAddressId"); } }

    private truckerContactId: string;
    public get TruckerContactId() { return this.truckerContactId; }
    public set TruckerContactId(newValue: string) { if (this.truckerContactId != newValue) { this.truckerContactId = newValue; this.MarkAsDirty("TruckerContactId"); } }

    private truckerReference1: string;
    public get TruckerReference1() { return this.truckerReference1; }
    public set TruckerReference1(newValue: string) { if (this.truckerReference1 != newValue) { this.truckerReference1 = newValue; this.MarkAsDirty("TruckerReference1"); } }

    private truckerReference2: string;
    public get TruckerReference2() { return this.truckerReference2; }
    public set TruckerReference2(newValue: string) { if (this.truckerReference2 != newValue) { this.truckerReference2 = newValue; this.MarkAsDirty("TruckerReference2"); } }

    private truckerName: string;
    public get TruckerName() { return this.truckerName; }
    public set TruckerName(newValue: string) { if (this.truckerName != newValue) { this.truckerName = newValue; this.MarkAsDirty("TruckerName"); } }


    private truckerNote: string;
    public get TruckerNote() { return this.truckerNote; }
    public set TruckerNote(newValue: string) { if (this.truckerNote != newValue) { this.truckerNote = newValue; this.MarkAsDirty("TruckerNote"); } }

    private originPreCarriageFromPortId: string;
    public get OriginPreCarriageFromPortId() { return this.originPreCarriageFromPortId; }
    public set OriginPreCarriageFromPortId(newValue: string) {
        if (this.originPreCarriageFromPortId != newValue) {
            this.originPreCarriageFromPortId = newValue;
            this.MarkAsDirty("OriginPreCarriageFromPortId");
        }
    }

    private originPreCarriageToPortId: string;
    public get OriginPreCarriageToPortId() { return this.originPreCarriageToPortId; }
    public set OriginPreCarriageToPortId(newValue: string) {
        if (this.originPreCarriageToPortId != newValue) {
            this.originPreCarriageToPortId = newValue;
            this.MarkAsDirty("OriginPreCarriageToPortId");
        }
    }

    private originOnCarriageToPortId: string;
    public get OriginOnCarriageToPortId() { return this.originOnCarriageToPortId; }
    public set OriginOnCarriageToPortId(newValue: string) {
        if (this.originOnCarriageToPortId != newValue) {
            this.originOnCarriageToPortId = newValue;
            this.MarkAsDirty("OriginOnCarriageToPortId");
        }
    }

    private originOnCarriageFromPortId: string;
    public get OriginOnCarriageFromPortId() { return this.originOnCarriageFromPortId; }
    public set OriginOnCarriageFromPortId(newValue: string) {
        if (this.originOnCarriageFromPortId != newValue) {
            this.originOnCarriageFromPortId = newValue;
            this.MarkAsDirty("OriginOnCarriageFromPortId");
        }
    }

    private documentFilingIds: string;
    public get DocumentFilingIds() { return this.documentFilingIds; }
    public set DocumentFilingIds(newValue: string) { if (this.documentFilingIds != newValue) { this.documentFilingIds = newValue; this.MarkAsDirty("DocumentFilingIds"); } }


    private isStandalonePickupDelivery: boolean;
    public get IsStandalonePickupDelivery() { return this.isStandalonePickupDelivery; }
    public set IsStandalonePickupDelivery(newValue: boolean) { if (this.isStandalonePickupDelivery != newValue) { this.isStandalonePickupDelivery = newValue; this.MarkAsDirty("IsStandalonePickupDelivery"); } }      

    private standalonePickupDeliveryId: string;
    public get StandalonePickupDeliveryId() { return this.standalonePickupDeliveryId; }
    public set StandalonePickupDeliveryId(newValue: string) { if (this.standalonePickupDeliveryId != newValue) { this.standalonePickupDeliveryId = newValue; this.MarkAsDirty("StandalonePickupDeliveryId"); } }      

    private standalonePickupDeliveryNumber: string;
    public get StandalonePickupDeliveryNumber() { return this.standalonePickupDeliveryNumber; }
    public set StandalonePickupDeliveryNumber(newValue: string) { if (this.standalonePickupDeliveryNumber != newValue) { this.standalonePickupDeliveryNumber = newValue; this.MarkAsDirty("StandalonePickupDeliveryNumber"); } }      

    private parentShipmentDirectionId: string;
    public get ParentShipmentDirectionId() { return this.parentShipmentDirectionId; }
    public set ParentShipmentDirectionId(newValue: string) { if (this.parentShipmentDirectionId != newValue) { this.parentShipmentDirectionId = newValue; this.MarkAsDirty("ParentShipmentDirectionId"); } }      

    private parentShipmentNumber: string;
    public get ParentShipmentNumber() { return this.parentShipmentNumber; }
    public set ParentShipmentNumber(newValue: string) { if (this.parentShipmentNumber != newValue) { this.parentShipmentNumber = newValue; this.MarkAsDirty("ParentShipmentNumber"); } }      

    private parentShipmentType: string;
    public get ParentShipmentType() { return this.parentShipmentType; }
    public set ParentShipmentType(newValue: string) { if (this.parentShipmentType != newValue) { this.parentShipmentType = newValue; this.MarkAsDirty("ParentShipmentType"); } }      
 
    private isProductItemsUpdated: boolean;
    public get IsProductItemsUpdated() { return this.isProductItemsUpdated; }
    public set IsProductItemsUpdated(newValue: boolean) { if (this.isProductItemsUpdated != newValue) { this.isProductItemsUpdated = newValue; this.MarkAsDirty("IsProductItemsUpdated"); } }

    private forwarderStandaloneShipmentId: string;
    public get ForwarderStandaloneShipmentId() { return this.forwarderStandaloneShipmentId; }
    public set ForwarderStandaloneShipmentId(newValue: string) { if (this.forwarderStandaloneShipmentId != newValue) { this.forwarderStandaloneShipmentId = newValue; this.MarkAsDirty("ForwarderStandaloneShipmentId"); } }      

    private forwarderPickUpDeliveryType: string;
    public get ForwarderPickUpDeliveryType() { return this.forwarderPickUpDeliveryType; }
    public set ForwarderPickUpDeliveryType(newValue: string) { if (this.forwarderPickUpDeliveryType != newValue) { this.forwarderPickUpDeliveryType = newValue; this.MarkAsDirty("forwarderPickUpDeliveryType"); } }

    private plannedCargoReadyDate: Date;
    public get PlannedCargoReadyDate() { return this.plannedCargoReadyDate; }
    public set PlannedCargoReadyDate(newValue: Date) { if (this.plannedCargoReadyDate != newValue) { this.plannedCargoReadyDate = newValue; this.MarkAsDirty("PlannedCargoReadyDate"); } }

    private approvedCargoReadyDate: Date;
    public get ApprovedCargoReadyDate() { return this.approvedCargoReadyDate; }
    public set ApprovedCargoReadyDate(newValue: Date) { if (this.approvedCargoReadyDate != newValue) { this.approvedCargoReadyDate = newValue; this.MarkAsDirty("ApprovedCargoReadyDate"); } }

    private handlerUserId: string;
    public get HandlerUserId() { return this.handlerUserId; }
    public set HandlerUserId(newValue: string) { if (this.handlerUserId != newValue) { this.handlerUserId = newValue; this.MarkAsDirty("HandlerUserId"); } }

    private destinationWarehouseId: string;
    public get DestinationWarehouseId() { return this.destinationWarehouseId; }
    public set DestinationWarehouseId(newValue: string) { if (this.destinationWarehouseId != newValue) { this.destinationWarehouseId = newValue; this.MarkAsDirty("DestinationWarehouseId"); } }

    private inlandDomesticFromZipCode: string;
    public get InlandDomesticFromZipCode() { return this.inlandDomesticFromZipCode; }
    public set InlandDomesticFromZipCode(newValue: string) { if (this.inlandDomesticFromZipCode != newValue) { this.inlandDomesticFromZipCode = newValue; this.MarkAsDirty("InlandDomesticFromZipCode"); } }

    private inlandDomesticToZipCode: string;
    public get InlandDomesticToZipCode() { return this.inlandDomesticToZipCode; }
    public set InlandDomesticToZipCode(newValue: string) { if (this.inlandDomesticToZipCode != newValue) { this.inlandDomesticToZipCode = newValue; this.MarkAsDirty("InlandDomesticToZipCode"); } }

    private inlandDomesticFromCity: string;
    public get InlandDomesticFromCity() { return this.inlandDomesticFromCity; }
    public set InlandDomesticFromCity(newValue: string) { if (this.inlandDomesticFromCity != newValue) { this.inlandDomesticFromCity = newValue; this.MarkAsDirty("InlandDomesticFromCity"); } }

    private inlandDomesticToCity: string;
    public get InlandDomesticToCity() { return this.inlandDomesticToCity; }
    public set InlandDomesticToCity(newValue: string) { if (this.inlandDomesticToCity != newValue) { this.inlandDomesticToCity = newValue; this.MarkAsDirty("InlandDomesticToCity"); } }

    private inlandDomesticFromCountryId: string;
    public get InlandDomesticFromCountryId() { return this.inlandDomesticFromCountryId; }
    public set InlandDomesticFromCountryId(newValue: string) { if (this.inlandDomesticFromCountryId != newValue) { this.inlandDomesticFromCountryId = newValue; this.MarkAsDirty("InlandDomesticFromCountryId"); } }

    private inlandDomesticToCountryId: string;
    public get InlandDomesticToCountryId() { return this.inlandDomesticToCountryId; }
    public set InlandDomesticToCountryId(newValue: string) { if (this.inlandDomesticToCountryId != newValue) { this.inlandDomesticToCountryId = newValue; this.MarkAsDirty("InlandDomesticToCountryId"); } }

    private inlandDomesticFromTypeCode: string;
    public get InlandDomesticFromTypeCode() { return this.inlandDomesticFromTypeCode; }
    public set InlandDomesticFromTypeCode(newValue: string) { if (this.inlandDomesticFromTypeCode != newValue) { this.inlandDomesticFromTypeCode = newValue; this.MarkAsDirty("InlandDomesticFromTypeCode"); } }

    private inlandDomesticToTypeCode: string;
    public get InlandDomesticToTypeCode() { return this.inlandDomesticToTypeCode; }
    public set InlandDomesticToTypeCode(newValue: string) { if (this.inlandDomesticToTypeCode != newValue) { this.inlandDomesticToTypeCode = newValue; this.MarkAsDirty("InlandDomesticToTypeCode"); } }

    private mainCarriageFromPortAddress: string;
    public get MainCarriageFromPortAddress() { return this.mainCarriageFromPortAddress; }
    public set MainCarriageFromPortAddress(newValue: string) { if (this.mainCarriageFromPortAddress != newValue) { this.mainCarriageFromPortAddress = newValue; this.MarkAsDirty("MainCarriageFromPortAddress"); } }

    private mainCarriageToPortAddress: string;
    public get MainCarriageToPortAddress() { return this.mainCarriageToPortAddress; }
    public set MainCarriageToPortAddress(newValue: string) { if (this.mainCarriageToPortAddress != newValue) { this.mainCarriageToPortAddress = newValue; this.MarkAsDirty("MainCarriageToPortAddress"); } }

    private houseMasterConcurrencyGUID: string;
    public get HouseMasterConcurrencyGUID() { return this.houseMasterConcurrencyGUID; }
    public set HouseMasterConcurrencyGUID(newValue: string) { if (this.houseMasterConcurrencyGUID != newValue) { this.houseMasterConcurrencyGUID = newValue; this.MarkAsDirty("HouseMasterConcurrencyGUID"); } }

    private houseMasterNewConcurrencyGUID: string;
    public get HouseMasterNewConcurrencyGUID() { return this.houseMasterNewConcurrencyGUID; }
    public set HouseMasterNewConcurrencyGUID(newValue: string) { if (this.houseMasterNewConcurrencyGUID != newValue) { this.houseMasterNewConcurrencyGUID = newValue; this.MarkAsDirty("HouseMasterNewConcurrencyGUID"); } }

    private billingStatusId: string;
    public get BillingStatusId() { return this.billingStatusId; }
    public set BillingStatusId(newValue: string) { if (this.billingStatusId != newValue) { this.billingStatusId = newValue; this.MarkAsDirty("BillingStatusId"); } }

    private operationalStatusId: string;
    public get OperationalStatusId() { return this.operationalStatusId; }
    public set OperationalStatusId(newValue: string) { if (this.operationalStatusId != newValue) { this.operationalStatusId = newValue; this.MarkAsDirty("OperationalStatusId"); } }

    private operationalStatusName: string;
    public get OperationalStatusName() { return this.operationalStatusName; }
    public set OperationalStatusName(newValue: string) { if (this.operationalStatusName != newValue) { this.operationalStatusName = newValue; this.MarkAsDirty("OperationalStatusName"); } }

    private billingStatusName: string;
    public get BillingStatusName() { return this.billingStatusName; }
    public set BillingStatusName(newValue: string) { if (this.billingStatusName != newValue) { this.billingStatusName = newValue; this.MarkAsDirty("BillingStatusName"); } }

    private isShipmentOrder: boolean;
    public get IsShipmentOrder() { return this.isShipmentOrder; }
    public set IsShipmentOrder(newValue: boolean) { if (this.isShipmentOrder != newValue) { this.isShipmentOrder = newValue; this.MarkAsDirty("IsShipmentOrder"); } }

    private carrierServiceLineId: string;
    public get CarrierServiceLineId() { return this.carrierServiceLineId; }
    public set CarrierServiceLineId(newValue: string) { if (this.carrierServiceLineId != newValue) { this.carrierServiceLineId = newValue; this.MarkAsDirty("CarrierServiceLineId"); } }

    private inlandDomesticToAddress1: string;
    public get InlandDomesticToAddress1() { return this.inlandDomesticToAddress1; }
    public set InlandDomesticToAddress1(newValue: string) { if (this.inlandDomesticToAddress1 != newValue) { this.inlandDomesticToAddress1 = newValue; this.MarkAsDirty("InlandDomesticToAddress1"); } }

    private inlandDomesticToAddress2: string;
    public get InlandDomesticToAddress2() { return this.inlandDomesticToAddress2; }
    public set InlandDomesticToAddress2(newValue: string) { if (this.inlandDomesticToAddress2 != newValue) { this.inlandDomesticToAddress2 = newValue; this.MarkAsDirty("InlandDomesticToAddress2"); } }

    private inlandDomesticToPhone: string;
    public get InlandDomesticToPhone() { return this.inlandDomesticToPhone; }
    public set InlandDomesticToPhone(newValue: string) { if (this.inlandDomesticToPhone != newValue) { this.inlandDomesticToPhone = newValue; this.MarkAsDirty("InlandDomesticToPhone"); } }

    private inlandDomesticToFax: string;
    public get InlandDomesticToFax() { return this.inlandDomesticToFax; }
    public set InlandDomesticToFax(newValue: string) { if (this.inlandDomesticToFax != newValue) { this.inlandDomesticToFax = newValue; this.MarkAsDirty("InlandDomesticToFax"); } }

    private inlandDomesticToStateId: string;
    public get InlandDomesticToStateId() { return this.inlandDomesticToStateId; }
    public set InlandDomesticToStateId(newValue: string) { if (this.inlandDomesticToStateId != newValue) { this.inlandDomesticToStateId = newValue; this.MarkAsDirty("InlandDomesticToStateId"); } }

    private inlandDomesticFromAddress1: string;
    public get InlandDomesticFromAddress1() { return this.inlandDomesticFromAddress1; }
    public set InlandDomesticFromAddress1(newValue: string) { if (this.inlandDomesticFromAddress1 != newValue) { this.inlandDomesticFromAddress1 = newValue; this.MarkAsDirty("InlandDomesticFromAddress1"); } }

    private inlandDomesticFromAddress2: string;
    public get InlandDomesticFromAddress2() { return this.inlandDomesticFromAddress2; }
    public set InlandDomesticFromAddress2(newValue: string) { if (this.inlandDomesticFromAddress2 != newValue) { this.inlandDomesticFromAddress2 = newValue; this.MarkAsDirty("InlandDomesticFromAddress2"); } }

    private inlandDomesticFromPhone: string;
    public get InlandDomesticFromPhone() { return this.inlandDomesticFromPhone; }
    public set InlandDomesticFromPhone(newValue: string) { if (this.inlandDomesticFromPhone != newValue) { this.inlandDomesticFromPhone = newValue; this.MarkAsDirty("InlandDomesticFromPhone"); } }

    private inlandDomesticFromFax: string;
    public get InlandDomesticFromFax() { return this.inlandDomesticFromFax; }
    public set InlandDomesticFromFax(newValue: string) { if (this.inlandDomesticFromFax != newValue) { this.inlandDomesticFromFax = newValue; this.MarkAsDirty("InlandDomesticFromFax"); } }

    private inlandDomesticFromStateId: string;
    public get InlandDomesticFromStateId() { return this.inlandDomesticFromStateId; }
    public set InlandDomesticFromStateId(newValue: string) { if (this.inlandDomesticFromStateId != newValue) { this.inlandDomesticFromStateId = newValue; this.MarkAsDirty("InlandDomesticFromStateId"); } }


    private customChildEntities: CustomChildEntity[];
    public get CustomChildEntities() { return this.customChildEntities; }
    public set CustomChildEntities(newValue: CustomChildEntity[]) { if (this.customChildEntities != newValue) { this.customChildEntities = newValue; this.MarkAsDirty("CustomChildEntities"); } }

    private isINTTRAFROB: boolean;
    public get IsINTTRAFROB() { return this.isINTTRAFROB; }
    public set IsINTTRAFROB(newValue: boolean) { if (this.isINTTRAFROB != newValue) { this.isINTTRAFROB = newValue; this.MarkAsDirty("IsINTTRAFROB"); } }

    private shippingLine: string;
    public get ShippingLine() { return this.shippingLine; }
    public set ShippingLine(newValue: string) { if (this.shippingLine != newValue) { this.shippingLine = newValue; this.MarkAsDirty("ShippingLine"); } }

    private placeOfDelivery: string;
    public get PlaceOfDelivery() { return this.placeOfDelivery; }
    public set PlaceOfDelivery(newValue: string) { if (this.placeOfDelivery != newValue) { this.placeOfDelivery = newValue; this.MarkAsDirty("PlaceOfDelivery"); } }

    private pickupPlace: string;
    public get PickupPlace() { return this.pickupPlace; }
    public set PickupPlace(newValue: string) { if (this.pickupPlace != newValue) { this.pickupPlace = newValue; this.MarkAsDirty("PickupPlace"); } }

    private sealNo: string;
    public get SealNo() { return this.sealNo; }
    public set SealNo(newValue: string) { if (this.sealNo != newValue) { this.sealNo = newValue; this.MarkAsDirty("SealNo"); } }

    private hSCode: string;
    public get HSCode() { return this.hSCode; }
    public set HSCode(newValue: string) { if (this.hSCode != newValue) { this.hSCode = newValue; this.MarkAsDirty("HSCode"); } }

    private weight1: number;
    public get Weight1() { return this.weight1; }
    public set Weight1(newValue: number) { if (this.weight1 != newValue) { this.weight1 = newValue; this.MarkAsDirty("Weight1"); } }

    private weight2: number;
    public get Weight2() { return this.weight2; }
    public set Weight2(newValue: number) { if (this.weight2 != newValue) { this.weight2 = newValue; this.MarkAsDirty("Weight2"); } }

    private weight3: number;
    public get Weight3() { return this.weight3; }
    public set Weight3(newValue: number) { if (this.weight3 != newValue) { this.weight3 = newValue; this.MarkAsDirty("Weight3"); } }

    private weight4: number;
    public get Weight4() { return this.weight4; }
    public set Weight4(newValue: number) { if (this.weight4 != newValue) { this.weight4 = newValue; this.MarkAsDirty("Weight4"); } }

    private iskaNumber: string;
    public get IskaNumber() { return this.iskaNumber; }
    public set IskaNumber(newValue: string) { if (this.iskaNumber != newValue) { this.iskaNumber = newValue; this.MarkAsDirty("IskaNumber"); } }

    private referantUserId: string;
    public get ReferantUserId() { return this.referantUserId; }
    public set ReferantUserId(newValue: string) { if (this.referantUserId != newValue) { this.referantUserId = newValue; this.MarkAsDirty("ReferantUserId"); } }

    private referantUserName: string;
    public get ReferantUserName() { return this.referantUserName; }
    public set ReferantUserName(newValue: string) { if (this.referantUserName != newValue) { this.referantUserName = newValue; this.MarkAsDirty("ReferantUserName"); } }

    private declarationOfficeCode: string;
    public get DeclarationOfficeCode() { return this.declarationOfficeCode; }
    public set DeclarationOfficeCode(newValue: string) { if (this.declarationOfficeCode != newValue) { this.declarationOfficeCode = newValue; this.MarkAsDirty("DeclarationOfficeCode"); } }

    private declarationOfficeName: string;
    public get DeclarationOfficeName() { return this.declarationOfficeName; }
    public set DeclarationOfficeName(newValue: string) { if (this.declarationOfficeName != newValue) { this.declarationOfficeName = newValue; this.MarkAsDirty("DeclarationOfficeName"); } }

    private isCustomShipment: boolean;
    public get IsCustomShipment() { return this.isCustomShipment; }
    public set IsCustomShipment(newValue: boolean) { if (this.isCustomShipment != newValue) { this.isCustomShipment = newValue; this.MarkAsDirty("IsCustomShipment"); } }

    private shipmentReferances: ShipmentReferancePM[];
    public get ShipmentReferances() { return this.shipmentReferances; }
    public set ShipmentReferances(newValue: ShipmentReferancePM[]) { if (this.shipmentReferances != newValue) { this.shipmentReferances = newValue; this.MarkAsDirty("ShipmentReferances"); } }


    private carrierCode: string;
    public get CarrierCode() { return this.carrierCode; }
    public set CarrierCode(newValue: string) { if (this.carrierCode != newValue) { this.carrierCode = newValue; this.MarkAsDirty("CarrierCode"); } }

    private mawb: string;
    public get Mawb() { return this.mawb; }
    public set Mawb(newValue: string) { if (this.mawb != newValue) { this.mawb = newValue; this.MarkAsDirty("Mawb"); } }

    private mawbDate: Date;
    public get MawbDate() { return this.mawbDate; }
    public set MawbDate(newValue: Date) { if (this.mawbDate != newValue) { this.mawbDate = newValue; this.MarkAsDirty("MawbDate"); } }

    private estimatedArrivalDate: Date;
    public get EstimatedArrivalDate() { return this.estimatedArrivalDate; }
    public set EstimatedArrivalDate(newValue: Date) { if (this.estimatedArrivalDate != newValue) { this.estimatedArrivalDate = newValue; this.MarkAsDirty("EstimatedArrivalDate"); } }

    private packageTypeCode: string;
    public get PackageTypeCode() { return this.packageTypeCode; }
    public set PackageTypeCode(newValue: string) { if (this.packageTypeCode != newValue) { this.packageTypeCode = newValue; this.MarkAsDirty("PackageTypeCode"); } }

    private arrivalDate: Date;
    public get ArrivalDate() { return this.arrivalDate; }
    public set ArrivalDate(newValue: Date) { if (this.arrivalDate != newValue) { this.arrivalDate = newValue; this.MarkAsDirty("ArrivalDate"); } }

    private vessel: string;
    public get Vessel() { return this.vessel; }
    public set Vessel(newValue: string) { if (this.vessel != newValue) { this.vessel = newValue; this.MarkAsDirty("Vessel"); } }

    private flightVoyageNumber: string;
    public get FlightVoyageNumber() { return this.flightVoyageNumber; }
    public set FlightVoyageNumber(newValue: string) { if (this.flightVoyageNumber != newValue) { this.flightVoyageNumber = newValue; this.MarkAsDirty("FlightVoyageNumber"); } }

    private commodity: string;
    public get Commodity() { return this.commodity; }
    public set Commodity(newValue: string) { if (this.commodity != newValue) { this.commodity = newValue; this.MarkAsDirty("Commodity"); } }

    private hatraDate: Date;
    public get HatraDate() { return this.hatraDate; }
    public set HatraDate(newValue: Date) { if (this.hatraDate != newValue) { this.hatraDate = newValue; this.MarkAsDirty("HatraDate"); } }

    private procedureCurrentCode: string;
    public get ProcedureCurrentCode() { return this.procedureCurrentCode; }
    public set ProcedureCurrentCode(newValue: string) { if (this.procedureCurrentCode != newValue) { this.procedureCurrentCode = newValue; this.MarkAsDirty("ProcedureCurrentCode"); } }

    private externalDeclarationNumber: string;
    public get ExternalDeclarationNumber() { return this.externalDeclarationNumber; }
    public set ExternalDeclarationNumber(newValue: string) { if (this.externalDeclarationNumber != newValue) { this.externalDeclarationNumber = newValue; this.MarkAsDirty("ExternalDeclarationNumber"); } }

    private declarationStatusTypeCode: string;
    public get DeclarationStatusTypeCode() { return this.declarationStatusTypeCode; }
    public set DeclarationStatusTypeCode(newValue: string) { if (this.declarationStatusTypeCode != newValue) { this.declarationStatusTypeCode = newValue; this.MarkAsDirty("DeclarationStatusTypeCode"); } }

    private carrierCodeMawb: string;
    public get CarrierCodeMawb() { return this.carrierCodeMawb; }
    public set CarrierCodeMawb(newValue: string) { if (this.carrierCodeMawb != newValue) { this.carrierCodeMawb = newValue; this.MarkAsDirty("CarrierCodeMawb"); } }
    
    private freightForwarderReferences: FreightForwarderReferencePM[];
    public get FreightForwarderReferences() { return this.freightForwarderReferences; }
    public set FreightForwarderReferences(newValue: FreightForwarderReferencePM[]) { if (this.freightForwarderReferences != newValue) { this.freightForwarderReferences = newValue; this.MarkAsDirty("FreightForwarderReferences"); } }


    public OldEntityPM: ShipmentPM;

    private aWBOCIPMs: AWBOCIPM[];
    get AWBOCIPMs() {
        if (this.aWBOCIPMs == null) {
            this.aWBOCIPMs = [];
        }

        return this.aWBOCIPMs;
    }
    set AWBOCIPMs(newValue: AWBOCIPM[]) {
        if (this.aWBOCIPMs != newValue) {
            this.aWBOCIPMs = newValue;
        }
    }
    public AddOCI(item: AWBOCIPM) {
        if (item != null) {
            var index = this.AWBOCIPMs.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.AWBOCIPMs.push(item);
                this.MarkAsDirty();
            }
        }
    }
    public RemoveOCI(item: AWBOCIPM) {
        if (item != null) {
            var index = this.AWBOCIPMs.indexOf(item);
            if (index > -1) {
                this.AWBOCIPMs.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    }

    private shipmentPackages: ShipmentPackagePM[];
    get ShipmentPackages() {
        if (this.shipmentPackages == null) {
            this.shipmentPackages = [];
        }

        return this.shipmentPackages;
    }
    set ShipmentPackages(newValue: ShipmentPackagePM[]) {
        if (this.shipmentPackages != newValue) {
            this.shipmentPackages = newValue;
        }
    }
    public AddPackage(item: ShipmentPackagePM) {
        if (item != null) {
            var index = this.ShipmentPackages.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.ShipmentPackages.push(item);
                this.MarkAsDirty();
            }
        }
    }
    public RemovePackage(item: ShipmentPackagePM) {
        if (item != null) {
            var index = this.ShipmentPackages.indexOf(item);
            if (index > -1) {
                this.ShipmentPackages.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    }

    private shipmentCommodities: ShipmentCommodityPM[];
    get ShipmentCommodities() {
        if (this.shipmentCommodities == null) {
            this.shipmentCommodities = [];
        }

        return this.shipmentCommodities;
    }
    set ShipmentCommodities(newValue: ShipmentCommodityPM[]) {
        if (this.shipmentCommodities != newValue) {
            this.shipmentCommodities = newValue;
        }
    }
    public AddCommodity(item: ShipmentCommodityPM) {
        if (item != null) {
            var index = this.ShipmentCommodities.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.ShipmentCommodities.push(item);
                this.MarkAsDirty();
            }
        }
    }
    public RemoveCommodity(item: ShipmentCommodityPM) {
        if (item != null) {
            var index = this.ShipmentCommodities.indexOf(item);
            if (index > -1) {
                this.ShipmentCommodities.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    }

    private shipmentOrderPackages: ShipmentOrderPackagePM[];
    get ShipmentOrderPackages() {
        if (this.shipmentOrderPackages == null) {
            this.shipmentOrderPackages = [];
        }

        return this.shipmentOrderPackages;
    }
    set ShipmentOrderPackages(newValue: ShipmentOrderPackagePM[]) {
        if (this.shipmentOrderPackages != newValue) {
            this.shipmentOrderPackages = newValue;
        }
    }
    public AddOrderPackage(item: ShipmentOrderPackagePM) {
        if (item != null) {
            var index = this.ShipmentOrderPackages.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.ShipmentOrderPackages.push(item);
                this.MarkAsDirty();
            }
        }
    }
    public RemoveOrderPackage(item: ShipmentOrderPackagePM) {
        if (item != null) {
            var index = this.ShipmentOrderPackages.indexOf(item);
            if (index > -1) {
                this.ShipmentOrderPackages.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    }

    private shipmentPayables: ShipmentPayablePM[];
    get ShipmentPayables() {
        if (this.shipmentPayables == null) {
            this.shipmentPayables = [];
        }

        return this.shipmentPayables;
    }
    set ShipmentPayables(newValue: ShipmentPayablePM[]) {
        if (this.shipmentPayables != newValue) {
            this.shipmentPayables = newValue;
        }
    }
    public AddPayable(item: ShipmentPayablePM) {
        if (item != null) {
            var index = this.ShipmentPayables.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.ShipmentPayables.push(item);
                this.MarkAsDirty();
            }
        }
    }
    public RemovePayable(item: ShipmentPayablePM) {
        if (item != null) {
            var index = this.ShipmentPayables.indexOf(item);
            if (index > -1) {
                this.ShipmentPayables.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    }

    private shipmentReceivables: ShipmentReceivablePM[];
    get ShipmentReceivables() {
        if (this.shipmentReceivables == null) {
            this.shipmentReceivables = [];
        }

        return this.shipmentReceivables;
    }
    set ShipmentReceivables(newValue: ShipmentReceivablePM[]) {
        if (this.shipmentReceivables != newValue) {
            this.shipmentReceivables = newValue;
        }
    }
    public AddReceivable(item: ShipmentReceivablePM) {
        if (item != null) {
            var index = this.ShipmentReceivables.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.ShipmentReceivables.push(item);
                this.MarkAsDirty();
            }
        }
    }
    public RemoveReceivable(item: ShipmentReceivablePM) {
        if (item != null) {
            var index = this.ShipmentReceivables.indexOf(item);
            if (index > -1) {
                this.ShipmentReceivables.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    }

    private shipmentAWBPrintOnlies: ShipmentAWBPrintOnlyPM[];
    get ShipmentAWBPrintOnlies() {
        if (this.shipmentAWBPrintOnlies == null) {
            this.shipmentAWBPrintOnlies = [];
        }

        return this.shipmentAWBPrintOnlies;
    }
    set ShipmentAWBPrintOnlies(newValue: ShipmentAWBPrintOnlyPM[]) {
        if (this.shipmentAWBPrintOnlies != newValue) {
            this.shipmentAWBPrintOnlies = newValue;
        }
    }
    public AddAWBPrintOnly(item: ShipmentAWBPrintOnlyPM) {
        if (item != null) {
            var index = this.ShipmentAWBPrintOnlies.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.ShipmentAWBPrintOnlies.push(item);
                this.MarkAsDirty();
            }
        }
    }
    public RemoveAWBPrintOnly(item: ShipmentAWBPrintOnlyPM) {
        if (item != null) {
            var index = this.ShipmentAWBPrintOnlies.indexOf(item);
            if (index > -1) {
                this.ShipmentAWBPrintOnlies.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    }

    private shipmentConsoleShipments: ConsoleShipmentPM[];
    get ShipmentConsoleShipments() {
        if (this.shipmentConsoleShipments == null) {
            this.shipmentConsoleShipments = [];
        }

        return this.shipmentConsoleShipments;
    }
    set ShipmentConsoleShipments(newValue: ConsoleShipmentPM[]) {
        if (this.shipmentConsoleShipments != newValue) {
            this.shipmentConsoleShipments = newValue;
            //this.MarkAsDirty();
        }
    }
    public AddConsoleShipment(item: ConsoleShipmentPM) {
        if (item != null) {
            var index = this.ShipmentConsoleShipments.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.ShipmentConsoleShipments.push(item);
                this.MarkAsDirty();
            }
        }
    }
    public RemoveConsoleShipment(item: ConsoleShipmentPM) {
        if (item != null) {
            var index = this.ShipmentConsoleShipments.indexOf(item);
            if (index > -1) {
                this.ShipmentConsoleShipments.splice(index, 1);
                this.MasterShipmentDataId = null;
                this.MarkAsDirty();
            }
        }
    }

    private shipmentPickUps: ShipmentPickUpPM[];
    get ShipmentPickUps() {
        if (this.shipmentPickUps == null) {
            this.shipmentPickUps = [];
        }

        return this.shipmentPickUps;
    }
    set ShipmentPickUps(newValue: ShipmentPickUpPM[]) {
        if (this.shipmentPickUps != newValue) {
            this.shipmentPickUps = newValue;
        }
    }
    public AddPickUp(item: ShipmentPickUpPM) {
        if (item != null) {
            var index = this.ShipmentPickUps.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.ShipmentPickUps.push(item);
                this.MarkAsDirty();
            }
        }
    }
    public RemovePickUp(item: ShipmentPickUpPM) {
        if (item != null) {
            var index = this.ShipmentPickUps.indexOf(item);
            if (index > -1) {
                this.ShipmentPickUps.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    }

    private shipmentDeliveries: ShipmentDeliveryPM[];
    get ShipmentDeliveries() {
        if (this.shipmentDeliveries == null) {
            this.shipmentDeliveries = [];
        }

        return this.shipmentDeliveries;
    }
    set ShipmentDeliveries(newValue: ShipmentDeliveryPM[]) {
        if (this.shipmentDeliveries != newValue) {
            this.shipmentDeliveries = newValue;
        }
    }
    public AddDelivery(item: ShipmentDeliveryPM) {
        if (item != null) {
            var index = this.ShipmentDeliveries.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.ShipmentDeliveries.push(item);
                this.MarkAsDirty();
            }
        }
    }
    public RemoveDelivery(item: ShipmentDeliveryPM) {
        if (item != null) {
            var index = this.ShipmentDeliveries.indexOf(item);
            if (index > -1) {
                this.ShipmentDeliveries.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    }

    private followUps: ShipmentFollowUpPM[];
    get FollowUps() {
        if (this.followUps == null) {
            this.followUps = [];
        }

        return this.followUps;
    }
    set FollowUps(newValue: ShipmentFollowUpPM[]) {
        if (this.followUps != newValue) {
            this.followUps = newValue;
        }
    }
    public AddShipmentFollowUp(item: ShipmentFollowUpPM) {
        if (item != null) {
            var index = this.FollowUps.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.FollowUps.push(item);
                this.MarkAsDirty();
            }
        }
    }
    public RemoveShipmentFollowUp(item: ShipmentFollowUpPM) {
        if (item != null) {
            var index = this.FollowUps.indexOf(item);
            if (index > -1) {
                this.FollowUps.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    }

    private shipmentAssemblies: ShipmentAssemblyPM[];
    get ShipmentAssemblies() {
        if (this.shipmentAssemblies == null) {
            this.shipmentAssemblies = [];
        }

        return this.shipmentAssemblies;
    }
    set ShipmentAssemblies(newValue: ShipmentAssemblyPM[]) {
        if (this.shipmentAssemblies != newValue) {
            this.shipmentAssemblies = newValue;
        }
    }
    public AddAssembly(item: ShipmentAssemblyPM) {
        if (item != null) {
            var index = this.ShipmentAssemblies.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.ShipmentAssemblies.push(item);
                this.MarkAsDirty();
            }
        }
    }
    public RemoveAssembly(item: ShipmentAssemblyPM) {
        if (item != null) {
            var index = this.ShipmentAssemblies.indexOf(item);
            if (index > -1) {
                this.ShipmentAssemblies.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    }

    private shipmentStoragePricings: ShipmentStoragePricingPM[];
    get ShipmentStoragePricings() {
        if (this.shipmentStoragePricings == null) {
            this.shipmentStoragePricings = [];
        }

        return this.shipmentStoragePricings;
    }
    set ShipmentStoragePricings(newValue: ShipmentStoragePricingPM[]) {
        if (this.shipmentStoragePricings != newValue) {
            this.shipmentStoragePricings = newValue;
        }
    }
    
    public AddShipmentStoragePricing(item: ShipmentStoragePricingPM) {
        if (item != null) {
            var index = this.ShipmentStoragePricings.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.ShipmentStoragePricings.push(item);
                this.MarkAsDirty();
            }
        }
    }
    public RemoveShipmentStoragePricing(item: ShipmentStoragePricingPM) {
        if (item != null) {
            var index = this.ShipmentStoragePricings.indexOf(item);
            if (index > -1) {
                this.ShipmentStoragePricings.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    }

    private shipmentProductItems: ShipmentProductItemPM[];
    get ShipmentProductItems() {
        if (this.shipmentProductItems == null) {
            this.shipmentProductItems = [];
        }

        return this.shipmentProductItems;
    }
    set ShipmentProductItems(newValue: ShipmentProductItemPM[]) {
        if (this.shipmentProductItems != newValue) {
            this.shipmentProductItems = newValue;
        }
    }
    public AddProductItem(item: ShipmentProductItemPM) {
        if (item != null) {
            var index = this.ShipmentProductItems.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.ShipmentProductItems.push(item);
                this.MarkAsDirty();
            }
        }
    }
    public RemoveProductItem(item: ShipmentProductItemPM) {
        if (item != null) {
            var index = this.ShipmentProductItems.indexOf(item);
            if (index > -1) {
                this.ShipmentProductItems.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    }

    private shipmentUnassignedFields: ShipmentUnassignedFieldPM[];
    get ShipmentUnassignedFields() {
        if (this.shipmentUnassignedFields == null) {
            this.shipmentUnassignedFields = [];
        }

        return this.shipmentUnassignedFields;
    }
    set ShipmentUnassignedFields(newValue: ShipmentUnassignedFieldPM[]) {
        if (this.shipmentUnassignedFields != newValue) {
            this.shipmentUnassignedFields = newValue;
        }
    }
    public AddShipmentUnassignedField(item: ShipmentUnassignedFieldPM) {
        if (item != null) {
            var index = this.shipmentUnassignedFields.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.shipmentUnassignedFields.push(item);
                this.MarkAsDirty();
            }
        }
    }
    public RemoveShipmentUnassignedFields(item: ShipmentUnassignedFieldPM) {
        if (item != null) {
            var index = this.shipmentUnassignedFields.indexOf(item);
            if (index > -1) {
                this.shipmentUnassignedFields.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    }

    private connectedMasterPackages: ShipmentPackagePM[];
    get ConnectedMasterPackages() {
        if (this.connectedMasterPackages == null) {
            this.connectedMasterPackages = [];
        }

        return this.connectedMasterPackages;
    }
    set ConnectedMasterPackages(newValue: ShipmentPackagePM[]) {
        if (this.connectedMasterPackages != newValue) {
            this.connectedMasterPackages = newValue;
        }
    }

    public ShipmentARInvoices: Array<any>;
    public ShipmentAPInvoices: Array<any>;
    public ShipmentCarrierStatuses: Array<any>;

    public DisableMarkAsDirty: boolean = false;
    MarkAsDirty(propertyName: string = null) {
        if (!this.DisableMarkAsDirty) {
            this.IsDirty = true;
            if (propertyName != null) {
                this.PropertyChanged.emit(new PropertyChangedArgs(propertyName, this));
                ShipmentPMCustomCode.ApplyEntityChanged(propertyName, this);
                ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "Shipment");
            }
        }
    }

    private MyClone: ShipmentPM;
    public CloneMe() {
        ServiceHelper.CloneEntityPM(this);
    }
    public RejectChanges() {
        ServiceHelper.RejectEntityPMChanges(this);
    }
}
