import {Component} from '@angular/core'; 
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator'; 
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {HybridPartnerPM} from '../../../../Common/EntityPMs/HybridPartnerPM';
 import {Headers} from '@angular/http';
 import {AppTool} from '../../../../Infrastructure/Tools';
 import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
 import {HybridPartnerPMService} from '../../../../Common/Services/StandardPMs/HybridPartnerPMService';

declare var window: any;

@Component({
    moduleId: module.id,
    templateUrl: './NewHybridPartnerComponent.html',
    //providers: [Http, ServiceArgs, EntityListService]
})

export class NewHybridPartnerComponent extends BaseComponent {

    DataContext: NewHybridPartnerComponent = this;  
    myentityPM: HybridPartnerPM = new HybridPartnerPM();
    _HybridPartnerPMService: HybridPartnerPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super(); 
        this._HybridPartnerPMService = new HybridPartnerPMService(); 
    }

    SetWindowArgs(args: any) { 
       
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

    onIsMislakaActivated(event) {

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
            this._HybridPartnerPMService.insert(this.myentityPM).subscribe(myResult => {
                if (!myResult.HasError) {
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    this.CurrentSession.CloseCurrentWindow();
                }
            });  
        }
         
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
