"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var Tools_2 = require("../../../../../Shipment/Tools");
var GeneralDetailsTabComponent = /** @class */ (function (_super) {
    __extends(GeneralDetailsTabComponent, _super);
    function GeneralDetailsTabComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.IsFWB = false;
        _this.LabelColumnWidth = 175;
        _this.SCIList = [];
        _this.selectedSCI = null;
        // SetUIProperties
        _this.IsEditingEnabled = false;
        // Validate    
        _this.ShowWarning_AWBAccountingInformation = false;
        _this.ShowWarning_AWBHandlingInformation = false;
        _this.ShowWarning_AWBComments = false;
        _this.ShowWarning_AWBSpecialHandlingCodes = false;
        _this.ShowWarning_SCI = false;
        _this.ShowWarning_MainHarmonize = false;
        _this.ShowWarning_AWBDeclaredValueForCarriage = false;
        _this.ShowWarning_AWBDeclaredValueForCustoms = false;
        _this.ShowWarning_AWBInsurrenceValue = false;
        _this.ShowWarning_AWBCarrierTarrifReference = false;
        _this.ShowWarning_ReferenceNumber = false;
        _this.ShowWarning_SupplementaryShipmentInformation1 = false;
        _this.ShowWarning_SupplementaryShipmentInformation2 = false;
        _this.ShowWarning_AWBSignature = false;
        _this.ShowWarning_AWBPlace = false;
        return _this;
    }
    GeneralDetailsTabComponent.prototype.InitTab = function (wizard) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.IsFWB = this.Wizard.IsFWB;
        this.FillSCIList();
        this.Listen();
        this.Validate();
        this.SetUIProperties();
        this.SetUIProperties_OneTime();
    };
    GeneralDetailsTabComponent.prototype.FillSCIList = function () {
        this.SCIList.push({ Code: "0", Name: null });
        this.SCIList.push({ Code: "C", Name: "C" });
        this.SCIList.push({ Code: "X", Name: "X" });
        this.SCIList.push({ Code: "TD", Name: "TD" });
        this.SCIList.push({ Code: "T1", Name: "T1" });
        this.SCIList.push({ Code: "T2", Name: "T2" });
        this.SCIList.push({ Code: "TF", Name: "TF" });
    };
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "SelectedSCI", {
        get: function () {
            var sCI = null;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SCI)) {
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
                        this.selectedSCI = this.SCIList.filter(function (d) { return d.Code == sCI; })[0];
                        break;
                    }
                default:
                    {
                        this.selectedSCI = this.SCIList.filter(function (d) { return d.Code == "0"; })[0];
                        break;
                    }
            }
            return this.selectedSCI;
        },
        set: function (newValue) {
            if (this.selectedSCI != newValue) {
                this.selectedSCI = newValue;
                if (newValue == null || newValue.Code == "0") {
                    this.SCI = null;
                }
                else {
                    this.SCI = newValue.Code;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    GeneralDetailsTabComponent.prototype.RefreshTab = function () {
        this.Validate();
        this.SetUIProperties();
    };
    GeneralDetailsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    _this.SetUIProperties();
                }
            });
            this.Wizard.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    _this.SetUIProperties();
                }
            });
        }
    };
    GeneralDetailsTabComponent.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = Tools_2.ShipmentTool.IsEditingEnabled(this.EntityPM);
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
    };
    GeneralDetailsTabComponent.prototype.SetUIProperties_RA = function () {
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
                if (!Tools_1.AppTool.IsNullOrEmpty(myRAField)) {
                    if (!isFieldFound) {
                        var myCodeField = this.AWBSpecialHandlingCodeId9;
                        if (!Tools_1.AppTool.IsNullOrEmpty(myCodeField)) {
                            if (myCodeField == myRAField) {
                                isFieldFound = true;
                                isHandlingCodeEnabled9 = false;
                            }
                        }
                    }
                    if (!isFieldFound) {
                        var myCodeField = this.AWBSpecialHandlingCodeId8;
                        if (!Tools_1.AppTool.IsNullOrEmpty(myCodeField)) {
                            if (myCodeField == myRAField) {
                                isFieldFound = true;
                                isHandlingCodeEnabled8 = false;
                            }
                        }
                    }
                    if (!isFieldFound) {
                        var myCodeField = this.AWBSpecialHandlingCodeId7;
                        if (!Tools_1.AppTool.IsNullOrEmpty(myCodeField)) {
                            if (myCodeField == myRAField) {
                                isFieldFound = true;
                                isHandlingCodeEnabled7 = false;
                            }
                        }
                    }
                    if (!isFieldFound) {
                        var myCodeField = this.AWBSpecialHandlingCodeId6;
                        if (!Tools_1.AppTool.IsNullOrEmpty(myCodeField)) {
                            if (myCodeField == myRAField) {
                                isFieldFound = true;
                                isHandlingCodeEnabled6 = false;
                            }
                        }
                    }
                    if (!isFieldFound) {
                        var myCodeField = this.AWBSpecialHandlingCodeId5;
                        if (!Tools_1.AppTool.IsNullOrEmpty(myCodeField)) {
                            if (myCodeField == myRAField) {
                                isFieldFound = true;
                                isHandlingCodeEnabled5 = false;
                            }
                        }
                    }
                    if (!isFieldFound) {
                        var myCodeField = this.AWBSpecialHandlingCodeId4;
                        if (!Tools_1.AppTool.IsNullOrEmpty(myCodeField)) {
                            if (myCodeField == myRAField) {
                                isFieldFound = true;
                                isHandlingCodeEnabled4 = false;
                            }
                        }
                    }
                    if (!isFieldFound) {
                        var myCodeField = this.AWBSpecialHandlingCodeId3;
                        if (!Tools_1.AppTool.IsNullOrEmpty(myCodeField)) {
                            if (myCodeField == myRAField) {
                                isFieldFound = true;
                                isHandlingCodeEnabled3 = false;
                            }
                        }
                    }
                    if (!isFieldFound) {
                        var myCodeField = this.AWBSpecialHandlingCodeId2;
                        if (!Tools_1.AppTool.IsNullOrEmpty(myCodeField)) {
                            if (myCodeField == myRAField) {
                                isFieldFound = true;
                                isHandlingCodeEnabled2 = false;
                            }
                        }
                    }
                    if (!isFieldFound) {
                        var myCodeField = this.AWBSpecialHandlingCodeId1;
                        if (!Tools_1.AppTool.IsNullOrEmpty(myCodeField)) {
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
    };
    GeneralDetailsTabComponent.prototype.SetUIProperties_AccountingInformation = function () {
        var isFieldEnabled = this.IsEditingEnabled;
        if (isFieldEnabled) {
            if (this.Wizard.IsFWB) {
                if (Tools_2.ShipmentTool.IsAdvancedAccountingInformation(this.EntityPM)) {
                    isFieldEnabled = false;
                }
            }
        }
        this.UIProperties.SetEnabled("AWBAccountingInformation", this.ObjectTableName, isFieldEnabled);
    };
    GeneralDetailsTabComponent.prototype.SetUIProperties_OneTime = function () {
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
    };
    GeneralDetailsTabComponent.prototype.FireWizardEvent = function () {
        this.Wizard.ValidateScreen_GEN();
    };
    GeneralDetailsTabComponent.prototype.Validate = function () {
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
    };
    GeneralDetailsTabComponent.prototype.Validate_AWBAccountingInformation = function () {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;
            if (this.Wizard.IsFWB) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.AWBAccountingInformation)) {
                    if (!Tools_1.FormatTool.IsTextFormatted(this.AWBAccountingInformation)) {
                        isValid = false;
                    }
                }
            }
            if (isValid) {
                var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "AWBAccountingInformation"; })[0];
                if (myFieldRule != null) {
                    if (!Tools_2.ShipmentTool.IsAdvancedAccountingInformation(this.EntityPM)) {
                        if (!Tools_1.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBAccountingInformation)) {
                            isValid = false;
                        }
                    }
                }
            }
            this.ShowWarning_AWBAccountingInformation = !isValid;
        }
    };
    GeneralDetailsTabComponent.prototype.Validate_AWBHandlingInformation = function () {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;
            if (this.Wizard.IsFWB) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.AWBHandlingInformation)) {
                    if (!Tools_1.FormatTool.IsTextFormatted(this.AWBHandlingInformation)) {
                        isValid = false;
                    }
                }
            }
            if (isValid) {
                var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "AWBHandlingInformation"; })[0];
                if (!Tools_1.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBHandlingInformation)) {
                    isValid = false;
                }
            }
            this.ShowWarning_AWBHandlingInformation = !isValid;
        }
    };
    GeneralDetailsTabComponent.prototype.Validate_AWBComments = function () {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;
            if (this.Wizard.IsFWB) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.AWBComments)) {
                    if (!Tools_1.FormatTool.IsTextFormatted(this.AWBComments)) {
                        isValid = false;
                    }
                }
            }
            if (isValid) {
                var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "AWBComments"; })[0];
                if (!Tools_1.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBComments)) {
                    isValid = false;
                }
            }
            this.ShowWarning_AWBComments = !isValid;
        }
    };
    GeneralDetailsTabComponent.prototype.Validate_AWBSpecialHandlingCodes = function () {
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
                var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "AWBSpecialHandlingCodeId1"; })[0];
                if (!Tools_1.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId1)) {
                    isValid = false;
                }
            }
            if (isValid) {
                var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "AWBSpecialHandlingCodeId2"; })[0];
                if (!Tools_1.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId2)) {
                    isValid = false;
                }
            }
            if (this.Wizard.IsFWB) {
                if (isValid) {
                    var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "AWBSpecialHandlingCodeId3"; })[0];
                    if (!Tools_1.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId3)) {
                        isValid = false;
                    }
                }
                if (isValid) {
                    var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "AWBSpecialHandlingCodeId4"; })[0];
                    if (!Tools_1.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId4)) {
                        isValid = false;
                    }
                }
                if (isValid) {
                    var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "AWBSpecialHandlingCodeId5"; })[0];
                    if (!Tools_1.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId5)) {
                        isValid = false;
                    }
                }
                if (isValid) {
                    var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "AWBSpecialHandlingCodeId6"; })[0];
                    if (!Tools_1.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId6)) {
                        isValid = false;
                    }
                }
                if (isValid) {
                    var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "AWBSpecialHandlingCodeId7"; })[0];
                    if (!Tools_1.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId7)) {
                        isValid = false;
                    }
                }
                if (isValid) {
                    var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "AWBSpecialHandlingCodeId8"; })[0];
                    if (!Tools_1.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId8)) {
                        isValid = false;
                    }
                }
                if (isValid) {
                    var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "AWBSpecialHandlingCodeId9"; })[0];
                    if (!Tools_1.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId9)) {
                        isValid = false;
                    }
                }
            }
            this.ShowWarning_AWBSpecialHandlingCodes = !isValid;
        }
    };
    GeneralDetailsTabComponent.prototype.Validate_SCI = function () {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;
            var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "SCI"; })[0];
            if (!Tools_1.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.SCI)) {
                isValid = false;
            }
            this.ShowWarning_SCI = !isValid;
        }
    };
    GeneralDetailsTabComponent.prototype.Validate_MainHarmonize = function () {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;
            if (this.Wizard.IsFWB) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.MainHarmonize)) {
                    isValid = false;
                    if (this.MainHarmonize.length >= 6 && this.MainHarmonize.length <= 18) {
                        if (Tools_1.FormatTool.IsAlphaNumeric(this.MainHarmonize)) {
                            isValid = true;
                        }
                    }
                }
            }
            this.ShowWarning_MainHarmonize = !isValid;
        }
    };
    GeneralDetailsTabComponent.prototype.Validate_AWBDeclaredValueForCarriage = function () {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;
            if (!Tools_1.FormatTool.Validate_DeclaredCarriage(this.AWBDeclaredValueForCarriage)) {
                isValid = false;
            }
            this.ShowWarning_AWBDeclaredValueForCarriage = !isValid;
        }
    };
    GeneralDetailsTabComponent.prototype.Validate_AWBDeclaredValueForCustoms = function () {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;
            if (!Tools_1.FormatTool.Validate_DeclaredCustoms(this.AWBDeclaredValueForCustoms)) {
                isValid = false;
            }
            this.ShowWarning_AWBDeclaredValueForCustoms = !isValid;
        }
    };
    GeneralDetailsTabComponent.prototype.Validate_AWBInsurrenceValue = function () {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;
            if (!Tools_1.FormatTool.Validate_DeclaredInsurrence(this.AWBInsurrenceValue)) {
                isValid = false;
            }
            this.ShowWarning_AWBInsurrenceValue = !isValid;
        }
    };
    GeneralDetailsTabComponent.prototype.Validate_AWBCarrierTarrifReference = function () {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;
            var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "AWBCarrierTarrifReference"; })[0];
            if (!Tools_1.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.EntityPM.AWBCarrierTarrifReference)) {
                isValid = false;
            }
            this.ShowWarning_AWBCarrierTarrifReference = !isValid;
        }
    };
    GeneralDetailsTabComponent.prototype.Validate_ReferenceNumber = function () {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;
            if (this.Wizard.IsFWB) {
                var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "ReferenceNumber"; })[0];
                if (!Tools_1.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.ReferenceNumber)) {
                    isValid = false;
                }
            }
            this.ShowWarning_ReferenceNumber = !isValid;
        }
    };
    GeneralDetailsTabComponent.prototype.Validate_SupplementaryShipmentInformation1 = function () {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;
            if (this.Wizard.IsFWB) {
                var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "SupplementaryShipmentInformation1"; })[0];
                if (!Tools_1.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.SupplementaryShipmentInformation1)) {
                    isValid = false;
                }
            }
            this.ShowWarning_SupplementaryShipmentInformation1 = !isValid;
        }
    };
    GeneralDetailsTabComponent.prototype.Validate_SupplementaryShipmentInformation2 = function () {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;
            if (this.Wizard.IsFWB) {
                var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "SupplementaryShipmentInformation1"; })[0];
                if (!Tools_1.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.SupplementaryShipmentInformation1)) {
                    isValid = false;
                }
            }
            this.ShowWarning_SupplementaryShipmentInformation2 = !isValid;
        }
    };
    GeneralDetailsTabComponent.prototype.Validate_AWBSignature = function () {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;
            if (this.Wizard.IsFWB) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.AWBSignature)) {
                    isValid = false;
                }
                else if (!Tools_1.FormatTool.IsTextFormatted(this.AWBSignature)) {
                    isValid = false;
                }
            }
            this.ShowWarning_AWBSignature = !isValid;
        }
    };
    GeneralDetailsTabComponent.prototype.Validate_AWBPlace = function () {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;
            if (this.Wizard.IsFWB) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.AWBPlace)) {
                    isValid = false;
                }
                else if (!Tools_1.FormatTool.IsTextFormatted(this.AWBPlace)) {
                    isValid = false;
                }
            }
            this.ShowWarning_AWBPlace = !isValid;
        }
    };
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "TenantZeroAirlineId", {
        // Properties
        get: function () { return this.EntityPM.TenantZeroAirlineId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBAccountingInformation", {
        get: function () { return this.EntityPM.AWBAccountingInformation; },
        set: function (newValue) {
            if (this.EntityPM.AWBAccountingInformation != newValue) {
                this.EntityPM.AWBAccountingInformation = newValue;
                this.FireWizardEvent();
                this.Validate_AWBAccountingInformation();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBHandlingInformation", {
        get: function () { return this.EntityPM.AWBHandlingInformation; },
        set: function (newValue) {
            if (this.EntityPM.AWBHandlingInformation != newValue) {
                this.EntityPM.AWBHandlingInformation = newValue;
                this.FireWizardEvent();
                this.Validate_AWBHandlingInformation();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBComments", {
        get: function () { return this.EntityPM.AWBComments; },
        set: function (newValue) {
            if (this.EntityPM.AWBComments != newValue) {
                this.EntityPM.AWBComments = newValue;
                this.FireWizardEvent();
                this.Validate_AWBComments();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBSpecialHandlingCodeId1", {
        get: function () { return this.EntityPM.AWBSpecialHandlingCodeId1; },
        set: function (newValue) {
            if (this.EntityPM.AWBSpecialHandlingCodeId1 != newValue) {
                this.EntityPM.AWBSpecialHandlingCodeId1 = newValue;
                this.FireWizardEvent();
                this.Validate_AWBSpecialHandlingCodes();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBSpecialHandlingCodeId2", {
        get: function () { return this.EntityPM.AWBSpecialHandlingCodeId2; },
        set: function (newValue) {
            if (this.EntityPM.AWBSpecialHandlingCodeId2 != newValue) {
                this.EntityPM.AWBSpecialHandlingCodeId2 = newValue;
                this.FireWizardEvent();
                this.Validate_AWBSpecialHandlingCodes();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBSpecialHandlingCodeId3", {
        get: function () { return this.EntityPM.AWBSpecialHandlingCodeId3; },
        set: function (newValue) {
            if (this.EntityPM.AWBSpecialHandlingCodeId3 != newValue) {
                this.EntityPM.AWBSpecialHandlingCodeId3 = newValue;
                this.FireWizardEvent();
                this.Validate_AWBSpecialHandlingCodes();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBSpecialHandlingCodeId4", {
        get: function () { return this.EntityPM.AWBSpecialHandlingCodeId4; },
        set: function (newValue) {
            if (this.EntityPM.AWBSpecialHandlingCodeId4 != newValue) {
                this.EntityPM.AWBSpecialHandlingCodeId4 = newValue;
                this.FireWizardEvent();
                this.Validate_AWBSpecialHandlingCodes();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBSpecialHandlingCodeId5", {
        get: function () { return this.EntityPM.AWBSpecialHandlingCodeId5; },
        set: function (newValue) {
            if (this.EntityPM.AWBSpecialHandlingCodeId5 != newValue) {
                this.EntityPM.AWBSpecialHandlingCodeId5 = newValue;
                this.FireWizardEvent();
                this.Validate_AWBSpecialHandlingCodes();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBSpecialHandlingCodeId6", {
        get: function () { return this.EntityPM.AWBSpecialHandlingCodeId6; },
        set: function (newValue) {
            if (this.EntityPM.AWBSpecialHandlingCodeId6 != newValue) {
                this.EntityPM.AWBSpecialHandlingCodeId6 = newValue;
                this.FireWizardEvent();
                this.Validate_AWBSpecialHandlingCodes();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBSpecialHandlingCodeId7", {
        get: function () { return this.EntityPM.AWBSpecialHandlingCodeId7; },
        set: function (newValue) {
            if (this.EntityPM.AWBSpecialHandlingCodeId7 != newValue) {
                this.EntityPM.AWBSpecialHandlingCodeId7 = newValue;
                this.FireWizardEvent();
                this.Validate_AWBSpecialHandlingCodes();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBSpecialHandlingCodeId8", {
        get: function () { return this.EntityPM.AWBSpecialHandlingCodeId8; },
        set: function (newValue) {
            if (this.EntityPM.AWBSpecialHandlingCodeId8 != newValue) {
                this.EntityPM.AWBSpecialHandlingCodeId8 = newValue;
                this.FireWizardEvent();
                this.Validate_AWBSpecialHandlingCodes();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBSpecialHandlingCodeId9", {
        get: function () { return this.EntityPM.AWBSpecialHandlingCodeId9; },
        set: function (newValue) {
            if (this.EntityPM.AWBSpecialHandlingCodeId9 != newValue) {
                this.EntityPM.AWBSpecialHandlingCodeId9 = newValue;
                this.FireWizardEvent();
                this.Validate_AWBSpecialHandlingCodes();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "SCI", {
        get: function () { return this.EntityPM.SCI; },
        set: function (newValue) {
            if (this.EntityPM.SCI != newValue) {
                this.EntityPM.SCI = newValue;
                this.FireWizardEvent();
                this.Validate_SCI();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "MainHarmonize", {
        get: function () { return this.EntityPM.MainHarmonize; },
        set: function (newValue) {
            if (this.EntityPM.MainHarmonize != newValue) {
                this.EntityPM.MainHarmonize = newValue;
                this.FireWizardEvent();
                this.Validate_MainHarmonize();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBDeclaredValueForCarriage", {
        get: function () { return this.EntityPM.AWBDeclaredValueForCarriage; },
        set: function (newValue) {
            if (this.EntityPM.AWBDeclaredValueForCarriage != newValue) {
                this.EntityPM.AWBDeclaredValueForCarriage = newValue;
                this.FireWizardEvent();
                this.Validate_AWBDeclaredValueForCarriage();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBDeclaredValueForCustoms", {
        get: function () { return this.EntityPM.AWBDeclaredValueForCustoms; },
        set: function (newValue) {
            if (this.EntityPM.AWBDeclaredValueForCustoms != newValue) {
                this.EntityPM.AWBDeclaredValueForCustoms = newValue;
                this.FireWizardEvent();
                this.Validate_AWBDeclaredValueForCustoms();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBInsurrenceValue", {
        get: function () { return this.EntityPM.AWBInsurrenceValue; },
        set: function (newValue) {
            if (this.EntityPM.AWBInsurrenceValue != newValue) {
                this.EntityPM.AWBInsurrenceValue = newValue;
                this.FireWizardEvent();
                this.Validate_AWBInsurrenceValue();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBCarrierTarrifReference", {
        get: function () { return this.EntityPM.AWBCarrierTarrifReference; },
        set: function (newValue) {
            if (this.EntityPM.AWBCarrierTarrifReference != newValue) {
                this.EntityPM.AWBCarrierTarrifReference = newValue;
                this.FireWizardEvent();
                this.Validate_AWBCarrierTarrifReference();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "ReferenceNumber", {
        get: function () { return this.EntityPM.ReferenceNumber; },
        set: function (newValue) {
            if (this.EntityPM.ReferenceNumber != newValue) {
                this.EntityPM.ReferenceNumber = newValue;
                this.FireWizardEvent();
                this.Validate_ReferenceNumber();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "SupplementaryShipmentInformation1", {
        get: function () { return this.EntityPM.SupplementaryShipmentInformation1; },
        set: function (newValue) {
            if (this.EntityPM.SupplementaryShipmentInformation1 != newValue) {
                this.EntityPM.SupplementaryShipmentInformation1 = newValue;
                this.FireWizardEvent();
                this.Validate_SupplementaryShipmentInformation1();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "SupplementaryShipmentInformation2", {
        get: function () { return this.EntityPM.SupplementaryShipmentInformation2; },
        set: function (newValue) {
            if (this.EntityPM.SupplementaryShipmentInformation2 != newValue) {
                this.EntityPM.SupplementaryShipmentInformation2 = newValue;
                this.FireWizardEvent();
                this.Validate_SupplementaryShipmentInformation2();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBSignature", {
        get: function () { return this.EntityPM.AWBSignature; },
        set: function (newValue) {
            if (this.EntityPM.AWBSignature != newValue) {
                this.EntityPM.AWBSignature = newValue;
                this.FireWizardEvent();
                this.Validate_AWBSignature();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBPlace", {
        get: function () { return this.EntityPM.AWBPlace; },
        set: function (newValue) {
            if (this.EntityPM.AWBPlace != newValue) {
                this.EntityPM.AWBPlace = newValue;
                this.FireWizardEvent();
                this.Validate_AWBPlace();
            }
        },
        enumerable: true,
        configurable: true
    });
    // Advanced Window
    GeneralDetailsTabComponent.prototype.AccountingAdvancedButtonClicked = function () {
        var _this = this;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 700;
        logitudeWindow.Height = 450;
        logitudeWindow.Title = "Accounting Information";
        logitudeWindow.WindowArgs = this.EntityPM;
        logitudeWindow.WindowClosed.subscribe(function ($event) { return _this.OnAdvancedAccountingWindowClosed($event); });
        logitudeWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/GeneralDetails/AdvancedAccountingComponent');
    };
    ;
    GeneralDetailsTabComponent.prototype.CommentsAdvancedButtonClicked = function () {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 400;
        logitudeWindow.Height = 300;
        logitudeWindow.Title = "AWB Comments";
        logitudeWindow.WindowArgs = this.EntityPM;
        logitudeWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/GeneralDetails/AdvancedCommentsComponent');
    };
    GeneralDetailsTabComponent.prototype.OnAdvancedAccountingWindowClosed = function (message) {
        if (message == "ok") {
            if (Tools_2.ShipmentTool.IsAdvancedAccountingInformation(this.EntityPM)) {
                this.AWBAccountingInformation = null;
            }
            this.SetUIProperties_AccountingInformation();
        }
    };
    GeneralDetailsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'GeneralDetailsTabComponent',
            templateUrl: './GeneralDetailsTabComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], GeneralDetailsTabComponent);
    return GeneralDetailsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.GeneralDetailsTabComponent = GeneralDetailsTabComponent;
var SCIClass = /** @class */ (function () {
    function SCIClass() {
    }
    return SCIClass;
}());
//# sourceMappingURL=GeneralDetailsTabComponent.js.map