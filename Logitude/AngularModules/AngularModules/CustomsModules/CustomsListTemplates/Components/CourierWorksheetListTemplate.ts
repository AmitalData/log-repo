
declare var window: any;
import { EditComponent } from "../../../Infrastructure/Components/EditComponent/EditComponent";
import {WebFreightDomainService} from '../../../Infrastructure/Services/WebFreightDomainService';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { Component, ChangeDetectorRef } from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ARPaymentExtendedListService} from '../../../Invoice/Services/ExtendedLists/ARPaymentExtendedListService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import { CustomsRequestMenuService } from '../../../Customs/Services/Others/CustomsRequestMenuService';
import { CustomsRequestSheetExtendedPMService } from '../../../Customs/Services/ExtendedPMs/CustomsRequestSheetExtendedPMService';
import { SendALLCorrectRequestParams } from '../../../Customs/DataContract/RequestParams/SendALLCorrectRequestParams';
import { ResponseDataBase } from '../../../Customs/DataContract/ResponseData/ResponseDataBase';
import { CustomsRequestsSheetPM } from '../../../Customs/EntityPMs/CustomsRequestsSheetPM';
import { CourierMasterService } from '../../../Customs/Services/Others/CourierMasterService';
import {SendRequestVIA} from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import {ShowProgressBarParams, CustomMessageProgressComponent} from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import {DeclarationWebService} from '../../../Customs/Services/WebServices/DeclarationWebService';
import { CourierWorksheetSharedDataService } from '../../../Customs/Services/DataChange/CourierWorksheetSharedDataService';
import { SendDeclarationService } from '../../../CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/SendDeclaration/SendDeclarationComponent';
import { SendManifestService } from '../../../CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/SendDeclaration/SendManifestComponent';
import {DeclarationPMService} from '../../../Customs/Services/StandardPMs/DeclarationPMService';
import {DropdownMenuFilterComponent} from '../../../CustomsModules/CustomsCourier/Components/CourierWorkSheet/DropdownMenuFilterComponent';
import {DeclarationCourierStatusPMService} from '../../../Customs/Services/StandardPMs/DeclarationCourierStatusPMService';
import { DeclarationCourierStatusPM } from '../../../Customs/EntityPMs/DeclarationCourierStatusPM';
import { DeclarationPM } from '../../../Customs/EntityPMs/DeclarationPM';
import { DeclarationCourierStatusList } from '../../../Customs/EntityLists/DeclarationCourierStatusList';
import { DeclarationCourierStatusListService } from '../../../Customs/Services/StandardLists/DeclarationCourierStatusListService';
import { DeclarationMamanSpecialActionListService } from '../../../Customs/Services/StandardLists/DeclarationMamanSpecialActionListService';
import { DeclarationMamanSpecialActionPM } from '../../../Customs/EntityPMs/DeclarationMamanSpecialActionPM';
import { DeclarationMamanSpecialActionPMService } from '../../../Customs/Services/StandardPMs/DeclarationMamanSpecialActionPMService';
import { DeclarationCourierStatusWebService } from '../../../Customs/Services/WebServices/DeclarationCourierStatusWebService';
import { retry } from 'rxjs/operator/retry';
import { forEach } from "@angular/router/src/utils/collection";
import { DeclarationMamanSpecialActionList } from "../../../Customs/EntityLists/DeclarationMamanSpecialActionList";
import { EntityResourceService } from "../../../Infrastructure/Services/EntityResourceService";

@Component({
    moduleId: module.id,
    templateUrl: './CourierWorksheetListTemplate.html',
})

export class CourierWorksheetListTemplate {

    _CourierWorksheet: DeclarationCourierStatusList;
    public fieldName: any;

    IsDocumentStatusGreen: boolean = false;
    IsDocumentStatusRed: boolean = false;
    IsDocumentStatusBlue: boolean = false;
    IsManifestStatusRed: boolean = false;
    IsManifestStatusGreen: boolean = false;
    IsManifestStatusBlue: boolean = false;
    IsManifestStatusOrange: boolean = false;
    IsDeclarationStatusRed: boolean = false;
    IsDeclarationStatusGreen: boolean = false;
    IsDeclarationStatusBlue: boolean = false;
    IsDeclarationStatusOrange: boolean = false;
    IsPaymentStatusBlueChecked: boolean = false;
    IsPaymentStatusGreen: boolean = false;
    IsPaymentStatusBlue: boolean = false;
    IsPaymentStatusOrange: boolean = false;
    IsHighLow: boolean = false;
    IsDeclarationChecked: boolean = false;
    SuspentionReasonText: string;
    SuspentionReasonTip: string;

    DelayCertificateDetails: DeclarationMamanSpecialActionPM = null;
    MamanStickerDetails: DeclarationMamanSpecialActionPM = null;
    PrintDocumentsDetails: DeclarationMamanSpecialActionPM = null;
    IsReceivingDelayCertificate: boolean = false;
    IsPrintDocuments: boolean = false;
    IsMamanSticker: boolean = false;

    private _DeclarationCourierStatusPMService: DeclarationCourierStatusPMService = new DeclarationCourierStatusPMService();
    private _CourierMasterService: CourierMasterService = new CourierMasterService();
    private _DeclarationMamanSpecialActionListService: DeclarationMamanSpecialActionListService = new DeclarationMamanSpecialActionListService();
    private _DeclarationMamanSpecialActionPMService: DeclarationMamanSpecialActionPMService = new DeclarationMamanSpecialActionPMService;
    private _DeclarationWebService: DeclarationWebService = new DeclarationWebService;
    private _DeclarationCourierStatusWebService: DeclarationCourierStatusWebService = new DeclarationCourierStatusWebService();

    FirePreventSelect() {
        SessionLocator.CurrentSession.PseventRowSelectEvent.emit("CourierWorksheetListTemplate.SendSplitButton");
    }
    FireUnSelect() {
        SessionLocator.CurrentSession.PseventRowSelectEvent.emit("FireUnSelect");
    }

    //  @ViewChild( SplitButtonComponent)  public MySplitButtonComponent: SplitButtonComponent = new SplitButtonComponent(null,null);
    //@ViewChild('ShortTitle', { read: ViewContainerRef }) ShortTitleViewContainerRef: ViewContainerRef;
    //@ViewChild('MySplitButtonComponent', { read: SplitButtonComponent }) MySplitButtonComponent: SplitButtonComponent;

    constructor(private _CourierWorksheetSharedDataService: CourierWorksheetSharedDataService, private CD: ChangeDetectorRef) {

    }

    //[AdditionalData] = "{rowIndex:row.rowIndex,gridId:LogGridId,RowOutEvent:RowOutEvent,RowOverEvent:RowOverEvent}"
    RefreshData() {

        var noLocal = true;
        if (noLocal) {
            this._CourierWorksheetSharedDataService.SendNextMessage("DoRefresh");
            return;
        }

        var myDeclarationCourierStatusListService = new DeclarationCourierStatusListService();
        myDeclarationCourierStatusListService.getSingle(this._CourierWorksheet.DeclarationId)
            .subscribe(serviceResponse => {
                //this._CourierWorksheet = serviceResponse.Result;
                //this._CourierWorksheet.CourierCustomStatusCode = "X";
                var courierWorksheet = serviceResponse.Result as DeclarationCourierStatusList;
                courierWorksheet.CourierCustomStatusCode = "X";
                //this.CD.detectChanges();
                this.setVariables(courierWorksheet, this.fieldName);/*, AdditionalData:any)*/
            });


    }
    DropdownMenuButtonClicked(event) {
        this.ButtonClick(event);
    }
    DropdownDisplayClose() {
        //if (!AppTool.IsNullOrEmpty(this.MySplitButtonComponent)) {
        //  this.MySplitButtonComponent.DropdownDisplayClose();
        //}
    }



    setVariables(courierWorksheet: DeclarationCourierStatusList, fieldName: string)/*, AdditionalData:any)*/ {
        this._CourierWorksheet = courierWorksheet;
        this.fieldName = fieldName;
        //this._AdditionalData = AdditionalData;
        this.DropdownDisplayClose();
        DropdownMenuFilterComponent.EnsureLastDropdownMenuIsClosed();

        if (this._CourierWorksheet.DocumentStatusCode != null) {
            switch (this._CourierWorksheet.DocumentStatusCode) {
                case "M":
                case "X": {
                    this.IsDocumentStatusRed = true;
                    break;
                }
                case "V": {
                    this.IsDocumentStatusGreen = true;
                    break;
                }
                case "I": {
                    this.IsDocumentStatusBlue = true;
                    break;
                }
            }
        }

        if (this._CourierWorksheet.CourierManifestStatusCode != null) {
            switch (this._CourierWorksheet.CourierManifestStatusCode) {
                case "M":
                case "X": {
                    this.IsManifestStatusRed = true;
                    break;
                }
                case "V": {
                    this.IsManifestStatusGreen = true;
                    break;
                }
                case "I": {
                    this.IsManifestStatusBlue = true;
                    break;
                }
                case "R": {
                    this.IsManifestStatusOrange = true;
                    break;
                }
            }
        }

        if (this._CourierWorksheet.CourierDeclarationStatusCode != null) {
            switch (this._CourierWorksheet.CourierDeclarationStatusCode) {
                case "M":
                case "X": {
                    this.IsDeclarationStatusRed = true;
                    break;
                }
                case "V": {
                    this.IsDeclarationStatusGreen = true;
                    break;
                }
                case "I": {
                    this.IsDeclarationStatusBlue = true;
                    break;
                }
                case "R": {
                    this.IsDeclarationStatusOrange = true;
                    break;
                }
            }
        }

        if (this._CourierWorksheet.CourierPaymentStatusCode != null) {
            switch (this._CourierWorksheet.CourierPaymentStatusCode) {
                case "R": {
                    this.IsPaymentStatusOrange = true;
                    break;
                }
                case "P": {
                    this.IsPaymentStatusBlueChecked = true;
                    break;
                }
                case "I": {
                    this.IsPaymentStatusBlue = true;
                    break;
                }
                case "O": {
                    this.IsPaymentStatusGreen = true;
                    break;
                }
            }
        }

        if (this._CourierWorksheet.HighLowValue == "H"
            || (this._CourierWorksheet.HighLowValue == "L" && this._CourierWorksheet.CourierCustomStatusCode == "2")) {
            this.IsHighLow = true;
        }
        else {
            this.IsHighLow = false;
        }


        if (this._CourierWorksheet.CourierCustomStatusCode == "2") {
            this.SuspentionReasonTip = this._CourierWorksheet.CourierSuspentionName;
        }

        this.SuspentionReasonText = this._CourierWorksheet.CourierCustomStatusName;

        this.BuildDeclarationsCheckBox();
        this.CD.detectChanges();
    }

    BuildDeclarationsCheckBox() {
        if (this._CourierWorksheetSharedDataService._SelectedItems.Collection.includes(this._CourierWorksheet.DeclarationId)) {
            this.IsDeclarationChecked = true;
        }
        else {
            this.IsDeclarationChecked = false;
        }
    }

    SendManifest(event) {
        this.ButtonClick(event);
        let myDeclarationPMService: DeclarationPMService = new DeclarationPMService()
        myDeclarationPMService.get(this._CourierWorksheet['DeclarationId'])
            .subscribe(rsptPMget => {
                let entitypm = rsptPMget.Result;
                let objectTable = window.ObjectTables.filter(d => d.Name === 'Customs.Declaration')[0];
                let _SendManifestService: SendManifestService = new SendManifestService();
                _SendManifestService.Run({ EntityPM: entitypm, ObjectTable: objectTable, CourierWorksheetmode: true });
                _SendManifestService.OnSuccessSendMethod =
                    (res1) => {
                        SessionLocator.CurrentSession.StopBusyIndicator();
                        //this._CourierWorksheetSharedDataService.SendNextMessage("DoRefresh");
                        this.RefreshData();
                    };
                _SendManifestService.OnCustomSendOptionsButtonClick({
                    RequestVIA: SendRequestVIA.WebServiceInteractive,
                    Option: "WI",
                    ForcePersonalSign: false
                });

            });



    }

    SendButtonClicked() {
        this.ButtonClick(null);
    }

    _IsSplitButtonMenuFilterReady: boolean = false;
    PrepareSplitButtonMenuFilter(){
        this._IsSplitButtonMenuFilterReady = false;
        this.IsWebAPICourierGWMessageECTHRDataMamanEnable = false;
        let myDeclarationPMService: DeclarationPMService = new DeclarationPMService()
        myDeclarationPMService.get(this._CourierWorksheet['DeclarationId']).subscribe(rsptPMget => {
            let entitypm: DeclarationPM = rsptPMget.Result;
            if (entitypm != null && entitypm.Consignments != null) {
                if (this.WebAPICourierGWMessageECTHRDataMaman.includes(entitypm.Consignments[0].StorageSiteCode)) {
                    this.IsWebAPICourierGWMessageECTHRDataMamanEnable = true;
                }
            }
            this._IsSplitButtonMenuFilterReady = true;
            this.CD.detectChanges();
        });
        this.CD.detectChanges();
    }

    SendDec(event) {
        //event.stopPropagation();
        //SplitButtonComponent.EnsureLastSplitButtonIsClosed();
        //DropdownMenuFilterComponent.EnsureLastDropdownMenuIsClosed();
        this.ButtonClick(event);

        let myDeclarationPMService: DeclarationPMService = new DeclarationPMService()
        myDeclarationPMService.get(this._CourierWorksheet['DeclarationId'])
            .subscribe(rsptPMget => {
                let entitypm = rsptPMget.Result;
                let objectTable = window.ObjectTables.filter(d => d.Name === 'Customs.Declaration')[0];
                let _SendDeclarationService: SendDeclarationService = new SendDeclarationService();
                _SendDeclarationService.Run({ EntityPM: entitypm, ObjectTable: objectTable, CourierWorksheetmode: true });
                _SendDeclarationService.OnSuccessSendMethod =
                    (res1) => {
                        SessionLocator.CurrentSession.StopBusyIndicator();
                        //this._CourierWorksheetSharedDataService.SendNextMessage("DoRefresh");
                        this.RefreshData()
                    };
                _SendDeclarationService.OnCustomSendOptionsButtonClick({
                    RequestVIA: SendRequestVIA.WebServiceInteractive,
                    Option: "WI",
                    ForcePersonalSign: false
                });

            });

    }

    ButtonClick(event) {
        this._CourierWorksheetSharedDataService.SupperssOnRowSelectedAction = true;


        //event.stopPropagation();
        //this.RowSelect()
        this.DropdownDisplayClose();//this.MySplitButtonComponent.DropdownDisplayClose();//SplitButtonComponent.EnsureLastSplitButtonIsClosed();
        //DropdownMenuFilterComponent.EnsureLastDropdownMenuIsClosed();
    }


    get WebAPICourierGWMessageECTHRDataMaman() { return this._CourierWorksheetSharedDataService.WebAPICourierGWMessageECTHRDataMaman }
    IsWebAPICourierGWMessageECTHRDataMamanEnable: boolean = false;

    GetSendECTHRDataMaman(event) {
        this.ButtonClick(event);
        SessionLocator.CurrentSession.StartBusyIndicatorCreating();
        this._CourierMasterService.GetSendECTHRDataMaman(this._CourierWorksheet['DeclarationId'])
            .subscribe(res => {
                SessionLocator.CurrentSession.StopBusyIndicator();
                var myMessageWindow = new MessageWindow();
                let mess = "";
                if (res.HasError) {
                    mess = res.ErrorsArray[0];
                } else {
                    mess = res.Result;
                }
                myMessageWindow.Show(mess);
            });
    }
    SpecialActionStatusXClicked(event) {
        this.ButtonClick(event);
        SessionLocator.CurrentSession.StartBusyIndicator("");
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 1000;
        filters.addAdditionalFilter("DeclarationId", this._CourierWorksheet['DeclarationId'], null, null, "Equals", false, false, false, "string");
        //filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");
        let myEntityResourceService: EntityResourceService = new EntityResourceService();
        myEntityResourceService.getEntityResourceByTableName("Customs.DeclarationMamanSpecialAction")
            .subscribe(response => {
                this._DeclarationMamanSpecialActionListService.getByFilters(filters)
                    .subscribe((response: ServiceResponse) => {
                        SessionLocator.CurrentSession.StopBusyIndicator();
                        if (!response.HasError && response.Result != null) {
                            let list: DeclarationMamanSpecialActionList[] = response.Result;
                            list.forEach((declarationMamanSpecialActionPMItem: any) => {
                                switch (declarationMamanSpecialActionPMItem.MamanSpecialActionCode) {
                                    case "2": {
                                        declarationMamanSpecialActionPMItem.MamanSpecialActionCode = "ת. עיכוב";//this.IsReceivingDelayCertificate = true;

                                        break;
                                    }
                                    case "4": {

                                        declarationMamanSpecialActionPMItem.MamanSpecialActionCode = "מדבקות";//this.IsMamanSticker = true;

                                        break;
                                    }
                                    case "5": {
                                        declarationMamanSpecialActionPMItem.MamanSpecialActionCode = "מסמכים";//this.IsPrintDocuments = true;
                                    }
                                        break;
                                }
                            });

                            
            
                            var logWindow = new LogitudeWindow();
                            logWindow.Width = 1000;
                            logWindow.Height = 350;
                            logWindow.Title = "פעולות מיוחדות מול מסוף";
                            logWindow.WindowArgs = {
                                MamanSpecialActionList: list
                            };
                            logWindow.ShowCloseButton = true;
                            logWindow.Show('./CustomsModules/CustomsCourier/Components/MamanSpecialAction/DeclarationMamanSpecialActionComponent');
                            logWindow.WindowClosed.subscribe(($event: any) => {
                                //this._CourierWorksheetSharedDataService.SendNextMessage("DoRefresh");
                            });

                        }
                    });
            });
    }
    SendPay(event) {
        this.ButtonClick(event);

        if (this._CourierWorksheet.CourierPendingReasonErrorPlace == "1" /*=="בתשלום"*/) {
            var myMessageWindow = new MessageWindow
            myMessageWindow.Show(
                //"לא ניתן לבצע הגשת תשלום כאשר יש השהייה מסוג עצירת תשלום. "
                TextCodeTranslator.Translate("Customs.CourierMaster.M.PaymentPendingHold")
            );
            return;
        }

        let BackButtonLabel = "תיק עמילות"
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                //this.SelectionChanged(myDeclarationEditTab);
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityId: this._CourierWorksheet['DeclarationId'],//"1-103991"
                    ObjectTableName: 'Customs.Declaration',//'Customs.Declaration'
                    BackButtonLabel: BackButtonLabel
                });


                let myEditComponent: EditComponent = cmpRef.instance;



                //let myDeclarationPMService: DeclarationPMService = new DeclarationPMService()
                //myDeclarationPMService.get(this._CourierWorksheet['DeclarationId'])
                //  .subscribe(rsptPMget => {
                let sub = myEditComponent.OnFirstTimeAfterSingleDataLoaded.subscribe(
                    (token1) => {
                        sub.unsubscribe();
                        let entitypm = myEditComponent.EntityPM; //rsptPMget.Result;
                        let objectTable = window.ObjectTables.filter(d => d.Name === 'Customs.Declaration')[0];

                        var args: any = {
                            EntityPM: entitypm,
                        };

                        var logWindow = new LogitudeWindow();
                        logWindow.Width = 1000;
                        logWindow.Height = 700;
                        logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.TH.Payments");
                        logWindow.WindowArgs = args;
                        logWindow.ShowCloseButton = true;

                        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/DeclarationPayment/DeclarationPaymentComponent');
                        logWindow.WindowClosed.subscribe(($event: any) => {
                            //this._CourierWorksheetSharedDataService.SendNextMessage("DoRefresh");
                            myEditComponent.BackButtonClicked();
                            this.RefreshData()
                        });
                        //TODO | !TODO   ???? >>>>this.ActivateUnifreightInstruction();
                    });

            }
            );
    }

    _IsDropdownMenuFilterReady: boolean = false;
    PrepareDropdownMenuFilter(event, declarationId) {
        this._IsDropdownMenuFilterReady = false;
        this.IsWebAPICourierGWMessageECTHRDataMamanEnable = false;
        this.IsReceivingDelayCertificate = false;
        this.IsPrintDocuments = false;
        this.DelayCertificateDetails = null;
        this.MamanStickerDetails = null;
        this.PrintDocumentsDetails = null;

        let myDeclarationPMService: DeclarationPMService = new DeclarationPMService()
        myDeclarationPMService.get(this._CourierWorksheet['DeclarationId']).subscribe(rsptPMget => {
            let entitypm: DeclarationPM = rsptPMget.Result;
            if (entitypm != null && entitypm.Consignments != null) {
                if (this.WebAPICourierGWMessageECTHRDataMaman.includes(entitypm.Consignments[0].StorageSiteCode)) {
                    this.IsWebAPICourierGWMessageECTHRDataMamanEnable = true;

                    var filters = new ApiQueryFilters();
                    filters.PageIndex = 0;
                    filters.PageSize = 1000;
                    filters.addAdditionalFilter("DeclarationId", declarationId, null, null, "Equals", false, false, false, "string");

                    this._DeclarationMamanSpecialActionListService.getByFilters(filters).subscribe((response: ServiceResponse) => {
                        if (!response.HasError && response.Result != null) {
                            response.Result.forEach((declarationMamanSpecialActionPMItem: DeclarationMamanSpecialActionPM) => {
                                switch (declarationMamanSpecialActionPMItem.MamanSpecialActionCode) {
                                    case "2": {
                                        this.DelayCertificateDetails = declarationMamanSpecialActionPMItem;
                                        if (declarationMamanSpecialActionPMItem.MamanSpecialActionStatusCode == "1") {
                                            this.IsReceivingDelayCertificate = true;
                                        }
                                        break;
                                    }
                                    case "4": {
                                        this.MamanStickerDetails = declarationMamanSpecialActionPMItem;
                                        if (declarationMamanSpecialActionPMItem.MamanSpecialActionStatusCode == "1") {
                                            this.IsMamanSticker = true;
                                        }
                                        break;
                                    }
                                    case "5": {
                                        this.PrintDocumentsDetails = declarationMamanSpecialActionPMItem;
                                        if (declarationMamanSpecialActionPMItem.MamanSpecialActionStatusCode == "1") {
                                            this.IsPrintDocuments = true;
                                        }
                                        break;
                                    }
                                }
                            });

                        }
                        this._IsDropdownMenuFilterReady = true;
                        this.CD.detectChanges();
                    });
                }

                this._IsDropdownMenuFilterReady = true;
                this.CD.detectChanges();
            }
            else {
                this._IsDropdownMenuFilterReady = true;
                this.CD.detectChanges();
            }
        });
        this.CD.detectChanges();
    }


    CourierPendingReasonCommand(event, declarationId, mode) {
      this.ButtonClick(event);

        var logitudeWindow = new LogitudeWindow();
        var windowArgs: any = {};
        var declarationIdList = [];

        this._DeclarationCourierStatusPMService.get(declarationId).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                declarationIdList.push(response.Result);
                windowArgs.DeclarationIdList = declarationIdList;
                windowArgs.CourierHawb = this._CourierWorksheet.CourierHawb;
                windowArgs.Mode = mode;
                
                if (mode == "Delete") {
                    var confirm = new ConfirmWindow();
                    confirm.Width = 350;
                    confirm.Height = 200;
                    confirm.Title = "מחיקת Pending";
                    confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");
                    confirm.ShowNoButton = true;
                    confirm.Show("האם למחוק Pending?");
                    confirm.WindowClosed.subscribe((event: any) => {
                        if (confirm.Yes) {
                            this.DeletePending(response.Result);
                        }
                        confirm.Close();
                    });
                }
                else {
                    if (mode == "Update") {
                        windowArgs.CourierPendingReasonCode = this._CourierWorksheet.CourierPendingReasonCode;
                        windowArgs.PendingRemarks = this._CourierWorksheet.PendingRemarks;
                    }
                    logitudeWindow.Width = 450;
                    logitudeWindow.Height = 280;
                    logitudeWindow.IsShowCloseButton = false;
                    logitudeWindow.Title = "סימון ב Pending";//TextCodeTranslator.Translate("CommunicationLog.O.MoreDetails");;
                    logitudeWindow.WindowArgs = windowArgs;
                    logitudeWindow.Show('./CustomsModules/CustomsCourier/Components/CourierPendingReason/CourierPendingReasonGeneralComponent');
                    logitudeWindow.WindowClosed.subscribe(($event: any) => {
                      this.RefreshData();
                    });
                }
            }
        });

        this.CD.detectChanges();
  }

    DeletePending(declarationCourierStatusPM: DeclarationCourierStatusPM) {
        SessionLocator.CurrentSession.StartBusyIndicatorSaving();
        declarationCourierStatusPM.CourierPendingReasonCode = null;
        declarationCourierStatusPM.PendingRemarks = null;
        this._DeclarationCourierStatusPMService.update(declarationCourierStatusPM).subscribe((response: ServiceResponse) => {
            SessionLocator.CurrentSession.StopBusyIndicator();
            this.RefreshData();
        });
    }

    OnCheckedWithSystemEvent(eventM) {
        eventM.stopPropagation();
        this.IsDeclarationChecked = !this.IsDeclarationChecked;
        //if (event.IsChecked) {
        if (this.IsDeclarationChecked) {
            if (!this._CourierWorksheetSharedDataService._SelectedItems.Collection.includes(this._CourierWorksheet.DeclarationId)) {
                this._CourierWorksheetSharedDataService._SelectedItems.Insert(this._CourierWorksheet.DeclarationId);
            }
        }
        else {
            var removedIndex = null;
            for (var i = 0; i < this._CourierWorksheetSharedDataService._SelectedItems.Collection.length; i++) {
                if (this._CourierWorksheet.DeclarationId == this._CourierWorksheetSharedDataService._SelectedItems.Collection[i]) {
                    removedIndex = i;
                    break;
                }
            }
            if (removedIndex != null) {
                this._CourierWorksheetSharedDataService._SelectedItems.RemoveFromIndex(removedIndex);
            }
        }
    }

    MamanStickerCommand(event, declarationId, mode) {
        this.ButtonClick(event);

        var logitudeWindow = new LogitudeWindow();
        var windowArgs: any = {};
        windowArgs.DeclarationId = declarationId;
        windowArgs.EntityPM = this.MamanStickerDetails;
        windowArgs.Mode = mode;

        logitudeWindow.Width = 450;
        logitudeWindow.Height = 280;
        logitudeWindow.IsShowCloseButton = false;
        logitudeWindow.Title = "פרטי מדבקה";//TextCodeTranslator.Translate("Customs.CourierMaster.O.StickerDetails");;
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./CustomsModules/CustomsCourier/Components/MamanSpecialAction/AddEditMamanStickerComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            this.RefreshData();
        });

        this.CD.detectChanges();
    }

    SendMamanSpecialAction(declarationId: string, actionCode: string, mamanSpecialActionCode: string) {
        var titleText: string = "מסר פעולות מיוחדות";
        var questionText: string = "אשר שליחת מסר ביטול פעולה מיוחדת";
        var declarationMamanSpecialActionPM: DeclarationMamanSpecialActionPM = null;

        switch (mamanSpecialActionCode) {
            case "2": {
                if (actionCode == "U") {
                    titleText = "הפקת תעודת עיכוב";
                    questionText = "אשר שליחת מסר פעולה מיוחדת של תעודת עיכוב למסוף";
                    if (this.DelayCertificateDetails != null) {
                        declarationMamanSpecialActionPM = this.DelayCertificateDetails;
                    }
                }
                else if (actionCode == "C") {
                    titleText = "ביטול תעודת עיכוב";
                    questionText = "אשר שליחת מסר ביטול פעולה מיוחדת של תעודת עיכוב למסוף";
                }
                break;
            }
            case "4": {
                titleText = "ביטול הפקת מדבקה";
                questionText = "אשר שליחת מסר ביטול פעולה מיוחדת של הדפסת מדבקה";
                break;
            }
            case "5": {
                if (actionCode == "U") {
                    titleText = "הדפסת מסמכים";
                    questionText = "אשר שליחת מסר פעולה מיוחדת של הדפסת מסמכים";
                    if (this.PrintDocumentsDetails != null) {
                        declarationMamanSpecialActionPM = this.PrintDocumentsDetails;
                    }
                }
                else if (actionCode == "C") {
                    titleText = "ביטול הדפסת מסמכים";
                    questionText = "אשר שליחת מסר ביטול פעולה מיוחדת של הדפסת מסמכים";
                }
                break;
            }
        }

        var confirm = new ConfirmWindow();
        confirm.Width = 350;
        confirm.Height = 200;
        confirm.Title = titleText;
        confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");
        confirm.ShowNoButton = true;
        confirm.Show(questionText);
        confirm.WindowClosed.subscribe((event: any) => {
            if (confirm.Yes) {
                this._IsDropdownMenuFilterReady = false;
                SessionLocator.CurrentSession.StartBusyIndicatorCreating();
                if (actionCode == "U") {
                    if (declarationMamanSpecialActionPM == null) {
                        declarationMamanSpecialActionPM = new DeclarationMamanSpecialActionPM();
                        declarationMamanSpecialActionPM.Tenant = SessionLocator.Tenant;
                        declarationMamanSpecialActionPM.DeclarationId = declarationId;
                        declarationMamanSpecialActionPM.MamanSpecialActionCode = mamanSpecialActionCode;

                        this._DeclarationMamanSpecialActionPMService.insert(declarationMamanSpecialActionPM).subscribe(res => {
                            this._DeclarationWebService.GetDeclarationMamanSpecialAction(declarationId, this._CourierWorksheet.Tenant, "U", mamanSpecialActionCode)
                                .subscribe((myResponse: ServiceResponse) => {
                                    SessionLocator.CurrentSession.StopBusyIndicator();
                                    var myMessageWindow = new MessageWindow();
                                    myMessageWindow.Show(myResponse.Result);
                                });
                        });
                    }
                    else {
                        declarationMamanSpecialActionPM.MamanSpecialActionStatusCode = null;
                        declarationMamanSpecialActionPM.MamanSpecialActionsErrorXml = null;
                        this._DeclarationMamanSpecialActionPMService.update(declarationMamanSpecialActionPM).subscribe(res => {
                            this._DeclarationWebService.GetDeclarationMamanSpecialAction(declarationId, this._CourierWorksheet.Tenant, "U", mamanSpecialActionCode)
                                .subscribe((myResponse: ServiceResponse) => {
                                    SessionLocator.CurrentSession.StopBusyIndicator();
                                    var myMessageWindow = new MessageWindow();
                                    myMessageWindow.Show(myResponse.Result);
                                });
                        });
                    }
                }
                else {
                    this._DeclarationWebService.GetDeclarationMamanSpecialAction(declarationId, this._CourierWorksheet.Tenant, "C", mamanSpecialActionCode)
                        .subscribe((myResponse: ServiceResponse) => {
                            SessionLocator.CurrentSession.StopBusyIndicator();
                            var myMessageWindow = new MessageWindow();
                            myMessageWindow.Show(myResponse.Result);
                        });
                }
            }
            confirm.Close();
        });

    }

    SetManualProcesscode(declarationId: string, manualProcessCode: string) {

        SessionLocator.CurrentSession.StartBusyIndicatorSaving();
        this._DeclarationCourierStatusWebService.GetSetManualProcesscode(declarationId, manualProcessCode).subscribe((response: ServiceResponse) => {
                SessionLocator.CurrentSession.StopBusyIndicator();
                this.RefreshData();
        });
    }
}
