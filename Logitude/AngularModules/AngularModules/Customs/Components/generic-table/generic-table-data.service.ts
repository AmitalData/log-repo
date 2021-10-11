import { Injectable } from '@angular/core';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { EntityListService } from 'Infrastructure/Services/EntityListService';
import { filterIsNotNull } from 'QuoteOPM/Components/NewEntity/Services/new-quote-data/new-quote-data.service';
import { take } from 'rxjs/operators';

@Injectable()
export class GenericTableDataService {

  constructor(
    private entityListService: EntityListService,
  ) { }

  async getTable(tableName: string): Promise<any[]> {
    const filters = new ApiQueryFilters();
    filters.GetAll = true;

    return await this._getTable(tableName, filters)
  }

  async getTableWithFilter(tableName: string, val: string, pageIndex: number, rowsTake: number, columnsFilter: string[]): Promise<any[]> {
    const filters = new ApiQueryFilters();
    columnsFilter.forEach(col => filters.addAdditionalFilter(col, val, null, null, "Contains", true, false, false, "Text", false, false));
    
    filters.SortDirection = "Ascending";
    filters.PageIndex = pageIndex;
    filters.PageSize = rowsTake;
    
    return await this._getTable(tableName, filters)
  }

  private async _getTable(tableName: string, filters: ApiQueryFilters) {
    return await new Promise<any[]>(async (resolve, reject) => {
      const resService: any = await this.entityListService.getByFilters(tableName, filters).then();
    
      resService.pipe(filterIsNotNull(), take(1))
        .subscribe((resp: any) => resolve(resp.Result));
    });
  }
}
