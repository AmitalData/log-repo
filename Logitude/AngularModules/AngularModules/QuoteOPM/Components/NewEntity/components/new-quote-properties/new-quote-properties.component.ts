import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl, FormArray, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { Carrier, Incoterm, NewQuoteDataService, Port, SpecialService } from '../../Services/new-quote-data/new-quote-data.service';

@Component({
  selector: 'app-new-quote-properties',
  templateUrl: './new-quote-properties.component.html',
  styleUrls: ['./new-quote-properties.component.scss']
})
export class NewQuotePropertiesComponent implements OnInit {
  @Input() EntityPM: QuoteOPPM = null as any;
  @Input() formGroup: FormGroup = null as any;

  fromPortList: Port[] = []
  toPortList: Port[] = []
  specialServiceList: SpecialService[] = []
  mainCarriageCarrierList: Carrier[] = []
  incotermList: Incoterm[] = []
  transportModeId: string = '';
  directionId: string = '';
  carrierColumns: any = {}
  formArray: FormArray = new FormArray([this.propForm]);
  index: number = 0;
 
  get propForm(): FormGroup {
    return new FormGroup({
      fromPort: new FormControl('', Validators.required),
      toPort: new FormControl('', Validators.required),
      specialService: new FormControl(),
      mainCarriageCarrier: new FormControl('', Validators.required),
      incoterm: new FormControl('', Validators.required),
      // delivery: new FormGroup({}),
      // pickup: new FormGroup({}),
    })
  }

  get properties(): any {
    return this.formGroup.get('properties') as any;
  }

  constructor(
    private newQuoteDataService: NewQuoteDataService,
    private messageService: MessageService,
  ) { }

  ngOnInit(): void {
    this.getIncoterms()
    let a = this.formArray[1]
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.formGroup.contains('properties')) {
      this.addFormControls()
      this.subscribeTransport()
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
    this.mainCarriageCarrierList = await this.newQuoteDataService.getCarrierses(this.directionId, this.transportModeId);
    this.mainCarriageCarrierList = this.sortArray(this.mainCarriageCarrierList, 'Name')

    this.carrierColumns = this.directionId === "E" ? { AIRLINE_ID: 'Code' } : { VENDOR_ID: 'Code' }
    this.carrierColumns = { ...this.carrierColumns, ...{ Name: 'Name', Prefix: 'Prefix' } }
  }

  async getIncoterms() {
    this.incotermList = await this.newQuoteDataService.getIncoterms();
  }

  addProperty() {
    console.log(this.formArray)
    if(this.formArray.valid){
      this.index = this.formArray.length;
      this.formArray.push(this.propForm)
    } else
      this.messageService.add({ severity: 'error', summary: 'Property not add', detail: 'have feild in exist propreties that not vlid'})
  }

  removeProperty(e: { originalEvent: PointerEvent, index: number }) {
    this.index = 0
    this.formArray.removeAt(e.index)
  }

  sortArray(arr: any[], prop: string): any[] {
    return arr.sort((a,b) => (a[prop] > b[prop]) ? 1 : ((b[prop] > a[prop]) ? -1 : 0))  
  }
}