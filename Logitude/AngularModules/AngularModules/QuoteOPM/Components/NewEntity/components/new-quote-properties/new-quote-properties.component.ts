import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl, FormArray, Validators } from '@angular/forms';
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
 
  get propForm(): FormGroup {
    return new FormGroup({
      fromPort: new FormControl('', Validators.required),
      toPort: new FormControl(),
      specialService: new FormControl(),
      mainCarriageCarrier: new FormControl(),
      incoterm: new FormControl(),
      delivery: new FormGroup({}),
      pickup: new FormGroup({}),
    })
  }

  get properties(): any {
    return this.formGroup.get('properties') as any;
  }

  constructor(
    private newQuoteDataService: NewQuoteDataService,
  ) { }

  ngOnInit(): void {
    this.getIncoterms()
    let a = this.formArray[1]
    console.log(a)
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

    this.carrierColumns = this.directionId === "E" ? { AIRLINE_ID: 'Code' } : { VENDOR_ID: 'Code' }
    this.carrierColumns = { ...this.carrierColumns, ...{ Name: 'Name', Prefix: 'Prefix' } }
  }

  async getIncoterms() {
    this.incotermList = await this.newQuoteDataService.getIncoterms();
  }

  onSelectedtoPort(value: Port) {
    this.EntityPM.ToPortId = value.Code;
  }

  onSelectedFromPort(value: Port) {
    this.EntityPM.FromPortId = value.Code;
  }

  onMainCarriageCarrier(value: Carrier) {
    this.EntityPM.MainCarriageCarrierId = value.AIRLINE_ID;
  }

  onSelectedIncoterm(value: Incoterm) {
    this.EntityPM.IncotermId = value.PTERMID;
  }

  onSelectedSpecialService(value: SpecialService) {
    this.EntityPM.SpecialServiceId = value.SERVLEVEL_ID;
  }

  addProperty() {
    this.formArray.push(this.propForm)
  }

  removeProperty(e: { originalEvent: PointerEvent, index: number }) {
    this.formArray.removeAt(e.index)
  }
}