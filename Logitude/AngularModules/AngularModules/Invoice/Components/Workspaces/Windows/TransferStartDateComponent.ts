import {Component} from '@angular/core';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {AccountingSettingPM} from '../../../../Common/EntityPMs/AccountingSettingPM';
import {InvoiceDomainService} from '../../../Services/InvoiceDomainService';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';

@Component({
    moduleId: module.id,
    templateUrl: './TransferStartDateComponent.html',
})

export class TransferStartDateComponent extends BaseComponent {
    public Code: string = null;
    public EntityPM: AccountingSettingPM;
    public DataContext = this;
    public ObjectTableName: string = "AccountingSetting";
    public ValidationErrorsList: string[] = [];
    public IsResourcesReady: boolean = false;
    private singleEntityName: string;
    private pluralEntityName: string;
    private invoiceDomainService: InvoiceDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.invoiceDomainService = new InvoiceDomainService();
    }

    SetWindowArgs(args: any) {
        this.Code = args['Code'];
        this.EntityPM = args['EntityPM'];

        switch (this.Code) {
            case "ARInvoice":
            case "APInvoice":
                {
                    this.singleEntityName = "invoice";
                    this.pluralEntityName = "invoices";
                    break;
                }

            case "ARPayment":
            case "APPayment":
                {
                    this.singleEntityName = "payment";
                    this.pluralEntityName = "payments";
                    break;
                }
        }

        this.IsResourcesReady = true;
        this.Clone();
    }

    public get ARInvoiceTransferStartDate() { return this.EntityPM.ARInvoiceTransferStartDate; }
    public set ARInvoiceTransferStartDate(value: Date) {
        if (this.EntityPM.ARInvoiceTransferStartDate != value) {
            this.EntityPM.ARInvoiceTransferStartDate = value;
        }
    }

    public get APInvoiceTransferStartDate() { return this.EntityPM.APInvoiceTransferStartDate; }
    public set APInvoiceTransferStartDate(value: Date) {
        if (this.EntityPM.APInvoiceTransferStartDate != value) {
            this.EntityPM.APInvoiceTransferStartDate = value;
        }
    }

    public get ARPaymentTransferStartDate() { return this.EntityPM.ARPaymentTransferStartDate }
    public set ARPaymentTransferStartDate(value: Date) {
        if (this.EntityPM.ARPaymentTransferStartDate != value) {
            this.EntityPM.ARPaymentTransferStartDate = value;
        }
    }

    CloseButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    private isOkButtonClicked: boolean = false;
    OkButtonClicked() {
        if (!this.isOkButtonClicked) {
            this.isOkButtonClicked = true;

            var errors: string[] = [];
            var myStartDate: Date = null;

            switch (this.Code) {
                case "ARInvoice": {
                    myStartDate = this.ARInvoiceTransferStartDate;
                    break;
                }

                case "APInvoice": {
                    myStartDate = this.APInvoiceTransferStartDate;
                    break;
                }

                case "ARPayment": {
                    myStartDate = this.ARPaymentTransferStartDate;
                    break;
                }
            }

            if (AppTool.IsNullOrEmpty(myStartDate)) {
                var msg: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");
                errors.push(msg.replace("%FieldName", "Start date"));
            }

            this.ValidationErrorsList = errors;

            if (errors.length == 0) {
                var myConfirmWindow = new ConfirmWindow();
                myConfirmWindow.Width = 400;
                myConfirmWindow.Show(this.GetConfirmMessage(myStartDate));

                myConfirmWindow.WindowClosed.subscribe((event: any) => {
                    if (myConfirmWindow.Yes) {
                        this.UpdateSystemStartDate(myStartDate);
                    }

                    else {
                        this.ReApplyOkButton();
                    }
                });
            }

            else {
                this.ReApplyOkButton();
            }
        }        
    }
    ReApplyOkButton() {
        this.isOkButtonClicked = false;
        this.CurrentSession.StopBusyIndicator();
    }
    GetConfirmMessage(myStartDate: Date) {
        var myResult: string = "";
        var myDateString: string = "";
        
        switch (this.Code) {
            case "ARInvoice":
                {
                    if (!AppTool.IsNullOrEmpty(this.ARInvoiceTransferStartDate)) {
                        myDateString = DateTool.GetDateFormats(this.ARInvoiceTransferStartDate).ShortDateString;
                    }

                    myResult = "Please note that all the AR invoices with an invoice date smaller than " + myDateString + ", will be updated and marked as blocked for transfer";
                    break;
                }

            case "APInvoice":
                {
                    if (!AppTool.IsNullOrEmpty(this.APInvoiceTransferStartDate)) {
                        myDateString = DateTool.GetDateFormats(this.APInvoiceTransferStartDate).ShortDateString;
                    }

                    myResult = "Please note that all the AP invoices with an invoice date smaller than " + myDateString + ", will be updated and marked as blocked for transfer";
                    break;
                }

            case "ARPayment":
                {
                    if (!AppTool.IsNullOrEmpty(this.ARPaymentTransferStartDate)) {
                        myDateString = DateTool.GetDateFormats(this.ARPaymentTransferStartDate).ShortDateString;
                    }

                    myResult = "Please note that all the AR payments with a register date smaller than " + myDateString + ", will be updated and marked as blocked for transfer";
                    break;
                }

            case "APPayment":
                {
                    myResult = "Please note that all the AP payments with a register date smaller than " + myDateString + ", will be updated and marked as blocked for transfer";
                    break;
                }
        }

        return myResult;
    }


    private stepCount: number = 10;
    private sentCount: number = 0;
    private allEntitiesCount: number = 0;
    private entitiesIdsList: string[] = [];
    public NoDataText: string = null;
    public UpdatedDataText: string = null;
    public IsNoDataTextVisible: boolean = false;
    public IsUpdateTextVisible: boolean = false;
    UpdateSystemStartDate(myStartDate: Date) {
        this.CurrentSession.StartBusyIndicator("Calculating data...");

        this.invoiceDomainService.SetAccountingSettingStartDate(this.Code, myStartDate).subscribe((myResponse1: ServiceResponse) => {
            if (myResponse1.HasError) {
                this.ValidationErrorsList = myResponse1.ErrorsArray;
                this.ReApplyOkButton();
            }

            else {
                this.Clone();

                this.invoiceDomainService.GetOnStartDateEntitiesIds(this.Code, myStartDate).subscribe((myResponse2: ServiceResponse) => {
                    this.sentCount = 0;
                    this.entitiesIdsList = [];
                    this.UpdatedDataText = "";
                    this.IsNoDataTextVisible = false;
                    this.IsUpdateTextVisible = false;

                    if (!myResponse2.HasError) {
                        this.entitiesIdsList = myResponse2.Result;
                    }

                    this.allEntitiesCount = this.entitiesIdsList.length;

                    if (this.allEntitiesCount == 0) {
                        this.NoDataText = "No " + this.pluralEntityName + " smaller than this date";
                        this.IsNoDataTextVisible = true;
                        this.ReApplyOkButton();
                    }

                    else {
                        this.IsUpdateTextVisible = true;
                        this.Blocking();
                    }
                });
            }
        });        
    }

    Blocking() {
        if (this.entitiesIdsList.length == 0) {
            if (this.sentCount == 1) {
                this.UpdatedDataText = "1 " + this.singleEntityName + " has been blocked";                
            }

            else {
                this.UpdatedDataText = this.sentCount + " " + this.pluralEntityName + " have been blocked";
            }

            this.ReApplyOkButton();
        }

        else {
            var idsList: string[] = [];

            this.entitiesIdsList.forEach(id => {
                if (idsList.length < this.stepCount) {
                    idsList.push(id);
                }
            });

            idsList.forEach(id => {
                var indexOfId: number = this.entitiesIdsList.indexOf(id);
                if (indexOfId > -1) {
                    this.entitiesIdsList.splice(indexOfId, 1);
                }
            });

            this.sentCount = this.sentCount + idsList.length;

            this.CurrentSession.StartBusyIndicator("Blocking " + this.sentCount + " from " + this.allEntitiesCount + " " + this.pluralEntityName);

            this.invoiceDomainService.BlockTransferEntities(idsList, this.Code).subscribe((myResponse: ServiceResponse) => {
                this.Blocking();
            });
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('ARInvoiceTransferStartDate');
        this.myCloner.AddField('APInvoiceTransferStartDate');
        this.myCloner.AddField('ARPaymentTransferStartDate');
        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
