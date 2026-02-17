

import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {LocationDirective} from '../../../../Infrastructure/Utilities/LocationDirective';

import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';

import {Component, OnInit, ChangeDetectorRef, QueryList, ViewChildren}  from '@angular/core';
@Component({
    moduleId: module.id,
    templateUrl: './MainSchedulerComponent.html',
})





export class MainSchedulerComponent implements OnInit {

    IsShowTabUpdate: boolean = true;
    private PageChild_STASK: any = null;
    private PageChild_SFTP: any = null;


    IsShowTaskScheduler: boolean = false;
    IsShowTabFTBScheduler: boolean = false;

    IsShowComponentWithTabs: boolean = false;
    IsShowComponentWithOutTabs: boolean = false;
    IsShowPackageNotIncludeMessage: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    constructor() {
       
        if (FeatureLocator.HasFeaturePermession("TasksScheduler", "TASK")) this.IsShowTaskScheduler = true;
        if (FeatureLocator.HasFeaturePermession("TasksScheduler", "FTP")) this.IsShowTabFTBScheduler = true;
        if (!FeatureLocator.HasFeaturePermession("TasksScheduler", "READ")) this.IsShowPackageNotIncludeMessage = true;


        if (this.IsShowTaskScheduler && this.IsShowTabFTBScheduler) this.IsShowComponentWithTabs = true;
        else if (this.IsShowTaskScheduler || this.IsShowTabFTBScheduler) this.IsShowComponentWithOutTabs = true;
        else this.IsShowPackageNotIncludeMessage = true; 



        if ((this.IsShowTaskScheduler || this.IsShowTabFTBScheduler) && !this.IsShowPackageNotIncludeMessage ) {
            this.RunComponent();
        }
 
    }

    ngOnInit(


    ) {

    }


    SetSelectedItem(tabCode:string) {
        this.SelectedTabCode = tabCode;

    }



    private isLoaderReady: boolean = false;
    RunComponent() {
        if (this.AllLocations) {

            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }

            else {
                this.isLoaderReady = true;

                var tabCode = this.IsShowComponentWithTabs || this.IsShowTaskScheduler ? "STASK" : "SFTP";
                this.SetSelectedItem(tabCode);
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
        this.CurrentSession.CloseCurrentWindow();
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

