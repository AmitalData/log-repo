declare var window: any;
import {Observable}     from 'rxjs/Rx';
import { Component, Output, EventEmitter, OnInit, ComponentRef } from '@angular/core';
import { BaseComponent } from       '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TextCodeTranslator } from  '../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { AppTool } from '../../../Infrastructure/Tools';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ListComponentArgs } from '../../../Infrastructure/Args';

import { ApiQueryFilters } from  '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from  '../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityListService } from   '../../../Infrastructure/Services/EntityListService';
import { SignStationExtendedListService, SignStationList, SignStationGroup} from   '../../../Customs/Services/ExtendedLists/SignStationExtendedListService';
   



import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';


@Component({
    moduleId: module.id,
    templateUrl: './SignStationsComponent.html',
})





export class SignStationsComponent
    extends BaseComponent
    implements OnInit {

    public DataContext: SignStationsComponent = this;
    public ObjectTableName: string = "Customs.CustomsSetting";
    public columns: any[] = null;

    _TranslationLoaded: boolean = false;

    private _entityResourceService: EntityResourceService = new EntityResourceService();
    ///public ComponentRef: ComponentRef<SignStationsComponent>;
    private _entityListService: EntityListService;

    //entityPM: SignStationPM;
    _SignStationExtendedListService: SignStationExtendedListService = new SignStationExtendedListService();
    ValidationErrorsList: string[] = [];
    _SelectedStatusValue: string='';
    _StatusList = ["Start", "תקין", "כשלון", "ממתין להזנת סיסמא", "כרטיס שגוי", "ללא הגדרה"];


    _AllNum: number = 0;
    _PinCodeNum: number = 0;
    _BadCardSelectedNum: number = 0;
    _OKNum: number = 0;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this._entityListService = new EntityListService();
        this.BuildColumns()
    }
    Loaded: boolean = false;
    ngOnInit() {
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(response => {


            this._TranslationLoaded = true;

            console.log("SignStationsC1111ompon");
            console.log("testX12333  dfgdsfg 55");
            ///console.log("te1tsssss");
            console.log("te111t");


            this.RefreshBtnClick()
        });

    }


    ///#region Properties

    //IsUnifreightCertificateActivatedEnabled: boolean = true;


    //get UnifreightCertificateActivated() { return this.entityPM != null ? this.entityPM.UnifreightCertificateActivated : false; }
    //set UnifreightCertificateActivated(value: boolean) { this.entityPM.UnifreightCertificateActivated = value; }


    IsSearchButtonEnabled: boolean = true;

    //#endregion
    @Output() MenuHeaderchangeevent = new EventEmitter();
    @Output() onQueryChangeEvent = new EventEmitter();

    SignStationGroupList: Array<SignStationGroup> = [];
    RefreshBtnClick() {
        this.IsSearchButtonEnabled = false;
        //this.CurrentSession.StartBusyIndicator("");

        setTimeout(() => {
            this._SignStationExtendedListService
                .GetSignStationGroupByStatus(this._SearchText)
                .subscribe(
                response => {
                    this.SignStationGroupList = response as any;
                    //this.RebuildTotals();
                    this._All = this.GetTotalOf("");
                    this._Waitingtoenterapassword = this.GetTotalOf("Waitingtoenterapassword");
                    this._Incorrectcard = this.GetTotalOf("IncorrectCard");
                    this._OK = this.GetTotalOf("OK");
                }
                );
        }, 1);

        setTimeout(() => {
            this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
        }, 10);

        //this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() }); 
    }
    _All: string;
    _Waitingtoenterapassword: string;
    _Incorrectcard: string;
    _OK: string;
    GetTotalOf(mystatus) {
        //_StatusList = ["Start", "תקין", "כישלון", "ממתין להזנת סיסמא", "כרטיס שגוי", "ללא הגדרה"];
        //_StatusList = ["Start", "OK", "Failure", "Waitingtoenterapassword", "Incorrectcard", "NoDefinition"];
        let tot = 0;
        this.SignStationGroupList.forEach(r => {
            if (AppTool.IsNullOrEmpty(mystatus)) {
                tot = tot + r.Total;
            }
            else {
                if (r.Key.toString().localeCompare(mystatus.toString()) == 0) {
                    //return " (" + r.Total.toString() + ") ";
                    tot = r.Total;
                }
                if (r.Key.toString() == mystatus.toString()) {
                    //return " (" + r.Total.toString() + ") ";
                    tot = r.Total;
                }
            }
        });
        return " (" + tot + ") ";
    }
    _SearchText: string = "";
    TextChanged($event) {
        this._SearchText = $event;
        this.RefreshBtnClick();
    }
    BuildColumns() {
        this.columns = [];
        


        this.columns.push({
            FieldName: 'SignerName',
            DataTypeCode: 'string',
            Display: 'שם החותם',
            Styles: { width: '130px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'SignerName'
        });

        this.columns.push({
            FieldName: 'PersonId',
            DataTypeCode: 'String',
            Display: 'מספר ת.ז',
            Styles: { width: '80px' },
            IsCustomTemplate: true
            , ServerSideSortable: true,
            SortByName: 'PersonId'
        });

        this.columns.push({
            FieldName: 'CustomsAgentId',
            DataTypeCode: 'string',
            Display: 'מספר ח.פ',
            Styles: { width: '80px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'CustomsAgentId'
        });

        this.columns.push({
            FieldName: 'MachineName',
            DataTypeCode: 'string',
            Display: 'תחנה',
            Styles: { width: '80px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'MachineName'
        });

        this.columns.push({
            FieldName: 'MachineUser',
            DataTypeCode: 'string',
            Display: 'משתמש (תחנה)',
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'MachineUser'
        });


        this.columns.push({
            FieldName: 'IsPersonalSignOn',
            DataTypeCode: 'string',
            Display: 'ח. אישית',
            Styles: { width: '60px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'IsPersonalSignOn',
            HtmlListComponentName: 'SignStationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/SignStationListTemplate',

        });

        this.columns.push({
            FieldName: 'IsCompanySignOn',
            DataTypeCode: 'string',
            Display: 'ח. חברתית',
            Styles: { width: '65px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'IsCompanySignOn',
            HtmlListComponentName: 'SignStationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/SignStationListTemplate',

        });
        this.columns.push({
            FieldName: 'VersionByFeatures',
            DataTypeCode: 'string',
            Display: 'גרסה',
            Styles: { width: '80px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'VersionByFeatures',
            
        });

        this.columns.push({
            FieldName: 'LastSignAt',
            DataTypeCode: 'string',
            Display: 'בוצעה חתימה',
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'LastSignAt'
            ,
            HtmlListComponentName: 'SignStationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/SignStationListTemplate',
        });

        this.columns.push({
            FieldName: 'Status',
            DataTypeCode: 'string',
            Display: 'סטטוס',
            Styles: { width: '150px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'Status',
            HtmlListComponentName: 'SignStationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/SignStationListTemplate',


        });

        

      


        //this.columns.push({
        //    FieldName: 'TestSign',
        //    DataTypeCode: 'String',
        //    Display: '',//TextCodeTranslator.Translate("CommunicationLogSteps.O.Log"),
        //    Styles: { width: '100px' },
        //    IsCustomTemplate: true,
        //    HtmlListComponentName: 'SignStationListTemplate',
        //    HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/SignStationListTemplate',
        //});


    }
    DataSource = {

        pageSize: 30,
        rowCount: null,
        //SortData("RequestCreateDate", "Descending", false, false);
        sortingCol: "SignerName",
        sortingDir: "Descending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {

            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;

        },
    };
    filterAgrs: ApiQueryFilters;
    
    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        if (filters == null) {
            filters = new ApiQueryFilters();
        }

        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = sortingCol;//"RequestCreateDate";
        filters.SortDirection = sortingDir; //"Descending";
        

        searchfields = this._SearchText || "";

        var myout =
            this.getExtendedByFilters(skip, take, sortingCol, sortingDir, getCount, searchfields);
        return myout;
    }
    
    getExtendedByFilters//(objectTableName: string, filters: ApiQueryFilters, MethodName: string = null) {
        (skip, take, sortingCol, sortingDir, getCount: boolean, searchfields: string) {
        let servicelink = './Customs/Services/ExtendedLists/SignStationExtendedListService';
        return new Promise((resolve, reject) => {
            SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                resolve(service.getByFilters(skip, take, sortingCol, sortingDir, getCount, searchfields, this._SelectedStatusValue));
            });
        });
    }






    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {

    }
}
