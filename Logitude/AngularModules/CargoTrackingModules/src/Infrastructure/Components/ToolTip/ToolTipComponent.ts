import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';

@Component({
    selector: 'app-tooltip',
    templateUrl: './ToolTipComponent.html',
    styleUrls: ['./ToolTipComponent.css']
})
export class ToolTipComponent implements OnInit
{

    startDelayTime: number = 500;
    hideDelayTime: number = 500;
    showPopup = false;
    @Input() ShowHeader: boolean = false;
    @Input() Title;

    constructor(private changeDetector: ChangeDetectorRef) { }

    ngOnInit(): void
    {
    }

    ShowPopup(){
        setTimeout(() => {
            this.showPopup = true;
            this.DetectChanges();
        }, this.startDelayTime);
    }

    HidePopup(){
        setTimeout(() => {
            this.showPopup = false;
            this.DetectChanges();
        }, this.hideDelayTime);
    }

    DetectChanges(){
        this.changeDetector.detectChanges();
    }


}
