import {Component, OnInit, Type, Output, EventEmitter, ComponentRef, ViewChild,Input,AfterViewInit,ChangeDetectorRef} from '@angular/core';
//import {TextCodeTranslationPipe} from '../../../../Controls/Pipes/TextCodeTranslationPipe';


@Component({
    moduleId: module.id,

    templateUrl: './LogitudeListBoxComponent.html',
    inputs: ['DataSource', 'Height', 'SelectedItem', 'Binding','event','DataSourceChanged'],
    selector:'LogListBox',
    //directives: [CORE_DIRECTIVES, IconButton, LogGridComponent, NgFormControl, AdvanceSearchComponent],
    //pipes: [TextCodeTranslationPipe],
    //providers: [HTTP_PROVIDERS, EntityListService, ServiceArgs, EntityResourceService, PubSubService, PubSubService1],
})

export class LogitudeListBoxComponent implements OnInit{

    @Output() SelectedItemChanged = new EventEmitter();
    public event: EventEmitter<any>; 
    public DataSourceChanged: EventEmitter<any>; 
    Height: string;
    Style: any;// = "width:100%;border:none;margin-top:-8px;height:100%; overflow:auto;background-color: transparent;";
    public Binding: string = null;
    public SelectedItem: any = null;
    ClassName: string = "ListBoxItem";
    DataSource: any[];
    SourceItems: any[];
    constructor(private CD: ChangeDetectorRef) {
    }
    ngOnInit() {
        if (this.Height != null) {
            this.Style = { 'width': '100%', 'margin-top': '5px', 'height': this.Height + 'px', 'overflow': 'auto', 'background-color': 'transparent', 'max-height:': this.Height + 'px' };
        }
        else {
            this.Style = { 'width': '100%', 'margin-top': '5px', 'height': '100%', 'overflow': 'auto', 'background-color': 'transparent', 'max-height:': '494px' };
        }
        this.SourceItems = []; 
        this.event.subscribe((res) => {
            if (this.DataSource) { 
                if (this.DataSource.length > 0) {
                    this.SourceItems = []; 
                    //this.SelectedItem = res;
                    this.DataSource.forEach((value, key) => {
                        if (value == this.SelectedItem && res != null) {
                            this.SourceItems.push(new ListBoxItem(value, this,true));
                        }
                        else {
                            this.SourceItems.push(new ListBoxItem(value, this));
                        }
                        this.CD.detectChanges();
                    });
                }
                else {
                    this.SourceItems = [];
                }
            }
        });

        if (this.DataSourceChanged){
            this.DataSourceChanged.subscribe((res) => {
                if (res) {
                    if (res.length > 0) {
                        this.DataSource = res;
                        this.SourceItems = [];
                        this.DataSource.forEach((value, key) => {
                            if (value == this.SelectedItem && res != null) {
                                this.SourceItems.push(new ListBoxItem(value, this, true));
                            }
                        else {
                            this.SourceItems.push(new ListBoxItem(value, this));
                        }
                        this.CD.detectChanges();
                    });
                }
                else {
                    this.SourceItems = [];
                }
            }
        });
        }
       
        
    }
 

    setSelectedItem(item:any) {
        this.SelectedItem = item;
        this.SourceItems.forEach((value, key) => {
            value.ClassName = "ListBoxItem";
        });
        this.SelectedItemChanged.emit(this.SelectedItem);
        
    }
}

export class ListBoxItem {

    private item: ListBoxItem;
    public get Item() { return this.item; }
    public set Item(newValue: ListBoxItem) { this.item = newValue; }

    private text: any;
    public get Text() { return this.text; }
    public set Text(newValue: any) { this.text = newValue; }
    ClassName: string = "ListBoxItem";
    constructor(SourceItem: any, private parentComponent: LogitudeListBoxComponent, isselected = false) { 
        this.Item = SourceItem;
        if (isselected == true) {
            this.ClassName = "SelectedListBoxItem";
        }
       //this.Text = SourceItem.FieldName;
        //if (this.parentComponent.SelectedItem != null) {
            if (this.parentComponent.Binding == null) {
                this.Text = SourceItem;
            }

            else {
                this.Text = SourceItem[this.parentComponent.Binding];//this.parentComponent.SelectedItem[this.parentComponent.Binding];
            }
       // } 
    }

    setSelected() {
        this.parentComponent.setSelectedItem(this.Item);
        this.ClassName = "SelectedListBoxItem"; 
    }
}