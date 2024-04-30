import { HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { reject } from "cypress/types/lodash";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { ObjectTablePM } from "Infrastructure/EntityPMs/ObjectTablePM";
import { EntityListService } from "Infrastructure/Services/EntityListService";
import { EntityResourceService } from "Infrastructure/Services/EntityResourceService";
import { ServiceHelper } from "Infrastructure/Utilities/ServiceHelper";
import { defer, Observable } from "rxjs";
import { catchError, map, take , filter } from "rxjs/operators";
declare const window: any;

@Injectable()
export class LogtuideTableDataService {
  

  constructor(
    private entityListService: EntityListService,
    private entityResourceService: EntityResourceService
  ) { }


  static createInstance() {
    return new LogtuideTableDataService(new EntityListService(), new EntityResourceService())
  }

 
  getTable(tableName: string): Promise<any> {
    return new Promise<ServiceResponse>((resolve, reject) => {
      this.entityResourceService.getEntityResourceByTableName(tableName, 0)
        .pipe(filterIsNotNull(), take(1))
        .subscribe(async () => {
          const LookUpTable: ObjectTablePM = window.ObjectTables.filter(d => d.Name === tableName)[0];
          const loadPr: any = (LookUpTable?.CacheOnClient) ?
            await this.entityListService.getAllFromCache(tableName, new ApiQueryFilters()) :
            await this.entityListService.getAll(tableName);

          const response: ServiceResponse = await (<Observable<Promise<ServiceResponse>>>loadPr).toPromise();
          resolve(response.Result);
        });
    })
  }


  getDataFromService(ob: Observable<any>): Promise<any> {
    return new Promise<any[]>((resolve, reject) =>
      ob.pipe(filterIsNotNull(), take(1))
        .subscribe(
          (res: ServiceResponse) => resolve(res.Result instanceof HttpResponse ? res.Result.body : res.Result),
          reject
        )
    )
  }


  standartSendAjax(ajax: Observable<any>): Observable<ServiceResponse> {
    return LogtuideTableDataService.standartSendAjax(ajax);
  }

  static standartSendAjax(ajax: Observable<any>): Observable<ServiceResponse> {
    return defer(() => {
      return ajax.pipe(map(response => {
        const serviceResponse: ServiceResponse = new ServiceResponse();
        serviceResponse.Result = response;
        return serviceResponse;
      }), catchError(ServiceHelper.HandleServiceError));
    });
  }

  
  sendAjaxAndGetDataStandart(ajax: Observable<any>) {
    return this.getDataFromService(this.standartSendAjax(ajax));
  }


  apiQueryFilterToQueryString(filters: ApiQueryFilters): string {
    let urlparameters: string = '';
    var mykeys = Object.keys(filters);
    var addtionalFiltersValues = null;

    for (var i in mykeys) {
      var propName = mykeys[i];
      var propValue = filters[propName];
      var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");

      if (urlparameters != "?") {
        urlparameters = urlparameters.concat('&');
      }

      if (!ignoreFilter) {
        propValue = encodeURIComponent(propValue);
        urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
      }

      if (propName == "AdditionalFilters" && propValue.length > 0) {
        addtionalFiltersValues = JSON.stringify(propValue);
      }
    }

    if (addtionalFiltersValues) {
      urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
    }

    return urlparameters;
  }
}

export function filterIsNotNull() {
  return filter((x: any) => x);
}
