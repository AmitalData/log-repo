
import { Component, OnInit, Input, forwardRef,ChangeDetectorRef } from '@angular/core';
import { AppTool } from '../../Infrastructure/Tools';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { ObjectsLocator } from '../../Infrastructure/Locators/ObjectsLocator';

@Component({
    selector: "LogChips",
    templateUrl: './LogChips.html',
    styleUrls: ['./LogChips.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => LogChipsComponent),
            multi: true
        }
    ]
})

export class LogChipsComponent implements ControlValueAccessor
{

    isRTL = ObjectsLocator.GlobalSetting ? (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl") : false;

    constructor(private changeDetector: ChangeDetectorRef){

    }

    @Input() Options: any[] = [];
    @Input() DisplayFieldName: string;;
    SelectedItems: any[] = [];


    ItemSelected(item){

        this.SelectedItems.push(item);

        this.RemoveOption(item);

        this.onChange(this.SelectedItems);
        this.onTouch(this.SelectedItems);
    }

    DeselectItem(item){
        var itemIndex = this.SelectedItems.indexOf(item);;
        if(itemIndex >= 0) {
            this.SelectedItems.splice(itemIndex,1);
            this.AddOption(item);
        }
    }
    RemoveOption(option){
        this.Options = this.Options.filter(el=> el[this.DisplayFieldName] != option[this.DisplayFieldName]);

        // var itemIndex = this.Options.indexOf(option);
        // if(itemIndex >= 0) {
        //     this.Options.splice(itemIndex,1);
        // }
    }

    AddOption(option){
        var itemIndex = this.Options.indexOf(option);;
        if(itemIndex < 0){
        this.Options.push(option);
        this.Options.sort((a,b) => (a[this.DisplayFieldName] > b[this.DisplayFieldName]) ? 1 : ((b[this.DisplayFieldName] > a[this.DisplayFieldName]) ? -1 : 0));
        }
    }

    onChange: any = () => { }
    onTouch: any = () => { }
    registerOnChange(fn: any): void
    {
        this.onChange = fn;
    }
    registerOnTouched(fn: any): void
    {
        this.onTouch = fn;
    }
    writeValue(value: any)
    {
        if(value && value.length > 0){
            this.SelectedItems = value;
            this.SelectedItems.forEach(item => {
                this.RemoveOption(item);
            });
        }
    }

}
