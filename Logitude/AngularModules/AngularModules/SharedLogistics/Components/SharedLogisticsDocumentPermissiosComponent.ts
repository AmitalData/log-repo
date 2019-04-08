
declare var window: any;
import {Component, OnInit, EventEmitter}  from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';

import {SharedLogisticsService} from '../Services/Others/SharedLogisticsService';
import {DocumentTypePMExtendedService} from '../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService';
import {DocumentTypePM} from '../../Common/EntityPMs/DocumentTypePM';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';

import {DocumentPermissiosViewModel} from './ViewModel/DocumentPermissiosViewModel';

@Component({
    moduleId: module.id,
    selector: 'SharedLogisticsDocumentPermissios',
    templateUrl: './SharedLogisticsDocumentPermissiosComponent.html',
    inputs: ['OnCloseWindowEvent'],
    providers: [DocumentTypePMExtendedService],
})
export class SharedLogisticsDocumentPermissiosComponent implements OnInit {


    myTenantZeroList: DocumentTypePM[];
    myTenantList: DocumentTypePM[];
    DocumentPermissiosLists: DocumentPermissiosViewModel[];
    OnCloseWindowEvent = new EventEmitter();
    ObjectTableId: string;
    FullComponentsVisibility: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _documentTypePMExtendedService: DocumentTypePMExtendedService) {
        this.CurrentSession.StartBusyIndicatorLoading();
    }

    ngOnInit(

    ) {

        this.OnCloseWindowEvent.subscribe(($event: any) => {
            this.SaveButtonClicked();

        });

        this.Run();



    }


    Run() {


        this.ObjectTableId = window.ObjectTables.filter(d=> d.Name == "Shipment")[0].Id;
        this.LoadTenantZeroDate();

    }
    LoadTenantZeroDate() {

        this.myTenantZeroList = [];

        this._documentTypePMExtendedService.GetDocumentTypesByObjectTableAndTenant(this.ObjectTableId, 0).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.myTenantZeroList = pmResponse.Result;

                this.LoadTenantData();
            }
            else this.CurrentSession.StopBusyIndicator();
        });


    }


    LoadTenantData() {
        this.myTenantList = [];

        this._documentTypePMExtendedService.GetDocumentTypesByObjectTableAndTenant(this.ObjectTableId, SessionLocator.Tenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.myTenantList = pmResponse.Result;

                this.BuildData();
            }

            this.CurrentSession.StopBusyIndicator();
        });

    }




    mySearchText: string;

    BuildData() {
        var myList: DocumentTypePM[] = [];
        this.DocumentPermissiosLists = [];
        if (!this.mySearchText) {
            myList = this.myTenantList;
        }
        else {

            myList = this.myTenantList.filter(d=> (d.Code && d.Code.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1) || (d.Name && d.Name.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1));
        }

        myList = this.SortItemSource(myList);
        myList.forEach((item) => {
            var tenantZeroItem = this.myTenantZeroList.filter(t => t.Code == item.Code )[0];
            if (tenantZeroItem != null) {
                this.DocumentPermissiosLists.push(new DocumentPermissiosViewModel(tenantZeroItem, item));
            }
     
        });


    }

    SortItemSource(items: any) {

        items.sort((a, b) => {
            if (a.Name && a.Name.toLowerCase() < b.Name.toLowerCase()) {
                return -1;
            }
            else if (a.Name  && a.Name.toLowerCase() > b.Name.toLowerCase()) {
                return 1;
            }
            else {

                return 0;
            }
        });
        return items;
    }


    CloseButtonClicked() {
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.CloseCurrentWindow();
    }



    SaveButtonClicked() {
        this.CurrentSession.StartBusyIndicatorSaving();
        this.myTenantList = [];
        this.DocumentPermissiosLists.forEach((item) => {

            if (item.entityPM.IsDirty) {
                this.myTenantList.push(item.entityPM);
            }
        });

        if (this.myTenantList.length > 0) {

            this._documentTypePMExtendedService.update(this.myTenantList).subscribe(res => {
                this.CloseButtonClicked();
            });
        }
        else {
            this.CloseButtonClicked();
        }


    }
    onSearchTextChangeEvent(searchText) {
        if (!searchText) searchText = "";

        this.mySearchText = searchText;
        this.BuildData();
    }

    SetWindowArgs(args: any) {
        this.FullComponentsVisibility = true;
    }




}
