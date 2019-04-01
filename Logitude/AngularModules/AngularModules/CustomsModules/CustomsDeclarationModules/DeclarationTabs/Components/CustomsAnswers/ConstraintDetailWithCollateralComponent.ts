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
import {CustomsCollateralPM} from '../../../../../Customs/EntityPMs/CustomsCollateralPM';
import {DeclarationErrorView} from '../../../../../Customs/EntityPMs/Extended/DeclarationErrorView';
import {DeclarationConstraintPM} from '../../../../../Customs/EntityPMs/DeclarationConstraintPM';
import {DeclarationEventManager} from '../../../../../Customs/Utilities/DeclarationEventManager';

import {DeclarationWebService} from '../../../../../Customs/Services/WebServices/DeclarationWebService';
import {CustomsCollateralListService} from '../../../../../Customs/Services/StandardLists/CustomsCollateralListService';

// Send Request
import {INF_MSG_GenericResponseData} from '../../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';
import {VendorCommunicationResult} from '../../../../../Customs/DataContract/ResponseData/VendorCommunicationResult';
import {VendorInsertUpdateDeleteMessageRequestParams, OperationTypes} from '../../../../../Customs/DataContract/RequestParams/VendorInsertUpdateDeleteMessageRequestParams';
import { CustomMessageProgressComponent } from '../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import {DeclarationMessagesService} from '../../../../../Customs/Services/WebServices/DeclarationMessagesService';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './ConstraintDetailWithCollateralComponent.html',
})

export class ConstraintDetailWithCollateralComponent extends BaseComponent {
    public DataContext: any = this;
    public ConstraintPM: DeclarationConstraintPM;
    public customsCollateralPM: CustomsCollateralPM;
    public DeclarationError: DeclarationErrorView;
    public ObjectTableName: string = "Customs.DeclarationConstraint";
    public IsDisplayOnly: boolean = false;
    public IsAgentObjectionButtonVisibile: boolean = false;
    gridHeight: number = 53;

    Conditionslist: any[] = [];
    //Services
    private declarationWebService: DeclarationWebService = new DeclarationWebService;
    private customsCollateralListService: CustomsCollateralListService = new CustomsCollateralListService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private EntityResourceService: EntityResourceService) {
        super();
       
    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            console.log("[Args] ",args);
            this.ConstraintPM = args.ConstraintPM;
            this.DeclarationError = args.DeclarationError;
            this.IsAgentObjectionButtonVisibile = args.IsAgentObjectionButtonVisibile;
            this.LoadCollateralFromConstraint();
            
        }
    }

    //#region Properties

    totalAmount: number = 0.0;
    get TotalAmount() {
        if (!AppTool.IsNullOrEmpty(this.customsCollateralPM)) {
            this.totalAmount = 0.0;

            if (!AppTool.IsNullOrEmpty(this.customsCollateralPM.CustomsCollateralsConditions)) {
                this.customsCollateralPM.CustomsCollateralsConditions.forEach(item => {
                    this.totalAmount += item.RequestedAmount;
                });
            }

        }
        return this.totalAmount;
    }

    fieldName: string = null;
    get FieldName() {

        var field = "";
        if (!AppTool.IsNullOrEmpty(this.DeclarationError.FieldNameTextCode)) {
            field = TextCodeTranslator.Translate(this.DeclarationError.FieldNameTextCode);
        }
        return field;
    }

    //#endregion

    LoadCollateralFromConstraint() {
        this.declarationWebService.GetSingleCustomsCollateral(this.ConstraintPM.CustomsCollateralId).subscribe((response: ServiceResponse) => {
            var res = response.Result;
            console.log("[Response] GetSingleCustomsCollateral", res);
            if (!AppTool.IsNullOrEmpty(res)) {
                this.customsCollateralPM = res;

                this.Conditionslist = [];
                this.Conditionslist = this.customsCollateralPM.CustomsCollateralsConditions;
                if (this.Conditionslist.length > 1) {
                    var itemsHeight = this.Conditionslist.length * 25;
                    this.gridHeight += itemsHeight - 25; //25 for the first item
                }
            }
        });
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

    ColleteralButtonClicked() {
        var windowArgs: any = {};
            this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateralsAnswer").subscribe(response => {
                
                windowArgs.CurrentEntity = this.customsCollateralPM;
                var logWindow = new LogitudeWindow();

                logWindow.Width = 600;
                logWindow.Height = 710;
                logWindow.WindowArgs = windowArgs;
                logWindow.ShowCloseButton = true;
                logWindow.IsHideHeader = true;
              logWindow.Show('./CustomsModules/CustomsCollateral/Components/CustomsCollateralComponent');
                logWindow.WindowClosed.subscribe(($event1: any) => {
            });

        });
    }
}
