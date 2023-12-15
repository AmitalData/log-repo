import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { CustomPickListList } from '../../../../Infrastructure/EntityLists/CustomPickListList';
import { CustomFields, CustomPickListItem } from '../../../../Infrastructure/EntityPMs/DeploymentPackageDetails';
import { DeploymentPackagePM } from '../../../../Infrastructure/EntityPMs/DeploymentPackagePM';
import { ObjectFieldPM } from '../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import { ObjectTablePM } from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import { GeneralDomainService } from '../../../../Infrastructure/Services/GeneralDomainService';
import { CustomPickListListService } from '../../../../Infrastructure/Services/StandardLists/CustomPickListListService';
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
    public ValidationErrorsList: string[] = [];
    private customPickListItems: CustomPickListItem[];
    private isEdit: boolean = false;
    private editedCustomField: CustomFields;
    constructor() {
        super();
        this.ValidationErrorsList = [];
    }

    SetWindowArgs(args: any) {
        this.customFieldsTabComponent = args.CustomFieldsTabComponent;
        this.isEdit = args.IsEdit;
        this.editedCustomField = args.EditedCustomField;
        this.generalDomainService = new GeneralDomainService();
        this.InitLOVFilters();
        this.IsLogLovReady = true;
        this.SetSelectedCustomFields();
        this.SetSelectedObjectTable();
    }

    SetSelectedObjectTable() {
        if (!this.isEdit) return;
        let objectTable = window.ObjectTables.filter(objectTable => objectTable.Name == this.editedCustomField.ObjectTableName)[0];
        this.SelectedObjectTableId = objectTable?.Id;
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
        this.ValidationErrorsList = [];
        this.ValidateCustomFields();
        if (this.ValidationErrorsList.length > 0) return;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.SetCustomPickLists();
    }

    public ValidateCustomFields() {

        if (AppTool.IsNullOrEmpty(this.SelectedObjectTableId)) {
            this.ValidationErrorsList.push("Object field is required");
            return;
        }
            
        if (this.CustomFields.length == 0) {
            this.ValidationErrorsList.push("This object doesn't contain custom fields, please add custom fields for this object first");
            return;
        }

        if (this.GetSelectedCustomFieldsCountBySelectedTable() == 0) {
            this.ValidationErrorsList.push("Please select at least one custom field");
            return;
        }

    }

    GetSelectedCustomFieldsCountBySelectedTable() {
        if (this.SelectedCustomFields.length == 0) return 0;
        let objectTable = window.ObjectTables.filter(objectTable => objectTable.Id == this.SelectedObjectTableId)[0];
        let selectedcustomFieldsOfObjectTable = this.SelectedCustomFields.filter(s => s.ObjectTableName == objectTable.Name);
        return selectedcustomFieldsOfObjectTable?.length;
    }

    SetCustomPickLists() {
        var customPickListListService = new CustomPickListListService();
        customPickListListService.getAllFromCache().subscribe((response: ServiceResponse) => {
            if (response.HasError) return;
            this.MapCustomPickListsDetails(response.Result);
            this.customFieldsTabComponent.CustomPickLists = this.customPickListItems;
            this.CurrentSession.StopBusyIndicator();
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        });
    }
    MapCustomPickListsDetails(customPickLists: Array<CustomPickListList>) {
        this.customPickListItems = [];
        this.SelectedCustomFields.forEach(customfield => {
            if (AppTool.IsNullOrEmpty(customfield.CustomPickListCode)) return;

            let customPickListItem = this.customPickListItems.filter(customPickListItem => customPickListItem.Code == customfield.CustomPickListCode)[0];
            if (customPickListItem) return;

            let customPickListList = customPickLists.filter(customPickList => customPickList.Code == customfield.CustomPickListCode);
            customPickListList.forEach(customPickList => this.customPickListItems.push(new CustomPickListItem(customPickList)));
        });
    }
}
