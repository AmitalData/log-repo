import { Injectable } from '@angular/core';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { PriceCheckComponent } from './price-check.component';

@Injectable()
export class PriceCheckService {

  constructor(
    private dialogService: DialogService,
  ) { }

  async open(quote: QuoteOPPM): Promise<DynamicDialogRef> {
    const config: DynamicDialogConfig = {}
    config.width = '1440px';
    config.height = '1230px';
    config.showHeader = false;
    config.styleClass = 'price-check';
    config.data = quote;

    return this.dialogService.open(PriceCheckComponent, config);
  }

}
