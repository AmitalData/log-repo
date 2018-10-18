import {Injectable} from 'angular2/core';
import {Http} from 'angular2/http';
import 'rxjs/add/operator/map';

import {EmployeeGroupPM} from '../../models/employee-group-pm';

@Injectable()
export class EmployeeGroupPMService {
  
    private _sharedEmployeeGroupPMArray: Array<EmployeeGroupPM> = [];
    private _apiUrl: string = 'http://localhost:9996/api/EmployeeGroupPMs';

    constructor(private _http: Http) {
        console.log("EmployeeGroupPMService instantiated");
        console.log(this._sharedEmployeeGroupPMArray);
    }

    getSingleEntityPMFromServer(id: string) {

        return this._http.get(this._apiUrl + '?id=' + id)
            .map(res => res.json());
    }

    //getSingleEntityFromArray(id: string) {

    //    return this.SharedEmployeeGroupPMList.filter(c => c.Id === id)[0];
    //}

   
    getSingleEntityPM(id: string) {
         
        var exists = this._sharedEmployeeGroupPMArray.filter(c => c.Id === id);
            if (!exists) {

                return this._http.get(this._apiUrl + '?id=' + id)
                    .map(res => {
                        var pm = res.json();
                        this._sharedEmployeeGroupPMArray.push(pm);

                        return Promise.Resolve(pm);
                    });
                //this.SharedShipmentPMList.push(shipmentsPromise.then(shipments => shipments.filter(c => c.Id === id)[0]));
            }
            else {

                return Promise.Resolve(this._sharedEmployeeGroupPMArray.filter(c => c.Id === id)[0]);
            }  
        //this.SharedShipmentPM = shipmentsPromise
        //    .then(shipments => shipments.filter(c => c.Id === id)[0]);
        ////console.log(this.SharedShipmentPM);
        //return shipmentsPromise
        //    .then(shipments => shipments.filter(c => c.Id === id)[0]);
    }

    updateEntityPM(entityPM: EmployeeGroupPM) {
    }

    insertEntityPM(entityPM: EmployeeGroupPM) {
    }
}