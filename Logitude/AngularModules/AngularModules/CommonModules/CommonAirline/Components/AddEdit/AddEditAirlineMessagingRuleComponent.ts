import {Component} from '@angular/core';
import {AirlinePM} from '../../../../Common/EntityPMs/AirlinePM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {CodeNameClass} from '../../../../Infrastructure/DataContracts/CodeNameClass';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {CachedDataManager} from '../../../../Infrastructure/Utilities/CachedDataManager';
import {AirlineMessagingRulePM} from '../../../../Common/EntityPMs/AirlineMessagingRulePM';
import {AirlineMessagingRulePMService} from '../../../../Common/Services/StandardPMs/AirlineMessagingRulePMService';
import {ObjectFieldPM} from '../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {ObjectTablePM} from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

declare var window: any;

@Component({
    moduleId: module.id,
    templateUrl: './AddEditAirlineMessagingRuleComponent.html',
})

export class AddEditAirlineMessagingRuleComponent extends BaseComponent {
    public AirlinePM: AirlinePM;
    public EntityPM: AirlineMessagingRulePM;
    public ObjectTableName: string;
    public DataContext: AddEditAirlineMessagingRuleComponent = this;
    public IsNew: boolean;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetWindowArgs(windowArgs: any) {
        this.AirlinePM = windowArgs['AirlinePM'];
        this.EntityPM = windowArgs['EntityPM'];
        this.IsNew = windowArgs['IsNew'];
        this.ObjectTableName = windowArgs['ObjectTableName'];
        this.SetUIProperties();
        this.BuildMessageTypesList();
        this.Clone();
    }

    SetUIProperties() {
        this.UIProperties.SetVisibility("InActive", this.ObjectTableName, !this.IsNew);
        this.UIProperties.SetRequired("MessageTypeCode", this.ObjectTableName, AppTool.IsNullOrEmpty(this.MessageTypeCode));
    }

    public MessageTypesList: CodeNameClass[];
    private BuildMessageTypesList() {
        this.MessageTypesList = [];
        this.MessageTypesList.push(new CodeNameClass("FWB", "FWB"));
        this.MessageTypesList.push(new CodeNameClass("FHL", "FHL"));
        this.MessageTypesList.push(new CodeNameClass("FFR", "FFR"));
        this.MessageTypesList.push(new CodeNameClass("FVR", "FVR"));

        if (!this.IsNew) {
            this.SelectedMessageType = this.MessageTypesList.filter(d => d.Code == this.EntityPM.MessageTypeCode)[0];
            this.LoadAllowedObjectFields();
        }
    }

    private selectedMessageType: CodeNameClass;
    get SelectedMessageType() { return this.selectedMessageType; }
    set SelectedMessageType(value: CodeNameClass) {
        if (this.selectedMessageType != value) {
            this.selectedMessageType = value;

            if (value != null) {
                this.MessageTypeCode = value.Code;

                this.LoadAllowedObjectFields();
            }
        }
    }

    public RuleFieldsList: CodeNameClass[] = [];
    private LoadAllowedObjectFields() {
        var tableName: string;

        if (this.MessageTypeCode == "FWB" || this.MessageTypeCode == "FHL") {
            tableName = "Shipment";
        }

        else if (this.MessageTypeCode == "FFR") {
            tableName = "Booking";
        }

        else if (this.MessageTypeCode == "FVR") {
            tableName = "FlightsSchedulesRequest";
        }

        var service: EntityResourceService = new EntityResourceService();
        service.getEntityResourceByTableName(tableName).subscribe(response => {
            var objectTable: ObjectTablePM = window.ObjectTables.filter(d => d.Name == tableName)[0];
            var objectFields: ObjectFieldPM[] = window.ObjectFields.filter(d => d.ObjectTableId == objectTable.Id && d.AllowedInAirlineMessaging);

            this.RuleFieldsList = [];
            if (objectFields.length > 0) {

                objectFields.forEach((item) => {
                    this.RuleFieldsList.push(new CodeNameClass(item.Id, item.FullNameTextCodeDefaultText, null, item.FieldCode));
                });

                if (!AppTool.IsNullOrEmpty(this.EntityPM.Id) && !AppTool.IsNullOrEmpty(this.EntityPM.RuleFieldCode)) {
                    this.selectedRuleField = this.RuleFieldsList.filter(d => d.AdditionalField == this.RuleFieldCode)[0];
                }
            }
        });
    }

    private selectedRuleField: CodeNameClass;
    get SelectedRuleField() { return this.selectedRuleField; }
    set SelectedRuleField(value: CodeNameClass) {
        if (this.selectedRuleField != value) {
            this.selectedRuleField = value;

            this.RuleFieldId = value.Code;
            this.RuleFieldCode = value.AdditionalField;
        }
    }

    get MessageTypeCode() { return this.EntityPM.MessageTypeCode; }
    set MessageTypeCode(value: string) {
        if (this.EntityPM.MessageTypeCode != value) {
            this.EntityPM.MessageTypeCode = value;

            this.SetUIProperties();
        }
    }

    get RuleFieldId() { return this.EntityPM.RuleFieldId; }
    set RuleFieldId(value: string) {
        if (this.EntityPM.RuleFieldId != value) {
            this.EntityPM.RuleFieldId = value;
        }
    }

    get RuleFieldCode() { return this.EntityPM.RuleFieldCode; }
    set RuleFieldCode(value: string) {
        if (this.EntityPM.RuleFieldCode != value) {
            this.EntityPM.RuleFieldCode = value;
        }
    }

    get MaxSize() { return this.EntityPM.MaxSize; }
    set MaxSize(value: number) {
        if (this.EntityPM.MaxSize != value) {
            this.EntityPM.MaxSize = value;
        }
    }

    get IsMandatoryForSending() { return this.EntityPM.IsMandatoryForSending; }
    set IsMandatoryForSending(value: boolean) {
        if (this.EntityPM.IsMandatoryForSending != value) {
            this.EntityPM.IsMandatoryForSending = value;
        }
    }

    get InActive() { return this.EntityPM.InActive; }
    set InActive(value: boolean) {
        if (this.EntityPM.InActive != value) {
            this.EntityPM.InActive = value;
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];

        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (!this.IsMandatoryForSending && (this.MaxSize == null || this.MaxSize == 0)) {
            errors.push("You have to choose at least one validation");
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            var myService: AirlineMessagingRulePMService = new AirlineMessagingRulePMService();

            if (this.IsNew) {
                this.CurrentSession.StartBusyIndicatorSaving();
                myService.insert(this.EntityPM).subscribe(Result => {

                    var mm: ServiceResponse = Result;
                    if (!mm.HasError) {
                        CachedDataManager.RefreshTableData(this.ObjectTableName, true);
                        this.CurrentSession.StopBusyIndicator();
                        this.CurrentSession.CloseCurrentWindowEmit("ok");

                    }

                    else {
                        this.ValidationErrorsList = mm.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }
                });
            }

            else {
                this.CurrentSession.StartBusyIndicatorSaving();
                myService.update(this.EntityPM).subscribe(Result => {

                    var mm: ServiceResponse = Result;
                    if (!mm.HasError) {
                        CachedDataManager.RefreshTableData(this.ObjectTableName, true);
                        this.CurrentSession.StopBusyIndicator();
                        this.CurrentSession.CloseCurrentWindowEmit("ok");

                    }

                    else {
                        this.ValidationErrorsList = mm.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }
                });
            }
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('MessageTypeCode');
        this.myCloner.AddField('RuleFieldId');
        this.myCloner.AddField('RuleFieldCode');
        this.myCloner.AddField('MaxSize');
        this.myCloner.AddField('IsMandatoryForSending');
        this.myCloner.AddField('InActive');
        this.myCloner.AddEntity(this.EntityPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
