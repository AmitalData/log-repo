import { Injectable } from '@angular/core';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { EntityListService } from 'Infrastructure/Services/EntityListService';
import { DirectionListService } from 'Infrastructure/Services/StandardLists/DirectionListService';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';

@Injectable()
export class GenericTableDataService {

  constructor(
    private entityListService: EntityListService,
  ) { }

  async getTable(tableName: string): Promise<any[]> {
    const filters: ApiQueryFilters = new ApiQueryFilters();
    filters.GetAll = true;

    return await this.getTableFromLogitude(tableName, filters)
  }

  async getTableWithFilter(tableName: string, fiterVal: string, pageIndex: number, rowsTake: number, columnFilter: string, sortField: string = '', sortOrder: number = 0): Promise<any[]> {
    const filters: ApiQueryFilters = this.createFilter(columnFilter, fiterVal, sortOrder, pageIndex, rowsTake, sortField);

    return await this.getTableFromLogitude(tableName, filters)
  }

  // async getDataFromFunc(f: (ApiQueryFilters: ApiQueryFilters) => Observable<any>,): Promise<any[]> {
  async getDataFromFunc(service: any): Promise<any[]> {
    const filters: ApiQueryFilters = new ApiQueryFilters();
    filters.GetAll = true;

    return this.getDataFromService(service, filters)
  }

  async getDataFromFuncWithFilter(service: any, fiterVal: string, pageIndex: number, rowsTake: number, columnFilter: string, sortField: string = '', sortOrder: number = 0): Promise<any[]> {
    const filters: ApiQueryFilters = this.createFilter(columnFilter, fiterVal, sortOrder, pageIndex, rowsTake, sortField);

    return this.getDataFromService(service, filters)
  }

  getDataFromService(service: any, filters: ApiQueryFilters) {
    const ob: Observable<any> = service.getAllFromCache ? service.getAllFromCache(filters) : service.getByFilters(filters)
    return this.getDataFromObservable(ob)
  }


  private createFilter(columnFilter: string, fiterVal: string, sortOrder: number, pageIndex: number, rowsTake: number, sortField: string) {
    const filters: ApiQueryFilters = new ApiQueryFilters();

    if (fiterVal)
      filters.addAdditionalFilter(columnFilter, fiterVal, null, null, "Contains", true, false, false, "Text", false, false);

    filters.SortDirection = sortOrder === 1 ? 'Descending' : 'Ascending';
    filters.PageIndex = pageIndex;
    filters.PageSize = rowsTake;

    if (sortField)
      filters.SortBy = sortField;

    return filters;
  }

  private async getTableFromLogitude(tableName: string, filters: ApiQueryFilters) {
    const resService: any = await this.entityListService.getByFilters(tableName, filters).then();
    return this.getDataFromObservable(resService);
  }

  async getDataFromObservable(ob: Observable<any>): Promise<any[]> {
    return new Promise<any[]>((resolve, reject) =>
      ob.pipe(filterIsNotNull(), take(1))
        .subscribe((res: ServiceResponse) =>
          resolve(res.Result)
        ));
  }
}
