import { Component, ViewChild, ViewContainerRef, ChangeDetectorRef } from '@angular/core';
import { ARInvoiceStockPM } from '../../../../Invoice/EntityPMs/ARInvoiceStockPM';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ARInvoiceStockInputTemplate } from '../ARInvoiceStockInputTemplate';
import { InvoiceStockInputArgs } from '../../../../Invoice/Args';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    templateUrl: './ARInvoiceStockGeneralTabComponent.html',
})

export class ARInvoiceStockGeneralTabComponent {
    public EntityPM: ARInvoiceStockPM = new ARInvoiceStockPM();
    public ObjectTableName: string = "ARInvoiceStock";

    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    constructor(public entityArgs: EntityArgs, private CD: ChangeDetectorRef) {        
        this.EntityPM = entityArgs.EntityPM;
        
        this.RunComponent();
        this.Listen();
    }

    private Listen() {
        if (SessionLocator.CurrentSession.CurrentEditComponent != null) {
            SessionLocator.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                    this.StockInputTemplate.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                }
            });

            SessionLocator.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                    this.StockInputTemplate.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                }
            });
        }
    }

    RunComponent() {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }

        else {
            this.RunComponentTimer();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 3) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    private StockInputTemplate: ARInvoiceStockInputTemplate;
    LoadChildComponent() {
        SessionLocator.DynamicLoader.Load("./InvoiceModules/InvoiceStocks/Components/ARInvoiceStockInputTemplate", this.viewContainerRef)
            .then(cmpRef => {
                this.StockInputTemplate = cmpRef.instance;
                var args = new InvoiceStockInputArgs();
                args.Stock = this.EntityPM;
                args.IsEditMode = true;
                this.StockInputTemplate.InitTemplate(args);                
            });
    }
}
