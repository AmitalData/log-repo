import {Component, ViewChild, AfterViewInit, OnDestroy}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ARInvoicePM} from '../../../../Invoice/EntityPMs/ARInvoicePM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ARInvoiceTransferTemplate} from '../NewEntity/ARInvoiceTransferTemplate';
import {AppTool} from '../../../../Infrastructure/Tools';
import { ChildDirective } from '../../../../Infrastructure/Directives/ChildDirective';

@Component({
  templateUrl: './ARInvoiceTransferTabComponent.html',
})

export class ARInvoiceTransferTabComponent extends BaseComponent implements AfterViewInit, OnDestroy {
  public EntityPM: ARInvoicePM = null;
  public ObjectTableName = "ARInvoice";
  public DataContext = this;
  public IsConstituentInvoice: boolean = false;
  @ViewChild(ChildDirective) Child: ChildDirective;
  constructor(private entityArgs: EntityArgs) {
    super();
    this.EntityPM = entityArgs.EntityPM;
    this.IsConstituentInvoice = this.EntityPM.IsConstituentInvoice;
    this.Listen();
  }

  ngAfterViewInit() {
    this.LoadChildComponent();
  }

  private SaveCompletedEvent: any = null;
  private LoadCompletedEvent: any = null;
  private Listen() {
    if (this.entityArgs.EditComponent != null) {

      this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
        if (isSaveSuccess) {
          this.EntityPM = this.entityArgs.EditComponent.EntityPM;

          if (this.InputTemplate) {
            this.InputTemplate.EntityPM = this.EntityPM;
            this.InputTemplate.BuildList();
          }
        }
      });

      this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
        if (isLoadSuccess) {
          this.EntityPM = this.entityArgs.EditComponent.EntityPM;

          if (this.InputTemplate) {
            this.InputTemplate.EntityPM = this.EntityPM;
            this.InputTemplate.BuildList();
          }
        }
      });
    }
  }
  ngOnDestroy() {
    AppTool.KillEventEmitter(this.SaveCompletedEvent);
    AppTool.KillEventEmitter(this.LoadCompletedEvent);
  }

  private InputTemplate: ARInvoiceTransferTemplate;
  LoadChildComponent() {
    if (!this.IsConstituentInvoice) {
      SessionLocator.DynamicLoader.Load("./InvoiceModules/ARInvoice/Components/NewEntity/ARInvoiceTransferTemplate", this.Child.Location)
        .then(cmpRef => {
          this.InputTemplate = cmpRef.instance;
          this.InputTemplate = cmpRef.instance;
          this.InputTemplate.InitTemplate(this.EntityPM);
        });
    }
  }
}
