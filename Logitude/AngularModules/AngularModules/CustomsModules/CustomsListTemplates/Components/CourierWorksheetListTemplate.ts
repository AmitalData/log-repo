

declare var window: any;

import {WebFreightDomainService} from '../../../Infrastructure/Services/WebFreightDomainService';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import { Component, ChangeDetectorRef, ViewChild ,OnInit, Output, EventEmitter, ComponentRef, QueryList} from '@angular/core';
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
import {GenericRequestParams} from '../../../Customs/DataContract/RequestParams/GenericRequestParams';
import {SendRequestVIA} from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import {ShowProgressBarParams, CustomMessageProgressComponent} from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import {DeclarationWebService} from '../../../Customs/Services/WebServices/DeclarationWebService';
import {SplitButtonComponent} from '../../../Controls/All/SplitButtonComponent';
import {MANIFESTRequestRequestParams} from '../../../Customs/DataContract/RequestParams/MANIFESTRequestRequestParams'; 
import { CourierWorksheetSharedDataService } from '../../../Customs/Services/DataChange/CourierWorksheetSharedDataService';
import { SendDeclarationService } from '../../../CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/SendDeclaration/SendDeclarationComponent';
import { SendManifestService } from '../../../CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/SendDeclaration/SendManifestComponent';
import {DeclarationPMService} from '../../../Customs/Services/StandardPMs/DeclarationPMService';
import {DropdownMenuFilterComponent} from '../../../CustomsModules/CustomsCourier/Components/CourierWorkSheet/DropdownMenuFilterComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import {DeclarationCourierStatusPMService} from '../../../Customs/Services/StandardPMs/DeclarationCourierStatusPMService';
import {DeclarationCourierStatusPM} from '../../../Customs/EntityPMs/DeclarationCourierStatusPM';
import { DeclarationCourierStatusList } from '../../../Customs/EntityLists/DeclarationCourierStatusList';
import { DeclarationCourierStatusListService } from '../../../Customs/Services/StandardLists/DeclarationCourierStatusListService';
import { retry } from 'rxjs/operator/retry';

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
  IsPaymentStatusRed: boolean = false;
  IsPaymentStatusGreen: boolean = false;
  IsPaymentStatusBlue: boolean = false;
  IsPaymentStatusOrange: boolean = false;
  IsHighLow: boolean = false;
  IsDeclarationChecked: boolean = false;
  SuspentionReasonText: string;
  SuspentionReasonTip: string;

  _DeclarationCourierStatusPMService: DeclarationCourierStatusPMService = new DeclarationCourierStatusPMService();
    _CourierMasterService: CourierMasterService = new CourierMasterService();

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
          this.IsPaymentStatusRed = true;
          break;
        }
        case "P": {
          this.IsPaymentStatusGreen = true;
          break;
        }
        case "I": {
          this.IsPaymentStatusBlue = true;
          break;
        }
        case "O": {
          this.IsPaymentStatusOrange = true;
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
      this.SuspentionReasonTip = this._CourierWorksheet.CourierSuspentionReasonName;
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


    get IsWebAPICourierGWMessageECTHRDataMamanEnable() { return this._CourierWorksheetSharedDataService.IsWebAPICourierGWMessageECTHRDataMamanEnable }

    GetSendECTHRDataMaman(event) {
        this.ButtonClick(event);
        SessionLocator.CurrentSession.StartBusyIndicatorCreating();
        this._CourierMasterService.GetSendECTHRDataMaman(this._CourierWorksheet['DeclarationId'])
            .subscribe(res => {
                SessionLocator.CurrentSession.StopBusyIndicator();
                var myMessageWindow = new MessageWindow();
                myMessageWindow.Show(res.Result);
            });
    }
  SendPay(event) {
    this.ButtonClick(event);

    let myDeclarationPMService: DeclarationPMService = new DeclarationPMService()
    myDeclarationPMService.get(this._CourierWorksheet['DeclarationId'])
      .subscribe(rsptPMget => {
        let entitypm = rsptPMget.Result;
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
          this.RefreshData()
        });
        //TODO | !TODO   ???? >>>>this.ActivateUnifreightInstruction();
      });

  }

    CourierPendingReasonCommand(event, declarationId, mode) {
        //event.stopPropagation();
        //SplitButtonComponent.EnsureLastSplitButtonIsClosed();
        //DropdownMenuFilterComponent.EnsureLastDropdownMenuIsClosed();
      this.ButtonClick(event);

        var logitudeWindow = new LogitudeWindow();
        var windowArgs: any = {};
        var declarationIdList = [];

        this._DeclarationCourierStatusPMService.get(declarationId).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                declarationIdList.push(response.Result);
                windowArgs.DeclarationIdList = declarationIdList;
                windowArgs.CourierHawb = this._CourierWorksheet.CourierHawb;
                
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
                    logitudeWindow.Width = 450;
                    logitudeWindow.Height = 280;
                    logitudeWindow.IsShowCloseButton = false;
                    logitudeWindow.Title = "סימון ב Pending";//TextCodeTranslator.Translate("CommunicationLog.O.MoreDetails");;
                    logitudeWindow.WindowArgs = windowArgs;
                    logitudeWindow.Show('./CustomsModules/CustomsCourier/Components/CourierPendingReason/CourierPendingReasonGeneralComponent');
                    logitudeWindow.WindowClosed.subscribe(($event: any) => {
                        //this._CourierWorksheetSharedDataService.SendNextMessage("DoRefresh");
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
            //this._CourierWorksheetSharedDataService.SendNextMessage("DoRefresh");
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

}
