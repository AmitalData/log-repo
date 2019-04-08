declare var System: any, window: any;
import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {AppTool} from '../../../../Infrastructure/Tools';
import {FSRWebService, FSRResultClass} from '../../../../Infrastructure/Services/WebServices/FSRWebService';
import {AWBOverviewTabComponent} from '../AWBWizard/Overview/AWBOverviewTabComponent';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,

    templateUrl: './SendFSRComponent.html',
})

export class SendFSRComponent {
    private entityPM: ShipmentPM;
    private Tenant: number;
    private tenantZeroAirlineField: string;
    private myFSRWebService: FSRWebService;
    public IsRecipientsVisible: boolean = false;
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.ValidationErrorsList = [];
    }

    private OverviewTab: AWBOverviewTabComponent = null;
    public SetWindowArgs(args: SendFSRArgs) {
        this.Tenant = args.EntityPM.Tenant;
        this.entityPM = args.EntityPM;
        this.OverviewTab = args.OverviewTab;
        this.tenantZeroAirlineField = args.EntityPM.TenantZeroAirlineTTY;
        this.OtherRecipient = this.tenantZeroAirlineField;
        this.InitializeComponent();
    }

    private InitializeComponent() {
        this.SetRecipientsVisibility();
        this.SetDefaultRecipient();
        this.SetSendButton();
    }
    private InitializeWebService() {
        if (this.myFSRWebService == null) {
            this.myFSRWebService = new FSRWebService();
        }
    }

    // Recipients
    public CargRecipient: string = "QIFFMXS"; // Cargo Test System
    public XXArRecipient: string = "LUXX9CV"; // Luxembourg
    public Fra1Recipient: string = "REUDLHT"; // Frankfurt
    public Fra2Recipient: string = "ZCSSMIT"; // Frankfurt Simulator
    public RichRecipient: string = "QIFRHXS"; // Richard Email
    public IhabRecipient: string = "QIFFMAL"; // Ihab Email
    public CargonautRecipient: string = "REUCGNP";
    public DEXXRecipient: string = "REUBCSP";
    public OtherRecipient: string;

    public IsDEXXVisible: boolean = false;
    public IsCargonautVisible: boolean = false;
    private SetRecipientsVisibility() {
        if (FeatureLocator.IsPackage_DVMT()) {
            this.IsRecipientsVisible = true;

            switch (this.entityPM.MainCarriageFromPortCode) {
                case "SPL":
                case "AMS":
                case "RTM":
                case "MST":
                    {
                        this.IsCargonautVisible = true;
                        break;
                    }

                case "LGG":
                case "BRU":
                    {
                        this.IsDEXXVisible = true;
                        break;
                    }

                default:
                    {
                        this.IsDEXXVisible = false;
                        this.IsCargonautVisible = false;
                        break;
                    }
            }
        }
    }
    private SetDefaultRecipient() {
        if (!AppTool.IsNullOrEmpty(this.tenantZeroAirlineField)) {
            this.OtheRecipientIsChecked = true;
        }

        else {
            this.CargRecipientIsChecked = true;
        }
    }
    private FalseAllRadioButtons() {
        this.cargRecipientIsChecked = false;
        this.xXArRecipientIsChecked = false;
        this.fra1RecipientIsChecked = false;
        this.fra2RecipientIsChecked = false;
        this.richRecipientIsChecked = false;
        this.ihabRecipientIsChecked = false;
        this.cargonautRecipientIsChecked = false;
        this.dEXXRecipientIsChecked = false;
        this.otheRecipientIsChecked = false;
    }

    private selectedRecipient: string = "QIFFMXS";
    get SelectedRecipient() { return this.selectedRecipient; }
    set SelectedRecipient(newValue: string) {
        if (this.selectedRecipient != newValue) {
            this.selectedRecipient = newValue;
        }
    }

    private cargRecipientIsChecked: boolean;
    get CargRecipientIsChecked() { return this.cargRecipientIsChecked; }
    set CargRecipientIsChecked(newValue: boolean) {
        if (this.cargRecipientIsChecked != newValue) {
            this.FalseAllRadioButtons();
            this.cargRecipientIsChecked = newValue;

            if (newValue) {
                this.SelectedRecipient = this.CargRecipient;
            }
        }
    }

    private xXArRecipientIsChecked: boolean;
    get XXArRecipientIsChecked() { return this.xXArRecipientIsChecked; }
    set XXArRecipientIsChecked(newValue: boolean) {
        if (this.xXArRecipientIsChecked != newValue) {
            this.FalseAllRadioButtons();
            this.xXArRecipientIsChecked = newValue;

            if (newValue) {
                this.SelectedRecipient = this.XXArRecipient;
            }
        }
    }

    private fra1RecipientIsChecked: boolean;
    get Fra1RecipientIsChecked() { return this.fra1RecipientIsChecked; }
    set Fra1RecipientIsChecked(newValue: boolean) {
        if (this.fra1RecipientIsChecked != newValue) {
            this.FalseAllRadioButtons();
            this.fra1RecipientIsChecked = newValue;

            if (newValue) {
                this.SelectedRecipient = this.Fra1Recipient;
            }
        }
    }

    private fra2RecipientIsChecked: boolean;
    get Fra2RecipientIsChecked() { return this.fra2RecipientIsChecked; }
    set Fra2RecipientIsChecked(newValue: boolean) {
        if (this.fra2RecipientIsChecked != newValue) {
            this.FalseAllRadioButtons();
            this.fra2RecipientIsChecked = newValue;

            if (newValue) {
                this.SelectedRecipient = this.Fra2Recipient;
            }
        }
    }

    private richRecipientIsChecked: boolean;
    get RichRecipientIsChecked() { return this.richRecipientIsChecked; }
    set RichRecipientIsChecked(newValue: boolean) {
        if (this.richRecipientIsChecked != newValue) {
            this.FalseAllRadioButtons();
            this.richRecipientIsChecked = newValue;

            if (newValue) {
                this.SelectedRecipient = this.RichRecipient;
            }
        }
    }

    private ihabRecipientIsChecked: boolean;
    get IhabRecipientIsChecked() { return this.ihabRecipientIsChecked; }
    set IhabRecipientIsChecked(newValue: boolean) {
        if (this.ihabRecipientIsChecked != newValue) {
            this.FalseAllRadioButtons();
            this.ihabRecipientIsChecked = newValue;

            if (newValue) {
                this.SelectedRecipient = this.IhabRecipient;
            }
        }
    }

    private cargonautRecipientIsChecked: boolean;
    get CargonautRecipientIsChecked() { return this.cargonautRecipientIsChecked; }
    set CargonautRecipientIsChecked(newValue: boolean) {
        if (this.cargonautRecipientIsChecked != newValue) {
            this.FalseAllRadioButtons();
            this.cargonautRecipientIsChecked = newValue;

            if (newValue) {
                this.SelectedRecipient = this.CargonautRecipient;
            }
        }
    }

    private dEXXRecipientIsChecked: boolean;
    get DEXXRecipientIsChecked() { return this.dEXXRecipientIsChecked; }
    set DEXXRecipientIsChecked(newValue: boolean) {
        if (this.dEXXRecipientIsChecked != newValue) {
            this.dEXXRecipientIsChecked = newValue;

            if (newValue) {
                this.SelectedRecipient = this.DEXXRecipient;
            }
        }
    }

    private otheRecipientIsChecked: boolean;
    get OtheRecipientIsChecked() { return this.otheRecipientIsChecked; }
    set OtheRecipientIsChecked(newValue: boolean) {
        if (this.otheRecipientIsChecked != newValue) {
            this.FalseAllRadioButtons();
            this.otheRecipientIsChecked = newValue;

            if (newValue) {
                this.SelectedRecipient = this.OtherRecipient;
            }
        }
    }

    public IsSendButtonVisible: boolean = false;
    private SetSendButton() {
        var isVisible = false;

        if (FeatureLocator.IsPackage_DVMT()) {
            isVisible = true;
        }

        else {
            var errors: string[] = this.ValidateSending();

            if (errors.length > 0) {
                isVisible = true;
            }
        }

        this.IsSendButtonVisible = isVisible;

        if (!isVisible) {
            this.SendClicked();
        }
    }

    CloseClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SendClicked() {

        this.CurrentSession.StartBusyIndicator("Sending in Progress..");

        var objectTableName = (this.entityPM.ShipmentLevelCode == "C") ? "Master" : "Shipment";
        var ObjectTable = window.ObjectTables.filter(x => x.Name === objectTableName)[0];
        var objectTableId = ObjectTable.Id;

        this.InitializeWebService();

        var errors: string[] = this.ValidateSending();

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.myFSRWebService.SendFSR(this.entityPM.Id, objectTableId, this.SelectedRecipient).subscribe((myResponse: ServiceResponse) => {
                if (myResponse == null) {
                    this.CurrentSession.StopBusyIndicator();
                }

                else if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }

                else {
                    var myResult: FSRResultClass = myResponse.Result;

                    if (myResult != null) {
                        this.CurrentSession.StopBusyIndicator();
                        this.SendingResultForeground = this.greenForeground;
                        this.SendingResultMessage = "FSR has been sent Successfully";
                        this.ReloadEntity();
                    }

                    else {
                        this.CurrentSession.StopBusyIndicator();
                    }
                }
            });
        }

        else {
            this.CurrentSession.StopBusyIndicator();
        }
    }
    public StockAreaIsVisible: boolean;

    // Validate
    private ValidateSending() {

        this.SendingResultMessage = "";
        var errors: string[] = [];

        if (AppTool.IsNullOrEmpty(this.entityPM.TenantZeroAirlineTTY)) {
            errors.push("Tenant communication parameter (TTY) is missing");
        }

        this.ValidateRecipient(errors);

        return errors;
    }
    private ValidateRecipient(errors: string[]) {
        if (FeatureLocator.IsPackage_DVMT()) {
            if (AppTool.IsNullOrEmpty(this.SelectedRecipient)) {
                errors.push("Please fill your Recipient");
            }

            else {
                this.SelectedRecipient = this.SelectedRecipient.toUpperCase();

                //if (this.SelectedRecipient.length != 7) {
                //    errors.push("Recipient length must be 7");
                //}
            }
        }
    }

    private redForeground = "#E53030";
    private greenForeground = "#009161";
    public SendingResultMessage: string;
    public SendingResultForeground: string;
    get DemoAreaIsVisible() {
        var myResult = false;

        if (this.Tenant == 65 || SessionLocator.TenantManagementJS.IsEAWBOnlyDemo) {
            myResult = true;
        }

        return myResult;
    }


    private ReloadEntity() {
        if (this.OverviewTab != null) {
            this.OverviewTab.ReloadEntity();
        }
    }
}
export class SendFSRArgs {
    public EntityPM: ShipmentPM;
    public OverviewTab: any;
}
