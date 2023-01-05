import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { CustomFields } from '../../../../Infrastructure/EntityPMs/DeploymentPackageDetails';
import { DeploymentPackagePM } from '../../../../Infrastructure/EntityPMs/DeploymentPackagePM';
import { ObjectFieldPM } from '../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import { ObjectTablePM } from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import { GeneralDomainService } from '../../../../Infrastructure/Services/GeneralDomainService';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { CustomFieldsTabComponent } from './CustomFieldsTabComponent';
declare var window: any;

@Component({

    templateUrl: './AddCustomFieldsComponent.html',
})

export class AddCustomFieldsComponent extends BaseComponent {
    
    private CurrentSession = SessionLocator.SelectedSession;
    private customFieldsTabComponent: CustomFieldsTabComponent;
    public CustomFields: CustomFields[] = [];
    public SelectedCustomFields: CustomFields[] = [];
    public DataContext: AddCustomFieldsComponent = this;
    public ObjectTablesFilterItems: ApiQueryFilters;
    public IsLogLovReady: boolean = false;
    private generalDomainService: GeneralDomainService;

    constructor() {
        super();
    }

    SetWindowArgs(args: any) {
        this.customFieldsTabComponent = args.CustomFieldsTabComponent;
        this.generalDomainService = new GeneralDomainService();
        this.InitLOVFilters();
        this.IsLogLovReady = true;
        this.SetSelectedCustomFields();
    }

    InitLOVFilters() {
        this.ObjectTablesFilterItems = new ApiQueryFilters();
        this.ObjectTablesFilterItems.addAdditionalFilter("IsDeploymentPackage", true, null, null, "Equals", true, false, false, "string");
        this.ObjectTablesFilterItems.Tenant = SessionLocator.Tenant;
    }

    SetSelectedCustomFields() {
        this.SelectedCustomFields = this.customFieldsTabComponent.ExportCustomFields;
    }

    IsSelected(cutsomField: CustomFields) {
        let field = this.SelectedCustomFields.filter(selectedCustomField => selectedCustomField.FieldCode == cutsomField.FieldCode)[0];
        if (!field) return false;
        return true;
    }

    SelectCustomField(event: any, customField: CustomFields) {
        if (!event) {
            this.SelectedCustomFields = this.customFieldsTabComponent.ExportCustomFields = this.SelectedCustomFields.filter(selectedCustomField => selectedCustomField.FieldCode != customField.FieldCode);
            return;
        }
        if(this.IsSelected(customField)) return;
        this.SelectedCustomFields.push(customField);
    }

    private selectedObjectTableId: string;
    public get SelectedObjectTableId() { return this.selectedObjectTableId; }
    public set SelectedObjectTableId(value: string) {
        if (this.selectedObjectTableId == value) return;
        this.selectedObjectTableId = value;
        this.BuildCustomFieldsList();
    }

    private selectAllCustomFields: boolean = true;
    public get SelectAllCustomFields() {
         this.CustomFields.forEach(customField => {
             if(!this.IsSelected(customField)) this.selectAllCustomFields = false;       
         });
        return this.selectAllCustomFields && this.CustomFields.length != 0;
    }
    public set SelectAllCustomFields(value: boolean) {
        if (this.selectAllCustomFields == value) return;
        this.selectAllCustomFields = value;
        this.CustomFields.forEach(customField =>{
             this.SelectCustomField(value,customField);
        });
    }

    public BuildCustomFieldsList() {
        if (AppTool.IsNullOrEmpty(this.SelectedObjectTableId)) return;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.generalDomainService.GetCustomFieldsByTableId(this.SelectedObjectTableId).subscribe((response: ServiceResponse) => {
            if (response.HasError) return;
            this.MapCustomFieldsDetails(response.Result);
        });
    }

    public MapCustomFieldsDetails(objectFieldsPM: ObjectFieldPM[]) {
        this.CustomFields = [];
        objectFieldsPM.forEach(objectField => this.CustomFields.push(new CustomFields(objectField)));
        this.CurrentSession.StopBusyIndicator();
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    }

    OkButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("OK");
    }

}
