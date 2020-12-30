import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { Component, Output, EventEmitter, OnInit, ComponentRef, ViewChild } from '@angular/core';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { CourierMasterService } from '../../../../Customs/Services/Others/CourierMasterService';
import { CourierMasterPMService } from '../../../../Customs/Services/StandardPMs/CourierMasterPMService';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { SendALLStorageSiteRequestParams } from '../../../../Customs/DataContract/RequestParams/SendALLStorageSiteRequestParams';
import { CourierMasterPM } from '../../../../Customs/EntityPMs/CourierMasterPM';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    selector: 'GetUnloadPortCodeComponent',
    templateUrl: './GetUnloadPortCodeComponent.html',
})

export class GetUnloadPortCodeComponent extends BaseComponent {
    public DataContext: GetUnloadPortCodeComponent = this;
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
            //this.UnloadPortCode = this.CourierMasterPM.AirlineId;
        }
    }

    FillErrors() {
        this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(this.UnloadPortCode)) {
            this.ValidationErrorsList.push("Storage Site Field is Required");

        } else {
            this.ValidationErrorsList = [];

        }
    }

    // Properties
    private _UnloadPortCode: string;
    public get UnloadPortCode() { return this._UnloadPortCode; }
    public set UnloadPortCode(newValue: string) {
        this._UnloadPortCode = newValue;
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
        confirm.Title = "שינוי אתר פריקה";
        confirm.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
        confirm.ShowNoButton = true;
        confirm.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.Cancel");
        confirm.Show("שינוי יבצע עדכון של כל ההצהרות באתר הפריקה החדש וישדר את ההצהרות למכס");
        confirm.WindowClosed.subscribe((event: any) => {
            if (confirm.Yes) {
                this.ChangeUnLoadPortCode();
            }
            confirm.Close();
        });
    }

    CancelButtonClicked() {
        SessionLocator.SelectedSession.CloseCurrentWindow();
    }

    ChangeUnLoadPortCode() {

        SessionLocator.SelectedSession.StartBusyIndicatorSaving();
        this._CourierMasterPMService.update(this.CourierMasterPM).subscribe((response: ServiceResponse) => {
            SessionLocator.SelectedSession.StopBusyIndicator();
            this.SendALLChangeUnloadPortCode();
        });
    }

    SendALLChangeUnloadPortCode() {

        var currRequestParams = new SendALLStorageSiteRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.CourierMasterId = this.CourierMasterPM.Id;
        currRequestParams.UnLoadPortCode = this.UnloadPortCode;
        currRequestParams.RequestName = "עדכון אתר פריקה";

        this._CourierMasterService.PostSendALLChangeStorageSiteCode(currRequestParams)
            .subscribe((res: any) => {
                SessionLocator.SelectedSession.StopBusyIndicator();
                var myMessageWindow = new MessageWindow();
                myMessageWindow.Show(res.Result);
                myMessageWindow.WindowClosed.subscribe(s => {
                    this.CancelButtonClicked();
                });
            });

    }
}
