import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { Component } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';



@Component({
  selector: 'BatchInvoicesComponent',

  templateUrl: './BatchInvoicesComponent.html',
})
export class BatchInvoicesComponent extends BaseComponent {
 
  DataContext: any = this;
  constructor() {
    super();
  }
}
