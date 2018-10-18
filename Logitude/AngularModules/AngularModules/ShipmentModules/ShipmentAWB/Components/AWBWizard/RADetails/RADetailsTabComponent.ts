import {Component} from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {AWBWizardComponent} from '../AWBWizardComponent';
import {ShipmentTool} from '../../../../../Shipment/Tools';
import {DateTool} from '../../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    selector: 'RADetailsTabComponent',
    templateUrl: './RADetailsTabComponent.html',
})

export class RADetailsTabComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    public Wizard: AWBWizardComponent;
    public DataContext: RADetailsTabComponent = this;
    public ObjectTableName: string;
    public LabelColumnWidth: number = 150;
    public ControlColumnWidth: number = 200;
    constructor() {
        super();     
    }

    InitTab(wizard: AWBWizardComponent) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.Listen();
        this.SetUIProperties();
    }

    RefreshTab() {

    }

    private Listen() {
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.Wizard.EntityPM;
                    this.SetUIProperties();
                }
            });

            this.Wizard.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.Wizard.EntityPM;
                    this.SetUIProperties();
                }
            });
        }
    }

    public IsEditingEnabled: boolean = false;
    private SetUIProperties() {
        this.IsEditingEnabled = ShipmentTool.IsEditingEnabled(this.EntityPM);
        this.UIProperties.SetEnabled('IsKnownCargo', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('RegulatedAgentRANumber', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('KnownConsignorNumber', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('ColoaderRANumber', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('AWBPrintingSecurityStatusId', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('AWBPrintingRANumber', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('AdditionalHandlingInfo', this.ObjectTableName, this.IsEditingEnabled);
    }

    // Regualted Agent Field Changed
    private RAFieldChanged() {
        ShipmentTool.OnRegulatedAgentFieldChanged(this.EntityPM);
    }

    get TenantZeroAirlineId() { return this.EntityPM.TenantZeroAirlineId; }

    // RA Properties
    get IsKnownCargo() { return this.EntityPM.IsKnownCargo; }
    set IsKnownCargo(newValue: boolean) {
        if (this.EntityPM.IsKnownCargo != newValue) {
            this.EntityPM.IsKnownCargo = newValue;
            this.RAFieldChanged();
        }
    }

    get RegulatedAgentRANumber() { return this.EntityPM.RegulatedAgentRANumber; }
    set RegulatedAgentRANumber(newValue: string) {
        if (this.EntityPM.RegulatedAgentRANumber != newValue) {
            this.EntityPM.RegulatedAgentRANumber = newValue;
            this.RAFieldChanged();
        }
    }

    get KnownConsignorNumber() { return this.EntityPM.KnownConsignorNumber; }
    set KnownConsignorNumber(newValue: string) {
        if (this.EntityPM.KnownConsignorNumber != newValue) {
            this.EntityPM.KnownConsignorNumber = newValue;
            this.RAFieldChanged();
        }
    }

    get KCExpirationDate() { return this.EntityPM.KCExpirationDate; }
    set KCExpirationDate(newValue: Date) {
        if (this.EntityPM.KCExpirationDate != newValue) {
            this.EntityPM.KCExpirationDate = newValue;
            this.RAFieldChanged();
        }
    }

    get KCExpirationDateForeground() {
        var myResult = "#282E30";
            
        if (this.KCExpirationDate != null) {
            var todayDate = DateTool.GetCurrentDateAsUtc();
            
            if (DateTool.TruncateTime(this.KCExpirationDate).valueOf() < DateTool.TruncateTime(todayDate).valueOf()) {
                myResult = "#E53030";
            }

            else if (this.EntityPM.MainCarriageETD != null) {
                var date = DateTool.AddDays(this.EntityPM.MainCarriageETD, 7);

                if (DateTool.TruncateTime(this.KCExpirationDate).valueOf() < DateTool.TruncateTime(date).valueOf()) {                
                    myResult = "Orange";
                }
            }
        }

        return myResult;
    }

    get ColoaderRANumber() { return this.EntityPM.ColoaderRANumber; }
    set ColoaderRANumber(newValue: string) {
        if (this.EntityPM.ColoaderRANumber != newValue) {
            this.EntityPM.ColoaderRANumber = newValue;
            this.RAFieldChanged();
        }
    }

    // Printing Properties
    get AWBPrintingRANumber() { return this.EntityPM.AWBPrintingRANumber; }
    set AWBPrintingRANumber(newValue: string) {
        if (this.EntityPM.AWBPrintingRANumber != newValue) {
            this.EntityPM.AWBPrintingRANumber = newValue;
        }

        if (newValue == ShipmentTool.ComputeAWBPrintingRANumber(this.EntityPM)) {
            this.AWBPrintingRANumberEdited = false;
        }

        else {
            this.AWBPrintingRANumberEdited = true;
        }
    }

    get AdditionalHandlingInfo() { return this.EntityPM.AdditionalHandlingInfo; }
    set AdditionalHandlingInfo(newValue: string) {
        if (this.EntityPM.AdditionalHandlingInfo != newValue) {
            var myOldValue = this.EntityPM.AdditionalHandlingInfo;
            this.EntityPM.AdditionalHandlingInfo = newValue;
            ShipmentTool.OnAdditionalHandlingInfoChanged(this.EntityPM, myOldValue, newValue);            
        }

        if (newValue == ShipmentTool.ComputeAdditionalHandlingInfo(this.EntityPM)) {
            this.AdditionalHandlingInfoEdited = false;
        }

        else {
            this.AdditionalHandlingInfoEdited = true;
        } 
    }

    get AWBPrintingSecurityStatusId() { return this.EntityPM.AWBPrintingSecurityStatusId; }
    set AWBPrintingSecurityStatusId(newValue: string) {
        if (this.EntityPM.AWBPrintingSecurityStatusId != newValue) {
            this.EntityPM.AWBPrintingSecurityStatusId = newValue;
            ShipmentTool.OnSecurityCodeChanged(this.EntityPM);
        }

        if (newValue == ShipmentTool.ComputeAWBPrintingSecurityStatus(this.EntityPM)) {
            this.AWBPrintingSecurityStatusEdited = false;
        }

        else {
            this.AWBPrintingSecurityStatusEdited = true;
        }
    }

    get AWBPrintingRANumberEdited() { return this.EntityPM.AWBPrintingRANumberEdited; }
    set AWBPrintingRANumberEdited(newValue: boolean) {
        if (this.EntityPM.AWBPrintingRANumberEdited != newValue) {
            this.EntityPM.AWBPrintingRANumberEdited = newValue;
        }
    }

    get AdditionalHandlingInfoEdited() { return this.EntityPM.AdditionalHandlingInfoEdited; }
    set AdditionalHandlingInfoEdited(newValue: boolean) {
        if (this.EntityPM.AdditionalHandlingInfoEdited != newValue) {
            this.EntityPM.AdditionalHandlingInfoEdited = newValue;
        }
    }

    get AWBPrintingSecurityStatusEdited() { return this.EntityPM.AWBPrintingSecurityStatusEdited; }
    set AWBPrintingSecurityStatusEdited(newValue: boolean) {
        if (this.EntityPM.AWBPrintingSecurityStatusEdited != newValue) {
            this.EntityPM.AWBPrintingSecurityStatusEdited = newValue;
        }
    }

    // Reset Commands
    private ResetAWBPrintingRANumber() {
        this.EntityPM.AWBPrintingRANumber = ShipmentTool.ComputeAWBPrintingRANumber(this.EntityPM);
        this.EntityPM.AWBPrintingRANumberEdited = false; 
    }
    private ResetAdditionalHandling() {
        var myOldValue = this.EntityPM.AdditionalHandlingInfo;
        this.EntityPM.AdditionalHandlingInfo = ShipmentTool.ComputeAdditionalHandlingInfo(this.EntityPM);
        this.EntityPM.AdditionalHandlingInfoEdited = false;
        ShipmentTool.OnAdditionalHandlingInfoChanged(this.EntityPM, myOldValue, this.EntityPM.AdditionalHandlingInfo);
    }
    private ResetAWBPrintingSecurity() {
        this.EntityPM.AWBPrintingSecurityStatusId = ShipmentTool.ComputeAWBPrintingSecurityStatus(this.EntityPM);
        this.EntityPM.AWBPrintingSecurityStatusEdited = false;
        ShipmentTool.OnSecurityCodeChanged(this.EntityPM);
    }
}