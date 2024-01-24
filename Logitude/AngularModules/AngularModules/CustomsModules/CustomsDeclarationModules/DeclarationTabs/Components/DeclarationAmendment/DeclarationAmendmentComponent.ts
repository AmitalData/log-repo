//import { Component } from "@angular/core";
//import { BaseComponent } from "../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent";
//import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
//import { ObservableCollection } from "../../../../../Infrastructure/Utilities/ObservableCollection";
//import { ServiceResponse } from "../../../../../Infrastructure/DataContracts/ServiceResponse";
//import { SessionLocator } from "../../../../../Infrastructure/Utilities/SessionLocator";
import { DeclarationExtendedListService } from "../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService";
//import { EntityArgs } from "../../../../../Infrastructure/DataContracts/EntityArgs";
//import { DeclarationPM } from "../../../../../Customs/EntityPMs/DeclarationPM";




 import { Component, OnInit, OnDestroy, EventEmitter, Output } from '@angular/core';
import { AppTool, ArrayTool } from '../../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { DeclarationWebService } from '../../../../../Customs/Services/WebServices/DeclarationWebService';
import { TapagPMService } from '../../../../../Customs/Services/StandardPMs/TapagPMService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';;
import { CustomsCollateralPM } from '../../../../../Customs/EntityPMs/CustomsCollateralPM';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { CustomsCollateralPMService } from '../../../../../Customs/Services/StandardPMs/CustomsCollateralPMService';
import { EntityListService } from '../../../../../Infrastructure/Services/EntityListService';
import { ApiQueryFilters } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { CustomsCollateralList } from '../../../../../Customs/EntityLists/CustomsCollateralList';
import { CustomsCollateralAnswerSharedDataService } from '../../../../../Customs/Services/DataChange/CustomsCollateralAnswerSharedDataService'
import { GenericRequestParams } from "../../../../../Customs/DataContract/RequestParams/GenericRequestParams";
import { SendRequestVIA } from "../../../../../Customs/DataContract/RequestParams/RequestParamsBase";
 import { DeclarationEventManager } from "../../../../../Customs/Utilities/DeclarationEventManager";
import { DeclarationAmendmentSharedDataService } from "../../../../../Customs/Services/DataChange/DeclarationAmendmentSharedDataService";

@Component({    
    templateUrl: './DeclarationAmendmentComponent.html',
    providers: [DeclarationExtendedListService, DeclarationWebService, DeclarationAmendmentSharedDataService]

})

export class DeclarationAmendmentComponent extends BaseComponent implements OnInit  {

 
    public amendmentObslist: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    private customFileNo: string;
    public EntityPM: DeclarationPM = null;
    public declarations: DeclarationPM[];
    IsLoaded: boolean;
    id: string;
    public columns: any[] = null;
    @Output() MenuHeaderchangeevent = new EventEmitter();
    @Output() onQueryChangeEvent = new EventEmitter();
    filterAgrs: ApiQueryFilters;
    private _entityListService: EntityListService = new EntityListService();
    _stratSearch: boolean = true;
     //CanOpenNewAmendment: boolean;
    public ObjectTableName: string = "Customs.Declaration";
    public DataContext: DeclarationAmendmentComponent = this;
    DeclarationAmendmentCancelledEVENT: any;
    public CurrentEditComponentId: string;
   // declarationAmendmentSharedDataService: DeclarationAmendmentSharedDataService = new DeclarationAmendmentSharedDataService();

    constructor(private EntityResourceService: EntityResourceService, public declarationExtendedListService: DeclarationExtendedListService,public declarationAmendmentSharedDataService: DeclarationAmendmentSharedDataService,
        private entityArgs: EntityArgs, private _declarationWebService: DeclarationWebService) {
        super();
        debugger;
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
                this.EntityPM = this.entityArgs.EntityPM;
                this.id = this.EntityPM.Id;
                if (this.EntityPM.Direction == "E")
                    this.declarationAmendmentSharedDataService.CanOpenNewAmendment = (this.EntityPM.IsSubmitDeclaration == true && this.EntityPM.AmendmentDontDisplayInList == false);
                else
                    this.declarationAmendmentSharedDataService.CanOpenNewAmendment = (this.EntityPM.PaymentDate != null && this.EntityPM.AmendmentDontDisplayInList == false);

                this.LoadDeclarationAmendmentsList();
                 this.BuildColumns();
                this.Listen();
 
 
            });

    }
    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
 

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.ngOnInit();
                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        this.ngOnInit();
                       // if (tabCode =="DCDA")
                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                DeclarationEventManager.DeclarationAmendmentCancelled.subscribe(data => {
                    this.ngOnInit();
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();



                }  ));
        }
    }

    ngOnInit(): void {
 
        setTimeout(() => {
            this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
        }, 10);
        if (this.EntityPM.Direction == "E")
            this.declarationAmendmentSharedDataService.CanOpenNewAmendment = (this.EntityPM.IsSubmitDeclaration == true && this.EntityPM.AmendmentDontDisplayInList == false);
        else
            this.declarationAmendmentSharedDataService.CanOpenNewAmendment = (this.EntityPM.PaymentDate != null && this.EntityPM.AmendmentDontDisplayInList == false);
        this.LoadDeclarationAmendmentsList();


        this.IsLoaded = true;
    }


 



    DataSource = {
        
        pageSize: 30,
        rowCount: null,
         sortingCol: "SignerName",
        sortingDir: "Descending",
        
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {

            var tempo = this.getRows(skip, take, sortingCol, sortingDir, false, searchFields, filters);
            return tempo;

        },
    };


    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {

 
        if (filters == null) {
            filters = new ApiQueryFilters();
        }

        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;

        filters.SortBy = "";//"Id";
        filters.SortDirection =  "Descending";
        getCount = false;
        filters.addAdditionalFilter("Id", this.id, null, null, "Equals", false, false, false, "string");
           return    this._entityListService.getExtendedByFilters("Customs.Declaration", filters);
 
 
    }

 

    BuildColumns() {
        this.columns = [];
        this.columns.push({

            FieldName: 'AmendmentNumber',
            DataTypeCode: 'String',//'Number',
            Display:"#",
            Styles: { width: '55px' },
            IsCustomTemplate: true
            , ServerSideSortable: true,
         });

        this.columns.push({

            FieldName: 'AmendmentRequestNumber',
            DataTypeCode: 'String',//'Number',
            Display: TextCodeTranslator.Translate("Customs.Declaration.F.AmendmentRequestNumber"),
            Styles: { width: '90px' },
            IsCustomTemplate: true
            , ServerSideSortable: true,
            SortByName: 'AmendmentRequestNumber'
        });

        this.columns.push({

            FieldName: 'DeclarationVersionId',
            DataTypeCode: 'String',//'Number',
            Display: TextCodeTranslator.Translate("Customs.Declaration.F.VersionId"),
            Styles: { width: '120px' },
            IsCustomTemplate: true
            , ServerSideSortable: true,
        });

        if (this.EntityPM.Direction == "E")
        {
            this.columns.push({

                FieldName: 'AmendmentTypeName',
                DataTypeCode: 'String',
                Display: TextCodeTranslator.Translate("Customs.Declaration.F.AmendmentTypeName"),
                Styles: { width: '120px' },
                IsCustomTemplate: true,
                ServerSideSortable: true,
            });
        }

        this.columns.push({

            FieldName: 'AmendmentCorrectedByUserName',
            DataTypeCode: 'String',//'Number',
            Display: this.EntityPM.Direction == "E" ? TextCodeTranslator.Translate("Customs.Declaration.O.CorrectedByUserName") : TextCodeTranslator.Translate("Customs.Declaration.F.AmendmentCorrectedByUserName"),
            Styles: { width: '80px' },
            IsCustomTemplate: true
            , ServerSideSortable: true,
         });

        this.columns.push({

            FieldName: 'AmendmentStatusName',
            DataTypeCode: 'String',//'Number',
            Display: TextCodeTranslator.Translate("Customs.Declaration.F.AmendmentStatusName"),
            Styles: { width: '140px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            HtmlListComponentName: 'DeclarationAmendmentListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationAmendmentListTemplate',

         });
        this.columns.push({
            FieldName: "Edit",
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '35px' },
            //IsCheckBox: true,
            HtmlListComponentName: 'DeclarationAmendmentListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationAmendmentListTemplate',
        });
        this.columns.push({
            FieldName: "ChangeAmendment",
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '135px' },
            //IsCheckBox: true,
            HtmlListComponentName: 'DeclarationAmendmentListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationAmendmentListTemplate',
        });
        this.columns.push({
            FieldName: "CopyAmendment",
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '135px' },
            //IsCheckBox: true,
            HtmlListComponentName: 'DeclarationAmendmentListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationAmendmentListTemplate',
        });
        this.columns.push({
            FieldName: "Delete",
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '35px' },
            //IsCheckBox: true,
            HtmlListComponentName: 'DeclarationAmendmentListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationAmendmentListTemplate',
        });
    }
    private LoadDeclarationAmendmentsList() {
        this.amendmentObslist = new ObservableCollection([]);
        this.CurrentSession.StartBusyIndicator("Loading...");

        this.declarationExtendedListService.GetDeclarationAmendmentsById(this.id)
            .subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                this.GetDeclarationAmendmentsListsOp_Completed(myResponse, false);
    
            });
    }

    private GetDeclarationAmendmentsListsOp_Completed(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        if (myResponse.Result != null && myResponse.Result.length > 0) {
            let i: number = 1;
            myResponse.Result.forEach((item) => {
          
                item.LineNumber = i;
                i++;
                if (this.EntityPM.Direction == "E")
                {
                    if ((item.AmendmentStatus == "6" || item.AmendmentStatus == null) && item.IsAmendment)
                    {
                        this.declarationAmendmentSharedDataService.CanOpenNewAmendment = false;
                    }
                }
                else
                {
                    if ((item.AmendmentStatus == "1" || item.AmendmentStatus == "2" || item.AmendmentStatus == null) && item.IsAmendment)
                    {
                        this.declarationAmendmentSharedDataService.CanOpenNewAmendment = false;
                    }
                }
                
                 this.amendmentObslist.Insert(item);
            });
             
        }
    }

    public OnOpenNewAmendmentClicked() {

        // in import, for the first amendment if there is a hatara date but no payment date, show a warning
        if (this.EntityPM.Direction != "E" && this.amendmentObslist.Length == 0 && this.EntityPM.HatraDate && !this.EntityPM.PaymentDate) {
            var messageWindow = new MessageWindow();
            messageWindow.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
            messageWindow.Width = 250;
            messageWindow.Height = 150;
            messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            messageWindow.Show(TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationReconstructionRequired"));
        }
        else {
            this.OpenNewAmendment(null, null);
        }
    }


    public OpenNewAmendment(id, declarationNumber, copy: boolean =false) {

        if (id == null) id = this.EntityPM.Id;//  !AppTool.IsNullOrEmpty(this.EntityPM.AmendmentOriginalDeclartation) ? this.EntityPM.AmendmentOriginalDeclartation :  this.EntityPM.Id;
        if (declarationNumber == null) declarationNumber = this.EntityPM.DeclarationNumber;

        var searchParams: GenericRequestParams = new GenericRequestParams();
        searchParams.Tenant = SessionLocator.Tenant;
        searchParams.AppicationId = id;
        searchParams.LoggingEnabled = true;
        searchParams.LoggingEntityId = id;
        searchParams.LoggingEntityReference = declarationNumber;
        searchParams.LoggingObjectTableId =  this.ObjectTableName;
        searchParams.LoggingUserId = SessionLocator.LoggedUserId;
        if (this.EntityPM.Direction == "E")
            searchParams.RequestName = "Export Declaration Request";
        else
            searchParams.RequestName = "Declaration Request";
        searchParams.ResponseName = "Declaration Response";
        searchParams.RequestVIA = SendRequestVIA.DCABatch;
        searchParams.ForcePersonalSign = false;
        searchParams.LoggingEntityId2 = copy==true ? "True" : "False";
         this.CurrentSession.StartBusyIndicatorCreating();

        this._declarationWebService
            .GetNewAmendmentDeclaration(searchParams)
            .subscribe((response: any) => {

                if (response) {
                    if (!response.HasError) {
                        var entity = response.Result;
                        if (entity != null) {
                             this.LoadDeclarationAmendmentsList();
                            setTimeout(() => {
                                this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
                            }, 10);
                            this.CurrentSession.StopBusyIndicator();
                            
                            this.openNewDeclaration(entity.Id);
                        }
                    }
                    
                    this.CurrentSession.StopBusyIndicator();
                    new MessageWindow().Show(response?.Result || TextCodeTranslator.Translate('General.O.ErrorwhileCreating'));   
                }
        });
    }

 
    OnRowSelected(event) {

        
        var selected = event.rowData.Id;
         if (selected) {

            this.openNewDeclaration(selected);

        }
    }

    openNewDeclaration(id:string) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityId: id, ObjectTableName: 'Customs.Declaration', BackButtonLabel: "תיקוני הצהרה" });
                cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                    DeclarationEventManager.DeclarationAmendmentCancelled.emit(null);

                    if (SessionLocator.SelectedSession != null && SessionLocator.SelectedSession.CurrentWindow != null) {
                        SessionLocator.SelectedSession.CurrentWindow.SuppressBusyIndicator = false;
                    }
                });

            });
    }

}
