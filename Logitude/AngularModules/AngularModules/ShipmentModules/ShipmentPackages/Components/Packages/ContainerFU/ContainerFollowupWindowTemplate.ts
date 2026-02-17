import {Component} from '@angular/core';
import {ShipmentPackagePM} from '../../../../../Shipment/EntityPMs/ShipmentPackagePM';
import {PackagesTabComponent, ShipmentPackageItem} from './../PackagesTabComponent';
import {ContainerFollowupWindowComponent} from './ContainerFollowupWindowComponent';
import {AppTool, DateTool} from '../../../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../../../Controls/Windows/MessageWindow';
import {ShipmentTool} from '../../../../../Shipment/Tools';
import {ShipmentDeliveryPM} from '../../../../../Shipment/EntityPMs/ShipmentDeliveryPM';
import {ShipmentPickUpDeliveryPackagePM} from '../../../../../Shipment/EntityPMs/ShipmentPickUpDeliveryPackagePM';

@Component({
    moduleId: module.id,
    templateUrl: './ContainerFollowupWindowTemplate.html',
})

export class ContainerFollowupWindowTemplate {
    public Code: string;
    public EntityPM: ShipmentPackagePM;
    public DataContext: ShipmentPackageItem;
    public TabComponent: PackagesTabComponent;
    public FatherComponent: ContainerFollowupWindowComponent;
    public ObjectTableName: string = "ShipmentPackage";
    public HasRouting: boolean = false;
    public ActionRoutingLinkText: string;
    public DeleteRoutingLinkText: string;
    constructor() {

    }

    public IsDeliveryConnectedWithMultiContainers: boolean = false;

    Run(args: any) {
        this.Code = args['Code'];
        this.DataContext = args['DataContext'];
        this.FatherComponent = args['FatherComponent'];        
        this.EntityPM = this.DataContext.EntityPM;
        this.TabComponent = this.DataContext.fatherComponent;

        if (this.Code == "D") {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.DeliveryId)) {
                var allPackages = this.TabComponent.EntityPM.ShipmentPackages.filter(f => f.DeliveryId == this.EntityPM.DeliveryId);
                if (allPackages.length > 1) {
                    this.IsDeliveryConnectedWithMultiContainers = true;
                }
            }
        }

        this.SetProperties();
    }

    public IsEditingEnabled: boolean;
    public IsFollowupProperty: string;
    public ETDProperty: string;
    public ATDProperty: string;
    public ETAProperty: string;
    public ATAProperty: string;
    public FromProperty: string;
    public ToProperty: string;
    public TransportModeProperty: string;
    SetProperties() {
        this.IsEditingEnabled = ShipmentTool.IsEditingEnabled(this.DataContext.ShipmentPM);

        if (this.IsEditingEnabled) {
            if (!this.IsFollowup) {
                this.IsEditingEnabled = false;
            }

            else if (this.Code == "D") {
                if (!AppTool.IsNullOrEmpty(this.EntityPM.DeliveryId)) {
                    var allPackages = this.TabComponent.EntityPM.ShipmentPackages.filter(f => f.DeliveryId == this.EntityPM.DeliveryId);
                    if (allPackages.length > 1) {
                        this.IsEditingEnabled = false;
                    }
                }
            }
        }

        switch (this.Code) {
            case "D": {
                this.IsFollowupProperty = "IsDeliveryFU";
                this.ETDProperty = "DeliveryETD";
                this.ATDProperty = "DeliveryATD";
                this.ETAProperty = "DeliveryETA";
                this.ATAProperty = "DeliveryATA";
                this.FromProperty = "DeliveryFrom";
                this.ToProperty = "DeliveryTo";
                this.TransportModeProperty = "DeliveryTransportModeCode";
                this.HasRouting = AppTool.IsNullOrEmpty(this.EntityPM.DeliveryId) ? false : true;
                this.ActionRoutingLinkText = this.HasRouting == false ? "Create Container Delivery" : "View Container Delivery";
                this.DeleteRoutingLinkText = "Delete Container Delivery";
                this.DataContext.SetUIProperties_ValidateActualDates_D();
                break;
            }

            case "R": {
                this.IsFollowupProperty = "IsEmptyContainerReturnFU";
                this.ETDProperty = "EmptyContainerReturnETD";
                this.ATDProperty = "EmptyContainerReturnATD";
                this.ETAProperty = "EmptyContainerReturnETA";
                this.ATAProperty = "EmptyContainerReturnATA";
                this.FromProperty = "EmptyContainerReturnFrom";
                this.ToProperty = "EmptyContainerReturnTo";
                this.TransportModeProperty = "ECRTransportModeCode";
                this.HasRouting = AppTool.IsNullOrEmpty(this.EntityPM.EmptyContainerReturnId) ? false : true;
                this.ActionRoutingLinkText = this.HasRouting == false ? "Create Empty Container Return" : "View Empty Container Return";
                this.DeleteRoutingLinkText = "Delete Empty Container Return";
                this.DataContext.SetUIProperties_ValidateActualDates_R();
                break;
            }
        }
    }

    get IsFollowup() {
        switch (this.Code) {
            case "D": { return this.DataContext.IsDeliveryFU; }
            case "R": { return this.DataContext.IsEmptyContainerReturnFU; }
            default: { return null; }
        }
    }
    set IsFollowup(value: boolean) {
        switch (this.Code) {
            case "D": { this.DataContext.IsDeliveryFU = value; break; }
            case "R": { this.DataContext.IsEmptyContainerReturnFU = value; break; }
        }
    }

    get ETD() {
        switch (this.Code) {
            case "D": { return this.DataContext.DeliveryETD; }
            case "R": { return this.DataContext.EmptyContainerReturnETD; }
            default: { return null; }
        }
    }
    set ETD(value: Date) {
        switch (this.Code) {
            case "D": { this.DataContext.DeliveryETD = value; break; }
            case "R": { this.DataContext.EmptyContainerReturnETD = value; break; }
        }
    }

    get ATD() {
        switch (this.Code) {
            case "D": { return this.DataContext.DeliveryATD; }
            case "R": { return this.DataContext.EmptyContainerReturnATD; }
            default: { return null; }
        }
    }
    set ATD(value: Date) {
        switch (this.Code) {
            case "D": { this.DataContext.DeliveryATD = value; break; }
            case "R": { this.DataContext.EmptyContainerReturnATD = value; break; }
        }
    }

    get ETA() {
        switch (this.Code) {
            case "D": { return this.DataContext.DeliveryETA; }
            case "R": { return this.DataContext.EmptyContainerReturnETA; }
            default: { return null; }
        }
    }
    set ETA(value: Date) {
        switch (this.Code) {
            case "D": { this.DataContext.DeliveryETA = value; break; }
            case "R": { this.DataContext.EmptyContainerReturnETA = value; break; }
        }
    }

    get ATA() {
        switch (this.Code) {
            case "D": { return this.DataContext.DeliveryATA; }
            case "R": { return this.DataContext.EmptyContainerReturnATA; }
            default: { return null; }
        }
    }
    set ATA(value: Date) {
        switch (this.Code) {
            case "D": { this.DataContext.DeliveryATA = value; break; }
            case "R": { this.DataContext.EmptyContainerReturnATA = value; break; }
        }
    }

    get From() {
        switch (this.Code) {
            case "D": { return this.DataContext.DeliveryFrom; }
            case "R": { return this.DataContext.EmptyContainerReturnFrom; }
            default: { return null; }
        }
    }
    set From(value: string) {
        switch (this.Code) {
            case "D": { this.DataContext.DeliveryFrom= value; break; }
            case "R": { this.DataContext.EmptyContainerReturnFrom = value; break; }
        }
    }

    get To() {
        switch (this.Code) {
            case "D": { return this.DataContext.DeliveryTo; }
            case "R": { return this.DataContext.EmptyContainerReturnTo; }
            default: { return null; }
        }
    }
    set To(value: string) {
        switch (this.Code) {
            case "D": { this.DataContext.DeliveryTo = value; break; }
            case "R": { this.DataContext.EmptyContainerReturnTo = value; break; }
        }
    }

    get TransportModeCode() {
        switch (this.Code) {
            case "D": { return this.DataContext.DeliveryTransportModeCode; }
            case "R": { return this.DataContext.ECRTransportModeCode; }
            default: { return null; }
        }
    }
    set TransportModeCode(value: string) {
        switch (this.Code) {
            case "D": { this.DataContext.DeliveryTransportModeCode = value; break; }
            case "R": { this.DataContext.ECRTransportModeCode = value; break; }
        }
    }

    SetActualDateClicked(fieldName: string) {
        switch (fieldName) {
            case "DeliveryETD":
            case "EmptyContainerReturnETD":
                {
                    this.ATD = DateTool.GetDateParts(this.ETD).DateObject;
                    break;
                }

            case "DeliveryETA":
            case "EmptyContainerReturnETA":
                {
                    this.ATA = DateTool.GetDateParts(this.ETA).DateObject;
                    break;
                }
        }
    }

    ActionRoutingLinkClicked() {
        var isValid: boolean = this.FatherComponent.Validate();

        if (isValid) {
            this.FatherComponent.Save("ActionLink");
        }
    }
    DeleteRoutingLinkClicked() {

        if (this.ATD || this.ATA) {
            var iWindow = new MessageWindow();
            iWindow.Show("Actual Dates filled, can't delete delivery");
        }

        else {
            var message = TextCodeTranslator.Translate("Shipment.M.DeleteThisDelivery");
            if (this.Code == "R") {
                message = TextCodeTranslator.Translate("Shipment.M.DeleteThisEmptyCR");
            }

            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show(message);
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    var isValid: boolean = this.FatherComponent.Validate();

                    if (isValid) {
                        var myDeliveryId: string = null;

                        switch (this.Code) {
                            case "D": {
                                myDeliveryId = this.EntityPM.DeliveryId;
                                this.DataContext.DeliveryId = null;
                                break;
                            }

                            case "R": {
                                myDeliveryId = this.EntityPM.EmptyContainerReturnId;
                                this.DataContext.EmptyContainerReturnId = null;
                                break;
                            }
                        }

                        if (myDeliveryId) {
                            var item = this.TabComponent.EntityPM.ShipmentDeliveries.filter(f => f.Id == myDeliveryId)[0];
                            if (item) {
                                this.TabComponent.EntityPM.RemoveDelivery(item);
                                this.FatherComponent.Save("DeleteLink");
                            }
                        }
                    }
                }
            });
        }
    }
    DisconnectDeliveryLinkClicked() {
        if (this.Code == "D") {

            var Delivery: ShipmentDeliveryPM = this.TabComponent.EntityPM.ShipmentDeliveries.filter(f => f.Id == this.EntityPM.DeliveryId)[0];

            if (Delivery) {

                Delivery.ConnectedPackageId = null;

                if (Delivery.AllConnectedPackagesId) {
                    var index = Delivery.AllConnectedPackagesId.indexOf(this.EntityPM.Id);
                    if (index > -1) {
                        Delivery.AllConnectedPackagesId.splice(index, 1);                        
                    }
                }

                var DeliveryPackagePM: ShipmentPickUpDeliveryPackagePM = Delivery.ShipmentPickUpDeliveryPackages.filter(f => f.OriginalShipmentPackageId == this.EntityPM.Id)[0];
                if (DeliveryPackagePM) {
                    Delivery.RemovePackage(DeliveryPackagePM);
                }
            }

            this.DataContext.DeliveryId = null;
            this.FatherComponent.Save("DeleteLink");
        }
    }
}
