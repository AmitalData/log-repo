import { DOCUMENT } from '@angular/common';
import { ChangeDetectorRef, Component, EventEmitter, HostListener, Inject, Input, Output, SimpleChanges, ViewChild } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn } from '@angular/forms';
import { GenericTableColumn } from 'Infrastructure/Components/generic-table/generic-table.component';
import { GenericTableService } from 'Infrastructure/Components/generic-table/generic-table.service';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { AutoComplete } from 'primeng/autocomplete';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { fromEvent } from 'rxjs';
import { debounceTime, take } from 'rxjs/operators';
// import { Subscription } from 'rxjs';

type SearchEvent = {originalEvent: InputEvent, query: string}

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
  @Input() outSearchIcon: boolean = false;
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
  getDataBusy: boolean = false;
  filterVal: string = ''
  toHighlight: string = null as any;
  defaultValidator: ValidatorFn = null as any;

  constructor(
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
    if (this.isGetAll || this.getDataBusy) return;

    this.getDataBusy = true;
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

    if (this.index === 0){
      this.selected.unshift(this.columnsHeader);

      const divDDL: HTMLDivElement = document.querySelector('.p-autocomplete-panel');
      if(divDDL)
        divDDL.scrollTop = 0;
    }

    this.getDataBusy = false;
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

  search(event: SearchEvent) {
    this.toHighlight = event.query;

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

    this.formGroup.controls[this.controlName].setValue(recordSelected);
    this.setDefaultValidator();
  }

  onSelected(val: any) {
    if (this.selected[0] == val)
      this.formGroup.controls[this.controlName].reset()
    else {
      this.onSelect.emit(val)
      this.setDefaultValidator()
    }
  }

  onBlur() {
    const ctrl: AbstractControl = this.formGroup.controls[this.controlName];
    const val: any = ctrl.value;

    if (val && !this.data?.includes(val) && !this.selected?.includes(val))
      this.setNotIdentityValueValidator()
    else {
      this.setDefaultValidator()
      this.onSelect.emit(val)
    }
  }

  private setNotIdentityValueValidator() {
    if (this.defaultValidator !== null) return;

    const ctrl: AbstractControl = this.formGroup.controls[this.controlName];
    this.defaultValidator = (this.formGroup.get(this.controlName)?.validator as ValidatorFn) || undefined;
    ctrl.setValidators([this.notIdentityValueValidator]);
    ctrl.updateValueAndValidity()
  }

  notIdentityValueValidator = (control: AbstractControl): ValidationErrors | null => {
    return { notIdentityValue: { value: control?.value } };
  };

  private setDefaultValidator() {
    if (this.defaultValidator === null) return

    const ctrl: AbstractControl = this.formGroup.controls[this.controlName];
    ctrl.setValidators(this.defaultValidator || null);
    this.defaultValidator = null as any
    ctrl.updateValueAndValidity()
  }

  private searchValueInObject(value: string, propsName: string[]): (value1: any, index: number, array: any[]) => unknown {
    return x => propsName.some(prop => ('' + x[prop])?.toLowerCase().includes(value?.toLowerCase()));
  }

  private arrayToObject(arr: string[]): {} {
    const obj: any = {};
    arr.forEach(x => { obj[x] = x })
    return obj;
  }

  private titleStyle(str: string) {
    return this.capitalize(this.addSpace(str))
  }

  private capitalize(str: string): string {
    return str.charAt(0).toUpperCase() + str.slice(1);
  }

  private addSpace(str: string) {
    return str.replace(/[A-Z]/g, letter => ' ' + letter);
  }

  subscribeScrollDDL(e: any) {
    fromEvent(e.element, 'scroll')
      .pipe(debounceTime(100))
      .subscribe((e: any) => {
        const divDDL = e.target as HTMLDivElement;
        this.alignRow(divDDL);

        if (!!this.getDataFunc)
          this.getNewData(divDDL)
      });
  }

  getNewData(divDDL: HTMLDivElement) {
    const rowHeight: number = divDDL.querySelector('li').offsetHeight;
    const scrollLeft: number = divDDL.scrollHeight - divDDL.scrollTop;
    const rowLeft: number = scrollLeft / rowHeight;

    if (rowLeft < 100) {
      this.index++;
      this.getDataFromFunc();
    }
  }

  private alignRow(divDDL: HTMLDivElement) {
    const rowHeight: number = divDDL.querySelector('li').offsetHeight;
    const diff: number = divDDL.scrollTop % rowHeight;
    if (diff > rowHeight + 2 && diff < rowHeight - 2)
      divDDL.scrollBy(0, rowHeight - diff + 1);
  }

  openDdl(e: Event) {
    e.stopPropagation();
    this.autoComplete.handleDropdownClick(this.autoComplete);
  }
}
