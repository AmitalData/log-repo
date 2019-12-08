

declare var System: any;
declare var window: any;
import {Component, OnInit, ViewChildren, QueryList} from '@angular/core';
import {LocationDirective} from '../../../../Infrastructure/Utilities/LocationDirective';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';

import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

import {AppTool} from '../../../../Infrastructure/Tools';
@Component({
    selector: 'WarehouseEntryPackagesTabComponent',
    moduleId: module.id,
    templateUrl: './WarehouseEntryPackagesTabComponent.html',

})


export class WarehouseEntryPackagesTabComponent implements OnInit {
    DataContext: any = this;


    public warehouseEntryPM: any;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs)
     {

    }

    ngOnInit() {
        this.warehouseEntryPM = this.entityArgs.EntityPM;
        this.Listen();
        if (this.warehouseEntryPM) {
            this.RunComponent();
        }



    }

    LoadCompletedEvent: any;
    SaveCompletedEvent: any;
    Listen() {

        if (this.CurrentSession.CurrentEditComponent != null) {

            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.warehouseEntryPM = this.CurrentSession.CurrentEditComponent.EntityPM;

                        if (this.WarehouseEntryPackagesDetailsComponent) {
                            var windowArgs: any = { WarehouseEntryPM: this.warehouseEntryPM, ViewModelTrigger: this, IsFromShipment: true, IsEditMode: true };
                            this.WarehouseEntryPackagesDetailsComponent.ReloadComponent(windowArgs);
                        }
                    }

                });
            }   

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.warehouseEntryPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }

        }
    }


    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);

    }



    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private timerToken: any;
    private Retries: number = 0;
    private GeneratedComponent: any;

    RunComponent() {
        if (this.AllLocations) {

            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }

            else {
                this.LoadChildComponent();
            }
        }

        else {
            this.RunComponentTimer();
        }
    }


    RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }


    WarehouseEntryPackagesDetailsComponent: any;
    LoadChildComponent() {

        let warehouseEntryPackagesDetailsComponenttLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == "WEPD")[0];
        if (warehouseEntryPackagesDetailsComponenttLocation != null) {
            SessionLocator.DynamicLoader.Load('./Warehouse/Components/WarehouseEntryPackagesDetailsComponent', warehouseEntryPackagesDetailsComponenttLocation.viewContainerRef)
                .then(cmpRef => {

                    this.WarehouseEntryPackagesDetailsComponent = cmpRef.instance;
                    var windowArgs: any = { WarehouseEntryPM: this.warehouseEntryPM, ViewModelTrigger: this, IsEditMode: true, ShowAddPackageButton:true, ShowPackageSummary:true};
                    cmpRef.instance.SetWindowArgs(windowArgs);


                 //   ShowAddPackageButton


                });

        }



    }



}
