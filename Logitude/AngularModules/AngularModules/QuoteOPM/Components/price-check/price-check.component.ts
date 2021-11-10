import { Component, OnInit } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { DynamicDialogConfig } from 'primeng/dynamicdialog';
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

  filters: {filter: string, alias: string}[] = [
    {filter: 'DirectFlight', alias: 'Direct'},
    {filter: 'Fastest', alias: 'Quickest'},
    {filter: 'Cheapest', alias: 'Cheapest'},
  ]

  summaryItems:{text: string, keyName:any}[] = [
    {text: 'Cost', keyName: 'TotalCost'},
    {text: 'Sale', keyName: 'TotalSale'},
    {text: 'Estimated Profit', keyName: 'EstimatedProfit'},
  ]

  constructor(
    private config: DynamicDialogConfig,
    private confirmationService: ConfirmationService,
  ) { }

  ngOnInit(): void {
    this.initData();
    console.log(this.config.data)
    console.log(this.offers)
  }

  private initData() {
    this.offers = (this.config.data as PriceChekRootResponse).PriceChekResponse.Offers.Offer;
    this.offersFilterd = this.offers;
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
