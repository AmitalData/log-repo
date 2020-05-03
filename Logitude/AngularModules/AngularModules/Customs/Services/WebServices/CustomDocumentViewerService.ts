import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {DeclarationList} from '../../EntityLists/DeclarationList';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class CustomDocumentViewerService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomDocumentViewer';

    }

    GetDocumentPage(documentId: string, currPage: number, isConnectedToUni: boolean,angle: number=0) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetDocumentPage/?documentId=" + documentId + "&currPage=" + currPage + "&isConnectedToUni=" + isConnectedToUni + '&angle=' +angle, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var json = response;

                var mappedObject: CustomDocumentPageObject = this.MapJsonToCustomDocumentPageObject(json);

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = mappedObject;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));

        }

        );
    }


    MapJsonToCustomDocumentPageObject(jsonPM: any) {

        var entity = new CustomDocumentPageObject();
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entity[property] = jsonPM[property];
        }

        return entity;
    }

}

export class CustomDocumentPageObject {
    Page: any[]; //byte[]
    TiffPageLines :string;
    ErrorMessage: string;
    Count: number;
}
