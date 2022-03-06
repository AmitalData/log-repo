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

@Component({
    templateUrl: './DeclarationReferantDataListActionBarComponent.html',
})

export class DeclarationReferantDataListActionBarComponent
    extends BaseComponent
    implements AfterViewInit {

    public itmImportDeclarationReferantDatas: boolean = false;
    
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext: DeclarationReferantDataListActionBarComponent = this;
    public ObjectTableName: string = "Customs.DeclarationReferantData";

    constructor(private _CD: ChangeDetectorRef) {
        super();

    }



    ngAfterViewInit() {
    }

    


    

  
    ShowQueueManagmentAQ1() {
        
        let myViewModelName = "DeclarationReferantDataListActionBarComponent.ts-ShowQueueManagmentAQ1";
        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
            let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
                .subscribe(
                    (mess: UnifreightMessageM) => {
                        var IsMatchUnifreightCallbackCommand = (
                            mess.LogitudeEntity == AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
                            mess.LogitudeEntityNumber == "" &&
                            mess.LogitudeViewModel == myViewModelName);
                        if (IsMatchUnifreightCallbackCommand) {
                            sub.unsubscribe();
                            SessionLocator.SelectedSession.StopBusyIndicator();
                            let sBool = UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
                            SessionLocator.SelectedSession.CurrentListComponent.DoRefresh();
                        }
                    }
                );

            SessionLocator.SelectedSession.StartBusyIndicator("");
            var unifreightMessageM =
                AmitalGatewayUtil.Instance.
                    DeclarationMessaging.GetMessage("", "",
                        myViewModelName
                        , AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity("I"));

            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
                "ScriptableGatewayUtil.ShowQueueManagmentAQ1",
                "CFIHMAIN.LogitudeTask",
                "ShowQueueManagmentAQ1",
                unifreightMessageM,
                " הצגת מסך : ניהול תורים");
        }
        else {
            alert("ShowQueueManagmentAQ1");
        }
    }

    ShowOCRQuery() {

        let myViewModelName = "DeclarationReferantDataListActionBarComponent.ts-ShowOCRQuery";
        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
            let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
                .subscribe(
                    (mess: UnifreightMessageM) => {
                        var IsMatchUnifreightCallbackCommand = (
                            mess.LogitudeEntity == AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
                            mess.LogitudeEntityNumber == "" &&
                            mess.LogitudeViewModel == myViewModelName);
                        if (IsMatchUnifreightCallbackCommand) {
                            sub.unsubscribe();
                            SessionLocator.SelectedSession.StopBusyIndicator();
                            let sBool = UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
                            SessionLocator.SelectedSession.CurrentListComponent.DoRefresh();
                        }
                    }
                );

            SessionLocator.SelectedSession.StartBusyIndicator("");
            var unifreightMessageM =
                AmitalGatewayUtil.Instance.
                    DeclarationMessaging.GetMessage("", "",
                        myViewModelName
                        , AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity("I"));

            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
                "ScriptableGatewayUtil.ShowOCRQuery",
                "CFIHMAIN.LogitudeTask",
                "ShowOCRQuery",
                unifreightMessageM,
                " הצגת מסך : שאילתא ל - OCR");
        }
        else {
            alert("ShowOCRQuery");
        }
    }
}
