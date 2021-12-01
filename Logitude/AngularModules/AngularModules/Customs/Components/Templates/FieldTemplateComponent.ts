import { Component, ViewChild, ViewContainerRef, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { CourierMasterService } from '../../Services/Others/CourierMasterService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { CustomsAutonomyKeywordExtendedPMService } from '../../Services/ExtendedPMs/CustomsAutonomyKeywordExtendedPMService';
import { ListComponentArgs } from '../../../Infrastructure/Args';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { DeclarationRemarksService } from '../../../Common/Services/ExtendedPMs/DeclarationRemarksService';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { DeclarationReferantDataList } from '../../EntityLists/DeclarationRefernatDataList';
import { AmitalGatewayUtil, UnifreightMessageM } from '../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { ResourceLoader } from '@angular/compiler';
import { DeclarationReferantDataPMService } from '../../Services/StandardPMs/DeclarationReferantDataPMService';

import { ExceptionReasonExtendedListService } from '../../Services/ExtendedLists/ExceptionReasonExtendedListService';
import { ExceptionReasonListService } from '../../Services/StandardLists/ExceptionReasonListService';
import { ExceptionReasonList } from '../../EntityLists/ExceptionReasonList';

import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { PhysicalChecksCloseSharedDataService } from '../../Services/DataChange/PhysicalChecksCloseSharedDataService';
@Component({

    templateUrl: './FieldTemplateComponent.html',
    providers: [ListComponentArgs],
})

export class FieldTemplateComponent {
    public IsDisplayOnly: boolean = false;

    public Entity: any = null;
    public FieldName: string = null;
    public FieldValue: any = null;
    public ObjectTableName: string = null;
    public SpotlightDataTemplate: string = null;
    public IsSpotLightTemplate: boolean = false;
    public IsHeaderScreenTemplate: boolean = false;
    courierMasterService: CourierMasterService = new CourierMasterService();
    customsAutonomyKeywordExtendedPMService: CustomsAutonomyKeywordExtendedPMService = new CustomsAutonomyKeywordExtendedPMService();
    exceptionReasonExtendedListService: ExceptionReasonExtendedListService = new ExceptionReasonExtendedListService();
    private _ListComponentArgs: ListComponentArgs;
    @ViewChild('SpotLight', { read: ViewContainerRef, static: false }) SpotLightViewContainerRef: ViewContainerRef;
    RowIndex: any;
    constructor(private CD: ChangeDetectorRef, private entityResourceService: EntityResourceService, private _physicalChecksCloseSharedDataService: PhysicalChecksCloseSharedDataService) {
        if (SessionLocator.SelectedSession.CurrentListComponent != null) {
            this._ListComponentArgs = SessionLocator.SelectedSession.CurrentListComponent._ListComponentArgs;
        } else {
            this._ListComponentArgs = new ListComponentArgs();
        }
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
        this.RowIndex = args['RowIndex'];
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

                    cmpRef.instance.Run(this.Entity, this.SpotLightViewContainerRef, this.RowIndex);

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
    get LastStatusNameText() {
        if (AppTool.IsNullOrEmpty(this.Entity.LastStatusDate)) {
            return "";
        }
        var myFormats = DateTool.GetDateFormats(this.Entity.LastStatusDate);
        return myFormats.DateString;
    }
    get ExceptionReasonText() {
        var ToolTipValue: string = this.Entity.ExceptionReasonsList; 
        var list = ToolTipValue.split(',').filter(Boolean);
        if (list.length > 1) {
            return list.toString();
        }
        ToolTipValue = list[0];
        var myExceptionReasonListService = new ExceptionReasonListService();
        myExceptionReasonListService.getSingleFromCache(ToolTipValue)
            .subscribe(serviceResponse => {
                if (serviceResponse.Result != null) {
                    var ExceptionReason = serviceResponse.Result as ExceptionReasonList;
                    ToolTipValue = ExceptionReason.LocalName;
                }
            });
        return ToolTipValue;
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
    private _declarationReferantDataPMService: DeclarationReferantDataPMService = new DeclarationReferantDataPMService();
    EditFavorite() {
        this._ListComponentArgs.SuppressOnRowSelectedField = true;
        var x = this.Entity.SortedColumns;
        this.Entity.Favorite = !this.Entity.Favorite;
        this._declarationReferantDataPMService.update(this.Entity).subscribe((response: any) => {
            SessionLocator.SelectedSession.CurrentListComponent.OnBackFromEdit(this.Entity.DeclarationId, { rowIndex: this.RowIndex });
        });

    }
    EditMyCloseCheckBox(eventM) {
        debugger;
        this._ListComponentArgs.SuppressOnRowSelectedField = true;
        

        //eventM.stopPropagation();
        if (!this._physicalChecksCloseSharedDataService._SelectedItems.Collection.includes(this.Entity.Id)) {
            this._physicalChecksCloseSharedDataService._SelectedItems.Insert(this.Entity.Id);
        }
        else {
            var removedIndex = null;
            for (var i = 0; i < this._physicalChecksCloseSharedDataService._SelectedItems.Collection.length; i++) {
                if (this.Entity.Id == this._physicalChecksCloseSharedDataService._SelectedItems.Collection[i]) {
                    removedIndex = i;
                    break;
                }
            }
            if (removedIndex != null) {
                this._physicalChecksCloseSharedDataService._SelectedItems.Collection.splice(removedIndex, 1);
            }

        }


        this._physicalChecksCloseSharedDataService.IsDisplayButtonClose = (this._physicalChecksCloseSharedDataService._SelectedItems.Collection.length > 0);
        this.CD.detectChanges();

    }

    OpenClassificationRemarks() {
        this._ListComponentArgs.SuppressOnRowSelectedField = true;
        var _declarationRemarksService: DeclarationRemarksService = new DeclarationRemarksService();
        var windowArgs: any = {};
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Height = 800;//400;
        logitudeWindow.Width = 900;
        logitudeWindow.ShowCloseButton = true;
        if (this.Entity.IsClassificationRemarks) {

            this.entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
                this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoice").subscribe((response: any) => {
                    this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItem").subscribe((response: any) => {


                        _declarationRemarksService.GetSVCOrSRVStatusList(this.Entity.Tenant, this.Entity.CustomFileNo)
                            .subscribe((response: any) => {
                                windowArgs.EntityPM = response.Result;
                                windowArgs.length = response.Result.length;
                                windowArgs.title = "  הערות מסווג  ";
                                windowArgs.IsSivug = true;
                                windowArgs.DeclarationId = this.Entity.DeclarationId;

                                logitudeWindow.Title = windowArgs.length + "  הערות מסווג  ";
                                logitudeWindow.WindowArgs = windowArgs;
                                logitudeWindow.Show('./CustomsModules/CustomsMaintenance/Components/DeclarationRemarksComponent');
                            });
                    });
                });
            });
        }
    }
    OpenControllerRemarks() {
        this._ListComponentArgs.SuppressOnRowSelectedField = true;
        var _declarationRemarksService: DeclarationRemarksService = new DeclarationRemarksService();
        var windowArgs: any = {};
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Height = 400;
        logitudeWindow.Width = 700;
        logitudeWindow.ShowCloseButton = true;
        if (this.Entity.IsControllerRemarks) {
            _declarationRemarksService.GetINCorINAtatusList(this.Entity.Tenant, this.Entity.CustomFileNo)
                .subscribe((response: any) => {
                    windowArgs.EntityPM = response.Result;
                    windowArgs.title = "  הערות מבקר  ";
                    let counter = response.Result.length;
                    logitudeWindow.Title = counter + "  הערות מבקר  ";
                    windowArgs.IsSivug = false;
                    windowArgs.DeclarationId = this.Entity.DeclarationId;

                    logitudeWindow.WindowArgs = windowArgs;
                    logitudeWindow.Show('./CustomsModules/CustomsMaintenance/Components/DeclarationRemarksComponent');
                });
        }
    }



    DeleteAutonomyKey(value: number) {
        this._ListComponentArgs.SuppressOnRowSelectedField = true;
        if (!AppTool.IsNullOrEmpty(value)) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 400;
            confirmWindow.Height = 150;
            confirmWindow.Show("הםם םתה בטוח שברצונך למחוק םת שורת המפתח?");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) { // YES
                    this.customsAutonomyKeywordExtendedPMService.deleteByid(value).subscribe((response: ServiceResponse) => {
                        SessionLocator.SelectedSession.CurrentListComponent.DoRefresh();
                        SessionLocator.SelectedSession.CurrentListComponent.OnBackFromEdit(this.Entity.DeclarationId, { rowIndex: this.RowIndex });
                        this.CD.detectChanges();
                    });
                }
            });
        }
    }

    public EditEntity(objectTableName: string, entityId: string, windowTitle: string, defaultSelectedTabCode: string) {


        var editWindow = new LogitudeWindow();

        editWindow.ShowHeaderButtons = true;
        editWindow.Title = windowTitle;
        editWindow.Height = 770;
        editWindow.Width = 1500;

        editWindow.ShowEditComponent(entityId, objectTableName, defaultSelectedTabCode);
        editWindow.WindowClosed.subscribe(res => {


        });

    }


    ShowCFIFILEMMoveToQueueScreen() {
        this._ListComponentArgs.SuppressOnRowSelectedField = true;
        /*
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
        */

        let myDeclarationReferantDataList: DeclarationReferantDataList = this.Entity;
        let myViewModelName = "FieldTemplateComponent.ts-ShowCFIFILEMMoveToQueueScreen";
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
                            //SessionLocator.SelectedSession.CurrentListComponent.DoRefresh();
                            SessionLocator.SelectedSession.CurrentListComponent.OnBackFromEdit(this.Entity.DeclarationId, { rowIndex: this.RowIndex });
                            this.CD.detectChanges();
                        }
                    }
                );

            SessionLocator.SelectedSession.StartBusyIndicator("");
            var unifreightMessageM =
                AmitalGatewayUtil.Instance.
                    DeclarationMessaging.GetMessage(myDeclarationReferantDataList.CustomFileNo, myDeclarationReferantDataList.DeclarationId,
                        myViewModelName);


            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
                "ScriptableGatewayUtil.ShowCFIFILEMMoveToQueueScreen",
                "CFIHMAIN.LogitudeTask",
                "ShowCFIFILEMMoveToQueueScreen",
                unifreightMessageM,
                " הצגת מסך : העברה לתור");

        }
        else {
            alert("ShowCFIFILEMMoveToQueueScreen");
        }
    }


    PreShowCFIFILEMMoveSIToOCRScreen(value: string) {
        this._ListComponentArgs.SuppressOnRowSelectedField = true;
        if (value == 'X' || value == 'E') {
            this.ShowCFIFILEMMoveSIToOCRScreen();
        }
    }

    ShowCFIFILEMMoveSIToOCRScreen() {
        /*
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
        */

        let myDeclarationReferantDataList: DeclarationReferantDataList = this.Entity;
        let myViewModelName = "FieldTemplateComponent.ts-ShowCFIFILEMMoveSIToOCRScreen";
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
                            SessionLocator.SelectedSession.CurrentListComponent.OnBackFromEdit(this.Entity.DeclarationId, { rowIndex: this.RowIndex });
                        }
                    }
                );

            SessionLocator.SelectedSession.StartBusyIndicator("");
            var unifreightMessageM =
                AmitalGatewayUtil.Instance.
                    DeclarationMessaging.GetMessage(myDeclarationReferantDataList.CustomFileNo, myDeclarationReferantDataList.DeclarationId,
                        myViewModelName);


            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
                "ScriptableGatewayUtil.ShowCFIFILEMMoveSIToOCRScreen",
                "CFIHMAIN.LogitudeTask",
                "ShowCFIFILEMMoveSIToOCRScreen",
                unifreightMessageM,
                " הצגת מסך : העברת חשבונות ספק ל- OCR");

        }
        else {
            alert("ShowCFIFILEMMoveSIToOCRScreen");
        }
    }


    ShowCFIFILEMMoveToCollector() {
        this._ListComponentArgs.SuppressOnRowSelectedField = true;
        

        let myDeclarationReferantDataList: DeclarationReferantDataList = this.Entity;
        let myViewModelName = "FieldTemplateComponent.ts-ShowCFIFILEMMoveToCollector";
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
                            SessionLocator.SelectedSession.CurrentListComponent.OnBackFromEdit(this.Entity.DeclarationId, { rowIndex: this.RowIndex });
                            this.CD.detectChanges();
                        }
                    }
                );

            SessionLocator.SelectedSession.StartBusyIndicator("");
            var unifreightMessageM =
                AmitalGatewayUtil.Instance.
                    DeclarationMessaging.GetMessage(myDeclarationReferantDataList.CustomFileNo, myDeclarationReferantDataList.DeclarationId,
                        myViewModelName);


            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
                "ScriptableGatewayUtil.ShowCFIFILEMMoveToCollector",
                "CFIHMAIN.LogitudeTask",
                "ShowCFIFILEMMoveToCollector",
                unifreightMessageM,
                " הצגת מסך : העברה לגובה");

        }
        else {
            alert("ShowCFIFILEMMoveToCollector");
        }
    }


    ShowCFIFILEMEnterRemarks() {
        /*
        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
            AmitalGatewayUtil.Instance.ShowCFIFILEMEnterRemarks(
                this.Entity.CustomFileNo,
                this.Entity.DeclarationId,
                "ShowCFIFILEMEnterRemarks");
        } else {
            var myMessageWindow = new MessageWindow();
            let mess = "ShowCFIFILEMEnterRemarks -" + this.Entity.CustomFileNo;
            myMessageWindow.Show(mess);
        }
        */

        let myDeclarationReferantDataList: DeclarationReferantDataList = this.Entity;
        let myViewModelName = "FieldTemplateComponent.ts-ShowCFIFILEMEnterRemarks";
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
                            SessionLocator.SelectedSession.CurrentListComponent.OnBackFromEdit(this.Entity.DeclarationId, { rowIndex: this.RowIndex });;
                        }
                    }
                );

            SessionLocator.SelectedSession.StartBusyIndicator("");
            var unifreightMessageM =
                AmitalGatewayUtil.Instance.
                    DeclarationMessaging.GetMessage(myDeclarationReferantDataList.CustomFileNo, myDeclarationReferantDataList.DeclarationId,
                        myViewModelName);


            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
                "ScriptableGatewayUtil.ShowCFIFILEMEnterRemarks",
                "CFIHMAIN.LogitudeTask",
                "ShowCFIFILEMEnterRemarks",
                unifreightMessageM,
                " הצגת מסך : הזנת הערות לתור");

        }
        else {
            alert("ShowCFIFILEMEnterRemarks");
        }

    }

    ShowSharedDocuments() {
        if (this.Entity.IsCustomerLogBoxActivated) {
            let myDeclarationReferantDataList: DeclarationReferantDataList = this.Entity;
            let myViewModelName = "FieldTemplateComponent.ts-ShowSharedDocuments";
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
                                SessionLocator.SelectedSession.CurrentListComponent.OnBackFromEdit(this.Entity.DeclarationId, { rowIndex: this.RowIndex });;
                            }
                        }
                    );

                SessionLocator.SelectedSession.StartBusyIndicator("");
                var unifreightMessageM =
                    AmitalGatewayUtil.Instance.
                        DeclarationMessaging.GetMessage(myDeclarationReferantDataList.CustomFileNo, myDeclarationReferantDataList.DeclarationId,
                            myViewModelName);


                AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
                    "ScriptableGatewayUtil.ShowSharedDocuments",
                    "CFIHMAIN.LogitudeTask",
                    "ShowSharedDocuments",
                    unifreightMessageM,
                    " הצגת מסך : שיתוף מסמכים");

            }
            else {
                alert("ShowSharedDocuments");
            }
        }
    }

    ShowMoneyOrder() {
        let myDeclarationReferantDataList: DeclarationReferantDataList = this.Entity;
        let myViewModelName = "FieldTemplateComponent.ts-ShowMoneyOrder";
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
                            SessionLocator.SelectedSession.CurrentListComponent.OnBackFromEdit(this.Entity.DeclarationId, { rowIndex: this.RowIndex });;
                        }
                    }
                );

            SessionLocator.SelectedSession.StartBusyIndicator("");
            var unifreightMessageM =
                AmitalGatewayUtil.Instance.
                    DeclarationMessaging.GetMessage(myDeclarationReferantDataList.CustomFileNo, myDeclarationReferantDataList.DeclarationId,
                        myViewModelName);


            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
                "ScriptableGatewayUtil.ShowMoneyOrder",
                "CFIHMAIN.LogitudeTask",
                "ShowMoneyOrder",
                unifreightMessageM,
                " הצגת מסך : הזמנת כסף");

        }
        else {
            alert("ShowMoneyOrder");
        }

    }

    ShowDelivery() {
        let myDeclarationReferantDataList: DeclarationReferantDataList = this.Entity;
        let myViewModelName = "FieldTemplateComponent.ts-ShowDelivery";
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
                            SessionLocator.SelectedSession.CurrentListComponent.OnBackFromEdit(this.Entity.DeclarationId, { rowIndex: this.RowIndex });;
                        }
                    }
                );

            SessionLocator.SelectedSession.StartBusyIndicator("");
            var unifreightMessageM =
                AmitalGatewayUtil.Instance.
                    DeclarationMessaging.GetMessage(myDeclarationReferantDataList.CustomFileNo, myDeclarationReferantDataList.DeclarationId,
                        myViewModelName);


            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
                "ScriptableGatewayUtil.ShowDelivery",
                "CFIHMAIN.LogitudeTask",
                "ShowDelivery",
                unifreightMessageM,
                " הצגת מסך : הובלה יבשתית");

        }
        else {
            alert("ShowDelivery");
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
                    this.OnBackFromEdit(this.Entity.DeclarationId, event);
                });
            });
    }


    OnBackFromEdit(selectedEntityId, $event) {
        if (SessionLocator.SelectedSession != null && SessionLocator.SelectedSession.CurrentListComponent != null) {
            SessionLocator.SelectedSession.CurrentListComponent.OnBackFromEdit(this.Entity.DeclarationId, { rowIndex: this.RowIndex });
        }
    }

    ShowAddOrEditExceptionReason() {
        this._ListComponentArgs.SuppressOnRowSelectedField = true;
        var logitudeWindow = new LogitudeWindow();
        var windowArgs: any = {};
        this._declarationReferantDataPMService.get(this.Entity.DeclarationId).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                windowArgs.EntityPM = response.Result
                windowArgs.RowIndex = this.RowIndex
                logitudeWindow.Width = 470;
                logitudeWindow.Height = 350;
                logitudeWindow.IsShowCloseButton = false;
                logitudeWindow.Title = "הזנת חריג";
                logitudeWindow.WindowArgs = windowArgs;
                logitudeWindow.Show('./CustomsModules/CustomsReferant/Components/ReferantExceptionReason/AddEditExceptionReasonComponent');
                logitudeWindow.WindowClosed.subscribe(($event: any) => {
                    SessionLocator.SelectedSession.CurrentListComponent.OnBackFromEdit(this.Entity.DeclarationId, { rowIndex: this.RowIndex });
                });
            }
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
                            mess.LogitudeEntityNumber == myDeclarationReferantDataList.DeclarationId &&
                            mess.LogitudeViewModel == myViewModelName);
                        if (IsMatchUnifreightCallbackCommand) {
                            sub.unsubscribe();
                            SessionLocator.SelectedSession.StopBusyIndicator();
                            let sBool = UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
                            SessionLocator.SelectedSession.CurrentListComponent.OnBackFromEdit(this.Entity.DeclarationId, { rowIndex: this.RowIndex });
                            //SessionLocator.SelectedSession.CurrentListComponent.DoRefresh();
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
                            //SessionLocator.SelectedSession.CurrentListComponent.DoRefresh();
                            SessionLocator.SelectedSession.CurrentListComponent.OnBackFromEdit(this.Entity.DeclarationId, { rowIndex: this.RowIndex });
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

//class MyClass {

//    ShowCFIFILEMMoveToQueueScreen() {
//        this._ListComponentArgs.SuppressOnRowSelectedField = true;
//        /*
//        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
//            AmitalGatewayUtil.Instance.ShowCFIFILEMMoveToQueueScreen(
//                this.Entity.CustomFileNo,
//                this.Entity.DeclarationId,
//                "ShowCFIFILEMMoveToQueueScreen");
//        } else {
//            var myMessageWindow = new MessageWindow();
//            let mess = "ShowCFIFILEMMoveToQueueScreen -" + this.Entity.CustomFileNo;
//            myMessageWindow.Show(mess);
//        }
//        */

//        let myDeclarationReferantDataList: DeclarationReferantDataList = this.Entity;
//        let myViewModelName = "FieldTemplateComponent.ts-ShowCFIFILEMMoveToQueueScreen";
//        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
//            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
//            let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
//                .subscribe(
//                    (mess: UnifreightMessageM) => {
//                        var IsMatchUnifreightCallbackCommand = (
//                            mess.LogitudeEntity == AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
//                            mess.LogitudeEntityNumber == myDeclarationReferantDataList.DeclarationId &&
//                            mess.LogitudeViewModel == myViewModelName);
//                        if (IsMatchUnifreightCallbackCommand) {
//                            sub.unsubscribe();
//                            SessionLocator.SelectedSession.StopBusyIndicator();
//                            let sBool = UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
//                            SessionLocator.SelectedSession.CurrentListComponent.DoRefresh();
//                        }
//                    }
//                );

//            SessionLocator.SelectedSession.StartBusyIndicator("");
//            var unifreightMessageM =
//                AmitalGatewayUtil.Instance.
//                    DeclarationMessaging.GetMessage(myDeclarationReferantDataList.CustomFileNo, myDeclarationReferantDataList.DeclarationId,
//                        myViewModelName);


//            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//                "ScriptableGatewayUtil.ShowCFIFILEMMoveToQueueScreen",
//                "CFIHMAIN.LogitudeTask",
//                "ShowCFIFILEMMoveToQueueScreen",
//                unifreightMessageM,
//                " הצגת מסך : העברה לתור");

//        }
//        else {
//            alert("ShowCFIFILEMMoveToQueueScreen");
//        }
//    }


//    PreShowCFIFILEMMoveSIToOCRScreen(value: string) {
//        this._ListComponentArgs.SuppressOnRowSelectedField = true;
//        if (value == 'X' || value == 'E') {
//            this.ShowCFIFILEMMoveSIToOCRScreen();
//        }
//    }

//    ShowCFIFILEMMoveSIToOCRScreen() {
//        /*
//        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
//            AmitalGatewayUtil.Instance.ShowCFIFILEMMoveSIToOCRScreen(
//                this.Entity.CustomFileNo,
//                this.Entity.DeclarationId,
//                "ShowCFIFILEMMoveSIToOCRScreen");
//        } else {
//            var myMessageWindow = new MessageWindow();
//            let mess = "ShowCFIFILEMMoveSIToOCRScreen -" + this.Entity.CustomFileNo;
//            myMessageWindow.Show(mess);
//        }
//        */

//        let myDeclarationReferantDataList: DeclarationReferantDataList = this.Entity;
//        let myViewModelName = "FieldTemplateComponent.ts-ShowCFIFILEMMoveSIToOCRScreen";
//        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
//            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
//            let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
//                .subscribe(
//                    (mess: UnifreightMessageM) => {
//                        var IsMatchUnifreightCallbackCommand = (
//                            mess.LogitudeEntity == AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
//                            mess.LogitudeEntityNumber == myDeclarationReferantDataList.DeclarationId &&
//                            mess.LogitudeViewModel == myViewModelName);
//                        if (IsMatchUnifreightCallbackCommand) {
//                            sub.unsubscribe();
//                            SessionLocator.SelectedSession.StopBusyIndicator();
//                            let sBool = UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
//                            SessionLocator.SelectedSession.CurrentListComponent.DoRefresh();
//                        }
//                    }
//                );

//            SessionLocator.SelectedSession.StartBusyIndicator("");
//            var unifreightMessageM =
//                AmitalGatewayUtil.Instance.
//                    DeclarationMessaging.GetMessage(myDeclarationReferantDataList.CustomFileNo, myDeclarationReferantDataList.DeclarationId,
//                        myViewModelName);


//            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//                "ScriptableGatewayUtil.ShowCFIFILEMMoveSIToOCRScreen",
//                "CFIHMAIN.LogitudeTask",
//                "ShowCFIFILEMMoveSIToOCRScreen",
//                unifreightMessageM,
//                " הצגת מסך : העברת חשבונות ספק ל- OCR");

//        }
//        else {
//            alert("ShowCFIFILEMMoveSIToOCRScreen");
//        }
//    }


//    ShowCFIFILEMEnterRemarks() {
//        /*
//        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
//            AmitalGatewayUtil.Instance.ShowCFIFILEMEnterRemarks(
//                this.Entity.CustomFileNo,
//                this.Entity.DeclarationId,
//                "ShowCFIFILEMEnterRemarks");
//        } else {
//            var myMessageWindow = new MessageWindow();
//            let mess = "ShowCFIFILEMEnterRemarks -" + this.Entity.CustomFileNo;
//            myMessageWindow.Show(mess);
//        }
//        */

//        let myDeclarationReferantDataList: DeclarationReferantDataList = this.Entity;
//        let myViewModelName = "FieldTemplateComponent.ts-ShowCFIFILEMEnterRemarks";
//        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
//            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
//            let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
//                .subscribe(
//                    (mess: UnifreightMessageM) => {
//                        var IsMatchUnifreightCallbackCommand = (
//                            mess.LogitudeEntity == AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
//                            mess.LogitudeEntityNumber == myDeclarationReferantDataList.DeclarationId &&
//                            mess.LogitudeViewModel == myViewModelName);
//                        if (IsMatchUnifreightCallbackCommand) {
//                            sub.unsubscribe();
//                            SessionLocator.SelectedSession.StopBusyIndicator();
//                            let sBool = UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
//                            SessionLocator.SelectedSession.CurrentListComponent.DoRefresh();
//                        }
//                    }
//                );

//            SessionLocator.SelectedSession.StartBusyIndicator("");
//            var unifreightMessageM =
//                AmitalGatewayUtil.Instance.
//                    DeclarationMessaging.GetMessage(myDeclarationReferantDataList.CustomFileNo, myDeclarationReferantDataList.DeclarationId,
//                        myViewModelName);


//            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//                "ScriptableGatewayUtil.ShowCFIFILEMEnterRemarks",
//                "CFIHMAIN.LogitudeTask",
//                "ShowCFIFILEMEnterRemarks",
//                unifreightMessageM,
//                " הצגת מסך : הזנת הערות לתור");

//        }
//        else {
//            alert("ShowCFIFILEMEnterRemarks");
//        }

//    }


//    ShowDeclaration(event) {
//        if (SessionLocator.SelectedSession != null && SessionLocator.SelectedSession.CurrentWindow != null) {
//            SessionLocator.SelectedSession.CurrentWindow.SuppressBusyIndicator = true;
//        }
//        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.SelectedSession.SessionLocation.viewContainerRef)
//            .then(cmpRef => {
//                //var label = TextCodeTranslator.Translate(this.SelectedQuery.NameTextCodeCode);
//                cmpRef.instance.ComponentRef = cmpRef;
//                cmpRef.instance.Run({
//                    EntityId: this.Entity.DeclarationId,
//                    SelectedTabCode: "DEGC",
//                    ObjectTableName: "Customs.Declaration",
//                    //BackButtonLabel: label
//                });
//                cmpRef.instance.BackCompleted.subscribe(($event1: any) => {
//                    if (SessionLocator.SelectedSession != null && SessionLocator.SelectedSession.CurrentWindow != null) {
//                        SessionLocator.SelectedSession.CurrentWindow.SuppressBusyIndicator = false;
//                    }
//                    this.OnBackFromEdit(this.Entity.DeclarationId, event)
//                });
//            });
//    }


//    OnBackFromEdit(selectedEntityId, $event) {
//        if (SessionLocator.SelectedSession != null && SessionLocator.SelectedSession.CurrentListComponent != null) {
//            SessionLocator.SelectedSession.CurrentListComponent.DoRefresh();
//        }
//    }

//    ShowCFIUFILEFromDeclarationReferantData() {

//        let myDeclarationReferantDataList: DeclarationReferantDataList = this.Entity;
//        let myViewModelName = "FieldTemplateComponent.ts-ShowCFIUFILEFromDeclarationReferantData";
//        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
//            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
//            let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
//                .subscribe(
//                    (mess: UnifreightMessageM) => {
//                        var IsMatchUnifreightCallbackCommand = (
//                            mess.LogitudeEntity == AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
//                            mess.LogitudeEntityNumber == myDeclarationReferantDataList.DeclarationId &&
//                            mess.LogitudeViewModel == myViewModelName);
//                        if (IsMatchUnifreightCallbackCommand) {
//                            sub.unsubscribe();
//                            SessionLocator.SelectedSession.StopBusyIndicator();
//                            let sBool = UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
//                            SessionLocator.SelectedSession.CurrentListComponent.DoRefresh();
//                            //this.CurrentSession.PseventRowSelectEvent.emit({ Name: 'btnComponentComputingPartnerEdit', Value: this.rowData, RowIndex: this.AdditionalData.rowIndex });

//                            //alert("reload");
//                        }
//                    }
//                );

//            SessionLocator.SelectedSession.StartBusyIndicator("");
//            var unifreightMessageM =
//                AmitalGatewayUtil.Instance.
//                    DeclarationMessaging.GetMessage(myDeclarationReferantDataList.CustomFileNo, myDeclarationReferantDataList.DeclarationId,
//                        myViewModelName);


//            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//                "ScriptableGatewayUtil.ShowCFIUFILEFromDeclarationReferantDataList",
//                "CFIHMAIN.LogitudeTask",
//                "ShowCustomFileOPCFromDeclaration",
//                unifreightMessageM,
//                " הצגת מסך :הזנת תיק כללי עמילות מכס");

//        }
//        else {
//            alert("ShowCustomFileOPCFromDeclaration");
//        }

//    }

//    ShowGFUUSTS() {

//        let myDeclarationReferantDataList: DeclarationReferantDataList = this.Entity;
//        let myViewModelName = "FieldTemplateComponent.ts-ShowGFUUSTS";
//        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
//            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
//            let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
//                .subscribe(
//                    (mess: UnifreightMessageM) => {
//                        var IsMatchUnifreightCallbackCommand = (
//                            mess.LogitudeEntity == AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
//                            mess.LogitudeEntityNumber == myDeclarationReferantDataList.DeclarationId &&
//                            mess.LogitudeViewModel == myViewModelName);
//                        if (IsMatchUnifreightCallbackCommand) {
//                            sub.unsubscribe();
//                            SessionLocator.SelectedSession.StopBusyIndicator();
//                            let sBool = UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
//                            SessionLocator.SelectedSession.CurrentListComponent.DoRefresh();
//                            //this.CurrentSession.PseventRowSelectEvent.emit({ Name: 'btnComponentComputingPartnerEdit', Value: this.rowData, RowIndex: this.AdditionalData.rowIndex });

//                            //alert("reload");
//                        }
//                    }
//                );

//            SessionLocator.SelectedSession.StartBusyIndicator("");
//            var unifreightMessageM =
//                AmitalGatewayUtil.Instance.
//                    DeclarationMessaging.GetMessage(myDeclarationReferantDataList.CustomFileNo, myDeclarationReferantDataList.DeclarationId,
//                        myViewModelName);


//            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
//                "ScriptableGatewayUtil.ShowCFIFILEMFUStatusScreenList",
//                "CFIHMAIN.LogitudeTask",
//                "ShowCFIFILEMFUStatusScreen",
//                unifreightMessageM,
//                " הצגת מסך :Follow Up Status");

//        }
//        else {
//            alert("ShowGFUUSTS");
//        }

//    }

//}
