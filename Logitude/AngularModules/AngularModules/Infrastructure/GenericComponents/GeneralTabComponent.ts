import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { EntityArgs } from '../DataContracts/EntityArgs';
import { SessionLocator } from '../Utilities/SessionLocator';
import { ChildDirective } from '../Directives/ChildDirective';
import { FeatureLocator } from '../Utilities/FeatureLocator';

@Component({
    templateUrl: './GeneralTabComponent.html',
})

export class GeneralTabComponent implements AfterViewInit {
    IsNewEntity: boolean = false;
    private ObjectTableName: string = null;
    @ViewChild(ChildDirective) Child: ChildDirective;
    constructor(private entityArgs: EntityArgs) {
        this.IsNewEntity = entityArgs.IsNewEntity;
        this.ObjectTableName = entityArgs.ObjectTableName;
    }

    OPObjectTablesName = ["ChargesGroup"];

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
                if (FeatureLocator.HasFeaturePermession("QuoteOP", "QuoteOPMaintence") && this.OPObjectTablesName.includes(this.ObjectTableName)){
                    var ScreenCode = this.ObjectTableName +"OP"+ ".GeneralTabScreen";
                }
                cmpRef.instance.EntityArgs = this.entityArgs;
                cmpRef.instance.Run(this.entityArgs.EntityPM, this.ObjectTableName, ScreenCode, this.IsNewEntity);
            });
    }
}

