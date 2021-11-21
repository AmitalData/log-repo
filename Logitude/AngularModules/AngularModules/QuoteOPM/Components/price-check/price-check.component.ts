import { Component, OnInit } from '@angular/core';
import { FormArray, FormControl } from '@angular/forms';
import { ChargesTypeList } from 'Common/EntityLists/ChargesTypeList';
import { ProductTypeList } from 'Common/EntityLists/ProductTypeList';
import { ConfirmationService } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { PriceCheckDataService } from './price-check-data/price-check-data.service';
import { PriceChekRootResponse, Offer } from './price-check.service';

@Component({
  selector: 'app-price-check',
  templateUrl: './price-check.component.html',
  styleUrls: ['./price-check.component.scss']
})
export class PriceCheckComponent implements OnInit {
  checkBoxs: FormArray = new FormArray([])
  selectedFilter: string = ''
  offers: Offer[] = [];
  offersFilterd: Offer[] = [];
  cahargesTypes: ChargesTypeList[] = [];
  productTypeList: ProductTypeList[] = [];

  filters: { filter: string, alias: string }[] = [
    { filter: 'Fastest', alias: 'Quickest' },
    { filter: 'Cheapest', alias: 'Cheapest' },
    { filter: 'DirectFlight', alias: 'Direct' },
  ]

  summaryItems: { text: string, keyName: any }[] = [
    { text: 'Cost', keyName: 'TotalCost' },
    { text: 'Sale', keyName: 'TotalSale' },
    { text: 'Estimated Profit', keyName: 'EstimatedProfit' },
  ]

  constructor(
    private config: DynamicDialogConfig,
    private confirmationService: ConfirmationService,
    private priceCheckDataS: PriceCheckDataService,
    private ref: DynamicDialogRef,
  ) { }

  async ngOnInit(): Promise<void> {
    Promise.all([this.initPricesDetails(), this.initProductType()])
    this.initData();
  }

  private initData() {
    this.offers = (this.config.data as PriceChekRootResponse).PriceChekResponse.Offers.Offer;
    this.offersFilterd = this.offers;
    this.updateCheckBoxFormArray();
    // console.log(this.config.data)
    // console.log(this.offers)
  }

  private updateCheckBoxFormArray() {
    this.checkBoxs = new FormArray(Array.from({ length: this.offersFilterd.length }, (v, i) => new FormControl(false)))
  }

  private async initPricesDetails() {
    this.cahargesTypes = await this.priceCheckDataS.getCahargesType()
  }

  private async initProductType() {
    this.productTypeList = await this.priceCheckDataS.getProducteType()
  }

  setFilter(filterType: string) {
    this.selectedFilter = filterType;

    this.offersFilterd = this.offers.filter((offer: Offer) => offer.Result.Summary[filterType] === 'True')
    this.updateCheckBoxFormArray();
  }

  sendData() {
    if ((this.checkBoxs.value as boolean[]).every(x => !x))
      this.confirmationService.confirm({
        message: 'No Offer has been Selected, Do you want to Continue?',
        header: 'Confirmation',
        icon: 'pi pi-exclamation-triangle',
        accept: () => {
          console.log('accept')
        },
      });
  }

  cancel() {
    this.ref.close();
  }
}
