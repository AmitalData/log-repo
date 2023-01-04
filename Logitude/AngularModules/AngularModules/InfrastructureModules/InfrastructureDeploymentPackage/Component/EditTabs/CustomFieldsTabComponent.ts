import { Component } from '@angular/core';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
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
    public CustomFieldsToExport: ObjectFieldPM[];
    public OriginalCustomFieldsToExport: ObjectFieldPM[] = [];

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.BuildCustomFieldsList();
    }


    public BuildCustomFieldsList() {
        this.CustomFieldsToExport = [];
        this.CustomFieldsToExport.forEach(o => {
            this.OriginalCustomFieldsToExport.push(o);
        });
    }
    AddCustomFields() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Add Custom Field";
        logWindow.Width = 700;
        logWindow.Height = 550;
        logWindow.WindowArgs = {
            CustomFieldsTabComponent: this,
            DeploymentPackagePM: this.EntityPM,
        }
        logWindow.Show('./InfrastructureModules/InfrastructureDeploymentPackage/Component/EditTabs/AddCustomFieldsComponent');
        logWindow.WindowClosed.subscribe((event: any) => {
            if (event == "OK") {
                this.OriginalCustomFieldsToExport = []
                this.CustomFieldsToExport.forEach(o => {
                    this.OriginalCustomFieldsToExport.push(o);
                });
            }
            //this.BuildCustomFieldsList();
        });
    }
    GetParentEntityNameById(objectTableId: string) {
        return "11";
    }
   

}
