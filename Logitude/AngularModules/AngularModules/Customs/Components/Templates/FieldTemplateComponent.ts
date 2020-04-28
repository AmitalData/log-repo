import {Component, ViewChild, ViewContainerRef, EventEmitter, ChangeDetectorRef} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {CourierMasterService} from '../../Services/Others/CourierMasterService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import { DeclarationRemarksService } from '../../../Common/Services/ExtendedPMs/DeclarationRemarksService';
import { ListComponentArgs } from '../../../Infrastructure/Args';

import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { DeclarationReferantDataList } from '../../EntityLists/DeclarationReferantDataList';
import { AmitalGatewayUtil, UnifreightMessageM } from '../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { ResourceLoader } from '@angular/compiler';

@Component({
    
    templateUrl: './FieldTemplateComponent.html',
})

export class FieldTemplateComponent {
    public Entity: any = null;
    public FieldName: string = null;
    public FieldValue: any = null;
    public ObjectTableName: string = null;
    public SpotlightDataTemplate: string = null;
    public IsSpotLightTemplate: boolean = false;
    public IsHeaderScreenTemplate: boolean = false;
    courierMasterService: CourierMasterService = new CourierMasterService();
    @ViewChild('SpotLight', { read: ViewContainerRef, static: false }) SpotLightViewContainerRef: ViewContainerRef;
    constructor() {

    }

    public ButtonClick() {
        this._ListComponentArgs.SuppressOnRowSelectedField = true;

     }
    ShowUnifaceCustomFile() {
        let myDeclarationReferantDataList: DeclarationReferantDataList = this.Entity;
        //myDeclarationReferantDataList.CustomFileNo

    }
    public Run(args: any) {
          this.Entity = args['Entity'];
        this.FieldName = args['FieldName'];
        this.ObjectTableName = args['ObjectTableName'];
        this.IsSpotLightTemplate = args['IsSpotLightTemplate'];
        this.SpotlightDataTemplate = args['SpotlightDataTemplate'];
        this.IsHeaderScreenTemplate = args['IsHeaderScreenTemplate'];
        if (this.Entity != null && this.FieldName != null) {
            this.FieldValue = this.Entity[this.FieldName];
        }

        if (this.IsSpotLightTemplate) {
            this.RunComponent();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    RunComponent() {
        if (this.SpotLightViewContainerRef) {
            this.SpotLightViewContainerRef.clear();

            var myComponentPath = "./Customs/Components/Spotlight/ReferantSpotlightDataTemplate";
            SessionLocator.DynamicLoader.Load(myComponentPath, this.SpotLightViewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.Run(this.Entity);
                });

        }

        else {
            this.RunComponentTimer();
        }
    }
    RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    OpenCourierMaster() {
        //static entityResourceService: EntityResourceService = new EntityResourceService();

        //entityResourceService.getEntityResourceByTableName("Customs.CourierMaster").subscribe((response:any) {
            //entityResourceService.getEntityResourceByTableName("Customs.DeclarationCourierStatus").subscribe((response:any) {
            this.courierMasterService.getCourierMasterByDeclarationId(this.Entity.Id).subscribe((response: ServiceResponse) => {
                if (response) {
                    if (!response.HasError) {
                        //this.EditEntity("Customs.CourierMaster", response.Result.Id, null, "COGN");
                        var windowArgs: any = {};
                        windowArgs.CurrentEntity = response.Result;
                        var logWindow = new LogitudeWindow();
                        logWindow.Width = 1500;
                        logWindow.Height = 1000;
                        logWindow.WindowArgs = windowArgs;
                        logWindow.ShowCloseButton = true;
                        //logWindow.IsHideHeader = true;
                        logWindow.IsFillScreen = true;
                        //AmitalGatewayUtil.Instance.IsAmitalBackButtonDisable = true;
                        logWindow.Show('./CustomsModules/CustomsCourier/Components/CourierWorkSheet/CourierWorksheetComponent');
                        logWindow.WindowClosed.subscribe(($event1: any) => {
                            //this.ShowCourierMasterByIdReturnCloseSaveCallBack(isSaved);
                        });

                    }
                }
                });
            //});
        //});

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
        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
            AmitalGatewayUtil.Instance.ShowCFIFILEMMoveToQueueScreen(
                this.Entity.CustomFileNo,
                this.Entity.Id,
                "ShowCFIFILEMMoveToQueueScreen");
        } else {
            var myMessageWindow = new MessageWindow();
            let mess = "ShowCFIFILEMMoveToQueueScreen -" + this.Entity.CustomFileNo;
            myMessageWindow.Show(mess);
        }
    }

    ShowCFIFILEMMoveSIToOCRScreen() {
        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
            AmitalGatewayUtil.Instance.ShowCFIFILEMMoveSIToOCRScreen(
                this.Entity.CustomFileNo,
                this.Entity.Id,
                "ShowCFIFILEMMoveSIToOCRScreen");
        } else {
            var myMessageWindow = new MessageWindow();
            let mess = "ShowCFIFILEMMoveSIToOCRScreen -" + this.Entity.CustomFileNo;
            myMessageWindow.Show(mess);
        }
    }

    public EditEntity(objectTableName: string, entityId: string, windowTitle: string, defaultSelectedTabCode: string) {


        var editWindow = new LogitudeWindow();

        editWindow.ShowHeaderButtons = true;
        editWindow.Title = windowTitle;
        editWindow.Height = 770;
        editWindow.Width = 1500;

        editWindow.ShowEditComponent(entityId, objectTableName, defaultSelectedTabCode);
        editWindow.WindowClosed.subscribe((res:any) => {


        });

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
                            mess.LogitudeEntityNumber == myDeclarationReferantDataList.DeclarationId  &&
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
                        myViewModelName );


            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
                "ScriptableGatewayUtil.ShowCFIUFILEFromDeclarationReferantDataList",
                "CFIHMAIN.LogitudeTask",
                "ShowCFIUFILEFromDeclarationReferantData",
                unifreightMessageM,
                " הצגת מסך :הזנת תיק כללי עמילות מכס");

        }
        else {
            alert("ShowCFIUFILEFromDeclarationReferantData");
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
                "ScriptableGatewayUtil.ShowCFIUFILEFromDeclarationReferantDataList",
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
