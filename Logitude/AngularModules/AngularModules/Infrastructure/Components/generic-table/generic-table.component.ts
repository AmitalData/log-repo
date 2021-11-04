import { Component, OnInit, ViewChild } from '@angular/core';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { LazyLoadEvent, SortEvent } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Table } from 'primeng/table';
import { GenericTableService } from './generic-table.service';

@Component({
  selector: 'app-generic-table',
  templateUrl: './generic-table.component.html',
  styleUrls: ['./generic-table.component.scss']
})
export class GenericTableComponent implements OnInit {
  @ViewChild('table') table: Table = null as any;

  placeholder: string = '';
  data: any[] = []//Array.from({ length: 1000 }) //[];
  columns: GenericTableColumn[] = [];
  columnsNames: string[] = [];
  rows: number = 100;
  lazy: boolean = false;
  sortField: string = ''
  sortOrder: number = 1;
  filterVal: string = '';
  allRowGet: boolean = false;

  constructor(
    private config: DynamicDialogConfig,
    private tableService: GenericTableService,
    private dialogRef: DynamicDialogRef,
  ) { }

  ngOnInit(): void {
    this.insertData(this.config.data);
    this.lazy = !!this.config.data.getData
    this.creasteSearchText();
  }

  creasteSearchText() {
    this.placeholder = TextCodeTranslator.Translate(this.config.data.tableName + ".F.SearchFields") || TextCodeTranslator.Translate("General.O.Search");
  }

  insertData(dataTable: GenericTableDataTable) {
    this.data = dataTable.data;
    let columns: any[] = dataTable.columns as any[];

    [this.columns, this.columnsNames] = this.tableService.columnsCleansing(columns, this.data);
  }

  async loadLazy(e: LazyLoadEvent) {
    this.addDataFromFunc(e.rows - this.config.data.rowTake);
  }

  filterTable(val: string) {
    this.filterVal = val

    if (this.config.data.getData) {
      this.data = Array.from({ length: this.config.data.rowTake });
      this.allRowGet = false;
      this.addDataFromFunc(0);
    } else
      this.table.filterGlobal(val, 'contains')
  }

  async onSort(e: SortEvent) {
    if (!this.lazy) return;

    this.allRowGet = false;
    this.sortField = e.field as string;
    this.sortOrder = e.order as number
    this.data = Array.from({ length: this.config.data.rowTake });
    this.addDataFromFunc(0);
  }

  async addDataFromFunc(index: number) {
    if (this.allRowGet) return;

    const loadedData: any[] = await this.config.data.getData(this.filterVal, index / this.config.data.rowTake, this.sortField, this.sortOrder);

    Array.prototype.splice.apply(this.data, [index, this.config.data.rowTake, ...loadedData]);
    Array.prototype.splice.apply(this.data, [index + this.config.data.rowTake, 0, ...Array.from({ length: this.config.data.rowTake })]);
    this.data = [...this.data]

    this.allRowGet = loadedData.length !== this.config.data.rowTake;
    if (this.allRowGet) {
      const rowsHave: number = index + loadedData.length;
      Array.prototype.splice.apply(this.data, [rowsHave, this.data.length - rowsHave]);
    }
  }

  onSelectedRow(e: any) {
    this.dialogRef.close(e)
  }
}


export type GenericTableColumn = { name: string, alias: string }
export type GenericTableDataTable = {
  data: {}[];
  columns: string[] | GenericTableColumn[];
}
