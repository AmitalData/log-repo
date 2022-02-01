import { Injectable } from "@angular/core";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { ObjectTablePM } from "Infrastructure/EntityPMs/ObjectTablePM";
import { EntityListService } from "Infrastructure/Services/EntityListService";
import { EntityResourceService } from "Infrastructure/Services/EntityResourceService";
import { ServiceHelper } from "Infrastructure/Utilities/ServiceHelper";
import { defer, Observable } from "rxjs";
import { catchError, map, take } from "rxjs/operators";
import { filterIsNotNull } from "../../Services/new-quote-data/new-quote-data.service";
declare const window: any;

@Injectable()
export class LogtuideTableDataService {
  private _entityResourceService: EntityResourceService = new EntityResourceService();

  constructor(
    private entityListService: EntityListService,
  ) { }

  getTable(tableName: string): Promise<any> {
    return new Promise<ServiceResponse>((resolve, reject) => {
      this._entityResourceService.getEntityResourceByTableName(tableName, 0)
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


  getDataFromService(ob: Observable<any>): Promise<any[]> {
    return new Promise<any[]>((resolve, reject) =>
      ob.pipe(filterIsNotNull(), take(1))
        .subscribe((res: ServiceResponse) =>
          resolve(res.Result)
        ));
  }


  standartSendAjax(ajax: Observable<any>) {
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
