import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeploymentPackagePM } from '../../../../Infrastructure/EntityPMs/DeploymentPackagePM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { CustomFieldsTabComponent } from './CustomFieldsTabComponent';


@Component({

    templateUrl: './AddCustomFieldsComponent.html',
})

export class AddCustomFieldsComponent extends BaseComponent {
    
    private CurrentSession = SessionLocator.SelectedSession;
    private deploymentPackagePM: DeploymentPackagePM;
    private customFieldsTabComponent: CustomFieldsTabComponent;
    constructor() {
        super();
       
    }

    SetWindowArgs(args: any) {
        this.customFieldsTabComponent = args.CustomFieldsTabComponent;
        this.deploymentPackagePM = args.DeploymentPackagePM;
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

}
