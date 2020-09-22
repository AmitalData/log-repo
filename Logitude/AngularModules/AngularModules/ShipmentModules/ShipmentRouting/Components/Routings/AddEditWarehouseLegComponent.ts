import {Component} from '@angular/core';
import {AppTool, DateTool, ArrayTool} from '../../../../Infrastructure/Tools';
import {ShipmentTool, RoutingHelper} from '../../../../Shipment/Tools';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentFollowUpPM} from '../../../../Shipment/EntityPMs/ShipmentFollowUpPM';
import {RoutingsTabComponent} from './RoutingsTabComponent';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {AddressList} from '../../../../Common/EntityLists/AddressList';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {AddressListService} from '../../../../Common/Services/StandardLists/AddressListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {WarehouseHelper} from '../../../../Warehouse/Helpers/WarehouseHelper';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {ShipmentPickUpPM} from '../../../../Shipment/EntityPMs/ShipmentPickUpPM';
import { CardPMService } from '../../../../Common/Services/StandardPMs/CardPMService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { WarehouseStoragePricingPM } from '../../../../Common/EntityPMs/WarehouseStoragePricingPM';
import { PartnersDomainService } from '../../../../Common/Services/PartnersDomainService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { ShipmentReceivablePM } from '../../../../Shipment/EntityPMs/ShipmentReceivablePM';
import { CommonDomainService } from '../../../../Common/Services/CommonDomainService';
import { ChargesTypeList } from '../../../../Common/EntityLists/ChargesTypeList';
import { CurrencyListService } from '../../../../Common/Services/StandardLists/CurrencyListService';
import { CurrencyList } from '../../../../Common/EntityLists/CurrencyList';
import { CurrencyRatesService, LastRate } from '../../../../Common/Services/CurrencyRatesService';
import { WarehouseEntryListExtendedService } from '../../../../Warehouse/Services/ExtendedLists/WarehouseEntryListExtendedService';
import { WarehouseExtendedListService } from '../../../../Common/Services/ExtendedLists/WarehouseExtendedListService';
import { ShipmentStoragePricingPM } from '../../../../Shipment/EntityPMs/ShipmentStoragePricingPM';
import { CardPM } from '../../../../Common/EntityPMs/CardPM';

@Component({
    moduleId: './ShipmentModules/ShipmentRouting/Components/Routings/',
    templateUrl: 'AddEditWarehouseLegComponent.html',
})

export class AddEditWarehouseLegComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    public TenantPM: TenantPM;
    public ObjectTableName: string;
    public DataContext = this;
    public LegType: string;
    public IsNewLeg: boolean = false;
    public IsFCLEntity: boolean = false;
    public TransportModeId: string = null;
    public ValidationErrorsList: string[] = [];
    public FatherComponent: RoutingsTabComponent;
    public IsImportShipment: boolean = false;
    IsShowNewWarehouseEntryButton: Boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public StoragePricingEnabled: boolean = false;
    public StoragePricingMessageVisible: boolean = false;
    public DisableNewWarehouseEntryButton: boolean = false;
    private warehouseType: string;
    public IsStoragePricingAreaVisible: boolean = false;
    constructor() {
        super();
        this.TenantPM = SessionLocator.TenantPM;
        this.InitServices();
        this.CurrentSession.SessionEvent.subscribe($event => {
            if ($event == "Refresh") {
                this.EntityPM.IsDirty = true;
                this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
        });
    }

    private myAddressListService: AddressListService;
    private cardService: CardPMService;
    private cardListService: CardListService;
    private warehouseEntryListExtendedService: WarehouseEntryListExtendedService;
    private warehouseExtendedListService: WarehouseExtendedListService;
    InitServices() {
        this.myAddressListService = new AddressListService();
        this.cardService = new CardPMService();
        this.cardListService = new CardListService();
        this.warehouseEntryListExtendedService = new WarehouseEntryListExtendedService();
        this.warehouseExtendedListService = new WarehouseExtendedListService();
    }

    GetShipmentDirection() {
        if (this.EntityPM.DirectionId == "I") {
            this.IsImportShipment = true;
        }
    }

    private myConsignee: CardPM;
    InitFreeDaysStorage() {
        this.cardService.get(this.EntityPM.ConsigneeId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.myConsignee = myResponse.Result;
                    if (this.myConsignee) {
                        if (this.IsNewLeg && this.myConsignee.IsCustomer) {
                            this.WarehouseStorageFreeDays = this.myConsignee.StorageFreeDays;                            
                        }
                    }
                }
            }
        });
    }
     
    SetWindowArgs(args: any) {
        this.EntityPM = args['EntityPM'];
        this.LegType = args['LegType'];
        this.IsNewLeg = args['IsNewLeg'];

        if (this.EntityPM) {
            this.TransportModeId = this.EntityPM.TransportModeId;
            this.IsFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);

            if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H") {
                if (FeatureLocator.HasFeaturePermession("WarehouseEntry", "Module")) {
                    this.IsShowNewWarehouseEntryButton = true;
                }
            }

            var FeatureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "STR" && d.TenantNumber == SessionLocator.Tenant)[0];
            if (FeatureToggle) {
                this.IsStoragePricingAreaVisible = true;
            }

            this.GetShipmentDirection();
            this.SetStorageDays();
            this.ComputeStorageFee();

            this.ObjectTableName = args['ObjectTableName'];
            this.FatherComponent = args['FatherComponent'];
            this.WarehouseAddressList = this.FatherComponent.WarehouseAddressList;
            this.SetUIProperties();
            this.InitFreeDaysStorage();
            this.Clone();

            if (this.IsNewLeg) {
                if (this.LegType == "WarehouseLeg_Pickups") {
                    if (this.EntityPM.ShipmentPickUps.length > 0) {
                        var FirstPickup: ShipmentPickUpPM = this.EntityPM.ShipmentPickUps.sort(function (a, b) { return a.PickUpDeliveryNumber.toLowerCase() == b.PickUpDeliveryNumber.toLowerCase() ? 0 : a.PickUpDeliveryNumber.toLowerCase() < b.PickUpDeliveryNumber.toLowerCase() ? -1 : 1; })[0];
                        if (FirstPickup) {
                            this.WarehouseLegExpectedEntryDate = FirstPickup.ETA
                            this.WarehouseLegActualEntryDate = FirstPickup.ATA;
                        }
                    }
                }

                this.GetShipmentDirection();
            }

            if (this.IsImportShipment) {
                if (!AppTool.IsNullOrEmpty(this.WarehouseLegWarehouseId)) {
                    this.cardListService.getSingle(this.WarehouseLegWarehouseId).subscribe((myResponse: ServiceResponse) => {
                        if (myResponse != null) {
                            if (!myResponse.HasError) {
                                var result: CardList = myResponse.Result;
                                if (result) {
                                    this.warehouseType = result.WarehouseTypeCode;                                    
                                    this.SetNewWarehouseEntryButtonProperty();
                                    this.SetChargeStorageProperies();
                                    this.SetUIProperties_Storage();
                                }
                            }
                        }
                    });
                }
            }
        }
    }

    SetIsBondedWarehouseProperities() {
        if (!this.EntityPM.IsBondedWarehouseChanged)
            this.warehouseExtendedListService.GetWarehouseTypeById(this.WarehouseLegWarehouseId).subscribe((serviceResponse: ServiceResponse) => {
                var warehouseType = serviceResponse.Result;
                if (this.warehouseType == "BO") { 
                this.IsBondedWarehouse = true;
                this.SetNewWarehouseEntryButtonProperty();
            }

            else {
                this.IsBondedWarehouse = false;
            }

            this.EntityPM.IsBondedWarehouseChanged = true;
        }

        else if (this.IsBondedWarehouse) {
            this.SetNewWarehouseEntryButtonProperty();
        }
    }
    SetNewWarehouseEntryButtonProperty() {
        if (this.IsBondedWarehouse) {
            this.warehouseEntryListExtendedService.GetActiveWarehouseEntriesByShipmentId(this.EntityPM.Id).subscribe((serviceResponse: ServiceResponse) => {
                var warehouseEntries = serviceResponse.Result;
                if (warehouseEntries && warehouseEntries.length > 0) {
                    this.DisableNewWarehouseEntryButton = true;
                }
            });
        }
    }
    SetChargeStorageProperies() {
        if (this.warehouseType == "BO" && this.IsBondedWarehouse) {
            this.StoragePricingEnabled = true;
            this.StoragePricingMessageVisible = false;
        }
        else {
            this.StoragePricingEnabled = false;
            this.StoragePricingMessageVisible = true;
        }
    }

    public IsEditingEnabled: boolean = true;
    public IsFirmCodeVisible: boolean = true;
    SetUIProperties() {
        var isEditingEnabled = ShipmentTool.IsEditingEnabled(this.EntityPM);   
        this.IsEditingEnabled = isEditingEnabled;
        this.IsFirmCodeVisible = (this.TenantPM.CountryCode.toUpperCase()) == "US" ? true : false;

        this.UIProperties.SetEnabled("IsBonded", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ChargeStorage", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegWarehouseId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegAddressId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegTerminalCode", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegRemarks", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegLastFreeDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegExpectedEntryDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegExpectedReleaseDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegActualEntryDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegActualReleaseDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegReference", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("TerminalAvailable", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetRequired("WarehouseLegWarehouseId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.WarehouseLegWarehouseId) ? true : false);

        this.SetUIProperties_ValidateActualDates();
        this.SetUIProperties_Storage();
    }
    SetUIProperties_ValidateActualDates() {

        this.UIProperties.SetValidity("WarehouseLegActualEntryDate", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("WarehouseLegActualReleaseDate", this.ObjectTableName, true, null);

        if (this.LegType == "WarehouseLeg_Pickups") {
            var date: Date = DateTool.GetCurrentDateAsUtc();

            if (this.IsDateBigger(this.WarehouseLegActualEntryDate, date)) {
                var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegActualEntryDate"));
                this.UIProperties.SetValidity("WarehouseLegActualEntryDate", this.ObjectTableName, false, errorMessage);
            }

            if (this.IsDateBigger(this.WarehouseLegActualReleaseDate, date)) {
                var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegActualReleaseDate"));
                this.UIProperties.SetValidity("WarehouseLegActualReleaseDate", this.ObjectTableName, false, errorMessage);
            }
        }

        else {
            if (!DateTool.IsActualDateValid(this.WarehouseLegActualEntryDate)) {
                var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegActualEntryDate"));
                this.UIProperties.SetValidity("WarehouseLegActualEntryDate", this.ObjectTableName, false, errorMessage);
            }

            if (!DateTool.IsActualDateValid(this.WarehouseLegActualReleaseDate)) {
                var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegActualReleaseDate"));
                this.UIProperties.SetValidity("WarehouseLegActualReleaseDate", this.ObjectTableName, false, errorMessage);
            }
        }
    }
    SetUIProperties_Storage() {
        this.UIProperties.SetEnabled("ChargeStorage", this.ObjectTableName, this.StoragePricingEnabled);
    }

    private IsDateBigger(date1: any, date2: any) {
        var myResult: boolean = false;

        if (!AppTool.IsNullOrEmpty(date1) && !AppTool.IsNullOrEmpty(date2)) {
            var Date1Parts = DateTool.GetDateParts(date1);
            var Date2Parts = DateTool.GetDateParts(date2);

            var Date1Ticks = (Date1Parts.Day * 1) + (Date1Parts.Month * 30) + (Date1Parts.Year * 365);
            var Date2Ticks = (Date2Parts.Day * 1) + (Date2Parts.Month * 30) + (Date2Parts.Year * 365);

            if (Date1Ticks > Date2Ticks) {
                myResult = true;
            }
        }

        return myResult;
    }

    get WarehouseLegWarehouseId() { return this.EntityPM.WarehouseLegWarehouseId; }
    set WarehouseLegWarehouseId(newValue: string) {
        if (this.EntityPM.WarehouseLegWarehouseId != newValue) {
            this.EntityPM.WarehouseLegWarehouseId = newValue;

            if (!AppTool.IsNullOrEmpty(newValue)) {
                this.GetAddress();
            }

            else {
                this.WarehouseLegAddressId = null;
                this.FatherComponent.WarehouseLegTerminalName = "";
            }

            this.SetUIProperties();
        }         
    }

    GetAddress() {        
        this.cardListService.getSingle(this.WarehouseLegWarehouseId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var result: CardList = myResponse.Result;
                    if (result) {
                        this.WarehouseLegAddressId = result.MainAddressId;
                        this.FatherComponent.WarehouseLegTerminalName = result.EnglishName;
                        this.FatherComponent.EntityPM.WarehouseLegTerminalName = result.EnglishName;
                        this.WarehouseLegTerminalCode = result.FirmCode;

                        if (this.myConsignee != null && !AppTool.IsNullOrZero(this.myConsignee.StorageFreeDays)) {
                            this.WarehouseStorageFreeDays = this.myConsignee.StorageFreeDays;
                        }

                        else {
                            this.WarehouseStorageFreeDays = result.StorageFreeDays;
                        }

                        this.warehouseType = result.WarehouseTypeCode;
                        this.EntityPM.IsBondedWarehouseChanged = false;
                        this.SetIsBondedWarehouseProperities();
                        this.SetChargeStorageProperies();
                        this.SetUIProperties_Storage();
                        this.SetStorageDefaults(result);

                        if (result.WarehouseTypeCode == "BO" && this.IsBondedWarehouse) {
                            this.LoadWarehouseStoragePricing();
                        }
                    }
                }
            }
        });
    }

    private warehouseStoragePricings: WarehouseStoragePricingPM[];
    private LoadWarehouseStoragePricing() {
        this.CurrentSession.StartBusyIndicatorLoading();

        var myService: PartnersDomainService = new PartnersDomainService();
        myService.GetWarehouseStoragePricingForWarehouse(this.EntityPM.WarehouseLegWarehouseId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.warehouseStoragePricings = myResponse.Result;
                    this.FillDefaultPricings();
                }

                this.CurrentSession.StopBusyIndicator();
            }
        });
    }
    private SetStorageDefaults(myWarehouse: CardList) {
        if (myWarehouse.WarehouseTypeCode == "BO") {
            this.ChargeStorage = myWarehouse.ChargeStorage;
            this.EntityPM.ChargeStorageCurrencyId = myWarehouse.ChargeStorageCurrencyId;

            switch (this.EntityPM.TransportModeId) {
                case "A":
                    {
                        this.EntityPM.WeightMeasurementCode = myWarehouse.AirWeightMeasurementCode;
                        this.EntityPM.WeightRoundingCode = myWarehouse.AirWeightRoundingCode;
                        break;
                    }

                case "O":
                    {
                        this.EntityPM.WeightMeasurementCode = myWarehouse.OceanWeightMeasurementCode;
                        this.EntityPM.WeightRoundingCode = myWarehouse.OceanWeightRoundingCode;
                        break;
                    }

                case "I":
                    {
                        this.EntityPM.WeightMeasurementCode = myWarehouse.InlandWeightMeasurementCode;
                        this.EntityPM.WeightRoundingCode = myWarehouse.InlandWeightRoundingCode;
                        break;
                    }
            }
        }

        else {
            this.ChargeStorage = false;
            this.EntityPM.ChargeStorageCurrencyId = null;
            this.EntityPM.WeightMeasurementCode = null;
            this.EntityPM.WeightRoundingCode = null;
            this.EntityPM.ShipmentStoragePricings = [];
        }
    }
    private FillDefaultPricings() {        
        if (this.warehouseStoragePricings != null && this.warehouseStoragePricings.length > 0) {
            var count: number = 1;
            this.warehouseStoragePricings.sort((a, b) => { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1 }).forEach(item => {
                var defaultItem: ShipmentStoragePricingPM = new ShipmentStoragePricingPM(this.EntityPM);
                defaultItem.Tenant = SessionLocator.Tenant;
                defaultItem.ShipmentId = this.EntityPM.Id;
                defaultItem.WarehouseId = this.EntityPM.WarehouseLegWarehouseId;
                defaultItem.StepFrom = item.StepFrom;
                defaultItem.StepTo = item.StepTo;
                defaultItem.Days = item.Days;
                defaultItem.SalePrice = item.SalePrice;
                defaultItem.LineNumber = count++;

                this.EntityPM.AddShipmentStoragePricing(defaultItem);
            });
        }
    }

    private myWarehouseAddressList: AddressList;
    get WarehouseAddressList() { return this.myWarehouseAddressList; }
    set WarehouseAddressList(newValue: AddressList) {
        this.myWarehouseAddressList = newValue;
        this.FatherComponent.WarehouseAddressList = newValue;
    } 

    get WarehouseLegAddressId() { return this.EntityPM.WarehouseLegAddressId; }
    set WarehouseLegAddressId(newValue: string) {
        if (this.EntityPM.WarehouseLegAddressId != newValue) {
            this.EntityPM.WarehouseLegAddressId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.WarehouseAddressList = null;
            }

            else {
                this.myAddressListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.WarehouseAddressList = myResponse.Result;
                    }
                });
            }
        }
    }

    get WarehouseLegTerminalCode() { return this.EntityPM.WarehouseLegTerminalCode; }
    set WarehouseLegTerminalCode(newValue: string) {
        if (this.EntityPM.WarehouseLegTerminalCode != newValue) {
            this.EntityPM.WarehouseLegTerminalCode = newValue;
        }
    }

    get WarehouseLegReference() { return this.EntityPM.WarehouseLegReference; }
    set WarehouseLegReference(newValue: string) {
        if (this.EntityPM.WarehouseLegReference != newValue) {
            this.EntityPM.WarehouseLegReference = newValue;
        }
    }

    get WarehouseLegRemarks() { return this.EntityPM.WarehouseLegRemarks; }
    set WarehouseLegRemarks(newValue: string) {
        if (this.EntityPM.WarehouseLegRemarks != newValue) {
            this.EntityPM.WarehouseLegRemarks = newValue;
        }
    }

    get WarehouseLegLastFreeDate() { return this.EntityPM.WarehouseLegLastFreeDate; }
    set WarehouseLegLastFreeDate(newValue: Date) {
        if (this.EntityPM.WarehouseLegLastFreeDate != newValue) {
            this.EntityPM.WarehouseLegLastFreeDate = newValue;
            if (newValue == null) {
                this.WarehouseLegLastFreeDate = null;
            } else {
                this.SetStorageFreeDays();
            }
        }
    }

    get WarehouseLegExpectedEntryDate() { return this.EntityPM.WarehouseLegExpectedEntryDate; }
    set WarehouseLegExpectedEntryDate(value: Date) {
        if (this.EntityPM.WarehouseLegExpectedEntryDate != value) {
            this.EntityPM.WarehouseLegExpectedEntryDate = value;
        }
    }

    get WarehouseLegExpectedReleaseDate() { return this.EntityPM.WarehouseLegExpectedReleaseDate; }
    set WarehouseLegExpectedReleaseDate(value: Date) {
        if (this.EntityPM.WarehouseLegExpectedReleaseDate != value) {
            this.EntityPM.WarehouseLegExpectedReleaseDate = value;
        }
    }

    get WarehouseLegActualEntryDate() { return this.EntityPM.WarehouseLegActualEntryDate; }
    set WarehouseLegActualEntryDate(value: Date) {
        if (this.EntityPM.WarehouseLegActualEntryDate != value) {
            this.EntityPM.WarehouseLegActualEntryDate = value;
            this.SetUIProperties_ValidateActualDates();
            if (value == null) {
                this.WarehouseLegActualEntryDate == null;
                this.StorageDays = null;
                this.Days = null;
            } else {
                this.SetLastFreeDate();
                this.SetStorageDays();
            }

            this.ComputeGrossWeight_PerStorageDays();
        }
    }

    get WarehouseLegActualReleaseDate() { return this.EntityPM.WarehouseLegActualReleaseDate; }
    set WarehouseLegActualReleaseDate(value: Date) {
        if (this.EntityPM.WarehouseLegActualReleaseDate != value) {
            this.EntityPM.WarehouseLegActualReleaseDate = value;
            this.SetUIProperties_ValidateActualDates();
            if (value == null) {
                this.WarehouseLegActualReleaseDate = null;
                this.StorageDays = null;
                this.Days = null;
            }
            else {
                this.SetStorageDays();
            }

            this.PricesChanged = true;
            this.ComputeGrossWeight_PerStorageDays();
        }
    }

    get GrossWeightPerStorageDays() { return this.EntityPM.GrossWeightPerStorageDays == null ? 0 : this.EntityPM.GrossWeightPerStorageDays; }
    set GrossWeightPerStorageDays(newValue: number) {
        if (this.EntityPM.GrossWeightPerStorageDays != newValue) {
            this.EntityPM.GrossWeightPerStorageDays = AppTool.Round(newValue, 3);
        }
    }

    get TerminalAvailable() { return this.EntityPM.TerminalAvailable; }
    set TerminalAvailable(value: Date) {
        if (this.EntityPM.TerminalAvailable != value) {
            this.EntityPM.TerminalAvailable = value;
            this.FatherComponent.EntityPM.TerminalAvailable = value;
        }
    }

    get WarehouseLegCutOffDate() { return this.EntityPM.WarehouseLegCutOffDate; }
    set WarehouseLegCutOffDate(value: Date) {
        if (this.EntityPM.WarehouseLegCutOffDate != value) {
            this.EntityPM.WarehouseLegCutOffDate = value;            
        }
    }

    get WarehouseLegVGMCutOffDate() { return this.EntityPM.WarehouseLegVGMCutOffDate; }
    set WarehouseLegVGMCutOffDate(value: Date) {
        if (this.EntityPM.WarehouseLegVGMCutOffDate != value) {
            this.EntityPM.WarehouseLegVGMCutOffDate = value;            
        }
    }

    get WarehouseStorageFreeDays() { return this.EntityPM.WarehouseStorageFreeDays; }
    set WarehouseStorageFreeDays(value: number) {
        if (this.EntityPM.WarehouseStorageFreeDays != value) {
            this.EntityPM.WarehouseStorageFreeDays = value;
            if (value == null) {
                this.WarehouseStorageFreeDays = null;
            }
            else {
                this.SetLastFreeDate();
            }

            this.PricesChanged = true;
        }
    }

    get IsBondedWarehouse() { return this.EntityPM.IsBondedWarehouse; }
    set IsBondedWarehouse(value: boolean) {
        if (this.EntityPM.IsBondedWarehouse != value) {
            this.EntityPM.IsBondedWarehouse = value;
            if (this.IsImportShipment) {
                this.SetIsBondedWarehouseProperities();
                this.SetChargeStorageProperies();
                this.SetUIProperties_Storage();
                this.PricesChanged = true;
            }
        }
    }

    get IsBondedWarehouseChanged() { return this.EntityPM.IsBondedWarehouseChanged; }
    set IsBondedWarehouseChanged(value: boolean) {
        if (this.EntityPM.IsBondedWarehouseChanged != value) {
            this.EntityPM.IsBondedWarehouseChanged = value;
        }
    }

    get ChargeStorage() { return this.EntityPM.ChargeStorage; }
    set ChargeStorage(value: boolean) {
        if (this.EntityPM.ChargeStorage != value) {
            this.EntityPM.ChargeStorage = value;            
        }
    }

    private PricesChanged: boolean = false;
    StoragePricingClicked() {
        var entityResourceService: EntityResourceService = new EntityResourceService();
        entityResourceService.getEntityResourceByTableName("ShipmentStoragePricing").subscribe((res1: any) => {
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Title = "Storage Pricing";
            logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, ObjectTableName: this.ObjectTableName };
            logitudeWindow.Show("./ShipmentModules/ShipmentRouting/Components/Routings/WarehouseStoragePricingComponent");
            logitudeWindow.WindowClosed.subscribe(s => {
                if (s == "PricesChanged") {
                    this.PricesChanged = true;
                    this.CheckStorageProperties();                    
                }
            });
        });
    }
    
    private SetLastFreeDate() {
        if (this.WarehouseLegActualEntryDate != null && this.WarehouseStorageFreeDays != null) {
            var date = DateTool.AddDays(this.WarehouseLegActualEntryDate, this.WarehouseStorageFreeDays);
            if (date == null) {
                this.WarehouseStorageFreeDays = 0;
            }
            else {
                this.WarehouseLegLastFreeDate = date;
            }
        }
    } 

    private SetStorageFreeDays() {
        if (this.WarehouseLegActualEntryDate != null && this.WarehouseLegLastFreeDate != null) {
            if (DateTool.GetDateFromDate(this.WarehouseLegLastFreeDate) >= DateTool.GetDateFromDate(this.WarehouseLegActualEntryDate)) {
                var days = DateTool.GetDaysBetweenDates(this.WarehouseLegActualEntryDate, this.WarehouseLegLastFreeDate);
                if (days == null) {
                    this.WarehouseStorageFreeDays = 0;
                } else if (this.WarehouseStorageFreeDays != days) {
                    this.WarehouseStorageFreeDays = days;
                }
            }
            else {
                this.WarehouseStorageFreeDays = 0;
            }

            this.ComputeGrossWeight_PerStorageDays();
        }
    }

    public StorageDays: number;
    public StorageFee: number;
    public Days: string;
    private SetStorageDays() {
        if (this.WarehouseLegActualEntryDate != null && this.WarehouseLegActualReleaseDate != null) {
            if (DateTool.GetDateFromDate(this.WarehouseLegActualReleaseDate) >= DateTool.GetDateFromDate( this.WarehouseLegActualEntryDate)) {
                var days = DateTool.GetDaysBetweenDates(this.WarehouseLegActualEntryDate, this.WarehouseLegActualReleaseDate);
                this.StorageDays = days;
                this.Days = " Days";
            }

            else {
                this.StorageDays = null;
                this.Days = null;
            }
        }
    }
    private ComputeStorageFee() {
        var storageReceivables: ShipmentReceivablePM[] = this.EntityPM.ShipmentReceivables.filter(d => d.ChargesTypeCode == "ISTOR" && d.MeasurementCode == "STFE");
        if (storageReceivables.length > 0) {
            this.StorageFee = ArrayTool.Sum(storageReceivables, "TotalAmount");
        }

        else {
            this.StorageFee = null;
        }
    }
    
    NewWarehouseEntryButtonClicked() {
        var windowArgs: any = {};
        windowArgs.ExpectedEntryDate = this.WarehouseLegExpectedEntryDate;
        windowArgs.ActualEntryDate = this.WarehouseLegActualEntryDate;
        windowArgs.WarehouseId = this.WarehouseLegWarehouseId;
        windowArgs.EntityPM = this.EntityPM;
        windowArgs.IsNotSetWarehouseIdForWarehouseLegShipment = true;
        windowArgs.ConnectedTo = "Warehouse/Terminal";
        var warehouseHelper: WarehouseHelper = new WarehouseHelper();
        warehouseHelper.ShowNewWarehouseEntryComponent(windowArgs);
    }
    SetActualDateClicked(fieldName: string) {
        switch (fieldName) {
            case "WarehouseLegExpectedReleaseDate": {
                this.WarehouseLegActualReleaseDate = DateTool.GetDateParts(this.WarehouseLegExpectedReleaseDate).DateObject; break;
            }
            case "WarehouseLegExpectedEntryDate": {
                this.WarehouseLegActualEntryDate = DateTool.GetDateParts(this.WarehouseLegExpectedEntryDate).DateObject; break;
            }
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors: string[] = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (AppTool.IsNullOrEmpty(this.WarehouseLegWarehouseId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegWarehouseId")));
        }

        // Series Dates
        RoutingHelper.ValidateRoutingsSeriesDates(this.EntityPM, errors, this.LegType);

        // Actual Dates
        if (this.LegType == "WarehouseLeg_Pickups") {
            var date: Date = DateTool.GetCurrentDateAsUtc();

            if (this.IsDateBigger(this.WarehouseLegActualEntryDate, date)) {
                errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegActualEntryDate")));
            }

            if (this.IsDateBigger(this.WarehouseLegActualReleaseDate, date)) {
                errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegActualReleaseDate")));
            }
        }

        else {
            if (!DateTool.IsActualDateValid(this.WarehouseLegActualEntryDate)) {
                errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegActualEntryDate")));
            }

            if (!DateTool.IsActualDateValid(this.WarehouseLegActualReleaseDate)) {
                errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegActualReleaseDate")));
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.CheckStorageProperties(); 
            this.FatherComponent.BuildItemsCollection();
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    }

    private CheckStorageProperties() {
        var storageReceivable: ShipmentReceivablePM = this.EntityPM.ShipmentReceivables.filter(d => d.ChargesTypeCode == "ISTOR" && d.MeasurementCode == "STFE" && AppTool.IsNullOrEmpty(d.ARInvoiceId))[0];

        if (this.IsBondedWarehouse && this.WarehouseLegActualReleaseDate != null && this.WarehouseLegActualReleaseDate != undefined && !AppTool.IsNullOrEmpty(this.EntityPM.ChargeStorageCurrencyId)
            && !AppTool.IsNullOrZero(this.StorageDays) && this.ChargeStorage && this.EntityPM.ShipmentStoragePricings.length > 0) {

            if (this.PricesChanged) {
                if (storageReceivable) {
                    this.UpdateStorageReceivable(storageReceivable);
                }

                else {
                    this.CreateReceivable();
                }
            }

            else {
                if (storageReceivable == null) {
                    this.CreateReceivable();
                }
            }
        }

        else {            
            if (storageReceivable) {
                this.EntityPM.RemoveReceivable(storageReceivable);
                this.CurrentSession.FireEvent("StorageReceivableRemoved");
            }
        }

        this.ComputeStorageFee();
    }
    private ComputeReceivableAmount(): number {
        var myResult: number = ShipmentTool.ComputeImportStorageReceivableAmount(this.StorageDays, this.EntityPM);

        return myResult;
    }
    private UpdateStorageReceivable(storageReceivable: ShipmentReceivablePM) {
        var amount: number = this.ComputeReceivableAmount();

        storageReceivable.TotalAmount = amount;
        storageReceivable.TotalAmountLocal = AppTool.Round(storageReceivable.TotalAmount * storageReceivable.Rate, 2);

        if (storageReceivable.CurrencyId == this.EntityPM.ProfitCurrencyId) {
            storageReceivable.AmountInProfitCurrency = storageReceivable.TotalAmount;
        }

        else {
            storageReceivable.AmountInProfitCurrency = (storageReceivable.TotalAmountLocal / storageReceivable.ProfitCurrencyExchangeRate);
        }
    }
    private CreateReceivable() {
        var amount: number = this.ComputeReceivableAmount();

        if (!AppTool.IsNullOrZero(amount)) {
            var myService = new CommonDomainService();
            myService.GetChargesTypeByCode('ISTOR').subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var chargesType: ChargesTypeList = myResponse.Result;

                    if (chargesType) {
                        var todayDate: Date = DateTool.GetCurrentDateAsUtc();
                        var myCurrencyRatesService = new CurrencyRatesService();
                        myCurrencyRatesService.getAll(SessionLocator.LocalCurrencyId, todayDate).subscribe((myResponse2: ServiceResponse) => {
                            if (!myResponse2.HasError) {
                                var allRates: LastRate[] = myResponse2.Result;

                                var myService = new CurrencyListService();
                                myService.getSingleFromCache(this.EntityPM.ChargeStorageCurrencyId).subscribe((myResponse: ServiceResponse) => {
                                    if (!myResponse.HasError) {
                                        var currencyList: CurrencyList = myResponse.Result;
                                        if (currencyList != null) {

                                            var storageReceivable: ShipmentReceivablePM = new ShipmentReceivablePM(this.EntityPM);
                                            storageReceivable.Tenant = this.EntityPM.Tenant;
                                            storageReceivable.ShipmentId = this.EntityPM.Id;
                                            storageReceivable.ChargesTypeId = chargesType.Id;
                                            storageReceivable.ChargesTypeCode = chargesType.Code;
                                            storageReceivable.ChargesTypeName = chargesType.EnglishName;
                                            storageReceivable.MeasurementId = chargesType.MeasurementId;
                                            storageReceivable.MeasurementCode = chargesType.MeasurementCode;
                                            storageReceivable.ChargesGroupCode = chargesType.ChargesGroupCode;
                                            storageReceivable.DueTypeCode = chargesType.DueTypeCode;
                                            storageReceivable.DueTypeName = chargesType.DueTypeName;
                                            storageReceivable.VatTypeId = chargesType.VatTypeId;
                                            storageReceivable.IATACodeId = chargesType.IATACodeId;
                                            storageReceivable.IsExpense = chargesType.IsExpense;
                                            storageReceivable.ShipmentNumber = this.EntityPM.ShipmentNumber;
                                            storageReceivable.CreateDate = DateTool.GetCurrentDateAsUtc();
                                            storageReceivable.UpdateDate = DateTool.GetCurrentDateAsUtc();
                                            storageReceivable.CreatedByUserId = SessionLocator.LoggedUserId;
                                            storageReceivable.UpdateByUserId = SessionLocator.LoggedUserId;
                                            storageReceivable.ShipmentReceivableLineStatusCode = "OAMT";
                                            storageReceivable.CurrencyId = this.EntityPM.ChargeStorageCurrencyId;
                                            storageReceivable.CurrencyCode = currencyList.Code;

                                            if (SessionLocator.LocalCurrencyId == storageReceivable.CurrencyId) {
                                                storageReceivable.Rate = 1;
                                            }
                                            else {
                                                var lastRate: LastRate = allRates.filter(d => d.ForeignCurrencyId == storageReceivable.CurrencyId)[0];
                                                if (lastRate != null) {
                                                    storageReceivable.Rate = lastRate.Rate;
                                                }
                                            }

                                            if (this.EntityPM.ProfitCurrencyId == SessionLocator.TenantPM.CurrencyId) {
                                                storageReceivable.ProfitCurrencyExchangeRate = 1;
                                            }

                                            else {
                                                var myLastRate: LastRate = allRates.filter(d => d.ForeignCurrencyId == this.EntityPM.ProfitCurrencyId)[0];
                                                if (myLastRate != null) {
                                                    storageReceivable.ProfitCurrencyExchangeRate = myLastRate.Rate;
                                                }
                                            }

                                            if (chargesType.ChargesGroupCode == "FRT") {
                                                storageReceivable.PrepaidCollectId = this.EntityPM.FreightPrepaidCollectId;
                                            }

                                            else {
                                                storageReceivable.PrepaidCollectId = this.EntityPM.OtherPrepaidCollectId;
                                            }

                                            storageReceivable.TotalAmount = amount;
                                            storageReceivable.TotalAmountLocal = AppTool.Round(storageReceivable.TotalAmount * storageReceivable.Rate, 2);

                                            if (storageReceivable.CurrencyId == this.EntityPM.ProfitCurrencyId) {
                                                storageReceivable.AmountInProfitCurrency = storageReceivable.TotalAmount;
                                            }

                                            else {
                                                storageReceivable.AmountInProfitCurrency = (storageReceivable.TotalAmountLocal / storageReceivable.ProfitCurrencyExchangeRate);
                                            }

                                            this.EntityPM.AddReceivable(storageReceivable);
                                            this.CurrentSession.FireEvent("StorageReceivableCreated");
                                        }
                                    }
                                });
                            }
                        });
                    }
                }
            });
        }
    }

    private myCloner: Cloner;
    private oldFollowups: ShipmentFollowUpPM[] = [];
    private Clone() {
        this.EntityPM.FollowUps.forEach(item => {
            var oldItem: ShipmentFollowUpPM = new ShipmentFollowUpPM(null);
            oldItem.Id = item.Id;
            oldItem.Date = item.Date;
            oldItem.Deleted = item.Deleted;
            oldItem.Done = item.Done;
            oldItem.DoneDateTime = item.DoneDateTime;
            oldItem.DoneNote = item.DoneNote;
            oldItem.EntityDateId = item.EntityDateId;
            oldItem.EventTypeFollowUpName = item.EventTypeFollowUpName;
            oldItem.EventTypeId = item.EventTypeId;
            oldItem.Tenant = item.Tenant;
            oldItem.ExternalDocumentId = item.ExternalDocumentId;
            oldItem.IsNew = item.IsNew;
            oldItem.JobId = item.JobId;
            oldItem.LegType = item.LegType;
            oldItem.ManualActivatedFollowUp = item.ManualActivatedFollowUp;
            oldItem.Note = item.Note;
            oldItem.OwnerUserId = item.OwnerUserId;
            oldItem.OwnerUserName = item.OwnerUserName;
            oldItem.ShipmentId = item.ShipmentId;
            oldItem.ChangeSetOp = item.ChangeSetOp;
            oldItem.OldEntityPM = item.OldEntityPM;
            oldItem.UIProperties = item.UIProperties;
            oldItem.UniqueKey = item.UniqueKey;
            oldItem.IsDirty = item.IsDirty;
            oldItem.EntityParentPM = item.EntityParentPM;
            this.oldFollowups.push(oldItem);
        });

        this.myCloner = new Cloner(this);
        this.myCloner.AddField('WarehouseLegWarehouseId');
        this.myCloner.AddField('WarehouseLegAddressId');
        this.myCloner.AddField('WarehouseLegTerminalCode');
        this.myCloner.AddField('WarehouseLegExpectedReleaseDate');
        this.myCloner.AddField('WarehouseLegExpectedEntryDate');
        this.myCloner.AddField('WarehouseLegActualEntryDate');
        this.myCloner.AddField('WarehouseLegActualReleaseDate');
        this.myCloner.AddField('WarehouseLegLastFreeDate');
        this.myCloner.AddField('WarehouseLegRemarks');
        this.myCloner.AddField('WarehouseLegTerminalName');
        this.myCloner.AddField('TerminalAvailable');
        this.myCloner.AddField('WarehouseLegCutOffDate');
        this.myCloner.AddField('WarehouseLegVGMCutOffDate');
        this.myCloner.AddField('WarehouseStorageFreeDays');
        this.myCloner.AddField('IsBondedWarehouse');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.WarehouseAddressList);
        this.myCloner.AddEntity(this.FatherComponent.WarehouseAddressList);
    }
    private RejectChanges() {

        var addedItems: any[] = [];
        var removedItems: any[] = [];

        this.oldFollowups.forEach(item => {
            var existingItem = this.EntityPM.FollowUps.filter(f => f.LegType == item.LegType)[0];
            if (!existingItem) {
                removedItems.push(item);
            }
        });

        this.EntityPM.FollowUps.forEach(item => {
            var oldItem = this.oldFollowups.filter(f => f.LegType == item.LegType)[0];
            if (oldItem == null) {
                addedItems.push(item);
            }
        });

        if (addedItems.length > 0 || removedItems.length > 0) {
            addedItems.forEach(item => {
                this.EntityPM.RemoveShipmentFollowUp(item);
            });

            removedItems.forEach(item => {
                this.EntityPM.AddShipmentFollowUp(item);
            });

            this.CurrentSession.FireEvent("FollowupsChanged");
        }

        if (this.IsNewLeg && this.EntityPM.ShipmentStoragePricings.length > 0) {
            this.EntityPM.ShipmentStoragePricings = [];

            //this.EntityPM.ShipmentStoragePricings.forEach(item => {
            //    this.EntityPM.RemoveShipmentStoragePricing(item);
            //});
        }

        this.myCloner.RejectChanges();
    }

    private ComputeGrossWeight_PerStorageDays() {
        var StorageDays = DateTool.GetDaysBetweenDates(this.EntityPM.WarehouseLegActualReleaseDate, this.EntityPM.WarehouseLegActualEntryDate);
        var weightPerStorageDays;
        if (this.EntityPM.TransportModeId != "A") {
            weightPerStorageDays = Math.ceil(this.EntityPM.GrossWeightPerTon) * (StorageDays - this.EntityPM.WarehouseStorageFreeDays);
        }
        else {
            weightPerStorageDays = this.EntityPM.ChargeableWeight * (StorageDays - this.EntityPM.WarehouseStorageFreeDays);
        }
        this.GrossWeightPerStorageDays = weightPerStorageDays < 0 ? 0 : weightPerStorageDays;

        ShipmentTool.OnWarehouseStorageFreeDaysChanged(this.EntityPM);
    }
}
