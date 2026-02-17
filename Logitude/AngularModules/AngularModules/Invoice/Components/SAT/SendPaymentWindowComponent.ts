import {Component} from '@angular/core';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {AppTool} from '../../../Infrastructure/Tools';
import {ARPaymentPM} from '../../../Invoice/EntityPMs/ARPaymentPM';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';

import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {InvoiceDomainService} from '../../Services/InvoiceDomainService';
import {ARInvoiceSATStatus} from '../../Services/InvoiceDomainService';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';

@Component({
    moduleId: module.id,
    templateUrl: './SendPaymentWindowComponent.html',
})

export class SendPaymentWindowComponent {
    private Tenant: number;
    private entityPM: ARPaymentPM;
    public ValidationErrorsList: string[];
    public ValidationWarningsList: string[];
    private invoiceDomainService: InvoiceDomainService = new InvoiceDomainService();
    public IsSendButtonVisible: boolean = false;

    public IsPaymentValid: boolean = false;
    public IsPaymentWarning: boolean = false;
    public IsPaymentError: boolean = false;
    public PaymentWarningText: string = null;
    public PaymentErrorText: string = null;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.ValidationErrorsList = [];
        this.ValidationWarningsList = [];
         
    }

    public SetWindowArgs(args: any) {
        this.Tenant = args.EnttiyPM.Tenant;
        this.entityPM = args.EnttiyPM;

      if (this.entityPM.MetodoPagoCode == "PUE") {
        this.IsInvoicesStatusVisible = false;
        this.IsPaymentValid = false;
        this.IsPaymentError = true;
        this.PaymentErrorText = "Invalid";
          this.ValidationErrorsList.push("You can only send payments to SAT when the Metodo Pago value is PPD");
        return;
      }

        if (this.entityPM.StatusCode == "AD" || this.entityPM.StatusCode == "CL") {
            if (this.entityPM.PaymentInvoices.length > 0) {
                this.IsPaymentValid = true;
                this.LoadARInvoiceSATStatus();
            }
            else {
                this.IsInvoicesStatusVisible = false;
                this.IsPaymentValid = false;
                this.IsPaymentError = true;
                this.PaymentErrorText = "Invalid";
                this.ValidationErrorsList.push("The payment should be connected to one invoice at least");
            }
        }
        else {
            this.IsInvoicesStatusVisible = false;
            this.IsPaymentValid = false;
            this.IsPaymentError = true;
            this.PaymentErrorText = "Invalid";
            this.ValidationErrorsList.push("The payment must be approved before sending it to SAT");
        }

    }

    

    public IsInvoicesValid: boolean = false;
    public IsInvoicesWarning: boolean = false;
    public IsInvoicesError: boolean = false;
    public InvoicesWarningText: string = null;
    public InvoicesErrorText: string = null;
    public IsInvoicesStatusVisible: boolean = true;
    public ARInvoiceSATStatusList: ARInvoiceSATStatusItemViewModel[] = [];
    get InvoicesStatusLabel() {
        var myResult: string = "";
        myResult = "Invoice(s) Status";
        return myResult;
    }


    LoadARInvoiceSATStatus() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Loading"));
        this.invoiceDomainService.GetARInvoiceSATStatus(this.entityPM.Id).subscribe((response: ServiceResponse) => {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            if (response != null) {
                if (!response.HasError) {
                    var myResult = response.Result;


                    if (myResult) {
                        myResult.forEach(item => {
                            this.ARInvoiceSATStatusList.push(new ARInvoiceSATStatusItemViewModel(item, this));
                        });
                    }

                    if (!this.ARInvoiceSATStatusList.some(a => a.IsSATValid == false)) {
                        this.IsPaymentValid = true;
                        this.IsInvoicesValid = true;
                        this.IsSendButtonVisible = true;
                    }
                    else {
                        this.ValidationErrorsList.push("Some invoices are not approved from SAT");
                        this.IsInvoicesError = true;
                        this.InvoicesErrorText = "Invalid";
                    }


                }
                else {
                    this.IsInvoicesStatusVisible = false;
                    this.IsPaymentValid = false;
                    this.IsPaymentError = true;
                    this.PaymentErrorText = "Invalid";
                    this.ValidationErrorsList = response.ErrorsArray;
                    //this.IsInvoicesValid = true;
                    //var messageWindow = new MessageWindow();
                    //messageWindow.Show(response.ErrorsArray.toString());
                }

            }

        });
    }

    CloseClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SendClicked() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        this.invoiceDomainService.SendARPaymentSATXML(this.entityPM.Id).subscribe((response: ServiceResponse) => {
            if (response != null) {
                if (!response.HasError) {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.CurrentSession.CurrentWindow.Close("");
                }
                else {
                    var messageWindow = new MessageWindow();
                    messageWindow.Show(response.ErrorsArray.toString());
                }
            }

        });
    }

}



export class ARInvoiceSATStatusItemViewModel {
    public IsSATValid: boolean = false;
    public BillTo: string;
    public SATStatusName: string;
    public ARInvoiceNumber: string;
    public SATStatusCode: string;
    constructor(private item: ARInvoiceSATStatus, private fatherComponent: SendPaymentWindowComponent) {
        this.BillTo = item.BillTo;
        this.SATStatusName = item.SATStatusName;
        this.ARInvoiceNumber = item.ARInvoiceNumber;
        this.IsSATValid = item.IsSATValid;
        this.SATStatusCode = item.SATStatusCode;

    }

    ViewShipment() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.Title = "Edit HAWB Wizard";
        logWindow.WindowArgs = this.item.ARInvoiceNumber;
        logWindow.WindowClosed.subscribe(($event: any) => this.fatherComponent.LoadARInvoiceSATStatus());
        logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardLoadComponent');
    }
}
