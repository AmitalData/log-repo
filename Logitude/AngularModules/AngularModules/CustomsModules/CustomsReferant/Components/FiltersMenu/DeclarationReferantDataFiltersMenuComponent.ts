import { Component, AfterViewInit, Output, EventEmitter, ContentChild, ViewChild, ViewChildren, QueryList, ChangeDetectorRef } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters, FilterItem } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { MultiSelectLOVComponent } from '../../../../Infrastructure/Components/LogitudeComponents/MultiSelectLOVComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { UserList } from '../../../../Common/EntityLists/UserList';
import { AppTool } from '../../../../Infrastructure/Tools';
import { AmitalGatewayUtil, UnifreightMessageM } from '../../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { UserListService } from '../../../../Common/Services/StandardLists/UserListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DepartmentList } from '../../../../Common/EntityLists/DepartmentList';
import { AdvancedQueryFilterPM } from 'Infrastructure/EntityPMs/AdvancedQueryFilterPM';
import { field } from 'Customs/EntityPMs/Extended/AmendmentView';
import { AdvancedQueryFiltersPMService } from 'Infrastructure/Services/StandardPMs/AdvancedQueryFiltersPMService';
import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';
import { DeclarationReferantDataWebService } from 'Customs/Services/WebServices/DeclarationReferantDataWebService';
import { ObjectFieldPMExtendedService } from 'Infrastructure/Services/ExtendedPMs/ObjectFieldPMExtendedService';
import { QueriesPMService } from 'Infrastructure/Services/StandardPMs/QueriesPMService';

@Component({
    selector: 'DeclarationReferantDataFiltersMenuComponent',
    templateUrl: './DeclarationReferantDataFiltersMenuComponent.html',
})

export class DeclarationReferantDataFiltersMenuComponent
    extends BaseComponent
    implements AfterViewInit {
    @Output() SelectedValueChanged = new EventEmitter();
    apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
    public itmImportDeclarationReferantDatas: boolean = false;
    public DirectionWidth: number = 140;
    public UserFilers: ApiQueryFilters;
    public apiQueryFiltersChanged: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext: DeclarationReferantDataFiltersMenuComponent = this;
    public ObjectTableName: string = "Customs.DeclarationReferantData";
    TransportFilter_A: string;
    TransportFilter_O: string;
    TransportFilter_I: string;
    @ViewChildren(MultiSelectLOVComponent)
    public myViewChildrenMultiSelectLOVComponent: QueryList<MultiSelectLOVComponent> = null;

    constructor(private _CD: ChangeDetectorRef) {
        super();
        if (this.CurrentSession == null) {
            this.TransportFilter_A = "TransportFilter_A_-1_-1";
            this.TransportFilter_O = "TransportFilter_O_-1_-1";
            this.TransportFilter_I = "TransportFilter_I_-1_-1";
        } else {
            var index_T = this.CurrentSession.GetNewId("ShipmentTransportFilterMenu");
            this.TransportFilter_A = "TransportFilter_A" + index_T;
            this.TransportFilter_O = "TransportFilter_O" + index_T;
            this.TransportFilter_I = "TransportFilter_I" + index_T;
        }
        if (FeatureLocator.HasFeaturePermession("DeclarationReferantData", "IMPORTSHIPMETNS")) {
            this.itmImportDeclarationReferantDatas = true;
            this.DirectionWidth = 170;
        }
        else {
            this.itmImportDeclarationReferantDatas = false;
            this.DirectionWidth = 140;
        }
    }

    public OpenQueryThruWorkSpace: boolean = false;
    public CurrentScreenIsWorkSpace: boolean = false;
    public UserFilters: ApiQueryFilters = new ApiQueryFilters();
    public UserNameFilters: ApiQueryFilters = new ApiQueryFilters();
    public DepartmentFilters: ApiQueryFilters = new ApiQueryFilters();
    public DepartmentNameFilters: ApiQueryFilters = new ApiQueryFilters();
    public TransportFilters: ApiQueryFilters = new ApiQueryFilters();
    public QueryId: string = "";
    public QueryCode: string = "Customs.DeclarationReferantData.AllCases";

    SetFiltersMenu(args: any) {
        this.OpenQueryThruWorkSpace = true;
        this.TransportFilters = new ApiQueryFilters();
        this.UserFilters = new ApiQueryFilters();
        this.UserNameFilters = new ApiQueryFilters();
        this.DepartmentFilters = new ApiQueryFilters();
        this.DepartmentNameFilters = new ApiQueryFilters();
        if (args.AdditionalFilters) {
            this.TransportFilters.AdditionalFilters = args.AdditionalFilters.filter(a => a.FieldName == "TransportModeId");
            this.UserFilters.AdditionalFilters = args.AdditionalFilters.filter(a => a.FieldName == "ReferentUserId");
            this.UserNameFilters.AdditionalFilters = args.AdditionalFilters.filter(a => a.FieldName == "ReferantUserName");
            this.DepartmentFilters.AdditionalFilters = args.AdditionalFilters.filter(a => a.FieldName == "DepartmentId");
            this.DepartmentNameFilters.AdditionalFilters = args.AdditionalFilters.filter(a => a.FieldName == "DepartmentName");
            this.CurrentScreenIsWorkSpace = false;
        }
        if (args.Filters) {
            this.CurrentScreenIsWorkSpace = true;
            this.LOVListUsers = [];
            this.LOVListDepartment = [];
            this.TransportFilters.AdditionalFilters = args.Filters.filter(a => a.FieldName == "TransportModeId");
            this.UserFilters.AdditionalFilters = args.Filters.filter(a => a.FieldName == "ReferentUserId");
            this.UserNameFilters.AdditionalFilters = args.Filters.filter(a => a.FieldName == "ReferantUserName");
            this.DepartmentFilters.AdditionalFilters = args.Filters.filter(a => a.FieldName == "DepartmentId");
            this.DepartmentNameFilters.AdditionalFilters = args.Filters.filter(a => a.FieldName == "DepartmentName");
            //this.apiQueryFilters.addAdditionalFilter("RetrievData", true, null, null, "Equal", true, false, false, "string", this._LOVListUsers.length == 0);
            this.QueryId = args.QuerySection;
            if (!this.QueryId) {
                var objectFieldPMExtendedService: ObjectFieldPMExtendedService = new ObjectFieldPMExtendedService();
                objectFieldPMExtendedService.getSingleFromQueries(this.QueryCode).subscribe((result: any) => {
                    if (result) {
                        this.QueryId = result.Id;
                    }
                });
                this.applyBasicFiltes();
            }
        }

        var myService: UserListService = new UserListService();
        var UserListFromFilters = this.UserFilters.AdditionalFilters.map(({ FieldValue }) => FieldValue);
        var UserNameListFromFilters = this.UserNameFilters.AdditionalFilters.map(({ FieldValue }) => FieldValue);

        var UserNameList;
        if (UserNameListFromFilters.length > 0 && UserNameListFromFilters[0] != null)
            UserNameList = UserNameListFromFilters[0].split("%2C");


        var DepartmentFromFilters = this.DepartmentFilters.AdditionalFilters.map(({ FieldValue }) => FieldValue);
        var DepartmentNameFromFilters = this.DepartmentNameFilters.AdditionalFilters.map(({ FieldValue }) => FieldValue);
        var DepartmentNameList;
        if (DepartmentNameFromFilters.length > 0 && DepartmentNameFromFilters[0] != null)
            DepartmentNameList = DepartmentNameFromFilters[0].split("%2C");

        var i = 0;
        var myService: UserListService = new UserListService();
        if (UserListFromFilters[0] != "HowCare" && UserListFromFilters.length != 0 && UserListFromFilters[0] != null ) {
            UserListFromFilters[0].split("%2C").forEach(function (value) {
                let ul = new UserList();
                ul.Id = value;
                if (UserNameList) {
                    ul.LocalName = decodeURIComponent(UserNameList[i]);
                    this.LOVListUsers.push(ul)
                }
                i++;
            }, this);
        } else {


        }
        i = 0;
        if (DepartmentFromFilters[0] != "HowCare" && DepartmentFromFilters.length != 0 && DepartmentFromFilters[0] != null) {
            DepartmentFromFilters[0].split("%2C").forEach(function (value) {
                let ul = new DepartmentList();
                ul.Id = value;
                if (DepartmentNameList) {
                    ul.LocalName = decodeURIComponent(DepartmentNameList[i]);
                    this.LOVListDepartment.push(ul)
                }
                i++;
                this._CD.detectChanges();
               // this.apiQueryFilters.addAdditionalFilter("DepartmentName", this.DepartmentNamesListString, null, null, "Equal", true, false, false, "string", true);
               // this.apiQueryFilters.addAdditionalFilter("DepartmentId", this.DepartmentListString, null, null, "InListExact", false, false, false, "string", this.LOVListDepartment.length == 0);

            }, this);
        } else {
        }

        if (this.TransportFilters.AdditionalFilters.length > 0) {
            if (this.TransportFilters.AdditionalFilters[0].FieldValue == null) {
                this.SetTransport("All");
            } else {
                this.SetTransport(this.TransportFilters.AdditionalFilters.map(({ FieldValue }) => FieldValue).toString());
                this.apiQueryFilters.AdditionalFilters.push(this.TransportFilters.AdditionalFilters[0]);
            }

        } else {
            this.SetTransport("All");
        }
        //this.SelectedValueChangedEmitUser();
        //this.SelectedValueChangedEmitDepartment();

        //this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters, RemoveFilter: false });
        this.ApplyTransportSelectedStyle();
        if (this.myViewChildrenMultiSelectLOVComponent != null) {
            this.myViewChildrenMultiSelectLOVComponent.first.Invalidate();
            this.myViewChildrenMultiSelectLOVComponent.last.Invalidate();
        }

        this._CD.detectChanges();
        this.apiQueryFiltersChanged = false;
    }
    OnChosenListItemsChanged() {
        this.SelectedValueChangedEmitUser();
    }

    GetAdvanceFilterPMFromFilterItem(field: any, objectField: any) {
        var advanceFilter = new AdvancedQueryFilterPM();
        advanceFilter.Tenant = SessionInfo.LoggedUserTenant;
        advanceFilter.ObjectFieldId = objectField.Id;
        advanceFilter.DataTypeCode = field.FieldDataType;
        advanceFilter.DisplayInList = field.DisplayInList;
        advanceFilter.IsCustomFilter = field.IsCustom;
        advanceFilter.ObjectFieldName = field.FieldName;
        advanceFilter.QueryCode = this.QueryCode;
        advanceFilter.QueryId = this.QueryId;
        advanceFilter.QueryObjectTableName = this.ObjectTableName;
        advanceFilter.Operator = field.Operator;
        advanceFilter.PredefinedValue = field.FieldValue;
        advanceFilter.UserId = SessionInfo.LoggedUserId;
        advanceFilter.ObjectFieldCode = objectField.FieldCode;
        return advanceFilter;
    }

    // count the clicks
    private clickTimeout = null;
    public PreventDoubleClickSaveFilters(itemId: string): void {
        if (this.clickTimeout) {
            this.setClickTimeout(() => { });
        } else {
            // if timeout doesn't exist, we know it's first click 
            // treat as single click until further notice
            this.setClickTimeout((itemId) =>
                this.handleSingleClick(itemId));
        }
    }
    // sets the click timeout and takes a callback 
    // for what operations you want to complete when
    // the click timeout completes
    public setClickTimeout(callback) {
        // clear any existing timeout
        clearTimeout(this.clickTimeout);
        this.clickTimeout = setTimeout(() => {
            this.clickTimeout = null;
            callback();
        }, 200);
    }
    public handleSingleClick(itemId: string) {
        //The actual action that should be performed on click      
        this.SaveFilters();
    }

    TransportAdvancedQueryFilterPM: AdvancedQueryFilterPM;
    SaveFilters() {
        this.updateOrInsertAdvanceFilter(this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName == "TransportModeId"));
        this.updateOrInsertAdvanceFilter(this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName == "ReferantUserName"));
        this.updateOrInsertAdvanceFilter(this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName == "ReferentUserId"));
        this.updateOrInsertAdvanceFilter(this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName == "DepartmentId"));
        this.updateOrInsertAdvanceFilter(this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName == "DepartmentName"));
        this.apiQueryFiltersChanged = false;
    }

    private updateOrInsertAdvanceFilter(AdditionalFilters: FilterItem[]) {
        var objectFieldPMExtendedService: ObjectFieldPMExtendedService = new ObjectFieldPMExtendedService();
        if (AdditionalFilters.length > 0) {
            objectFieldPMExtendedService.GetObjectFieldByName(AdditionalFilters[0].FieldName, this.ObjectTableName).subscribe((objectField: any) => {
                if (objectField) {
                    var myService: DeclarationReferantDataWebService = new DeclarationReferantDataWebService();
                    myService.GetSingleByObjectFieldCodeAndTenant(objectField[0].Id, SessionInfo.LoggedUserTenant, this.QueryCode, SessionInfo.LoggedUserId).subscribe((advanceFilterFromDb: any) => {
                        if (advanceFilterFromDb) { // update if exist in db 
                            advanceFilterFromDb.PredefinedValue = this.getPredefinedValue(AdditionalFilters[0].FieldValue);
                            myService.update(advanceFilterFromDb).subscribe((myResult: any) => {
                            });
                        } else { // insert if doesnt exist in db
                            this.TransportAdvancedQueryFilterPM = this.GetAdvanceFilterPMFromFilterItem(AdditionalFilters[0], objectField[0]);
                            myService.insertDeclarationReferantFilters(this.TransportAdvancedQueryFilterPM).subscribe((myResult: any) => {
                            });
                        }
                    });
                }
            });
        }
    }

    getPredefinedValue(value: string) {
        if (value != "All") {
            return value;
        }
        return null;
    }

    applyBasicFiltes() {
         let ul = new UserList();
         ul.Id = (SessionLocator.LoggedUserPM.Id == null || SessionLocator.LoggedUserPM.Id == "0") ? "9999999" : SessionLocator.LoggedUserPM.Id;
         ul.LocalName = SessionLocator.LoggedUserPM.LocalName;
         if (AppTool.IsNullOrEmpty(ul.LocalName)) {
             ul.LocalName = SessionLocator.LoggedUserPM.EnglishName;
         }
         this.LOVListUsers.push(ul);
         this.SelectedValueChangedEmitUser();
         this.ApplyTransportSelectedStyle();
    }
    ngAfterViewInit() {
        if (this.OpenQueryThruWorkSpace) {

        } else {
            //this.applyBasicFiltes();
            // this.myViewChildrenMultiSelectLOVComponent.first.Invalidate();

            //  this.SelectedValueChangedEmitUser();
            // this.SelectedValueChangedEmitDepartment();

        }
        this.myViewChildrenMultiSelectLOVComponent.first.Invalidate();
        this.myViewChildrenMultiSelectLOVComponent.last.Invalidate();

        this._CD.detectChanges();
    }

    SetTransport(itemValue: string) {
        if (itemValue != null) {
            this.selectedValue = itemValue;
            this.ApplyTransportSelectedStyle();
        }
    }

    _LOVListUsers: any[] = [];
    get LOVListUsers() { return this._LOVListUsers; }
    set LOVListUsers(value) {
        if (this._LOVListUsers != value) {
            this._LOVListUsers = value;
        }
    }

    _LOVListDepartment: any[] = [];
    get LOVListDepartment() { return this._LOVListDepartment; }
    set LOVListDepartment(value) {
        if (this._LOVListDepartment != value) {
            this._LOVListDepartment = value;
        }
    }

    private selectedValue: string = "All";
    public get SelectedValue() { return this.selectedValue; }
    public set SelectedValue(value: string) {
        if (this.selectedValue != value) {
            this.selectedValue = value;
        }
    }

    private usersListString: string = "";
    public get UsersListString() { return this.usersListString; }
    public set UsersListString(value: string) {
        if (this.usersListString != value) {
            this.usersListString = value;
        }
    }

    private departmentListString: string = "";
    public get DepartmentListString() { return this.departmentListString; }
    public set DepartmentListString(value: string) {
        if (this.departmentListString != value) {
            this.departmentListString = value;
        }
    }

    private userNamesListString: string = "";
    public get UserNamesListString() { return this.userNamesListString; }
    public set UserNamesListString(value: string) {
        if (this.userNamesListString != value) {
            this.userNamesListString = value;
        }
    }

    private departmentNamesListString: string = "";
    public get DepartmentNamesListString() { return this.departmentNamesListString; }
    public set DepartmentNamesListString(value: string) {
        if (this.departmentNamesListString != value) {
            this.departmentNamesListString = value;
        }
    }

    ApplyTransportSelectedStyle() {
        var itemValue = this.SelectedValue;
        var img_A = document.getElementById(this.TransportFilter_A);
        var img_O = document.getElementById(this.TransportFilter_O);
        var img_I = document.getElementById(this.TransportFilter_I);
        if (img_A) {
            this.CurrentSession.ChangeSessionHeader({ TransportId: itemValue });
            img_A.setAttribute("src", "./Images/TransportModes/A_g.png");
            img_O.setAttribute("src", "./Images/TransportModes/O_g.png");
            img_I.setAttribute("src", "./Images/TransportModes/I_g.png");
            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./Images/TransportModes/A_w.png");
                    break;
                }

                case "O": {
                    img_O.setAttribute("src", "./Images/TransportModes/O_w.png");
                    break;
                }

                case "L": {
                    img_I.setAttribute("src", "./Images/TransportModes/I_w.png");
                    break;
                }
            }
        }
        this.apiQueryFiltersChanged = true;
    }

    itemMouseOver(itemValue: string) {
        if (this.SelectedValue != itemValue) {
            var img_A = document.getElementById(this.TransportFilter_A);
            var img_O = document.getElementById(this.TransportFilter_O);
            var img_I = document.getElementById(this.TransportFilter_I);

            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./Images/TransportModes/A.png");
                    break;
                }

                case "O": {
                    img_O.setAttribute("src", "./Images/TransportModes/O.png");
                    break;
                }

                case "L": {
                    img_I.setAttribute("src", "./Images/TransportModes/I.png");
                    //img_I.style.top = "1px";
                    break;
                }
            }
        }
    }

    itemMouseLeave(itemValue: string) {
        if (this.SelectedValue != itemValue) {
            var img_A = document.getElementById(this.TransportFilter_A);
            var img_O = document.getElementById(this.TransportFilter_O);
            var img_I = document.getElementById(this.TransportFilter_I);

            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./Images/TransportModes/A_g.png");
                    break;
                }

                case "O": {
                    img_O.setAttribute("src", "./Images/TransportModes/O_g.png");
                    break;
                }

                case "L": {
                    img_I.setAttribute("src", "./Images/TransportModes/I_g.png");
                    break;
                }
            }
        }
    }

    SelectedValueChangedEmitUser() {
        var RemoveFilter = false;
        this.UserNamesListString = "";
        this.UsersListString = "";
        if (this.apiQueryFilters.AdditionalFilters.length > 0) {
            this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "ReferentUserId" && a.FieldName != "ReferantUserName");
        }

        if (this._LOVListUsers.length > 0) {

            this._LOVListUsers.forEach(item => { this.UsersListString += item["Id"] + ","; });//Id: "1-3697"
            this.UsersListString = this.UsersListString.slice(0, -1); // trim last comma

            this._LOVListUsers.forEach(item => { this.UserNamesListString += item["LocalName"] + ","; });//Id: "1-3697"
            this.UserNamesListString = this.UserNamesListString.slice(0, -1); // trim last comma


        } else {
            this.UsersListString = "HowCare"
            //  UserNamesListString = "HowCare"LOVListUsers
            //RemoveFilter = true;
        }
        this.apiQueryFilters.addAdditionalFilter("ReferantUserName", this.UserNamesListString, null, null, "Equal", true, false, false, "string", true);
        this.apiQueryFilters.addAdditionalFilter("ReferentUserId", this.UsersListString, null, null, "InListExact", false, false, false, "string", this._LOVListUsers.length == 0);
        this.apiQueryFilters.addAdditionalFilter("RetrievData", true, null, null, "Equal", true, false, false, "string", this._LOVListUsers.length == 0);
        this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters, RemoveFilter: RemoveFilter });
        this.apiQueryFiltersChanged = true;
    }

    SelectedValueChangedEmitDepartment() {
        var RemoveFilter = false;
        if (this.apiQueryFilters.AdditionalFilters.length > 0) {
            this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "DepartmentId" && a.FieldName != "DepartmentName");
        }
        this.DepartmentListString = "";
        this.DepartmentNamesListString = "";

        if (this.LOVListDepartment.length > 0) {
            this.LOVListDepartment.forEach(item => { this.DepartmentListString += item["Id"] + ","; });//Id: "1-3697"
            this.DepartmentListString = this.DepartmentListString.slice(0, -1); // trim last comma
            this.LOVListDepartment.forEach(item => { this.DepartmentNamesListString += item["LocalName"] + ","; });//Id: "1-3697"
            this.DepartmentNamesListString = this.DepartmentNamesListString.slice(0, -1); // trim last comma

        } else {
            this.DepartmentListString = "HowCare";
            //  DepartmentNamesListString =  "HowCare";
            RemoveFilter = true;
        }
        this.apiQueryFilters.addAdditionalFilter("DepartmentName", this.DepartmentNamesListString, null, null, "Equal", true, false, false, "string", true);
        this.apiQueryFilters.addAdditionalFilter("DepartmentId", this.DepartmentListString, null, null, "InListExact", false, false, false, "string", this.LOVListDepartment.length == 0);
        this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters, RemoveFilter: RemoveFilter });
        this.apiQueryFiltersChanged = true;

    }

    transportmodeId: string = "All";
    itemClicked(itemValue: string) {
        this.transportmodeId = itemValue;
        var RemoveFilter = false;
        if (this.SelectedValue != itemValue) {
            this.SelectedValue = itemValue;
        }
        if (this.apiQueryFilters.AdditionalFilters.length > 0) {
            this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "TransportModeId")
            this.apiQueryFilters.addAdditionalFilter("TransportModeId", itemValue, null, null, "Equals", false, true, false, "string", (itemValue == "All" ? true : false));
        }
        this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters, RemoveFilter: RemoveFilter });
        this.ApplyTransportSelectedStyle();
    }

    ShowQueueManagmentAQ1() {

        let myViewModelName = "DeclarationReferantDataFiltersMenuComponent.ts-ShowQueueManagmentAQ1";
        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
            let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
                .subscribe(
                    (mess: UnifreightMessageM) => {
                        var IsMatchUnifreightCallbackCommand = (
                            mess.LogitudeEntity == AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
                            mess.LogitudeEntityNumber == "" &&
                            mess.LogitudeViewModel == myViewModelName);
                        if (IsMatchUnifreightCallbackCommand) {
                            sub.unsubscribe();
                            SessionLocator.SelectedSession.StopBusyIndicator();
                            let sBool = UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
                            SessionLocator.SelectedSession.CurrentListComponent.DoRefresh();
                        }
                    }
                );

            SessionLocator.SelectedSession.StartBusyIndicator("");
            var unifreightMessageM =
                AmitalGatewayUtil.Instance.
                    DeclarationMessaging.GetMessage("", "",
                        myViewModelName);

            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
                "ScriptableGatewayUtil.ShowQueueManagmentAQ1",
                "CFIHMAIN.LogitudeTask",
                "ShowQueueManagmentAQ1",
                unifreightMessageM,
                " הצגת מסך : ניהול תורים");
        }
        else {
            alert("ShowQueueManagmentAQ1");
        }
    }

    ShowOCRQuery() {

        let myViewModelName = "DeclarationReferantDataFiltersMenuComponent.ts-ShowOCRQuery";
        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
            let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
                .subscribe(
                    (mess: UnifreightMessageM) => {
                        var IsMatchUnifreightCallbackCommand = (
                            mess.LogitudeEntity == AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
                            mess.LogitudeEntityNumber == "" &&
                            mess.LogitudeViewModel == myViewModelName);
                        if (IsMatchUnifreightCallbackCommand) {
                            sub.unsubscribe();
                            SessionLocator.SelectedSession.StopBusyIndicator();
                            let sBool = UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
                            SessionLocator.SelectedSession.CurrentListComponent.DoRefresh();
                        }
                    }
                );

            SessionLocator.SelectedSession.StartBusyIndicator("");
            var unifreightMessageM =
                AmitalGatewayUtil.Instance.
                    DeclarationMessaging.GetMessage("", "",
                        myViewModelName);

            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
                "ScriptableGatewayUtil.ShowOCRQuery",
                "CFIHMAIN.LogitudeTask",
                "ShowOCRQuery",
                unifreightMessageM,
                " הצגת מסך : שםילתם ל - OCR");
        }
        else {
            alert("ShowOCRQuery");
        }
    }
}
