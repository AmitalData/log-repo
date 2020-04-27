import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { ARInvoiceStockPM } from '../../../../Invoice/EntityPMs/ARInvoiceStockPM';
import { ARInvoiceStockPMInitService } from '../../../../Invoice/EntityPMInitServices/ARInvoiceStockPMInitService';
import { ARInvoiceStockPMService } from '../../../../Invoice/Services/StandardPMs/ARInvoiceStockPMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';
import { ARInvoiceStockInputTemplate } from '../ARInvoiceStockInputTemplate';
import { InvoiceStockInputArgs } from '../../../../Invoice/Args';
import { ChildDirective } from '../../../../Infrastructure/Directives/ChildDirective';

@Component({

  templateUrl: './NewARInvoiceStockComponent.html',
})

export class NewARInvoiceStockComponent implements AfterViewInit {
  public DataContext: NewARInvoiceStockComponent = this;
  public ObjectTableName: string = "ARInvoiceStock";
  public ValidationErrorsList: string[] = [];
  public EntityPM: ARInvoiceStockPM;
  private stockPMService: ARInvoiceStockPMService;
  private CurrentSession = SessionLocator.SelectedSession;
  @ViewChild(ChildDirective) Child: ChildDirective;
  constructor() {
    this.stockPMService = new ARInvoiceStockPMService();
  }

  ngAfterViewInit() {
    this.LoadChildComponent();
  }

  private StockInputTemplate: ARInvoiceStockInputTemplate;
  LoadChildComponent() {
    SessionLocator.DynamicLoader.Load("./InvoiceModules/InvoiceStocks/Components/ARInvoiceStockInputTemplate", this.Child.Location)
      .then(cmpRef => {
        this.StockInputTemplate = cmpRef.instance;
        var args = new InvoiceStockInputArgs();
        args.Stock = this.EntityPM;
        args.IsEditMode = false;
        this.StockInputTemplate.InitTemplate(args);
      });
  }

  SetWindowArgs(args: InvoiceStockInputArgs) {
    if (args != null) {
      if (args.Stock != null) {
        this.EntityPM = args.Stock;
      }
      else {
        this.EntityPM = new ARInvoiceStockPM();
        ARInvoiceStockPMInitService.InitValues(this.EntityPM, true);
      }
    }
    else {
      this.EntityPM = new ARInvoiceStockPM();
      ARInvoiceStockPMInitService.InitValues(this.EntityPM, true);
    }

    this.Clone();
  }

  CancelClicked() {
    this.RejectChanges();
    this.CurrentSession.CloseCurrentWindow();
  }

  private myCloner: Cloner;
  private Clone() {
    this.myCloner = new Cloner(this.DataContext);
    this.myCloner.AddField('Name');
    this.myCloner.AddField('StartDate');
    this.myCloner.AddField('EndDate');
    this.myCloner.AddField('Description');
    this.myCloner.AddField('Notes');
    this.myCloner.AddEntity(this.EntityPM);
  }
  private RejectChanges() {
    this.myCloner.RejectChanges();
  }

  OkClicked() {
    var errors: string[] = [];
    Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

    if (this.EntityPM.StartDate > this.EntityPM.EndDate) {
      errors.push("Start Date cannot be greater than End Date");
    }

    this.ValidationErrorsList = errors;

    if (this.ValidationErrorsList.length == 0) {
      if (this.EntityPM.IsDirty) {
        this.CurrentSession.StartBusyIndicatorSaving();

        this.stockPMService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

          this.CurrentSession.StopBusyIndicator();

          if (!myResponse.HasError) {
            this.CurrentSession.CloseCurrentWindowEmit("OK");
          }

          else {
            this.ValidationErrorsList = myResponse.ErrorsArray;
          }
        });
      }

      else {
        this.CurrentSession.CloseCurrentWindowEmit("OK");
      }
    }
  }
}
