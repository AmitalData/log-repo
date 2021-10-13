import { Component, OnInit, ViewChild } from '@angular/core';
import { LazyLoadEvent, SortEvent } from 'primeng/api';
import { DynamicDialogConfig } from 'primeng/dynamicdialog';
import { Table } from 'primeng/table';
import { GenericTableService } from './generic-table.service';

@Component({
  selector: 'app-generic-table',
  templateUrl: './generic-table.component.html',
  styleUrls: ['./generic-table.component.scss']
})
export class GenericTableComponent implements OnInit {
  @ViewChild('table') table: Table = null as any;

  data: any[] = []//Array.from({ length: 1000 }) //[];
  columns: GenericTableColumn[] = [];
  columnsNames: string[] = [];
  rows: number = 100;
  lazy: boolean = false;
  first = 0;
  sortField: string = ''
  sortOrder: number = 1;
  filterVal: string = '';

  constructor(
    private config: DynamicDialogConfig,
    private tableService: GenericTableService,
  ) { }

  ngOnInit(): void {
    this.insertData(this.config.data);
    this.lazy = !!this.config.data.getData
  }

  insertData(dataTable: GenericTableDataTable) {
    this.data = dataTable.data;
    let columns: any[] = dataTable.columns as any[];

    [this.columns, this.columnsNames] = this.tableService.columnsCleansing(columns, this.data);
  }

  async loadLazy(e: LazyLoadEvent) {
    // console.log('loadLazy',e )
    this.addDataFromFunc(e.first as number);
  }

  filterTable(val: string) {
    this.filterVal = val

    if (this.config.data.getData) {
      this.table.clearCache()
      this.data = Array.from({ length: 100 });;
      this.addDataFromFunc(0);
    } else
      this.table.filterGlobal(val, 'contains')
  }

  async onSort(e: SortEvent) {
    if (!this.lazy) return;

    this.sortField = e.field as string;
    this.sortOrder = e.order as number
    this.table.clearCache()
    this.data = Array.from({ length: 100 });
    this.addDataFromFunc(0);
  }

  async addDataFromFunc(index: number) {
    let loadedData: any[] = await this.config.data.getData(this.filterVal, index, this.sortField, this.sortOrder);
    Array.prototype.splice.apply(this.data, [index, 100, ...loadedData]);
    Array.prototype.splice.apply(this.data, [index + 100, 0, ...Array.from({ length: 100 })]);
    this.data = [...this.data]
  }
}


export type GenericTableColumn = { name: string, alias: string }
export type GenericTableDataTable = {
  data: {}[];
  columns: string[] | GenericTableColumn[];
}
