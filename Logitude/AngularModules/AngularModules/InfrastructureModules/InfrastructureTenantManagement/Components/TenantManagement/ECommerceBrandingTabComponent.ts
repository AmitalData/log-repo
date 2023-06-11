
declare var System: any;
declare var window: any;


import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';

import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TenantManagementPM } from '../../../../Infrastructure/EntityPMs/TenantManagementPM';

import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import { InfraSettings } from '../../../../Infrastructure/Utilities/InfraSettings';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';

import { UIProperty, UIProperties } from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';

import { FormGroup } from '@angular/forms';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { GlobalDomainService } from '../../../../Common/Services/GlobalDomainService';
import { TenantPM } from 'Common/EntityPMs/TenantPM';
import { TenantPMService } from 'Common/Services/StandardPMs/TenantPMService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { Validator } from 'Infrastructure/Validators/Validator';

@Component({
    selector: 'ECommerceBrandingTabComponent',
    templateUrl: './ECommerceBrandingTabComponent.html',
})

export class ECommerceBrandingTabComponent extends BaseComponent implements AfterViewInit, OnInit, OnDestroy {
    public EntityPM: TenantManagementPM;
    private TenantPM: TenantPM = new TenantPM();

 
    public DataContext: any = this;
    IsVisibile: boolean;
    public EntityId: number;
  
    public ComapnylogoId: string;
    private entityResourceService: EntityResourceService = new EntityResourceService();
    public ECommerceBrandingTabName = "E-Commerce Branding";
    private CurrentSession = SessionLocator.SelectedSession;
    private iGlobalDomainService: GlobalDomainService;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.EntityId = this.EntityPM.Id;
        this.iGlobalDomainService = new GlobalDomainService();
        this.InitializeImageIds();
     
        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                   
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.iGlobalDomainService.UpdateTenantManagementJS(this.EntityPM);
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.iGlobalDomainService.UpdateTenantManagementJS(this.EntityPM);
                }
            });
        }
    }
 
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
       
    } 

    private InitializeImageIds() {
      
        this.ComapnylogoId = this.EntityPM.ComapnylogoId;
      
    }

    RemoveImage(name) {
     
        if (name == 'company') {
            this.EntityPM.ComapnylogoId = null;
            this.ComapnylogoId = null;
        }
       
    }
    ngAfterViewInit() {
        this.ListenToEntitySavedEvent();
    }

    ListenToEntitySavedEvent() {
        SessionLocator.SelectedSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
            if (isSaveSuccess) {
                this.EntityPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM;
             
            }
        });
    }
    get EcommerceSupportEmail() {
        return this.EntityPM.EcommerceSupportEmail;
    }
    set EcommerceSupportEmail(value: string) {
        if (this.EntityPM.EcommerceSupportEmail != value) {
            this.EntityPM.EcommerceSupportEmail = value;

        }
    }
    get LogoURL() {
        return this.EntityPM.LogoURL;
    }
    set LogoURL(value: string) {
        if (this.EntityPM.LogoURL != value) {
            this.EntityPM.LogoURL = value;
        }
    }
 
    get WhatsAppMessagingPhoneNumber() { return this.EntityPM.WhatsAppMessagingPhoneNumber; }
    set WhatsAppMessagingPhoneNumber(newValue) {
        if (this.EntityPM.WhatsAppMessagingPhoneNumber != newValue) {
            this.EntityPM.WhatsAppMessagingPhoneNumber = newValue;

        }
    }

    get ServiceAgreementURL() {
        return this.EntityPM.ServiceAgreementURL;
    }
    set ServiceAgreementURL(value: string) {
        if (this.EntityPM.ServiceAgreementURL != value) {
            this.EntityPM.ServiceAgreementURL = value;
        }
    }
   

    ngOnInit() {
        this.entityResourceService.getEntityResourceByTableName("TenantManagement", 0).subscribe((response: any) => {
         
            this.EntityPM = this.entityArgs.EntityPM;
            this.IsVisibile = true;
          
        });
    }
    
   
    ComapnylogoUploadedCompleted(code) {
        this.ComapnylogoId = code;
        this.EntityPM.ComapnylogoId = code;
    }
   


   

    
}






