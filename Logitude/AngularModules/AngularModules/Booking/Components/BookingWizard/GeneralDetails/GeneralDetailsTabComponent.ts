import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {BookingTool} from '../../../Tools';
import {AppTool} from '../../../../Infrastructure/Tools';
import {BookingWizardComponent} from '../BookingWizardComponent';
import {BookingPM} from '../../../EntityPMs/BookingPM';

@Component({
    selector: 'GeneralDetailsTabComponent',
    moduleId: module.id,
    templateUrl: './GeneralDetailsTabComponent.html',
})

export class GeneralDetailsTabComponent extends BaseComponent {
    public EntityPM: BookingPM;
    public Wizard: BookingWizardComponent;
    public DataContext: GeneralDetailsTabComponent = this;
    public ObjectTableName: string;
    constructor() {
        super();
    }
    
    InitTab(wizard: BookingWizardComponent) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.Listen();
        this.SetUIProperties();
        this.Validate();       
    }

    RefreshTab() {
        this.SetUIProperties();
        this.Validate();        
    }

    private Listen() {
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.Wizard.EntityPM;
                    this.RefreshTab();
                }
            });

            this.Wizard.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.Wizard.EntityPM;
                    this.RefreshTab();
                }
            });
        }
    }

    public IsBookingProductVisibile: boolean = false;
    private isEditingEnabled_Others: boolean;
    public SetUIProperties() {
        this.isEditingEnabled_Others = BookingTool.IsEditingFieldsEnabled_Others(this.EntityPM);

        this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId1", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId2", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId3", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId4", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId5", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId6", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId7", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId8", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId9", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("SpecialServicesRequest", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("OtherServicesInformation", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("AWBCarrierTarrifReference", this.ObjectTableName, this.isEditingEnabled_Others); 

        this.SetUIProperties_Product();
    }
    private SetUIProperties_Product() {
        if (this.EntityPM.TenantZeroIsManagingProduct) {
            this.UIProperties.SetVisibility("BookingProductId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("BookingProductId", this.ObjectTableName, this.isEditingEnabled_Others);
            this.IsBookingProductVisibile = true;
        }
        else {
            this.UIProperties.SetVisibility("BookingProductId", this.ObjectTableName, false);
            this.IsBookingProductVisibile = false;
        }
    }

    public ShowWarning_BookingProduct: boolean = false;
    public ShowWarning_TarrifReference: boolean = false;
    public ShowWarning_AWBSpecialHandlingCodes: boolean = false;
    public ShowWarning_SpecialServicesRequest: boolean = false;
    public ShowWarning_OtherServicesInformation: boolean = false;
    private FireWizardEvent() {
        this.Wizard.ValidateScreen_GEN();
    }

    private Validate() {
        this.Validate_BookingProduct();
        this.Validate_AWBCarrierTarrifReference();
        this.Validate_AWBSpecialHandlingCodes();
        this.Validate_SpecialServicesRequest();
        this.Validate_OtherServicesInformation();
    }

    private Validate_BookingProduct() {
        if (this.EntityPM.TenantZeroIsProductMandatory) {
            if (AppTool.IsNullOrEmpty(this.BookingProductId)) {
                this.ShowWarning_BookingProduct = true;
            }

            else {
                this.ShowWarning_BookingProduct = false;
            }
        }

        else {
            this.ShowWarning_BookingProduct = false;
        }
    }
    private Validate_AWBCarrierTarrifReference() {
        var isValid = true;

        var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "AWBCarrierTarrifReference")[0];
        if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBCarrierTarrifReference)) {
            isValid = false;
        }

        this.ShowWarning_TarrifReference = !isValid;
    }
    public Validate_AWBSpecialHandlingCodes() {
        var isValid = true;

        if (this.EntityPM.IsTemperatureSensitive) {
            if (AppTool.IsNullOrEmpty(this.AWBSpecialHandlingCodeId1) && AppTool.IsNullOrEmpty(this.AWBSpecialHandlingCodeId2) && AppTool.IsNullOrEmpty(this.AWBSpecialHandlingCodeId3)
                && AppTool.IsNullOrEmpty(this.AWBSpecialHandlingCodeId4) && AppTool.IsNullOrEmpty(this.AWBSpecialHandlingCodeId5) && AppTool.IsNullOrEmpty(this.AWBSpecialHandlingCodeId6)
                && AppTool.IsNullOrEmpty(this.AWBSpecialHandlingCodeId7) && AppTool.IsNullOrEmpty(this.AWBSpecialHandlingCodeId8) && AppTool.IsNullOrEmpty(this.AWBSpecialHandlingCodeId9)) {
                isValid = false;
            }
        }

        if (isValid) {
            var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "AWBSpecialHandlingCodeId1")[0];
            if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId1)) {
                isValid = false;
            }
        }

        if (isValid) {
            var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "AWBSpecialHandlingCodeId2")[0];
            if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId2)) {
                isValid = false;
            }
        }

        if (isValid) {
            var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "AWBSpecialHandlingCodeId3")[0];
            if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId3)) {
                isValid = false;
            }
        }

        if (isValid) {
            var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "AWBSpecialHandlingCodeId4")[0];
            if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId4)) {
                isValid = false;
            }
        }

        if (isValid) {
            var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "AWBSpecialHandlingCodeId5")[0];
            if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId5)) {
                isValid = false;
            }
        }

        if (isValid) {
            var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "AWBSpecialHandlingCodeId6")[0];
            if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId6)) {
                isValid = false;
            }
        }

        if (isValid) {
            var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "AWBSpecialHandlingCodeId7")[0];
            if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId7)) {
                isValid = false;
            }
        }

        if (isValid) {
            var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "AWBSpecialHandlingCodeId8")[0];
            if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId8)) {
                isValid = false;
            }
        }

        if (isValid) {
            var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "AWBSpecialHandlingCodeId9")[0];
            if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId9)) {
                isValid = false;
            }
        }

        this.ShowWarning_AWBSpecialHandlingCodes = !isValid;
    }
    private Validate_SpecialServicesRequest() {
        var isValid = true;

        var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "SpecialServicesRequest")[0];
        if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.SpecialServicesRequest)) {
            isValid = false;
        }

        this.ShowWarning_SpecialServicesRequest = !isValid;
    }
    private Validate_OtherServicesInformation() {
        var isValid = true;

        var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "OtherServicesInformation")[0];
        if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.OtherServicesInformation)) {
            isValid = false;
        }

        this.ShowWarning_OtherServicesInformation = !isValid;
    }
    
    // Properties
    get TenantZeroAirlineId() { return this.EntityPM.TenantZeroAirlineId; }

    get TenantZeroIsManagingProduct() { return this.EntityPM.TenantZeroIsManagingProduct; }

    get BookingProductId() { return this.EntityPM.BookingProductId; }
    set BookingProductId(newValue: string) {
        if (this.EntityPM.BookingProductId != newValue) {
            this.EntityPM.BookingProductId = newValue;
            this.FireWizardEvent();
            this.Validate_BookingProduct();
        }
    }

    get AWBCarrierTarrifReference() { return this.EntityPM.AWBCarrierTarrifReference; }
    set AWBCarrierTarrifReference(newValue: string) {
        if (this.EntityPM.AWBCarrierTarrifReference != newValue) {
            this.EntityPM.AWBCarrierTarrifReference = newValue;
            this.FireWizardEvent();
            this.Validate_AWBCarrierTarrifReference();
        }
    }

    get SpecialServicesRequest() { return this.EntityPM.SpecialServicesRequest; }
    set SpecialServicesRequest(newValue: string) {
        if (this.EntityPM.SpecialServicesRequest != newValue) {
            this.EntityPM.SpecialServicesRequest = newValue;
            this.FireWizardEvent();
            this.Validate_SpecialServicesRequest();
        }
    }

    get OtherServicesInformation() { return this.EntityPM.OtherServicesInformation; }
    set OtherServicesInformation(newValue: string) {
        if (this.EntityPM.OtherServicesInformation != newValue) {
            this.EntityPM.OtherServicesInformation = newValue;
            this.FireWizardEvent();
            this.Validate_OtherServicesInformation();
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(newValue: string) {
        if (this.EntityPM.Notes != newValue) {
            this.EntityPM.Notes = newValue;
        }
    }

    get AWBSpecialHandlingCodeId1() { return this.EntityPM.AWBSpecialHandlingCodeId1; }
    set AWBSpecialHandlingCodeId1(newValue: string) {
        if (this.EntityPM.AWBSpecialHandlingCodeId1 != newValue) {
            this.EntityPM.AWBSpecialHandlingCodeId1 = newValue;
            this.FireWizardEvent();
            this.Validate_AWBSpecialHandlingCodes();
        }
    }

    get AWBSpecialHandlingCodeId2() { return this.EntityPM.AWBSpecialHandlingCodeId2; }
    set AWBSpecialHandlingCodeId2(newValue: string) {
        if (this.EntityPM.AWBSpecialHandlingCodeId2 != newValue) {
            this.EntityPM.AWBSpecialHandlingCodeId2 = newValue;
            this.FireWizardEvent();
            this.Validate_AWBSpecialHandlingCodes();
        }
    }

    get AWBSpecialHandlingCodeId3() { return this.EntityPM.AWBSpecialHandlingCodeId3; }
    set AWBSpecialHandlingCodeId3(newValue: string) {
        if (this.EntityPM.AWBSpecialHandlingCodeId3 != newValue) {
            this.EntityPM.AWBSpecialHandlingCodeId3 = newValue;
            this.FireWizardEvent();
            this.Validate_AWBSpecialHandlingCodes();
        }
    }

    get AWBSpecialHandlingCodeId4() { return this.EntityPM.AWBSpecialHandlingCodeId4; }
    set AWBSpecialHandlingCodeId4(newValue: string) {
        if (this.EntityPM.AWBSpecialHandlingCodeId4 != newValue) {
            this.EntityPM.AWBSpecialHandlingCodeId4 = newValue;
            this.FireWizardEvent();
            this.Validate_AWBSpecialHandlingCodes();
        }
    }

    get AWBSpecialHandlingCodeId5() { return this.EntityPM.AWBSpecialHandlingCodeId5; }
    set AWBSpecialHandlingCodeId5(newValue: string) {
        if (this.EntityPM.AWBSpecialHandlingCodeId5 != newValue) {
            this.EntityPM.AWBSpecialHandlingCodeId5 = newValue;
            this.FireWizardEvent();
            this.Validate_AWBSpecialHandlingCodes();
        }
    }

    get AWBSpecialHandlingCodeId6() { return this.EntityPM.AWBSpecialHandlingCodeId6; }
    set AWBSpecialHandlingCodeId6(newValue: string) {
        if (this.EntityPM.AWBSpecialHandlingCodeId6 != newValue) {
            this.EntityPM.AWBSpecialHandlingCodeId6 = newValue;
            this.FireWizardEvent();
            this.Validate_AWBSpecialHandlingCodes();
        }
    }

    get AWBSpecialHandlingCodeId7() { return this.EntityPM.AWBSpecialHandlingCodeId7; }
    set AWBSpecialHandlingCodeId7(newValue: string) {
        if (this.EntityPM.AWBSpecialHandlingCodeId7 != newValue) {
            this.EntityPM.AWBSpecialHandlingCodeId7 = newValue;
            this.FireWizardEvent();
            this.Validate_AWBSpecialHandlingCodes();
        }
    }

    get AWBSpecialHandlingCodeId8() { return this.EntityPM.AWBSpecialHandlingCodeId8; }
    set AWBSpecialHandlingCodeId8(newValue: string) {
        if (this.EntityPM.AWBSpecialHandlingCodeId8 != newValue) {
            this.EntityPM.AWBSpecialHandlingCodeId8 = newValue;
            this.FireWizardEvent();
            this.Validate_AWBSpecialHandlingCodes();
        }
    }

    get AWBSpecialHandlingCodeId9() { return this.EntityPM.AWBSpecialHandlingCodeId9; }
    set AWBSpecialHandlingCodeId9(newValue: string) {
        if (this.EntityPM.AWBSpecialHandlingCodeId9 != newValue) {
            this.EntityPM.AWBSpecialHandlingCodeId9 = newValue;
            this.FireWizardEvent();
            this.Validate_AWBSpecialHandlingCodes();
        }
    }
}