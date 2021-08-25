import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationRestoreArgs } from '../../../Customs/Args';
import { DeclarationPM } from '../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { DeclarationList } from '../../../Customs/EntityLists/DeclarationList';
import { ClientMessagesService } from '../../../Customs/Services/WebServices/ClientMessagesService';
import { ClientSearchRequestParams } from '../../../Customs/DataContract/RequestParams/ClientSearchRequestParams';
import { RTGSInfoQueryResponseData } from '../../../Customs/DataContract/ResponseData/RTGSInfoQueryResponseData';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomSendOptionsArgs } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent, CustomMessageProgressHelper } from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {ImageParameter} from '../../../Infrastructure/DataContracts/ImageParameter';


import {Guid} from '../../../Infrastructure/Utilities/Guid';
declare var attachmentUploader, ResultAsArray: any;

@Component({
    selector: 'RecallClientsForCutoms',
    
    templateUrl: './RecallClientsForCutoms.html',
})

export class RecallClientsForCutoms
    extends BaseRequestsSheetMassaging
    implements AfterViewInit, IRequestsSheetMassagingComponent {

    public DataContext: RecallClientsForCutoms = this;
    public ObjectTableName: string = "Customs.Client";
    UploadButtonIsEnabled: boolean = true;

    _ClientMessagesService: ClientMessagesService = new ClientMessagesService();

    filterImageParameter: ImageParameter;
    ProgressBarPercentText: string;

    ResponseMessage: any;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();


    }


    @ViewChild(CustomMessageWrapperComponent)
    SuperCustomMessageWrapperComponent: CustomMessageWrapperComponent = new CustomMessageWrapperComponent();
    ngAfterViewInit() {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        } else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent()
    }

    OnMassageDisplayMethod() {
        if (this.RequestParams == null) {
            this.RequestParams = new ClientSearchRequestParams();
        }

        //if (this.ResponseData) {
        //    if (this.ResponseData.TransactionsList) {
        //        this.TransactionsResultList.InsertCollection(this.ResponseData.TransactionsList);
        //    }

        //}
    }

    //#region Properties
    private message: string = "";
    get Message() { return this.message; }
    set Message(value: string) {
        if (this.message != value) {
            this.message = value;
        }
    }

    //#endregion Properties

    //#region General Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        this.SendRecallMessageToServer();
    }

    public ShowMessage(message: string) {
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
    }

    public SendRecallMessageToServer() {
        this.ProgressBarPercentText = "0%";

        this.filterImageParameter = new ImageParameter();
        this.filterImageParameter.Key = Guid.newGuid();
        this.filterImageParameter.IsFirstTry = true;
        this.filterImageParameter.UploadMode = "Block";
        this.filterImageParameter.Tenant = SessionLocator.Tenant;

        var myCustomMessageProgressHelper = new CustomMessageProgressHelper(this.CurrentSession);
        myCustomMessageProgressHelper.BasicResponse = true;
        myCustomMessageProgressHelper.StartProgress(this.filterImageParameter.Key, 5, true);

        this._ClientMessagesService.PutRecallClientsForCutomsRequest(this.filterImageParameter).subscribe((myServiceResponse: ServiceResponse) => {
            console.log("[Send] Response/PutRecallClientsForCutomsRequest : ", myServiceResponse.Result);
            var response = myServiceResponse.Result;

            myCustomMessageProgressHelper.MessageArrived = true;
            this.CurrentSession.StopBusyIndicator();
            if (!AppTool.IsNullOrEmpty(response)) {
            }
        });

    }

    //#endregion Commands
}
