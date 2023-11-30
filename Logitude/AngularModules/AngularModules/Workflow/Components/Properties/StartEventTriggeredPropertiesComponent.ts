import { Component, Inject } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { ConditionOperations } from "Workflow/Constants/ConditionOperations";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { StartEventApplications } from "Workflow/Constants/StartEventApplications";
import { StartEventTypes } from "Workflow/Constants/StartEventTypes";
import { StartTriggerTypes } from "Workflow/Constants/StartTriggerTypes";
import { StartEventApplicationsList } from "Workflow/Lists/StartEventApplicationsList";
import { StartEventTypesList } from "Workflow/Lists/StartEventTypesList";
import { Condition } from "Workflow/Models/Condition";
import { ListItem } from "Workflow/Models/ListItem";
import { MsalService, MSAL_GUARD_CONFIG, MsalGuardConfiguration } from "@azure/msal-angular";
import { AuthenticationResult, PopupRequest } from "@azure/msal-browser";
import { MsalConfigurations } from "Workflow/Utilities/MsalConfigurations";
import { ServiceProviderSubscriptionExtendedService } from "Workflow/Services/Extended/ServiceProviderSubscriptionExtendedService";
import { ServiceProviderSubscriptionPM } from "Workflow/EntityPMs/ServiceProviderSubscriptionPM";
import { MicrosoftOffice365Service } from "Workflow/Services/Extended/MicrosoftOffice365Service";
import { CreateSubscription } from "Workflow/Models/CreateSubscription";

@Component({
    templateUrl: "./StartEventTriggeredPropertiesComponent.html"
})

export class StartEventTriggeredPropertiesComponent extends BaseComponent {

    public DataContext: any = this;
    public Data: any;
    public WorkflowNumber: string;
    public IsNew: boolean;
    public Application: string = null;
    public EventType: string = null;
    public ValidationErrorsList: string[];
    public StartEventApplicationsListItems: ListItem[];
    public StartEventTypesListItems: ListItem[];
    public WorkflowProviderSubscription: ServiceProviderSubscriptionPM | null = null;
    public CurrentSession = SessionLocator.SelectedSession;

    constructor(
        @Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration,
        private authService: MsalService
    ) {
        super();
    }

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};
        this.WorkflowNumber = args.WorkflowNumber || null;
    }

    ngOnInit() {
        this.initializeWindowEvents();
        this.initializeStartEventApplicationsList();
        this.initializeStartEventTypesList();
        this.initialize();
        this.initializeWorkflowProviderSubscription();
    }

    initializeWindowEvents() {
        this.CurrentSession.CurrentWindow.FooterButtonsClicked.subscribe((e: any) => {
            if (e === "submit") {
                this.saveButtonClicked();
            } else {
                this.cancelButtonClicked();
            }
        });
    }

    initializeStartEventApplicationsList() {
        this.StartEventApplicationsListItems = new StartEventApplicationsList().Items;
    }

    initializeStartEventTypesList() {
        this.StartEventTypesListItems = new StartEventTypesList().Items;
    }

    initialize() {
        this.Data["triggerType"] = StartTriggerTypes.EventTriggered;

        this.IsNew = Object.keys(this.Data).length === 0;

        let defaultStartEventApplication = this.StartEventApplicationsListItems.find(i => i.Code === StartEventApplications.MicrosoftOffice365);
        let defaultStartEventType = this.StartEventTypesListItems.find(i => i.Code === StartEventTypes.NewEmail);

        this.Application = this.Data["application"] || defaultStartEventApplication.Code;
        this.EventType = this.Data["eventType"] || defaultStartEventType.Code;

        this.Data["application"] = this.Application;
        this.Data["eventType"] = this.EventType;

        this.Data["applicationLabel"] = defaultStartEventApplication.Name;
        this.Data["eventTypeLabel"] = defaultStartEventType.Name;

        this.setDefaultData();
    }

    initializeWorkflowProviderSubscription() {
        setTimeout(() => {
            this.loadWorkflowProviderSubscription();
        }, 150);
    }

    loadWorkflowProviderSubscription() {
        this.startBusyIndicator();
        let serviceProviderSubscriptionExtendedService = new ServiceProviderSubscriptionExtendedService();
        serviceProviderSubscriptionExtendedService.getByWorkflowNumber(this.WorkflowNumber).subscribe(serviceResponse => {
            if (!serviceResponse.HasError) {
                this.WorkflowProviderSubscription = serviceResponse.Result;
            }
            this.stopBusyIndicator();
        });
    }

    setDefaultData() {
        this.Data["entity"] = "CommunicationLog";
        this.Data["trigger"] = "create";
        this.Data["conditions"] = this.getEventConditions();
        this.Data["conditionsOperation"] = ConditionOperations.And;
    }

    getEventConditions() {
        let conditions = [];
        let condition = new Condition();
        condition.field = "Channel";
        condition.fieldCode = "CommunicationLog.Channel";
        condition.type = FieldTypes.Text;
        condition.value = this.WorkflowNumber;
        conditions.push(condition);
        return conditions;
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    saveButtonClicked() {
        this.ValidationErrorsList = [];
        let notValidUIProperties = this.UIProperties.UIPropertyList.filter(u => !u.ValidValue);
        if (notValidUIProperties.length === 0) {
            console.log(this.Data);
            this.CurrentSession.CurrentWindow.Close(this.Data);
        } else {
            let validationErrors = notValidUIProperties.map(t => { return t.ValidationError; });
            this.ValidationErrorsList = validationErrors;
        }
    }

    openMsalLoginPopup() {
        let popupRequest: PopupRequest = this.msalGuardConfig.authRequest ?
            { ...this.msalGuardConfig.authRequest, scopes: MsalConfigurations.LoginScopes } :
            { scopes: MsalConfigurations.LoginScopes };

        this.authService.loginPopup(popupRequest).subscribe((authenticationResult: AuthenticationResult) => {
            this.createSubscription(authenticationResult);
        });
    }

    createSubscription(authenticationResult: AuthenticationResult) {
        if (authenticationResult) {
            this.startBusyIndicator();
            let createSubscription: CreateSubscription = {
                WorkflowNumber: this.WorkflowNumber,
                AccessToken: authenticationResult.accessToken,
                RefreshToken: this.getMsalRefreshToken(authenticationResult),
                UserEmail: authenticationResult.account.username,
                AccessTokenExpirationDateTime: authenticationResult.expiresOn
            };
            let microsoftOffice365Service = new MicrosoftOffice365Service();
            microsoftOffice365Service.createSubscription(createSubscription).subscribe(serviceResponse => {
                if (!serviceResponse.HasError) {
                    this.WorkflowProviderSubscription = serviceResponse.Result;
                }
                this.stopBusyIndicator();
            });
        }
    }

    getMsalRefreshToken(authenticationResult: AuthenticationResult) {
        if (authenticationResult) {
            let refreshTokenStorageKey = Object.keys(localStorage)
                .filter(key => key.indexOf(authenticationResult.uniqueId) !== -1 && key.indexOf("login.windows.net-refreshtoken") !== -1)[0];

            if (refreshTokenStorageKey) {
                let refreshTokenStorage = JSON.parse(localStorage.getItem(refreshTokenStorageKey));
                return refreshTokenStorage ? refreshTokenStorage.secret : null;
            }
        }
        return null;
    }

    startBusyIndicator(message: string = "Loading ...") {
        this.CurrentSession.CurrentWindow.StartBusyIndicator(message);
    }

    stopBusyIndicator() {
        this.CurrentSession.CurrentWindow.StopBusyIndicator();
    }
}