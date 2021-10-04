import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { NewQuoteDataService } from '../../Services/new-quote-data/new-quote-data.service';

@Component({
  selector: 'app-new-quote-properties',
  templateUrl: './new-quote-properties.component.html',
  styleUrls: ['./new-quote-properties.component.scss']
})
export class NewQuotePropertiesComponent implements OnInit {
  @Input() EntityPM: QuoteOPPM = null as any;
  @Input() formGroup: FormGroup = null as any;

  fromPortList:any[] = []
  toPortList:any[] = []
  specialServiceList:any[] = []
  mainCarriageCarrierList:any[] = []
  incotermList:any[] = []

  constructor(
    private newQuoteDataService: NewQuoteDataService,
  ) { }

  ngOnInit(): void {
    this.getFromPort()
    this.getToPort()
    this.getSpecialService()
    this.getMainCarriageCarrier()
    this.getIncoterms()

    // this.EntityPM.FromPortId 
    // this.EntityPM.ToPortId 
    // this.EntityPM.SpecialServiceId 
    // this.EntityPM.MainCarriageCarrierId 
    // this.EntityPM.IncotermId
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.formGroup.contains('fromPort'))
      this.addFormControls()
  }

  addFormControls() {
    this.formGroup.addControl('fromPort', new FormControl());
    this.formGroup.addControl('toPort', new FormControl());
    this.formGroup.addControl('specialService', new FormControl());
    this.formGroup.addControl('mainCarriageCarrier', new FormControl());
    this.formGroup.addControl('incoterm', new FormControl());
  }
  
  async getFromPort() {
    this.fromPortList = await this.newQuoteDataService.getIncoterms();
    console.log(this.fromPortList)
  }

  async getToPort() {
    this.toPortList= await this.newQuoteDataService.getIncoterms();
    console.log(this.toPortList)
  }

  async getSpecialService() {
    this.specialServiceList= await this.newQuoteDataService.getIncoterms();
    console.log(this.specialServiceList)
  }

  async getMainCarriageCarrier() {
    this.mainCarriageCarrierList = await this.newQuoteDataService.getIncoterms();
    console.log(this.mainCarriageCarrierList)
  }

  async getIncoterms() {
    this.incotermList = await this.newQuoteDataService.getIncoterms();
    console.log(this.incotermList)
  }
  
  onSelectedtoPort(value:any){
    this.EntityPM.ToPortId = value;
  }

  onSelectedFromPort(value:any){
    this.EntityPM.FromPortId = value;
  }

  onMainCarriageCarrier(value:any){
    this.EntityPM.MainCarriageCarrierId = value;
  }

  onSelectedIncoterm(value:any){
    this.EntityPM.IncotermId = value;
  }

  onSelectedSpecialService(value:any){
    this.EntityPM.SpecialServiceId = value;
  }

}
