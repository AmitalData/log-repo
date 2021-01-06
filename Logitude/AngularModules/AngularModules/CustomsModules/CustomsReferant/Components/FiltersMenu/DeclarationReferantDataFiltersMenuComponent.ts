import { Component, AfterViewInit, Output, EventEmitter, ContentChild, ViewChild, ViewChildren, QueryList, ChangeDetectorRef } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { MultiSelectLOVComponent } from '../../../../Infrastructure/Components/LogitudeComponents/MultiSelectLOVComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { UserList } from '../../../../Common/EntityLists/UserList';
import { AppTool } from '../../../../Infrastructure/Tools';
import { AmitalGatewayUtil, UnifreightMessageM } from '../../../../Infrastructure/Utilities/AmitalGatewayUtil';

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

    OnChosenListItemsChanged() {
        this.SelectedValueChangedEmitUser();
    }
    //public LoadingContactFilterItems: ApiQueryFilters;
    //InitLOVFilters() {//38388
   //     this.LoadingContactFilterItems.removeAdditionalFilter("InActive");
 //       this.LoadingContactFilterItems.addAdditionalFilter("InActive","True", null, null, "Equals", false, false, false, "Boolean", false, true);
     //   ("ReferentUserId", UsersListString, null, null, "InListExact", false, false, false, "string", this._LOVListUsers.length == 0);
  //  }
    ngAfterViewInit() {
        this.ApplyTransportSelectedStyle();
        //
        //this.LOVListUsers.push(SessionLocator.LoggedUserPM); // by default is the grid filtered by the current user
        //let myUserListService: UserListService = new UserListService();
        //myUserListService.getSingleFromCache(SessionLocator.LoggedUserId)
        //    .subscribe(r => {
        //        let myUserList: UserList = r.Result;
        //        if (!AppTool.IsNullOrEmpty(myUserList)) {
        //            this.myViewChildrenMultiSelectLOVComponent.first.AddUserList(myUserList);
        //        }
        //    });
        let ul = new UserList();
        ul.Id = (SessionLocator.LoggedUserPM.Id == null || SessionLocator.LoggedUserPM.Id == "0") ? "9999999" : SessionLocator.LoggedUserPM.Id ;
        ul.LocalName = SessionLocator.LoggedUserPM.LocalName; 
        if (AppTool.IsNullOrEmpty(ul.LocalName)) {
            ul.LocalName = SessionLocator.LoggedUserPM.EnglishName;
        }
        this.LOVListUsers.push(ul);
        this.myViewChildrenMultiSelectLOVComponent.first.Invalidate();
        this.SelectedValueChangedEmitUser();

        this._CD.detectChanges();
    }

    SetTransport(itemValue: string) {
        this.selectedValue = itemValue;
        this.ApplyTransportSelectedStyle();
    }

    _LOVListUsers :any[] = [];
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
        if (this.apiQueryFilters.AdditionalFilters.length > 0) {
            this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "ReferentUserId");
        }
        var UsersListString = "";
        if (this._LOVListUsers.length > 0) {

            this._LOVListUsers.forEach(item => { UsersListString += item["Id"] + ","; });//Id: "1-3697"
            UsersListString = UsersListString.slice(0, -1); // trim last comma

        } else {
            UsersListString = "HowCare"
            RemoveFilter = true;
        }
        this.apiQueryFilters.addAdditionalFilter("ReferentUserId", UsersListString, null, null, "InListExact", false, false, false, "string", this._LOVListUsers.length == 0);
        this.apiQueryFilters.addAdditionalFilter("RetrievData", true, null, null, "Equal", true, false, false, "string", this._LOVListUsers.length == 0);
        this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters, RemoveFilter: RemoveFilter });
    }

    SelectedValueChangedEmitDepartment() {
        var RemoveFilter = false;
        if (this.apiQueryFilters.AdditionalFilters.length > 0) {
            this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "DepartmentId");
        }
        var LOVListDepartment = "";
        if (this._LOVListDepartment.length > 0) {
            this._LOVListDepartment.forEach(item => { LOVListDepartment += item["Id"] + ","; });//Id: "1-3697"
            LOVListDepartment = LOVListDepartment.slice(0, -1); // trim last comma

        } else {
            LOVListDepartment = "HowCare"
            RemoveFilter = true;
        }
        this.apiQueryFilters.addAdditionalFilter("DepartmentId", LOVListDepartment, null, null, "InListExact", false, false, false, "string", this._LOVListDepartment.length == 0);
        this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters, RemoveFilter: RemoveFilter });
    }

    itemClicked(itemValue: string) {
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
                " הצגת מסך : שאילתא ל - OCR");
        }
        else {
            alert("ShowOCRQuery");
        }
    }
}
