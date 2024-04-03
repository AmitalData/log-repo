import {Component,ChangeDetectorRef} from '@angular/core'; 
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator'; 
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {HybridPartnerPM} from '../../../../Common/EntityPMs/HybridPartnerPM';
 
 import {AppTool} from '../../../../Infrastructure/Tools';
 import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
 import {HybridPartnerPMService} from '../../../../Common/Services/StandardPMs/HybridPartnerPMService';
 import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
 import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
 import {WebFreightDomainService} from '../../../../Infrastructure/Services/WebFreightDomainService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';

declare var window: any;

@Component({
    
    templateUrl: './HybridPartnerTabComponent.html',
    //providers: [Http, ServiceArgs, EntityListService]
})

export class HybridPartnerTabComponent extends BaseComponent {

    DataContext: HybridPartnerTabComponent = this;  
    myentityPM: HybridPartnerPM = new HybridPartnerPM();
    _HybridPartnerPMService: HybridPartnerPMService;
    public Source: any = "";
    Tooltip: any;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private CD: ChangeDetectorRef) {
        super(); 
        this._HybridPartnerPMService = new HybridPartnerPMService(); 
        this.myentityPM = entityArgs.EntityPM;
        var myService: WebFreightDomainService = new WebFreightDomainService();
        myService.getHypridPartnerLogo(this.myentityPM.LogoId).subscribe((myResult: ServiceResponse) => {
            if (myResult) {
                this.Source = "data:image/JPEG;base64," + myResult;
               
                var isDestroyed: boolean = this.CD['destroyed'];
                if (!isDestroyed) {
                    this.CD.detectChanges();
                }
            }
        });
    }

    SetWindowArgs(args: any) {
        alert("Yeaaaa");
    } 

    public get Name() { return this.myentityPM.Name }
    public set Name(newValue: string) { this.myentityPM.Name = newValue; }

    public get LocalName() { return this.myentityPM.LocalName }
    public set LocalName(newValue: string) { this.myentityPM.LocalName = newValue; }

    public get PartnerTenant() { return this.myentityPM.PartnerTenant }
    public set PartnerTenant(newValue: number) { this.myentityPM.PartnerTenant = newValue; }

    public get IsMislakaActivated() { return this.myentityPM.IsMislakaActivated }
    public set IsMislakaActivated(newValue: boolean) { this.myentityPM.IsMislakaActivated = newValue; }

    public get IsExternalPartner() { return this.myentityPM.IsExternalPartner }
    public set IsExternalPartner(newValue: boolean) { this.myentityPM.IsExternalPartner = newValue; }

    public get ReceiveAllStatuses() { return this.myentityPM.ReceiveAllStatuses }
    public set ReceiveAllStatuses(newValue: boolean) { this.myentityPM.ReceiveAllStatuses = newValue; }

    public get AllowSendingDocsToAgent() { return this.myentityPM.AllowSendingDocsToAgent }
    public set AllowSendingDocsToAgent(newValue: boolean) { this.myentityPM.AllowSendingDocsToAgent = newValue; }

    public get InActive() { return this.myentityPM.InActive }
    public set InActive(newValue: boolean) { this.myentityPM.InActive = newValue; }

    onIsMislakaActivated(event) {
        this.IsMislakaActivated = event;
    }

    onIsExternalPartner(event) {
        this.IsExternalPartner = event;
    }

    onReceiveAllStatuses(event) {
        this.ReceiveAllStatuses = event;
    }

    onAllowSendingDocsToAgent(event) {
        this.AllowSendingDocsToAgent = event;
    }

    OnInActiveChange(event) {
        this.InActive = event;
    }

    OpenUpLoadLogo() {
        var logitudeWindow = new LogitudeWindow();
        var windowArgs: any = {};
        windowArgs.EntityPM = this.myentityPM; 
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Width = 750;
        logitudeWindow.Height = 500;
        logitudeWindow.Title = "";
        logitudeWindow.Show('./InfrastructureModules/InfrastructureHybrid/Components/HypridPartner/HybridPartnerUploadLogoComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            var myService: WebFreightDomainService = new WebFreightDomainService();
            myService.getHypridPartnerLogo(this.myentityPM.LogoId).subscribe((myResult: ServiceResponse) => {
                if (myResult) {
                    this.Source = "data:image/JPEG;base64," + myResult;

                    var isDestroyed: boolean = this.CD['destroyed'];
                    if (!isDestroyed) {
                        this.CD.detectChanges();
                    }
                }
            });
        });
    }

    ValidationErrorsList: any[];
    SaveChanges() {
        this.ValidationErrorsList = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (AppTool.IsNullOrEmpty(this.Name)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "Name"));
        }

        if (AppTool.IsNullOrEmpty(this.PartnerTenant)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "Partner Tenant"));
        } 
      
       
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ..");
            this._HybridPartnerPMService.insert(this.myentityPM).subscribe((myResult:any) => {
                if (!myResult.HasError) {
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    this.CurrentSession.CloseCurrentWindow();
                }
            });  
        }
         
    }
}
