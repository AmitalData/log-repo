import {Component} from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties}  from '../../../../../Infrastructure/Components/LogitudeComponents/UIProperties'
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {AWBWizardComponent} from '../AWBWizardComponent';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {AppTool, FormatTool} from '../../../../../Infrastructure/Tools';
import {ShipmentTool} from '../../../../../Shipment/Tools';

@Component({
    moduleId: module.id,

    selector: 'GeneralDetailsTabComponent',
    templateUrl: './GeneralDetailsTabComponent.html',    
})

export class GeneralDetailsTabComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    public Wizard: AWBWizardComponent;
    public DataContext: GeneralDetailsTabComponent = this;
    public ObjectTableName: string;
    public IsFWB: boolean = false; 
    public LabelColumnWidth: number = 175;
    public SCIList: SCIClass[] = [];
    constructor() {
        super(); 
    }

    InitTab(wizard: AWBWizardComponent) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.IsFWB = this.Wizard.IsFWB;
        this.FillSCIList();
        this.Listen();
        this.Validate();
        this.SetUIProperties();
        this.SetUIProperties_OneTime();
    }

    FillSCIList() {
        this.SCIList.push({ Code: "0", Name: null });
        this.SCIList.push({ Code: "C", Name: "C" });
        this.SCIList.push({ Code: "X", Name: "X" });
        this.SCIList.push({ Code: "TD", Name: "TD" });
        this.SCIList.push({ Code: "T1", Name: "T1" });
        this.SCIList.push({ Code: "T2", Name: "T2" });
        this.SCIList.push({ Code: "TF", Name: "TF" });
    }

    private selectedSCI: SCIClass = null;
    get SelectedSCI() {

        var sCI: string = null;
        if (!AppTool.IsNullOrEmpty(this.SCI)) {
            sCI = this.SCI.toUpperCase();
        }

        switch (sCI) {
            case "X":
            case "C":
            case "TD":
            case "T1":
            case "T2":
            case "TF":
                {
                    this.selectedSCI = this.SCIList.filter(d => d.Code == sCI)[0];
                    break;
                }

            default:
                {
                    this.selectedSCI = this.SCIList.filter(d => d.Code == "0")[0];
                    break;
                }
        }

        return this.selectedSCI;
    }
    set SelectedSCI(newValue: SCIClass) {
        if (this.selectedSCI != newValue) {
            this.selectedSCI = newValue;

            if (newValue == null || newValue.Code == "0") {
                this.SCI = null;
            }

            else {
                this.SCI = newValue.Code;
            }
        }
    }

    RefreshTab() {
        this.Validate();
        this.SetUIProperties();
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

    // SetUIProperties
    public IsEditingEnabled: boolean = false;
    private SetUIProperties() {
        this.IsEditingEnabled = ShipmentTool.IsEditingEnabled(this.EntityPM);

        this.UIProperties.SetEnabled("AWBHandlingInformation", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("AWBComments", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("SCI", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("MainHarmonize", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("AWBDeclaredValueForCarriage", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("AWBDeclaredValueForCustoms", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("AWBInsurrenceValue", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("AWBCarrierTarrifReference", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ReferenceNumber", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("SupplementaryShipmentInformation1", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("SupplementaryShipmentInformation2", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("AWBSignature", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("AWBPlace", this.ObjectTableName, this.IsEditingEnabled);

        this.SetUIProperties_RA();
        this.SetUIProperties_AccountingInformation();
    }
    
    private SetUIProperties_RA() {

        //http://stackoverflow.com/questions/7744611/pass-variables-by-reference-in-javascript

        if (this.Wizard.IsFWB) {
            if (this.Wizard.TenantPM.RegulatedAgentRegimeActivated) {
                var isFieldFound = false;
                var isHandlingCodeEnabled1 = this.IsEditingEnabled;
                var isHandlingCodeEnabled2 = this.IsEditingEnabled;
                var isHandlingCodeEnabled3 = this.IsEditingEnabled;
                var isHandlingCodeEnabled4 = this.IsEditingEnabled;
                var isHandlingCodeEnabled5 = this.IsEditingEnabled;
                var isHandlingCodeEnabled6 = this.IsEditingEnabled;
                var isHandlingCodeEnabled7 = this.IsEditingEnabled;
                var isHandlingCodeEnabled8 = this.IsEditingEnabled;
                var isHandlingCodeEnabled9 = this.IsEditingEnabled;
                var myRAField = this.EntityPM.AWBPrintingSecurityStatusId;

                if (!AppTool.IsNullOrEmpty(myRAField)) {

                    if (!isFieldFound) {
                        var myCodeField = this.AWBSpecialHandlingCodeId9;
                        if (!AppTool.IsNullOrEmpty(myCodeField)) {
                            if (myCodeField == myRAField) {
                                isFieldFound = true;
                                isHandlingCodeEnabled9 = false;
                            }
                        }
                    }

                    if (!isFieldFound) {
                        var myCodeField = this.AWBSpecialHandlingCodeId8;
                        if (!AppTool.IsNullOrEmpty(myCodeField)) {
                            if (myCodeField == myRAField) {
                                isFieldFound = true;
                                isHandlingCodeEnabled8 = false;
                            }
                        }
                    }

                    if (!isFieldFound) {
                        var myCodeField = this.AWBSpecialHandlingCodeId7;
                        if (!AppTool.IsNullOrEmpty(myCodeField)) {
                            if (myCodeField == myRAField) {
                                isFieldFound = true;
                                isHandlingCodeEnabled7 = false;
                            }
                        }
                    }

                    if (!isFieldFound) {
                        var myCodeField = this.AWBSpecialHandlingCodeId6;
                        if (!AppTool.IsNullOrEmpty(myCodeField)) {
                            if (myCodeField == myRAField) {
                                isFieldFound = true;
                                isHandlingCodeEnabled6 = false;
                            }
                        }
                    }

                    if (!isFieldFound) {
                        var myCodeField = this.AWBSpecialHandlingCodeId5;
                        if (!AppTool.IsNullOrEmpty(myCodeField)) {
                            if (myCodeField == myRAField) {
                                isFieldFound = true;
                                isHandlingCodeEnabled5 = false;
                            }
                        }
                    }

                    if (!isFieldFound) {
                        var myCodeField = this.AWBSpecialHandlingCodeId4;
                        if (!AppTool.IsNullOrEmpty(myCodeField)) {
                            if (myCodeField == myRAField) {
                                isFieldFound = true;
                                isHandlingCodeEnabled4 = false;
                            }
                        }
                    }

                    if (!isFieldFound) {
                        var myCodeField = this.AWBSpecialHandlingCodeId3;
                        if (!AppTool.IsNullOrEmpty(myCodeField)) {
                            if (myCodeField == myRAField) {
                                isFieldFound = true;
                                isHandlingCodeEnabled3 = false;
                            }
                        }
                    }

                    if (!isFieldFound) {
                        var myCodeField = this.AWBSpecialHandlingCodeId2;
                        if (!AppTool.IsNullOrEmpty(myCodeField)) {
                            if (myCodeField == myRAField) {
                                isFieldFound = true;
                                isHandlingCodeEnabled2 = false;
                            }
                        }
                    }

                    if (!isFieldFound) {
                        var myCodeField = this.AWBSpecialHandlingCodeId1;
                        if (!AppTool.IsNullOrEmpty(myCodeField)) {
                            if (myCodeField == myRAField) {
                                isFieldFound = true;
                                isHandlingCodeEnabled1 = false;
                            }
                        }
                    }
                }

                this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId1", this.ObjectTableName, isHandlingCodeEnabled1);
                this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId2", this.ObjectTableName, isHandlingCodeEnabled2);
                this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId3", this.ObjectTableName, isHandlingCodeEnabled3);
                this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId4", this.ObjectTableName, isHandlingCodeEnabled4);
                this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId5", this.ObjectTableName, isHandlingCodeEnabled5);
                this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId6", this.ObjectTableName, isHandlingCodeEnabled6);
                this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId7", this.ObjectTableName, isHandlingCodeEnabled7);
                this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId8", this.ObjectTableName, isHandlingCodeEnabled8);
                this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId9", this.ObjectTableName, isHandlingCodeEnabled9);
            }
        }

        else {
            this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId1", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId2", this.ObjectTableName, this.IsEditingEnabled);
        }
    }
    private SetUIProperties_AccountingInformation() {
        var isFieldEnabled = this.IsEditingEnabled;

        if (isFieldEnabled) {
            if (this.Wizard.IsFWB) {
                if (ShipmentTool.IsAdvancedAccountingInformation(this.EntityPM)) {
                    isFieldEnabled = false;
                }
            }
        }

        this.UIProperties.SetEnabled("AWBAccountingInformation", this.ObjectTableName, isFieldEnabled);
    }
    private SetUIProperties_OneTime() {
        this.UIProperties.SetVisibility("ReferenceNumber", this.ObjectTableName, this.Wizard.IsFWB);
        this.UIProperties.SetVisibility("SupplementaryShipmentInformation1", this.ObjectTableName, this.Wizard.IsFWB);
        this.UIProperties.SetVisibility("SupplementaryShipmentInformation2", this.ObjectTableName, this.Wizard.IsFWB);

        if (this.Wizard.IsFHL) {
            this.UIProperties.SetVisibility("AWBSpecialHandlingCodeId3", this.ObjectTableName, false);
            this.UIProperties.SetVisibility("AWBSpecialHandlingCodeId4", this.ObjectTableName, false);
            this.UIProperties.SetVisibility("AWBSpecialHandlingCodeId5", this.ObjectTableName, false);
            this.UIProperties.SetVisibility("AWBSpecialHandlingCodeId6", this.ObjectTableName, false);
            this.UIProperties.SetVisibility("AWBSpecialHandlingCodeId7", this.ObjectTableName, false);
            this.UIProperties.SetVisibility("AWBSpecialHandlingCodeId8", this.ObjectTableName, false);
            this.UIProperties.SetVisibility("AWBSpecialHandlingCodeId9", this.ObjectTableName, false);
        }
    }

    // Validate    
    public ShowWarning_AWBAccountingInformation: boolean = false;
    public ShowWarning_AWBHandlingInformation: boolean = false;
    public ShowWarning_AWBComments: boolean = false;
    public ShowWarning_AWBSpecialHandlingCodes: boolean = false;
    public ShowWarning_SCI: boolean = false;
    public ShowWarning_MainHarmonize: boolean = false;
    public ShowWarning_AWBDeclaredValueForCarriage: boolean = false;
    public ShowWarning_AWBDeclaredValueForCustoms: boolean = false;
    public ShowWarning_AWBInsurrenceValue: boolean = false;
    public ShowWarning_AWBCarrierTarrifReference: boolean = false;
    public ShowWarning_ReferenceNumber: boolean = false;
    public ShowWarning_SupplementaryShipmentInformation1: boolean = false;
    public ShowWarning_SupplementaryShipmentInformation2: boolean = false;
    public ShowWarning_AWBSignature: boolean = false;
    public ShowWarning_AWBPlace: boolean = false;
    private FireWizardEvent() {
        this.Wizard.ValidateScreen_GEN();
    }
    private Validate() {
        if (!this.Wizard.IsImportWizard) {
            this.Validate_AWBAccountingInformation();
            this.Validate_AWBHandlingInformation();
            this.Validate_AWBComments();
            this.Validate_AWBSpecialHandlingCodes();
            this.Validate_SCI();
            this.Validate_MainHarmonize();
            this.Validate_AWBDeclaredValueForCarriage();
            this.Validate_AWBDeclaredValueForCustoms();
            this.Validate_AWBInsurrenceValue();
            this.Validate_AWBCarrierTarrifReference();
            this.Validate_ReferenceNumber();
            this.Validate_SupplementaryShipmentInformation1();
            this.Validate_SupplementaryShipmentInformation2();
            this.Validate_AWBSignature();
            this.Validate_AWBPlace();
        }
    }
    private Validate_AWBAccountingInformation() {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;

            if (this.Wizard.IsFWB) {
                if (!AppTool.IsNullOrEmpty(this.AWBAccountingInformation)) {
                    if (!FormatTool.IsTextFormatted(this.AWBAccountingInformation)) {
                        isValid = false;
                    }
                }
            }

            if (isValid) {
                var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "AWBAccountingInformation")[0];
                if (myFieldRule != null) {
                    if (!ShipmentTool.IsAdvancedAccountingInformation(this.EntityPM)) {
                        if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBAccountingInformation)) {
                            isValid = false;
                        }
                    }
                }
            }

            this.ShowWarning_AWBAccountingInformation = !isValid;
        }
    }
    private Validate_AWBHandlingInformation() {
        if (!this.Wizard.IsImportWizard) {

            var isValid = true;

            if (this.Wizard.IsFWB) {
                if (!AppTool.IsNullOrEmpty(this.AWBHandlingInformation)) {
                    if (!FormatTool.IsTextFormatted(this.AWBHandlingInformation)) {
                        isValid = false;
                    }
                }
            }

            if (isValid) {
                var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "AWBHandlingInformation")[0];
                if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBHandlingInformation)) {
                    isValid = false;
                }
            }

            this.ShowWarning_AWBHandlingInformation = !isValid;
        }
    }
    private Validate_AWBComments() {
        if (!this.Wizard.IsImportWizard) {

            var isValid = true;

            if (this.Wizard.IsFWB) {
                if (!AppTool.IsNullOrEmpty(this.AWBComments)) {
                    if (!FormatTool.IsTextFormatted(this.AWBComments)) {
                        isValid = false;
                    }
                }
            }

            if (isValid) {
                var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "AWBComments")[0];
                if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBComments)) {
                    isValid = false;
                }
            }

            this.ShowWarning_AWBComments = !isValid;
        }
    }
    private Validate_AWBSpecialHandlingCodes() {
        if (!this.Wizard.IsImportWizard) {


            var isValid = true;

            if (this.EntityPM.IsDangerous) {
                isValid = false;

                if (this.AWBSpecialHandlingCodeId1 != null) {
                    isValid = true;
                }

                else if (this.AWBSpecialHandlingCodeId2 != null) {
                    isValid = true;
                }

                else if (this.AWBSpecialHandlingCodeId3 != null) {
                    isValid = true;
                }

                else if (this.AWBSpecialHandlingCodeId4 != null) {
                    isValid = true;
                }

                else if (this.AWBSpecialHandlingCodeId5 != null) {
                    isValid = true;
                }

                else if (this.AWBSpecialHandlingCodeId6 != null) {
                    isValid = true;
                }

                else if (this.AWBSpecialHandlingCodeId7 != null) {
                    isValid = true;
                }

                else if (this.AWBSpecialHandlingCodeId8 != null) {
                    isValid = true;
                }

                else if (this.AWBSpecialHandlingCodeId9 != null) {
                    isValid = true;
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

            if (this.Wizard.IsFWB) {
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
            }

            this.ShowWarning_AWBSpecialHandlingCodes = !isValid;
        }
    }
    private Validate_SCI() {
        if (!this.Wizard.IsImportWizard) {

            var isValid = true;

            var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "SCI")[0];
            if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.SCI)) {
                isValid = false;
            }

            this.ShowWarning_SCI = !isValid;
        }
    }
    private Validate_MainHarmonize() {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;

            if (this.Wizard.IsFWB) {
                if (!AppTool.IsNullOrEmpty(this.MainHarmonize)) {
                    isValid = false;

                    if (this.MainHarmonize.length >= 6 && this.MainHarmonize.length <= 18) {
                        if (FormatTool.IsAlphaNumeric(this.MainHarmonize)) {
                            isValid = true;
                        }
                    }
                }
            }

            this.ShowWarning_MainHarmonize = !isValid;
        }
    }
    private Validate_AWBDeclaredValueForCarriage() {
        if (!this.Wizard.IsImportWizard) {

            var isValid = true;

            if (!FormatTool.Validate_DeclaredCarriage(this.AWBDeclaredValueForCarriage)) {
                isValid = false;
            }

            this.ShowWarning_AWBDeclaredValueForCarriage = !isValid;
        }
    }
    private Validate_AWBDeclaredValueForCustoms() {
        if (!this.Wizard.IsImportWizard) {

            var isValid = true;

            if (!FormatTool.Validate_DeclaredCustoms(this.AWBDeclaredValueForCustoms)) {
                isValid = false;
            }

            this.ShowWarning_AWBDeclaredValueForCustoms = !isValid;
        }
    }
    private Validate_AWBInsurrenceValue() {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;

            if (!FormatTool.Validate_DeclaredInsurrence(this.AWBInsurrenceValue)) {
                isValid = false;
            }

            this.ShowWarning_AWBInsurrenceValue = !isValid;
        }
    }
    private Validate_AWBCarrierTarrifReference() {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;

            var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "AWBCarrierTarrifReference")[0];
            if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.EntityPM.AWBCarrierTarrifReference)) {
                isValid = false;
            }

            this.ShowWarning_AWBCarrierTarrifReference = !isValid;
        }
    }
    private Validate_ReferenceNumber() {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;

            if (this.Wizard.IsFWB) {
                var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "ReferenceNumber")[0];
                if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.ReferenceNumber)) {
                    isValid = false;
                }
            }

            this.ShowWarning_ReferenceNumber = !isValid;
        }
    }
    private Validate_SupplementaryShipmentInformation1() {
        if (!this.Wizard.IsImportWizard) {

            var isValid = true;

            if (this.Wizard.IsFWB) {
                var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "SupplementaryShipmentInformation1")[0];
                if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.SupplementaryShipmentInformation1)) {
                    isValid = false;
                }
            }

            this.ShowWarning_SupplementaryShipmentInformation1 = !isValid;
        }
    }
    private Validate_SupplementaryShipmentInformation2() {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;

            if (this.Wizard.IsFWB) {
                var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "SupplementaryShipmentInformation1")[0];
                if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.SupplementaryShipmentInformation1)) {
                    isValid = false;
                }
            }

            this.ShowWarning_SupplementaryShipmentInformation2 = !isValid;
        }
    }
    private Validate_AWBSignature() {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;

            if (this.Wizard.IsFWB) {
                if (AppTool.IsNullOrEmpty(this.AWBSignature)) {
                    isValid = false;
                }

                else if (!FormatTool.IsTextFormatted(this.AWBSignature)) {
                    isValid = false;
                }
            }

            this.ShowWarning_AWBSignature = !isValid;
        }
    }
    private Validate_AWBPlace() {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;

            if (this.Wizard.IsFWB) {
                if (AppTool.IsNullOrEmpty(this.AWBPlace)) {
                    isValid = false;
                }

                else if (!FormatTool.IsTextFormatted(this.AWBPlace)) {
                    isValid = false;
                }
            }

            this.ShowWarning_AWBPlace = !isValid;
        }
    }

    // Properties
    get TenantZeroAirlineId() { return this.EntityPM.TenantZeroAirlineId; }

    get AWBAccountingInformation() { return this.EntityPM.AWBAccountingInformation; }
    set AWBAccountingInformation(newValue: string) {
        if (this.EntityPM.AWBAccountingInformation != newValue) {
            this.EntityPM.AWBAccountingInformation = newValue;
            this.FireWizardEvent();
            this.Validate_AWBAccountingInformation();
        }
    }

    get AWBHandlingInformation() { return this.EntityPM.AWBHandlingInformation; }
    set AWBHandlingInformation(newValue: string) {
        if (this.EntityPM.AWBHandlingInformation != newValue) {
            this.EntityPM.AWBHandlingInformation = newValue;
            this.FireWizardEvent();
            this.Validate_AWBHandlingInformation();
        }
    }

    get AWBComments() { return this.EntityPM.AWBComments; }
    set AWBComments(newValue: string) {
        if (this.EntityPM.AWBComments != newValue) {
            this.EntityPM.AWBComments = newValue;
            this.FireWizardEvent();
            this.Validate_AWBComments();
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

    get SCI() { return this.EntityPM.SCI; }
    set SCI(newValue: string) {
        if (this.EntityPM.SCI != newValue) {
            this.EntityPM.SCI = newValue;
            this.FireWizardEvent();
            this.Validate_SCI();
        }
    }

    get MainHarmonize() { return this.EntityPM.MainHarmonize; }
    set MainHarmonize(newValue: string) {
        if (this.EntityPM.MainHarmonize != newValue) {
            this.EntityPM.MainHarmonize = newValue;
            this.FireWizardEvent();
            this.Validate_MainHarmonize();
        }
    }

    get AWBDeclaredValueForCarriage() { return this.EntityPM.AWBDeclaredValueForCarriage; }
    set AWBDeclaredValueForCarriage(newValue: string) {
        if (this.EntityPM.AWBDeclaredValueForCarriage != newValue) {
            this.EntityPM.AWBDeclaredValueForCarriage = newValue;
            this.FireWizardEvent();
            this.Validate_AWBDeclaredValueForCarriage();
        }
    }

    get AWBDeclaredValueForCustoms() { return this.EntityPM.AWBDeclaredValueForCustoms; }
    set AWBDeclaredValueForCustoms(newValue: string) {
        if (this.EntityPM.AWBDeclaredValueForCustoms != newValue) {
            this.EntityPM.AWBDeclaredValueForCustoms = newValue;
            this.FireWizardEvent();
            this.Validate_AWBDeclaredValueForCustoms();
        }
    }

    get AWBInsurrenceValue() { return this.EntityPM.AWBInsurrenceValue; }
    set AWBInsurrenceValue(newValue: string) {
        if (this.EntityPM.AWBInsurrenceValue != newValue) {
            this.EntityPM.AWBInsurrenceValue = newValue;
            this.FireWizardEvent();
            this.Validate_AWBInsurrenceValue();
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

    get ReferenceNumber() { return this.EntityPM.ReferenceNumber; }
    set ReferenceNumber(newValue: string) {
        if (this.EntityPM.ReferenceNumber != newValue) {
            this.EntityPM.ReferenceNumber = newValue;
            this.FireWizardEvent();
            this.Validate_ReferenceNumber();
        }
    }

    get SupplementaryShipmentInformation1() { return this.EntityPM.SupplementaryShipmentInformation1; }
    set SupplementaryShipmentInformation1(newValue: string) {
        if (this.EntityPM.SupplementaryShipmentInformation1 != newValue) {
            this.EntityPM.SupplementaryShipmentInformation1 = newValue;
            this.FireWizardEvent();
            this.Validate_SupplementaryShipmentInformation1();
        }
    }

    get SupplementaryShipmentInformation2() { return this.EntityPM.SupplementaryShipmentInformation2; }
    set SupplementaryShipmentInformation2(newValue: string) {
        if (this.EntityPM.SupplementaryShipmentInformation2 != newValue) {
            this.EntityPM.SupplementaryShipmentInformation2 = newValue;
            this.FireWizardEvent();
            this.Validate_SupplementaryShipmentInformation2();
        }
    }

    get AWBSignature() { return this.EntityPM.AWBSignature; }
    set AWBSignature(newValue: string) {
        if (this.EntityPM.AWBSignature != newValue) {
            this.EntityPM.AWBSignature = newValue;
            this.FireWizardEvent();
            this.Validate_AWBSignature();
        }
    }

    get AWBPlace() { return this.EntityPM.AWBPlace; }
    set AWBPlace(newValue: string) {
        if (this.EntityPM.AWBPlace != newValue) {
            this.EntityPM.AWBPlace = newValue;
            this.FireWizardEvent();
            this.Validate_AWBPlace();
        }
    }

    // Advanced Window
    AccountingAdvancedButtonClicked() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 700;
        logitudeWindow.Height = 450;
        logitudeWindow.Title = "Accounting Information";
        logitudeWindow.WindowArgs = this.EntityPM;
        logitudeWindow.WindowClosed.subscribe(($event: any) => this.OnAdvancedAccountingWindowClosed($event));
        logitudeWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/GeneralDetails/AdvancedAccountingComponent');
    };

    CommentsAdvancedButtonClicked() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 400;
        logitudeWindow.Height = 300;
        logitudeWindow.Title = "AWB Comments";
        logitudeWindow.WindowArgs = this.EntityPM;
        logitudeWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/GeneralDetails/AdvancedCommentsComponent');
    }

    OnAdvancedAccountingWindowClosed(message: string) {
        if (message == "ok") {

            if (ShipmentTool.IsAdvancedAccountingInformation(this.EntityPM)) {
                this.AWBAccountingInformation = null;
            }

            this.SetUIProperties_AccountingInformation();
        }
    }
}

class SCIClass {
    public Code: string;
    public Name: string;
}
