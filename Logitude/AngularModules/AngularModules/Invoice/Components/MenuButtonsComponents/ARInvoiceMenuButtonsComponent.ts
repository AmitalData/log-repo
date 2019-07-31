import {Component}  from '@angular/core';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {ARInvoicePM} from '../../EntityPMs/ARInvoicePM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {InvoiceDomainService} from '../../Services/InvoiceDomainService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {Cloner} from '../../../Infrastructure/Utilities/Cloner';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    moduleId: module.id,
    templateUrl: './ARInvoiceMenuButtonsComponent.html',
})

export class ARInvoiceMenuButtonsComponent extends BaseComponent {
    public EntityPM: ARInvoicePM = null;
    public EventCode: string = null;
    public DataContext = this;
    public ObjectTableName: string = "ARInvoice";
    public OkButtonLabel: string = "Ok";
    public IsOkButtonEnabled: boolean = true;
    public ValidationErrorsList: string[] = [];
    public isRTL: boolean = false;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");       
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args["EntityPM"];
        this.EventCode = args["EventCode"];
        this.InitializeComponent();
        this.SetButtonLabel();
        this.SetButtonEnabled();
        this.Clone();
    }

    private InitializeComponent() {
        if (this.EventCode == "AutoCreditInvoiceDate") {
            this.Date = DateTool.GetCurrentDateAsUtc();
        }
    }
    private SetButtonLabel() {
            switch (this.EventCode) {
            case "SetAsSent": {
                    this.OkButtonLabel = TextCodeTranslator.Translate("General.O.Confirm");
                break;
            }

            default: {
                    this.OkButtonLabel = TextCodeTranslator.Translate("General.B.Ok");
                break;
            }
        }
    }
    private SetButtonEnabled() {
        switch (this.EventCode) {
            case "AutoCreditInvoiceDate": {
                this.IsOkButtonEnabled = this.Date == null ? false : true;
                break;
            }

            case "AutoCreditManualNumber": {
                this.IsOkButtonEnabled = AppTool.IsNullOrEmpty(this.ManualNumber) ? false : true;
                break;
            }

            default: {
                this.IsOkButtonEnabled = true;
                break;
            }
        }
    }

    // Properties
    get EventNote() { return this.EntityPM.EventNote; }
    set EventNote(value: string) {
        if (this.EntityPM.EventNote != value) {
            this.EntityPM.EventNote = value;
        }
    }

    private manualNumber: string;
    get ManualNumber() { return this.manualNumber; }
    set ManualNumber(value: string) {
        if (this.manualNumber != value) {
            this.manualNumber = value;
            this.SetButtonEnabled();
        }
    }

    private date: Date;
    get Date() { return this.date; }
    set Date(value: Date) {
        if (this.date != value) {
            this.date = value;
            this.SetButtonEnabled();
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        switch (this.EventCode) {
            case "AutoCreditManualNumber": {
                if (AppTool.IsNullOrEmpty(this.ManualNumber)) {
                    errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARInvoice.F.InvoiceNumber")));
                }

                break;
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {

            if (this.EventCode == "AutoCreditManualNumber") {
                this.CurrentSession.StartBusyIndicatorLoading();

                var myService = new InvoiceDomainService();
                myService.IsARInvoiceNumberExists(this.ManualNumber).subscribe((myResponse: ServiceResponse) => {

                    this.CurrentSession.StopBusyIndicator();

                    if (!myResponse.HasError) {
                        var isExists: boolean = myResponse.Result;
                        if (isExists) {
                            errors.push(TextCodeTranslator.Translate("ARInvoice.S.AutoCreditingMsg5"));
                            this.ValidationErrorsList = errors;
                        }

                        else {
                            this.CurrentSession.CloseCurrentWindowEmit("OK");
                        }
                    }
                });
            }

            else {
                this.CurrentSession.CloseCurrentWindowEmit("OK");
            }
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.EntityPM);
        this.myCloner.AddField('EventNote');
        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
