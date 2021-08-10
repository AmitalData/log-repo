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
    templateUrl: './VirtualScrollNG.html',
   
})

export class VirtualScrollNG implements OnInit {

    //cachedData: Car[];
    virtualCars: Car[]=[];
    cols: any[];
    TotalRecords?: number;
    loading: boolean;
    carService: CarService = new CarService(null);
    myIIGGeneralMessagesService: IIGGeneralMessagesService = new IIGGeneralMessagesService();
    LoadFirstCount: boolean = false;

    constructor(private primengConfig: PrimeNGConfig) {

    }

    ngOnInit() {
        //datasource imitation
        //customerService = new CustomerService();
        //this.customerService.getCustomersLarge().then(data => {
        //    this.datasource = data;
        //    this.totalRecords = data.length;
        //});
        this.TotalRecords = 500;
        this.loading = true;
        this.primengConfig.ripple = true;
        this.primengConfig.ripple = false;


        this.cols = [
            { field: 'vin', header: 'Vin' },
            { field: 'year', header: 'Year' },
            { field: 'brand', header: 'Brand' },
            { field: 'color', header: 'Color' }
        ];

        //this.cars = Array.from({ length: 10000 }).map(() => this.carService.generateCar());
        //this.virtualCars = Array.from({ length: 10000 });
        

    }

    loadCustomers(event: LazyLoadEvent) {
        this.loading = true;

        //in a real application, make a remote request to load data using state metadata from event
        //event.first = First row offset
        //event.rows = Number of rows per page
        //event.sortField = Field name to sort with
        //event.sortOrder = Sort order as number, 1 for asc and -1 for dec
        //filters: FilterMetadata object having field as key and filter value, filter matchMode as value

        //imitate db connection over a network
        //setTimeout(() => {
        //    if (this.datasource) {
        //        this.customers = this.datasource.slice(event.first, (event.first + event.rows));
        //        this.loading = false;
        //    }
        //}, 1000);
    }
    getCount: boolean = true;
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
        let realfirst = event.first; 
        //if ( this.TotalRecords > 0) {
            
        //    for (var i = event.first; i < event.rows; i++) {
        //        if (AppTool.IsNullOrEmpty(this.cachedData[i])) {
        //            break;
        //        }
        //        realfirst = i; 
        //    }
        //}
        //if (realfirst > event.rows) {
        //    this.virtualCars = [...this.virtualCars];
        //    return;
        //}
        
        this.myIIGGeneralMessagesService.GetVirtualCar(AppTool.IsNullOrZero(this.TotalRecords), event)
            .subscribe(
                (serviceResponse: ServiceResponse) => {
                    this.getCount = false;
                    this.loading = false;
                    let newAry:Car[] = serviceResponse.Result.Result;
                    if (AppTool.IsNullOrZero(this.TotalRecords)) {
                        this.virtualCars = []
                        this.TotalRecords = serviceResponse.Result.Count;
                        ///this.cachedData = Array.from({ length: this.TotalRecords});
                    }
                    
                    //let loadedCars = this.cars.slice(event.first, (event.first + event.rows));
                    //populate page of virtual cars
                    ///Array.prototype.splice.apply(this.virtualCars, [...[event.first, event.rows], ...newAry]);

                    //trigger change detection
                    this.virtualCars = [...newAry];

                });
    }

    loadCarsLazy11(event: LazyLoadEvent) {
        //simulate remote connection with a timeout 
        setTimeout(() => {
            //load data of required page
            ///let loadedCars = this.cars.slice(event.first, (event.first + event.rows));

            //populate page of virtual cars
            //Array.prototype.splice.apply(this.virtualCars, [...[event.first, event.rows], ...loadedCars]);

            //trigger change detection
            //this.virtualCars = [...this.virtualCars];
        }, Math.random() * 1000 + 250);
    }
    loadDataOnScroll1(event: LazyLoadEvent) {

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

    }


}
export interface Country {
    name?: string;
    code?: string;
}

export interface Representative {
    name?: string;
    image?: string;
}

export interface Customer {
    id?: number;
    name?: number;
    country?: Country;
    company?: string;
    date?: string;
    status?: string;
    representative?: Representative;
}




@Injectable()
export class CarService {
    brands: string[] = ['Vapid', 'Carson', 'Kitano', 'Dabver', 'Ibex', 'Morello', 'Akira', 'Titan', 'Dover', 'Norma'];

    colors: string[] = ['Black', 'White', 'Red', 'Blue', 'Silver', 'Green', 'Yellow'];

    constructor(private http: HttpClient) { }

    getCarsSmall() {
        return this.http.get<any>('assets/showcase/data/cars-small.json')
            .toPromise()
            .then(res => <Car[]>res.data)
            .then(data => { return data; });
    }

    getCarsMedium() {
        return this.http.get<any>('assets/showcase/data/cars-medium.json')
            .toPromise()
            .then(res => <Car[]>res.data)
            .then(data => { return data; });
    }

    getCarsLarge() {
        return this.http.get<any>('assets/showcase/data/cars-large.json')
            .toPromise()
            .then(res => <Car[]>res.data)
            .then(data => { return data; });
    }

    generateCar(): Car {
        return {
            vin: this.generateVin(),
            brand: this.generateBrand(),
            color: this.generateColor(),
            year: this.generateYear()
        }
    }

    generateVin() {
        let text = "";
        let possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        for (var i = 0; i < 5; i++) {
            text += possible.charAt(Math.floor(Math.random() * possible.length));
        }

        return text;
    }

    generateBrand() {
        return this.brands[Math.floor(Math.random() * Math.floor(10))];
    }

    generateColor() {
        return this.colors[Math.floor(Math.random() * Math.floor(7))];
    }

    generateYear() {
        return 2000 + Math.floor(Math.random() * Math.floor(19));
    }
}

export interface Car {
    vin?;
    year?;
    brand?;
    color?;
    price?;
    saleDate?;
}
