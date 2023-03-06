import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeploymentPackagePM } from '../../../../Infrastructure/EntityPMs/DeploymentPackagePM';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { DownloadManager } from '../../../../Infrastructure/Utilities/DownloadManager';
import { DeploymentPackagePMService } from '../../../../Infrastructure/Services/StandardPMs/DeploymentPackagePMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    templateUrl: './DeploymentPackageGeneralTabComponent.html',
})

export class DeploymentPackageGeneralTabComponent extends BaseComponent {
    public EntityPM: DeploymentPackagePM;
    public ObjectTableName: string = "DeploymentPackage";
    public DataContext: DeploymentPackageGeneralTabComponent = this;
    private service: DeploymentPackagePMService;
    private CurrentSession = SessionLocator.SelectedSession;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.service = new DeploymentPackagePMService();
    }

    ngOnInit() {
        this.SetUIProperties();
    }

    SetUIProperties() {
        this.UIProperties.SetEnabled("Code", this.ObjectTableName, false);
    }

    // Properties 
    get Code() { return this.EntityPM.Code; }
    set Code(newValue: string) {
        if (this.EntityPM.Code != newValue) {
            this.EntityPM.Code = newValue;
        }
    }

    get Name() { return this.EntityPM.Name; }
    set Name(newValue: string) {
        if (this.EntityPM.Name != newValue) {
            this.EntityPM.Name = newValue;
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(newValue: string) {
        if (this.EntityPM.Description != newValue) {
            this.EntityPM.Description = newValue;
        }
    }

    public get IsDisabledEntity() {
        return this.EntityPM.IsExported || this.EntityPM.DirectionId == "I";
    }
    Download() {
        this.EntityPM.IsExported = true;
        DownloadManager.DownloadPage(this.EntityPM.DocumentId,null, true);
        this.service.update(this.EntityPM).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                return;
            }
            console.log(response.ErrorsArray);
        });
    }
}
