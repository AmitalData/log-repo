import {Component, OnInit, OnDestroy}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentOrderPackagePM} from '../../../../Shipment/EntityPMs/ShipmentOrderPackagePM';
import {ShipmentPickUpPM} from '../../../../Shipment/EntityPMs/ShipmentPickUpPM';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ShipmentTool, RoutingHelper} from '../../../../Shipment/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {AirlineList} from '../../../../Common/EntityLists/AirlineList';
import {VesselList} from '../../../../Common/EntityLists/VesselList';
import {PackageTypeList} from '../../../../Common/EntityLists/PackageTypeList';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {AirlineListService} from '../../../../Common/Services/StandardLists/AirlineListService';
import {VesselListService} from '../../../../Common/Services/StandardLists/VesselListService';
import {PackageTypeListService} from '../../../../Common/Services/StandardLists/PackageTypeListService';
import {PartnersDomainService} from '../../../../Common/Services/PartnersDomainService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
declare var window: any;

@Component({    
    templateUrl: './OrdersTabComponent.html',
})

export class OrdersTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: ShipmentPM = null;
    public IsLCLEntity: boolean = false;
    public IsFCLEntity: boolean = false;
    public ObjectTableName: string = null;
    public TransportModeId: string = null;
    public IsInlandDomestic: boolean = false;
    public ItemsSource: ShipmentOrderPackageItem[] = [];
    public DataContext: OrdersTabComponent = this;
    public IsResourcesReady: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsCarrierServiceLineVisible: boolean = false;
    constructor(public entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.TransportModeId = this.EntityPM.TransportModeId;
        this.InitServices();
        this.Listen();
    }

    private myCardListService: CardListService;
    private myAirlineListService: AirlineListService;
    private myVesselListService: VesselListService;
    private myPartnersDomainService: PartnersDomainService;
    InitServices() {
        this.myCardListService = new CardListService();
        this.myAirlineListService = new AirlineListService();
        this.myVesselListService = new VesselListService();
        this.myPartnersDomainService = new PartnersDomainService();
    }

    private TabSelectedEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent) {

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                    this.BuildItemsSource();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
                    this.IsFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);

                    this.SetLabels();
                    this.SetUIProperties();
                    this.BuildItemsSource();
                }
            });

            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "SHOR" || tabCode == "JHOR") {
                    this.SetUIProperties();
                    this.BuildItemsSource();
                    this.BuildShipmentPickup();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
        AppTool.KillEventEmitter(this.TabSelectedEvent);
    }
    ngOnInit() {        
        if (this.EntityPM != null) {
            this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.IsFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.IsInlandDomestic = ShipmentTool.IsInlandDomestic(this.EntityPM);

            if (FeatureLocator.HasFeaturePermession("ShippingLine", "ShippingLine.Tab.ServiceLines") && this.TransportModeId == "O") {
                this.IsCarrierServiceLineVisible = true;
            }

            this.entityResourceService.getEntityResourceByTableName("ShipmentOrderPackage").subscribe((res1: any) => {
                this.entityResourceService.getEntityResourceByTableName("ShipmentPickUpDelivery").subscribe((res2: any) => {
                    this.IsResourcesReady = true;

                    this.SetLabels();
                    this.SetUIProperties();
                    this.BuildItemsSource();
                    this.SetBookingConfirmation();
                    this.BuildShipmentPickup();
                });
            });
        }


        //Tip
        var table = window.ObjectTables.filter(d=> d.Name == this.ObjectTableName && (d.Tenant == SessionLocator.Tenant || d.Tenant == 0))[0];
        if (table) {

            var tip = window.Tips.filter(d=> d.Code == "ORDT" && d.ObjectTableId == table.Id)[0];
            if (tip) {
                var hasTip = true;
           
                var isVisible: boolean = tip.VisibilityDefaultValue;

                var tipVisibility = window.TipsVisibilities.filter(d=> d.TipCode == tip.Code && d.UserId == SessionLocator.LoggedUserId)[0];
                if (tipVisibility) isVisible = tipVisibility.IsVisible;

                if (!isVisible && hasTip) this.IsTipsOpened = false;
                else this.IsTipsOpened = true;
            }
        }

    }

    // Labels
    public TabHeaderLabel: string;
    public AddButtonLabel: string;
    public DimensionsColumnHeader: string;
    public VolumeColumnHeader: string;
    public WeightColumnHeader: string;
    public ChargeableWeightUnitCodeLabel: string;
    public BookingGrossWeightLabel: string;
    public BookingVolumeLabel: string;
    public BookingChargeableWeightLabel: string;
    SetLabels() {

        if (this.EntityPM.ShipmentLevelCode == "C") {
            this.TabHeaderLabel = TextCodeTranslator.Translate("Shipment.S.Orders.Booking");
        }

        else {
            this.TabHeaderLabel = TextCodeTranslator.Translate("Shipment.S.Orders.Order");
        }

        if (this.IsLCLEntity) {
            this.AddButtonLabel = TextCodeTranslator.Translate("Shipment.B.Order.AddOrderPackage");
        }

        else {
            this.AddButtonLabel = "Add Order Container";
        }

        this.MeasurmentsButtonToolTip = TextCodeTranslator.Translate("Shipment.B.Packages.MeasurmentsSettings");

        this.SetAttachedLabels();
    }
    SetAttachedLabels() {
        this.DimensionsColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.Dimensions").replace("%UnitCode", this.EntityPM.DimensionsUnitCode);
        this.WeightColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.GrossWeight").replace("%UnitCode", this.EntityPM.GrossWeightUnitCode);
        this.VolumeColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.Volume").replace("%UnitCode", this.EntityPM.VolumeUnitCode)

        this.BookingVolumeLabel = TextCodeTranslator.Translate("Shipment.F.BookingVolume.Short").replace("%VolumeCode", this.EntityPM.VolumeUnitCode);
        this.BookingGrossWeightLabel = TextCodeTranslator.Translate("Shipment.F.OrderGrossWeight.Short").replace("%GrossWeightCode", this.EntityPM.GrossWeightUnitCode);

        if (this.EntityPM.TransportModeId == "A") {
            this.ChargeableWeightUnitCodeLabel = TextCodeTranslator.Translate("Shipment.F.ChargeableWeightUnitCode");
            this.BookingChargeableWeightLabel = TextCodeTranslator.Translate("Shipment.F.ChargeableWeight").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
        }

        else {
            this.ChargeableWeightUnitCodeLabel = TextCodeTranslator.Translate("Shipment.F.ChargeableWeightUnitCode.Short");
            this.BookingChargeableWeightLabel = TextCodeTranslator.Translate("Shipment.F.ChargeableWeight.Short").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
        }
    }

    public IsEditingEnabled: boolean = false;
    public IsAMSClosingDateVisible: boolean = false;
    public IsWarehouseFields_PickupsVisible: boolean = false;
    public IsWarehouseFields_DeliveriesVisible: boolean = false;
    SetUIProperties() {  
        var isEditingEnabled = ShipmentTool.IsEditingEnabled(this.EntityPM);
        var isCarrierEnabled = false;
        var isConfirmationEnabled = false;

        var isAMSClosingDateVisible: boolean = false;
        var isWarehouseFields_PickupsVisible: boolean = false;
        var isWarehouseFields_DeliveriesVisible: boolean = false;

        if (isEditingEnabled) {
            if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "C") {
                isCarrierEnabled = true;
                isConfirmationEnabled = true;

                if (!AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
                    isCarrierEnabled = false;
                }

                else {
                    if (this.TransportModeId == "A") {
                        var isTakenFromStock = (this.EntityPM.MainCarriageIsFromStack || this.EntityPM.MAWBTakenFromStack) ? true : false;

                        if (isTakenFromStock || !AppTool.IsNullOrEmpty(this.Master)) {
                            isCarrierEnabled = false;
                        }
                    }
                }
            }
        }

        if (this.EntityPM.DirectionId == "E" || this.EntityPM.DirectionId == "D" || this.EntityPM.DirectionId == "R") {
            if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H") {
                if (this.EntityPM.TransportModeId == "O" || this.EntityPM.TransportModeId == "I") {
                    if (this.IsInlandDomestic) {

                    }

                    else {
                        isWarehouseFields_PickupsVisible = true;
                    }
                }
            }
        }

        if (this.EntityPM.DirectionId == "I") {
            if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H") {
                if (this.EntityPM.TransportModeId == "O" || this.EntityPM.TransportModeId == "I") {
                    isWarehouseFields_DeliveriesVisible = true;
                }
            }
        }

        if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H") {
            if (this.TransportModeId == "O" || this.TransportModeId == "I") {
                isAMSClosingDateVisible = true;
            }
        }

        this.UIProperties.SetEnabled("DimensionsUnitCode", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("GrossWeightUnitCode", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("ChargeableWeightUnitCode", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("VolumeUnitCode", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("Ratio", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("DimFactor", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("DescriptionOfGoods", this.ObjectTableName, isEditingEnabled);        
        this.UIProperties.SetEnabled("OrderIsDangerouseGoods", this.ObjectTableName, isEditingEnabled);               
        this.UIProperties.SetEnabled("EmptyPickupContainerPartnerId", "ShipmentPickUpDelivery", isEditingEnabled);        
        this.UIProperties.SetEnabled("Notes", "ShipmentPickUpDelivery", isEditingEnabled);               
        this.UIProperties.SetEnabled("MainCarriageCarrierId", this.ObjectTableName, isCarrierEnabled);
        this.UIProperties.SetEnabled("MainCarriageCarrierNumber", this.ObjectTableName, isConfirmationEnabled);
        this.UIProperties.SetEnabled("BookingConfirmationNumber", this.ObjectTableName, isConfirmationEnabled);
        this.UIProperties.SetEnabled("BookingConfirmedBy", this.ObjectTableName, isConfirmationEnabled);
        this.UIProperties.SetEnabled("CutoffDate", this.ObjectTableName, isConfirmationEnabled);
        this.UIProperties.SetEnabled("MainCarriageVesselId", this.ObjectTableName, isConfirmationEnabled);
        this.UIProperties.SetEnabled("BookingConfirmationNotes", this.ObjectTableName, isConfirmationEnabled);
        this.UIProperties.SetEnabled("INTTRAContractNumber", this.ObjectTableName, isConfirmationEnabled);
        this.UIProperties.SetEnabled("MainHarmonize", this.ObjectTableName, isConfirmationEnabled);

        this.UIProperties.SetEnabled("WarehouseLegCutOffDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegVGMCutOffDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("TerminalAvailable", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegLastFreeDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("AMSClosingDate", this.ObjectTableName, isEditingEnabled);

        this.IsEditingEnabled = isEditingEnabled;
        this.IsAMSClosingDateVisible = isAMSClosingDateVisible;
        this.IsWarehouseFields_PickupsVisible = isWarehouseFields_PickupsVisible;
        this.IsWarehouseFields_DeliveriesVisible = isWarehouseFields_DeliveriesVisible;

        this.SetUIProperties_Totals();
        this.SetUIProperties_DimFactor();
        this.SetUIProperties_DimensionsUnitCode();
        this.SetUIProperties_ServiceLine();
    }
    SetUIProperties_Totals() {
        var isTotalsFieldEnabled = false;
        var isTotalsEditedFieldEnabled = false;

        if (this.IsEditingEnabled) {
            isTotalsFieldEnabled = true;
            isTotalsEditedFieldEnabled = true;

            if (this.EntityPM.ShipmentOrderPackages.length > 0) {
                isTotalsFieldEnabled = false;
            }
        }

        this.UIProperties.SetEnabled("BookingVolume", this.ObjectTableName, isTotalsFieldEnabled);
        this.UIProperties.SetEnabled("BookingNumberOfPackages", this.ObjectTableName, isTotalsFieldEnabled);
        this.UIProperties.SetEnabled("OrderGrossWeight", this.ObjectTableName, isTotalsEditedFieldEnabled);
        this.UIProperties.SetEnabled("OrderChargeableWeight", this.ObjectTableName, isTotalsEditedFieldEnabled);
    }
    SetUIProperties_ServiceLine() {
        this.UIProperties.SetEnabled("CarrierServiceLineId", this.ObjectTableName, !AppTool.IsNullOrEmpty(this.MainCarriageCarrierId));
    }
    public DimensionsDependencyProperty1: string = null;
    public DimensionsDependencyProperty1IsList: boolean = false;
    private SetUIProperties_DimensionsUnitCode() {
        var isFieldEnabled: boolean = false;

        if (this.IsEditingEnabled && this.VolumeUnitCode == "CBF") {
            isFieldEnabled = true;
        }

        if (this.VolumeUnitCode == "CBF") {
            this.DimensionsDependencyProperty1 = "Ft,Inc";
            this.DimensionsDependencyProperty1IsList = true;
        }

        else {
            this.DimensionsDependencyProperty1 = null;
            this.DimensionsDependencyProperty1IsList = false;
        }

        //this.UIProperties.SetEnabled("DimensionsUnitCode", this.ObjectTableName, isFieldEnabled);
    }

    // Measurments
    MeasurmentsButtonToolTip: string = null;
    IsMeasurmentsHidden: boolean = true;
    MeasurmentsSettingsClicked() {
        this.IsMeasurmentsHidden = !this.IsMeasurmentsHidden;

        if (this.IsMeasurmentsHidden) {
            this.MeasurmentsButtonToolTip = TextCodeTranslator.Translate("Shipment.B.Packages.HideMeasurmentsSettings");
        }

        else {
            this.MeasurmentsButtonToolTip = TextCodeTranslator.Translate("Shipment.B.Packages.MeasurmentsSettings");
        }
    }
    
    get VolumeUnitCode() { return this.EntityPM.VolumeUnitCode; }
    set VolumeUnitCode(newValue: string) {
        if (this.EntityPM.VolumeUnitCode != newValue) {
            this.EntityPM.VolumeUnitCode = newValue;

            this.EntityPM.DimensionsUnitCode = AppTool.GetDimentionsCodeFromVolumeCode(newValue);

            this.ComputeDimFactor();
            this.SetUIProperties_DimFactor();
            this.SetUIProperties_DimensionsUnitCode();
            this.OnMeasurmentsSettingsChanged();
        }
    }

    get DimensionsUnitCode() { return this.EntityPM.DimensionsUnitCode; }
    set DimensionsUnitCode(newValue: string) {
        if (this.EntityPM.DimensionsUnitCode != newValue) {
            this.EntityPM.DimensionsUnitCode = newValue;

            this.ComputeDimFactor();
            this.SetUIProperties_DimFactor();
            this.OnMeasurmentsSettingsChanged();
        }
    }

    get GrossWeightUnitCode() { return this.EntityPM.GrossWeightUnitCode; }
    set GrossWeightUnitCode(newValue: string) {
        if (this.EntityPM.GrossWeightUnitCode != newValue) {
            this.EntityPM.GrossWeightUnitCode = newValue;
            this.OnMeasurmentsSettingsChanged();
        }
    }

    get ChargeableWeightUnitCode() { return this.EntityPM.ChargeableWeightUnitCode; }
    set ChargeableWeightUnitCode(newValue: string) {
        if (this.EntityPM.ChargeableWeightUnitCode != newValue) {
            this.EntityPM.ChargeableWeightUnitCode = newValue;

            this.ComputeDimFactor();
            this.OnMeasurmentsSettingsChanged();
        }
    }

    get Ratio() { return this.EntityPM.Ratio; }
    set Ratio(newValue: number) {
        if (this.EntityPM.Ratio != newValue) {
            this.EntityPM.Ratio = newValue;

            this.ComputeDimFactor();
            ShipmentTool.OnShipmentRatioChanged(this.EntityPM);
        }
    }

    get DimFactor() { return this.EntityPM.DimFactor; }
    set DimFactor(newValue: number) {
        if (this.EntityPM.DimFactor != newValue) {
            this.EntityPM.DimFactor = newValue;

            this.EntityPM.Ratio = AppTool.GetRatioFromDimFactor(this.DimFactor, this.DimensionsUnitCode, this.ChargeableWeightUnitCode);
            ShipmentTool.OnShipmentRatioChanged(this.EntityPM);
        }
    }

    ComputeDimFactor() {
        this.EntityPM.DimFactor = AppTool.GetDimFactorFromRatio(this.Ratio, this.DimensionsUnitCode, this.ChargeableWeightUnitCode);
    }
    OnMeasurmentsSettingsChanged() {
        this.SetAttachedLabels();
        ShipmentTool.RecalculateShipmentFields(this.EntityPM);        
    }
    SetUIProperties_DimFactor() {
        var isDimFactorVisibile: boolean = false;

        if (!AppTool.IsNullOrEmpty(this.DimensionsUnitCode)) {
            if (this.DimensionsUnitCode.toUpperCase() == "INC") {
                isDimFactorVisibile = true;
            }
        }

        this.UIProperties.SetVisibility("DimFactor", this.ObjectTableName, isDimFactorVisibile);
    }

    // Tips
    public IsTipsOpened: boolean = false;
    public IsFirstTipLoad: boolean = true;
    TipVisibilityChanged(event) {

        if (event == "true") this.IsTipsOpened = true;
        else this.IsTipsOpened = false;
        this.IsFirstTipLoad = false;
    }
    TipsButtonClicked() {
        this.IsTipsOpened = !this.IsTipsOpened;
    }


    // OrderBookingDetails    
    BuildItemsSource() {
        this.ItemsSource = [];

        this.EntityPM.ShipmentOrderPackages.forEach((item) => {
            this.ItemsSource.push(new ShipmentOrderPackageItem(item, this, false));
        });

        this.SetUIProperties_Totals();
    }

    GrossWeightLostFocus(input: any) {
        if (this.EntityPM.ShipmentOrderPackages.length > 0) {
            var valueComputed: number = 0;
            var valueInserted: number = 0;

            this.EntityPM.ShipmentOrderPackages.forEach((item) => {
                if (!AppTool.IsNullOrEmpty(item.GrossWeight)) {
                    valueComputed += item.GrossWeight;
                }
            });

            // if (!AppTool.IsNullOrEmpty(input)) {
            //     input = AppTool.Replace(input, ",", "");
            //     valueInserted = Number(input);
            // }

            valueComputed = valueComputed == 0 ? null : valueComputed;
            valueInserted = AppTool.GetNumberFromText(input);
            this.OrderGrossWeightEdited = !(valueComputed == valueInserted);
            this.OrderGrossWeight = valueInserted;
            this.ComputeTotals();
        }
    }
    ChargeableWeightLostFocus(input: any) {
        if (this.EntityPM.ShipmentOrderPackages.length > 0) {
            var valueComputed: number = 0;
            var valueInserted: number = 0;

            valueComputed = AppTool.CalculateChargeableWeight(this.EntityPM.OrderGrossWeight, this.EntityPM.OrderVolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);

            // if (!AppTool.IsNullOrEmpty(input)) {
            //     input = AppTool.Replace(input, ",", "");
            //     valueInserted = Number(input);
            // }

            valueComputed = valueComputed == 0 ? null : valueComputed;
            valueInserted = AppTool.GetNumberFromText(input);
            this.OrderChargeableWeightEdited = !(valueComputed == valueInserted);
            this.OrderChargeableWeight = AppTool.RoundChargeableWeight(valueInserted, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
            this.ComputeTotals();
        }
    }
    ResetGrossWeightEdited() {
        this.OrderGrossWeightEdited = false;
        this.ComputeTotals();
    }
    ResetChargeableWeightEdited() {
        this.OrderChargeableWeightEdited = false;
        this.ComputeTotals();
    }
    ResetTotalEditedValues() {
        this.OrderGrossWeightEdited = false;
        this.OrderChargeableWeightEdited = false;
    }

    ComputeTotals() {

        if (this.EntityPM.ShipmentOrderPackages.length == 0) {
            this.BookingNumberOfPackages = null;
            this.OrderGrossWeight = null;
            this.BookingVolume = null;
            this.OrderVolumetricWeight = null;
            this.OrderChargeableWeight = null;
            this.OrderGrossWeightEdited = false;
            this.OrderChargeableWeightEdited = false;
        }

        else {
            var myQuantity: number = 0;
            var myVolume: number = 0;
            var myGrossWeight: number = 0;
            var myVolumetricWeight: number = 0;

            this.EntityPM.ShipmentOrderPackages.forEach(item => {

                if (item.Quantity != null) {
                    myQuantity += item.Quantity;
                }

                if (item.Volume != null) {
                    myVolume += item.Volume;
                }

                if (item.VolumetricWeight != null) {
                    myVolumetricWeight += item.VolumetricWeight;
                }

                if (item.GrossWeight != null) {
                    myGrossWeight += item.GrossWeight;
                }
            });

            this.BookingNumberOfPackages = myQuantity;
            this.BookingVolume = AppTool.Round(myVolume, 3);
            this.OrderVolumetricWeight = AppTool.Round(myVolumetricWeight, 3);

            if (!this.OrderGrossWeightEdited) {
                this.OrderGrossWeight = AppTool.Round(myGrossWeight, 3);
            }

            if (!this.OrderChargeableWeightEdited) {
                this.OrderChargeableWeight = AppTool.CalculateChargeableWeight(this.EntityPM.OrderGrossWeight, this.EntityPM.OrderVolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
            }
        }

        this.SetUIProperties_Totals();
    }

    get OrderGrossWeightEdited() { return this.EntityPM.OrderGrossWeightEdited; }
    set OrderGrossWeightEdited(value: boolean) {
        if (this.EntityPM.OrderGrossWeightEdited != value) {
            this.EntityPM.OrderGrossWeightEdited = value;
        }
    }

    get OrderChargeableWeightEdited() { return this.EntityPM.OrderChargeableWeightEdited; }
    set OrderChargeableWeightEdited(value: boolean) {
        if (this.EntityPM.OrderChargeableWeightEdited != value) {
            this.EntityPM.OrderChargeableWeightEdited = value;
        }
    }

    public AddPackage() {
        var itemPM = new ShipmentOrderPackagePM(null);
        itemPM.ShipmentId = this.EntityPM.Id;
        itemPM.Tenant = this.EntityPM.Tenant;
        var itemComponent = new ShipmentOrderPackageItem(itemPM, this, true);

        var title = TextCodeTranslator.Translate("Shipment.S.Orders.AddOrderPackage");
        if (this.IsFCLEntity) {
            title = "Add Container";
        }

        this.RunPackageWindow(itemComponent, title);
    }
    public EditPackage(itemComponent: ShipmentOrderPackageItem)
    {
        var title = TextCodeTranslator.Translate("Shipment.S.Orders.EditOrderPackage");
        if (this.IsFCLEntity) {
            title = "Edit Container";
        }

        this.RunPackageWindow(itemComponent, title);
    }
    public DeletePackage(itemComponent: ShipmentOrderPackageItem) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show(TextCodeTranslator.Translate("Shipment.M.DeleteThisOrderPackage"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {

                this.EntityPM.RemoveOrderPackage(itemComponent.EntityPM);

                var index = this.ItemsSource.indexOf(itemComponent);
                if (index > -1) {
                    this.ItemsSource.splice(index, 1);
                }

                this.ComputeTotals();
            }
        });
    }
    private RunPackageWindow(itemComponent: ShipmentOrderPackageItem, windowTitle: string) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = windowTitle;
        logitudeWindow.DataContext = itemComponent;
        logitudeWindow.Show('./ShipmentModules/ShipmentTabs/Components/Orders/AddEditOrderPackageComponent');
    }

    // Booking Confirmation
    public CarrierTextCode: string = null;
    public CarrierNumberTextCode: string = null;
    public CarrierDependencyFilter1Value: string = null;
    private SetBookingConfirmation() {

        switch (this.EntityPM.TransportModeId.toUpperCase()) {
            case "A":
                {
                    this.CarrierTextCode = "Shipment.O.Routings.Airline";
                    this.CarrierNumberTextCode = "Shipment.O.Routings.FlightNo";
                    this.CarrierDependencyFilter1Value = "AL";
                    break;
                }

            case "O":
                {
                    this.CarrierTextCode = "Shipment.O.Routings.Shippingline";
                    this.CarrierNumberTextCode = "Shipment.O.Routings.VoyageNo";
                    this.CarrierDependencyFilter1Value = "SL";
                    break;
                }

            default:
                {
                    this.CarrierTextCode = "Shipment.O.Routings.Trucker";
                    this.CarrierNumberTextCode = "Shipment.O.Routings.TruckNo";
                    this.CarrierDependencyFilter1Value = "TR";
                    break;
                }
        }

    }

    // Main Carrier
    get MainCarriageCarrierId() { return this.EntityPM.MainCarriageCarrierId; }
    set MainCarriageCarrierId(value: string) {
        if (this.EntityPM.MainCarriageCarrierId != value) {
            this.EntityPM.MainCarriageCarrierId = value;

            this.CarrierServiceLineId = null;
            this.SetUIProperties_ServiceLine();

            if (AppTool.IsNullOrEmpty(value)) {
                this.MainCarriageCarrierPrefix = null;
                this.MainCarriageCarrierNumber = null;
                this.Master = null;                

                RoutingHelper.MainCarriageCarrierChanged(this.EntityPM, null);                

                if (this.TransportModeId == "A") {
                    if (AppTool.IsNullOrEmpty(this.EntityPM.InterlineId)) {

                        this.CarrierIsCheckDigit = false;
                        this.CarrierIsLimitedLength = false;
                        this.AirlinePrefix = null;
                    }

                    this.CarrierIsChampRegistered = false;
                    this.CarrierIsGLSHKRegistered = false;
                    ShipmentTool.MapTenantZeroAirline(this.EntityPM, null);
                }
            }

            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {

                            if (this.TransportModeId == "A") {
                                this.MainCarriageCarrierPrefix = list.Code;
                            }

                            RoutingHelper.MainCarriageCarrierChanged(this.EntityPM, list);

                            if (this.TransportModeId == "A") {

                                // dont get from chach: if user choosed from tenant0 it wont get it
                                this.myAirlineListService.getSingle(value).subscribe((myAirlineListResponse: ServiceResponse) => {
                                    if (myAirlineListResponse != null) {

                                        var myAirlineList: AirlineList = myAirlineListResponse.Result;
                                        if (myAirlineList != null) {
                                            if (AppTool.IsNullOrEmpty(this.EntityPM.InterlineId)) {

                                                this.CarrierIsCheckDigit = myAirlineList.CheckDigit;
                                                this.CarrierIsLimitedLength = myAirlineList.LimitedLength;

                                                var myPrefix: string = null;
                                                if (!AppTool.IsNullOrEmpty(myAirlineList.Prefix)) {
                                                    myPrefix = myAirlineList.Prefix.toString().trim();
                                                    myPrefix = AppTool.PadLeft(myPrefix, 3, '0');
                                                }

                                                this.AirlinePrefix = myPrefix;
                                            }

                                            this.CarrierIsChampRegistered = myAirlineList.IsChampRegistered;
                                            this.CarrierIsGLSHKRegistered = myAirlineList.IsGLSHKRegistered;

                                            this.myPartnersDomainService.GetAirlineByCode(myAirlineList.Code, 0).subscribe((myResponse: ServiceResponse) => {
                                                if (!myResponse.HasError) {
                                                    ShipmentTool.MapTenantZeroAirline(this.EntityPM, myResponse.Result);
                                                }
                                            });
                                        }
                                    }
                                });
                            }
                        }
                    }
                });
            }
        }
    }
    get CarrierServiceLineId() { return this.EntityPM.CarrierServiceLineId; }
    set CarrierServiceLineId(value: string) {
        if (this.EntityPM.CarrierServiceLineId != value) {
            this.EntityPM.CarrierServiceLineId = value;
        }
    }

    get MainCarriageCarrierCode() { return this.EntityPM.MainCarriageCarrierCode; }
    set MainCarriageCarrierCode(value: string) {
        if (this.EntityPM.MainCarriageCarrierCode != value) {
            this.EntityPM.MainCarriageCarrierCode = value;
        }
    }

    get MainCarriageCarrierName() { return this.EntityPM.MainCarriageCarrierName; }
    set MainCarriageCarrierName(value: string) {
        if (this.EntityPM.MainCarriageCarrierName != value) {
            this.EntityPM.MainCarriageCarrierName = value;
        }
    }

    get MainCarriageCarrierPrefix() { return this.EntityPM.MainCarriageCarrierPrefix; }
    set MainCarriageCarrierPrefix(value: string) {
        if (this.EntityPM.MainCarriageCarrierPrefix != value) {
            this.EntityPM.MainCarriageCarrierPrefix = AppTool.IsNullOrEmpty(value) ? value : value.trim();
        }
    }

    get MainCarriageCarrierWebSite() { return this.EntityPM.MainCarriageCarrierWebSite; }
    set MainCarriageCarrierWebSite(value: string) {
        if (this.EntityPM.MainCarriageCarrierWebSite != value) {
            this.EntityPM.MainCarriageCarrierWebSite = value;
        }
    }

    get MainCarriageCarrierNumber() { return this.EntityPM.MainCarriageCarrierNumber; }
    set MainCarriageCarrierNumber(value: string) {
        if (this.EntityPM.MainCarriageCarrierNumber != value) {
            this.EntityPM.MainCarriageCarrierNumber = AppTool.IsNullOrEmpty(value) ? value : value.trim();
        }
    }

    get CarrierIsCheckDigit() { return this.EntityPM.CarrierIsCheckDigit; }
    set CarrierIsCheckDigit(value: boolean) {
        if (this.EntityPM.CarrierIsCheckDigit != value) {
            this.EntityPM.CarrierIsCheckDigit = value;
        }
    }

    get CarrierIsLimitedLength() { return this.EntityPM.CarrierIsLimitedLength; }
    set CarrierIsLimitedLength(value: boolean) {
        if (this.EntityPM.CarrierIsLimitedLength != value) {
            this.EntityPM.CarrierIsLimitedLength = value;
        }
    }

    get CarrierIsChampRegistered() { return this.EntityPM.CarrierIsChampRegistered; }
    set CarrierIsChampRegistered(value: boolean) {
        if (this.EntityPM.CarrierIsChampRegistered != value) {
            this.EntityPM.CarrierIsChampRegistered = value;
        }
    }

    get CarrierIsGLSHKRegistered() { return this.EntityPM.CarrierIsGLSHKRegistered; }
    set CarrierIsGLSHKRegistered(value: boolean) {
        if (this.EntityPM.CarrierIsGLSHKRegistered != value) {
            this.EntityPM.CarrierIsGLSHKRegistered = value;
        }
    }

    get AirlinePrefix() { return this.EntityPM.AirlinePrefix; }
    set AirlinePrefix(value: string) {
        if (this.EntityPM.AirlinePrefix != value) {
            this.EntityPM.AirlinePrefix = value;
            this.LongMaster = ShipmentTool.GetLongMasterField(this.EntityPM.TransportModeId, this.EntityPM.AirlinePrefix, this.EntityPM.Master);
        }
    }

    get Master() { return this.EntityPM.Master; }
    set Master(value: string) {
        if (this.EntityPM.Master != value) {
            this.EntityPM.Master = value;
            this.LongMaster = ShipmentTool.GetLongMasterField(this.EntityPM.TransportModeId, this.EntityPM.AirlinePrefix, this.EntityPM.Master);
        }
    }

    get LongMaster() { return this.EntityPM.LongMaster; }
    set LongMaster(newValue: string) {
        if (this.EntityPM.LongMaster != newValue) {
            this.EntityPM.LongMaster = newValue;
        }
    }

    get BookingConfirmationNumber() { return this.EntityPM.BookingConfirmationNumber; }
    set BookingConfirmationNumber(newValue: string) {
        if (this.EntityPM.BookingConfirmationNumber != newValue) {
            this.EntityPM.BookingConfirmationNumber = newValue;
        }
    }

    get BookingConfirmedBy() { return this.EntityPM.BookingConfirmedBy; }
    set BookingConfirmedBy(newValue: string) {
        if (this.EntityPM.BookingConfirmedBy != newValue) {
            this.EntityPM.BookingConfirmedBy = newValue;
        }
    }

    get CutoffDate() { return this.EntityPM.CutoffDate; }
    set CutoffDate(newValue: Date) {
        if (this.EntityPM.CutoffDate != newValue) {
            this.EntityPM.CutoffDate = newValue;
        }
    }

    get MainCarriageVesselId() { return this.EntityPM.MainCarriageVesselId; }
    set MainCarriageVesselId(value: string) {
        if (this.EntityPM.MainCarriageVesselId != value) {
            this.EntityPM.MainCarriageVesselId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.EntityPM.MainCarriageVesselName = null;
            }

            else {
                this.myVesselListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: VesselList = myResponse.Result;
                        if (list) {
                            this.EntityPM.MainCarriageVesselName = list.EnglishName;
                        }
                    }
                });
            }
        }
    }

    get TerminalAvailable() { return this.EntityPM.TerminalAvailable; }
    set TerminalAvailable(value: Date) {
        if (this.EntityPM.TerminalAvailable != value) {
            this.EntityPM.TerminalAvailable = value;
        }
    }

    get WarehouseLegLastFreeDate() { return this.EntityPM.WarehouseLegLastFreeDate; }
    set WarehouseLegLastFreeDate(newValue: Date) {
        if (this.EntityPM.WarehouseLegLastFreeDate != newValue) {
            this.EntityPM.WarehouseLegLastFreeDate = newValue;
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

    get AMSClosingDate() { return this.EntityPM.AMSClosingDate; }
    set AMSClosingDate(newValue: Date) {
        if (this.EntityPM.AMSClosingDate != newValue) {
            this.EntityPM.AMSClosingDate = newValue;
        }
    }


    get BookingConfirmationNotes() { return this.EntityPM.BookingConfirmationNotes; }
    set BookingConfirmationNotes(newValue: string) {
        if (this.EntityPM.BookingConfirmationNotes != newValue) {
            this.EntityPM.BookingConfirmationNotes = newValue;
        }
    }

    get INTTRAContractNumber() { return this.EntityPM.INTTRAContractNumber; }
    set INTTRAContractNumber(value: string) {
        if (this.EntityPM.INTTRAContractNumber != value) {
            this.EntityPM.INTTRAContractNumber = value;
        }
    }

    get MainHarmonize() { return this.EntityPM.MainHarmonize; }
    set MainHarmonize(value: string) {
        if (this.EntityPM.MainHarmonize != value) {
            this.EntityPM.MainHarmonize = value;
        }
    }

    ChooseHarmonizeClicked() {
        if (this.IsEditingEnabled) {
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Title = TextCodeTranslator.TranslateTablePlural("HarmonizeCode") + " Search";
            logitudeWindow.WindowArgs = { Entity: this.EntityPM, FieldName: 'MainHarmonize' };
            logitudeWindow.Show("./ShipmentModules/ShipmentTabs/Components/Windows/Harmonizes/HarmonizesComponent");
            logitudeWindow.WindowClosed.subscribe(s => {

            });
        }
    }

    //Details
    get BookingNumberOfPackages() { return this.EntityPM.BookingNumberOfPackages; }
    set BookingNumberOfPackages(newValue: number) {
        if (this.EntityPM.BookingNumberOfPackages != newValue) {
            this.EntityPM.BookingNumberOfPackages = newValue;

            this.CurrentSession.SessionEvent.emit("UpdatePackagesTab");
        }
    }

    get OrderGrossWeight() { return this.EntityPM.OrderGrossWeight; }
    set OrderGrossWeight(newValue: number) {
        if (this.EntityPM.OrderGrossWeight != newValue) {
            this.EntityPM.OrderGrossWeight = AppTool.Round(newValue, 3);

            if (this.EntityPM.ShipmentOrderPackages.length == 0) {
                this.EntityPM.OrderChargeableWeight = AppTool.CalculateChargeableWeight(this.EntityPM.OrderGrossWeight, this.EntityPM.OrderVolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
                this.ComputeOrderVolumetricWeight();
            }
        }
    }

    get BookingVolume() { return this.EntityPM.BookingVolume; }
    set BookingVolume(newValue: number) {
        if (this.EntityPM.BookingVolume != newValue) {
            this.EntityPM.BookingVolume = AppTool.Round(newValue, 3);

            if (this.EntityPM.ShipmentOrderPackages.length == 0) {
                this.ComputeOrderVolumetricWeight();
            }
        }
    }

    get OrderVolumetricWeight() { return this.EntityPM.OrderVolumetricWeight; }
    set OrderVolumetricWeight(newValue: number) {
        if (this.EntityPM.OrderVolumetricWeight != newValue) {
            this.EntityPM.OrderVolumetricWeight = AppTool.Round(newValue, 3);

            if (this.EntityPM.ShipmentOrderPackages.length == 0) {
                this.EntityPM.OrderChargeableWeight = AppTool.CalculateChargeableWeight(this.EntityPM.OrderGrossWeight, this.EntityPM.OrderVolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
            }
        }
    }

    get OrderChargeableWeight() { return this.EntityPM.OrderChargeableWeight; }
    set OrderChargeableWeight(newValue: number) {
        if (this.EntityPM.OrderChargeableWeight != newValue) {
            var myResult: number = AppTool.Round(newValue, 3);
            this.EntityPM.OrderChargeableWeight = myResult;

            if (this.EntityPM.ShipmentOrderPackages.length == 0) {
                if (this.OrderGrossWeight == null && this.OrderVolumetricWeight == null) {
                    this.EntityPM.OrderVolumetricWeight = myResult;
                    this.EntityPM.OrderGrossWeight = AppTool.GetWeightFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.GrossWeightUnitCode, myResult);
                    this.EntityPM.BookingVolume = AppTool.GetVolumeFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.VolumeUnitCode, myResult, this.EntityPM.Ratio);
                }
            }
        }
    }

    get DescriptionOfGoods() { return this.EntityPM.DescriptionOfGoods; }
    set DescriptionOfGoods(newValue: string) {
        if (this.EntityPM.DescriptionOfGoods != newValue) {
            this.EntityPM.DescriptionOfGoods = newValue;
        }
    }

    get OrderIsDangerouseGoods() { return this.EntityPM.OrderIsDangerouseGoods; }
    set OrderIsDangerouseGoods(newValue: boolean) {
        if (this.EntityPM.OrderIsDangerouseGoods != newValue) {
            this.EntityPM.OrderIsDangerouseGoods = newValue;
        }
    }

    ComputeOrderVolumetricWeight() {
        var myResult: number = null;

        if (this.EntityPM.Ratio == null) {
            this.EntityPM.Ratio = AppTool.GetRatio(this.EntityPM.DirectionId, this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
        }

        if (this.EntityPM.BookingVolume != null) {
            myResult = AppTool.GetWeightFromVolume(this.EntityPM.VolumeUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.BookingVolume, this.EntityPM.Ratio);
        }

        else if (this.OrderGrossWeight != null) {
            myResult = AppTool.GetWeightFromWeight(this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.OrderGrossWeight);
        }

        this.EntityPM.OrderVolumetricWeight = myResult;
    }
    ComputeChargeableWeight() {
        this.OrderChargeableWeight = AppTool.CalculateChargeableWeight(this.EntityPM.OrderGrossWeight, this.EntityPM.OrderVolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
    }

    // Pickup Details
    public ShipmentPickup: ShipmentPickUpPM;
    public PickupFromPortCode: string;
    public PickupFromPortName: string;
    public PickupFromCountryCode: string;
    public PickupFromCountryName: string;
    public PickupToPortCode: string;
    public PickupToPortName: string;
    public PickupToCountryCode: string;
    public PickupToCountryName: string;
    BuildShipmentPickup() {

        var fromPortCode = "";
        var fromPortName = "";
        var fromCountryCode = "";
        var fromCountryName = "";
        var toPortCode = "";
        var toPortName = "";
        var toCountryCode = "";
        var toCountryName = "";

        this.ShipmentPickup = this.EntityPM.ShipmentPickUps.sort(function (a, b) { return a.PickUpDeliveryNumber.toLowerCase() == b.PickUpDeliveryNumber.toLowerCase() ? 0 : a.PickUpDeliveryNumber.toLowerCase() < b.PickUpDeliveryNumber.toLowerCase() ? -1 : 1; })[0];

        if (this.ShipmentPickup != null) {            

            if (this.ShipmentPickup.FromPortId != null) {
                fromPortCode = this.ShipmentPickup.FromPortCode;
                fromPortName = this.ShipmentPickup.FromPortName;
                fromCountryCode = this.ShipmentPickup.FromPortCountryCode;
                fromCountryName = this.ShipmentPickup.FromPortCountryName;
            }
            else {
                fromPortCode = this.ShipmentPickup.FromAddressCountryCode;
                fromPortName = this.ShipmentPickup.FromAddressCountryName;
                fromCountryCode = this.ShipmentPickup.FromAddressCountryCode;
                fromCountryName = this.ShipmentPickup.FromAddressCountryName;
            }

            if (this.ShipmentPickup.ToPortId != null) {
                toPortCode = this.ShipmentPickup.ToPortCode;
                toPortName = this.ShipmentPickup.ToPortName;
                toCountryCode = this.ShipmentPickup.ToPortCountryCode;
                toCountryName = this.ShipmentPickup.ToPortCountryName;
            }
            else {
                toPortCode = this.ShipmentPickup.ToAddressCountryCode;
                toPortName = this.ShipmentPickup.ToAddressCountryName;
                toCountryCode = this.ShipmentPickup.ToAddressCountryCode;
                toCountryName = this.ShipmentPickup.ToAddressCountryName;
            }
        }

        this.PickupFromPortCode = fromPortCode;
        this.PickupFromPortName = fromPortName;
        this.PickupFromCountryCode = fromCountryCode;
        this.PickupFromCountryName = fromCountryName;
        this.PickupToPortCode = toPortCode;
        this.PickupToPortName = toPortName;
        this.PickupToCountryCode = toCountryCode;
        this.PickupToCountryName = toCountryName;
    }

    get EmptyPickupContainerPartnerId() {
        var myResult: string = null;

        if (this.ShipmentPickup != null) {
            myResult = this.ShipmentPickup.EmptyPickupContainerPartnerId;
        }

        return myResult;
    }
    set EmptyPickupContainerPartnerId(newValue: string) {
        if (this.ShipmentPickup != null) {
            if (this.ShipmentPickup.EmptyPickupContainerPartnerId != newValue) {
                this.ShipmentPickup.EmptyPickupContainerPartnerId = newValue;
            }
        }
    }

    get Notes() {
        var myResult: string = null;

        if (this.ShipmentPickup != null) {
            myResult = this.ShipmentPickup.Notes;
        }

        return myResult;
    }
    set Notes(newValue: string) {
        if (this.ShipmentPickup != null) {
            if (this.ShipmentPickup.Notes != newValue) {
                this.ShipmentPickup.Notes = newValue;
            }
        }
    }    
}
export class ShipmentOrderPackageItem extends BaseComponent {
    public EntityPM: ShipmentOrderPackagePM;
    public ShipmentPM: ShipmentPM;
    public ObjectTableName = "ShipmentOrderPackage";
    public IsNewEntity: boolean = false;
    constructor(entity: ShipmentOrderPackagePM, public fatherComponent: OrdersTabComponent, isNewEntity: boolean) {
        super();
        this.EntityPM = entity;
        this.ShipmentPM = fatherComponent.EntityPM;
        this.IsNewEntity = isNewEntity;
        this.SetUIProperties();
    }

    public IsEditingEnabled: boolean = false;
    public SetUIProperties() {

        var isFieldEnabled = false;
        var isVolumeEnabled = false;
        var isDimensionEnabled = false;

        this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;

        if (this.IsEditingEnabled) {

            if (this.Quantity > 0) {
                isFieldEnabled = true;
                isVolumeEnabled = true;
                isDimensionEnabled = true;

                if (this.Height != null || this.Width != null || this.Length != null) {
                    isVolumeEnabled = false;
                }

                else if (this.Volume != null) {
                    isDimensionEnabled = false;
                }
            }
        }

        this.UIProperties.SetEnabled("PackageTypeId", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("VolumetricWeight", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isFieldEnabled);

        var isFieldRequired1 = false;
        var isFieldRequired2 = false;

        if (this.fatherComponent.TransportModeId != "A") {
            if (AppTool.IsNullOrEmpty(this.PackageTypeId)) {
                isFieldRequired1 = true;
            }

            if (AppTool.IsNullOrEmpty(this.GrossWeight)) {
                isFieldRequired2 = true;
            }
        }

        this.UIProperties.SetRequired("PackageTypeId", this.ObjectTableName, isFieldRequired1);
        this.UIProperties.SetRequired("GrossWeight", this.ObjectTableName, isFieldRequired2);
    }

    get PackageTypeId() { return this.EntityPM.PackageTypeId; }
    set PackageTypeId(newValue: string) {
        if (this.EntityPM.PackageTypeId != newValue) {
            this.EntityPM.PackageTypeId = newValue;
            this.SetUIProperties();

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.PackageTypeName = null;
                this.IsContainer = false;
            }

            else {
                var myService: PackageTypeListService = new PackageTypeListService();
                myService.getSingleFromCache(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PackageTypeList = myResponse.Result;
                        if (list != null) {
                            this.PackageTypeName = list.EnglishName;
                            this.IsContainer = list.IsContainer;
                        }
                    }
                });
            }
        }
    }

    get PackageTypeName() { return this.EntityPM.PackageTypeName; }
    set PackageTypeName(newValue: string) {
        if (this.EntityPM.PackageTypeName != newValue) {
            this.EntityPM.PackageTypeName = newValue;
        }
    }

    get IsContainer() { return this.EntityPM.IsContainer; }
    set IsContainer(newValue: boolean) {
        if (this.EntityPM.IsContainer != newValue) {
            this.EntityPM.IsContainer = newValue;
        }
    }

    get Quantity() { return this.EntityPM.Quantity; }
    set Quantity(newValue: number) {
        if (this.EntityPM.Quantity != newValue) {
            this.EntityPM.Quantity = newValue;

            this.SetUIProperties();
            this.ComputeVolume();
            this.ComputeTotals();
        }
    }

    get Length() { return this.EntityPM.Length; }
    set Length(newValue: number) {
        if (this.EntityPM.Length != newValue) {
            this.EntityPM.Length = AppTool.Round(newValue, 2);
            this.ComputeVolume();
            this.SetUIProperties();
        }
    }

    get Width() { return this.EntityPM.Width; }
    set Width(newValue: number) {
        if (this.EntityPM.Width != newValue) {
            this.EntityPM.Width = AppTool.Round(newValue, 2);
            this.ComputeVolume();
            this.SetUIProperties();
        }
    }

    get Height() { return this.EntityPM.Height; }
    set Height(newValue: number) {
        if (this.EntityPM.Height != newValue) {
            this.EntityPM.Height = AppTool.Round(newValue, 2);
            this.ComputeVolume();
            this.SetUIProperties();
        }
    }

    get Dimensions() {
        var myDimensions: string;

        if (this.Length == null && this.Width == null && this.Height == null) {
            myDimensions = " - - ";
        }

        else {
            var myLength: number = 0;
            var myWidth: number = 0;
            var myHeight: number = 0;

            if (this.Length != null) {
                myLength = this.Length;
            }

            if (this.Width != null) {
                myWidth = this.Width;
            }

            if (this.Height != null) {
                myHeight = this.Height;
            }

            myDimensions = myLength + "-" + myWidth + "-" + myHeight;
        }

        return myDimensions;
    }

    get Volume() { return this.EntityPM.Volume; }
    set Volume(newValue: number) {
        if (this.EntityPM.Volume != newValue) {
            this.EntityPM.Volume = AppTool.Round(newValue, 3);
            this.ComputeVolumetricWeight();
            this.SetUIProperties();
        }
    }

    get GrossWeight() { return this.EntityPM.GrossWeight; }
    set GrossWeight(newValue: number) {
        var myValue: number = AppTool.Round(newValue, 3);

        if (this.EntityPM.GrossWeight != myValue) {
            this.EntityPM.GrossWeight = myValue

            this.SetUIProperties();
            this.ComputeTotals();
        }
    }

    OnGrossWeightLostFocus(input1: number) {
        if (this.fatherComponent.IsLCLEntity) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.Volume)) {
                if (this.Width == null || this.Height == null || this.Length == null) {
                    this.EntityPM.VolumetricWeight = AppTool.GetWeightFromWeight(this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode, this.EntityPM.GrossWeight);
                    this.EntityPM.Volume = AppTool.GetVolumeFromWeight(this.ShipmentPM.ChargeableWeightUnitCode, this.ShipmentPM.VolumeUnitCode, this.EntityPM.VolumetricWeight, this.ShipmentPM.Ratio);

                    this.SetUIProperties();
                    this.ComputeTotals();
                }
            }
        }
    }

    get VolumetricWeight() { return this.EntityPM.VolumetricWeight; }
    set VolumetricWeight(newValue: number) {
        if (this.EntityPM.VolumetricWeight != newValue) {
            this.EntityPM.VolumetricWeight = AppTool.Round(newValue, 3);
            this.ComputeTotals();
        }
    }

    private ComputeVolume() {
        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
        }

        this.Volume = AppTool.ComputePackageVolume(this.Quantity, this.Width, this.Height, this.Length, this.GrossWeight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode);
    }
    private ComputeVolumetricWeight() {
        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
        }

        this.VolumetricWeight = AppTool.ComputePackageVolumetricWeight(this.Quantity, this.Width, this.Height, this.Length, this.Volume, this.GrossWeight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode);
    }
    private ComputeTotals() {
        if (!this.IsNewEntity) {
            this.fatherComponent.ComputeTotals();
        }
    }
}
