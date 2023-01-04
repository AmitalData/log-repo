import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DeploymentPackagePM } from '../../../../Infrastructure/EntityPMs/DeploymentPackagePM';
import { ObjectFieldPM } from '../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import { ObjectTablePM } from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { CustomFieldsTabComponent } from './CustomFieldsTabComponent';
declare var window: any;

@Component({

    templateUrl: './AddCustomFieldsComponent.html',
})

export class AddCustomFieldsComponent extends BaseComponent {
    
    private CurrentSession = SessionLocator.SelectedSession;
    private deploymentPackagePM: DeploymentPackagePM;
    private customFieldsTabComponent: CustomFieldsTabComponent;
    public CustomFields: ObjectFieldPM[];
    public SelectedCustomFields: ObjectFieldPM[];
    public DataContext: AddCustomFieldsComponent = this;
    public EntityFilterItems: ApiQueryFilters;
    public IsLogLovReady: boolean = false;
    
    constructor() {
        super();
    }

    SetWindowArgs(args: any) {
        this.customFieldsTabComponent = args.CustomFieldsTabComponent;
        this.deploymentPackagePM = args.DeploymentPackagePM;
        this.InitLOVFilters();
        this.IsLogLovReady = true;
        this.BuildSelectedCustomFields();
    }

    InitLOVFilters() {
        this.EntityFilterItems = new ApiQueryFilters();
        this.EntityFilterItems.addAdditionalFilter("IsDeploymentPackage", true, null, null, "Equals", true, false, false, "string");
        this.EntityFilterItems.Tenant = SessionLocator.Tenant;
    }

    BuildSelectedCustomFields() {
        this.SelectedCustomFields = this.customFieldsTabComponent.CustomFieldsToExport;
    }
    CheckIfFieldIsSelected(cutsomField: ObjectFieldPM) {
        let field = this.SelectedCustomFields.filter(selectedCustomField => selectedCustomField == cutsomField);
        if (!field) return false;
        if (!field[0]) return false;
        return true;
    }

    CheckCustomField(event: any, cutsomField: ObjectFieldPM) {
        if (event) {
            this.SelectedCustomFields.push(cutsomField);
            return;
        }
        this.SelectedCustomFields = this.customFieldsTabComponent.CustomFieldsToExport = this.SelectedCustomFields.filter(selectedCustomField => selectedCustomField.Id != cutsomField.Id);
    }

    private selectedEntityId: string;
    public get SelectedEntityId() { return this.selectedEntityId; }
    public set SelectedEntityId(value: string) {
        if (this.selectedEntityId == value) return;
        this.selectedEntityId = value;
        this.BuildCustomFieldsList();
    }

    private checkAllCustomFields: boolean = false;
    public get CheckAllCustomFields() { return this.checkAllCustomFields; }
    public set CheckAllCustomFields(value: boolean) {
        if (this.checkAllCustomFields == value) return;
        this.checkAllCustomFields = value;
    }

    public BuildCustomFieldsList() {
        if (AppTool.IsNullOrEmpty(this.SelectedEntityId)) return;
        this.CustomFields = window.ObjectFields.filter(objectField => objectField.ObjectTableId == this.SelectedEntityId && objectField.IsCustom);
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("OK");
    }

}
