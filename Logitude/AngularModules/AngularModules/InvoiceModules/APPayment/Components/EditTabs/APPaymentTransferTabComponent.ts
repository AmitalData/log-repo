import { Component, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {APPaymentPM} from '../../../../Invoice/EntityPMs/APPaymentPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {APPaymentTransferTemplate} from '../NewEntity/APPaymentTransferTemplate';
import {AppTool} from '../../../../Infrastructure/Tools';
import { ChildDirective } from '../../../../Infrastructure/Directives/ChildDirective';

@Component({
  templateUrl: './APPaymentTransferTabComponent.html',
})

export class APPaymentTransferTabComponent extends BaseComponent implements AfterViewInit, OnDestroy {
  public EntityPM: APPaymentPM = null;
  public ObjectTableName = "APPayment";
  public DataContext = this;
  @ViewChild(ChildDirective) Child: ChildDirective;
  constructor(private entityArgs: EntityArgs) {
    super();
    this.EntityPM = entityArgs.EntityPM;
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
            this.InputTemplate.EntityPM = this.EntityPM
            this.InputTemplate.BuildList();
          }
        }
      });

      this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
        if (isLoadSuccess) {
          this.EntityPM = this.entityArgs.EditComponent.EntityPM;

          if (this.InputTemplate) {
            this.InputTemplate.EntityPM = this.EntityPM
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

  private InputTemplate: APPaymentTransferTemplate;
  LoadChildComponent() {
    SessionLocator.DynamicLoader.Load("./InvoiceModules/APPayment/Components/NewEntity/APPaymentTransferTemplate", this.Child.Location)
      .then(cmpRef => {
        this.InputTemplate = cmpRef.instance;
        this.InputTemplate.InitTemplate(this.EntityPM);
      });
  }
}
