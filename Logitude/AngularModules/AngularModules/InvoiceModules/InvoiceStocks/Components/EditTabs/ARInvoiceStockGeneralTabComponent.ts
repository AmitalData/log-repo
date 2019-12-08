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
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private CD: ChangeDetectorRef) {        
        this.EntityPM = entityArgs.EntityPM;
        
        this.RunComponent();
        this.Listen();
    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.StockInputTemplate.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            });

            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.StockInputTemplate.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
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

        if (this.Retries < 20) {
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
