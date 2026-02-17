import {Component} from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ContainerFollowupWizardComponent} from './ContainerFollowupWizardComponent';
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentPackagePM} from '../../../../../Shipment/EntityPMs/ShipmentPackagePM';
import {AppTool, DateTool} from '../../../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';

@Component({
    moduleId: module.id,
    templateUrl: './ContainerFollowupWizardTemplate.html',
})

export class ContainerFollowupWizardTemplate extends BaseComponent {
    public ShipmentPM: ShipmentPM;
    public EntityPM: ShipmentPackagePM;
    public DataContext = this;
    public ObjectTableName: string = "ShipmentPackage";
    public FatherComponent: ContainerFollowupWizardComponent;
    public IsDeliveryConnectedWithMultiContainers: boolean = false;
    constructor() {
        super();
    }

    Run(args: any) {        
        this.FatherComponent = args['FatherComponent'];
        this.EntityPM = this.FatherComponent.EntityPM;
        this.ShipmentPM = this.FatherComponent.ShipmentPM;
        this.IsDeliveryConnectedWithMultiContainers = this.FatherComponent.IsDeliveryConnectedWithMultiContainers;
        this.SetProperties();
        this.SetUIProperties();
    }

    public HasRouting_D: boolean = false;
    public HasRouting_R: boolean = false;
    public ActionRoutingLinkText_D: string;
    public ActionRoutingLinkText_R: string;
    SetProperties() {
        this.HasRouting_D = AppTool.IsNullOrEmpty(this.DeliveryId) ? false : true;
        this.HasRouting_R = AppTool.IsNullOrEmpty(this.EmptyContainerReturnId) ? false : true;
        this.ActionRoutingLinkText_D = this.HasRouting_D == false ? "Create Container Delivery" : "View Container Delivery";
        this.ActionRoutingLinkText_R = this.HasRouting_R == false ? "Create Empty Container Return" : "View Empty Container Return";
    }

    public IsEditingEnabled: boolean = true;
    public IsEditingEnabled_D: boolean = true;

    SetUIProperties() {
        this.SetUIProperties_ContainerFU();
        this.SetUIProperties_ValidateActualDates_D();
        this.SetUIProperties_ValidateActualDates_R();
    }
    SetUIProperties_ContainerFU() {

        var isDeliveryFieldsEnabled: boolean = false;
        var isDeliveryPlacesEnabled: boolean = false;
        if (this.IsEditingEnabled) {
            if (this.IsDeliveryFU) {

                if (this.IsDeliveryConnectedWithMultiContainers == false) {

                    isDeliveryFieldsEnabled = true;

                    if (AppTool.IsNullOrEmpty(this.DeliveryId)) {
                        isDeliveryPlacesEnabled = true;
                    }
                }
            }
        }

        this.IsEditingEnabled_D = isDeliveryFieldsEnabled;
        this.UIProperties.SetEnabled("IsDeliveryFU", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("DeliveryETD", this.ObjectTableName, isDeliveryFieldsEnabled);
        this.UIProperties.SetEnabled("DeliveryATD", this.ObjectTableName, isDeliveryFieldsEnabled);
        this.UIProperties.SetEnabled("DeliveryETA", this.ObjectTableName, isDeliveryFieldsEnabled);
        this.UIProperties.SetEnabled("DeliveryATA", this.ObjectTableName, isDeliveryFieldsEnabled);
        this.UIProperties.SetEnabled("DeliveryTransportModeCode", this.ObjectTableName, isDeliveryFieldsEnabled);
        this.UIProperties.SetEnabled("DeliveryFrom", this.ObjectTableName, isDeliveryPlacesEnabled);
        this.UIProperties.SetEnabled("DeliveryTo", this.ObjectTableName, isDeliveryPlacesEnabled);

        var isEmptyContainerReturnFieldsEnabled: boolean = false;
        var isEmptyContainerReturnPlacesEnabled: boolean = false;
        if (this.IsEditingEnabled) {
            if (this.IsEmptyContainerReturnFU) {
                isEmptyContainerReturnFieldsEnabled = true;

                if (AppTool.IsNullOrEmpty(this.EmptyContainerReturnId)) {
                    isEmptyContainerReturnPlacesEnabled = true;
                }
            }
        }

        this.UIProperties.SetEnabled("IsEmptyContainerReturnFU", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("EmptyContainerReturnETD", this.ObjectTableName, isEmptyContainerReturnFieldsEnabled);
        this.UIProperties.SetEnabled("EmptyContainerReturnATD", this.ObjectTableName, isEmptyContainerReturnFieldsEnabled);
        this.UIProperties.SetEnabled("EmptyContainerReturnETA", this.ObjectTableName, isEmptyContainerReturnFieldsEnabled);
        this.UIProperties.SetEnabled("EmptyContainerReturnATA", this.ObjectTableName, isEmptyContainerReturnFieldsEnabled);
        this.UIProperties.SetEnabled("ECRTransportModeCode", this.ObjectTableName, isEmptyContainerReturnFieldsEnabled);
        this.UIProperties.SetEnabled("EmptyContainerReturnFrom", this.ObjectTableName, isEmptyContainerReturnPlacesEnabled);
        this.UIProperties.SetEnabled("EmptyContainerReturnTo", this.ObjectTableName, isEmptyContainerReturnPlacesEnabled);
    }
    SetUIProperties_ValidateActualDates_D() {

        this.UIProperties.SetValidity("DeliveryATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("DeliveryATA", this.ObjectTableName, true, null);

        if (!DateTool.IsActualDateValid(this.DeliveryATD)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("ShipmentPackage.F.DeliveryATD"));
            this.UIProperties.SetValidity("DeliveryATD", this.ObjectTableName, false, errorMessage);
        }

        if (!DateTool.IsActualDateValid(this.DeliveryATA)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("ShipmentPackage.F.DeliveryATA"));
            this.UIProperties.SetValidity("DeliveryATA", this.ObjectTableName, false, errorMessage);
        }
    }
    SetUIProperties_ValidateActualDates_R() {

        this.UIProperties.SetValidity("EmptyContainerReturnATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("EmptyContainerReturnATA", this.ObjectTableName, true, null);

        if (!DateTool.IsActualDateValid(this.EmptyContainerReturnATD)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("ShipmentPackage.F.EmptyContainerReturnATD"));
            this.UIProperties.SetValidity("EmptyContainerReturnATD", this.ObjectTableName, false, errorMessage);
        }

        if (!DateTool.IsActualDateValid(this.EmptyContainerReturnATA)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("ShipmentPackage.F.EmptyContainerReturnATA"));
            this.UIProperties.SetValidity("EmptyContainerReturnATA", this.ObjectTableName, false, errorMessage);
        }
    }

    get IsDeliveryFU() { return this.EntityPM.IsDeliveryFU; }
    set IsDeliveryFU(value: boolean) {
        if (this.EntityPM.IsDeliveryFU != value) {
            this.EntityPM.IsDeliveryFU = value;
            this.SetUIProperties_ContainerFU();
        }
    }

    get DeliveryId() { return this.EntityPM.DeliveryId; }
    set DeliveryId(value: string) {
        if (this.EntityPM.DeliveryId != value) {
            this.EntityPM.DeliveryId = value;
            this.SetUIProperties_ContainerFU();
        }
    }

    get DeliveryFrom() { return this.EntityPM.DeliveryFrom; }
    set DeliveryFrom(value: string) {
        if (this.EntityPM.DeliveryFrom != value) {
            this.EntityPM.DeliveryFrom = value;
        }
    }

    get DeliveryTo() { return this.EntityPM.DeliveryTo; }
    set DeliveryTo(value: string) {
        if (this.EntityPM.DeliveryTo != value) {
            this.EntityPM.DeliveryTo = value;
        }
    }

    get DeliveryTransportModeCode() { return this.EntityPM.DeliveryTransportModeCode; }
    set DeliveryTransportModeCode(value: string) {
        if (this.EntityPM.DeliveryTransportModeCode != value) {
            this.EntityPM.DeliveryTransportModeCode = value;
        }
    }

    get DeliveryETD() { return this.EntityPM.DeliveryETD; }
    set DeliveryETD(value: Date) {
        if (this.EntityPM.DeliveryETD != value) {
            this.EntityPM.DeliveryETD = value;
        }
    }

    get DeliveryETA() { return this.EntityPM.DeliveryETA; }
    set DeliveryETA(value: Date) {
        if (this.EntityPM.DeliveryETA != value) {
            this.EntityPM.DeliveryETA = value;
        }
    }

    get DeliveryATD() { return this.EntityPM.DeliveryATD; }
    set DeliveryATD(value: Date) {
        if (this.EntityPM.DeliveryATD != value) {
            this.EntityPM.DeliveryATD = value;
            this.SetUIProperties_ValidateActualDates_D();
        }
    }

    get DeliveryATA() { return this.EntityPM.DeliveryATA; }
    set DeliveryATA(value: Date) {
        if (this.EntityPM.DeliveryATA != value) {
            this.EntityPM.DeliveryATA = value;
            this.SetUIProperties_ValidateActualDates_D();
        }
    }

    get IsEmptyContainerReturnFU() { return this.EntityPM.IsEmptyContainerReturnFU; }
    set IsEmptyContainerReturnFU(value: boolean) {
        if (this.EntityPM.IsEmptyContainerReturnFU != value) {
            this.EntityPM.IsEmptyContainerReturnFU = value;
            this.SetUIProperties_ContainerFU();
        }
    }

    get EmptyContainerReturnId() { return this.EntityPM.EmptyContainerReturnId; }
    set EmptyContainerReturnId(value: string) {
        if (this.EntityPM.EmptyContainerReturnId != value) {
            this.EntityPM.EmptyContainerReturnId = value;
            this.SetUIProperties_ContainerFU();
        }
    }

    get EmptyContainerReturnFrom() { return this.EntityPM.EmptyContainerReturnFrom; }
    set EmptyContainerReturnFrom(value: string) {
        if (this.EntityPM.EmptyContainerReturnFrom != value) {
            this.EntityPM.EmptyContainerReturnFrom = value;
        }
    }

    get EmptyContainerReturnTo() { return this.EntityPM.EmptyContainerReturnTo; }
    set EmptyContainerReturnTo(value: string) {
        if (this.EntityPM.EmptyContainerReturnTo != value) {
            this.EntityPM.EmptyContainerReturnTo = value;
        }
    }

    get ECRTransportModeCode() { return this.EntityPM.ECRTransportModeCode; }
    set ECRTransportModeCode(value: string) {
        if (this.EntityPM.ECRTransportModeCode != value) {
            this.EntityPM.ECRTransportModeCode = value;
        }
    }

    get EmptyContainerReturnETD() { return this.EntityPM.EmptyContainerReturnETD; }
    set EmptyContainerReturnETD(value: Date) {
        if (this.EntityPM.EmptyContainerReturnETD != value) {
            this.EntityPM.EmptyContainerReturnETD = value;
        }
    }

    get EmptyContainerReturnETA() { return this.EntityPM.EmptyContainerReturnETA; }
    set EmptyContainerReturnETA(value: Date) {
        if (this.EntityPM.EmptyContainerReturnETA != value) {
            this.EntityPM.EmptyContainerReturnETA = value;
        }
    }

    get EmptyContainerReturnATD() { return this.EntityPM.EmptyContainerReturnATD; }
    set EmptyContainerReturnATD(value: Date) {
        if (this.EntityPM.EmptyContainerReturnATD != value) {
            this.EntityPM.EmptyContainerReturnATD = value;
            this.SetUIProperties_ValidateActualDates_R();
        }
    }

    get EmptyContainerReturnATA() { return this.EntityPM.EmptyContainerReturnATA; }
    set EmptyContainerReturnATA(value: Date) {
        if (this.EntityPM.EmptyContainerReturnATA != value) {
            this.EntityPM.EmptyContainerReturnATA = value;
            this.SetUIProperties_ValidateActualDates_R();
        }
    }

    SetActualDateClicked(fieldName: string) {
        switch (fieldName) {
            case "DeliveryETD": { this.DeliveryATD = DateTool.GetDateParts(this.DeliveryETD).DateObject; break; }
            case "DeliveryETA": { this.DeliveryATA = DateTool.GetDateParts(this.DeliveryETA).DateObject; break; }
            case "EmptyContainerReturnETD": { this.EmptyContainerReturnATD = DateTool.GetDateParts(this.EmptyContainerReturnETD).DateObject; break; }
            case "EmptyContainerReturnETA": { this.EmptyContainerReturnATA = DateTool.GetDateParts(this.EmptyContainerReturnETA).DateObject; break; }
        }
    }

    ActionRoutingLinkClicked(typeCode: string) {
        var isValid: boolean = this.FatherComponent.Validate();

        if (isValid) {
            this.FatherComponent.Save("ActionLink_" + typeCode);
        }
    }
    DeleteRoutingLinkClicked(typeCode: string) {
        var message = TextCodeTranslator.Translate("Shipment.M.DeleteThisDelivery");
        if (typeCode == "R") {
            message = TextCodeTranslator.Translate("Shipment.M.DeleteThisEmptyCR");
        }

        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show(message);
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                var isValid: boolean = this.FatherComponent.Validate();

                if (isValid) {
                    var myDeliveryId: string = null;

                    switch (typeCode) {
                        case "D": {
                            myDeliveryId = this.DeliveryId;
                            this.DeliveryId = null;
                            break;
                        }

                        case "R": {
                            myDeliveryId = this.EmptyContainerReturnId;
                            this.EmptyContainerReturnId = null;
                            break;
                        }
                    }

                    if (myDeliveryId) {
                        var item = this.ShipmentPM.ShipmentDeliveries.filter(f => f.Id == myDeliveryId)[0];
                        if (item) {
                            this.ShipmentPM.RemoveDelivery(item);
                            this.FatherComponent.Save("DeleteLink");
                        }
                    }
                }
            }
        });
    }
}
