import { AfterViewInit, ChangeDetectorRef, Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl, FormArray, Validators } from '@angular/forms';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { MessageService } from 'primeng/api';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { BehaviorSubject, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { NewQuoteDataShareService } from '../../Services/new-quote-data-share/new-quote-data-share.service';
import { Carrier, Incoterm, NewQuoteDataService, Port, SpecialService } from '../../Services/new-quote-data/new-quote-data.service';

@Component({
  selector: 'app-new-quote-properties',
  templateUrl: './new-quote-properties.component.html',
  styleUrls: ['./new-quote-properties.component.scss']
})
export class NewQuotePropertiesComponent implements OnInit, AfterViewInit {
  @Input() EntityPM: QuoteOPPM = null as any;
  @Input() formGroup: FormGroup = null as any;

  fromPortList: Port[] = []
  toPortList: Port[] = []
  specialServiceList: SpecialService[] = []
  mainCarriageCarrierFunc: (filter: ApiQueryFilters) => Promise<any[]> = null as any;
  incotermList: Incoterm[] = []
  transportModeId: string = '';
  directionId: string = '';
  carrierColumns: any = {}
  formArray: FormArray = new FormArray([this.propForm]);
  indexTabs: BehaviorSubject<number> = null as any;
  activeTab: number = 0;
  destroyObservable: Subject<void> = new Subject<void>();  

  get propForm(): FormGroup {
    return new FormGroup({
      fromPort: new FormControl('', Validators.required),
      toPort: new FormControl('', Validators.required),
      specialService: new FormControl(),
      mainCarriageCarrier: new FormControl(),
      incoterm: new FormControl(),
    })
  }

  get properties(): any {
    return this.formGroup.get('properties') as any;
  }

  constructor(
    private newQuoteDataService: NewQuoteDataService,
    private messageService: MessageService,
    private dataShareService: NewQuoteDataShareService,
    private cdr: ChangeDetectorRef,
  ) { }

  ngAfterViewInit(): void {
    if (this.EntityPM != null && this.EntityPM.Id != null) {
      if (this.EntityPM.QuoteProperties.length > 0) {
        for (let QuoteOpProperty of this.EntityPM.QuoteProperties) {
          //edit part
          if (QuoteOpProperty.FromPortId) {

          }
        }
      }
    }
  }

  ngOnInit(): void {
    this.subscribeIndexTab();
    this.getIncoterms()
    this.resetForm();
  }

  private subscribeIndexTab() {
    this.indexTabs = this.dataShareService.indexPropertyTab;
    this.indexTabs.pipe(takeUntil(this.destroyObservable)).subscribe(x => this.activeTab = x);
  }

  resetForm() {
    this.newQuoteDataService.$resetForm.subscribe(() => {
      this.formArray.clear()
      this.formArray.push(this.propForm);
      this.indexTabs.next(0);
    })
  }


  ngOnChanges(changes: SimpleChanges) {
    if (!this.formGroup.contains('properties')) {
      this.addFormControls()
      if (this.formGroup.contains('transportMode')) {
        this.subscribeTransport()
      }
    }
  }

  addFormControls() {
    this.formGroup.addControl('properties', this.formArray)
  }

  subscribeTransport() {
    this.formGroup.controls.transportMode.valueChanges.subscribe(() => this.updateList())
    this.formGroup.controls.direction.valueChanges.subscribe(() => this.updateList())
  }

  updateList() {
    this.transportModeId = this.formGroup.controls.transportMode.value?.Id;
    this.directionId = this.formGroup.controls.direction.value?.Id;

    if (!this.transportModeId || !this.directionId) return;

    this.getPorts()
    this.getSpecialService()
    this.getMainCarriageCarrier()
  }

  async getPorts() {
    this.fromPortList = await this.newQuoteDataService.getPorts(this.directionId, this.transportModeId);
    this.toPortList = this.fromPortList;
  }

  async getSpecialService() {
    this.specialServiceList = await this.newQuoteDataService.getSpecialServices(this.directionId, this.transportModeId);
  }

  async getMainCarriageCarrier() {
    const filters = new ApiQueryFilters();
    filters.PageIndex = 0;
    filters.PageSize = 100;

    this.carrierColumns = this.directionId === "E" ? { AIRLINE_ID: 'Code' } : { VENDOR_ID: 'Code' }
    this.carrierColumns = { ...this.carrierColumns, ...{ Name: 'Name', Prefix: 'Prefix' } }

    this.mainCarriageCarrierFunc = (qf: ApiQueryFilters) => this.newQuoteDataService.getCarrierses(this.directionId, this.transportModeId, qf);
  }

  async getIncoterms() {
    this.incotermList = await this.newQuoteDataService.getIncoterms();
  }

  addProperty() {
    if (this.formArray.valid) {
      this.formArray.push(this.propForm)
      this.cdr.detectChanges()
      this.indexTabs.next(this.formArray.length - 1);
    } else
      this.messageService.add({ severity: 'error', summary: 'Property not add', detail: 'Not all the field in Quote Properties were entered/filled' })
  }

  removeProperty(e: { originalEvent: PointerEvent, index: number }) {
    this.indexTabs.next(0);
    this.formArray.removeAt(e.index)
  }

  sortArray(arr: any[], prop: string): any[] {
    return arr.sort((a, b) => (a[prop] > b[prop]) ? 1 : ((b[prop] > a[prop]) ? -1 : 0))
  }

  ngOnDestroy() {
    this.destroyObservable.next()
    this.destroyObservable.complete()
  }
}