
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
import { ImageLibraryService } from 'Common/Services/Others/ImageLibraryService';
import { Guid } from 'Infrastructure/Utilities/Guid';
import { ImageParameter } from 'Infrastructure/DataContracts/ImageParameter';
declare var UploadLogoFile, HideImage , SetImage, ShowHideProgressDownload ,ArrayBufferToBase64: any;
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
    SmalllogoHtmlId: string = Guid.newGuid();
    LogoFileHtmlId: string = Guid.NewRandomString();
    ProgressDownloadId: string = Guid.newGuid();

    DemoMessageVisibility: boolean;

    public _imageLibraryService: ImageLibraryService = new ImageLibraryService();
    
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

    private InitializeImageIds(isLoad:boolean = false) {
     
        this.ComapnylogoId = this.EntityPM.ComapnylogoId;
         if(isLoad){
        if (ObjectsLocator.IsDemoTenant(this.EntityId.toString())) {
            this.DemoMessageVisibility = true;
       
            if (SessionLocator.LoggedUserPM.Email.toLowerCase() == "customercare@logitudeworld.com‏") {
                this.DemoMessageVisibility = false;
             
            }
        }
    }
    ShowHideProgressDownload(true, this.ProgressDownloadId);
    // this.CurrentSession.StartBusyIndicator("load img");

        this._imageLibraryService.DownloadFile("smalllogo" + this.EntityId, "jpg", "logos", this.EntityId,"",SessionInfo.LoggedUserTenant).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            ShowHideProgressDownload(false, this.ProgressDownloadId);
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    SetImage(this.SmalllogoHtmlId, result, false);
                } else HideImage(this.SmalllogoHtmlId);

            } else HideImage(this.SmalllogoHtmlId);
            this.CurrentSession.StopBusyIndicator();
        });
    }
    OpenUpLoadLogo() {
        this.CurrentSession.StartBusyIndicator("load img");
        document.getElementById(this.LogoFileHtmlId).click();
        this.CurrentSession.StopBusyIndicator();

    }
    UploadogoFile(event: any) {
        var file: any = UploadLogoFile(this.LogoFileHtmlId);
        if (file && (file.type == "image/jpeg" || file.type == "image/jpg")) {
        // this.IsShowMessageComplate = false;
        // this.IsShowProgressLoading = true;
        this.CurrentSession.StartBusyIndicator("load img");
        this.ArrayBufferToBase64(file, "logo", 300, 300, "jpg", this);
        }
    }
    ArrayBufferToBase64(file: any, filename: any, widht: number, height: number, extension: string, viewmode: any) {

        if (file) {
            var reader: FileReader = new FileReader();

            var reader = new FileReader();
            reader.onload = function (e) {
                var binary = '';
                var result=  ArrayBufferToBase64(e);
                var bytes = new Uint8Array(result);
                var len = bytes.byteLength;
                for (var i = 0; i < len; i++) {
                    binary += String.fromCharCode(bytes[i]);
                }

                viewmode.SendBlockToServer(window.btoa(binary), filename, widht, height, extension);

            };

            reader.onerror = function (e) {
                console.log(e);
            };
            reader.readAsArrayBuffer(file);



        }
    }
    
    SendBlockToServer(data: any, filename, widht: number, height: number, extension: string) {
        var filter = new ImageParameter();
        filter.Base64String = data;
        filter.FileName = filename;

        filter.BufferNumber = 0;
        filter.Tenant = this.EntityId;
        filter.Width = widht;
        filter.Height = height;
        filter.Extension = extension;
        filter.UploadMode = "CompanyLogos";
        filter.TokenTenant = SessionInfo.LoggedUserTenant;
        this._imageLibraryService.UploadFile(filter).subscribe((res:any) => {

            var pmResponse: ServiceResponse = res;
            var result: any;
            if (!pmResponse.HasError) {
                 var result = pmResponse.Result;
                 if (result) {

                    if (filename == "logo") {
                        this.SendBlockToServer(filter.Base64String, "smalllogo", 150, 150, "jpg");
                    }
                    else {

                     
                        if (filename == "logo" || filename == "smalllogo") {
                            this.InitializeImageIds(true);
                        }
                     

                    }


                }
            }

        });

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
    get EcommerceTenant() {
        return this.EntityPM.EcommerceTenant;
    }
    set EcommerceTenant(value: boolean) {
        if (this.EntityPM.EcommerceTenant != value) {
            this.EntityPM.EcommerceTenant = value;

        }
    }
    get TranzilaPaymentWithBit() {
        return this.EntityPM.TranzilaPaymentWithBit;
    }
    set TranzilaPaymentWithBit(value: boolean) {
        if (this.EntityPM.TranzilaPaymentWithBit != value) {
            this.EntityPM.TranzilaPaymentWithBit = value;

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






