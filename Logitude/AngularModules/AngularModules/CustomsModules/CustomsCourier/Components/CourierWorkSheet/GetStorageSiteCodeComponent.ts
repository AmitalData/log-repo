import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { Component, Output, EventEmitter, OnInit, ComponentRef, ViewChild} from '@angular/core';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { CourierMasterPMService } from '../../../../Customs/Services/StandardPMs/CourierMasterPMService';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { SendALLStorageSiteRequestParams } from '../../../../Customs/DataContract/RequestParams/SendALLStorageSiteRequestParams';
import { CourierMasterPM } from '../../../../Customs/EntityPMs/CourierMasterPM';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { CourierMasterService } from 'Customs/Services/Others/CourierMasterService';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';

@Component({
    selector: 'GetStorageSiteCodeComponent',
    templateUrl: './GetStorageSiteCodeComponent.html',
})

export class GetStorageSiteCodeComponent extends BaseComponent {
    public DataContext: GetStorageSiteCodeComponent = this;
    public ObjectTableName: string = "Customs.CourierMaster";
    public ValidationErrorsList: string[];
    CourierMasterPM: CourierMasterPM = new CourierMasterPM();

    _CourierMasterService: CourierMasterService = new CourierMasterService();
    _CourierMasterPMService: CourierMasterPMService = new CourierMasterPMService();

    constructor(private _entityResourceService: EntityResourceService, public entityArgs: EntityArgs) {
        super();
        
    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.CourierMasterPM = args.CourierMasterPM;
            this.StorageSiteCode = this.CourierMasterPM.StorageSiteCode;
        }
    }

    FillErrors() {
        this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(this.StorageSiteCode)) {
            this.ValidationErrorsList.push("Storage Site Field is Required");
        
        } else {
            this.ValidationErrorsList = [];

        }
    }

    // Properties
    private _StorageSiteCode: string;
    public get StorageSiteCode() { return this._StorageSiteCode; }
    public set StorageSiteCode(newValue: string) {
        this._StorageSiteCode = newValue;
        this.ValidationErrorsList = [];
    }
   
    OkButtonClicked() {

        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        var confirm = new ConfirmWindow();
        confirm.Width = 320;
        confirm.Height = 180;
        confirm.Title = "שינוי אתר אחסון";
        confirm.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
        confirm.ShowNoButton = true;
        confirm.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.Cancel");
        confirm.Show("שינוי יבצע עדכון של כל ההצהרות באתר האחסון החדש וישדר את ההצהרות למכס");
        confirm.WindowClosed.subscribe((event: any) => {
            if (confirm.Yes) {
                this.ChangeStorageSiteCode();
            }
            confirm.Close();
        });
    }
    
    CancelButtonClicked() {
        SessionLocator.SelectedSession.CloseCurrentWindow();
    }

    ChangeStorageSiteCode() {

        SessionLocator.SelectedSession.StartBusyIndicatorSaving();
        this.CourierMasterPM.StorageSiteCode = this.StorageSiteCode;
        this._CourierMasterPMService.update(this.CourierMasterPM).subscribe((response: ServiceResponse) => {
            SessionLocator.SelectedSession.StopBusyIndicator();
            this.SendALLChangeStorageSiteCode();
        });
    }

    SendALLChangeStorageSiteCode() {

        var currRequestParams = new SendALLStorageSiteRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.CourierMasterId = this.CourierMasterPM.Id;
        currRequestParams.StorageSiteCode = this.StorageSiteCode;

        this._CourierMasterService.PostSendALLChangeStorageSiteCode(currRequestParams)
            .subscribe((res:any) => {
                SessionLocator.SelectedSession.StopBusyIndicator();
                var myMessageWindow = new MessageWindow();
                if(!AppTool.IsNullOrEmpty(res.RequestInProgressList)){
                    myMessageWindow.ShowEventButton=true;
                    TextCodeTranslator.Translate("Customs.Declaration.TH.RequestSheet");
                }  
                myMessageWindow.Show(res.Message);
                myMessageWindow.WindowClosed.subscribe(s => {
                    this.CancelButtonClicked();
                });
                myMessageWindow.SendEvent.subscribe(s=>{
                    if(s){
                        this.LoadCustomsRequestSheetsScreen(res.RequestInProgressList)
                    }
                });
            });

    }
    LoadCustomsRequestSheetsScreen(RequestInProgressList:string){
       
        var entityArgs=new EntityArgs();
       // entityArgs.EntityPM = this.entityPM;
        entityArgs.ObjectTableName="Customs.CourierMaster";
        entityArgs.OriginEntity=RequestInProgressList;
        let windowTitle = TextCodeTranslator.Translate("TextCodeTranslator");
        let logWindow = new LogitudeWindow();
        logWindow.Width = 1300;
        logWindow.Height = 700;
        logWindow.Title = windowTitle;
        logWindow.IsShowCloseButton = true;
        logWindow.WindowArgs = entityArgs;
        
        logWindow.Show('./CustomsModules/CustomsControls/Components/CustomsRequestsSheetsComponent');


 
    }
}
