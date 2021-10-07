import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
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

  fromPortList:Port[] = []
  toPortList:Port[] = []
  specialServiceList:SpecialService[] = []
  mainCarriageCarrierList:Carrier[] = []
  incotermList:Incoterm[] = []
  transportModeId: string = '';
  directionId: string = '';

  constructor(
    private newQuoteDataService: NewQuoteDataService,
  ) { }

  ngOnInit(): void {
    this.getIncoterms()
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.formGroup.contains('fromPort')){
      this.addFormControls()
      this.subscribeTransport()
    }
  }

  addFormControls() {
    this.formGroup.addControl('fromPort', new FormControl());
    this.formGroup.addControl('toPort', new FormControl());
    this.formGroup.addControl('specialService', new FormControl());
    this.formGroup.addControl('mainCarriageCarrier', new FormControl());
    this.formGroup.addControl('incoterm', new FormControl());
  }

  subscribeTransport() {
    this.formGroup.controls.transportMode.valueChanges.subscribe(()=>this.updateList())
    this.formGroup.controls.direction.valueChanges.subscribe(()=>this.updateList())
  }

  updateList() {
    this.transportModeId = this.formGroup.controls.transportMode.value?.Id;
    this.directionId = this.formGroup.controls.direction.value?.Id;

    if(!this.transportModeId || !this.directionId) return;
    
    this.getPorts()
    this.getSpecialService()
    this.getMainCarriageCarrier()
  }
  
  async getPorts() {
    this.fromPortList = await this.newQuoteDataService.getPorts(this.directionId, this.transportModeId);
    this.toPortList = this.fromPortList;
  }

  async getSpecialService() {
    this.specialServiceList= await this.newQuoteDataService.getSpecialServices(this.directionId, this.transportModeId);
  }

  async getMainCarriageCarrier() {
    this.mainCarriageCarrierList = await this.newQuoteDataService.getCarrierses(this.directionId, this.transportModeId);
  }

  async getIncoterms() {
    this.incotermList = await this.newQuoteDataService.getIncoterms();
  }
  
  onSelectedtoPort(value:Port){
    this.EntityPM.ToPortId = value.Code;
  }

  onSelectedFromPort(value:Port){
    this.EntityPM.FromPortId = value.Code;
  }

  onMainCarriageCarrier(value:Carrier){
    this.EntityPM.MainCarriageCarrierId = value.AIRLINE_ID;
  }

  onSelectedIncoterm(value:Incoterm){
    this.EntityPM.IncotermId = value.PTERMID;
  }

  onSelectedSpecialService(value:SpecialService){
    this.EntityPM.SpecialServiceId = value.SERVLEVEL_ID;
  }
}
