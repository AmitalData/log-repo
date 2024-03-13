declare var window: any;
import { Component, EventEmitter, Output, Input, OnInit, ElementRef, AfterViewInit, OnDestroy } from '@angular/core';
import { ChangeDetectorRef } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ResponseDataBase, CustomsStepEnum } from '../../../Customs/DataContract/ResponseData/ResponseDataBase';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';

import { CustomsRequestsSheetList } from '../../../Customs/EntityLists/CustomsRequestsSheetList';
import { CustomsRequestsSheetStatusList } from '../../../Customs/EntityLists/CustomsRequestsSheetStatusList';
import { CustomsRequestsSheetStatusListService } from '../../../Customs/Services/StandardLists/CustomsRequestsSheetStatusListService';

import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { CustomsRequestsSheetExtendedListService } from '../../../Customs/Services/ExtendedLists/CustomsRequestsSheetExtendedListService';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { CustomsRequestsSheetWebService } from '../../../Customs/Services/WebServices/CustomsRequestsSheetWebService';
import { List } from '../../../Infrastructure/DataContracts/Dashboard/List';
import { CustomsSettingListService } from 'Customs/Services/StandardLists/CustomsSettingListService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { interval, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

//////////////////////////////////////////////////////////////////


@Component({
    selector: 'CustomsRequestsSheetsComponent',
    styleUrls: ['./CustomsRequestsSheetsComponent.scss'],
    templateUrl: './CustomsRequestsSheetsComponent.html',
    providers: [CustomsRequestsSheetExtendedListService]
})


export class CustomsRequestsSheetsComponent
    extends BaseComponent
    implements OnInit, AfterViewInit, OnDestroy {
    private ngUnsubscribe = new Subject();

    private _entityResourceService: EntityResourceService = new EntityResourceService();
    _Id: string = Guid.newGuid();

    //////<<<<<<<<<<<<<<<<<<Request/Query Property

    _FromRequestCreateDate: Date;
    get FromRequestCreateDate() { return this._FromRequestCreateDate; }
    set FromRequestCreateDate(val: Date) {
        this._FromRequestCreateDate = val;
        this.DatesValidate("FromDate");
    }
    _FromRequestTime: Date;
    get FromRequestTime() { return this._FromRequestTime; }
    set FromRequestTime(val: Date) {
        this._FromRequestTime = val;
        this.DatesValidate("FromDate");
    }
    FromDateTime: Date;

    _ToRequestCreateDate: Date;
    get ToRequestCreateDate() { return this._ToRequestCreateDate; }
    set ToRequestCreateDate(val: Date) {
        this._ToRequestCreateDate = val;
        this.DatesValidate("ToDate");
    }
    _ToRequestTime: Date;
    get ToRequestTime() { return this._ToRequestTime; }
    set ToRequestTime(val: Date) {
        this._ToRequestTime = val;
        this.DatesValidate("ToDate");
    }
    ToDateTime: Date;

    MyRequestOnly: boolean;
    CorrelationId: string;
    CustomFileNo: string;
    InterfaceTypeCode: string;
    SearchFields: string;
    EntityReference: string;
    Title: string;
    RequestStatusString: string;
    _AllCustomsRequestsSheetStatusListVM: CustomsRequestsSheetStatusListVM[];
    IsRestored: boolean;
    //IsDCA?: boolean;
    _SelectedDCAValue: string = 'ALL';//'ALL';//DCA//!DCA

    //////Request/Query Property>>>>>>>>>>>>>>>>>>>>>>>>>>>

    private _entityListService: EntityListService;

    public DataContext: CustomsRequestsSheetsComponent = this;
    public ObjectTableName: string = "Customs.CustomsRequestsSheet";
    public columns: any[] = null;
    public columnsStatistics: any[] = null;


    _MySearchText = "Search";
    _CustomsRequestsSheetStatusListService: CustomsRequestsSheetStatusListService;
    public _TranslationLoaded: boolean = false;
    _AllCRSSChecked: boolean
    FiltersSectionVisibility: boolean = true;
    StatisticsVisibility: boolean = false;
    RefreshButtonVisibility: boolean;
    CloseButtonVisibility: boolean;//?????
    selectStatusesHeight: string;
    //_stratSearch: boolean = true;
    IncludingFuture: boolean = false;
    
    public get AllCRSSChecked() { return this._AllCRSSChecked };
    public set AllCRSSChecked(value: boolean) {
        this._AllCRSSChecked = value;
        if (this._AllCRSSChecked) {
            this._AllCustomsRequestsSheetStatusListVM.forEach((VM) => {
                VM.IsChecked = false;
            })
        }
    }

    @Output() MenuHeaderchangeevent = new EventEmitter();
    @Output() onQueryChangeEvent = new EventEmitter();
    isReAnAnalysis: boolean;
    private CurrentSession = SessionLocator.SelectedSession;
    
    constructor(public entityArgs: EntityArgs, private _CD: ChangeDetectorRef, public customsRequestsSheetExtendedListService: CustomsRequestsSheetExtendedListService) {
        super();
        this._CustomsRequestsSheetStatusListService = new CustomsRequestsSheetStatusListService();
        this._AllCustomsRequestsSheetStatusListVM = [];
        console.log("12....");

    }

    SetWindowArgs(args) {
        this.entityArgs = args;
        if (args != null) {
            this.isReAnAnalysis = args.isReAnAnalysis;
            if (this.isReAnAnalysis)
                this.selectStatusesHeight = "100px";
            else
                this.selectStatusesHeight = "410px";

        }
    }

    SetEntityArgs(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
    }
    ngOnInit() {
       this.MyRequestOnly = true;
        if (this.entityArgs.ObjectTableName) {
            if (this.entityArgs.ObjectTableName == "Customs.Declaration") {

                this.RefreshButtonVisibility = true;//Visibility.Visible;
                this.CustomFileNo = this.entityArgs.EntityPM.CustomFileNo;

                this.MyRequestOnly = false;
                this.UIProperties.SetEnabled("CustomFileNo", this.ObjectTableName, false);

            } else if (this.entityArgs.ObjectTableName == "Customs.Notification") {
                this.CloseButtonVisibility = true;;//Visibility.Visible; ///??????
            }
            
            else {
                this.FiltersSectionVisibility = false;//Visibility.Collapsed;
                this.RefreshButtonVisibility = true;//Visibility.Visible;
            }
        
        } else {
            this.FromRequestCreateDate = DateTool.AddDays(DateTool.GetCurrentDateAsUtc(), 0);
            this.RefreshButtonVisibility = true;//Visibility.Visible;
        }

        this.InitScreen()

    }
    ngOnDestroy() {
        console.log("CustomsRequestsSheetsComponent:ngOnDestroy");
        this.entityArgs = null;
        this._CD = null;
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }
    InitScreen() {
        //this.CurrentSession.StartBusyIndicator("");
        this._entityResourceService.getEntityResourceByTableName("Customs.CustomsRequestsSheet", 0).subscribe((response: any) => {
            this._entityResourceService.getEntityResourceByTableName("CommunicationLog", 0).subscribe((response: any) => {
                this._entityResourceService.getEntityResourceByTableName("Customs.Declaration", 0).subscribe((response: any) => {
                    //this._entityResourceService.getEntityResourceByTableName("CustomsRequestsSheetStatus", 0).subscribe((response:any) => {
                    if (AppTool.IsNullOrEmpty(this.Title)) {
                        this.Title = TextCodeTranslator.Translate("Customs.Declaration.TH.RequestSheet");
                    }

                    this._entityListService = new EntityListService();
                    this.BuildColumns();

                    interval(1000 * 3).pipe(takeUntil(this.ngUnsubscribe)).subscribe(() => this.GetStatistics());

                    //alert(TextCodeTranslator.Translate("Customs.CustomsRequestsSheet.F.RequestDescription"));
                    this._MySearchText = TextCodeTranslator.Translate("Customs.Notification.O.Search");

                    this._CustomsRequestsSheetStatusListService.getAll()/*getAllFromCache()*/.subscribe((resCRSSttsList) => {

                        //this.CurrentSession.StopBusyIndicator();

                        var listCustomsRequestsSheetStatusList: CustomsRequestsSheetStatusList[]
                            = resCRSSttsList.Result;
                        listCustomsRequestsSheetStatusList.sort((a, b) => {
                            return (a.LocalName === b.LocalName) ? 0 : (a.LocalName < b.LocalName) ? -1 : 1

                        }).forEach((item) => {
                            if (this.isReAnAnalysis && ["25", "21", "15"].includes(item.Code)) {
                                this._AllCustomsRequestsSheetStatusListVM.push(new CustomsRequestsSheetStatusListVM(item, this.entityArgs.ObjectTableName == "Customs.Declaration", true,this.entityArgs.ObjectTableName == "Customs.CourierMaster"));
                            }
                            else if (!this.isReAnAnalysis) {
                                this._AllCustomsRequestsSheetStatusListVM.push(new CustomsRequestsSheetStatusListVM(item, this.entityArgs.ObjectTableName == "Customs.Declaration", false,this.entityArgs.ObjectTableName == "Customs.CourierMaster"));

                            }
                        });

                        if (this.entityArgs.ObjectTableName == "Customs.Declaration") {
                            this.AllCRSSChecked = true;
                        }

                        this._TranslationLoaded = true;
                        var isDestroyed: boolean = this._CD['destroyed'];
                        if (!isDestroyed) {
                            this._CD.detectChanges();
                        }
                        this.CRSSearch();
                        //this.CurrentSession.StopBusyIndicator();

                    })
                });

            });



        })
    }

    private customsSettingListService: CustomsSettingListService = new CustomsSettingListService;
    customsRequestsSheetSummary = new Array<CustomsRequestsSheetSummary>();
    IsTherecustomsRequestsSheetSummary = false;
    SumRequests = 0;

    trackByCustomsRequestsSheetSummary(index: number, item: any): any {
        return item.InterfaceTypeName;
    }
    
    GetStatistics() {
        this.customsSettingListService.getSingleFromCache(SessionLocator.Tenant.toString()).subscribe((response: ServiceResponse) => {
            var customsSetting = response.Result;
            //if (!AppTool.IsNullOrEmpty(customsSetting) && customsSetting.CompanyType == "B") {
                this.StatisticsVisibility = !this.CurrentSession?.CurrentEditComponent?.EntityPM;;
                new CustomsRequestsSheetWebService().GetStatistics(this.IncludingFuture).subscribe((response: any) => {
                    if (response.Result != null) {
                        this.SumRequests = 0;
                        this.customsRequestsSheetSummary = response.Result;
                        for (var request of (this.customsRequestsSheetSummary as any[])) {
                            this.SumRequests += request.count;
                            this.IsTherecustomsRequestsSheetSummary = true;
                        }
                        this._CD.detectChanges();
                    }                    
                });
           // }
        });

    }

    ngAfterViewInit() {
        //this._CD.detectChanges();
        //this.CRSSearch();
    }
    CancelByFilters() {
        // if (!this.CheckValidation("Cancel")) {
        //    var messageWindow = new MessageWindow();
        //    messageWindow.Width = 400;
        //    messageWindow.Height = 150;
        //    messageWindow.ShowErrorIcon = true;
        //     messageWindow.Show("אין אפשרות לבטל בקשות בסטטוס ניתוח נכשל/תשובה תקינה , הסר את הסטטוס ונסה שוב");
        //    return;
        //}
        this.CurrentSession.StartBusyIndicator("");

        this.InitFilter();
        this.customsRequestsSheetExtendedListService.CancelByFilters(this.filterAgrs).subscribe(
            data => {
                this.CurrentSession.StopBusyIndicator();

                if (data.HasError) {
                    var messageWindow = new MessageWindow();
                    messageWindow.Width = 400;
                    messageWindow.Height = 150;
                    messageWindow.ShowErrorIcon = true;
                    messageWindow.Show(data.ErrorsArray[0]);

                    //messageWindow.Show(TextCodeTranslator.Translate("Customs.RequestSheet.O.CancelAllError"));
                }

                else {
                    var messageWindow = new MessageWindow();
                    messageWindow.Width = 400;
                    messageWindow.Height = 150;
                    messageWindow.ShowErrorIcon = true;
                    messageWindow.Show(TextCodeTranslator.Translate("Customs.RequestSheet.O.CancelAllSuccess"));
                }


            }
        );




    }

    ReAnalysisByFilters() {
        //if (!this.CheckValidation("ReAnalysis")) {
        //    var messageWindow = new MessageWindow();
        //    messageWindow.Width = 400;
        //    messageWindow.Height = 150;
        //    messageWindow.ShowErrorIcon = true;
        //    messageWindow.Show("אין אפשרות לנתח מחדש בקשות בסטטוס שליחה נכשלה , הסר את הסטטוס ונסה שוב");
        //    return;
        //}
        this.CurrentSession.StartBusyIndicator("");

        this.InitFilter();
        this.customsRequestsSheetExtendedListService.ReAnalysisByFilters(this.filterAgrs).subscribe(
            data => {
                this.CurrentSession.StopBusyIndicator();

                if (data.HasError) {
                    var messageWindow = new MessageWindow();
                    messageWindow.Width = 400;
                    messageWindow.Height = 150;
                    messageWindow.ShowErrorIcon = true;
                    messageWindow.Show(data.ErrorsArray[0]);
                    //messageWindow.Show(TextCodeTranslator.Translate("Customs.RequestSheet.O.ErrorSendReAnalysis"));

                }
                else {
                    var messageWindow = new MessageWindow();
                    messageWindow.Width = 400;
                    messageWindow.Height = 150;
                    messageWindow.ShowErrorIcon = true;
                    messageWindow.Show(TextCodeTranslator.Translate("Customs.RequestSheet.O.SendReAnalysisInBackground"));
                }

            });
    }
    CheckValidation(type: string) {
        if (this.AllCRSSChecked) return false;

        for (var i = 0; i < this._AllCustomsRequestsSheetStatusListVM.length; i++) {
            if (this._AllCustomsRequestsSheetStatusListVM[i].IsChecked) {
                if (type == "ReAnalysis" && this._AllCustomsRequestsSheetStatusListVM[i].MyItem.Code != "21" && this._AllCustomsRequestsSheetStatusListVM[i].MyItem.Code != "25")
                    return false;
                if (type == "Cancel" && this._AllCustomsRequestsSheetStatusListVM[i].MyItem.Code != "15")
                    return false;
            }
        }



        return true;
    }

    InitFilter() {


        var filters = new ApiQueryFilters();


        filters.GetAll = true;
        filters.GetCount = true;

        if (AppTool.IsNullOrEmpty(filters.SortBy)) {
            filters.SortBy = "RequestCreateDate";
        }
        if (AppTool.IsNullOrEmpty(filters.SortDirection)) {
            filters.SortDirection = "Descending";
        }

        filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");
        let objectTableName = "";
        let objectTableId1 = "";
        if (this.entityArgs) {
            if (!AppTool.IsNullOrEmpty(this.entityArgs.ObjectTableName)) {
                objectTableName = this.entityArgs.ObjectTableName;
                var objectTablePM = //window.ObjectTables.filter(d => d.Id == ObjectTableId)[0];
                    window.ObjectTables.filter(t => t.Name == objectTableName)[0];
                objectTableId1 = objectTablePM.Id;
            }
        }
        if (objectTableName == "Customs.CourierMaster") {

            if (!AppTool.IsNullOrEmpty(this.entityArgs.OriginEntity)) {
                filters.addAdditionalFilter("Id", this.entityArgs.OriginEntity, null, null, "InList", false, false, false, "string");
            }
            this.GetRequestStatusString(filters);

        }
        //if (!AppTool.IsNullOrEmpty(objectTableName)) {
        else if (objectTableName === "Customs.Notification") {
            //////never tested !!!!!!!- copy from silverlight
            filters.addAdditionalFilter("Id", this.entityArgs.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        }
        else if (objectTableName === "Customs.ExportStorage") {
          
            filters.addAdditionalFilter("ObjectTableId2", objectTableId1, null, null, "Equals", false, false, false, "string");
            let EntityId2 = this.entityArgs.EntityPM.Id;
            filters.addAdditionalFilter("EntityId2", EntityId2, null, null, "Equals", false, false, false, "string");
        }
        else {
            if (!AppTool.IsNullOrEmpty(objectTableName) && objectTableName != "Customs.Declaration") {
                //////never tested !!!!!!!- copy from silverlight
                //filters.addAdditionalFilter("ObjectTableId1", objectTableId, null, null, "Equals", false, false, false, "string");
                filters.addAdditionalFilter("ObjectTableId1", objectTableId1, null, null, "Equals", false, false, false, "string");
                let EntityId1 = this.entityArgs.EntityPM.Id;
                if (AppTool.IsNullOrEmpty(EntityId1)) {
                    EntityId1 = "new Entity do not get any rows !!!!";
                }
                //filters.addAdditionalFilter("EntityId1", this.entityArgs.EntityPM.Id, null, null, "Equals", false, false, false, "string");
                filters.addAdditionalFilter("EntityId1", EntityId1, null, null, "Equals", false, false, false, "string");

            }

            else {

                if (this.FromDateTime != null || this.ToDateTime != null) {// for Region
                    filters.addAdditionalFilter("RequestCreateDate", this.FromDateTime, this.ToDateTime, null, "Between", false, false, false, "DateTime");
                }

                if (this.MyRequestOnly) {
                    filters.addAdditionalFilter("RequestOwnerId", SessionLocator.LoggedUserId, null, null, "Equals", false, false, false, "string");
                }

                if (!AppTool.IsNullOrEmpty(this.CorrelationId)) {
                    filters.addAdditionalFilter("CorrelationId", this.CorrelationId, null, null, "Equals", false, false, false, "string");
                }

                if (!AppTool.IsNullOrEmpty(this.CustomFileNo)) {
                    filters.addAdditionalFilter("CustomFileNo", this.CustomFileNo, null, null, "Equals", false, false, false, "string");
                }
                if (!AppTool.IsNullOrEmpty(this.InterfaceTypeCode)) {

                    filters.addAdditionalFilter("InterfaceTypeCode", this.InterfaceTypeCode, null, null, "Equals", false, false, false, "string");

                }
                if (!AppTool.IsNullOrEmpty(this.SearchFields)) {
                    filters.addAdditionalFilter("SearchFields", this.SearchFields, null, null, "Contains", false, false, false, "string");

                }


                if (!AppTool.IsNullOrEmpty(this.EntityReference)) {

                    filters.addAdditionalFilter("EntityReference", this.EntityReference, null, null, "Equals", false, false, false, "string");
                }
                if (AppTool.IsNullOrEmpty(objectTableName) || objectTableName == "Customs.Declaration") {
                    this.GetRequestStatusString(filters);
                }


                if (this.IsRestored) {
                    filters.addAdditionalFilter("IsRestored", this.IsRestored, null, null, "Equals", false, false, false, "boolean");
                }
                //_SelectedDCAValue: string = 'ALL';//'ALL';//DCA//!DCA
                if (this._SelectedDCAValue != "ALL") {
                    filters.addAdditionalFilter("IsDCA", this._SelectedDCAValue === "DCA", null, null, "Equals", false, false, false, "boolean");

                }
            }
        }

        this.filterAgrs = filters;

    }

    CRSSearch() {
        this.GetStatistics();
        this.IsSearchButtonEnabled = false;
        //this.CurrentSession.StartBusyIndicator("");
        setTimeout(() => {
            this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
        }, 10);

        //this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() }); 
    }



    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: 'RequestDescription',
            DataTypeCode: 'String',//'Number',
            Display: TextCodeTranslator.Translate("Customs.CustomsRequestsSheet.F.RequestDescription"),
            Styles: { width: '220px' },
            IsCustomTemplate: true
            , ServerSideSortable: true,
            SortByName: 'RequestDescription'
        });

        this.columns.push({
            FieldName: 'CustomFileNo',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.CustomsRequestsSheet.F.CustomFileNo"),
            Styles: { width: '100px' },
            IsCustomTemplate: true,

            ServerSideSortable: true,
            SortByName: 'CustomFileNo'
        });
        this.columns.push({
            FieldName: 'RequestStatusName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.CustomsRequestsSheet.F.RequestStatusName"),
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'RequestStatusName'
        });
        this.columns.push({
            FieldName: 'RequestCreateDate',
            DataTypeCode: 'Date',
            Display: TextCodeTranslator.Translate("Customs.CustomsRequestsSheet.F.RequestCreateDate"),
            Styles: { width: '160px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsRequestsSheetsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsRequestsSheetsListTemplate',
            ServerSideSortable: true,
            SortByName: 'RequestCreateDate'

        });

        this.columns.push({
            FieldName: 'RequestOwnerName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.CustomsRequestsSheet.F.RequestOwnerName"),
            Styles: { width: '140px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'RequestOwnerName'

        });

        this.columns.push({
            FieldName: 'IsDCA',
            DataTypeCode: 'boolean',
            Display: TextCodeTranslator.Translate("Customs.CustomsRequestsSheet.F.IsDCA"),
            Styles: { width: '50px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsRequestsSheetsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsRequestsSheetsListTemplate',
            ServerSideSortable: true,
            SortByName: 'IsDCA'
        });
        this.columns.push({
            FieldName: 'IsRestored',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.CustomsRequestsSheet.F.IsRestored"),
            Styles: { width: '60px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsRequestsSheetsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsRequestsSheetsListTemplate',
            ServerSideSortable: true,
            SortByName: 'IsRestored'
        });
        this.columns.push({
            FieldName: 'CancleRequest',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.RequestSheet.O.Cancelled"),
            Styles: { width: '80px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsRequestsSheetsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsRequestsSheetsListTemplate',
            AdditionalDataCustom: this.isReAnAnalysis
        });


        this.columns.push({
            FieldName: 'ShowFormatedResponse',
            DataTypeCode: 'String',
            Display: '',//
            Styles: { width: '80px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsRequestsSheetsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsRequestsSheetsListTemplate',
        });
        this.columns.push({
            FieldName: 'ShowLog',
            DataTypeCode: 'String',
            Display: '',//TextCodeTranslator.Translate("CommunicationLogSteps.O.Log"),
            Styles: { width: '80px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsRequestsSheetsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsRequestsSheetsListTemplate',
        });

        this.columns.push({
            FieldName: 'ReAnalyze',
            DataTypeCode: 'String',
            Display: '',//TextCodeTranslator.Translate("CommunicationLogSteps.O.Log"),
            Styles: { width: '80px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsRequestsSheetsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsRequestsSheetsListTemplate',
            AdditionalDataCustom: this.isReAnAnalysis

        });

        this.columns.push({
            FieldName: 'CorrelationId',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.CustomsRequestsSheet.F.CorrelationId"),
            Styles: { width: '250px' },
            IsCustomTemplate: true,

            ServerSideSortable: true,
            SortByName: 'CorrelationId'
        });

        
        this.columns.push({
            FieldName: 'CopyCorrelationId',
            DataTypeCode: 'String',
            Display: '',
            Styles: { width: '80px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsRequestsSheetsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsRequestsSheetsListTemplate',
        });


    }
    OnFirstRowSelected($event) {
        // alert("OnFirstRowSelected()" + $event);
    }
    OnrowSelectedEvent($event) {
        // alert("OnrowSelectedEvent()" + $event);
    }
    OnColumnResisedevent($event) {
        //  alert("ViewInitCompleted()" + $event);
    }

    ViewInitCompleted($event) {
        //alert("ViewInitCompleted()" + $event);
        //this.SelectedRow = this.ItemsSource.Collection[0];
        //this.OnRowSelected(this.SelectedRow);
    }

    DataSource = {

        pageSize: 30,
        rowCount: null,
        //SortData("RequestCreateDate", "Descending", false, false);
        sortingCol: "RequestCreateDate",
        sortingDir: "Descending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {

            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;

        },
    };
    filterAgrs: ApiQueryFilters;
    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        // if (filters == null) {
        filters = new ApiQueryFilters();
        // }

        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;

        filters.SortBy = sortingCol;//"RequestCreateDate";
        filters.SortDirection = sortingDir; //"Descending";
        if (AppTool.IsNullOrEmpty(filters.SortBy)) {
            filters.SortBy = "RequestCreateDate";
        }
        if (AppTool.IsNullOrEmpty(filters.SortDirection)) {
            filters.SortDirection = "Descending";
        }

        filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");
        let objectTableName = "";
        let objectTableId1 = "";
        if (this.entityArgs) {
            if (!AppTool.IsNullOrEmpty(this.entityArgs.ObjectTableName)) {
                objectTableName = this.entityArgs.ObjectTableName;
                var objectTablePM = //window.ObjectTables.filter(d => d.Id == ObjectTableId)[0];
                    window.ObjectTables.filter(t => t.Name == objectTableName)[0];
                objectTableId1 = objectTablePM.Id;
            }
        }
        if (objectTableName == "Customs.CourierMaster") {

            if (!AppTool.IsNullOrEmpty(this.entityArgs.OriginEntity)) {
                filters.addAdditionalFilter("Id", this.entityArgs.OriginEntity, null, null, "InList", false, false, false, "string");
            }
            this.GetRequestStatusString(filters);
        }
        
        else if (objectTableName === "Customs.Notification") {
            //////never tested !!!!!!!- copy from silverlight
            filters.addAdditionalFilter("Id", this.entityArgs.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        }
        else if (objectTableName === "Customs.ExportStorage") {
            
            filters.addAdditionalFilter("ObjectTableId2", objectTableId1, null, null, "Equals", false, false, false, "string");
            let EntityId2 = this.entityArgs.EntityPM.Id;
            filters.addAdditionalFilter("EntityId2", EntityId2, null, null, "Equals", false, false, false, "string");
        }
        else {
            if (!AppTool.IsNullOrEmpty(objectTableName) && objectTableName != "Customs.Declaration" ) {
                //////never tested !!!!!!!- copy from silverlight
                //filters.addAdditionalFilter("ObjectTableId1", objectTableId, null, null, "Equals", false, false, false, "string");
                filters.addAdditionalFilter("ObjectTableId1", objectTableId1, null, null, "Equals", false, false, false, "string");
                let EntityId1 = this.entityArgs.EntityPM.Id;
                if (AppTool.IsNullOrEmpty(EntityId1)) {
                    EntityId1 = "new Entity do not get any rows !!!!";
                }
                filters.addAdditionalFilter("EntityId1", EntityId1, null, null, "Equals", false, false, false, "string");

            }

           else {

                if (this.FromDateTime != null || this.ToDateTime != null) {// for Region
                    filters.addAdditionalFilter("RequestCreateDate", this.FromDateTime, this.ToDateTime, null, "Between", false, false, false, "DateTime");
                }

                if (this.MyRequestOnly) {
                    filters.addAdditionalFilter("RequestOwnerId", SessionLocator.LoggedUserId, null, null, "Equals", false, false, false, "string");
                }

                if (!AppTool.IsNullOrEmpty(this.CorrelationId)) {
                    filters.addAdditionalFilter("CorrelationId", this.CorrelationId, null, null, "Equals", false, false, false, "string");
                }

                if (!AppTool.IsNullOrEmpty(this.CustomFileNo)) {
                    filters.addAdditionalFilter("CustomFileNo", this.CustomFileNo, null, null, "Equals", false, false, false, "string");
                }
                if (!AppTool.IsNullOrEmpty(this.InterfaceTypeCode)) {

                    filters.addAdditionalFilter("InterfaceTypeCode", this.InterfaceTypeCode, null, null, "Equals", false, false, false, "string");

                }
                if (!AppTool.IsNullOrEmpty(this.SearchFields)) {
                    filters.addAdditionalFilter("SearchFields", this.SearchFields, null, null, "Contains", false, false, false, "string");

                }

                if (!AppTool.IsNullOrEmpty(this.EntityReference)) {

                    filters.addAdditionalFilter("EntityReference", this.EntityReference, null, null, "Equals", false, false, false, "string");
                }
                
                if (AppTool.IsNullOrEmpty(objectTableName) || objectTableName == "Customs.Declaration"  ) {
                    this.GetRequestStatusString(filters);
                }

                if (this.IsRestored) {
                    filters.addAdditionalFilter("IsRestored", this.IsRestored, null, null, "Equals", false, false, false, "boolean");
                }
                //_SelectedDCAValue: string = 'ALL';//'ALL';//DCA//!DCA
                if (this._SelectedDCAValue != "ALL") {
                    filters.addAdditionalFilter("IsDCA", this._SelectedDCAValue === "DCA", null, null, "Equals", false, false, false, "boolean");

                }
            }
           

        }
        var myout = this._entityListService
            .getExtendedByFilters("Customs.CustomsRequestsSheet", filters);
        myout.then(res => {
            this.IsSearchButtonEnabled = true;
            //this.CurrentSession.StopBusyIndicator();
        });

        return myout;

    }

    GetRequestStatusString(filters: any) {
        let RequestStatusString: string = "";
        if (this.AllCRSSChecked) return;
        this._AllCustomsRequestsSheetStatusListVM.forEach((requestStatus) => {

            if (requestStatus.IsChecked) {
                if (!AppTool.IsNullOrEmpty(RequestStatusString)) {
                    RequestStatusString = RequestStatusString + ","
                }
                RequestStatusString = RequestStatusString + requestStatus.MyItem.Code;
            }
        });

        if (!AppTool.IsNullOrEmpty(RequestStatusString)) {
            //.SetFilter("RequestStatusCode", RequestStatusString, false, "InListExact", null, true);
            filters.addAdditionalFilter("RequestStatusCode", RequestStatusString, null, null, "InListExact", false, false, false, "string");
        }

    }
    ErrorMessage: string = TextCodeTranslator.Translate("Customs.RequestSheet.O.RequestCreateDate");
    IsSearchButtonEnabled: boolean = true

    public DatesValidate(mydate: string) {


        if (!AppTool.IsNullOrEmpty(this.FromRequestCreateDate)) {
            this.FromDateTime = new Date(this.FromRequestCreateDate.toString());
            this.FromDateTime = new Date(
                this.FromDateTime.getUTCFullYear(), this.FromDateTime.getUTCMonth(), this.FromDateTime.getUTCDate(), 0, 0, 0);
        }

        if (this.FromRequestTime) {
            //this.FromDateTime = DateTool.AddHour(this.FromDateTime, this.FromRequestTime.getHours())
            //this.FromDateTime = DateTool.AddMinute(this.FromDateTime, this.FromRequestTime.getMinutes())
            //this.FromDateTime = DateTool.AddSecond(this.FromDateTime, this.FromRequestTime.getSeconds())
            this.FromDateTime = new Date(
                this.FromDateTime.getUTCFullYear(), this.FromDateTime.getUTCMonth(), this.FromDateTime.getUTCDate() + 1,
                this.FromRequestTime.getUTCHours(), this.FromRequestTime.getUTCMinutes(), this.FromRequestTime.getUTCSeconds());
        }

        if (!AppTool.IsNullOrEmpty(this.ToRequestCreateDate)) {
            this.ToDateTime = new Date(this.ToRequestCreateDate.toString());
            this.ToDateTime = new Date(
                this.ToDateTime.getUTCFullYear(), this.ToDateTime.getUTCMonth(), this.ToDateTime.getUTCDate(), 23, 59, 59);
        }
        if (this.ToRequestTime) {
            //this.ToDateTime = DateTool.AddHour(this.ToDateTime, this.ToRequestTime.getHours())
            //this.ToDateTime = DateTool.AddMinute(this.ToDateTime, this.ToRequestTime.getMinutes())
            //this.ToDateTime = DateTool.AddSecond(this.ToDateTime, this.ToRequestTime.getSeconds())
            this.ToDateTime = new Date(
                this.ToDateTime.getUTCFullYear(), this.ToDateTime.getUTCMonth(), this.ToDateTime.getUTCDate(),
                this.ToRequestTime.getUTCHours(), this.ToRequestTime.getUTCMinutes(), this.ToRequestTime.getUTCSeconds());
        }

        if (DateTool.IsNullOrMinDateTime(this.FromDateTime) && DateTool.IsNullOrMinDateTime(this.ToDateTime)) {
            this.DateOk();
            return;

        }
        if (!DateTool.IsNullOrMinDateTime(this.FromDateTime) && DateTool.IsNullOrMinDateTime(this.ToDateTime)) {
            this.DateOk();
            return;
        }
        if (DateTool.IsNullOrMinDateTime(this.FromDateTime) && !DateTool.IsNullOrMinDateTime(this.ToDateTime)) {
            this.DateOk();
            return;
        }

        if (this.ToDateTime.valueOf() <= this.FromDateTime.valueOf()) {
            if (mydate == "FromDate") {
                //this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
                this.UIProperties.SetValidity("FromRequestCreateDate", "Customs.CustomsRequestsSheet", false, this.ErrorMessage);
                this.IsSearchButtonEnabled = false;
            }
            else if (mydate == "ToDate") {
                this.UIProperties.SetValidity("ToRequestCreateDate", "Customs.CustomsRequestsSheet", false, this.ErrorMessage);
                this.IsSearchButtonEnabled = false;
            }
        }

        else {
            this.DateOk();
        }


    }
    DateOk() {
        this.UIProperties.SetValidity("FromRequestCreateDate", "Customs.CustomsRequestsSheet", true, this.ErrorMessage);
        this.UIProperties.SetValidity("ToRequestCreateDate", "Customs.CustomsRequestsSheet", true, this.ErrorMessage);
        this.IsSearchButtonEnabled = true;
    }

    RefreshButtonClicked() {
        this.CRSSearch();
    }
}

////////////////////////////////////////
export class CustomsRequestsSheetStatusListVM {
    constructor(public MyItem: CustomsRequestsSheetStatusList, isdeclaration?: boolean, isReAnAnalysis?: boolean,isCourierMaster?:boolean) {
      var Code = MyItem.Code;
      if(isCourierMaster){
        if(Code == "0" || Code == "1" || Code == "2" || Code == "5" || Code == "20" || Code == "21" || Code == "23"){
            this.IsChecked = true;
        }
        else{
            this.IsChecked = false;
        }
      }
      else if (!isReAnAnalysis) {
            
            if (Code == "1" || Code == "2" || Code == "3" || Code == "4" || Code == "5" || Code == "21" || Code == "99") {
                this.IsChecked = true;
            }

            if (//declarationPM != null
                isdeclaration
                && Code != "99") {

                this.IsChecked = true;
            }
        }
    }
    IsChecked: boolean;
}
export class CustomsRequestsSheetSummary {
    id: string;
    count: number;
    InterfaceTypeName: string;
}
//////////////////////////////////////////////////
