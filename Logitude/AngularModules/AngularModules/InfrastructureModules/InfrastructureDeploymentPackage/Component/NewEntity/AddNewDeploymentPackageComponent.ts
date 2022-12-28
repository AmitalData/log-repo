import { Component } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeploymentPackagePM } from '../../../../Infrastructure/EntityPMs/DeploymentPackagePM';
import { DeploymentPackagePMService } from '../../../../Infrastructure/Services/StandardPMs/DeploymentPackagePMService';

@Component({

    templateUrl: './AddNewDeploymentPackageComponent.html',
})

export class AddNewDeploymentPackageComponent extends BaseComponent {
    public EntityPM: DeploymentPackagePM;
    public ValidationErrorsList: string[] = [];
    public DataContext: AddNewDeploymentPackageComponent = this;
    public ObjectTableName: string = "DeploymentPackage";
    public myService: DeploymentPackagePMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.myService = new DeploymentPackagePMService();
    }

}
