//import { Component } from "@angular/core";
//import { BaseComponent } from "../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent";
//import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
//import { ObservableCollection } from "../../../../../Infrastructure/Utilities/ObservableCollection";
//import { ServiceResponse } from "../../../../../Infrastructure/DataContracts/ServiceResponse";
//import { SessionLocator } from "../../../../../Infrastructure/Utilities/SessionLocator";
import { DeclarationExtendedListService } from "../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService";
//import { EntityArgs } from "../../../../../Infrastructure/DataContracts/EntityArgs";
//import { DeclarationPM } from "../../../../../Customs/EntityPMs/DeclarationPM";




 import { Component, OnInit, OnDestroy, EventEmitter } from '@angular/core';
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

@Component({
    moduleId: module.id,
    templateUrl: './DeclarationAmendmentComponent.html',
    providers:[DeclarationExtendedListService]
})

export class DeclarationAmendmentComponent extends BaseComponent   {

    public amendmentObslist: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    private customFileNo: string;
    public EntityPM: DeclarationPM = null;
    public declarations: DeclarationPM[];
    IsLoaded: boolean;
    id: string;
    public columns: any[] = null;
    MenuHeaderchangeevent = new EventEmitter();
    filterAgrs: ApiQueryFilters;
    private _entityListService: EntityListService = new EntityListService();
    _stratSearch: boolean = true;


    constructor(private EntityResourceService: EntityResourceService, public declarationExtendedListService: DeclarationExtendedListService,
    private entityArgs: EntityArgs) {
        super();
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
                this.EntityPM = this.entityArgs.EntityPM;
                this.id = this.EntityPM.Id;
                this.BuildColumns();

                setTimeout(() => {
                    this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
                }, 10);


               // this.LoadDeclarationAmendmentsList();
            });

    }

    DataSource = {

        pageSize: 10,
        rowCount: null,
        //SortData("RequestCreateDate", "Descending", false, false);
        sortingCol: "",// "Id",
        sortingDir: "",//"Descending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {

            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
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

        filters.SortBy = "Id";
        filters.SortDirection = "Descending";

        filters.SortBy = "AmendmentRequestNumber";//"Id";
        filters.SortDirection = "Descending";//"Descending";



        let declarationId = this.EntityPM.Id;
     //   filters.addAdditionalFilter("DeclarationId", declarationId, null, null, "Equals", false, false, false, "string");

        /// filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");

        var myout = this.declarationExtendedListService.GetDeclarationAmendmentsById(this.id).subscribe();

            //this._entityListService
            //.getExtendedByFilters("Customs.CustomsDeclaration", filters);
        //myout.then(res => {
        //    this._stratSearch = false;
        //    //this.CurrentSession.StopBusyIndicator();
        //});

        return myout;

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

            FieldName: 'VersionId',
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
            IsCustomTemplate: true
            , ServerSideSortable: true,
         });



    }
    private LoadDeclarationAmendmentsList() {
        this.amendmentObslist = new ObservableCollection([]);
        this.CurrentSession.StartBusyIndicator("Loading...");

        this.declarationExtendedListService.GetDeclarationAmendmentsById(this.id)
            .subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
              //  this.declarations = myResponse.Result;
               // debugger;
                this.GetDeclarationAmendmentsListsOp_Completed(myResponse, false);
                this.IsLoaded = true;
               // this.TapagIdEdit();
            });
    }

    private GetDeclarationAmendmentsListsOp_Completed(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        if (myResponse.Result != null && myResponse.Result.length > 0) {
            let i: number = 1;
            myResponse.Result.forEach((item) => {
                item.LineNumber = i;
                i++;
                 this.amendmentObslist.Insert(item);
            });
             
        }
    }


}
