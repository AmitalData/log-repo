import {Component} from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties}  from '../../../../../Infrastructure/Components/LogitudeComponents/UIProperties'
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {AWBWizardComponent} from '../AWBWizardComponent';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {ShipmentTool} from '../../../../../Shipment/Tools';

@Component({
    

    selector: 'FreightChargesTabComponent',
    templateUrl: './FreightChargesTabComponent.html',    
})

export class FreightChargesTabComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    public Wizard: AWBWizardComponent;
    public DataContext: FreightChargesTabComponent = this;
    public ObjectTableName: string;
    public LabelColumnWidth: number = 150;
    public IsKRateClass: boolean = false;

    constructor() {
        super();
    }

    InitTab(wizard: AWBWizardComponent) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.SetLabels();        
        this.Listen();
        this.Validate();
        this.SetUIProperties();
        this.CheckKRateClassCode();
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

        this.UIProperties.SetEnabled("AWBCurrencyId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("AWBChargesCodeCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("AsAgreedFreight", this.ObjectTableName, this.IsEditingEnabled);                
        this.UIProperties.SetEnabled("RateClassCode", this.ObjectTableName, !this.EntityPM.IsMultipleCommodities && this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ChargeableWeight", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ChargeableWeightInKG", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("AWBFreightAmountPrepaid", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("AWBFreightAmountCollect", this.ObjectTableName, false);
        this.SetRateClassUIProperties();
    }
    private SetRateClassUIProperties() {
        if (this.EntityPM.IsMultipleCommodities) {
            this.UIProperties.SetEnabled("AWBChargeRate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AWBChargeAmount", this.ObjectTableName, false);
        }

        else {
            var rateClassGroupCode = ShipmentTool.GetRateClassGroupCode(this.EntityPM.RateClassCode);

            //if (rateClassGroupCode == "S") {
            //    this.AWBChargeRate = null;
            //    this.UIProperties.SetEnabled("AWBChargeRate", this.ObjectTableName, false);
            //    this.UIProperties.SetEnabled("AWBChargeAmount", this.ObjectTableName, this.IsEditingEnabled);

            //}

            //else {
                this.UIProperties.SetEnabled("AWBChargeRate", this.ObjectTableName, this.IsEditingEnabled);
                this.UIProperties.SetEnabled("AWBChargeAmount", this.ObjectTableName, false);
           // }
        }
    }

    public ChargeableWeightLabel: string;
    public ChargeableWeightKGLabel: string;
    private SetLabels() {
        this.ChargeableWeightLabel = TextCodeTranslator.Translate("Shipment.F.ChargeableWeight").replace('%ChargWeightCode', this.EntityPM.ChargeableWeightUnitCode);
        this.ChargeableWeightKGLabel = TextCodeTranslator.Translate("Shipment.F.ChargeableWeight").replace('%ChargWeightCode', "KG");
    }

    // Validate    
    public ShowWarning_AWBCurrencyId: boolean = false;
    public ShowWarning_RateClassCode: boolean = false;
    public ShowWarning_AWBChargeRate: boolean = false;
    public ShowWarning_ChargeableWeight: boolean = false;
    public ShowWarning_AWBChargeAmount: boolean = false;
    public ShowWarning_AWBChargesCodeCode: boolean = false;
    private FireWizardEvent() {
        this.Wizard.ValidateScreen_PAC();
        this.Wizard.ValidateScreen_FRE();
    }
    private Validate() {
        if (!this.Wizard.IsImportWizard) {
            var isShowWarning_AWBCurrencyId = false;
            var isShowWarning_RateClassCode = false;
            var isShowWarning_AWBChargeRate = false;
            var isShowWarning_ChargeableWeight = false;
            var isShowWarning_AWBChargeAmount = false;
            var isShowWarning_AWBChargesCodeCode = false;

            if (this.Wizard.IsFWB) {
                if (AppTool.IsNullOrEmpty(this.AWBCurrencyId)) {
                    isShowWarning_AWBCurrencyId = true;
                }

                if (AppTool.IsNullOrEmpty(this.AWBChargesCodeCode)) {
                    isShowWarning_AWBChargesCodeCode = true;
                }

                if (!this.EntityPM.IsMultipleCommodities) {
                    if (AppTool.IsNullOrEmpty(this.RateClassCode)) {
                        isShowWarning_RateClassCode = true;
                    }

                    if (AppTool.IsNullOrZero(this.ChargeableWeight)) {
                        isShowWarning_ChargeableWeight = true;
                    }

                    if (!this.AsAgreedFreight) {

                        var rateClassGroupCode = ShipmentTool.GetRateClassGroupCode(this.RateClassCode);
                        //if (rateClassGroupCode != "S") {
                            if (AppTool.IsNullOrZero(this.AWBChargeRate)) {
                                isShowWarning_AWBChargeRate = true;
                            }
                        //}

                        if (AppTool.IsNullOrZero(this.AWBChargeAmount)) {
                            isShowWarning_AWBChargeAmount = true;
                        }
                    }
                }
            }

            else {
                var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "AWBChargeRate")[0];
                if (!ShipmentTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBChargeRate)) {
                    isShowWarning_AWBChargeRate = true;
                }
            }

            this.ShowWarning_AWBCurrencyId = isShowWarning_AWBCurrencyId;
            this.ShowWarning_RateClassCode = isShowWarning_RateClassCode;
            this.ShowWarning_AWBChargeRate = isShowWarning_AWBChargeRate;
            this.ShowWarning_ChargeableWeight = isShowWarning_ChargeableWeight;
            this.ShowWarning_AWBChargeAmount = isShowWarning_AWBChargeAmount;
            this.ShowWarning_AWBChargesCodeCode = isShowWarning_AWBChargesCodeCode;
        }
    }

    // Properties
    get AWBCurrencyId() { return this.EntityPM.AWBCurrencyId; }
    set AWBCurrencyId(newValue: string) {
        if (this.EntityPM.AWBCurrencyId != newValue) {
            this.EntityPM.AWBCurrencyId = newValue;
            this.Validate();
            this.FireWizardEvent();
            this.UpdateOtherCharges();
        }
    }

    get RateClassCode() { return this.EntityPM.RateClassCode; }
    set RateClassCode(newValue: string) {
        if (this.EntityPM.RateClassCode != newValue) {
            this.EntityPM.RateClassCode = newValue;
            this.Validate();
            this.FireWizardEvent();
            this.ComputeAWBChargeAmount();
            this.SetRateClassUIProperties();
            this.CheckKRateClassCode();
        }
    }

    private CheckKRateClassCode() {
        if (this.RateClassCode == "K") {
            this.IsKRateClass = true;
        }
        else {
            this.IsKRateClass = false;
        }
    }

    get AWBChargeRate() { return this.EntityPM.AWBChargeRate; }
    set AWBChargeRate(newValue: number) {
        if (this.EntityPM.AWBChargeRate != newValue) {
            this.EntityPM.AWBChargeRate = AppTool.Round(newValue, 3);
            this.Validate();
            this.FireWizardEvent();
            this.ComputeAWBChargeAmount();
        }
    }

    get AsAgreedFreight() { return this.EntityPM.AsAgreedFreight; }
    set AsAgreedFreight(newValue: boolean) {
        if (this.EntityPM.AsAgreedFreight != newValue) {
            this.EntityPM.AsAgreedFreight = newValue;
            this.Validate();
            this.FireWizardEvent();
        }
    }

    get ChargeableWeight() { return AppTool.IsNullOrZero(this.EntityPM.ChargeableWeight) ? 0 : this.EntityPM.ChargeableWeight; }
    set ChargeableWeight(newValue: number) {
        if (this.EntityPM.ChargeableWeight != newValue) {
            this.EntityPM.ChargeableWeight = AppTool.Round(newValue, 3);
            this.ComputeAWBChargeAmount();
        }
    }

    get ChargeableWeightInKG() { return AppTool.IsNullOrZero(this.EntityPM.ChargeableWeightInKG) ? 0 : this.EntityPM.ChargeableWeightInKG; }
    set ChargeableWeightInKG(newValue: number) {
        if (this.EntityPM.ChargeableWeightInKG != newValue) {
            this.EntityPM.ChargeableWeightInKG = AppTool.Round(newValue, 3);
            this.ComputeAWBChargeAmount();
        }
    }

    get AWBChargeAmount() { return this.EntityPM.AWBChargeAmount; }
    set AWBChargeAmount(newValue: number) {
        if (this.EntityPM.AWBChargeAmount != newValue) {
            this.EntityPM.AWBChargeAmount = AppTool.Round(newValue, 3);
            this.ComputeAWBFrieghtAmount();
        }
    }

    get AWBChargesCodeCode() { return this.EntityPM.AWBChargesCodeCode; }
    set AWBChargesCodeCode(newValue: string) {
        if (this.EntityPM.AWBChargesCodeCode != newValue) {
            this.EntityPM.AWBChargesCodeCode = newValue;
            this.Validate();
            this.FireWizardEvent();
        }
    }

    get FreightPrepaidCollectId() { return this.EntityPM.FreightPrepaidCollectId; }
    set FreightPrepaidCollectId(newValue: string) {
        if (this.EntityPM.FreightPrepaidCollectId != newValue) {
            this.EntityPM.FreightPrepaidCollectId = newValue;
            ShipmentTool.BuildAWBChargesCodeCode(this.EntityPM);
            this.ComputeAWBFrieghtAmount();
        }
    }

    get AWBFreightAmountPrepaid() { return this.EntityPM.AWBFreightAmountPrepaid; }
    set AWBFreightAmountPrepaid(newValue: number) {
        if (this.EntityPM.AWBFreightAmountPrepaid != newValue) {
            this.EntityPM.AWBFreightAmountPrepaid = newValue;
            this.Validate();
            this.FireWizardEvent();
        }
    }

    get AWBFreightAmountCollect() { return this.EntityPM.AWBFreightAmountCollect; }
    set AWBFreightAmountCollect(newValue: number) {
        if (this.EntityPM.AWBFreightAmountCollect != newValue) {
            this.EntityPM.AWBFreightAmountCollect = newValue;
            this.Validate();
            this.FireWizardEvent();
        }
    }

    private UpdateOtherCharges() {
        //foreach(ShipmentPayablePM item in shipmentPM.ShipmentPayables.Where(d => d.ChargesGroupCode != "FRT"))
        //{
        //    if (string.IsNullOrEmpty(item.IATACodeId) || string.IsNullOrEmpty(item.PrepaidCollectId) || string.IsNullOrEmpty(item.DueTypeCode) || item.CurrencyId != AWBCurrencyId) {
        //        item.AWBPrint = false;
        //    }
        //}

        //foreach(ShipmentReceivablePM item in shipmentPM.ShipmentReceivables.Where(d => d.ChargesGroupCode != "FRT"))
        //{
        //    if (string.IsNullOrEmpty(item.IATACodeId) || string.IsNullOrEmpty(item.PrepaidCollectId) || string.IsNullOrEmpty(item.DueTypeCode) || item.CurrencyId != AWBCurrencyId) {
        //        item.AWBPrint = false;
        //    }
        //}
    }
    private ComputeAWBChargeAmount() {
        this.AWBChargeAmount = ShipmentTool.ComputeAWBChargeAmount(this.EntityPM);
    }
    private ComputeAWBFrieghtAmount() {
        var computedAmount = this.AWBChargeAmount;
        var totaAmount = this.AWBFreightAmountPrepaid + this.AWBFreightAmountCollect;

        var recompute = true;
        var isPrepaidHasAmount = (this.AWBFreightAmountPrepaid != 0 && this.AWBFreightAmountPrepaid != null);
        var isCollectHasAmount = (this.AWBFreightAmountCollect != 0 && this.AWBFreightAmountCollect != null);

        if (!AppTool.IsNullOrEmpty(this.FreightPrepaidCollectId)) {
            if (isPrepaidHasAmount && isCollectHasAmount && (computedAmount == totaAmount)) {
                recompute = false;
            }
        }

        if (recompute) {
            if (this.FreightPrepaidCollectId == "P") {
                this.AWBFreightAmountCollect = 0;
                this.AWBFreightAmountPrepaid = computedAmount == null ? 0 : computedAmount;
            }

            else if (this.FreightPrepaidCollectId == "C") {
                this.AWBFreightAmountPrepaid = 0;
                this.AWBFreightAmountCollect = computedAmount == null ? 0 : computedAmount;
            }
        }

        this.Validate();
        this.FireWizardEvent();
    }
    SetFreightPrepaidCollect(newValue: string) {
        this.FreightPrepaidCollectId = newValue;
    }

}
