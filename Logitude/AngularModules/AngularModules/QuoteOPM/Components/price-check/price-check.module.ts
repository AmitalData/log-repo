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
import { DraggableModule } from 'Infrastructure/Directives/draggable/draggable.module';
import { AccordionModule } from 'Infrastructure/Components/accordion/accordion.module';

@NgModule({
  declarations: [
    PriceCheckComponent,
  ],
  exports: [
    PriceCheckComponent
  ],
  providers: [
    PriceCheckService,
    ConfirmationService,
  ],
  imports: [
    CommonModule,
    DraggableModule,
    TagModule,
    AccordionModule,
    CheckboxModule,
    TooltipModule,
    ScrollPanelModule,
    ButtonModule,
    ConfirmDialogModule,
  ],
  entryComponents: [PriceCheckComponent]
})
export class PriceCheckModule { }
