import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {LedgerTransactionList} from '../../EntityLists/LedgerTransactionList';
//import {ReconcileExternalPageLineList} from '../../EntityLists/ReconcileExternalPageLineList';

@Injectable()

export class ExternalReconciliationExtendedListService {
    private _http: Http
    private _apiUrl: string;

    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ExternalReconciliation';
    }

    getExternalAutomaticReconcilationsByFilter(args: ExternalAutoReconcileServiceArgs)
    {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + "/GetExternalAutomaticReconcilationsByFilter";

        var urlparameters =
            '?amountReconcile=' + args.amountReconcile
            + '&referenceReconcile=' + args.referenceReconcile
            + '&refDateReconcile=' + args.refDateReconcile
            + '&objectTableId=' + args.objectTableId
            + '&entityId=' + args.entityId
            + '&glAccountId=' + args.glAccountId
            + '&filters=' + args.filters;


        //#region Parse Filters into URI
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
        //#endregion End Parse


        var callUrl = url.concat(urlparameters);

        return Observable.defer(() => {
            return this._http.get(callUrl, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();

                var result = new AutoSelectedExternalReconciliationLines();
                result = serviceResponse.Result.Result;

                //
                //var mappedTransactions: Array<LedgerTransactionList> = [];
                //if (serviceResponse.Result) {
                //    for (var key in serviceResponse.Result.transactionLines) {
                //        var list: LedgerTransactionList;
                //        list = this.MapJsonToEntityLedgerTransactionList(serviceResponse.Result[key]);
                //        mappedTransactions.push(list);
                //    }
                //}
                //result.transactionLines = mappedTransactions;

                //
                //var mappedPageLines: Array<ReconcileExternalPageLineList> = [];
                //if (serviceResponse.Result) {
                //    for (var key in serviceResponse.Result.pageLines) {
                //        var list: ReconcileExternalPageLineList;
                //        list = this.MapJsonToEntityReconcileExternalBankPageLineList(serviceResponse.Result[key]);
                //        mappedPageLines.push(list);
                //    }
                //}
                //result.pageLines = mappedPageLines;

                //
                result.Count = serviceResponse.Result.Count;

                console.log("[Result]", result);

                serviceResponse.Result = result;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    getGenerateTestRecordsForExternalReco(bankAccountId: string, glAccountId: string, type: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + "/GetGenerateTestRecordsForExternalReco";

        var urlparameters = '?glAccountId=' + glAccountId + '&bankAccountId=' + bankAccountId + '&type=' + type;

        var callUrl = url.concat(urlparameters);

        return Observable.defer(() => {
            return this._http.get(callUrl, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();

                serviceResponse.Result = response.json();

                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
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
    //MapJsonToEntityReconcileExternalBankPageLineList(jsonList: any) {

    //    var entityList: ReconcileExternalPageLineList;
    //    entityList = new ReconcileExternalPageLineList();
    //    var jsonListKeys = Object.keys(jsonList);

    //    for (var key in jsonListKeys) {
    //        var property = jsonListKeys[key];
    //        entityList[property] = jsonList[property];
    //    }


    //    return entityList;
    //}
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
    objectTableId: string;
    entityId: string;
    glAccountId: string;
    filters: ApiQueryFilters;
}
