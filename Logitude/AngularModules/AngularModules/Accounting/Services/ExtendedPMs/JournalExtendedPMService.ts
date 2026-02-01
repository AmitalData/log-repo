import {Injectable} from '@angular/core';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {JournalPM} from '../../EntityPMs/JournalPM';
import {JournalLinePM} from '../../EntityPMs/JournalLinePM';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
import { ImageParameter } from '../../../Infrastructure/DataContracts/ImageParameter';
 

@Injectable()

export class JournalExtendedPMService {

    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {
  
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + /*'api/journalviews'*/ 'api/journalop';
    }


    VoidJournal(tenant, JournalId, AccountingEntityCode, AccountingEntityId, AccountingEntityReference) {
        //http://localhost:9996/api/JournalOp?JournalOp=void&JournalId=1-93808&tenant=1071&AccountingEntityCode=7&AccountingEntityId=Deposit1212&AccountingEntityReference=Cash%20Deposit%207



        let url = this._apiUrl + '?JournalOp=void&JournalId=' + JournalId + '&tenant=' + tenant + '&AccountingEntityCode=' + AccountingEntityCode + '&AccountingEntityId=' + AccountingEntityId + '&AccountingEntityReference=' + AccountingEntityReference;
        return this.httpClient.delete( url,  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                var pm = response;
                if (pm) {
                    var mappedResult: JournalPM;
                
                    serviceResponse.Result = mappedResult;
                }


                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
       

    }


    GetByAccountingEntityId(accountingEntityId: string, accountingEntityCode:string) {
   
      return this.httpClient.get(this._apiUrl + '/GetJournalByAccountingEntityId?accountingEntityId=' + accountingEntityId + '&accountingEntityCode=' + accountingEntityCode,   ServiceHelper.GetHttpHeaders()).pipe(
        map(res => {
            var serviceResponse: ServiceResponse = new ServiceResponse();

            var result = res;
            var entity: JournalPM;
            if (result) {
                entity = this.MapJsonToEntityPM(result);
            }
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            serviceResponse.Result = entity;
            return serviceResponse;
        }),
        catchError(ServiceHelper.HandleServiceError));
     

    }
    PostJournalAsCSV(fileUploadParamerter: ImageParameter) {

        return this.httpClient.post(this._apiUrl + '/PostJournalAsCSV', JSON.stringify(fileUploadParamerter), ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var result = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result;
                return pmresponse;
            }),
            catchError(ServiceHelper.HandleServiceError));


    }

    GetFailedJournalsInReconcileProcess(accountId: string) {
   
        return this.httpClient.get(this._apiUrl + '/GetFailedJournalInReconcileProcess?accountId=' + accountId, ServiceHelper.GetHttpHeaders()).pipe(
          map(res => {
              var serviceResponse: ServiceResponse = new ServiceResponse();
              serviceResponse.Result = res;
              return serviceResponse;
          }),
          catchError(ServiceHelper.HandleServiceError));
       
  
      }

    
    PostJournalAsCSVWithSkip(fileUploadParamerter: ImageParameter) {

        return this.httpClient.post(this._apiUrl + '/PostJournalAsCSVWithSkip', JSON.stringify(fileUploadParamerter), ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var result = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result;
                return pmresponse;
            }),
            catchError(ServiceHelper.HandleServiceError));


    }
    PostJournalAsMichpal(entityPM: JournalPM) {
        var mappedEntity: JournalPM = this.MapJsonToEntityPM(entityPM, false);

        return this.httpClient.post(this._apiUrl + '/PostJournalAsMichpal', JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var result = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result;
                return pmresponse;
            }),
            catchError(ServiceHelper.HandleServiceError));


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
