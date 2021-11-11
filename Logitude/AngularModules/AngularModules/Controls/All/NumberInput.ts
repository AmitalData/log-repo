
import { Component, OnInit, Input, forwardRef,ChangeDetectorRef } from '@angular/core';
import { AppTool } from '../../Infrastructure/Tools';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { ObjectsLocator } from '../../Infrastructure/Locators/ObjectsLocator';

@Component({
    selector: "NumberInput",
    templateUrl: './NumberInput.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => NumberInputComponent),
            multi: true
        }
    ]
})

export class NumberInputComponent implements ControlValueAccessor
{
    isRTL = ObjectsLocator.GlobalSetting ? (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl") : false;

    constructor(private changeDetector: ChangeDetectorRef){

    }

    @Input() min: number;
    @Input() max: number;
    @Input() allowNull: boolean = false;
    private _value: number;
    public get value(): number
    {
        return this._value;
    }
    public set value(newValue: number)
    {
        if ((newValue !== undefined && newValue !== this._value) || !newValue) {
            newValue = this.limitValueBoundaries(newValue);
            this.replaceOldValueWithNewValue(newValue);

            this.onChange(newValue);
            this.onTouch(newValue);
        }
    }

    onChange: any = () => { }
    onTouch: any = () => { }

    private 

    private replaceOldValueWithNewValue(newValue: number)
    {
        this._value = null;
        this.changeDetector.detectChanges();

        this._value = newValue;
        this.changeDetector.detectChanges();
    }

    private limitValueBoundaries(v: number)
    {
        if(this.allowNull && v == null) {
           return v; 
        }
        if (this.max && v > this.max)
            v = this.max;

        if (this.min && v < this.min)
            v = this.min;
        return v;
    }

    registerOnChange(fn: any): void
    {
        this.onChange = fn;
    }
    registerOnTouched(fn: any): void
    {
        this.onTouch = fn;
    }
    writeValue(input: number)
    {
        this.value = input;
    }

    IncrementValue()
    {
        this.value = (this.value + 1 || 0);
    }
    DecrementValue()
    {
        this.value = (this.value - 1 || 0);
    }

}
