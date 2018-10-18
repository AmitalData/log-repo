
declare var System: any;
declare var window: any;


import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Component, OnInit}  from '@angular/core';

import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TenantManagementPM} from '../../../../Infrastructure/EntityPMs/TenantManagementPM';

import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';

import {UIProperty, UIProperties}  from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';

import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {FormBuilder, FormGroup} from '@angular/forms';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,

    selector: 'BrandingTabComponent',
    templateUrl: './BrandingTabComponent.html',


})

export class BrandingTabComponent extends BaseComponent implements OnInit {
    public EntityPM: TenantManagementPM;
    public myForm: FormGroup;
    IsVisibile: boolean;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    constructor(fb: FormBuilder, public entityArgs: EntityArgs) {
        super();
        this.myForm = fb.group({});

    }

    ngOnInit() {
        this._entityResourceService.getEntityResourceByTableName("TenantManagement", 0).subscribe(response => {
            this.IsVisibile = true;
            this.EntityPM = this.entityArgs.EntityPM;
            if (this.EntityPM) {
               
              this.SetUIPropertiesEnabled(this.EntityPM.EnableBranding);
    
            }
        });



    }


    EnableBrandingChange(value:any) {

        this.EntityPM.UpdateByUserId = SessionInfo.LoggedUserId + "^" + SessionInfo.LoggedUserTenant.toString();
  
        this.SetUIPropertiesEnabled(value);
 
    }

    SetUIPropertiesEnabled(value: boolean) {

        this.EntityPM.UIProperties.SetEnabled("CustomerURL", "TenantManagement", value);
        this.EntityPM.UIProperties.SetEnabled("ContactEmail", "TenantManagement", value);
        this.EntityPM.UIProperties.SetEnabled("HideSharedlogistics", "TenantManagement", value);
    }






}






