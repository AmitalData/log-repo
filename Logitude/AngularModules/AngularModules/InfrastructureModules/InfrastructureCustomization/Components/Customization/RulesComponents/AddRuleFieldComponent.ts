declare var window: any;
declare var insertAtSubject;
import {Component, ViewContainerRef, OnInit, AfterViewInit, ViewChildren, QueryList, Output, EventEmitter, ChangeDetectorRef} from '@angular/core';
import {TextCodeTranslationPipe} from '../../../../../Controls/Pipes/TextCodeTranslationPipe';
import {LogitudeListBoxComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/LogitudeListBox/LogitudeListBoxComponent';
import {ObjectFieldPM} from '../../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {SessionInfo} from '../../../../../Infrastructure/Utilities/SessionInfo';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceArgs} from '../../../../../Infrastructure/DataContracts/ServiceArgs';
import {ServiceHelper} from '../../../../../Infrastructure/Utilities/ServiceHelper';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {ObjectTablePM} from '../../../../../Infrastructure/EntityPMs/ObjectTablePM';
import {ObjectTableRuleFieldPM} from '../../../../../Infrastructure/EntityPMs/ObjectTableRuleFieldPM';
import {Guid} from '../../../../../Infrastructure/Utilities/Guid';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ObjectFieldList } from '../../../../../Infrastructure/EntityLists/ObjectFieldList';


@Component({
    moduleId: module.id,
    selector: 'AddRuleFieldComponent',
    templateUrl: './AddRuleFieldComponent.html',
})

export class AddRuleFieldComponent extends BaseComponent {

    public Expression: string;
    public ObjectTable: ObjectTablePM;
    public ObjectFields: ObjectFieldPM[] = [];
    public DataContext: AddRuleFieldComponent = this;
    private ObjectTableId: string;
    public SelectedObjectFieldId: string;
    public RuleFieldTXTAreaId: string;
    public SelectedObjectField: ObjectFieldPM;
    FieldsLovQueryFilters: ApiQueryFilters;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }
    SetWindowArgs(args: any) {

        this.RuleFieldTXTAreaId = Guid.newGuid();

        this.ObjectTableId = args.ObjectTableId;

        this.ObjectTable = window.ObjectTables.filter(d => d.Id == this.ObjectTableId)[0];


        this.ObjectTable = window.ObjectTables.filter((d: any) => d.Id == this.ObjectTableId)[0];
        this.ObjectTableId = this.ObjectTable.Id;
        this.ObjectFields = window.ObjectFields.filter((d: any) => d.ObjectTableId === this.ObjectTableId && !AppTool.IsNullOrEmpty(d.PMPropertyPath) && !d.DisplayOnly && !d.IsMulti && !d.IsCustomFilter);
        this.ObjectFields = this.ObjectFields.sort((a, b) => { return (a.FieldName.toLowerCase() === b.FieldName.toLowerCase()) ? 0 : (a.FieldName.toLowerCase() < b.FieldName.toLowerCase()) ? -1 : 1 });

        this.FieldsLovQueryFilters = new ApiQueryFilters();
        this.FieldsLovQueryFilters.Tenant = 0;
        this.FieldsLovQueryFilters.addAdditionalFilter("ObjectTableId", this.ObjectTableId, null, null, "Equals", false, false, false, "string");

    }

    onAddFieldTagClicked() {
        this.ViewDataField(this.ObjectTable.Id);
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
                this.Expression = insertAtSubject(this.RuleFieldTXTAreaId, $event);
            }

        });
    }

    onSelectFieldTagClicked() {
        var windowArgs: any = {};
        windowArgs.ObjectTableId = this.ObjectTable.Id;
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
                this.SelectedObjectFieldId = $event;
            }

        });
    }

    public HideLov: boolean = false;
    onSelectedItemChanged(item) {
        this.Expression = null;
      this.HideLov = true;
      if (item) {
        this.SelectedObjectField = this.ObjectFields.filter(f => f.Id == item.Id)[0];
        if (item && this.SelectedObjectFieldId != item.Id) {
          this.SelectedObjectFieldId = item.Id; item

        }
      }


        setTimeout(() => {
            this.HideLov = false;
        }, 300);


    }

    SelectedLOVTextChange($event) {
        this.Expression = $event;
    }

    OkButtonClicked() {
        if (this.SelectedObjectFieldId) {
            if (this.Expression) {
                this.CurrentSession.CurrentWindow.Close(this.SelectedObjectFieldId + ',' + this.Expression);
            }
            else {
                this.CurrentSession.CurrentWindow.Close(this.SelectedObjectFieldId);
            }
        }
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

}
