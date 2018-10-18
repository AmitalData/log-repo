import {Component} from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties}  from '../../../../../Infrastructure/Components/LogitudeComponents/UIProperties'
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {AWBWizardComponent} from '../AWBWizardComponent';
import {AppTool,FormatTool} from '../../../../../Infrastructure/Tools';
import {ShipmentTool} from '../../../../../Shipment/Tools';

@Component({
    moduleId: module.id,

    selector: 'OtherPartnersTabComponent',
    templateUrl: './OtherPartnersTabComponent.html',   
})

export class OtherPartnersTabComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    public Wizard: AWBWizardComponent;
    public DataContext: OtherPartnersTabComponent = this;
    public ObjectTableName: string;
    constructor() {
        super();   
    }

    InitTab(wizard: AWBWizardComponent) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.Listen();        
        this.Validate();
        this.SetUIProperties();
    }

    RefreshTab() {
        this.Validate();
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
        this.UIProperties.SetEnabled('NominatedHandlingPartyId', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantIdCode1', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantInformationCode1', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantInformationName1', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantInformationPortCode1', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantInformationReference1', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantIdCode2', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantInformationCode2', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantInformationName2', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantInformationPortCode2', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantInformationReference2', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantIdCode3', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantInformationCode3', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantInformationName3', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantInformationPortCode3', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantInformationReference3', this.ObjectTableName, this.IsEditingEnabled);
    }

    // Validate
    public ShowWarning_NominatedHandlingPartyId: boolean = false;
    public ShowWarning_Participant1: boolean = false;
    public ShowWarning_Participant2: boolean = false;
    public ShowWarning_Participant3: boolean = false;    
    private FireWizardEvent() {
        this.Wizard.ValidateScreen_OTP();
    }
    private Validate() {
        this.Validate_Nominated();
        this.Validate_Participant1();
        this.Validate_Participant2();
        this.Validate_Participant3();
    }
    private Validate_Nominated() {
        var isValid = true;

        var myFieldRule = this.Wizard.AirlineRulesList.filter(d=> d.RuleFieldName == "NominatedHandlingPartyId")[0];
        if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.EntityPM.NominatedHandlingPartyId)) {
            isValid = false;
        }

        this.ShowWarning_NominatedHandlingPartyId = !isValid;
    }
    private Validate_Participant1() {
        var isValid = true;

        var isFieldFilled = ShipmentTool.IsParticipant1Filled(this.EntityPM);
        if (isFieldFilled) {
            var isMissingData = ShipmentTool.IsParticipant1MissingData(this.EntityPM);
            if (isMissingData) {
                isValid = false;
            }

            else {
                if (!FormatTool.IsAlphaNumeric(this.EntityPM.OtherParticipantIdCode1)) {
                    isValid = false;
                }

                else if (!FormatTool.IsAlphaNumeric(this.EntityPM.OtherParticipantInformationCode1)) {
                    isValid = false;
                }

                else if (!FormatTool.IsAlpha(this.EntityPM.OtherParticipantInformationPortCode1)) {
                    isValid = false;
                }

                else if (this.EntityPM.OtherParticipantInformationPortCode1.length != 3) {
                    isValid = false;
                }

                else if (!FormatTool.IsText(this.EntityPM.OtherParticipantInformationName1)) {
                    isValid = false;
                }

                else if (!FormatTool.IsText(this.EntityPM.OtherParticipantInformationReference1)) {
                    isValid = false;
                }
            }
        }

        if (isValid) {
            var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "OtherParticipantIdCode1")[0];
            if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.EntityPM.OtherParticipantIdCode1)) {
                isValid = false;
            }
        }

        this.ShowWarning_Participant1 = !isValid;
    }
    private Validate_Participant2() {
        var isValid = true;

        var isFieldFilled = ShipmentTool.IsParticipant2Filled(this.EntityPM);
        if (isFieldFilled) {
            var isMissingData = ShipmentTool.IsParticipant2MissingData(this.EntityPM);
            if (isMissingData) {
                isValid = false;
            }

            else {
                if (!FormatTool.IsAlphaNumeric(this.EntityPM.OtherParticipantIdCode2)) {
                    isValid = false;
                }

                else if (!FormatTool.IsAlphaNumeric(this.EntityPM.OtherParticipantInformationCode2)) {
                    isValid = false;
                }

                else if (!FormatTool.IsAlpha(this.EntityPM.OtherParticipantInformationPortCode2)) {
                    isValid = false;
                }

                else if (this.EntityPM.OtherParticipantInformationPortCode2.length != 3) {
                    isValid = false;
                }

                else if (!FormatTool.IsText(this.EntityPM.OtherParticipantInformationName2)) {
                    isValid = false;
                }

                else if (!FormatTool.IsText(this.EntityPM.OtherParticipantInformationReference2)) {
                    isValid = false;
                }
            }
        }

        if (isValid) {
            var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "OtherParticipantIdCode2")[0];
            if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.EntityPM.OtherParticipantIdCode2)) {
                isValid = false;
            }
        }

        this.ShowWarning_Participant2 = !isValid;
    }
    private Validate_Participant3() {
        var isValid = true;

        var isFieldFilled = ShipmentTool.IsParticipant3Filled(this.EntityPM);
        if (isFieldFilled) {
            var isMissingData = ShipmentTool.IsParticipant3MissingData(this.EntityPM);
            if (isMissingData) {
                isValid = false;
            }

            else {
                if (!FormatTool.IsAlphaNumeric(this.EntityPM.OtherParticipantIdCode3)) {
                    isValid = false;
                }

                else if (!FormatTool.IsAlphaNumeric(this.EntityPM.OtherParticipantInformationCode3)) {
                    isValid = false;
                }

                else if (!FormatTool.IsAlpha(this.EntityPM.OtherParticipantInformationPortCode3)) {
                    isValid = false;
                }

                else if (this.EntityPM.OtherParticipantInformationPortCode3.length != 3) {
                    isValid = false;
                }

                else if (!FormatTool.IsText(this.EntityPM.OtherParticipantInformationName3)) {
                    isValid = false;
                }

                else if (!FormatTool.IsText(this.EntityPM.OtherParticipantInformationReference3)) {
                    isValid = false;
                }
            }
        }

        if (isValid) {
            var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "OtherParticipantIdCode3")[0];
            if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.EntityPM.OtherParticipantIdCode3)) {
                isValid = false;
            }
        }

        this.ShowWarning_Participant3 = !isValid;
    }
    

    // [Properties]
    get NominatedHandlingPartyId() { return this.EntityPM.NominatedHandlingPartyId; }
    set NominatedHandlingPartyId(newValue: string) {
        if (this.EntityPM.NominatedHandlingPartyId != newValue) {
            this.EntityPM.NominatedHandlingPartyId = newValue;
            this.FireWizardEvent();
            this.Validate_Nominated();
        }
    }

    get OtherParticipantIdCode1() { return this.EntityPM.OtherParticipantIdCode1; }
    set OtherParticipantIdCode1(newValue: string) {
        if (this.EntityPM.OtherParticipantIdCode1 != newValue) {
            this.EntityPM.OtherParticipantIdCode1 = newValue;
            this.FireWizardEvent();          
            this.Validate_Participant1();            
        }
    }
    get OtherParticipantIdCode2() { return this.EntityPM.OtherParticipantIdCode2; }
    set OtherParticipantIdCode2(newValue: string) {
        if (this.EntityPM.OtherParticipantIdCode2 != newValue) {
            this.EntityPM.OtherParticipantIdCode2 = newValue;
            this.FireWizardEvent();
            this.Validate_Participant2();            
        }
    }
    get OtherParticipantIdCode3() { return this.EntityPM.OtherParticipantIdCode3; }
    set OtherParticipantIdCode3(newValue: string) {
        if (this.EntityPM.OtherParticipantIdCode3 != newValue) {
            this.EntityPM.OtherParticipantIdCode3 = newValue;
            this.FireWizardEvent();
            this.Validate_Participant3();
        }
    }

    get OtherParticipantInformationCode1() { return this.EntityPM.OtherParticipantInformationCode1; }
    set OtherParticipantInformationCode1(newValue: string) {
        if (this.EntityPM.OtherParticipantInformationCode1 != newValue) {
            this.EntityPM.OtherParticipantInformationCode1 = newValue;
            this.FireWizardEvent();
            this.Validate_Participant1();
        }
    }
    get OtherParticipantInformationCode2() { return this.EntityPM.OtherParticipantInformationCode2; }
    set OtherParticipantInformationCode2(newValue: string) {
        if (this.EntityPM.OtherParticipantInformationCode2 != newValue) {
            this.EntityPM.OtherParticipantInformationCode2 = newValue;
            this.FireWizardEvent();
            this.Validate_Participant2();
        }
    }
    get OtherParticipantInformationCode3() { return this.EntityPM.OtherParticipantInformationCode3; }
    set OtherParticipantInformationCode3(newValue: string) {
        if (this.EntityPM.OtherParticipantInformationCode3 != newValue) {
            this.EntityPM.OtherParticipantInformationCode3 = newValue;
            this.FireWizardEvent();
            this.Validate_Participant3();
        }
    }

    get OtherParticipantInformationName1() { return this.EntityPM.OtherParticipantInformationName1; }
    set OtherParticipantInformationName1(newValue: string) {
        if (this.EntityPM.OtherParticipantInformationName1 != newValue) {
            this.EntityPM.OtherParticipantInformationName1 = newValue;
            this.FireWizardEvent();
            this.Validate_Participant1();
        }
    }
    get OtherParticipantInformationName2() { return this.EntityPM.OtherParticipantInformationName2; }
    set OtherParticipantInformationName2(newValue: string) {
        if (this.EntityPM.OtherParticipantInformationName2 != newValue) {
            this.EntityPM.OtherParticipantInformationName2 = newValue;
            this.FireWizardEvent();
            this.Validate_Participant2();
        }
    }
    get OtherParticipantInformationName3() { return this.EntityPM.OtherParticipantInformationName3; }
    set OtherParticipantInformationName3(newValue: string) {
        if (this.EntityPM.OtherParticipantInformationName3 != newValue) {
            this.EntityPM.OtherParticipantInformationName3 = newValue;
            this.FireWizardEvent();
            this.Validate_Participant3();
        }
    }

    get OtherParticipantInformationPortCode1() { return this.EntityPM.OtherParticipantInformationPortCode1; }
    set OtherParticipantInformationPortCode1(newValue: string) {
        if (this.EntityPM.OtherParticipantInformationPortCode1 != newValue) {
            this.EntityPM.OtherParticipantInformationPortCode1 = newValue;
            this.FireWizardEvent();
            this.Validate_Participant1();
        }
    }
    get OtherParticipantInformationPortCode2() { return this.EntityPM.OtherParticipantInformationPortCode2; }
    set OtherParticipantInformationPortCode2(newValue: string) {
        if (this.EntityPM.OtherParticipantInformationPortCode2 != newValue) {
            this.EntityPM.OtherParticipantInformationPortCode2 = newValue;
            this.FireWizardEvent();
            this.Validate_Participant2();
        }
    }
    get OtherParticipantInformationPortCode3() { return this.EntityPM.OtherParticipantInformationPortCode3; }
    set OtherParticipantInformationPortCode3(newValue: string) {
        if (this.EntityPM.OtherParticipantInformationPortCode3 != newValue) {
            this.EntityPM.OtherParticipantInformationPortCode3 = newValue;
            this.FireWizardEvent();
            this.Validate_Participant3();
        }
    }

    get OtherParticipantInformationReference1() { return this.EntityPM.OtherParticipantInformationReference1; }
    set OtherParticipantInformationReference1(newValue: string) {
        if (this.EntityPM.OtherParticipantInformationReference1 != newValue) {
            this.EntityPM.OtherParticipantInformationReference1 = newValue;
            this.FireWizardEvent();
            this.Validate_Participant1();
        }
    }
    get OtherParticipantInformationReference2() { return this.EntityPM.OtherParticipantInformationReference2; }
    set OtherParticipantInformationReference2(newValue: string) {
        if (this.EntityPM.OtherParticipantInformationReference2 != newValue) {
            this.EntityPM.OtherParticipantInformationReference2 = newValue;
            this.FireWizardEvent();
            this.Validate_Participant2();
        }
    }
    get OtherParticipantInformationReference3() { return this.EntityPM.OtherParticipantInformationReference3; }
    set OtherParticipantInformationReference3(newValue: string) {
        if (this.EntityPM.OtherParticipantInformationReference3 != newValue) {
            this.EntityPM.OtherParticipantInformationReference3 = newValue;
            this.FireWizardEvent();
            this.Validate_Participant3();
        }
    }
}