import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { LazyLoadEvent, SortEvent } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Table } from 'primeng/table';
import { fromEvent } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
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
    private elem: ElementRef,
    private config: DynamicDialogConfig,
    private tableService: GenericTableService,
    private dialogRef: DynamicDialogRef,
  ) { }

  ngOnInit(): void {
    this.insertData(this.config.data);
    this.lazy = !!this.config.data.getData
    this.creasteSearchText();
  }

  ngAfterViewInit() {
    this.alignRow();
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
    this.addDataFromFunc(e.rows + e.first - this.config.data.rowTake);
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

  async addDataFromFunc(rowIndex: number) {
    if (this.allRowGet) return;

    const rowTake: number = this.config.data.rowTake;
    const index: number = rowIndex + rowTake * 2;
    const loadedData: any[] = await this.config.data.getData(this.filterVal, index / rowTake, this.sortField, this.sortOrder);
    this.allRowGet = loadedData.length !== rowTake;

    Array.prototype.splice.apply(this.data, [index, rowTake, ...loadedData]);
    if (this.data.length <= index + rowTake && !this.allRowGet)
      Array.prototype.splice.apply(this.data, [index + rowTake, 0, ...Array.from({ length: rowTake })]);
    this.data = [...this.data]

    if (this.allRowGet) {
      const rowsHave: number = index + loadedData.length;
      Array.prototype.splice.apply(this.data, [rowsHave, this.data.length - rowsHave]);
    }

    if (rowIndex === 0) {
      this.allRowGet = false;
      this.addDataFromFunc(rowTake * -2);
      this.addDataFromFunc(rowTake * -1);
    }

    if (index - rowTake > -1 && !this.data[index - rowTake]) {
      this.allRowGet = false;
      this.addDataFromFunc(index - rowTake * 3);
    }

    if (index - rowTake * 2 > -1 && !this.data[index - rowTake * 2]) {
      this.allRowGet = false;
      this.addDataFromFunc(index - rowTake * 4);
    }
  }

  onSelectedRow(e: any) {
    this.dialogRef.close(e)
  }

  private async alignRow() {
    let scrollBarElement = this.getScrollBarElement();
    while (!scrollBarElement) {
      await new Promise(r => setTimeout(r, 100));
      scrollBarElement = this.getScrollBarElement();
    }

    fromEvent(scrollBarElement, 'scroll')
      .pipe(debounceTime(100))
      .subscribe((e: any) => {
        const div = e.target as HTMLDivElement;
        const diff: number = div.scrollTop % 41;
        if (diff > 2 && diff < 39)
          div.scrollBy(0, 41 - diff + 1);
      });
  }

  private getScrollBarElement() {
    return this.elem.nativeElement.querySelectorAll('.p-datatable-scrollable-body')[0];
  }
}


export type GenericTableColumn = { name: string, alias: string }
export type GenericTableDataTable = {
  data: {}[];
  columns: string[] | GenericTableColumn[];
}
