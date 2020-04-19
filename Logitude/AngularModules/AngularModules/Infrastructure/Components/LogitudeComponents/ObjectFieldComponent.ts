declare var window: any; 
import { Input, Component, AfterViewInit, ViewChild } from '@angular/core';
import {SessionLocator} from '../../Utilities/SessionLocator';
import {ObjectFieldPM} from '../../EntityPMs/ObjectFieldPM';
import {ObjectTablePM} from '../../EntityPMs/ObjectTablePM';
import { ChildDirective } from '../../Directives/ChildDirective';

@Component({
  selector: 'ObjectFieldComponent',
  template: `<div ChildDirective></div>`,
})

export class ObjectFieldComponent implements AfterViewInit {

  @Input() ObjectField: ObjectFieldPM;
  @Input() ObjectTableName: string;
  @Input() DataContext: any;
  @Input() IsNewEntityCall: boolean;
  ObjectTable: ObjectTablePM;
  private ComponentRef: any = null;
  private ComponentInstance: any = null;

  @ViewChild(ChildDirective) Child: ChildDirective;

  ngAfterViewInit() {
    var table = window.ObjectTables.filter(d => d.Name === this.ObjectTableName)[0];
    if (table) {
      //this.ObjectField = window.ObjectFields.filter(d => d.ObjectTableId === table.Id && d.FieldName === this.ObjectFieldName)[0];
      if (this.ObjectField) {
        SessionLocator.DynamicLoader.Load(this.ObjectField.GeneratedComponentPath, this.Child.Location)
          .then(cmpRef => {
            this.ComponentRef = cmpRef;
            cmpRef.instance.ComponentRef = cmpRef;
            //cmpRef.instance.IsInsideWindow = true;
            //cmpRef.instance.ComponentBackground = "transparent";
            cmpRef.instance.Run({ ObjectField: this.ObjectField, ObjectTableName: this.ObjectTableName, DataContext: this.DataContext, IsNewEntityCall: this.IsNewEntityCall });
          });
      }
    }
  }
}
