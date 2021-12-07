import { Component} from '@angular/core';
import { BaseComponent } from '../../../LogitudeComponents/BaseComponent';
import { WebHookAutomationDetails } from '../../../../DataContracts/AutomationSendInterface';
import { SessionLocator } from '../../../../Utilities/SessionLocator';
import { AppTool } from '../../../../Tools';
import { Operator } from '../ViewModel/AutomationConditionViewModel';

@Component({
    templateUrl: './WebHookAutomationDetailsComponent.html',

})
export class WebHookAutomationDetailsComponent extends BaseComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext: any;
    public WebHookAutomationDetails: WebHookAutomationDetails;
    public ObjectTableName: string = "WebHookAutomationDetails";
    ValidationErrorsList: string[];

    AuthenticationTypes: Operator[] = [];

    constructor() {
        super();
        this.DataContext = this;
        this.FillAuthenticationTypes();
    }

    FillAuthenticationTypes() {
        this.AuthenticationTypes = [];
        this.AuthenticationTypes.push(new Operator("None", "NONE"));
        this.AuthenticationTypes.push(new Operator("Basic Authentication", "BASICAUTHENTICATION"));
    }

    SetWindowArgs(args: any) {
        this.WebHookAutomationDetails = args.WebHookDetails;
        if (this.WebHookAutomationDetails) {
            this.FillWebHookDetails();
        }
    }

    private FillWebHookDetails() {
        this.URL = this.WebHookAutomationDetails.URL;
        this.AuthenticationTypeSelected = this.AuthenticationTypes.filter(d => d.Code == this.WebHookAutomationDetails.AuthenticationType)[0];
        this.BasicAuthUserName = this.WebHookAutomationDetails.BasicAuthUserName;
        this.BasicAuthPassword = this.WebHookAutomationDetails.BasicAuthPassword;
    }

    SaveButtonClicked() {
        this.ValidateDetails();
        if (this.ValidationErrorsList.length != 0) return;

        this.WebHookAutomationDetails.URL = this.URL;
        this.WebHookAutomationDetails.AuthenticationType = this.AuthenticationTypeSelected?.Code;
        this.WebHookAutomationDetails.BasicAuthUserName = this.BasicAuthUserName;
        this.WebHookAutomationDetails.BasicAuthPassword = this.BasicAuthPassword;
        this.CurrentSession.CurrentWindow.Close("Changed");
    }

    private ValidateDetails() {
        this.ValidationErrorsList = [];

        if (AppTool.IsNullOrEmpty(this.URL))
            this.ValidationErrorsList.push("WebHook URL is required.");
        if (this.AuthenticationTypeSelected?.Code == "BASICAUTHENTICATION" && AppTool.IsNullOrEmpty(this.BasicAuthUserName))
            this.ValidationErrorsList.push("User Name is required.");
        if (this.AuthenticationTypeSelected?.Code == "BASICAUTHENTICATION" && AppTool.IsNullOrEmpty(this.BasicAuthPassword))
            this.ValidationErrorsList.push("Password is required.");
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    private url: string;
    get URL() { return this.url; }
    set URL(value: string) {
        if (this.url != value) {
            this.url= value;
        }
    }

    private authenticationTypeSelected: Operator;
    get AuthenticationTypeSelected() { return this.authenticationTypeSelected; }
    set AuthenticationTypeSelected(newValue: Operator) {
        if (newValue != this.authenticationTypeSelected) {
            this.authenticationTypeSelected = newValue;
        }
    }

    private basicAuthUserName: string;
    get BasicAuthUserName() { return this.basicAuthUserName; }
    set BasicAuthUserName(value: string) {
        if (this.basicAuthUserName != value) {
            this.basicAuthUserName = value;
        }
    }

    private basicAuthPassword: string;
    get BasicAuthPassword() { return this.basicAuthPassword; }
    set BasicAuthPassword(value: string) {
        if (this.basicAuthPassword != value) {
            this.basicAuthPassword = value;
        }
    }
}
