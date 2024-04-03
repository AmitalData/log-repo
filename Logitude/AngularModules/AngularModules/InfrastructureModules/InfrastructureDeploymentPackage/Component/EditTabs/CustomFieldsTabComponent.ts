import { Component } from '@angular/core';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { CustomFields, CustomPickListItem } from '../../../../Infrastructure/EntityPMs/DeploymentPackageDetails';
import { DeploymentPackagePM } from '../../../../Infrastructure/EntityPMs/DeploymentPackagePM';
import { ObjectFieldPM } from '../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import { GeneralDomainService } from '../../../../Infrastructure/Services/GeneralDomainService';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
declare var window: any;

@Component({

    templateUrl: './CustomFieldsTabComponent.html',
})

export class CustomFieldsTabComponent extends BaseComponent {

    public EntityPM: DeploymentPackagePM;
    public ObjectTableName: string = "DeploymentPackage";
    public DataContext: CustomFieldsTabComponent = this;
    public ExportCustomFields: CustomFields[];
    public OriginalExportCustomFields: CustomFields[] = [];
    public CustomPickLists: CustomPickListItem[] = [];

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.BuildExportCustomFieldsList();
    }

    AddCustomFields() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Add Custom Field";
        logWindow.Width = 720;
        logWindow.Height = 550;
        logWindow.WindowArgs = {
            CustomFieldsTabComponent: this,
        }

        logWindow.Show('./InfrastructureModules/InfrastructureDeploymentPackage/Component/EditTabs/AddCustomFieldsComponent');
        logWindow.WindowClosed.subscribe((event: any) => {
            if (event == "OK") {
                this.BuildOriginalExportCustomFieldsList();
                 return;
            }
            this.BuildExportCustomFieldsList();
        });
    }

    private BuildExportCustomFieldsList() {
        this.ExportCustomFields = [];
        this.OriginalExportCustomFields = this.EntityPM.DeploymentPackageDetails.CustomFields;
        this.OriginalExportCustomFields.forEach(customObjectField => {
            this.ExportCustomFields.push(customObjectField);
        });
    }

    private BuildOriginalExportCustomFieldsList() {
        this.OriginalExportCustomFields = [];
        this.ExportCustomFields.forEach(exportCustomField => {
            this.OriginalExportCustomFields.push(exportCustomField);
        });

        this.EntityPM.DeploymentPackageDetails.CustomFields = this.OriginalExportCustomFields;
        this.EntityPM.DeploymentPackageDetails.CustomPickLists = this.CustomPickLists;

        this.EntityPM.MarkAsDirty("DeploymentPackageDetails");

    }


    DeleteCustomField(item: CustomFields) {
        if (this.EntityPM.IsExported || this.EntityPM.DirectionId == 'I') return;
        this.OriginalExportCustomFields = this.OriginalExportCustomFields.filter(customField => customField.FieldCode != item.FieldCode);
        this.ExportCustomFields = this.ExportCustomFields.filter(customField => customField.FieldCode != item.FieldCode);
        this.EntityPM.DeploymentPackageDetails.CustomFields = this.EntityPM.DeploymentPackageDetails.CustomFields.filter(customField => customField.FieldCode != item.FieldCode);
        this.DeleteRelatedPickLists(item.CustomPickListCode);
        this.EntityPM.MarkAsDirty("DeploymentPackageDetails");
    }

    DeleteRelatedPickLists(customPickListCode: string) {
        if (AppTool.IsNullOrEmpty(customPickListCode)) return;
        let isPickListFieldExist = this.EntityPM.DeploymentPackageDetails.CustomFields.filter(customField => customField.CustomPickListCode == customPickListCode)[0] ? true : false;
        if (isPickListFieldExist) return;
        this.EntityPM.DeploymentPackageDetails.CustomPickLists = this.EntityPM.DeploymentPackageDetails.CustomPickLists.filter(customPickList => customPickList.Code != customPickListCode);
    }

    DeleteAllCustomFields() {
        this.ExportCustomFields = [];
        this.OriginalExportCustomFields = [];
        this.EntityPM.DeploymentPackageDetails.CustomFields = [];
        this.EntityPM.DeploymentPackageDetails.CustomPickLists = [];
        this.EntityPM.MarkAsDirty("DeploymentPackageDetails");
    }

    EditCustomField(item: CustomFields) {
        if (this.EntityPM.IsExported || this.EntityPM.DirectionId == 'I') return;
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Edit Custom Field";
        logWindow.Width = 720;
        logWindow.Height = 550;
        logWindow.WindowArgs = {
            CustomFieldsTabComponent: this,
            IsEdit: true,
            EditedCustomField: item,
        }

        logWindow.Show('./InfrastructureModules/InfrastructureDeploymentPackage/Component/EditTabs/AddCustomFieldsComponent');
        logWindow.WindowClosed.subscribe((event: any) => {
            if (event == "OK") {
                this.BuildOriginalExportCustomFieldsList();
                return;
            }
            this.BuildExportCustomFieldsList();
        });
    }

}
