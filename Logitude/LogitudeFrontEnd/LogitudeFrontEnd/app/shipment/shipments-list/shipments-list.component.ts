import {Component, OnInit, provide, ElementRef, Injector, IterableDiffers, KeyValueDiffers, Renderer}     from 'angular2/core';
import {Router, RouteConfig, RouterOutlet, ROUTER_DIRECTIVES} from 'angular2/router';
import {HTTP_PROVIDERS, Http} from 'angular2/http';
import {CORE_DIRECTIVES} from 'angular2/common';

import {ShipmentsService} from '../services/shipment-service/shipments.service';
import {ShipmentPM} from '../EntityPMs/ShipmentPM';
import {TextcodeTranslationPipe} from '../../infrastructure/pipes/textcode-translation/textcode-translation.pipe';

import {ModalDialogInstance} from '../../infrastructure/modal-libs/angular2-modal/models/ModalDialogInstance';
import {ModalConfig} from '../../infrastructure/modal-libs/angular2-modal/models/ModalConfig';
import {Modal} from '../../infrastructure/modal-libs/angular2-modal/providers/Modal';
import {ICustomModal} from '../../infrastructure/modal-libs/angular2-modal/models/ICustomModal';
import {YesNoModalContent, YesNoModal} from '../../infrastructure/modal-libs/angular2-modal/commonModals/yesNoModal';
import {OKOnlyContent, OKOnlyModal} from '../../infrastructure/modal-libs/angular2-modal/commonModals/okOnlyModal';
import {AdditionCalculateWindowData, AddNewShipmentWindow} from '../new-shipment-modal/new-shipment-modal.component';

import {SampleElement} from '../../infrastructure/modal-libs/sampleElement';
import {IconButton} from '../../ApplicationControls/IconButton'

@Component({
    templateUrl: './app/shipment/shipments-list/shipments-list.component.html',
    directives: [CORE_DIRECTIVES, (<any>window).ag.grid.AgGridNg2, SampleElement, IconButton],
    providers: [HTTP_PROVIDERS, Modal],
    pipes: [TextcodeTranslationPipe],
})

export class ShipmentsListComponent implements OnInit {

    public ShowModalWindow: boolean = false;

    constructor(
        private _modal: Modal,
        private _injector: Injector,
        private _router: Router,
        private _shipmentsService: ShipmentsService,
        private _elementRef: ElementRef,
        private _renderer: Renderer
    ) {
        //var injector = Injector.resolveAndCreate([
        //    provide("validToken", { useValue: "Value" })
        //]);
        //this.mySampleElement = this._elementRef;
        this.ShowModalWindow = true;
        this.ShowModalWindow = false;
    }

    public SearchText: string = "Search Partners / Ports / Ref.#";
    public IsAdvancedSearchOpened: boolean = false;
    onOpenFilterAreaClick() {
        this.IsAdvancedSearchOpened = true;
    }

    onCloseFilterAreaClick() {
        this.IsAdvancedSearchOpened = false;
    }

    public rowData: Array<ShipmentPM>;
    public gridOptions: any = {
        //    ////rowData: null,
        //    ////rowHeight: 35,
        //    ////rowSelection: 'single',
        //    //suppressMultiSort: false,
        //    ////enableColResize: true,
        //    ////enableSorting: false,
            //enableFilter: false,
            //enableServerSideSorting: true,
            //enableServerSideFilter: false,
            //enableCellExpressions: true,
            //angularCompileHeaders: true,
            //angularCompileRows: true,
            //unSortIcon: false,
        //    //showToolPanel: false,
        //    //virtualPaging: true,
        //    ////debug: true,
            headerCellRenderer: this.headerCellRendererFunc,
        //    ////columnDefs: this.columnDefs
    };
    public showGrid: boolean;
    public dataCount: number;

    ngOnInit() {
        //this._shipmentsService.getShipments().then(res => { this.rowData = res; this.showGrid = true; });
        this.RefreshGrid();
    }

    headerCellRendererFunc(params) {
        //console.log(params.value);
        if (params.value === "DirectionId") {
            return "<img height='20px' width='22px' src='../img/headerD.png' />"
        }
        if (params.value === "TransportModeId") {
            return "<img height='20px' width='22px' src='./img/headerTM.png' />"
        }
        else {
            return params.value;
        }
    }
    columnDefs = [
        { field: "Id", headerName: "Id", width: 130 },
        { field: "ShipmentNumber", headerName: "Shipment #", width: 200 },
        { field: "Shipper", headerName: "Shipper" },
        { field: "Consignee", headerName: "Consignee" },
        { field: "DirectionId", headerName: "DirectionId", width: 50, suppressMenu: true, suppressSorting: true/*, cellRenderer: this.directionsConverter*//*, template: "<img height='20px' width='22px' src='../img/Directions/{{data.DirectionId}}.png' />"*/ },
        { field: "TransportModeId", headerName: "TransportModeId", width: 50, suppressMenu: true, suppressSorting: true/*, cellRenderer: this.transportmodesConverter*//*, template: "<img height='20px' width='20px' src='../img/TransportModes/{{data.TransportModeId}}.png' />"*/ },
        { field: "CreateDateTime", headerName: "Date Created", width: 220/*, cellRenderer: this.dateValueConverter */},
        { field: "Routing", headerName: "Routing", width: 200/*, cellRenderer: this.routingValueGetter */},
        { field: "FromPort", headerName: "From Port", width: 100 },
        { field: "ToPort", headerName: "To Port", width: 100 },
        { field: "FromCountryName", headerName: "From Country", width: 300/*, cellRenderer: this.countryFlag */},
        { field: "StatusName", headerName: "Status", width: 130/*, cellRenderer: this.statusColor */}
    ];
    routingValueGetter(params) {
        if (params.data.FromPort != null && params.data.ToPort != null) {
            return params.data.FromPort + " > " + params.data.ToPort;
        }
    }
    dateValueConverter(params) {
    if (params.value) {
        var dateVar = new Date(params.value);
        var minutes = dateVar.getMinutes() < 10 ? "0" + dateVar.getMinutes() : dateVar.getMinutes();
        if (dateVar.getDate() == new Date().getDate()) {
            return "Today" + "<span style='float:right;color:black;' class='label'>" + dateVar.getHours() + ":" + minutes + "</span>";
        }
        //var date = new Date();
        //date.setTime(Date);
        if (dateVar.getDate() == new Date().getDate() - 1) {
            return "Yesterday" + "<span style='float:right;color:black;' class='label'>" + dateVar.getHours() + ":" + minutes + "</span>";
        }
        else {
            return "<span>" + dateVar.getDate() + "/" + dateVar.getMonth() + "/" + dateVar.getFullYear() + "</span>" + "<span style='float:right;color:black;' class='label'>" + dateVar.getHours() + ":" + minutes + "</span>";
        }
    }
    }
    directionsConverter(params) {
        if (params.data.DirectionId) {
            return "<img height='20px' width='22px' src='images/Directions/" + params.data.DirectionId + ".png' />";
        }
    }
    transportmodesConverter(params) {
        if (params.data.TransportModeId) {
            return "<img height='20px' width='20px' src='images/TransportModes/" + params.data.TransportModeId + ".png' />";
        }
    }
    countryFlag(params) {
        var flagValue = new String(params.value);
        if (params.data.FromCountryCode != null && params.data.FromCountryCode != undefined) {
            //if (params.data.FromPortCountry != null && params.data.FromPortCountry != undefined) {
            return "<img height='30px' width='30px' src='images/Flags/" + params.data.FromCountryCode + ".png' /> <span>" + (params.data.FromPortCountry ? params.data.FromPortCountry : "") + "</span>";
            //}
            //return params.data.FromCountryCode;
        }
        else {
            return null;
        }
    }
    statusColor(params) {
        var statusValue = new String(params.value);
        if (statusValue == "Order") {
            return "<span class='label label-sm label-success'>" + params.value + "</span>";
        }
        if (statusValue == "Arrived") {
            return "<span class='label label-sm label-primary'>" + params.value + "</span>";
        }
        if (statusValue == "Delivered") {
            return "<span class='label label-sm label-info'>" + params.value + "</span>";
        }
        if (statusValue == "Departed") {
            return "<span class='label label-sm label-warning'>" + params.value + "</span>";
        }
        if (statusValue == "Pick Up") {
            return "<span class='label label-sm label-default'>" + params.value + "</span>";
        }
        if (statusValue == "Delivery") {
            return "<span class='label label-sm label-danger'>" + params.value + "</span>";
        }
        if (statusValue == "Cleared") {
            return "<span class='label label-sm label-warning'>" + params.value + "</span>";
        }
        if (statusValue == "On Hand") {
            return "<span class='label label-sm label-default'>" + params.value + "</span>";
        }
    }

    onRowSelected($event) {
        //console.log("Row Selected", $event.node.data);
        this._router.navigate(['/Root', 'EditControl', { id: $event.node.data.Id }, /*'DYNAMIC', 'SHOV', { id: $event.node.data.Id }*/]);
    }

    dataSource = {
        rowCount: this.dataCount,
        pageSize: 20,
        overflowSize: 20,
        maxConcurrentRequests: 1,
        maxPagesInCache: 50,
        getRows: (params) => {
            console.log('asking for ' + params.startRow + ' to ' + params.endRow);
            var sortingCol, sortingDir;
            //if (params) {
            //    if (typeof params.sortModel[0] !== 'undefined') {
            //        sortingCol = params.sortModel[0].colId;
            //        sortingDir = params.sortModel[0].sort;
            //        console.log(sortingCol, sortingDir);
            //    } else {
                    sortingCol = "CreateDateTime";
                    sortingDir = "desc";
            //    }
            //}
            this._shipmentsService.GetShipmentsList(/*this.userData.CurrentTenant*/1, 20, params.startRow, sortingCol, sortingDir)
                .subscribe((shipmentList: Array<ShipmentPM>) => {
                    var rowsThisPage = shipmentList;
                    // if on or after the last page, work out the last row.
                    var lastRow = -1;
                    if (params.endRow > this.dataCount) {
                        lastRow = this.dataCount;
                    }
                    if (params.endRow <= this.dataCount) {
                        lastRow = params.endRow;
                    }
                    // call the success callback
                    params.successCallback(rowsThisPage, lastRow); // or $scope.dataCount
                    console.log(params.startRow)
                });
        }
    };

    RefreshGrid = () => {
        this._shipmentsService.GetShipmentsListCount(1).subscribe((shipmentsListCount: number) => {
            this.dataCount = shipmentsListCount;
            //console.log('in the count callback')
            this.dataSource.rowCount = this.dataCount;
            this.gridOptions.api.setDatasource(this.dataSource);
            //this.gridOptions.api.hideOverlay();
        });
    }

    onReady($event) {
        //this.gridOptions.api.sizeColumnsToFit();
    }

    onRowClicked($event) {
        //console.log("Row Clicked", $event.data);
    }

    public mySampleElement: ElementRef; // = this._elementRef;
    public lastModalResult: string;
    AddNewShipment(type: string) {
        this.ShowModalWindow = true;
        let dialog: Promise<ModalDialogInstance>;
        let component = (type == 'customWindow') ? AddNewShipmentWindow : YesNoModal;
       // let bindings = Injector.resolve([provide(ICustomModal, { useValue: ShipmentsListComponent.modalData[type] })]);
        let bindings = Injector.resolve([
            provide(ICustomModal, { useValue: ShipmentsListComponent.modalData[type] }),
            provide(IterableDiffers, { useValue: this._injector.get(IterableDiffers) }),
            provide(KeyValueDiffers, { useValue: this._injector.get(KeyValueDiffers) }),
            provide(Renderer, { useValue: this._renderer })
        ]);

        if (type === "inElement") {
            dialog = this._modal.openInside(<any>component, this.mySampleElement, "myModal", bindings, ShipmentsListComponent.modalConfigs[type]);
        }
        //else if (type === "large") {
        //    dialog = this._modal.openInside(<any>component, this.mySampleElement, "myModal", bindings, ShipmentsListComponent.modalConfigs[type]);
        //}
        //else {
        //    dialog = this._modal.open(<any>component, bindings, ShipmentsListComponent.modalConfigs[type]);
        //}

        dialog.then((resultPromise) => {
            return resultPromise.result.then((result) => {
                console.log(result);
                this.lastModalResult = result;
                this.ShowModalWindow = false;
            }, () => { console.log("Rejected!"); this.lastModalResult = 'Rejected!'; this.ShowModalWindow = false; });
        });
    }

    static modalConfigs = {
        'large': new ModalConfig("lg", false, 27),
        'small': new ModalConfig("sm", false, 27),
        'yesno': new ModalConfig("sm", false, 27),
        'key': undefined, // Modal will use default config, which we set at app bootstrap (setting in app bootstrap is optional)
        'blocking': new ModalConfig("lg", true, null), // null for keyboard means no keyboard keys can close the modal.
        'inElement': new ModalConfig("lg", true, null),
        'customWindow': new ModalConfig("lg", true, null)
    };
    static modalData = {
        'large': new YesNoModalContent('Simple Large modal', 'Press ESC or click OK / outside area to close.', true),
        'small': new YesNoModalContent('Simple Small modal', 'Press ESC or click OK / outside area to close.', true),
        'yesno': new YesNoModalContent('Simple 2 button custom modal', 'Answer the question', false, "Yes", "No"),
        'key': new YesNoModalContent('Special Exit Key', 'Press q to close.', true),
        'blocking': new YesNoModalContent('Simple Blocking modal', 'You can only click OK to close this modal.', true),
        'inElement': new YesNoModalContent('Simple In Element modal', 'Try stacking more modals, click OK to close.', true),
        'customWindow': new AdditionCalculateWindowData(2, 3)
    };

}