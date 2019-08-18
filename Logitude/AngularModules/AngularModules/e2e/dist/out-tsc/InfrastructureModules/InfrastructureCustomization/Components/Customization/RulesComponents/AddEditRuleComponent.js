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
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var ObjectFieldPM_1 = require("../../../../../Infrastructure/EntityPMs/ObjectFieldPM");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var ObjectTableRulePMService_1 = require("../../../../../Infrastructure/Services/StandardPMs/ObjectTableRulePMService");
var ObjectTableRuleFieldPMService_1 = require("../../../../../Infrastructure/Services/StandardPMs/ObjectTableRuleFieldPMService");
var ObjectTableRulePM_1 = require("../../../../../Infrastructure/EntityPMs/ObjectTableRulePM");
var RuleConditionFieldPM_1 = require("../../../../../Infrastructure/EntityPMs/RuleConditionFieldPM");
var ObjectTableRuleFieldPM_1 = require("../../../../../Infrastructure/EntityPMs/ObjectTableRuleFieldPM");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var Guid_1 = require("../../../../../Infrastructure/Utilities/Guid");
var FieldValueResolver_1 = require("../../../../../Infrastructure/Utilities/FieldValueResolver");
var ApiQueryFilters_1 = require("../../../../../Infrastructure/DataContracts/ApiQueryFilters");
var ConditionFilterField_1 = require("./ConditionFilterField");
var AddEditRuleComponent = /** @class */ (function (_super) {
    __extends(AddEditRuleComponent, _super);
    function AddEditRuleComponent() {
        var _this = _super.call(this) || this;
        _this.ValidationErrorsList = [];
        _this._objectTableRuleFieldPMService = new ObjectTableRuleFieldPMService_1.ObjectTableRuleFieldPMService();
        _this._objectTableRulePMService = new ObjectTableRulePMService_1.ObjectTableRulePMService();
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.IsResourcesReady = false;
        _this.IsNewEntity = false;
        _this.RuleTypes = [];
        _this.TriggerTypes = [];
        _this.RuleNotificationTypes = [];
        _this.ObjectFields = [];
        _this.PageNumber = 0;
        _this.FeildsChanged = false;
        _this.RuleFields = [];
        _this.currentRuleFields = [];
        _this.removedFields = [];
        _this.BooleanValues = ["True", "False"];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SelectedConditionObjectFields = [];
        _this.RemovedConditionObjectFields = [];
        _this.BuildQuestionTypes();
        _this.BuildTriggerTypes();
        _this.BuildNotificationTypes();
        _this.ConditionTXTAreaId = Guid_1.Guid.newGuid();
        _this.FieldsValues = new ConditionFilterField_1.FieldsValues();
        _this.RemovedConditionObjectFields = [];
        if (_this.CurrentSession == null) {
            _this.SearchFieldsId = "RuleSearchFields_-1_-1";
            _this.FiltersSearchFieldsId = "RuleFiltersSearchFieldsId_-1_-1";
        }
        else {
            _this.SearchFieldsId = "NewRuleSearchFields_" + _this.CurrentSession.GetNewId("NewRuleSearchFields");
            _this.FiltersSearchFieldsId = "NewRuleFiltersSearchFieldsId_" + _this.CurrentSession.GetNewId("NewRuleFiltersSearchFieldsId");
        }
        return _this;
    }
    //SetDataContext(dataContext: ObjectTableRulePM) {
    //    this.DataContext = dataContext;
    //}
    AddEditRuleComponent.prototype.SetWindowArgs = function (windowArgs) {
        var _this = this;
        this.ObjectTableId = windowArgs.ObjectTableId;
        this.IsNewEntity = windowArgs.IsNewEntity;
        if (this.IsNewEntity) {
            this.DataContext = new ObjectTableRulePM_1.ObjectTableRulePM();
            this.DataContext.Tenant = SessionLocator_1.SessionLocator.Tenant;
            this.DataContext.ObjectTableId = this.ObjectTableId;
            this.DataContext.RuleNotificationTypeCode = "WAR";
        }
        else {
            this.DataContext = windowArgs.EntityPM;
            this.currentRuleFields = window.ObjectTableRuleFields.filter(function (r) { return r.ObjectTableRuleId === _this.DataContext.Id; }); //
            this.RuleFields = window.ObjectTableRuleFields.filter(function (r) { return r.ObjectTableRuleId === _this.DataContext.Id; });
            ;
        }
        this.ObjectTable = window.ObjectTables.filter(function (d) { return d.Id == _this.ObjectTableId; })[0];
        this.ObjectTableId = this.ObjectTable.Id;
        this.ObjectFields = window.ObjectFields.filter(function (d) { return d.ObjectTableId === _this.ObjectTableId && !Tools_1.AppTool.IsNullOrEmpty(d.PMPropertyPath) && !d.DisplayOnly && !d.IsMulti; }); //!d.IsCustomFilter             
        this.ObjectFields = this.ObjectFields.sort(function (a, b) { return (a.FieldName.toLowerCase() === b.FieldName.toLowerCase()) ? 0 : (a.FieldName.toLowerCase() < b.FieldName.toLowerCase()) ? -1 : 1; });
        this.FieldsLovQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
        this.FieldsLovQueryFilters.Tenant = 0;
        this.FieldsLovQueryFilters.addAdditionalFilter("ObjectTableId", this.ObjectTableId, null, null, "Equals", false, false, false, "string");
        //var mappedFields = [];
        //this.ObjectFields.forEach((item, key) => {
        //    if (item) {
        //        var xx = this.MapJsonToEntityPM(item);
        //        mappedFields.push(xx);
        //    }
        //});
        //this.ObjectFields = mappedFields;
        this.filterFields = new ConditionFilterField_1.ConditionFilterFieldsClass(true, this, null); //this.pubSubAdvanceQueryFiltersService
        this.NEWallFilterFieldsClass = new ConditionFilterField_1.ConditionFilterFieldsClass(true, this, null); //this.pubSubAdvanceQueryFiltersService
        if (this.ObjectFields) {
            this.allFilterFields = this.ObjectFields.filter(function (o) { return o.DataTypeCode != "Constant"; });
            this.constantFilterFieldsList = this.ObjectFields.filter(function (o) { return o.DataTypeCode == "Constant"; });
            //this.timeFilterFieldsClass.AddFiltersList(window.ObjectFields.filter(o => o.CanFilter == true && o.FieldName != "TimeFrameFilter" && o.IsTimeFrameFilter == true && (o.ValidForQuerySection1 == this.currentQuery.QuerySection || o.ValidForQuerySection2 == this.currentQuery.QuerySection)), this.currentQuery.Id, myResult);
            this.NEWallFilterFieldsClass.AddFiltersList(this.ObjectFields.filter(function (o) { return o.DataTypeCode != "Constant"; }), null, []);
        }
        else {
            this.allFilterFields = [];
            this.constantFilterFieldsList = [];
            this.NEWallFilterFieldsClass.AddFiltersList([], null, []);
        }
        this.filterFields.AddFiltersList(this.allFilterFields, null, []);
        this.fieldsStaticList = this.NEWallFilterFieldsClass.FilterFields;
        for (var k in this.DataContext.RuleConditionFields) {
            var field = this.DataContext.RuleConditionFields[k];
            var objectField = this.ObjectFields.filter(function (f) { return f.Id == field.ObjectFieldId; })[0];
            if (objectField) {
                var mappedField = this.MapJsonToEntityPM(objectField);
                this.AddFilterField(mappedField);
            }
        }
    };
    //LoadRuleFields() {
    //    this._objectTableRuleFieldPMService.getAllByTenant
    //}
    AddEditRuleComponent.prototype.MapJsonToEntityPM = function (jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        if (jsonPM) {
            var entityPM;
            entityPM = new ObjectFieldPM_1.ObjectFieldPM();
            var jsonPMKeys = Object.keys(jsonPM);
            for (var key in jsonPMKeys) {
                if (jsonPMKeys[key] === "UIProperties") {
                    continue;
                }
                var property = jsonPMKeys[key];
                entityPM[property] = jsonPM[property];
            }
            entityPM.IsDirty = false;
            return entityPM;
        }
    };
    AddEditRuleComponent.prototype.AddFilterField = function (field) {
        if (this.SelectedConditionObjectFields == undefined) {
            this.SelectedConditionObjectFields = [];
        }
        var conditionField = this.DataContext.RuleConditionFields.filter(function (d) { return d.ObjectFieldId == field.Id; })[0];
        if (conditionField) {
            var value = conditionField.Value;
            this.FieldsValues.SetFieldValue(field.Id, value);
        }
        var mappedField = this.MapJsonToEntityPM(field);
        var filter = this.RemovedConditionObjectFields.filter(function (a) { return a.ObjectField.Id === field.Id; })[0];
        if (filter) {
            this.SelectedConditionObjectFields.push(filter); //
            this.RemovedConditionObjectFields = this.RemovedConditionObjectFields.filter(function (a) { return a.ObjectField.Id != filter.ObjectField.Id; });
        }
        else {
            this.SelectedConditionObjectFields.push(new ConditionFilterField_1.ConditionFilterField(mappedField, this.DataContext.Id, true, this.DataContext.RuleConditionFields, this, null)); //
        }
        //if (!this.IsNewEntity) {
        //    this.SelectedConditionObjectFields.push(new ConditionFilterField(mappedField, this.DataContext.Id, true, this.DataContext.RuleConditionFields, this, null));//this.pubSubAdvanceQueryFiltersService
        //}
        //else {
        //    this.SelectedConditionObjectFields.push(new ConditionFilterField(mappedField, this.DataContext.Id, true, this.DataContext.RuleConditionFields, this, null));//this.pubSubAdvanceQueryFiltersService
        //}
        //if (this.SelectedObjectFields.length > 10) {
        //    this.ValidationErrorsList = [];
        //    this.ValidationErrorsList.push("The max. number of filters you can use is 10");
        //    this.NEWallFilterFieldsClass.SetExists(field, false);
        //    return;
        //}
        this.NEWallFilterFieldsClass.SetExists(mappedField, true);
    };
    Object.defineProperty(AddEditRuleComponent.prototype, "SelectedRuleType", {
        get: function () {
            var _this = this;
            if (this.DataContext) {
                this.selectedRuleType = this.RuleTypes.filter(function (t) { return t.Code === _this.DataContext.RuleTypeCode; })[0];
            }
            return this.selectedRuleType;
        },
        set: function (newValue) {
            if (this.selectedRuleType != newValue) {
                this.selectedRuleType = newValue;
                this.DataContext.RuleTypeCode = newValue.Code;
                this.BuildTriggerTypes();
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditRuleComponent.prototype.onTypeSelected = function (code) {
        this.DataContext.RuleTypeCode = code;
    };
    AddEditRuleComponent.prototype.onTriggerTypeSelected = function (code) {
        this.DataContext.TriggerTypeCode = code;
    };
    Object.defineProperty(AddEditRuleComponent.prototype, "SelectedTriggerType", {
        get: function () {
            var _this = this;
            if (this.DataContext) {
                this.selectedTriggerType = this.TriggerTypes.filter(function (t) { return t.Code === _this.DataContext.TriggerTypeCode; })[0];
            }
            return this.selectedTriggerType;
        },
        set: function (newValue) {
            if (this.selectedTriggerType != newValue) {
                this.selectedTriggerType = newValue;
                this.DataContext.TriggerTypeCode = newValue.Code;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditRuleComponent.prototype.onNotificationTypeSelected = function (code) {
        this.DataContext.RuleNotificationTypeCode = code;
    };
    Object.defineProperty(AddEditRuleComponent.prototype, "SelectedRuleNotificationTypeCode", {
        get: function () {
            var _this = this;
            if (this.DataContext) {
                this.selectedRuleNotificationTypeCode = this.RuleNotificationTypes.filter(function (t) { return t.Code === _this.DataContext.RuleNotificationTypeCode; })[0];
            }
            return this.selectedRuleNotificationTypeCode;
        },
        set: function (newValue) {
            if (this.selectedRuleNotificationTypeCode != newValue) {
                this.selectedRuleNotificationTypeCode = newValue;
                this.DataContext.RuleNotificationTypeCode = newValue.Code;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditRuleComponent.prototype.onAddFieldTagClicked = function () {
        this.AddDataField();
    };
    AddEditRuleComponent.prototype.AddDataField = function () {
        var _this = this;
        var tableName = "";
        var tableId = this.ObjectTableId;
        var table = window.ObjectTables.filter(function (d) { return d.Id == tableId; })[0];
        if (table)
            tableName = table.Name;
        this._entityResourceService.getEntityResourceByTableName("SystemData").subscribe(function (response) {
            if (table) {
                _this._entityResourceService.getEntityResourceByTableName(tableName).subscribe(function (response) {
                    _this.ViewDataField(tableId);
                });
            }
            else
                _this.ViewDataField(tableId);
        });
    };
    AddEditRuleComponent.prototype.ViewDataField = function (tableId) {
        var _this = this;
        var windowArgs = {};
        windowArgs.ObjectTableId = tableId;
        windowArgs.ObjectTypeField = "";
        windowArgs.HideSystemDataTab = true;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 600;
        logWindow.Title = "Insert Data Field";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocumentObjectFieldsComponent');
        logWindow.WindowClosed.subscribe(function ($event) {
            if ($event) {
                _this.DataContext.Condition = insertAtSubject(_this.ConditionTXTAreaId, $event);
            }
        });
    };
    AddEditRuleComponent.prototype.SaveClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if (this.DataContext.TriggerTypeCode == "COND") {
            if (this.DataContext.AdvancedCondition) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.DataContext.Condition)) {
                    this.ValidationErrorsList.push("Condition field is required");
                }
            }
            else {
                if (this.SelectedConditionObjectFields.length == 0) {
                    this.ValidationErrorsList.push("Condition field is required");
                }
            }
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.DataContext.RuleCode)) {
            this.ValidationErrorsList.push("Code field is required");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.DataContext.Name)) {
            this.ValidationErrorsList.push("Name field is required");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.DataContext.RuleTypeCode)) {
            this.ValidationErrorsList.push("Rule Type is required");
        }
        if (this.DataContext.TriggerTypeCode == "FLDC") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.DataContext.TriggerFieldId)) {
                this.ValidationErrorsList.push("Trigger Field is required");
            }
        }
        if (this.ValidationErrorsList.length != 0) {
            return;
        }
        if (this.IsNewEntity) {
            this.SelectedConditionObjectFields.forEach(function (item, key) {
                if (item.RuleConditionFieldPM) {
                    item.RuleConditionFieldPM.Value = FieldValueResolver_1.FieldValueResolver.GetFieldStringValue(item.ObjectField, item.TextValue);
                    item.RuleConditionFieldPM.Operator = item.Operation.Code;
                    item.RuleConditionFieldPM.ChangeSetOp = "Insert";
                }
                else {
                    var ruleConditionField = new RuleConditionFieldPM_1.RuleConditionFieldPM(_this.DataContext);
                    var newField = new RuleConditionFieldPM_1.RuleConditionFieldPM(_this.DataContext);
                    newField.ObjectFieldId = item.ObjectField.Id;
                    newField.ObjectFieldName = item.ObjectField.FieldName;
                    newField.Operator = item.Operation.Code;
                    newField.Value = FieldValueResolver_1.FieldValueResolver.GetFieldStringValue(item.ObjectField, item.TextValue);
                    newField.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    newField.ObjectTableRuleId = _this.DataContext.Id;
                    newField.ChangeSetOp = "Insert";
                    _this.DataContext.AddRuleConditionField(newField);
                }
            });
            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
            this._objectTableRulePMService.insert(this.DataContext).subscribe(function (resp) {
                if (!resp.HasError && resp.Result) {
                    var ruleFields = [];
                    _this.RuleFields.forEach(function (r) {
                        r.ObjectTableRuleId = resp.Result.Id;
                        r.ChangeSetOp = 'Insert';
                        ruleFields.push(r);
                    });
                    _this._objectTableRuleFieldPMService.updateRuleFieldsList(ruleFields).subscribe(function (resp2) {
                        _this.CurrentSession.CurrentWindow.Close('saved');
                    });
                }
                else {
                    _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    _this.ValidationErrorsList = resp.ErrorsArray;
                }
            });
        }
        else {
            this.SelectedConditionObjectFields.forEach(function (item, key) {
                if (item.RuleConditionFieldPM) {
                    item.RuleConditionFieldPM.Value = FieldValueResolver_1.FieldValueResolver.GetFieldStringValue(item.ObjectField, item.TextValue);
                    item.RuleConditionFieldPM.Operator = item.Operation.Code;
                    item.RuleConditionFieldPM.ChangeSetOp = "Update";
                }
                else {
                    var ruleConditionField = new RuleConditionFieldPM_1.RuleConditionFieldPM(_this.DataContext);
                    var newField = new RuleConditionFieldPM_1.RuleConditionFieldPM(_this.DataContext);
                    newField.ObjectFieldId = item.ObjectField.Id;
                    newField.ObjectFieldName = item.ObjectField.FieldName;
                    newField.Operator = item.Operation.Code;
                    newField.Value = FieldValueResolver_1.FieldValueResolver.GetFieldStringValue(item.ObjectField, item.TextValue);
                    newField.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    newField.ObjectTableRuleId = _this.DataContext.Id;
                    newField.ChangeSetOp = "Insert";
                    _this.DataContext.AddRuleConditionField(newField);
                }
            });
            this.RemovedConditionObjectFields.forEach(function (item, key) {
                if (item) {
                    _this.DataContext.RemoveRuleConditionField(item.RuleConditionFieldPM);
                }
            });
            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
            this._objectTableRulePMService.update(this.DataContext).subscribe(function (resp) {
                if (!resp.HasError) {
                    var ruleFields = [];
                    _this.RuleFields.forEach(function (r) {
                        if (r.ChangeSetOp != 'Insert') {
                            r.ChangeSetOp = 'Update';
                        }
                        ruleFields.push(r);
                    });
                    _this.removedFields.forEach(function (r) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(r.Id) && r.Id != 'New') {
                            r.ChangeSetOp = 'Delete';
                        }
                        ruleFields.push(r);
                    });
                    _this._objectTableRuleFieldPMService.updateRuleFieldsList(ruleFields).subscribe(function (resp2) {
                        _this.CurrentSession.CurrentWindow.Close('saved');
                    });
                }
                else {
                    _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    _this.ValidationErrorsList = resp.ErrorsArray;
                }
            });
        }
    };
    AddEditRuleComponent.prototype.CancelClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditRuleComponent.prototype.NextPageClicked = function () {
        if (this.PageNumber != 1) {
            this.PageNumber += 1;
        }
    };
    AddEditRuleComponent.prototype.PreviousPageClicked = function () {
        if (this.PageNumber != 0) {
            this.PageNumber -= 1;
        }
    };
    AddEditRuleComponent.prototype.onAddRuleFieldClicked = function () {
        var _this = this;
        var windowArgs = {};
        windowArgs.ObjectTableId = this.ObjectTable.Id;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Insert Rule Field";
        logWindow.WindowArgs = windowArgs;
        if (this.DataContext.RuleTypeCode === "SETV") {
            logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/RulesComponents/AddRuleFieldComponent');
            logWindow.Width = 700;
            logWindow.Height = 300;
        }
        else {
            logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/RulesComponents/ObjectFieldsSearchComponent');
            logWindow.Width = 450;
            logWindow.Height = 530;
        }
        logWindow.WindowClosed.subscribe(function ($event) {
            if ($event) {
                var expression = null;
                var objectFieldId = $event;
                if (_this.DataContext.RuleTypeCode == 'SETV') {
                    objectFieldId = $event.split(',')[0];
                    if ($event.split(',').length > 1) {
                        expression = $event.split(',')[1];
                    }
                }
                var selectedField = _this.ObjectFields.filter(function (f) { return f.Id == objectFieldId; })[0];
                if (selectedField) {
                    if (!_this.currentRuleFields.filter(function (f) { return f.ObjectFieldId == selectedField.Id; })[0]) {
                        var ruleField = null;
                        if (!_this.removedFields.filter(function (f) { return f.ObjectFieldId == selectedField.Id; })[0]) {
                            ruleField = new ObjectTableRuleFieldPM_1.ObjectTableRuleFieldPM();
                            ruleField.Id = 'New';
                            ruleField.ObjectFieldId = selectedField.Id;
                            ruleField.ObjectTableRuleId = _this.DataContext.Id;
                            ruleField.SystemLevel = false;
                            ruleField.Tenant = SessionLocator_1.SessionLocator.Tenant;
                            ruleField.ObjectFieldName = selectedField.FieldName;
                            ruleField.Expression = expression;
                            ruleField.ObjectTableRuleCode = _this.DataContext.RuleCode;
                            ruleField.ObjectTableRuleTypeCode = _this.DataContext.RuleTypeCode;
                            if (_this.DataContext.RuleTypeCode != 'SETV' && _this.DataContext.RuleTypeCode != 'BLCK') {
                                ruleField.RuleNotificationTypeCode = "ERR";
                            }
                            ruleField.ChangeSetOp = 'Insert';
                        }
                        else {
                            ruleField = _this.removedFields.filter(function (f) { return f.ObjectFieldId == selectedField.Id; })[0];
                            ruleField.Expression = expression;
                            var index = _this.removedFields.indexOf(ruleField);
                            if (index > -1) {
                                _this.removedFields.splice(index, 1);
                            }
                        }
                        _this.RuleFields.push(ruleField);
                        _this.currentRuleFields.push(ruleField);
                    }
                    else {
                        // SessionLocator.ShowSimplogMessageWindow("This Field already exists!");
                    }
                }
            }
        });
    };
    AddEditRuleComponent.prototype.OnDeleteRuleField = function (ruleField) {
        if (ruleField && !ruleField.SystemLevel) {
            this.FeildsChanged = true;
            var index = this.currentRuleFields.indexOf(ruleField);
            if (index > -1) {
                this.currentRuleFields.splice(index, 1);
            }
            var index = this.RuleFields.indexOf(ruleField);
            if (index > -1) {
                this.RuleFields.splice(index, 1);
            }
            this.removedFields.push(ruleField);
        }
    };
    AddEditRuleComponent.prototype.ClearPlaceHolder = function () {
        var temp = document.getElementById(this.SearchFieldsId);
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
    };
    AddEditRuleComponent.prototype.FillPlaceHolder = function () {
        var temp = document.getElementById(this.SearchFieldsId);
        temp.placeholder = "Search";
        temp.style.background = "url(Images/Search.png) no-repeat scroll";
        temp.style.backgroundPosition = "right center";
        temp.style.paddingRight = "30px";
    };
    AddEditRuleComponent.prototype.ClearFiltersPlaceHolder = function () {
        var temp = document.getElementById(this.FiltersSearchFieldsId);
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
    };
    AddEditRuleComponent.prototype.FillFiltersPlaceHolder = function () {
        var temp = document.getElementById(this.FiltersSearchFieldsId);
        temp.placeholder = "Search";
        temp.style.background = "url(Images/Search.png) no-repeat scroll";
        temp.style.backgroundPosition = "right center";
        temp.style.paddingRight = "30px";
    };
    Object.defineProperty(AddEditRuleComponent.prototype, "FilterssearchText", {
        get: function () { return this.filterssearchText; },
        set: function (newValue) {
            var _this = this;
            this.filterssearchText = newValue;
            if (newValue != null && newValue != "") {
                this.NEWallFilterFieldsClass.FilterFields = this.fieldsStaticList.filter(function (f) { return TextCodeTranslator_1.TextCodeTranslator.Translate(f.ObjectField.FullNameTextCodeCode).toLowerCase().indexOf(newValue.toLowerCase()) > -1; });
                this.SelectedConditionObjectFields.forEach(function (item, key) {
                    _this.NEWallFilterFieldsClass.SetExists(item, true);
                    _this.filterFields.SetExists(item, true);
                });
            }
            else {
                this.NEWallFilterFieldsClass.FilterFields = this.fieldsStaticList;
                this.SelectedConditionObjectFields.forEach(function (item, key) {
                    _this.NEWallFilterFieldsClass.SetExists(item, true);
                    _this.filterFields.SetExists(item, true);
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditRuleComponent.prototype.DeteteFilter = function (field) {
        //if (this.AdvancedQueryFilterPMs.filter(f => f.ObjectFieldId == field.ObjectField.Id && f.QueryId == this.QueryId)[0] != null) {
        //    var advanceFilter = this.AdvancedQueryFilterPMs.filter(f => f.ObjectFieldId == field.ObjectField.Id && f.QueryId == this.QueryId)[0];
        //var myService: AdvancedQueryFiltersPMService = new AdvancedQueryFiltersPMService();
        //myService.setServiceArgs(this.serviceArgs);
        //myService.delete(advanceFilter).subscribe(myResult => {
        if (this.SelectedConditionObjectFields != null) {
            var filter = this.SelectedConditionObjectFields.filter(function (a) { return a.ObjectField.Id == field.ObjectField.Id; })[0];
            if (filter) {
                this.RemovedConditionObjectFields.push(filter);
                this.SelectedConditionObjectFields = this.SelectedConditionObjectFields.filter(function (a) { return a.ObjectField.Id != field.ObjectField.Id; });
            }
        }
        if (this.NEWallFilterFieldsClass != null) {
            this.NEWallFilterFieldsClass.SetExists(field, false);
        }
        //});
        //}
    };
    AddEditRuleComponent.prototype.CLearAll = function () {
        var _this = this;
        if (this.NEWallFilterFieldsClass) {
            this.SelectedConditionObjectFields.forEach(function (field, key) {
                _this.NEWallFilterFieldsClass.SetExists(field, false);
            });
        }
        if (this.SelectedConditionObjectFields) {
            this.SelectedConditionObjectFields = [];
        }
    };
    AddEditRuleComponent.prototype.BuildQuestionTypes = function () {
        this.RuleTypes = [];
        var type1 = new RuleType("BLCK", "Block Field");
        this.RuleTypes.push(type1);
        var type2 = new RuleType("DUPL", "Field Duplication");
        this.RuleTypes.push(type2);
        var type3 = new RuleType("EVAL", "Entity Validation");
        this.RuleTypes.push(type3);
        var type4 = new RuleType("REQ", "Required");
        this.RuleTypes.push(type4);
        var type5 = new RuleType("SETV", "Set Field Value");
        this.RuleTypes.push(type5);
    };
    AddEditRuleComponent.prototype.BuildTriggerTypes = function () {
        this.TriggerTypes = [];
        var type1 = new TriggerType("ALLW", "Always");
        this.TriggerTypes.push(type1);
        var type2 = new TriggerType("COND", "Condition");
        this.TriggerTypes.push(type2);
        var type3 = new TriggerType("FLDC", "Field Changed");
        if (this.DataContext) {
            if (this.DataContext.RuleTypeCode != 'DUPL') {
                this.TriggerTypes.push(type3);
            }
        }
        else {
            this.TriggerTypes.push(type3);
        }
    };
    AddEditRuleComponent.prototype.BuildNotificationTypes = function () {
        this.RuleNotificationTypes = [];
        var type1 = new RuleNotificationType("ERR", "Error");
        this.RuleNotificationTypes.push(type1);
        var type2 = new RuleNotificationType("WAR", "Warning");
        this.RuleNotificationTypes.push(type2);
    };
    AddEditRuleComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditRuleComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditRuleComponent);
    return AddEditRuleComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditRuleComponent = AddEditRuleComponent;
var RuleType = /** @class */ (function () {
    function RuleType(Code, Name) {
        this.Code = Code;
        this.Name = Name;
    }
    return RuleType;
}());
exports.RuleType = RuleType;
var TriggerType = /** @class */ (function () {
    function TriggerType(Code, Name) {
        this.Code = Code;
        this.Name = Name;
    }
    return TriggerType;
}());
exports.TriggerType = TriggerType;
var RuleNotificationType = /** @class */ (function () {
    function RuleNotificationType(Code, Name) {
        this.Code = Code;
        this.Name = Name;
    }
    return RuleNotificationType;
}());
exports.RuleNotificationType = RuleNotificationType;
//# sourceMappingURL=AddEditRuleComponent.js.map