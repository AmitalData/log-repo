
import { Component, Input ,OnInit,ChangeDetectorRef} from '@angular/core';
import { AppTool, DateTool } from '../../Tools';
@Component({
    selector: 'LogWaterMark', 
    template: 
    //width:166px;height: 22px;
`<div 
    style="position: relative;" 
    (mouseleave)="SetFucos(false)"  
    >
    <ng-content></ng-content>
    <input 
        style="position: absolute;top: 0px;left: 0px;opacity:0.7;"
        (click)="SetFucos(true)"  
        *ngIf="ShowWaterMark"
        placeholder="{{Watermark}}" />
</div>`,
})
export class LogWaterMarkComponent  {
    _SetFucos: boolean = false;

    @Input()
    public LogWaterMarkValue: string = "";
    @Input()
    public Watermark: string = "";
    
    constructor() {
      
    }
    SetFucos(val: boolean) {
        this._SetFucos = val;
    }
    public get ShowWaterMark(): boolean {
        if (!AppTool.IsNullOrEmpty(this.LogWaterMarkValue)) {
            return false;
        }
        if (this._SetFucos) {
            return false;
        }
        return true;
    }
}
