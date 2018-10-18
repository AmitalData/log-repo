import { Component, Input ,OnInit,ChangeDetectorRef} from '@angular/core';

@Component({
    selector: 'row-template', 
    template: `<div><ng-content></ng-content></div>`,
})
export class LogRowDetailsTemplateComponent  { 
    constructor() {
      
    }
}
