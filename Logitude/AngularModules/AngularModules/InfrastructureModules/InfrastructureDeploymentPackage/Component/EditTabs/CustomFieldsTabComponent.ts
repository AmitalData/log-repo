import { Component } from '@angular/core';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { CustomFields } from '../../../../Infrastructure/EntityPMs/DeploymentPackageDetails';
import { DeploymentPackagePM } from '../../../../Infrastructure/EntityPMs/DeploymentPackagePM';
import { ObjectFieldPM } from '../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import { GeneralDomainService } from '../../../../Infrastructure/Services/GeneralDomainService';
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
    private CurrentSession = SessionLocator.SelectedSession;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.BuildExportCustomFieldsList();
    }

    AddCustomFields() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Add Custom Field";
        logWindow.Width = 700;
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
        this.EntityPM.MarkAsDirty("DeploymentPackageDetails");

    }   

}
