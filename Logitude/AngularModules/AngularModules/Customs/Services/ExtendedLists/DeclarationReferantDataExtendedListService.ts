import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {InfraGenericFilter} from '../../../Infrastructure/Utilities/InfraGenericFilter';
import {CachedDataManager} from '../../../Infrastructure/Utilities/CachedDataManager';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {CustomsSettingList} from '../../EntityLists/CustomsSettingList';
import { GenericRequestParams } from '../../DataContract/RequestParams/GenericRequestParams';
import { ChartingDataClass } from '../../../Infrastructure/DataContracts/Dashboard/ChartingDataClass';


export class DeclarationReferantDataExtendedListService {
   

    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationReferantDataExtended';
    }

 
}
