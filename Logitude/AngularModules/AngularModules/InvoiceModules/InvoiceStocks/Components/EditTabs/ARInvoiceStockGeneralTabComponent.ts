import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { ARInvoiceStockPM } from '../../../../Invoice/EntityPMs/ARInvoiceStockPM';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ARInvoiceStockInputTemplate } from '../ARInvoiceStockInputTemplate';
import { InvoiceStockInputArgs } from '../../../../Invoice/Args';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ChildDirective } from '../../../../Infrastructure/Directives/ChildDirective';

@Component({

  templateUrl: './ARInvoiceStockGeneralTabComponent.html',
})

export class ARInvoiceStockGeneralTabComponent implements AfterViewInit {
  public EntityPM: ARInvoiceStockPM = new ARInvoiceStockPM();
  public ObjectTableName: string = "ARInvoiceStock";
  @ViewChild(ChildDirective) Child: ChildDirective;
  private CurrentSession = SessionLocator.SelectedSession;
  constructor(public entityArgs: EntityArgs) {
    this.EntityPM = entityArgs.EntityPM;
    this.Listen();
  }

  ngAfterViewInit() {
    this.LoadChildComponent();
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

  private StockInputTemplate: ARInvoiceStockInputTemplate;
  LoadChildComponent() {
    SessionLocator.DynamicLoader.Load("./InvoiceModules/InvoiceStocks/Components/ARInvoiceStockInputTemplate", this.Child.Location)
      .then(cmpRef => {
        this.StockInputTemplate = cmpRef.instance;
        var args = new InvoiceStockInputArgs();
        args.Stock = this.EntityPM;
        args.IsEditMode = true;
        this.StockInputTemplate.InitTemplate(args);
      });
  }
}
