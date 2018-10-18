/// <reference path="../pipes/datetimetodatepipe.ts" />
import {Component, OnInit, DynamicComponentLoader, ElementRef, Injector, provide, ViewEncapsulation, Type, Renderer, ComponentRef}     from 'angular2/core';
import {Router, RouteParams, RouteConfig, RouterOutlet, ROUTER_DIRECTIVES, AsyncRoute} from 'angular2/router';
import {HTTP_PROVIDERS, Http} from 'angular2/http';
import {CORE_DIRECTIVES} from 'angular2/common';
import {DynamicRouteConfigurator} from '../utilities/DynamicRouteConfigurator';
import {ShipmentsService} from '../../shipment/services/shipment-service/shipments.service';
import {TextcodeTranslationPipe} from '../pipes/textcode-translation/textcode-translation.pipe';
import {TextCodeTranslator} from '../utilities/TextCodeTranslator';
import {ShipmentPM} from '../../shipment/EntityPMs/ShipmentPM';

import {EntityArgs} from '../data-contracts/entity-args';
import {HeaderScreenValueComponent} from '../logitude-components/header-screen-value/header-screen-value.component';
//import {GeneralComponent} from '../../shipment/shipment-tabs/general/general.component';
//import {OverviewComponent} from '../../shipment/shipment-tabs/overview/overview.component';
//import {PackagesComponent} from '../../shipment/shipment-tabs/packages/packages.component';
//import {PartnersComponent} from '../../shipment/shipment-tabs/Partners/PartnersTab.component';
//import {RoutingsComponent} from '../../shipment/shipment-tabs/Routings/RoutingsTab.component';
//import {PayablesComponent} from '../../shipment/shipment-tabs/Payables/PayablesTab.component';
//import {ReceivablesComponent} from '../../shipment/shipment-tabs/Receivables/ReceivablesTab.component';
//import {OrderComponent} from '../../shipment/shipment-tabs/Order/OrderTab.component';
//import {MasterComponent} from '../../shipment/shipment-tabs/Master/MasterTab.component';
//import {ShipmentsComponent} from '../../shipment/shipment-tabs/Shipments/ShipmentsTab.component';
//import {CustomsFileComponent} from '../../shipment/shipment-tabs/CustomsFile/CustomsFileTab.component';
//import {FreightFilesComponent} from '../../shipment/shipment-tabs/FreightFiles/FreightFilesTab.component';
//import {EventsComponent} from '../../shipment/shipment-tabs/Events/EventsTab.component';
//import {DocsInComponent} from '../../shipment/shipment-tabs/DocsIn/DocsInTab.component';
//import {DocsOutComponent} from '../../shipment/shipment-tabs/DocsOut/DocsOutTab.component';
//import {CommunicationsComponent} from '../../shipment/shipment-tabs/Communications/CommunicationsTab.component';

//import {ShipmentShortTitleComponent} from '../../shipment/short-title/shipment-short-title.component';

@Component({
    templateUrl: './app/infrastructure/edit-control/EditView.html',
    viewProviders: [DynamicRouteConfigurator],
    directives: [RouterOutlet, ROUTER_DIRECTIVES, CORE_DIRECTIVES, HeaderScreenValueComponent/*, ShipmentShortTitleComponent*/],
    providers: [/*ShipmentsService, */HTTP_PROVIDERS, EntityArgs, ComponentRef],
    pipes: [TextcodeTranslationPipe],
})

//@RouteConfig([
//    new AsyncRoute({
//            path: '/DYNAMIC-DEFAULT',
//                loader: () => System.import('./app/infrastructure/dynamic-dummy-component/dynamic-dummy.component').then(m => m["DynamicDummyComponent"]),
//                name: 'DYNAMIC',
//                //useAsDefault: true
//            }),
//    //{ path: '/', name: 'EditControl', component: OverviewComponent },
//])

export class EditControlComponent implements OnInit {

    private _selectedId: string;
    appRoutes: string[][];
    private _shortTitleComponent: Type;
    objectTableName: string;
    //public myWindow: LogitudeWindow;

    constructor(
        private _router: Router,
        private _renderer: Renderer,
        private _shipmentsService: ShipmentsService,
        routeParams: RouteParams,
        private _dynamicComponentLoader: DynamicComponentLoader,
        private _injector: Injector,
        private dynamicRouteConfigurator: DynamicRouteConfigurator,
        public entityArgs: EntityArgs,
        private _elementRef: ElementRef,
        private _componentRef: ComponentRef
    ) {
        this.hideBusyIndicator = false;
        this._selectedId = routeParams.get('id');

        //console.log(this.myWindow.ObjectFields.length);
        //this._renderer.
        console.log(this._componentRef);
        /* Routing Configurations
        this.appRoutes = this.getAppRoutes();
     
        //setTimeout(_ => {
        //console.log("routeer difsaflskdjflasdkjfklasdjf fjasdkl fj sekl;fjsdklafjsdkl;fjai");
        var routes = this._shipmentsService.GetRouts();

        for (var i in routes) {
            this.dynamicRouteConfigurator.addRoute(this.constructor, routes[i]);
        }

        this.appRoutes = this.getAppRoutes();
        //console.log(this.appRoutes);

        // }, 1000);
        */

        //System.import('./app/shipment/short-title/' + 'shipment' + '-short-title.component')
        //    .then(m => {
        //        /*m["ShipmentShortTitle" + "Component"];*/
        //        this._shortTitleComponent = m["ShipmentShortTitle" + "Component"];
        //        console.log(m["ShipmentShortTitle" + "Component"]);
        //        var injector = Injector.resolveAndCreate([provide("ShipmentShortTitleComponent", { useClass: this._shortTitleComponent })]);
        //        var shipmentTitle = injector.get("ShipmentShortTitleComponent");
        //        console.log(shipmentTitle);
        //    })
        this.objectTableName = "Shipment";
        this.headerScreen = this.GetHeaderScreenSettings(this.objectTableName);
        this.headerScreenRows = this.headerScreen.NumberOfRows;
        this.headerScreenColumns = this.headerScreen.NumberOfColumns;
        //this.screenFields = this.GetHeaderScreenScreenFields(this.headerScreen.Id);
        //this.AssignObjectFieldsToTranslationsHeaderScreen();
    }

    GetHeaderScreenSettings = (objectTableName: string) => {
        var headerScreen;
        var headerScreenId;
        for (var v = 0; v < window.ObjectTables.length; v++) {
            if (window.ObjectTables[v].Name === objectTableName) {
                headerScreenId = window.ObjectTables[v].HeaderScreenId;
                //console.log(this.$window.ObjectTables[v]);
                break;
            }
        }

        for (var i = 0; i < window.Screens.length; i++) {
            if (window.Screens[i].Id === headerScreenId) {
                headerScreen = window.Screens[i];
                //console.log(headerScreen);
                break;
            }
        }
        return headerScreen;
    }

    GetHeaderScreenScreenFields = (headerScreenId: string) => {
        var screenFields = [];
        for (var i = 0; i < window.ScreenFields.length; i++) {
            if (window.ScreenFields[i].ScreenId === headerScreenId) {
                screenFields.push(window.ScreenFields[i]);
            }
        }
        screenFields = screenFields.sort((a, b) => { return a.Row - b.Row });
        screenFields = screenFields.sort((a, b) => { return a.Column - b.Column });
        //screenFields = this.$filter('orderBy')(screenFields, ["Row", "Column"], false);
        //console.log(screenFields);
        return screenFields;
    }

    AssignObjectFieldsToTranslationsHeaderScreen = () => {
        var headerScreenTable: HTMLTableElement = <HTMLTableElement>document.getElementById("HeaderScreen");
        var s = 0; // cells counter
        for (var r = 0; r < this.headerScreen.NumberOfRows; r++) {
            var row = <HTMLTableRowElement>headerScreenTable.insertRow(r);
            for (var c = 0; c < this.headerScreen.NumberOfColumns; c++) {
                var cell = row.insertCell(c);
                cell.className = "HeaderScreenCell";
                var objectField = this.GetObjectField(this.screenFields[s].ObjectFieldId);
                var lowerCaseObjectField = (<string>objectField.FieldName).toLowerCase();
                //console.log(lowerCaseObjectField);
                
                //var anchorSpan: HTMLSpanElement = document.createElement("SPAN");
                //anchorSpan.setAttribute("class", "HeaderScreenValue");
                //anchorSpan.setAttribute("#" + lowerCaseObjectField, "");
                //var att = document.createAttribute("#" + lowerCaseObjectField);       // Create a "class" attribute
                //att.value = "democlass";                           // Set the value of the class attribute
                //anchorSpan.setAttributeNode(att);
                cell.innerHTML = "<span class='HeaderScreenLable'>" + TextCodeTranslator.transform(objectField.FullNameTextCodeCode) + ": </span>" + //anchorSpan;
                    '<span class="HeaderScreenValue" *ngIf="EntityPM" #' + lowerCaseObjectField + '>{{EntityPM[' + objectField.FieldName + ']}}</span>';
                s++;

                //console.log("ObjectField Name: ---> ", (<string>objectField.FieldName).toLowerCase());
                if (objectField.HtmlHeaderComponent) {
                    System.import(objectField.HtmlHeaderComponent)
                        .then(result => {
                            //console.log(result);
                            //console.log("has header component");
                            //this._dynamicComponentLoader.loadIntoLocation(this._shortTitleComponent, this._elementRef, 'ShortTitle');
                            //this._dynamicComponentLoader.loadIntoLocation(result.IsOperationalClosedHeaderTemplate, this._elementRef, lowerCaseObjectField);
                        })
                }
                else {
                    System.import("./app/infrastructure/logitude-components/header-screen-value/header-screen-value.component")
                        .then(result => {
                            //console.log(result.HeaderScreenValueComponent);
                            //console.log("!!!!! DOES NOT have header component");
                            //this._dynamicComponentLoader.loadAsRoot(result.HeaderScreenValueComponent, objectField.FieldName, this._injector);
                            //this._dynamicComponentLoader.loadIntoLocation(result.HeaderScreenValueComponent, this._elementRef, lowerCaseObjectField);
                        })
                }
                //var fieldValue = (this.EntityPM[objectField.FieldName]) ? this.EntityPM[objectField.FieldName] : '';
                
            }
        }
        //this.$compile(headerScreenTable)(this.$scope);
    }

    private getAppRoutes(): string[][] {
        return this.dynamicRouteConfigurator
            .getRoutes(this.constructor).configs.map(route => {
                //console.log(route);
                return { path: route.path, name: route.as,component:route.component };
            });
    }

    public SelectedTab: any;
    public HeaderScreen: any;
    public ScreenFields: any[];
    public ObjectTableTabs: any[];
    public ObjectFieldsOfObjectTable: any[];
    public hideBusyIndicator: boolean;
    public EntityPM: ShipmentPM;
    public CachedImports: any[];

    ngOnInit() {
        
        this.ObjectFieldsOfObjectTable = window.ObjectFields.filter(d => d.ObjectTableId === "1-4");
        //console.log(this.ObjectFieldsOfObjectTable);
        this.HeaderScreen = window.Screens.filter(d => d.ObjectTableId === "1-4" && d.Code === "Shipment.HeaderScreen")[0];
        //console.log(this.HeaderScreen);
        this.ScreenFields = window.ScreenFields.filter(d => d.ScreenId === this.HeaderScreen.Id);
        //console.log(this.ScreenFields);
        this.ScreenFields = this.ScreenFieldsOrderBy(this.ScreenFields);
        //console.log(this.ScreenFields);
        this.ObjectTableTabs = window.ObjectTableTabs.filter(d => d.ObjectTableId === "1-4" && d.Code !== "MHGC");
        //console.log(this.ObjectTableTabs);
        this.ObjectTableTabs = this.OrderObjectTableTabs(this.ObjectTableTabs);
        //console.log(this.ObjectTableTabs);
        //this._shipmentsService.getSingleShipmentById().subscribe(res => console.log(res));
        //this._shipmentsService.getShipment(this._selectedId).then(res => { this.EntityPM = res; console.log(res); this.hideBusyIndicator = true; });

        this.CachedImports = [];
        if (this.ObjectTableTabs != null) {
            this.onSelect(this.ObjectTableTabs[0]);
            //this.selectDefaultTab(this.ObjectTableTabs[0]);
        }
        
        this._shipmentsService.getSingleEntityPMFromServer(this._selectedId, 1).subscribe(res =>
        {
            this.EntityPM = res;
            //console.log(this.EntityPM);
            this.entityArgs.EntityPM = this.EntityPM;
            this.entityArgs.ObjectTableName = "Shipment";
            this.hideBusyIndicator = true;
            //console.log("now it's here the args i mean", this.entityArgs);
            this.initializeShortTitleControl();


            //this.objectTableName = "Shipment";
            //this.headerScreen = this.GetHeaderScreenSettings(this.objectTableName);
            //this.headerScreenRows = this.headerScreen.NumberOfRows;
            //this.headerScreenColumns = this.headerScreen.NumberOfColumns;
            //this.screenFields = this.GetHeaderScreenScreenFields(this.headerScreen.Id);
            //this.AssignObjectFieldsToTranslationsHeaderScreen();
        });
        

        //var injector = new Injector([ShipmentShortTitleComponent]);
        //var title = injector.get(ShipmentShortTitleComponent);
        //console.log(title);
        var shortTitleObjectTableName = "Shipment";
        //var injector = Injector.resolveAndCreate([shortTitle + "ShortTitleComponent"]);
        //var injector = Injector.resolveAndCreate([provide(this._shortTitleComponent, { useClass: this._shortTitleComponent })]);
        //var shipmentTitle = injector.get(shortTitle + "ShortTitleComponent");
        //var shipmentTitle = injector.get(this._shortTitleComponent);
        //console.log(shipmentTitle);

    }

    initializeShortTitleControl() {
        var shortTitle = "Shipment";
        System.import('./app/shipment/short-title/' + 'shipment' + '-short-title.component')
                //.then(result => {
                //    /*result["ShipmentShortTitle" + "Component"];*/
                //    this._shortTitleComponent = result[shortTitle + "ShortTitleComponent"];

                //    this._dynamicComponentLoader.loadIntoLocation(this._shortTitleComponent, this._elementRef, 'ShortTitle'/*, this._injector*/)
                //        .then((res) => {
                //            console.log(res);
                //            res.instance.setParameters(this.EntityPM);
                //        });
            .then(m => {
                //console.log("i'm inside the import for short title");
                this._shortTitleComponent = m["ShipmentShortTitle" + "Component"];
                //console.log("i'm the current entity args ", this.entityArgs.EntityPM);

                this._dynamicComponentLoader.loadIntoLocation(this._shortTitleComponent, this._elementRef, 'ShortTitle');
            })
        //});

    }

    headerScreen: any;
    screenFields: any[];
    headerScreenRows: number;
    headerScreenColumns: number;
    screenFieldsArray: any[];
    MakeRowArrays() {
        var rowsArray = [];
        for (var i = 0; i < this.headerScreenRows; i++) {
            var colsArray = [];
            for (var v = 0; v < this.headerScreenColumns; v++) {
                for (var x = 0; x < this.screenFields.length; x++) {
                    if (this.screenFields[x].Column === v && this.screenFields[x].Row === i) {
                        //colsArray.push(this.screenFields[x]);
                        var objectfield = this.GetObjectField(this.screenFields[x].ObjectFieldId);
                        colsArray.push(objectfield);
                    }
                }
            }
            rowsArray.push(colsArray);
    }
        this.screenFieldsArray = rowsArray;
    }

    MapEntityPM(shipmentPM) {
        //this.EntityPM = shipmentPM;
        this.EntityPM = shipmentPM.json();
        //console.log(this.EntityPM);

        this.hideBusyIndicator = true;
    }
    //screenFieldsArray: any[];
    ScreenFieldsOrderBy = (screenfields: any[]) => {
        var rowsArray = [];
        for (var i = 0; i < this.HeaderScreen.NumberOfRows; i++) {
            var colsArray = [];
            for (var v = 0; v < this.HeaderScreen.NumberOfColumns; v++) {
                for (var x = 0; x < screenfields.length; x++) {
                    if (screenfields[x].Column === v && screenfields[x].Row === i) {
                        //colsArray.push(this.screenFields[x]);
                        var objectfield = this.GetObjectField(screenfields[x].ObjectFieldId);
                        colsArray.push(objectfield);
                    }
                }
            }
            rowsArray.push(colsArray);
        }
        //this.screenFieldsArray = rowsArray;
        return rowsArray;
    }
    GetObjectField = (objectFieldId: string) => {
        //for (var i = 0; i < this.ObjectFieldsOfObjectTable.length; i++) {
        //    if (this.ObjectFieldsOfObjectTable[i].Id === objectFieldId) {
        //        //return this.$window.ObjectFields[i].FullNameTextCodeCode;
        //        return this.ObjectFieldsOfObjectTable[i];
        //    }
        //}
        return window.ObjectFields.filter(d => d.Id === objectFieldId)[0];
    }
    OrderObjectTableTabs(objectTableTabs: any[]) {
        var orderedtabs: any[] = [];
        orderedtabs = objectTableTabs.sort((a, b) => {return a.IndexOrder - b.IndexOrder });
        return orderedtabs;
    }

    //OpenView(ViewCode: string, ObjectTableName: string) {
    //    console.log(ViewCode, ObjectTableName);
    //    this._router.navigate([ViewCode, { id: this._selectedId }]);
    //}

    //selectDefaultTab(defaultTab) {

    //    System.import(defaultTab.HtmlComponentUrl)
    //        .then((result) => {
    //            //console.log(result);
    //            this.CachedImports.push({ HtmlComponentName: defaultTab.HtmlComponentName, ComponentResult: result });
    //            this._dynamicComponentLoader.loadIntoLocation(result[defaultTab.HtmlComponentName], this._elementRef, 'TabView');
    //        })

    //}
    public ActiveViewTab: ComponentRef;
    onSelect(mySelectedTab: any) {
        // IF TAB ALREADY IMPORTED TAKE FROM CACHE OR ARRAY, IF NOT IMPORTED, IMPORT // CHECK
        // Tab Component Link, Tab Component Name // CHECK
        //console.log(mySelectedTab.HtmlComponentName);
        //console.log("---------------------->", this.ActiveViewTab);
        if (this.ActiveViewTab) {
            this.ActiveViewTab.dispose();
        }
        var cmpObject = this.CachedImports.filter(x => x.HtmlComponentName === mySelectedTab.HtmlComponentName)[0];
        if (cmpObject) {
            //console.log("getting from cached imports", cmpObject);
            //return cmpObject;
            //console.log(System.get('http://site.com/normalized/module/name.js').exportedFunction());
            this._dynamicComponentLoader.loadIntoLocation(cmpObject.ComponentResult[mySelectedTab.HtmlComponentName], this._elementRef, 'TabView')
                .then((result) => {
                    //console.log(result);
                    this.ActiveViewTab = result;
                    //console.log();
                });
        }
        else {
            //console.log("getting from system import");
            System.import(mySelectedTab.HtmlComponentUrl/*'./app/shipment/shipment-tabs/Overview/overview.component'*/)
                .then((result) => {
                    //console.log("inside system import", result);
                    var cmpObject = { HtmlComponentName: mySelectedTab.HtmlComponentName, ComponentResult: result };
                    this.CachedImports.push(cmpObject);
                    //console.log(mySelectedTab.HtmlComponentName);
                    this._dynamicComponentLoader.loadIntoLocation(result[mySelectedTab.HtmlComponentName], this._elementRef, 'TabView')
                        .then((result) => {
                            //console.log(result);
                            this.ActiveViewTab = result;
                            //console.log();
                        });
                })
        }


        //if (this.SelectedTab != mySelectedTab) {
        //    this.SelectedTab = mySelectedTab;

        //    //console.log(mySelectedTab.Code, mySelectedTab.ObjectTableName);
        //    this._router.navigate([mySelectedTab.Code, { id: this._selectedId }]);
        //}
    }

    GoBackToList() {
        //this._router.renavigate();
        this._router.navigate(['MainMenu', 'ShipmentsList']);
    }
    SaveAndClose(close: boolean) {
        //this._shipmentsService.UpdateShipment(this.EntityPM);
        if (close) {
            //this._router.renavigate();
            this._router.navigate(['MainMenu', 'ShipmentsList']);
        }
    }

    }

class LogitudeWindow extends Window {
    ScreenFields: any[];
    ObjectFields: any[];
}
