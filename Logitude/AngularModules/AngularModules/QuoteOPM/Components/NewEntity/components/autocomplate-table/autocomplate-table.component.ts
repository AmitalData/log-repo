import { DOCUMENT } from '@angular/common';
import { ChangeDetectorRef, Component, EventEmitter, HostListener, Inject, Input, Output, SimpleChanges, ViewChild } from '@angular/core';
import { AbstractControl, FormGroup } from '@angular/forms';
import { GenericTableColumn } from 'Infrastructure/Components/generic-table/generic-table.component';
import { GenericTableService } from 'Infrastructure/Components/generic-table/generic-table.service';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { AutoComplete } from 'primeng/autocomplete';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { fromEvent } from 'rxjs';
import { debounceTime, take } from 'rxjs/operators';
// import { Subscription } from 'rxjs';

@Component({
  selector: 'app-autocomplate-table',
  templateUrl: './autocomplate-table.component.html',
  styleUrls: ['./autocomplate-table.component.scss']
})
export class AutocomplateTableComponent {
  @ViewChild('autoComplete') autoComplete: AutoComplete = null as any;

  @Input() formGroup: FormGroup = null as any;
  @Input() label: any = '';
  @Input() fieldShow: string = '';
  @Input() placeholder: string = '';
  @Input() disabled: boolean = false;
  @Input() controlName: string = '';
  @Input() data: any[] = []
  @Input() getDataFunc: (filter: ApiQueryFilters) => Promise<any[]> = null as any;
  @Input() logTableName: string = '';
  @Input() columnsFilter: string[] = []
  @Input() searchIcon: boolean = false;
  @Input() dropIcon: boolean = false;
  @Input() virtualScroll: boolean = false;
  @Input() itemSize: number = 26;
  @Input() set columnsShow(columns: any) {
    this.initColumns(columns);
  }
  @Output() onSelect = new EventEmitter()

  selected: any[] = []
  columnsNames: string[] = [];
  columnsHeader: any = [];
  selectedChoice: any;
  rowTake = 200;
  index: number = 0;
  isGetAll: boolean = false;
  filterVal: string = ''

  constructor(
    @Inject(DOCUMENT) private document: any,
    private genericTableService: GenericTableService,
  ) { }

  ngOnInit() {
    this.InitColumns();
  }

  private InitColumns() {
    if (this.data.length && !this.columnsNames.length)
      this.columnsShow = Object.keys(this.data[0]);
  }

  async getDataFromFunc() {
    const filters: ApiQueryFilters = new ApiQueryFilters()
    filters.PageIndex = this.index;
    filters.PageSize = this.rowTake;

    if (this.filterVal)
      (this.columnsFilter?.length ? this.columnsFilter : this.columnsNames)
        .forEach(col => filters.addAdditionalFilter(col, this.filterVal, null, null, "Contains", false, false, false, "Text", false, false));

    const data = await this.getDataFunc(filters)
    this.isGetAll = data.length !== this.rowTake;

    this.selected = this.index === 0 ? data : this.selected.concat(data)

    if (this.selected.length && !this.columnsNames.length)
      this.columnsShow = Object.keys(this.selected[0]);

    if(this.index === 0)
      this.selected.unshift(this.columnsHeader);
  }

  initialData() {
    this.isGetAll = false;
    this.selected = [];
  }

  private initColumns(columns: any[]) {
    if (typeof columns[0] === 'string') {
      this.columnsNames = columns;
      this.columnsHeader = this.arrayToObject(columns);
    } else {
      this.columnsNames = Object.keys(columns);
      this.columnsHeader = columns;
    }
  }

  search(event: any) {
    if (this.getDataFunc !== null) {
      this.filterVal = event.query;
      this.isGetAll = false;
      this.index = 0;
      this.getDataFromFunc();
      return;
    }

    this.selected = this.data?.filter(this.searchValueInObject(event.query, this.columnsNames));
    this.selected.unshift(this.columnsHeader);
  }

  async openSearchDialog() {
    const columns: GenericTableColumn[] = Object.keys(this.columnsHeader).map(columnsName => { return { name: columnsName, alias: this.columnsHeader[columnsName] } })
    // const recordSelected: any = await this.genericTableService.open(this.data, this.label, columns).onClose.toPromise()
    const recordSelected: any = await new Promise<any>(async (resolve) => {
      const header: string = this.titleStyle(this.controlName) + ' Search'
      const dialogRef: DynamicDialogRef = !!this.getDataFunc ?
        await this.genericTableService.openByApiOpenQuoeryFilter(this.getDataFunc, header, this.columnsFilter, columns) :
        this.genericTableService.open(this.data, header, columns, null, this.logTableName);

      dialogRef.onClose.pipe(take(1)).subscribe(x => resolve(x));
    });

    this.formGroup.controls[this.controlName].setValue(recordSelected)
  }

  onSelected(val: any) {
    if (this.selected[0] == val)
      this.formGroup.controls[this.controlName].reset()
    else
      this.onSelect.emit(val)
  }

  onBlur() {
    const ctrl: AbstractControl = this.formGroup.controls[this.controlName];
    const val: any = ctrl.value;

    if (!this.data?.includes(val))
      ctrl.reset();
    else
      this.onSelect.emit(val)
  }

  private searchValueInObject(value: string, propsName: string[]): (value1: any, index: number, array: any[]) => unknown {
    return x => propsName.some(prop => x[prop]?.toLowerCase().includes(value?.toLowerCase()));
  }

  private arrayToObject(arr: string[]): {} {
    const obj: any = {};
    arr.forEach(x => { obj[x] = x })
    return obj;
  }

  ngDoCheck() {
    if (!!this.getDataFunc)
      this.getNewData()
  }

  getNewData() {
    const elms: HTMLCollection = this.document.getElementsByClassName('cdk-virtual-scroll-content-wrapper');
    if (!elms.length) return;
    const div: HTMLDivElement = elms[0] as HTMLDivElement;
    const transform: string = div.style.transform;
    const px: number = + transform.substring(transform.indexOf('(') + 1, transform.length - 3)
    if (!px) return;
    const rowIndex = px / this.itemSize + 10;

    if (50 + this.index * rowIndex < rowIndex) {
      this.index++;
      this.getDataFromFunc();
    }
  }

  private titleStyle(str: string) {
    return this.capitalize(this.addSpace(str))
  }

  private capitalize(str: string):string {
    return str.charAt(0).toUpperCase() + str.slice(1);
  }

  private addSpace(str:string) {
    return str.replace(/[A-Z]/g, letter => ' ' + letter);
  }
  
  alignRow(e: any) {
    fromEvent(e.element, 'scroll')
      .pipe(debounceTime(100))
      .subscribe((e: any) => {
        const div = e.target as HTMLDivElement;
        const diff: number = div.scrollTop % 26;
        if (diff > 2 && diff < 24)
          div.scrollBy(0, 26 - diff + 1);
      });
  }
}
