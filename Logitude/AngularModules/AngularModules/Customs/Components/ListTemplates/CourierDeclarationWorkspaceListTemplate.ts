
declare var window: any;
import { EditComponent } from "../../../Infrastructure/Components/EditComponent/EditComponent";
import { WebFreightDomainService } from '../../../Infrastructure/Services/WebFreightDomainService';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { Component, ChangeDetectorRef } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ARPaymentExtendedListService } from '../../../Invoice/Services/ExtendedLists/ARPaymentExtendedListService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { CustomsRequestMenuService } from '../../../Customs/Services/Others/CustomsRequestMenuService';
import { CustomsRequestSheetExtendedPMService } from '../../../Customs/Services/ExtendedPMs/CustomsRequestSheetExtendedPMService';
import { SendALLCorrectRequestParams } from '../../../Customs/DataContract/RequestParams/SendALLCorrectRequestParams';
import { ResponseDataBase } from '../../../Customs/DataContract/ResponseData/ResponseDataBase';
import { CustomsRequestsSheetPM } from '../../../Customs/EntityPMs/CustomsRequestsSheetPM';
import { CourierMasterService } from '../../../Customs/Services/Others/CourierMasterService';
import { SendRequestVIA } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { ShowProgressBarParams, CustomMessageProgressComponent } from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { DeclarationWebService } from '../../../Customs/Services/WebServices/DeclarationWebService';

@Component({
    moduleId: module.id,
    templateUrl: './CourierDeclarationWorkspaceListTemplate.html',
})

export class CourierDeclarationWorkspaceListTemplate {

    _CourierWorksheet: DeclarationCourierStatusList;
    public fieldName: any;

    
    //private _DeclarationCourierStatusPMService: DeclarationCourierStatusPMService = new DeclarationCourierStatusPMService();
    ////private declarationPendingPMService: DeclarationPendingPMService = new DeclarationPendingPMService();
    //private _CourierMasterService: CourierMasterService = new CourierMasterService();
    //private _DeclarationMamanSpecialActionListService: DeclarationMamanSpecialActionListService = new DeclarationMamanSpecialActionListService();
    //private _DeclarationMamanSpecialActionPMService: DeclarationMamanSpecialActionPMService = new DeclarationMamanSpecialActionPMService;
    //private _DeclarationWebService: DeclarationWebService = new DeclarationWebService;
    //private _DeclarationCourierStatusWebService: DeclarationCourierStatusWebService = new DeclarationCourierStatusWebService();
    //_DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    

    FirePreventSelect() {
        SessionLocator.SelectedSession.PseventRowSelectEvent.emit("CourierWorksheetListTemplate.SendSplitButton");
    }
    FireUnSelect() {
        SessionLocator.SelectedSession.PseventRowSelectEvent.emit("FireUnSelect");
    }

    //  @ViewChild( SplitButtonComponent)  public MySplitButtonComponent: SplitButtonComponent = new SplitButtonComponent(null,null);
    //@ViewChild('ShortTitle', { read: ViewContainerRef }) ShortTitleViewContainerRef: ViewContainerRef;
    //@ViewChild('MySplitButtonComponent', { read: SplitButtonComponent }) MySplitButtonComponent: SplitButtonComponent;

    constructor(private CD: ChangeDetectorRef) {
        
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
        //DropdownMenuFilterComponent.EnsureLastDropdownMenuIsClosed();


        this.SuspentionReasonText = this._CourierWorksheet.CourierCustomStatusName;
        this.CD.detectChanges();
    }

    _IsSplitButtonMenuFilterReady: boolean = false;
    PrepareSplitButtonMenuFilter() {
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
                        SessionLocator.SelectedSession.StopBusyIndicator();
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

    get IsDisplayOnly() { return this._CourierWorksheetSharedDataService.IsDisplayOnly }
    get WebAPICourierGWMessageECTHRDataMaman() { return this._CourierWorksheetSharedDataService.WebAPICourierGWMessageECTHRDataMaman }
    //get CourierPendingReasonListToolTip() { return this.CourierPendingReasonListToolTip ; }
    get CourierPendingReasonListToolTip() {

        if (AppTool.IsNullOrEmpty(this._CourierWorksheet.CourierPendingReasonList)) {
            return "";
        }
        if (this._CourierWorksheet.CourierPendingReasonList.indexOf(',') < 0) {
            return this._CourierWorksheet.CourierPendingReasonName;
        }

        let myToolTip = "";
        var mycache: Array<CourierPendingReasonList> = CacheCourierPendingReasonService.Instance.GetCache();
        let listString: string =this._CourierWorksheet.CourierPendingReasonList;
        let arry = listString.split(',');
        arry.forEach(itemReason => {
            let rec = mycache.filter(r => r.Code == itemReason)[0];
            if (rec != null) {
                if (!AppTool.IsNullOrEmpty(myToolTip)) {
                    myToolTip += '\n'
                }
                if (!AppTool.IsNullOrEmpty(rec.LocalName)) {
                    myToolTip += rec.LocalName;
                } else if (!AppTool.IsNullOrEmpty(rec.EnglishName)) {
                    myToolTip += rec.EnglishName;
                } else {
                    myToolTip += rec.Code;
                }
            }
        });
        
        return myToolTip; 

        ///return this.getCourierPendingReasonName(this._CourierWorksheet.CourierPendingReasonList);
    }
    get CourierPendingReasonListText() {
        if (AppTool.IsNullOrEmpty(this._CourierWorksheet.CourierPendingReasonList)) {
            return "";
        }
        if (this._CourierWorksheet.CourierPendingReasonList.indexOf(',') < 0) {
            return this._CourierWorksheet.CourierPendingReasonName;
        }
        return "הצג רשימה";

    }

    IsWebAPICourierGWMessageECTHRDataMamanEnable: boolean = false;

    _IsDropdownMenuFilterReady: boolean = false;
    PrepareDropdownMenuFilter(event, declarationId) {
        this._IsDropdownMenuFilterReady = false;
        this.IsWebAPICourierGWMessageECTHRDataMamanEnable = false;
        this.IsReceivingDelayCertificate = false;
        this.IsPrintDocuments = false;
        this.IsSban = false;
        this.DelayCertificateDetails = null;
        this.MamanStickerDetails = null;
        this.PrintDocumentsDetails = null;
        this.SbanDetails = null;
        this.IsMamanEnabled = false;       

        let myDeclarationPMService: DeclarationPMService = new DeclarationPMService()
        myDeclarationPMService.get(this._CourierWorksheet['DeclarationId']).subscribe(rsptPMget => {
            let entitypm: DeclarationPM = rsptPMget.Result;
            if (entitypm != null && entitypm.Consignments != null) {
                if (this.WebAPICourierGWMessageECTHRDataMaman.includes("ILMMN") && entitypm.Consignments[0].StorageSiteCode == "ILMMN") {
                    this.IsMamanEnabled = true;
                }
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
                                    case "6": {
                                        this.SbanDetails = declarationMamanSpecialActionPMItem;
                                        if (declarationMamanSpecialActionPMItem.MamanSpecialActionStatusCode == "1") {
                                            this.IsSban = true;
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
}
