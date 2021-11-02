import { Injectable } from '@angular/core';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { GenericTableDataService } from './generic-table-data.service';
import { GenericTableColumn, GenericTableComponent } from './generic-table.component';
/* 
example: 
        this.genericTableService.openByTableName('QuoteOP','QuoteOP',['CreatedByUser','CreatedByUserId','CustomerId','CustomerName'])
        this.genericTableService.openByService(new DocumentsFilingListService(),'', ['Name','DocumentId','ObjectTableId','OrigionalDocumentId','OwnerId','OwnerName','Received'], true)
        this.genericTableService.openByTableName('DocumentsFiling','', ['Name','DocumentId','ObjectTableId','OrigionalDocumentId','OwnerId','OwnerName','Received'], true)
        const promise  = async (filterVal: string, pageIndex: number, rowsTake: number, columnsFilter: '', sortField: string, sortOrder: number) =>{
            const res: ServiceResponse = await this.newQuoteOPWebService.GetPortsItemsList("I", 'O', filterVal, 100, false).toPromise();
            return res.Result;
        }
        this.genericTableService.openByPromise(promise,'', ['Name','Notes','ObjectTableId','OrigionalDocumentId','OwnerId','OwnerName','Received'], false)
 */

@Injectable()
export class GenericTableService {
  rowsTake: number = 100;

  constructor(
    private dialogService: DialogService,
    private dataService: GenericTableDataService,
  ) { }

  open(data: any[], header: string = '', columns: string[] | GenericTableColumn[] = null as any, getDataFunction: () => any = null as any, rowTake: number = 100): DynamicDialogRef {
    if (!data) throw ("Generic Table cannot be get null or undefined data");

    const config: DynamicDialogConfig = {}
    config.width = '70vw';
    config.header = header;
    config.data = { data: data, columns: columns, getData: getDataFunction, rowTake: rowTake }

    return this.dialogService.open(GenericTableComponent, config);
  }

  async openByApiOpenQuoeryFilter(f: (qf: ApiQueryFilters) => Promise<any[]>, header: string = '', columnsFilter: any[] = null as any, columns: string[] | GenericTableColumn[] = null as any): Promise<DynamicDialogRef> {
    const filters: ApiQueryFilters = new ApiQueryFilters();
    filters.PageIndex = 0;
    filters.PageSize = this.rowsTake;
    const data: any[] = await f(filters);

    // const [columnsClean, columnsNames]: [GenericTableColumn[], string[]] = this.columnsCleansing(columns, data);

    const getDataFunction: any = 
      (val: string, pageIndex: number, sortField: string, sortOrder: number) => {
        const filters: ApiQueryFilters = new ApiQueryFilters();
        filters.PageIndex = pageIndex;
        filters.PageSize = this.rowsTake;

        filters.SortBy = sortField;
        filters.SortDirection = sortOrder === 1 ? 'Descending' : 'Ascending';

        if (val)
          columnsFilter.forEach(col =>
            filters.addAdditionalFilter(col, val, null, null, "Contains", false, false, false, "Text", false, false));

        return f(filters);
      };

    return this.open(data, header, columns, getDataFunction)
  }

  async openByPromise(f: FunctionGetDataPromise, header: string = '', columns: string[] | GenericTableColumn[] = null as any, virtualScroll: boolean = false, columnFilter: string = null): Promise<DynamicDialogRef> {
    const data: any[] = await f('', 0, 100, null, null, null);

    const [columnsClean, columnsNames]: [GenericTableColumn[], string[]] = this.columnsCleansing(columns, data);

    const getDataFunction: any = virtualScroll ?
      (val: string, pageIndex: number, sortField: string, sortOrder: number) =>
        f(val, pageIndex, this.rowsTake, columnFilter, sortField, sortOrder) :
      null;

    return this.open(data, header, columns, getDataFunction)
  }

  async openByService(service: any, header: string = '', columns: string[] | GenericTableColumn[] = null as any, virtualScroll: boolean = false, columnFilter: string = null): Promise<DynamicDialogRef> {
    const data: any[] = virtualScroll ? await this.dataService.getDataFromFuncWithFilter(service, '', 0, 100, null) : await this.dataService.getDataFromFunc(service);

    const [columnsClean, columnsNames]: [GenericTableColumn[], string[]] = this.columnsCleansing(columns, data);

    const getDataFunction: any = virtualScroll ?
      (val: string, pageIndex: number, sortField: string, sortOrder: number) =>
        this.dataService.getDataFromFuncWithFilter(service, val, pageIndex, this.rowsTake, columnFilter, sortField, sortOrder) :
      null;

    return this.open(data, header, columns, getDataFunction)
  }

  async openByTableName(tableName: string, header: string = tableName, columns: string[] | GenericTableColumn[] = null as any, virtualScroll: boolean = false, columnFilter: string = 'SearchFields'): Promise<DynamicDialogRef> {
    const data: any[] = virtualScroll ? await this.dataService.getTableWithFilter(tableName, '', 0, 100, null) : await this.dataService.getTable(tableName);
    // console.log(data)
    const [columnsClean, columnsNames]: [GenericTableColumn[], string[]] = this.columnsCleansing(columns, data);

    const getDataFunction: any = virtualScroll ?
      (val: string, pageIndex: number, sortField: string, sortOrder: number) =>
        this.dataService.getTableWithFilter(tableName, val, pageIndex, this.rowsTake, columnFilter, sortField, sortOrder) :
      null;

    return this.open(data, header, columns, getDataFunction)
  }

  columnsCleansing(columns: string[] | GenericTableColumn[] | null, data: any[]): [GenericTableColumn[], string[]] {
    let columnsRes: any[];
    if (!columns)
      columnsRes = Object.keys(data[0]).map(col => { return { name: col, alias: col } })
    else if (typeof columns[0] === 'string')
      columnsRes = (<any[]>columns).map(col => { return { name: col, alias: col } })
    else
      columnsRes = columns;

    const columnsNames: string[] = columnsRes.map(col => col.name);
    return [columnsRes as GenericTableColumn[], columnsNames]
  }

  pickPropFromObject(object: any, props: string[]) {
    return props.filter(key => key in object).reduce(function (o: any, k: string) { o[k] = object[k]; return o; }, {});
  }

}

export type FunctionGetDataPromise = (fiterVal: string, pageIndex: number, rowsTake: number, columnsFilter: string, sortField: string, sortOrder: number) => Promise<any[]>