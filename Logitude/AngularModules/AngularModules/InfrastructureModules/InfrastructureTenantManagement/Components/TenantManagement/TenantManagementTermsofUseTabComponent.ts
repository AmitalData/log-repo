import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { TenantManagementPM } from '../../../../Infrastructure/EntityPMs/TenantManagementPM';
import { AppTool } from '../../../../Infrastructure/Tools';

@Component({

    selector: 'TenantManagementTermsofUseTabComponent',
    templateUrl: './TenantManagementTermsofUseTabComponent.html',
})

export class TenantManagementTermsofUseTabComponent extends BaseComponent implements OnInit {
    public ObjectTableName: string = "TenantManagement";
    public EntityPM: TenantManagementPM;
    public IsSupportDomainVisible: boolean = false;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
    }

    ngOnInit() {
        if (this.EntityPM != null) {
            this.LoadTermsOfUse();
        }
    }

    private LoadTermsOfUse() {

    }

    private UploadTermOfUse() {

    }

}
