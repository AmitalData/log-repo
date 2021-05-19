declare var window: any;
import { Component, AfterViewInit, ChangeDetectorRef, Output, EventEmitter } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { LogTab } from '../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { AppTool, ArrayTool } from '../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { Validator } from '../../../../Infrastructure/Validators/Validator';

import { ContainerizationPM } from '../../../../Customs/EntityPMs/ContainerizationPM';

// Send Request
import { INF_MSG_GenericResponseData } from '../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';

import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';

import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';

@Component({

    templateUrl: './ContainerizationGeneralComponent.html',
})

export class ContainerizationGeneralComponent extends BaseComponent {
    @Output() FillValidationErrorList: EventEmitter<any> = new EventEmitter();
    public EntityPM: ContainerizationPM;
    public ObjectTableName: string = "Customs.Containerization";
    public DataContext: any = this;
    public IsNewEntity: boolean = false;
    public ValidationErrorsList: any[];
    public SubCountryCodeEnabled: boolean = false;
    IsDelete: boolean = false;
    public CurrentEditComponentId: string;

    ResponseData: INF_MSG_GenericResponseData;


    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef, private EntityResourceService: EntityResourceService) {
        super();

        this.EntityResourceService.getEntityResourceByTableName("Customs.Containerization").subscribe((response: any) => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
                this.EntityPM = this.entityArgs.EntityPM;
                this.ObjectTableName = this.entityArgs.ObjectTableName;
                this.Listen();
                this.SetFieldsEditability();
            });
        });

    }

    // log tab
    selectedTab: LogTab;
    public get SelectedTab() { return this.selectedTab; }
    public set SelectedTab(tab: LogTab) {
        this.selectedTab = tab;
    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                })
            );
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        //this.RefreshEntity();
                    }
                })
            );
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "DEGC") {
                            this.RefreshEntity();
                        }
                    }
                })
            );
        }
    }

    //public SetTabArgs(args: any, ValidationErrorsList: any[]) {
    public SetTabArgs(args: any) {
        this.EntityPM = args.EntityPM;
        this.IsNewEntity = args.IsNewEntity;

        console.log("EntityPM", this.EntityPM);


        this.SetFieldsEditability();
    }

    RefreshEntity() {
        if (this.CurrentSession.CurrentEditComponent) {
            this.CurrentSession.CurrentEditComponent.EditComponentController.ResetMustRefresh();
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        }
        this.SetFieldsEditability();
    }
    SetFieldsEditability() {
        //throw new Error('Method not implemented.');
    }

   

    //#endregion



    ///#region Properties



    public SendButtonsVisibility: boolean = false;



    get ContainerizationDate() { return this.EntityPM != null ? this.EntityPM.ContainerizationDate:null; }
    set ContainerizationDate(value) { this.EntityPM.ContainerizationDate= value; }


    //#endregion

    line = 0;

    //#region Send + Delete
    SendButtonClicked() {
        var errors = [];
        this.FillValidationErrorList.emit(errors); // clear validation msgs

        // validate Containerization
        Validator.TryValidateObject(this.EntityPM, "Customs.Containerization", errors);

        

        if (errors.length > 0) {
            this.ValidationErrorsList = errors;
            this.FillValidationErrorList.emit(errors);
        } else {
            // send request
            //this.SendRequest(false);
        }

    }


   
    //#endregion
}

