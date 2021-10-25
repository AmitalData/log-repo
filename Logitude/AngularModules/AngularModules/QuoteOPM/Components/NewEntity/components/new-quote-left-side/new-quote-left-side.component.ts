import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DirectionList } from 'Infrastructure/EntityLists/DirectionList';
import { TransportModeList } from 'Infrastructure/EntityLists/TransportModeList';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { ShipmentTypeList } from 'Shipment/EntityLists/ShipmentTypeList';
import { NewQuoteDataService } from '../../Services/new-quote-data/new-quote-data.service';


@Component({
  selector: 'app-new-quote-left-side',
  templateUrl: './new-quote-left-side.component.html',
  styleUrls: ['./new-quote-left-side.component.scss']
})
export class NewQuoteLeftSideComponent implements OnInit {
  @Input() EntityPM: QuoteOPPM = null as any;
  @Input() formGroup: FormGroup = null as any;

  transportModeList: TransportModeList[] = []
  directionList: DirectionList[] = []
  shipmentTypeList: ShipmentTypeList[] = []
  shipmentTypeListFilter: ShipmentTypeList[] = []

  icons: {} = {
    Air: 'assets/icons/airplane.png',
    Ocean: 'assets/icons/ocean.png',
    Inland: 'assets/icons/inland.png',
    Export: 'assets/icons/box-up.png',
    Domestic: 'assets/icons/house.png',
    Import: 'assets/icons/box-down.png',
    Drop: 'assets/icons/recycle.png',
    FTL: 'assets/icons/inland.png',
    FCL: 'assets/icons/inland.png',
    LTL: 'assets/icons/inland.png',
    LCL: 'assets/icons/inland.png',
  }

  constructor(
    private newQuoteDataService: NewQuoteDataService,
  ) { }

  ngOnInit(): void {
    this.getTransportModeList()
    this.getDirectionList();
    this.getShipmentTypeList()
  }

  async getDirectionList() {
    this.directionList = await this.newQuoteDataService.getDirectionList()
    this.directionList = this.directionList.filter(x => !x.Name.toLowerCase().includes('customs'))
    this.sortDirectionList();
  }

  sortDirectionList() {
    ['Export', 'Import', 'Domestic', 'Drop'].forEach((val: string, i: number) =>
      this.changePositionInArr(this.directionList, this.directionList.findIndex(x => x.Name === val), i));
  }

  async getTransportModeList() {
    this.transportModeList = await this.newQuoteDataService.getTransportModeList()
    this.sortTransportModeList();
  }

  sortTransportModeList() {
    ['Air', 'Inland', 'Ocean'].forEach((val: string, i: number) =>
      this.changePositionInArr(this.transportModeList, this.transportModeList.findIndex(x => x.Name === val), i));
  }

  async getShipmentTypeList() {
    this.shipmentTypeList = await this.newQuoteDataService.getShipmentTypeList()
  }
  
  getShipmentTypeListFilter(transportMode:string) {
    const shipmentTypes: string[] = transportMode === 'Ocean' ? ['FCL', 'LCL'] : ['FTL', 'LTL']
    this.shipmentTypeListFilter = this.shipmentTypeList.filter(x => shipmentTypes.includes(x.Name))
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.formGroup.contains('shipmentType')) {
      this.addFormControls()
      this.subscribeCtrls()
    }
  }

  addFormControls() {
    this.formGroup.addControl('direction', new FormControl(null, Validators.required));
    this.formGroup.addControl('transportMode', new FormControl(null, Validators.required));
    this.formGroup.addControl('shipmentType', new FormControl());
  }

  subscribeCtrls() {
    this.formGroup.controls.direction.valueChanges.subscribe((val: DirectionList) => this.EntityPM.DirectionId = val.Id);
    this.formGroup.controls.shipmentType.valueChanges.subscribe((val: ShipmentTypeList) => this.EntityPM.ShipmentTypeId = val.Id);
    this.formGroup.controls.transportMode.valueChanges.subscribe((val: TransportModeList) => {
      this.getShipmentTypeListFilter(val.Name);
      this.EntityPM.TransportModeId = val.Id;
    });
  }

  private changePositionInArr(arr, fromIndex, toIndex) {
    var element = arr[fromIndex];
    arr.splice(fromIndex, 1);
    arr.splice(toIndex, 0, element);
  }
}