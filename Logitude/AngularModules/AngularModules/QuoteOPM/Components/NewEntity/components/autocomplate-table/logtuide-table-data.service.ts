import { HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { reject } from "cypress/types/lodash";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { ObjectTablePM } from "Infrastructure/EntityPMs/ObjectTablePM";
import { EntityListService } from "Infrastructure/Services/EntityListService";
import { EntityResourceService } from "Infrastructure/Services/EntityResourceService";
import { Observable } from "rxjs";
import { take } from "rxjs/operators";
import { filterIsNotNull } from "../../Services/new-quote-data/new-quote-data.service";
declare const window: any;

@Injectable()
export class LogtuideTableDataService {

  constructor(
    private entityListService: EntityListService,
    private entityResourceService: EntityResourceService
  ) { }

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
}
