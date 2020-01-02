import {Component} from '@angular/core';
import {GeneralDomainService, FieldsTranslations} from '../../../../../Infrastructure/Services/GeneralDomainService';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {ObjectTablePM} from '../../../../../Infrastructure/EntityPMs/ObjectTablePM';
import {ObjectFieldPM} from '../../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {TextCodePM} from '../../../../../Infrastructure/EntityPMs/TextCodePM';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {ObjectTableRulePMService} from '../../../../../Infrastructure/Services/StandardPMs/ObjectTableRulePMService';
import {ObjectTableRuleFieldPMService} from '../../../../../Infrastructure/Services/StandardPMs/ObjectTableRuleFieldPMService';
import {ObjectTableRulePM} from '../../../../../Infrastructure/EntityPMs/ObjectTableRulePM';
import {RuleConditionFieldPM} from '../../../../../Infrastructure/EntityPMs/RuleConditionFieldPM';
import {ObjectTableRuleFieldPM} from '../../../../../Infrastructure/EntityPMs/ObjectTableRuleFieldPM';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import {Guid} from '../../../../../Infrastructure/Utilities/Guid';
import {FieldValueResolver} from '../../../../../Infrastructure/Utilities/FieldValueResolver';
import {ApiQueryFilters} from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ConditionFilterField, ConditionFilterFieldsClass, FieldsValues} from './ConditionFilterField';
declare var window: any;
declare var insertAtSubject;

@Component({
    moduleId: module.id,
    templateUrl: './AddEditRuleComponent.html',
})
export class AddEditRuleComponent extends BaseComponent {
    FieldsLovQueryFilters: ApiQueryFilters;
    public DataContext: ObjectTableRulePM;
    public ValidationErrorsList: string[] = [];
    private _objectTableRuleFieldPMService: ObjectTableRuleFieldPMService = new ObjectTableRuleFieldPMService();
    private _objectTableRulePMService: ObjectTableRulePMService = new ObjectTableRulePMService();
    private _entityResourceService: EntityResourceService = new EntityResourceService();

    public IsResourcesReady: boolean = false;
    private ObjectTableId: string;
    private IsNewEntity: boolean = false;
    private IsCopyFromSystemRule: boolean = false;
    public ConditionTXTAreaId: string;

    
    public RuleTypes: RuleType[] = [];
    public TriggerTypes: TriggerType[] = [];
    public RuleNotificationTypes: RuleNotificationType[] = [];

    public ObjectTable: ObjectTablePM;
    public ObjectFields: ObjectFieldPM[] = [];
    public PageNumber: number = 0;
    private FeildsChanged: boolean = false;
    NEWallFilterFieldsClass: ConditionFilterFieldsClass;
    filterFields: ConditionFilterFieldsClass;
 
    allFilterFields: ObjectFieldPM[]; 
    constantFilterFieldsList: ObjectFieldPM[];
    CurrentFilters: ObjectFieldPM[];//
    fieldsStaticList: ConditionFilterField[];//
    public FieldsValues: FieldsValues;
    SearchFieldsId: string;
    FiltersSearchFieldsId: string;
    
    public RuleFields: ObjectTableRuleFieldPM[] = [];
    private currentRuleFields: ObjectTableRuleFieldPM[] = [];
    private removedFields: ObjectTableRuleFieldPM[] = [];
    public BooleanValues = ["True", "False"];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
       
        this.BuildQuestionTypes();
        this.BuildTriggerTypes();
        this.BuildNotificationTypes();
        this.ConditionTXTAreaId = Guid.newGuid();
        this.FieldsValues = new FieldsValues();
        this.RemovedConditionObjectFields = [];

        if (this.CurrentSession == null) {
            this.SearchFieldsId = "RuleSearchFields_-1_-1";
            this.FiltersSearchFieldsId = "RuleFiltersSearchFieldsId_-1_-1";
        }

        else {
            this.SearchFieldsId = "NewRuleSearchFields_" + this.CurrentSession.GetNewId("NewRuleSearchFields");
            this.FiltersSearchFieldsId = "NewRuleFiltersSearchFieldsId_" + this.CurrentSession.GetNewId("NewRuleFiltersSearchFieldsId");
        }
    }

    //SetDataContext(dataContext: ObjectTableRulePM) {
    //    this.DataContext = dataContext;
    //}

 
    SetWindowArgs(windowArgs: any) {
        this.ObjectTableId = windowArgs.ObjectTableId;

        this.IsNewEntity = windowArgs.IsNewEntity;
        if (this.IsNewEntity) {
            this.DataContext = new ObjectTableRulePM();
            this.DataContext.Tenant = SessionLocator.Tenant;
            this.DataContext.ObjectTableId = this.ObjectTableId;
            this.DataContext.RuleNotificationTypeCode = "WAR";
        }
        else {

            if (windowArgs.EntityPM.SystemLevel && windowArgs.EntityPM.Tenant == 0 && SessionLocator.Tenant != 0) {
                let tenantLevelRule: ObjectTableRulePM = this.CreateTenantLevelRule(windowArgs.EntityPM);

                this.DataContext = tenantLevelRule;


            }
            else {
                this.DataContext = windowArgs.EntityPM;
                this.currentRuleFields = window.ObjectTableRuleFields.filter(r => r.ObjectTableRuleId === this.DataContext.Id);//
                this.RuleFields = window.ObjectTableRuleFields.filter(r => r.ObjectTableRuleId === this.DataContext.Id);
            }
        }

        this.ObjectTable = window.ObjectTables.filter((d: any) => d.Id == this.ObjectTableId)[0];
        this.ObjectTableId = this.ObjectTable.Id;
        this.ObjectFields = window.ObjectFields.filter((d: any) => d.ObjectTableId === this.ObjectTableId && !AppTool.IsNullOrEmpty(d.PMPropertyPath) && !d.DisplayOnly && !d.IsMulti);//!d.IsCustomFilter             
        this.ObjectFields = this.ObjectFields.sort((a, b) => { return (a.FieldName.toLowerCase() === b.FieldName.toLowerCase()) ? 0 : (a.FieldName.toLowerCase() < b.FieldName.toLowerCase()) ? -1 : 1 });

        this.FieldsLovQueryFilters = new ApiQueryFilters();
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

        this.filterFields = new ConditionFilterFieldsClass(true, this, null);//this.pubSubAdvanceQueryFiltersService
        this.NEWallFilterFieldsClass = new ConditionFilterFieldsClass(true, this, null);//this.pubSubAdvanceQueryFiltersService

        if (this.ObjectFields) {

            this.allFilterFields = this.ObjectFields.filter(o => o.DataTypeCode != "Constant");

            this.constantFilterFieldsList = this.ObjectFields.filter(o => o.DataTypeCode == "Constant");

            //this.timeFilterFieldsClass.AddFiltersList(window.ObjectFields.filter(o => o.CanFilter == true && o.FieldName != "TimeFrameFilter" && o.IsTimeFrameFilter == true && (o.ValidForQuerySection1 == this.currentQuery.QuerySection || o.ValidForQuerySection2 == this.currentQuery.QuerySection)), this.currentQuery.Id, myResult);

            this.NEWallFilterFieldsClass.AddFiltersList(this.ObjectFields.filter(o => o.DataTypeCode != "Constant"), null, []);

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
            var objectField: ObjectFieldPM = this.ObjectFields.filter(f => f.FieldCode == field.ObjectFieldCode)[0];
            if (objectField) {
                var mappedField = this.MapJsonToEntityPM(objectField);
                this.AddFilterField(mappedField);
            }
        }



    }

    private CreateTenantLevelRule(systemLevelRule: ObjectTableRulePM) {
        this.IsCopyFromSystemRule = true;
        let tenantLevelRule: ObjectTableRulePM = new ObjectTableRulePM();
        tenantLevelRule.RuleTypeCode = systemLevelRule.RuleTypeCode;
        tenantLevelRule.RuleCode = systemLevelRule.RuleCode;
        tenantLevelRule.RuleNotificationTypeCode = systemLevelRule.RuleNotificationTypeCode;
        tenantLevelRule.TriggerTypeCode = systemLevelRule.TriggerTypeCode;
        tenantLevelRule.TriggerFieldId = systemLevelRule.TriggerFieldId;
        tenantLevelRule.ActiveForNew = systemLevelRule.ActiveForNew;
        tenantLevelRule.ActiveForUpdate = systemLevelRule.ActiveForUpdate;
        tenantLevelRule.AdvancedCondition = systemLevelRule.AdvancedCondition;
        tenantLevelRule.Condition = systemLevelRule.Condition;
        tenantLevelRule.Name = systemLevelRule.Name;
        tenantLevelRule.ObjectTableId = systemLevelRule.ObjectTableId;
        tenantLevelRule.OutputMessage = systemLevelRule.OutputMessage;
        tenantLevelRule.RuleTypeName = systemLevelRule.RuleTypeName;
        tenantLevelRule.Tenant = SessionLocator.Tenant;
        systemLevelRule.RuleConditionFields.forEach(item => {
            var newField: RuleConditionFieldPM = new RuleConditionFieldPM(tenantLevelRule);
            newField.ObjectFieldId = item.ObjectFieldId;
            newField.ObjectFieldCode = item.ObjectFieldCode;
            newField.ObjectFieldName = item.ObjectFieldName;
            newField.Operator = item.Operator;
            newField.Value = item.Value;
            newField.Tenant = SessionLocator.Tenant;
            newField.ObjectTableRuleId = tenantLevelRule.Id;
            newField.ChangeSetOp = "Insert";
            tenantLevelRule.AddRuleConditionField(newField);
        });
        let zeroRuleFields: ObjectTableRuleFieldPM[] = window.ObjectTableRuleFields.filter(r => r.ObjectTableRuleId === systemLevelRule.Id && r.Tenant == 0);
        zeroRuleFields.forEach(item => {
            let ruleField: ObjectTableRuleFieldPM = new ObjectTableRuleFieldPM();
            ruleField.Id = 'New';
            ruleField.ObjectFieldId = item.ObjectFieldId;
            ruleField.ObjectFieldCode = item.ObjectFieldCode;
            ruleField.ObjectTableRuleId = tenantLevelRule.Id;
            ruleField.SystemLevel = false;
            ruleField.Tenant = SessionLocator.Tenant;
            ruleField.ObjectFieldName = item.ObjectFieldName;
            ruleField.Expression = item.Expression;
            ruleField.ObjectTableRuleCode = item.ObjectTableRuleCode;
            ruleField.ObjectTableRuleTypeCode = item.ObjectTableRuleTypeCode;
            ruleField.RuleNotificationTypeCode = item.RuleNotificationTypeCode;
            ruleField.ChangeSetOp = 'Insert';
            this.RuleFields.push(ruleField);
            this.currentRuleFields.push(ruleField);
        });
        return tenantLevelRule;
    }

    //LoadRuleFields() {
    //    this._objectTableRuleFieldPMService.getAllByTenant
    //}


    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true) {
        if (jsonPM) {
            var entityPM: ObjectFieldPM;
            entityPM = new ObjectFieldPM();
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
    }

    SelectedConditionObjectFields: ConditionFilterField[] = [];
    RemovedConditionObjectFields: ConditionFilterField[] = [];
    public AddFilterField(field: ObjectFieldPM) {

        if (this.SelectedConditionObjectFields == undefined) {
            this.SelectedConditionObjectFields = [];
        }
        var conditionField = this.DataContext.RuleConditionFields.filter(d => d.ObjectFieldCode == field.FieldCode)[0];
        if (conditionField) {
            var value = conditionField.Value;
            this.FieldsValues.SetFieldValue(field.Id, value);
        }
        var mappedField = this.MapJsonToEntityPM(field);

        var filter: ConditionFilterField = this.RemovedConditionObjectFields.filter(a => a.ObjectField.Id === field.Id)[0];
        if (filter) {
            this.SelectedConditionObjectFields.push(filter);//
            this.RemovedConditionObjectFields = this.RemovedConditionObjectFields.filter(a => a.ObjectField.Id != filter.ObjectField.Id);
        }
        else {
            this.SelectedConditionObjectFields.push(new ConditionFilterField(mappedField, this.DataContext.Id, true, this.DataContext.RuleConditionFields, this, null));//
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
    }

    //public RemoveFilterField(field: ObjectFieldPM) {
    //    if (this.SelectedConditionObjectFields != null) {
    //        this.SelectedConditionObjectFields = this.SelectedConditionObjectFields.filter(a => a.ObjectField.Id != field.Id);
    //    }
    //}



    private selectedRuleType: RuleType;
    public get SelectedRuleType() {
        if (this.DataContext) {
            this.selectedRuleType = this.RuleTypes.filter(t => t.Code === this.DataContext.RuleTypeCode)[0];
        }

        return this.selectedRuleType;

    }
    public set SelectedRuleType(newValue: RuleType) {
        if (this.selectedRuleType != newValue) {
            this.selectedRuleType = newValue;
            this.DataContext.RuleTypeCode = newValue.Code;
            this.BuildTriggerTypes();
         
        }
    }

    onTypeSelected(code: string) {
        this.DataContext.RuleTypeCode = code;
    }



    onTriggerTypeSelected(code: string) {
        this.DataContext.TriggerTypeCode = code;
    }

    private selectedTriggerType: TriggerType;
    public get SelectedTriggerType() {
        if (this.DataContext) {
            this.selectedTriggerType = this.TriggerTypes.filter(t => t.Code === this.DataContext.TriggerTypeCode)[0];
        }

        return this.selectedTriggerType;

    }
    public set SelectedTriggerType(newValue: TriggerType) {
        if (this.selectedTriggerType != newValue) {
            this.selectedTriggerType = newValue;
            this.DataContext.TriggerTypeCode = newValue.Code;

        }
    }




    onNotificationTypeSelected(code: string) {
        this.DataContext.RuleNotificationTypeCode = code;
    }

    private selectedRuleNotificationTypeCode: RuleNotificationType;
    public get SelectedRuleNotificationTypeCode() {
        if (this.DataContext) {
            this.selectedRuleNotificationTypeCode = this.RuleNotificationTypes.filter(t => t.Code === this.DataContext.RuleNotificationTypeCode)[0];
        }

        return this.selectedRuleNotificationTypeCode;

    }
    public set SelectedRuleNotificationTypeCode(newValue: RuleNotificationType) {
        if (this.selectedRuleNotificationTypeCode != newValue) {
            this.selectedRuleNotificationTypeCode = newValue;
            this.DataContext.RuleNotificationTypeCode = newValue.Code;

        }
    }

    onAddFieldTagClicked() {
        this.AddDataField();
    }



    AddDataField() {
        var tableName = "";
        var tableId: string = this.ObjectTableId;
        var table = window.ObjectTables.filter(d => d.Id == tableId)[0];
        if (table) tableName = table.Name;

        this._entityResourceService.getEntityResourceByTableName("SystemData").subscribe(response => {

            if (table) {
                this._entityResourceService.getEntityResourceByTableName(tableName).subscribe(response => {
                    this.ViewDataField(tableId);
                });
            }
            else this.ViewDataField(tableId);

        });


    }


    ViewDataField(tableId: string) {

        var windowArgs: any = {};
        windowArgs.ObjectTableId = tableId;
        windowArgs.ObjectTypeField = "";
        windowArgs.HideSystemDataTab = true;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 600;

        logWindow.Title = "Insert Data Field";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocumentObjectFieldsComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {

            if ($event) {
                this.DataContext.Condition = insertAtSubject(this.ConditionTXTAreaId, $event);
           
            }

        });
    }


    SaveClicked() {

        this.ValidationErrorsList = [];

        if (this.DataContext.TriggerTypeCode == "COND") {
            if (this.DataContext.AdvancedCondition) {
                if (AppTool.IsNullOrEmpty(this.DataContext.Condition)) {
                    this.ValidationErrorsList.push("Condition field is required");
                }
            }
            else {

                if (this.SelectedConditionObjectFields.length == 0) {
                    this.ValidationErrorsList.push("Condition field is required");
                }
            }
        }

        if (AppTool.IsNullOrEmpty(this.DataContext.RuleCode)) {
            this.ValidationErrorsList.push("Code field is required");
        }

        if (AppTool.IsNullOrEmpty(this.DataContext.Name)) {
            this.ValidationErrorsList.push("Name field is required");
        }

        if (AppTool.IsNullOrEmpty(this.DataContext.RuleTypeCode)) {
            this.ValidationErrorsList.push("Rule Type is required");
        }

        if (this.DataContext.TriggerTypeCode == "FLDC") {
            if (AppTool.IsNullOrEmpty(this.DataContext.TriggerFieldId)) {
                this.ValidationErrorsList.push("Trigger Field is required");
            }
        }
        if (AppTool.IsNullOrEmpty(this.DataContext.TriggerTypeCode)) {
            this.ValidationErrorsList.push("Trigger Type Field is required");
        }
        if (this.ValidationErrorsList.length != 0) {
            return;
        }

        if (this.IsNewEntity) {
            this.SaveNewRule();
        }
        else if (this.IsCopyFromSystemRule) {
            this.SaveNewRule();
            //this.SaveNewRuleFromSystemRule();
        } else {

            this.SaveEditedTenantRule();
        }
    }
    SaveNewRuleFromSystemRule() {
         
    }


    private SaveEditedTenantRule() {
        this.SelectedConditionObjectFields.forEach((item, key) => {
            if (item.RuleConditionFieldPM) {
                item.RuleConditionFieldPM.Value = FieldValueResolver.GetFieldStringValue(item.ObjectField, item.TextValue);
                item.RuleConditionFieldPM.Operator = item.Operation.Code;
                item.RuleConditionFieldPM.ChangeSetOp = "Update";
            }
            else {
                var ruleConditionField = new RuleConditionFieldPM(this.DataContext);
                var newField: RuleConditionFieldPM = new RuleConditionFieldPM(this.DataContext);
                newField.ObjectFieldId = item.ObjectField.Id;
                newField.ObjectFieldCode = item.ObjectField.FieldCode;
                newField.ObjectFieldName = item.ObjectField.FieldName;
                newField.Operator = item.Operation.Code;
                newField.Value = FieldValueResolver.GetFieldStringValue(item.ObjectField, item.TextValue);
                newField.Tenant = SessionLocator.Tenant;
                newField.ObjectTableRuleId = this.DataContext.Id;
                newField.ChangeSetOp = "Insert";
                this.DataContext.AddRuleConditionField(newField);
            }
        });
        this.RemovedConditionObjectFields.forEach((item, key) => {
            if (item) {
                this.DataContext.RemoveRuleConditionField(item.RuleConditionFieldPM);
            }
        });
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        this._objectTableRulePMService.update(this.DataContext).subscribe(resp => {
            if (!resp.HasError) {
                var ruleFields: ObjectTableRuleFieldPM[] = [];
                this.RuleFields.forEach(r => {
                    if (r.ChangeSetOp != 'Insert') {
                        r.ChangeSetOp = 'Update';
                    }
                    ruleFields.push(r);
                });
                this.removedFields.forEach(r => {
                    if (!AppTool.IsNullOrEmpty(r.Id) && r.Id != 'New') {
                        r.ChangeSetOp = 'Delete';
                    }
                    ruleFields.push(r);
                });
                this._objectTableRuleFieldPMService.updateRuleFieldsList(ruleFields).subscribe(resp2 => {
                    this.CurrentSession.CurrentWindow.Close('saved');
                });
            }
            else {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                this.ValidationErrorsList = resp.ErrorsArray;
            }
        });
    }

    private SaveNewRule() {
        this.SelectedConditionObjectFields.forEach((item, key) => {
            if (item.RuleConditionFieldPM) {
                item.RuleConditionFieldPM.Value = FieldValueResolver.GetFieldStringValue(item.ObjectField, item.TextValue);
                item.RuleConditionFieldPM.Operator = item.Operation.Code;
                item.RuleConditionFieldPM.ChangeSetOp = "Insert";
            }
            else {
                var ruleConditionField = new RuleConditionFieldPM(this.DataContext);
                var newField: RuleConditionFieldPM = new RuleConditionFieldPM(this.DataContext);
                newField.ObjectFieldId = item.ObjectField.Id;
                newField.ObjectFieldCode = item.ObjectField.FieldCode;
                newField.ObjectFieldName = item.ObjectField.FieldName;
                newField.Operator = item.Operation.Code;
                newField.Value = FieldValueResolver.GetFieldStringValue(item.ObjectField, item.TextValue);
                newField.Tenant = SessionLocator.Tenant;
                newField.ObjectTableRuleId = this.DataContext.Id;
                newField.ChangeSetOp = "Insert";
                this.DataContext.AddRuleConditionField(newField);
            }
        });
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        this._objectTableRulePMService.insert(this.DataContext).subscribe(resp => {
            if (!resp.HasError && resp.Result) {
                var ruleFields: ObjectTableRuleFieldPM[] = [];
                this.RuleFields.forEach(r => {
                    r.ObjectTableRuleId = resp.Result.Id;
                    r.ChangeSetOp = 'Insert';
                    ruleFields.push(r);
                });
                this._objectTableRuleFieldPMService.updateRuleFieldsList(ruleFields).subscribe(resp2 => {
                    this.CurrentSession.CurrentWindow.Close('saved');
                });
            }
            else {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                this.ValidationErrorsList = resp.ErrorsArray;
            }
        });
    }

    CancelClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    
    NextPageClicked() {
        if (this.PageNumber != 1) {
            this.PageNumber += 1;
        }
    }

    PreviousPageClicked() {
        if (this.PageNumber != 0) {
            this.PageNumber -= 1;
        }
    }

    onAddRuleFieldClicked() {

        var windowArgs: any = {};
        windowArgs.ObjectTableId = this.ObjectTable.Id;

        var logWindow = new LogitudeWindow();


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

        logWindow.WindowClosed.subscribe(($event: string) => {

            if ($event) {

                var expression = null;
                var objectFieldCode = $event;
                if (this.DataContext.RuleTypeCode == 'SETV') {
                    objectFieldCode = $event.split(',')[0];
                    if ($event.split(',').length > 1) {
                        expression = $event.split(',')[1];
                    }
                }
                var selectedField: ObjectFieldPM = this.ObjectFields.filter(f => f.FieldCode == objectFieldCode)[0];

                if (selectedField) {
                    if (!this.currentRuleFields.filter(f => f.ObjectFieldCode == selectedField.FieldCode)[0]) {
                        var ruleField: ObjectTableRuleFieldPM = null;

                        if (!this.removedFields.filter(f => f.ObjectFieldCode == selectedField.FieldCode)[0]) {
                           
                            ruleField = new ObjectTableRuleFieldPM();
                            ruleField.Id = 'New';
                            ruleField.ObjectFieldId = selectedField.Id;
                            ruleField.ObjectFieldCode = selectedField.FieldCode;
                            ruleField.ObjectTableRuleId = this.DataContext.Id;
                            ruleField.SystemLevel = false;
                            ruleField.Tenant = SessionLocator.Tenant;
                            ruleField.ObjectFieldName = selectedField.FieldName;
                            ruleField.Expression = expression;
                            ruleField.ObjectTableRuleCode = this.DataContext.RuleCode;
                            ruleField.ObjectTableRuleTypeCode = this.DataContext.RuleTypeCode;
                            if (this.DataContext.RuleTypeCode != 'SETV' && this.DataContext.RuleTypeCode != 'BLCK') {
                                ruleField.RuleNotificationTypeCode = "ERR";
                            }
                            ruleField.ChangeSetOp = 'Insert';

                        }

                        else {
                            ruleField = this.removedFields.filter(f => f.ObjectFieldCode == selectedField.FieldCode)[0];
                            ruleField.Expression = expression;
                            var index = this.removedFields.indexOf(ruleField);
                            if (index > -1) {
                                this.removedFields.splice(index, 1);

                            }

                        }
                        this.RuleFields.push(ruleField);
                        this.currentRuleFields.push(ruleField);
                    }
                    else {
                        // SessionLocator.ShowSimplogMessageWindow("This Field already exists!");
                    }

                }
            }

        });
    }


    OnDeleteRuleField(ruleField: ObjectTableRuleFieldPM) {
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
    }

    ClearPlaceHolder() {
        var temp = document.getElementById(this.SearchFieldsId) as HTMLInputElement;
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
    }

    FillPlaceHolder() {
        var temp = document.getElementById(this.SearchFieldsId) as HTMLInputElement;
        temp.placeholder = "Search";
        temp.style.background = "url(Images/Search.png) no-repeat scroll";
        temp.style.backgroundPosition = "right center";
        temp.style.paddingRight = "30px";
    }

    ClearFiltersPlaceHolder() {
        var temp = document.getElementById(this.FiltersSearchFieldsId) as HTMLInputElement;
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
    }

    FillFiltersPlaceHolder() {
        var temp = document.getElementById(this.FiltersSearchFieldsId) as HTMLInputElement;
        temp.placeholder = "Search";
        temp.style.background = "url(Images/Search.png) no-repeat scroll";
        temp.style.backgroundPosition = "right center";
        temp.style.paddingRight = "30px";
    }


    private filterssearchText: string;
    public get FilterssearchText() { return this.filterssearchText; }
    public set FilterssearchText(newValue: string) {
        this.filterssearchText = newValue;
        if (newValue != null && newValue != "") {

            this.NEWallFilterFieldsClass.FilterFields = this.fieldsStaticList.filter(f => TextCodeTranslator.Translate(f.ObjectField.FullNameTextCodeCode).toLowerCase().indexOf(newValue.toLowerCase()) > -1);
            this.SelectedConditionObjectFields.forEach((item, key) => {
                this.NEWallFilterFieldsClass.SetExists(item, true);
                this.filterFields.SetExists(item, true);
            });

        }
        else {
            this.NEWallFilterFieldsClass.FilterFields = this.fieldsStaticList;
            this.SelectedConditionObjectFields.forEach((item, key) => {
                this.NEWallFilterFieldsClass.SetExists(item, true);
                this.filterFields.SetExists(item, true);
            });
        }


    }

    public DeteteFilter(field: ConditionFilterField) {
        //if (this.AdvancedQueryFilterPMs.filter(f => f.ObjectFieldId == field.ObjectField.Id && f.QueryId == this.QueryId)[0] != null) {
        //    var advanceFilter = this.AdvancedQueryFilterPMs.filter(f => f.ObjectFieldId == field.ObjectField.Id && f.QueryId == this.QueryId)[0];
        //var myService: AdvancedQueryFiltersPMService = new AdvancedQueryFiltersPMService();
        //myService.setServiceArgs(this.serviceArgs);
        //myService.delete(advanceFilter).subscribe(myResult => {
        if (this.SelectedConditionObjectFields != null) {
            var filter: ConditionFilterField = this.SelectedConditionObjectFields.filter(a => a.ObjectField.Id == field.ObjectField.Id)[0];
            if (filter) {
                this.RemovedConditionObjectFields.push(filter);
                this.SelectedConditionObjectFields = this.SelectedConditionObjectFields.filter(a => a.ObjectField.Id != field.ObjectField.Id);
            }
        }
        if (this.NEWallFilterFieldsClass != null) {
            this.NEWallFilterFieldsClass.SetExists(field, false);
        }
        //});
        //}
    }

    public CLearAll() {

        if (this.NEWallFilterFieldsClass) {
            this.SelectedConditionObjectFields.forEach((field, key) => {
                this.NEWallFilterFieldsClass.SetExists(field, false);
            });
           
        }
        if (this.SelectedConditionObjectFields) {
            this.SelectedConditionObjectFields = [];
        }

    }

    BuildQuestionTypes() {

        this.RuleTypes = [];

         
        var type1: RuleType = new RuleType("BLCK", "Block Field");
        this.RuleTypes.push(type1);

        var type2: RuleType = new RuleType("DUPL", "Field Duplication");
        this.RuleTypes.push(type2);

        var type3: RuleType = new RuleType("EVAL", "Entity Validation");
        this.RuleTypes.push(type3);

        var type4: RuleType = new RuleType("REQ", "Required");
        this.RuleTypes.push(type4);

        var type5: RuleType = new RuleType("SETV", "Set Field Value");
        this.RuleTypes.push(type5);

       
    }

    BuildTriggerTypes() {

        this.TriggerTypes = [];


        var type1: TriggerType = new TriggerType("ALLW", "Always");
        this.TriggerTypes.push(type1);

        var type2: TriggerType = new TriggerType("COND", "Condition");
        this.TriggerTypes.push(type2);
        var type3: TriggerType = new TriggerType("FLDC", "Field Changed");
        if (this.DataContext) {
            if (this.DataContext.RuleTypeCode != 'DUPL') {
                this.TriggerTypes.push(type3);
            }
        } else {
            this.TriggerTypes.push(type3);
        }

    }

    BuildNotificationTypes() {

        this.RuleNotificationTypes = [];


        var type1: RuleNotificationType = new RuleNotificationType("ERR", "Error");
        this.RuleNotificationTypes.push(type1);

        var type2: RuleNotificationType = new RuleNotificationType("WAR", "Warning");
        this.RuleNotificationTypes.push(type2);
 

    }

 
}
 

export class RuleType {
    constructor(public Code: string, public Name: string) {

    }
}

export class TriggerType {
    constructor(public Code: string, public Name: string) {

    }
}

export class RuleNotificationType {
    constructor(public Code: string, public Name: string) {

    }

}


