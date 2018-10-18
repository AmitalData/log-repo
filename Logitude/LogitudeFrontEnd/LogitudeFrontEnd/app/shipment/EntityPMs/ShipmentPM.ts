import {UIProperties, UIProperty} from '../../infrastructure/logitude-components/UIProperties';
import {ShipmentPackagePM} from './ShipmentPackagePM';
export class ShipmentPM {
    

    public UIProperties: UIProperties;

    constructor() {
        this.UIProperties = new UIProperties;
        this.IsDirty = false;
    }

    public IsDirty: boolean;


        private _Id: string;
        public get Id() { return this._Id; }
        public set Id(newValue: string) { this._Id = newValue; this.MarkAsDirty(); }

        private _IsHybrid: boolean;
        public get IsHybrid() { return this._IsHybrid; }
        public set IsHybrid(newValue: boolean) { this._IsHybrid = newValue; this.MarkAsDirty(); }

        private _BaseShipmentNumber: string;
        public get BaseShipmentNumber() { return this._BaseShipmentNumber; }
        public set BaseShipmentNumber(newValue: string) { this._BaseShipmentNumber = newValue; this.MarkAsDirty(); }

        private _ConcurrencyGUID: string;
        public get ConcurrencyGUID() { return this._ConcurrencyGUID; }
        public set ConcurrencyGUID(newValue: string) { this._ConcurrencyGUID = newValue; this.MarkAsDirty(); }

        private _CASSCode: string;
        public get CASSCode() { return this._CASSCode; }
        public set CASSCode(newValue: string) { this._CASSCode = newValue; this.MarkAsDirty(); }

        private _DeliveryOrder: string;
        public get DeliveryOrder() { return this._DeliveryOrder; }
        public set DeliveryOrder(newValue: string) { this._DeliveryOrder = newValue; this.MarkAsDirty(); }

        private _ImportManifest: string;
        public get ImportManifest() { return this._ImportManifest; }
        public set ImportManifest(newValue: string) { this._ImportManifest = newValue; this.MarkAsDirty(); }

        private _FreightLocationId: string;
        public get FreightLocationId() { return this._FreightLocationId; }
        public set FreightLocationId(newValue: string) { this._FreightLocationId = newValue; this.MarkAsDirty(); }

        private _TransportDocumentNumber: string;
        public get TransportDocumentNumber() { return this._TransportDocumentNumber; }
        public set TransportDocumentNumber(newValue: string) { this._TransportDocumentNumber = newValue; this.MarkAsDirty(); }

        private _CarrierTransportDocumentNumber: string;
        public get CarrierTransportDocumentNumber() { return this._CarrierTransportDocumentNumber; }
        public set CarrierTransportDocumentNumber(newValue: string) { this._CarrierTransportDocumentNumber = newValue; this.MarkAsDirty(); }


        private _OpenReceivablesInLocalCurrency: number;
        public get OpenReceivablesInLocalCurrency() { return this._OpenReceivablesInLocalCurrency; }
        public set OpenReceivablesInLocalCurrency(newValue: number) { this._OpenReceivablesInLocalCurrency = newValue; this.MarkAsDirty(); }

        private _AccountedReceivablesInLocalCurrency: number;
        public get AccountedReceivablesInLocalCurrency() { return this._AccountedReceivablesInLocalCurrency; }
        public set AccountedReceivablesInLocalCurrency(newValue: number) { this._AccountedReceivablesInLocalCurrency = newValue; this.MarkAsDirty(); }

        private _ProfitInLocalCurrency: number;
        public get ProfitInLocalCurrency() { return this._ProfitInLocalCurrency; }
        public set ProfitInLocalCurrency(newValue: number) { this._ProfitInLocalCurrency = newValue; this.MarkAsDirty(); }

        private _EstimateProfitInLocalCurrency: number;
        public get EstimateProfitInLocalCurrency() { return this._EstimateProfitInLocalCurrency; }
        public set EstimateProfitInLocalCurrency(newValue: number) { this._EstimateProfitInLocalCurrency = newValue; this.MarkAsDirty(); }

        private _OpenReceivablesInProfitCurrency: number;
        public get OpenReceivablesInProfitCurrency() { return this._OpenReceivablesInProfitCurrency; }
        public set OpenReceivablesInProfitCurrency(newValue: number) { this._OpenReceivablesInProfitCurrency = newValue; this.MarkAsDirty(); }

        private _AccountedReceivablesInProfitCurrency: number;
        public get AccountedReceivablesInProfitCurrency() { return this._AccountedReceivablesInProfitCurrency; }
        public set AccountedReceivablesInProfitCurrency(newValue: number) { this._AccountedReceivablesInProfitCurrency = newValue; this.MarkAsDirty(); }

        private _ProfitInProfitCurrency: number;
        public get ProfitInProfitCurrency() { return this._ProfitInProfitCurrency; }
        public set ProfitInProfitCurrency(newValue: number) { this._ProfitInProfitCurrency = newValue; this.MarkAsDirty(); }

        private _EstimateProfitInProfitCurrency: number;
        public get EstimateProfitInProfitCurrency() { return this._EstimateProfitInProfitCurrency; }
        public set EstimateProfitInProfitCurrency(newValue: number) { this._EstimateProfitInProfitCurrency = newValue; this.MarkAsDirty(); }

        private _OpenPayablesInLocalCurrency: number;
        public get OpenPayablesInLocalCurrency() { return this._OpenPayablesInLocalCurrency; }
        public set OpenPayablesInLocalCurrency(newValue: number) { this._OpenPayablesInLocalCurrency = newValue; this.MarkAsDirty(); }

        private _AccountedPayablesInLocalCurrency: number;
        public get AccountedPayablesInLocalCurrency() { return this._AccountedPayablesInLocalCurrency; }
        public set AccountedPayablesInLocalCurrency(newValue: number) { this._AccountedPayablesInLocalCurrency = newValue; this.MarkAsDirty(); }

        private _OpenPayablesInProfitCurrency: number;
        public get OpenPayablesInProfitCurrency() { return this._OpenPayablesInProfitCurrency; }
        public set OpenPayablesInProfitCurrency(newValue: number) { this._OpenPayablesInProfitCurrency = newValue; this.MarkAsDirty(); }

        private _AccountedPayablesInProfitCurrency: number;
        public get AccountedPayablesInProfitCurrency() { return this._AccountedPayablesInProfitCurrency; }
        public set AccountedPayablesInProfitCurrency(newValue: number) { this._AccountedPayablesInProfitCurrency = newValue; this.MarkAsDirty(); }

        private _CountryForStatisticsId: string;
        public get CountryForStatisticsId() { return this._CountryForStatisticsId; }
        public set CountryForStatisticsId(newValue: string) { this._CountryForStatisticsId = newValue; this.MarkAsDirty(); }

        private _MainCarriageCarrierId: string;
        public get MainCarriageCarrierId() { return this._MainCarriageCarrierId; }
        public set MainCarriageCarrierId(newValue: string) { this._MainCarriageCarrierId = newValue; this.MarkAsDirty(); }


        private _MainCarriageCarrierName: string;
        public get MainCarriageCarrierName() { return this._MainCarriageCarrierName; }
        public set MainCarriageCarrierName(newValue: string) { this._MainCarriageCarrierName = newValue; this.MarkAsDirty(); }


        private _MainCarriageCarrierCode: string;
        public get MainCarriageCarrierCode() { return this._MainCarriageCarrierCode; }
        public set MainCarriageCarrierCode(newValue: string) { this._MainCarriageCarrierCode = newValue; this.MarkAsDirty(); }


        private _MainCarriageCarrierNumber: string;
        public get MainCarriageCarrierNumber() { return this._MainCarriageCarrierNumber; }
        public set MainCarriageCarrierNumber(newValue: string) { this._MainCarriageCarrierNumber = newValue; this.MarkAsDirty(); }


        private _MainCarriageCarrierAddressId: string;
        public get MainCarriageCarrierAddressId() { return this._MainCarriageCarrierAddressId; }
        public set MainCarriageCarrierAddressId(newValue: string) { this._MainCarriageCarrierAddressId = newValue; this.MarkAsDirty(); }


        private _MainCarriageCarrierWebSite: string;
        public get MainCarriageCarrierWebSite() { return this._MainCarriageCarrierWebSite; }
        public set MainCarriageCarrierWebSite(newValue: string) { this._MainCarriageCarrierWebSite = newValue; this.MarkAsDirty(); }


        private _IsFSRSent: boolean;
        public get IsFSRSent() { return this._IsFSRSent; }
        public set IsFSRSent(newValue: boolean) { this._IsFSRSent = newValue; this.MarkAsDirty(); }


        private _LastFSRStatusRequestDate: Date;
        public get LastFSRStatusRequestDate() { return this._LastFSRStatusRequestDate; }
        public set LastFSRStatusRequestDate(newValue: Date) { this._LastFSRStatusRequestDate = newValue; this.MarkAsDirty(); }


        private _FHLStatusDate: Date;
        public get FHLStatusDate() { return this._FHLStatusDate; }
        public set FHLStatusDate(newValue: Date) { this._FHLStatusDate = newValue; this.MarkAsDirty(); }


        private _FWBStatusDate: Date;
        public get FWBStatusDate() { return this._FWBStatusDate; }
        public set FWBStatusDate(newValue: Date) { this._FWBStatusDate = newValue; this.MarkAsDirty(); }


        private _CarrierLastStatusCode: string;
        public get CarrierLastStatusCode() { return this._CarrierLastStatusCode; }
        public set CarrierLastStatusCode(newValue: string) { this._CarrierLastStatusCode = newValue; this.MarkAsDirty(); }


        private _CarrierLastStatusName: string;
        public get CarrierLastStatusName() { return this._CarrierLastStatusName; }
        public set CarrierLastStatusName(newValue: string) { this._CarrierLastStatusName = newValue; this.MarkAsDirty(); }


        private _CarrierLastStatusDate: Date;
        public get CarrierLastStatusDate() { return this._CarrierLastStatusDate; }
        public set CarrierLastStatusDate(newValue: Date) { this._CarrierLastStatusDate = newValue; this.MarkAsDirty(); }


        private _FNAReason: string;
        public get FNAReason() { return this._FNAReason; }
        public set FNAReason(newValue: string) { this._FNAReason = newValue; this.MarkAsDirty(); }


        private _AWBSpecialHandlingCodeId1: string;
        public get AWBSpecialHandlingCodeId1() { return this._AWBSpecialHandlingCodeId1; }
        public set AWBSpecialHandlingCodeId1(newValue: string) { this._AWBSpecialHandlingCodeId1 = newValue; this.MarkAsDirty(); }


        private _AWBSpecialHandlingCodeId2: string;
        public get AWBSpecialHandlingCodeId2() { return this._AWBSpecialHandlingCodeId2; }
        public set AWBSpecialHandlingCodeId2(newValue: string) { this._AWBSpecialHandlingCodeId2 = newValue; this.MarkAsDirty(); }


        private _AWBSpecialHandlingCodeId3: string;
        public get AWBSpecialHandlingCodeId3() { return this._AWBSpecialHandlingCodeId3; }
        public set AWBSpecialHandlingCodeId3(newValue: string) { this._AWBSpecialHandlingCodeId3 = newValue; this.MarkAsDirty(); }


        private _AWBSpecialHandlingCodeId4: string;
        public get AWBSpecialHandlingCodeId4() { return this._AWBSpecialHandlingCodeId4; }
        public set AWBSpecialHandlingCodeId4(newValue: string) { this._AWBSpecialHandlingCodeId4 = newValue; this.MarkAsDirty(); }


        private _AWBSpecialHandlingCodeId5: string;
        public get AWBSpecialHandlingCodeId5() { return this._AWBSpecialHandlingCodeId5; }
        public set AWBSpecialHandlingCodeId5(newValue: string) { this._AWBSpecialHandlingCodeId5 = newValue; this.MarkAsDirty(); }


        private _AWBSpecialHandlingCodeId6: string;
        public get AWBSpecialHandlingCodeId6() { return this._AWBSpecialHandlingCodeId6; }
        public set AWBSpecialHandlingCodeId6(newValue: string) { this._AWBSpecialHandlingCodeId6 = newValue; this.MarkAsDirty(); }


        private _AWBSpecialHandlingCodeId7: string;
        public get AWBSpecialHandlingCodeId7() { return this._AWBSpecialHandlingCodeId7; }
        public set AWBSpecialHandlingCodeId7(newValue: string) { this._AWBSpecialHandlingCodeId7 = newValue; this.MarkAsDirty(); }


        private _AWBSpecialHandlingCodeId8: string;
        public get AWBSpecialHandlingCodeId8() { return this._AWBSpecialHandlingCodeId8; }
        public set AWBSpecialHandlingCodeId8(newValue: string) { this._AWBSpecialHandlingCodeId8 = newValue; this.MarkAsDirty(); }

        private _AWBSpecialHandlingCodeId9: string;
        public get AWBSpecialHandlingCodeId9() { return this._AWBSpecialHandlingCodeId9; }
        public set AWBSpecialHandlingCodeId9(newValue: string) { this._AWBSpecialHandlingCodeId9 = newValue; this.MarkAsDirty(); }


        private _AWBChargeRate: number;
        public get AWBChargeRate() { return this._AWBChargeRate; }
        public set AWBChargeRate(newValue: number) { this._AWBChargeRate = newValue; this.MarkAsDirty(); }


        private _AWBChargeAmount: number;
        public get AWBChargeAmount() { return this._AWBChargeAmount; }
        public set AWBChargeAmount(newValue: number) { this._AWBChargeAmount = newValue; this.MarkAsDirty(); }


        private _AWBCommodityItemNumber: string;
        public get AWBCommodityItemNumber() { return this._AWBCommodityItemNumber; }
        public set AWBCommodityItemNumber(newValue: string) { this._AWBCommodityItemNumber = newValue; this.MarkAsDirty(); }


        private _PPCC: string;
        public get PPCC() { return this._PPCC; }
        public set PPCC(newValue: string) { this._PPCC = newValue; this.MarkAsDirty(); }


        private _FlightDate: Date;
        public get FlightDate() { return this._FlightDate; }
        public set FlightDate(newValue: Date) { this._FlightDate = newValue; this.MarkAsDirty(); }



        private _IsFlightDateActual: boolean;
        public get IsFlightDateActual() { return this._IsFlightDateActual; }
        public set IsFlightDateActual(newValue: boolean) { this._IsFlightDateActual = newValue; this.MarkAsDirty(); }


        private _ConnectedShipments: number;
        public get ConnectedShipments() { return this._ConnectedShipments; }
        public set ConnectedShipments(newValue: number) { this._ConnectedShipments = newValue; this.MarkAsDirty(); }


        private _MasterShipmentNumber: string;
        public get MasterShipmentNumber() { return this._MasterShipmentNumber; }
        public set MasterShipmentNumber(newValue: string) { this._MasterShipmentNumber = newValue; this.MarkAsDirty(); }


        private _ChargeableWeightInKG: number;
        public get ChargeableWeightInKG() { return this._ChargeableWeightInKG; }
        public set ChargeableWeightInKG(newValue: number) { this._ChargeableWeightInKG = newValue; this.MarkAsDirty(); }


        private _GrossWeightInKG: number;
        public get GrossWeightInKG() { return this._GrossWeightInKG; }
        public set GrossWeightInKG(newValue: number) { this._GrossWeightInKG = newValue; this.MarkAsDirty(); }


        private _ChargeableWeight: number;
        public get ChargeableWeight() { return this._ChargeableWeight; }
        public set ChargeableWeight(newValue: number) { this._ChargeableWeight = newValue; this.MarkAsDirty(); }


        private _GrossWeight: number;
        public get GrossWeight() { return this._GrossWeight; }
        public set GrossWeight(newValue: number) { this._GrossWeight = newValue; this.MarkAsDirty(); }


        private _CurrentUserId: string;
        public get CurrentUserId() { return this._CurrentUserId; }
        public set CurrentUserId(newValue: string) { this._CurrentUserId = newValue; this.MarkAsDirty(); }

        private _Tenant: number;
        public get Tenant() { return this._Tenant; }
        public set Tenant(newValue: number) { this._Tenant = newValue; this.MarkAsDirty(); }



        private _BasketId: string;
        public get BasketId() { return this._BasketId; }
        public set BasketId(newValue: string) { this._BasketId = newValue; this.MarkAsDirty(); }


        private _ShipmentNumber: string;
        public get ShipmentNumber() { return this._ShipmentNumber; }
        public set ShipmentNumber(newValue: string) { this._ShipmentNumber = newValue; this.MarkAsDirty(); }


        private _DirectionId: string;
        public get DirectionId() { return this._DirectionId; }
        public set DirectionId(newValue: string) { this._DirectionId = newValue; this.MarkAsDirty(); }


        private _DirectionName: string;
        public get DirectionName() { return this._DirectionName; }
        public set DirectionName(newValue: string) { this._DirectionName = newValue; this.MarkAsDirty(); }

        private _TransportModeId: string;
        public get TransportModeId() { return this._TransportModeId; }
        public set TransportModeId(newValue: string) { this._TransportModeId = newValue; this.MarkAsDirty(); }

        private _TransportModeName: string;
        public get TransportModeName() { return this._TransportModeName; }
        public set TransportModeName(newValue: string) { this._TransportModeName = newValue; this.MarkAsDirty(); }

        private _ShipmentTypeId: string;
        public get ShipmentTypeId() { return this._ShipmentTypeId; }
        public set ShipmentTypeId(newValue: string) { this._ShipmentTypeId = newValue; this.MarkAsDirty(); }

        private _ShipmentTypeName: string;
        public get ShipmentTypeName() { return this._ShipmentTypeName; }
        public set ShipmentTypeName(newValue: string) { this._ShipmentTypeName = newValue; this.MarkAsDirty(); }

        private _House: string;
        public get House() { return this._House; }
        public set House(newValue: string) { this._House = newValue; this.MarkAsDirty(); }

        private _CreateDateTime: Date;
        public get CreateDateTime() { return this._CreateDateTime; }
        public set CreateDateTime(newValue: Date) { this._CreateDateTime = newValue; this.MarkAsDirty(); }

        private _MAWBTakenFromStack: boolean;
        public get MAWBTakenFromStack() { return this._MAWBTakenFromStack; }
        public set MAWBTakenFromStack(newValue: boolean) { this._MAWBTakenFromStack = newValue; this.MarkAsDirty(); }

        private _MAWBReturnedToStack: boolean;
        public get MAWBReturnedToStack() { return this._MAWBReturnedToStack; }
        public set MAWBReturnedToStack(newValue: boolean) { this._MAWBReturnedToStack = newValue; this.MarkAsDirty(); }


        private _BranchId: string;
        public get BranchId() { return this._BranchId; }
        public set BranchId(newValue: string) { this._BranchId = newValue; this.MarkAsDirty(); }

        private _BranchName: string;
        public get BranchName() { return this._BranchName; }
        public set BranchName(newValue: string) { this._BranchName = newValue; this.MarkAsDirty(); }

        private _IncotermId: string;
        public get IncotermId() { return this._IncotermId; }
        public set IncotermId(newValue: string) { this._IncotermId = newValue; this.MarkAsDirty(); }

        private _IncotermCode: string;
        public get IncotermCode() { return this._IncotermCode; }
        public set IncotermCode(newValue: string) { this._IncotermCode = newValue; this.MarkAsDirty(); }

        private _IncotermName: string;
        public get IncotermName() { return this._IncotermName; }
        public set IncotermName(newValue: string) { this._IncotermName = newValue; this.MarkAsDirty(); }

        private _Routing: string;
        public get Routing() { return this._Routing; }
        public set Routing(newValue: string) { this._Routing = newValue; this.MarkAsDirty(); }

        private _SalesmanUserId: string;
        public get SalesmanUserId() { return this._SalesmanUserId; }
        public set SalesmanUserId(newValue: string) { this._SalesmanUserId = newValue; this.MarkAsDirty(); }

        private _SalesmanUserName: string;
        public get SalesmanUserName() { return this._SalesmanUserName; }
        public set SalesmanUserName(newValue: string) { this._SalesmanUserName = newValue; this.MarkAsDirty(); }

        private _CreatedByUserId: string;
        public get CreatedByUserId() { return this._CreatedByUserId; }
        public set CreatedByUserId(newValue: string) { this._CreatedByUserId = newValue; this.MarkAsDirty(); }

        private _DepartmentId: string;
        public get DepartmentId() { return this._DepartmentId; }
        public set DepartmentId(newValue: string) { this._DepartmentId = newValue; this.MarkAsDirty(); }

        private _Notes: string;
        public get Notes() { return this._Notes; }
        public set Notes(newValue: string) { this._Notes = newValue; this.MarkAsDirty(); }

        private _DescriptionOfGoods: string;
        public get DescriptionOfGoods() { return this._DescriptionOfGoods; }
        public set DescriptionOfGoods(newValue: string) { this._DescriptionOfGoods = newValue; this.MarkAsDirty(); }

        private _HAWBDate: Date;
        public get HAWBDate() { return this._HAWBDate; }
        public set HAWBDate(newValue: Date) { this._HAWBDate = newValue; this.MarkAsDirty(); }

        private _MAWBStackNumber: string;
        public get MAWBStackNumber() { return this._MAWBStackNumber; }
        public set MAWBStackNumber(newValue: string) { this._MAWBStackNumber = newValue; this.MarkAsDirty(); }

        private _IsOperationalClosed: boolean;
        public get IsOperationalClosed() { return this._IsOperationalClosed; }
        public set IsOperationalClosed(newValue: boolean) { this._IsOperationalClosed = newValue; this.MarkAsDirty(); }




        private _SearchFields: string;
        public get SearchFields() { return this._SearchFields; }
        public set SearchFields(newValue: string) { this._SearchFields = newValue; this.MarkAsDirty(); }

        private _IsSecured: boolean;
        public get IsSecured() { return this._IsSecured; }
        public set IsSecured(newValue: boolean) { this._IsSecured = newValue; this.MarkAsDirty(); }

        private _Master: string;
        public get Master() { return this._Master; }
        public set Master(newValue: string) { this._Master = newValue; this.MarkAsDirty(); }

        private _ShipmentTypeViewField: string;
        public get ShipmentTypeViewField() { return this._ShipmentTypeViewField; }
        public set ShipmentTypeViewField(newValue: string) { this._ShipmentTypeViewField = newValue; this.MarkAsDirty(); }

        private _LongMaster: string;
        public get LongMaster() { return this._LongMaster; }
        public set LongMaster(newValue: string) { this._LongMaster = newValue; this.MarkAsDirty(); }

        private _FreightPrepaidCollectId: string;
        public get FreightPrepaidCollectId() { return this._FreightPrepaidCollectId; }
        public set FreightPrepaidCollectId(newValue: string) { this._FreightPrepaidCollectId = newValue; this.MarkAsDirty(); }

        private _OtherPrepaidCollectId: string;
        public get OtherPrepaidCollectId() { return this._OtherPrepaidCollectId; }
        public set OtherPrepaidCollectId(newValue: string) { this._OtherPrepaidCollectId = newValue; this.MarkAsDirty(); }

        private _GrossWeightUnitCode: string;
        public get GrossWeightUnitCode() { return this._GrossWeightUnitCode; }
        public set GrossWeightUnitCode(newValue: string) { this._GrossWeightUnitCode = newValue; this.MarkAsDirty(); }

        private _ChargeableWeightUnitCode: string;
        public get ChargeableWeightUnitCode() { return this._ChargeableWeightUnitCode; }
        public set ChargeableWeightUnitCode(newValue: string) { this._ChargeableWeightUnitCode = newValue; this.MarkAsDirty(); }

        private _DimensionsUnitCode: string;
        public get DimensionsUnitCode() { return this._DimensionsUnitCode; }
        public set DimensionsUnitCode(newValue: string) { this._DimensionsUnitCode = newValue; this.MarkAsDirty(); }

        private _RateClassCode: string;
        public get RateClassCode() { return this._RateClassCode; }
        public set RateClassCode(newValue: string) { this._RateClassCode = newValue; this.MarkAsDirty(); }

        private _VolumetricWeight: number;
        public get VolumetricWeight() { return this._VolumetricWeight; }
        public set VolumetricWeight(newValue: number) { this._VolumetricWeight = newValue; this.MarkAsDirty(); }

        private _VolumeInCBM: number;
        public get VolumeInCBM() { return this._VolumeInCBM; }
        public set VolumeInCBM(newValue: number) { this._VolumeInCBM = newValue; this.MarkAsDirty(); }


        private _Volume: number;
        public get Volume() { return this._Volume; }
        public set Volume(newValue: number) { this._Volume = newValue; this.MarkAsDirty(); }


        private _PackagesQuantity: number;
        public get PackagesQuantity() { return this._PackagesQuantity; }
        public set PackagesQuantity(newValue: number) { this._PackagesQuantity = newValue; this.MarkAsDirty(); }


        private _NumberOfPackages: number;
        public get NumberOfPackages() { return this._NumberOfPackages; }
        public set NumberOfPackages(newValue: number) { this._NumberOfPackages = newValue; this.MarkAsDirty(); }


        private _NumberOfContainers: number;
        public get NumberOfContainers() { return this._NumberOfContainers; }
        public set NumberOfContainers(newValue: number) { this._NumberOfContainers = newValue; this.MarkAsDirty(); }


        private _Ratio: number;
        public get Ratio() { return this._Ratio; }
        public set Ratio(newValue: number) { this._Ratio = newValue; this.MarkAsDirty(); }


        private _DimFactor: number;
        public get DimFactor() { return this._DimFactor; }
        public set DimFactor(newValue: number) { this._DimFactor = newValue; this.MarkAsDirty(); }


        private _MAWBOBLDate: Date;
        public get MAWBOBLDate() { return this._MAWBOBLDate; }
        public set MAWBOBLDate(newValue: Date) { this._MAWBOBLDate = newValue; this.MarkAsDirty(); }

        private _GrossWeightEdited: boolean;
        public get GrossWeightEdited() { return this._GrossWeightEdited; }
        public set GrossWeightEdited(newValue: boolean) { this._GrossWeightEdited = newValue; this.MarkAsDirty(); }

        private _ChargeableWeightEdited: boolean;
        public get ChargeableWeightEdited() { return this._ChargeableWeightEdited; }
        public set ChargeableWeightEdited(newValue: boolean) { this._ChargeableWeightEdited = newValue; this.MarkAsDirty(); }


        private _VolumeUnitCode: string;
        public get VolumeUnitCode() { return this._VolumeUnitCode; }
        public set VolumeUnitCode(newValue: string) { this._VolumeUnitCode = newValue; this.MarkAsDirty(); }


        private _StatusId: string;
        public get StatusId() { return this._StatusId; }
        public set StatusId(newValue: string) { this._StatusId = newValue; this.MarkAsDirty(); }


        private _StatusName: string;
        public get StatusName() { return this._StatusName; }
        public set StatusName(newValue: string) { this._StatusName = newValue; this.MarkAsDirty(); }


        private _StatusDate: Date;
        public get StatusDate() { return this._StatusDate; }
        public set StatusDate(newValue: Date) { this._StatusDate = newValue; this.MarkAsDirty(); }


        private _StatusLocation: string;
        public get StatusLocation() { return this._StatusLocation; }
        public set StatusLocation(newValue: string) { this._StatusLocation = newValue; this.MarkAsDirty(); }


        private _QuoteId: string;
        public get QuoteId() { return this._QuoteId; }
        public set QuoteId(newValue: string) { this._QuoteId = newValue; this.MarkAsDirty(); }


        private _BookingId: string;
        public get BookingId() { return this._BookingId; }
        public set BookingId(newValue: string) { this._BookingId = newValue; this.MarkAsDirty(); }

        private _BookingNumber: string;
        public get BookingNumber() { return this._BookingNumber; }
        public set BookingNumber(newValue: string) { this._BookingNumber = newValue; this.MarkAsDirty(); }


        private _TotalContainers: string;
        public get TotalContainers() { return this._TotalContainers; }
        public set TotalContainers(newValue: string) { this._TotalContainers = newValue; this.MarkAsDirty(); }


        private _ShipmentType: string;
        public get ShipmentType() { return this._ShipmentType; }
        public set ShipmentType(newValue: string) { this._ShipmentType = newValue; this.MarkAsDirty(); }


        private _FollowUpType: string;
        public get FollowUpType() { return this._FollowUpType; }
        public set FollowUpType(newValue: string) { this._FollowUpType = newValue; this.MarkAsDirty(); }


        private _FollowUpId: string;
        public get FollowUpId() { return this._FollowUpId; }
        public set FollowUpId(newValue: string) { this._FollowUpId = newValue; this.MarkAsDirty(); }


        private _FollowUpDate: Date;
        public get FollowUpDate() { return this._FollowUpDate; }
        public set FollowUpDate(newValue: Date) { this._FollowUpDate = newValue; this.MarkAsDirty(); }


        private _ShipmentPMId: string;
        public get ShipmentPMId() { return this._ShipmentPMId; }
        public set ShipmentPMId(newValue: string) { this._ShipmentPMId = newValue; this.MarkAsDirty(); }


        private _LastUpdate: Date;
        public get LastUpdate() { return this._LastUpdate; }
        public set LastUpdate(newValue: Date) { this._LastUpdate = newValue; this.MarkAsDirty(); }

        private _NewMessage: string;
        public get NewMessage() { return this._NewMessage; }
        public set NewMessage(newValue: string) { this._NewMessage = newValue; this.MarkAsDirty(); }


        private _FollowUpNotes: string;
        public get FollowUpNotes() { return this._FollowUpNotes; }
        public set FollowUpNotes(newValue: string) { this._FollowUpNotes = newValue; this.MarkAsDirty(); }

        private _IsAnyConversation: boolean;
        public get IsAnyConversation() { return this._IsAnyConversation; }
        public set IsAnyConversation(newValue: boolean) { this._IsAnyConversation = newValue; this.MarkAsDirty(); }

        private _NumberOfShipments: number;
        public get NumberOfShipments() { return this._NumberOfShipments; }
        public set NumberOfShipments(newValue: number) { this._NumberOfShipments = newValue; this.MarkAsDirty(); }


        private _MainCarriageFinalDestinationPortId: string;
        public get MainCarriageFinalDestinationPortId() { return this._MainCarriageFinalDestinationPortId; }
        public set MainCarriageFinalDestinationPortId(newValue: string) { this._MainCarriageFinalDestinationPortId = newValue; this.MarkAsDirty(); }


        private _MainCarriageFinalDestinationPortCode: string;
        public get MainCarriageFinalDestinationPortCode() { return this._MainCarriageFinalDestinationPortCode; }
        public set MainCarriageFinalDestinationPortCode(newValue: string) { this._MainCarriageFinalDestinationPortCode = newValue; this.MarkAsDirty(); }


        private _MainCarriageFinalDestinationPortName: string;
        public get MainCarriageFinalDestinationPortName() { return this._MainCarriageFinalDestinationPortName; }
        public set MainCarriageFinalDestinationPortName(newValue: string) { this._MainCarriageFinalDestinationPortName = newValue; this.MarkAsDirty(); }


        private _MainCarriageFinalDestinationPortCountryCode: string;
        public get MainCarriageFinalDestinationPortCountryCode() { return this._MainCarriageFinalDestinationPortCountryCode; }
        public set MainCarriageFinalDestinationPortCountryCode(newValue: string) { this._MainCarriageFinalDestinationPortCountryCode = newValue; this.MarkAsDirty(); }


        private _MainCarriageFinalDestinationPortCountryName: string;
        public get MainCarriageFinalDestinationPortCountryName() { return this._MainCarriageFinalDestinationPortCountryName; }
        public set MainCarriageFinalDestinationPortCountryName(newValue: string) { this._MainCarriageFinalDestinationPortCountryName = newValue; this.MarkAsDirty(); }


        private _MainHarmonize: string;
        public get MainHarmonize() { return this._MainHarmonize; }
        public set MainHarmonize(newValue: string) { this._MainHarmonize = newValue; this.MarkAsDirty(); }

        private _IsDangerous: boolean;
        public get IsDangerous() { return this._IsDangerous; }
        public set IsDangerous(newValue: boolean) { this._IsDangerous = newValue; this.MarkAsDirty(); }

        private _DangerousClassNumber: string;
        public get DangerousClassNumber() { return this._DangerousClassNumber; }
        public set DangerousClassNumber(newValue: string) { this._DangerousClassNumber = newValue; this.MarkAsDirty(); }


        private _DangerousUnNumber: string;
        public get DangerousUnNumber() { return this._DangerousUnNumber; }
        public set DangerousUnNumber(newValue: string) { this._DangerousUnNumber = newValue; this.MarkAsDirty(); }


        private _DangerousPackagingGroup: string;
        public get DangerousPackagingGroup() { return this._DangerousPackagingGroup; }
        public set DangerousPackagingGroup(newValue: string) { this._DangerousPackagingGroup = newValue; this.MarkAsDirty(); }


        private _DangerousIMDGCode: string;
        public get DangerousIMDGCode() { return this._DangerousIMDGCode; }
        public set DangerousIMDGCode(newValue: string) { this._DangerousIMDGCode = newValue; this.MarkAsDirty(); }


        private _DangerousFlashPoint: string;
        public get DangerousFlashPoint() { return this._DangerousFlashPoint; }
        public set DangerousFlashPoint(newValue: string) { this._DangerousFlashPoint = newValue; this.MarkAsDirty(); }


        private _DangerousMaterialDescription: string;
        public get DangerousMaterialDescription() { return this._DangerousMaterialDescription; }
        public set DangerousMaterialDescription(newValue: string) { this._DangerousMaterialDescription = newValue; this.MarkAsDirty(); }

        private _LTCWEdited: boolean;
        public get LTCWEdited() { return this._LTCWEdited; }
        public set LTCWEdited(newValue: boolean) { this._LTCWEdited = newValue; this.MarkAsDirty(); }

        private _ShipmentPickUpIndex: number;
        public get ShipmentPickUpIndex() { return this._ShipmentPickUpIndex; }
        public set ShipmentPickUpIndex(newValue: number) { this._ShipmentPickUpIndex = newValue; this.MarkAsDirty(); }

        private _ShipmentDeliveryIndex: number;
        public get ShipmentDeliveryIndex() { return this._ShipmentDeliveryIndex; }
        public set ShipmentDeliveryIndex(newValue: number) { this._ShipmentDeliveryIndex = newValue; this.MarkAsDirty(); }


        private _ShipmentPayableStatusCode: string;
        public get ShipmentPayableStatusCode() { return this._ShipmentPayableStatusCode; }
        public set ShipmentPayableStatusCode(newValue: string) { this._ShipmentPayableStatusCode = newValue; this.MarkAsDirty(); }


        private _ShipmentReceivableStatusCode: string;
        public get ShipmentReceivableStatusCode() { return this._ShipmentReceivableStatusCode; }
        public set ShipmentReceivableStatusCode(newValue: string) { this._ShipmentReceivableStatusCode = newValue; this.MarkAsDirty(); }


        private _ShipmentReceivableStatusName: string;
        public get ShipmentReceivableStatusName() { return this._ShipmentReceivableStatusName; }
        public set ShipmentReceivableStatusName(newValue: string) { this._ShipmentReceivableStatusName = newValue; this.MarkAsDirty(); }


        private _ShipmentPayableStatusName: string;
        public get ShipmentPayableStatusName() { return this._ShipmentPayableStatusName; }
        public set ShipmentPayableStatusName(newValue: string) { this._ShipmentPayableStatusName = newValue; this.MarkAsDirty(); }

        private _IsCancelled: boolean;
        public get IsCancelled() { return this._IsCancelled; }
        public set IsCancelled(newValue: boolean) { this._IsCancelled = newValue; this.MarkAsDirty(); }

        private _IsAccountingClosed: boolean;
        public get IsAccountingClosed() { return this._IsAccountingClosed; }
        public set IsAccountingClosed(newValue: boolean) { this._IsAccountingClosed = newValue; this.MarkAsDirty(); }


        private _AccessDate: Date;
        public get AccessDate() { return this._AccessDate; }
        public set AccessDate(newValue: Date) { this._AccessDate = newValue; this.MarkAsDirty(); }


        private _UpdatedByUserId: string;
        public get UpdatedByUserId() { return this._UpdatedByUserId; }
        public set UpdatedByUserId(newValue: string) { this._UpdatedByUserId = newValue; this.MarkAsDirty(); }


        private _LastUpdateDate: Date;
        public get LastUpdateDate() { return this._LastUpdateDate; }
        public set LastUpdateDate(newValue: Date) { this._LastUpdateDate = newValue; this.MarkAsDirty(); }


        private _ProfitCurrencyId: string;
        public get ProfitCurrencyId() { return this._ProfitCurrencyId; }
        public set ProfitCurrencyId(newValue: string) { this._ProfitCurrencyId = newValue; this.MarkAsDirty(); }


        private _ProfitExchangeRate: number;
        public get ProfitExchangeRate() { return this._ProfitExchangeRate; }
        public set ProfitExchangeRate(newValue: number) { this._ProfitExchangeRate = newValue; this.MarkAsDirty(); }


        private _UpdatedByUserName: string;
        public get UpdatedByUserName() { return this._UpdatedByUserName; }
        public set UpdatedByUserName(newValue: string) { this._UpdatedByUserName = newValue; this.MarkAsDirty(); }


        private _EventNote: string;
        public get EventNote() { return this._EventNote; }
        public set EventNote(newValue: string) { this._EventNote = newValue; this.MarkAsDirty(); }


        private _NextLegCode: string;
        public get NextLegCode() { return this._NextLegCode; }
        public set NextLegCode(newValue: string) { this._NextLegCode = newValue; this.MarkAsDirty(); }


        private _NextLegName: string;
        public get NextLegName() { return this._NextLegName; }
        public set NextLegName(newValue: string) { this._NextLegName = newValue; this.MarkAsDirty(); }


        private _NextETD: Date;
        public get NextETD() { return this._NextETD; }
        public set NextETD(newValue: Date) { this._NextETD = newValue; this.MarkAsDirty(); }


        private _NextETA: Date;
        public get NextETA() { return this._NextETA; }
        public set NextETA(newValue: Date) { this._NextETA = newValue; this.MarkAsDirty(); }


        private _ShipmentLevelCode: string;
        public get ShipmentLevelCode() { return this._ShipmentLevelCode; }
        public set ShipmentLevelCode(newValue: string) { this._ShipmentLevelCode = newValue; this.MarkAsDirty(); }


        private _ShipmentLevelName: string;
        public get ShipmentLevelName() { return this._ShipmentLevelName; }
        public set ShipmentLevelName(newValue: string) { this._ShipmentLevelName = newValue; this.MarkAsDirty(); }


        private _MasterShipmentDataId: string;
        public get MasterShipmentDataId() { return this._MasterShipmentDataId; }
        public set MasterShipmentDataId(newValue: string) { this._MasterShipmentDataId = newValue; this.MarkAsDirty(); }


        private _QuoteNumber: string;
        public get QuoteNumber() { return this._QuoteNumber; }
        public set QuoteNumber(newValue: string) { this._QuoteNumber = newValue; this.MarkAsDirty(); }
        
        private _ShipmentCustomerTypeCode: string;
        public get ShipmentCustomerTypeCode() { return this._ShipmentCustomerTypeCode; }
        public set ShipmentCustomerTypeCode(newValue: string) { this._ShipmentCustomerTypeCode = newValue; this.MarkAsDirty(); }

        private _ConsigneeAddressOneTime: boolean;
        public get ConsigneeAddressOneTime() { return this._ConsigneeAddressOneTime; }
        public set ConsigneeAddressOneTime(newValue: boolean) { this._ConsigneeAddressOneTime = newValue; this.MarkAsDirty(); }

        private _ShipperAddressOneTime: boolean;
        public get ShipperAddressOneTime() { return this._ShipperAddressOneTime; }
        public set ShipperAddressOneTime(newValue: boolean) { this._ShipperAddressOneTime = newValue; this.MarkAsDirty(); }


        private _CustomerId: string;
        public get CustomerId() { return this._CustomerId; }
        public set CustomerId(newValue: string) { this._CustomerId = newValue; this.MarkAsDirty(); }


        private _CustomerAddressId: string;
        public get CustomerAddressId() { return this._CustomerAddressId; }
        public set CustomerAddressId(newValue: string) { this._CustomerAddressId = newValue; this.MarkAsDirty(); }


        private _CustomerContactId: string;
        public get CustomerContactId() { return this._CustomerContactId; }
        public set CustomerContactId(newValue: string) { this._CustomerContactId = newValue; this.MarkAsDirty(); }


        private _CustomerReference1: string;
        public get CustomerReference1() { return this._CustomerReference1; }
        public set CustomerReference1(newValue: string) { this._CustomerReference1 = newValue; this.MarkAsDirty(); }


        private _CustomerReference2: string;
        public get CustomerReference2() { return this._CustomerReference2; }
        public set CustomerReference2(newValue: string) { this._CustomerReference2 = newValue; this.MarkAsDirty(); }


        private _CustomerName: string;
        public get CustomerName() { return this._CustomerName; }
        public set CustomerName(newValue: string) { this._CustomerName = newValue; this.MarkAsDirty(); }


        private _CustomerNote: string;
        public get CustomerNote() { return this._CustomerNote; }
        public set CustomerNote(newValue: string) { this._CustomerNote = newValue; this.MarkAsDirty(); }


        private _FreelancerId: string;
        public get FreelancerId() { return this._FreelancerId; }
        public set FreelancerId(newValue: string) { this._FreelancerId = newValue; this.MarkAsDirty(); }


        private _FreelancerAddressId: string;
        public get FreelancerAddressId() { return this._FreelancerAddressId; }
        public set FreelancerAddressId(newValue: string) { this._FreelancerAddressId = newValue; this.MarkAsDirty(); }


        private _FreelancerContactId: string;
        public get FreelancerContactId() { return this._FreelancerContactId; }
        public set FreelancerContactId(newValue: string) { this._FreelancerContactId = newValue; this.MarkAsDirty(); }

        private _FreelancerName: string;
        public get FreelancerName() { return this._FreelancerName; }
        public set FreelancerName(newValue: string) { this._FreelancerName = newValue; this.MarkAsDirty(); }


        private _IssuingCarrierAgentId: string;
        public get IssuingCarrierAgentId() { return this._IssuingCarrierAgentId; }
        public set IssuingCarrierAgentId(newValue: string) { this._IssuingCarrierAgentId = newValue; this.MarkAsDirty(); }


        private _IssuingCarrierAddressId: string;
        public get IssuingCarrierAddressId() { return this._IssuingCarrierAddressId; }
        public set IssuingCarrierAddressId(newValue: string) { this._IssuingCarrierAddressId = newValue; this.MarkAsDirty(); }


        private _IssuingCarrierAgentName: string;
        public get IssuingCarrierAgentName() { return this._IssuingCarrierAgentName; }
        public set IssuingCarrierAgentName(newValue: string) { this._IssuingCarrierAgentName = newValue; this.MarkAsDirty(); }


        private _IssuingCarrierAgentNote: string;
        public get IssuingCarrierAgentNote() { return this._IssuingCarrierAgentNote; }
        public set IssuingCarrierAgentNote(newValue: string) { this._IssuingCarrierAgentNote = newValue; this.MarkAsDirty(); }


        private _IssuingCarrierIATACode: string;
        public get IssuingCarrierIATACode() { return this._IssuingCarrierIATACode; }
        public set IssuingCarrierIATACode(newValue: string) { this._IssuingCarrierIATACode = newValue; this.MarkAsDirty(); }


        private _FreightForwarderId: string;
        public get FreightForwarderId() { return this._FreightForwarderId; }
        public set FreightForwarderId(newValue: string) { this._FreightForwarderId = newValue; this.MarkAsDirty(); }


        private _FreightForwarderAddressId: string;
        public get FreightForwarderAddressId() { return this._FreightForwarderAddressId; }
        public set FreightForwarderAddressId(newValue: string) { this._FreightForwarderAddressId = newValue; this.MarkAsDirty(); }


        private _FreightForwarderContactId: string;
        public get FreightForwarderContactId() { return this._FreightForwarderContactId; }
        public set FreightForwarderContactId(newValue: string) { this._FreightForwarderContactId = newValue; this.MarkAsDirty(); }

        private _FreightForwarderReference: string;
        public get FreightForwarderReference() { return this._FreightForwarderReference; }
        public set FreightForwarderReference(newValue: string) { this._FreightForwarderReference = newValue; this.MarkAsDirty(); }


        private _FreightForwarderName: string;
        public get FreightForwarderName() { return this._FreightForwarderName; }
        public set FreightForwarderName(newValue: string) { this._FreightForwarderName = newValue; this.MarkAsDirty(); }


        private _FreightForwarderNote: string;
        public get FreightForwarderNote() { return this._FreightForwarderNote; }
        public set FreightForwarderNote(newValue: string) { this._FreightForwarderNote = newValue; this.MarkAsDirty(); }


        private _ShipperId: string;
        public get ShipperId() { return this._ShipperId; }
        public set ShipperId(newValue: string) { this._ShipperId = newValue; this.MarkAsDirty(); }


        private _ShipperAddressId: string;
        public get ShipperAddressId() { return this._ShipperAddressId; }
        public set ShipperAddressId(newValue: string) { this._ShipperAddressId = newValue; this.MarkAsDirty(); }


        private _ShipperContactId: string;
        public get ShipperContactId() { return this._ShipperContactId; }
        public set ShipperContactId(newValue: string) { this._ShipperContactId = newValue; this.MarkAsDirty(); }


        private _ShipperReference1: string;
        public get ShipperReference1() { return this._ShipperReference1; }
        public set ShipperReference1(newValue: string) { this._ShipperReference1 = newValue; this.MarkAsDirty(); }


        private _ShipperReference2: string;
        public get ShipperReference2() { return this._ShipperReference2; }
        public set ShipperReference2(newValue: string) { this._ShipperReference2 = newValue; this.MarkAsDirty(); }


        private _ShipperName: string;
        public get ShipperName() { return this._ShipperName; }
        public set ShipperName(newValue: string) { this._ShipperName = newValue; this.MarkAsDirty(); }


        private _ShipperNote: string;
        public get ShipperNote() { return this._ShipperNote; }
        public set ShipperNote(newValue: string) { this._ShipperNote = newValue; this.MarkAsDirty(); }


        private _ShipperAddressText: string;
        public get ShipperAddressText() { return this._ShipperAddressText; }
        public set ShipperAddressText(newValue: string) { this._ShipperAddressText = newValue; this.MarkAsDirty(); }


        private _ConsigneeId: string;
        public get ConsigneeId() { return this._ConsigneeId; }
        public set ConsigneeId(newValue: string) { this._ConsigneeId = newValue; this.MarkAsDirty(); }


        private _ConsigneeAddressId: string;
        public get ConsigneeAddressId() { return this._ConsigneeAddressId; }
        public set ConsigneeAddressId(newValue: string) { this._ConsigneeAddressId = newValue; this.MarkAsDirty(); }


        private _ConsigneeContactId: string;
        public get ConsigneeContactId() { return this._ConsigneeContactId; }
        public set ConsigneeContactId(newValue: string) { this._ConsigneeContactId = newValue; this.MarkAsDirty(); }


        private _ConsigneeReference1: string;
        public get ConsigneeReference1() { return this._ConsigneeReference1; }
        public set ConsigneeReference1(newValue: string) { this._ConsigneeReference1 = newValue; this.MarkAsDirty(); }


        private _ConsigneeReference2: string;
        public get ConsigneeReference2() { return this._ConsigneeReference2; }
        public set ConsigneeReference2(newValue: string) { this._ConsigneeReference2 = newValue; this.MarkAsDirty(); }


        private _ConsigneeName: string;
        public get ConsigneeName() { return this._ConsigneeName; }
        public set ConsigneeName(newValue: string) { this._ConsigneeName = newValue; this.MarkAsDirty(); }


        private _ConsigneeNote: string;
        public get ConsigneeNote() { return this._ConsigneeNote; }
        public set ConsigneeNote(newValue: string) { this._ConsigneeNote = newValue; this.MarkAsDirty(); }


        private _ConsigneeAddressText: string;
        public get ConsigneeAddressText() { return this._ConsigneeAddressText; }
        public set ConsigneeAddressText(newValue: string) { this._ConsigneeAddressText = newValue; this.MarkAsDirty(); }


        private _AgentId: string;
        public get AgentId() { return this._AgentId; }
        public set AgentId(newValue: string) { this._AgentId = newValue; this.MarkAsDirty(); }


        private _AgentAddressId: string;
        public get AgentAddressId() { return this._AgentAddressId; }
        public set AgentAddressId(newValue: string) { this._AgentAddressId = newValue; this.MarkAsDirty(); }


        private _AgentContactId: string;
        public get AgentContactId() { return this._AgentContactId; }
        public set AgentContactId(newValue: string) { this._AgentContactId = newValue; this.MarkAsDirty(); }


        private _AgentReference1: string;
        public get AgentReference1() { return this._AgentReference1; }
        public set AgentReference1(newValue: string) { this._AgentReference1 = newValue; this.MarkAsDirty(); }


        private _AgentReference2: string;
        public get AgentReference2() { return this._AgentReference2; }
        public set AgentReference2(newValue: string) { this._AgentReference2 = newValue; this.MarkAsDirty(); }


        private _AgentName: string;
        public get AgentName() { return this._AgentName; }
        public set AgentName(newValue: string) { this._AgentName = newValue; this.MarkAsDirty(); }


        private _AgentNote: string;
        public get AgentNote() { return this._AgentNote; }
        public set AgentNote(newValue: string) { this._AgentNote = newValue; this.MarkAsDirty(); }


        private _AgentAddressText: string;
        public get AgentAddressText() { return this._AgentAddressText; }
        public set AgentAddressText(newValue: string) { this._AgentAddressText = newValue; this.MarkAsDirty(); }


        private _CustomAgentExportId: string;
        public get CustomAgentExportId() { return this._CustomAgentExportId; }
        public set CustomAgentExportId(newValue: string) { this._CustomAgentExportId = newValue; this.MarkAsDirty(); }


        private _CustomAgentExportAddressId: string;
        public get CustomAgentExportAddressId() { return this._CustomAgentExportAddressId; }
        public set CustomAgentExportAddressId(newValue: string) { this._CustomAgentExportAddressId = newValue; this.MarkAsDirty(); }


        private _CustomAgentExportContactId: string;
        public get CustomAgentExportContactId() { return this._CustomAgentExportContactId; }
        public set CustomAgentExportContactId(newValue: string) { this._CustomAgentExportContactId = newValue; this.MarkAsDirty(); }


        private _CustomAgentExportReference: string;
        public get CustomAgentExportReference() { return this._CustomAgentExportReference; }
        public set CustomAgentExportReference(newValue: string) { this._CustomAgentExportReference = newValue; this.MarkAsDirty(); }


        private _CustomAgentExportName: string;
        public get CustomAgentExportName() { return this._CustomAgentExportName; }
        public set CustomAgentExportName(newValue: string) { this._CustomAgentExportName = newValue; this.MarkAsDirty(); }


        private _CustomAgentExportNote: string;
        public get CustomAgentExportNote() { return this._CustomAgentExportNote; }
        public set CustomAgentExportNote(newValue: string) { this._CustomAgentExportNote = newValue; this.MarkAsDirty(); }


        private _CustomAgentImportId: string;
        public get CustomAgentImportId() { return this._CustomAgentImportId; }
        public set CustomAgentImportId(newValue: string) { this._CustomAgentImportId = newValue; this.MarkAsDirty(); }


        private _CustomAgentImportAddressId: string;
        public get CustomAgentImportAddressId() { return this._CustomAgentImportAddressId; }
        public set CustomAgentImportAddressId(newValue: string) { this._CustomAgentImportAddressId = newValue; this.MarkAsDirty(); }


        private _CustomAgentImportContactId: string;
        public get CustomAgentImportContactId() { return this._CustomAgentImportContactId; }
        public set CustomAgentImportContactId(newValue: string) { this._CustomAgentImportContactId = newValue; this.MarkAsDirty(); }


        private _CustomAgentImportReference: string;
        public get CustomAgentImportReference() { return this._CustomAgentImportReference; }
        public set CustomAgentImportReference(newValue: string) { this._CustomAgentImportReference = newValue; this.MarkAsDirty(); }


        private _CustomAgentImportName: string;
        public get CustomAgentImportName() { return this._CustomAgentImportName; }
        public set CustomAgentImportName(newValue: string) { this._CustomAgentImportName = newValue; this.MarkAsDirty(); }

        private _CustomAgentImportNote: string;
        public get CustomAgentImportNote() { return this._CustomAgentImportNote; }
        public set CustomAgentImportNote(newValue: string) { this._CustomAgentImportNote = newValue; this.MarkAsDirty(); }


        private _Notify1Id: string;
        public get Notify1Id() { return this._Notify1Id; }
        public set Notify1Id(newValue: string) { this._Notify1Id = newValue; this.MarkAsDirty(); }


        private _Notify1AddressId: string;
        public get Notify1AddressId() { return this._Notify1AddressId; }
        public set Notify1AddressId(newValue: string) { this._Notify1AddressId = newValue; this.MarkAsDirty(); }


        private _Notify1ContactId: string;
        public get Notify1ContactId() { return this._Notify1ContactId; }
        public set Notify1ContactId(newValue: string) { this._Notify1ContactId = newValue; this.MarkAsDirty(); }


        private _Notify1Name: string;
        public get Notify1Name() { return this._Notify1Name; }
        public set Notify1Name(newValue: string) { this._Notify1Name = newValue; this.MarkAsDirty(); }


        private _Notify1Note: string;
        public get Notify1Note() { return this._Notify1Note; }
        public set Notify1Note(newValue: string) { this._Notify1Note = newValue; this.MarkAsDirty(); }


        private _Notify2Id: string;
        public get Notify2Id() { return this._Notify2Id; }
        public set Notify2Id(newValue: string) { this._Notify2Id = newValue; this.MarkAsDirty(); }


        private _Notify2AddressId: string;
        public get Notify2AddressId() { return this._Notify2AddressId; }
        public set Notify2AddressId(newValue: string) { this._Notify2AddressId = newValue; this.MarkAsDirty(); }


        private _Notify2ContactId: string;
        public get Notify2ContactId() { return this._Notify2ContactId; }
        public set Notify2ContactId(newValue: string) { this._Notify2ContactId = newValue; this.MarkAsDirty(); }


        private _Notify2Name: string;
        public get Notify2Name() { return this._Notify2Name; }
        public set Notify2Name(newValue: string) { this._Notify2Name = newValue; this.MarkAsDirty(); }


        private _Notify2Note: string;
        public get Notify2Note() { return this._Notify2Note; }
        public set Notify2Note(newValue: string) { this._Notify2Note = newValue; this.MarkAsDirty(); }


        private _ShipperNotExporterId: string;
        public get ShipperNotExporterId() { return this._ShipperNotExporterId; }
        public set ShipperNotExporterId(newValue: string) { this._ShipperNotExporterId = newValue; this.MarkAsDirty(); }


        private _ShipperNotExporterAddressId: string;
        public get ShipperNotExporterAddressId() { return this._ShipperNotExporterAddressId; }
        public set ShipperNotExporterAddressId(newValue: string) { this._ShipperNotExporterAddressId = newValue; this.MarkAsDirty(); }


        private _ShipperNotExporterContactId: string;
        public get ShipperNotExporterContactId() { return this._ShipperNotExporterContactId; }
        public set ShipperNotExporterContactId(newValue: string) { this._ShipperNotExporterContactId = newValue; this.MarkAsDirty(); }


        private _ShipperNotExporterName: string;
        public get ShipperNotExporterName() { return this._ShipperNotExporterName; }
        public set ShipperNotExporterName(newValue: string) { this._ShipperNotExporterName = newValue; this.MarkAsDirty(); }


        private _ShipperNotExporterNote: string;
        public get ShipperNotExporterNote() { return this._ShipperNotExporterNote; }
        public set ShipperNotExporterNote(newValue: string) { this._ShipperNotExporterNote = newValue; this.MarkAsDirty(); }


        private _ConsigneeNotImporterId: string;
        public get ConsigneeNotImporterId() { return this._ConsigneeNotImporterId; }
        public set ConsigneeNotImporterId(newValue: string) { this._ConsigneeNotImporterId = newValue; this.MarkAsDirty(); }


        private _ConsigneeNotImporterAddressId: string;
        public get ConsigneeNotImporterAddressId() { return this._ConsigneeNotImporterAddressId; }
        public set ConsigneeNotImporterAddressId(newValue: string) { this._ConsigneeNotImporterAddressId = newValue; this.MarkAsDirty(); }


        private _ConsigneeNotImporterContactId: string;
        public get ConsigneeNotImporterContactId() { return this._ConsigneeNotImporterContactId; }
        public set ConsigneeNotImporterContactId(newValue: string) { this._ConsigneeNotImporterContactId = newValue; this.MarkAsDirty(); }


        private _ConsigneeNotImporterName: string;
        public get ConsigneeNotImporterName() { return this._ConsigneeNotImporterName; }
        public set ConsigneeNotImporterName(newValue: string) { this._ConsigneeNotImporterName = newValue; this.MarkAsDirty(); }


        private _ConsigneeNotImporterNote: string;
        public get ConsigneeNotImporterNote() { return this._ConsigneeNotImporterNote; }
        public set ConsigneeNotImporterNote(newValue: string) { this._ConsigneeNotImporterNote = newValue; this.MarkAsDirty(); }


        private _CustomClearancePointId: string;
        public get CustomClearancePointId() { return this._CustomClearancePointId; }
        public set CustomClearancePointId(newValue: string) { this._CustomClearancePointId = newValue; this.MarkAsDirty(); }


        private _CustomClearancePointAddressId: string;
        public get CustomClearancePointAddressId() { return this._CustomClearancePointAddressId; }
        public set CustomClearancePointAddressId(newValue: string) { this._CustomClearancePointAddressId = newValue; this.MarkAsDirty(); }


        private _CustomClearancePointContactId: string;
        public get CustomClearancePointContactId() { return this._CustomClearancePointContactId; }
        public set CustomClearancePointContactId(newValue: string) { this._CustomClearancePointContactId = newValue; this.MarkAsDirty(); }


        private _CustomClearancePointReference1: string;
        public get CustomClearancePointReference1() { return this._CustomClearancePointReference1; }
        public set CustomClearancePointReference1(newValue: string) { this._CustomClearancePointReference1 = newValue; this.MarkAsDirty(); }


        private _CustomClearancePointName: string;
        public get CustomClearancePointName() { return this._CustomClearancePointName; }
        public set CustomClearancePointName(newValue: string) { this._CustomClearancePointName = newValue; this.MarkAsDirty(); }


        private _CustomClearancePointNote: string;
        public get CustomClearancePointNote() { return this._CustomClearancePointNote; }
        public set CustomClearancePointNote(newValue: string) { this._CustomClearancePointNote = newValue; this.MarkAsDirty(); }


        private _ColoaderId: string;
        public get ColoaderId() { return this._ColoaderId; }
        public set ColoaderId(newValue: string) { this._ColoaderId = newValue; this.MarkAsDirty(); }


        private _ColoaderAddressId: string;
        public get ColoaderAddressId() { return this._ColoaderAddressId; }
        public set ColoaderAddressId(newValue: string) { this._ColoaderAddressId = newValue; this.MarkAsDirty(); }


        private _ColoaderContactId: string;
        public get ColoaderContactId() { return this._ColoaderContactId; }
        public set ColoaderContactId(newValue: string) { this._ColoaderContactId = newValue; this.MarkAsDirty(); }


        private _ColoaderReference1: string;
        public get ColoaderReference1() { return this._ColoaderReference1; }
        public set ColoaderReference1(newValue: string) { this._ColoaderReference1 = newValue; this.MarkAsDirty(); }

        private _ColoaderName: string;
        public get ColoaderName() { return this._ColoaderName; }
        public set ColoaderName(newValue: string) { this._ColoaderName = newValue; this.MarkAsDirty(); }


        private _ColoaderNote: string;
        public get ColoaderNote() { return this._ColoaderNote; }
        public set ColoaderNote(newValue: string) { this._ColoaderNote = newValue; this.MarkAsDirty(); }


        private _ConsolidatorId: string;
        public get ConsolidatorId() { return this._ConsolidatorId; }
        public set ConsolidatorId(newValue: string) { this._ConsolidatorId = newValue; this.MarkAsDirty(); }


        private _ConsolidatorAddressId: string;
        public get ConsolidatorAddressId() { return this._ConsolidatorAddressId; }
        public set ConsolidatorAddressId(newValue: string) { this._ConsolidatorAddressId = newValue; this.MarkAsDirty(); }


        private _ConsolidatorContactId: string;
        public get ConsolidatorContactId() { return this._ConsolidatorContactId; }
        public set ConsolidatorContactId(newValue: string) { this._ConsolidatorContactId = newValue; this.MarkAsDirty(); }


        private _ConsolidatorReference: string;
        public get ConsolidatorReference() { return this._ConsolidatorReference; }
        public set ConsolidatorReference(newValue: string) { this._ConsolidatorReference = newValue; this.MarkAsDirty(); }


        private _ConsolidatorName: string;
        public get ConsolidatorName() { return this._ConsolidatorName; }
        public set ConsolidatorName(newValue: string) { this._ConsolidatorName = newValue; this.MarkAsDirty(); }


        private _ConsolidatorNote: string;
        public get ConsolidatorNote() { return this._ConsolidatorNote; }
        public set ConsolidatorNote(newValue: string) { this._ConsolidatorNote = newValue; this.MarkAsDirty(); }


        private _ForwarderShipmentNumber: string;
        public get ForwarderShipmentNumber() { return this._ForwarderShipmentNumber; }
        public set ForwarderShipmentNumber(newValue: string) { this._ForwarderShipmentNumber = newValue; this.MarkAsDirty(); }


        private _CustomerShipmentNumber: string;
        public get CustomerShipmentNumber() { return this._CustomerShipmentNumber; }
        public set CustomerShipmentNumber(newValue: string) { this._CustomerShipmentNumber = newValue; this.MarkAsDirty(); }


        private _CustomsDeclarationNumber: string;
        public get CustomsDeclarationNumber() { return this._CustomsDeclarationNumber; }
        public set CustomsDeclarationNumber(newValue: string) { this._CustomsDeclarationNumber = newValue; this.MarkAsDirty(); }

        private _NoFreightFile: boolean;
        public get NoFreightFile() { return this._NoFreightFile; }
        public set NoFreightFile(newValue: boolean) { this._NoFreightFile = newValue; this.MarkAsDirty(); }

        private _IsExceptionResolved: boolean;
        public get IsExceptionResolved() { return this._IsExceptionResolved; }
        public set IsExceptionResolved(newValue: boolean) { this._IsExceptionResolved = newValue; this.MarkAsDirty(); }
        
        private _AirlinePrefix: string;
        public get AirlinePrefix() { return this._AirlinePrefix; }
        public set AirlinePrefix(newValue: string) { this._AirlinePrefix = newValue; this.MarkAsDirty(); }


        private _MainCarriageCarrierPrefix: string;
        public get MainCarriageCarrierPrefix() { return this._MainCarriageCarrierPrefix; }
        public set MainCarriageCarrierPrefix(newValue: string) { this._MainCarriageCarrierPrefix = newValue; this.MarkAsDirty(); }


        private _Transshipment1CarrierPrefix: string;
        public get Transshipment1CarrierPrefix() { return this._Transshipment1CarrierPrefix; }
        public set Transshipment1CarrierPrefix(newValue: string) { this._Transshipment1CarrierPrefix = newValue; this.MarkAsDirty(); }


        private _Transshipment2CarrierPrefix: string;
        public get Transshipment2CarrierPrefix() { return this._Transshipment2CarrierPrefix; }
        public set Transshipment2CarrierPrefix(newValue: string) { this._Transshipment2CarrierPrefix = newValue; this.MarkAsDirty(); }


        private _Transshipment3CarrierPrefix: string;
        public get Transshipment3CarrierPrefix() { return this._Transshipment3CarrierPrefix; }
        public set Transshipment3CarrierPrefix(newValue: string) { this._Transshipment3CarrierPrefix = newValue; this.MarkAsDirty(); }

        private _FromPortId: string;
        public get FromPortId() { return this._FromPortId; }
        public set FromPortId(newValue: string) { this._FromPortId = newValue; this.MarkAsDirty(); }

        private _ToPortId: string;
        public get ToPortId() { return this._ToPortId; }
        public set ToPortId(newValue: string) { this._ToPortId = newValue; this.MarkAsDirty(); }

        private _PreCarriageTransportModeId: string;
        public get PreCarriageTransportModeId() { return this._PreCarriageTransportModeId; }
        public set PreCarriageTransportModeId(newValue: string) { this._PreCarriageTransportModeId = newValue; this.MarkAsDirty(); }

        private _PreCarriageFromPortId: string;
        public get PreCarriageFromPortId() { return this._PreCarriageFromPortId; }
        public set PreCarriageFromPortId(newValue: string) { this._PreCarriageFromPortId = newValue; this.MarkAsDirty(); }

        private _PreCarriageToPortId: string;
        public get PreCarriageToPortId() { return this._PreCarriageToPortId; }
        public set PreCarriageToPortId(newValue: string) { this._PreCarriageToPortId = newValue; this.MarkAsDirty(); }

        private _PreCarriageCarrierId: string;
        public get PreCarriageCarrierId() { return this._PreCarriageCarrierId; }
        public set PreCarriageCarrierId(newValue: string) { this._PreCarriageCarrierId = newValue; this.MarkAsDirty(); }

        private _PreCarriageCarrierNumber: string;
        public get PreCarriageCarrierNumber() { return this._PreCarriageCarrierNumber; }
        public set PreCarriageCarrierNumber(newValue: string) { this._PreCarriageCarrierNumber = newValue; this.MarkAsDirty(); }

        private _PreCarriageCarrierName: string;
        public get PreCarriageCarrierName() { return this._PreCarriageCarrierName; }
        public set PreCarriageCarrierName(newValue: string) { this._PreCarriageCarrierName = newValue; this.MarkAsDirty(); }

        private _PreCarriageCarrierCode: string;
        public get PreCarriageCarrierCode() { return this._PreCarriageCarrierCode; }
        public set PreCarriageCarrierCode(newValue: string) { this._PreCarriageCarrierCode = newValue; this.MarkAsDirty(); }

        private _PreCarriageFromPortCode: string;
        public get PreCarriageFromPortCode() { return this._PreCarriageFromPortCode; }
        public set PreCarriageFromPortCode(newValue: string) { this._PreCarriageFromPortCode = newValue; this.MarkAsDirty(); }

        private _PreCarriageFromPortName: string;
        public get PreCarriageFromPortName() { return this._PreCarriageFromPortName; }
        public set PreCarriageFromPortName(newValue: string) { this._PreCarriageFromPortName = newValue; this.MarkAsDirty(); }

        private _PreCarriageFromPortCountryCode: string;
        public get PreCarriageFromPortCountryCode() { return this._PreCarriageFromPortCountryCode; }
        public set PreCarriageFromPortCountryCode(newValue: string) { this._PreCarriageFromPortCountryCode = newValue; this.MarkAsDirty(); }

        private _PreCarriageFromPortCountryName: string;
        public get PreCarriageFromPortCountryName() { return this._PreCarriageFromPortCountryName; }
        public set PreCarriageFromPortCountryName(newValue: string) { this._PreCarriageFromPortCountryName = newValue; this.MarkAsDirty(); }

        private _PreCarriageToPortCode: string;
        public get PreCarriageToPortCode() { return this._PreCarriageToPortCode; }
        public set PreCarriageToPortCode(newValue: string) { this._PreCarriageToPortCode = newValue; this.MarkAsDirty(); }

        private _PreCarriageToPortName: string;
        public get PreCarriageToPortName() { return this._PreCarriageToPortName; }
        public set PreCarriageToPortName(newValue: string) { this._PreCarriageToPortName = newValue; this.MarkAsDirty(); }

        private _PreCarriageToPortCountryCode: string;
        public get PreCarriageToPortCountryCode() { return this._PreCarriageToPortCountryCode; }
        public set PreCarriageToPortCountryCode(newValue: string) { this._PreCarriageToPortCountryCode = newValue; this.MarkAsDirty(); }

        private _PreCarriageToPortCountryName: string;
        public get PreCarriageToPortCountryName() { return this._PreCarriageToPortCountryName; }
        public set PreCarriageToPortCountryName(newValue: string) { this._PreCarriageToPortCountryName = newValue; this.MarkAsDirty(); }

        private _PreCarriageETD: Date;
        public get PreCarriageETD() { return this._PreCarriageETD; }
        public set PreCarriageETD(newValue: Date) { this._PreCarriageETD = newValue; this.MarkAsDirty(); }

        private _PreCarriageATD: Date;
        public get PreCarriageATD() { return this._PreCarriageATD; }
        public set PreCarriageATD(newValue: Date) { this._PreCarriageATD = newValue; this.MarkAsDirty(); }

        private _PreCarriageETA: Date;
        public get PreCarriageETA() { return this._PreCarriageETA; }
        public set PreCarriageETA(newValue: Date) { this._PreCarriageETA = newValue; this.MarkAsDirty(); }

        private _PreCarriageATA: Date;
        public get PreCarriageATA() { return this._PreCarriageATA; }
        public set PreCarriageATA(newValue: Date) { this._PreCarriageATA = newValue; this.MarkAsDirty(); }

        private _PreCarriageCarrierWebSite: string;
        public get PreCarriageCarrierWebSite() { return this._PreCarriageCarrierWebSite; }
        public set PreCarriageCarrierWebSite(newValue: string) { this._PreCarriageCarrierWebSite = newValue; this.MarkAsDirty(); }


        private _OnCarriageTransportModeId: string;
        public get OnCarriageTransportModeId() { return this._OnCarriageTransportModeId; }
        public set OnCarriageTransportModeId(newValue: string) { this._OnCarriageTransportModeId = newValue; this.MarkAsDirty(); }

        private _OnCarriageFromPortId: string;
        public get OnCarriageFromPortId() { return this._OnCarriageFromPortId; }
        public set OnCarriageFromPortId(newValue: string) { this._OnCarriageFromPortId = newValue; this.MarkAsDirty(); }

        private _OnCarriageToPortId: string;
        public get OnCarriageToPortId() { return this._OnCarriageToPortId; }
        public set OnCarriageToPortId(newValue: string) { this._OnCarriageToPortId = newValue; this.MarkAsDirty(); }

        private _OnCarriageCarrierId: string;
        public get OnCarriageCarrierId() { return this._OnCarriageCarrierId; }
        public set OnCarriageCarrierId(newValue: string) { this._OnCarriageCarrierId = newValue; this.MarkAsDirty(); }

        private _OnCarriageCarrierNumber: string;
        public get OnCarriageCarrierNumber() { return this._OnCarriageCarrierNumber; }
        public set OnCarriageCarrierNumber(newValue: string) { this._OnCarriageCarrierNumber = newValue; this.MarkAsDirty(); }

        private _OnCarriageCarrierName: string;
        public get OnCarriageCarrierName() { return this._OnCarriageCarrierName; }
        public set OnCarriageCarrierName(newValue: string) { this._OnCarriageCarrierName = newValue; this.MarkAsDirty(); }

        private _OnCarriageCarrierCode: string;
        public get OnCarriageCarrierCode() { return this._OnCarriageCarrierCode; }
        public set OnCarriageCarrierCode(newValue: string) { this._OnCarriageCarrierCode = newValue; this.MarkAsDirty(); }

        private _OnCarriageFromPortCode: string;
        public get OnCarriageFromPortCode() { return this._OnCarriageFromPortCode; }
        public set OnCarriageFromPortCode(newValue: string) { this._OnCarriageFromPortCode = newValue; this.MarkAsDirty(); }

        private _OnCarriageFromPortName: string;
        public get OnCarriageFromPortName() { return this._OnCarriageFromPortName; }
        public set OnCarriageFromPortName(newValue: string) { this._OnCarriageFromPortName = newValue; this.MarkAsDirty(); }

        private _OnCarriageFromPortCountryCode: string;
        public get OnCarriageFromPortCountryCode() { return this._OnCarriageFromPortCountryCode; }
        public set OnCarriageFromPortCountryCode(newValue: string) { this._OnCarriageFromPortCountryCode = newValue; this.MarkAsDirty(); }

        private _OnCarriageFromPortCountryName: string;
        public get OnCarriageFromPortCountryName() { return this._OnCarriageFromPortCountryName; }
        public set OnCarriageFromPortCountryName(newValue: string) { this._OnCarriageFromPortCountryName = newValue; this.MarkAsDirty(); }

        private _OnCarriageToPortCode: string;
        public get OnCarriageToPortCode() { return this._OnCarriageToPortCode; }
        public set OnCarriageToPortCode(newValue: string) { this._OnCarriageToPortCode = newValue; this.MarkAsDirty(); }

        private _OnCarriageToPortName: string;
        public get OnCarriageToPortName() { return this._OnCarriageToPortName; }
        public set OnCarriageToPortName(newValue: string) { this._OnCarriageToPortName = newValue; this.MarkAsDirty(); }

        private _OnCarriageToPortCountryCode: string;
        public get OnCarriageToPortCountryCode() { return this._OnCarriageToPortCountryCode; }
        public set OnCarriageToPortCountryCode(newValue: string) { this._OnCarriageToPortCountryCode = newValue; this.MarkAsDirty(); }

        private _OnCarriageToPortCountryName: string;
        public get OnCarriageToPortCountryName() { return this._OnCarriageToPortCountryName; }
        public set OnCarriageToPortCountryName(newValue: string) { this._OnCarriageToPortCountryName = newValue; this.MarkAsDirty(); }


        private _OnCarriageETD: Date;
        public get OnCarriageETD() { return this._OnCarriageETD; }
        public set OnCarriageETD(newValue: Date) { this._OnCarriageETD = newValue; this.MarkAsDirty(); }

        private _OnCarriageATD: Date;
        public get OnCarriageATD() { return this._OnCarriageATD; }
        public set OnCarriageATD(newValue: Date) { this._OnCarriageATD = newValue; this.MarkAsDirty(); }

        private _OnCarriageETA: Date;
        public get OnCarriageETA() { return this._OnCarriageETA; }
        public set OnCarriageETA(newValue: Date) { this._OnCarriageETA = newValue; this.MarkAsDirty(); }

        private _OnCarriageATA: Date;
        public get OnCarriageATA() { return this._OnCarriageATA; }
        public set OnCarriageATA(newValue: Date) { this._OnCarriageATA = newValue; this.MarkAsDirty(); }

        private _MainCarriageFinalDestinationETA: Date;
        public get MainCarriageFinalDestinationETA() { return this._MainCarriageFinalDestinationETA; }
        public set MainCarriageFinalDestinationETA(newValue: Date) { this._MainCarriageFinalDestinationETA = newValue; this.MarkAsDirty(); }

        private _MainCarriageFinalDestinationATA: Date;
        public get MainCarriageFinalDestinationATA() { return this._MainCarriageFinalDestinationATA; }
        public set MainCarriageFinalDestinationATA(newValue: Date) { this._MainCarriageFinalDestinationATA = newValue; this.MarkAsDirty(); }

        private _OnCarriageCarrierWebSite: string;
        public get OnCarriageCarrierWebSite() { return this._OnCarriageCarrierWebSite; }
        public set OnCarriageCarrierWebSite(newValue: string) { this._OnCarriageCarrierWebSite = newValue; this.MarkAsDirty(); }

        private _MainCarriageTransportModeId: string;
        public get MainCarriageTransportModeId() { return this._MainCarriageTransportModeId; }
        public set MainCarriageTransportModeId(newValue: string) { this._MainCarriageTransportModeId = newValue; this.MarkAsDirty(); }


        private _MainCarriageFromPortId: string;
        public get MainCarriageFromPortId() { return this._MainCarriageFromPortId; }
        public set MainCarriageFromPortId(newValue: string) { this._MainCarriageFromPortId = newValue; this.MarkAsDirty(); }

        private _MainCarriageToPortId: string;
        public get MainCarriageToPortId() { return this._MainCarriageToPortId; }
        public set MainCarriageToPortId(newValue: string) { this._MainCarriageToPortId = newValue; this.MarkAsDirty(); }

        private _MainCarriageFromPortCode: string;
        public get MainCarriageFromPortCode() { return this._MainCarriageFromPortCode; }
        public set MainCarriageFromPortCode(newValue: string) { this._MainCarriageFromPortCode = newValue; this.MarkAsDirty(); }

        private _MainCarriageFromPortName: string;
        public get MainCarriageFromPortName() { return this._MainCarriageFromPortName; }
        public set MainCarriageFromPortName(newValue: string) { this._MainCarriageFromPortName = newValue; this.MarkAsDirty(); }

        private _MainCarriageFromPortCountryName: string;
        public get MainCarriageFromPortCountryName() { return this._MainCarriageFromPortCountryName; }
        public set MainCarriageFromPortCountryName(newValue: string) { this._MainCarriageFromPortCountryName = newValue; this.MarkAsDirty(); }

        private _MainCarriageFromPortCountryCode: string;
        public get MainCarriageFromPortCountryCode() { return this._MainCarriageFromPortCountryCode; }
        public set MainCarriageFromPortCountryCode(newValue: string) { this._MainCarriageFromPortCountryCode = newValue; this.MarkAsDirty(); }

        private _MainCarriageToPortCode: string;
        public get MainCarriageToPortCode() { return this._MainCarriageToPortCode; }
        public set MainCarriageToPortCode(newValue: string) { this._MainCarriageToPortCode = newValue; this.MarkAsDirty(); }

        private _MainCarriageToPortName: string;
        public get MainCarriageToPortName() { return this._MainCarriageToPortName; }
        public set MainCarriageToPortName(newValue: string) { this._MainCarriageToPortName = newValue; this.MarkAsDirty(); }

        private _MainCarriageToPortCountryCode: string;
        public get MainCarriageToPortCountryCode() { return this._MainCarriageToPortCountryCode; }
        public set MainCarriageToPortCountryCode(newValue: string) { this._MainCarriageToPortCountryCode = newValue; this.MarkAsDirty(); }

        private _MainCarriageToPortCountryName: string;
        public get MainCarriageToPortCountryName() { return this._MainCarriageToPortCountryName; }
        public set MainCarriageToPortCountryName(newValue: string) { this._MainCarriageToPortCountryName = newValue; this.MarkAsDirty(); }

        private _MainCarriageFromPortCountryEC: boolean;
        public get MainCarriageFromPortCountryEC() { return this._MainCarriageFromPortCountryEC; }
        public set MainCarriageFromPortCountryEC(newValue: boolean) { this._MainCarriageFromPortCountryEC = newValue; this.MarkAsDirty(); }

        private _MainCarriageToPortCountryEC: boolean;
        public get MainCarriageToPortCountryEC() { return this._MainCarriageToPortCountryEC; }
        public set MainCarriageToPortCountryEC(newValue: boolean) { this._MainCarriageToPortCountryEC = newValue; this.MarkAsDirty(); }


        private _MainCarriageVesselId: string;
        public get MainCarriageVesselId() { return this._MainCarriageVesselId; }
        public set MainCarriageVesselId(newValue: string) { this._MainCarriageVesselId = newValue; this.MarkAsDirty(); }

        private _PreCarriageVesselId: string;
        public get PreCarriageVesselId() { return this._PreCarriageVesselId; }
        public set PreCarriageVesselId(newValue: string) { this._PreCarriageVesselId = newValue; this.MarkAsDirty(); }

        private _OnCarriageVesselId: string;
        public get OnCarriageVesselId() { return this._OnCarriageVesselId; }
        public set OnCarriageVesselId(newValue: string) { this._OnCarriageVesselId = newValue; this.MarkAsDirty(); }

        private _Transshipment1VesselId: string;
        public get Transshipment1VesselId() { return this._Transshipment1VesselId; }
        public set Transshipment1VesselId(newValue: string) { this._Transshipment1VesselId = newValue; this.MarkAsDirty(); }

        private _Transshipment2VesselId: string;
        public get Transshipment2VesselId() { return this._Transshipment2VesselId; }
        public set Transshipment2VesselId(newValue: string) { this._Transshipment2VesselId = newValue; this.MarkAsDirty(); }

        private _Transshipment3VesselId: string;
        public get Transshipment3VesselId() { return this._Transshipment3VesselId; }
        public set Transshipment3VesselId(newValue: string) { this._Transshipment3VesselId = newValue; this.MarkAsDirty(); }

        private _MainCarriageVesselName: string;
        public get MainCarriageVesselName() { return this._MainCarriageVesselName; }
        public set MainCarriageVesselName(newValue: string) { this._MainCarriageVesselName = newValue; this.MarkAsDirty(); }

        private _PreCarriageVesselName: string;
        public get PreCarriageVesselName() { return this._PreCarriageVesselName; }
        public set PreCarriageVesselName(newValue: string) { this._PreCarriageVesselName = newValue; this.MarkAsDirty(); }

        private _OnCarriageVesselName: string;
        public get OnCarriageVesselName() { return this._OnCarriageVesselName; }
        public set OnCarriageVesselName(newValue: string) { this._OnCarriageVesselName = newValue; this.MarkAsDirty(); }

        private _Transshipment1VesselName: string;
        public get Transshipment1VesselName() { return this._Transshipment1VesselName; }
        public set Transshipment1VesselName(newValue: string) { this._Transshipment1VesselName = newValue; this.MarkAsDirty(); }

        private _Transshipment2VesselName: string;
        public get Transshipment2VesselName() { return this._Transshipment2VesselName; }
        public set Transshipment2VesselName(newValue: string) { this._Transshipment2VesselName = newValue; this.MarkAsDirty(); }

        private _Transshipment3VesselName: string;
        public get Transshipment3VesselName() { return this._Transshipment3VesselName; }
        public set Transshipment3VesselName(newValue: string) { this._Transshipment3VesselName = newValue; this.MarkAsDirty(); }

        private _MainCarriageIsFromStack: boolean;
        public get MainCarriageIsFromStack() { return this._MainCarriageIsFromStack; }
        public set MainCarriageIsFromStack(newValue: boolean) { this._MainCarriageIsFromStack = newValue; this.MarkAsDirty(); }

        private _MainCarriageATD: Date;
        public get MainCarriageATD() { return this._MainCarriageATD; }
        public set MainCarriageATD(newValue: Date) { this._MainCarriageATD = newValue; this.MarkAsDirty(); }

        private _MainCarriageATA: Date;
        public get MainCarriageATA() { return this._MainCarriageATA; }
        public set MainCarriageATA(newValue: Date) { this._MainCarriageATA = newValue; this.MarkAsDirty(); }

        private _MainCarriageETD: Date;
        public get MainCarriageETD() { return this._MainCarriageETD; }
        public set MainCarriageETD(newValue: Date) { this._MainCarriageETD = newValue; this.MarkAsDirty(); }

        private _MainCarriageETA: Date;
        public get MainCarriageETA() { return this._MainCarriageETA; }
        public set MainCarriageETA(newValue: Date) { this._MainCarriageETA = newValue; this.MarkAsDirty(); }

        private _Transshipment1FromPortId: string;
        public get Transshipment1FromPortId() { return this._Transshipment1FromPortId; }
        public set Transshipment1FromPortId(newValue: string) { this._Transshipment1FromPortId = newValue; this.MarkAsDirty(); }

        private _Transshipment1ToPortId: string;
        public get Transshipment1ToPortId() { return this._Transshipment1ToPortId; }
        public set Transshipment1ToPortId(newValue: string) { this._Transshipment1ToPortId = newValue; this.MarkAsDirty(); }

        private _Transshipment1ATD: Date;
        public get Transshipment1ATD() { return this._Transshipment1ATD; }
        public set Transshipment1ATD(newValue: Date) { this._Transshipment1ATD = newValue; this.MarkAsDirty(); }

        private _Transshipment1ATA: Date;
        public get Transshipment1ATA() { return this._Transshipment1ATA; }
        public set Transshipment1ATA(newValue: Date) { this._Transshipment1ATA = newValue; this.MarkAsDirty(); }

        private _Transshipment1ETD: Date;
        public get Transshipment1ETD() { return this._Transshipment1ETD; }
        public set Transshipment1ETD(newValue: Date) { this._Transshipment1ETD = newValue; this.MarkAsDirty(); }

        private _Transshipment1ETA: Date;
        public get Transshipment1ETA() { return this._Transshipment1ETA; }
        public set Transshipment1ETA(newValue: Date) { this._Transshipment1ETA = newValue; this.MarkAsDirty(); }

        private _Transshipment1CarrierNumber: string;
        public get Transshipment1CarrierNumber() { return this._Transshipment1CarrierNumber; }
        public set Transshipment1CarrierNumber(newValue: string) { this._Transshipment1CarrierNumber = newValue; this.MarkAsDirty(); }

        private _Transshipment1CarrierId: string;
        public get Transshipment1CarrierId() { return this._Transshipment1CarrierId; }
        public set Transshipment1CarrierId(newValue: string) { this._Transshipment1CarrierId = newValue; this.MarkAsDirty(); }

        private _Transshipment1CarrierName: string;
        public get Transshipment1CarrierName() { return this._Transshipment1CarrierName; }
        public set Transshipment1CarrierName(newValue: string) { this._Transshipment1CarrierName = newValue; this.MarkAsDirty(); }

        private _Transshipment1CarrierCode: string;
        public get Transshipment1CarrierCode() { return this._Transshipment1CarrierCode; }
        public set Transshipment1CarrierCode(newValue: string) { this._Transshipment1CarrierCode = newValue; this.MarkAsDirty(); }

        private _Transshipment1FromPortCode: string;
        public get Transshipment1FromPortCode() { return this._Transshipment1FromPortCode; }
        public set Transshipment1FromPortCode(newValue: string) { this._Transshipment1FromPortCode = newValue; this.MarkAsDirty(); }

        private _Transshipment1FromPortName: string;
        public get Transshipment1FromPortName() { return this._Transshipment1FromPortName; }
        public set Transshipment1FromPortName(newValue: string) { this._Transshipment1FromPortName = newValue; this.MarkAsDirty(); }

        private _Transshipment1FromPortCountryCode: string;
        public get Transshipment1FromPortCountryCode() { return this._Transshipment1FromPortCountryCode; }
        public set Transshipment1FromPortCountryCode(newValue: string) { this._Transshipment1FromPortCountryCode = newValue; this.MarkAsDirty(); }

        private _Transshipment1FromPortCountryName: string;
        public get Transshipment1FromPortCountryName() { return this._Transshipment1FromPortCountryName; }
        public set Transshipment1FromPortCountryName(newValue: string) { this._Transshipment1FromPortCountryName = newValue; this.MarkAsDirty(); }

        private _Transshipment1ToPortCode: string;
        public get Transshipment1ToPortCode() { return this._Transshipment1ToPortCode; }
        public set Transshipment1ToPortCode(newValue: string) { this._Transshipment1ToPortCode = newValue; this.MarkAsDirty(); }

        private _Transshipment1ToPortName: string;
        public get Transshipment1ToPortName() { return this._Transshipment1ToPortName; }
        public set Transshipment1ToPortName(newValue: string) { this._Transshipment1ToPortName = newValue; this.MarkAsDirty(); }

        private _Transshipment1ToPortCountryCode: string;
        public get Transshipment1ToPortCountryCode() { return this._Transshipment1ToPortCountryCode; }
        public set Transshipment1ToPortCountryCode(newValue: string) { this._Transshipment1ToPortCountryCode = newValue; this.MarkAsDirty(); }

        private _Transshipment1ToPortCountryName: string;
        public get Transshipment1ToPortCountryName() { return this._Transshipment1ToPortCountryName; }
        public set Transshipment1ToPortCountryName(newValue: string) { this._Transshipment1ToPortCountryName = newValue; this.MarkAsDirty(); }

        private _Transshipment1CarrierWebSite: string;
        public get Transshipment1CarrierWebSite() { return this._Transshipment1CarrierWebSite; }
        public set Transshipment1CarrierWebSite(newValue: string) { this._Transshipment1CarrierWebSite = newValue; this.MarkAsDirty(); }

        private _Transshipment2CarrierWebSite: string;
        public get Transshipment2CarrierWebSite() { return this._Transshipment2CarrierWebSite; }
        public set Transshipment2CarrierWebSite(newValue: string) { this._Transshipment2CarrierWebSite = newValue; this.MarkAsDirty(); }

        private _Transshipment3CarrierWebSite: string;
        public get Transshipment3CarrierWebSite() { return this._Transshipment3CarrierWebSite; }
        public set Transshipment3CarrierWebSite(newValue: string) { this._Transshipment3CarrierWebSite = newValue; this.MarkAsDirty(); }

        private _Transshipment2FromPortId: string;
        public get Transshipment2FromPortId() { return this._Transshipment2FromPortId; }
        public set Transshipment2FromPortId(newValue: string) { this._Transshipment2FromPortId = newValue; this.MarkAsDirty(); }

        private _Transshipment2ToPortId: string;
        public get Transshipment2ToPortId() { return this._Transshipment2ToPortId; }
        public set Transshipment2ToPortId(newValue: string) { this._Transshipment2ToPortId = newValue; this.MarkAsDirty(); }

        private _Transshipment2ATD: Date;
        public get Transshipment2ATD() { return this._Transshipment2ATD; }
        public set Transshipment2ATD(newValue: Date) { this._Transshipment2ATD = newValue; this.MarkAsDirty(); }

        private _Transshipment2ATA: Date;
        public get Transshipment2ATA() { return this._Transshipment2ATA; }
        public set Transshipment2ATA(newValue: Date) { this._Transshipment2ATA = newValue; this.MarkAsDirty(); }

        private _Transshipment2ETD: Date;
        public get Transshipment2ETD() { return this._Transshipment2ETD; }
        public set Transshipment2ETD(newValue: Date) { this._Transshipment2ETD = newValue; this.MarkAsDirty(); }

        private _Transshipment2ETA: Date;
        public get Transshipment2ETA() { return this._Transshipment2ETA; }
        public set Transshipment2ETA(newValue: Date) { this._Transshipment2ETA = newValue; this.MarkAsDirty(); }

        private _Transshipment2CarrierNumber: string;
        public get Transshipment2CarrierNumber() { return this._Transshipment2CarrierNumber; }
        public set Transshipment2CarrierNumber(newValue: string) { this._Transshipment2CarrierNumber = newValue; this.MarkAsDirty(); }

        private _Transshipment2CarrierId: string;
        public get Transshipment2CarrierId() { return this._Transshipment2CarrierId; }
        public set Transshipment2CarrierId(newValue: string) { this._Transshipment2CarrierId = newValue; this.MarkAsDirty(); }

        private _Transshipment2CarrierName: string;
        public get Transshipment2CarrierName() { return this._Transshipment2CarrierName; }
        public set Transshipment2CarrierName(newValue: string) { this._Transshipment2CarrierName = newValue; this.MarkAsDirty(); }

        private _Transshipment2CarrierCode: string;
        public get Transshipment2CarrierCode() { return this._Transshipment2CarrierCode; }
        public set Transshipment2CarrierCode(newValue: string) { this._Transshipment2CarrierCode = newValue; this.MarkAsDirty(); }

        private _Transshipment2FromPortCode: string;
        public get Transshipment2FromPortCode() { return this._Transshipment2FromPortCode; }
        public set Transshipment2FromPortCode(newValue: string) { this._Transshipment2FromPortCode = newValue; this.MarkAsDirty(); }

        private _Transshipment2FromPortName: string;
        public get Transshipment2FromPortName() { return this._Transshipment2FromPortName; }
        public set Transshipment2FromPortName(newValue: string) { this._Transshipment2FromPortName = newValue; this.MarkAsDirty(); }

        private _Transshipment2FromPortCountryCode: string;
        public get Transshipment2FromPortCountryCode() { return this._Transshipment2FromPortCountryCode; }
        public set Transshipment2FromPortCountryCode(newValue: string) { this._Transshipment2FromPortCountryCode = newValue; this.MarkAsDirty(); }

        private _Transshipment2FromPortCountryName: string;
        public get Transshipment2FromPortCountryName() { return this._Transshipment2FromPortCountryName; }
        public set Transshipment2FromPortCountryName(newValue: string) { this._Transshipment2FromPortCountryName = newValue; this.MarkAsDirty(); }

        private _Transshipment2ToPortCode: string;
        public get Transshipment2ToPortCode() { return this._Transshipment2ToPortCode; }
        public set Transshipment2ToPortCode(newValue: string) { this._Transshipment2ToPortCode = newValue; this.MarkAsDirty(); }

        private _Transshipment2ToPortName: string;
        public get Transshipment2ToPortName() { return this._Transshipment2ToPortName; }
        public set Transshipment2ToPortName(newValue: string) { this._Transshipment2ToPortName = newValue; this.MarkAsDirty(); }

        private _Transshipment2ToPortCountryCode: string;
        public get Transshipment2ToPortCountryCode() { return this._Transshipment2ToPortCountryCode; }
        public set Transshipment2ToPortCountryCode(newValue: string) { this._Transshipment2ToPortCountryCode = newValue; this.MarkAsDirty(); }

        private _Transshipment2ToPortCountryName: string;
        public get Transshipment2ToPortCountryName() { return this._Transshipment2ToPortCountryName; }
        public set Transshipment2ToPortCountryName(newValue: string) { this._Transshipment2ToPortCountryName = newValue; this.MarkAsDirty(); }

        private _Transshipment3FromPortId: string;
        public get Transshipment3FromPortId() { return this._Transshipment3FromPortId; }
        public set Transshipment3FromPortId(newValue: string) { this._Transshipment3FromPortId = newValue; this.MarkAsDirty(); }

        private _Transshipment3ToPortId: string;
        public get Transshipment3ToPortId() { return this._Transshipment3ToPortId; }
        public set Transshipment3ToPortId(newValue: string) { this._Transshipment3ToPortId = newValue; this.MarkAsDirty(); }

        private _Transshipment3ATD: Date;
        public get Transshipment3ATD() { return this._Transshipment3ATD; }
        public set Transshipment3ATD(newValue: Date) { this._Transshipment3ATD = newValue; this.MarkAsDirty(); }

        private _Transshipment3ATA: Date;
        public get Transshipment3ATA() { return this._Transshipment3ATA; }
        public set Transshipment3ATA(newValue: Date) { this._Transshipment3ATA = newValue; this.MarkAsDirty(); }

        private _Transshipment3ETD: Date;
        public get Transshipment3ETD() { return this._Transshipment3ETD; }
        public set Transshipment3ETD(newValue: Date) { this._Transshipment3ETD = newValue; this.MarkAsDirty(); }

        private _Transshipment3ETA: Date;
        public get Transshipment3ETA() { return this._Transshipment3ETA; }
        public set Transshipment3ETA(newValue: Date) { this._Transshipment3ETA = newValue; this.MarkAsDirty(); }


        private _Transshipment3CarrierNumber: string;
        public get Transshipment3CarrierNumber() { return this._Transshipment3CarrierNumber; }
        public set Transshipment3CarrierNumber(newValue: string) { this._Transshipment3CarrierNumber = newValue; this.MarkAsDirty(); }

        private _Transshipment3CarrierId: string;
        public get Transshipment3CarrierId() { return this._Transshipment3CarrierId; }
        public set Transshipment3CarrierId(newValue: string) { this._Transshipment3CarrierId = newValue; this.MarkAsDirty(); }

        private _Transshipment3CarrierName: string;
        public get Transshipment3CarrierName() { return this._Transshipment3CarrierName; }
        public set Transshipment3CarrierName(newValue: string) { this._Transshipment3CarrierName = newValue; this.MarkAsDirty(); }

        private _Transshipment3CarrierCode: string;
        public get Transshipment3CarrierCode() { return this._Transshipment3CarrierCode; }
        public set Transshipment3CarrierCode(newValue: string) { this._Transshipment3CarrierCode = newValue; this.MarkAsDirty(); }

        private _Transshipment3FromPortCode: string;
        public get Transshipment3FromPortCode() { return this._Transshipment3FromPortCode; }
        public set Transshipment3FromPortCode(newValue: string) { this._Transshipment3FromPortCode = newValue; this.MarkAsDirty(); }

        private _Transshipment3FromPortName: string;
        public get Transshipment3FromPortName() { return this._Transshipment3FromPortName; }
        public set Transshipment3FromPortName(newValue: string) { this._Transshipment3FromPortName = newValue; this.MarkAsDirty(); }

        private _Transshipment3FromPortCountryCode: string;
        public get Transshipment3FromPortCountryCode() { return this._Transshipment3FromPortCountryCode; }
        public set Transshipment3FromPortCountryCode(newValue: string) { this._Transshipment3FromPortCountryCode = newValue; this.MarkAsDirty(); }

        private _Transshipment3FromPortCountryName: string;
        public get Transshipment3FromPortCountryName() { return this._Transshipment3FromPortCountryName; }
        public set Transshipment3FromPortCountryName(newValue: string) { this._Transshipment3FromPortCountryName = newValue; this.MarkAsDirty(); }

        private _Transshipment3ToPortCode: string;
        public get Transshipment3ToPortCode() { return this._Transshipment3ToPortCode; }
        public set Transshipment3ToPortCode(newValue: string) { this._Transshipment3ToPortCode = newValue; this.MarkAsDirty(); }

        private _Transshipment3ToPortName: string;
        public get Transshipment3ToPortName() { return this._Transshipment3ToPortName; }
        public set Transshipment3ToPortName(newValue: string) { this._Transshipment3ToPortName = newValue; this.MarkAsDirty(); }

        private _Transshipment3ToPortCountryCode: string;
        public get Transshipment3ToPortCountryCode() { return this._Transshipment3ToPortCountryCode; }
        public set Transshipment3ToPortCountryCode(newValue: string) { this._Transshipment3ToPortCountryCode = newValue; this.MarkAsDirty(); }

        private _Transshipment3ToPortCountryName: string;
        public get Transshipment3ToPortCountryName() { return this._Transshipment3ToPortCountryName; }
        public set Transshipment3ToPortCountryName(newValue: string) { this._Transshipment3ToPortCountryName = newValue; this.MarkAsDirty(); }


        private _Transshipment1ToPortCountryEC: boolean;
        public get Transshipment1ToPortCountryEC() { return this._Transshipment1ToPortCountryEC; }
        public set Transshipment1ToPortCountryEC(newValue: boolean) { this._Transshipment1ToPortCountryEC = newValue; this.MarkAsDirty(); }

        private _Transshipment2ToPortCountryEC: boolean;
        public get Transshipment2ToPortCountryEC() { return this._Transshipment2ToPortCountryEC; }
        public set Transshipment2ToPortCountryEC(newValue: boolean) { this._Transshipment2ToPortCountryEC = newValue; this.MarkAsDirty(); }

        private _Transshipment3ToPortCountryEC: boolean;
        public get Transshipment3ToPortCountryEC() { return this._Transshipment3ToPortCountryEC; }
        public set Transshipment3ToPortCountryEC(newValue: boolean) { this._Transshipment3ToPortCountryEC = newValue; this.MarkAsDirty(); }

        private _FinalDistenationPortId: string;
        public get FinalDistenationPortId() { return this._FinalDistenationPortId; }
        public set FinalDistenationPortId(newValue: string) { this._FinalDistenationPortId = newValue; this.MarkAsDirty(); }


        private _Transshipment1AdditionalMAWBOBLBL: string;
        public get Transshipment1AdditionalMAWBOBLBL() { return this._Transshipment1AdditionalMAWBOBLBL; }
        public set Transshipment1AdditionalMAWBOBLBL(newValue: string) { this._Transshipment1AdditionalMAWBOBLBL = newValue; this.MarkAsDirty(); }

        private _Transshipment2AdditionalMAWBOBLBL: string;
        public get Transshipment2AdditionalMAWBOBLBL() { return this._Transshipment2AdditionalMAWBOBLBL; }
        public set Transshipment2AdditionalMAWBOBLBL(newValue: string) { this._Transshipment2AdditionalMAWBOBLBL = newValue; this.MarkAsDirty(); }

        private _Transshipment3AdditionalMAWBOBLBL: string;
        public get Transshipment3AdditionalMAWBOBLBL() { return this._Transshipment3AdditionalMAWBOBLBL; }
        public set Transshipment3AdditionalMAWBOBLBL(newValue: string) { this._Transshipment3AdditionalMAWBOBLBL = newValue; this.MarkAsDirty(); }


        private _FromPort: string;
        public get FromPort() { return this._FromPort; }
        public set FromPort(newValue: string) { this._FromPort = newValue; this.MarkAsDirty(); }

        private _FromPortName: string;
        public get FromPortName() { return this._FromPortName; }
        public set FromPortName(newValue: string) { this._FromPortName = newValue; this.MarkAsDirty(); }

        private _FromPortCountry: string;
        public get FromPortCountry() { return this._FromPortCountry; }
        public set FromPortCountry(newValue: string) { this._FromPortCountry = newValue; this.MarkAsDirty(); }

        private _FromPortCountryName: string;
        public get FromPortCountryName() { return this._FromPortCountryName; }
        public set FromPortCountryName(newValue: string) { this._FromPortCountryName = newValue; this.MarkAsDirty(); }

        private _ToPort: string;
        public get ToPort() { return this._ToPort; }
        public set ToPort(newValue: string) { this._ToPort = newValue; this.MarkAsDirty(); }

        private _ToPortName: string;
        public get ToPortName() { return this._ToPortName; }
        public set ToPortName(newValue: string) { this._ToPortName = newValue; this.MarkAsDirty(); }

        private _ToPortCountry: string;
        public get ToPortCountry() { return this._ToPortCountry; }
        public set ToPortCountry(newValue: string) { this._ToPortCountry = newValue; this.MarkAsDirty(); }

        private _ToPortCountryName: string;
        public get ToPortCountryName() { return this._ToPortCountryName; }
        public set ToPortCountryName(newValue: string) { this._ToPortCountryName = newValue; this.MarkAsDirty(); }




        private _OrderGrossWeight: number;
        public get OrderGrossWeight() { return this._OrderGrossWeight; }
        public set OrderGrossWeight(newValue: number) { this._OrderGrossWeight = newValue; this.MarkAsDirty(); }

        private _BookingVolume: number;
        public get BookingVolume() { return this._BookingVolume; }
        public set BookingVolume(newValue: number) { this._BookingVolume = newValue; this.MarkAsDirty(); }


        private _BookingNumberOfPackages: number;
        public get BookingNumberOfPackages() { return this._BookingNumberOfPackages; }
        public set BookingNumberOfPackages(newValue: number) { this._BookingNumberOfPackages = newValue; this.MarkAsDirty(); }

        private _OrderIsDangerouseGoods: boolean;
        public get OrderIsDangerouseGoods() { return this._OrderIsDangerouseGoods; }
        public set OrderIsDangerouseGoods(newValue: boolean) { this._OrderIsDangerouseGoods = newValue; this.MarkAsDirty(); }


        private _BookingConfirmationNumber: string;
        public get BookingConfirmationNumber() { return this._BookingConfirmationNumber; }
        public set BookingConfirmationNumber(newValue: string) { this._BookingConfirmationNumber = newValue; this.MarkAsDirty(); }


        private _BookingConfirmedBy: string;
        public get BookingConfirmedBy() { return this._BookingConfirmedBy; }
        public set BookingConfirmedBy(newValue: string) { this._BookingConfirmedBy = newValue; this.MarkAsDirty(); }


        private _BookingConfirmationNotes: string;
        public get BookingConfirmationNotes() { return this._BookingConfirmationNotes; }
        public set BookingConfirmationNotes(newValue: string) { this._BookingConfirmationNotes = newValue; this.MarkAsDirty(); }

        private _OrderVolumetricWeight: number;
        public get OrderVolumetricWeight() { return this._OrderVolumetricWeight; }
        public set OrderVolumetricWeight(newValue: number) { this._OrderVolumetricWeight = newValue; this.MarkAsDirty(); }

        private _OrderChargeableWeight: number;
        public get OrderChargeableWeight() { return this._OrderChargeableWeight; }
        public set OrderChargeableWeight(newValue: number) { this._OrderChargeableWeight = newValue; this.MarkAsDirty(); }


        private _CutoffDate: Date;
        public get CutoffDate() { return this._CutoffDate; }
        public set CutoffDate(newValue: Date) { this._CutoffDate = newValue; this.MarkAsDirty(); }





        private _AWBPrint: boolean;
        public get AWBPrint() { return this._AWBPrint; }
        public set AWBPrint(newValue: boolean) { this._AWBPrint = newValue; this.MarkAsDirty(); }


        private _FWBStatusCode: string;
        public get FWBStatusCode() { return this._FWBStatusCode; }
        public set FWBStatusCode(newValue: string) { this._FWBStatusCode = newValue; this.MarkAsDirty(); }


        private _FWBStatusName: string;
        public get FWBStatusName() { return this._FWBStatusName; }
        public set FWBStatusName(newValue: string) { this._FWBStatusName = newValue; this.MarkAsDirty(); }


        private _FHLStatusCode: string;
        public get FHLStatusCode() { return this._FHLStatusCode; }
        public set FHLStatusCode(newValue: string) { this._FHLStatusCode = newValue; this.MarkAsDirty(); }


        private _FHLStatusName: string;
        public get FHLStatusName() { return this._FHLStatusName; }
        public set FHLStatusName(newValue: string) { this._FHLStatusName = newValue; this.MarkAsDirty(); }


        private _AWBCurrencyId: string;
        public get AWBCurrencyId() { return this._AWBCurrencyId; }
        public set AWBCurrencyId(newValue: string) { this._AWBCurrencyId = newValue; this.MarkAsDirty(); }

        private _AWBCurrencyCode: string;
        public get AWBCurrencyCode() { return this._AWBCurrencyCode; }
        public set AWBCurrencyCode(newValue: string) { this._AWBCurrencyCode = newValue; this.MarkAsDirty(); }


        private _AWBFreightAmountPrepaid: number;
        public get AWBFreightAmountPrepaid() { return this._AWBFreightAmountPrepaid; }
        public set AWBFreightAmountPrepaid(newValue: number) { this._AWBFreightAmountPrepaid = newValue; this.MarkAsDirty(); }


        private _AWBFreightAmountCollect: number;
        public get AWBFreightAmountCollect() { return this._AWBFreightAmountCollect; }
        public set AWBFreightAmountCollect(newValue: number) { this._AWBFreightAmountCollect = newValue; this.MarkAsDirty(); }


        private _AWBCarrierTarrifReference: string;
        public get AWBCarrierTarrifReference() { return this._AWBCarrierTarrifReference; }
        public set AWBCarrierTarrifReference(newValue: string) { this._AWBCarrierTarrifReference = newValue; this.MarkAsDirty(); }


        private _AWBDeclaredValueForCarriage: string;
        public get AWBDeclaredValueForCarriage() { return this._AWBDeclaredValueForCarriage; }
        public set AWBDeclaredValueForCarriage(newValue: string) { this._AWBDeclaredValueForCarriage = newValue; this.MarkAsDirty(); }


        private _AWBDeclaredValueForCustoms: string;
        public get AWBDeclaredValueForCustoms() { return this._AWBDeclaredValueForCustoms; }
        public set AWBDeclaredValueForCustoms(newValue: string) { this._AWBDeclaredValueForCustoms = newValue; this.MarkAsDirty(); }


        private _AWBAccountingInformation: string;
        public get AWBAccountingInformation() { return this._AWBAccountingInformation; }
        public set AWBAccountingInformation(newValue: string) { this._AWBAccountingInformation = newValue; this.MarkAsDirty(); }


        private _AWBInsurrenceValue: string;
        public get AWBInsurrenceValue() { return this._AWBInsurrenceValue; }
        public set AWBInsurrenceValue(newValue: string) { this._AWBInsurrenceValue = newValue; this.MarkAsDirty(); }


        private _AWBHandlingInformation: string;
        public get AWBHandlingInformation() { return this._AWBHandlingInformation; }
        public set AWBHandlingInformation(newValue: string) { this._AWBHandlingInformation = newValue; this.MarkAsDirty(); }


        private _SCI: string;
        public get SCI() { return this._SCI; }
        public set SCI(newValue: string) { this._SCI = newValue; this.MarkAsDirty(); }


        private _AWBComments: string;
        public get AWBComments() { return this._AWBComments; }
        public set AWBComments(newValue: string) { this._AWBComments = newValue; this.MarkAsDirty(); }


        private _AWBSignature: string;
        public get AWBSignature() { return this._AWBSignature; }
        public set AWBSignature(newValue: string) { this._AWBSignature = newValue; this.MarkAsDirty(); }


        private _AWBPlace: string;
        public get AWBPlace() { return this._AWBPlace; }
        public set AWBPlace(newValue: string) { this._AWBPlace = newValue; this.MarkAsDirty(); }


        private _AWBChargesCodeCode: string;
        public get AWBChargesCodeCode() { return this._AWBChargesCodeCode; }
        public set AWBChargesCodeCode(newValue: string) { this._AWBChargesCodeCode = newValue; this.MarkAsDirty(); }



        private _TenantZeroAirlineId: string;
        public get TenantZeroAirlineId() { return this._TenantZeroAirlineId; }
        public set TenantZeroAirlineId(newValue: string) { this._TenantZeroAirlineId = newValue; this.MarkAsDirty(); }

        private _TenantZeroAirlineTTY: string;
        public get TenantZeroAirlineTTY() { return this._TenantZeroAirlineTTY; }
        public set TenantZeroAirlineTTY(newValue: string) { this._TenantZeroAirlineTTY = newValue; this.MarkAsDirty(); }

        private _TenantZeroAirlinePIMA: string;
        public get TenantZeroAirlinePIMA() { return this._TenantZeroAirlinePIMA; }
        public set TenantZeroAirlinePIMA(newValue: string) { this._TenantZeroAirlinePIMA = newValue; this.MarkAsDirty(); }

        private _TenantZeroAirlineChampFWB: boolean;
        public get TenantZeroAirlineChampFWB() { return this._TenantZeroAirlineChampFWB; }
        public set TenantZeroAirlineChampFWB(newValue: boolean) { this._TenantZeroAirlineChampFWB = newValue; this.MarkAsDirty(); }

        private _TenantZeroAirlineChampFHL: boolean;
        public get TenantZeroAirlineChampFHL() { return this._TenantZeroAirlineChampFHL; }
        public set TenantZeroAirlineChampFHL(newValue: boolean) { this._TenantZeroAirlineChampFHL = newValue; this.MarkAsDirty(); }

        private _TenantZeroAirlineChampFSU: boolean;
        public get TenantZeroAirlineChampFSU() { return this._TenantZeroAirlineChampFSU; }
        public set TenantZeroAirlineChampFSU(newValue: boolean) { this._TenantZeroAirlineChampFSU = newValue; this.MarkAsDirty(); }

        private _TenantZeroAirlineChampFSRFSA: boolean;
        public get TenantZeroAirlineChampFSRFSA() { return this._TenantZeroAirlineChampFSRFSA; }
        public set TenantZeroAirlineChampFSRFSA(newValue: boolean) { this._TenantZeroAirlineChampFSRFSA = newValue; this.MarkAsDirty(); }

        private _TenantZeroAirlineChampFVRFVA: boolean;
        public get TenantZeroAirlineChampFVRFVA() { return this._TenantZeroAirlineChampFVRFVA; }
        public set TenantZeroAirlineChampFVRFVA(newValue: boolean) { this._TenantZeroAirlineChampFVRFVA = newValue; this.MarkAsDirty(); }

        private _CarrierIsChampRegistered: boolean;
        public get CarrierIsChampRegistered() { return this._CarrierIsChampRegistered; }
        public set CarrierIsChampRegistered(newValue: boolean) { this._CarrierIsChampRegistered = newValue; this.MarkAsDirty(); }

        private _TenantZeroAirlineChampNeedsRegistration: boolean;
        public get TenantZeroAirlineChampNeedsRegistration() { return this._TenantZeroAirlineChampNeedsRegistration; }
        public set TenantZeroAirlineChampNeedsRegistration(newValue: boolean) { this._TenantZeroAirlineChampNeedsRegistration = newValue; this.MarkAsDirty(); }

        private _TenantZeroAirlineGLSHKFWB: boolean;
        public get TenantZeroAirlineGLSHKFWB() { return this._TenantZeroAirlineGLSHKFWB; }
        public set TenantZeroAirlineGLSHKFWB(newValue: boolean) { this._TenantZeroAirlineGLSHKFWB = newValue; this.MarkAsDirty(); }

        private _TenantZeroAirlineGLSHKFHL: boolean;
        public get TenantZeroAirlineGLSHKFHL() { return this._TenantZeroAirlineGLSHKFHL; }
        public set TenantZeroAirlineGLSHKFHL(newValue: boolean) { this._TenantZeroAirlineGLSHKFHL = newValue; this.MarkAsDirty(); }

        private _TenantZeroAirlineGLSHKFSU: boolean;
        public get TenantZeroAirlineGLSHKFSU() { return this._TenantZeroAirlineGLSHKFSU; }
        public set TenantZeroAirlineGLSHKFSU(newValue: boolean) { this._TenantZeroAirlineGLSHKFSU = newValue; this.MarkAsDirty(); }

        private _TenantZeroAirlineGLSHKFSRFSA: boolean;
        public get TenantZeroAirlineGLSHKFSRFSA() { return this._TenantZeroAirlineGLSHKFSRFSA; }
        public set TenantZeroAirlineGLSHKFSRFSA(newValue: boolean) { this._TenantZeroAirlineGLSHKFSRFSA = newValue; this.MarkAsDirty(); }

        private _TenantZeroAirlineGLSHKFVRFVA: boolean;
        public get TenantZeroAirlineGLSHKFVRFVA() { return this._TenantZeroAirlineGLSHKFVRFVA; }
        public set TenantZeroAirlineGLSHKFVRFVA(newValue: boolean) { this._TenantZeroAirlineGLSHKFVRFVA = newValue; this.MarkAsDirty(); }

        private _CarrierIsGLSHKRegistered: boolean;
        public get CarrierIsGLSHKRegistered() { return this._CarrierIsGLSHKRegistered; }
        public set CarrierIsGLSHKRegistered(newValue: boolean) { this._CarrierIsGLSHKRegistered = newValue; this.MarkAsDirty(); }

        private _TenantZeroAirlineGLSHKNeedsRegistration: boolean;
        public get TenantZeroAirlineGLSHKNeedsRegistration() { return this._TenantZeroAirlineGLSHKNeedsRegistration; }
        public set TenantZeroAirlineGLSHKNeedsRegistration(newValue: boolean) { this._TenantZeroAirlineGLSHKNeedsRegistration = newValue; this.MarkAsDirty(); }

        private _CarrierIsCheckDigit: boolean;
        public get CarrierIsCheckDigit() { return this._CarrierIsCheckDigit; }
        public set CarrierIsCheckDigit(newValue: boolean) { this._CarrierIsCheckDigit = newValue; this.MarkAsDirty(); }

        private _CarrierIsLimitedLength: boolean;
        public get CarrierIsLimitedLength() { return this._CarrierIsLimitedLength; }
        public set CarrierIsLimitedLength(newValue: boolean) { this._CarrierIsLimitedLength = newValue; this.MarkAsDirty(); }



        private _ProductCode: string;
        public get ProductCode() { return this._ProductCode; }
        public set ProductCode(newValue: string) { this._ProductCode = newValue; this.MarkAsDirty(); }

        private _SecurityKey: string;
        public get SecurityKey() { return this._SecurityKey; }
        public set SecurityKey(newValue: string) { this._SecurityKey = newValue; this.MarkAsDirty(); }

        private _TEU: number;
        public get TEU() { return this._TEU; }
        public set TEU(newValue: number) { this._TEU = newValue; this.MarkAsDirty(); }

        private _ConvertFromHouseToDirect: boolean;
        public get ConvertFromHouseToDirect() { return this._ConvertFromHouseToDirect; }
        public set ConvertFromHouseToDirect(newValue: boolean) { this._ConvertFromHouseToDirect = newValue; this.MarkAsDirty(); }

        private _ConvertFromDirectToHouse: boolean;
        public get ConvertFromDirectToHouse() { return this._ConvertFromDirectToHouse; }
        public set ConvertFromDirectToHouse(newValue: boolean) { this._ConvertFromDirectToHouse = newValue; this.MarkAsDirty(); }

        private _CustomerRankName: string;
        public get CustomerRankName() { return this._CustomerRankName; }
        public set CustomerRankName(newValue: string) { this._CustomerRankName = newValue; this.MarkAsDirty(); }

        private _FinalArrivalDate: Date;
        public get FinalArrivalDate() { return this._FinalArrivalDate; }
        public set FinalArrivalDate(newValue: Date) { this._FinalArrivalDate = newValue; this.MarkAsDirty(); }

        private _ForeignPartnerCountryCode: string;
        public get ForeignPartnerCountryCode() { return this._ForeignPartnerCountryCode; }
        public set ForeignPartnerCountryCode(newValue: string) { this._ForeignPartnerCountryCode = newValue; this.MarkAsDirty(); }

        private _LastStatusLogDate: Date;
        public get LastStatusLogDate() { return this._LastStatusLogDate; }
        public set LastStatusLogDate(newValue: Date) { this._LastStatusLogDate = newValue; this.MarkAsDirty(); }

        private _MainCarriageFullCarrierNumber: string;
        public get MainCarriageFullCarrierNumber() { return this._MainCarriageFullCarrierNumber; }
        public set MainCarriageFullCarrierNumber(newValue: string) { this._MainCarriageFullCarrierNumber = newValue; this.MarkAsDirty(); }

        private _Transshipment1FullCarrierNumber: string;
        public get Transshipment1FullCarrierNumber() { return this._Transshipment1FullCarrierNumber; }
        public set Transshipment1FullCarrierNumber(newValue: string) { this._Transshipment1FullCarrierNumber = newValue; this.MarkAsDirty(); }

        private _Transshipment2FullCarrierNumber: string;
        public get Transshipment2FullCarrierNumber() { return this._Transshipment2FullCarrierNumber; }
        public set Transshipment2FullCarrierNumber(newValue: string) { this._Transshipment2FullCarrierNumber = newValue; this.MarkAsDirty(); }

        private _Transshipment3FullCarrierNumber: string;
        public get Transshipment3FullCarrierNumber() { return this._Transshipment3FullCarrierNumber; }
        public set Transshipment3FullCarrierNumber(newValue: string) { this._Transshipment3FullCarrierNumber = newValue; this.MarkAsDirty(); }

        private _ExceptionDescription: string;
        public get ExceptionDescription() { return this._ExceptionDescription; }
        public set ExceptionDescription(newValue: string) { this._ExceptionDescription = newValue; this.MarkAsDirty(); }

        private _ExceptionDate: Date;
        public get ExceptionDate() { return this._ExceptionDate; }
        public set ExceptionDate(newValue: Date) { this._ExceptionDate = newValue; this.MarkAsDirty(); }

        private _HasException: boolean;
        public get HasException() { return this._HasException; }
        public set HasException(newValue: boolean) { this._HasException = newValue; this.MarkAsDirty(); }

        private _FromLocation: string;
        public get FromLocation() { return this._FromLocation; }
        public set FromLocation(newValue: string) { this._FromLocation = newValue; this.MarkAsDirty(); }

        private _ToLocation: string;
        public get ToLocation() { return this._ToLocation; }
        public set ToLocation(newValue: string) { this._ToLocation = newValue; this.MarkAsDirty(); }

        private _MoveTypeId: string;
        public get MoveTypeId() { return this._MoveTypeId; }
        public set MoveTypeId(newValue: string) { this._MoveTypeId = newValue; this.MarkAsDirty(); }

        private _MoveTypeCode: string;
        public get MoveTypeCode() { return this._MoveTypeCode; }
        public set MoveTypeCode(newValue: string) { this._MoveTypeCode = newValue; this.MarkAsDirty(); }

        private _MoveTypeName: string;
        public get MoveTypeName() { return this._MoveTypeName; }
        public set MoveTypeName(newValue: string) { this._MoveTypeName = newValue; this.MarkAsDirty(); }

        private _AMSBL: string;
        public get AMSBL() { return this._AMSBL; }
        public set AMSBL(newValue: string) { this._AMSBL = newValue; this.MarkAsDirty(); }

        private _CustomFileId: string;
        public get CustomFileId() { return this._CustomFileId; }
        public set CustomFileId(newValue: string) { this._CustomFileId = newValue; this.MarkAsDirty(); }

        private _CustomFileNumber: string;
        public get CustomFileNumber() { return this._CustomFileNumber; }
        public set CustomFileNumber(newValue: string) { this._CustomFileNumber = newValue; this.MarkAsDirty(); }

        private _MainCarriageSTD: Date;
        public get MainCarriageSTD() { return this._MainCarriageSTD; }
        public set MainCarriageSTD(newValue: Date) { this._MainCarriageSTD = newValue; this.MarkAsDirty(); }

        private _MainCarriageSTA: Date;
        public get MainCarriageSTA() { return this._MainCarriageSTA; }
        public set MainCarriageSTA(newValue: Date) { this._MainCarriageSTA = newValue; this.MarkAsDirty(); }

        private _Transshipment1STD: Date;
        public get Transshipment1STD() { return this._Transshipment1STD; }
        public set Transshipment1STD(newValue: Date) { this._Transshipment1STD = newValue; this.MarkAsDirty(); }

        private _Transshipment1STA: Date;
        public get Transshipment1STA() { return this._Transshipment1STA; }
        public set Transshipment1STA(newValue: Date) { this._Transshipment1STA = newValue; this.MarkAsDirty(); }

        private _Transshipment2STD: Date;
        public get Transshipment2STD() { return this._Transshipment2STD; }
        public set Transshipment2STD(newValue: Date) { this._Transshipment2STD = newValue; this.MarkAsDirty(); }

        private _Transshipment2STA: Date;
        public get Transshipment2STA() { return this._Transshipment2STA; }
        public set Transshipment2STA(newValue: Date) { this._Transshipment2STA = newValue; this.MarkAsDirty(); }

        private _Transshipment3STD: Date;
        public get Transshipment3STD() { return this._Transshipment3STD; }
        public set Transshipment3STD(newValue: Date) { this._Transshipment3STD = newValue; this.MarkAsDirty(); }

        private _Transshipment3STA: Date;
        public get Transshipment3STA() { return this._Transshipment3STA; }
        public set Transshipment3STA(newValue: Date) { this._Transshipment3STA = newValue; this.MarkAsDirty(); }

        private _IsMultipleCommodities: boolean;
        public get IsMultipleCommodities() { return this._IsMultipleCommodities; }
        public set IsMultipleCommodities(newValue: boolean) { this._IsMultipleCommodities = newValue; this.MarkAsDirty(); }


        private _NominatedHandlingPartyId: string;
        public get NominatedHandlingPartyId() { return this._NominatedHandlingPartyId; }
        public set NominatedHandlingPartyId(newValue: string) { this._NominatedHandlingPartyId = newValue; this.MarkAsDirty(); }


        private _OtherParticipantInformationId1: string;
        public get OtherParticipantInformationId1() { return this._OtherParticipantInformationId1; }
        public set OtherParticipantInformationId1(newValue: string) { this._OtherParticipantInformationId1 = newValue; this.MarkAsDirty(); }


        private _OtherParticipantInformationId2: string;
        public get OtherParticipantInformationId2() { return this._OtherParticipantInformationId2; }
        public set OtherParticipantInformationId2(newValue: string) { this._OtherParticipantInformationId2 = newValue; this.MarkAsDirty(); }


        private _OtherParticipantInformationId3: string;
        public get OtherParticipantInformationId3() { return this._OtherParticipantInformationId3; }
        public set OtherParticipantInformationId3(newValue: string) { this._OtherParticipantInformationId3 = newValue; this.MarkAsDirty(); }


        private _OtherParticipantInformationCode1: string;
        public get OtherParticipantInformationCode1() { return this._OtherParticipantInformationCode1; }
        public set OtherParticipantInformationCode1(newValue: string) { this._OtherParticipantInformationCode1 = newValue; this.MarkAsDirty(); }


        private _OtherParticipantInformationCode2: string;
        public get OtherParticipantInformationCode2() { return this._OtherParticipantInformationCode2; }
        public set OtherParticipantInformationCode2(newValue: string) { this._OtherParticipantInformationCode2 = newValue; this.MarkAsDirty(); }


        private _OtherParticipantInformationCode3: string;
        public get OtherParticipantInformationCode3() { return this._OtherParticipantInformationCode3; }
        public set OtherParticipantInformationCode3(newValue: string) { this._OtherParticipantInformationCode3 = newValue; this.MarkAsDirty(); }


        private _OtherParticipantInformationPortCode1: string;
        public get OtherParticipantInformationPortCode1() { return this._OtherParticipantInformationPortCode1; }
        public set OtherParticipantInformationPortCode1(newValue: string) { this._OtherParticipantInformationPortCode1 = newValue; this.MarkAsDirty(); }


        private _OtherParticipantInformationPortCode2: string;
        public get OtherParticipantInformationPortCode2() { return this._OtherParticipantInformationPortCode2; }
        public set OtherParticipantInformationPortCode2(newValue: string) { this._OtherParticipantInformationPortCode2 = newValue; this.MarkAsDirty(); }


        private _OtherParticipantInformationPortCode3: string;
        public get OtherParticipantInformationPortCode3() { return this._OtherParticipantInformationPortCode3; }
        public set OtherParticipantInformationPortCode3(newValue: string) { this._OtherParticipantInformationPortCode3 = newValue; this.MarkAsDirty(); }


        private _OtherParticipantInformationName1: string;
        public get OtherParticipantInformationName1() { return this._OtherParticipantInformationName1; }
        public set OtherParticipantInformationName1(newValue: string) { this._OtherParticipantInformationName1 = newValue; this.MarkAsDirty(); }


        private _OtherParticipantInformationName2: string;
        public get OtherParticipantInformationName2() { return this._OtherParticipantInformationName2; }
        public set OtherParticipantInformationName2(newValue: string) { this._OtherParticipantInformationName2 = newValue; this.MarkAsDirty(); }


        private _OtherParticipantInformationName3: string;
        public get OtherParticipantInformationName3() { return this._OtherParticipantInformationName3; }
        public set OtherParticipantInformationName3(newValue: string) { this._OtherParticipantInformationName3 = newValue; this.MarkAsDirty(); }


        private _OtherParticipantInformationReference1: string;
        public get OtherParticipantInformationReference1() { return this._OtherParticipantInformationReference1; }
        public set OtherParticipantInformationReference1(newValue: string) { this._OtherParticipantInformationReference1 = newValue; this.MarkAsDirty(); }


        private _OtherParticipantInformationReference2: string;
        public get OtherParticipantInformationReference2() { return this._OtherParticipantInformationReference2; }
        public set OtherParticipantInformationReference2(newValue: string) { this._OtherParticipantInformationReference2 = newValue; this.MarkAsDirty(); }


        private _OtherParticipantInformationReference3: string;
        public get OtherParticipantInformationReference3() { return this._OtherParticipantInformationReference3; }
        public set OtherParticipantInformationReference3(newValue: string) { this._OtherParticipantInformationReference3 = newValue; this.MarkAsDirty(); }


        private _AccountingInformation1: string;
        public get AccountingInformation1() { return this._AccountingInformation1; }
        public set AccountingInformation1(newValue: string) { this._AccountingInformation1 = newValue; this.MarkAsDirty(); }


        private _AccountingInformation2: string;
        public get AccountingInformation2() { return this._AccountingInformation2; }
        public set AccountingInformation2(newValue: string) { this._AccountingInformation2 = newValue; this.MarkAsDirty(); }


        private _AccountingInformation3: string;
        public get AccountingInformation3() { return this._AccountingInformation3; }
        public set AccountingInformation3(newValue: string) { this._AccountingInformation3 = newValue; this.MarkAsDirty(); }


        private _AccountingInformation4: string;
        public get AccountingInformation4() { return this._AccountingInformation4; }
        public set AccountingInformation4(newValue: string) { this._AccountingInformation4 = newValue; this.MarkAsDirty(); }


        private _AccountingInformation5: string;
        public get AccountingInformation5() { return this._AccountingInformation5; }
        public set AccountingInformation5(newValue: string) { this._AccountingInformation5 = newValue; this.MarkAsDirty(); }


        private _AccountingInformation6: string;
        public get AccountingInformation6() { return this._AccountingInformation6; }
        public set AccountingInformation6(newValue: string) { this._AccountingInformation6 = newValue; this.MarkAsDirty(); }


        private _AccountingInformationIdentifierCode1: string;
        public get AccountingInformationIdentifierCode1() { return this._AccountingInformationIdentifierCode1; }
        public set AccountingInformationIdentifierCode1(newValue: string) { this._AccountingInformationIdentifierCode1 = newValue; this.MarkAsDirty(); }


        private _AccountingInformationIdentifierCode2: string;
        public get AccountingInformationIdentifierCode2() { return this._AccountingInformationIdentifierCode2; }
        public set AccountingInformationIdentifierCode2(newValue: string) { this._AccountingInformationIdentifierCode2 = newValue; this.MarkAsDirty(); }


        private _AccountingInformationIdentifierCode3: string;
        public get AccountingInformationIdentifierCode3() { return this._AccountingInformationIdentifierCode3; }
        public set AccountingInformationIdentifierCode3(newValue: string) { this._AccountingInformationIdentifierCode3 = newValue; this.MarkAsDirty(); }


        private _AccountingInformationIdentifierCode4: string;
        public get AccountingInformationIdentifierCode4() { return this._AccountingInformationIdentifierCode4; }
        public set AccountingInformationIdentifierCode4(newValue: string) { this._AccountingInformationIdentifierCode4 = newValue; this.MarkAsDirty(); }


        private _AccountingInformationIdentifierCode5: string;
        public get AccountingInformationIdentifierCode5() { return this._AccountingInformationIdentifierCode5; }
        public set AccountingInformationIdentifierCode5(newValue: string) { this._AccountingInformationIdentifierCode5 = newValue; this.MarkAsDirty(); }


        private _AccountingInformationIdentifierCode6: string;
        public get AccountingInformationIdentifierCode6() { return this._AccountingInformationIdentifierCode6; }
        public set AccountingInformationIdentifierCode6(newValue: string) { this._AccountingInformationIdentifierCode6 = newValue; this.MarkAsDirty(); }


        private _ReferenceNumber: string;
        public get ReferenceNumber() { return this._ReferenceNumber; }
        public set ReferenceNumber(newValue: string) { this._ReferenceNumber = newValue; this.MarkAsDirty(); }


        private _SupplementaryShipmentInformation1: string;
        public get SupplementaryShipmentInformation1() { return this._SupplementaryShipmentInformation1; }
        public set SupplementaryShipmentInformation1(newValue: string) { this._SupplementaryShipmentInformation1 = newValue; this.MarkAsDirty(); }


        private _SupplementaryShipmentInformation2: string;
        public get SupplementaryShipmentInformation2() { return this._SupplementaryShipmentInformation2; }
        public set SupplementaryShipmentInformation2(newValue: string) { this._SupplementaryShipmentInformation2 = newValue; this.MarkAsDirty(); }



        public FollowUps: Array<any>;
      

        public ShipmentReceivables: Array<any>;

        public ShipmentPayables: Array<any>;

        public ShipmentPackages: Array<ShipmentPackagePM>;

        public ShipmentOrderPackages: Array<any>;

        public ShipmentPickUps: Array<any>;

        public ShipmentDeliveries: Array<any>;

        public ShipmentARInvoices: Array<any>;

        public ShipmentAPInvoices: Array<any>;

        public ShipmentConsoleShipments: Array<any>;


        public ShipmentAWBPrintOnlies: Array<any>;

        public AWBOCIPMs: Array<any>;

        public ShipmentCarrierStatuses: Array<any>;

        public ShipmentCommodities: Array<any>;




        private _MainCarriageFromPartnerId: string;
        public get MainCarriageFromPartnerId() { return this._MainCarriageFromPartnerId; }
        public set MainCarriageFromPartnerId(newValue: string) { this._MainCarriageFromPartnerId = newValue; this.MarkAsDirty(); }

        private _MainCarriageFromAddressId: string;
        public get MainCarriageFromAddressId() { return this._MainCarriageFromAddressId; }
        public set MainCarriageFromAddressId(newValue: string) { this._MainCarriageFromAddressId = newValue; this.MarkAsDirty(); }

        private _MainCarriageToPartnerId: string;
        public get MainCarriageToPartnerId() { return this._MainCarriageToPartnerId; }
        public set MainCarriageToPartnerId(newValue: string) { this._MainCarriageToPartnerId = newValue; this.MarkAsDirty(); }

        private _MainCarriageToAddressId: string;
        public get MainCarriageToAddressId() { return this._MainCarriageToAddressId; }
        public set MainCarriageToAddressId(newValue: string) { this._MainCarriageToAddressId = newValue; this.MarkAsDirty(); }

        private _Driver: string;
        public get Driver() { return this._Driver; }
        public set Driver(newValue: string) { this._Driver = newValue; this.MarkAsDirty(); }

        private _TruckNumber: string;
        public get TruckNumber() { return this._TruckNumber; }
        public set TruckNumber(newValue: string) { this._TruckNumber = newValue; this.MarkAsDirty(); }

        private _TrailerNumber: string;
        public get TrailerNumber() { return this._TrailerNumber; }
        public set TrailerNumber(newValue: string) { this._TrailerNumber = newValue; this.MarkAsDirty(); }

        private _AsAgreedFreight: boolean;
        public get AsAgreedFreight() { return this._AsAgreedFreight; }
        public set AsAgreedFreight(newValue: boolean) { this._AsAgreedFreight = newValue; this.MarkAsDirty(); }

        private _AsAgreedOtherCharges: boolean;
        public get AsAgreedOtherCharges() { return this._AsAgreedOtherCharges; }
        public set AsAgreedOtherCharges(newValue: boolean) { this._AsAgreedOtherCharges = newValue; this.MarkAsDirty(); }


        private _ARInvoiceIssued: boolean;
        public get ARInvoiceIssued() { return this._ARInvoiceIssued; }
        public set ARInvoiceIssued(newValue: boolean) { this._ARInvoiceIssued = newValue; this.MarkAsDirty(); }

        private _CreditNoteIssued: boolean;
        public get CreditNoteIssued() { return this._CreditNoteIssued; }
        public set CreditNoteIssued(newValue: boolean) { this._CreditNoteIssued = newValue; this.MarkAsDirty(); }



        private _AccountNumber: string;
        public get AccountNumber() { return this._AccountNumber; }
        public set AccountNumber(newValue: string) { this._AccountNumber = newValue; this.MarkAsDirty(); }

        private _CargonautFHLStatusCode: string;
        public get CargonautFHLStatusCode() { return this._CargonautFHLStatusCode; }
        public set CargonautFHLStatusCode(newValue: string) { this._CargonautFHLStatusCode = newValue; this.MarkAsDirty(); }

        private _CargonautFHLStatusName: string;
        public get CargonautFHLStatusName() { return this._CargonautFHLStatusName; }
        public set CargonautFHLStatusName(newValue: string) { this._CargonautFHLStatusName = newValue; this.MarkAsDirty(); }

        private _CargonautFHLStatusDate: Date;
        public get CargonautFHLStatusDate() { return this._CargonautFHLStatusDate; }
        public set CargonautFHLStatusDate(newValue: Date) { this._CargonautFHLStatusDate = newValue; this.MarkAsDirty(); }

        private _CargonautFWBStatusCode: string;
        public get CargonautFWBStatusCode() { return this._CargonautFWBStatusCode; }
        public set CargonautFWBStatusCode(newValue: string) { this._CargonautFWBStatusCode = newValue; this.MarkAsDirty(); }

        private _CargonautFWBStatusName: string;
        public get CargonautFWBStatusName() { return this._CargonautFWBStatusName; }
        public set CargonautFWBStatusName(newValue: string) { this._CargonautFWBStatusName = newValue; this.MarkAsDirty(); }

        private _CargonautFWBStatusDate: Date;
        public get CargonautFWBStatusDate() { return this._CargonautFWBStatusDate; }
        public set CargonautFWBStatusDate(newValue: Date) { this._CargonautFWBStatusDate = newValue; this.MarkAsDirty(); }
        // Dummy
        private _MarkFollowUpsAsDone: boolean;
        public get MarkFollowUpsAsDone() { return this._MarkFollowUpsAsDone; }
        public set MarkFollowUpsAsDone(newValue: boolean) { this._MarkFollowUpsAsDone = newValue; this.MarkAsDirty(); }

        private _CalculateProfit: boolean;
        public get CalculateProfit() { return this._CalculateProfit; }
        public set CalculateProfit(newValue: boolean) { this._CalculateProfit = newValue; this.MarkAsDirty(); }

        private _CalculateStatus: boolean;
        public get CalculateStatus() { return this._CalculateStatus; }
        public set CalculateStatus(newValue: boolean) { this._CalculateStatus = newValue; this.MarkAsDirty(); }


        private _ComputedStatusId: string;
        public get ComputedStatusId() { return this._ComputedStatusId; }
        public set ComputedStatusId(newValue: string) { this._ComputedStatusId = newValue; this.MarkAsDirty(); }

        private _ComputedStatusDate: Date;
        public get ComputedStatusDate() { return this._ComputedStatusDate; }
        public set ComputedStatusDate(newValue: Date) { this._ComputedStatusDate = newValue; this.MarkAsDirty(); }

        private _ComputedStatusName: string;
        public get ComputedStatusName() { return this._ComputedStatusName; }
        public set ComputedStatusName(newValue: string) { this._ComputedStatusName = newValue; this.MarkAsDirty(); }

        private _CustomFilePocoId: string;
        public get CustomFilePocoId() { return this._CustomFilePocoId; }
        public set CustomFilePocoId(newValue: string) { this._CustomFilePocoId = newValue; this.MarkAsDirty(); }


        private _CalculatePayables: boolean;
        public get CalculatePayables() { return this._CalculatePayables; }
        public set CalculatePayables(newValue: boolean) { this._CalculatePayables = newValue; this.MarkAsDirty(); }

        private _IsAddingStackEvents: boolean;
        public get IsAddingStackEvents() { return this._IsAddingStackEvents; }
        public set IsAddingStackEvents(newValue: boolean) { this._IsAddingStackEvents = newValue; this.MarkAsDirty(); }

        private _IsRemovingStackEvents: boolean;
        public get IsRemovingStackEvents() { return this._IsRemovingStackEvents; }
        public set IsRemovingStackEvents(newValue: boolean) { this._IsRemovingStackEvents = newValue; this.MarkAsDirty(); }

        private _StackAirlineId: string;
        public get StackAirlineId() { return this._StackAirlineId; }
        public set StackAirlineId(newValue: string) { this._StackAirlineId = newValue; this.MarkAsDirty(); }

        private _FromPartnerCity: string;
        public get FromPartnerCity() { return this._FromPartnerCity; }
        public set FromPartnerCity(newValue: string) { this._FromPartnerCity = newValue; this.MarkAsDirty(); }

        private _FromPartnerCountryCode: string;
        public get FromPartnerCountryCode() { return this._FromPartnerCountryCode; }
        public set FromPartnerCountryCode(newValue: string) { this._FromPartnerCountryCode = newValue; this.MarkAsDirty(); }

        private _FromPartnerCountryName: string;
        public get FromPartnerCountryName() { return this._FromPartnerCountryName; }
        public set FromPartnerCountryName(newValue: string) { this._FromPartnerCountryName = newValue; this.MarkAsDirty(); }

        private _ToPartnerCity: string;
        public get ToPartnerCity() { return this._ToPartnerCity; }
        public set ToPartnerCity(newValue: string) { this._ToPartnerCity = newValue; this.MarkAsDirty(); }

        private _ToPartnerCountryCode: string;
        public get ToPartnerCountryCode() { return this._ToPartnerCountryCode; }
        public set ToPartnerCountryCode(newValue: string) { this._ToPartnerCountryCode = newValue; this.MarkAsDirty(); }

        private _ToPartnerCountryName: string;
        public get ToPartnerCountryName() { return this._ToPartnerCountryName; }
        public set ToPartnerCountryName(newValue: string) { this._ToPartnerCountryName = newValue; this.MarkAsDirty(); }

        private _IsSendFSRCreatingShipment: boolean;
        public get IsSendFSRCreatingShipment() { return this._IsSendFSRCreatingShipment; }
        public set IsSendFSRCreatingShipment(newValue: boolean) { this._IsSendFSRCreatingShipment = newValue; this.MarkAsDirty(); }

        private _ToCountryId: string;
        public get ToCountryId() { return this._ToCountryId; }
        public set ToCountryId(newValue: string) { this._ToCountryId = newValue; this.MarkAsDirty(); }

        private _FromCountryId: string;
        public get FromCountryId() { return this._FromCountryId; }
        public set FromCountryId(newValue: string) { this._FromCountryId = newValue; this.MarkAsDirty(); }

        private _ToCountryIsEC: boolean;
        public get _oCountryIsEC() { return this._ToCountryIsEC; }
        public set _oCountryIsEC(newValue: boolean) { this._ToCountryIsEC = newValue; this.MarkAsDirty(); }

        private _FromCountryIsEC: boolean;
        public get FromCountryIsEC() { return this._FromCountryIsEC; }
        public set FromCountryIsEC(newValue: boolean) { this._FromCountryIsEC = newValue; this.MarkAsDirty(); }

        private _CopyFromShipmentId: string;
        public get CopyFromShipmentId() { return this._CopyFromShipmentId; }
        public set CopyFromShipmentId(newValue: string) { this._CopyFromShipmentId = newValue; this.MarkAsDirty(); }

        private _IsCopyFromShipment: boolean;
        public get IsCopyFromShipment() { return this._IsCopyFromShipment; }
        public set IsCopyFromShipment(newValue: boolean) { this._IsCopyFromShipment = newValue; this.MarkAsDirty(); }

        private _IsBuildFromQuote: boolean;
        public get IsBuildFromQuote() { return this._IsBuildFromQuote; }
        public set IsBuildFromQuote(newValue: boolean) { this._IsBuildFromQuote = newValue; this.MarkAsDirty(); }

        private _IsBuildFromBooking: boolean;
        public get IsBuildFromBooking() { return this._IsBuildFromBooking; }
        public set IsBuildFromBooking(newValue: boolean) { this._IsBuildFromBooking = newValue; this.MarkAsDirty(); }

        private _ShipperMainAddressId: string;
        public get ShipperMainAddressId() { return this._ShipperMainAddressId; }
        public set ShipperMainAddressId(newValue: string) { this._ShipperMainAddressId = newValue; this.MarkAsDirty(); }

        private _ShipperPickAddressId: string;
        public get ShipperPickAddressId() { return this._ShipperPickAddressId; }
        public set ShipperPickAddressId(newValue: string) { this._ShipperPickAddressId = newValue; this.MarkAsDirty(); }

        private _ConsigneeMainAddressId: string;
        public get ConsigneeMainAddressId() { return this._ConsigneeMainAddressId; }
        public set ConsigneeMainAddressId(newValue: string) { this._ConsigneeMainAddressId = newValue; this.MarkAsDirty(); }

        private _ConsigneePickAddressId: string;
        public get ConsigneePickAddressId() { return this._ConsigneePickAddressId; }
        public set ConsigneePickAddressId(newValue: string) { this._ConsigneePickAddressId = newValue; this.MarkAsDirty(); }

        private _HasPreCarriage: boolean;
        public get HasPreCarriage() { return this._HasPreCarriage; }
        public set HasPreCarriage(newValue: boolean) { this._HasPreCarriage = newValue; this.MarkAsDirty(); }

        private _HasOnCarriage: boolean;
        public get HasOnCarriage() { return this._HasOnCarriage; }
        public set HasOnCarriage(newValue: boolean) { this._HasOnCarriage = newValue; this.MarkAsDirty(); }


        private _IncludePickUp: boolean;
        public get IncludePickUp() { return this._IncludePickUp; }
        public set IncludePickUp(newValue: boolean) { this._IncludePickUp = newValue; this.MarkAsDirty(); }

        private _FromAddressCity: string;
        public get FromAddressCity() { return this._FromAddressCity; }
        public set FromAddressCity(newValue: string) { this._FromAddressCity = newValue; this.MarkAsDirty(); }

        private _FromAddressZipCode: string;
        public get FromAddressZipCode() { return this._FromAddressZipCode; }
        public set FromAddressZipCode(newValue: string) { this._FromAddressZipCode = newValue; this.MarkAsDirty(); }

        private _FromAddressCountryId: string;
        public get FromAddressCountryId() { return this._FromAddressCountryId; }
        public set FromAddressCountryId(newValue: string) { this._FromAddressCountryId = newValue; this.MarkAsDirty(); }

        private _PickUpAddressId: string;
        public get PickUpAddressId() { return this._PickUpAddressId; }
        public set PickUpAddressId(newValue: string) { this._PickUpAddressId = newValue; this.MarkAsDirty(); }

        private _CustomConnectToShipment: boolean;
        public get CustomConnectToShipment() { return this._CustomConnectToShipment; }
        public set CustomConnectToShipment(newValue: boolean) { this._CustomConnectToShipment = newValue; this.MarkAsDirty(); }

        private _IncludeDelivery: boolean;
        public get IncludeDelivery() { return this._IncludeDelivery; }
        public set IncludeDelivery(newValue: boolean) { this._IncludeDelivery = newValue; this.MarkAsDirty(); }

        private _ToAddressCity: string;
        public get ToAddressCity() { return this._ToAddressCity; }
        public set ToAddressCity(newValue: string) { this._ToAddressCity = newValue; this.MarkAsDirty(); }

        private _ToAddressZipCode: string;
        public get ToAddressZipCode() { return this._ToAddressZipCode; }
        public set ToAddressZipCode(newValue: string) { this._ToAddressZipCode = newValue; this.MarkAsDirty(); }


        private _ToAddressCountryId: string;
        public get ToAddressCountryId() { return this._ToAddressCountryId; }
        public set ToAddressCountryId(newValue: string) { this._ToAddressCountryId = newValue; this.MarkAsDirty(); }

        private _DeliveryAddressId: string;
        public get DeliveryAddressId() { return this._DeliveryAddressId; }
        public set DeliveryAddressId(newValue: string) { this._DeliveryAddressId = newValue; this.MarkAsDirty(); }

        private _SpecialServicesTypeId: string;
        public get SpecialServicesTypeId() { return this._SpecialServicesTypeId; }
        public set SpecialServicesTypeId(newValue: string) { this._SpecialServicesTypeId = newValue; this.MarkAsDirty(); }

        private _SpecialServicesTypeName: string;
        public get SpecialServicesTypeName() { return this._SpecialServicesTypeName; }
        public set SpecialServicesTypeName(newValue: string) { this._SpecialServicesTypeName = newValue; this.MarkAsDirty(); }


        private _Quantity1: number;
        public get Quantity1() { return this._Quantity1; }
        public set Quantity1(newValue: number) { this._Quantity1 = newValue; this.MarkAsDirty(); }

        private _Quantity2: number;
        public get Quantity2() { return this._Quantity2; }
        public set Quantity2(newValue: number) { this._Quantity2 = newValue; this.MarkAsDirty(); }

        private _Quantity3: number;
        public get Quantity3() { return this._Quantity3; }
        public set Quantity3(newValue: number) { this._Quantity3 = newValue; this.MarkAsDirty(); }

        private _Quantity4: number;
        public get Quantity4() { return this._Quantity4; }
        public set Quantity4(newValue: number) { this._Quantity4 = newValue; this.MarkAsDirty(); }

        private _Quantity5: number;
        public get Quantity5() { return this._Quantity5; }
        public set Quantity5(newValue: number) { this._Quantity5 = newValue; this.MarkAsDirty(); }


        private _PackageTypeId1: string;
        public get PackageTypeId1() { return this._PackageTypeId1; }
        public set PackageTypeId1(newValue: string) { this._PackageTypeId1 = newValue; this.MarkAsDirty(); }

        private _PackageTypeId2: string;
        public get PackageTypeId2() { return this._PackageTypeId2; }
        public set PackageTypeId2(newValue: string) { this._PackageTypeId2 = newValue; this.MarkAsDirty(); }

        private _PackageTypeId3: string;
        public get PackageTypeId3() { return this._PackageTypeId3; }
        public set PackageTypeId3(newValue: string) { this._PackageTypeId3 = newValue; this.MarkAsDirty(); }

        private _PackageTypeId4: string;
        public get PackageTypeId4() { return this._PackageTypeId4; }
        public set PackageTypeId4(newValue: string) { this._PackageTypeId4 = newValue; this.MarkAsDirty(); }

        private _PackageTypeId5: string;
        public get PackageTypeId5() { return this._PackageTypeId5; }
        public set PackageTypeId5(newValue: string) { this._PackageTypeId5 = newValue; this.MarkAsDirty(); }


        private _NewConcurrencyGUID: string;
        public get NewConcurrencyGUID() { return this._NewConcurrencyGUID; }
        public set NewConcurrencyGUID(newValue: string) { this._NewConcurrencyGUID = newValue; this.MarkAsDirty(); }

        private _ConnectedShipmentsPayablesCount: number;
        public get ConnectedShipmentsPayablesCount() { return this._ConnectedShipmentsPayablesCount; }
        public set ConnectedShipmentsPayablesCount(newValue: number) { this._ConnectedShipmentsPayablesCount = newValue; this.MarkAsDirty(); }

        private _ConnectedShipmentsReceivablesCount: number;
        public get ConnectedShipmentsReceivablesCount() { return this._ConnectedShipmentsReceivablesCount; }
        public set ConnectedShipmentsReceivablesCount(newValue: number) { this._ConnectedShipmentsReceivablesCount = newValue; this.MarkAsDirty(); }

        private _NumberOfInsidePackages: number;
        public get NumberOfInsidePackages() { return this._NumberOfInsidePackages; }
        public set NumberOfInsidePackages(newValue: number) { this._NumberOfInsidePackages = newValue; this.MarkAsDirty(); }

        private _NumberOfInsidePackagesDetails: string;
        public get NumberOfInsidePackagesDetails() { return this._NumberOfInsidePackagesDetails; }
        public set NumberOfInsidePackagesDetails(newValue: string) { this._NumberOfInsidePackagesDetails = newValue; this.MarkAsDirty(); }


        private _AccountManagerUserId: string;
        public get AccountManagerUserId() { return this._AccountManagerUserId; }
        public set AccountManagerUserId(newValue: string) { this._AccountManagerUserId = newValue; this.MarkAsDirty(); }

        private _AccountManagerUserName: string;
        public get AccountManagerUserName() { return this._AccountManagerUserName; }
        public set AccountManagerUserName(newValue: string) { this._AccountManagerUserName = newValue; this.MarkAsDirty(); }



        private _ManifestReason: string;
        public get ManifestReason() { return this._ManifestReason; }
        public set ManifestReason(newValue: string) { this._ManifestReason = newValue; this.MarkAsDirty(); }


        private _ManifestStatusCode: string;
        public get ManifestStatusCode() { return this._ManifestStatusCode; }
        public set ManifestStatusCode(newValue: string) { this._ManifestStatusCode = newValue; this.MarkAsDirty(); }

        private _IsKnownCargo: boolean;
        public get IsKnownCargo() { return this._IsKnownCargo; }
        public set IsKnownCargo(newValue: boolean) { this._IsKnownCargo = newValue; this.MarkAsDirty(); }


        private _RegulatedAgentRANumber: string;
        public get RegulatedAgentRANumber() { return this._RegulatedAgentRANumber; }
        public set RegulatedAgentRANumber(newValue: string) { this._RegulatedAgentRANumber = newValue; this.MarkAsDirty(); }


        private _KnownConsignorNumber: string;
        public get KnownConsignorNumber() { return this._KnownConsignorNumber; }
        public set KnownConsignorNumber(newValue: string) { this._KnownConsignorNumber = newValue; this.MarkAsDirty(); }


        private _KCExpirationDate: Date;
        public get KCExpirationDate() { return this._KCExpirationDate; }
        public set KCExpirationDate(newValue: Date) { this._KCExpirationDate = newValue; this.MarkAsDirty(); }


        private _ColoaderRANumber: string;
        public get ColoaderRANumber() { return this._ColoaderRANumber; }
        public set ColoaderRANumber(newValue: string) { this._ColoaderRANumber = newValue; this.MarkAsDirty(); }

        private _AWBPrintingSecurityStatusId: string;
        public get AWBPrintingSecurityStatusId() { return this._AWBPrintingSecurityStatusId; }
        public set AWBPrintingSecurityStatusId(newValue: string) { this._AWBPrintingSecurityStatusId = newValue; this.MarkAsDirty(); }

        private _AWBPrintingRANumber: string;
        public get AWBPrintingRANumber() { return this._AWBPrintingRANumber; }
        public set AWBPrintingRANumber(newValue: string) { this._AWBPrintingRANumber = newValue; this.MarkAsDirty(); }


        private _AdditionalHandlingInfo: string;
        public get AdditionalHandlingInfo() { return this._AdditionalHandlingInfo; }
        public set AdditionalHandlingInfo(newValue: string) { this._AdditionalHandlingInfo = newValue; this.MarkAsDirty(); }

        private _AWBPrintingSecurityStatusEdited: boolean;
        public get AWBPrintingSecurityStatusEdited() { return this._AWBPrintingSecurityStatusEdited; }
        public set AWBPrintingSecurityStatusEdited(newValue: boolean) { this._AWBPrintingSecurityStatusEdited = newValue; this.MarkAsDirty(); }

        private _AWBPrintingRANumberEdited: boolean;
        public get AWBPrintingRANumberEdited() { return this._AWBPrintingRANumberEdited; }
        public set AWBPrintingRANumberEdited(newValue: boolean) { this._AWBPrintingRANumberEdited = newValue; this.MarkAsDirty(); }

        private _AdditionalHandlingInfoEdited: boolean;
        public get AdditionalHandlingInfoEdited() { return this._AdditionalHandlingInfoEdited; }
        public set AdditionalHandlingInfoEdited(newValue: boolean) { this._AdditionalHandlingInfoEdited = newValue; this.MarkAsDirty(); }

        private _ViaColoader: boolean;
        public get ViaColoader() { return this._ViaColoader; }
        public set ViaColoader(newValue: boolean) { this._ViaColoader = newValue; this.MarkAsDirty(); }



        private _IssuingCarrierReference1: string;
        public get IssuingCarrierReference1() { return this._IssuingCarrierReference1; }
        public set IssuingCarrierReference1(newValue: string) { this._IssuingCarrierReference1 = newValue; this.MarkAsDirty(); }


        private _IsMissingDocument: boolean;
        public get IsMissingDocument() { return this._IsMissingDocument; }
        public set IsMissingDocument(newValue: boolean) { this._IsMissingDocument = newValue; this.MarkAsDirty(); }

        private _DocumentsSearchFields: string;
        public get DocumentsSearchFields() { return this._DocumentsSearchFields; }
        public set DocumentsSearchFields(newValue: string) { this._DocumentsSearchFields = newValue; this.MarkAsDirty(); }


        private _InterlineId: string;
        public get InterlineId() { return this._InterlineId; }
        public set InterlineId(newValue: string) { this._InterlineId = newValue; this.MarkAsDirty(); }

        private _ShipperAddress1: string;
        public get ShipperAddress1() { return this._ShipperAddress1; }
        public set ShipperAddress1(newValue: string) { this._ShipperAddress1 = newValue; this.MarkAsDirty(); }

        private _ShipperAddress2: string;
        public get ShipperAddress2() { return this._ShipperAddress2; }
        public set ShipperAddress2(newValue: string) { this._ShipperAddress2 = newValue; this.MarkAsDirty(); }

        private _ShipperZipCode: string;
        public get ShipperZipCode() { return this._ShipperZipCode; }
        public set ShipperZipCode(newValue: string) { this._ShipperZipCode = newValue; this.MarkAsDirty(); }

        private _ShipperStateId: string;
        public get ShipperStateId() { return this._ShipperStateId; }
        public set ShipperStateId(newValue: string) { this._ShipperStateId = newValue; this.MarkAsDirty(); }

        private _ShipperCountryId: string;
        public get ShipperCountryId() { return this._ShipperCountryId; }
        public set ShipperCountryId(newValue: string) { this._ShipperCountryId = newValue; this.MarkAsDirty(); }

        private _ShipperCity: string;
        public get ShipperCity() { return this._ShipperCity; }
        public set ShipperCity(newValue: string) { this._ShipperCity = newValue; this.MarkAsDirty(); }

        private _ConsigneeAddress1: string;
        public get ConsigneeAddress1() { return this._ConsigneeAddress1; }
        public set ConsigneeAddress1(newValue: string) { this._ConsigneeAddress1 = newValue; this.MarkAsDirty(); }

        private _ConsigneeAddress2: string;
        public get ConsigneeAddress2() { return this._ConsigneeAddress2; }
        public set ConsigneeAddress2(newValue: string) { this._ConsigneeAddress2 = newValue; this.MarkAsDirty(); }

        private _ConsigneeZipCode: string;
        public get ConsigneeZipCode() { return this._ConsigneeZipCode; }
        public set ConsigneeZipCode(newValue: string) { this._ConsigneeZipCode = newValue; this.MarkAsDirty(); }

        private _ConsigneeStateId: string;
        public get ConsigneeStateId() { return this._ConsigneeStateId; }
        public set ConsigneeStateId(newValue: string) { this._ConsigneeStateId = newValue; this.MarkAsDirty(); }

        private _ConsigneeCountryId: string;
        public get ConsigneeCountryId() { return this._ConsigneeCountryId; }
        public set ConsigneeCountryId(newValue: string) { this._ConsigneeCountryId = newValue; this.MarkAsDirty(); }

        private _ConsigneeCity: string;
        public get ConsigneeCity() { return this._ConsigneeCity; }
        public set ConsigneeCity(newValue: string) { this._ConsigneeCity = newValue; this.MarkAsDirty(); }


        private _Notify1Address1: string;
        public get Notify1Address1() { return this._Notify1Address1; }
        public set Notify1Address1(newValue: string) { this._Notify1Address1 = newValue; this.MarkAsDirty(); }

        private _Notify1Address2: string;
        public get Notify1Address2() { return this._Notify1Address2; }
        public set Notify1Address2(newValue: string) { this._Notify1Address2 = newValue; this.MarkAsDirty(); }

        private _Notify1ZipCode: string;
        public get Notify1ZipCode() { return this._Notify1ZipCode; }
        public set Notify1ZipCode(newValue: string) { this._Notify1ZipCode = newValue; this.MarkAsDirty(); }

        private _Notify1StateId: string;
        public get Notify1StateId() { return this._Notify1StateId; }
        public set Notify1StateId(newValue: string) { this._Notify1StateId = newValue; this.MarkAsDirty(); }

        private _Notify1CountryId: string;
        public get Notify1CountryId() { return this._Notify1CountryId; }
        public set Notify1CountryId(newValue: string) { this._Notify1CountryId = newValue; this.MarkAsDirty(); }

        private _Notify1City: string;
        public get Notify1City() { return this._Notify1City; }
        public set Notify1City(newValue: string) { this._Notify1City = newValue; this.MarkAsDirty(); }


        private _MAWBReturnedToStackWithCancel: boolean;
        public get MAWBReturnedToStackWithCancel() { return this._MAWBReturnedToStackWithCancel; }
        public set MAWBReturnedToStackWithCancel(newValue: boolean) { this._MAWBReturnedToStackWithCancel = newValue; this.MarkAsDirty(); }

        private _MAWBStackAirlineId: string;
        public get MAWBStackAirlineId() { return this._MAWBStackAirlineId; }
        public set MAWBStackAirlineId(newValue: string) { this._MAWBStackAirlineId = newValue; this.MarkAsDirty(); }

        private _DontAddToImportersQueue: boolean;
        public get DontAddToImportersQueue() { return this._DontAddToImportersQueue; }
        public set DontAddToImportersQueue(newValue: boolean) { this._DontAddToImportersQueue = newValue; this.MarkAsDirty(); }

        private _ForwarderPartnerId: string;
        public get ForwarderPartnerId() { return this._ForwarderPartnerId; }
        public set ForwarderPartnerId(newValue: string) { this._ForwarderPartnerId = newValue; this.MarkAsDirty(); }

        private _OperationalCloseDate: Date;
        public get OperationalCloseDate() { return this._OperationalCloseDate; }
        public set OperationalCloseDate(newValue: Date) { this._OperationalCloseDate = newValue; this.MarkAsDirty(); }

        private _AccountingCloseDate: Date;
        public get AccountingCloseDate() { return this._AccountingCloseDate; }
        public set AccountingCloseDate(newValue: Date) { this._AccountingCloseDate = newValue; this.MarkAsDirty(); }

        private _FromCountryCode: string;
        public get FromCountryCode() { return this._FromCountryCode; }
        public set FromCountryCode(newValue: string) { this._FromCountryCode = newValue; this.MarkAsDirty(); }

        private _ToCountryCode: string;
        public get ToCountryCode() { return this._ToCountryCode; }
        public set ToCountryCode(newValue: string) { this._ToCountryCode = newValue; this.MarkAsDirty(); }


        //islam: for testing the importers data mapping
        private _ConvertToCustomFile: boolean;
        public get ConvertToCustomFile() { return this._ConvertToCustomFile; }
        public set ConvertToCustomFile(newValue: boolean) { this._ConvertToCustomFile = newValue; this.MarkAsDirty(); }
    
        MarkAsDirty() {
            this.IsDirty = true;
        }
}