declare var window: any;
import {Component, AfterViewInit, ChangeDetectorRef}  from '@angular/core';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {AppTool, ArrayTool} from '../../../../../Infrastructure/Tools';
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ObservableCollection} from '../../../../../Infrastructure/Utilities/ObservableCollection';
import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../../../Controls/Windows/MessageWindow';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {DeclarationDisplayOnlyChecks, DisplayOnlyCheckResult} from '../../../../../Customs/Utilities/DeclarationDisplayOnlyChecks';

import {DeclarationPM} from '../../../../../Customs/EntityPMs/DeclarationPM';
import {DeclarationErrorView} from '../../../../../Customs/EntityPMs/Extended/DeclarationErrorView';
import {DeclarationConstraintPM} from '../../../../../Customs/EntityPMs/DeclarationConstraintPM';
import {DeclarationEventManager} from '../../../../../Customs/Utilities/DeclarationEventManager';

import {DeclarationPMService} from '../../../../../Customs/Services/StandardPMs/DeclarationPMService';

// Send Request
import {INF_MSG_GenericResponseData} from '../../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';
import {VendorCommunicationResult} from '../../../../../Customs/DataContract/ResponseData/VendorCommunicationResult';
import {ConstraintAgentObjectionRequestParams} from '../../../../../Customs/DataContract/RequestParams/ConstraintAgentObjectionRequestParams';
import { CustomMessageProgressComponent } from '../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import {DeclarationMessagesService} from '../../../../../Customs/Services/WebServices/DeclarationMessagesService';

@Component({
    
    templateUrl: './AgentObjectionComponent.html',
})

export class AgentObjectionComponent extends BaseComponent {
    public DataContext: any = this;
    public ConstraintPM: DeclarationConstraintPM;
    public DeclarationError: DeclarationErrorView;
    public ObjectTableName: string = "Customs.DeclarationConstraint";
    public IsDisplayOnly: boolean = false;

    //Services
    private declarationMessagesService: DeclarationMessagesService = new DeclarationMessagesService;
    private declarationPMService: DeclarationPMService = new DeclarationPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
       
    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.ConstraintPM = args.ConstraintPM;
            this.DeclarationError = args.DeclarationError;
        }
    }

    get AgentObjection() {
        return this.ConstraintPM.AgentObjection;
    }
    set AgentObjection(value: string) {
        this.ConstraintPM.AgentObjection = value;
    }

    SendButtonClicked() {
        //1. save changes
        //this.declarationPMService.update(this.ConstraintPM).subscribe((myRespone: ServiceResponse) => {
        this.CurrentSession.CurrentEditComponent.SaveChanges();

        //2. send
        if (!AppTool.IsNullOrEmpty(this.ConstraintPM.AgentObjection)) {
            this.SendConstraintAgentObjectionMethod();
        }
        else {

            var message = new MessageWindow();
            message.Show(TextCodeTranslator.Translate("Customs.Declaration.O.FillAgentObjection"));
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SendConstraintAgentObjectionMethod() {

        var ObjectTable = window.ObjectTables.filter(x => x.Name === "Customs.Declaration")[0];

        var requestParams = new ConstraintAgentObjectionRequestParams();
        requestParams.LoggingEnabled = true;
        requestParams.LoggingUserId = SessionLocator.LoggedUserId;
        requestParams.ConstraintNumber = this.ConstraintPM.ConstraintNumber;
        requestParams.AgentObjection = this.ConstraintPM.AgentObjection;
        requestParams.Tenant = SessionLocator.Tenant;
        requestParams.RequestName = "Declaration Constriant";
        requestParams.ResponseName = "Declaration Constriant";
        //requestParams.TestCase = SelectedTest;
        requestParams.DeclarationId = this.ConstraintPM.DeclarationID;
        requestParams.LoggingEntityId = this.ConstraintPM.DeclarationID;
        //requestParams.//LoggingEntityReference = declarationPM.DeclarationNumber;
        requestParams.LoggingObjectTableId = ObjectTable.Id;

        CustomMessageProgressComponent.ShowProgressBar(this.CurrentSession,requestParams.PBId, "שליחת בקשת ערעור", false).then((res) => {

            console.log("[Send] Response/ShowProgressBar : ", res);

        }).catch((err) => {
            console.error("[Send Service Error] ", err);
        });


        this.declarationMessagesService.PostSendDeclarationConstraintAgentObjection(requestParams).subscribe((myServiceResponse: ServiceResponse) => {
            console.log("[Send] Response/PostSendDeclarationConstraintAgentObjection : ", myServiceResponse.Result);
            var response = myServiceResponse.Result;

            if (!AppTool.IsNullOrEmpty(response)) {

            }
            else {
                var message = "Service returned a null response!";
            }

            //this.OnSendCompleted();


        });

    }
}
