                                               import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {InfraGenericFilter} from '../../../Infrastructure/Utilities/InfraGenericFilter';
import {CachedDataManager} from '../../../Infrastructure/Utilities/CachedDataManager';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

export class DeclarationVehicleModificationListService{

    private _http: Http;
    private _apiUrl: string;
    
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationVehicleModification';
    }



    GetDeclarationVehicleModification(
        declarationId: string,
        chassisNumber: string,
        adjustmentTypeCode: string,
        tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetDeclarationVehicleModification';

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetDeclarationVehicleModification/?'
                + '&declarationId=' + declarationId
                + '&chassisNumber=' + chassisNumber
                + '&adjustmentTypeCode=' + adjustmentTypeCode
                + '&tenant=' + tenant.toString()
                
                , { headers: authHeader }).map(response => {


                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                var _mappedListsArray: Array<DeclarationVehicleModificationList> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: DeclarationVehicleModificationList;
                        entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }  

 
    MapJsonToEntityList(jsonList: any) {

        var entityList: DeclarationVehicleModificationList;
        entityList = new DeclarationVehicleModificationList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

  

  
}

export class DeclarationVehicleModificationList{

    public Id: string;
    public ChassisNumber: string;
    public VehicleNumber: string;
    public AdjustmentType: string;
    public AdjustmentTypeName: string;
    
    public DeductAmount: number;
    
}