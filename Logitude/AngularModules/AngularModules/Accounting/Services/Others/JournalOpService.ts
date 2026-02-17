import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {JournalList} from '../../EntityLists/JournalList';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {JournalPM} from '../../EntityPMs/JournalPM';
import {JournalLinePM} from '../../EntityPMs/JournalLinePM';
import {Guid} from '../../../Infrastructure/Utilities/Guid';

@Injectable()

export class JournalOpService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/JournalOp';
    }

    GetYearTransferJournal(year: string, myOperation: string, lastYearTransferJournalPMId: string ) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var url = this._apiUrl + '/GetYearTransferJournal?year=' + year + "&myOperation=" + myOperation + "&lastYearTransferJournalPMId=" + lastYearTransferJournalPMId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var result = response.json();
                var entity: JournalPM;
                if (result) {
                    entity = this.MapJsonToEntityPM(result);
                }
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetTaskLoadTest(tenant: number, actionType: string, amount: number, sleepEveryMinute: number, year: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var url = this._apiUrl + '/GetTaskLoadTest?tenant=' + tenant.toString() + "&actionType=" + actionType + "&amount=" + amount.toString() + "&sleepEveryMinute=" + sleepEveryMinute.toString() + "&year=" + year;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var result = response.json();
                
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: JournalPM = null) {


        if (!entityPM) {

            entityPM = new JournalPM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }

        var oldJournalLines: JournalLinePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldJournalLines = entityPM.OldEntityPM.JournalLines;
        }


        entityPM.JournalLines = new Array<JournalLinePM>();
        for (var item in jsonPM.JournalLines) {

            var jItem = jsonPM.JournalLines[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newJournalLinePM: JournalLinePM;
            if (mapParent) {
                newJournalLinePM = new JournalLinePM(entityPM);
            }
            else {
                newJournalLinePM = new JournalLinePM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {

                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newJournalLinePM[pmProperty] = jItem[pmProperty];
            }
            newJournalLinePM.IsDirty = false;
            if (mapParent) {
                newJournalLinePM.OldEntityPM = this.clone(newJournalLinePM);
                newJournalLinePM.UniqueKey = Guid.newGuid();
                newJournalLinePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";

            }
            else {

                if (newJournalLinePM.UniqueKey) {

                    if (jItem.IsDirty)
                        newJournalLinePM.ChangeSetOp = "Update";
                }
                else {
                    newJournalLinePM.ChangeSetOp = "Insert";
                }

                newJournalLinePM.OldEntityPM = null;
                newJournalLinePM.EntityParentPM = null;
            }


            entityPM.JournalLines.push(newJournalLinePM);
        }

        if (oldJournalLines) {

            for (var itemKey in oldJournalLines) {
                if (entityPM.JournalLines.filter(p => p.UniqueKey === oldJournalLines[itemKey].UniqueKey).length === 0) {

                    if (oldJournalLines[itemKey]) {
                        oldJournalLines[itemKey].ChangeSetOp = "Delete";
                        entityPM.JournalLines.push(oldJournalLines[itemKey]);
                    }
                }
            }
        }
        entityPM.IsDirty = false;

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.JournalLines = [];
            for (var m in entityPM.JournalLines) {
                entityPM.OldEntityPM.JournalLines.push(this.clone(entityPM.JournalLines[m]));
            }
        }
        else {

            entityPM.OldEntityPM = null;
        }

        return entityPM;
    }

    public clone(jsonPM: any) {
        var entityPM: any;
        entityPM = {};

        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {

            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM") {
                continue;
            }

            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];

        }
        return entityPM;
    }

}
