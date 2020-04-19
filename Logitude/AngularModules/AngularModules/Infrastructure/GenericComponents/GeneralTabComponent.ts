import { Component, AfterViewInit, ViewChild } from '@angular/core';
import {EntityArgs} from '../DataContracts/EntityArgs';
import {SessionLocator} from '../Utilities/SessionLocator';
import { ChildDirective } from '../Directives/ChildDirective';

@Component({

  templateUrl: './GeneralTabComponent.html',
})

export class GeneralTabComponent implements AfterViewInit {
  private ObjectTableName: string = null;
  //@ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;

  @ViewChild(ChildDirective) Child: ChildDirective;

  IsNewEntity: boolean = false;
  constructor(private entityArgs: EntityArgs) {
    this.IsNewEntity = entityArgs.IsNewEntity;
    this.ObjectTableName = entityArgs.ObjectTableName;
    //this.RunComponent();
  }

  ngAfterViewInit() {
    SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.Child.Location)
      .then(cmpRef => {

        var ScreenCode = this.ObjectTableName + ".GeneralTabScreen";

        if (this.ObjectTableName == "Shipment" || this.ObjectTableName == "Master") {
          if (this.entityArgs.EntityPM.ShipmentLevelCode == "C") {
            ScreenCode = "Master.GeneralTabScreen";
          }
        }
        if (this.ObjectTableName == "BatchTaskExecution") {
          ScreenCode = "BatchTaskExecutionGeneralTabScreen";
        }
        cmpRef.instance.EntityArgs = this.entityArgs;
        cmpRef.instance.Run(this.entityArgs.EntityPM, this.ObjectTableName, ScreenCode, this.IsNewEntity);
      });
  }

  //RunComponent() {
  //    if (this.viewContainerRef) {
  //        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
  //            .then(cmpRef => {

  //                var ScreenCode = this.ObjectTableName + ".GeneralTabScreen";

  //                if (this.ObjectTableName == "Shipment" || this.ObjectTableName == "Master") {
  //                    if (this.entityArgs.EntityPM.ShipmentLevelCode == "C") {
  //                        ScreenCode = "Master.GeneralTabScreen";
  //                    }
  //                }
  //                if (this.ObjectTableName == "BatchTaskExecution") {
  //                    ScreenCode = "BatchTaskExecutionGeneralTabScreen";
  //                }
  //                cmpRef.instance.EntityArgs = this.entityArgs;
  //                cmpRef.instance.Run(this.entityArgs.EntityPM, this.ObjectTableName, ScreenCode, this.IsNewEntity);
  //            });
  //    }

  //    else {
  //        this.RunComponentTimer();
  //    }
  //}

  //private Retries: number = 0;
  //private timerToken: any;
  //private RunComponentTimer() {
  //    this.Retries++;

  //    if (this.timerToken) {
  //        clearTimeout(this.timerToken);
  //    }

  //    if (this.Retries < 20) {
  //        this.timerToken = setTimeout(() => this.RunComponent(), 1);
  //    }
  //}
}
