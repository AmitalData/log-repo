import {Component,ChangeDetectorRef} from '@angular/core'; 
import {WebFreightDomainService} from '../../../Infrastructure/Services/WebFreightDomainService';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {OnInit, Output, EventEmitter, ComponentRef, QueryList} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ARPaymentExtendedListService} from '../../../Invoice/Services/ExtendedLists/ARPaymentExtendedListService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { CustomsRequestMenuService } from '../../../Customs/Services/Others/CustomsRequestMenuService';
import { CustomsRequestSheetExtendedPMService } from '../../../Customs/Services/ExtendedPMs/CustomsRequestSheetExtendedPMService';


import { ResponseDataBase } from '../../../Customs/DataContract/ResponseData/ResponseDataBase';
import { CustomsRequestsSheetPM } from '../../../Customs/EntityPMs/CustomsRequestsSheetPM';

@Component({
    
    templateUrl: './CustomsRequestsSheetsListTemplate.html',
})

export class CustomsRequestsSheetsListTemplate {

    _CustomsRequestsSheet: any;
    public fieldName: any;
    ReAnalyzeButtonIsEnabled: boolean = false;
    ReAnalyzeButtonVisibility: boolean = false;
    CancleButtonOpacity: string = "1";
    isReAnAnalysis: boolean;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef) {
        //        this.TenantCurrencySign = SessionLocator.TenantPM.CurrencySign;
    }


    setVariables(customsRequestsSheet: any, fieldName: string, AdditionalDataCustom:any) {
        ///console.log(rowData);
        this.isReAnAnalysis = AdditionalDataCustom;
        
        this._CustomsRequestsSheet = customsRequestsSheet;
        this.fieldName = fieldName;

        //#region Set Icons

        //#endregion 
        if (customsRequestsSheet.RequestStatusCode == "99" || customsRequestsSheet.RequestStatusCode == "30") {
            //CancleButtonVisibility = Visibility.Collapsed;

            this.IsCancelled = true;
        }

        this.ReAnalyzeButtonIsEnabled =
            customsRequestsSheet.RequestStatusCode == "25"  //Analyze Failed 
            ||
            (customsRequestsSheet.IsRestored || customsRequestsSheet.RequestStatusCode == "21"); //21,Received,תשובה תקינה

        ;
        let LoggedUserPMCode = SessionLocator.LoggedUserPM.Code || "";
        LoggedUserPMCode = LoggedUserPMCode.toLowerCase();
        
        this.ReAnalyzeButtonVisibility = SessionLocator.LoggedUserPM.IsCustomerCare || (LoggedUserPMCode == "amital" || LoggedUserPMCode.startsWith("amital."))  ? true : false;

        if (!this.IsCancleButtonEnabled(customsRequestsSheet.RequestStatusCode, customsRequestsSheet.IsDCA)) {
            this.CancleButtonOpacity = "0.95";
        }

        this.CD.detectChanges();
    }


    OpenJournal(id) {
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal', BackButtonLabel: 'Back' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                    });
                });
        }
    }

    ShowFormatedResponse(RequestComminicationId, InterfaceTypeCode) {
        ///alert("ShowFormatedResponse(id)" + RequestComminicationId);
         let customsRequestMenuService = new CustomsRequestMenuService();
        customsRequestMenuService.ShowModalByIdAndIntreface(RequestComminicationId, InterfaceTypeCode, this._CustomsRequestsSheet.RequestDescription);


    }
    CancleButtonVisibility(RequestStatusCode: string, IsDCA: boolean): boolean {
        if (RequestStatusCode == "30") {
            return false;
        }
        if (!this.IsCancelled && RequestStatusCode != "99") {
            return true;
        }
        
        return false;
    }

    IsCancleButtonEnabled(RequestStatusCode: string, IsDCA: boolean): boolean {

        return ResponseDataBase.RequestSheetCanCancelled(RequestStatusCode, IsDCA);;
    }
    IsCancelled: boolean = false;
    public CancleRequestMethod(id: string) {
        
        //CancleButtonVisibility = Visibility.Collapsed;
        //FirePropertyChanged("CancleButtonVisibility");
        this.IsCancelled = true;
        //FirePropertyChanged("IsCancelled");
        this.CD.detectChanges();
        if (this.ShowBusyIndicator())  this.CurrentSession.StartBusyIndicator("");
        var mappedEntity = new CustomsRequestsSheetPM();
        mappedEntity.Id = id;
        mappedEntity.RequestStatusCode = "99";
        var myCustomsRequestSheetExtendedPMService = new CustomsRequestSheetExtendedPMService();
        myCustomsRequestSheetExtendedPMService.PostSetCustomsRequestSheetStatus(mappedEntity)
            .subscribe((r: ServiceResponse) => {

                if (this.ShowBusyIndicator())  this.CurrentSession.StopBusyIndicator();
                if (r.Result) {
                    //alert(r.Result);
                    this.IsCancelled = false;
                    if (this.ShowBusyIndicator()) {
                        var messageWindow = new MessageWindow();
                        messageWindow.Width = 400;
                        messageWindow.Height = 150;
                        messageWindow.Title = "Cancelled Customs Request Sheet Failed !!";
                        messageWindow.Show(r.Result);
                    }
                } else {
                    this._CustomsRequestsSheet.RequestStatusCode = "6";
                    this._CustomsRequestsSheet.RequestStatusName = "מבוטלת";
                }
                this.CD.detectChanges();
            }
            );
        //InvokeOperation < string > op = context.SetCustomsRequestSheetStatus(Id, customsRequestsSheetList.Tenant, "99");
        //op.Completed += op_Completed;
    }
    ShowBusyIndicator() {
        if ((this.fieldName == "CancleRequest" || this.fieldName == "ReAnalyze") && this.isReAnAnalysis==true) {
            return false;
        }
        return true;
    }
    CanShowFormatedResponseCommand(): boolean {
        if (this._CustomsRequestsSheet.RequestStatusCode == "99" ||
            AppTool.IsNullOrEmpty(this._CustomsRequestsSheet.RequestStatusCode)) {
            return false;
        }
        var myint: number;
        myint = this._CustomsRequestsSheet.RequestStatusCode

        if (myint >= 21) {

            return true;
        }
        return false;
    }
    OnShowLogclick() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 1000;
        logitudeWindow.Height = 700;
        logitudeWindow.IsShowCloseButton = true;
        logitudeWindow.Title = TextCodeTranslator.Translate("CommunicationLog.O.MoreDetails");;
        logitudeWindow.WindowArgs = this._CustomsRequestsSheet;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureCommunications/Components/Communications/CommunicationLogMoreDetailsComponent');

    }
    ReAnalyzeButtonCommandAction() {
        this.ReAnalyzeButtonIsEnabled = false;


        this.CD.detectChanges();
        if (this.ShowBusyIndicator())  this.CurrentSession.StartBusyIndicator("");

        var myCustomsRequestSheetExtendedPMService = new CustomsRequestSheetExtendedPMService();
        myCustomsRequestSheetExtendedPMService.PostSetCustomsRequestSheetStatus
        myCustomsRequestSheetExtendedPMService.PostCustomsRequestSheetReQueue(this._CustomsRequestsSheet)
            .subscribe((r: ServiceResponse) => {

                if (this.ShowBusyIndicator())    this.CurrentSession.StopBusyIndicator();
                if (r.Result) {
                    //alert(r.Result);
                    this.IsCancelled = false;
                    if (this.ShowBusyIndicator()) {
                        var messageWindow = new MessageWindow();
                        messageWindow.Width = 400;
                        messageWindow.Height = 150;
                        messageWindow.Title = "Cancelle Customs Request Sheet Failed !!";
                        messageWindow.Show(r.Result);
                    }
                } else {

                    this._CustomsRequestsSheet.RequestStatusName = "תשובה תקינה";
                    this._CustomsRequestsSheet.RequestStatusName = "תשובה תקינה";
                }
                this.CD.detectChanges();



            });
    }
    
    Copy2Clipboard() {    
        var valueToCopy = this._CustomsRequestsSheet.CorrelationId;
        var tempTextArea = document.createElement("textarea");
        tempTextArea.value = valueToCopy;
        document.body.appendChild(tempTextArea);
        tempTextArea.select();
        document.execCommand('copy');
        document.body.removeChild(tempTextArea); 
    }
}
