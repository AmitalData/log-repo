import { Component, AfterViewInit, Output, EventEmitter } from '@angular/core';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';

@Component({
    moduleId: module.id,
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
    constructor() {
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
        //this.MyObservableCollection = new ObservableCollection(this._LOVListUsers);
       // this.MyObservableCollection.Changed.subscribe(r => { this.SelectedValueChangedEmitUser(); });
    }
    OnChosenListItemsChanged() {
        this.SelectedValueChangedEmitUser();
    }
    ngAfterViewInit() {
        this.ApplyTransportSelectedStyle();
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
            img_I.setAttribute("src", "./Images/TransportModes/I_G.png");

            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./Images/TransportModes/A_w.png");
                    break;
                }

                case "O": {
                    img_O.setAttribute("src", "./Images/TransportModes/O_w.png");
                    break;
                }

                case "I": {
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

                case "I": {
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

                case "I": {
                    img_I.setAttribute("src", "./Images/TransportModes/I_g.png");
                    break;
                }
            }
        }
    }
    
    SelectedValueChangedEmitUser() {
        var RemoveFilter = false;
        //if (this.apiQueryFilters.AdditionalFilters.length > 0) {
        //    this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "ReferentUserId");
        //}

        //this.apiQueryFilters.addAdditionalFilter("TransportModeId", itemValue, null, null, "Equals", false, true, false, "string", (itemValue == "All" ? true : false));
        //if (itemValue == "All") {
        //    RemoveFilter = true;
        //}
        //if (this._LOVListUsers.length == 0) {
        //    this.apiQueryFilters.removeAdditionalFilter("ReferentUserId");
        //    RemoveFilter = true;
        //    this.SelectedValueChanged.emit({ Filters: null, RemoveFilter: RemoveFilter });

        //} else {
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
        this.apiQueryFilters.addAdditionalFilter("ReferentUserId", UsersListString, null, null, "InList", false, false, false, "string", this._LOVListUsers.length == 0);
        this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters, RemoveFilter: RemoveFilter });
        //}

    }
    SelectedValueChangedEmitDepartment() {
        var RemoveFilter = false;
        if (this.LOVListDepartment.length == 0) {
            this.apiQueryFilters.removeAdditionalFilter("DepartmentId");
            RemoveFilter = true;

        } else {
            if (this.LOVListDepartment.length > 0) {
                var DepartmentListString = "";
                if (this.apiQueryFilters.AdditionalFilters.length > 0) {
                    this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "DepartmentId");
                }
                this.LOVListDepartment.forEach(item => { DepartmentListString += item["Id"] + ","; });//Id: "1-3697"
                DepartmentListString = DepartmentListString.slice(0, -1); // trim last comma
                this.apiQueryFilters.addAdditionalFilter("DepartmentId", DepartmentListString, null, null, "InList", false, false, false, "string");
            } 
        }
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
}
