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
    providers: [],
})

export class PhysicalCheckListActionBarComponent
    extends BaseComponent
    implements AfterViewInit {

    private _PhysicalCheckWebService: PhysicalCheckWebService = new PhysicalCheckWebService;
 
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext: PhysicalCheckListActionBarComponent = this;
    public ObjectTableName: string = "Customs.PhysicalCheck";

    constructor(private _CD: ChangeDetectorRef, public _physicalChecksCloseSharedDataService: PhysicalChecksCloseSharedDataService) {
        super();

    }



    ngAfterViewInit() {
        SessionLocator.SelectedSession.CurrentListComponent.onRefershQueryEvent.subscribe(data => {
            this._physicalChecksCloseSharedDataService._SelectedItems.Clear();
            this._physicalChecksCloseSharedDataService.IsDisplayButtonClose = false;

        });

    }

    
    get count() {
        return this._physicalChecksCloseSharedDataService._SelectedItems.Collection.length.toString()

    }

    

  
    CloseMarkChecks() {
        var checkList = this._physicalChecksCloseSharedDataService._SelectedItems.Collection.join(',');
        
        this._PhysicalCheckWebService.PostCloseMarkPhysicalChecks(checkList, SessionLocator.Tenant)
            .subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    let messageWindow = new MessageWindow();
                    messageWindow.Width = 300;
                    messageWindow.Height = 180;
                    messageWindow.Show("הבדיקות נסגרו בהצלחה");
                    this.CurrentSession.CurrentListComponent.RefreshBtnClick();
                }
                else {
                    let messageWindow = new MessageWindow();
                    messageWindow.Width = 300;
                    messageWindow.ShowErrorIcon = true;
                    messageWindow.Height = 180;
                    messageWindow.Show("נכשל");
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
