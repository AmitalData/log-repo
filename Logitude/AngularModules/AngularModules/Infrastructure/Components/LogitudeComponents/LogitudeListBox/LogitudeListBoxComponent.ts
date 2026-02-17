import {Component, OnInit, Type, Output, EventEmitter,ChangeDetectorRef} from '@angular/core';
import { TextCodeTranslator } from '../../../Utilities/TextCodeTranslator';
import { AppTool } from '../../../Tools';

@Component({
    moduleId: module.id,

    templateUrl: './LogitudeListBoxComponent.html',
    inputs: ['DataSource', 'Height', 'SelectedItem', 'Binding', 'event', 'DataSourceChanged', 'Sort'],
    selector: 'LogListBox',
})

export class LogitudeListBoxComponent implements OnInit {

    @Output() SelectedItemChanged = new EventEmitter();
    public event: EventEmitter<any>;
    public DataSourceChanged: EventEmitter<any>;
    Height: string;
    Style: any;
    public Binding: string = null;
    public Sort: boolean = false;
    public SelectedItem: any = null;
    ClassName: string = "ListBoxItem";
    DataSource: any[];
    SourceItems: ListBoxItem[];
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
                    var iSourceItems: ListBoxItem[] = [];

                    this.DataSource.forEach((value, key) => {
                        if (value == this.SelectedItem && res != null) {
                            iSourceItems.push(new ListBoxItem(value, this, true));
                        }

                        else {
                            iSourceItems.push(new ListBoxItem(value, this));
                        }

                        if (this.Sort) {
                            this.SourceItems = iSourceItems.sort((a, b) => a.TranslatedText.toLowerCase() !== b.TranslatedText.toLowerCase() ? a.TranslatedText.toLowerCase() < b.TranslatedText.toLowerCase() ? -1 : 1 : 0);
                        }
                        else {
                            this.SourceItems = iSourceItems;
                        }

                        this.CD.detectChanges();
                    });
                }

                else {
                    this.SourceItems = [];
                }
            }
        });

        if (this.DataSourceChanged) {
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

    setSelectedItem(item: any) {
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
    public TranslatedText: string = "";    
    constructor(SourceItem: any, private parentComponent: LogitudeListBoxComponent, isselected = false) {
        this.Item = SourceItem;

        if (isselected == true) {
            this.ClassName = "SelectedListBoxItem";
        }

        if (this.parentComponent.Binding == null) {
            this.Text = SourceItem;
        }

        else {
            this.Text = SourceItem[this.parentComponent.Binding];
        }

        this.TranslatedText = TextCodeTranslator.Translate(this.Text);
        if (AppTool.IsNullOrEmpty(this.TranslatedText)) {
            this.TranslatedText = "";
        }
    }

    setSelected() {
        this.parentComponent.setSelectedItem(this.Item);
        this.ClassName = "SelectedListBoxItem"; 
    }
}
