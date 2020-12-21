declare var window: any;
import { Component, EventEmitter, ChangeDetectorRef, OnInit } from '@angular/core';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {AdvancedQueryFilterPM} from '../../../Infrastructure/EntityPMs/AdvancedQueryFilterPM';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {AdvancedQueryFiltersPMService} from '../../../Infrastructure/Services/StandardPMs/AdvancedQueryFiltersPMService';

import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {ObjectFieldPM} from '../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {QueryPM} from '../../../Infrastructure/EntityPMs/QueryPM';
import {FormGroup, FormBuilder} from '@angular/forms';
import {LogEvents} from '../../../Infrastructure/Utilities/LogEvents';
import {PubSubService} from '../../../Infrastructure/Utilities/events/ApiFiltersEvent';
import {FilterField, FilterFieldsClass, FieldsValues} from '../../../Infrastructure/Components/LogitudeComponents/QueryListComponent/FilterField';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow'; 
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { HttpClient } from '@angular/common/http';

@Component({
    
    selector: 'AdvSearchComponent',
    templateUrl: './AdvanceSearchComponent.html',
    inputs: ['ObjectTableName', 'QueryChangeEvent', 'isWindowViewMode', 'isNewViewMode', 'QueryId','QueryCode', 'Filterchangeevent', 'rabaia'],
    providers: [HttpClient],
})

export class AdvanceSearchComponent implements OnInit {
    public ObjectTable: any;
    public ObjectTableId: string;
    public ObjectTableName: string;
    public QueryId: string;
    public QueryCode: string;
    public ObjectFields: any;
    allFilterFieldsClass: FilterFieldsClass;
    filterFields: FilterFieldsClass;
    constantFilterFields: FilterFieldsClass;
    public temp: FilterField[];
    IsOpened: boolean;
    isWindowViewMode: boolean;
    isNewViewMode: boolean;
    HasChanges: boolean;
    currentQuery: QueryPM;
    IsUserQuery: boolean = false;
    public QueryChangeEvent: EventEmitter<any>;
    public myForm: FormGroup;
    public AdvancedQueryFilterPMs: AdvancedQueryFilterPM[];
    public FieldsValues: FieldsValues;
    private myAdvancedQueryFiltersPMService: AdvancedQueryFiltersPMService;
    public AdvanceQueryDropButtonId: string;
    public AdvanceQuerySearchFieldsId: string;
    Filterchangeevent: LogEvents.EventManager;
    Height: any;
    private serviceArgs: ServiceArgs;
    public BooleanValues = ["True", "False", "No Filter"];
    LayoutDirection: string = 'ltr';
    public TopImageRedX: string = (ObjectsLocator.GlobalSetting.WorkEnvironment == "customs") ? '0px' : '7px';
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(fb: FormBuilder, private pubSubService: PubSubService, private CD: ChangeDetectorRef) {
        this.serviceArgs = new ServiceArgs();
        this.serviceArgs.http = ServiceHelper.HttpClient;
        this.myForm = fb.group({
            //'ShipperName': ['', Validators.required]

        });
        if (this.CurrentSession == null) {
            this.AdvanceQueryDropButtonId = "dvanceQueryDropButton_-1_-1";
            this.AdvanceQuerySearchFieldsId = "AdvanceQuerySearchFields_-1_-1"; 
        }

        else {
            this.AdvanceQueryDropButtonId = "dvanceQueryDropButton_" + this.CurrentSession.GetNewId("dvanceQueryDropButton");
            this.AdvanceQuerySearchFieldsId = "AdvanceQuerySearchFields_" + this.CurrentSession.GetNewId("AdvanceQuerySearchFields"); 
        }

        //RTL Layout
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;

    }

    ShowFieldsList() {
        document.getElementById("FieldsDropdown").classList.toggle("showDDButton");
    }

    ClearPlaceHolder() {
        var temp = document.getElementById(this.AdvanceQuerySearchFieldsId) as HTMLInputElement;
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
        var ToggleBTN = document.getElementById(this.AdvanceQueryDropButtonId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenuTemp";

        //this.CD.detectChanges();
    }

    OnDeleteValue() {
        var temp = document.getElementById(this.AdvanceQuerySearchFieldsId) as HTMLInputElement;
        temp.value = null;
        this.SearchText = null;
        temp.focus(); 
    }

    FillPlaceHolder() {
        if (!this.SearchText) {
            var temp = document.getElementById(this.AdvanceQuerySearchFieldsId) as HTMLInputElement;
            temp.placeholder = "Search";
            temp.style.background = "url(Images/Search.png) no-repeat scroll";
            if (this.LayoutDirection == "rtl") {
                temp.style.backgroundPosition = "left center";
                temp.style.paddingRight = "0px";
                temp.style.paddingLeft = "30px";
               
            } else {
                temp.style.backgroundPosition = "right center";
                temp.style.paddingRight = "30px";
                temp.style.paddingLeft = "0px";
            }
        }
        var ToggleBTN = document.getElementById(this.AdvanceQueryDropButtonId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenu";
    }

    setToggleButtonMenuTemp() {
     
        var ToggleBTN = document.getElementById(this.AdvanceQueryDropButtonId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenuTemp";
         
    } 

    setToggleButtonMenu() { 
        var ToggleBTN = document.getElementById(this.AdvanceQueryDropButtonId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenu";
    }

    ngOnInit() {
        //min-height: 163px
        this.Height = {};
        this.QueryChangeEvent.subscribe((res) => {
            if (this.QueryCode != res.QueryCode) {
                this.SearchText = null;
                this.OnQueryFilterChanged(res.QueryCode);
            }
        });

        //this.textChange.subscribe((res) => {
        //    this.onTextChange(res);
        //});
        this.filterFields = new FilterFieldsClass(this.isWindowViewMode == undefined ? false : this.isWindowViewMode, this, this.pubSubService);
        this.allFilterFieldsClass = new FilterFieldsClass(this.isWindowViewMode == undefined ? false : this.isWindowViewMode, this, this.pubSubService);
        this.constantFilterFields = new FilterFieldsClass(this.isWindowViewMode == undefined ? false : this.isWindowViewMode, this, this.pubSubService);
        //if (this.isWindowViewMode) {
        //    advanceViewBorder.Visibility = Visibility.Collapsed;
        //    windowViewBorder.Visibility = Visibility.Visible;
        //}

        //else {
        //    advanceViewBorder.Visibility = Visibility.Visible;
        //    windowViewBorder.Visibility = Visibility.Collapsed;
        //}
        //// Close the dropdown menu if the user clicks outside of it
        //window.onclick = function (event) {
        //    if (event.target.id != "SearchFields" && event.target.id != "FieldsDropdown" && event.target.id != "AddFilterButton" && event.target.className.indexOf("dropcontent") < 0) {

        //        var dropdowns = document.getElementsByClassName("dropdown-content");
        //        var i;
        //        for (i = 0; i < dropdowns.length; i++) {
        //            var openDropdown = dropdowns[i];
        //            if (openDropdown.classList.contains('showDDButton')) {
        //                openDropdown.classList.remove('showDDButton');
        //            }
        //        }
        //    }
        //}

        this.ObjectTable = window.ObjectTables.filter(x => x.Name === this.ObjectTableName)[0];
        this.ObjectTableId = this.ObjectTable.Id;
        this.ObjectFields = window.ObjectFields.filter(d => d.ObjectTableId === this.ObjectTableId && d.CanFilter === true);
        this.temp = [];
        //this.ObjectFields.forEach((item, key) => {
        //    this.temp.push(new FilterField(item, "", false));
        //});
        this.Run();
    }

    Run() {
        this.HasChanges = false;
        //FilterParametersChangedEvent evt = eventAggregator.GetEvent<FilterParametersChangedEvent>();
        //evt.Subscribe(OnFilterParametersChanged);

        //if (newGrid == null) {
        this.QueryFilterChangedAction(this.QueryCode);
        //}
        //if (!loaded) {
        //    loaded = true;
        //}
    }

    allFilterFields: ObjectFieldPM[];// = new List<ObjectFieldPM>();
    constantFilterFieldsList: ObjectFieldPM[];// = new List<ObjectFieldPM>();
    CurrentFilters: ObjectFieldPM[];// = new List<ObjectFieldPM>();
    advancedQueryFiltersList: AdvancedQueryFilterPM[]; //List<AdvancedQueryFilterPM> 
    fieldsStaticList: FilterField[];//FilterField

    timeFilterFieldsClass: FilterFieldsClass = new FilterFieldsClass(false, this, this.pubSubService);
    timeFrameFields: FilterField[];
    noFiltersField: FilterField;
    objectField: ObjectFieldPM;
    QueryFilterChangedAction(QueryCode: string) {

        this.allFilterFields = [];
        this.constantFilterFieldsList = [];
        this.CurrentFilters = [];
        this.advancedQueryFiltersList = [];
        this.fieldsStaticList = [];
        this.timeFrameFields = [];
        this.AdvancedQueryFilterPMs = [];
        if (this.myAdvancedQueryFiltersPMService == null) {
            this.myAdvancedQueryFiltersPMService = new AdvancedQueryFiltersPMService();
            this.myAdvancedQueryFiltersPMService.setServiceArgs(this.serviceArgs);
        }

        this.myAdvancedQueryFiltersPMService.getadvancedqueryfiltersbytenantByQuery(SessionInfo.LoggedUserTenant, SessionInfo.LoggedUserId, QueryCode).subscribe((myResult:any) => {
            if (myResult == null) {
                this.AdvancedQueryFilterPMs = [];
            }

            else {
                this.AdvancedQueryFilterPMs = myResult;
            }

            this.timeFilterFieldsClass = new FilterFieldsClass(this.isNewViewMode, this, this.pubSubService);
            this.FieldsValues = new FieldsValues();
            this.currentQuery = window.Queries.filter(q => q.UniqueCode == /*this.ObjectTableName+"."+*/QueryCode)[0];

            if (!this.currentQuery) {
                var iMessageWindow = new MessageWindow();
                iMessageWindow.Show("Query not found");
            }

            else {
                //newGrid = new Grid() { Background = new SolidColorBrush(Colors.Transparent) };
                if (this.currentQuery.UserId != null) {
                    this.IsUserQuery = true;
                }
                else {
                    this.IsUserQuery = false;
                }
                if (this.currentQuery.DisplayAsCustom) {

                }

                if (this.ObjectFields) {

                    this.allFilterFields = window.ObjectFields.filter(o => o.ObjectTableId == this.ObjectTableId && o.CanFilter == true && o.DataTypeCode != "Constant" && ((o.ValidForQuerySection1 == this.currentQuery.QuerySection || o.ValidForQuerySection2 == this.currentQuery.QuerySection) || o.IsCustom == true) && o.IsCustomFilter == false);

                    this.constantFilterFieldsList = window.ObjectFields.filter(o => o.ObjectTableId == this.ObjectTableId && o.CanFilter == true && o.DataTypeCode == "Constant" && ((o.ValidForQuerySection1 == this.currentQuery.QuerySection || o.ValidForQuerySection2 == this.currentQuery.QuerySection) || o.IsCustom == true) && o.IsCustomFilter == false);

                    this.timeFilterFieldsClass.AddFiltersList(window.ObjectFields.filter(o => o.ObjectTableId == this.ObjectTableId && o.CanFilter == true && o.FieldName != "TimeFrameFilter" && o.IsTimeFrameFilter == true && (o.ValidForQuerySection1 == this.currentQuery.QuerySection || o.ValidForQuerySection2 == this.currentQuery.QuerySection)), this.currentQuery.Id, myResult);
                    var MyFields = window.ObjectFields.filter(o => o.ObjectTableId == this.ObjectTableId && o.CanFilter == true && o.DataTypeCode != "Constant" && ((o.ValidForQuerySection1 == this.currentQuery.QuerySection || o.ValidForQuerySection2 == this.currentQuery.QuerySection) || o.IsCustom == true) && o.IsCustomFilter == false);
                    var MyFilteredFields: any[] = [];
                    MyFields.forEach((item, key) => {
                        var Temp = MyFilteredFields.filter(a => a.FieldName == item.FieldName);
                        if (Temp != null && Temp.length == 0) {
                            MyFilteredFields.push(item);
                        }
                    });
                    this.allFilterFieldsClass.AddFiltersList(MyFilteredFields, this.currentQuery.Id, myResult);

                }
                else {
                    this.allFilterFields = [];
                    this.constantFilterFieldsList = [];

                    this.allFilterFieldsClass.AddFiltersList([], this.currentQuery.Id, myResult);
                }


                this.filterFields.AddFiltersList(this.allFilterFields, this.currentQuery.Id, myResult);

                this.constantFilterFields.AddFiltersList(this.constantFilterFieldsList, this.currentQuery.Id, myResult);

                this.fieldsStaticList = this.allFilterFieldsClass.FilterFields;


                this.timeFrameFields = this.timeFilterFieldsClass.FilterFields;//fieldsStaticList.Where(d => d.ObjectField.IsTimeFrameFilter == true).ToList();
                var OFPM = new ObjectFieldPM();
                OFPM.FieldName = "NoFilter";
                OFPM.FullNameTextCodeCode = "No Filter";

                this.noFiltersField = new FilterField(OFPM, this.currentQuery.Id, this.isWindowViewMode, myResult);
                this.timeFrameFields.push(this.noFiltersField);

                this.advancedQueryFiltersList = myResult.filter(q => q.QueryCode == QueryCode);
                this.fillqueryfilters();
            }
        });

    }

    fillqueryfilters() {
        //if (this.advancedQueryFiltersList == undefined || this.advancedQueryFiltersList == null) {
        this.SelectedObjectFields = [];
        //}
        //else {
        this.advancedQueryFiltersList.forEach((item, key) => {
            this.objectField = window.ObjectFields.filter(o => o.FieldCode === item.ObjectFieldCode)[0];
            var xx = this.MapJsonToEntityPM(this.objectField);
            this.AddFilterField(xx);
        });
        //}
    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true) {

        if (mapParent === undefined) {
            mapParent = true;
        }

        var entityPM: ObjectFieldPM;
        entityPM = new ObjectFieldPM();

        if (jsonPM) {
            var jsonPMKeys = Object.keys(jsonPM);

            for (var key in jsonPMKeys) {
                if (jsonPMKeys[key] === "UIProperties") {

                    continue;
                }
                var property = jsonPMKeys[key];
                entityPM[property] = jsonPM[property];
            }
        }

        entityPM.IsDirty = false;
        return entityPM;
    }
    SelectedObjectFields: FilterField[];
    public AddFilterField(field: ObjectFieldPM) {
        if (this.SelectedObjectFields == undefined) {
            this.SelectedObjectFields = [];
        }
        var filters = this.AdvancedQueryFilterPMs.filter(d => d.ObjectFieldCode == field.FieldCode);
        if (filters != null && filters[0] != null && filters[0].IsPredefined == true) {
            var value = this.AdvancedQueryFilterPMs.filter(d => d.IsPredefined == true && d.ObjectFieldCode == field.FieldCode)[0].PredefinedValue;
            this.FieldsValues.SetFieldValue(field.Id, value);
        }
        if (this.SelectedObjectFields.filter(a => a.FieldName == field.FieldName).length == 0) {
            if (field.DataTypeCode != "Constant") {
                this.SelectedObjectFields.push(new FilterField(field, this.QueryCode, false, filters, this, this.pubSubService));
            }
        }
        if (this.SelectedObjectFields.length % 2 == 0) {
            this.Height = { "min-height": (this.SelectedObjectFields.length / 2 * 27) > 161 ? this.SelectedObjectFields.length / 2 * 27 + "px" : "161px" };
        }
        else {
            this.Height = { "min-height": (this.SelectedObjectFields.length + 1 / 2 * 27) > 161 ? this.SelectedObjectFields.length + 1 / 2 * 27 + "px" : "161px" };
        }
        if (this.SelectedObjectFields.length <= 1) {
            this.Height = { "min-height": "0px" };
        }
        //if (this.AdvancedQueryFilterPMs.filter(d => d.IsPredefined == true && d.ObjectFieldId == field.Id)[0] != null) {
        //    var value = this.AdvancedQueryFilterPMs.filter(d => d.IsPredefined == true && d.ObjectFieldId == field.Id)[0].PredefinedValue;
        //    this.FieldsValues.SetFieldValue(field.Id, value);
        //}
        this.allFilterFieldsClass.SetExists(field, true);
    }

    public RemoveFilterField(field: ObjectFieldPM) {
        if (this.SelectedObjectFields != null) {
            this.SelectedObjectFields = this.SelectedObjectFields.filter(a => a.ObjectField.Id != field.Id);
            if (this.SelectedObjectFields.length % 2 == 0) {
                this.Height = { "min-height": (this.SelectedObjectFields.length / 2 * 27) > 161 ? this.SelectedObjectFields.length / 2 * 27 + "px" : "161px" };
            }
            else {
                this.Height = { "min-height": (this.SelectedObjectFields.length + 1 / 2 * 27) > 161 ? this.SelectedObjectFields.length + 1 / 2 * 27 + "px" : "161px" };
            }
            if (this.SelectedObjectFields.length <= 1) {
                this.Height = { "min-height": "0px" };
            }
        }
    }


    OnQueryFilterChanged(QueryCode: string) {
        this.QueryCode = QueryCode;
        this.QueryFilterChangedAction(this.QueryCode);
        if (this.IsOpened) {
            this.SaveChangesAndRecreate();
        }
    }

    private searchText: string;
    public get SearchText() { return this.searchText; }
    public set SearchText(newValue: string) {
        this.searchText = newValue;
        if (newValue != null && newValue != "") {
            if (this.isWindowViewMode) {
                this.allFilterFieldsClass.FilterFields = this.fieldsStaticList.filter(f => TextCodeTranslator.Translate(f.ObjectField.FullNameTextCodeCode).toLowerCase().indexOf(newValue.toLowerCase()) > -1);
                this.SelectedObjectFields.forEach((item, key) => {
                    this.allFilterFieldsClass.SetExists(item, true);
                    this.filterFields.SetExists(item, true);
                });
                //foreach(ObjectFieldPM field in CurrentFilters)
                //{

                //    filterFields.SetExists(field, true);
                //}
            }

            else {
                this.allFilterFieldsClass.FilterFields = this.fieldsStaticList.filter(f => TextCodeTranslator.Translate(f.ObjectField.FullNameTextCodeCode).toLowerCase().indexOf(newValue.toLowerCase()) > -1);
                this.SelectedObjectFields.forEach((item, key) => {
                    this.allFilterFieldsClass.SetExists(item, true);
                    this.filterFields.SetExists(item, true);
                });
                //foreach(ObjectFieldPM field in CurrentFilters)
                //{
                //    allFilterFieldsClass.SetExists(field, true);
                //    filterFields.SetExists(field, true);
                //}
            }
        }
        else {
            this.allFilterFieldsClass.FilterFields = this.fieldsStaticList;
            this.SelectedObjectFields.forEach((item, key) => {
                this.allFilterFieldsClass.SetExists(item, true);
                this.filterFields.SetExists(item, true);
            });
        }


    }

    SaveChangesAndRecreate() {
        //searchControl.SaveChanges();
        //searchControl.SaveChangesCompleted += (ss, ee) => {

        //    DisposeSettingControls();
        //    CreateAdvanceControls();

        //};
    }

    public RunSave(field: FilterField) {

        //isSaving = true;
        //CancelAllOperations();


        if (this.isWindowViewMode) { // || isLocalSave
            //this.SelectedObjectFields.forEach((field, key) => {
            if (this.AdvancedQueryFilterPMs.filter(f => f.ObjectFieldCode == field.ObjectField.FieldCode && f.QueryCode == this.QueryCode)[0] != null) {
                var advanceFilter = this.AdvancedQueryFilterPMs.filter(f => f.ObjectFieldCode == field.ObjectField.FieldCode && f.QueryCode == this.QueryCode)[0]
                //if (this.FieldsValues.GetFieldValue(advanceFilter.ObjectFieldId) != null) {

                //    if (field.DataTypeCode == "DateTime" || field.DataTypeCode == "Date") {

                //        string date = this.FieldsValues.GetFieldValue(advanceFilter.ObjectFieldId).ToString();
                //        advanceFilter.PredefinedValue = date;
                //        //string[] datesArr = date.Split(',');
                //        //DateTime date1;
                //        //DateTime date2;
                //        //if (datesArr[0] != null)
                //        //{
                //        //    bool ss = DateTime.TryParse(datesArr[0], out  date1);
                //        //    if (ss)
                //        //    {
                //        //        advanceFilter.PredefinedValue = date1.ToString();
                //        //    }
                //        //    else
                //        //    {
                //        //        advanceFilter.PredefinedValue = null;
                //        //    }

                //        //}

                //        //if (datesArr.Count() > 1)
                //        //{
                //        //    bool ss = DateTime.TryParse(datesArr[1], out  date2);
                //        //    if (ss)
                //        //    {
                //        //        advanceFilter.PredefinedValue2 = date2.ToString();
                //        //    }
                //        //    else
                //        //    {
                //        //        advanceFilter.PredefinedValue2 = null;
                //        //    }

                //        //}


                //    }
                //    else {
                //        string valueString = this.FieldsValues.GetFieldValue(advanceFilter.ObjectFieldId).ToString();
                //        advanceFilter.PredefinedValue = valueString;
                //    }



                //    advanceFilter.IsPredefined = true;
                //}
                //else {
                advanceFilter.PredefinedValue = null;
                advanceFilter.IsPredefined = false;
                //}



            }

            //});
        }
        //this.SelectedObjectFields.forEach((field, key) => {
        if (this.AdvancedQueryFilterPMs.filter(f => f.ObjectFieldCode == field.ObjectField.FieldCode && f.QueryCode == this.QueryCode)[0] == null) {
            var value = this.FieldsValues.GetFieldValue(field.ObjectField.Id);
            var predefinedValue = null;
            var isPredifined = false;
            if (this.isWindowViewMode) {//|| isLocalSave
                if (value != null) {
                    predefinedValue = value.ToString();

                    isPredifined = true;
                }
            }



            var advanceFilter = new AdvancedQueryFilterPM();

            advanceFilter.Tenant = SessionInfo.LoggedUserTenant,
            advanceFilter.ObjectFieldId = field.ObjectField.Id;
            advanceFilter.QueryId = this.QueryId;
            advanceFilter.DataTypeCode = field.ObjectField.DataTypeCode;
            advanceFilter.DisplayInList = field.ObjectField.DisplayInList;
            advanceFilter.IsCustomFilter = field.ObjectField.IsCustomFilter;
            advanceFilter.ObjectFieldName = field.ObjectField.FieldName;
            advanceFilter.QueryCode = this.QueryCode;
            advanceFilter.QueryObjectTableName = this.currentQuery.ObjectTableName;
            advanceFilter.QueryUserId = this.currentQuery.UserId;
            advanceFilter.ObjectFieldOperator = field.ObjectField.Operator;
            advanceFilter.Operator = field.Operation.Code;
            advanceFilter.PredefinedValue = predefinedValue;
            advanceFilter.IsPredefined = isPredifined;
            advanceFilter.UserId = SessionInfo.LoggedUserId;
            advanceFilter.ObjectFieldCode = field.ObjectField.FieldCode;

            if (field.ObjectField.DataTypeCode == "DateTime" || field.ObjectField.DataTypeCode == "Date") {
                if (this.FieldsValues.GetFieldValue(advanceFilter.ObjectFieldCode) != null) {
                    //var date = this.FieldsValues.GetFieldValue(advanceFilter.ObjectFieldId).ToString();

                    //string[] datesArr = date.Split(',');
                    //DateTime date1;
                    //DateTime date2;
                    //if (datesArr[0] != null) {
                    //    bool ss = DateTime.TryParse(datesArr[0], out  date1);
                    //    if (ss) {
                    //        advanceFilter.PredefinedValue = date1.ToString();
                    //    }
                    //    else {
                    //        advanceFilter.PredefinedValue = null;
                    //    }

                    //}

                    //if (datesArr.Count() > 1) {
                    //    bool ss = DateTime.TryParse(datesArr[1], out  date2);
                    //    if (ss) {
                    //        advanceFilter.PredefinedValue2 = date2.ToString();
                    //    }
                    //    else {
                    //        advanceFilter.PredefinedValue2 = null;
                    //    }

                    //}
                }


            }

            var myService: AdvancedQueryFiltersPMService = new AdvancedQueryFiltersPMService();
            myService.setServiceArgs(this.serviceArgs);
            myService.insert(advanceFilter).subscribe((myResult:any) => {
                this.AdvancedQueryFilterPMs.push(myResult.Result);
            });

            //generalContext.AdvancedQueryFilterPMs.Add(advanceFilter);

        }

        //});


        //foreach(ObjectFieldPM field in removedFiltersList)
        //{
        //    AdvancedQueryFilterPM filter = (from a in generalContext.AdvancedQueryFilterPMs
        //    where a.QueryId == QueryChangedEventArgs.QueryId && a.ObjectFieldId == field.Id
        //    select a).FirstOrDefault();
        //    if (filter != null) {
        //        generalContext.AdvancedQueryFilterPMs.Remove(filter);
        //    }




        //}

        //if (isLocalSave) {
        //    TenantContext.Current.CurrentSession.StartBusyIndicator("Saving Changes");
        //}

        //TenantContext.Current.CurrentSession.StartBusyIndicator("Saving Changes");
        //HasChanges = false;
        //try {
        //    submitOp = generalContext.SubmitChanges();
        //    submitOp.Completed += new EventHandler(submit_Op_Completed);
        //}
        //catch { TenantContext.Current.CurrentSession.StopBusyIndicator(); }



    }

    public DeteteFilter(field: FilterField) {
        //if (this.AdvancedQueryFilterPMs.filter(f => f.ObjectFieldId == field.ObjectField.Id && f.QueryId == this.QueryId)[0] != null) {
        //var advanceFilter = this.AdvancedQueryFilterPMs.filter(f => f.ObjectFieldId == field.ObjectField.Id && f.QueryId == this.QueryId)[0];
        this.myAdvancedQueryFiltersPMService.getuseradvancedqueryfilterbytenantobjecttablequery(SessionInfo.LoggedUserTenant, field.ObjectField.FieldCode, field.QueryCode, SessionInfo.LoggedUserId).subscribe((filter: any) => {
            if (filter) {
                var myService: AdvancedQueryFiltersPMService = new AdvancedQueryFiltersPMService();
                myService.setServiceArgs(this.serviceArgs);
                myService.delete(filter).subscribe((myResult:any) => {
                    if (this.SelectedObjectFields != null) {
                        this.SelectedObjectFields = this.SelectedObjectFields.filter(a => a.ObjectField.Id != field.ObjectField.Id);
                    }
                    if (this.allFilterFieldsClass != null) {
                        //this.allFilterFieldsClass.SetDeleted(field, true);
                        this.allFilterFieldsClass.SetExists(field, false);
                    }
                    if (this.SelectedObjectFields.length % 2 == 0) {
                        this.Height = { "min-height": (this.SelectedObjectFields.length / 2 * 27) > 161 ? this.SelectedObjectFields.length / 2 * 27 + "px" : "161px" };
                    }
                    else {
                        this.Height = { "min-height": (this.SelectedObjectFields.length + 1 / 2 * 27) > 161 ? this.SelectedObjectFields.length + 1 / 2 * 27 + "px" : "161px" };
                    }
                    if (this.SelectedObjectFields.length <= 1) {
                        this.Height = { "min-height": "0px" };
                    }
                });
            }
            else {
                if (this.SelectedObjectFields != null) {
                    this.SelectedObjectFields = this.SelectedObjectFields.filter(a => a.ObjectField.Id != field.ObjectField.Id);
                }
                if (this.allFilterFieldsClass != null) {
                    //this.allFilterFieldsClass.SetDeleted(field, true);
                    this.allFilterFieldsClass.SetExists(field, false);
                }
                if (this.SelectedObjectFields.length % 2 == 0) {
                    this.Height = { "min-height": (this.SelectedObjectFields.length / 2 * 27) > 161 ? this.SelectedObjectFields.length / 2 * 27 + "px" : "161px" };
                }
                else {
                    this.Height = { "min-height": (this.SelectedObjectFields.length + 1 / 2 * 27) > 161 ? this.SelectedObjectFields.length + 1 / 2 * 27 + "px" : "161px" };
                }
                if (this.SelectedObjectFields.length <= 1) {
                    this.Height = { "min-height": "0px" };
                }
            }
        });
            
        //}
    }

    public EditViewClicked() {
        var windowArgs: any = {};
        windowArgs.queryCode = this.currentQuery.UniqueCode;
        windowArgs.queryId = this.currentQuery.Id;
        windowArgs.currentObjectTable = this.ObjectTableName;
        windowArgs.IsNew = false;
        windowArgs.QueryName = TextCodeTranslator.Translate(this.currentQuery.NameTextCodeCode);
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 960;
        logitudeWindow.Height = 610;
        logitudeWindow.Title = "Edit View";
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Infrastructure/Components/NewViewComponent/NewViewComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            var ObjectTable = window.ObjectTables.filter(x => x.Name === this.ObjectTableName)[0];
            var Query = window.Queries.filter(a => a.ObjectTableId === ObjectTable.Id && a.Id == $event)[0];
            // this.QueriesChangedEvent.emit(Query);
        });
    }

    FiltersBtnClicked() { 
        var temp = document.getElementById(this.AdvanceQuerySearchFieldsId) as HTMLInputElement;
        temp.value = "";
        this.SearchText = "";
        //temp.focus();
        //this.ClearPlaceHolder(); 
        //var ActiveElement = document.activeElement;
        //ActiveElement.blur();
        console.log(temp); 
        temp.focus();
        temp.focus();
        temp.select();
        //console.log(document.activeElement); 
    }

}



