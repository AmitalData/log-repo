declare var window: any;
import { Component, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AdvancedQueryFilterPM } from '../../../Infrastructure/EntityPMs/AdvancedQueryFilterPM';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { AdvancedQueryFiltersPMService } from '../../../Infrastructure/Services/StandardPMs/AdvancedQueryFiltersPMService';
import { QueriesPMService } from '../../../Infrastructure/Services/StandardPMs/QueriesPMService';
import { TextCodePMService } from '../../../Infrastructure/Services/StandardPMs/TextCodePMService';
import { GeneralEntitiesService } from '../../../Infrastructure/Services/StandardPMs/GeneralEntitiesService';
import { ServiceArgs } from '../../../Infrastructure/DataContracts/ServiceArgs';
import { ObjectFieldPM } from '../../../Infrastructure/EntityPMs/ObjectFieldPM';
import { QueryPM } from '../../../Infrastructure/EntityPMs/QueryPM';
import { FormGroup, FormBuilder } from '@angular/forms';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { QueryColumnDetails } from '../../../Infrastructure/Components/QueryColumnsComponents/QueryColumnsEditComponent';
import { QueryColumnPM } from '../../../Infrastructure/EntityPMs/QueryColumnPM';
import { FilterField, FilterFieldsClass, FieldsValues } from '../../../Infrastructure/Components/LogitudeComponents/QueryListComponent/FilterField';
import { GeneralEntitiesArgs } from '../../../Infrastructure/DataContracts/GeneralEntitiesArgs';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { PubSubService } from '../../../Infrastructure/Utilities/events/ApiFiltersEvent';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { CachedDataManager } from '../../../Infrastructure/Utilities/CachedDataManager';
import { ObjectsLocator } from '../../Locators/ObjectsLocator';
import { ServiceLocator } from '../../Locators/ServiceLocator';
import { CodeNameClass } from '../../DataContracts/CodeNameClass';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { ChooseUserArgs } from '../../../Infrastructure/Components/NewViewComponent/ChooseUserComponent';
import { FeatureLocator } from '../../Utilities/FeatureLocator';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { SharedUserQueryPM } from '../../EntityPMs/SharedUserQueryPM';
import { ApiQueryFilters } from '../../DataContracts/ApiQueryFilters';
import { UserList } from '../../../Common/EntityLists/UserList';
import { UserListService } from '../../../Common/Services/StandardLists/UserListService';
import { QueryColumnsPMService } from '../../Services/StandardPMs/QueryColumnsPMService';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'NewViewComponent',


    templateUrl: './NewViewComponent.html',
    inputs: ['ObjectTableName', 'event', 'isWindowViewMode', 'isNewViewMode', 'QueryId', 'QueryCode', 'Filterchangeevent', 'rabaia'],
    providers: [HttpClient, ServiceArgs],
})

export class NewViewComponent {
    public ViewNameId: any = null;
    public LayoutDirection: any = null;

    public EntityPM: QueryPM = null;
    RTL: boolean = ObjectsLocator.GlobalSetting == undefined ? false : (ObjectsLocator.GlobalSetting.LayoutDirection == 'rtl' ? true : false);
    @Output() onDataSourceChangedEvent = new EventEmitter();
    @Output() onUnSelectedDataLoadedEvent = new EventEmitter();
    @Output() onSelectedDataLoadedEvent = new EventEmitter();
    @Output() onUnSelectedDataSourceChangedEvent = new EventEmitter();
    SelectedTabCode: string;
    SearchFieldsId: string;
    FiltersSearchFieldsId: string;
    queryColumnsList: any[];
    unselectedObjectFields: any[];
    staticColumnsList: any[];
    addedQueryColumnList: any[];
    removedQueryColumnList: any[];
    unSelectedList: any[];
    OrderedQueryColumnsList: any[];
    unselected: any[];
    Fixedunselected: any[];
    QueryId: string;
    QueryCode: string;
    CurrentObjectTable: string;
    IsEnabled: boolean;
    ObjectTable: any;
    ObjectTableId: string;
    IsNew: boolean = true;
    CreateWithoutOriginalQuery: boolean = false;
    IsFromCustomization: boolean = false;
    SelectedDefaultViewName: string;
    CurrentObjectTableName: string;
    removedQueryFilters: any[];
    public myForm: FormGroup; 
    public SpotlightFeatureEnabled: boolean = false;
    GeneralEntitiesArgs: GeneralEntitiesArgs;
    pubSubAdvanceQueryFiltersService: PubSubService;
    ValidationErrorsList: string[] = [];
    CreateBtnText: string = TextCodeTranslator.Translate("General.B.Create");
    public serviceArgs: ServiceArgs;
    public _http: HttpClient;
    public BooleanValues = ["True", "False", "No Filter"];
    public ShareTabIsVisible: boolean = false;
    public IsSharedByMessageVisible: boolean = false;
    public IsSaveButtonEnabled: boolean = false;
    public IsSharedByVisible: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public ViewOnlyHelpText: string = "View only shared views can't be edited or changed";

    constructor(fb: FormBuilder, private CD: ChangeDetectorRef) {
        this.serviceArgs = new ServiceArgs();
        this.serviceArgs.http = ServiceHelper.HttpClient;
        this._http = ServiceHelper.HttpClient;
        this.removedQueryFilters = [];
        if (this.GeneralEntitiesArgs == null) {
            this.GeneralEntitiesArgs = new GeneralEntitiesArgs();
            this.GeneralEntitiesArgs.AdvancedQueryFilterPMs = [];
            this.GeneralEntitiesArgs.QueryColumnsPMs = [];
        }
        else {
            if (this.GeneralEntitiesArgs.AdvancedQueryFilterPMs == null) {
                this.GeneralEntitiesArgs.AdvancedQueryFilterPMs = [];
            }
            if (this.GeneralEntitiesArgs.QueryColumnsPMs == null) {
                this.GeneralEntitiesArgs.QueryColumnsPMs = [];
            }
        }
        if (this.CurrentSession == null) {
            this.SearchFieldsId = "SearchFields_-1_-1";
            this.FiltersSearchFieldsId = "FiltersSearchFieldsId_-1_-1";
        }


        else {
            this.SearchFieldsId = "NewViewSearchFields_" + this.CurrentSession.GetNewId("NewViewSearchFields");
            this.FiltersSearchFieldsId = "NewViewFiltersSearchFieldsId_" + this.CurrentSession.GetNewId("NewViewFiltersSearchFieldsId");
        }
        this.SelectedTabCode = "COL";

        this.myForm = fb.group({
            //'ShipperName': ['', Validators.required]

        });
    }

    SetWindowArgs(args: any) {
        this.IsNew = args.IsNew;
        this.SelectedDefaultViewName = args.SelectedDefaultViewName;
        this.CreateWithoutOriginalQuery = args.CreateWithoutOriginalQuery;
        this.IsFromCustomization = args.IsFromCustomization ? true : false;
        this.pubSubAdvanceQueryFiltersService = args.pubSubAdvanceQueryFiltersService;

        this.QueryId = args.queryId;
        this.QueryCode = args.queryCode;
        this.CurrentObjectTable = args.currentObjectTable;
        this.IsEnabled = false;
        this.ObjectTable = window.ObjectTables.filter((d: any) => d.Name == args.currentObjectTable)[0];
        this.ObjectTableId = this.ObjectTable.Id;
        this.CurrentObjectTableName = this.GetCurrentObjectTableName();
        this.ObjectFields = window.ObjectFields.filter((d: any) => d.ObjectTableId === this.ObjectTableId && d.CanFilter === true);
        this.filterFields = new FilterFieldsClass(true, this, this.pubSubAdvanceQueryFiltersService);
        this.NEWallFilterFieldsClass = new FilterFieldsClass(true, this, this.pubSubAdvanceQueryFiltersService);
        this.constantFilterFields = new FilterFieldsClass(true, this, this.pubSubAdvanceQueryFiltersService);


        if (!this.IsNew) {
            this.QueryName = args.QueryName;
            this.CreateBtnText = TextCodeTranslator.Translate("General.B.Save");

            //this.LoadQueryPM();
        }

        else {
            this.IsSaveButtonEnabled = true;
            this.CreateBtnText = TextCodeTranslator.Translate("General.B.Create");
            this.EntityPM = new QueryPM();
        }

        if (FeatureLocator.HasFeaturePermession("User", "User.Feature.ViewsSharing") && !this.IsFromCustomization) {
            this.ShareTabIsVisible = true;
        }

        if (this.ShareTabIsVisible) {
            this.LoadUsers();
            this.FillShareValuesList();
            this.SetSelectedSharedValue();
        }

        else {
            if (!this.IsNew) {
                this.LoadQueryPM();
            }
        }

        this.Run();
    }

    GetCurrentObjectTableName() {
        if (!this.ObjectTable) {
            return "";
        }

        return this.ObjectTable.FullNameTextCodeDefaultText;
    }

    private myUsersList: UserList[] = [];
    private LoadUsers() {
        var filters: ApiQueryFilters = new ApiQueryFilters();
        filters.SortBy = "EnglishName";
        filters.SortDirection = "Ascending";
        filters.PageIndex = 0;
        filters.PageSize = 5000;
        filters.Tenant = SessionLocator.Tenant;

        //filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");

        var userService: UserListService = new UserListService();
        userService.getByFilters(filters).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.myUsersList = pmResponse.Result;

                if (!this.IsNew) {
                    this.LoadQueryPM();
                }
            }
        });
    }

    private LoadQueryPM() {
        this.CurrentSession.StartBusyIndicatorLoading();
        var myService: QueriesPMService = new QueriesPMService();
        myService.setServiceArgs(this.serviceArgs);

        myService.get(this.QueryCode).subscribe((myResult: any) => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {
                this.EntityPM = myResponse.Result;

                if (this.EntityPM) {
                    this.ShareWithUsersCount = this.EntityPM.SharedUserQueries.length;
                    this.SharedByUserName = this.EntityPM.SharedByUserName;
                    this.SharedByUserEmail = this.EntityPM.SharedByUserEmail;

                    this.FillSharedWithUsersItemsSource();
                    this.SetSelectedSharedValue();
                    this.CheckEditSharedViewsFeature();

                    if (!AppTool.IsNullOrEmpty(this.EntityPM.SharedByUserId) && this.EntityPM.SharedByUserId != SessionLocator.LoggedUserId) {
                        this.IsSharedByVisible = true;
                    }

                    if (FeatureLocator.HasFeaturePermession("Shipment", "CSPV") && (this.EntityPM.ObjectTableName == "Shipment" || this.EntityPM.ObjectTableName == "Master")) {
                        this.SpotlightFeatureEnabled = true;
                    }

                    if (FeatureLocator.HasFeaturePermession("ARInvoice", "SSPV") && this.EntityPM.ObjectTableName == "ARInvoice") {
                        this.SpotlightFeatureEnabled = true;
                    }

                    this.ShowInSpotLight = this.EntityPM.SpotlightModeActivated;
                }

                else {
                    this.ValidationErrorsList.push("This View was deleted");
                }

                this.CurrentSession.StopBusyIndicator();
            }
            else {
                //show error
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

    CheckEditSharedViewsFeature(type: string = null) {
        var isEditEnabled = true;
        var isUpDownEnabled = false;
        var isAddEnabled = false;
        var isRemoveEnabled = false;

        if (this.EntityPM.SharedWithAll || this.EntityPM.SharedWithSpecificUsers) {
            if (this.EntityPM.SharedByUserId != SessionLocator.LoggedUserId) {
                if (!FeatureLocator.HasFeaturePermession("User", "User.Feature.EditSharedViews")) {
                    isEditEnabled = false
                }
            }
        }

        if (isEditEnabled) {
            if (type == "Selected") {
                isAddEnabled = false;
                isRemoveEnabled = true;
                isUpDownEnabled = true;
            }

            else if (type == "Available") {
                isAddEnabled = true;
                isRemoveEnabled = false;
                isUpDownEnabled = false;
            }
        }

        this.IsSharedByMessageVisible = !isEditEnabled;
        this.IsSaveButtonEnabled = isEditEnabled;

        this.IsbtnUpEnabled = isUpDownEnabled;
        this.IsbtnDownEnabled = isUpDownEnabled;

        this.IsbtnAddEnabled = isAddEnabled;
        this.IsbtnRemoveEnabled = isRemoveEnabled;
    }

    ClearPlaceHolder() {
        var temp = document.getElementById(this.SearchFieldsId) as HTMLInputElement;
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
    }

    FillPlaceHolder() {
        var temp = document.getElementById(this.SearchFieldsId) as HTMLInputElement;
        temp.placeholder = TextCodeTranslator.Translate("General.O.Search");
        temp.style.background = "url(Images/Search.png) no-repeat scroll";
        temp.style.backgroundPosition = "right center";
        temp.style.paddingRight = "30px";
    }

    ClearFiltersPlaceHolder() {
        var temp = document.getElementById(this.FiltersSearchFieldsId) as HTMLInputElement;
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
    }

    FillFiltersPlaceHolder() {
        var temp = document.getElementById(this.FiltersSearchFieldsId) as HTMLInputElement;
        temp.placeholder = TextCodeTranslator.Translate("General.O.Search");
        temp.style.background = "url(Images/Search.png) no-repeat scroll";
        temp.style.backgroundPosition = "right center";
        temp.style.paddingRight = "30px";
    }

    Run() {

        ServiceLocator.SendTotangoUserActivity("Customization", "QueryDefinition");
        var copy = false;

        var currentQuery = window.Queries.filter(d => d.UniqueCode == this.QueryCode)[0];


        this.addedQueryColumnList = [];
        this.removedQueryColumnList = [];
        currentQuery = this.GetCurrentQuery(currentQuery);
        if (currentQuery) {
            if (this.IsNew) {
                if (FeatureLocator.HasFeaturePermession("Shipment", "CSPV") && (currentQuery.ObjectTableName == "Shipment" || currentQuery.ObjectTableName == "Master")) {
                    this.SpotlightFeatureEnabled = true;
                }

                if (FeatureLocator.HasFeaturePermession("ARInvoice", "SSPV") && currentQuery.ObjectTableName == "ARInvoice") {
                    this.SpotlightFeatureEnabled = true;
                }

                this.ShowInSpotLight = currentQuery.SpotlightModeActivated;
            }


            this.IsViewOnly = currentQuery.IsViewOnly;
            this.IsDefault = currentQuery.IsDefault;


            this._http.get(ServiceHelper.GetLogitudeURL() + "api/ngMetaData?tenant=" + SessionInfo.LoggedUserTenant + "&queryCode=" + this.QueryCode + "&objecttableid=" + this.ObjectTable.Id + "&userid=" + SessionInfo.LoggedUserId + "&getfromsystemlevel=" + this.IsFromCustomization)
                .subscribe((response: any) => {
                    this.queryColumnsList = response;
                    this.queryColumnsList = this.queryColumnsList.sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 });

                    if (this.queryColumnsList.length == 0) {
                        var zeroColumnsList: any[] = [];
                        this._http.get(ServiceHelper.GetLogitudeURL() + "api/ngMetaData?tenant=0&queryCode=" + this.QueryCode + "&objecttableid=" + this.ObjectTable.Id + "&userid=null" + "&getfromsystemlevel=" + this.IsFromCustomization)
                            .subscribe((response: any) => {
                                zeroColumnsList = response;
                                zeroColumnsList = zeroColumnsList.sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 });
                                zeroColumnsList.forEach((querycolumn, key) => {
                                    var newcolumn = new QueryColumnPM();

                                    newcolumn.Tenant = SessionInfo.LoggedUserTenant;
                                    newcolumn.UserId = this.GetUserId();
                                    newcolumn.DisplayInList = querycolumn.DisplayInList;
                                    newcolumn.ObjectFieldName = querycolumn.ObjectFieldName;
                                    newcolumn.ColumnWidth = querycolumn.ColumnWidth;
                                    newcolumn.ConverterName = querycolumn.ConverterName;
                                    newcolumn.DataTemplateName = querycolumn.DataTemplateName;
                                    newcolumn.ColumnHeaderTemplateName = querycolumn.ColumnHeaderTemplateName;
                                    newcolumn.IndexOrder = querycolumn.IndexOrder;
                                    newcolumn.ObjectFieldDataTypeCode = querycolumn.ObjectFieldDataTypeCode;
                                    newcolumn.ObjectFieldFieldLableTextCodeDefaultText = querycolumn.ObjectFieldFieldLableTextCodeDefaultText;
                                    newcolumn.ObjectFieldId = querycolumn.ObjectFieldId;
                                    newcolumn.ObjectFieldListLabelTextCodeCode = querycolumn.ObjectFieldListLabelTextCodeCode;
                                    newcolumn.QueryCode = querycolumn.QueryCode;
                                    newcolumn.QueryId = querycolumn.QueryId;
                                    newcolumn.QueryObjectTableName = querycolumn.QueryObjectTableName;
                                    newcolumn.ObjectFieldFieldLableTextCodeCode = querycolumn.ObjectFieldFieldLableTextCodeCode;
                                    newcolumn.ObjectFieldCode = querycolumn.ObjectFieldCode;
                                    this.queryColumnsList.push(newcolumn);
                                    this.addedQueryColumnList.push(newcolumn);
                                });
                            });
                    }


                    this.staticColumnsList = this.queryColumnsList.filter(q => q.QueryCode == this.QueryCode && ((q.UserId == SessionInfo.LoggedUserId && q.Tenant == SessionInfo.LoggedUserTenant))).sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 });

                    var listColumns = this.queryColumnsList.filter(q => q.QueryCode == this.QueryCode && ((q.UserId == SessionInfo.LoggedUserId && q.Tenant == SessionInfo.LoggedUserTenant))).sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 });


                    this.unselectedObjectFields = window.ObjectFields.filter(a => a.ObjectTableName == this.CurrentObjectTable).filter(d => d.DisplayInList == true && (d.Tenant == SessionInfo.LoggedUserTenant || d.Tenant == 0) && ((d.ValidForQuerySection1 == currentQuery.QuerySection || d.ValidForQuerySection2 == currentQuery.QuerySection || (d.AdditionalQuerySections && d.AdditionalQuerySections.split(',').indexOf(currentQuery.QuerySection) > -1)) || d.IsCustom == true));


                    this.unselected = [];
                    this.unselectedObjectFields.forEach((field, key) => {
                        var xx = this.queryColumnsList.filter(q => q.QueryCode == this.QueryCode && q.ObjectFieldCode == field.FieldCode && field.FieldName != "TimeFrameFilter");
                        if (xx.length == 0) {
                            this.unselected.push(field);
                        }
                    });


                    //this.UnSelectedQueryColumnsList.ItemsSource = unselected.OrderBy(c => c.FieldName);
                    this.OrderedQueryColumnsList = [];
                    this.unSelectedList = this.unselected.sort((a, b) => { return (a.FieldName.toLowerCase() === b.FieldName.toLowerCase()) ? 0 : (a.FieldName.toLowerCase() < b.FieldName.toLowerCase()) ? -1 : 1 });
                    this.queryColumnsList.forEach((qc, key) => {
                        this.OrderedQueryColumnsList.push(new QueryColumnDetails(qc));
                    });

                    this.OrderedQueryColumnsList = this.OrderedQueryColumnsList.sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 });
                    this.CD.detectChanges();
                    //SelectedQueryColumnsList.ItemsSource = OrderedQueryColumnsList;
                    this.Fixedunselected = this.unSelectedList;

                    this.IsEnabled = true;
                    this.onUnSelectedDataLoadedEvent.emit(this.SelectedItem);
                    this.onSelectedDataLoadedEvent.emit(this.FieldSelectedItem);
                });
            this.QueryFilterChangedAction(this.QueryCode);
            var xx = this.NEWallFilterFieldsClass;
        }
    }
    GetCurrentQuery(currentQuery: any): any {
        if (!this.CreateWithoutOriginalQuery) return currentQuery;

        let newQueryPM = new QueryPM();
        newQueryPM.ObjectTableId = this.ObjectTable.Id;
        newQueryPM.ObjectTableName = this.ObjectTable.Name;
        newQueryPM.QuerySection = this.ObjectTable.Name;
        let customObjectTableQueryGroupCode = "CUOB";
        newQueryPM.QueryGroupCode = customObjectTableQueryGroupCode;
        newQueryPM.IsAddNewEntityEnabled = true;
        return newQueryPM;
    }

    public IsChooseUsersVisible: boolean = false;
    public ShareValuesList: CodeNameClass[] = [];
    private FillShareValuesList() {
        this.ShareValuesList = [];

        var obj1: CodeNameClass = new CodeNameClass();
        obj1.Code = "ALL";
        obj1.Name = "All Users";

        var obj2: CodeNameClass = new CodeNameClass();
        obj2.Code = "SPF";
        obj2.Name = "Specific Users";

        var obj3: CodeNameClass = new CodeNameClass();
        obj3.Code = "NON";
        obj3.Name = "None";

        this.ShareValuesList.push(obj1);
        this.ShareValuesList.push(obj2);
        this.ShareValuesList.push(obj3);
    }
    private SetSelectedSharedValue() {

        if (this.IsNew) {
            this.shareValueSelectedItem = this.ShareValuesList.filter(d => d.Code == "NON")[0];
        }

        else {
            if (this.EntityPM != null) {

                this.ShareWithUsersCount = this.EntityPM.SharedUserQueries.length;

                if (this.EntityPM.SharedWithAll) {
                    this.shareValueSelectedItem = this.ShareValuesList.filter(d => d.Code == "ALL")[0];
                }

                else if (this.EntityPM.SharedWithSpecificUsers) {
                    this.shareValueSelectedItem = this.ShareValuesList.filter(d => d.Code == "SPF")[0];
                    this.IsChooseUsersVisible = true;
                }

                else {
                    this.shareValueSelectedItem = this.ShareValuesList.filter(d => d.Code == "NON")[0];
                }
            }
        }
    }

    private shareValueSelectedItem: CodeNameClass;
    get ShareValueSelectedItem() { return this.shareValueSelectedItem; }
    set ShareValueSelectedItem(value: CodeNameClass) {
        if (this.shareValueSelectedItem != value) {
            this.shareValueSelectedItem = value;

            if (value.Code == "SPF") {
                this.IsChooseUsersVisible = true;
            }
            else {
                this.IsChooseUsersVisible = false;
            }
        }
    }





    public ShareWithUsersCount: number;
    public SharedByUserName: string;
    public SharedByUserEmail: string;
    ChooseUsers() {
        var args = new ChooseUserArgs();
        args.MyQuery = this.EntityPM;
        args.AllUsers = this.myUsersList;

        var logWindow = new LogitudeWindow();
        logWindow.Title = "Users List";
        logWindow.Width = 725;
        logWindow.Height = 520;
        logWindow.WindowArgs = args;
        logWindow.Show("./Infrastructure/Components/NewViewComponent/ChooseUserComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.ShareWithUsersCount = this.EntityPM.SharedUserQueries.length;
            this.FillSharedWithUsersItemsSource();
        });
    }

    public SharedWithUsersItemsSource: SharedWithUserItem[] = [];
    FillSharedWithUsersItemsSource() {
        this.SharedWithUsersItemsSource = [];

        this.EntityPM.SharedUserQueries.forEach((item) => {
            var user: UserList = this.myUsersList.filter(d => d.Id == item.UserId)[0];
            this.SharedWithUsersItemsSource.push(new SharedWithUserItem(item, user));
        });
    }

    DeleteUser(user: SharedWithUserItem) {
        var itemIndex = this.SharedWithUsersItemsSource.indexOf(user);
        if (itemIndex > -1) {
            this.SharedWithUsersItemsSource.splice(itemIndex, 1);
        }

        var index = this.EntityPM.SharedUserQueries.indexOf(user.myEnity);
        if (index > -1) {
            this.EntityPM.RemoveSharedUserQueryPM(user.myEnity);
        }

        this.ShareWithUsersCount = this.EntityPM.SharedUserQueries.length;
    }

    //txtSearch_TextChanged
    private searchText: string;
    public get SearchText() { return this.searchText; }
    public set SearchText(newValue: string) {
        this.searchText = newValue;
        if (newValue != null && newValue != "") {
            this.unSelectedList = this.Fixedunselected.filter(f => (f.ListTextCodeCode.toLowerCase().indexOf(newValue.toLowerCase()) > -1)
                || ((f.FullNameTextCodeLocalDefaultText != null && f.FullNameTextCodeLocalDefaultText != "") && f.FullNameTextCodeLocalDefaultText.toLowerCase().indexOf(newValue.toLowerCase()) > -1)
                || ((f.FullNameTextCodeDefaultText != null && f.FullNameTextCodeDefaultText != "") && f.FullNameTextCodeDefaultText.toLowerCase().indexOf(newValue.toLowerCase()) > -1));
        }
        else {
            this.unSelectedList = this.Fixedunselected;
        }
        this.onUnSelectedDataSourceChangedEvent.emit(this.unSelectedList);
        //this.onUnSelectedDataLoadedEvent.emit(this.SelectedItem);

    }
    private selectedItem: any;
    public get SelectedItem() { return this.selectedItem; }
    public set SelectedItem(newValue: any) {
        this.selectedItem = newValue;
    }

    private queryName: string;
    public get QueryName() { return this.queryName; }
    public set QueryName(newValue: string) {
        this.queryName = newValue;
    }

    private showInSpotLight: boolean;
    public get ShowInSpotLight() { return this.showInSpotLight; }
    public set ShowInSpotLight(newValue: boolean) {
        if (this.showInSpotLight != newValue) {
            this.showInSpotLight = newValue;
        }
    }

    private fieldSelectedItem: any;
    public get FieldSelectedItem() { return this.fieldSelectedItem; }
    public set FieldSelectedItem(newValue: any) {
        this.fieldSelectedItem = newValue;
    }

    private isbtnAddEnabled: boolean = true;
    public get IsbtnAddEnabled() { return this.isbtnAddEnabled; }
    public set IsbtnAddEnabled(newValue: boolean) {
        this.isbtnAddEnabled = newValue;
    }

    private isbtnRemoveEnabled: boolean = true;
    public get IsbtnRemoveEnabled() { return this.isbtnRemoveEnabled; }
    public set IsbtnRemoveEnabled(newValue: boolean) {
        this.isbtnRemoveEnabled = newValue;
    }

    private isbtnUpEnabled: boolean = true;
    public get IsbtnUpEnabled() { return this.isbtnUpEnabled; }
    public set IsbtnUpEnabled(newValue: boolean) {
        this.isbtnUpEnabled = newValue;
    }

    private isbtnDownEnabled: boolean = true;
    public get IsbtnDownEnabled() { return this.isbtnDownEnabled; }
    public set IsbtnDownEnabled(newValue: boolean) {
        this.isbtnDownEnabled = newValue;
    }


    private isViewOnly: boolean;
    public get IsViewOnly() { return this.isViewOnly; }
    public set IsViewOnly(newValue: boolean) {
        if (this.isViewOnly != newValue) {
            this.isViewOnly = newValue;
        }
    }

    private isDefault: boolean;
    public get IsDefault() { return this.isDefault; }
    public set IsDefault(newValue: boolean) {
        if (this.isDefault != newValue) {
            this.isDefault = newValue;
        }
    }



    onSelectedItemChanged(item) {
        this.SelectedItem = item;
        this.onUnSelectedDataLoadedEvent.emit(null);
        this.CheckEditSharedViewsFeature("Selected");
    }

    onFieldSelectedItemChanged(item) {
        this.FieldSelectedItem = item;
        this.onSelectedDataLoadedEvent.emit(null);
        this.CheckEditSharedViewsFeature("Available");
    }

    btnUp_Click() {

        var item = this.SelectedItem;
        if (item != null) {
            var i = this.OrderedQueryColumnsList.indexOf(item);

            this.ReorderColumnsList();
            //var upColumn = this.OrderedQueryColumnsList.filter(d => d.ObjectFieldId == item.ObjectFieldId && ((d.Tenant == SessionInfo.LoggedUserTenant && d.UserId == SessionInfo.LoggedUserId) || d.Tenant == 0) && d.QueryId == this.QueryId)[0];
            var upColumn = this.OrderedQueryColumnsList.filter(d => d.ObjectFieldCode == item.ObjectFieldCode)[0];

            if (i > 0) {
                this.OrderedQueryColumnsList = this.OrderedQueryColumnsList.filter(d => d.ObjectFieldCode != upColumn.ObjectFieldCode);
                this.OrderedQueryColumnsList.filter(o => o.IndexOrder == i - 1)[0].IndexOrder = i;
                upColumn.IndexOrder = i - 1;
                this.OrderedQueryColumnsList.splice(i - 1, 0, upColumn);
                this.onDataSourceChangedEvent.emit(this.OrderedQueryColumnsList);
                //this.ReorderColumnsList();
                this.onSelectedDataLoadedEvent.emit(this.SelectedItem);
            }
        }

    }

    btnDown_Click() {
        var item = this.SelectedItem;
        if (item != null) {

            var i = this.OrderedQueryColumnsList.indexOf(item);

            this.ReorderColumnsList();

            //var downColumn = this.OrderedQueryColumnsList.filter(d => d.ObjectFieldId == item.ObjectFieldId && ((d.Tenant == SessionInfo.LoggedUserTenant && d.UserId == SessionInfo.LoggedUserId) || d.Tenant == 0) && d.QueryId == this.QueryId)[0];
            var downColumn = this.OrderedQueryColumnsList.filter(d => d.ObjectFieldCode == item.ObjectFieldCode)[0];

            if (i < this.OrderedQueryColumnsList.length - 1) {
                this.OrderedQueryColumnsList = this.OrderedQueryColumnsList.filter(d => d.ObjectFieldCode != downColumn.ObjectFieldCode);

                this.OrderedQueryColumnsList.filter(o => o.IndexOrder == i + 1)[0].IndexOrder = i;
                downColumn.IndexOrder = i + 1;
                this.OrderedQueryColumnsList.splice(i + 1, 0, downColumn);
                this.onDataSourceChangedEvent.emit(this.OrderedQueryColumnsList);
                //this.ReorderColumnsList();
                this.onSelectedDataLoadedEvent.emit(this.SelectedItem);
            }
        }

    }

    btnAdd_Click() {
        if (this.FieldSelectedItem != null) {

            var field = this.FieldSelectedItem;

            var queryColumn = this.unSelectedList.filter(a => a.QueryCode == this.QueryCode && a.FieldName == field.FieldName)[0];
            if (queryColumn) {
                this.removedQueryColumnList = this.removedQueryColumnList.filter(a => a.FieldName != queryColumn.FieldName);
            }
            if (queryColumn) {
                if (queryColumn.Id) {
                    var newQueryColumn = new QueryColumnPM();

                    newQueryColumn.QueryCode = this.QueryCode;
                    newQueryColumn.QueryId = this.QueryId;
                    newQueryColumn.ObjectFieldId = field.Id;
                    newQueryColumn.ObjectFieldName = field.FieldName;
                    newQueryColumn.ObjectFieldFieldLableTextCodeDefaultText = field.FullNameTextCodeDefaultText;
                    newQueryColumn.Tenant = SessionInfo.LoggedUserTenant;
                    newQueryColumn.IndexOrder = (this.OrderedQueryColumnsList.length > 0 ? this.OrderedQueryColumnsList[this.OrderedQueryColumnsList.length - 1].IndexOrder + 1 : 0);
                    newQueryColumn.ColumnWidth = 100;
                    newQueryColumn.ConverterName = field.ConverterName;
                    newQueryColumn.DataTemplateName = field.DataTemplateName;
                    newQueryColumn.ObjectFieldListLabelTextCodeCode = field.ListTextCodeCode;
                    newQueryColumn.DisplayInList = true;
                    newQueryColumn.UserId = this.GetUserId();
                    newQueryColumn.ObjectFieldFieldLableTextCodeCode = field.FullNameTextCodeCode;
                    newQueryColumn.ObjectFieldCode = field.FieldCode;

                    this.OrderedQueryColumnsList.push(new QueryColumnDetails(newQueryColumn));
                    this.addedQueryColumnList.push(newQueryColumn);
                }

                else {
                    queryColumn.IndexOrder = (this.OrderedQueryColumnsList.length > 0 ? this.OrderedQueryColumnsList[this.OrderedQueryColumnsList.length - 1].IndexOrder + 1 : 0);
                    this.OrderedQueryColumnsList.push(new QueryColumnDetails(queryColumn));
                }
            }

            else {
                var newQueryColumn = new QueryColumnPM();

                newQueryColumn.QueryCode = this.QueryCode;
                newQueryColumn.QueryId = this.QueryId;
                newQueryColumn.ObjectFieldId = field.Id;
                newQueryColumn.ObjectFieldName = field.FieldName;
                newQueryColumn.ObjectFieldFieldLableTextCodeDefaultText = field.FullNameTextCodeDefaultText;
                newQueryColumn.Tenant = SessionInfo.LoggedUserTenant;
                newQueryColumn.IndexOrder = (this.OrderedQueryColumnsList.length > 0 ? this.OrderedQueryColumnsList[this.OrderedQueryColumnsList.length - 1].IndexOrder + 1 : 0);
                newQueryColumn.ColumnWidth = 100;
                newQueryColumn.ConverterName = field.ConverterName;
                newQueryColumn.DataTemplateName = field.DataTemplateName;
                newQueryColumn.ObjectFieldListLabelTextCodeCode = field.ListTextCodeCode;
                newQueryColumn.DisplayInList = true;
                newQueryColumn.UserId = this.GetUserId();
                newQueryColumn.ObjectFieldFieldLableTextCodeCode = field.FullNameTextCodeCode;
                newQueryColumn.ObjectFieldCode = field.FieldCode;

                this.OrderedQueryColumnsList.push(new QueryColumnDetails(newQueryColumn));
                this.addedQueryColumnList.push(newQueryColumn);
            }

            this.unSelectedList = this.unSelectedList.filter(a => a.FieldName != field.FieldName);
            this.IsbtnAddEnabled = false;
            this.CD.detectChanges();


            if (this.SearchText != null && this.SearchText != "") {
                this.unSelectedList = this.unSelectedList.filter(f => f.FullNameTextCodeDefaultText.toLowerCase().indexOf(this.SearchText.toLowerCase()) >= 0 || ((f.FullNameTextCodeLocalDefaultText != null && f.FullNameTextCodeLocalDefaultText != "") && f.FullNameTextCodeLocalDefaultText.toLowerCase().indexOf(this.SearchText.toLowerCase())));
            }

            this.onUnSelectedDataLoadedEvent.emit(this.SelectedItem);
        }

        this.onSelectedDataLoadedEvent.emit(this.FieldSelectedItem);
        this.FieldSelectedItem = null;
        this.SelectedItem = null;
    }

    GetUserId(): string {
        return this.IsFromCustomization ? null : SessionLocator.LoggedUserId;
    }

    btnRemove_Click() {
        if (this.SelectedItem) {
            var queryColumn = this.SelectedItem;

            var objectField = window.ObjectFields.filter(a => a.FieldCode == queryColumn.ObjectFieldCode)[0];

            this.unSelectedList.push(objectField);
            //-----
            this.OrderedQueryColumnsList = this.OrderedQueryColumnsList.filter(a => a.ObjectFieldCode != queryColumn.ObjectFieldCode);


            var pm = this.queryColumnsList.filter(a => a.ObjectFieldCode == queryColumn.ObjectFieldCode && a.QueryCode == this.QueryCode)[0];

            if (pm != null) {
                this.removedQueryColumnList.push(pm);
            }

            var queryColumn2 = this.addedQueryColumnList.filter(a => a.ObjectFieldCode == queryColumn.ObjectFieldCode && a.QueryCode == this.QueryCode)[0];

            if (queryColumn2 != null) {
                this.addedQueryColumnList = this.addedQueryColumnList.filter(a => a.ObjectFieldCode != queryColumn2.ObjectFieldCode);
            }

            this.OrderedQueryColumnsList.forEach((column, key) => {
                if (column.IndexOrder > queryColumn.IndexOrder) {
                    column.IndexOrder--;
                }
            });

            this.IsbtnRemoveEnabled = false;
            this.CD.detectChanges();
            this.onUnSelectedDataLoadedEvent.emit(this.SelectedItem);
            this.onSelectedDataLoadedEvent.emit(this.FieldSelectedItem);
            this.FieldSelectedItem = null;
            this.SelectedItem = null;
        }
    }

    ReorderColumnsList() {
        var queryColumnList = this.OrderedQueryColumnsList.sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 });
        //.filter(d => d.Id != null && d.QueryId == this.QueryId && d.UserId == SessionInfo.LoggedUserId && d.Tenant == SessionInfo.LoggedUserTenant)
        //var TenantZeroqueryColumnList = this.OrderedQueryColumnsList.filter(d => d.QueryId == this.QueryId && d.UserId == null && d.Tenant == 0).sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 });

        var i = 0;
        for (; i < queryColumnList.length; i++) {
            queryColumnList[i].IndexOrder = i;
        }
        //var queryColumnList = this.OrderedQueryColumnsList.filter(d => d.QueryId == this.QueryId && d.UserId == SessionInfo.LoggedUserId && d.Tenant == SessionInfo.LoggedUserTenant).sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 });
        //var i = 0;
        //for (; i < queryColumnList.length; i++) {
        //    queryColumnList[i].IndexOrder = i;
        //}

    }

    CancelButtonClicked() {
        this.CurrentSession.CurrentWindow.Close("");
    }

    DeleteButtonClicked() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Title = TextCodeTranslator.Translate("General.O.DeletQuery");

        if (this.EntityPM.UserId == SessionInfo.LoggedUserId) {
            confirmWindow.Show(TextCodeTranslator.Translate("General.M.WantToDeleteThisQuery"));
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.DoDelete(SessionInfo.LoggedUserId);
                }
            });
        }

        else {
            confirmWindow.Show("This view has been shared with users other than you. Are you sure you want to delete it?");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.DoDelete(this.EntityPM.SharedByUserId);
                }
            });
        }
    }
    private DoDelete(userId: string) {
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        var query = window.Queries.filter(q => q.UniqueCode == this.EntityPM.UniqueCode)[0];

        var myService: QueriesPMService = new QueriesPMService();
        myService.setServiceArgs(this.serviceArgs);
        var myGeneralService: GeneralEntitiesService = new GeneralEntitiesService();
        myService.delete(query, userId).subscribe((myResult: any) => {
            this.CurrentSession.StopBusyIndicator();
            window.Queries = window.Queries.filter(a => a.UniqueCode != query.UniqueCode);
            var ObjectTable = window.ObjectTables.filter(x => x.Name === this.CurrentObjectTable)[0];
            var Query = window.Queries.filter(a => a.ObjectTableId === ObjectTable.Id && a.IndexOrder == 0)[0];

            this.CurrentSession.CloseCurrentWindow();
        });









        //this.GeneralEntitiesArgs = new GeneralEntitiesArgs();
        //this.GeneralEntitiesArgs.RemovedQueryColumnsPMs = [];
        //this.GeneralEntitiesArgs.RemovedQueryFilters = [];
        //var ObjectTable = window.ObjectTables.filter(a => a.Name == this.CurrentObjectTable)[0];

        //this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        //this.GeneralEntitiesArgs.Tenant = SessionInfo.LoggedUserTenant;
        //var myQCService: QueryColumnsPMService = new QueryColumnsPMService();
        //myQCService.setServiceArgs(this.serviceArgs);
        //myQCService.GetQueryColumnPMs(SessionInfo.LoggedUserTenant, this.EntityPM.Id, ObjectTable.Id, userId).subscribe((myResult:any) => {
        //    var queryColumns = myResult;

        //    queryColumns.forEach((column, key) => {
        //        this.GeneralEntitiesArgs.RemovedQueryColumnsPMs.push(column);
        //    });

        //    if (this.myAdvancedQueryFiltersPMService == null) {
        //        this.myAdvancedQueryFiltersPMService = new AdvancedQueryFiltersPMService();
        //    }

        //    this.myAdvancedQueryFiltersPMService.setServiceArgs(this.serviceArgs);
        //    this.myAdvancedQueryFiltersPMService.getadvancedqueryfiltersbytenantByQuery(SessionInfo.LoggedUserTenant, userId, this.EntityPM.Id).subscribe((myResult:any) => {
        //        if (myResult == null) {
        //            this.AdvancedQueryFilterPMs = [];
        //        }

        //        else {
        //            this.AdvancedQueryFilterPMs = myResult;
        //            var advanceQueryFilters = this.AdvancedQueryFilterPMs.filter(c => c.QueryId == this.EntityPM.Id);
        //            advanceQueryFilters.forEach((filter, key) => {
        //                this.GeneralEntitiesArgs.RemovedQueryFilters.push(filter);
        //            });
        //        }

        //        var query = window.Queries.filter(q => q.Id == this.EntityPM.Id)[0];
        //        var myService: QueriesPMService = new QueriesPMService();
        //        myService.setServiceArgs(this.serviceArgs);
        //        var myGeneralService: GeneralEntitiesService = new GeneralEntitiesService();
        //        myGeneralService.setServiceArgs(this.serviceArgs);
        //        myGeneralService.update(this.GeneralEntitiesArgs).subscribe((myResult:any) => {
        //            myService.delete(query).subscribe((myResult:any) => {
        //                this.CurrentSession.StopBusyIndicator();
        //                window.Queries = window.Queries.filter(a => a.Id != query.Id);
        //                var ObjectTable = window.ObjectTables.filter(x => x.Name === this.CurrentObjectTable)[0];
        //                var Query = window.Queries.filter(a => a.ObjectTableId === ObjectTable.Id && a.IndexOrder == 0)[0];

        //                this.CurrentSession.CloseCurrentWindow();
        //            });
        //        });
        //    });
        //});
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    NEWallFilterFieldsClass: FilterFieldsClass;
    filterFields: FilterFieldsClass;
    constantFilterFields: FilterFieldsClass;
    public temp: FilterField[];

    allFilterFields: ObjectFieldPM[];// = new List<ObjectFieldPM>();
    constantFilterFieldsList: ObjectFieldPM[];// = new List<ObjectFieldPM>();
    CurrentFilters: ObjectFieldPM[];// = new List<ObjectFieldPM>();
    advancedQueryFiltersList: AdvancedQueryFilterPM[]; //List<AdvancedQueryFilterPM> 
    fieldsStaticList: FilterField[];//FilterField

    timeFilterFieldsClass: FilterFieldsClass = new FilterFieldsClass(false, this, this.pubSubAdvanceQueryFiltersService);
    timeFrameFields: FilterField[];
    noFiltersField: FilterField;
    objectField: ObjectFieldPM;
    private myAdvancedQueryFiltersPMService: AdvancedQueryFiltersPMService;
    public AdvancedQueryFilterPMs: AdvancedQueryFilterPM[];
    public ObjectFields: any;
    public FieldsValues: FieldsValues;
    currentQuery: QueryPM;
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

        this.myAdvancedQueryFiltersPMService.getadvancedqueryfiltersbytenantByQuery(SessionInfo.LoggedUserTenant, SessionInfo.LoggedUserId, QueryCode).subscribe((myResult: any) => {
            this.GetFiltersComplete(myResult, QueryCode);
        });
    }

    GetFiltersComplete(myResult: any, QueryCode: any) {
        if (myResult == null) {
            this.AdvancedQueryFilterPMs = myResult = [];
        }

        else {
            this.AdvancedQueryFilterPMs = myResult;
        }
        this.timeFilterFieldsClass = new FilterFieldsClass(true, this, this.pubSubAdvanceQueryFiltersService);
        this.FieldsValues = new FieldsValues();
        this.currentQuery = window.Queries.filter(q => q.UniqueCode == QueryCode)[0];
        this.currentQuery = this.GetCurrentQuery(this.currentQuery);

        if (!this.currentQuery) {

        }

        else {

            if (this.currentQuery.DisplayAsCustom) {
                //btnEditView.IsEnabled = true;
            }

            if (this.ObjectFields) {

                this.allFilterFields = window.ObjectFields.filter(o => o.CanFilter == true && o.DataTypeCode != "Constant" && ((o.ValidForQuerySection1 == this.currentQuery.QuerySection || o.ValidForQuerySection2 == this.currentQuery.QuerySection) || (o.AdditionalQuerySections && o.AdditionalQuerySections.split(',').indexOf(this.currentQuery.QuerySection) > -1) || (o.ObjectTableId == this.ObjectTable.Id && o.IsCustom == true)));

                this.constantFilterFieldsList = window.ObjectFields.filter(o => o.CanFilter == true && o.DataTypeCode == "Constant" && ((o.ValidForQuerySection1 == this.currentQuery.QuerySection || o.ValidForQuerySection2 == this.currentQuery.QuerySection) || (o.AdditionalQuerySections && o.AdditionalQuerySections.split(',').indexOf(this.currentQuery.QuerySection) > -1) || (o.ObjectTableId == this.ObjectTable.Id && o.IsCustom == true)));

                this.timeFilterFieldsClass.AddFiltersList(window.ObjectFields.filter(o => o.CanFilter == true && o.FieldName != "TimeFrameFilter" && o.IsTimeFrameFilter == true && (o.ValidForQuerySection1 == this.currentQuery.QuerySection || o.ValidForQuerySection2 == this.currentQuery.QuerySection || (o.AdditionalQuerySections && o.AdditionalQuerySections.split(',').indexOf(this.currentQuery.QuerySection) > -1)  )), this.currentQuery.UniqueCode, myResult);

                this.NEWallFilterFieldsClass.AddFiltersList(window.ObjectFields.filter(o => o.CanFilter == true && o.DataTypeCode != "Constant" && ((o.ValidForQuerySection1 == this.currentQuery.QuerySection || o.ValidForQuerySection2 == this.currentQuery.QuerySection) || (o.AdditionalQuerySections && o.AdditionalQuerySections.split(',').indexOf(this.currentQuery.QuerySection) > -1) || (o.ObjectTableId == this.ObjectTable.Id && o.IsCustom == true))), this.currentQuery.UniqueCode, myResult);

            }
            else {
                this.allFilterFields = [];
                this.constantFilterFieldsList = [];

                this.NEWallFilterFieldsClass.AddFiltersList([], this.currentQuery.UniqueCode, myResult);
            }


            this.filterFields.AddFiltersList(this.allFilterFields, this.currentQuery.UniqueCode, myResult);

            this.constantFilterFields.AddFiltersList(this.constantFilterFieldsList, this.currentQuery.UniqueCode, myResult);

            this.fieldsStaticList = this.NEWallFilterFieldsClass.FilterFields;


            this.timeFrameFields = this.timeFilterFieldsClass.FilterFields;//fieldsStaticList.Where(d => d.ObjectField.IsTimeFrameFilter == true).ToList();
            var OFPM = new ObjectFieldPM();
            OFPM.FieldName = "NoFilter";
            OFPM.FullNameTextCodeCode = "No Filter";

            this.noFiltersField = new FilterField(OFPM, this.currentQuery.UniqueCode, true, myResult, this, this.pubSubAdvanceQueryFiltersService);
            this.timeFrameFields.push(this.noFiltersField);
            //if (!this.IsNew) {
            this.advancedQueryFiltersList = myResult.filter(q => q.QueryCode == QueryCode);
            //}
            //else {
            //    this.advancedQueryFiltersList = [];
            //}
            this.fillqueryfilters();
        }
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

        var entityPM: ObjectFieldPM;
        entityPM = new ObjectFieldPM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }

        entityPM.IsDirty = false;
        return entityPM;
    }
    SelectedObjectFields: FilterField[];
    public AddFilterField(field: ObjectFieldPM) {

        if (this.SelectedObjectFields == undefined) {
            this.SelectedObjectFields = [];
        }
        var filters = this.AdvancedQueryFilterPMs.filter(d => d.IsPredefined == true && d.ObjectFieldCode == field.FieldCode);
        if (filters != null && filters[0] != null) {
            var value = this.AdvancedQueryFilterPMs.filter(d => d.IsPredefined == true && d.ObjectFieldCode == field.FieldCode)[0].PredefinedValue;
            this.FieldsValues.SetFieldValue(field.Id, value);
        }
        if (!this.IsNew) {
            this.SelectedObjectFields.push(new FilterField(field, this.QueryCode, true, filters, this, this.pubSubAdvanceQueryFiltersService));
        }
        else {
            this.SelectedObjectFields.push(new FilterField(field, "", true, filters, this, this.pubSubAdvanceQueryFiltersService));
        }
        if (this.SelectedObjectFields.length > 10) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push("The max. number of filters you can use is 10");
            this.NEWallFilterFieldsClass.SetExists(field, false);
            return;
        }
        this.NEWallFilterFieldsClass.SetExists(field, true);
    }

    public RemoveFilterField(field: ObjectFieldPM) {
        if (this.SelectedObjectFields != null) {
            this.SelectedObjectFields = this.SelectedObjectFields.filter(a => a.ObjectField.Id != field.Id);
        }
    }

    OnQueryFilterChanged(QueryCode: string) {
        this.QueryCode = QueryCode;
        this.QueryFilterChangedAction(this.QueryCode);
        //if (this.IsOpened) {
        //    this.SaveChangesAndRecreate();
        //}
    }

    private filterssearchText: string;
    public get FilterssearchText() { return this.filterssearchText; }
    public set FilterssearchText(newValue: string) {
        this.filterssearchText = newValue;
        if (newValue != null && newValue != "") {

            this.NEWallFilterFieldsClass.FilterFields = this.fieldsStaticList.filter(f => TextCodeTranslator.Translate(f.ObjectField.FullNameTextCodeCode).toLowerCase().indexOf(newValue.toLowerCase()) > -1);
            this.SelectedObjectFields.forEach((item, key) => {
                this.NEWallFilterFieldsClass.SetExists(item, true);
                this.filterFields.SetExists(item, true);
            });

        }
        else {
            this.NEWallFilterFieldsClass.FilterFields = this.fieldsStaticList;
            this.SelectedObjectFields.forEach((item, key) => {
                this.NEWallFilterFieldsClass.SetExists(item, true);
                this.filterFields.SetExists(item, true);
            });
        }


    }

    public DeteteFilter(field: FilterField) {
        //if (this.AdvancedQueryFilterPMs.filter(f => f.ObjectFieldId == field.ObjectField.Id && f.QueryId == this.QueryId)[0] != null) {
        //    var advanceFilter = this.AdvancedQueryFilterPMs.filter(f => f.ObjectFieldId == field.ObjectField.Id && f.QueryId == this.QueryId)[0];
        //var myService: AdvancedQueryFiltersPMService = new AdvancedQueryFiltersPMService();
        //myService.setServiceArgs(this.serviceArgs);
        //myService.delete(advanceFilter).subscribe((myResult:any) => {
        if (this.SelectedObjectFields != null) {
            this.removedQueryFilters.push(this.SelectedObjectFields.filter(a => a.ObjectField.Id == field.ObjectField.Id)[0]);
            this.SelectedObjectFields = this.SelectedObjectFields.filter(a => a.ObjectField.Id != field.ObjectField.Id);
        }
        if (this.NEWallFilterFieldsClass != null) {
            this.NEWallFilterFieldsClass.SetExists(field, false);
        }
        //});
        //}
    }

    public CLearAll() {

        if (this.NEWallFilterFieldsClass != null) {
            this.SelectedObjectFields.forEach((field, key) => {
                this.NEWallFilterFieldsClass.SetExists(field, false);
            });
        }
        if (this.SelectedObjectFields != null) {
            this.SelectedObjectFields = [];
        }

    }

    btnCreateNewQuery_Click() {
        this.ValidateQuery(this.QueryName);
        if (this.ValidationErrorsList.length != 0) return;
        if (!AppTool.IsNullOrEmpty(this.SelectedDefaultViewName) && this.IsDefault) {
            this.OpenIsDefaultWarningWindow();
            return;
        }

        if (this.IsNew) {
            this.SavePredifinedQuery(this.QueryName);
            return;
        }

        this.SaveAndClose(this.QueryName);
    }

    OpenIsDefaultWarningWindow() {
        let confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Please notice that the default view already exists (" + this.SelectedDefaultViewName + ") will be changed, ok?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes && this.IsNew) {
                this.SavePredifinedQuery(this.QueryName);
                return;
            }
            if (confirmWindow.Yes) {
                this.SaveAndClose(this.QueryName);
            }
        });
    }

    SavePredifinedQuery(queryName: string) {
        var myService: QueriesPMService = new QueriesPMService();
        var textCodesService: TextCodePMService = new TextCodePMService();

        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));

        var theCurrentQuery = (window.Queries.filter(q => q.UniqueCode == this.QueryCode)[0]);
        theCurrentQuery = this.GetCurrentQuery(theCurrentQuery);
        var temp = window.Queries.filter(q => q.ObjectTableId == theCurrentQuery.ObjectTableId && q.UserId == theCurrentQuery.UserId && q.QueryGroupCode == theCurrentQuery.QueryGroupCode).sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 });
        var maxIndex = temp[temp.length - 1];

        this.EntityPM.QuerySection = theCurrentQuery.QuerySection;
        this.EntityPM.ObjectTableId = theCurrentQuery.ObjectTableId;
        this.EntityPM.ObjectTableName = theCurrentQuery.ObjectTableName;
        this.EntityPM.Tenant = SessionInfo.LoggedUserTenant;
        this.EntityPM.UserId = this.GetUserId();
        this.EntityPM.Code = "111";
        this.EntityPM.IndexOrder = this.GetNewQueryIndexOrder(maxIndex);
        this.EntityPM.ObjectTableIsNewWizard = theCurrentQuery.ObjectTableIsNewWizard;
        this.EntityPM.ObjectTableNewWizardControlName = theCurrentQuery.ObjectTableNewWizardControlName;
        this.EntityPM.OriginalQueryCode = theCurrentQuery.UniqueCode;
        this.EntityPM.QueryGroupCode = theCurrentQuery.QueryGroupCode;
        this.EntityPM.IsAddNewEntityEnabled = theCurrentQuery.IsAddNewEntityEnabled;
        this.EntityPM.NewViewName = queryName;
        this.EntityPM.Perspective = theCurrentQuery.Perspective;
        this.EntityPM.EditWizardName = theCurrentQuery.EditWizardName;
        this.EntityPM.SpotlightModeActivated = this.ShowInSpotLight;
        this.EntityPM.IsViewOnly = this.IsViewOnly;
        this.EntityPM.IsDefault = this.IsDefault;
        this.EntityPM.IsFromCustomObjectTable = this.ObjectTable.IsCustom;
        this.EntityPM.SystemLevel = this.IsFromCustomization;

        if (this.ShareValueSelectedItem) {
            switch (this.ShareValueSelectedItem.Code) {
                case "ALL": {
                    this.EntityPM.SharedWithAll = true;
                    this.EntityPM.SharedWithSpecificUsers = false;
                    this.EntityPM.SharedByUserId = SessionLocator.LoggedUserId;

                    if (this.EntityPM.SharedUserQueries != null && this.EntityPM.SharedUserQueries.length > 0) {
                        for (var i = this.EntityPM.SharedUserQueries.length - 1; i >= 0; i--) {
                            var item = this.EntityPM.SharedUserQueries[i];
                            this.EntityPM.RemoveSharedUserQueryPM(item);
                        }
                    }
                    break;
                }

                case "SPF": {
                    this.EntityPM.SharedWithAll = false;
                    this.EntityPM.SharedWithSpecificUsers = true;
                    this.EntityPM.SharedByUserId = SessionLocator.LoggedUserId;
                    break;
                }

                case "NON": {
                    this.EntityPM.SharedWithAll = false;
                    this.EntityPM.SharedWithSpecificUsers = false;
                    this.EntityPM.SharedByUserId = null;

                    if (this.EntityPM.SharedUserQueries != null && this.EntityPM.SharedUserQueries.length > 0) {
                        for (var i = this.EntityPM.SharedUserQueries.length - 1; i >= 0; i--) {
                            var item = this.EntityPM.SharedUserQueries[i];
                            this.EntityPM.RemoveSharedUserQueryPM(item);
                        }
                    }
                    break;
                }
            }
        }

        var spotlightTemplate: string = "";
        if (this.EntityPM.SpotlightModeActivated) {
            if (this.SpotlightFeatureEnabled) {
                switch (this.EntityPM.ObjectTableName) {
                    case "Shipment": {
                        spotlightTemplate = "ShipmentSpotlightDataTemplate";
                        break;
                    }
                    case "Master": {
                        spotlightTemplate = "MasterSpotlightDataTemplate";
                        break;
                    }

                    case "ARInvoice": {
                        spotlightTemplate = "ARInvoiceSpotlightDataTemplate";
                        break;
                    }
                }
                this.EntityPM.SpotlightDataTemplate = spotlightTemplate;
            }
        }
        else {
            this.EntityPM.SpotlightDataTemplate = null;
        }

        myService.setServiceArgs(this.serviceArgs);
        textCodesService.setServiceArgs(this.serviceArgs);
        myService.insert(this.EntityPM).subscribe((myResult: any) => {
            textCodesService.getByCode(myResult.Result.NameTextCodeCode, myResult.Result.Tenant).subscribe((res: any) => {
                window.TextCodesTranslations.push(res);
                this.AddFiltersAndColumns(myResult.Result);
                window.Queries.push(myResult.Result);
            });
        });


    }

    private GetNewQueryIndexOrder(maxIndex: any): number {
        if (!this.CreateWithoutOriginalQuery) return maxIndex.IndexOrder + 1;

        let objectTableQueries = window.Queries.filter(q => q.ObjectTableId == this.ObjectTableId);
        if (objectTableQueries && objectTableQueries.length > 0) {
            return objectTableQueries.length;
        }
        return 0;
    }

    AddFiltersAndColumns(newQuery) {
        var myGeneralService: GeneralEntitiesService = new GeneralEntitiesService();
        var columns = this.OrderedQueryColumnsList;
        columns.forEach((column, key) => {
            var newColumn = new QueryColumnPM();

            newColumn.IndexOrder = column.IndexOrder;
            newColumn.ObjectFieldId = column.ObjectFieldId;
            newColumn.QueryId = this.IsNew ? newQuery.Id : this.QueryId;
            newColumn.QueryCode = this.IsNew ? newQuery.UniqueCode : this.QueryCode;
            newColumn.Tenant = newQuery.Tenant;
            newColumn.ColumnWidth = column.ColumnWidth;
            newColumn.ConverterName = column.ConverterName;
            newColumn.DataTemplateName = column.DataTemplateName;
            newColumn.ObjectFieldFieldLableTextCodeDefaultText = column.ObjectFieldFieldLableTextCodeDefaultText;
            newColumn.ObjectFieldListLabelTextCodeCode = column.ObjectFieldListLabelTextCodeCode;
            newColumn.ObjectFieldName = column.ObjectFieldName;
            //newColumn.QueryCode = column.QueryCode;
            newColumn.QueryObjectTableName = column.QueryObjectTableName;
            newColumn.DisplayInList = true;
            newColumn.ObjectFieldCode = column.ObjectFieldCode;

            if (!AppTool.IsNullOrEmpty(this.EntityPM.SharedByUserId)) {
                newColumn.UserId = this.EntityPM.SharedByUserId;
            }
            else {
                newColumn.UserId = this.GetUserId();
            }

            this.GeneralEntitiesArgs.QueryColumnsPMs.push(newColumn);
        });

        this.SelectedObjectFields.forEach((field, key) => {
            if (field.TextValue != null) {
                var advanceFilter = new AdvancedQueryFilterPM();

                advanceFilter.Tenant = SessionInfo.LoggedUserTenant;
                advanceFilter.ObjectFieldId = field.ObjectField.Id;
                advanceFilter.QueryId = this.IsNew ? newQuery.Id : this.QueryId;
                advanceFilter.QueryCode = this.IsNew ? newQuery.UniqueCode : this.QueryCode;
                advanceFilter.DataTypeCode = field.ObjectField.DataTypeCode;
                advanceFilter.DisplayInList = field.ObjectField.DisplayInList;
                advanceFilter.IsCustomFilter = field.ObjectField.IsCustomFilter;
                advanceFilter.ObjectFieldName = field.ObjectField.FieldName;
                //advanceFilter.QueryCode = this.currentQuery.Code;
                advanceFilter.QueryObjectTableName = this.currentQuery.ObjectTableName;
                advanceFilter.QueryUserId = this.IsFromCustomization ? null : this.currentQuery.UserId;
                advanceFilter.ObjectFieldOperator = field.ObjectField.Operator;
                advanceFilter.Operator = field.Operation.Code;
                advanceFilter.PredefinedValue = field.TextValue;
                advanceFilter.IsPredefined = true;
                advanceFilter.ObjectFieldCode = field.ObjectField.FieldCode;
                if (!AppTool.IsNullOrEmpty(this.EntityPM.SharedByUserId)) {
                    advanceFilter.UserId = this.EntityPM.SharedByUserId;
                }
                else {
                    advanceFilter.UserId = this.GetUserId();
                }

                if (!AppTool.IsNullOrEmpty(field.MyName)) {
                    advanceFilter.PredefinedValue = field.MyName;
                }
                else if (field.ObjectField.DataTypeCode == "DateTime") {
                    var date = this.FieldsValues.GetFieldValue(field.ObjectField.Id);
                    if (field.TextValue != null && date != null && field.Operation.Code != "LargerThan" && field.Operation.Code != "LessThan") {
                        advanceFilter.PredefinedValue = date;
                    }

                }
                if (field.ObjectField.DataTypeCode == "DateTime" && field.Operation.Code == "Between") {
                    advanceFilter.PredefinedValue2 = field.TextValue1;
                }

                this.GeneralEntitiesArgs.AdvancedQueryFilterPMs.push(advanceFilter);

            }
            else {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("General.M.SomeFiltersHaveNoValue"));
                return;
            }
        });

        this.GeneralEntitiesArgs.Tenant = SessionInfo.LoggedUserTenant;
        myGeneralService.setServiceArgs(this.serviceArgs);
        if (this.IsNew) {
            myGeneralService.insert(this.GeneralEntitiesArgs).subscribe((myResult: any) => {
                myResult.Result.AdvancedQueryFilterPMs.forEach((filter, key) => {
                    window.PreDefinedFilters.push(filter);
                });
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                this.CurrentSession.CurrentWindow.Close(newQuery.UniqueCode);
            });
        }
        else {
            myGeneralService.update(this.GeneralEntitiesArgs).subscribe((myResult: any) => {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                this.CurrentSession.CurrentWindow.Close(newQuery.UniqueCode);
            });
        }
    }

    SaveAndClose(queryName) {
        var myService: QueriesPMService = new QueriesPMService();
        var textCodesService: TextCodePMService = new TextCodePMService();

        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        this.EntityPM.NewViewName = queryName;
        this.EntityPM.IsViewOnly = this.IsViewOnly;
        this.EntityPM.IsDefault = this.IsDefault;

        this.EntityPM.SpotlightModeActivated = this.ShowInSpotLight;
        var spotlightTemplate: string = "";
        if (this.EntityPM.SpotlightModeActivated) {
            if (this.ShowInSpotLight) {
                switch (this.EntityPM.ObjectTableName) {
                    case "Shipment": {
                        spotlightTemplate = "ShipmentSpotlightDataTemplate";
                        break;
                    }
                    case "Master": {
                        spotlightTemplate = "MasterSpotlightDataTemplate";
                        break;
                    }

                    case "ARInvoice": {
                        spotlightTemplate = "ARInvoiceSpotlightDataTemplate";
                        break;
                    }
                }
                this.EntityPM.SpotlightDataTemplate = spotlightTemplate;
            }
        }
        else {
            this.EntityPM.SpotlightDataTemplate = null;
        }

        if (this.ShareValueSelectedItem) {
            switch (this.ShareValueSelectedItem.Code) {
                case "ALL": {
                    this.EntityPM.SharedWithAll = true;
                    this.EntityPM.SharedWithSpecificUsers = false;

                    if (AppTool.IsNullOrEmpty(this.EntityPM.SharedByUserId)) {
                        this.EntityPM.SharedByUserId = SessionInfo.LoggedUserId;
                    }

                    if (this.EntityPM.SharedUserQueries != null && this.EntityPM.SharedUserQueries.length > 0) {
                        for (var i = this.EntityPM.SharedUserQueries.length - 1; i >= 0; i--) {
                            var item = this.EntityPM.SharedUserQueries[i];
                            this.EntityPM.RemoveSharedUserQueryPM(item);
                        }
                    }
                    break;
                }

                case "SPF": {
                    this.EntityPM.SharedWithAll = false;
                    this.EntityPM.SharedWithSpecificUsers = true;

                    if (AppTool.IsNullOrEmpty(this.EntityPM.SharedByUserId)) {
                        this.EntityPM.SharedByUserId = SessionInfo.LoggedUserId;
                    }
                    break;
                }

                case "NON": {
                    this.EntityPM.SharedWithAll = false;
                    this.EntityPM.SharedWithSpecificUsers = false;
                    this.EntityPM.SharedByUserId = null;

                    if (this.EntityPM.SharedUserQueries != null && this.EntityPM.SharedUserQueries.length > 0) {
                        for (var i = this.EntityPM.SharedUserQueries.length - 1; i >= 0; i--) {
                            var item = this.EntityPM.SharedUserQueries[i];
                            this.EntityPM.RemoveSharedUserQueryPM(item);
                        }
                    }
                    break;
                }
            }
        }

        myService.setServiceArgs(this.serviceArgs);
        textCodesService.setServiceArgs(this.serviceArgs);
        myService.update(this.EntityPM).subscribe((myResult: any) => {
            if (!SessionLocator.UseCachedData) {
                textCodesService.getByCode(myResult.Result.NameTextCodeCode, myResult.Result.Tenant).subscribe((res: any) => {
                    window.TextCodesTranslations = window.TextCodesTranslations.filter(a => a.TextCodeCode != myResult.Result.NameTextCodeCode);
                    window.TranslationsCache = window.TranslationsCache.filter(d => d.Code != myResult.Result.NameTextCodeCode);
                    window.TextCodesTranslations.push(res);
                    window.TranslationsCache.push(res);
                    this.UpdateColumnsAndFilters();
                    var CurrentQuery = window.Queries.filter(x => x.UniqueCode == this.QueryCode)[0];
                    CurrentQuery = this.EntityPM;
                });
            }
            else {
                CachedDataManager.RefreshTenantTextCodes().subscribe((response: any) => {
                    this.UpdateColumnsAndFilters();
                    var CurrentQuery = window.Queries.filter(x => x.UniqueCode == this.QueryCode)[0];
                    CurrentQuery = this.EntityPM;
                });
            }

            var oldItem = window.Queries.filter(t => t.Code == this.EntityPM.Code)[0];
            if (oldItem) {
                var index = window.Queries.indexOf(oldItem);
                window.Queries.splice(index, 1);
                window.Queries.push(this.EntityPM);
            }
        });

    }

    ValidateQuery(queryName: string) {
        this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(queryName)) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("General.M.QueryNamecannotBeEmpty"));
        }
        if (!AppTool.IsNullOrEmpty(queryName) && queryName.length > 30) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("General.M.QueryNameLength"));
        }

        if (!this.SelectedObjectFields || this.SelectedObjectFields.length == 0) return;
        this.SelectedObjectFields.forEach((item, key) => {
            if ((item.TextValue == null || item.TextValue == "" || item.TextValue == undefined) && item.ObjectField.DataTypeCode != "Boolean") {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("General.M.SomeFiltersHaveNoValue"));
                return;
            }
        });
    }

    UpdateColumnsAndFilters() {
        var myGeneralService: GeneralEntitiesService = new GeneralEntitiesService();
        this.GeneralEntitiesArgs.QueryColumnsPMs = [];
        this.GeneralEntitiesArgs.RemovedQueryColumnsPMs = [];
        this.GeneralEntitiesArgs.RemovedQueryFilters = [];
        this.removedQueryColumnList.forEach((queryColumn, key) => {
            this.GeneralEntitiesArgs.RemovedQueryColumnsPMs.push(queryColumn);
        });

        this.OrderedQueryColumnsList.forEach((queryColumn, key) => {
            this.GeneralEntitiesArgs.QueryColumnsPMs.push(queryColumn);
        });

        this.removedQueryFilters.forEach((item, key) => {
            if (item != null && item.AdvancedQueryFilterPM != null) {
                this.GeneralEntitiesArgs.RemovedQueryFilters.push(item.AdvancedQueryFilterPM);
                window.PreDefinedFilters = window.PreDefinedFilters.filter(a => a.ObjectFieldCode != item.AdvancedQueryFilterPM.ObjectFieldCode);
                this.AdvancedQueryFilterPMs = this.AdvancedQueryFilterPMs.filter(a => a.ObjectFieldCode != item.AdvancedQueryFilterPM.ObjectFieldCode);
            }
        });

        this.SelectedObjectFields.forEach((item, key) => {
            if (item.AdvancedQueryFilterPM != null) {
                if (item.ObjectField.DataTypeCode != "DateTime" && item.ObjectField.DataTypeCode != "Date") {
                    item.AdvancedQueryFilterPM.PredefinedValue = item.TextValue;
                }

                else if (!AppTool.IsNullOrEmpty(item.MyName)) {
                    item.AdvancedQueryFilterPM.PredefinedValue = item.MyName;
                }

                else {
                    if (item.Operation.Code == "LargerThan" || item.Operation.Code == "LessThan") {
                        var myDate: Date = new Date(item.TextValue.toString());
                        var temp = DateTool.GetDateParts(myDate);
                        var dt = temp.Year + "-" + temp.Month + "-" + temp.Day;
                        item.AdvancedQueryFilterPM.PredefinedValue = dt;
                    }

                    if (item.Operation.Code == "Between") {
                        //var myDate: Date = new Date(item.TextValue1.toString());
                        //var temp = DateTool.GetDateParts(myDate);
                        //var dt = temp.Year + "-" + temp.Month + "-" + temp.Day;
                        //item.AdvancedQueryFilterPM.PredefinedValue2 = dt;
                        item.AdvancedQueryFilterPM.PredefinedValue2 = item.TextValue1;
                    }
                    else {
                        item.AdvancedQueryFilterPM.PredefinedValue2 = null;
                    }
                }

                item.AdvancedQueryFilterPM.Operator = item.Operation.Code;
                this.GeneralEntitiesArgs.AdvancedQueryFilterPMs.push(item.AdvancedQueryFilterPM);
            }
            else {
                var advanceFilter = new AdvancedQueryFilterPM();

                advanceFilter.Tenant = SessionInfo.LoggedUserTenant;
                advanceFilter.ObjectFieldId = item.ObjectField.Id;
                advanceFilter.QueryId = this.QueryId;
                advanceFilter.QueryCode = this.QueryCode;
                advanceFilter.DataTypeCode = item.ObjectField.DataTypeCode;
                advanceFilter.DisplayInList = item.ObjectField.DisplayInList;
                advanceFilter.IsCustomFilter = item.ObjectField.IsCustomFilter;
                advanceFilter.ObjectFieldName = item.ObjectField.FieldName;
                //advanceFilter.QueryCode = this.currentQuery.Code;
                advanceFilter.QueryObjectTableName = this.currentQuery.ObjectTableName;
                advanceFilter.QueryUserId = this.IsFromCustomization ? null : this.currentQuery.UserId;
                advanceFilter.ObjectFieldOperator = item.ObjectField.Operator;
                advanceFilter.Operator = item.Operation.Code;
                advanceFilter.PredefinedValue = item.TextValue;
                advanceFilter.IsPredefined = true;
                advanceFilter.ObjectFieldCode = item.ObjectField.FieldCode;


                if (!AppTool.IsNullOrEmpty(this.EntityPM.SharedByUserId)) {
                    advanceFilter.UserId = this.EntityPM.SharedByUserId;
                }
                else {
                    advanceFilter.UserId = this.GetUserId();
                }

                if (!AppTool.IsNullOrEmpty(item.MyName)) {
                    advanceFilter.PredefinedValue = item.MyName;
                }
                else if (item.ObjectField.DataTypeCode == "DateTime") {

                    var date = this.FieldsValues.GetFieldValue(item.ObjectField.Id);
                    if (item.TextValue != null && date != null) {
                        advanceFilter.PredefinedValue = date;
                    }
                    if (item.Operation.Code == "Between") {
                        //var myDate: Date = new Date(item.TextValue1.toString());
                        //var temp = DateTool.GetDateParts(myDate);
                        //var dt = temp.Year + "-" + temp.Month + "-" + temp.Day;//myDate.getDay() + "-" + (myDate.getMonth() + 1) + "-" + myDate.getFullYear();
                        //advanceFilter.PredefinedValue2 = dt;
                        advanceFilter.PredefinedValue2 = item.TextValue1;
                    }
                }

                this.GeneralEntitiesArgs.AdvancedQueryFilterPMs.push(advanceFilter);
            }
        });

        this.GeneralEntitiesArgs.Tenant = SessionInfo.LoggedUserTenant;
        myGeneralService.setServiceArgs(this.serviceArgs);
        myGeneralService.update(this.GeneralEntitiesArgs).subscribe((myResult: any) => {
            myResult.Result.AdvancedQueryFilterPMs.forEach((filter, key) => {
                if (window.PreDefinedFilters.filter(o => o.Id === filter.Id).length == 0) {
                    window.PreDefinedFilters.push(filter);
                }

                else {
                    window.PreDefinedFilters = window.PreDefinedFilters.filter(o => o.Id != filter.Id);
                    window.PreDefinedFilters.push(filter);
                }
            });

            this.removedQueryFilters.forEach((item, key) => {
                if (item != null && item.AdvancedQueryFilterPM != null) {
                    window.PreDefinedFilters = window.PreDefinedFilters.filter(a => a.Id != item.AdvancedQueryFilterPM.Id);
                }
            });

            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            this.CurrentSession.CurrentWindow.Close(this.QueryCode);
        });
    }
}

export class SharedWithUserItem {
    public myEnity: SharedUserQueryPM;
    private myUser: UserList;
    constructor(entity: SharedUserQueryPM, user: UserList) {
        this.myEnity = entity;
        this.myUser = user;
    }

    get Email() { return this.myUser.Email; }
    get Name() { return this.myUser.EnglishName; }



}
