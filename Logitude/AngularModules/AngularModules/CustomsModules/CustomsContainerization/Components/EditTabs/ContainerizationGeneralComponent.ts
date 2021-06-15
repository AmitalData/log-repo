declare var window: any;
import { Component, AfterViewInit, ChangeDetectorRef, Output, EventEmitter, OnInit } from '@angular/core';
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
import { DeclarationList } from '../../../../Customs/EntityLists/DeclarationList';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DeclarationListService } from '../../../../Customs/Services/StandardLists/DeclarationListService';
import { DeclarationEventManager } from '../../../../Customs/Utilities/DeclarationEventManager';


@Component({

    templateUrl: './ContainerizationGeneralComponent.html',
})

export class ContainerizationGeneralComponent extends BaseComponent implements AfterViewInit {
    @Output() FillValidationErrorList: EventEmitter<any> = new EventEmitter();
    public EntityPM: ContainerizationPM;
    public ObjectTableName: string = "Customs.Containerization";
    public DataContext: any = this;
    public IsNewEntity: boolean = false;
    public ValidationErrorsList: any[];
    public SubCountryCodeEnabled: boolean = false;
    IsDelete: boolean = false;
    public CurrentEditComponentId: string;
    AddDeclarationToContainerizationEVENT: any;
    ;
    ResponseData: INF_MSG_GenericResponseData;


    public ContainerizationDeclarationList: ObservableCollection;
    
    private CurrentSession = SessionLocator.SelectedSession;
    IsDisplayOnly: boolean = false;
    visibile: boolean = true;
    private declarationListService: DeclarationListService = new DeclarationListService();
    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef, private EntityResourceService: EntityResourceService) {
        super();

        this.EntityResourceService.getEntityResourceByTableName("Customs.Containerization").subscribe((response: any) => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
                this.EntityPM = this.entityArgs.EntityPM;
                this.ObjectTableName = this.entityArgs.ObjectTableName;
                this.Listen();
                this.SetFieldsEditability();
                
                
                this.getRows(); 
            });
        });

    }

    // log tab
    selectedTab: LogTab;
    public get SelectedTab() { return this.selectedTab; }
    public set SelectedTab(tab: LogTab) {
        this.selectedTab = tab;
    }
    ngAfterViewInit() {
        //setTimeout(() => {
        //    let List: DeclarationList[] = [];
        //    let dec = new DeclarationList();
        //    dec.Id = "1";
        //    dec.DeclarationNumber = "11111";
        //    List.push(dec);
        //    dec = new DeclarationList();
        //    dec.Id = "2";
        //    dec.DeclarationNumber = "222";
        //    List.push(dec);
        //    this.ContainerizationDeclarationList.InsertCollection(List);
        //}, 2000)
        
        
    }

    getRows()//skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
    {

        let filters = new ApiQueryFilters();


        filters.PageSize = 200;
        filters.PageIndex = 0;
        filters.GetAll = false;
        filters.GetCount = true;
        //filters.SortBy = sortingCol;
        //filters.SortDirection = sortingDir;
        //Customs.Declaration.F.ExportContainerizationID
        filters.addAdditionalFilter("ExportContainerizationID", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        return this.declarationListService.getByFilters(filters)
            .subscribe(r => {
                this.ContainerizationDeclarationList = new ObservableCollection([]);
                this.ContainerizationDeclarationList.InsertCollection(r.Result);
            });

    }

    getRowsWithNewAddedDeclarations() {
        let filters = new ApiQueryFilters();
        filters.PageSize = 200;
        filters.PageIndex = 0;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.addAdditionalFilter("Id", this.EntityPM.ConnectedDeclarations, null, null, "InListExact", false, false, false, "string", this.EntityPM.ConnectedDeclarations.length == 0);
        return this.declarationListService.getByFilters(filters)
            .subscribe(r => {
                r.Result.forEach(element => this.ContainerizationDeclarationList.Insert(element));
                var e=this.ContainerizationDeclarationList;
            });
    }
    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.getRows(); 
                    }
                })
            );
            //this.CurrentSession.CurrentEditComponent.saveco
           // SessionLocator.SelectedSession.CurrentEditComponent
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



            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                DeclarationEventManager.AddDeclarationToContainerization.subscribe(data => {
                    this.getRowsWithNewAddedDeclarations();
                }));

        }
    }

    //public SetTabArgs(args: any, ValidationErrorsList: any[]) {
    //public SetTabArgs(args: any) {
    //    this.EntityPM = args.EntityPM;
    //    //this.IsNewEntity = args.IsNewEntity;

    //    console.log("EntityPM", this.EntityPM);
        

    //    //this.SetFieldsEditability();
    //}

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
    DeleteButtonClicked(item) {
        if (AppTool.IsNullOrEmpty(this.EntityPM.NotConnectedDeclarations)) {
            this.EntityPM.NotConnectedDeclarations = item.Id;
        } else {
            this.EntityPM.NotConnectedDeclarations += "," + item.Id;
        }
        
        this.ContainerizationDeclarationList.Remove(item);
    }
    EditButtonClicked(item) {

        //  this.EditEntity("Customs.Declaration", this.rowData.Id, null, "DEGC");

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.SelectedSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityId: item.Id,
                    ObjectTableName: "Customs.Declaration"
                });
            });


    }

    public SendButtonsVisibility: boolean = false;
    OnRowLoaded($event) {

    }


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

