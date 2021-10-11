import { Component, OnInit, ViewChild } from '@angular/core';
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

  data: any[] = [];
  columns: GenericTableColumn[] = [];
  columnsNames: string[] = [];
  paginator: boolean = true
  totalRecords: number = 0;
  firstRowIndex: number = 0;
  pageNumber: number = 0;
  rows: number = 10;
  filterVal: string = '';

  constructor(
    // private ref: DynamicDialogRef, 
    private config: DynamicDialogConfig,
    private tableService: GenericTableService,
  ) { }

  ngOnInit(): void {
    this.insertData(this.config.data);
  }

  insertData(dataTable: GenericTableDataTable) {
    this.data = dataTable.data;
    let columns: any[] = dataTable.columns as any[];

    [this.columns, this.columnsNames] = this.tableService.columnsCleansing(columns, this.data);
    this.totalRecords = this.data.length;
  }


  filterTable(val: string = null as any) {
    if (val !== null)
      this.filterVal = val;

    if (this.config.data.filter)
      this.data = this.config.data.filter(this.filterVal, this.pageNumber, this.rows);
    else
      this.table.filterGlobal(this.filterVal, 'contains')
  }


  onPageChange(e: { page: number, first: number, rows: number, pageCount: number }) {
    this.rows = e.rows
    this.firstRowIndex = e.first
    this.pageNumber = e.page;

    if (this.config.data.filter)
      this.filterTable();
  }
}


export type GenericTableColumn = { name: string, alias: string }
export type GenericTableDataTable = {
  data: {}[];
  columns: string[] | GenericTableColumn[];
}