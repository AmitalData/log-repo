import { AfterViewInit, Component, Injectable, OnInit } from "@angular/core";
import { CourierWorksheetSharedDataService } from "../../../../Customs/Services/DataChange/CourierWorksheetSharedDataService";
import { DeclarationCourierStatusWebService } from "../../../../Customs/Services/WebServices/DeclarationCourierStatusWebService";

///import { Component } from '@angular/core';
///import { CarService } from './carservice';
///import { Car } from './car';
//import { FilterUtils } from 'primeng/utils';
///import { LazyLoadEvent } from 'primeng/api';
import { SelectItem } from 'primeng/api';
import { MessageService } from 'primeng/api';



import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { CalendarModule } from 'primeng/calendar';
import { SliderModule } from 'primeng/slider';
import { MultiSelectModule } from 'primeng/multiselect';
import { ContextMenuModule } from 'primeng/contextmenu';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { ProgressBarModule } from 'primeng/progressbar';
import { InputTextModule } from 'primeng/inputtext';

/*import { FilterUtils } from 'primeng/utils';*/
import { LazyLoadEvent } from 'primeng/api';
import { PrimeNGConfig } from 'primeng/api';
import { HttpClient } from "@angular/common/http";
import { IIGGeneralMessagesService } from "../../../../Customs/Services/WebServices/IIGGeneralMessagesService";
import { ServiceResponse } from "../../../../Infrastructure/DataContracts/ServiceResponse";
import { AppTool } from "../../../../Infrastructure/Tools";


@Component({
    templateUrl: './VirtualScrollNGScroll.html',

})

export class VirtualScrollNGScroll implements OnInit {

    //cachedData: Car[];
    DataSource: Car[] = [];
    columns: any[];
    TotalRecords?: number;
    loading: boolean;
    
    myIIGGeneralMessagesService: IIGGeneralMessagesService = new IIGGeneralMessagesService();
    LoadFirstCount: boolean = false;

    constructor(private primengConfig: PrimeNGConfig) {

    }
    filter: string[];
    ngOnInit() {
        //datasource imitation
        //customerService = new CustomerService();
        //this.customerService.getCustomersLarge().then(data => {
        //    this.datasource = data;
        //    this.totalRecords = data.length;
        //});
        ///this.TotalRecords = 500;
        this.loading = true;
        this.primengConfig.ripple = true;
        //this.primengConfig.ripple = false;


        this.columns = [
            { FieldName: 'vin', Display: 'Vin', myType:'text' },
            { FieldName: 'CreatAt', Display: 'CreatAt', myType: 'date' },
            { FieldName: 'brand', Display: 'Brand', myType: 'text' },
            { FieldName: 'color', Display: 'Color', myType: 'text' },
        ];
        this.filter = this.columns.map(r => r.FieldName);
        console.log(this.filter);
        //this.cars = Array.from({ length: 10000 }).map(() => this.carService.generateCar());
        //this.virtualCars = Array.from({ length: 10000 });


    }

    
    
    oldEvent: LazyLoadEvent
    loadCarsLazy(event: LazyLoadEvent) {

        let firstTime: boolean;
        let onScroll: boolean;
        let onSort: boolean;
        let globalSearch: boolean;

        console.log(event);
        if (event.globalFilter == null && !event.filters.hasOwnProperty('global') &&
            event.hasOwnProperty('sortField') && event.sortField == undefined) {
            firstTime = true;
            console.log('user iscoming for the first time ');
        } else if (event.globalFilter != null && event.globalFilter.length > 0 && event.filters.hasOwnProperty('global') &&
            !event.hasOwnProperty('sortField')) {
            globalSearch = true;
            console.log('user is doing only global search');
        } else if (event.hasOwnProperty('sortField') && event.sortField != undefined && event.globalFilter == null && !event.filters.hasOwnProperty('global')) {
            onSort = true;
            console.log('user is doing sorting');
        } else if (event.hasOwnProperty('sortField') && event.globalFilter != null && event.filters.hasOwnProperty('global')) {
            globalSearch = true;
            onSort = true;
            console.log('user is doing global search along with sort');
        } else {
            console.log('user is doing onscroll so do increment for page number');
            // here we need to take entire event and get all possible values to call
            // get api and update grid data
            // 
        }
        
        let useCache: boolean = true;
        if (this.oldEvent) {

            if (
                this.oldEvent.sortField != event.sortField || this.oldEvent.sortOrder != event.sortOrder
                ||
                JSON.stringify(this.oldEvent.filters) != JSON.stringify(event.filters)
                ||
                JSON.stringify(this.oldEvent.globalFilter) != JSON.stringify(event.globalFilter)
            )

             {
                console.log('Clear Cache');
                this.TotalRecords = null;
                this.DataSource = null; //Array.from({ length: this.TotalRecords });
                useCache = false;
            }
        }
        if (useCache) {
            if (this.TotalRecords > 0) {

                for (var i = 0; i < event.rows; i++) {

                    if (AppTool.IsNullOrEmpty(this.DataSource[event.first+i])) {
                        useCache = false;
                        break;
                    }
        
                }
            } else {
                useCache = false;
            }
        }
        if (useCache) {
            return;
        }
        //if (realfirst > event.rows) {
        //    this.virtualCars = [...this.virtualCars];
        //    return;
        //}

        //event.first = First row offset
        //event.rows = Number of rows per page
        //event.sortField = Field name to sort in single sort mode
        //event.sortOrder = Sort order as number, 1 for asc and -1 for dec in single sort mode
        //multiSortMeta: An array of SortMeta objects used in multiple columns sorting. Each SortMeta has field and order properties.
        //filters: Filters object having field as key and filter value, filter matchMode as value
        //globalFilter: Value of the global filter if available
        this.oldEvent = event;
        this.loading = true;
        this.myIIGGeneralMessagesService.GetVirtualCar(this.TotalRecords==null, event)
            .subscribe(
                (serviceResponse: ServiceResponse) => {
                    this.loading = false;
                    let newAry: Car[] = serviceResponse.Result.Result;
                    if (AppTool.IsNullOrZero(this.TotalRecords)) {
                        this.TotalRecords = serviceResponse.Result.Count;
                        this.DataSource = Array.from({ length: this.TotalRecords });
                    }

                    Array.prototype.splice.apply(this.DataSource, [...[event.first, event.rows], ...newAry]);

                    //trigger change detection
                    this.DataSource = [...this.DataSource];

                });
    }


}


export interface Car {
    vin?;
    CreatAt?;
    brand?;
    color?;
    price?;
    saleDate?;
}
