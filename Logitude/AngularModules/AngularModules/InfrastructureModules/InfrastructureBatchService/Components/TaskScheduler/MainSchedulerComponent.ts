

import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {LocationDirective} from '../../../../Infrastructure/Utilities/LocationDirective';

import {Component, OnInit, ChangeDetectorRef, QueryList, ViewChildren}  from '@angular/core';
@Component({
    moduleId: module.id,
    templateUrl: './MainSchedulerComponent.html',
})





export class MainSchedulerComponent implements OnInit {

    IsShowTabUpdate: boolean = true;
    private PageChild_STASK: any = null;
    private PageChild_SFTP: any = null;

    
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    constructor() {
        this.RunComponent();
 
    }

    ngOnInit(


    ) {

    }


    SetSelectedItem() {
        this.SelectedTabCode = "STASK";

    }



    private isLoaderReady: boolean = false;
    RunComponent() {
        if (this.AllLocations) {

            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }

            else {
                this.isLoaderReady = true;
                this.SetSelectedItem();
            }
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







    CloseButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }





    private selectedTabCode: string;
    get SelectedTabCode() { return this.selectedTabCode; }
    set SelectedTabCode(newValue: string) {
        if (this.selectedTabCode != newValue) {
            this.selectedTabCode = newValue;
            this.SelectionChanged();
        }
    }


    SelectionChanged() {
        if (this.isLoaderReady) {
            if (this.SelectedTabCode != null) {

                let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedTabCode)[0];
                if (myLocation != null) {

                    switch (this.SelectedTabCode) {
                           //Task
                        case "STASK": {
                            if (this.PageChild_STASK == null) {
                                SessionLocator.DynamicLoader.Load('./InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/TaskSchedulerComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.PageChild_STASK = cmpRef.instance;
                                        this.PageChild_STASK.LoadData("Task");
                                    });
                            }

                            break;
                        }
                            //SFTP
                        case "SFTP": {
                            if (this.PageChild_SFTP == null) {
                                SessionLocator.DynamicLoader.Load('./InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/TaskSchedulerComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.PageChild_SFTP = cmpRef.instance;
                                        this.PageChild_SFTP.LoadData("FTP");
                                    });
                            }

                            break;
                        }


                    }
                }
            }
        }
    }

  

}

