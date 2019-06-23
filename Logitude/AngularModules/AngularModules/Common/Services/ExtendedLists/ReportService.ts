import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable} from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse'; 
import {ReportFliter} from '../../../Report/Components/Filters/ReportFliter';

@Injectable()
export class ReportService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/Report';
    }

    GetReportListsByGroupId(groupId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + '?groupId=' + groupId + '&tenant=' + tenant, { headers: authHeader }).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }
    
    GetPrepareSendReport(type: string, fileName: string,  tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + "/GetPrepareSendReport" + '?type=' + type + '&fileName=' + fileName  +  '&tenant=' + tenant, { headers: authHeader }).map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    GetCheckIfStimulSoftReportIsBliud(reportKey: string,  tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + "/GetCheckIfStimulSoftReportIsBliud" + '?reportKey=' + reportKey + '&tenant=' + tenant , { headers: authHeader }).map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    GetCheckIfReportsRunUsingWR() {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + "/GetCheckIfReportsRunUsingWR", { headers: authHeader }).map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }
    
    GenerateReportMethod(filter: ReportFliter) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.put(this._apiUrl, JSON.stringify(filter), {
                headers: authHeader,

            }).map(response => {
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = response.json();
                return pmresponse;
            }).catch(ServiceHelper.HandleServiceError);
        }

        );

    }

    GenerateReportForCustomerPotentialActual(filter: ReportFliter) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.put(this._apiUrl, JSON.stringify(filter), { headers: authHeader }).map(response => {

                var myJsonResult = response.json();
                var myResult = new CustomersDataProvider();

                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        myResult[property] = myJsonResult[property];
                    }
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        }
        );
    }
}

export class CustomersDataProvider {
    public Customers: CustomersData[] = [];
}

export class CustomersData {
    public CustomerId: string;
    public CustomerName: string;
    public PrimaryContactName: string;
    public PrimaryContactEmail: string;
    public Salesman: string;
    public LocationsCount: number;
    public AD_POT: number;
    public AR_POT: number;
    public AE_POT: number;
    public AI_POT: number;
    public ID_POT: number;
    public IR_POT: number;
    public IE_POT: number;
    public II_POT: number;
    public OD_POT: number;
    public OR_POT: number;
    public OE_POT: number;
    public OI_POT: number;
    public CI_POT: number;
    public DL_POT: number;
    public IN_POT: number;
    public AD_ACT: number;
    public AR_ACT: number;
    public AE_ACT: number;
    public AI_ACT: number;
    public ID_ACT: number;
    public IR_ACT: number;
    public IE_ACT: number;
    public II_ACT: number;
    public OD_ACT: number;
    public OR_ACT: number;
    public OE_ACT: number;
    public OI_ACT: number;
    public CI_ACT: number;
    public DL_ACT: number;
    public IN_ACT: number;
}