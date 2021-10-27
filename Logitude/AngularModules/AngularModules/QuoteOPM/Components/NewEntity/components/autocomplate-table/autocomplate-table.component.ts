import { ChangeDetectorRef, Component, EventEmitter, Input, Output, SimpleChanges, ViewChild } from '@angular/core';
import { AbstractControl, FormGroup } from '@angular/forms';
import { GenericTableColumn } from 'Customs/Components/generic-table/generic-table.component';
import { GenericTableService } from 'Customs/Components/generic-table/generic-table.service';
import { AutoComplete } from 'primeng/autocomplete';
import { take } from 'rxjs/operators';
// import { Subscription } from 'rxjs';

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
  @Input() searchIcon: boolean = false;
  @Input() dropIcon: boolean= false;
  @ViewChild('autoComplete') autoComplete: AutoComplete = null as any;
   @Input() set columnsShow(columns: any[]) {
    this.initColumns(columns);
  }
  @Output() onSelect = new EventEmitter()

  selected: any[] = []
  columnsNames: string[] = [];
  columnsHeader: any = [];
  selectedChoice: any;

  constructor(
    private genericTableService: GenericTableService,
  ) {}

  // subscribeRef: Subscription = null as any;

  // constructor(private cdref: ChangeDetectorRef){}

  // ngOnDestroy() {
  //   this.subscribeRef?.unsubscribe()
  // }

  // ngOnChanges(changes: SimpleChanges) {
  //   if (this.formGroup && !this.subscribeRef) 
  //     this.subscribeCtrl();
  // }

  // private subscribeCtrl() {
  //   this.subscribeRef = this.formGroup.controls[this.controlName].valueChanges.subscribe(val=> {
  //     this.selectedChoice = val
  //     this.cdref.detectChanges();
  //   })
  // }

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
    this.selected = this.data?.filter(this.searchValueInObject(event.query, this.columnsNames));
    this.selected.unshift(this.columnsHeader);
  }

  async openSearchDialog() {
    const columns: GenericTableColumn[] = Object.keys(this.columnsHeader).map(columnsName => { return { name: columnsName , alias: this.columnsHeader[columnsName] } }) 
    // const recordSelected: any = await this.genericTableService.open(this.data, this.label, columns).onClose.toPromise()
    const recordSelected: any = await new Promise<any>((resolve) => 
      this.genericTableService.open(this.data, this.label, columns).onClose.pipe(take(1)).subscribe(x=>resolve(x)));      
    
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
}
