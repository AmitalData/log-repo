import {Component, ChangeDetectorRef, Output, EventEmitter} from '@angular/core';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import { CourierMasterPM } from '../../../Customs/EntityPMs/CourierMasterPM';
import { CourierMasterValidator } from '../../../Customs/Validators/CourierMasterValidator';
import { CustomsRequestsSheetPM } from '../../../Customs/EntityPMs/CustomsRequestsSheetPM';
import { DeclarationPM } from '../../../Customs/EntityPMs/DeclarationPM';
import { DeclarationList } from '../../../Customs/EntityLists/DeclarationList';
import { GenericRequestParams } from '../../../Customs/DataContract/RequestParams/GenericRequestParams';
import { SendRequestVIA } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { DeclarationWebService } from '../../../Customs/Services/WebServices/DeclarationWebService';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DeclarationAmendmentComponent } from '../../CustomsDeclarationModules/DeclarationTabs/Components/DeclarationAmendment/DeclarationAmendmentComponent';
import { DeclarationReferantDataList } from '../../../Customs/EntityLists/DeclarationReferantDataList';
import { AmitalGatewayUtil, UnifreightMessageM } from '../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { ListComponentArgs } from '../../../Infrastructure/Args';
import { DeclarationRemarksService } from '../../../Common/Services/ExtendedPMs/DeclarationRemarksService';

@Component({
    moduleId: module.id,
    templateUrl: './DeclarationReferantDataListTemplate.html',
})

export class DeclarationReferantDataListTemplate {

    public Entity: DeclarationReferantDataList;
    public fieldName: any;

    constructor(private _ListComponentArgs: ListComponentArgs,
        private CD: ChangeDetectorRef, private _EntityResourceService: EntityResourceService) {

    }

    setVariables(DeclarationListRecord: DeclarationReferantDataList, fieldName: string, additionalData: any) {
        this._EntityResourceService.getEntityResourceByTableName("General").subscribe(response => {
            debugger;
            this.Entity = DeclarationListRecord;
            this.fieldName = fieldName;
             this.CD.detectChanges();
        });
    }
    
    public ButtonClick() {
        this._ListComponentArgs.SuppressOnRowSelectedField = true;

    }
    ShowUnifaceCustomFile() {
        let myDeclarationReferantDataList: DeclarationReferantDataList = this.Entity;
        //myDeclarationReferantDataList.CustomFileNo


    }


    OpenRemarks() {
        this._ListComponentArgs.SuppressOnRowSelectedField = true;

        var _declarationRemarksService: DeclarationRemarksService = new DeclarationRemarksService();
        var windowArgs: any = {};
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.ShowHeaderButtons = true;
        logitudeWindow.Height = 525;
        logitudeWindow.Width = 750;
        logitudeWindow.ShowCloseButton = true;

        if (this.Entity.IsClassificationRemarks) {
            _declarationRemarksService.GetSVCOrSRVStatusList(this.Entity.Tenant, this.Entity.CustomFileNo)
                .subscribe((response: any) => {
                    windowArgs.EntityPM = response.Result;
                    windowArgs.length = response.Result.length;
                    logitudeWindow.Title = windowArgs.length + "  הערות מסווג  ";
                    logitudeWindow.WindowArgs = windowArgs;
                    logitudeWindow.Show('./CustomsModules/CustomsMaintenance/Components/DeclarationRemarksComponent');
                });
        }
        else {
            if (this.Entity.IsControllerRemarks) {
                _declarationRemarksService.GetINCorINAtatusList(this.Entity.Tenant, this.Entity.CustomFileNo)
                    .subscribe((response: any) => {
                        windowArgs.EntityPM = response.Result;
                        let counter = response.Result.length;
                        logitudeWindow.Title = counter + "  הערות מבקר  ";
                        logitudeWindow.WindowArgs = windowArgs;
                        logitudeWindow.Show('./CustomsModules/CustomsMaintenance/Components/DeclarationRemarksComponent');
                    });
            }
        }
    }

    ShowCFIFILEMMoveToQueueScreen() {
        this._ListComponentArgs.SuppressOnRowSelectedField = true;
        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
            AmitalGatewayUtil.Instance.ShowCFIFILEMMoveToQueueScreen(
                this.Entity.CustomFileNo,
                this.Entity.DeclarationId,
                "ShowCFIFILEMMoveToQueueScreen");
        } else {
            var myMessageWindow = new MessageWindow();
            let mess = "ShowCFIFILEMMoveToQueueScreen -" + this.Entity.CustomFileNo;
            myMessageWindow.Show(mess);
        }
    }


    PreShowCFIFILEMMoveSIToOCRScreen(value: string) {
        this._ListComponentArgs.SuppressOnRowSelectedField = true;
        if (value == 'X' || value == 'E') {
            this.ShowCFIFILEMMoveSIToOCRScreen();
        }
    }

    ShowCFIFILEMMoveSIToOCRScreen() {

        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
            AmitalGatewayUtil.Instance.ShowCFIFILEMMoveSIToOCRScreen(
                this.Entity.CustomFileNo,
                this.Entity.DeclarationId,
                "ShowCFIFILEMMoveSIToOCRScreen");
        } else {
            var myMessageWindow = new MessageWindow();
            let mess = "ShowCFIFILEMMoveSIToOCRScreen -" + this.Entity.CustomFileNo;
            myMessageWindow.Show(mess);
        }
    }

    ShowDeclaration(event) {
        if (SessionLocator.SelectedSession != null && SessionLocator.SelectedSession.CurrentWindow != null) {
            SessionLocator.SelectedSession.CurrentWindow.SuppressBusyIndicator = true;
        }
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.SelectedSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                //var label = TextCodeTranslator.Translate(this.SelectedQuery.NameTextCodeCode);
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityId: this.Entity.DeclarationId,
                    SelectedTabCode: "DEGC",
                    ObjectTableName: "Customs.Declaration",
                    //BackButtonLabel: label
                });
                cmpRef.instance.BackCompleted.subscribe(($event1: any) => {
                    if (SessionLocator.SelectedSession != null && SessionLocator.SelectedSession.CurrentWindow != null) {
                        SessionLocator.SelectedSession.CurrentWindow.SuppressBusyIndicator = false;
                    }
                    this.OnBackFromEdit(this.Entity.DeclarationId, event)
                });
            });
    }


    OnBackFromEdit(selectedEntityId, $event) {
        if (SessionLocator.SelectedSession != null && SessionLocator.SelectedSession.CurrentListComponent != null) {
            SessionLocator.SelectedSession.CurrentListComponent.DoRefresh();
        }
    }


    ShowCFIUFILEFromDeclarationReferantData() {

        let myDeclarationReferantDataList: DeclarationReferantDataList = this.Entity;
        let myViewModelName = "FieldTemplateComponent.ts-ShowCFIUFILEFromDeclarationReferantData";
        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
            let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
                .subscribe(
                    (mess: UnifreightMessageM) => {
                        var IsMatchUnifreightCallbackCommand = (
                            mess.LogitudeEntity == AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
                            mess.LogitudeEntityNumber == myDeclarationReferantDataList.DeclarationId &&
                            mess.LogitudeViewModel == myViewModelName);
                        if (IsMatchUnifreightCallbackCommand) {
                            sub.unsubscribe();
                            SessionLocator.SelectedSession.StopBusyIndicator();
                            let sBool = UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
                            SessionLocator.SelectedSession.CurrentListComponent.DoRefresh();
                            //this.CurrentSession.PseventRowSelectEvent.emit({ Name: 'btnComponentComputingPartnerEdit', Value: this.rowData, RowIndex: this.AdditionalData.rowIndex });

                            //alert("reload");
                        }
                    }
                );

            SessionLocator.SelectedSession.StartBusyIndicator("");
            var unifreightMessageM =
                AmitalGatewayUtil.Instance.
                    DeclarationMessaging.GetMessage(myDeclarationReferantDataList.CustomFileNo, myDeclarationReferantDataList.DeclarationId,
                        myViewModelName);


            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
                "ScriptableGatewayUtil.ShowCFIUFILEFromDeclarationReferantDataList",
                "CFIHMAIN.LogitudeTask",
                "ShowCustomFileOPCFromDeclaration",
                unifreightMessageM,
                " הצגת מסך :הזנת תיק כללי עמילות מכס");

        }
        else {
            alert("ShowCustomFileOPCFromDeclaration");
        }

    }

    ShowGFUUSTS() {

        let myDeclarationReferantDataList: DeclarationReferantDataList = this.Entity;
        let myViewModelName = "FieldTemplateComponent.ts-ShowGFUUSTS";
        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
            let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
                .subscribe(
                    (mess: UnifreightMessageM) => {
                        var IsMatchUnifreightCallbackCommand = (
                            mess.LogitudeEntity == AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
                            mess.LogitudeEntityNumber == myDeclarationReferantDataList.DeclarationId &&
                            mess.LogitudeViewModel == myViewModelName);
                        if (IsMatchUnifreightCallbackCommand) {
                            sub.unsubscribe();
                            SessionLocator.SelectedSession.StopBusyIndicator();
                            let sBool = UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
                            SessionLocator.SelectedSession.CurrentListComponent.DoRefresh();
                            //this.CurrentSession.PseventRowSelectEvent.emit({ Name: 'btnComponentComputingPartnerEdit', Value: this.rowData, RowIndex: this.AdditionalData.rowIndex });

                            //alert("reload");
                        }
                    }
                );

            SessionLocator.SelectedSession.StartBusyIndicator("");
            var unifreightMessageM =
                AmitalGatewayUtil.Instance.
                    DeclarationMessaging.GetMessage(myDeclarationReferantDataList.CustomFileNo, myDeclarationReferantDataList.DeclarationId,
                        myViewModelName);


            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
                "ScriptableGatewayUtil.ShowCFIFILEMFUStatusScreenList",
                "CFIHMAIN.LogitudeTask",
                "ShowCFIFILEMFUStatusScreen",
                unifreightMessageM,
                " הצגת מסך :Follow Up Status");

        }
        else {
            alert("ShowGFUUSTS");
        }

    }

}
