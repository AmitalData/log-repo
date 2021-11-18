import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PriceCheckComponent } from './price-check.component';
import { PriceCheckService } from './price-check.service';
import { TagModule } from 'primeng/tag';
import { CheckboxModule } from 'primeng/checkbox';
import {TooltipModule} from 'primeng/tooltip';
import {ScrollPanelModule} from 'primeng/scrollpanel';
import { ButtonModule } from 'primeng/button';
import {ConfirmDialogModule} from 'primeng/confirmdialog';
import {ConfirmationService} from 'primeng/api';
import { AccordionModule } from 'Infrastructure/Components/accordion/accordion.module';
import { DraggableModule } from 'Infrastructure/Directives/draggable/draggable.module';
import { Xml2jsonService } from 'Infrastructure/Services/xml2json/xml2json.service';
import { PriceCheckDataService } from './price-check-data/price-check-data.service';
import { ChargesTypeListService } from 'Common/Services/StandardLists/ChargesTypeListService';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PriceCheckDetailsPipe } from './price-check-details/price-check-details.pipe';
import { IconSrcPipe } from './icon-src/icon-src.pipe';
import { IconCurrencyPipe } from './icon-currency/icon-currency.pipe';
import { ProductTypeListService } from 'Common/Services/StandardLists/ProductTypeListService';

@NgModule({
  declarations: [
    PriceCheckComponent,
    PriceCheckDetailsPipe,
    IconSrcPipe,
    IconCurrencyPipe,
  ],
  exports: [
    PriceCheckComponent
  ],
  providers: [
    PriceCheckService,
    ConfirmationService,
    Xml2jsonService,
    PriceCheckDataService,
    ChargesTypeListService,
    ProductTypeListService,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    DraggableModule,
    AccordionModule,
    TagModule,
    CheckboxModule,
    TooltipModule,
    ScrollPanelModule,
    ButtonModule,
    ConfirmDialogModule,
  ],
  entryComponents: [PriceCheckComponent]
})
export class PriceCheckModule { }
