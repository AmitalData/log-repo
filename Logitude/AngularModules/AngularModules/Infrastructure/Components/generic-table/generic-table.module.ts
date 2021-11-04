import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GenericTableComponent } from './generic-table.component';
import { GenericTableService } from './generic-table.service';
import {DialogService, DynamicDialogModule} from 'primeng/dynamicdialog';
import {TableModule} from 'primeng/table';
import {InputTextModule} from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { PaginatorModule } from 'primeng/paginator';
import { GenericTableDataService } from './generic-table-data.service';
import { DraggableModule } from 'Infrastructure/Directives/draggable/draggable.module';


@NgModule({
  declarations: [
    GenericTableComponent
  ],
  exports: [
    GenericTableComponent
  ],
  providers: [
    GenericTableService,
    DialogService,
    GenericTableDataService, 
  ],
  imports: [
    CommonModule,
    DynamicDialogModule,
    TableModule,
    InputTextModule,
    ButtonModule,
    PaginatorModule,
    DraggableModule,
  ],entryComponents: [
    GenericTableComponent    
  ]
})
export class GenericTableModule { }
