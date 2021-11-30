import { Component, AfterViewInit, Output, EventEmitter, ContentChild, ViewChild, ViewChildren, QueryList, ChangeDetectorRef } from '@angular/core';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { MultiSelectLOVComponent } from '../../../Infrastructure/Components/LogitudeComponents/MultiSelectLOVComponent';
import { UserListService } from '../../../Common/Services/StandardLists/UserListService';
import { UserList } from '../../../Common/EntityLists/UserList';
import { AppTool } from '../../../Infrastructure/Tools';
import { AmitalGatewayUtil, UnifreightMessageM } from '../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { PhysicalChecksCloseSharedDataService } from '../../Services/DataChange/PhysicalChecksCloseSharedDataService';
import { PhysicalCheckWebService } from '../../Services/WebServices/PhysicalCheckWebService';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    templateUrl: './PhysicalCheckListActionBarComponent.html',
})

export class PhysicalCheckListActionBarComponent
    extends BaseComponent
    implements AfterViewInit {

    public itmImportDeclarationReferantDatas: boolean = false;
    private _PhysicalCheckWebService: PhysicalCheckWebService = new PhysicalCheckWebService;

    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext: PhysicalCheckListActionBarComponent = this;
    public ObjectTableName: string = "Customs.PhysicalCheck";

    constructor(private _CD: ChangeDetectorRef, public _physicalChecksCloseSharedDataService: PhysicalChecksCloseSharedDataService) {
        super();

    }



    ngAfterViewInit() {
        this._physicalChecksCloseSharedDataService.IsDisplayButtonClose = (this._physicalChecksCloseSharedDataService._SelectedItems.Collection.length > 0);
    }

    


    

  
    CloseMarkChecks(eventM) {
        var checkList = this._physicalChecksCloseSharedDataService._SelectedItems.Collection.join(',');
        this._PhysicalCheckWebService.PostCloseMarkPhysicalChecks(checkList, this.EntityPM.Tenant)
            .subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    let messageWindow = new MessageWindow();
                    messageWindow.Width = 300;
                    messageWindow.Height = 180;
                    messageWindow.Show("הבדיקה נסגרה בהצלחה");//TextCodeTranslator.Translate("Customs.PhysicalCheck.O.ClosePhysicalCheck"));
                }
            });
      
    }

    ShowOCRQuery() {

        //let myViewModelName = "DeclarationReferantDataListActionBarComponent.ts-ShowOCRQuery";
        //if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
        //    SessionLocator.SelectedSession.StartBusyIndicatorLoading();
        //    let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
        //        .subscribe(
        //            (mess: UnifreightMessageM) => {
        //                var IsMatchUnifreightCallbackCommand = (
        //                    mess.LogitudeEntity == AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
        //                    mess.LogitudeEntityNumber == "" &&
        //                    mess.LogitudeViewModel == myViewModelName);
        //                if (IsMatchUnifreightCallbackCommand) {
        //                    sub.unsubscribe();
        //                    SessionLocator.SelectedSession.StopBusyIndicator();
        //                    let sBool = UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
        //                    SessionLocator.SelectedSession.CurrentListComponent.DoRefresh();
        //                }
        //            }
        //        );

        //    SessionLocator.SelectedSession.StartBusyIndicator("");
        //    var unifreightMessageM =
        //        AmitalGatewayUtil.Instance.
        //            DeclarationMessaging.GetMessage("", "",
        //                myViewModelName);

        //    AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
        //        "ScriptableGatewayUtil.ShowOCRQuery",
        //        "CFIHMAIN.LogitudeTask",
        //        "ShowOCRQuery",
        //        unifreightMessageM,
        //        " הצגת מסך : שאילתא ל - OCR");
        //}
        //else {
        //    alert("ShowOCRQuery");
        //}
    }
}
