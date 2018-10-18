import {Injectable, Injector, provide} from 'angular2/core';
import {Http, Headers} from 'angular2/http';
import {AsyncRoute} from 'angular2/router';
import Rx from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import {ShipmentPM} from '../../EntityPMs/ShipmentPM';
import {ShipmentPackagePM} from '../../EntityPMs/ShipmentPackagePM';


@Injectable()
export class ShipmentsService {

    //public SharedShipmentPM: Promise<ShipmentPM>;
    public SharedShipmentPMArray: Array<ShipmentPM> = [];
    private _apiUrl: string = "http://localhost:9996/api/ngShipments";

    constructor(private _http: Http) {
        //console.log("ShipmentsService instantiated");
        console.warn("ShipmentsService instantiated");
        //console.log(this.SharedShipmentPM);
    }

    GetShipmentsList(tenant: number, pageSize: number, startRow: number, sortingCol: string, sortingDir: string) {
        return this._http.get('http://localhost:9996/api/ngShipments' + '?tenant=' + tenant + '&PageSize=' + pageSize + '&PageIndex=' + startRow + '&sortby=' + sortingCol + '&sortDir=' + sortingDir + '&searchfields=sss')
            .map(res => res.json());
    }

    GetShipmentsListCount(tenant: number) {
        return this._http.get('http://localhost:9996/api/ngShipments' + '?tenant=' + tenant)
            .map(res => res.json());
    }

    getSingleEntityPM(id: string, tenant: number) {

        console.log('--------------------------------------> calling getSingleEntityPM:');
        var authHeader = new Headers();
        authHeader.append('Token', '2acc0a3d-a948-41fe-8a19-51a6d787e4fc');
        var exists = this.SharedShipmentPMArray.filter(c => c.Id === id).length;
        if (exists === 0) {
            console.log('server call ---------');
            return Rx.Observable.defer(() => {
                return this._http.get(this._apiUrl + '?id=' + id + '&tenant=' + tenant, {
                    headers: authHeader
                }).map(response => {
                    var pm = response.json();
                    var shipment: ShipmentPM;
                    shipment = this.MapJsonToEntityPM(pm);
                    this.SharedShipmentPMArray.push(shipment);
                    return shipment;
                });
            }

            );
        }
        else {
            console.log('cache call ---------');
            return Rx.Observable.fromArray(this.SharedShipmentPMArray).filter(c => c.Id === id);
        }



    }

    getSingleEntityPMFromServer(id: string, tenant: number) {

        //console.log('--------------------------------------> calling getSingleEntityPMFromServer:');
        var authHeader = new Headers();
        authHeader.append('Token', '2acc0a3d-a948-41fe-8a19-51a6d787e4fc');
        
            return Rx.Observable.defer(() => {
                return this._http.get(this._apiUrl + '?id=' + id + '&tenant=' + tenant, {
                    headers: authHeader
                }).map(response => {
                    var pm = response.json();
                    var shipment: ShipmentPM;
                    shipment = this.MapJsonToEntityPM(pm);
                    this.SharedShipmentPMArray.push(shipment);
                    return shipment;
                });
            }

            );
       
    }

    getSingleShipmentById(id: string, tenant: number) {
        return this._http.get('http://localhost:9996/api/ngShipments?id=' + id + '&tenant=' + tenant)
            .map(res => {
                this.SharedShipmentPMArray.push(res.json());
                return this.SharedShipmentPMArray.filter(d => d.Id === id)[0];
                //return res.json();
            });
    }

    getSingleShipmentByIdFromArray(id: string, tenant: number) {
        var pm = this.SharedShipmentPMArray.filter(d => d.Id === id)[0];
        if (pm) {
            return pm;
        }
    }

    getShipmentFromArrayById(id: string) {
        return this.SharedShipmentPMArray.filter(c => c.Id === id)[0];
    }

    MapJsonToEntityPM(jsonPM: any) {
       
            var entityPM: ShipmentPM;
            entityPM = new ShipmentPM();
            var jsonPMKeys = Object.keys(jsonPM);

            for (var key in jsonPMKeys) {
                var property = jsonPMKeys[key];
                entityPM[property] = jsonPM[property];
            }

            entityPM.ShipmentPackages = new Array<ShipmentPackagePM>();

        for (var pack in jsonPM.ShipmentPackages) {
                var shipPack = jsonPM.ShipmentPackages[pack];
                var shipmentpackagePM: ShipmentPackagePM;
                shipmentpackagePM = new ShipmentPackagePM(entityPM);
                var packPmKeys = Object.keys(shipPack);
                for (var packKey in packPmKeys) {
                    var packProperty = packPmKeys[packKey];
                    shipmentpackagePM[packProperty] = shipPack[packProperty];
                }
                shipmentpackagePM.IsDirty = false;
                entityPM.ShipmentPackages.push(shipmentpackagePM);
            }

            entityPM.IsDirty = false;
        return entityPM;
    }

   
    GetRouts() {
        let routs = [
            //{ path: '/overview/:id', name: 'SHOV', component: OverviewComponent/*, useAsDefault: true*/ },
            new AsyncRoute({
                path: '/overview/:id',
                loader: () => System.import('./app/shipment/shipment-tabs/overview/overview.component').then(m => m["OverviewComponent"]),
                name: 'SHOV',
                useAsDefault: true
            }),
            //{ path: '/general/:id', name: 'SHGC', component: GeneralComponent },
            new AsyncRoute({
                path: '/general/:id',
                loader: () => System.import('./app/shipment/shipment-tabs/general/general.component').then(m => m["GeneralComponent"]),
                name: 'SHGC',
            }),
            //{ path: '/packages/:id', name: 'SHPK', component: PackagesComponent },
            new AsyncRoute({
                path: '/packages/:id',
                loader: () => System.import('./app/shipment/shipment-tabs/packages/packages.component').then(m => m["PackagesComponent"]),
                name: 'SHPK',
            }),
            //{ path: '/Partners/:id', name: 'SHPA', component: PartnersComponent },
            new AsyncRoute({
                path: '/Partners/:id',
                loader: () => System.import('./app/shipment/shipment-tabs/Partners/PartnersTab.component').then(m => m["PartnersComponent"]),
                name: 'SHPA',
            }),
            //{ path: '/Routings/:id', name: 'SHRT', component: RoutingsComponent },
            new AsyncRoute({
                path: '/Routings/:id',
                loader: () => System.import('./app/shipment/shipment-tabs/Routings/RoutingsTab.component').then(m => m["RoutingsComponent"]),
                name: 'SHRT',
            }),
            //{ path: '/Payables/:id', name: 'SHPY', component: PayablesComponent },
            new AsyncRoute({
                path: '/Payables/:id',
                loader: () => System.import('./app/shipment/shipment-tabs/Payables/PayablesTab.component').then(m => m["PayablesComponent"]),
                name: 'SHPY',
            }),
            //{ path: '/Receivables/:id', name: 'SHRE', component: ReceivablesComponent },
            new AsyncRoute({
                path: '/Receivables/:id',
                loader: () => System.import('./app/shipment/shipment-tabs/Receivables/ReceivablesTab.component').then(m => m["ReceivablesComponent"]),
                name: 'SHRE',
            }),
            //{ path: '/Order/:id', name: 'SHOR', component: OrderComponent },
            new AsyncRoute({
                path: '/Order/:id',
                loader: () => System.import('./app/shipment/shipment-tabs/Order/OrderTab.component').then(m => m["OrderComponent"]),
                name: 'SHOR',
            }),
            //{ path: '/Master/:id', name: 'SHMS', component: MasterComponent },
            new AsyncRoute({
                path: '/Master/:id',
                loader: () => System.import('./app/shipment/shipment-tabs/Master/MasterTab.component').then(m => m["MasterComponent"]),
                name: 'SHMS',
            }),
            //{ path: '/Shipments/:id', name: 'SHCO', component: ShipmentsComponent },
            new AsyncRoute({
                path: '/Shipments/:id',
                loader: () => System.import('./app/shipment/shipment-tabs/Shipments/ShipmentsTab.component').then(m => m["ShipmentsComponent"]),
                name: 'SHCO',
            }),
            //{ path: '/CustomsFile/:id', name: 'SHCF', component: CustomsFileComponent },
            new AsyncRoute({
                path: '/CustomsFile/:id',
                loader: () => System.import('./app/shipment/shipment-tabs/CustomsFile/CustomsFileTab.component').then(m => m["CustomsFileComponent"]),
                name: 'SHCF',
            }),
            //{ path: '/FreightFiles/:id', name: 'SHFF', component: FreightFilesComponent },
            new AsyncRoute({
                path: '/FreightFiles/:id',
                loader: () => System.import('./app/shipment/shipment-tabs/FreightFiles/FreightFilesTab.component').then(m => m["FreightFilesComponent"]),
                name: 'SHFF',
            }),
            //{ path: '/Events/:id', name: 'SHEV', component: EventsComponent },
            new AsyncRoute({
                path: '/Events/:id',
                loader: () => System.import('./app/shipment/shipment-tabs/Events/EventsTab.component').then(m => m["EventsComponent"]),
                name: 'SHEV',
            }),
            //{ path: '/DocsIn/:id', name: 'SHDI', component: DocsInComponent },
            new AsyncRoute({
                path: '/DocsIn/:id',
                loader: () => System.import('./app/shipment/shipment-tabs/DocsIn/DocsInTab.component').then(m => m["DocsInComponent"]),
                name: 'SHDI',
            }),
            //{ path: '/DocsOut/:id', name: 'SHDO', component: DocsOutComponent },
            new AsyncRoute({
                path: '/DocsOut/:id',
                loader: () => System.import('./app/shipment/shipment-tabs/DocsOut/DocsOutTab.component').then(m => m["DocsOutComponent"]),
                name: 'SHDO',
            }),
            //{ path: '/Communications/:id', name: 'SHCM', component: CommunicationsComponent },
            new AsyncRoute({
                path: '/Communications/:id',
                loader: () => System.import('./app/shipment/shipment-tabs/Communications/CommunicationsTab.component').then(m => m["CommunicationsComponent"]),
                name: 'SHCM',
            }),
        ]
                return routs;
    }
}
