import { Component, Input, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-btn-menu-plus',
  template: `
  <p-menu appendTo="body" #menu [popup]="true" [model]="ddl"></p-menu>
  <p-button (onClick)='menu.toggle($event)'>
    <i class="pi pi-plus"></i>
    <i class="pi pi-angle-down"></i>
  </p-button>
  `,
  styles: [`
  
  :host ::ng-deep .p-button {
        width: auto;
        border-color: #e2f0fb;
        background: #e2f0fb;
        color: #1084dc;
        padding: 11px !important;
    }

    :host ::ng-deep .pi-plus {
        font-weight: bold;
        padding-right: 4px;
    }

    :host ::ng-deep .pi-angle-down {
        padding-left: 3px;
        border-left: 1px solid #ced4da;
        margin-left: 11px;
    }

    :host ::ng-deep .p-button-label {
        display: none;
    }   
  `]
})
export class BtnMenuPlusComponent {
  @Input() ddl: MenuItem[] = [];
}
