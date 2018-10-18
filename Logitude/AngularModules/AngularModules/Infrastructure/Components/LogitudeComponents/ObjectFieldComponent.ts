declare var window: any;
 
import {Directive, ElementRef, Renderer, Input, Output, Component, OnInit, OnChanges, EventEmitter, AfterViewInit, OnDestroy, ViewChild, ViewContainerRef} from '@angular/core';
import {CustomFieldClass} from '../../DataContracts/CustomFieldClass';
import {BaseComponent} from './BaseComponent';
import {UIProperty, UIProperties, UIPropertyArgs} from './UIProperties';
import {SessionLocator} from '../../Utilities/SessionLocator';
import {AppTool} from '../../Tools';
import {TextCodeTranslator} from '../../Utilities/TextCodeTranslator';
import {ObjectFieldPM} from '../../EntityPMs/ObjectFieldPM';
import {ObjectTablePM} from '../../EntityPMs/ObjectTablePM';
import {ApiQueryFilters, FilterItem} from '../../DataContracts/ApiQueryFilters';
import {EntityResourceService} from '../../Services/EntityResourceService';
import {EntityListService} from '../../Services/EntityListService';
import {ServiceResponse} from '../../DataContracts/ServiceResponse';
import {Observable} from 'rxjs/Observable';

@Component({
    moduleId: module.id,

    selector: 'ObjectFieldComponent',
    template: `<div #ComponentContent></div>`,
    
})

export class ObjectFieldComponent implements OnInit, AfterViewInit, OnDestroy {

    @Input() ObjectField: ObjectFieldPM;
    @Input() ObjectTableName: string;
    @Input() DataContext: any;
    @Input() IsNewEntityCall: boolean;

    ObjectTable: ObjectTablePM;
    //ObjectField: ObjectFieldPM;
    
    @ViewChild("ComponentContent", { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;

    private ComponentRef: any = null;
    private ComponentInstance: any = null;
    ngOnInit() {

        var table = window.ObjectTables.filter(d => d.Name === this.ObjectTableName)[0];
        if (table) {
            //this.ObjectField = window.ObjectFields.filter(d => d.ObjectTableId === table.Id && d.FieldName === this.ObjectFieldName)[0];
            if (this.ObjectField) {
                SessionLocator.DynamicLoader.Load(this.ObjectField.GeneratedComponentPath, this.viewContainerRef)
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

    ngAfterViewInit() {
    }

    ngOnDestroy() {
    }
}