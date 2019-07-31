import { Component, Input, OnInit, QueryList, Directive, ViewChildren, ContentChildren, ViewChild, TemplateRef, ContentChild, forwardRef, ElementRef, AfterViewInit, ViewContainerRef } from '@angular/core'; 
import {LogCellTemplateComponent} from './LogCellTemplateComponent';
import {LogFooterComponent} from './LogFooterComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {EditableLogGridComponent} from './EditableLogGridComponent';


@Component({
    selector: 'log-column',
    template: '<ng-content></ng-content>', 
    inputs: [
        'header: header',
        'binding: binding',
        'HeaderStyle: HeaderStyle',
        'width: width',
        'visibility: visibility',
        'Editable: Editable',
        'Style: Style',
        'Alignment: Alignment',
        'required: required',
        'AllowDisableCells:AllowDisableCells',
        'Disabled: Disabled',
        'format: format',
        'SortFieldName:SortFieldName',
        'IgnoreColumn:IgnoreColumn'
    ]
})
//@Directive({ selector: 'log-cell-template' })
export class LogColumnComponent implements AfterViewInit {
    public customcolumns: any[];
    ColId: string;
    header: string;
    footer: string;
    binding: string;
    HeaderStyle: {};
    width: string;
    //visibility: string;
    Editable: boolean;
    Style: {};
    Alignment: string;
    required: string;
    hastemplate: boolean;
    hasHeadertemplate: boolean;
    hasFootertemplate: boolean;
    index: number;
    //Editindex: number;
    AllowDisableCells: boolean;
    Disabled: boolean;
    format: string = null;
    LogGridId: string = "";
    IsReadOnlyGrid: boolean = false;
    SortFieldName: string;
    IgnoreColumn: boolean = false;
    EditableLogGridComponent: EditableLogGridComponent;
    FooterContentTemplate: any;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        //this.EditableLogGridComponent = ELG;
    }
    @ContentChildren(TemplateRef) innerContentTpl: any;
    @ContentChild(LogFooterComponent) childChildren: LogFooterComponent;
    //@ContentChildren(LogCellTemplateComponent) panes: QueryList<LogCellTemplateComponent>;
    //@ContentChild(forwardRef(() => LogCellTemplateComponent))
    //private navComponent: LogCellTemplateComponent;
    //@ViewChildren(LogCellTemplateComponent) cellChildren: QueryList<LogCellTemplateComponent>;
    ngAfterContentInit() {
        this.ColId = this.CurrentSession.LogitudeGridHelper.GetColumnId();
        var temp = this.childChildren;
        if (temp) {
            this.hasFootertemplate = true;
            this.FooterContentTemplate = temp.myChild;
            this.innerContentTpl = this.innerContentTpl.filter(a => a != temp.myChild);
        }
        else {
            this.hasFootertemplate = false;
            this.innerContentTpl = this.innerContentTpl.toArray();
        }
        // get all active tabs
        //if (this.childChildren.length > 0) {
        //    //alert("Yea");
        //}
        this.hastemplate = this.innerContentTpl.length == 0 ? false : true;  
        this.hasHeadertemplate = this.innerContentTpl.length < 2 ? false : true;
        // var temp = this.navComponent;
        this.CurrentSession.SubscriptionAdd(
            this.CurrentSession.SessionEvent.subscribe((res) => {
                if (res.IsCell) {
                    if (res.IsEnterCLicked == true) {
                        var element = document.getElementById(res.Id);
                        element.blur();
                        //alert(res.Id);
                    }
                }
            })
        ); 
        //if (this.hastemplate) {
        //    this.Editindex = this.CurrentSession.GetEditCellIndex();
        //}
    }
    //@ContentChildren(TemplateRef) contentTpl: any;
    ngAfterViewInit() {
        //var temp = this.navComponent;
        //var ss = this.cellChildren;
        //alert(this.LogGridId);

    }
    ngOnInit() {
        //this.CurrentSession.LogitudeGridHelper.SetColumnsCount(false, this.LogGridId);
        //this.headerStyle = {
        //    'width': (this.ViewWidth) + 'px',
        //    'min-width': (this.ViewWidth) + 'px'
        //};
        this.Style = {
            width: + this.width + 'px',
            'background-color': this.Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
            'text-align': this.Alignment ? this.Alignment : 'right',
            'display': this.visibility == 'hidden' ? 'none' : 'inline'
        };
        this.HeaderStyle = {
            width: + this.width + 'px',
            'min-width': this.width + 'px'
            //'display': this.visibility == 'hidden' ? 'none' : 'inline-block'
        };
    }

    private visi: string;
    public get visibility() { return this.visi; }
    public set visibility(newValue: string) {
        if (this.visi != newValue) { 
            this.visi = newValue;
        }
    }
}
