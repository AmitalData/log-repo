import {Directive, ElementRef, Renderer, Input, Component, View, OnInit,OnChanges} from 'angular2/core';
import {BaseComponent} from './app/infrastructure/logitude-components/Base.Component';
import {TextCodeTranslator} from '../../utilities/TextCodeTranslator';
import {Control, Validators, CORE_DIRECTIVES, FORM_DIRECTIVES} from 'angular2/common';
import {UIProperty, UIProperties} from './app/infrastructure/logitude-components/UIProperties';


@Component({
    selector: 'logitude-label',
    template: `
<div *ngIf="uiProperty.IsVisible">
<div [hidden]="!uiProperty.IsRequired || (DataContext[fieldname] !=null && DataContext[fieldname] !='')">
    <svg  height="20" width="20" style="position: static;right: 5px;top: 25px; float:left">
    <circle cx="10" cy="10" r="5" fill="red" />
    </svg>
</div>
    <label> {{translation}}</label>
</div>
`,
    inputs: ['fieldname', 'objecttable', 'DataContext'],
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES]
})

export class LogitudeLabelComponent implements OnInit{

    public translation: string;

     fieldname: string;
     objecttable: string;
     public DataContext: any;

     objectfield: any;
    
     uiProperty: UIProperty;
     constructor() {
       
         
     }
    ngOnInit() {
        
        
        this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.fieldname, this.objecttable);
        this.objectfield = window.ObjectFields.filter(d=> d.FieldName == this.fieldname && d.ObjectTableName == this.objecttable)[0];
        var textcodecode = this.objectfield.FullNameTextCodeCode;
        this.translation = TextCodeTranslator.transform(textcodecode);
       
        
     }
   

}