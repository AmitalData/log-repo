import { Injectable } from '@angular/core';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { GenericTableDataService } from './generic-table-data.service';
import { GenericTableColumn, GenericTableComponent } from './generic-table.component';

@Injectable()
export class GenericTableService {

  constructor(
    private dialogService: DialogService,
    private dataService: GenericTableDataService,
  ) { }

  open(data: any[], header: string, columns: string[] | GenericTableColumn[] = null as any, filter: (val: any, pageIndex: number, rowsTake: number) => any[] = null as any): DynamicDialogRef {
    if (!data) throw ("Generic Table cannot be get null or undefined data");

    const config: DynamicDialogConfig = {}
    config.width = '70vw';
    config.header = header;
    config.data = { data: data, columns: columns, filter: filter }

    return this.dialogService.open(GenericTableComponent, config);
  }

  async openByTableName(tableName: string, header: string = tableName, columns: string[] | GenericTableColumn[] = null as any, serverSidePagention: boolean = false) {
    const data: any = serverSidePagention ? await this.dataService.getTableWithFilter(tableName, '', 0, 100, []).then() : await this.dataService.getTable(tableName).then();

    const [columnsClean, columnsNames]: [GenericTableColumn[], string[]] = this.columnsCleansing(columns, data);

    const filter: any = serverSidePagention ?
      (val: string, pageIndex: number, rowsTake: number) =>
        this.dataService.getTableWithFilter(tableName, val, pageIndex, rowsTake, columnsNames) :
      null;

    this.open(data, header, columns, filter)
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
