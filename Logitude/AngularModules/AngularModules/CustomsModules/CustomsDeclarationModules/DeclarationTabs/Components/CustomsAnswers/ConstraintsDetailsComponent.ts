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

import {DeclarationWebService} from '../../../../../Customs/Services/WebServices/DeclarationWebService';

// Send Request
import {INF_MSG_GenericResponseData} from '../../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';
import {VendorCommunicationResult} from '../../../../../Customs/DataContract/ResponseData/VendorCommunicationResult';
import {VendorInsertUpdateDeleteMessageRequestParams, OperationTypes} from '../../../../../Customs/DataContract/RequestParams/VendorInsertUpdateDeleteMessageRequestParams';
import { CustomMessageProgressComponent } from '../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import {DeclarationMessagesService} from '../../../../../Customs/Services/WebServices/DeclarationMessagesService';

@Component({
    moduleId: module.id,
    templateUrl: './ConstraintsDetailsComponent.html',
})

export class ConstraintsDetailsComponent extends BaseComponent {
    public DataContext: any = this;
    public ConstraintPM: DeclarationConstraintPM;
    public DeclarationError: DeclarationErrorView;
    public ObjectTableName: string = "Customs.DeclarationConstraint";
    public IsDisplayOnly: boolean = false;
    public IsAgentObjectionButtonVisibile: boolean = false;
    ApprovalDenaialTitle = "";

    //Services
    private declarationWebService: DeclarationWebService = new DeclarationWebService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
       
    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.ConstraintPM = args.ConstraintPM;
            this.DeclarationError = args.DeclarationError;
            this.ApprovalDenaialTitle = args.ApprovalDenaialTitle;
            this.IsAgentObjectionButtonVisibile = args.IsAgentObjectionButtonVisibile;
            
        }
    }

    AgentButtonClicked() {
        var window = new LogitudeWindow();
        window.Width = 500;
        window.Height = 500;
        window.Title = TextCodeTranslator.Translate("Customs.CustomsCollateral.O.AgentObjection");
        window.WindowArgs = {
            DeclarationError: this.DeclarationError,
            ConstraintPM: this.ConstraintPM,
        };
        window.Show("./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/CustomsAnswers/AgentObjectionComponent");
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
