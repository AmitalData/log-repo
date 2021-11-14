import { Component, OnInit } from '@angular/core';
import { ChargesTypeList } from 'Common/EntityLists/ChargesTypeList';
import { ConfirmationService } from 'primeng/api';
import { DynamicDialogConfig } from 'primeng/dynamicdialog';
import { PriceCheckDataService } from './price-check-data/price-check-data.service';
import { PriceChekRootResponse, Offer } from './price-check.service';

@Component({
  selector: 'app-price-check',
  templateUrl: './price-check.component.html',
  styleUrls: ['./price-check.component.scss']
})
export class PriceCheckComponent implements OnInit {
  selectedFilter: string = ''
  offers: Offer[] = [];
  offersFilterd: Offer[] = [];
  cahargesTypes: ChargesTypeList[] = [];

  filters: { filter: string, alias: string }[] = [
    { filter: 'DirectFlight', alias: 'Direct' },
    { filter: 'Fastest', alias: 'Quickest' },
    { filter: 'Cheapest', alias: 'Cheapest' },
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
  ) { }

  async ngOnInit(): Promise<void> {
    this.initData();
    await this.initPricesDetails()
  }

  private initData() {
    this.offers = (this.config.data as PriceChekRootResponse).PriceChekResponse.Offers.Offer;
    this.offersFilterd = this.offers;
    console.log(this.config.data)
    console.log(this.offers)
  }

  private async initPricesDetails() {
    this.cahargesTypes = await this.priceCheckDataS.getCahargesType()
    console.log(this.cahargesTypes)

  }

  setFilter(filterType: string) {
    this.selectedFilter = filterType;
    console.log(filterType)

    this.offersFilterd = this.offers.filter((offer: Offer) => offer.Result.Summary[filterType] === 'True')
  }

  sendData() {
    this.confirmationService.confirm({
      message: 'No Offer has been Selected, Do you want to Continue?',
      header: 'Confirmation',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        console.log('accept')
      },
    });
  }
}
