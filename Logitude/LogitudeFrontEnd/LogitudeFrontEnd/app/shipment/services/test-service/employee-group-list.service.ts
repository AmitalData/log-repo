import {Injectable} from 'angular2/core';
import {Http} from 'angular2/http';
import 'rxjs/add/operator/map';
import {EmployeeGroupList} from '../../models/employee-group-list';
import {QueryOperations} from '../../models/query-operations';

@Injectable()
export class EmployeeGroupListService {

    public _sharedEmployeeGroupListArray: Array<EmployeeGroupList> = [];
    private _apiUrl: string = 'http://localhost:9996/api/EmployeeGroupPMs';

    constructor(private _http: Http) {
        console.log("EmployeeGroupPMService instantiated");
        console.log(this._sharedEmployeeGroupListArray);
    }

    getSingleEntityListFromServer(id: string, tenant: number) {

        return this._http.get(this._apiUrl + '?id=')
            .map(res => res.json());
    }

    //getSingleEntityFromArray(id: string) {

    //    return this._sharedEmployeeGroupListArray.filter(c => c.Id === id)[0];
    //}

   
    getSingleEntityList(id: string, tenant: number) {

        var exists = this._sharedEmployeeGroupListArray.filter(c => c.Id === id);
        if (!exists) {

            return this._http.get(this._apiUrl + '?id=' + id)
                .map(res => {
                    var pm = res.json();
                    this._sharedEmployeeGroupListArray.push(pm);

                    return Promise.Resolve(pm);
                });
            //this.SharedShipmentPMList.push(shipmentsPromise.then(shipments => shipments.filter(c => c.Id === id)[0]));
        }
        else {

            return Promise.Resolve(this._sharedEmployeeGroupListArray.filter(c => c.Id === id)[0]);
        }


    }

    getAllEntityListsFromServer(tenant: number) {
        return this._http.get(this._apiUrl)
            .map(res => {
                var dataList = res.json();
                //this._sharedEmployeeGroupListArray.push(dataList);

                return Promise.Resolve(dataList);
            });
    }

    getFilteredEntityListsFromServer(filters: QueryOperations) {

        var request = JSON.stringify(filters);
        return this._http.get(this._apiUrl + '?queryOperations=');
            //.map(res => res.json());
    }
    //this.SharedShipmentPM = shipmentsPromise
    //    .then(shipments => shipments.filter(c => c.Id === id)[0]);
    ////console.log(this.SharedShipmentPM);
    //return shipmentsPromise
    //    .then(shipments => shipments.filter(c => c.Id === id)[0]);
}

   
