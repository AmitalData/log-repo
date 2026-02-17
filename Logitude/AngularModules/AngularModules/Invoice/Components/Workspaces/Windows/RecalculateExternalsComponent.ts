import {Component} from '@angular/core';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {InvoiceDomainService} from '../../../Services/InvoiceDomainService';

@Component({
    moduleId: module.id,
    templateUrl: './RecalculateExternalsComponent.html',
})

export class RecalculateExternalsComponent {
    private invoiceDomainService: InvoiceDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.invoiceDomainService = new InvoiceDomainService();
    }

    private entityCode: string = null;
    private stepCount: number = 10;
    private sentCount: number = 0;
    private allEntitiesCount: number = 0;
    private entitiesIdsList: string[] = [];
    SetWindowArgs(args: string) {
        this.entityCode = args;
        this.LoadData();
    }

    public CountText: string = "";
    public RecalculateButtonIsEnabled: boolean = false;
    LoadData() {
        this.sentCount = 0;
        this.entitiesIdsList = [];

        if (this.entityCode == "ARInvoice") {
            this.invoiceDomainService.GetNotReadyARInvoicesIds().subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.OnLoadDataCompleted(myResponse.Result, " Invoices");
                }
            });
        }

        else if (this.entityCode == "APInvoice") {
            this.invoiceDomainService.GetNotReadyAPInvoicesIds().subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.OnLoadDataCompleted(myResponse.Result, " Invoices");
                }
            });
        }
        else if (this.entityCode == "ARPayment") {
            this.invoiceDomainService.GetNotReadyARPaymentsIds().subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.OnLoadDataCompleted(myResponse.Result, " Payments");
                }
            });
        }
        else if (this.entityCode == "APPayment") {
            this.invoiceDomainService.GetNotReadyAPPaymentsIds().subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.OnLoadDataCompleted(myResponse.Result, " Payments");
                }
            });
        }
    }
    OnLoadDataCompleted(ids: string[], text:string) {
        this.entitiesIdsList = ids;
        this.allEntitiesCount = this.entitiesIdsList.length;
        this.CountText = this.allEntitiesCount + text;
        this.RecalculateButtonIsEnabled = this.entitiesIdsList.length > 0 ? true : false;
    }

    private isRecalculateButtonClicked: boolean = false;
    RecalculateClicked() {
        if (!this.isRecalculateButtonClicked) {
            this.Recalculate();
            this.isRecalculateButtonClicked = true;
        }
    }

    Recalculate() {
        if (this.entitiesIdsList.length == 0) {
            this.isRecalculateButtonClicked = false;
            this.CurrentSession.StopBusyIndicator();
            this.LoadData();
            this.CurrentSession.FireEvent("Accounting_T");
        }

        else {
            var idsList: string[] = [];

            this.entitiesIdsList.forEach(id => {
                if (idsList.length < this.stepCount) {
                    idsList.push(id);
                }
            });

            idsList.forEach(id => {
                var indexOfId = this.entitiesIdsList.indexOf(id);
                if (indexOfId > -1) {
                    this.entitiesIdsList.splice(indexOfId, 1);
                }
            });

            this.sentCount = this.sentCount + idsList.length;

            this.CurrentSession.StartBusyIndicator("Recalculating " + this.sentCount + " from " + this.allEntitiesCount);

            this.invoiceDomainService.GetRecalculateTransfer(idsList, this.entityCode).subscribe((myResponse: ServiceResponse) => {
                this.Recalculate();
            });
        }
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
