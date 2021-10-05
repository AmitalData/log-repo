import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AbstractControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-autocomplate-table',
  templateUrl: './autocomplate-table.component.html',
  styleUrls: ['./autocomplate-table.component.scss']
})
export class AutocomplateTableComponent {
  @Input() formGroup: FormGroup = null as any;

  @Input() label: string = '';
  @Input() fieldShow: string = '';
  @Input() controlName: string = '';
  @Input() data: any[] = []
  @Input() set columnsShow(columns: any[]) {
    this.initColumns(columns);
  }
  @Output() onSelect = new EventEmitter()

  selected: any[] = []
  columnsNames: string[] = [];
  columnsHeader: {} = [];

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
    this.selected = this.data.filter(this.searchValueInObject(event.query, this.columnsNames));
    this.selected.unshift(this.columnsHeader);
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

    if (!this.data.includes(val))
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
}
