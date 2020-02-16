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

@Component({
    moduleId: module.id,
    templateUrl: './DeclarationAmendmentComponent.html',
    providers: [DeclarationExtendedListService, DeclarationWebService]
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
     CanOpenNewAmendment: boolean;
    public ObjectTableName: string = "Customs.Declaration";
    public DataContext: DeclarationAmendmentComponent = this;

    constructor(private EntityResourceService: EntityResourceService, public declarationExtendedListService: DeclarationExtendedListService,
        private entityArgs: EntityArgs, private _declarationWebService: DeclarationWebService) {
        super();
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
                this.EntityPM = this.entityArgs.EntityPM;
                this.id = this.EntityPM.Id;
                this.CanOpenNewAmendment = (this.EntityPM.PaymentDate != null && this.EntityPM.AmendmentDontDisplayInList==false);
                this.LoadDeclarationAmendmentsList();
                 this.BuildColumns();


 
            });

    }

    ngOnInit(): void {
        setTimeout(() => {
            this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
        }, 10);
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

            FieldName: 'LineNumber',
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

        this.columns.push({

            FieldName: 'AmendmentCorrectedByUserName',
            DataTypeCode: 'String',//'Number',
            Display: TextCodeTranslator.Translate("Customs.Declaration.F.AmendmentCorrectedByUserName"),
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
                   if (item.AmendmentStatus == "1" || item.AmendmentStatus=="2" || item.AmendmentStatus == null)
                 this.CanOpenNewAmendment = false;
                 this.amendmentObslist.Insert(item);
            });
             
        }
    }


    public OpenNewAmendment() {


        var searchParams: GenericRequestParams = new GenericRequestParams();
        searchParams.Tenant = SessionLocator.Tenant;
        searchParams.AppicationId = this.EntityPM.Id;
        searchParams.LoggingEnabled = true;
        searchParams.LoggingEntityId = this.EntityPM.Id;
        searchParams.LoggingEntityReference = this.EntityPM.DeclarationNumber;
        searchParams.LoggingObjectTableId =  this.ObjectTableName;
        searchParams.LoggingUserId = SessionLocator.LoggedUserId;
        searchParams.RequestName = "Declaration Request";
        searchParams.ResponseName = "Declaration Response";
        searchParams.RequestVIA = SendRequestVIA.DCABatch;
        searchParams.ForcePersonalSign = false;
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
                            debugger;
                            this.openNewDeclaration(entity.Id);

                             }

                    }
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
                    if (SessionLocator.SelectedSession != null && SessionLocator.SelectedSession.CurrentWindow != null) {
                        SessionLocator.SelectedSession.CurrentWindow.SuppressBusyIndicator = false;
                    }
                });

            });
    }

}
