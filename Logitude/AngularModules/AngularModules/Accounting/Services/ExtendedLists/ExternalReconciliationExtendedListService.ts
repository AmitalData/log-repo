import {Injectable} from '@angular/core';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {LedgerTransactionList} from '../../EntityLists/LedgerTransactionList';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'

@Injectable()

export class ExternalReconciliationExtendedListService {

    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {

        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ExternalReconciliation';
    }

    getExternalAutomaticReconcilationsByFilter(args: ExternalAutoReconcileServiceArgs)
    {


        var url = this._apiUrl + "/GetExternalAutomaticReconcilationsByFilter";

        var urlparameters =
            '?amountReconcile=' + args.amountReconcile
            + '&referenceReconcile=' + args.referenceReconcile
            + '&refDateReconcile=' + args.refDateReconcile
            + '&accoutingDateReconcile=' + args.accoutingDateReconcile
            + '&objectTableId=' + args.objectTableId
            + '&entityId=' + args.entityId
            + '&glAccountId=' + args.glAccountId;
            // + '&filters=' + args.filters;


        var mykeys = Object.keys(args.filters);
        var addtionalFiltersValues = null;
        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = args.filters[propName];

            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");

            if (urlparameters != "?") {
                urlparameters = urlparameters.concat('&');
            }
            if (!ignoreFilter) {
                propValue = encodeURIComponent(propValue);
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
            }

            if (propName == "AdditionalFilters" && propValue.length > 0)
                addtionalFiltersValues = JSON.stringify(propValue);


        }
        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        }



        var callUrl = url.concat(urlparameters);

        return this.httpClient.get(callUrl,  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;

                var result = new AutoSelectedExternalReconciliationLines();
                result = serviceResponse.Result.Result;


                result.Count = serviceResponse.Result.Count;

                console.log("[Result]", result);

                serviceResponse.Result = result;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }

    getGenerateTestRecordsForExternalReco(bankAccountId: string, glAccountId: string, type: string) {


        var url = this._apiUrl + "/GetGenerateTestRecordsForExternalReco";

        var urlparameters = '?glAccountId=' + glAccountId + '&bankAccountId=' + bankAccountId + '&type=' + type;

        var callUrl = url.concat(urlparameters);

        return this.httpClient.get(callUrl,  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();

                serviceResponse.Result = response;

                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));


    }


    MapJsonToEntityLedgerTransactionList(jsonList: any) {

        var entityList: LedgerTransactionList;
        entityList = new LedgerTransactionList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

}

export class AutoSelectedExternalReconciliationLines {
    transactionLines;
    pageLines;
    Count;
}

export class ExternalAutoReconcileServiceArgs{
    amountReconcile: boolean;
    referenceReconcile: boolean;
    refDateReconcile: boolean;
    accoutingDateReconcile: boolean;
    objectTableId: string;
    entityId: string;
    glAccountId: string;
    filters: ApiQueryFilters;
}
