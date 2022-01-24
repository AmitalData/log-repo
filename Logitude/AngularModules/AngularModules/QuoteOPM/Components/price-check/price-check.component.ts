import { ChangeDetectorRef, Component, ElementRef, isDevMode, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { ChargesTypeList } from 'Common/EntityLists/ChargesTypeList';
import { ProductTypeList } from 'Common/EntityLists/ProductTypeList';
import { ConfirmationService } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ScrollPanel } from 'primeng/scrollpanel';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { PriceCheckDataService } from './price-check-data/price-check-data.service';
import { PriceCheckUnifreightService } from './price-check-unifreight/price-check-unifreight.service';
import { Offer, PriceChekRootResponse } from './price-check.type';

@Component({
  selector: 'app-price-check',
  templateUrl: './price-check.component.html',
  styleUrls: ['./price-check.component.scss']
})
export class PriceCheckComponent implements OnInit {
  filterForm: FormGroup = new FormGroup({})
  checkBoxs: FormArray = new FormArray([])
  offers: Offer[] = [];
  cahargesTypes: ChargesTypeList[] = [];
  productTypeList: ProductTypeList[] = [];
  quote: QuoteOPPM = null as any;

  filters: string[] = ['Quickest', 'Cheapest', 'Direct']

  summaryItems: { text: string, keyName: any }[] = [
    { text: 'Cost', keyName: 'TotalCost' },
    { text: 'Sale', keyName: 'TotalSale' },
    { text: 'Estimated Profit', keyName: 'EstimatedProfit' },
  ]

  constructor(
    private config: DynamicDialogConfig,
    private confirmationService: ConfirmationService,
    private priceCheckDataS: PriceCheckDataService,
    private unifreight: PriceCheckUnifreightService,
    private ref: DynamicDialogRef,
    private cdr: ChangeDetectorRef,
  ) { }

  async ngOnInit(): Promise<void> {
    this.quote = this.config.data;

    this.initPricesDetails()
    this.initProductType()
    this.getPrices()

    this.initFilterFrom();
  }

  initFilterFrom() {
    this.filters.forEach(filter => this.filterForm.addControl(filter, new FormControl()))
  }

  private async getPrices() {
    const prices: PriceChekRootResponse = /* isDevMode() ? await this.unifreight.getPricesTest(this.quote, this.filterForm.value) : */ await this.unifreight.getPrices(this.quote, this.filterForm.value);

    this.offers = prices.PriceChekResponse.Offers.Offer;
    this.updateCheckBoxFormArray();
  }

  private updateCheckBoxFormArray() {
    this.checkBoxs = new FormArray(Array.from({ length: this.offers.length }, (v, i) => new FormControl(false)))
  }

  private async initPricesDetails() {
    this.cahargesTypes = await this.priceCheckDataS.getCahargesType()
  }

  private async initProductType() {
    this.productTypeList = await this.priceCheckDataS.getProducteType()
  }

  setFilter(filterName: string) {
    this.filterForm.controls[filterName].setValue(!this.filterForm.value[filterName]);

    this.getPrices();
  }

  sendData() {
    if ((this.checkBoxs.value as boolean[]).every(x => !x))
      this.confirmationService.confirm({
        message: 'No Offer has been Selected, Do you want to Continue?',
        header: 'Price Check',
        icon: 'pi pi-exclamation-triangle',
        accept: () => {
          console.log('accept')
        },
      });
  }

  cancel() {
    this.ref.close();
  }

  scrollExtend(isHidden: boolean, s: ScrollPanel, offerContainer: HTMLElement) {
    if (isHidden) return;

    s.refresh();
    this.cdr.detectChanges()

    const scrollTopContainr: number = s.contentViewChild.nativeElement.scrollTop;
    const scrollTopElement: number = offerContainer.offsetTop;
    const scrollTopScreen: number = scrollTopElement - scrollTopContainr;
    
    const screen: number = s.containerViewChild.nativeElement.clientHeight;
    const offer: number = offerContainer.clientHeight;
    
    console.log('scrollTopContainr:', scrollTopContainr, 'scrollTopElement:', scrollTopElement, 'scrollTopScreen:', scrollTopScreen, 'screen:', screen, 'offer:', offer)
    
    if (scrollTopScreen < 0 || screen < offer + scrollTopScreen)
      s.scrollTop(scrollTopContainr + scrollTopScreen)
  }
}
