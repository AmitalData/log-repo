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
    
    constructor() {
        super();
        if (this.CurrentSession == null) {
        }

        else {
            //var index_T = this.CurrentSession.GetNewId("DeclarationReferantDataTransportFilterMenu");
            //var index_D = this.CurrentSession.GetNewId("DeclarationReferantDataDirectionFilterMenu");
            //this.TransportFilter_A = "TransportFilter_A" + index_T;
            //this.TransportFilter_O = "TransportFilter_O" + index_T;
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
        //this.MyObservableCollection.Changed.subscribe(r => { this.SelectedValueChangedEmit(); });
    }
    OnChosenListItemsChanged() {
        this.SelectedValueChangedEmit();
    }
    ngAfterViewInit() {
        //this.ApplyTransportSelectedStyle();
        
    }

    // Transport
    _LOVListUsers :any[] = [];
    get LOVListUsers() { return this._LOVListUsers; }
    set LOVListUsers(value) {
        if (this._LOVListUsers != value) {
            this._LOVListUsers = value;
        }
    }

    
    SelectedValueChangedEmit() {

        while (this.apiQueryFilters.AdditionalFilters.length) {
            this.apiQueryFilters.AdditionalFilters.pop();
        }

        
        var RemoveFilter = false;
        if (this._LOVListUsers.length > 0) {
            var UsersListString = "";
            
            this._LOVListUsers.forEach(item => { UsersListString += item["Id"] + ","; });//Id: "1-3697"
            UsersListString = UsersListString.slice(0, -1); // trim last comma
            this.apiQueryFilters.addAdditionalFilter("Users", UsersListString, null, null, "InList", false, false, false, "string");
        } else {
            RemoveFilter = true;
        }


            


        this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters, RemoveFilter: RemoveFilter });
   
        
    }
  
   

    

}
