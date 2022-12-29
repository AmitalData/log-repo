import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeploymentPackagePM } from '../../../../Infrastructure/EntityPMs/DeploymentPackagePM';


@Component({

    templateUrl: './NewImportDeploymentPackageComponent.html',
})

export class NewImportDeploymentPackageComponent extends BaseComponent {
    public EntityPM: DeploymentPackagePM;
    public ValidationErrorsList: string[] = [];
    public DataContext: NewImportDeploymentPackageComponent = this;
    public ObjectTableName: string = "DeploymentPackage";

    constructor() {
        super();   
    }

    Run() {

    }
    public ValidateDeploymentPackage() {

       

    }
}
