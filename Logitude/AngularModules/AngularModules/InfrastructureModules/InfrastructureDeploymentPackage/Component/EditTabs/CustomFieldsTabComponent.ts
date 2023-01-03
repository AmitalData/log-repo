import { Component } from '@angular/core';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DeploymentPackagePM } from '../../../../Infrastructure/EntityPMs/DeploymentPackagePM';
import { ObjectFieldPM } from '../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import { GeneralDomainService } from '../../../../Infrastructure/Services/GeneralDomainService';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';


@Component({

    templateUrl: './CustomFieldsTabComponent.html',
})

export class CustomFieldsTabComponent extends BaseComponent {

    public CustomFields: ObjectFieldPM[];
    private EntityPM: DeploymentPackagePM;
    constructor() {
        super();
        this.BuildCustomFieldsList();
    }

    public BuildCustomFieldsList() {
        this.CustomFields = [];
    }
    AddCustomFields() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Add Custom Field";
        logWindow.Width = 800;
        logWindow.Height = 600;
        logWindow.WindowArgs = {
            CustomFieldsTabComponent: this,
            DeploymentPackagePM: this.EntityPM,
        }
        logWindow.Show('./InfrastructureModules/InfrastructureDeploymentPackage/Component/EditTabs/AddCustomFieldsComponent');
        logWindow.WindowClosed.subscribe((event: any) => {
            
            this.BuildCustomFieldsList();
        });
    }
   

}
