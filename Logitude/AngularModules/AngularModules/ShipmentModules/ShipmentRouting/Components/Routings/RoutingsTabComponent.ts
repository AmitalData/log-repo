import {Component, OnInit, OnDestroy}  from '@angular/core';
import {AppTool, DateTool, FontTool} from '../../../../Infrastructure/Tools';
import {ShipmentTool, RoutingHelper} from '../../../../Shipment/Tools';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ShipmentPM } from '../../../../Shipment/EntityPMs/ShipmentPM';
import { ShipmentPackagePM } from '../../../../Shipment/EntityPMs/ShipmentPackagePM';
import {ShipmentPickUpPM} from '../../../../Shipment/EntityPMs/ShipmentPickUpPM';
import { ShipmentDeliveryPM } from '../../../../Shipment/EntityPMs/ShipmentDeliveryPM';
import { ShipmentPickUpDeliveryPackagePM } from '../../../../Shipment/EntityPMs/ShipmentPickUpDeliveryPackagePM';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {AddressPM} from '../../../../Common/EntityPMs/AddressPM';
import {CardList} from '../../../../Common/EntityLists/CardList'; 
import {AddressList} from '../../../../Common/EntityLists/AddressList';      
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {AddressListService} from '../../../../Common/Services/StandardLists/AddressListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    templateUrl: './RoutingsTabComponent.html',
})

export class RoutingsTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: ShipmentPM;
    public ObjectTableName: string = null;
    public DataContext = this;
    public IsLCLEntity: boolean = false;
    public IsFCLEntity: boolean = false;
    public ShipmentLevelCode: string = null;
    public TransportModeId: string = null;
    public IsInlandDomestic: boolean = false;
    public CardLOVDependencyProperty1: string = null;
    public ItemsSource: RoutingItem[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.ObjectTableName = entityArgs.ObjectTableName;
        this.ShipmentLevelCode = this.EntityPM.ShipmentLevelCode;
        this.TransportModeId = this.EntityPM.TransportModeId;
        this.SetUIProperties();
        this.Listen();
    }

    private SessionEvent: any = null;
    private TabSelectedEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null; 
    private Listen() {
        if (this.entityArgs.EditComponent) {

            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "AWBWizardClosed") {
                    this.UpdateScreen();
                }

                if (s == "RefreshWareHouseLeg") {
                    this.GetWarehouseAddress();
                }

            });

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.UpdateScreen();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.IsInlandDomestic = ShipmentTool.IsInlandDomestic(this.EntityPM);
                    this.UpdateScreen();
                }
            });

            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "SHRT" || tabCode == "JHRT") {
                    this.UpdateScreen();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
        AppTool.KillEventEmitter(this.TabSelectedEvent);
    }
    ngOnInit() {
        if (this.EntityPM != null) {
            this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.IsFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.IsInlandDomestic = ShipmentTool.IsInlandDomestic(this.EntityPM);

            if (this.IsInlandDomestic) {
                this.CardLOVDependencyProperty1 = (this.ShipmentLevelCode == "C") ? "AG" : "CS";
                this.InitializePartners();
            }

            this.UpdateScreen(); 
            this.GetWarehouseAddress();           
        }
    }

    UpdateScreen() {
        this.SetUIProperties();

        if (this.IsInlandDomestic) {

        }

        else {
            this.BuildItemsCollection();
        }
    }

    public IsEditingEnabled: boolean = true;
    SetUIProperties() {
        this.IsEditingEnabled = ShipmentTool.IsEditingEnabled(this.EntityPM);

        if (this.IsInlandDomestic) {
            this.SetUIProperties_InlandDomestic();
        }
    }

    public IsAddPreCarriageDisabled: boolean = false;
    public IsAddOnCarriageDisabled: boolean = false;
    SetAddButtonsIsDisabled() {
        if (this.EntityPM.ShipmentLevelCode == "C") {
            this.IsAddPreCarriageDisabled = true;
            this.IsAddOnCarriageDisabled = true;
        }

        else {
            if (this.EntityPM.PreCarriageFromPortId == null && this.EntityPM.PreCarriageToPortId == null) {
                this.IsAddPreCarriageDisabled = false;
            }

            else {
                this.IsAddPreCarriageDisabled = true;
            }

            if (this.EntityPM.OnCarriageFromPortId == null && this.EntityPM.OnCarriageToPortId == null) {
                this.IsAddOnCarriageDisabled = false;
            }

            else {
                this.IsAddOnCarriageDisabled = true;
            }
        }
    }
    BuildItemsCollection() {

        this.ItemsSource = [];

        if (this.EntityPM.ShipmentPickUps.length == 0) {
            var newPickup: ShipmentPickUpPM = new ShipmentPickUpPM(null);
            newPickup.ShipmentId = this.EntityPM.Id;
            newPickup.ShipmentNumber = this.EntityPM.ShipmentNumber;
            newPickup.Tenant = this.EntityPM.Tenant;
            this.ItemsSource.push(new RoutingItem(newPickup, "Pick Up", this));
        }

        else {
            var myShipmentPickUps: ShipmentPickUpPM[] = this.EntityPM.ShipmentPickUps.sort(function (a, b) { return a.PickUpDeliveryNumber.toLowerCase() == b.PickUpDeliveryNumber.toLowerCase() ? 0 : a.PickUpDeliveryNumber.toLowerCase() < b.PickUpDeliveryNumber.toLowerCase() ? -1 : 1; });
            myShipmentPickUps.forEach((item) => {
                this.ItemsSource.push(new RoutingItem(item, "Pick Up", this));
            });
        }

        // WarehouseLeg_Pickups
        if (this.EntityPM.DirectionId == "E" || this.EntityPM.DirectionId == "D" || this.EntityPM.DirectionId == "R") {
            if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H") {
                if (this.IsInlandDomestic) {

                }

                else {
                    this.ItemsSource.push(new RoutingItem(this.EntityPM, "WarehouseLeg_Pickups", this));
                }
            }
        }

        if (this.EntityPM.PreCarriageFromPortId != null && this.EntityPM.PreCarriageToPortId != null) {
            this.ItemsSource.push(new RoutingItem(this.EntityPM, "Pre Carriage", this));
        }

        this.ItemsSource.push(new RoutingItem(this.EntityPM, "Main Carriage", this));

        if (this.EntityPM.Transshipment1FromPortId != null && this.EntityPM.Transshipment1ToPortId != null) {
            this.ItemsSource.push(new RoutingItem(this.EntityPM, "Transshipment1", this));
        }

        if (this.EntityPM.Transshipment2FromPortId != null && this.EntityPM.Transshipment2ToPortId != null) {
            this.ItemsSource.push(new RoutingItem(this.EntityPM, "Transshipment2", this));
        }

        if (this.EntityPM.Transshipment3FromPortId != null && this.EntityPM.Transshipment3ToPortId != null) {
            this.ItemsSource.push(new RoutingItem(this.EntityPM, "Transshipment3", this));
        }

        if (this.EntityPM.OnCarriageFromPortId != null && this.EntityPM.OnCarriageToPortId != null) {
            this.ItemsSource.push(new RoutingItem(this.EntityPM, "On Carriage", this));
        }

        // WarehouseLeg_Deliveries
        if (this.EntityPM.DirectionId == "I") {
            if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H") {
                this.ItemsSource.push(new RoutingItem(this.EntityPM, "WarehouseLeg", this));
            }
        }

        var allDeliveries = this.EntityPM.ShipmentDeliveries.filter(f => f.PickUpDeliveryTypeCode == "DELV").sort(function (a, b) { return a.PickUpDeliveryNumber.toLowerCase() == b.PickUpDeliveryNumber.toLowerCase() ? 0 : a.PickUpDeliveryNumber.toLowerCase() < b.PickUpDeliveryNumber.toLowerCase() ? -1 : 1; });
        var allEmptyContainerReturns = this.EntityPM.ShipmentDeliveries.filter(f => f.PickUpDeliveryTypeCode == "EMPT").sort(function (a, b) { return a.PickUpDeliveryNumber.toLowerCase() == b.PickUpDeliveryNumber.toLowerCase() ? 0 : a.PickUpDeliveryNumber.toLowerCase() < b.PickUpDeliveryNumber.toLowerCase() ? -1 : 1; });

        if (allDeliveries.length == 0) {
            var newDelivery: ShipmentDeliveryPM = new ShipmentDeliveryPM(null);
            newDelivery.ShipmentId = this.EntityPM.Id;
            newDelivery.ShipmentNumber = this.EntityPM.ShipmentNumber;
            newDelivery.Tenant = this.EntityPM.Tenant;
            this.ItemsSource.push(new RoutingItem(newDelivery, "Delivery", this));
        }

        else {
            allDeliveries.forEach((item) => {
                this.ItemsSource.push(new RoutingItem(item, "Delivery", this));
            });
        }

        allEmptyContainerReturns.forEach((item) => {
            this.ItemsSource.push(new RoutingItem(item, "EmptyCR", this));
        });

        this.SetAddButtonsIsDisabled();
    }
    AddLeg(myLegType: string) {
        var isNewEntity = true;
        var windowTitle = "Add " + myLegType;

        switch (myLegType) {
            case "Pick Up": {
                var myPickUpIndex = 1;
                if (this.EntityPM.ShipmentPickUpIndex) {
                    myPickUpIndex = this.EntityPM.ShipmentPickUpIndex + 1;
                }

                var newPickupPM = new ShipmentPickUpPM(null);
                newPickupPM.FullResponsibility = true;
                newPickupPM.Tenant = this.EntityPM.Tenant;
                newPickupPM.ShipmentId = this.EntityPM.Id;
                newPickupPM.ShipmentNumber = this.EntityPM.ShipmentNumber;
                newPickupPM.PickUpDeliveryNumber = this.EntityPM.ShipmentNumber + "/" + myPickUpIndex;
                newPickupPM.PickUpDeliveryTypeCode = "PICK";
                newPickupPM.PickUpDeliveryFromTypeCode = "PART";
                newPickupPM.PickUpDeliveryToTypeCode = "PORT";
                newPickupPM.TransportModeCode = "BYTR";

                var logitudeWindow = new LogitudeWindow();
                logitudeWindow.Title = TextCodeTranslator.Translate("Shipment.O.Routings.AddPickup");
                logitudeWindow.WindowArgs = { ShipmentPM: this.EntityPM, EntityPM: newPickupPM, IsNewEntity: true };
                logitudeWindow.Width = 950;
                logitudeWindow.Height = 595;
                logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditPickupComponent');

                logitudeWindow.WindowClosed.subscribe(s => {
                    this.BuildItemsCollection();
                });

                break;
            }

            case "Delivery": {

                var myDeliveryIndex = 1;
                if (this.EntityPM.ShipmentDeliveryIndex) {
                    myDeliveryIndex = this.EntityPM.ShipmentDeliveryIndex + 1;
                }

                var newDeliveryPM = new ShipmentDeliveryPM(null);
                newDeliveryPM.FullResponsibility = true;
                newDeliveryPM.Tenant = this.EntityPM.Tenant;
                newDeliveryPM.ShipmentId = this.EntityPM.Id;
                newDeliveryPM.ShipmentNumber = this.EntityPM.ShipmentNumber;
                newDeliveryPM.PickUpDeliveryNumber = this.EntityPM.ShipmentNumber + "/" + myDeliveryIndex;
                newDeliveryPM.PickUpDeliveryTypeCode = "DELV";
                newDeliveryPM.TransportModeCode = "BYTR";

                var logitudeWindow = new LogitudeWindow();
                logitudeWindow.Title = TextCodeTranslator.Translate("Shipment.O.Routings.AddDelivery");
                logitudeWindow.WindowArgs = { ShipmentPM: this.EntityPM, EntityPM: newDeliveryPM, IsNewEntity: true };
                logitudeWindow.Width = 950;
                logitudeWindow.Height = 595;
                logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditDeliveryComponent');

                logitudeWindow.WindowClosed.subscribe(s => {
                    this.BuildItemsCollection();
                });

                break;
            }

            case "Pre Carriage":
                {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Title = TextCodeTranslator.Translate("Shipment.O.Routings.AddPreCarriage");
                    logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, ObjectTableName: this.ObjectTableName, FatherComponent: this }
                    logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditPreCarriageComponent');
                    break;
                }

            case "On Carriage":
                {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Title = TextCodeTranslator.Translate("Shipment.O.Routings.AddOnCarriage");
                    logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, ObjectTableName: this.ObjectTableName, FatherComponent: this }
                    logitudeWindow.Height = 580;
                    logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditOnCarriageComponent');
                    break;
                }

            case "WarehouseLeg":
            case "WarehouseLeg_Pickups":
                {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Width = 900;
                    logitudeWindow.Height = 500;
                    logitudeWindow.Title = TextCodeTranslator.Translate("Shipment.O.Routings.AddWarehouseLeg");
                    logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, ObjectTableName: this.ObjectTableName, FatherComponent: this, LegType: myLegType, IsNewLeg: true }
                    logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditWarehouseLegComponent');
                    break;
                }

            default: {
                if (this.EntityPM.ShipmentLevelCode == "H" && AppTool.IsNullOrEmpty(this.EntityPM.MasterShipmentDataId)) {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Title = TextCodeTranslator.Translate("Shipment.O.Routings.EditMainCarriagePorts");
                    logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, ObjectTableName: this.ObjectTableName, FatherComponent: this }
                    logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditHouseRoutingComponent');
                }

                else {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.IsFillScreen = true;
                    logitudeWindow.Title = TextCodeTranslator.Translate("Shipment.O.Routings.AddEditMainCarriageLegs");
                    logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, ObjectTableName: this.ObjectTableName, FatherComponent: this }
                    logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditMainCarriageComponent');
                }

                break;
            }
        }
    }
    EditLeg(myRoutingItem: RoutingItem) {
        var myLegType: string = myRoutingItem.LegType;

        switch (myLegType) {
            case "Pick Up": {

                var windowTitle = TextCodeTranslator.Translate("Shipment.O.Routings.EditPickup");
                if (!AppTool.IsNullOrEmpty(myRoutingItem.LegName)) {
                    if (myRoutingItem.LegName.indexOf(':') > -1) {
                        windowTitle = windowTitle + ": " + myRoutingItem.LegName.split(':')[1];
                    }
                }

                var logitudeWindow = new LogitudeWindow();
                logitudeWindow.Title = windowTitle;
                logitudeWindow.WindowArgs = { ShipmentPM: this.EntityPM, EntityPM: myRoutingItem.Pickup, IsNewEntity: false };
                logitudeWindow.Width = 950;
                logitudeWindow.Height = 595;
                logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditPickupComponent');

                logitudeWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        this.BuildItemsCollection();
                    }
                });

                break;
            }

            case "Delivery": {

                var windowTitle = TextCodeTranslator.Translate("Shipment.O.Routings.EditDelivery");
                if (!AppTool.IsNullOrEmpty(myRoutingItem.LegName)) {
                    if (myRoutingItem.LegName.indexOf(':') > -1) {
                        windowTitle = windowTitle + ": " + myRoutingItem.LegName.split(':')[1];
                    }
                }

                var logitudeWindow = new LogitudeWindow();
                logitudeWindow.Title = windowTitle;
                logitudeWindow.WindowArgs = { ShipmentPM: this.EntityPM, EntityPM: myRoutingItem.Delivery, IsNewEntity: false };
                logitudeWindow.Width = 950;
                logitudeWindow.Height = 595;
                logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditDeliveryComponent');

                logitudeWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        this.BuildItemsCollection();
                    }
                });

                break;
            }

            case "EmptyCR": {

                var windowTitle = TextCodeTranslator.Translate("Shipment.O.Routings.EditEmptyCR");
                if (!AppTool.IsNullOrEmpty(myRoutingItem.LegName)) {
                    if (myRoutingItem.LegName.indexOf(':') > -1) {
                        windowTitle = windowTitle + ": " + myRoutingItem.LegName.split(':')[1];
                    }
                }

                var logitudeWindow = new LogitudeWindow();
                logitudeWindow.Title = windowTitle;
                logitudeWindow.WindowArgs = { ShipmentPM: this.EntityPM, EntityPM: myRoutingItem.Delivery, IsNewEntity: false };
                logitudeWindow.Width = 950;
                logitudeWindow.Height = 595;
                logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditDeliveryComponent');

                logitudeWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        this.BuildItemsCollection();
                    }
                });

                break;
            }

            case "Pre Carriage":
                {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Title = TextCodeTranslator.Translate("Shipment.O.Routings.EditPreCarriage");
                    logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, ObjectTableName: this.ObjectTableName, FatherComponent: this }
                    logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditPreCarriageComponent');
                    break;
                }

            case "On Carriage":
                {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Title = TextCodeTranslator.Translate("Shipment.O.Routings.EditOnCarriage");
                    logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, ObjectTableName: this.ObjectTableName, FatherComponent: this }
                    logitudeWindow.Height = 580;
                    logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditOnCarriageComponent');
                    break;
                }

            case "WarehouseLeg":
            case "WarehouseLeg_Pickups":
                {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Width = 900;
                    logitudeWindow.Height = 500;
                    logitudeWindow.Title = TextCodeTranslator.Translate("Shipment.O.Routings.EditWarehouseLeg");
                    logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, ObjectTableName: this.ObjectTableName, FatherComponent: this, LegType: myLegType}
                    logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditWarehouseLegComponent');
                    break;
                }
            default: {
                if (this.EntityPM.ShipmentLevelCode == "H" && AppTool.IsNullOrEmpty(this.EntityPM.MasterShipmentDataId)) {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Title = TextCodeTranslator.Translate("Shipment.O.Routings.EditMainCarriagePorts");
                    logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, ObjectTableName: this.ObjectTableName, FatherComponent: this }
                    logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditHouseRoutingComponent');
                }

                else {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.IsFillScreen = true;
                    logitudeWindow.Title = TextCodeTranslator.Translate("Shipment.O.Routings.AddEditMainCarriageLegs");
                    logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, ObjectTableName: this.ObjectTableName, FatherComponent: this }
                    logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditMainCarriageComponent');
                }

                break;
            }
        }
    }
    DeleteLeg(myRoutingItem: RoutingItem) {
        if (myRoutingItem) {

            var message: string = null;
            var myLegType: string = myRoutingItem.LegType;           

            switch (myLegType) {
                case "Pick Up": {
                    message = TextCodeTranslator.Translate("Shipment.M.DeleteThisPickup");
                    break;
                }

                case "Pre Carriage": {
                    message = TextCodeTranslator.Translate("Shipment.M.DeleteThisPreCarriage");
                    break;
                }

                case "On Carriage": {
                    message = TextCodeTranslator.Translate("Shipment.M.DeleteThisOnCarriage");
                    break;
                }

                case "Delivery": {
                    message = TextCodeTranslator.Translate("Shipment.M.DeleteThisDelivery");
                    break;
                }

                case "WarehouseLeg":
                case "WarehouseLeg_Pickups": {
                    message = "Delete Warehouse \ Terminal?";
                    break;
                }
            }

            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show(message);
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {

                    switch (myLegType) {
                        case "Pick Up": {

                            var followups = this.EntityPM.FollowUps.filter(f => f.LegType != null);
                            followups = followups.filter(f => f.LegType.indexOf(myRoutingItem.Pickup.PickUpDeliveryNumber) > -1);
                            if (followups.length > 0) {
                                followups.forEach(item => {
                                    this.EntityPM.RemoveShipmentFollowUp(item);
                                });

                                this.CurrentSession.FireEvent("FollowupsChanged");
                            }

                            this.EntityPM.RemovePickUp(myRoutingItem.Pickup);
                            break;
                        }

                        case "Pre Carriage": {
                            RoutingHelper.RemovePreCarriageLeg(this.EntityPM);
                            this.SetAddButtonsIsDisabled();
                            break;
                        }

                        case "On Carriage": {
                            RoutingHelper.RemoveOnCarriageLeg(this.EntityPM);
                            this.SetAddButtonsIsDisabled();
                            break;
                        }

                        case "Delivery": {

                            var followups = this.EntityPM.FollowUps.filter(f => f.LegType != null);
                            followups = followups.filter(f => f.LegType.indexOf(myRoutingItem.Delivery.PickUpDeliveryNumber) > -1);
                            if (followups.length > 0) {
                                followups.forEach(item => {
                                    this.EntityPM.RemoveShipmentFollowUp(item);
                                });

                                this.CurrentSession.FireEvent("FollowupsChanged");
                            }

                            this.EntityPM.RemoveDelivery(myRoutingItem.Delivery);
                            break;
                        }

                        case "WarehouseLeg":
                        case "WarehouseLeg_Pickups": {
                            this.EntityPM.WarehouseLegWarehouseId = null;
                            this.EntityPM.WarehouseLegAddressId = null;
                            this.EntityPM.WarehouseLegReference = null;
                            this.EntityPM.WarehouseLegTerminalCode = null;
                            this.EntityPM.WarehouseLegLastFreeDate = null;
                            this.EntityPM.TerminalAvailable = null;
                            this.EntityPM.WarehouseLegCutOffDate = null;
                            this.EntityPM.WarehouseLegRemarks = null;

                            this.EntityPM.WarehouseLegExpectedEntryDate = null;
                            this.EntityPM.WarehouseLegExpectedReleaseDate = null;
                            this.EntityPM.WarehouseLegActualEntryDate = null;
                            this.EntityPM.WarehouseLegActualReleaseDate = null;
                            this.EntityPM.WarehouseLegVGMCutOffDate = null;

                            var followups = this.EntityPM.FollowUps.filter(f => f.LegType != null);
                            followups = followups.filter(f => f.LegType.indexOf("WarehouseLeg") > -1);

                            if (followups.length > 0) {
                                followups.forEach(item => {
                                    this.EntityPM.RemoveShipmentFollowUp(item);
                                });

                                this.CurrentSession.FireEvent("FollowupsChanged");
                            }

                            break;
                        }
                    }

                    this.BuildItemsCollection();           
                }
            });
        }       
    }    

    // Inland Domestic
    private myCardListService: CardListService;
    private myAddressListService: AddressListService;
    InitializePartners() {
        this.myCardListService = new CardListService();
        this.myAddressListService = new AddressListService();
        this.LoadFromAddress();
        this.LoadToAddress();
    }

    SetUIProperties_InlandDomestic() {
        if (this.IsInlandDomestic) {
            this.UIProperties.SetEnabled("MainCarriageFromPartnerId", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("MainCarriageFromAddressId", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("MainCarriageToPartnerId", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("MainCarriageToAddressId", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("MainCarriageCarrierId", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("Driver", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("TruckNumber", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("TrailerNumber", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("Master", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("MainCarriageETD", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("MainCarriageETA", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("MainCarriageATD", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("MainCarriageATA", this.ObjectTableName, this.IsEditingEnabled);
            this.SetUIProperties_ValidateActualDates();
        }
    }
    SetUIProperties_ValidateActualDates() {

        this.UIProperties.SetValidity("MainCarriageATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("MainCarriageATA", this.ObjectTableName, true, null);

        if (!DateTool.IsActualDateValid(this.MainCarriageATD)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.ATD"));
            this.UIProperties.SetValidity("MainCarriageATD", this.ObjectTableName, false, errorMessage);
        }

        if (!DateTool.IsActualDateValid(this.MainCarriageATA)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.ATA"));
            this.UIProperties.SetValidity("MainCarriageATA", this.ObjectTableName, false, errorMessage);
        }
    }

    get MainCarriageFromPartnerId() { return this.EntityPM.MainCarriageFromPartnerId; }
    set MainCarriageFromPartnerId(value: string) {
        if (this.EntityPM.MainCarriageFromPartnerId != value) {
            this.EntityPM.MainCarriageFromPartnerId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.MainCarriageFromAddressId = null;
            }

            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {
                            this.MainCarriageFromAddressId = list.MainAddressId;
                        }
                    }
                });
            }
        }
    }

    get MainCarriageFromAddressId() { return this.EntityPM.MainCarriageFromAddressId; }
    set MainCarriageFromAddressId(value: string) {
        if (this.EntityPM.MainCarriageFromAddressId != value) {
            this.EntityPM.MainCarriageFromAddressId = value;
            this.LoadFromAddress();
        }
    }

    get MainCarriageToPartnerId() { return this.EntityPM.MainCarriageToPartnerId; }
    set MainCarriageToPartnerId(value: string) {
        if (this.EntityPM.MainCarriageToPartnerId != value) {
            this.EntityPM.MainCarriageToPartnerId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.MainCarriageToAddressId = null;
            }

            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {
                            this.MainCarriageToAddressId = list.MainAddressId;
                        }
                    }
                });
            }
        }
    }

    get MainCarriageToAddressId() { return this.EntityPM.MainCarriageToAddressId; }
    set MainCarriageToAddressId(value: string) {
        if (this.EntityPM.MainCarriageToAddressId != value) {
            this.EntityPM.MainCarriageToAddressId = value;
            this.LoadToAddress();
        }
    }

    public ToAddressList: AddressList;
    public FromAddressList: AddressList;
    private LoadToAddress() {
        if (AppTool.IsNullOrEmpty(this.MainCarriageToAddressId)) {
            this.ToAddressList = null;

            if (this.EntityPM.ToCountryId != null) {
                this.EntityPM.ToCountryId = null;
            }

            if (this.EntityPM.ToCountryIsEC != false) {
                this.EntityPM.ToCountryIsEC = false;
            }
        }

        else {
            this.myAddressListService.getSingle(this.MainCarriageToAddressId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: AddressList = myResponse.Result;
                    if (list) {
                        this.ToAddressList = list;

                        if (this.EntityPM.ToCountryId != list.CountryId) {
                            this.EntityPM.ToCountryId = list.CountryId;
                        }

                        if (this.EntityPM.ToCountryIsEC != list.CountryEC) {
                            this.EntityPM.ToCountryIsEC = list.CountryEC;
                        }
                    }
                }
            });
        }
    }
    private LoadFromAddress() {
        if (AppTool.IsNullOrEmpty(this.MainCarriageFromAddressId)) {
            this.FromAddressList = null;

            if (this.EntityPM.FromCountryId != null) {
                this.EntityPM.FromCountryId = null;
            }

            if (this.EntityPM.FromCountryIsEC != false) {
                this.EntityPM.FromCountryIsEC = false;
            }
        }

        else {
            this.myAddressListService.getSingle(this.MainCarriageFromAddressId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: AddressList = myResponse.Result;
                    if (list) {
                        this.FromAddressList = list;

                        if (this.EntityPM.FromCountryId != list.CountryId) {
                            this.EntityPM.FromCountryId = list.CountryId;
                        }

                        if (this.EntityPM.FromCountryIsEC != list.CountryEC) {
                            this.EntityPM.FromCountryIsEC = list.CountryEC;
                        }
                    }
                }
            });
        }
    }

    get MainCarriageCarrierId() { return this.EntityPM.MainCarriageCarrierId; }
    set MainCarriageCarrierId(value: string) {
        if (this.EntityPM.MainCarriageCarrierId != value) {
            this.EntityPM.MainCarriageCarrierId = value;
        }
    }

    get Driver() { return this.EntityPM.Driver; }
    set Driver(value: string) {
        if (this.EntityPM.Driver != value) {
            this.EntityPM.Driver = value;
        }
    }

    get TruckNumber() { return this.EntityPM.TruckNumber; }
    set TruckNumber(value: string) {
        if (this.EntityPM.TruckNumber != value) {
            this.EntityPM.TruckNumber = value;
        }
    }

    get TrailerNumber() { return this.EntityPM.TrailerNumber; }
    set TrailerNumber(value: string) {
        if (this.EntityPM.TrailerNumber != value) {
            this.EntityPM.TrailerNumber = value;
        }
    }

    get Master() { return this.EntityPM.Master; }
    set Master(value: string) {
        if (this.EntityPM.Master != value) {
            this.EntityPM.Master = value;
        }
    }

    get MainCarriageETD() { return this.EntityPM.MainCarriageETD; }
    set MainCarriageETD(value: Date) {
        if (this.EntityPM.MainCarriageETD != value) {
            this.EntityPM.MainCarriageETD = value;
        }
    }

    get MainCarriageETA() { return this.EntityPM.MainCarriageETA; }
    set MainCarriageETA(value: Date) {
        if (this.EntityPM.MainCarriageETA != value) {
            this.EntityPM.MainCarriageETA = value;
        }
    }

    get MainCarriageATD() { return this.EntityPM.MainCarriageATD; }
    set MainCarriageATD(value: Date) {
        if (this.EntityPM.MainCarriageATD != value) {
            this.EntityPM.MainCarriageATD = value;
            this.SetUIProperties_ValidateActualDates();
        }
    }

    get MainCarriageATA() { return this.EntityPM.MainCarriageATA; }
    set MainCarriageATA(value: Date) {
        if (this.EntityPM.MainCarriageATA != value) {
            this.EntityPM.MainCarriageATA = value;
            this.SetUIProperties_ValidateActualDates();
        }
    }

    SetActualDateClicked(fieldName: string) {
        switch (fieldName) {
            case "MainCarriageETD": { this.MainCarriageATD = DateTool.GetDateParts(this.MainCarriageETD).DateObject; break; }
            case "MainCarriageETA": { this.MainCarriageATA = DateTool.GetDateParts(this.MainCarriageETA).DateObject; break; }
        }
    }

    EditAddressClicked(myCode: string) {
        var myAddressId = myCode == "F" ? this.MainCarriageFromAddressId : this.MainCarriageToAddressId;

        if (!AppTool.IsNullOrEmpty(myAddressId)) {
            var logeWindow = new LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Edit Address";
            logeWindow.WindowArgs = { EntityId: myAddressId, PartnerTypeId: this.CardLOVDependencyProperty1 };
            logeWindow.Show("./ShipmentModules/ShipmentTabs/Components/Partners/AddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    if (myCode == "F") {
                        this.LoadFromAddress();
                    }

                    else {
                        this.LoadToAddress();
                    }
                }
            });
        }
    }
    AddAddressClicked(myCode: string) {
        var entityPM: AddressPM = new AddressPM();
        entityPM.Tenant = SessionLocator.Tenant;
        entityPM.AddressTypeId = "O";
        entityPM.CardId = myCode == "F" ? this.MainCarriageFromPartnerId : this.MainCarriageToPartnerId;

        if (entityPM != null) {
            var logeWindow = new LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Add Address";
            logeWindow.WindowArgs = { EntityPM: entityPM, PartnerTypeId: this.CardLOVDependencyProperty1 };
            logeWindow.Show("./ShipmentModules/ShipmentTabs/Components/Partners/AddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    if (myCode == "F") {
                        this.MainCarriageFromAddressId = entityPM.Id;
                    }

                    else {
                        this.MainCarriageToAddressId = entityPM.Id;
                    }
                }
            });
        }
    }

    // Warehouse Leg
    public WarehouseLegTerminalName: string = "";
    private myWarehouseAddressList: AddressList;
    get WarehouseAddressList() { return this.myWarehouseAddressList; }
    set WarehouseAddressList(newValue: AddressList) {
        this.myWarehouseAddressList = newValue;
    }
    GetWarehouseAddress() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.WarehouseLegAddressId)) {
            this.WarehouseLegTerminalName = this.EntityPM.WarehouseLegTerminalName;
            var myService = new AddressListService();
            myService.getSingle(this.EntityPM.WarehouseLegAddressId).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        this.WarehouseAddressList = myResponse.Result;
                    }
                }
            });
        }
    }
}
export class RoutingItem extends BaseComponent {
    public EntityPM: ShipmentPM;
    public Pickup: ShipmentPickUpPM;
    public Delivery: ShipmentDeliveryPM;
    public LegType: string;
    public ObjectTableName: string;
    public PickUpDeliveryNumber: string;
    public FollowupLegTypeDeparture: string;
    public FollowupLegTypeArrival: string;
    public IsWarehouseLeg: boolean = false;
    constructor(entity: any, type: string, private fatherComponent: RoutingsTabComponent) {
        super();

        if (entity instanceof ShipmentPickUpPM) {
            this.Pickup = entity;
            this.EntityPM = fatherComponent.EntityPM;
            this.ObjectTableName = "ShipmentPickUpDelivery";
            this.PickUpDeliveryNumber = this.Pickup.PickUpDeliveryNumber;
            this.FollowupLegTypeDeparture = type + 'Departure' + this.PickUpDeliveryNumber;
            this.FollowupLegTypeArrival = type + 'Arrival' + this.PickUpDeliveryNumber;
        }

        else if (entity instanceof ShipmentDeliveryPM) {
            this.Delivery = entity;
            this.EntityPM = fatherComponent.EntityPM;
            this.ObjectTableName = "ShipmentPickUpDelivery";
            this.PickUpDeliveryNumber = this.Delivery.PickUpDeliveryNumber;
            this.FollowupLegTypeDeparture = type + 'Departure' + this.PickUpDeliveryNumber;
            this.FollowupLegTypeArrival = type + 'Arrival' + this.PickUpDeliveryNumber;
        }

        else {
            this.EntityPM = entity;
            this.ObjectTableName = fatherComponent.ObjectTableName;
           
            if (type == "WarehouseLeg" || type == "WarehouseLeg_Pickups") {
                this.IsWarehouseLeg = true;
                this.FollowupLegTypeDeparture = 'WarehouseLegEntry'
                this.FollowupLegTypeArrival = 'WarehouseLegRelease'
            }

            else {
                this.FollowupLegTypeDeparture = type + 'Departure'
                this.FollowupLegTypeArrival = type + 'Arrival'
            }
        }
        
        this.LegType = type;
        this.SetLegAppearance();
        this.GetLegName();
        this.GetImageSource();
        this.GetFromCountryData();
        this.GetToCountryData();
        this.GetCarrierData();
        this.GetDates();
        this.GetContainersNumbers();
    }

    public LegHeight: number;
    public IsMainLeg: boolean;
    public IsLegExists: boolean;
    public NoLegTextCode: string;
    public IsAddButtonVisible: boolean;
    public IsEditButtonVisible: boolean;
    public IsDeleteButtonVisible: boolean;
    SetLegAppearance() {
        this.LegHeight = 100;
        this.IsMainLeg = false;
        this.IsLegExists = true;
        this.NoLegTextCode = null;
        this.IsAddButtonVisible = false;
        this.IsEditButtonVisible = false;
        this.IsDeleteButtonVisible = false;

        switch (this.LegType) {
            case "Pick Up": {
                if (this.Pickup == null || this.EntityPM.ShipmentPickUps.indexOf(this.Pickup) == -1) {
                    this.LegHeight = 50;
                    this.IsLegExists = false;
                    this.IsAddButtonVisible = true;
                    this.NoLegTextCode = "Shipment.O.Routings.NoPickup";
                }

                break;
            }

            case "WarehouseLeg_Pickups": {
                if (AppTool.IsNullOrEmpty(this.EntityPM.WarehouseLegWarehouseId)) {
                    this.LegHeight = 50;
                    this.IsLegExists = false;
                    this.IsAddButtonVisible = true;
                    this.NoLegTextCode = "Shipment.O.Routings.NoWarehouseTerminal";
                }

                else {
                    this.LegHeight = 130;
                }

                break;
            }

            case "WarehouseLeg": {
                if (AppTool.IsNullOrEmpty(this.EntityPM.WarehouseLegWarehouseId)) {
                    this.LegHeight = 50;
                    this.IsLegExists = false;
                    this.IsAddButtonVisible = true;
                    this.NoLegTextCode = "Shipment.O.Routings.NoWarehouseTerminal";
                }

                else {
                    this.LegHeight = 130;
                }

                break;
            }

            case "Delivery": {
                if (this.Delivery == null || this.EntityPM.ShipmentDeliveries.indexOf(this.Delivery) == -1) {
                    this.LegHeight = 50;
                    this.IsLegExists = false;
                    this.IsAddButtonVisible = true;
                    this.NoLegTextCode = "Shipment.O.Routings.NoDelivery";
                }

                break;
            }
        }

        if (this.IsLegExists) {
            this.IsEditButtonVisible = true;

            switch (this.LegType) {
                case "Main Carriage":
                case "Transshipment1":
                case "Transshipment2":
                case "Transshipment3":
                    {
                        this.IsMainLeg = true;
                        break;
                    }

                case "Delivery": {
                    var isDeleteButtonVisible = true;

                    if (this.Delivery) {
                        if (!AppTool.IsNullOrEmpty(this.Delivery.ConnectedPackageId)) {
                            isDeleteButtonVisible = false;
                        }

                        if (this.Delivery.AllConnectedPackagesId) {
                            if (this.Delivery.AllConnectedPackagesId.length > 0) {
                                isDeleteButtonVisible = false;
                            }
                        }
                    }

                    this.IsDeleteButtonVisible = isDeleteButtonVisible;

                    break;
                }

                case "Pick Up":
                case "On Carriage":
                case "Pre Carriage":
                case "WarehouseLeg":
                case "WarehouseLeg_Pickups":
                    {
                        this.IsDeleteButtonVisible = true;
                        break;
                    }
            }
        }
    }

    public LegName: string;
    public LegTransportModeId: string;
    GetLegName() {

        var myTextCode: string;
        var myLegTransportModeId: string;
        var myExtention: string = "";

        switch (this.LegType) {

            case "Pick Up": {
                myTextCode = "Shipment.O.Routings.Pickup";
                myLegTransportModeId = "I";

                if (!AppTool.IsNullOrEmpty(this.Pickup.PickUpDeliveryNumber)) {
                    myExtention = ":" + this.Pickup.PickUpDeliveryNumber;
                }

                break;
            }

            case "WarehouseLeg_Pickups": {
                myTextCode = "Shipment.O.Routings.WarehouseLeg";
                break;
            }

            case "Pre Carriage": {
                myTextCode = "Shipment.O.Routings.PreCarriage";
                myLegTransportModeId = this.EntityPM.PreCarriageTransportModeId;
                break;
            }

            case "Main Carriage": {
                myTextCode = "Shipment.O.Routings.MainCarriageLeg1";
                myLegTransportModeId = this.EntityPM.TransportModeId;
                break;
            }

            case "Transshipment1": {
                myTextCode = (this.EntityPM.TransportModeId.toUpperCase() == "A") ? "Shipment.O.Routings.MainCarriageLeg2" : "Shipment.O.Routings.Transshipment1";
                myLegTransportModeId = this.EntityPM.TransportModeId;
                break;
            }

            case "Transshipment2": {
                myTextCode = (this.EntityPM.TransportModeId.toUpperCase() == "A") ? "Shipment.O.Routings.MainCarriageLeg3" : "Shipment.O.Routings.Transshipment2";
                myLegTransportModeId = this.EntityPM.TransportModeId;
                break;
            }

            case "Transshipment3": {
                myTextCode = (this.EntityPM.TransportModeId.toUpperCase() == "A") ? "Shipment.O.Routings.MainCarriageLeg4" : "Shipment.O.Routings.Transshipment3";
                myLegTransportModeId = this.EntityPM.TransportModeId;
                break;
            }

            case "On Carriage": {
                myTextCode = "Shipment.O.Routings.OnCarriage";
                myLegTransportModeId = this.EntityPM.OnCarriageTransportModeId;
                break;
            }

            case "WarehouseLeg": {
                myTextCode = "Shipment.O.Routings.WarehouseLeg";
                break;
            }

            case "Delivery": {
                myTextCode = "Shipment.O.Routings.Delivery";
                myLegTransportModeId = "I";

                if (!AppTool.IsNullOrEmpty(this.Delivery.PickUpDeliveryNumber)) {
                    myExtention = ":" + this.Delivery.PickUpDeliveryNumber;
                }

                break;
            }

            case "EmptyCR": {
                myTextCode = "Shipment.O.Routings.EmptyCR";
                myLegTransportModeId = "I";

                if (!AppTool.IsNullOrEmpty(this.Delivery.PickUpDeliveryNumber)) {
                    myExtention = ":" + this.Delivery.PickUpDeliveryNumber;
                }

                break;
            }
        }

        this.LegName = TextCodeTranslator.Translate(myTextCode) + myExtention;
        this.LegTransportModeId = myLegTransportModeId;
    }

    public ImageSource: string;
    GetImageSource() {

        switch (this.LegTransportModeId) {
            case "A": {
                this.ImageSource = 'Images/Airline.png';
                break;
            }

            case "O": {
                this.ImageSource = 'Images/Vessel.png';
                break;
            }

            case "I": {
                this.ImageSource = 'Images/Trucker.png';
                break;
            }
        }
    }

    public FromCountryCode: string;
    public FromCountryName: string;
    public FromCountryText: string;
    GetFromCountryData() {

        var myCountryCode: string = "";
        var myCountryName: string = "";

        var myTextPart1: string = "";
        var myTextPart2: string = "";
        var myTextParts: string = "";

        switch (this.LegType) {

            case "Pick Up": {
                if (this.Pickup != null) {

                    if (this.Pickup.FromPortId != null) {
                        myCountryCode = this.Pickup.FromPortCountryCode;
                        myCountryName = this.Pickup.FromPortCountryName;
                    }

                    else {
                        myCountryCode = this.Pickup.FromAddressCountryCode;
                        myCountryName = this.Pickup.FromAddressCountryName;
                    }

                    switch (this.Pickup.PickUpDeliveryFromTypeCode) {

                        case "PORT": {
                            myTextPart1 = this.Pickup.FromPortCode;
                            myTextPart2 = this.Pickup.FromPortName;
                            break;
                        }

                        case "PART": {

                            if (this.Pickup.FromAddressId != null) {
                                myTextPart1 = this.Pickup.FromAddressCity_Dummy;
                                myTextPart2 = this.Pickup.FromAddressCountryName;
                            }

                            else {
                                myTextPart1 = this.Pickup.FromAddressCountryCode;
                                myTextPart2 = this.Pickup.FromAddressCountryName;
                            }

                            break;
                        }

                        default: {

                            if (this.Pickup.FromAddressCity != null) {
                                myTextPart1 = this.Pickup.FromAddressCity_Dummy;
                                myTextPart2 = this.Pickup.FromAddressCountryName;
                            }

                            else {
                                myTextPart1 = this.Pickup.FromAddressCountryCode;
                                myTextPart2 = this.Pickup.FromAddressCountryName;
                            }

                            break;
                        }
                    }
                }
                break;
            }

            case "Pre Carriage": {
                myCountryCode = this.EntityPM.PreCarriageFromPortCountryCode;
                myCountryName = this.EntityPM.PreCarriageFromPortCountryName;
                myTextPart1 = this.EntityPM.PreCarriageFromPortCode;
                myTextPart2 = this.EntityPM.PreCarriageFromPortName;
                break;
            }

            case "Main Carriage": {
                myCountryCode = this.EntityPM.MainCarriageFromPortCountryCode;
                myCountryName = this.EntityPM.MainCarriageFromPortCountryName;
                myTextPart1 = this.EntityPM.MainCarriageFromPortCode;
                myTextPart2 = this.EntityPM.MainCarriageFromPortName;
                break;
            }

            case "Transshipment1": {
                myCountryCode = this.EntityPM.Transshipment1FromPortCountryCode;
                myCountryName = this.EntityPM.Transshipment1FromPortCountryName;
                myTextPart1 = this.EntityPM.Transshipment1FromPortCode;
                myTextPart2 = this.EntityPM.Transshipment1FromPortName;
                break;
            }

            case "Transshipment2": {
                myCountryCode = this.EntityPM.Transshipment2FromPortCountryCode;
                myCountryName = this.EntityPM.Transshipment2FromPortCountryName;
                myTextPart1 = this.EntityPM.Transshipment2FromPortCode;
                myTextPart2 = this.EntityPM.Transshipment2FromPortName;
                break;
            }

            case "Transshipment3": {
                myCountryCode = this.EntityPM.Transshipment3FromPortCountryCode;
                myCountryName = this.EntityPM.Transshipment3FromPortCountryName;
                myTextPart1 = this.EntityPM.Transshipment3FromPortCode;
                myTextPart2 = this.EntityPM.Transshipment3FromPortName;
                break;
            }

            case "On Carriage": {
                myCountryCode = this.EntityPM.OnCarriageFromPortCountryCode;
                myCountryName = this.EntityPM.OnCarriageFromPortCountryName;
                myTextPart1 = this.EntityPM.OnCarriageFromPortCode;
                myTextPart2 = this.EntityPM.OnCarriageFromPortName;
                break;
            }

            case "Delivery":
            case "EmptyCR": {
                if (this.Delivery != null) {

                    if (this.Delivery.FromPortId != null) {
                        myCountryCode = this.Delivery.FromPortCountryCode;
                        myCountryName = this.Delivery.FromPortCountryName;
                    }

                    else {
                        myCountryCode = this.Delivery.FromAddressCountryCode;
                        myCountryName = this.Delivery.FromAddressCountryName;
                    }

                    switch (this.Delivery.PickUpDeliveryFromTypeCode) {

                        case "PORT": {
                            myTextPart1 = this.Delivery.FromPortCode;
                            myTextPart2 = this.Delivery.FromPortName;
                            break;
                        }

                        case "PART": {

                            if (this.Delivery.FromAddressId != null) {
                                myTextPart1 = this.Delivery.FromAddressCity_Dummy;
                                myTextPart2 = this.Delivery.FromAddressCountryName;
                            }

                            else {
                                myTextPart1 = this.Delivery.FromAddressCountryCode;
                                myTextPart2 = this.Delivery.FromAddressCountryName;
                            }

                            break;
                        }

                        default: {

                            if (this.Delivery.FromAddressCity != null) {
                                myTextPart1 = this.Delivery.FromAddressCity_Dummy;
                                myTextPart2 = this.Delivery.FromAddressCountryName;
                            }

                            else {
                                myTextPart1 = this.Delivery.FromAddressCountryCode;
                                myTextPart2 = this.Delivery.FromAddressCountryName;
                            }

                            break;
                        }
                    }
                }
                break;
            }
        }

        if (myTextPart1 != null && myTextPart2 != null) {
            myTextParts = myTextPart1 + " " + myTextPart2;
        }

        this.FromCountryCode = myCountryCode == null ? "" : myCountryCode;
        this.FromCountryName = myCountryName == null ? "" : myCountryName;
        this.FromCountryText = myTextParts;
    }

    public ToCountryCode: string;
    public ToCountryName: string;
    public ToCountryText: string;
    GetToCountryData() {

        var myCountryCode: string = "";
        var myCountryName: string = "";

        var myTextPart1: string = "";
        var myTextPart2: string = "";
        var myTextParts: string = "";

        switch (this.LegType) {

            case "Pick Up": {
                if (this.Pickup != null) {

                    if (this.Pickup.ToPortId != null) {
                        myCountryCode = this.Pickup.ToPortCountryCode;
                        myCountryName = this.Pickup.ToPortCountryName;
                    }

                    else {
                        myCountryCode = this.Pickup.ToAddressCountryCode;
                        myCountryName = this.Pickup.ToAddressCountryName;
                    }

                    switch (this.Pickup.PickUpDeliveryToTypeCode) {

                        case "PORT": {
                            myTextPart1 = this.Pickup.ToPortCode;
                            myTextPart2 = this.Pickup.ToPortName;
                            break;
                        }

                        case "PART": {

                            if (this.Pickup.ToAddressId != null) {
                                myTextPart1 = this.Pickup.ToAddressCity_Dummy;
                                myTextPart2 = this.Pickup.ToAddressCountryName;
                            }

                            else {
                                myTextPart1 = this.Pickup.ToAddressCountryCode;
                                myTextPart2 = this.Pickup.ToAddressCountryName;
                            }

                            break;
                        }

                        default: {

                            if (this.Pickup.ToAddressCity != null) {
                                myTextPart1 = this.Pickup.ToAddressCity_Dummy;
                                myTextPart2 = this.Pickup.ToAddressCountryName;
                            }

                            else {
                                myTextPart1 = this.Pickup.ToAddressCountryCode;
                                myTextPart2 = this.Pickup.ToAddressCountryName;
                            }

                            break;
                        }
                    }
                }
                break;
            }

            case "Pre Carriage": {
                myCountryCode = this.EntityPM.PreCarriageToPortCountryCode;
                myCountryName = this.EntityPM.PreCarriageToPortCountryName;
                myTextPart1 = this.EntityPM.PreCarriageToPortCode;
                myTextPart2 = this.EntityPM.PreCarriageToPortName;
                break;
            }

            case "Main Carriage": {
                myCountryCode = this.EntityPM.MainCarriageToPortCountryCode;
                myCountryName = this.EntityPM.MainCarriageToPortCountryName;
                myTextPart1 = this.EntityPM.MainCarriageToPortCode;
                myTextPart2 = this.EntityPM.MainCarriageToPortName;
                break;
            }

            case "Transshipment1": {
                myCountryCode = this.EntityPM.Transshipment1ToPortCountryCode;
                myCountryName = this.EntityPM.Transshipment1ToPortCountryName;
                myTextPart1 = this.EntityPM.Transshipment1ToPortCode;
                myTextPart2 = this.EntityPM.Transshipment1ToPortName;
                break;
            }

            case "Transshipment2": {
                myCountryCode = this.EntityPM.Transshipment2ToPortCountryCode;
                myCountryName = this.EntityPM.Transshipment2ToPortCountryName;
                myTextPart1 = this.EntityPM.Transshipment2ToPortCode;
                myTextPart2 = this.EntityPM.Transshipment2ToPortName;
                break;
            }

            case "Transshipment3": {
                myCountryCode = this.EntityPM.Transshipment3ToPortCountryCode;
                myCountryName = this.EntityPM.Transshipment3ToPortCountryName;
                myTextPart1 = this.EntityPM.Transshipment3ToPortCode;
                myTextPart2 = this.EntityPM.Transshipment3ToPortName;
                break;
            }

            case "On Carriage": {
                myCountryCode = this.EntityPM.OnCarriageToPortCountryCode;
                myCountryName = this.EntityPM.OnCarriageToPortCountryName;
                myTextPart1 = this.EntityPM.OnCarriageToPortCode;
                myTextPart2 = this.EntityPM.OnCarriageToPortName;
                break;
            }

            case "Delivery":
            case "EmptyCR": {
                if (this.Delivery != null) {

                    if (this.Delivery.ToPortId != null) {
                        myCountryCode = this.Delivery.ToPortCountryCode;
                        myCountryName = this.Delivery.ToPortCountryName;
                    }

                    else {
                        myCountryCode = this.Delivery.ToAddressCountryCode;
                        myCountryName = this.Delivery.ToAddressCountryName;
                    }

                    switch (this.Delivery.PickUpDeliveryToTypeCode) {

                        case "PORT": {
                            myTextPart1 = this.Delivery.ToPortCode;
                            myTextPart2 = this.Delivery.ToPortName;
                            break;
                        }

                        case "PART": {

                            if (this.Delivery.ToAddressId != null) {
                                myTextPart1 = this.Delivery.ToAddressCity_Dummy;
                                myTextPart2 = this.Delivery.ToAddressCountryName;
                            }

                            else {
                                myTextPart1 = this.Delivery.ToAddressCountryCode;
                                myTextPart2 = this.Delivery.ToAddressCountryName;
                            }

                            break;
                        }

                        default: {

                            if (this.Delivery.ToAddressCity != null) {
                                myTextPart1 = this.Delivery.ToAddressCity_Dummy;
                                myTextPart2 = this.Delivery.ToAddressCountryName;
                            }

                            else {
                                myTextPart1 = this.Delivery.ToAddressCountryCode;
                                myTextPart2 = this.Delivery.ToAddressCountryName;
                            }

                            break;
                        }
                    }
                }
                break;
            }
        }

        if (myTextPart1 != null && myTextPart2 != null) {
            myTextParts = myTextPart1 + " " + myTextPart2;
        }

        this.ToCountryCode = myCountryCode == null ? "" : myCountryCode;
        this.ToCountryName = myCountryName == null ? "" : myCountryName;
        this.ToCountryText = myTextParts;
    }

    public CarrierText: string;
    public CarrierSite: string;
    public IsCarrierTextVisible: boolean = false;
    public IsCarrierSiteVisible: boolean = false;
    public CarrierNumber: string;
    public VesselName: string;
    public IsVesselVisible: boolean = false;
    public IsBookingConfirmationVisible: boolean = false;
    GetCarrierData() {

        var myCarrierCode: string = "";
        var myCarrierName: string = "";
        var myCarrierSite: string = "";
        var myCarrierNumber: string = "";
        var myVesselName: string = "";
        var isBookingnVisible: boolean = false;
        var isVesselVisible: boolean = false;
        var isCarrierSiteVisible: boolean = false;
        var isCarrierTextVisible: boolean = false;

        switch (this.LegType) {
            case "Pick Up": {
                if (this.Pickup != null) {
                    myCarrierCode = this.Pickup.CarrierCode;
                    myCarrierName = this.Pickup.CarrierName;
                    myCarrierSite = this.Pickup.CarrierWebSite;
                    myCarrierNumber = this.Pickup.CarrierNumber;
                }
                break;
            }

            case "Pre Carriage": {
                myCarrierCode = this.EntityPM.PreCarriageCarrierCode;
                myCarrierName = this.EntityPM.PreCarriageCarrierName;
                myCarrierSite = this.EntityPM.PreCarriageCarrierWebSite;
                myCarrierNumber = this.EntityPM.PreCarriageCarrierNumber;
                myVesselName = this.EntityPM.PreCarriageVesselName;

                if (this.EntityPM.PreCarriageTransportModeId == "O") {
                    isVesselVisible = true;
                }

                break;
            }

            case "Main Carriage": {
                myCarrierCode = this.EntityPM.MainCarriageCarrierCode;
                myCarrierName = this.EntityPM.MainCarriageCarrierName;
                myCarrierSite = this.EntityPM.MainCarriageCarrierWebSite;
                myCarrierNumber = this.EntityPM.MainCarriageCarrierNumber;
                myVesselName = this.EntityPM.MainCarriageVesselName;

                if (this.EntityPM.BookingConfirmationNumber != null) {
                    isBookingnVisible = true;
                }

                if (this.EntityPM.TransportModeId == "O") {
                    isVesselVisible = true;
                }

                break;
            }

            case "Transshipment1": {
                myCarrierCode = this.EntityPM.Transshipment1CarrierCode;
                myCarrierName = this.EntityPM.Transshipment1CarrierName;
                myCarrierSite = this.EntityPM.Transshipment1CarrierWebSite;
                myCarrierNumber = this.EntityPM.Transshipment1CarrierNumber;
                myVesselName = this.EntityPM.Transshipment1VesselName;

                if (this.EntityPM.TransportModeId == "O") {
                    isVesselVisible = true;
                }

                break;
            }

            case "Transshipment2": {
                myCarrierCode = this.EntityPM.Transshipment2CarrierCode;
                myCarrierName = this.EntityPM.Transshipment2CarrierName;
                myCarrierSite = this.EntityPM.Transshipment2CarrierWebSite;
                myCarrierNumber = this.EntityPM.Transshipment2CarrierNumber;
                myVesselName = this.EntityPM.Transshipment2VesselName;

                if (this.EntityPM.TransportModeId == "O") {
                    isVesselVisible = true;
                }

                break;
            }

            case "Transshipment3": {
                myCarrierCode = this.EntityPM.Transshipment3CarrierCode;
                myCarrierName = this.EntityPM.Transshipment3CarrierName;
                myCarrierSite = this.EntityPM.Transshipment3CarrierWebSite;
                myCarrierNumber = this.EntityPM.Transshipment3CarrierNumber;
                myVesselName = this.EntityPM.Transshipment3VesselName;

                if (this.EntityPM.TransportModeId == "O") {
                    isVesselVisible = true;
                }

                break;
            }

            case "On Carriage": {
                myCarrierCode = this.EntityPM.OnCarriageCarrierCode;
                myCarrierName = this.EntityPM.OnCarriageCarrierName;
                myCarrierSite = this.EntityPM.OnCarriageCarrierWebSite;
                myCarrierNumber = this.EntityPM.OnCarriageCarrierNumber;
                myVesselName = this.EntityPM.OnCarriageVesselName;

                if (this.EntityPM.OnCarriageTransportModeId == "O") {
                    isVesselVisible = true;
                }

                break;
            }

            case "Delivery":
            case "EmptyCR":
                {
                    if (this.Delivery != null) {
                        myCarrierCode = this.Delivery.CarrierCode;
                        myCarrierName = this.Delivery.CarrierName;
                        myCarrierSite = this.Delivery.CarrierWebSite;
                        myCarrierNumber = this.Delivery.CarrierNumber;
                    }

                    break;
                }
        }

        // remove undefined text
        if (AppTool.IsNullOrEmpty(myCarrierCode)) {
            myCarrierCode = "";
        }

        if (AppTool.IsNullOrEmpty(myCarrierName)) {
            myCarrierName = "";
        }

        if (AppTool.IsNullOrEmpty(myCarrierNumber)) {
            myCarrierNumber = "";
        }

        if (AppTool.IsNullOrEmpty(myCarrierSite)) {
            myCarrierSite = "";
        }

        if (AppTool.IsNullOrEmpty(myVesselName)) {
            myVesselName = "";
        }

        if (!AppTool.IsNullOrEmpty(myCarrierCode) || !AppTool.IsNullOrEmpty(myCarrierName)) {
            if (!AppTool.IsNullOrEmpty(myCarrierSite)) {
                isCarrierSiteVisible = true;

                if (myCarrierSite.indexOf('http://') == -1) {
                    myCarrierSite = "http://" + myCarrierSite;
                }
            }

            else {
                isCarrierTextVisible = true;
            }
        }

        this.CarrierSite = myCarrierSite;
        this.CarrierText = myCarrierCode + " " + myCarrierName;
        this.CarrierNumber = myCarrierNumber;
        this.VesselName = myVesselName;

        this.IsCarrierSiteVisible = isCarrierSiteVisible;
        this.IsCarrierTextVisible = isCarrierTextVisible;
        this.IsVesselVisible = isVesselVisible;
        this.IsBookingConfirmationVisible = isBookingnVisible;
    }

    public DepartureDate: Date;
    public DepartureColor: string;
    public ArrivalDate: Date;
    public ArrivalColor: string;
    public IsSetActualDepartureVisible: boolean = false;
    public IsSetActualArrivalVisible: boolean = false;
    GetDates() {

        var myETD: Date;
        var myATD: Date;
        var myETA: Date;
        var myATA: Date;
        var myDepartureDate: Date;
        var myArrivalDate: Date;
        var myDepartureColor: string = FontTool.Orange;
        var myArrivalColor: string = FontTool.Orange;
        this.IsSetActualDepartureVisible = false;
        this.IsSetActualArrivalVisible = false;

        switch (this.LegType) {

            case "Pick Up": {
                if (this.Pickup != null) {
                    myETD = this.Pickup.ETD;
                    myATD = this.Pickup.ATD;
                    myETA = this.Pickup.ETA;
                    myATA = this.Pickup.ATA;
                }

                break;
            }

            case "Pre Carriage": {
                myETD = this.EntityPM.PreCarriageETD;
                myATD = this.EntityPM.PreCarriageATD;
                myETA = this.EntityPM.PreCarriageETA;
                myATA = this.EntityPM.PreCarriageATA;
                break;
            }

            case "Main Carriage": {
                myETD = this.EntityPM.MainCarriageETD;
                myATD = this.EntityPM.MainCarriageATD;
                myETA = this.EntityPM.MainCarriageETA;
                myATA = this.EntityPM.MainCarriageATA;
                break;
            }

            case "Transshipment1": {
                myETD = this.EntityPM.Transshipment1ETD;
                myATD = this.EntityPM.Transshipment1ATD;
                myETA = this.EntityPM.Transshipment1ETA;
                myATA = this.EntityPM.Transshipment1ATA;
                break;
            }

            case "Transshipment2": {
                myETD = this.EntityPM.Transshipment2ETD;
                myATD = this.EntityPM.Transshipment2ATD;
                myETA = this.EntityPM.Transshipment2ETA;
                myATA = this.EntityPM.Transshipment2ATA;
                break;
            }

            case "Transshipment3": {
                myETD = this.EntityPM.Transshipment3ETD;
                myATD = this.EntityPM.Transshipment3ATD;
                myETA = this.EntityPM.Transshipment3ETA;
                myATA = this.EntityPM.Transshipment3ATA;
                break;
            }

            case "On Carriage": {
                myETD = this.EntityPM.OnCarriageETD;
                myATD = this.EntityPM.OnCarriageATD;
                myETA = this.EntityPM.OnCarriageETA;
                myATA = this.EntityPM.OnCarriageATA;
                break;
            }

            case "WarehouseLeg":
            case "WarehouseLeg_Pickups": {
                myETD = this.EntityPM.WarehouseLegExpectedEntryDate;
                myATD = this.EntityPM.WarehouseLegActualEntryDate;
                myETA = this.EntityPM.WarehouseLegExpectedReleaseDate;
                myATA = this.EntityPM.WarehouseLegActualReleaseDate;
                break;
            }

            case "Delivery":
            case "EmptyCR": {
                if (this.Delivery != null) {
                    myETD = this.Delivery.ETD;
                    myATD = this.Delivery.ATD;
                    myETA = this.Delivery.ETA;
                    myATA = this.Delivery.ATA;
                }

                break;
            }
        }

        var todayDate: Date = DateTool.GetCurrentDateAsUtc();

        if (myATD != null) {
            myDepartureDate = myATD;
            myDepartureColor = FontTool.Black;
        }

        else if (myETD != null) {
            myDepartureDate = myETD;

            if (DateTool.GetDateParts(myDepartureDate).DateTicks < DateTool.GetDateParts(todayDate).DateTicks) {
                myDepartureColor = FontTool.Red;
            }

            switch (this.LegType) {
                case "Main Carriage":
                case "Transshipment1":
                case "Transshipment2":
                case "Transshipment3":
                    {
                        if (this.EntityPM.ShipmentLevelCode != "H") {
                            this.IsSetActualDepartureVisible = true;
                        }

                        break;
                    }

                default: {
                    this.IsSetActualDepartureVisible = true;
                    break;
                }
            }
        }

        if (myATA != null) {
            myArrivalDate = myATA;
            myArrivalColor = FontTool.Black;
        }

        else if (myETA != null) {
            myArrivalDate = myETA;

            if (DateTool.GetDateParts(myArrivalDate).DateTicks < DateTool.GetDateParts(todayDate).DateTicks) {
                myArrivalColor = FontTool.Red;
            }


            switch (this.LegType) {
                case "Main Carriage":
                case "Transshipment1":
                case "Transshipment2":
                case "Transshipment3":
                    {
                        if (this.EntityPM.ShipmentLevelCode != "H") {
                            this.IsSetActualArrivalVisible = true;
                        }

                        break;
                    }

                default: {
                    this.IsSetActualArrivalVisible = true;
                    break;
                }
            }
        }

        this.DepartureDate = myDepartureDate;
        this.DepartureColor = myDepartureColor;
        this.ArrivalDate = myArrivalDate;
        this.ArrivalColor = myArrivalColor;
    }

    SetActualDepartureClicked() {
        switch (this.LegType) {
            case "Pick Up": {
                this.Pickup.ATD = this.Pickup.ETD;
                break;
            }

            case "Pre Carriage": {
                this.EntityPM.PreCarriageATD = this.EntityPM.PreCarriageETD;
                break;
            }

            case "Main Carriage": {
                this.EntityPM.MainCarriageATD = this.EntityPM.MainCarriageETD;
                break;
            }

            case "Transshipment1": {
                this.EntityPM.Transshipment1ATD = this.EntityPM.Transshipment1ETD;
                break;
            }

            case "Transshipment2": {
                this.EntityPM.Transshipment2ATD = this.EntityPM.Transshipment2ETD;
                break;
            }

            case "Transshipment3": {
                this.EntityPM.Transshipment3ATD = this.EntityPM.Transshipment3ETD;
                break;
            }

            case "On Carriage": {
                this.EntityPM.OnCarriageATD = this.EntityPM.OnCarriageETD;
                break;
            }

            case "WarehouseLeg":
            case "WarehouseLeg_Pickups":
                {
                    this.EntityPM.WarehouseLegActualEntryDate = this.EntityPM.WarehouseLegExpectedEntryDate;
                    break;
                }

            case "EmptyCR":
            case "Delivery": {
                this.Delivery.ATD = this.Delivery.ETD;
                break;
            }
        }

        this.GetDates();
    }
    SetActualArrivalClicked() {
        switch (this.LegType) {
            case "Pick Up": {
                this.Pickup.ATA = this.Pickup.ETA;
                break;
            }

            case "Pre Carriage": {
                this.EntityPM.PreCarriageATA = this.EntityPM.PreCarriageETA;
                break;
            }

            case "Main Carriage": {
                this.EntityPM.MainCarriageATA = this.EntityPM.MainCarriageETA;
                break;
            }

            case "Transshipment1": {
                this.EntityPM.Transshipment1ATA = this.EntityPM.Transshipment1ETA;
                break;
            }

            case "Transshipment2": {
                this.EntityPM.Transshipment2ATA = this.EntityPM.Transshipment2ETA;
                break;
            }

            case "Transshipment3": {
                this.EntityPM.Transshipment3ATA = this.EntityPM.Transshipment3ETA;
                break;
            }

            case "On Carriage": {
                this.EntityPM.OnCarriageATA = this.EntityPM.OnCarriageETA;
                break;
            }

            case "WarehouseLeg":
            case "WarehouseLeg_Pickups":
                {
                    this.EntityPM.WarehouseLegActualReleaseDate = this.EntityPM.WarehouseLegExpectedReleaseDate;
                    break;
                }

            case "EmptyCR":
            case "Delivery": {
                this.Delivery.ATA = this.Delivery.ETA;
                break;
            }
        }

        this.GetDates();
    }

    // Warehouse Leg Fields 
    get WarehouseLegWarehouseId() { return this.EntityPM.WarehouseLegWarehouseId; }
    set WarehouseLegWarehouseId(value: string) {
        if (this.EntityPM.WarehouseLegWarehouseId != value) {
            this.EntityPM.WarehouseLegWarehouseId = value;
        }
    }

    get WarehouseLegReference() { return this.EntityPM.WarehouseLegReference; }
    set WarehouseLegReference(value: string) {
        if (this.EntityPM.WarehouseLegReference != value) {
            this.EntityPM.WarehouseLegReference = value;
        }
    } 

    get WarehouseLegTerminalCode() { return this.EntityPM.WarehouseLegTerminalCode; }
    set WarehouseLegTerminalCode(value: string) {
        if (this.EntityPM.WarehouseLegTerminalCode != value) {
            this.EntityPM.WarehouseLegTerminalCode = value;
        }
    }

    get WarehouseLegLastFreeDate() { return this.EntityPM.WarehouseLegLastFreeDate; }
    set WarehouseLegLastFreeDate(value: Date) {
        if (this.EntityPM.WarehouseLegLastFreeDate != value) {
            this.EntityPM.WarehouseLegLastFreeDate = value;
        }
    }

    get TerminalAvailable() { return this.EntityPM.TerminalAvailable; }
    set TerminalAvailable(value: Date) {
        if (this.EntityPM.TerminalAvailable != value) {
            this.EntityPM.TerminalAvailable = value;
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

    public HasContainers: boolean = false;
    public AllContainers: string = null;
    public DisplayContainers: string = null;
    GetContainersNumbers() {
        this.HasContainers = false;
        this.AllContainers = null;
        this.DisplayContainers = null;
        var AllContainersNumbers: string[] = [];

        if (this.fatherComponent.IsFCLEntity) {

            switch (this.LegType) {            
                case "Pick Up": {
                    if (this.Pickup) {
                        this.Pickup.ShipmentPickUpDeliveryPackages.forEach((item: ShipmentPickUpDeliveryPackagePM) => {
                            if (!AppTool.IsNullOrEmpty(item.ContainerNumber)) {
                                AllContainersNumbers.push(item.ContainerNumber);
                            }
                        });
                    }

                    break;
                }
                case "EmptyCR":
                case "Delivery": {
                    if (this.Delivery) {
                        this.Delivery.ShipmentPickUpDeliveryPackages.forEach((item: ShipmentPickUpDeliveryPackagePM) => {
                            if (!AppTool.IsNullOrEmpty(item.ContainerNumber)) {
                                AllContainersNumbers.push(item.ContainerNumber);
                            }
                        });
                    }

                    break;
                }
                case "On Carriage": {
                    if (this.EntityPM.SplitOnCarriage == true) {
                        this.EntityPM.ShipmentPackages.forEach((item: ShipmentPackagePM) => {
                            if (!AppTool.IsNullOrEmpty(item.ContainerNumber)) {
                                AllContainersNumbers.push(item.ContainerNumber);
                            }
                        });
                    }

                    break;
                }
            }

            if (AllContainersNumbers.length > 0) {

                var count: number = 0;

                AllContainersNumbers.forEach(item => {

                    count++;

                    if (AppTool.IsNullOrEmpty(this.AllContainers)) {
                        this.AllContainers = item;
                        this.DisplayContainers = item;
                    }

                    else {
                        this.AllContainers += "," + item;

                        if (count <= 5) {
                            this.DisplayContainers += "," + item;
                        }
                    }
                });

                this.HasContainers = true;
            }
        }
    }
}


