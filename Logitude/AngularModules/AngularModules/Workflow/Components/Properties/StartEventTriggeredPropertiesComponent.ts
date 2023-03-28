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
    public WorkflowProviderSubscription: ServiceProviderSubscriptionPM;
    public BusyIndicatorText: string = null;
    public BusyIndicatorWidth: number = 200;
    public ShowBusyIndicator: boolean = false;
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
        this.initialize();
        this.initializeStartEventApplicationsList();
        this.initializeStartEventTypesList();
        this.loadWorkflowProviderSubscription();
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

    initialize() {
        this.Data["triggerType"] = StartTriggerTypes.EventTriggered;

        this.IsNew = Object.keys(this.Data).length === 0;

        this.Application = this.Data["application"] || StartEventApplications.MicrosoftOffice365;
        this.EventType = this.Data["eventType"] || StartEventTypes.NewEmail;

        this.Data["application"] = this.Application;
        this.Data["eventType"] = this.EventType;

        this.setDefaultData();
    }

    initializeStartEventApplicationsList() {
        this.StartEventApplicationsListItems = new StartEventApplicationsList().Items;
    }

    initializeStartEventTypesList() {
        this.StartEventTypesListItems = new StartEventTypesList().Items;
    }

    loadWorkflowProviderSubscription() {
        this.startBusyIndicator();
        let serviceProviderSubscriptionExtendedService = new ServiceProviderSubscriptionExtendedService();
        serviceProviderSubscriptionExtendedService.getByWorkflowNumber(this.WorkflowNumber).subscribe(serviceResponse => {
            if (!serviceResponse.HasError) {
                this.WorkflowProviderSubscription = serviceResponse.Data;
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
            console.log(authenticationResult);
            console.log(this.getMsalRefreshToken(authenticationResult));

            let accessToken = authenticationResult.accessToken;
            let refreshToken = this.getMsalRefreshToken(authenticationResult);

            this.startBusyIndicator();
            let microsoftOffice365Service = new MicrosoftOffice365Service();
            microsoftOffice365Service.createSubscription(this.WorkflowNumber, accessToken, refreshToken).subscribe(serviceResponse => {
                if (!serviceResponse.HasError) {
                    this.WorkflowProviderSubscription = serviceResponse.Data;
                }
                this.stopBusyIndicator();
            });
        });
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

    // logout() {
    //     this.authService.logoutPopup({
    //         mainWindowRedirectUri: "/"
    //     });
    // }

    startBusyIndicator(message: string = "Loading ...") {
        this.BusyIndicatorText = message;
        this.ShowBusyIndicator = true;
    }

    stopBusyIndicator() {
        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
    }
}