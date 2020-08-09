import {Injectable} from '@angular/core';
import { defer, of } from 'rxjs';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ExternalReconciliationPM} from '../../EntityPMs/ExternalReconciliationPM';
import {LedgerTransactionPM} from '../../EntityPMs/LedgerTransactionPM';
import {JournalPM} from '../../EntityPMs/JournalPM';
import {ReconciliationLinePM} from '../../EntityPMs/ReconciliationLinePM';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import { ExternalReconciliationLinePM } from '../../EntityPMs/ExternalReconciliationLinePM';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
 
@Injectable()

export class ExternalReconciliationExtendedPMService {

    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {
   
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ExternalReconciliationExtended';
    }
    
    


    CreateJournalReconcileAdjustBankFee(
        reconcileExternalPageLineId,//        reconcileExternalPageLineIdList: string[],
        ledgerTransactionIds: string[],
        TheAccountId: string, AdjustAccountId: string, AccountDate: string,Remarks: string) {

        return defer(() => {

           

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
           

            return this.httpClient.post(this._apiUrl + "/PostCreateJournalReconcileAdjustBankFee?"
                
                + "&reconcileExternalPageLineId=" + reconcileExternalPageLineId
            + "&TheAccountId=" + TheAccountId
            + "&AdjustAccountId=" + AdjustAccountId
            + "&AccountDate=" + AccountDate                
            + "&Remarks=" + Remarks
                , JSON.stringify(ledgerTransactionIds),  ServiceHelper.GetHttpHeaders()).pipe(
                map(res => {
                    var pm = res;
                    if (pm) {
                        var mappedResult: JournalPM;
                        //mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                        serviceResponse.Result = pm;
                    }
                    return serviceResponse;
                }),
                catchError(ServiceHelper.HandleServiceError));
          
            
        }

        );

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
